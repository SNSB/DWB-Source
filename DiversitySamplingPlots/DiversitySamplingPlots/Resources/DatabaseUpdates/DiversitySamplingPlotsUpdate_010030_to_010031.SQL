
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.30'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Alter function procInsertSamplingPlotCopy to copy Localisations, Properties and Resources, too  ############
--#####################################################################################################################

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER  PROCEDURE [dbo].[procInsertSamplingPlotCopy]
    (@PlotID int output ,
    @OriginalPlotID int ,
    @PlotIdentifier  nvarchar(50))
AS
declare @count int

-- Sampling Plot
INSERT INTO SamplingPlot
                      (PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes)
SELECT     PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes
FROM         SamplingPlot
WHERE     (PlotID = @OriginalPlotID)

SELECT @PlotID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

-- SamplingProject
INSERT INTO SamplingProject
                      (PlotID, ProjectID)
SELECT     @PlotID, ProjectID
FROM         SamplingProject
WHERE     (PlotID = @OriginalPlotID)

-- SamplingPlotLocalisation
INSERT INTO SamplingPlotLocalisation
	(PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, 
	ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache)
SELECT  @PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, 
		ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache
FROM         SamplingPlotLocalisation
WHERE     (PlotID = @OriginalPlotID)

-- SamplingPlotProperty
INSERT INTO SamplingPlotProperty
	(PlotID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache)
SELECT  @PlotID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache
FROM         SamplingPlotProperty
WHERE     (PlotID = @OriginalPlotID)

-- SamplingPlotResource
INSERT INTO SamplingPlotResource
	(PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes)
SELECT  @PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes
FROM         SamplingPlotResource
WHERE     (PlotID = @OriginalPlotID)

SELECT @PlotID

GO

--#####################################################################################################################
--######   [trgInsSamplingPlotLocalisation] ################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgInsSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR INSERT AS
/*
Setting the average coordinates if the entered values are numeric
*/
Update S
set S.AverageLongitudeCache = cast(replace(I.Location1, ',', '.') as varchar)
, S.AverageLatitudeCache = cast(replace(I.Location2, ',', '.') as varchar)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND isnumeric(I.Location1 + 'e0') = 1
AND isnumeric(I.Location2 + 'e0') = 1
AND I.LocalisationSystemID in (3,7,8)

/*
Setting the average coordinates if the entered values are numeric - second trial
*/
Update S
set S.AverageLongitudeCache = cast(replace(I.Location1, ',', '.') as float)
, S.AverageLatitudeCache = cast(replace(I.Location2, ',', '.') as float)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND isnumeric(replace(I.Location1, ',', '.') + 'e0') = 1
AND isnumeric(replace(I.Location2, ',', '.') + 'e0') = 1
AND I.LocalisationSystemID in (3,7,8)

/*
Setting the average altitude if the entered values are numeric
*/
Update S
set S.AverageAltitudeCache = 
CASE 
	WHEN I.Location2 IS NULL OR RTRIM(i.Location2) = ''
	THEN CAST(REPLACE(I.Location1, ',', '.') AS FLOAT)
	ELSE
	(CAST(REPLACE(I.Location1, ',', '.') AS FLOAT) + CAST(REPLACE(I.Location2, ',', '.') AS FLOAT))/2
END
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.LocalisationSystemID in (4)
AND ISNUMERIC(I.Location1) = 1
AND NOT I.Location1 IS NULL

/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set Geography =
case when I.Geography IS null OR I.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+' ' +replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')', 4326)
else I.Geography end
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 180
AND (S.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
OR S.Geography IS NULL)

/*
Setting the cached geography of the plot if it is missing or for WGS84
*/
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P, inserted I
where 1 = 1
AND L.PlotID = I.PlotID
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL)

GO

--#####################################################################################################################
--######   [trgUpdSamplingPlotLocalisation] ################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR UPDATE AS
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState)
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED
Update SamplingPlotLocalisation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotLocalisation, deleted
where 1 = 1
AND SamplingPlotLocalisation.LocalisationSystemID = deleted.LocalisationSystemID
AND SamplingPlotLocalisation.PlotID = deleted.PlotID

/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set S.AverageAltitudeCache = 
CASE 
	WHEN I.Location2 IS NULL OR RTRIM(i.Location2) = ''
	THEN CAST(REPLACE(I.Location1, ',', '.') AS FLOAT)
	ELSE
	(CAST(REPLACE(I.Location1, ',', '.') AS FLOAT) + CAST(REPLACE(I.Location2, ',', '.') AS FLOAT))/2
END
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.LocalisationSystemID in (4)
AND ISNUMERIC(I.Location1) = 1
AND NOT I.Location1 IS NULL

/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set Geography =
case when I.Geography IS null OR I.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+' ' +replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')', 4326)
else I.Geography end
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 180
AND (S.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
OR S.Geography IS NULL)

/*
Setting the cached geography of the plot if it is missing or for WGS84
*/
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P, inserted I
where 1 = 1
AND L.PlotID = I.PlotID
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL)

GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.11.00' 
END

GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.31'
END

GO

SELECT [dbo].[Version] ()  

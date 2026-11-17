
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.05'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   [SamplingPlotListWithProject]   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[SamplingPlotListForCurrentProject] ()  
RETURNS @SamplingPlotList TABLE ([PlotID] [int] Primary key ,
	[PartOfPlotID] [int] NULL,
	[PlotIdentifier] [nvarchar](500) NOT NULL,
	[PlotGeography_Cache] [geography] NULL,
	[PlotGeometry_Cache] [geometry] NULL,
	[PlotDescription] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[DisplayText] [nvarchar](max) NOT NULL)
/*
Returns a table that lists all the SamplingPlot items related to the given project.
MW 26.07.2013
TEST:
SELECT * FROM DBO.SamplingPlotListForCurrentProject()
*/
AS
BEGIN
declare @ProjectID int
set @ProjectID = (SELECT [dbo].[DefaultProjectID] ())

-- getting the TopIDs
INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText)
SELECT     S.PlotID, S.PartOfPlotID, S.PlotIdentifier, S.PlotGeography_Cache, S.PlotGeometry_Cache, S.PlotDescription, S.InternalNotes, S.PlotIdentifier AS DisplayText
FROM         SamplingPlot AS S INNER JOIN
                      SamplingProject AS P ON S.PlotID = P.PlotID
WHERE     (S.PartOfPlotID IS NULL) AND (P.ProjectID = @ProjectID)

declare @i int
set @i = (select count(*) from SamplingPlot where PlotID not IN (select PlotID from  @SamplingPlotList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText)
	SELECT C.PlotID, C.PartOfPlotID, C.PlotIdentifier, C.PlotGeography_Cache, C.PlotGeometry_Cache, C.PlotDescription, C.InternalNotes, L.DisplayText + ' | ' + C.PlotIdentifier AS DisplayText
	FROM SamplingPlot C, @SamplingPlotList L, SamplingProject P
	WHERE C.PartOfPlotID = L.PlotID
	AND C.PlotID NOT IN (select PlotID from @SamplingPlotList)
	AND (P.ProjectID = @ProjectID)
	AND C.PlotID = P.PlotID

	set @i = (SELECT     count(*)FROM SamplingPlot C, @SamplingPlotList L, SamplingProject P
	WHERE C.PartOfPlotID = L.PlotID
	AND C.PlotID NOT IN (select PlotID from @SamplingPlotList)
	AND (P.ProjectID = @ProjectID)
	AND C.PlotID = P.PlotID)

end


   RETURN
END




GO

--#####################################################################################################################
--######   [trgSamplingPlotLocalisation]   ######################################################################################
--#####################################################################################################################


IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgSamplingPlotLocalisation]'))
DROP TRIGGER [dbo].[trgSamplingPlotLocalisation]
GO


--#####################################################################################################################
--######   [trgUpdSamplingPlotLocalisation]   ######################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
FOR UPDATE AS
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED

/*
Setting the Log info values
*/
Update S
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotLocalisation S, deleted D
where 1 = 1 
AND S.PlotID = D.PlotID
AND S.LocalisationSystemID = D.LocalisationSystemID

/*
Setting the average altitude if the entered values are numeric
*/
Update S
set S.[AverageAltitudeCache] = 
(case when isnumeric(I.Location1 + 'e0') = 1 then cast(replace(I.Location1, ',', '.') as real) else 0 end +
 case when isnumeric(I.Location2 + 'e0') = 1 then cast(replace(I.Location2, ',', '.') as real) else 0 end)
 / (case when isnumeric(I.Location1 + 'e0') = 1 then 1 else 0 end +
 case when isnumeric(I.Location2 + 'e0') = 1 then 1 else 0 end)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1 
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND (isnumeric(I.Location1 + 'e0') = 1 OR I.Location1 is null)
AND (isnumeric(I.Location2 + 'e0') = 1 OR I.Location2 is null)
AND S.LocalisationSystemID in (4)  
--AND S.AverageAltitudeCache is null


/*
Setting the average coordinates if the entered values are numeric
*/
Update S
set [AverageLongitudeCache] = cast(replace(I.Location1, ',', '.') as varchar)
, AverageLatitudeCache = cast(replace(I.Location2, ',', '.') as varchar)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1 
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND isnumeric(I.Location1 + 'e0') = 1
AND isnumeric(I.Location2 + 'e0') = 1
AND I.LocalisationSystemID in (3,7,8)   
--AND S.AverageLatitudeCache is null
--AND S.AverageLongitudeCache is null

Update S
set S.[AverageLongitudeCache] = cast(replace(I.Location1, ',', '.') as float)
, S.AverageLatitudeCache = cast(replace(I.Location2, ',', '.') as float)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1 
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND isnumeric(replace(I.Location1, ',', '.') + 'e0') = 1
AND isnumeric(replace(I.Location1, ',', '.') + 'e0') = 1
AND I.LocalisationSystemID in (3,7,8)   


/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set Geography = 
case when I.Geography IS null OR I.Geography.ToString() <> 'POINT(' + replace(cast(I.[AverageLongitudeCache] as varchar), ',', '.')+ ' ' + replace(cast(I.[AverageLatitudeCache] as varchar), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(I.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(I.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
else I.Geography end
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1 
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 180
AND (S.Geography.ToString() <> 'POINT(' + replace(cast(I.[AverageLongitudeCache] as varchar), ',', '.')+ ' ' + replace(cast(I.[AverageLatitudeCache] as varchar), ',', '.')+')'
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
AND (P.PlotGeography_Cache IS NULL
OR L.LocalisationSystemID = 8)
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.06'
END

GO


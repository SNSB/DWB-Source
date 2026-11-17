
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.18'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   setting the geography if missing   #########################################################################
--#####################################################################################################################

disable trigger trgUpdSamplingPlotLocalisation ON SamplingPlotLocalisation;
GO

disable trigger trgUpdSamplingPlot ON SamplingPlot;
GO


-- set Geography for Localisation from WGS84 missing geography
Update S
set Geography =
case when S.Geography IS null OR S.Geography.ToString() <> 'POINT(' + replace(cast(S.[AverageLongitudeCache] as varchar), ',', '.')+ ' ' + replace(cast(S.[AverageLatitudeCache] as varchar), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(S.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(S.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
else S.Geography end
FROM SamplingPlotLocalisation S
where 1 = 1
AND S.AverageLatitudeCache between -90 and 90
AND S.AverageLongitudeCache between -180 and 180
AND (S.Geography IS NULL)
and S.LocalisationSystemID = 8

-- set Geography for Localisation from Gazetteer missing geography
Update S
set Geography =
case when S.Geography IS null OR S.Geography.ToString() <> 'POINT(' + replace(cast(S.[AverageLongitudeCache] as varchar), ',', '.')+ ' ' + replace(cast(S.[AverageLatitudeCache] as varchar), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(S.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(S.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
else S.Geography end
FROM SamplingPlotLocalisation S
where 1 = 1
AND S.AverageLatitudeCache between -90 and 90
AND S.AverageLongitudeCache between -180 and 180
AND (S.Geography IS NULL)
and S.LocalisationSystemID = 7

-- set Geography for Localisation from MTB missing geography
Update S
set Geography =
case when S.Geography IS null OR S.Geography.ToString() <> 'POINT(' + replace(cast(S.[AverageLongitudeCache] as varchar), ',', '.')+ ' ' + replace(cast(S.[AverageLatitudeCache] as varchar), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(S.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(S.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
else S.Geography end
FROM SamplingPlotLocalisation S
where 1 = 1
AND S.AverageLatitudeCache between -90 and 90
AND S.AverageLongitudeCache between -180 and 180
AND (S.Geography IS NULL)
and S.LocalisationSystemID = 3

GO

-- set Geography for Localisation from WGS84 missing geography
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P
where 1 = 1
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = 8
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL) 


-- set Geography for Localisation from Gazetteer missing geography
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P
where 1 = 1
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = 7
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL) 


-- set Geography for Localisation from MTB missing geography
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P
where 1 = 1
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = 3
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL) 

GO

enable trigger trgUpdSamplingPlotLocalisation ON SamplingPlotLocalisation;
GO

enable trigger trgUpdSamplingPlot ON SamplingPlot;
GO


--#####################################################################################################################
--######   [trgUpdSamplingPlotLocalisation]   ################################################
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
AND (P.PlotGeography_Cache IS NULL) 

GO

--#####################################################################################################################
--######   [trgInsSamplingPlotLocalisation]   ################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgInsSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR INSERT AS
/* 
Setting the average coordinates if the entered values are numeric 
*/
Update S
set S.[AverageLongitudeCache] = cast(replace(I.Location1, ',', '.') as varchar)
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
AND (P.PlotGeography_Cache IS NULL) 

GO

/*
Remove Returns from Plot Idetifiers
*/
UPDATE P set [PlotIdentifier] = replace([PlotIdentifier], CHAR(13)+CHAR(10), '')
  FROM [dbo].[SamplingPlot]
p where p.plotidentifier like '%' + CHAR(13)+CHAR(10) + '%' 

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.06.04' 
END

GO

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.19'
END

GO

select [dbo].[Version] ()  

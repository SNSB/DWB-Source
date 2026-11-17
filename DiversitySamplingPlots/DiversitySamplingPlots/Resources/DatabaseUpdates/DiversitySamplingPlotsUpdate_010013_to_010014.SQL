
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.13'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   SamplingPlotGeography   ######################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgInsSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR INSERT AS
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
RETURN '01.00.14'
END

GO


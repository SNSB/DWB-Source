
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.12'
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


CREATE TRIGGER [dbo].[trgInsSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR INSERT AS
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
RETURN '01.00.13'
END

GO


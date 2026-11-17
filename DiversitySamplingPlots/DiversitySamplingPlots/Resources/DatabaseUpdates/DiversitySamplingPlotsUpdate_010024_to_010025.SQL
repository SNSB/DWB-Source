
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.24'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   update for Database   ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotListForCurrentProject]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, ProjectID
FROM            dbo.SamplingPlotListForCurrentProject() AS ViewSamplingPlotListForCurrentProject')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON [ViewSamplingPlotListForCurrentProject] TO [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.08.01' 
END

GO

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.25'
END

GO

select [dbo].[Version] ()  

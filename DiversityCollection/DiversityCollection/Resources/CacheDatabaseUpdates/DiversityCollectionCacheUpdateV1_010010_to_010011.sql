
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.10'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO




--#####################################################################################################################
--######   procTransferAnalysis      #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferAnalysis] 
AS
/*
-- TEST
EXECUTE  [dbo].[procTransferAnalysis]
*/
truncate table AnalysisCache

INSERT INTO [dbo].[AnalysisCache]
           ([AnalysisID]
		   ,[AnalysisParentID]
           ,[DisplayText]
           ,[Description]
           ,[MeasurementUnit]
           ,[Notes]
           ,[AnalysisURI]
           ,[OnlyHierarchy])
SELECT DISTINCT A.AnalysisID, A.AnalysisParentID, A.DisplayText, A.Description, A.MeasurementUnit, A.Notes, A.AnalysisURI, A.OnlyHierarchy
FROM ' + dbo.SourceDatebase() + '.dbo.Analysis AS A INNER JOIN
' + dbo.SourceDatebase() + '.dbo.ProjectAnalysis AS PA ON A.AnalysisID = PA.AnalysisID INNER JOIN
ProjectPublished AS P ON PA.ProjectID = P.ProjectID

DECLARE @U int
SET @U = (SELECT COUNT(*) FROM AnalysisCache)
SELECT CAST(@U AS VARCHAR) + '' analysis imported''')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.11'
END

GO



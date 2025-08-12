
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
--######   ViewProcessing   ###########################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewProcessing]
AS
SELECT DISTINCT P.ProcessingID, P.ProcessingParentID, P.DisplayText, P.Description, P.Notes, P.ProcessingURI, P.OnlyHierarchy
FROM ' +  dbo.SourceDatabase() + '.dbo.Processing AS P INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.ProjectProcessing AS PP 
ON P.ProcessingID = PP.ProcessingID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewProcessing') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for processings' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewProcessing'
GO

GRANT SELECT ON ViewProcessing TO [CacheUser]
GO



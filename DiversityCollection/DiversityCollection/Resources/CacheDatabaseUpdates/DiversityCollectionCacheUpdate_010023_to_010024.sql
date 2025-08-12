
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.23'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   TaxonAnalysis: Adding Notes, AnalysisValue -> nvarchar(max)   ##############################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'TaxonAnalysis' and t.COLUMN_NAME = 'Notes') = 0
begin
	ALTER TABLE [dbo].TaxonAnalysis ADD Notes nvarchar(max) NULL;
end
GO

ALTER TABLE [dbo].TaxonAnalysis ALTER Column AnalysisValue nvarchar(max) NULL;
GO


declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.07'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   TaxonSynonymySource    #####################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TaxonSynonymySource' AND C.COLUMN_NAME = 'Subsets') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymySource] ADD [Subsets] [nvarchar](500) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'List of additional data transferred into the cache database separated by |' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'Subsets'
end
GO





--#####################################################################################################################
--######   ScientificTermSource   #####################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ScientificTermSource' AND C.COLUMN_NAME = 'Subsets') = 0
begin
	ALTER TABLE [dbo].[ScientificTermSource] ADD [Subsets] [nvarchar](500) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'List of additional data transferred into the cache database separated by |' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'Subsets'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ScientificTermSource' AND C.COLUMN_NAME = 'Terminology') = 1
begin
	exec sp_rename 'ScientificTermSource.Terminology', 'Source', 'COLUMN'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ScientificTermSource' AND C.COLUMN_NAME = 'TerminologyID') = 1
begin
	exec sp_rename 'ScientificTermSource.TerminologyID', 'SourceID', 'COLUMN'
end



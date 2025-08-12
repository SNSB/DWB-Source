
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.17'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   AgentSource  add IncludeInTransfer  ########################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[AgentSource] ADD [TransferProtocol] [nvarchar](max) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

ALTER TABLE [dbo].[AgentSource] ADD [IncludeInTransfer] [bit] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subsets of a source: The names of the tables included in the transfer separted by "|"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'Subsets'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source is located on a linked server, the name of the linked server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'LinkedServerName'
GO


--#####################################################################################################################
--######   GazetteerSource  add IncludeInTransfer  ####################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[GazetteerSource] ADD [Subsets] [nvarchar](500) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subsets of a source: The names of the tables included in the transfer separted by "|"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'Subsets'
GO

ALTER TABLE [dbo].[GazetteerSource] ADD [TransferProtocol] [nvarchar](max) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

ALTER TABLE [dbo].[GazetteerSource] ADD [IncludeInTransfer] [bit] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source is located on a linked server, the name of the linked server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'LinkedServerName'
GO


--#####################################################################################################################
--######   ScientificTermSource  add IncludeInTransfer  ###############################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ScientificTermSource] ADD [TransferProtocol] [nvarchar](max) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

ALTER TABLE [dbo].[ScientificTermSource] ADD [IncludeInTransfer] [bit] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'ScientificTermSource' and c.COLUMN_NAME = 'ServerName') = 1
begin
	EXEC sp_rename 'dbo.ScientificTermSource.ServerName', 'LinkedServerName', 'COLUMN';  
end
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source is located on a linked server, the name of the linked server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'LinkedServerName'
GO


--#####################################################################################################################
--######   TaxonSynonymySource   add IncludeInTransfer  ###############################################################
--#####################################################################################################################

ALTER TABLE [dbo].[TaxonSynonymySource] ADD [TransferProtocol] [nvarchar](max) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

ALTER TABLE [dbo].[TaxonSynonymySource] ADD [IncludeInTransfer] [bit] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source is located on a linked server, the name of the linked server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'LinkedServerName'
GO



--#####################################################################################################################
--######   Agent       ################################################################################################
--#####################################################################################################################



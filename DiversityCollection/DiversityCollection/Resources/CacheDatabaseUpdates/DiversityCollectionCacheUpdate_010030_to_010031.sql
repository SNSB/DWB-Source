
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.30'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ReferenceTitle Add AuthorsCache for authors from RLL #######################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ReferenceTitle] ADD [AuthorsCache] [nvarchar](1000) NULL;
GO

--#####################################################################################################################
--######   New table AgentIdentifier   ################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'AgentIdentifier') = 0
begin
	CREATE TABLE [dbo].[AgentIdentifier](
		[AgentID] [int] NOT NULL,
		[Identifier] [nvarchar](190) NOT NULL,
		[IdentifierURI] [varchar](500) NULL,
		[Type] [nvarchar](50) NULL,
		[Notes] [nvarchar](max) NULL,
		[SourceView] [varchar](128) NOT NULL,
		[BaseURL] [varchar](500) NOT NULL,
	 CONSTRAINT [PK_AgentIdentifier] PRIMARY KEY CLUSTERED 
	(
		[AgentID] ASC,
		[BaseURL] ASC,
		[Identifier] ASC
	) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end
GO

GRANT SELECT ON [AgentIdentifier] TO [CacheUser]
GO
GRANT DELETE ON [AgentIdentifier] TO [CacheAdmin] 
GO
GRANT UPDATE ON [AgentIdentifier] TO [CacheAdmin]
GO
GRANT INSERT ON [AgentIdentifier] TO [CacheAdmin]
GO



--#####################################################################################################################
--######   procTaxonNameHierarchy for setting of NameParentID   #######################################################
--#####################################################################################################################

ALTER PROCEDURE [dbo].[procTaxonNameHierarchy] 
@View nvarchar(50)
AS
BEGIN
/*
Sets the NameParentID as defined by the hierarchy
exec dbo.procTaxonNameHierarchy 'Names_Arthropoda_Test_SMNKspidernames_H'
*/
	SET NOCOUNT ON;

	declare @SQL nvarchar(max)
	set @SQL = (select 'update S set S.NameParentID = H.NameParentID
	from [dbo].[TaxonSynonymy] S inner join [dbo].[' + @View + '_H] H on H.BaseURL = S.BaseURL and H.NameID = S.NameID
	and S.SourceView = ''' + @View + '''' )

	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	--exec sp_executesql @SQL
	end catch

END
GO



--#####################################################################################################################
--######   New column MountPoint in table Target   ####################################################################
--#####################################################################################################################

if(select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'Target' and C.COLUMN_NAME = 'MountPoint') = 0
begin
	ALTER TABLE Target ADD [MountPoint] VARCHAR(50);
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mount point name of the transfer folder' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target', @level2type=N'COLUMN',@level2name=N'MountPoint'
end
GO

--#####################################################################################################################
--######   New column BashFile in table Target if missing   ###########################################################
--#####################################################################################################################

if(select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'Target' and C.COLUMN_NAME = 'BashFile') = 0
begin
	ALTER TABLE Target ADD [BashFile] VARCHAR(500);
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BashFile on the Postgres server used for conversion of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target', @level2type=N'COLUMN',@level2name=N'BashFile'
end
GO


--#####################################################################################################################
--######   New column UseBulkTransfer in table ProjectTarget if missing   #############################################
--#####################################################################################################################

if(select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectTarget' and C.COLUMN_NAME = 'UseBulkTransfer') = 0
begin
	ALTER TABLE ProjectTarget ADD [UseBulkTransfer] [bit] NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the bulk transfer should be used for the transfer of data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'UseBulkTransfer'
end
GO



--#####################################################################################################################
--######   Trigger trgUpdTarget for setting UseBulkTransfer   #########################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdTarget] ON [dbo].[Target]
FOR UPDATE AS

/* updating the [UseBulkTransfer] column in table [ProjectTarget] in case all columns for the bulk transfer are filled */
Update P
set [UseBulkTransfer] = 1
FROM [dbo].[Target] T, inserted i , [dbo].[ProjectTarget] P
where T.TargetID = i.TargetID 
and P.TargetID = T.TargetID
and T.TransferDirectory <> ''
and T.BashFile <> ''
and T.MountPoint <> ''

GO

ALTER TABLE [dbo].[Target] ENABLE TRIGGER [trgUpdTarget]
GO


--#####################################################################################################################
--######   New column Restriction in table ProjectPublished  ##########################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectPublished] ADD [Restriction] nvarchar(MAX) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An additional restriction of the content of the published data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'Restriction'
GO





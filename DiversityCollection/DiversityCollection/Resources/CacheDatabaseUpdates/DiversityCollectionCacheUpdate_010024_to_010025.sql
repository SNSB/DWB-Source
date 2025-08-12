
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
--######  rename ReferenceSource into ReferenceTitleSource  ###########################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ReferenceSource') > 0
begin
exec sp_rename 'dbo.ReferenceSource', 'ReferenceTitleSource'
end
GO

--#####################################################################################################################
--######  rename ReferenceSourceTarget into ReferenceTitleSourceTarget  ###############################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ReferenceSourceTarget') > 0
begin
exec sp_rename 'dbo.ReferenceSourceTarget', 'ReferenceTitleSourceTarget'
end
GO

--#####################################################################################################################
--######   TaxonAnalysisCategory: Adding AnalysisURI, ReferenceTitle, ReferenceURI   ##################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'TaxonAnalysisCategory' and t.COLUMN_NAME = 'AnalysisURI') = 0
begin
	ALTER TABLE [dbo].TaxonAnalysisCategory ADD AnalysisURI varchar(255) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'TaxonAnalysisCategory' and t.COLUMN_NAME = 'ReferenceTitle') = 0
begin
	ALTER TABLE [dbo].TaxonAnalysisCategory ADD ReferenceTitle nvarchar(600) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'TaxonAnalysisCategory' and t.COLUMN_NAME = 'ReferenceURI') = 0
begin
	ALTER TABLE [dbo].TaxonAnalysisCategory ADD ReferenceURI varchar(255) NULL;
end
GO


--#####################################################################################################################
--######   Target   ###################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'Target') = 0
begin
	CREATE TABLE [dbo].[Target](
		[TargetID] [int] IDENTITY(1,1) NOT NULL,
		[Server] [nvarchar](50) NOT NULL,
		[Port] [smallint] NOT NULL,
		[DatabaseName] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_Target] PRIMARY KEY CLUSTERED 
	(
		[TargetID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the target on a postgres server, PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target', @level2type=N'COLUMN',@level2name=N'TargetID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of IP of the Server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target', @level2type=N'COLUMN',@level2name=N'Server'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Port for accessing the server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target', @level2type=N'COLUMN',@level2name=N'Port'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target', @level2type=N'COLUMN',@level2name=N'DatabaseName'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The postgres databases as targets for the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target'
END


GRANT INSERT ON [Target] TO [CacheAdmin]
GO
GRANT UPDATE ON [Target] TO [CacheAdmin]
GO
GRANT SELECT ON [Target] TO [CacheUser]
GO
GRANT DELETE ON [Target] TO [CacheAdmin]
GO

--#####################################################################################################################
--######   Idx_Target   ###############################################################################################
--#####################################################################################################################

if (select count(*) from sys.indexes i where name = 'Idx_Target') = 0
begin
	CREATE UNIQUE NONCLUSTERED INDEX [Idx_Target] ON [dbo].[Target]
	(
		[Server] ASC,
		[Port] ASC,
		[DatabaseName] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
end
GO


--#####################################################################################################################
--######   ProjectTarget: Change PK   #################################################################################
--######   Clear table and add new column TargetID and PK   ###########################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'ProjectTarget' and t.COLUMN_NAME = 'TargetID') = 0
begin
	ALTER TABLE [dbo].[ProjectTarget] DROP CONSTRAINT [PK_ProjectTarget]

	DELETE FROM [ProjectTarget]

	ALTER TABLE [dbo].[ProjectTarget] DROP Column [Target]

	ALTER TABLE [dbo].ProjectTarget ADD TargetID int not null;

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the server, relates to table Target' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'TargetID'

	ALTER TABLE [dbo].[ProjectTarget] ADD  CONSTRAINT [PK_ProjectTarget] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[TargetID] ASC
	)

	ALTER TABLE [dbo].[ProjectTarget]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTarget_Target] FOREIGN KEY([TargetID])
	REFERENCES [dbo].[Target] ([TargetID])

	ALTER TABLE [dbo].[ProjectTarget] CHECK CONSTRAINT [FK_ProjectTarget_Target]
end
GO



--#####################################################################################################################
--######   ProjectTargetPackage   #####################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectTargetPackage') = 0
begin
	CREATE TABLE [dbo].[ProjectTargetPackage](
		[ProjectID] [int] NOT NULL,
		[TargetID] [int] NOT NULL,
		[Package] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_ProjectTargetPackage] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[TargetID] ASC,
		[Package] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[ProjectTargetPackage]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTargetPackage_ProjectTarget] FOREIGN KEY([ProjectID], [TargetID])
	REFERENCES [dbo].[ProjectTarget] ([ProjectID], [TargetID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ProjectTargetPackage] CHECK CONSTRAINT [FK_ProjectTargetPackage_ProjectTarget]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to ProjectID in table ProjectTarget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTargetPackage', @level2type=N'COLUMN',@level2name=N'ProjectID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Referes to TargetID in table ProjectTarget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTargetPackage', @level2type=N'COLUMN',@level2name=N'TargetID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Package installed for this project target' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTargetPackage', @level2type=N'COLUMN',@level2name=N'Package'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packages for projects as documented in the table Package in the Postgres database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTargetPackage'
END
GO

GRANT INSERT ON [ProjectTargetPackage] TO [CacheAdmin]
GO
GRANT UPDATE ON [ProjectTargetPackage] TO [CacheAdmin]
GO
GRANT SELECT ON [ProjectTargetPackage] TO [CacheUser]
GO
GRANT DELETE ON [ProjectTargetPackage] TO [CacheAdmin]
GO



--#####################################################################################################################
--######   Gazetteer: ADD ExternalNameID, ExternalDatabaseID    #######################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'Gazetteer' and t.COLUMN_NAME = 'ExternalNameID') = 0
begin
	ALTER TABLE [dbo].Gazetteer ADD ExternalNameID nvarchar(50) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'Gazetteer' and t.COLUMN_NAME = 'ExternalDatabaseID') = 0
begin
	ALTER TABLE [dbo].Gazetteer ADD ExternalDatabaseID int NULL;
end
GO

--#####################################################################################################################
--######   GazetteerExternalDatabase    ###############################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'GazetteerExternalDatabase') = 0
begin
	CREATE TABLE [dbo].[GazetteerExternalDatabase](
		[ExternalDatabaseID] [int] NOT NULL,
		[ExternalDatabaseName] [nvarchar](60) NOT NULL,
		[ExternalDatabaseVersion] [nvarchar](255) NOT NULL,
		[ExternalAttribute_NameID] [nvarchar](255) NULL,
		[ExternalAttribute_PlaceID] [nvarchar](255) NULL,
		[ExternalCoordinatePrecision] [nvarchar](255) NULL,
	 CONSTRAINT [PK_ExternalDatabase] PRIMARY KEY CLUSTERED 
	(
		[ExternalDatabaseID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

GRANT INSERT ON [GazetteerExternalDatabase] TO [CacheAdmin]
GO
GRANT UPDATE ON [GazetteerExternalDatabase] TO [CacheAdmin]
GO
GRANT SELECT ON [GazetteerExternalDatabase] TO [CacheUser]
GO
GRANT DELETE ON [GazetteerExternalDatabase] TO [CacheAdmin]
GO



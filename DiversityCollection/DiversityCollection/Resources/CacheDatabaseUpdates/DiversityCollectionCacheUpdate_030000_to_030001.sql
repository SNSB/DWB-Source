
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '03.00.00'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   ROLES   ######################################################################################
--#####################################################################################################################

CREATE ROLE [CacheAdministrator] AUTHORIZATION [dbo]
GO

CREATE ROLE [CacheUser] AUTHORIZATION [dbo]
GO

sp_addrolemember 'CacheUser', 'CacheAdministrator' 
GO


--#####################################################################################################################
--######   [DiversityWorkbenchModule]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) 
DROP FUNCTION [dbo].[DiversityWorkbenchModule]
GO
CREATE FUNCTION [dbo].[DiversityWorkbenchModule] () RETURNS nvarchar(50) AS BEGIN RETURN 'DiversityCollectionCache' END
GO
                
GRANT EXEC ON [DiversityWorkbenchModule] TO [CacheUser]
GO


--#####################################################################################################################
--######   [[ProjectDataType_Enum]]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectDataType_Enum]( 
[Code] [nvarchar](50) NOT NULL, 
[Description] [nvarchar](500) NULL, 
[DisplayText] [nvarchar](50) NULL, 
[DisplayOrder] [smallint] NULL, 
[DisplayEnable] [bit] NULL, 
[InternalNotes] [nvarchar](500) NULL, 
[ParentCode] [nvarchar](50) NULL, 
[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL, 
CONSTRAINT [PK_ProjectDataType_Enum] PRIMARY KEY CLUSTERED  
([Code] ASC))
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the type of transfer that should be used for the data of a project.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDataType_Enum' 
GO

ALTER TABLE [dbo].ProjectDataType_Enum ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

GRANT SELECT ON [ProjectDataType_Enum] TO [CacheUser]
GO


INSERT INTO ProjectDataType_Enum 
 (Code, Description, DisplayText, DisplayOrder, DisplayEnable) 
VALUES ('Specimen', 'Specimen with accession number, where the whole specimen is presented as main published item', 'Specimen', 1, 1)
GO

INSERT INTO ProjectDataType_Enum 
 (Code, Description, DisplayText, DisplayOrder, DisplayEnable) 
VALUES ('Parts', 'Parts with accession number, where the part of a specimen is presented as main published item', 'Parts', 2, 1)
GO

INSERT INTO ProjectDataType_Enum 
 (Code, Description, DisplayText, DisplayOrder, DisplayEnable) 
VALUES ('Observations', 'Observations without accession number, where the observation is presented as main published item', 'Observations', 3, 1)
GO

INSERT INTO ProjectDataType_Enum 
 (Code, Description, DisplayText, DisplayOrder, DisplayEnable) 
VALUES ('Organisms', 'Organisms resp. identification unis in a collection, where the organims within a specimen are presented as main published items', 'Organims', 3, 1)
GO


--#####################################################################################################################
--######   [ProjectProxy]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectProxy](
[ProjectID] [int] NOT NULL, 
[Project] [nvarchar](50) NULL, 
[ProjectURI] [varchar](255) NULL, 
[ProjectSettings] [xml] NULL,
[DataType] [nvarchar](50) NULL, 
[CoordinatePrecision] tinyint NULL, 
CONSTRAINT [PK_ProjectProxy] PRIMARY KEY CLUSTERED  
([ProjectID] ASC))
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the Projects derived from the assigned DiversityProjects database on the main server and their transfer type.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy' 
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data type of the Project as defined in ProjectDataType_Enum, used for transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'DataType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Precision of the coordinates published within the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'CoordinatePrecision'
GO


ALTER TABLE [dbo].[ProjectProxy]  WITH CHECK ADD  CONSTRAINT [FK_ProjectProxy_ProjectDataType_Enum] FOREIGN KEY([DataType]) 
REFERENCES [dbo].[ProjectDataType_Enum] ([Code])
GO

ALTER TABLE [dbo].[ProjectProxy] CHECK CONSTRAINT [FK_ProjectProxy_ProjectDataType_Enum]
GO

GRANT INSERT ON [ProjectProxy] TO [CacheAdministrator]
GO

GRANT UPDATE ON [ProjectProxy] TO [CacheAdministrator]
GO

GRANT DELETE ON [ProjectProxy] TO [CacheAdministrator]
GO

GRANT SELECT ON [ProjectProxy] TO [CacheUser]
GO


--#####################################################################################################################
--######   [ProjectTransferSetting_Enum]  ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TransferSetting_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_TransferSetting_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the possible settings used for a data transfer.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferSetting_Enum'
GO

ALTER TABLE [dbo].[TransferSetting_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT SELECT ON [TransferSetting_Enum] TO [CacheUser]
GO

INSERT INTO [TransferSetting_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('TaxonomicGroup'
           ,'The taxonomic groups that should be transfered within a project'
           ,'Taxonomic group'
           ,1
           ,1)
GO


--#####################################################################################################################
--######   [ProjectTransferSetting]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectTransferSetting](
	[ProjectID] [int] NOT NULL,
	[TransferSetting] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
	[DisplayText] [nvarchar](500) NOT NULL,
	[TransferToCache] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_ProjectTransferSetting] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[TransferSetting] ASC,
	[Value] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the transfer settings of a project.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransferSetting'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Transfer setting, e.g. taxonomic group' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransferSetting', @level2type=N'COLUMN',@level2name=N'TransferSetting'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value of Transfer setting, e.g. plant (as a taxonomic group)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransferSetting', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Display text of Transfer setting as shown in the user interface, e.g. Altitude for LocalisationSystemID = 4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransferSetting', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the content related to the entry should be transfered into the cache database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransferSetting', @level2type=N'COLUMN',@level2name=N'TransferToCache'
GO


ALTER TABLE [dbo].[ProjectTransferSetting]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTransferSetting_ProjectProxy] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectProxy] ([ProjectID])
GO

ALTER TABLE [dbo].[ProjectTransferSetting] CHECK CONSTRAINT [FK_ProjectTransferSetting_ProjectProxy]
GO

ALTER TABLE [dbo].[ProjectTransferSetting]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTransferSetting_TransferSetting_Enum] FOREIGN KEY([TransferSetting])
REFERENCES [dbo].[TransferSetting_Enum] ([Code])
GO

ALTER TABLE [dbo].[ProjectTransferSetting] CHECK CONSTRAINT [FK_ProjectTransferSetting_TransferSetting_Enum]
GO

ALTER TABLE [dbo].[ProjectTransferSetting] ADD  CONSTRAINT [DF_ProjectTransferSetting_TransferToCache]  DEFAULT ((1)) FOR [TransferToCache]
GO

ALTER TABLE [dbo].[ProjectTransferSetting] ADD  CONSTRAINT [DF_ProjectTransferSetting_LogInsertedBy]  DEFAULT (user_name()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[ProjectTransferSetting] ADD  CONSTRAINT [DF_ProjectTransferSetting_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[ProjectTransferSetting] ADD  CONSTRAINT [DF_ProjectTransferSetting_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectTransferSetting] ADD  CONSTRAINT [DF_ProjectTransferSetting_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO


ALTER TABLE [dbo].[ProjectTransferSetting] CHECK CONSTRAINT [FK_ProjectProxy_ProjectDataType_Enum]
GO

GRANT SELECT ON [ProjectTransferSetting] TO [CacheUser]
GO


GRANT INSERT ON [ProjectTransferSetting] TO [CacheAdministrator]
GO

GRANT UPDATE ON [ProjectTransferSetting] TO [CacheAdministrator]
GO

GRANT DELETE ON [ProjectTransferSetting] TO [CacheAdministrator]
GO


--#####################################################################################################################
--######   [DiversityTaxonNamesSources]  ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[DiversityTaxonNamesSources]( 
[DataSource] [varchar](255) NOT NULL, 
[DatabaseName] [varchar](255) NOT NULL, 
[LastUpdate] [datetime] NULL, 
CONSTRAINT [PK_DiversityTaxonNamesSources] PRIMARY KEY CLUSTERED  
([DataSource] ASC, 
[DatabaseName] ASC))
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the server and databases for the taxa.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesSources' 
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The server (including the port) where the database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesSources', @level2type=N'COLUMN',@level2name=N'DataSource'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesSources', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time of the last retrieval of data from the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesSources', @level2type=N'COLUMN',@level2name=N'LastUpdate'
GO

GRANT INSERT ON [DiversityTaxonNamesSources] TO [CacheAdministrator]
GO

GRANT UPDATE ON [DiversityTaxonNamesSources] TO [CacheAdministrator]
GO

GRANT DELETE ON [DiversityTaxonNamesSources] TO [CacheAdministrator]
GO

GRANT SELECT ON [DiversityTaxonNamesSources] TO [CacheAdministrator]
GO




--#####################################################################################################################
--######   [DiversityTaxonNamesProjectSequence]  ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[DiversityTaxonNamesProjectSequence]( 
[DataSource] [varchar](255) NOT NULL, 
[DatabaseName] [varchar](255) NOT NULL, 
[ProjectID] [int] NOT NULL, 
[Sequence] [int] NOT NULL, 
[Project] [nvarchar](255) NULL, 
CONSTRAINT [PK_DiversityTaxonNamesProjectSequence] PRIMARY KEY CLUSTERED  
( 
[DataSource] ASC, 
[DatabaseName] ASC, 
[ProjectID] ASC
))
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the sequence of the projects within the taxa, that means which projects should be used first.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesProjectSequence' 
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The server (including the port) where the database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesProjectSequence', @level2type=N'COLUMN',@level2name=N'DataSource'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesProjectSequence', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the project in the source database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesProjectSequence', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The sequence in the data retrieval' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesProjectSequence', @level2type=N'COLUMN',@level2name=N'Sequence'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The cached name of the project as shown in a interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DiversityTaxonNamesProjectSequence', @level2type=N'COLUMN',@level2name=N'Project'
GO


ALTER TABLE [dbo].[DiversityTaxonNamesProjectSequence]  WITH CHECK ADD  CONSTRAINT [FK_DiversityTaxonNamesProjectSequence_DiversityTaxonNamesSources] FOREIGN KEY([DataSource], [DatabaseName])
REFERENCES [dbo].[DiversityTaxonNamesSources] ([DataSource], [DatabaseName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


ALTER TABLE [dbo].[DiversityTaxonNamesProjectSequence] CHECK CONSTRAINT [FK_DiversityTaxonNamesProjectSequence_DiversityTaxonNames]
GO

GRANT INSERT ON [DiversityTaxonNamesProjectSequence] TO [CacheAdministrator]
GO

GRANT UPDATE ON [DiversityTaxonNamesProjectSequence] TO [CacheAdministrator]
GO

GRANT DELETE ON [DiversityTaxonNamesProjectSequence] TO [CacheAdministrator]
GO

GRANT SELECT ON [DiversityTaxonNamesProjectSequence] TO [CacheUser]
GO


--#####################################################################################################################
--######   [TaxonSynonymy]  ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaxonSynonymy](
[NameURI] [varchar](255) NOT NULL, 
[AcceptedName] [nvarchar](255) NOT NULL, 
[SynNameURI] [varchar](255) NOT NULL, 
[SynonymName] [nvarchar](255) NOT NULL, 
[TaxonomicRank] [nvarchar](50) NULL, 
[GenusOrSupragenericName] [nvarchar](200) NULL, 
[SpeciesGenusNameURI] [varchar](255) NULL, 
[TaxonNameSinAuthor] [nvarchar](800) NULL, 
[LogInsertedWhen] [datetime] NULL, 
[ProjectID] [int] NULL, 
CONSTRAINT [PK_TaxonSynonymy] PRIMARY KEY CLUSTERED 
([SynNameURI] ASC))
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds cached data from DiversityTaxonNames as base for other procedures.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymy' 
GO

ALTER TABLE [dbo].[TaxonSynonymy] ADD  CONSTRAINT [DF_TaxonSynonymy_LogInsertedWhen_1]  DEFAULT (getdate()) FOR [LogInsertedWhen] 
GO

GRANT INSERT ON [TaxonSynonymy] TO [CacheAdministrator]
GO

GRANT UPDATE ON [TaxonSynonymy] TO [CacheAdministrator]
GO

GRANT DELETE ON [TaxonSynonymy] TO [CacheAdministrator]
GO

GRANT SELECT ON [TaxonSynonymy] TO [CacheUser]
GO




--#####################################################################################################################
--######   [Interface]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Interface](
	[InterfaceName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Interface] PRIMARY KEY CLUSTERED 
(
	[InterfaceName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Interface', @level2type=N'COLUMN',@level2name=N'InterfaceName'
GO

GRANT INSERT ON [Interface] TO [CacheAdministrator]
GO

GRANT UPDATE ON [Interface] TO [CacheAdministrator]
GO

GRANT DELETE ON [Interface] TO [CacheAdministrator]
GO

GRANT SELECT ON [Interface] TO [CacheUser]
GO




--#####################################################################################################################
--######   [InterfaceTable]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[InterfaceTable](
	[InterfaceName] [nvarchar](50) NOT NULL,
	[TableName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_InterfaceTable] PRIMARY KEY CLUSTERED 
(
	[InterfaceName] ASC,
	[TableName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InterfaceTable', @level2type=N'COLUMN',@level2name=N'InterfaceName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the table, function of view within the interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InterfaceTable', @level2type=N'COLUMN',@level2name=N'TableName'
GO

GRANT INSERT ON [InterfaceTable] TO [CacheAdministrator]
GO

GRANT UPDATE ON [InterfaceTable] TO [CacheAdministrator]
GO

GRANT DELETE ON [InterfaceTable] TO [CacheAdministrator]
GO

GRANT SELECT ON [InterfaceTable] TO [CacheUser]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '03.00.01'
END

GO



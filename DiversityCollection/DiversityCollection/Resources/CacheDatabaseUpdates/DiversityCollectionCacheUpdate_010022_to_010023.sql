
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.22'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   Agent     ##################################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].Agent ADD AgentURI varchar(255) NULL;
GO


--#####################################################################################################################
--######   trgInsAgent   ##############################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgInsAgent] ON [dbo].[Agent] 
FOR INSERT AS
/* setting the AgentURI */ 
UPDATE t 
SET AgentURI = i.[BaseURL] + cast(i.[AgentID] as varchar)
FROM Agent T, inserted i
where i.AgentID = T.AgentID and T.BaseURL = i.BaseURL;
GO



--#####################################################################################################################
--######   Gazetteer    ###############################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].Gazetteer ADD NameURI varchar(255) NULL;
GO

--#####################################################################################################################
--######   trgInsGazetteer   ##########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgInsGazetteer] ON [dbo].[Gazetteer] 
FOR INSERT AS
/* setting the NameURI */ 
UPDATE t 
SET NameURI = i.[BaseURL] + cast(i.[NameID] as varchar)
FROM Gazetteer T, inserted i
where i.NameID = T.NameID and T.BaseURL = i.BaseURL;
GO



--#####################################################################################################################
--######   ReferenceTitle    ##########################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].ReferenceTitle ADD ReferenceURI varchar(255) NULL;
GO

--#####################################################################################################################
--######   trgInsReferenceTitle   #####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgInsReferenceTitle] ON [dbo].[ReferenceTitle] 
FOR INSERT AS
/* setting the NameURI */ 
UPDATE t 
SET ReferenceURI = i.[BaseURL] + cast(i.[RefID] as varchar)
FROM ReferenceTitle T, inserted i
where i.RefID = T.RefID and T.BaseURL = i.BaseURL;
GO


--#####################################################################################################################
--######   ScientificTerm_PK   ########################################################################################
--#####################################################################################################################

DELETE FROM [dbo].[ScientificTerm] WHERE [RepresentationURI] IS NULL
GO

ALTER  TABLE [dbo].[ScientificTerm] ALTER COLUMN [RepresentationURI] nvarchar(255) NOT NULL
GO

ALTER TABLE [dbo].[ScientificTerm]
ADD CONSTRAINT [ScientificTerm_PK] PRIMARY KEY NONCLUSTERED 
(
	[RepresentationURI] ASC
)
GO

--#####################################################################################################################
--######   TaxonSynonymy    ###########################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].TaxonSynonymy ADD NameURI varchar(255) NULL;
GO

--#####################################################################################################################
--######   trgInsTaxonSynonymy   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgInsTaxonSynonymy] ON [dbo].[TaxonSynonymy] 
FOR INSERT AS
/* setting the NameURI */ 
UPDATE t 
SET NameURI = i.[BaseURL] + cast(i.[NameID] as varchar)
FROM TaxonSynonymy T, inserted i
where i.NameID = T.NameID and T.BaseURL = i.BaseURL;
GO



--#####################################################################################################################
--######   ViewAnalysis: No restriction to project   ##################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAnalysis]
AS
SELECT DISTINCT A.[AnalysisID]
,[AnalysisParentID]
,[DisplayText]
,[Description]
,[MeasurementUnit]
,[Notes]
,[AnalysisURI]
,[OnlyHierarchy]
,A.[LogUpdatedWhen]
FROM            
' +  dbo.SourceDatabase() + '.dbo.Analysis AS A')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   ProjectTarget   ############################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ProjectTarget](
	[ProjectID] [int] NOT NULL,
	[Target] [nvarchar](255) NOT NULL,
	[LastUpdatedWhen] [datetime] NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL CONSTRAINT [DF_ProjectTarget_IncludeInTransfer]  DEFAULT ((1)),
	[CompareLogDate] [bit] NULL CONSTRAINT [DF_ProjectTarget_CompareLogDate]  DEFAULT ((0)),
	[TransferDays] [varchar](7) NULL CONSTRAINT [DF_ProjectTarget_TransferDays]  DEFAULT ((0)),
	[TransferTime] [time](7) NULL CONSTRAINT [DF_ProjectTarget_TransferTime]  DEFAULT ('00:00:00.00'),
	[TransferIsExecutedBy] [nvarchar](500) NULL,
	[TransferErrors] [nvarchar](max) NULL,
 CONSTRAINT [PK_ProjectTarget] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[Target] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


ALTER TABLE [dbo].[ProjectTarget]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTarget_ProjectPublished] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectPublished] ([ProjectID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectTarget] CHECK CONSTRAINT [FK_ProjectTarget_ProjectPublished]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project to which the specimen belongs (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases where the data should be transferred to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the project data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the project should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget'
GO


GRANT SELECT ON [ProjectTarget] TO [CacheUser]
GO
GRANT INSERT ON [ProjectTarget] TO [CacheAdmin]
GO
GRANT UPDATE ON [ProjectTarget] TO [CacheAdmin]
GO
GRANT DELETE ON [ProjectTarget] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   ProjectPublished - Adding columns for scheduled transfer   #################################################
--#####################################################################################################################

ALTER TABLE ProjectPublished ADD [CompareLogDate] [bit] NULL CONSTRAINT [DF_ProjectPublished_CompareLogDate]  DEFAULT ((0))
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

ALTER TABLE ProjectPublished ADD [TransferDays] [varchar](7) NULL CONSTRAINT [DF_ProjectPublished_TransferDays]  DEFAULT ('0')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

ALTER TABLE ProjectPublished ADD [TransferTime] [time](7) NULL CONSTRAINT [DF_ProjectPublished_TransferTime]  DEFAULT ('00:00:00.00')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

ALTER TABLE ProjectPublished ADD [TransferIsExecutedBy] [nvarchar](500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

ALTER TABLE ProjectPublished ADD [TransferErrors] [nvarchar](max) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO


--#####################################################################################################################
--######   AgentSource - Adding columns for scheduled transfer   ######################################################
--#####################################################################################################################

ALTER TABLE AgentSource ADD [LastUpdatedWhen] [datetime] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

ALTER TABLE AgentSource ADD [CompareLogDate] [bit] NULL CONSTRAINT [DF_AgentSource_CompareLogDate]  DEFAULT ((0))
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

ALTER TABLE AgentSource ADD [TransferDays] [varchar](7) NULL CONSTRAINT [DF_AgentSource_TransferDays]  DEFAULT ('0')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

ALTER TABLE AgentSource ADD [TransferTime] [time](7) NULL CONSTRAINT [DF_AgentSource_TransferTime]  DEFAULT ('00:00:00.00')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

ALTER TABLE AgentSource ADD [TransferIsExecutedBy] [nvarchar](500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

ALTER TABLE AgentSource ADD [TransferErrors] [nvarchar](max) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO


--#####################################################################################################################
--######   GazetteerSource - Adding columns for scheduled transfer   ##################################################
--#####################################################################################################################

ALTER TABLE GazetteerSource ADD [LastUpdatedWhen] [datetime] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

ALTER TABLE GazetteerSource ADD [CompareLogDate] [bit] NULL CONSTRAINT [DF_GazetteerSource_CompareLogDate]  DEFAULT ((0))
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

ALTER TABLE GazetteerSource ADD [TransferDays] [varchar](7) NULL CONSTRAINT [DF_GazetteerSource_TransferDays]  DEFAULT ('0')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

ALTER TABLE GazetteerSource ADD [TransferTime] [time](7) NULL CONSTRAINT [DF_GazetteerSource_TransferTime]  DEFAULT ('00:00:00.00')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

ALTER TABLE GazetteerSource ADD [TransferIsExecutedBy] [nvarchar](500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

ALTER TABLE GazetteerSource ADD [TransferErrors] [nvarchar](max) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO



--#####################################################################################################################
--######   ReferenceSource - Adding columns for scheduled transfer   ##################################################
--#####################################################################################################################

ALTER TABLE ReferenceSource ADD [LastUpdatedWhen] [datetime] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

ALTER TABLE ReferenceSource ADD [CompareLogDate] [bit] NULL CONSTRAINT [DF_ReferenceSource_CompareLogDate]  DEFAULT ((0))
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

ALTER TABLE ReferenceSource ADD [TransferDays] [varchar](7) NULL CONSTRAINT [DF_ReferenceSource_TransferDays]  DEFAULT ('0')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

ALTER TABLE ReferenceSource ADD [TransferTime] [time](7) NULL CONSTRAINT [DF_ReferenceSource_TransferTime]  DEFAULT ('00:00:00.00')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

ALTER TABLE ReferenceSource ADD [TransferIsExecutedBy] [nvarchar](500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

ALTER TABLE ReferenceSource ADD [TransferErrors] [nvarchar](max) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO




--#####################################################################################################################
--######   ScientificTermSource - Adding columns for scheduled transfer   #############################################
--#####################################################################################################################

ALTER TABLE ScientificTermSource ADD [LastUpdatedWhen] [datetime] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

ALTER TABLE ScientificTermSource ADD [CompareLogDate] [bit] NULL CONSTRAINT [DF_ScientificTermSource_CompareLogDate]  DEFAULT ((0))
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

ALTER TABLE ScientificTermSource ADD [TransferDays] [varchar](7) NULL CONSTRAINT [DF_ScientificTermSource_TransferDays]  DEFAULT ('0')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

ALTER TABLE ScientificTermSource ADD [TransferTime] [time](7) NULL CONSTRAINT [DF_ScientificTermSource_TransferTime]  DEFAULT ('00:00:00.00')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

ALTER TABLE ScientificTermSource ADD [TransferIsExecutedBy] [nvarchar](500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

ALTER TABLE ScientificTermSource ADD [TransferErrors] [nvarchar](max) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO



--#####################################################################################################################
--######   TaxonSynonymySource - Adding columns for scheduled transfer   ##############################################
--#####################################################################################################################

ALTER TABLE TaxonSynonymySource ADD [LastUpdatedWhen] [datetime] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

ALTER TABLE TaxonSynonymySource ADD [CompareLogDate] [bit] NULL CONSTRAINT [DF_TaxonSynonymySource_CompareLogDate]  DEFAULT ((0))
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

ALTER TABLE TaxonSynonymySource ADD [TransferDays] [varchar](7) NULL CONSTRAINT [DF_TaxonSynonymySource_TransferDays]  DEFAULT ('0')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

ALTER TABLE TaxonSynonymySource ADD [TransferTime] [time](7) NULL CONSTRAINT [DF_TaxonSynonymySource_TransferTime]  DEFAULT ('00:00:00.00')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

ALTER TABLE TaxonSynonymySource ADD [TransferIsExecutedBy] [nvarchar](500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

ALTER TABLE TaxonSynonymySource ADD [TransferErrors] [nvarchar](max) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO




--#####################################################################################################################
--######   AgentSourceTarget   ########################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[AgentSourceTarget](
	[SourceView] [nvarchar](200) NOT NULL,
	[Target] [nvarchar](200) NOT NULL,
	[LastUpdatedWhen] [datetime] NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL CONSTRAINT [DF_AgentSourceTarget_IncludeInTransfer]  DEFAULT ((1)),
	[CompareLogDate] [bit] NULL CONSTRAINT [DF_AgentSourceTarget_CompareLogDate]  DEFAULT ((0)),
	[TransferDays] [varchar](7) NULL CONSTRAINT [DF_AgentSourceTarget_TransferDays]  DEFAULT ((0)),
	[TransferTime] [time](7) NULL CONSTRAINT [DF_AgentSourceTarget_TransferTime]  DEFAULT ('00:00:00.00'),
	[TransferIsExecutedBy] [nvarchar](500) NULL,
	[TransferErrors] [nvarchar](max) NULL,
 CONSTRAINT [PK_AgentSourceTarget] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC,
	[Target] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


ALTER TABLE [dbo].[AgentSourceTarget]  WITH CHECK ADD  CONSTRAINT [FK_AgentSourceTarget_AgentSource] FOREIGN KEY([SourceView])
REFERENCES [dbo].[AgentSource] ([SourceView])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AgentSourceTarget] CHECK CONSTRAINT [FK_AgentSourceTarget_AgentSource]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SourceView as defined in table AgentSource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases where the data should be transferred to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the project data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the project should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget'
GO


GRANT SELECT ON [AgentSourceTarget] TO [CacheUser]
GO
GRANT INSERT ON [AgentSourceTarget] TO [CacheAdmin]
GO
GRANT UPDATE ON [AgentSourceTarget] TO [CacheAdmin]
GO
GRANT DELETE ON [AgentSourceTarget] TO [CacheAdmin]
GO




--#####################################################################################################################
--######   GazetteerSourceTarget   ####################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[GazetteerSourceTarget](
	[SourceView] [nvarchar](200) NOT NULL,
	[Target] [nvarchar](200) NOT NULL,
	[LastUpdatedWhen] [datetime] NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL CONSTRAINT [DF_GazetteerSourceTarget_IncludeInTransfer]  DEFAULT ((1)),
	[CompareLogDate] [bit] NULL CONSTRAINT [DF_GazetteerSourceTarget_CompareLogDate]  DEFAULT ((0)),
	[TransferDays] [varchar](7) NULL CONSTRAINT [DF_GazetteerSourceTarget_TransferDays]  DEFAULT ((0)),
	[TransferTime] [time](7) NULL CONSTRAINT [DF_GazetteerSourceTarget_TransferTime]  DEFAULT ('00:00:00.00'),
	[TransferIsExecutedBy] [nvarchar](500) NULL,
	[TransferErrors] [nvarchar](max) NULL,
 CONSTRAINT [PK_GazetteerSourceTarget] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC,
	[Target] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


ALTER TABLE [dbo].[GazetteerSourceTarget]  WITH CHECK ADD  CONSTRAINT [FK_GazetteerSourceTarget_GazetteerSource] FOREIGN KEY([SourceView])
REFERENCES [dbo].[GazetteerSource] ([SourceView])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[GazetteerSourceTarget] CHECK CONSTRAINT [FK_GazetteerSourceTarget_GazetteerSource]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SourceView as defined in table GazetteerSource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases where the data should be transferred to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the project data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the project should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget'
GO


GRANT SELECT ON [GazetteerSourceTarget] TO [CacheUser]
GO
GRANT INSERT ON [GazetteerSourceTarget] TO [CacheAdmin]
GO
GRANT UPDATE ON [GazetteerSourceTarget] TO [CacheAdmin]
GO
GRANT DELETE ON [GazetteerSourceTarget] TO [CacheAdmin]
GO




--#####################################################################################################################
--######   ReferenceSourceTarget   ####################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ReferenceSourceTarget](
	[SourceView] [nvarchar](200) NOT NULL,
	[Target] [nvarchar](200) NOT NULL,
	[LastUpdatedWhen] [datetime] NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL CONSTRAINT [DF_ReferenceSourceTarget_IncludeInTransfer]  DEFAULT ((1)),
	[CompareLogDate] [bit] NULL CONSTRAINT [DF_ReferenceSourceTarget_CompareLogDate]  DEFAULT ((0)),
	[TransferDays] [varchar](7) NULL CONSTRAINT [DF_ReferenceSourceTarget_TransferDays]  DEFAULT ((0)),
	[TransferTime] [time](7) NULL CONSTRAINT [DF_ReferenceSourceTarget_TransferTime]  DEFAULT ('00:00:00.00'),
	[TransferIsExecutedBy] [nvarchar](500) NULL,
	[TransferErrors] [nvarchar](max) NULL,
 CONSTRAINT [PK_ReferenceSourceTarget] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC,
	[Target] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


ALTER TABLE [dbo].[ReferenceSourceTarget]  WITH CHECK ADD  CONSTRAINT [FK_ReferenceSourceTarget_ReferenceSource] FOREIGN KEY([SourceView])
REFERENCES [dbo].[ReferenceSource] ([SourceView])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ReferenceSourceTarget] CHECK CONSTRAINT [FK_ReferenceSourceTarget_ReferenceSource]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SourceView as defined in table ReferenceSource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases where the data should be transferred to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the project data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the project should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSourceTarget'
GO


GRANT SELECT ON [ReferenceSourceTarget] TO [CacheUser]
GO
GRANT INSERT ON [ReferenceSourceTarget] TO [CacheAdmin]
GO
GRANT UPDATE ON [ReferenceSourceTarget] TO [CacheAdmin]
GO
GRANT DELETE ON [ReferenceSourceTarget] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   ScientificTermSource SourceView -> 200   ###################################################################
--#####################################################################################################################

DELETE FROM ScientificTermSource WHERE LEN(SourceView) > 200
GO

ALTER TABLE [dbo].[ScientificTermSource] DROP CONSTRAINT [PK_ScientificTermSource]
GO

ALTER TABLE ScientificTermSource ALTER COLUMN SourceView nvarchar(200) NOT NULL
GO

ALTER TABLE [dbo].[ScientificTermSource] ADD  CONSTRAINT [PK_ScientificTermSource] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC
)
GO

--#####################################################################################################################
--######   ScientificTermSourceTarget   ###############################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ScientificTermSourceTarget](
	[SourceView] [nvarchar](200) NOT NULL,
	[Target] [nvarchar](200) NOT NULL,
	[LastUpdatedWhen] [datetime] NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL CONSTRAINT [DF_ScientificTermSourceTarget_IncludeInTransfer]  DEFAULT ((1)),
	[CompareLogDate] [bit] NULL CONSTRAINT [DF_ScientificTermSourceTarget_CompareLogDate]  DEFAULT ((0)),
	[TransferDays] [varchar](7) NULL CONSTRAINT [DF_ScientificTermSourceTarget_TransferDays]  DEFAULT ((0)),
	[TransferTime] [time](7) NULL CONSTRAINT [DF_ScientificTermSourceTarget_TransferTime]  DEFAULT ('00:00:00.00'),
	[TransferIsExecutedBy] [nvarchar](500) NULL,
	[TransferErrors] [nvarchar](max) NULL,
 CONSTRAINT [PK_ScientificTermSourceTarget] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC,
	[Target] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


ALTER TABLE [dbo].[ScientificTermSourceTarget]  WITH CHECK ADD  CONSTRAINT [FK_ScientificTermSourceTarget_ScientificTermSource] FOREIGN KEY([SourceView])
REFERENCES [dbo].[ScientificTermSource] ([SourceView])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ScientificTermSourceTarget] CHECK CONSTRAINT [FK_ScientificTermSourceTarget_ScientificTermSource]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SourceView as defined in table ScientificTermSource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases where the data should be transferred to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the project data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the project should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget'
GO


GRANT SELECT ON [ScientificTermSourceTarget] TO [CacheUser]
GO
GRANT INSERT ON [ScientificTermSourceTarget] TO [CacheAdmin]
GO
GRANT UPDATE ON [ScientificTermSourceTarget] TO [CacheAdmin]
GO
GRANT DELETE ON [ScientificTermSourceTarget] TO [CacheAdmin]
GO




--#####################################################################################################################
--######   TaxonSynonymySourceTarget   ################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[TaxonSynonymySourceTarget](
	[SourceView] [nvarchar](200) NOT NULL,
	[Target] [nvarchar](200) NOT NULL,
	[LastUpdatedWhen] [datetime] NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL CONSTRAINT [DF_TaxonSynonymySourceTarget_IncludeInTransfer]  DEFAULT ((1)),
	[CompareLogDate] [bit] NULL CONSTRAINT [DF_TaxonSynonymySourceTarget_CompareLogDate]  DEFAULT ((0)),
	[TransferDays] [varchar](7) NULL CONSTRAINT [DF_TaxonSynonymySourceTarget_TransferDays]  DEFAULT ((0)),
	[TransferTime] [time](7) NULL CONSTRAINT [DF_TaxonSynonymySourceTarget_TransferTime]  DEFAULT ('00:00:00.00'),
	[TransferIsExecutedBy] [nvarchar](500) NULL,
	[TransferErrors] [nvarchar](max) NULL,
 CONSTRAINT [PK_TaxonSynonymySourceTarget] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC,
	[Target] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


ALTER TABLE [dbo].[TaxonSynonymySourceTarget]  WITH CHECK ADD  CONSTRAINT [FK_TaxonSynonymySourceTarget_TaxonSynonymySource] FOREIGN KEY([SourceView])
REFERENCES [dbo].[TaxonSynonymySource] ([SourceView])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TaxonSynonymySourceTarget] CHECK CONSTRAINT [FK_TaxonSynonymySourceTarget_TaxonSynonymySource]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SourceView as defined in table TaxonSynonymySource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases where the data should be transferred to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the project data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the project should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget'
GO


GRANT SELECT ON [TaxonSynonymySourceTarget] TO [CacheUser]
GO
GRANT INSERT ON [TaxonSynonymySourceTarget] TO [CacheAdmin]
GO
GRANT UPDATE ON [TaxonSynonymySourceTarget] TO [CacheAdmin]
GO
GRANT DELETE ON [TaxonSynonymySourceTarget] TO [CacheAdmin]
GO

--#####################################################################################################################
--######   TaxonNameExternalDatabase: ExternalDatabaseName -> nvarchar(800)   #########################################
--#####################################################################################################################

ALTER TABLE TaxonNameExternalDatabase ALTER COLUMN [ExternalDatabaseName] nvarchar(800) NULL
GO

--#####################################################################################################################
--######   Removing constraints for LastUpdatedBy and LastUpdatedWhen in ProjectPublished  ############################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectPublished] DROP CONSTRAINT [DF_ProjectPublished_LastUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectPublished] DROP CONSTRAINT [DF_ProjectPublished_LastUpdatedWhen]
GO


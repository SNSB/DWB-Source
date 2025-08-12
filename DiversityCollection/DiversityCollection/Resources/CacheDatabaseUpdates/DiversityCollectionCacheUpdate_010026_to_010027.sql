
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.26'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  New tables to include module DiversitySamplingPlots    ######################################################
--#####################################################################################################################
--######  SamplingPlot    #############################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[SamplingPlot](
	[BaseURL] [varchar](500) NOT NULL,
	[PlotID] [int] NOT NULL,
	[PartOfPlotID] [int] NULL,
	[PlotURI] [nvarchar](255) NULL,
	[PlotIdentifier] [nvarchar](500) NULL,
	[PlotGeography_Cache] [nvarchar](max) NULL,
	[PlotDescription] [nvarchar](max) NULL,
	[PlotType] [nvarchar](50) NULL,
	[CountryCache] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[ProjectID] [int] NULL,
	[SourceView] [nvarchar](400) NULL,
 CONSTRAINT [SamplingPlot_PK] PRIMARY KEY NONCLUSTERED 
(
	[BaseURL] ASC,
	[PlotID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SamplingPlot] ADD  CONSTRAINT [DF_SamplingPlot_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The source of the data,i.e. the name of the view in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds cached data from DiversitySamplingPlots as base for other procedures.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot'
GO

GRANT SELECT ON [SamplingPlot] TO [CacheUser]
GO

GRANT INSERT ON [SamplingPlot] TO [CacheAdmin] 
GO

GRANT UPDATE ON [SamplingPlot] TO [CacheAdmin] 
GO

GRANT DELETE ON [SamplingPlot] TO [CacheAdmin] 
GO



--#####################################################################################################################
--######  SamplingPlotLocalisation    #################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[SamplingPlotLocalisation](
	[PlotID] [int] NOT NULL,
	[LocalisationSystemID] [int] NOT NULL,
	[Location1] [nvarchar](255) NULL,
	[Location2] [nvarchar](255) NULL,
	[LocationAccuracy] [nvarchar](50) NULL,
	[LocationNotes] [nvarchar](max) NULL,
	[Geography] [nvarchar](max) NULL,
	[AverageAltitudeCache] [float] NULL,
	[AverageLatitudeCache] [float] NULL,
	[AverageLongitudeCache] [float] NULL,
	[SourceView] [nvarchar](400) NULL,
 CONSTRAINT [PK_SamplingPlotLocalisation] PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[LocalisationSystemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GRANT SELECT ON [SamplingPlotLocalisation] TO [CacheUser]
GO

GRANT INSERT ON [SamplingPlotLocalisation] TO [CacheAdmin] 
GO

GRANT UPDATE ON [SamplingPlotLocalisation] TO [CacheAdmin] 
GO

GRANT DELETE ON [SamplingPlotLocalisation] TO [CacheAdmin] 
GO



--#####################################################################################################################
--######  SamplingPlotProperty    #####################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[SamplingPlotProperty](
	[PlotID] [int] NOT NULL,
	[PropertyID] [int] NOT NULL,
	[DisplayText] [nvarchar](255) NULL,
	[PropertyURI] [varchar](255) NULL,
	[PropertyHierarchyCache] [nvarchar](max) NULL,
	[PropertyValue] [nvarchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[AverageValueCache] [float] NULL,
	[SourceView] [nvarchar](400) NULL,
 CONSTRAINT [PK_SamplingPlotProperty] PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[PropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GRANT SELECT ON [SamplingPlotProperty] TO [CacheUser]
GO

GRANT INSERT ON [SamplingPlotProperty] TO [CacheAdmin] 
GO

GRANT UPDATE ON [SamplingPlotProperty] TO [CacheAdmin] 
GO

GRANT DELETE ON [SamplingPlotProperty] TO [CacheAdmin] 
GO



--#####################################################################################################################
--######  SamplingPlotSource    #######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[SamplingPlotSource](
	[SourceView] [nvarchar](200) NOT NULL,
	[Source] [nvarchar](50) NULL,
	[SourceID] [int] NULL,
	[LinkedServerName] [nvarchar](400) NULL,
	[DatabaseName] [nvarchar](400) NULL,
	[Subsets] [nvarchar](500) NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL,
	[LastUpdatedWhen] [datetime] NULL,
	[CompareLogDate] [bit] NULL,
	[TransferDays] [varchar](7) NULL,
	[TransferTime] [time](7) NULL,
	[TransferIsExecutedBy] [nvarchar](500) NULL,
	[TransferErrors] [nvarchar](max) NULL,
 CONSTRAINT [PK_SamplingPlotSource] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SamplingPlotSource] ADD  CONSTRAINT [DF_SamplingPlotSource_LastUpdatedWhen]  DEFAULT (getdate()) FOR [LastUpdatedWhen]
GO

ALTER TABLE [dbo].[SamplingPlotSource] ADD  CONSTRAINT [DF_SamplingPlotSource_CompareLogDate]  DEFAULT ((0)) FOR [CompareLogDate]
GO

ALTER TABLE [dbo].[SamplingPlotSource] ADD  CONSTRAINT [DF_SamplingPlotSource_TransferDays]  DEFAULT ('0') FOR [TransferDays]
GO

ALTER TABLE [dbo].[SamplingPlotSource] ADD  CONSTRAINT [DF_SamplingPlotSource_TransferTime]  DEFAULT ('00:00:00.00') FOR [TransferTime]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'the name of the view retrieving the data from the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source is located on a linked server, the name of the linked server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'LinkedServerName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database where the data are taken from' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'List of additional data transferred into the cache database separated by |' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'Subsets'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO

GRANT SELECT ON [SamplingPlotSource] TO [CacheUser]
GO

GRANT INSERT ON [SamplingPlotSource] TO [CacheAdmin] 
GO

GRANT UPDATE ON [SamplingPlotSource] TO [CacheAdmin] 
GO

GRANT DELETE ON [SamplingPlotSource] TO [CacheAdmin] 
GO




--#####################################################################################################################
--######  SamplingPlotSourceTarget    #################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[SamplingPlotSourceTarget](
	[SourceView] [nvarchar](200) NOT NULL,
	[Target] [nvarchar](200) NOT NULL,
	[LastUpdatedWhen] [datetime] NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL,
	[CompareLogDate] [bit] NULL,
	[TransferDays] [varchar](7) NULL,
	[TransferTime] [time](7) NULL,
	[TransferIsExecutedBy] [nvarchar](500) NULL,
	[TransferErrors] [nvarchar](max) NULL,
 CONSTRAINT [PK_SamplingPlotSourceTarget] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC,
	[Target] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SamplingPlotSourceTarget] ADD  CONSTRAINT [DF_SamplingPlotSourceTarget_LastUpdatedWhen]  DEFAULT (getdate()) FOR [LastUpdatedWhen]
GO

ALTER TABLE [dbo].[SamplingPlotSourceTarget] ADD  CONSTRAINT [DF_SamplingPlotSourceTarget_IncludeInTransfer]  DEFAULT ((1)) FOR [IncludeInTransfer]
GO

ALTER TABLE [dbo].[SamplingPlotSourceTarget] ADD  CONSTRAINT [DF_SamplingPlotSourceTarget_CompareLogDate]  DEFAULT ((0)) FOR [CompareLogDate]
GO

ALTER TABLE [dbo].[SamplingPlotSourceTarget] ADD  CONSTRAINT [DF_SamplingPlotSourceTarget_TransferDays]  DEFAULT ((0)) FOR [TransferDays]
GO

ALTER TABLE [dbo].[SamplingPlotSourceTarget] ADD  CONSTRAINT [DF_SamplingPlotSourceTarget_TransferTime]  DEFAULT ('00:00:00.00') FOR [TransferTime]
GO

ALTER TABLE [dbo].[SamplingPlotSourceTarget]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotSourceTarget_SamplingPlotSource] FOREIGN KEY([SourceView])
REFERENCES [dbo].[SamplingPlotSource] ([SourceView])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SamplingPlotSourceTarget] CHECK CONSTRAINT [FK_SamplingPlotSourceTarget_SamplingPlotSource]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SourceView as defined in table SamplingPlotSource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases where the data should be transferred to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the project data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the project should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the log dates of the transferred data should be compared to decide if data are transferred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'CompareLogDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The days the transfer should be done, coded as integer values with Sunday = 0 up to Saturday = 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when the transfer should be executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If any transfer of the data is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferIsExecutedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Errors that occurred during the data transfers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'TransferErrors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The targets of the projects, i.e. the Postgres databases' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget'
GO

GRANT SELECT ON [SamplingPlotSourceTarget] TO [CacheUser]
GO

GRANT INSERT ON [SamplingPlotSourceTarget] TO [CacheAdmin] 
GO

GRANT UPDATE ON [SamplingPlotSourceTarget] TO [CacheAdmin] 
GO

GRANT DELETE ON [SamplingPlotSourceTarget] TO [CacheAdmin] 
GO



--#####################################################################################################################
--######  Sources - for access by clients - inclusion of Sampling plots    ############################################
--#####################################################################################################################
CREATE VIEW [dbo].[Sources]
AS
SELECT        SourceView, AgentURI AS URI, AgentName AS DisplayText, AgentID AS ID, BaseURL
FROM            dbo.Agent
UNION
SELECT        SourceView, NameURI AS URI, TaxonName AS DisplayText, NameID AS ID, BaseURL
FROM            TaxonSynonymy
UNION
SELECT        SourceView, ReferenceURI AS URI, RefDescription_Cache AS DisplayText, RefID AS ID, BaseURL
FROM            ReferenceTitle
UNION
SELECT        SourceView, NameURI AS URI, Name AS DisplayText, NameID AS ID, BaseURL
FROM            Gazetteer
UNION
SELECT        SourceView, PlotURI AS URI, PlotIdentifier AS DisplayText, PlotID AS ID, BaseURL
FROM            SamplingPlot
UNION
SELECT        SourceView, RepresentationURI AS URI, DisplayText, cast(reverse(substring(reverse(RepresentationURI), 1, CHARINDEX('/', reverse(RepresentationURI))-1)) as int) AS ID, reverse(substring(reverse(RepresentationURI), CHARINDEX('/', reverse(RepresentationURI)), 500)) AS BaseURL
FROM            ScientificTerm
GO

grant select on sources to [CacheUser]

--#####################################################################################################################
--######  Additional columns for table ScientificTerm   ###############################################################
--#####################################################################################################################
-- evtl. überflüssig - kann mit Sicht geloest werden

/*
ALTER TABLE [dbo].[ScientificTerm] ADD [BaseURL] [varchar](500) NULL;
GO

ALTER TABLE [dbo].[ScientificTerm] ADD [RepresentationID] [int] NULL;
GO
*/

--#####################################################################################################################
--######  Increasing size of columns in table Target   ################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[Target] ALTER COLUMN [Server] [nvarchar](255) NOT NULL;
GO

ALTER TABLE [dbo].[Target] ALTER COLUMN [DatabaseName] [nvarchar](255) NOT NULL;
GO

--#####################################################################################################################
--######  Adding columns for last check to source and target tables  ##################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'AgentSource' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[AgentSource] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'AgentSourceTarget' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[AgentSourceTarget] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceTarget', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'GazetteerSource' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[GazetteerSource] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'GazetteerSourceTarget' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[GazetteerSourceTarget] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceTarget', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectPublished' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[ProjectPublished] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectTarget' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[ProjectTarget] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ReferenceTitleSource' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[ReferenceTitleSource] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceTitleSource', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ReferenceTitleSourceTarget' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[ReferenceTitleSourceTarget] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceTitleSourceTarget', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ScientificTermSource' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[ScientificTermSource] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ScientificTermSourceTarget' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[ScientificTermSourceTarget] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceTarget', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TaxonSynonymySource' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymySource] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TaxonSynonymySourceTarget' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymySourceTarget] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceTarget', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotSource' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[SamplingPlotSource] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSource', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotSourceTarget' AND C.COLUMN_NAME = 'LastCheckedWhen') = 0
begin
	ALTER TABLE [dbo].[SamplingPlotSourceTarget] ADD LastCheckedWhen datetime NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the last check for the need of an update of the content occurred' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceTarget', @level2type=N'COLUMN',@level2name=N'LastCheckedWhen'
end
GO




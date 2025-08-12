
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '03.00.01'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO




--#####################################################################################################################
--######   [CollectionAgent]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionAgent](
	[CollectionSpecimenID] [int] NOT NULL,
	[CollectorsName] [nvarchar](255) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[CollectorsAgentURI] [varchar](255) NULL,
	[CollectorsSequence] [datetime] NULL,
	[CollectorsNumber] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CollectionAgent] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[CollectorsName] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionAgent] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionAgent] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionAgent] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionAgent] TO [CacheUser]
GO




--#####################################################################################################################
--######   [CollectionEvent]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionEvent](
	[CollectionEventID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[SeriesID] [int] NULL,
	[CollectorsEventNumber] [nvarchar](50) NULL,
	[CollectionDate] [datetime] NULL,
	[CollectionDay] [tinyint] NULL,
	[CollectionMonth] [tinyint] NULL,
	[CollectionYear] [smallint] NULL,
	[CollectionDateSupplement] [nvarchar](100) NULL,
	[CollectionDateCategory] [nvarchar](50) NULL,
	[CollectionTime] [varchar](50) NULL,
	[CollectionTimeSpan] [varchar](50) NULL,
	[LocalityDescription] [nvarchar](max) NULL,
	[HabitatDescription] [nvarchar](max) NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[CollectingMethod] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CountryCache] [nvarchar](50) NULL,
 CONSTRAINT [PK_CollectionEvent] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionEvent] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionEvent] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionEvent] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionEvent] TO [CacheUser]
GO




--#####################################################################################################################
--######   [CollectionEventImage]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionEventImage](
	[CollectionEventID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ResourceURI] [varchar](255) NULL,
	[ImageType] [nvarchar](50) NULL,
	[Description] [xml] NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CollectionEventImage] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[URI] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionEventImage] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionEventImage] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionEventImage] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionEventImage] TO [CacheUser]
GO


--#####################################################################################################################
--######   [CollectionEventLocalisation]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionEventLocalisation](
	[CollectionEventID] [int] NOT NULL,
	[LocalisationSystemID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Location1] [nvarchar](255) NULL,
	[Location2] [nvarchar](255) NULL,
	[LocationAccuracy] [nvarchar](50) NULL,
	[LocationNotes] [nvarchar](max) NULL,
	[DeterminationDate] [smalldatetime] NULL,
	[DistanceToLocation] [varchar](50) NULL,
	[DirectionToLocation] [varchar](50) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[RecordingMethod] [nvarchar](500) NULL,
	[Geography] [geography] NULL,
	[AverageAltitudeCache] [float] NULL,
	[AverageLatitudeCache] [float] NULL,
	[AverageLongitudeCache] [float] NULL,
 CONSTRAINT [PK_CollectionLocalisation] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[LocalisationSystemID] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionEventLocalisation] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionEventLocalisation] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionEventLocalisation] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionEventLocalisation] TO [CacheUser]
GO



--#####################################################################################################################
--######   [CollectionEventProperty]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionEventProperty](
	[CollectionEventID] [int] NOT NULL,
	[PropertyID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[DisplayText] [nvarchar](255) NULL,
	[PropertyURI] [varchar](255) NULL,
	[PropertyHierarchyCache] [nvarchar](max) NULL,
	[PropertyValue] [nvarchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[AverageValueCache] [float] NULL,
 CONSTRAINT [PK_CollectionEventCharacter] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[PropertyID] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionEventProperty] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionEventProperty] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionEventProperty] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionEventProperty] TO [CacheUser]
GO



--#####################################################################################################################
--######   [CollectionEventSeries]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionEventSeries](
	[SeriesID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[SeriesParentID] [int] NULL,
	[Description] [nvarchar](max) NOT NULL,
	[SeriesCode] [nvarchar](50) NULL,
	[Geography] [geography] NULL,
	[Notes] [nvarchar](max) NULL,
	[DateStart] [datetime] NULL,
	[DateEnd] [datetime] NULL,
 CONSTRAINT [PK_CollectionEventSeries] PRIMARY KEY CLUSTERED 
(
	[SeriesID] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionEventSeries] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionEventSeries] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionEventSeries] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionEventSeries] TO [CacheUser]
GO


--#####################################################################################################################
--######   [CollectionEventSeriesImage]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionEventSeriesImage](
	[SeriesID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ResourceURI] [varchar](255) NULL,
	[ImageType] [nvarchar](50) NULL,
	[Description] [xml] NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CollectionEventSeriesImage] PRIMARY KEY CLUSTERED 
(
	[SeriesID] ASC,
	[URI] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionEventSeriesImage] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionEventSeriesImage] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionEventSeriesImage] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionEventSeriesImage] TO [CacheUser]
GO


--#####################################################################################################################
--######   [CollectionSpecimen]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimen](
	[CollectionSpecimenID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[CollectionEventID] [int] NULL,
	[CollectionID] [int] NULL,
	[AccessionNumber] [nvarchar](50) NULL,
	[AccessionDate] [datetime] NULL,
	[AccessionDay] [tinyint] NULL,
	[AccessionMonth] [tinyint] NULL,
	[AccessionYear] [smallint] NULL,
	[AccessionDateSupplement] [nvarchar](255) NULL,
	[AccessionDateCategory] [nvarchar](50) NULL,
	[DepositorsName] [nvarchar](255) NULL,
	[DepositorsAgentURI] [varchar](255) NULL,
	[DepositorsAccessionNumber] [nvarchar](50) NULL,
	[LabelTitle] [nvarchar](255) NULL,
	[LabelType] [nvarchar](50) NULL,
	[LabelTranscriptionState] [nvarchar](50) NULL,
	[LabelTranscriptionNotes] [nvarchar](255) NULL,
	[ExsiccataURI] [varchar](255) NULL,
	[ExsiccataAbbreviation] [nvarchar](255) NULL,
	[OriginalNotes] [nvarchar](max) NULL,
	[AdditionalNotes] [nvarchar](max) NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[Problems] [nvarchar](255) NULL,
	[ExternalDatasourceID] [int] NULL,
	[ExternalIdentifier] [nvarchar](100) NULL,
	[LogInsertedWhen] [datetime] NULL,
 CONSTRAINT [PK_CollectionSpecimen] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CollectionSpecimen] ADD  CONSTRAINT [DF_CollectionSpecimen_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

GRANT INSERT ON [CollectionSpecimen] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionSpecimen] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionSpecimen] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionSpecimen] TO [CacheUser]
GO



--#####################################################################################################################
--######   [CollectionSpecimenImage]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenImage](
	[CollectionSpecimenID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ResourceURI] [varchar](255) NULL,
	[SpecimenPartID] [int] NULL,
	[IdentificationUnitID] [int] NULL,
	[ImageType] [nvarchar](50) NULL,
	[Description] [xml] NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CollectionSpecimenImage] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[URI] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionSpecimenImage] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionSpecimenImage] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionSpecimenImage] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionSpecimenImage] TO [CacheUser]
GO




--#####################################################################################################################
--######   [CollectionSpecimenPart]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionSpecimenPart](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenPartID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[DerivedFromSpecimenPartID] [int] NULL,
	[PreparationMethod] [nvarchar](max) NULL,
	[PreparationDate] [datetime] NULL,
	[AccessionNumber] [nvarchar](50) NULL,
	[PartSublabel] [nvarchar](50) NULL,
	[CollectionID] [int] NOT NULL,
	[MaterialCategory] [nvarchar](50) NOT NULL,
	[StorageLocation] [nvarchar](255) NULL,
	[StorageContainer] [nvarchar](500) NULL,
	[Stock] [float] NULL,
	[StockUnit] [nvarchar](50) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[PreparationMethodID] [int] NULL,
	[Status] [nvarchar](500) NULL,
 CONSTRAINT [PK_CollectionSpecimenPart] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenPartID] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionSpecimenPart] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionSpecimenPart] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionSpecimenPart] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionSpecimenPart] TO [CacheUser]
GO



--#####################################################################################################################
--######   [CollectionSpecimenRelation]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenRelation](
	[CollectionSpecimenID] [int] NOT NULL,
	[RelatedSpecimenURI] [varchar](255) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[RelatedSpecimenDisplayText] [varchar](255) NOT NULL,
	[RelationType] [nvarchar](50) NULL,
	[RelatedSpecimenCollectionID] [int] NULL,
	[RelatedSpecimenDescription] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[IsInternalRelationCache] [bit] NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenRelation] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[RelatedSpecimenURI] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [CollectionSpecimenRelation] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionSpecimenRelation] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionSpecimenRelation] TO [CacheAdministrator]
GO

GRANT SELECT ON [CollectionSpecimenRelation] TO [CacheUser]
GO


--#####################################################################################################################
--######   [Identification]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Identification](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[IdentificationSequence] [smallint] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[IdentificationDate] [datetime] NULL,
	[IdentificationDay] [tinyint] NULL,
	[IdentificationMonth] [tinyint] NULL,
	[IdentificationYear] [smallint] NULL,
	[IdentificationDateSupplement] [nvarchar](255) NULL,
	[IdentificationDateCategory] [nvarchar](50) NULL,
	[VernacularTerm] [nvarchar](255) NULL,
	[TaxonomicName] [nvarchar](255) NULL,
	[NameURI] [varchar](255) NULL,
	[IdentificationCategory] [nvarchar](50) NULL,
	[IdentificationQualifier] [nvarchar](50) NULL,
	[TypeStatus] [nvarchar](50) NULL,
	[TypeNotes] [nvarchar](max) NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
 CONSTRAINT [PK_Identification] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[IdentificationSequence] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [Identification] TO [CacheAdministrator]
GO

GRANT UPDATE ON [Identification] TO [CacheAdministrator]
GO

GRANT DELETE ON [Identification] TO [CacheAdministrator]
GO

GRANT SELECT ON [Identification] TO [CacheUser]
GO



--#####################################################################################################################
--######   [IdentificationUnit]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[IdentificationUnit](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[LastIdentificationCache] [nvarchar](255) NOT NULL,
	[FamilyCache] [nvarchar](255) NULL,
	[OrderCache] [nvarchar](255) NULL,
	[TaxonomicGroup] [nvarchar](50) NOT NULL,
	[OnlyObserved] [bit] NULL,
	[RelatedUnitID] [int] NULL,
	[RelationType] [nvarchar](50) NULL,
	[ColonisedSubstratePart] [nvarchar](255) NULL,
	[LifeStage] [nvarchar](255) NULL,
	[Gender] [nvarchar](50) NULL,
	[NumberOfUnits] [smallint] NULL,
	[ExsiccataNumber] [nvarchar](50) NULL,
	[ExsiccataIdentification] [smallint] NULL,
	[UnitIdentifier] [nvarchar](50) NULL,
	[UnitDescription] [nvarchar](50) NULL,
	[Circumstances] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[HierarchyCache] [nvarchar](500) NULL,
	[ParentUnitID] [int] NULL,
 CONSTRAINT [PK_IdentificationUnit] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [IdentificationUnit] TO [CacheAdministrator]
GO

GRANT UPDATE ON [IdentificationUnit] TO [CacheAdministrator]
GO

GRANT DELETE ON [IdentificationUnit] TO [CacheAdministrator]
GO

GRANT SELECT ON [IdentificationUnit] TO [CacheUser]
GO



--#####################################################################################################################
--######   [IdentificationUnitInPart]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[IdentificationUnitAnalysis](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[AnalysisID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[AnalysisNumber] [nvarchar](50) NOT NULL,
	[AnalysisResult] [nvarchar](max) NULL,
	[ExternalAnalysisURI] [varchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[AnalysisDate] [nvarchar](50) NULL,
	[SpecimenPartID] [int] NULL,
	[ToolID] [int] NULL,
	[Notes] [nvarchar](max) NULL,
	[ToolUsage] [xml] NULL,
 CONSTRAINT [PK_ProvisionalDescriptionData] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[AnalysisID] ASC,
	[AnalysisNumber] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [IdentificationUnitAnalysis] TO [CacheAdministrator]
GO

GRANT UPDATE ON [IdentificationUnitAnalysis] TO [CacheAdministrator]
GO

GRANT DELETE ON [IdentificationUnitAnalysis] TO [CacheAdministrator]
GO

GRANT SELECT ON [IdentificationUnitAnalysis] TO [CacheUser]
GO


--#####################################################################################################################
--######   [IdentificationUnitInPart]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[IdentificationUnitInPart](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[SpecimenPartID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[DisplayOrder] [smallint] NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_IdentificationUnitInPart] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[SpecimenPartID] ASC, 
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT ON [IdentificationUnitInPart] TO [CacheAdministrator]
GO

GRANT UPDATE ON [IdentificationUnitInPart] TO [CacheAdministrator]
GO

GRANT DELETE ON [IdentificationUnitInPart] TO [CacheAdministrator]
GO

GRANT SELECT ON [IdentificationUnitInPart] TO [CacheUser]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '03.00.02'
END

GO



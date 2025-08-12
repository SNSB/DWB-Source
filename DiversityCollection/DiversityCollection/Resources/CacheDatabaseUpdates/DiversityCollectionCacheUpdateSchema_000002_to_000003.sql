

--#####################################################################################################################
--#####################################################################################################################
--######   Creating the cache tables to store the data in the cache database   ###################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   CacheCollectionSpecimen  ######################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionSpecimen') = 0
begin
CREATE TABLE [#project#].[CacheCollectionSpecimen](
	[CollectionSpecimenID] [int] NOT NULL,
	[LabelTranscriptionNotes] [nvarchar](255) NULL,
	[OriginalNotes] [nvarchar](max) NULL,
	[LogUpdatedWhen] [datetime] NULL CONSTRAINT [DF_CacheCollectionSpecimen_LogUpdatedWhen]  DEFAULT (getdate()),
	[CollectionEventID] [int] NULL,
	[AccessionNumber] [nvarchar](50) NULL,
	[AccessionDate] [datetime] NULL,
	[AccessionDay] [tinyint] NULL,
	[AccessionMonth] [tinyint] NULL,
	[AccessionYear] [smallint] NULL,
	[DepositorsName] [nvarchar](255) NULL,
	[DepositorsAccessionNumber] [nvarchar](50) NULL,
	[ExsiccataURI] [varchar](255) NULL,
	[ExsiccataAbbreviation] [nvarchar](255) NULL,
	[AdditionalNotes] [nvarchar](max) NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[ReferenceURI] [varchar](255) NULL,
 CONSTRAINT [PK_CacheCollectionSpecimen] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GRANT SELECT ON [#project#].[CacheCollectionSpecimen] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheCollectionSpecimen] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheCollectionSpecimen] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheCollectionSpecimen] TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   CacheCollectionAgent  ######################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionAgent') = 0
begin
CREATE TABLE [#project#].[CacheCollectionAgent](
	[CollectionSpecimenID] [int] NOT NULL,
	[CollectorsName] [nvarchar](255) NOT NULL,
	[CollectorsSequence] [datetime2](7) NULL,
	[CollectorsNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_CacheCollectionAgent] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[CollectorsName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GRANT SELECT ON [#project#].CacheCollectionAgent TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheCollectionAgent TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheCollectionAgent TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheCollectionAgent TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   CacheCollectionEvent  ######################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionEvent') = 0
begin

CREATE TABLE [#project#].[CacheCollectionEvent](
	[CollectionEventID] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[CollectorsEventNumber] [nvarchar](50) NULL,
	[CollectionDate] [datetime] NULL,
	[CollectionDay] [tinyint] NULL,
	[CollectionMonth] [tinyint] NULL,
	[CollectionYear] [smallint] NULL,
	[CollectionDateSupplement] [nvarchar](100) NULL,
	[CollectionTime] [varchar](50) NULL,
	[CollectionTimeSpan] [varchar](50) NULL,
	[LocalityDescription] [nvarchar](max) NULL,
	[HabitatDescription] [nvarchar](max) NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[CollectingMethod] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CountryCache] [nvarchar](50) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[LocalityVerbatim] [nvarchar](max) NULL,
	[CollectionEndDay] [tinyint] NULL,
	[CollectionEndMonth] [tinyint] NULL,
	[CollectionEndYear] [smallint] NULL,
 CONSTRAINT [PK_CacheCollectionEvent] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheCollectionEvent TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheCollectionEvent TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheCollectionEvent TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheCollectionEvent TO [CacheAdmin_#project#]
GO




--#####################################################################################################################
--######   CacheCollectionEventLocalisation  ##########################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionEventLocalisation') = 0
begin

CREATE TABLE [#project#].[CacheCollectionEventLocalisation](
	[CollectionEventID] [int] NOT NULL,
	[LocalisationSystemID] [int] NOT NULL,
	[Location1] [nvarchar](255) NULL,
	[Location2] [nvarchar](255) NULL,
	[LocationAccuracy] [nvarchar](50) NULL,
	[LocationNotes] [nvarchar](max) NULL,
	[DeterminationDate] [smalldatetime] NULL,
	[DistanceToLocation] [varchar](50) NULL,
	[DirectionToLocation] [varchar](50) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[AverageAltitudeCache] [float] NULL,
	[AverageLatitudeCache] [float] NULL,
	[AverageLongitudeCache] [float] NULL,
	[RecordingMethod] [nvarchar](500) NULL,
 CONSTRAINT [PK_CacheCollectionEventLocalisation] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[LocalisationSystemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheCollectionEventLocalisation TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheCollectionEventLocalisation TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheCollectionEventLocalisation TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheCollectionEventLocalisation TO [CacheAdmin_#project#]
GO





--#####################################################################################################################
--######   CacheCollectionEventProperty  ##############################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionEventProperty') = 0
begin

CREATE TABLE [#project#].[CacheCollectionEventProperty](
	[CollectionEventID] [int] NOT NULL,
	[PropertyID] [int] NOT NULL,
	[DisplayText] [nvarchar](255) NULL,
	[PropertyURI] [varchar](255) NULL,
	[PropertyHierarchyCache] [nvarchar](max) NULL,
	[PropertyValue] [nvarchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[AverageValueCache] [float] NULL,
 CONSTRAINT [PK_CacheCollectionEventProperty] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[PropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheCollectionEventProperty TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheCollectionEventProperty TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheCollectionEventProperty TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheCollectionEventProperty TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   CacheCollectionSpecimenImage  ##############################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionSpecimenImage') = 0
begin

CREATE TABLE [#project#].[CacheCollectionSpecimenImage](
	[CollectionSpecimenID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[ResourceURI] [varchar](255) NULL,
	[SpecimenPartID] [int] NULL,
	[IdentificationUnitID] [int] NULL,
	[ImageType] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[LicenseURI] [varchar](500) NULL,
	[LicenseNotes] [nvarchar](500) NULL,
	[DisplayOrder] [int] NULL,
	[LicenseYear] [nvarchar](50) NULL,
	[LicenseHolderAgentURI] [nvarchar](500) NULL,
	[LicenseHolder] [nvarchar](500) NULL,
	[LicenseType] [nvarchar](500) NULL,
	[CopyrightStatement] [nvarchar](500) NULL,
	[CreatorAgentURI] [varchar](255) NULL,
	[CreatorAgent] [nvarchar](500) NULL,
	[IPR] [nvarchar](500) NULL,
	[Title] [nvarchar](500) NULL,
 CONSTRAINT [PK_CacheCollectionSpecimenImage] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[URI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheCollectionSpecimenImage TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheCollectionSpecimenImage TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheCollectionSpecimenImage TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheCollectionSpecimenImage TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   CacheCollectionSpecimenPart  ###############################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionSpecimenPart') = 0
begin

CREATE TABLE [#project#].[CacheCollectionSpecimenPart](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenPartID] [int] NOT NULL,
	[DerivedFromSpecimenPartID] [int] NULL,
	[PreparationMethod] [nvarchar](max) NULL,
	[PreparationDate] [datetime] NULL,
	[PartSublabel] [nvarchar](50) NULL,
	[CollectionID] [int] NOT NULL,
	[MaterialCategory] [nvarchar](50) NOT NULL,
	[StorageLocation] [nvarchar](255) NULL,
	[Stock] [float] NULL,
	[Notes] [nvarchar](max) NULL,
	[AccessionNumber] [nvarchar](50) NULL,
	[StorageContainer] [nvarchar](500) NULL,
	[StockUnit] [nvarchar](50) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
 CONSTRAINT [PK_CacheCollectionSpecimenPart] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenPartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheCollectionSpecimenPart TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheCollectionSpecimenPart TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheCollectionSpecimenPart TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheCollectionSpecimenPart TO [CacheAdmin_#project#]
GO






--#####################################################################################################################
--######   CacheCollectionSpecimenRelation  ###########################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionSpecimenRelation') = 0
begin
CREATE TABLE [#project#].[CacheCollectionSpecimenRelation](
	[CollectionSpecimenID] [int] NOT NULL,
	[RelatedSpecimenURI] [varchar](255) NOT NULL,
	[RelatedSpecimenDisplayText] [varchar](255) NOT NULL,
	[RelationType] [nvarchar](50) NULL,
	[RelatedSpecimenCollectionID] [int] NULL,
	[RelatedSpecimenDescription] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[IdentificationUnitID] [int] NULL,
	[SpecimenPartID] [int] NULL,
 CONSTRAINT [PK_CacheCollectionSpecimenRelation] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[RelatedSpecimenURI] ASC,
	[RelatedSpecimenDisplayText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GRANT SELECT ON [#project#].CacheCollectionSpecimenRelation TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheCollectionSpecimenRelation TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheCollectionSpecimenRelation TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheCollectionSpecimenRelation TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   CacheIdentificationUnit  ###################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheIdentificationUnit') = 0
begin

CREATE TABLE [#project#].[CacheIdentificationUnit](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[LastIdentificationCache] [nvarchar](255) NOT NULL,
	[TaxonomicGroup] [nvarchar](50) NOT NULL,
	[RelatedUnitID] [int] NULL,
	[RelationType] [nvarchar](50) NULL,
	[ExsiccataNumber] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NOT NULL,
	[ColonisedSubstratePart] [nvarchar](255) NULL,
	[FamilyCache] [nvarchar](255) NULL,
	[OrderCache] [nvarchar](255) NULL,
	[LifeStage] [nvarchar](255) NULL,
	[Gender] [nvarchar](50) NULL,
	[HierarchyCache] [nvarchar](500) NULL,
	[UnitIdentifier] [nvarchar](50) NULL,
	[UnitDescription] [nvarchar](50) NULL,
	[Circumstances] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[NumberOfUnits] [smallint] NULL,
	[OnlyObserved] [bit] NULL,
 CONSTRAINT [PK_CacheIdentificationUnit] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheIdentificationUnit TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheIdentificationUnit TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheIdentificationUnit TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheIdentificationUnit TO [CacheAdmin_#project#]
GO




--#####################################################################################################################
--######   CacheIdentification  #######################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheIdentification') = 0
begin

CREATE TABLE [#project#].[CacheIdentification](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[IdentificationSequence] [smallint] NOT NULL DEFAULT ((1)),
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
	[ReferenceDetails] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
 CONSTRAINT [PK_CacheIdentification] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[IdentificationSequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


END

GRANT SELECT ON [#project#].CacheIdentification TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheIdentification TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheIdentification TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheIdentification TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   CacheIdentificationUnitAnalysis  ###########################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheIdentificationUnitAnalysis') = 0
begin

CREATE TABLE [#project#].[CacheIdentificationUnitAnalysis](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[AnalysisID] [int] NOT NULL,
	[AnalysisNumber] [nvarchar](50) NOT NULL,
	[AnalysisResult] [nvarchar](max) NULL,
	[ExternalAnalysisURI] [varchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[AnalysisDate] [nvarchar](50) NULL,
	[SpecimenPartID] [int] NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheIdentificationUnitAnalysis] PRIMARY KEY CLUSTERED 
(
	[AnalysisID] ASC,
	[AnalysisNumber] ASC,
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheIdentificationUnitAnalysis TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheIdentificationUnitAnalysis TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheIdentificationUnitAnalysis TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheIdentificationUnitAnalysis TO [CacheAdmin_#project#]
GO




--#####################################################################################################################
--######   CacheIdentificationUnitInPart  #############################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheIdentificationUnitInPart') = 0
begin

CREATE TABLE [#project#].[CacheIdentificationUnitInPart](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[SpecimenPartID] [int] NOT NULL,
	[DisplayOrder] [smallint] NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_CacheIdentificationUnitInPart] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[SpecimenPartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheIdentificationUnitInPart TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheIdentificationUnitInPart TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheIdentificationUnitInPart TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheIdentificationUnitInPart TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   CacheMetadata   ############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheMetadata') = 0
begin

CREATE TABLE [#project#].[CacheMetadata](
	[ProjectID] [int] NOT NULL,
	[ProjectTitleCode] [nvarchar](254) NULL,
	[TaxonomicGroup] [nvarchar](254) NULL,
	[DatasetGUID] [nvarchar](254) NULL,
	[TechnicalContactName] [nvarchar](254) NULL,
	[TechnicalContactEmail] [nvarchar](254) NULL,
	[TechnicalContactPhone] [nvarchar](254) NULL,
	[TechnicalContactAddress] [nvarchar](254) NULL,
	[ContentContactName] [nvarchar](254) NULL,
	[ContentContactEmail] [nvarchar](254) NULL,
	[ContentContactPhone] [nvarchar](254) NULL,
	[ContentContactAddress] [nvarchar](254) NULL,
	[OtherProviderUDDI] [nvarchar](254) NULL,
	[DatasetTitle] [nvarchar](254) NULL,
	[DatasetDetails] [nvarchar](254) NULL,
	[DatasetCoverage] [nvarchar](254) NULL,
	[DatasetURI] [nvarchar](254) NULL,
	[DatasetIconURI] [nvarchar](254) NULL,
	[DatasetVersionMajor] [nvarchar](254) NULL,
	[DatasetCreators] [nvarchar](254) NULL,
	[DatasetContributors] [nvarchar](254) NULL,
	[DateCreated] [nvarchar](254) NULL,
	[DateModified] [nvarchar](254) NULL,
	[SourceID] [nvarchar](254) NULL,
	[SourceInstitutionID] [nvarchar](254) NULL,
	[OwnerOrganizationName] [nvarchar](254) NULL,
	[OwnerOrganizationAbbrev] [nvarchar](254) NULL,
	[OwnerContactPerson] [nvarchar](254) NULL,
	[OwnerContactRole] [nvarchar](254) NULL,
	[OwnerAddress] [nvarchar](254) NULL,
	[OwnerTelephone] [nvarchar](254) NULL,
	[OwnerEmail] [nvarchar](254) NULL,
	[OwnerURI] [nvarchar](254) NULL,
	[OwnerLogoURI] [nvarchar](254) NULL,
	[IPRText] [nvarchar](254) NULL,
	[IPRDetails] [nvarchar](254) NULL,
	[IPRURI] [nvarchar](254) NULL,
	[CopyrightText] [nvarchar](254) NULL,
	[CopyrightDetails] [nvarchar](254) NULL,
	[CopyrightURI] [nvarchar](254) NULL,
	[TermsOfUseText] [nvarchar](254) NULL,
	[TermsOfUseDetails] [nvarchar](254) NULL,
	[TermsOfUseURI] [nvarchar](254) NULL,
	[DisclaimersText] [nvarchar](254) NULL,
	[DisclaimersDetails] [nvarchar](254) NULL,
	[DisclaimersURI] [nvarchar](254) NULL,
	[LicenseText] [nvarchar](254) NULL,
	[LicensesDetails] [nvarchar](254) NULL,
	[LicenseURI] [nvarchar](254) NULL,
	[AcknowledgementsText] [nvarchar](254) NULL,
	[AcknowledgementsDetails] [nvarchar](254) NULL,
	[AcknowledgementsURI] [nvarchar](254) NULL,
	[CitationsText] [nvarchar](254) NULL,
	[CitationsDetails] [nvarchar](254) NULL,
	[CitationsURI] [nvarchar](254) NULL,
	[RecordBasis] [nvarchar](254) NULL,
	[KindOfUnit] [nvarchar](254) NULL,
	[HigherTaxonRank] [nvarchar](254) NULL,
	[RecordURI] [nvarchar](254) NULL,
 CONSTRAINT [PK_ABCD_Metadata] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

GO

GRANT SELECT ON [#project#].[CacheMetadata] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheMetadata] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheMetadata] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheMetadata] TO [CacheAdmin_#project#]
GO





















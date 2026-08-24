
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.32'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   TaxonAnalysis - Add column AnalysisNumber ##################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'TaxonAnalysis' AND T.COLUMN_NAME = 'AnalysisNumber') = 0
begin

	ALTER TABLE [dbo].[TaxonAnalysis] DROP CONSTRAINT [PK_TaxonAnalysis] WITH ( ONLINE = OFF )

	ALTER TABLE [dbo].[TaxonAnalysis] ADD AnalysisNumber int NOT NULL CONSTRAINT DF_TaxonAnalysis_AnalysisNumber DEFAULT (1);

	ALTER TABLE [dbo].[TaxonAnalysis] ADD  CONSTRAINT [PK_TaxonAnalysis] PRIMARY KEY CLUSTERED 
	(
		[NameID] ASC,
		[ProjectID] ASC,
		[AnalysisID] ASC,
		[AnalysisNumber] ASC
	) ON [PRIMARY]
end
GO


--#####################################################################################################################
--######   TaxonRelation   ############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'TaxonRelation') = 0
begin

CREATE TABLE [dbo].[TaxonRelation](
	[NameID] [int] NOT NULL,
	[BaseURL] [varchar](500) NOT NULL,
	[RelationType] [nvarchar](50) NOT NULL,
	[RelationNameURI] [varchar](400) NOT NULL,
	[TaxonNameCache] [nvarchar](255) NULL,
	[Stage] [nvarchar](500) NULL,
	[RelatedStage] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[SourceView] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TaxonRelation] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[NameID] ASC,
	[RelationType] ASC,
	[RelationNameURI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds cached data of relations between taxa from DiversityTaxonNames as base for other procedures.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonRelation'

END


GRANT SELECT ON [TaxonRelation] TO [CacheUser]
GO
GRANT DELETE ON [TaxonRelation] TO [CacheAdmin] 
GO
GRANT UPDATE ON [TaxonRelation] TO [CacheAdmin]
GO
GRANT INSERT ON [TaxonRelation] TO [CacheAdmin]
GO







declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.06'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   TaxonSynonymy     ##########################################################################################
--#####################################################################################################################

DROP TABLE [dbo].[TaxonSynonymy]
GO

CREATE TABLE [dbo].[TaxonSynonymy](
	[NameID] [int] NOT NULL,
	[BaseURL] [varchar](255) NOT NULL,
	[TaxonName] [nvarchar](255) NULL,
	[AcceptedNameID] [int] NULL,
	[AcceptedName] [nvarchar](255) NULL,
	[TaxonomicRank] [nvarchar](50) NULL,
	[SpeciesGenusNameID] [int] NULL,
	[GenusOrSupragenericName] [nvarchar](200) NULL,
	[NameParentID] [int] NULL,
	[TaxonNameSinAuthor] [nvarchar](2000) NULL,
	[LogInsertedWhen] [smalldatetime] NULL CONSTRAINT [DF_TaxonSynonymy_LogInsertedWhen]  DEFAULT (getdate()),
	[ProjectID] [int] NULL,
	[SourceView] [nvarchar](128) NULL,
 CONSTRAINT [PK_TaxonSynonymy] PRIMARY KEY CLUSTERED 
(
	[NameID] ASC,
	[BaseURL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymy', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The source of the data, e.g. the name of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymy', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds cached data from DiversityTaxonNames as base for other procedures.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymy'
GO


GRANT SELECT ON [TaxonSynonymy] TO [CacheUser]
GO
GRANT DELETE ON [TaxonSynonymy] TO [CacheAdmin] 
GO
GRANT UPDATE ON [TaxonSynonymy] TO [CacheAdmin]
GO
GRANT INSERT ON [TaxonSynonymy] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   Remove foreign key on ScientificTermSource if existing   #####################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE k where k.CONSTRAINT_NAME = 'FK_TaxonSynonymySourceTarget_TaxonSynonymySource') = 1
begin
	ALTER TABLE [dbo].[TaxonSynonymySourceTarget] DROP CONSTRAINT [FK_TaxonSynonymySourceTarget_TaxonSynonymySource]
end
GO

--#####################################################################################################################
--######   ScientificTermSource   #####################################################################################
--#####################################################################################################################

DROP TABLE [dbo].[TaxonSynonymySource]
GO

CREATE TABLE [dbo].[TaxonSynonymySource](
	[SourceView] [nvarchar](200) NOT NULL,
	[Source] [nvarchar](500) NULL,
	[SourceID] [int] NULL,
	[LinkedServerName] [nvarchar](500) NULL,
	[DatabaseName] [nvarchar](50) NULL,
 CONSTRAINT [PK_TaxonSynonymySource] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GRANT SELECT ON [TaxonSynonymySource] TO [CacheUser]
GO
GRANT DELETE ON [TaxonSynonymySource] TO [CacheAdmin] 
GO
GRANT UPDATE ON [TaxonSynonymySource] TO [CacheAdmin]
GO
GRANT INSERT ON [TaxonSynonymySource] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   Restore foreign key ########################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'TaxonSynonymySourceTarget' and c.COLUMN_NAME = 'SourceView') = 1
and (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'TaxonSynonymySource' and c.COLUMN_NAME = 'SourceView') = 1
and (select count(*) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE k where k.CONSTRAINT_NAME = 'FK_TaxonSynonymySourceTarget_TaxonSynonymySource') = 0
begin
	
	ALTER TABLE [dbo].[TaxonSynonymySourceTarget]  WITH CHECK ADD  CONSTRAINT [FK_TaxonSynonymySourceTarget_TaxonSynonymySource] FOREIGN KEY([SourceView])
	REFERENCES [dbo].[TaxonSynonymySource] ([SourceView])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[TaxonSynonymySourceTarget] CHECK CONSTRAINT [FK_TaxonSynonymySourceTarget_TaxonSynonymySource]

end
GO


--#####################################################################################################################
--######   TaxonCommonName ############################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaxonCommonName](
	[NameID] [int] NOT NULL,
	[CommonName] [nvarchar](300) NOT NULL,
	[LanguageCode] [varchar](2) NOT NULL,
	[CountryCode] [varchar](2) NOT NULL,
	[SourceView] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TaxonCommonName] PRIMARY KEY CLUSTERED 
(
	[NameID] ASC,
	[CommonName] ASC,
	[LanguageCode] ASC,
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GRANT SELECT ON [TaxonCommonName] TO [CacheUser]
GO
GRANT DELETE ON [TaxonCommonName] TO [CacheAdmin] 
GO
GRANT UPDATE ON [TaxonCommonName] TO [CacheAdmin]
GO
GRANT INSERT ON [TaxonCommonName] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   TaxonList       ############################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaxonList](
	[ProjectID] [int] NOT NULL,
	[Project] [nvarchar](50) NOT NULL,
	[DisplayText] [nvarchar](50) NULL,
	[SourceView] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TaxonList] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



GRANT SELECT ON [TaxonList] TO [CacheUser]
GO
GRANT DELETE ON [TaxonList] TO [CacheAdmin] 
GO
GRANT UPDATE ON [TaxonList] TO [CacheAdmin]
GO
GRANT INSERT ON [TaxonList] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   TaxonAnalysis   ############################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[TaxonAnalysis](
	[NameID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[AnalysisID] [int] NOT NULL,
	[AnalysisValue] [nvarchar](255) NULL,
	[SourceView] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TaxonAnalysis] PRIMARY KEY CLUSTERED 
(
	[NameID] ASC,
	[ProjectID] ASC,
	[AnalysisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GRANT SELECT ON TaxonAnalysis TO [CacheUser]
GO
GRANT DELETE ON TaxonAnalysis TO [CacheAdmin] 
GO
GRANT UPDATE ON TaxonAnalysis TO [CacheAdmin]
GO
GRANT INSERT ON TaxonAnalysis TO [CacheAdmin]
GO



--#####################################################################################################################
--######   TaxonAnalysisCategory   ####################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaxonAnalysisCategory](
	[AnalysisID] [int] NOT NULL,
	[AnalysisParentID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[SourceView] [nvarchar](128) NULL,
 CONSTRAINT [PK_TaxonAnalysisCategory] PRIMARY KEY CLUSTERED 
(
	[AnalysisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


GRANT SELECT ON TaxonAnalysisCategory TO [CacheUser]
GO
GRANT DELETE ON TaxonAnalysisCategory TO [CacheAdmin] 
GO
GRANT UPDATE ON TaxonAnalysisCategory TO [CacheAdmin]
GO
GRANT INSERT ON TaxonAnalysisCategory TO [CacheAdmin]
GO



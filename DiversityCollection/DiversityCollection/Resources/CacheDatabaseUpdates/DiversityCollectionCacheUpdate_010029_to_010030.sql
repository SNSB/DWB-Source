
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.29'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   procTaxonNameHierarchy for setting of NameParentID   #######################################################
--#####################################################################################################################

CREATE PROCEDURE [dbo].[procTaxonNameHierarchy] 
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

GRANT EXEC ON [dbo].[procTaxonNameHierarchy] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   Redesign of the source tables   ############################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   Redesign of Agent source tables   ##########################################################################
--#####################################################################################################################
--######   Redesign of AgentContactInformation   ######################################################################
--#####################################################################################################################

ALTER TABLE AgentContactInformation ADD [BaseURL] VARCHAR(500);
GO
UPDATE C SET [BaseURL] = A.[BaseURL]
FROM Agent A INNER JOIN AgentContactInformation C ON A.AgentID = C.AgentID;
GO
DELETE I
FROM AgentContactInformation I WHERE I.BaseURL is null;
GO
ALTER TABLE AgentContactInformation ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[AgentContactInformation] DROP CONSTRAINT [PK_AgentAddress] 
GO

ALTER TABLE [dbo].[AgentContactInformation] ADD  CONSTRAINT [PK_AgentContactInformation] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[BaseURL] ASC,
	[DisplayOrder] ASC
)
GO


--#####################################################################################################################
--######   Redesign of AgentImage   ###################################################################################
--#####################################################################################################################

ALTER TABLE AgentImage ADD [BaseURL] VARCHAR(500);
GO
UPDATE I SET [BaseURL] = A.[BaseURL]
FROM Agent A INNER JOIN AgentImage I ON A.AgentID = I.AgentID;
GO
DELETE I
FROM AgentImage I WHERE I.BaseURL is null;
GO
ALTER TABLE AgentImage ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[AgentImage] DROP CONSTRAINT [PK_AgentImage] WITH ( ONLINE = OFF )
GO

ALTER TABLE [dbo].[AgentImage] ADD  CONSTRAINT [PK_AgentImage] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[BaseURL] ASC,
	[URI] ASC
)
GO

--#####################################################################################################################
--######   New table AgentSourceView   ################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentSourceView](
	[BaseURL] [varchar](500) NOT NULL,
	[AgentID] int NOT NULL,
	[SourceView] [nvarchar](128) NOT NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_AgentSourceView] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[AgentID] ASC,
	[SourceView] ASC
)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentSourceView] ADD  CONSTRAINT [DF_AgentSourceView_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceView', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSourceView', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

GRANT SELECT ON [AgentSourceView] TO [CacheUser];
GRANT INSERT ON [AgentSourceView] TO [CacheAdmin];
GRANT UPDATE ON [AgentSourceView] TO [CacheAdmin];
GRANT DELETE ON [AgentSourceView] TO [CacheAdmin];
GO


INSERT INTO [dbo].[AgentSourceView]
           ([BaseURL]
           ,[AgentID]
           ,[SourceView]
           ,[LogInsertedWhen])
SELECT [BaseURL]
      ,[AgentID]
      ,[SourceView]
      ,[LogInsertedWhen]
  FROM [dbo].[Agent]

GO

--#####################################################################################################################
--######   Redesign of Gazetteer source tables   ######################################################################
--#####################################################################################################################
--######   Redesign of GazetteerExternalDatabase   ####################################################################
--#####################################################################################################################

ALTER TABLE GazetteerExternalDatabase ADD [BaseURL] VARCHAR(500);
GO
UPDATE E SET [BaseURL] = G.[BaseURL]
FROM [Gazetteer] G INNER JOIN GazetteerExternalDatabase E ON G.ExternalDatabaseID = E.ExternalDatabaseID;
GO
DELETE I
FROM GazetteerExternalDatabase I WHERE I.BaseURL is null;
GO
ALTER TABLE GazetteerExternalDatabase ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[GazetteerExternalDatabase] DROP CONSTRAINT [PK_ExternalDatabase] WITH ( ONLINE = OFF )
GO

ALTER TABLE [dbo].[GazetteerExternalDatabase] ADD  CONSTRAINT [PK_ExternalDatabase] PRIMARY KEY CLUSTERED 
(
	[ExternalDatabaseID] ASC,
	[BaseURL] ASC
)
GO

--#####################################################################################################################
--######   New table GazetteerSourceView   ############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[GazetteerSourceView](
	[BaseURL] [varchar](500) NOT NULL,
	[NameID] int NOT NULL,
	[SourceView] [nvarchar](128) NOT NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_GazetteerSourceView] PRIMARY KEY CLUSTERED 
(
	[NameID] ASC,
	[BaseURL] ASC,
	[SourceView] ASC
)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GazetteerSourceView] ADD  CONSTRAINT [DF_GazetteerSourceView_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceView', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSourceView', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

GRANT SELECT ON [GazetteerSourceView] TO [CacheUser];
GRANT INSERT ON [GazetteerSourceView] TO [CacheAdmin];
GRANT UPDATE ON [GazetteerSourceView] TO [CacheAdmin];
GRANT DELETE ON [GazetteerSourceView] TO [CacheAdmin];
GO

INSERT INTO [dbo].[GazetteerSourceView]
           ([BaseURL]
           ,[NameID]
           ,[SourceView]
           ,[LogInsertedWhen])
SELECT [BaseURL]
      ,[NameID]
      ,[SourceView]
      ,[LogInsertedWhen]
  FROM [dbo].[Gazetteer]

GO




--#####################################################################################################################
--######   Redesign of Reference source tables   ######################################################################
--#####################################################################################################################
--######   Redesign of ReferenceRelator    ############################################################################
--#####################################################################################################################

ALTER TABLE ReferenceRelator ADD [BaseURL] VARCHAR(500);
GO
UPDATE I SET [BaseURL] = A.[BaseURL]
FROM [ReferenceTitle] A INNER JOIN ReferenceRelator I ON A.RefID = I.RefID;
GO
DELETE I
FROM ReferenceRelator I WHERE I.BaseURL is null;
GO
ALTER TABLE ReferenceRelator ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[ReferenceRelator] DROP CONSTRAINT [ReferenceRelator_PK]
GO

ALTER TABLE [dbo].[ReferenceRelator] ADD  CONSTRAINT [ReferenceRelator_PK] PRIMARY KEY NONCLUSTERED 
(
	[BaseURL] ASC,
	[RefID] ASC,
	[Role] ASC,
	[Sequence] ASC
)
GO

--#####################################################################################################################
--######   New table ReferenceTitleSourceView   #######################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ReferenceTitleSourceView](
	[BaseURL] [varchar](500) NOT NULL,
	[RefID] int NOT NULL,
	[SourceView] [nvarchar](128) NOT NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_ReferenceTitleSourceView] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[RefID] ASC,
	[SourceView] ASC
)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReferenceTitleSourceView] ADD  CONSTRAINT [DF_ReferenceTitleSourceView_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceTitleSourceView', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceTitleSourceView', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

GRANT SELECT ON [ReferenceTitleSourceView] TO [CacheUser];
GRANT INSERT ON [ReferenceTitleSourceView] TO [CacheAdmin];
GRANT UPDATE ON [ReferenceTitleSourceView] TO [CacheAdmin];
GRANT DELETE ON [ReferenceTitleSourceView] TO [CacheAdmin];
GO

INSERT INTO [dbo].[ReferenceTitleSourceView]
           ([BaseURL]
           ,[RefID]
           ,[SourceView]
           ,[LogInsertedWhen])
SELECT [BaseURL]
      ,[RefID]
      ,[SourceView]
      ,[LogInsertedWhen]
  FROM [dbo].[ReferenceTitle]

GO

--#####################################################################################################################
--######   Redesign of SamplingPlot source tables   ###################################################################
--#####################################################################################################################
--######   Redesign of SamplingPlotLocalisation   #####################################################################
--#####################################################################################################################

ALTER TABLE SamplingPlotLocalisation ADD [BaseURL] VARCHAR(500);
GO
UPDATE I SET [BaseURL] = A.[BaseURL]
FROM SamplingPlot A INNER JOIN SamplingPlotLocalisation I ON A.PlotID = I.PlotID;
GO
DELETE I
FROM SamplingPlotLocalisation I WHERE I.BaseURL is null;
GO
ALTER TABLE SamplingPlotLocalisation ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] DROP CONSTRAINT [PK_SamplingPlotLocalisation]
GO

ALTER TABLE [dbo].[SamplingPlotLocalisation] ADD  CONSTRAINT [PK_SamplingPlotLocalisation] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[PlotID] ASC,
	[LocalisationSystemID] ASC
)
GO


--#####################################################################################################################
--######   Redesign of SamplingPlotProperty   #########################################################################
--#####################################################################################################################

ALTER TABLE SamplingPlotProperty ADD [BaseURL] VARCHAR(500);
GO
UPDATE I SET [BaseURL] = A.[BaseURL]
FROM SamplingPlot A INNER JOIN SamplingPlotProperty I ON A.PlotID = I.PlotID;
GO
DELETE I
FROM SamplingPlotProperty I WHERE I.BaseURL is null;
GO
ALTER TABLE SamplingPlotProperty ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[SamplingPlotProperty] DROP CONSTRAINT [PK_SamplingPlotProperty] 
GO

ALTER TABLE [dbo].[SamplingPlotProperty] ADD  CONSTRAINT [PK_SamplingPlotProperty] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[PlotID] ASC,
	[PropertyID] ASC
)
GO

--#####################################################################################################################
--######   New table SamplingPlotSourceView   #########################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[SamplingPlotSourceView](
	[BaseURL] [varchar](500) NOT NULL,
	[PlotID] int NOT NULL,
	[SourceView] [nvarchar](128) NOT NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_SamplingPlotSourceView] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[PlotID] ASC,
	[SourceView] ASC
)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SamplingPlotSourceView] ADD  CONSTRAINT [DF_SamplingPlotSourceView_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceView', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotSourceView', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

GRANT SELECT ON [SamplingPlotSourceView] TO [CacheUser];
GRANT INSERT ON [SamplingPlotSourceView] TO [CacheAdmin];
GRANT UPDATE ON [SamplingPlotSourceView] TO [CacheAdmin];
GRANT DELETE ON [SamplingPlotSourceView] TO [CacheAdmin];
GO

INSERT INTO [dbo].[SamplingPlotSourceView]
           ([BaseURL]
           ,[PlotID]
           ,[SourceView]
           ,[LogInsertedWhen])
SELECT [BaseURL]
      ,[PlotID]
      ,[SourceView]
      ,[LogInsertedWhen]
  FROM [dbo].[SamplingPlot]

GO


--#####################################################################################################################
--######   Redesign of ScientificTerm source tables   #################################################################
--#####################################################################################################################
--######   Redesign of ScientificTerm    ##############################################################################
--#####################################################################################################################

ALTER TABLE ScientificTerm ADD RepresentationID int;
ALTER TABLE ScientificTerm ADD [BaseURL] VARCHAR(500);
GO
UPDATE S SET 
S.[BaseURL] = reverse(substring(reverse([RepresentationURI]), charindex('/', reverse([RepresentationURI])), 500)),
S.RepresentationID = cast(reverse(substring(reverse([RepresentationURI]), 1, charindex('/', reverse([RepresentationURI]))-1)) as int)
FROM ScientificTerm S
GO
DELETE I
FROM ScientificTerm I WHERE I.BaseURL is null OR I.RepresentationID is null;
GO
ALTER TABLE ScientificTerm ALTER COLUMN RepresentationID int NOT NULL;
ALTER TABLE ScientificTerm ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[ScientificTerm] DROP CONSTRAINT [ScientificTerm_PK]
GO

ALTER TABLE [dbo].[ScientificTerm] ADD  CONSTRAINT [ScientificTerm_PK] PRIMARY KEY NONCLUSTERED 
(
	RepresentationID ASC,
	[BaseURL] ASC
)
GO


--#####################################################################################################################
--######   New table ScientificTermSourceView   #######################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ScientificTermSourceView](
	[BaseURL] [varchar](500) NOT NULL,
	RepresentationID int NOT NULL,
	[SourceView] [nvarchar](128) NOT NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_ScientificTermSourceView] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	RepresentationID ASC,
	[SourceView] ASC
)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScientificTermSourceView] ADD  CONSTRAINT [DF_ScientificTermSourceView_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceView', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSourceView', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

GRANT SELECT ON [ScientificTermSourceView] TO [CacheUser];
GRANT INSERT ON [ScientificTermSourceView] TO [CacheAdmin];
GRANT UPDATE ON [ScientificTermSourceView] TO [CacheAdmin];
GRANT DELETE ON [ScientificTermSourceView] TO [CacheAdmin];
GO

INSERT INTO [dbo].[ScientificTermSourceView]
           ([BaseURL]
           ,[RepresentationID]
           ,[SourceView]
           ,[LogInsertedWhen])
SELECT [BaseURL]
      ,[RepresentationID]
      ,[SourceView]
      ,[LogInsertedWhen]
  FROM [dbo].[ScientificTerm]

GO

--#####################################################################################################################
--######   Redesign of Taxon source tables   ##########################################################################
--#####################################################################################################################
--#####################################################################################################################
--######   trgInsTaxonSynonymy - no inclusion of NameID for webservices   #############################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgInsTaxonSynonymy] ON [dbo].[TaxonSynonymy] 
FOR INSERT AS
/* setting the NameURI */ 
UPDATE t 
SET NameURI = i.[BaseURL] + case when i.[NameID] = -1 then '' else cast(i.[NameID] as varchar) end
FROM TaxonSynonymy T, inserted i
where i.NameID = T.NameID and T.BaseURL = i.BaseURL;
GO

--#####################################################################################################################
--######   Redesign of TaxonAnalysis   ################################################################################
--#####################################################################################################################

ALTER TABLE TaxonAnalysis ADD [BaseURL] VARCHAR(500);
GO
UPDATE C SET [BaseURL] = A.[BaseURL] 
FROM TaxonSynonymy A INNER JOIN TaxonAnalysis C ON A.NameID = C.NameID;
GO
DELETE I
FROM TaxonAnalysis I WHERE I.BaseURL is null;
GO
ALTER TABLE TaxonAnalysis ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[TaxonAnalysis] DROP CONSTRAINT [PK_TaxonAnalysis] 
GO

ALTER TABLE [dbo].[TaxonAnalysis] ADD  CONSTRAINT [PK_TaxonAnalysis] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	NameID ASC,
	[ProjectID] ASC,
	[AnalysisID] ASC
)
GO


--#####################################################################################################################
--######   Redesign of TaxonAnalysisCategory   ########################################################################
--#####################################################################################################################

ALTER TABLE TaxonAnalysisCategory ADD [BaseURL] VARCHAR(500);
GO
UPDATE C SET [BaseURL] = T.[BaseURL] 
FROM [TaxonAnalysis] A INNER JOIN TaxonAnalysisCategory C ON A.[AnalysisID] = C.[AnalysisID]
INNER JOIN [TaxonSynonymy] T ON T.NameID = A.NameID;
GO
DELETE I
FROM TaxonAnalysisCategory I WHERE I.BaseURL is null;
GO
ALTER TABLE TaxonAnalysisCategory ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].TaxonAnalysisCategory DROP CONSTRAINT [PK_TaxonAnalysisCategory] 
GO

ALTER TABLE [dbo].TaxonAnalysisCategory ADD CONSTRAINT [PK_TaxonAnalysisCategory] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[AnalysisID] ASC
)
GO

--#####################################################################################################################
--######   Redesign of TaxonAnalysisCategoryValue   ###################################################################
--#####################################################################################################################

ALTER TABLE TaxonAnalysisCategoryValue ADD [BaseURL] VARCHAR(500);
GO
UPDATE A SET [BaseURL] = C.[BaseURL] 
FROM TaxonAnalysisCategoryValue A INNER JOIN TaxonAnalysisCategory C ON A.[AnalysisID] = C.[AnalysisID]
GO
DELETE I
FROM TaxonAnalysisCategoryValue I WHERE I.BaseURL is null;
GO
ALTER TABLE TaxonAnalysisCategoryValue ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].TaxonAnalysisCategoryValue DROP CONSTRAINT [PK_TaxonAnalysisCategoryValue] 
GO

ALTER TABLE [dbo].TaxonAnalysisCategoryValue ADD CONSTRAINT [PK_TaxonAnalysisCategoryValue] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[AnalysisID] ASC,
	[AnalysisValue] ASC
)
GO

--#####################################################################################################################
--######   Redesign of TaxonCommonName   ##############################################################################
--#####################################################################################################################

ALTER TABLE TaxonCommonName ADD [BaseURL] VARCHAR(500);
GO
UPDATE C SET [BaseURL] = A.[BaseURL] 
FROM TaxonSynonymy A INNER JOIN TaxonCommonName C ON A.NameID = C.NameID;
GO
DELETE I
FROM TaxonCommonName I WHERE I.BaseURL is null;
GO
ALTER TABLE TaxonCommonName ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[TaxonCommonName] DROP CONSTRAINT [PK_TaxonCommonName] 
GO

ALTER TABLE [dbo].[TaxonCommonName] ADD  CONSTRAINT [PK_TaxonCommonName] PRIMARY KEY CLUSTERED 
(
	[NameID] ASC,
	[BaseURL] ASC,
	[CommonName] ASC,
	[LanguageCode] ASC,
	[CountryCode] ASC
)
GO

--#####################################################################################################################
--######   Redesign of TaxonList   ####################################################################################
--#####################################################################################################################

ALTER TABLE TaxonList ADD [BaseURL] VARCHAR(500);
GO
UPDATE C SET [BaseURL] = T.[BaseURL] 
FROM [TaxonAnalysis] A INNER JOIN TaxonList C ON A.ProjectID = C.ProjectID
INNER JOIN [TaxonSynonymy] T ON T.NameID = A.NameID;
GO
DELETE I
FROM TaxonList I WHERE I.BaseURL is null;
GO
ALTER TABLE TaxonList ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[TaxonList] DROP CONSTRAINT [PK_TaxonList]
GO

ALTER TABLE [dbo].[TaxonList] ADD  CONSTRAINT [PK_TaxonList] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[ProjectID] ASC
)
GO


--#####################################################################################################################
--######   Redesign of TaxonNameExternalDatabase   ####################################################################
--#####################################################################################################################

ALTER TABLE TaxonNameExternalDatabase ADD [BaseURL] VARCHAR(500);
GO
UPDATE C SET [BaseURL] = T.[BaseURL] 
FROM TaxonNameExternalID A INNER JOIN TaxonNameExternalDatabase C ON A.ExternalDatabaseID = C.ExternalDatabaseID
INNER JOIN [TaxonSynonymy] T ON T.NameID = A.NameID;
GO
DELETE I
FROM TaxonNameExternalDatabase I WHERE I.BaseURL is null;
GO
ALTER TABLE TaxonNameExternalDatabase ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[TaxonNameExternalDatabase] DROP CONSTRAINT [PK_TaxonNameExternalDatabase] 
GO

ALTER TABLE [dbo].[TaxonNameExternalDatabase] ADD  CONSTRAINT [PK_TaxonNameExternalDatabase] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[ExternalDatabaseID] ASC
)
GO

--#####################################################################################################################
--######   Redesign of TaxonNameExternalID   ##########################################################################
--#####################################################################################################################

ALTER TABLE TaxonNameExternalID ADD [BaseURL] VARCHAR(500);
GO
UPDATE C SET [BaseURL] = A.[BaseURL]
FROM TaxonSynonymy A INNER JOIN TaxonNameExternalID C ON A.NameID = C.NameID;
GO
DELETE I
FROM TaxonNameExternalID I WHERE I.BaseURL is null;
GO
ALTER TABLE TaxonNameExternalID ALTER COLUMN [BaseURL] VARCHAR(500) NOT NULL;
GO
ALTER TABLE [dbo].[TaxonNameExternalID] DROP CONSTRAINT [PK_TaxonNameExternalID] 
GO

ALTER TABLE [dbo].[TaxonNameExternalID] ADD  CONSTRAINT [PK_TaxonNameExternalID] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	NameID ASC,
	[ExternalDatabaseID] ASC
)
GO


--#####################################################################################################################
--######   New table TaxonSynonymySourceView   ########################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaxonSynonymySourceView](
	[BaseURL] [varchar](500) NOT NULL,
	NameID int NOT NULL,
	[SourceView] [nvarchar](128) NOT NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_TaxonSynonymySourceView] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	NameID ASC,
	[SourceView] ASC
)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaxonSynonymySourceView] ADD  CONSTRAINT [DF_TaxonSynonymySourceView_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceView', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySourceView', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

GRANT SELECT ON [TaxonSynonymySourceView] TO [CacheUser];
GRANT INSERT ON [TaxonSynonymySourceView] TO [CacheAdmin];
GRANT UPDATE ON [TaxonSynonymySourceView] TO [CacheAdmin];
GRANT DELETE ON [TaxonSynonymySourceView] TO [CacheAdmin];
GO

INSERT INTO [dbo].[TaxonSynonymySourceView]
           ([BaseURL]
           ,[NameID]
           ,[SourceView]
           ,[LogInsertedWhen])
SELECT [BaseURL]
      ,[NameID]
      ,[SourceView]
      ,[LogInsertedWhen]
  FROM [dbo].[TaxonSynonymy]

GO





--#####################################################################################################################
--######   set Default for LogInsertedWhen in table ReferenceTitle   ##################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c
where c.TABLE_CATALOG = 'DiversityCollectionCache_Test'
and c.COLUMN_NAME = 'loginsertedwhen'
and c.TABLE_NAME = 'ReferenceTitle'
and c.COLUMN_DEFAULT is null
) = 1
begin
ALTER TABLE [dbo].[ReferenceTitle] ADD  CONSTRAINT [DF_ReferenceTitle_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
end
GO

ALTER TABLE [dbo].[ReferenceTitle] ALTER COLUMN [LogInsertedWhen] datetime NULL
GO

--#####################################################################################################################
--######   set Grants   ###############################################################################################
--#####################################################################################################################

ALTER ROLE [db_ddladmin] ADD MEMBER [CacheAdmin]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [CacheAdmin]
GO
GRANT UPDATE ON [ProjectPublished] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   set subsets for sources   ##################################################################################
--#####################################################################################################################

UPDATE [dbo].[AgentSource]
   SET [Subsets] = 'AgentContactInformation|AgentImage'
GO

UPDATE [dbo].[GazetteerSource]
   SET [Subsets] = 'GazetteerExternalDatabase'
GO

UPDATE [dbo].[ReferenceTitleSource]
   SET [Subsets] = 'ReferenceRelator'
GO

UPDATE [dbo].[SamplingPlotSource]
   SET [Subsets] = 'SamplingPlotLocalisation|SamplingPlotProperty|procSamplingPlotLocalisationHierarchy|procSamplingPlotPropertyHierarchy'
GO

UPDATE [dbo].[TaxonSynonymySource]
   SET [Subsets] = 'TaxonAnalysis|TaxonAnalysisCategory|TaxonAnalysisCategoryValue|TaxonCommonName|TaxonList|TaxonNameExternalDatabase|TaxonNameExternalID'
GO

--#####################################################################################################################
--######   set new names for taxon sources   ##########################################################################
--#####################################################################################################################

--update S set [SourceView] = 'Taxon' + [SourceView]
--  FROM [TaxonSynonymySource] S where S.SourceView like 'Names_%'




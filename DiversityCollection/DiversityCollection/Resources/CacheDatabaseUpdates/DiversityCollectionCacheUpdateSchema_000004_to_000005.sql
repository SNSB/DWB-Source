


--#####################################################################################################################
--######  creating CacheAnalysis  #####################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheAnalysis') = 0
begin

	CREATE TABLE [#project#].[CacheAnalysis](
		[AnalysisID] [int] NOT NULL,
		[AnalysisParentID] [int] NULL,
		[DisplayText] [nvarchar](50) NULL,
		[Description] [nvarchar](max) NULL,
		[MeasurementUnit] [nvarchar](50) NULL,
		[Notes] [nvarchar](max) NULL,
		[AnalysisURI] [varchar](255) NULL,
	 CONSTRAINT [PK_CacheAnalysis] PRIMARY KEY CLUSTERED 
	(
		[AnalysisID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheAnalysis TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheAnalysis TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheAnalysis TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheAnalysis TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--#####################################################################################################################
--######   correction of the procedures resp. views for publishing the data         ###################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   procPublishAnalysis   ######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishAnalysis] 
AS
/*
-- TEST
EXECUTE  [dbo].[procPublishAnalysis]
*/
truncate table [#project#].CacheAnalysis

INSERT INTO [#project#].[CacheAnalysis]
           ([AnalysisID]
		   ,[AnalysisParentID]
           ,[DisplayText]
           ,[Description]
           ,[MeasurementUnit]
           ,[Notes]
           ,[AnalysisURI])
SELECT DISTINCT A.AnalysisID, A.AnalysisParentID, A.DisplayText, A.Description, A.MeasurementUnit, A.Notes, A.AnalysisURI
FROM dbo.ViewAnalysis A')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   procPublishCollectionSpecimenPart  #########################################################################
--#####################################################################################################################


ALTER  PROCEDURE [#project#].[procPublishCollectionSpecimenPart] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenPart]
*/
truncate table [#project#].CacheCollectionSpecimenPart

INSERT INTO [#project#].[CacheCollectionSpecimenPart]
           ([CollectionSpecimenID]
      ,[SpecimenPartID]
      ,[DerivedFromSpecimenPartID]
      ,[PreparationMethod]
      ,[PreparationDate]
      ,[PartSublabel]
      ,[CollectionID]
      ,[MaterialCategory]
      ,[StorageLocation]
      ,[Stock]
      ,[Notes]
      ,[AccessionNumber]
      ,[StorageContainer]
      ,[StockUnit]
      ,[ResponsibleName])
SELECT DISTINCT S.[CollectionSpecimenID]
      ,S.[SpecimenPartID]
      ,[DerivedFromSpecimenPartID]
      ,[PreparationMethod]
      ,[PreparationDate]
      ,[PartSublabel]
      ,[CollectionID]
      ,S.[MaterialCategory]
      ,[StorageLocation]
      ,[Stock]
      ,[Notes]
      ,[AccessionNumber]
      ,[StorageContainer]
      ,[StockUnit]
      ,[ResponsibleName]
  FROM [dbo].[ViewCollectionSpecimenPart] S, dbo.ViewCollectionProject P, [#project#].ProjectMaterialCategory M
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()
  and M.MaterialCategory COLLATE DATABASE_DEFAULT = S.MaterialCategory COLLATE DATABASE_DEFAULT


GO


--#####################################################################################################################
--######  ProjectLocalisationSystem - adding Parsing method  ##########################################################
--#####################################################################################################################
if (SELECT count(*) from INFORMATION_SCHEMA.COLUMNS C where C.COLUMN_NAME = 'ParsingMethodName' and C.TABLE_NAME = 'ProjectLocalisationSystem' and C.TABLE_SCHEMA = '#project#') = 0
begin
ALTER TABLE [#project#].[ProjectLocalisationSystem] ADD [ParsingMethodName] [nvarchar](50) NULL
end
GO

--#####################################################################################################################
--######  writing Parsing method in existing data            ##########################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'UPDATE P
SET P.[ParsingMethodName] = L.[ParsingMethodName]
FROM [#project#].[ProjectLocalisationSystem] P
' +  dbo.SourceDatabase() + '.dbo.[LocalisationSystem] L
WHERE P.[LocalisationSystemID] = L.[LocalisationSystemID]')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = ''
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   procPublishCollectionEventLocalisation  ######################################################################################
--#####################################################################################################################


ALTER  PROCEDURE [#project#].[procPublishCollectionEventLocalisation] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEventLocalisation]
*/
truncate table [#project#].CacheCollectionEventLocalisation

INSERT INTO [#project#].[CacheCollectionEventLocalisation]
           ([CollectionEventID]
      ,[LocalisationSystemID]
      ,[Location1]
      ,[Location2]
      ,[LocationAccuracy]
      ,[LocationNotes]
      ,[DeterminationDate]
      ,[DistanceToLocation]
      ,[DirectionToLocation]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AverageAltitudeCache]
      ,[AverageLatitudeCache]
      ,[AverageLongitudeCache]
      ,[RecordingMethod])
SELECT DISTINCT E.[CollectionEventID]
      ,E.[LocalisationSystemID]
      ,[Location1]
      ,[Location2]
      ,[LocationAccuracy]
      ,[LocationNotes]
      ,[DeterminationDate]
      ,[DistanceToLocation]
      ,[DirectionToLocation]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AverageAltitudeCache]
      ,[AverageLatitudeCache]
      ,[AverageLongitudeCache]
      ,[RecordingMethod]
  FROM [dbo].[ViewCollectionEventLocalisation] E,
dbo.ViewCollectionSpecimen S, 
dbo.ViewCollectionProject P,
[#project#].ProjectLocalisationSystem L
  where E.CollectionEventID = S.CollectionEventID 
  AND S.CollectionSpecimenID = p.CollectionSpecimenID
  AND L.[LocalisationSystemID] = E.[LocalisationSystemID]
  and p.ProjectID = [#project#].ProjectID()

if not (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())  is null
begin
	declare @Precision int
	set @Precision = (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())

	-- rounding average values for all entries
	update L 
	set [AverageLatitudeCache] = round(L.[AverageLatitudeCache], @Precision ),
	[AverageLongitudeCache] = round([AverageLongitudeCache], @Precision)
	from [#project#].[CacheCollectionEventLocalisation] L

	-- rounding location values for coordinate entries
	update E 
	set [Location2] = round(E.[AverageLatitudeCache], @Precision ),
	[Location1] = round(E.[AverageLongitudeCache], @Precision)
	from [#project#].[CacheCollectionEventLocalisation] E,
	[#project#].[ProjectLocalisationSystem] L
	WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
	AND L.ParsingMethodName = 'Coordinates'

	-- rounding location values for Gauss Krueger entries
	declare @GK int
	if @Precision < 5
	begin
		set @GK = 5 - @Precision
		update E 
		set [Location2] = substring([Location2], 1, 7 - @GK) + replicate('0', @GK)  ,
		[Location1] = substring([Location1], 1, 7 - @GK) + replicate('0', @GK)  
		from [#project#].[CacheCollectionEventLocalisation] E,
		[#project#].[ProjectLocalisationSystem] L
		WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
		AND L.ParsingMethodName = 'GK'
	end
end

GO






--#####################################################################################################################
--######   procPublishIdentification  #################################################################################
--#####################################################################################################################


ALTER  PROCEDURE [#project#].[procPublishIdentification] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentification]
*/
truncate table [#project#].CacheIdentification

INSERT INTO [#project#].[CacheIdentification]
           ([CollectionSpecimenID]
           ,[IdentificationUnitID]
           ,[IdentificationSequence]
           ,[IdentificationDay]
           ,[IdentificationMonth]
           ,[IdentificationYear]
           ,[IdentificationDateSupplement]
           ,[VernacularTerm]
           ,[TaxonomicName]
           ,[NameURI]
           ,[IdentificationCategory]
           ,[IdentificationQualifier]
           ,[TypeStatus]
           ,[TypeNotes]
           ,[ReferenceTitle]
           ,[ReferenceDetails]
           ,[Notes]
           ,[ResponsibleName])
SELECT DISTINCT S.[CollectionSpecimenID]
      ,S.[IdentificationUnitID]
           ,[IdentificationSequence]
           ,[IdentificationDay]
           ,[IdentificationMonth]
           ,[IdentificationYear]
           ,[IdentificationDateSupplement]
           ,[VernacularTerm]
           ,[TaxonomicName]
           ,[NameURI]
           ,[IdentificationCategory]
           ,[IdentificationQualifier]
           ,[TypeStatus]
           ,[TypeNotes]
           ,[ReferenceTitle]
           ,[ReferenceDetails]
           ,S.[Notes]
           ,[ResponsibleName]
  FROM [dbo].[ViewIdentification] S, 
  dbo.ViewCollectionProject P,
  [#project#].CacheIdentificationUnit U
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and U.IdentificationUnitID = S.IdentificationUnitID
  and U.CollectionSpecimenID = S.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()

  GO



--#####################################################################################################################
--######   procPublishIdentificationUnit  ######################################################################################
--#####################################################################################################################


ALTER PROCEDURE [#project#].[procPublishIdentificationUnit] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnit]
*/
truncate table [#project#].CacheIdentificationUnit

INSERT INTO [#project#].[CacheIdentificationUnit]
           ([CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[LastIdentificationCache]
      ,[TaxonomicGroup]
      ,[RelatedUnitID]
      ,[RelationType]
      ,[ExsiccataNumber]
      ,[DisplayOrder]
      ,[ColonisedSubstratePart]
      ,[FamilyCache]
      ,[OrderCache]
      ,[LifeStage]
      ,[Gender]
      ,[HierarchyCache]
      ,[UnitIdentifier]
      ,[UnitDescription]
      ,[Circumstances]
      ,[Notes]
      ,[NumberOfUnits]
      ,[OnlyObserved])
SELECT DISTINCT S.[CollectionSpecimenID]
      ,S.[IdentificationUnitID]
      ,[LastIdentificationCache]
      ,S.[TaxonomicGroup]
      ,[RelatedUnitID]
      ,[RelationType]
      ,[ExsiccataNumber]
      ,[DisplayOrder]
      ,[ColonisedSubstratePart]
      ,[FamilyCache]
      ,[OrderCache]
      ,[LifeStage]
      ,[Gender]
      ,[HierarchyCache]
      ,[UnitIdentifier]
      ,[UnitDescription]
      ,[Circumstances]
      ,[Notes]
      ,[NumberOfUnits]
      ,[OnlyObserved]
  FROM [dbo].[ViewIdentificationUnit] S
  , dbo.ViewCollectionProject P
  , [#project#].ProjectTaxonomicGroup T
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()
  and T.TaxonomicGroup COLLATE DATABASE_DEFAULT = S.TaxonomicGroup COLLATE DATABASE_DEFAULT
GO

GRANT EXEC ON [#project#].[procPublishIdentificationUnit] TO [CacheAdmin_#project#] 
GO


--#####################################################################################################################
--######   procPublishIdentificationUnitAnalysis  ######################################################################################
--#####################################################################################################################


ALTER PROCEDURE [#project#].[procPublishIdentificationUnitAnalysis] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnitAnalysis]
*/
truncate table [#project#].CacheIdentificationUnitAnalysis

INSERT INTO [#project#].[CacheIdentificationUnitAnalysis]
           ([AnalysisID]
      ,[AnalysisNumber]
      ,[AnalysisResult]
      ,[ExternalAnalysisURI]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AnalysisDate]
      ,[SpecimenPartID]
      ,[CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[Notes])
SELECT DISTINCT S.[AnalysisID]
      ,[AnalysisNumber]
      ,[AnalysisResult]
      ,[ExternalAnalysisURI]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AnalysisDate]
      ,[SpecimenPartID]
      ,S.[CollectionSpecimenID]
      ,S.[IdentificationUnitID]
      ,S.[Notes]
  FROM [dbo].[ViewIdentificationUnitAnalysis] S, 
  dbo.ViewCollectionProject P,
  [#project#].CacheIdentificationUnit U
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and U.IdentificationUnitID = S.IdentificationUnitID
  and U.CollectionSpecimenID = S.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()
GO






--#####################################################################################################################
--######   procPublishIdentificationUnitInPart  ######################################################################################
--#####################################################################################################################


ALTER  PROCEDURE [#project#].[procPublishIdentificationUnitInPart] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnitInPart]
*/
truncate table [#project#].CacheIdentificationUnitInPart

INSERT INTO [#project#].[CacheIdentificationUnitInPart]
           ([Description]
      ,[CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[SpecimenPartID]
      ,[DisplayOrder])
SELECT DISTINCT S.[Description]
      ,S.[CollectionSpecimenID]
      ,S.[IdentificationUnitID]
      ,S.[SpecimenPartID]
      ,S.[DisplayOrder]
  FROM [dbo].[ViewIdentificationUnitInPart] S, 
  [#project#].CacheIdentificationUnit U,
  [#project#].[CacheCollectionSpecimenPart] P
  where S.[SpecimenPartID] = p.[SpecimenPartID]
  AND S.[IdentificationUnitID] = U.[IdentificationUnitID]
GO



--#####################################################################################################################
--######   procPublishCollectionAgent  ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionAgent] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionAgent]
*/
truncate table [#project#].CacheCollectionAgent

declare @AnonymCollector TABLE (
	[ID] int Identity NOT NULL,
	[CollectorsName] [nvarchar](400) COLLATE DATABASE_DEFAULT NOT NULL  Primary key,
	[Anonymisation] [nvarchar](50) COLLATE DATABASE_DEFAULT NOT NULL) 

INSERT INTO @AnonymCollector(CollectorsName, Anonymisation)
SELECT CollectorsName, Anonymisation
FROM ' +  dbo.SourceDatabase() + '.dbo.AnonymCollector

INSERT INTO [#project#].[CacheCollectionAgent]
           ([CollectionSpecimenID]
           ,[CollectorsName]
           ,[CollectorsSequence]
           ,[CollectorsNumber])
SELECT DISTINCT C.CollectionSpecimenID
, CASE WHEN A.Anonymisation COLLATE DATABASE_DEFAULT IS NULL THEN C.CollectorsName COLLATE DATABASE_DEFAULT ELSE A.Anonymisation COLLATE DATABASE_DEFAULT + '' '' + CAST (A.ID AS varchar) END AS CollectorsName
, C.CollectorsSequence, CASE WHEN A.Anonymisation COLLATE DATABASE_DEFAULT IS NULL THEN C.CollectorsNumber COLLATE DATABASE_DEFAULT ELSE NULL END AS CollectorsNumber
FROM dbo.ViewCollectionProject AS P INNER JOIN dbo.ViewCollectionAgent AS C ON P.CollectionSpecimenID = C.CollectionSpecimenID 
LEFT OUTER JOIN @AnonymCollector AS A ON C.CollectorsName COLLATE DATABASE_DEFAULT = A.CollectorsName COLLATE DATABASE_DEFAULT
WHERE P.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishCollectionAgent] TO [CacheAdmin_#project#] 
GO








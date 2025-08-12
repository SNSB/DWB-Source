
--#####################################################################################################################
--######   procPublishCollectionEvent  #################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionAgent] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionAgent]
*/
truncate table [#project#].CacheCollectionAgent

declare @AnonymCollector TABLE (
	[ID] int Identity NOT NULL,
	[CollectorsName] [nvarchar](400) NOT NULL  Primary key,
	[Anonymisation] [nvarchar](50) NOT NULL) 

INSERT INTO @AnonymCollector(CollectorsName, Anonymisation)
SELECT CollectorsName, Anonymisation
FROM ' +  dbo.SourceDatabase() + '.dbo.AnonymCollector

INSERT INTO [#project#].[CacheCollectionAgent]
           ([CollectionSpecimenID]
           ,[CollectorsName]
           ,[CollectorsSequence]
           ,[CollectorsNumber])
SELECT DISTINCT C.CollectionSpecimenID, CASE WHEN A.Anonymisation IS NULL THEN C.CollectorsName COLLATE DATABASE_DEFAULT ELSE A.Anonymisation COLLATE DATABASE_DEFAULT + '' '' + CAST (A.ID AS varchar) END AS CollectorsName, C.CollectorsSequence, 
CASE WHEN A.Anonymisation IS NULL THEN C.CollectorsNumber ELSE NULL END AS CollectorsNumber
FROM [#project#].CacheCollectionSpecimen AS S INNER JOIN [' +  dbo.SourceDatabase() + '].dbo.CollectionAgent AS C ON S.CollectionSpecimenID = C.CollectionSpecimenID 
LEFT OUTER JOIN @AnonymCollector AS A ON C.CollectorsName COLLATE DATABASE_DEFAULT = A.CollectorsName COLLATE DATABASE_DEFAULT
WHERE (C.DataWithholdingReason = N'''') OR (C.DataWithholdingReason IS NULL)
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   procPublishCollectionEvent  #################################################################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionEvent] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEvent]
*/
truncate table [#project#].CacheCollectionEvent

INSERT INTO [#project#].[CacheCollectionEvent]
           ([CollectionEventID]
      ,[Version]
      ,[CollectorsEventNumber]
      ,[CollectionDate]
      ,[CollectionDay]
      ,[CollectionMonth]
      ,[CollectionYear]
      ,[CollectionDateSupplement]
      ,[CollectionTime]
      ,[CollectionTimeSpan]
      ,[LocalityDescription]
      ,[HabitatDescription]
      ,[ReferenceTitle]
      ,[CollectingMethod]
      ,[Notes]
      ,[CountryCache]
      ,[ReferenceDetails]
      ,[LocalityVerbatim]
      ,[CollectionEndDay]
      ,[CollectionEndMonth]
      ,[CollectionEndYear])
SELECT DISTINCT E.[CollectionEventID]
      ,[Version]
      ,[CollectorsEventNumber]
      ,[CollectionDate]
      ,[CollectionDay]
      ,[CollectionMonth]
      ,[CollectionYear]
      ,[CollectionDateSupplement]
      ,[CollectionTime]
      ,[CollectionTimeSpan]
      ,[LocalityDescription]
      ,[HabitatDescription]
      ,E.[ReferenceTitle]
      ,[CollectingMethod]
      ,[Notes]
      ,[CountryCache]
      ,[ReferenceDetails]
      ,[LocalityVerbatim]
      ,[CollectionEndDay]
      ,[CollectionEndMonth]
      ,[CollectionEndYear]
  FROM ' +  dbo.SourceDatabase() + '.dbo.[CollectionEvent] E,
[#project#].CacheCollectionSpecimen S
  where E.CollectionEventID = S.CollectionEventID 
  AND (E.DataWithholdingReason = '''') OR (E.DataWithholdingReason IS NULL)')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   procPublishCollectionEventLocalisation  ####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionEventLocalisation] 
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
SELECT DISTINCT L.[CollectionEventID]
      ,L.[LocalisationSystemID]
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
  FROM ' +  dbo.SourceDatabase() + '.[dbo].[CollectionEventLocalisation] L,
[#project#].[CacheCollectionEvent] E
  where E.CollectionEventID = L.CollectionEventID 

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
	[#project#].[CacheLocalisationSystem] L
	WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
	AND L.ParsingMethodName = ''Coordinates''

	-- rounding location values for Gauss Krueger entries
	declare @GK int
	if @Precision < 5
	begin
		set @GK = 5 - @Precision
		update E 
		set [Location2] = substring([Location2], 1, 7 - @GK) + replicate(''0'', @GK)  ,
		[Location1] = substring([Location1], 1, 7 - @GK) + replicate(''0'', @GK)  
		from [#project#].[CacheCollectionEventLocalisation] E,
		[#project#].[CacheLocalisationSystem] L
		WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
		AND L.ParsingMethodName = ''GK''
	end
end
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch



GO



--#####################################################################################################################
--######   procPublishCollectionEventProperty  ########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionEventProperty] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEventProperty]
*/
truncate table [#project#].CacheCollectionEventProperty

INSERT INTO [#project#].[CacheCollectionEventProperty]
           ([CollectionEventID]
      ,[PropertyID]
      ,[DisplayText]
      ,[PropertyURI]
      ,[PropertyHierarchyCache]
      ,[PropertyValue]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[Notes]
      ,[AverageValueCache])
SELECT DISTINCT P.[CollectionEventID]
      ,[PropertyID]
      ,[DisplayText]
      ,[PropertyURI]
      ,[PropertyHierarchyCache]
      ,[PropertyValue]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,P.[Notes]
      ,[AverageValueCache]
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionEventProperty] P,
  [#project#].[CacheCollectionEvent] E
  where E.CollectionEventID = P.CollectionEventID
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch



GO


--#####################################################################################################################
--######   procPublishCollectionSpecimenImage  ########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionSpecimenImage] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenImage]
*/
truncate table [#project#].CacheCollectionSpecimenImage

INSERT INTO [#project#].[CacheCollectionSpecimenImage]
           ([CollectionSpecimenID]
      ,[URI]
      ,[ResourceURI]
      ,[SpecimenPartID]
      ,[IdentificationUnitID]
      ,[ImageType]
      ,[Notes]
      ,[LicenseURI]
      ,[LicenseNotes]
      ,[DisplayOrder]
      ,[LicenseYear]
      ,[LicenseHolderAgentURI]
      ,[LicenseHolder]
      ,[LicenseType]
      ,[CopyrightStatement]
      ,[CreatorAgentURI]
      ,[CreatorAgent]
      ,[IPR]
      ,[Title])
SELECT DISTINCT I.[CollectionSpecimenID]
      ,[URI]
      ,[ResourceURI]
      ,[SpecimenPartID]
      ,[IdentificationUnitID]
      ,[ImageType]
      ,[Notes]
      ,[LicenseURI]
      ,[LicenseNotes]
      ,[DisplayOrder]
      ,[LicenseYear]
      ,[LicenseHolderAgentURI]
      ,[LicenseHolder]
      ,[LicenseType]
      ,[CopyrightStatement]
      ,[CreatorAgentURI]
      ,[CreatorAgent]
      ,[IPR]
      ,[Title]
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimenImage] I, 
  [#project#].[CacheCollectionSpecimen] S
  where I.CollectionSpecimenID = S.CollectionSpecimenID
  and (I.DataWithholdingReason IS NULL OR I.DataWithholdingReason = '''')
')
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

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionSpecimenPart] 
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
SELECT DISTINCT P.[CollectionSpecimenID]
      ,P.[SpecimenPartID]
      ,[DerivedFromSpecimenPartID]
      ,[PreparationMethod]
      ,[PreparationDate]
      ,[PartSublabel]
      ,[CollectionID]
      ,P.[MaterialCategory]
      ,[StorageLocation]
      ,[Stock]
      ,[Notes]
      ,P.[AccessionNumber]
      ,[StorageContainer]
      ,[StockUnit]
      ,[ResponsibleName]
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimenPart] P, 
  [#project#].CacheCollectionSpecimen S, [#project#].ProjectMaterialCategory M
  where S.CollectionSpecimenID = P.CollectionSpecimenID
  and M.MaterialCategory COLLATE DATABASE_DEFAULT = P.MaterialCategory COLLATE DATABASE_DEFAULT
  and (P.DataWithholdingReason IS NULL OR P.DataWithholdingReason = '''')
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   procPublishCollectionSpecimenProcessing  ###################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionSpecimenProcessing] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenProcessing]
*/
truncate table [#project#].CacheCollectionSpecimenProcessing

INSERT INTO [#project#].[CacheCollectionSpecimenProcessing]
           (CollectionSpecimenID, 
		   SpecimenProcessingID, 
		   SpecimenPartID, 
		   ProcessingDate, 
		   ProcessingID, 
		   Protocoll, 
		   ProcessingDuration, 
		   ResponsibleName, 
		   ResponsibleAgentURI, 
		   Notes)
SELECT DISTINCT R.[CollectionSpecimenID]
      ,[SpecimenProcessingID]
      ,R.[SpecimenPartID]
      ,[ProcessingDate]
      ,[ProcessingID]
      ,[Protocoll]
      ,[ProcessingDuration]
      ,R.[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,R.[Notes]
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimenProcessing] R, 
  [#project#].[CacheCollectionSpecimenPart] P
  where R.CollectionSpecimenID = P.CollectionSpecimenID
  and R.SpecimenPartID = P.SpecimenPartID
  UNION
SELECT DISTINCT R.[CollectionSpecimenID]
      ,[SpecimenProcessingID]
      ,R.[SpecimenPartID]
      ,[ProcessingDate]
      ,[ProcessingID]
      ,[Protocoll]
      ,[ProcessingDuration]
      ,R.[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,R.[Notes]
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimenProcessing] R, 
  [#project#].[CacheCollectionSpecimen] S
  where R.CollectionSpecimenID = S.CollectionSpecimenID
  and R.SpecimenPartID IS NULL
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   procPublishCollectionSpecimenRelation  #####################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionSpecimenRelation] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenRelation]
*/
truncate table [#project#].CacheCollectionSpecimenRelation

INSERT INTO [#project#].[CacheCollectionSpecimenRelation]
           ([CollectionSpecimenID]
      ,[RelatedSpecimenURI]
      ,[RelatedSpecimenDisplayText]
      ,[RelationType]
      ,[RelatedSpecimenCollectionID]
      ,[RelatedSpecimenDescription]
      ,[Notes]
      ,[IdentificationUnitID]
      ,[SpecimenPartID])
SELECT DISTINCT R.[CollectionSpecimenID]
      ,[RelatedSpecimenURI]
      ,[RelatedSpecimenDisplayText]
      ,[RelationType]
      ,[RelatedSpecimenCollectionID]
      ,[RelatedSpecimenDescription]
      ,[Notes]
      ,[IdentificationUnitID]
      ,[SpecimenPartID]
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimenRelation] R, 
  [#project#].CacheCollectionSpecimen S
  where S.CollectionSpecimenID = R.CollectionSpecimenID
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   CacheIdentification  #######################################################################################
--#####################################################################################################################

ALTER TABLE [#project#].[CacheIdentification] ADD NameID int NULL;
GO

ALTER TABLE [#project#].[CacheIdentification] ADD BaseURL varchar(255) NULL;
GO


--#####################################################################################################################
--######   procPublishIdentification  #################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER PROCEDURE [#project#].[procPublishIdentification] 
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
           ,[ResponsibleName]
		   ,[NameID]
		   ,[BaseURL])
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
 		   ,CASE WHEN NameURI LIKE ''%/%'' THEN REVERSE(SUBSTRING(REVERSE(NameURI), 1, CHARINDEX(''/'', REVERSE(NameURI))-1)) ELSE NULL END AS NameID
		   ,CASE WHEN NameURI LIKE ''%/%'' THEN REVERSE(SUBSTRING(REVERSE(NameURI), CHARINDEX(''/'', REVERSE(NameURI)), 255)) ELSE NULL END AS BaseURL
 FROM ' +  dbo.SourceDatabase() + '.dbo.Identification S, 
  [#project#].CacheIdentificationUnit U
  where U.IdentificationUnitID = S.IdentificationUnitID
  and U.CollectionSpecimenID = S.CollectionSpecimenID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   procPublishIdentificationUnitAnalysis  #####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishIdentificationUnit] 
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
      ,[OnlyObserved]
	  ,[RetrievalType])
SELECT DISTINCT U.[CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[LastIdentificationCache]
      ,U.[TaxonomicGroup]
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
	  ,[RetrievalType]
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[IdentificationUnit] U, 
  [#project#].CacheCollectionSpecimen S, 
  [#project#].ProjectTaxonomicGroup T
  where S.CollectionSpecimenID = U.CollectionSpecimenID
  and T.TaxonomicGroup COLLATE DATABASE_DEFAULT = U.TaxonomicGroup COLLATE DATABASE_DEFAULT
  and   (U.DataWithholdingReason IS NULL) OR
                         (U.DataWithholdingReason = N'''')
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   procPublishIdentificationUnitAnalysis  #####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER PROCEDURE [#project#].[procPublishIdentificationUnitAnalysis] 
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
  FROM ' +  dbo.SourceDatabase() + '.[dbo].[IdentificationUnitAnalysis] S, 
  [#project#].CacheIdentificationUnit U
  where U.IdentificationUnitID = S.IdentificationUnitID
  and U.CollectionSpecimenID = S.CollectionSpecimenID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch


GO



--#####################################################################################################################
--######   procPublishIdentificationUnitInPart  #######################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishIdentificationUnitInPart] 
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
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[IdentificationUnitInPart] S, 
  [#project#].CacheIdentificationUnit U,
  [#project#].[CacheCollectionSpecimenPart] P
  where S.[SpecimenPartID] = P.[SpecimenPartID]
  AND S.[IdentificationUnitID] = U.[IdentificationUnitID]
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch


GO












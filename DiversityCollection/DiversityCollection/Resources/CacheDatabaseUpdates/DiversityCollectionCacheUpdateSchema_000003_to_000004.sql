

--#####################################################################################################################
--#####################################################################################################################
--######   Creating the procedures to publish the data   ##############################################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   procPublishAnalysis  #######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishAnalysis] 
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
           ,[AnalysisURI]
           ,[OnlyHierarchy])
SELECT DISTINCT A.AnalysisID, A.AnalysisParentID, A.DisplayText, A.Description, A.MeasurementUnit, A.Notes, A.AnalysisURI, A.OnlyHierarchy
FROM dbo.ViewAnalysis A
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishAnalysis] TO [CacheAdmin_#project#] 
GO


--#####################################################################################################################
--######   procPublishCollectionAgent  ################################################################################
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
, C.CollectorsSequence, CASE WHEN A.Anonymisation IS NULL THEN C.CollectorsNumber ELSE NULL END AS CollectorsNumber
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





--#####################################################################################################################
--######   procPublishCollectionSpecimen  #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionSpecimen] 
AS
TRUNCATE TABLE [#project#].CacheCollectionSpecimen

INSERT INTO [#project#].CacheCollectionSpecimen
                      ([CollectionSpecimenID]
      ,[LabelTranscriptionNotes]
      ,[OriginalNotes]
      ,[LogUpdatedWhen]
      ,[CollectionEventID]
      ,[AccessionNumber]
      ,[AccessionDate]
      ,[AccessionDay]
      ,[AccessionMonth]
      ,[AccessionYear]
      ,[DepositorsName]
      ,[DepositorsAccessionNumber]
      ,[ExsiccataURI]
      ,[ExsiccataAbbreviation]
      ,[AdditionalNotes]
      ,[ReferenceTitle]
      ,[ReferenceURI])
SELECT DISTINCT S.[CollectionSpecimenID]
      ,[LabelTranscriptionNotes]
      ,[OriginalNotes]
      ,S.[LogUpdatedWhen]
      ,[CollectionEventID]
      ,[AccessionNumber]
      ,[AccessionDate]
      ,[AccessionDay]
      ,[AccessionMonth]
      ,[AccessionYear]
      ,[DepositorsName]
      ,[DepositorsAccessionNumber]
      ,[ExsiccataURI]
      ,[ExsiccataAbbreviation]
      ,[AdditionalNotes]
      ,[ReferenceTitle]
      ,[ReferenceURI]
  FROM [dbo].[ViewCollectionSpecimen] S, dbo.ViewCollectionProject P
  where s.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishCollectionSpecimen] TO [CacheAdmin_#project#] 
GO



--#####################################################################################################################
--######   procPublishCollectionSpecimenImage  ########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionSpecimenImage] 
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
  FROM [dbo].[ViewCollectionSpecimenImage] I, dbo.ViewCollectionProject P
  where I.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


GRANT EXEC ON [#project#].[procPublishCollectionSpecimenImage] TO [CacheAdmin_#project#] 
GO



--#####################################################################################################################
--######   procPublishCollectionSpecimenPart  #########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionSpecimenPart] 
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
      ,[SpecimenPartID]
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
  and M.MaterialCategory COLLATE DATABASE_DEFAULT = S.MaterialCategory COLLATE DATABASE_DEFAULT')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


GRANT EXEC ON [#project#].[procPublishCollectionSpecimenPart] TO [CacheAdmin_#project#] 
GO





--#####################################################################################################################
--######   procPublishCollectionSpecimenRelation  #####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionSpecimenRelation] 
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
SELECT DISTINCT S.[CollectionSpecimenID]
      ,[RelatedSpecimenURI]
      ,[RelatedSpecimenDisplayText]
      ,[RelationType]
      ,[RelatedSpecimenCollectionID]
      ,[RelatedSpecimenDescription]
      ,[Notes]
      ,[IdentificationUnitID]
      ,[SpecimenPartID]
  FROM [dbo].[ViewCollectionSpecimenRelation] S, dbo.ViewCollectionProject P
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishCollectionSpecimenRelation] TO [CacheAdmin_#project#] 
GO





--#####################################################################################################################
--######   procPublishCollectionEvent  ################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionEvent] 
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
  FROM [dbo].[ViewCollectionEvent] E,
ViewCollectionSpecimen S, dbo.ViewCollectionProject P
  where E.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishCollectionEvent] TO [CacheAdmin_#project#] 
GO




--#####################################################################################################################
--######   procPublishCollectionEventLocalisation  ####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionEventLocalisation] 
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
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishCollectionEventLocalisation] TO [CacheAdmin_#project#] 
GO




--#####################################################################################################################
--######   procPublishCollectionEventProperty  ########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionEventProperty] 
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
SELECT DISTINCT E.[CollectionEventID]
      ,[PropertyID]
      ,[DisplayText]
      ,[PropertyURI]
      ,[PropertyHierarchyCache]
      ,[PropertyValue]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[Notes]
      ,[AverageValueCache]
  FROM [dbo].[ViewCollectionEventProperty] E,
ViewCollectionSpecimen S, dbo.ViewCollectionProject P
  where E.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishCollectionEventProperty] TO [CacheAdmin_#project#] 
GO




--#####################################################################################################################
--######   procPublishIdentification  #################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishIdentification] 
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
  FROM [dbo].[ViewIdentification] S, dbo.ViewCollectionProject P
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishIdentification] TO [CacheAdmin_#project#] 
GO



--#####################################################################################################################
--######   procPublishIdentificationUnit  #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishIdentificationUnit] 
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
      ,[IdentificationUnitID]
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
  FROM [dbo].[ViewIdentificationUnit] S, dbo.ViewCollectionProject P, [#project#].ProjectTaxonomicGroup T
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()
  and T.TaxonomicGroup COLLATE DATABASE_DEFAULT = S.TaxonomicGroup COLLATE DATABASE_DEFAULT')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishIdentificationUnit] TO [CacheAdmin_#project#] 
GO


--#####################################################################################################################
--######   procPublishIdentificationUnitAnalysis  #####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishIdentificationUnitAnalysis] 
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
      ,[IdentificationUnitID]
      ,[Notes]
  FROM [dbo].[ViewIdentificationUnitAnalysis] S, dbo.ViewCollectionProject P
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishIdentificationUnitAnalysis] TO [CacheAdmin_#project#] 
GO




--#####################################################################################################################
--######   procPublishIdentificationUnitInPart  #######################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishIdentificationUnitInPart] 
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
      ,[IdentificationUnitID]
      ,[SpecimenPartID]
      ,[DisplayOrder]
  FROM [dbo].[ViewIdentificationUnitInPart] S, dbo.ViewCollectionProject P
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishIdentificationUnitInPart] TO [CacheAdmin_#project#] 
GO


--#####################################################################################################################
--######   procPublishMetadata  #######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishMetadata] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishMetadata] 
*/
truncate table  [#project#].[CacheMetadata]
-- temporary table
insert into [#project#].[CacheMetadata] (ProjectID) values ([#project#].ProjectID())

declare  @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectSetting] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
')

set @SQL = (@SQL + '
	INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
	SELECT SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, 
	case when isdate(value) = 1 and ProjectSetting like ''%Date%'' then convert(varchar(20), cast(value as datetime), 126) else Value end as Value 
	FROM ' + dbo.ProjectsDatabase() + '.DBO.SettingsForProject([#project#].ProjectID(), ''ABCD | %'', ''.'', 2)
	IF (SELECT COUNT(*) FROM @SettingList L WHERE ProjectSetting = ''DateCreated'') = 0
	BEGIN
		INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
		SELECT -1, null, ''DateCreated'', ''DateCreated'', ''DateCreated'', 
		convert(varchar(19), min(LogCreatedWhen), 126) 
		from ' + dbo.SourceDatabase() + '.dbo.CollectionProject 
		where ProjectID = [#project#].ProjectID() 
	END
	')

set @SQL = (@SQL + '
	IF (SELECT COUNT(*) FROM @SettingList L WHERE ProjectSetting = ''DateModified'') = 0
	BEGIN
		INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
		SELECT -2, null, ''DateModified'', ''DateModified'', ''DateModified'', 
		convert(varchar(19), max(LastUpdatedWhen), 126) 
		from [dbo].ProjectPublished P WHERE P.ProjectID = [#project#].ProjectID()
	END
	')

set @SQL = (@SQL + '
UPDATE [#project#].[CacheMetadata] SET ProjectTitleCode  = (SELECT P.Project FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = [#project#].ProjectID())
UPDATE [#project#].[CacheMetadata] SET LicenseText  =   (SELECT MIN(L.LicenseType)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = [#project#].ProjectID()
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID)))

UPDATE [#project#].[CacheMetadata] SET LicensesDetails  =   (SELECT MIN(L.LicenseDetails)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = [#project#].ProjectID()
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID)))

UPDATE [#project#].[CacheMetadata] SET LicenseURI  =   (SELECT MIN(L.LicenseURI)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = [#project#].ProjectID()
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID)))
')

set @SQL = (@SQL + '
UPDATE [#project#].[CacheMetadata] SET TaxonomicGroup  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TaxonomicGroup'')
UPDATE [#project#].[CacheMetadata] SET DatasetGUID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetGUID'')
UPDATE [#project#].[CacheMetadata] SET TechnicalContactName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactName'')
UPDATE [#project#].[CacheMetadata] SET TechnicalContactEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactEmail'')
UPDATE [#project#].[CacheMetadata] SET TechnicalContactPhone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactPhone'')
UPDATE [#project#].[CacheMetadata] SET TechnicalContactAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactAddress'')
UPDATE [#project#].[CacheMetadata] SET ContentContactName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactName'')
UPDATE [#project#].[CacheMetadata] SET ContentContactEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactEmail'')
UPDATE [#project#].[CacheMetadata] SET ContentContactPhone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactPhone'')
UPDATE [#project#].[CacheMetadata] SET ContentContactAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactAddress'')
UPDATE [#project#].[CacheMetadata] SET OtherProviderUDDI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OtherProviderUDDI'')
UPDATE [#project#].[CacheMetadata] SET DatasetTitle  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetTitle'')
UPDATE [#project#].[CacheMetadata] SET DatasetDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetDetails'')
UPDATE [#project#].[CacheMetadata] SET DatasetCoverage  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetCoverage'')
UPDATE [#project#].[CacheMetadata] SET DatasetURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetURI'')
UPDATE [#project#].[CacheMetadata] SET DatasetIconURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetIconURI'')
UPDATE [#project#].[CacheMetadata] SET DatasetVersionMajor  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetVersionMajor'')
UPDATE [#project#].[CacheMetadata] SET DatasetCreators  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetCreators'')
')

set @SQL = (@SQL + '
UPDATE [#project#].[CacheMetadata] SET DatasetContributors  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetContributors'')
UPDATE [#project#].[CacheMetadata] SET DateCreated  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DateCreated'')
UPDATE [#project#].[CacheMetadata] SET DateModified  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DateModified'')
UPDATE [#project#].[CacheMetadata] SET SourceID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''SourceID'')
UPDATE [#project#].[CacheMetadata] SET SourceInstitutionID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''SourceInstitutionID'')
UPDATE [#project#].[CacheMetadata] SET OwnerOrganizationName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerOrganizationName'')
UPDATE [#project#].[CacheMetadata] SET OwnerOrganizationAbbrev  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerOrganizationAbbrev'')
UPDATE [#project#].[CacheMetadata] SET OwnerContactPerson  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerContactPerson'')
UPDATE [#project#].[CacheMetadata] SET OwnerContactRole  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerContactRole'')
UPDATE [#project#].[CacheMetadata] SET OwnerAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerAddress'')
UPDATE [#project#].[CacheMetadata] SET OwnerTelephone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerTelephone'')
UPDATE [#project#].[CacheMetadata] SET OwnerEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerEmail'')
UPDATE [#project#].[CacheMetadata] SET OwnerURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerURI'')
UPDATE [#project#].[CacheMetadata] SET OwnerLogoURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerLogoURI'')
UPDATE [#project#].[CacheMetadata] SET IPRText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRText'')
UPDATE [#project#].[CacheMetadata] SET IPRDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRDetails'')
UPDATE [#project#].[CacheMetadata] SET IPRURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRURI'')
UPDATE [#project#].[CacheMetadata] SET CopyrightText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightText'')
')

set @SQL = (@SQL + '
UPDATE [#project#].[CacheMetadata] SET CopyrightDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightDetails'')
UPDATE [#project#].[CacheMetadata] SET CopyrightURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightURI'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseText'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseDetails'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseURI'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersText'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersDetails'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersURI'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsText'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsDetails'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsURI'')
UPDATE [#project#].[CacheMetadata] SET CitationsText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsText'')
UPDATE [#project#].[CacheMetadata] SET CitationsDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsDetails'')
UPDATE [#project#].[CacheMetadata] SET CitationsURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsURI'')
UPDATE [#project#].[CacheMetadata] SET RecordBasis  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''RecordBasis'')
UPDATE [#project#].[CacheMetadata] SET KindOfUnit  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''KindOfUnit'')
UPDATE [#project#].[CacheMetadata] SET HigherTaxonRank  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''HigherTaxonRank'')
UPDATE [#project#].[CacheMetadata] SET RecordURI = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''RecordURI'')
')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch


GO


GRANT EXEC ON [#project#].[procPublishMetadata]  TO [CacheAdmin_#project#] 
GO














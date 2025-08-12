



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




--#####################################################################################################################
--######   procPublishIdentificationUnit   ############################################################################
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
  and  ((U.DataWithholdingReason IS NULL) OR (U.DataWithholdingReason = N''''))
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO










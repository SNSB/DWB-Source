

--#####################################################################################################################
--######   StableIdentifier in CacheIdentificationUnitInPart  #########################################################
--#####################################################################################################################
if (
select count(*) from INFORMATION_SCHEMA.COLUMNS C 
where C.Column_Name = 'StableIdentifier'
and C.TABLE_NAME = 'CacheIdentificationUnitInPart'
and C.TABLE_SCHEMA = '#project#')  = 0
begin
ALTER TABLE [#project#].CacheIdentificationUnitInPart ADD [StableIdentifier] nvarchar(500)
end
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
      ,[DisplayOrder]
	  ,[StableIdentifier])
SELECT DISTINCT S.[Description]
      ,S.[CollectionSpecimenID]
      ,S.[IdentificationUnitID]
      ,S.[SpecimenPartID]
      ,S.[DisplayOrder]
	  ,[' +  dbo.SourceDatabase() + '].[dbo].StableIdentifier(CP.ProjectID, S.CollectionSpecimenID, S.IdentificationUnitID, S.SpecimenPartID) AS StableIdentifier
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[IdentificationUnitInPart] S, 
  [' +  dbo.SourceDatabase() + '].[dbo].[CollectionProject] CP,
  [#project#].CacheIdentificationUnit U,
  [#project#].[CacheCollectionSpecimenPart] P
  where S.[SpecimenPartID] = P.[SpecimenPartID]
  AND S.[IdentificationUnitID] = U.[IdentificationUnitID]
  AND S.CollectionSpecimenID = CP.CollectionSpecimenID
  AND CP.ProjectID = [#project#].ProjectID()
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
--######   StableIdentifier in CacheIdentificationUnit  ###############################################################
--#####################################################################################################################
if (
select count(*) from INFORMATION_SCHEMA.COLUMNS C 
where C.Column_Name = 'StableIdentifier'
and C.TABLE_NAME = 'CacheIdentificationUnit'
and C.TABLE_SCHEMA = '#project#')  = 0
begin
ALTER TABLE [#project#].CacheIdentificationUnit ADD [StableIdentifier] nvarchar(500)
end
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
	  ,[RetrievalType]
	  ,[StableIdentifier])
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
	  ,[' +  dbo.SourceDatabase() + '].[dbo].StableIdentifier(CP.ProjectID, U.CollectionSpecimenID, U.IdentificationUnitID, NULL) AS StableIdentifier
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[IdentificationUnit] U, 
  [' +  dbo.SourceDatabase() + '].[dbo].[CollectionProject] CP, 
  [#project#].CacheCollectionSpecimen S, 
  [#project#].ProjectTaxonomicGroup T
  where S.CollectionSpecimenID = U.CollectionSpecimenID
  AND S.[CollectionSpecimenID] = CP.[CollectionSpecimenID]
  AND CP.[ProjectID] = [#project#].ProjectID()
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

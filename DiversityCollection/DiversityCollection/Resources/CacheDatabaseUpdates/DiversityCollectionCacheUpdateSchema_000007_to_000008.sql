

--#####################################################################################################################
--######   CacheIdentificationUnit  ###################################################################################
--#####################################################################################################################

if (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = 'CacheIdentificationUnit' AND C.COLUMN_NAME = 'RetrievalType') = 0
BEGIN
ALTER TABLE [#project#].[CacheIdentificationUnit] ADD RetrievalType nvarchar(50) NULL;
END
GO


--#####################################################################################################################
--######   procPublishIdentificationUnit  #############################################################################
--#####################################################################################################################

if(select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.COLUMN_NAME = 'RetrievalType' and C.TABLE_NAME = 'CacheIdentificationUnit' and C.TABLE_SCHEMA = '#project#') = 0
begin
	ALTER TABLE [#project#].CacheIdentificationUnit ADD RetrievalType nvarchar(50)
end


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
      ,[OnlyObserved]
	  ,[RetrievalType])
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
	  ,[RetrievalType]
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






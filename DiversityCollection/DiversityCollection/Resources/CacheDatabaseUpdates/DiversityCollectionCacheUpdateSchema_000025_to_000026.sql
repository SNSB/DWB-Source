
--#####################################################################################################################
--######   procPublishIdentificationUnitGeoAnalysis - inclusion of geography with reduced precision  ##################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishIdentificationUnitGeoAnalysis] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnitGeoAnalysis]
*/
truncate table [#project#].CacheIdentificationUnitGeoAnalysis

if (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID()) is null
begin
	-- insert data with full precision
	INSERT INTO [#project#].[CacheIdentificationUnitGeoAnalysis]
      ([CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[AnalysisDate]
      ,[Geography]
      ,[Geometry]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[Notes])
	SELECT DISTINCT G.[CollectionSpecimenID]
      ,G.[IdentificationUnitID]
      ,G.[AnalysisDate]
      ,G.[Geography].ToString()
      ,G.[Geometry].ToString()
      ,G.[ResponsibleName]
      ,G.[ResponsibleAgentURI]
      ,G.[Notes]
  FROM [#project#].[CacheIdentificationUnit] U, ' + dbo.SourceDatabase() + '.dbo.IdentificationUnitGeoAnalysis G
  where U.CollectionSpecimenID = G.CollectionSpecimenID
  and G.IdentificationUnitID = U.IdentificationUnitID
end
else
begin
	-- insert data with reduced precision

	declare @Precision int
	set @Precision = (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())

	INSERT INTO [#project#].[CacheIdentificationUnitGeoAnalysis]
      ([CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[AnalysisDate]
      ,[Geography]
      ,[Geometry]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[Notes])
	SELECT DISTINCT G.[CollectionSpecimenID]
      ,G.[IdentificationUnitID]
      ,G.[AnalysisDate]
      ,''POINT (''
      + cast(round(G.[Geography].EnvelopeCenter().Long, @Precision) as varchar) + '' ''
      + cast(round(G.[Geography].EnvelopeCenter().Lat, @Precision) as varchar) +'')''
      ,G.[Geometry].ToString()
      ,G.[ResponsibleName]
      ,G.[ResponsibleAgentURI]
      ,G.[Notes]
  FROM [#project#].[CacheIdentificationUnit] U, ' + dbo.SourceDatabase() + '.dbo.IdentificationUnitGeoAnalysis G
  where U.CollectionSpecimenID = G.CollectionSpecimenID
  and G.IdentificationUnitID = U.IdentificationUnitID
end')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   procPublishIdentificationUnit - optimzied  #################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishIdentificationUnit] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnit]
*/
truncate table [#project#].CacheIdentificationUnit

-- insert basic data
INSERT INTO [#project#].[CacheIdentificationUnit]
           ([CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[LastIdentificationCache]
      ,[TaxonomicGroup]
	  ,[DisplayOrder])
SELECT U.[CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,MAX([LastIdentificationCache])
      ,MAX(U.[TaxonomicGroup])
      ,MAX([DisplayOrder])
  FROM [' + dbo.SourceDatabase() + '].[dbo].[IdentificationUnit] U, 
  [' + dbo.SourceDatabase() + '].[dbo].[CollectionProject] CP, 
  [#project#].ProjectTaxonomicGroup T,
  [#project#].CacheCollectionSpecimen S
  where U.[CollectionSpecimenID] = CP.[CollectionSpecimenID]
  and U.[CollectionSpecimenID] = S.[CollectionSpecimenID]
  AND CP.[ProjectID] = [#project#].ProjectID()
  and T.TaxonomicGroup COLLATE DATABASE_DEFAULT = U.TaxonomicGroup COLLATE DATABASE_DEFAULT
  and  ((U.DataWithholdingReason IS NULL) OR (U.DataWithholdingReason = N''''))
  GROUP BY U.[CollectionSpecimenID],[IdentificationUnitID]')

set @SQL = (@SQL + '
-- setting the stable identifier
DECLARE @StableIdentifierBase nvarchar(255), @ProjectID int
set @ProjectID = (select [#project#].ProjectID())
  set @StableIdentifierBase = (select [' + dbo.SourceDatabase() + '].[dbo].StableIdentifier(@ProjectID, 0, NULL, NULL))
  set @StableIdentifierBase = (select substring(@StableIdentifierBase, 1, len(@StableIdentifierBase) - 1))
  select @StableIdentifierBase

UPDATE U SET U.StableIdentifier = @StableIdentifierBase + cast( U.CollectionSpecimenID as varchar) + ''/'' + cast( U.IdentificationUnitID as varchar)
  FROM [#project#].[CacheIdentificationUnit] U')

set @SQL = (@SQL + '
-- setting the still missing values
UPDATE CU SET
       CU.RelatedUnitID            = U.[RelatedUnitID]
      ,CU.[RelationType]           = U.[RelationType]
      ,CU.[ExsiccataNumber]        = U.[ExsiccataNumber]
      ,CU.[ColonisedSubstratePart] = U.[ColonisedSubstratePart]
      ,CU.[FamilyCache]            = U.[FamilyCache]
      ,CU.[OrderCache]             = U.[OrderCache]
      ,CU.[LifeStage]              = U.[LifeStage]
      ,CU.[Gender]                 = U.[Gender]
      ,CU.[HierarchyCache]         = U.[HierarchyCache]
      ,CU.[UnitIdentifier]         = U.[UnitIdentifier]
      ,CU.[UnitDescription]        = U.[UnitDescription]
      ,CU.[Circumstances]          = U.[Circumstances]
      ,CU.[Notes]                  = U.[Notes]
      ,CU.[NumberOfUnits]          = U.[NumberOfUnits]
      ,CU.[OnlyObserved]           = U.[OnlyObserved]
	  ,CU.[RetrievalType]          = U.[RetrievalType]
  FROM [' + dbo.SourceDatabase() + '].[dbo].[IdentificationUnit] U,
  [#project#].[CacheIdentificationUnit] CU
  WHERE U.CollectionSpecimenID = CU.CollectionSpecimenID
  AND U.IdentificationUnitID = CU.IdentificationUnitID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   CacheMetadata - RecordURI -> nvarchar(500) #################################################################
--#####################################################################################################################

ALTER TABLE [#project#].[CacheMetadata] ALTER COLUMN RecordURI nvarchar(500)
GO 

--#####################################################################################################################
--######   ProjectAnalysis for blocking analysis  #####################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectAnalysis' and T.TABLE_SCHEMA = '#project#') = 0
begin
	CREATE TABLE [#project#].[ProjectAnalysis](
		[AnalysisID] int NOT NULL,
	 CONSTRAINT [PK_ProjectAnalysis] PRIMARY KEY CLUSTERED 
	(
		[AnalysisID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the analysis that should be published' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'ProjectAnalysis', @level2type=N'COLUMN',@level2name=N'AnalysisID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Analysis that should be published' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'ProjectAnalysis'
end
GO 

GRANT SELECT ON [#project#].[ProjectAnalysis] TO [CacheUser]
GO
GRANT DELETE ON [#project#].[ProjectAnalysis] TO [CacheAdmin] 
GO
GRANT UPDATE ON [#project#].[ProjectAnalysis] TO [CacheAdmin]
GO
GRANT INSERT ON [#project#].[ProjectAnalysis] TO [CacheAdmin]
GO

--#####################################################################################################################
--######   Transfer existing data into ProjectAnalysis   ##############################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'INSERT INTO [#project#].ProjectAnalysis (AnalysisID) 
select distinct U.AnalysisID 
from ' +  dbo.SourceDatabase() + '.dbo.IdentificationUnitAnalysis U, ' +  dbo.SourceDatabase() + '.dbo.CollectionProject P 
where U.CollectionSpecimenID = P.CollectionSpecimenID 
and P.ProjectID = [#project#].ProjectID() 
and U.AnalysisID not in (SELECT AnalysisID FROM [#project#].ProjectAnalysis) 
ORDER BY U.AnalysisID ')
begin try
exec sp_executesql @SQL
end try
begin catch
exec sp_executesql @SQL
end catch
GO


--#####################################################################################################################
--######   procPublishIdentificationUnitAnalysis - blocking analysis  #################################################
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
  [#project#].CacheIdentificationUnit U,
  [#project#].ProjectAnalysis A
  where U.IdentificationUnitID = S.IdentificationUnitID
  and A.AnalysisID = S.AnalysisID
  and U.CollectionSpecimenID = S.CollectionSpecimenID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 7, 80000)
exec sp_executesql @SQL
end catch

GO

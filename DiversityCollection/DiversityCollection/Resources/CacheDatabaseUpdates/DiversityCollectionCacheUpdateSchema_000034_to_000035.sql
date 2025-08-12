
--#####################################################################################################################
--######   procPublishProjectAgent - JOIN with CacheMetadata  #########################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishProjectAgent] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProjectAgent]
*/
truncate table [#project#].CacheProjectAgent

INSERT INTO [#project#].[CacheProjectAgent]
           ([ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole]
      ,[AgentType]
      ,[Notes]
      ,[AgentSequence]
	  ,[ProjectAgentID])
SELECT DISTINCT A.[ProjectID]
      ,A.[AgentName]
      ,A.[AgentURI]
      ,A.[AgentRole]
      ,A.[AgentType]
      ,A.[Notes]
      ,A.[AgentSequence]
	  ,A.[ProjectAgentID]
  FROM ' + dbo.ProjectsDatabase() + '.DBO.ProjectAgent A
  INNER JOIN [#project#].[CacheMetadata] M ON A.ProjectID = M.ProjectID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   procPublishProjectAgentRole - JOIN with CacheMetadata  #####################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishProjectAgentRole] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProjectAgentRole]
*/
truncate table [#project#].CacheProjectAgentRole

INSERT INTO [#project#].[CacheProjectAgentRole]
           ([ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole]
	  ,[ProjectAgentID])
SELECT DISTINCT       
A.ProjectID, 
A.AgentName, 
A.AgentURI,
R.AgentRole,
R.[ProjectAgentID]
FROM  ' + dbo.ProjectsDatabase() + '.DBO.ProjectAgentRole AS R 
  INNER JOIN ' + dbo.ProjectsDatabase() + '.DBO.ProjectAgent AS A ON R.ProjectAgentID = A.ProjectAgentID
  INNER JOIN [#project#].[CacheMetadata] M ON A.ProjectID = M.ProjectID
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
--######   New table CacheProjectDescriptor  ##########################################################################
--#####################################################################################################################

if (
select count(*) from INFORMATION_SCHEMA.Tables t 
where t.TABLE_NAME = 'CacheProjectDescriptor'
and t.TABLE_SCHEMA = '#project#')  = 0
begin
CREATE TABLE [#project#].[CacheProjectDescriptor](
	[ID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Language] [varchar](5) NOT NULL,
	[Element] [nvarchar](80) NOT NULL,
	[Content] [nvarchar](255) NULL,
	[ContentURI] [varchar](500) NULL,
 CONSTRAINT [PK_CacheProjectDescriptor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
end
GO

GRANT SELECT ON [#project#].CacheProjectDescriptor TO [CacheAdmin] 
GO
GRANT INSERT ON [#project#].CacheProjectDescriptor TO [CacheAdmin] 
GO
GRANT UPDATE ON [#project#].CacheProjectDescriptor TO [CacheAdmin] 
GO
GRANT DELETE ON [#project#].CacheProjectDescriptor TO [CacheAdmin] 
GO


--#####################################################################################################################
--######   procPublishProjectDescriptor  ##############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishProjectDescriptor] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProjectDescriptor]
*/
truncate table [#project#].CacheProjectDescriptor

INSERT INTO [#project#].[CacheProjectDescriptor](
	[ID],
	[ProjectID],
	[Language],
	[Element],
	[Content],
	[ContentURI])
SELECT DISTINCT 	
    D.[ID],
	D.[ProjectID],
	D.[Language],
	E.[DisplayText],
	D.[Content],
	D.[ContentURI]
  FROM ' + dbo.ProjectsDatabase() + '.DBO.ProjectDescriptor D
  INNER JOIN ' + dbo.ProjectsDatabase() + '.DBO.ProjectDescriptorElement E ON D.ElementID = E.ElementID
  INNER JOIN [#project#].[CacheMetadata] M ON D.ProjectID = M.ProjectID
  ')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].procPublishCollectionProject TO [CacheAdmin] 
GO

--#####################################################################################################################
--######   procPublishCount - include CacheProjectDescriptor  #########################################################
--#####################################################################################################################

ALTER PROCEDURE [#project#].[procPublishCount] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCount]
-- CREATION
SELECT 'INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT '' + T.TABLE_NAME + '', COUNT(*) FROM [#project#].['  + T.TABLE_NAME + '];'  
FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_SCHEMA = '#project#' AND T.TABLE_TYPE = 'BASE TABLE' ORDER BY T.TABLE_NAME
*/
truncate table [#project#].[CacheCount]

INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheAnalysis', COUNT(*) FROM [#project#].[CacheAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheAnnotation', COUNT(*) FROM [#project#].[CacheAnnotation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollection', COUNT(*) FROM [#project#].[CacheCollection];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionAgent', COUNT(*) FROM [#project#].[CacheCollectionAgent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEvent', COUNT(*) FROM [#project#].[CacheCollectionEvent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEventLocalisation', COUNT(*) FROM [#project#].[CacheCollectionEventLocalisation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEventMethod', COUNT(*) FROM [#project#].[CacheCollectionEventMethod];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEventParameterValue', COUNT(*) FROM [#project#].[CacheCollectionEventParameterValue];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEventProperty', COUNT(*) FROM [#project#].[CacheCollectionEventProperty];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionExternalDatasource', COUNT(*) FROM [#project#].[CacheCollectionExternalDatasource];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimen', COUNT(*) FROM [#project#].[CacheCollectionSpecimen];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionProject', COUNT(*) FROM [#project#].[CacheCollectionProject];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenImage', COUNT(*) FROM [#project#].[CacheCollectionSpecimenImage];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenPart', COUNT(*) FROM [#project#].[CacheCollectionSpecimenPart];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenProcessing', COUNT(*) FROM [#project#].[CacheCollectionSpecimenProcessing];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenReference', COUNT(*) FROM [#project#].[CacheCollectionSpecimenReference];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenRelation', COUNT(*) FROM [#project#].[CacheCollectionSpecimenRelation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheExternalIdentifier', COUNT(*) FROM [#project#].[CacheExternalIdentifier];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentification', COUNT(*) FROM [#project#].[CacheIdentification];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentificationUnit', COUNT(*) FROM [#project#].[CacheIdentificationUnit];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentificationUnitAnalysis', COUNT(*) FROM [#project#].[CacheIdentificationUnitAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentificationUnitGeoAnalysis', COUNT(*) FROM [#project#].[CacheIdentificationUnitGeoAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentificationUnitInPart', COUNT(*) FROM [#project#].[CacheIdentificationUnitInPart];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheLocalisationSystem', COUNT(*) FROM [#project#].[CacheLocalisationSystem];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheMetadata', COUNT(*) FROM [#project#].[CacheMetadata];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheMethod', COUNT(*) FROM [#project#].[CacheMethod];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheProcessing', COUNT(*) FROM [#project#].[CacheProcessing];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheProjectAgent', COUNT(*) FROM [#project#].[CacheProjectAgent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheProjectReference', COUNT(*) FROM [#project#].[CacheProjectReference];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheProjectDescriptor', COUNT(*) FROM [#project#].[CacheProjectDescriptor];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'ProjectMaterialCategory', COUNT(*) FROM [#project#].[ProjectMaterialCategory];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'ProjectTaxonomicGroup', COUNT(*) FROM [#project#].[ProjectTaxonomicGroup];

INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCount', COUNT(*) FROM [#project#].[CacheCount];

GO



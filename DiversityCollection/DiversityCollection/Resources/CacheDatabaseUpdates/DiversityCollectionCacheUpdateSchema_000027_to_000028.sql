
--#####################################################################################################################
--######   CacheMethod  ###############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CacheMethod' and T.TABLE_SCHEMA = '#project#') = 0
begin
	CREATE TABLE [#project#].[CacheMethod](
	[MethodID] [int] NOT NULL,
	[MethodParentID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[MethodURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheMethod] PRIMARY KEY CLUSTERED 
(
	[MethodID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO 

GRANT SELECT ON [#project#].[CacheMethod] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheMethod] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheMethod] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheMethod] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   procPublishMethod - transfer in new table for Method  ######################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishMethod] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishMethod]
*/
truncate table [#project#].CacheMethod

INSERT INTO [#project#].[CacheMethod]
   ([MethodID] ,
	[MethodParentID],
	[DisplayText],
	[Description],
	[MethodURI],
	[Notes]
)
SELECT [MethodID] ,
	[MethodParentID],
	[DisplayText],
	[Description],
	[MethodURI],
	[Notes]
  FROM ' + dbo.SourceDatabase() + '.DBO.Method M')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXECUTE ON [#project#].[procPublishMethod] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   CacheCollectionEventMethod  ################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CacheCollectionEventMethod' and T.TABLE_SCHEMA = '#project#') = 0
begin
	CREATE TABLE [#project#].[CacheCollectionEventMethod](
	[CollectionEventID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[MethodMarker] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CacheCollectionEventMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[MethodID] ASC,
	[MethodMarker] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO 

GRANT SELECT ON [#project#].[CacheCollectionEventMethod] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheCollectionEventMethod] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheCollectionEventMethod] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheCollectionEventMethod] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   procPublishCollectionEventMethod - transfer in new table for CollectionEventMethod  ########################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishCollectionEventMethod] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEventMethod]
*/
truncate table [#project#].CacheCollectionEventMethod

INSERT INTO [#project#].[CacheCollectionEventMethod]
      ([CollectionEventID]
      ,[MethodID]
      ,[MethodMarker])
SELECT [CollectionEventID]
      ,[MethodID]
      ,[MethodMarker]
  FROM ' + dbo.SourceDatabase() + '.DBO.CollectionEventMethod M')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXECUTE ON [#project#].[procPublishCollectionEventMethod] TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   CollectionEventParameterValue  #############################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CacheCollectionEventParameterValue' and T.TABLE_SCHEMA = '#project#') = 0
begin
	CREATE TABLE [#project#].[CacheCollectionEventParameterValue](
	[CollectionEventID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[MethodMarker] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheCollectionEventParameterValue] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[MethodID] ASC,
	[ParameterID] ASC,
	[MethodMarker] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO 

GRANT SELECT ON [#project#].[CacheCollectionEventParameterValue] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheCollectionEventParameterValue] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheCollectionEventParameterValue] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheCollectionEventParameterValue] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   procPublishCollectionEventParameterValue - transfer in new table for CollectionEventParameterValue  ########
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishCollectionEventParameterValue] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEventParameterValue]
*/
truncate table [#project#].CacheCollectionEventParameterValue

INSERT INTO [#project#].[CacheCollectionEventParameterValue]
      ([CollectionEventID]
      ,[MethodID]
      ,[ParameterID]
      ,[Value]
      ,[MethodMarker]
      ,[Notes])
SELECT [CollectionEventID]
      ,[MethodID]
      ,[ParameterID]
      ,[Value]
      ,[MethodMarker]
      ,[Notes]
  FROM ' + dbo.SourceDatabase() + '.DBO.CollectionEventParameterValue M')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXECUTE ON [#project#].[procPublishCollectionEventParameterValue] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   CacheCount - table for holding the total counts for all tables  ############################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CacheCount' and T.TABLE_SCHEMA = '#project#') = 0
begin
	CREATE TABLE [#project#].[CacheCount](
	[Tablename] [nvarchar] (200) NOT NULL,
	[TotalCount] [int] NULL,
 CONSTRAINT [PK_CacheCount] PRIMARY KEY CLUSTERED 
(
	[Tablename] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO 

GRANT SELECT ON [#project#].[CacheCount] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheCount] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheCount] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheCount] TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   procPublishCount - fill table CacheCount with values according to the total numbers  #######################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishCount] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCount]
-- CREATION
SELECT ''INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT '''' + T.TABLE_NAME + '''', COUNT(*) FROM [#project#].[''  + T.TABLE_NAME + ''];''  
FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_SCHEMA = ''#project#'' AND T.TABLE_TYPE = ''BASE TABLE'' ORDER BY T.TABLE_NAME
*/
truncate table [#project#].[CacheCount]

INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheAnalysis'', COUNT(*) FROM [#project#].[CacheAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheAnnotation'', COUNT(*) FROM [#project#].[CacheAnnotation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollection'', COUNT(*) FROM [#project#].[CacheCollection];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionAgent'', COUNT(*) FROM [#project#].[CacheCollectionAgent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionEvent'', COUNT(*) FROM [#project#].[CacheCollectionEvent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionEventLocalisation'', COUNT(*) FROM [#project#].[CacheCollectionEventLocalisation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionEventMethod'', COUNT(*) FROM [#project#].[CacheCollectionEventMethod];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionEventParameterValue'', COUNT(*) FROM [#project#].[CacheCollectionEventParameterValue];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionEventProperty'', COUNT(*) FROM [#project#].[CacheCollectionEventProperty];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionExternalDatasource'', COUNT(*) FROM [#project#].[CacheCollectionExternalDatasource];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionSpecimen'', COUNT(*) FROM [#project#].[CacheCollectionSpecimen];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionSpecimenImage'', COUNT(*) FROM [#project#].[CacheCollectionSpecimenImage];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionSpecimenPart'', COUNT(*) FROM [#project#].[CacheCollectionSpecimenPart];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionSpecimenProcessing'', COUNT(*) FROM [#project#].[CacheCollectionSpecimenProcessing];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionSpecimenReference'', COUNT(*) FROM [#project#].[CacheCollectionSpecimenReference];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCollectionSpecimenRelation'', COUNT(*) FROM [#project#].[CacheCollectionSpecimenRelation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheExternalIdentifier'', COUNT(*) FROM [#project#].[CacheExternalIdentifier];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheIdentification'', COUNT(*) FROM [#project#].[CacheIdentification];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheIdentificationUnit'', COUNT(*) FROM [#project#].[CacheIdentificationUnit];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheIdentificationUnitAnalysis'', COUNT(*) FROM [#project#].[CacheIdentificationUnitAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheIdentificationUnitGeoAnalysis'', COUNT(*) FROM [#project#].[CacheIdentificationUnitGeoAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheIdentificationUnitInPart'', COUNT(*) FROM [#project#].[CacheIdentificationUnitInPart];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheLocalisationSystem'', COUNT(*) FROM [#project#].[CacheLocalisationSystem];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheMetadata'', COUNT(*) FROM [#project#].[CacheMetadata];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheMethod'', COUNT(*) FROM [#project#].[CacheMethod];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheProcessing'', COUNT(*) FROM [#project#].[CacheProcessing];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheProjectAgent'', COUNT(*) FROM [#project#].[CacheProjectAgent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheProjectReference'', COUNT(*) FROM [#project#].[CacheProjectReference];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''ProjectMaterialCategory'', COUNT(*) FROM [#project#].[ProjectMaterialCategory];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''ProjectTaxonomicGroup'', COUNT(*) FROM [#project#].[ProjectTaxonomicGroup];

INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT ''CacheCount'', COUNT(*) FROM [#project#].[CacheCount];

')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXECUTE ON [#project#].[procPublishCount] TO [CacheAdmin_#project#]
GO

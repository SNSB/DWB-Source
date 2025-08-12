



--#####################################################################################################################
--######   ExternalDatasourceID in CacheCollectionSpecimen  ###########################################################
--#####################################################################################################################

if (
select count(*) from INFORMATION_SCHEMA.COLUMNS C 
where C.Column_Name = 'ExternalDatasourceID'
and C.TABLE_NAME = 'CacheCollectionSpecimen'
and C.TABLE_SCHEMA = '#project#')  = 0
begin
ALTER TABLE [#project#].CacheCollectionSpecimen ADD ExternalDatasourceID int
end
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
      ,[ReferenceURI]
	  ,[ExternalDatasourceID])
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
	  ,[ExternalDatasourceID]
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


--#####################################################################################################################
--######   CacheCollectionExternalDatasource  #########################################################################
--#####################################################################################################################

if (
select count(*) from INFORMATION_SCHEMA.Tables t 
where t.TABLE_NAME = 'CacheCollectionExternalDatasource'
and t.TABLE_SCHEMA = '#project#')  = 0
begin
	CREATE TABLE [#project#].[CacheCollectionExternalDatasource](
		[ExternalDatasourceID] [int] NOT NULL,
		[ExternalDatasourceName] [nvarchar](255) NULL,
		[ExternalDatasourceVersion] [nvarchar](255) NULL,
		[Rights] [nvarchar](500) NULL,
		[ExternalDatasourceAuthors] [nvarchar](200) NULL,
		[ExternalDatasourceURI] [nvarchar](300) NULL,
		[ExternalDatasourceInstitution] [nvarchar](300) NULL,
	 CONSTRAINT [PK_CollectionExternalDatasource] PRIMARY KEY CLUSTERED 
	(
		[ExternalDatasourceID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO



--#####################################################################################################################
--######   procPublishCollectionExternalDatasource  ###################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishCollectionExternalDatasource] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionExternalDatasource]
*/
truncate table [#project#].CacheCollectionExternalDatasource

INSERT INTO [#project#].[CacheCollectionExternalDatasource]
      ([ExternalDatasourceID]
      ,[ExternalDatasourceName]
      ,[ExternalDatasourceVersion]
      ,[Rights]
      ,[ExternalDatasourceAuthors]
      ,[ExternalDatasourceURI]
      ,[ExternalDatasourceInstitution])
SELECT DISTINCT 
	E.ExternalDatasourceID, 
	E.ExternalDatasourceName, 
	E.ExternalDatasourceVersion, 
	E.Rights, 
	E.ExternalDatasourceAuthors, 
	E.ExternalDatasourceURI, 
	E.ExternalDatasourceInstitution
 FROM ' +  dbo.SourceDatabase() + '.dbo.CollectionExternalDatasource E, 
  [#project#].CacheCollectionSpecimen S
  WHERE E.ExternalDatasourceID = S.ExternalDatasourceID 
  AND ((E.Disabled IS NULL) OR (E.Disabled = 0))')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


GRANT EXEC ON [#project#].[procPublishCollectionExternalDatasource] TO [CacheAdmin]
GO



--#####################################################################################################################
--######   Issue #451  ################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   Adding column IdentificationSequence to Table CacheCollectionSpecimenReference   ###########################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CacheCollectionSpecimenReference' AND C.COLUMN_NAME = 'IdentificationSequence' and C.TABLE_SCHEMA = '#project#') = 0
begin
    ALTER TABLE [#project#].[CacheCollectionSpecimenReference] ADD [IdentificationSequence] [int] NULL;
end;

--#####################################################################################################################
--######   Adding column IdentificationSequence to procedure procPublishCollectionSpecimenReference   #################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishCollectionSpecimenReference] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenReference]
*/
truncate table [#project#].CacheCollectionSpecimenReference

INSERT INTO [#project#].[CacheCollectionSpecimenReference]
           ([CollectionSpecimenID]
           ,[ReferenceID]
           ,[ReferenceTitle]
           ,[ReferenceURI]
           ,[IdentificationUnitID]
           ,[SpecimenPartID]
           ,[ReferenceDetails]
           ,[Notes]
           ,[ResponsibleName]
           ,[ResponsibleAgentURI]
           ,[IdentificationSequence])
SELECT R.[CollectionSpecimenID]
      ,R.[ReferenceID]
      ,R.[ReferenceTitle]
      ,R.[ReferenceURI]
      ,R.[IdentificationUnitID]
      ,R.[SpecimenPartID]
      ,R.[ReferenceDetails]
      ,R.[Notes]
      ,R.[ResponsibleName]
      ,R.[ResponsibleAgentURI]
      ,R.[IdentificationSequence]
  FROM '  +  dbo.SourceDatabase()  + '.dbo.CollectionSpecimenReference R
  INNER JOIN [#project#].CacheCollectionSpecimen S
  ON R.CollectionSpecimenID = S.CollectionSpecimenID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

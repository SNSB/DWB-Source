
--#####################################################################################################################
--######   Issue #87  #################################################################################################
--#####################################################################################################################
--######   procPublishCollectionSpecimenProcessingMethodParameter - Restricted to filled values  ######################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionSpecimenProcessingMethodParameter] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenProcessingMethodParameter] 
*/
truncate table  [#project#].[CacheCollectionSpecimenProcessingMethodParameter]
-- insert Data
INSERT INTO [#project#].[CacheCollectionSpecimenProcessingMethodParameter] (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, MethodMarker, ParameterID, Value) 
  SELECT P.CollectionSpecimenID, P.SpecimenProcessingID, P.ProcessingID, P.MethodID, P.MethodMarker, P.ParameterID, P.Value 
  FROM '  +  dbo.SourceDatabase()  + '.DBO.[CollectionSpecimenProcessingMethodParameter] P
  INNER JOIN [#project#].[CacheCollectionSpecimenProcessingMethod] M
  on M.SpecimenProcessingID = P.SpecimenProcessingID
  and P.CollectionSpecimenID = M.CollectionSpecimenID
  and P.MethodID = M.MethodID
  and P.MethodMarker COLLATE DATABASE_DEFAULT = M.MethodMarker COLLATE DATABASE_DEFAULT
  and P.ProcessingID = M.ProcessingID
  and P.Value <> ''''')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



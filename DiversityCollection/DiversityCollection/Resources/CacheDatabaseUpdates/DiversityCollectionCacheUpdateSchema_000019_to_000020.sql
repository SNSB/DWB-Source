



--#####################################################################################################################
--######   procPublishCollectionEvent     #############################################################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionEvent] 
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
      ,CASE WHEN DataWithholdingReasonDate <> '''' THEN NULL ELSE [CollectionDate] END AS CollectionDate
      ,CASE WHEN DataWithholdingReasonDate <> '''' THEN NULL ELSE [CollectionDay] END AS CollectionDay
      ,CASE WHEN DataWithholdingReasonDate <> '''' THEN NULL ELSE [CollectionMonth] END AS CollectionMonth
      ,CASE WHEN DataWithholdingReasonDate <> '''' THEN NULL ELSE [CollectionYear] END AS CollectionYear
      ,CASE WHEN DataWithholdingReasonDate <> '''' THEN NULL ELSE [CollectionDateSupplement] END AS CollectionDateSupplement
      ,CASE WHEN DataWithholdingReasonDate <> '''' THEN NULL ELSE [CollectionTime] END AS CollectionTime
      ,CASE WHEN DataWithholdingReasonDate <> '''' THEN NULL ELSE [CollectionTimeSpan] END AS CollectionTimeSpan
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
  FROM ' +  dbo.SourceDatabase() + '.dbo.[CollectionEvent] E,
[#project#].CacheCollectionSpecimen S
  where E.CollectionEventID = S.CollectionEventID 
  AND ((E.DataWithholdingReason = '''') OR (E.DataWithholdingReason IS NULL))')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



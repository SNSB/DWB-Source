
--#####################################################################################################################
--######   procPublishIdentification  #################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER PROCEDURE [#project#].[procPublishIdentification] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentification]
*/
truncate table [#project#].CacheIdentification

INSERT INTO [#project#].[CacheIdentification]
           ([CollectionSpecimenID]
           ,[IdentificationUnitID]
           ,[IdentificationSequence]
           ,[IdentificationDay]
           ,[IdentificationMonth]
           ,[IdentificationYear]
           ,[IdentificationDateSupplement]
           ,[VernacularTerm]
           ,[TaxonomicName]
           ,[NameURI]
           ,[IdentificationCategory]
           ,[IdentificationQualifier]
           ,[TypeStatus]
           ,[TypeNotes]
           ,[ReferenceTitle]
           ,[ReferenceDetails]
           ,[Notes]
           ,[ResponsibleName]
		   ,[NameID]
		   ,[BaseURL])
SELECT DISTINCT S.[CollectionSpecimenID]
           ,S.[IdentificationUnitID]
           ,[IdentificationSequence]
           ,[IdentificationDay]
           ,[IdentificationMonth]
           ,[IdentificationYear]
           ,[IdentificationDateSupplement]
           ,[VernacularTerm]
           ,[TaxonomicName]
           ,[NameURI]
           ,[IdentificationCategory]
           ,[IdentificationQualifier]
           ,[TypeStatus]
           ,[TypeNotes]
           ,[ReferenceTitle]
           ,[ReferenceDetails]
           ,S.[Notes]
           ,[ResponsibleName]
 		   ,CASE WHEN NameURI LIKE ''%/%'' 
		   THEN case when isnumeric(REVERSE(SUBSTRING(REVERSE(NameURI), 1, CHARINDEX(''/'', REVERSE(NameURI))-1))) = 1
				then REVERSE(SUBSTRING(REVERSE(NameURI), 1, CHARINDEX(''/'', REVERSE(NameURI))-1)) ELSE NULL end else null END AS NameID
		   ,CASE WHEN NameURI LIKE ''%/%'' THEN REVERSE(SUBSTRING(REVERSE(NameURI), CHARINDEX(''/'', REVERSE(NameURI)), 255)) ELSE NULL END AS BaseURL
 FROM ' +  dbo.SourceDatabase() + '.dbo.Identification S, 
  [#project#].CacheIdentificationUnit U
  where U.IdentificationUnitID = S.IdentificationUnitID
  and U.CollectionSpecimenID = S.CollectionSpecimenID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO




--#####################################################################################################################
--######   procPublishCollectionEventLocalisation  ####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionEventLocalisation] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEventLocalisation]
*/
truncate table [#project#].CacheCollectionEventLocalisation

INSERT INTO [#project#].[CacheCollectionEventLocalisation]
           ([CollectionEventID]
      ,[LocalisationSystemID]
      ,[Location1]
      ,[Location2]
      ,[LocationAccuracy]
      ,[LocationNotes]
      ,[DeterminationDate]
      ,[DistanceToLocation]
      ,[DirectionToLocation]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AverageAltitudeCache]
      ,[AverageLatitudeCache]
      ,[AverageLongitudeCache]
      ,[RecordingMethod]
	  ,[Geography])
SELECT DISTINCT L.[CollectionEventID]
      ,L.[LocalisationSystemID]
      ,[Location1]
      ,[Location2]
      ,[LocationAccuracy]
      ,[LocationNotes]
      ,[DeterminationDate]
      ,[DistanceToLocation]
      ,[DirectionToLocation]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AverageAltitudeCache]
      ,[AverageLatitudeCache]
      ,[AverageLongitudeCache]
      ,[RecordingMethod]
	  ,[Geography].ToString()
  FROM ' +  dbo.SourceDatabase() + '.[dbo].[CollectionEventLocalisation] L,
[#project#].[CacheCollectionEvent] E
  where E.CollectionEventID = L.CollectionEventID 

if not (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())  is null
begin
	declare @Precision int
	set @Precision = (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())

	-- rounding average values for all entries
	update L 
	set [AverageLatitudeCache] = round(L.[AverageLatitudeCache], @Precision ),
	[AverageLongitudeCache] = round([AverageLongitudeCache], @Precision)
	from [#project#].[CacheCollectionEventLocalisation] L

	-- rounding location values for coordinate entries
	update E 
	set [Location2] = round(E.[AverageLatitudeCache], @Precision ),
	[Location1] = round(E.[AverageLongitudeCache], @Precision)
	from [#project#].[CacheCollectionEventLocalisation] E,
	[#project#].[CacheLocalisationSystem] L
	WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
	AND L.ParsingMethodName = ''Coordinates''

	-- rounding location values for Gauss Krueger entries
	declare @GK int
	if @Precision < 5
	begin
		set @GK = 5 - @Precision
		update E 
		set [Location2] = substring([Location2], 1, 7 - @GK) + replicate(''0'', @GK)  ,
		[Location1] = substring([Location1], 1, 7 - @GK) + replicate(''0'', @GK)  
		from [#project#].[CacheCollectionEventLocalisation] E,
		[#project#].[CacheLocalisationSystem] L
		WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
		AND L.ParsingMethodName = ''GK''
	end
end
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch



GO


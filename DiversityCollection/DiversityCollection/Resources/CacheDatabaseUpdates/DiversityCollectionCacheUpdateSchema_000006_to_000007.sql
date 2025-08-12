

--#####################################################################################################################
--######   procPublishCollectionEventLocalisation  ####################################################################
--#####################################################################################################################


ALTER  PROCEDURE [#project#].[procPublishCollectionEventLocalisation] 
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
      ,[RecordingMethod])
SELECT DISTINCT E.[CollectionEventID]
      ,E.[LocalisationSystemID]
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
  FROM [dbo].[ViewCollectionEventLocalisation] E,
dbo.ViewCollectionSpecimen S, 
dbo.ViewCollectionProject P,
[#project#].CacheLocalisationSystem L
  where E.CollectionEventID = S.CollectionEventID 
  AND S.CollectionSpecimenID = p.CollectionSpecimenID
  AND L.[LocalisationSystemID] = E.[LocalisationSystemID]
  and p.ProjectID = [#project#].ProjectID()

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
	AND L.ParsingMethodName = 'Coordinates'

	-- rounding location values for Gauss Krueger entries
	declare @GK int
	if @Precision < 5
	begin
		set @GK = 5 - @Precision
		update E 
		set [Location2] = substring([Location2], 1, 7 - @GK) + replicate('0', @GK)  ,
		[Location1] = substring([Location1], 1, 7 - @GK) + replicate('0', @GK)  
		from [#project#].[CacheCollectionEventLocalisation] E,
		[#project#].[CacheLocalisationSystem] L
		WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
		AND L.ParsingMethodName = 'GK'
	end
end

GO


--#####################################################################################################################
--######   remove old ProjectLocalisationSystem    ####################################################################
--#####################################################################################################################
if (Select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectLocalisationSystem' and T.TABLE_SCHEMA = '#project#') = 1
begin
	DROP TABLE [#project#].[ProjectLocalisationSystem]
end
GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.41'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   change datetype of CollectorsSequence To enable import  ####################################################
--#####################################################################################################################

-- Remove objects that relate to the column CollectorsSequence
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CollectionAgent]') AND name = N'IX_CollectionAgentSequence')
DROP INDEX [IX_CollectionAgentSequence] ON [dbo].[CollectionAgent] WITH ( ONLINE = OFF )
GO

IF EXISTS (SELECT name FROM sys.objects
         WHERE name = 'DF_CollectionAgent2_CollectorsSequence' 
            AND type = 'D')
   BEGIN 
      EXEC sp_unbindefault 'CollectionAgent.CollectorsSequence.DF_CollectionAgent2_CollectorsSequence'
	  ALTER TABLE CollectionAgent DROP CONSTRAINT DF_CollectionAgent2_CollectorsSequence
   END
GO


IF EXISTS (SELECT name FROM sys.objects
         WHERE name = 'DF_CollectionAgent_CollectorsSequence' 
            AND type = 'D')
   BEGIN 
      EXEC sp_unbindefault 'CollectionAgent.CollectorsSequence.DF_CollectionAgent_CollectorsSequence'
	  ALTER TABLE CollectionAgent DROP CONSTRAINT DF_CollectionAgent_CollectorsSequence
   END
GO


ALTER TABLE dbo.CollectionAgent ALTER COLUMN CollectorsSequence [datetime] NULL
GO

-- Change datatype
ALTER TABLE dbo.CollectionAgent ALTER COLUMN CollectorsSequence [datetime2](7) NULL
GO

ALTER TABLE dbo.CollectionAgent_log ALTER COLUMN CollectorsSequence [datetime2](7) NULL
GO

-- add removed objects
ALTER TABLE [dbo].[CollectionAgent] ADD  CONSTRAINT [DF_CollectionAgent_CollectorsSequence]  DEFAULT (SYSDATETIME()) FOR [CollectorsSequence]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_CollectionAgentSequence] ON [dbo].[CollectionAgent] 
(
	[CollectionSpecimenID] ASC,
	[CollectorsSequence] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

--#####################################################################################################################
--######   Grant VIEW DEFINITION on all tables to enable Import   #####################################################################################
--#####################################################################################################################

/*
select 'GRANT VIEW DEFINITION ON [' + t.TABLE_NAME + '] TO Editor', t.TABLE_NAME from INFORMATION_SCHEMA.TABLES
t where t.TABLE_NAME not like 'xx%'
and t.TABLE_TYPE = 'BASE TABLE'
and t.TABLE_NAME not like '%[_]Log%'
and t.TABLE_NAME not like '%[_]enum'
and t.TABLE_NAME not in ('dtproperties', 'sysdiagrams', 'ApplicationSearchSelectionStrings')
order by t.TABLE_NAME
*/

GRANT VIEW DEFINITION ON [Analysis] TO Editor
GRANT VIEW DEFINITION ON [AnalysisResult] TO Editor
GRANT VIEW DEFINITION ON [AnalysisTaxonomicGroup] TO Editor
GRANT VIEW DEFINITION ON [Annotation] TO Editor
GRANT VIEW DEFINITION ON [Collection] TO Editor
GRANT VIEW DEFINITION ON [CollectionAgent] TO Editor
GRANT VIEW DEFINITION ON [CollectionEvent] TO Editor
GRANT VIEW DEFINITION ON [CollectionEventImage] TO Editor
GRANT VIEW DEFINITION ON [CollectionEventLocalisation] TO Editor
GRANT VIEW DEFINITION ON [CollectionEventMethod] TO Editor
GRANT VIEW DEFINITION ON [CollectionEventParameterValue] TO Editor
GRANT VIEW DEFINITION ON [CollectionEventProperty] TO Editor
GRANT VIEW DEFINITION ON [CollectionEventSeries] TO Editor
GRANT VIEW DEFINITION ON [CollectionEventSeriesImage] TO Editor
GRANT VIEW DEFINITION ON [CollectionExternalDatasource] TO Editor
GRANT VIEW DEFINITION ON [CollectionImage] TO Editor
GRANT VIEW DEFINITION ON [CollectionManager] TO Editor
GRANT VIEW DEFINITION ON [CollectionProject] TO Editor
GRANT VIEW DEFINITION ON [CollectionRequester] TO Editor
GRANT VIEW DEFINITION ON [CollectionSpecimen] TO Editor
GRANT VIEW DEFINITION ON [CollectionSpecimenImage] TO Editor
GRANT VIEW DEFINITION ON [CollectionSpecimenPart] TO Editor
GRANT VIEW DEFINITION ON [CollectionSpecimenProcessing] TO Editor
--GRANT VIEW DEFINITION ON [CollectionSpecimenProcessingMethod] TO Editor
--GRANT VIEW DEFINITION ON [CollectionSpecimenProcessingMethodParameter] TO Editor
GRANT VIEW DEFINITION ON [CollectionSpecimenRelation] TO Editor
GRANT VIEW DEFINITION ON [CollectionSpecimenTransaction] TO Editor
GRANT VIEW DEFINITION ON [CollectionUser] TO Editor
GRANT VIEW DEFINITION ON [Entity] TO Editor
GRANT VIEW DEFINITION ON [EntityRepresentation] TO Editor
GRANT VIEW DEFINITION ON [EntityUsage] TO Editor
GRANT VIEW DEFINITION ON [ExternalRequestCredentials ] TO Editor
GRANT VIEW DEFINITION ON [Identification] TO Editor
GRANT VIEW DEFINITION ON [IdentificationUnit] TO Editor
GRANT VIEW DEFINITION ON [IdentificationUnitAnalysis] TO Editor
GRANT VIEW DEFINITION ON [IdentificationUnitGeoAnalysis] TO Editor
GRANT VIEW DEFINITION ON [IdentificationUnitInPart] TO Editor
GRANT VIEW DEFINITION ON [LocalisationSystem] TO Editor
GRANT VIEW DEFINITION ON [Method] TO Editor
GRANT VIEW DEFINITION ON [MethodForAnalysis] TO Editor
GRANT VIEW DEFINITION ON [MethodForProcessing] TO Editor
GRANT VIEW DEFINITION ON [Parameter] TO Editor
GRANT VIEW DEFINITION ON [Processing] TO Editor
GRANT VIEW DEFINITION ON [ProcessingMaterialCategory] TO Editor
GRANT VIEW DEFINITION ON [ProjectAnalysis] TO Editor
GRANT VIEW DEFINITION ON [ProjectProcessing] TO Editor
GRANT VIEW DEFINITION ON [ProjectProxy] TO Editor
GRANT VIEW DEFINITION ON [ProjectUser] TO Editor
GRANT VIEW DEFINITION ON [Property] TO Editor
GRANT VIEW DEFINITION ON [ReplicationPublisher] TO Editor
GRANT VIEW DEFINITION ON [Tool] TO Editor
GRANT VIEW DEFINITION ON [ToolForAnalysis] TO Editor
GRANT VIEW DEFINITION ON [ToolForProcessing] TO Editor
GRANT VIEW DEFINITION ON [Transaction] TO Editor
GRANT VIEW DEFINITION ON [TransactionDocument] TO Editor
GRANT VIEW DEFINITION ON [UserProxy] TO Editor
GO

--#####################################################################################################################
--######   trgUpdCollectionEventLocalisation   ######################################################################################
--######   Eintrag der Cache werte falls nur Geographie geliefert wird   ######################################################################################
--#####################################################################################################################



ALTER TRIGGER [dbo].[trgUpdCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR UPDATE AS
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
set @i = (select count(*) from deleted) 
if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM deleted)
   EXECUTE procSetVersionCollectionEvent @ID
   SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)
END 
if (not @Version is null) 
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, 
LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, 
ResponsibleAgentURI, RecordingMethod, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion,  LogState) 
SELECT D.CollectionEventID, D.LocalisationSystemID, D.Location1, D.Location2, D.LocationAccuracy, D.LocationNotes, 
D.DeterminationDate, D.DistanceToLocation, D.DirectionToLocation, D.ResponsibleName, D.ResponsibleAgentURI, D.RecordingMethod, 
D.Geography, D.AverageAltitudeCache, D.AverageLatitudeCache, D.AverageLongitudeCache, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, 
D.LogUpdatedWhen, D.LogUpdatedBy,  @Version,  'D'
FROM DELETED D, INSERTED I
WHERE D.CollectionEventID = I.CollectionEventID AND I.LocalisationSystemID = D.LocalisationSystemID
AND (
(I.Location1 <> D.Location1 OR I.Location1 IS NULL AND NOT D.Location1 IS NULL OR NOT I.Location1 IS NULL AND D.Location1 IS NULL)
OR (I.Location2 <> D.Location2 OR I.Location2 IS NULL AND NOT D.Location2 IS NULL OR NOT I.Location2 IS NULL AND D.Location2 IS NULL)
OR (I.LocationAccuracy <> D.LocationAccuracy OR I.LocationAccuracy IS NULL AND NOT D.LocationAccuracy IS NULL OR NOT I.LocationAccuracy IS NULL AND D.LocationAccuracy IS NULL)
OR (I.LocationNotes <> D.LocationNotes OR I.LocationNotes IS NULL AND NOT D.LocationNotes IS NULL OR NOT I.LocationNotes IS NULL AND D.LocationNotes IS NULL)
OR (I.DeterminationDate <> D.DeterminationDate OR I.DeterminationDate IS NULL AND NOT D.DeterminationDate IS NULL OR NOT I.DeterminationDate IS NULL AND D.DeterminationDate IS NULL)
OR (I.DistanceToLocation <> D.DistanceToLocation OR I.DistanceToLocation IS NULL AND NOT D.DistanceToLocation IS NULL OR NOT I.DistanceToLocation IS NULL AND D.DistanceToLocation IS NULL)
OR (I.DirectionToLocation <> D.DirectionToLocation OR I.DirectionToLocation IS NULL AND NOT D.DirectionToLocation IS NULL OR NOT I.DirectionToLocation IS NULL AND D.DirectionToLocation IS NULL)
OR (I.ResponsibleName <> D.ResponsibleName OR I.ResponsibleName IS NULL AND NOT D.ResponsibleName IS NULL OR NOT I.ResponsibleName IS NULL AND D.ResponsibleName IS NULL)
OR (I.ResponsibleAgentURI <> D.ResponsibleAgentURI OR I.ResponsibleAgentURI IS NULL AND NOT D.ResponsibleAgentURI IS NULL OR NOT I.ResponsibleAgentURI IS NULL AND D.ResponsibleAgentURI IS NULL)
OR (I.RecordingMethod <> D.RecordingMethod OR I.RecordingMethod IS NULL AND NOT D.RecordingMethod IS NULL OR NOT I.RecordingMethod IS NULL AND D.RecordingMethod IS NULL)
OR (I.Geography.ToString() <> D.Geography.ToString() OR I.Geography IS NULL AND NOT D.Geography IS NULL OR NOT I.Geography IS NULL AND D.Geography IS NULL)
OR (I.AverageAltitudeCache <> D.AverageAltitudeCache OR I.AverageAltitudeCache IS NULL AND NOT D.AverageAltitudeCache IS NULL OR NOT I.AverageAltitudeCache IS NULL AND D.AverageAltitudeCache IS NULL)
OR (I.AverageLatitudeCache <> D.AverageLatitudeCache OR I.AverageLatitudeCache IS NULL AND NOT D.AverageLatitudeCache IS NULL OR NOT I.AverageLatitudeCache IS NULL AND D.AverageLatitudeCache IS NULL)
OR (I.AverageLongitudeCache <> D.AverageLongitudeCache OR I.AverageLongitudeCache IS NULL AND NOT D.AverageLongitudeCache IS NULL OR NOT I.AverageLongitudeCache IS NULL AND D.AverageLongitudeCache IS NULL)
)
end
else
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, 
DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, RecordingMethod, 
Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion, LogState) 
SELECT D.CollectionEventID, D.LocalisationSystemID, D.Location1, D.Location2, D.LocationAccuracy, D.LocationNotes, 
D.DeterminationDate, D.DistanceToLocation, D.DirectionToLocation, D.ResponsibleName, D.ResponsibleAgentURI, D.RecordingMethod, 
D.Geography, D.AverageAltitudeCache, D.AverageLatitudeCache, D.AverageLongitudeCache, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, 
D.LogUpdatedWhen, D.LogUpdatedBy, E.Version, 'D' 
FROM DELETED D, CollectionEvent E
WHERE D.CollectionEventID = E.CollectionEventID
end

-- set logging columns
Update CollectionEventLocalisation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventLocalisation, deleted 
where 1 = 1 
AND CollectionEventLocalisation.CollectionEventID = deleted.CollectionEventID
AND CollectionEventLocalisation.LocalisationSystemID = deleted.LocalisationSystemID

-- set geography if missing
Update CollectionEventLocalisation
set Geography = 
case when inserted.Geography IS null then 
	geography::STPointFromText('POINT(' + replace(cast(inserted.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(inserted.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
else inserted.Geography end
FROM CollectionEventLocalisation, inserted 
where 1 = 1 
AND CollectionEventLocalisation.CollectionEventID = inserted.CollectionEventID
AND CollectionEventLocalisation.LocalisationSystemID = inserted.LocalisationSystemID
AND inserted.AverageLatitudeCache between -90 and 90
AND inserted.AverageLongitudeCache between -180 and 180
AND (CollectionEventLocalisation.Geography.ToString() <> inserted.Geography.ToString()
OR CollectionEventLocalisation.Geography IS NULL)

-- set average cache values if missing
Update CollectionEventLocalisation
set AverageLatitudeCache = inserted.Geography.EnvelopeCenter().Lat,
AverageLongitudeCache = inserted.Geography.EnvelopeCenter().Long
FROM CollectionEventLocalisation, inserted 
where 1 = 1 
AND CollectionEventLocalisation.CollectionEventID = inserted.CollectionEventID
AND CollectionEventLocalisation.LocalisationSystemID = inserted.LocalisationSystemID
AND inserted.AverageLatitudeCache IS NULL
AND inserted.AverageLongitudeCache IS NULL
AND CollectionEventLocalisation.AverageLatitudeCache IS NULL
AND CollectionEventLocalisation.AverageLongitudeCache IS NULL
AND NOT inserted.Geography IS NULL

GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.42'
END

GO



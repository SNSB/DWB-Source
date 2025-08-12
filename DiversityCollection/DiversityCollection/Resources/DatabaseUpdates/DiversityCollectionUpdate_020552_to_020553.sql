
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.52'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Loeschrechte fuer CollectionManager  ######################################################################################
--#####################################################################################################################

grant delete on [Transaction] to CollectionManager
go


--#####################################################################################################################
--######   Removal of sorting of Transaction types, return to alphabetic order  ######################################################################################
--#####################################################################################################################

UPDATE E set E.[DisplayOrder] = NULL FROM [dbo].[CollTransactionType_Enum] E
GO


--#####################################################################################################################
--######  Change in update trigger of CollectionEventLocalisation:  Save geography in any case in a point  ######################################################################################
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

Update L
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventLocalisation L, deleted D
where 1 = 1 
AND L.CollectionEventID = D.CollectionEventID
AND L.LocalisationSystemID = D.LocalisationSystemID

Update L
set Geography = 
case when I.Geography IS null then 
	geography::STPointFromText('POINT(' + replace(cast(I.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(I.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
else I.Geography end
FROM CollectionEventLocalisation L, inserted I
where 1 = 1 
AND L.CollectionEventID = I.CollectionEventID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 180
AND (L.Geography.ToString() <> I.Geography.ToString()
OR L.Geography IS NULL)

Update L
set AverageLatitudeCache = I.Geography.EnvelopeCenter().Lat,
AverageLongitudeCache = I.Geography.EnvelopeCenter().Long
FROM CollectionEventLocalisation L, inserted I
where 1 = 1 
AND L.CollectionEventID = I.CollectionEventID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache IS NULL
AND I.AverageLongitudeCache IS NULL
AND L.AverageLatitudeCache IS NULL
AND L.AverageLongitudeCache IS NULL
AND NOT I.Geography IS NULL


Update L
set Geography = geography::STPointFromText('POINT(' + replace(cast(I.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(I.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
FROM CollectionEventLocalisation L, inserted I
where 1 = 1 
AND L.CollectionEventID = I.CollectionEventID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 360
AND (L.Geography.ToString() LIKE 'POINT%')
AND @i = 1

GO

--#####################################################################################################################
--######   Beschreibung von MaterialDescription  korrigieren ######################################################################################
--#####################################################################################################################


EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Description of the material of this transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Transaction', @level2type=N'COLUMN',@level2name=N'MaterialDescription'
GO



--#####################################################################################################################
--######   Name von ExternalRequestCredentials  korrigieren ######################################################################################
--#####################################################################################################################


sp_rename 'ExternalRequestCredentials ', 'ExternalRequestCredentials'
GO


--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.06' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.53'
END

GO



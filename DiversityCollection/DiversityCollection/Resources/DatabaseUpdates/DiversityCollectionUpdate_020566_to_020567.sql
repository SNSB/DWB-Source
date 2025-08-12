
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.66'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ssp. -> subsp.                       #######################################################################
--#####################################################################################################################

UPDATE E SET
--SELECT [Code],
	  [Code] = replace(Code, 'ssp.', 'subsp.')
      ,[DisplayText] = replace([DisplayText], 'ssp.', 'subsp.')
  FROM [CollIdentificationQualifier_Enum] E
  WHERE Code like '% ssp.%'


UPDATE I SET
--SELECT [TaxonomicName]
	  [TaxonomicName] = replace([TaxonomicName], ' ssp. ', ' subsp. ')
  FROM [dbo].[Identification]
I where I.NameURI LIKE  '%/TaxonNames%' 
and I.TaxonomicName like '% ssp. %'


--#####################################################################################################################
--######   Cnidaria                     ###############################################################################
--#####################################################################################################################

UPDATE T SET T.[ParentCode] = 'evertebrate' FROM [dbo].[CollTaxonomicGroup_Enum] T WHERE T.[Code] = 'cnidaria'
GO

--#####################################################################################################################
--######   trgUpdCollectionEvent             ##########################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdCollectionEvent] ON [dbo].[CollectionEvent]  
 FOR UPDATE AS 
 /*  Created by DiversityWorkbench Administration.  */  
 /*  DiversityWorkbenchMaintenance  2.0.0.3 */  
 /*  Date: 30.08.2007  */  
 /*  Changed: 01.12.2015 - sysuser instead of user */
 if not update(Version)  
 BEGIN  /* setting the version in the main table */  
	 DECLARE @i int  
	 DECLARE @ID int 
	 DECLARE @Version int  
	 set @i = (select count(*) from deleted)   
	 if @i = 1  
	 BEGIN     
		 SET  @ID = (SELECT CollectionEventID FROM deleted)    
		 EXECUTE procSetVersionCollectionEvent @ID 
	 END   
	 Update CollectionEvent set CollectionDate = case when isdate (cast (	cast( CollectionEvent.CollectionDay as varchar ) + '.' +  	
	 cast( CollectionEvent.CollectionMonth as varchar )  + '.' +  	
	 cast( CollectionEvent.CollectionYear as varchar ) as varchar) ) = 1 then cast ( 	
	 cast( CollectionEvent.CollectionDay as varchar ) + '.' +  	
	 cast( CollectionEvent.CollectionMonth as varchar )  + '.' +  	
	 cast( CollectionEvent.CollectionYear as varchar ) as datetime)  
	 else null end 
	 FROM CollectionEvent, deleted  
	 where 1 = 1  AND CollectionEvent.CollectionEventID = deleted.CollectionEventID   
	 
	 /* saving the original dataset in the logging table */  
	 INSERT INTO CollectionEvent_Log (
	 CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, 
	 CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionDateCategory, CollectionTime, 
	 CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, ReferenceDetails, 
	 CollectingMethod, Notes, CountryCache, DataWithholdingReason, RowGUID,  LogCreatedWhen, 
	 LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, LogState, LocalityVerbatim, CollectionEndDay, CollectionEndMonth, CollectionEndYear, DataWithholdingReasonDate)  
	 SELECT D.CollectionEventID, D.Version, D.SeriesID, D.CollectorsEventNumber, D.CollectionDate, 
	 D.CollectionDay, D.CollectionMonth, D.CollectionYear, D.CollectionDateSupplement, D.CollectionDateCategory, D.CollectionTime, 
	 D.CollectionTimeSpan, D.LocalityDescription, D.HabitatDescription, D.ReferenceTitle, D.ReferenceURI, D.ReferenceDetails, 
	 D.CollectingMethod, D.Notes, D.CountryCache, D.DataWithholdingReason, D.RowGUID,  D.LogCreatedWhen, 
	 D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, 'U', D.LocalityVerbatim, D.CollectionEndDay, D.CollectionEndMonth, D.CollectionEndYear, D.DataWithholdingReasonDate
	 FROM DELETED  D
 END  
 
 Update CollectionEvent set LogUpdatedWhen = getdate(), LogUpdatedBy =  suser_sname()
 FROM CollectionEvent, deleted  
 where 1 = 1  AND CollectionEvent.CollectionEventID = deleted.CollectionEventID

 GO


--#####################################################################################################################
--######   trgUpdCollectionEventLocalisation      ###############################################################################
--#####################################################################################################################

 
ALTER TRIGGER [dbo].[trgUpdCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR UPDATE AS
 /*  Changed: 01.12.2015 - sysuser instead of user and DoUpdate for check if there are real changes */
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
DECLARE @DoUpdate int
set @DoUpdate = 0

SET @DoUpdate = (SELECT count(*)
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
))

set @i = (select count(*) from deleted) 
if @i = 1 AND @DoUpdate > 0
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

if (@DoUpdate > 0)
begin

	Update L
	set LogUpdatedWhen = getdate(), LogUpdatedBy = suser_sname()
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
end

GO


--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.00' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.67'
END

GO



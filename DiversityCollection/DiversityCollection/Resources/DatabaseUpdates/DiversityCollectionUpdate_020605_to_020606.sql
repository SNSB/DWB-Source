declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.05'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--#####################################################################################################################
--######  Delete trigger - ensure transfer in logtables for cascading deletes   #######################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  trgDelCollectionAgent     ###################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionAgent] ON [dbo].[CollectionAgent] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionAgent_Log (CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.CollectorsName, deleted.CollectorsAgentURI, deleted.CollectorsSequence, deleted.CollectorsNumber, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionAgent_Log (CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.CollectorsName, deleted.CollectorsAgentURI, deleted.CollectorsSequence, deleted.CollectorsNumber, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionAgent_Log (CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.CollectorsName, deleted.CollectorsAgentURI, deleted.CollectorsSequence, deleted.CollectorsNumber, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, -1, 'D' 
FROM DELETED
end
end
GO

--#####################################################################################################################
--######  trgDelCollectionEventImage    ###############################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionEventImage] ON [dbo].[CollectionEventImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
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

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, CopyrightStatement, CreatorAgent, CreatorAgentURI, DataWithholdingReason, Description, ImageType, InternalNotes, IPR, LicenseHolder, LicenseHolderAgentURI, LicenseType, LicenseYear, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResourceURI, RowGUID, Title, URI,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.CopyrightStatement, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.DataWithholdingReason, deleted.Description, deleted.ImageType, deleted.InternalNotes, deleted.IPR, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseType, deleted.LicenseYear, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResourceURI, deleted.RowGUID, deleted.Title, deleted.URI,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionEvent WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID) > 0 
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, CopyrightStatement, CreatorAgent, CreatorAgentURI, DataWithholdingReason, Description, ImageType, InternalNotes, IPR, LicenseHolder, LicenseHolderAgentURI, LicenseType, LicenseYear, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResourceURI, RowGUID, Title, URI,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.CopyrightStatement, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.DataWithholdingReason, deleted.Description, deleted.ImageType, deleted.InternalNotes, deleted.IPR, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseType, deleted.LicenseYear, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResourceURI, deleted.RowGUID, deleted.Title, deleted.URI, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
else
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, CopyrightStatement, CreatorAgent, CreatorAgentURI, DataWithholdingReason, Description, ImageType, InternalNotes, IPR, LicenseHolder, LicenseHolderAgentURI, LicenseType, LicenseYear, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResourceURI, RowGUID, Title, URI,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.CopyrightStatement, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.DataWithholdingReason, deleted.Description, deleted.ImageType, deleted.InternalNotes, deleted.IPR, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseType, deleted.LicenseYear, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResourceURI, deleted.RowGUID, deleted.Title, deleted.URI, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######  trgDelCollectionEventLocalisation    ########################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
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

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventLocalisation_Log (AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, CollectionEventID, DeterminationDate, DirectionToLocation, DistanceToLocation, Geography, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, RecordingMethod, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion,  LogState) 
SELECT deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.CollectionEventID, deleted.DeterminationDate, deleted.DirectionToLocation, deleted.DistanceToLocation, deleted.Geography, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RecordingMethod, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionEvent WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID) > 0 
begin
INSERT INTO CollectionEventLocalisation_Log (AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, CollectionEventID, DeterminationDate, DirectionToLocation, DistanceToLocation, Geography, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, RecordingMethod, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion, LogState) 
SELECT deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.CollectionEventID, deleted.DeterminationDate, deleted.DirectionToLocation, deleted.DistanceToLocation, deleted.Geography, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RecordingMethod, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
else
begin
INSERT INTO CollectionEventLocalisation_Log (AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, CollectionEventID, DeterminationDate, DirectionToLocation, DistanceToLocation, Geography, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, RecordingMethod, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion, LogState) 
SELECT deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.CollectionEventID, deleted.DeterminationDate, deleted.DirectionToLocation, deleted.DistanceToLocation, deleted.Geography, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RecordingMethod, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######  trgDelCollectionEventMethod    ##############################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionEventMethod] ON [dbo].[CollectionEventMethod] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
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

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionEvent WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID) > 0 
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.RowGUID, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
else
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.RowGUID, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######  trgDelCollectionEventProperty    ############################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelCollectionEventProperty] ON [dbo].[CollectionEventProperty] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
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

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventProperty_Log (AverageValueCache, CollectionEventID, DisplayText, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PropertyHierarchyCache, PropertyID, PropertyURI, PropertyValue, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion,  LogState) 
SELECT deleted.AverageValueCache, deleted.CollectionEventID, deleted.DisplayText, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.PropertyHierarchyCache, deleted.PropertyID, deleted.PropertyURI, deleted.PropertyValue, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionEvent WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID) > 0 
begin
INSERT INTO CollectionEventProperty_Log (AverageValueCache, CollectionEventID, DisplayText, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PropertyHierarchyCache, PropertyID, PropertyURI, PropertyValue, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion, LogState) 
SELECT deleted.AverageValueCache, deleted.CollectionEventID, deleted.DisplayText, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.PropertyHierarchyCache, deleted.PropertyID, deleted.PropertyURI, deleted.PropertyValue, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
else
begin
INSERT INTO CollectionEventProperty_Log (AverageValueCache, CollectionEventID, DisplayText, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PropertyHierarchyCache, PropertyID, PropertyURI, PropertyValue, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion, LogState) 
SELECT deleted.AverageValueCache, deleted.CollectionEventID, deleted.DisplayText, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.PropertyHierarchyCache, deleted.PropertyID, deleted.PropertyURI, deleted.PropertyValue, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, -1, 'D' 
FROM DELETED
end
end
GO

--#####################################################################################################################
--######  trgDelCollectionSpecimenImage    ############################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.Description, deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.DisplayOrder, deleted.LicenseNotes, deleted.LicenseURI,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.Description, deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.DisplayOrder, deleted.LicenseNotes, deleted.LicenseURI, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.Description, deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.DisplayOrder, deleted.LicenseNotes, deleted.LicenseURI, -1, 'D' 
FROM DELETED
end
end
GO


--#####################################################################################################################
--######  trgDelCollectionSpecimenPart    #############################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenPart] ON [dbo].[CollectionSpecimenPart] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenPart_Log (AccessionNumber, CollectionID, CollectionSpecimenID, DataWithholdingReason, DerivedFromSpecimenPartID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MaterialCategory, Notes, PartSublabel, PreparationDate, PreparationMethod, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, Stock, StockUnit, StorageContainer, StorageLocation,  LogVersion,  LogState) 
SELECT deleted.AccessionNumber, deleted.CollectionID, deleted.CollectionSpecimenID, deleted.DataWithholdingReason, deleted.DerivedFromSpecimenPartID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MaterialCategory, deleted.Notes, deleted.PartSublabel, deleted.PreparationDate, deleted.PreparationMethod, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.Stock, deleted.StockUnit, deleted.StorageContainer, deleted.StorageLocation,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenPart_Log (AccessionNumber, CollectionID, CollectionSpecimenID, DataWithholdingReason, DerivedFromSpecimenPartID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MaterialCategory, Notes, PartSublabel, PreparationDate, PreparationMethod, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, Stock, StockUnit, StorageContainer, StorageLocation,  LogVersion, LogState) 
SELECT deleted.AccessionNumber, deleted.CollectionID, deleted.CollectionSpecimenID, deleted.DataWithholdingReason, deleted.DerivedFromSpecimenPartID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MaterialCategory, deleted.Notes, deleted.PartSublabel, deleted.PreparationDate, deleted.PreparationMethod, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.Stock, deleted.StockUnit, deleted.StorageContainer, deleted.StorageLocation, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (AccessionNumber, CollectionID, CollectionSpecimenID, DataWithholdingReason, DerivedFromSpecimenPartID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MaterialCategory, Notes, PartSublabel, PreparationDate, PreparationMethod, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, Stock, StockUnit, StorageContainer, StorageLocation,  LogVersion, LogState) 
SELECT deleted.AccessionNumber, deleted.CollectionID, deleted.CollectionSpecimenID, deleted.DataWithholdingReason, deleted.DerivedFromSpecimenPartID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MaterialCategory, deleted.Notes, deleted.PartSublabel, deleted.PreparationDate, deleted.PreparationMethod, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.Stock, deleted.StockUnit, deleted.StorageContainer, deleted.StorageLocation, -1, 'D' 
FROM DELETED
end
end
GO


--#####################################################################################################################
--######  trgDelCollectionSpecimenPartDescription   ###################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenPartDescription] ON [dbo].[CollectionSpecimenPartDescription] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Description, deleted.DescriptionTermURI, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.PartDescriptionID, deleted.RowGUID, deleted.SpecimenPartID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Description, deleted.DescriptionTermURI, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.PartDescriptionID, deleted.RowGUID, deleted.SpecimenPartID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Description, deleted.DescriptionTermURI, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.PartDescriptionID, deleted.RowGUID, deleted.SpecimenPartID, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######  trgDelCollectionSpecimenProcessing  #########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenProcessing] ON [dbo].[CollectionSpecimenProcessing] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ProcessingDate, ProcessingDuration, ProcessingID, Protocoll, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, SpecimenProcessingID, ToolUsage,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ProcessingDate, deleted.ProcessingDuration, deleted.ProcessingID, deleted.Protocoll, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.SpecimenProcessingID, deleted.ToolUsage,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ProcessingDate, ProcessingDuration, ProcessingID, Protocoll, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, SpecimenProcessingID, ToolUsage,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ProcessingDate, deleted.ProcessingDuration, deleted.ProcessingID, deleted.Protocoll, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.SpecimenProcessingID, deleted.ToolUsage, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ProcessingDate, ProcessingDuration, ProcessingID, Protocoll, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, SpecimenProcessingID, ToolUsage,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ProcessingDate, deleted.ProcessingDuration, deleted.ProcessingID, deleted.Protocoll, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.SpecimenProcessingID, deleted.ToolUsage, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######  trgDelCollectionSpecimenProcessingMethod  ###################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenProcessingMethod] ON [dbo].[CollectionSpecimenProcessingMethod] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ProcessingID, RowGUID, SpecimenProcessingID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ProcessingID, deleted.RowGUID, deleted.SpecimenProcessingID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ProcessingID, RowGUID, SpecimenProcessingID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ProcessingID, deleted.RowGUID, deleted.SpecimenProcessingID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ProcessingID, RowGUID, SpecimenProcessingID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ProcessingID, deleted.RowGUID, deleted.SpecimenProcessingID, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######  trgDelCollectionSpecimenProcessingMethodParameter  ##########################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenProcessingMethodParameter] ON [dbo].[CollectionSpecimenProcessingMethodParameter] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ParameterID, ProcessingID, RowGUID, SpecimenProcessingID, Value,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ParameterID, deleted.ProcessingID, deleted.RowGUID, deleted.SpecimenProcessingID, deleted.Value,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ParameterID, ProcessingID, RowGUID, SpecimenProcessingID, Value,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ParameterID, deleted.ProcessingID, deleted.RowGUID, deleted.SpecimenProcessingID, deleted.Value, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ParameterID, ProcessingID, RowGUID, SpecimenProcessingID, Value,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ParameterID, deleted.ProcessingID, deleted.RowGUID, deleted.SpecimenProcessingID, deleted.Value, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######  trgDelCollectionSpecimenReference    ########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenReference] ON [dbo].[CollectionSpecimenReference] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ReferenceDetails, ReferenceID, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ReferenceDetails, deleted.ReferenceID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ReferenceDetails, ReferenceID, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ReferenceDetails, deleted.ReferenceID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ReferenceDetails, ReferenceID, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ReferenceDetails, deleted.ReferenceID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######  trgDelCollectionSpecimenRelation    #########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenRelation] ON [dbo].[CollectionSpecimenRelation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, IdentificationUnitID, IsInternalRelationCache, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RelatedSpecimenCollectionID, RelatedSpecimenDescription, RelatedSpecimenDisplayText, RelatedSpecimenURI, RelationType, RowGUID, SpecimenPartID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IsInternalRelationCache, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.RelatedSpecimenDisplayText, deleted.RelatedSpecimenURI, deleted.RelationType, deleted.RowGUID, deleted.SpecimenPartID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, IdentificationUnitID, IsInternalRelationCache, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RelatedSpecimenCollectionID, RelatedSpecimenDescription, RelatedSpecimenDisplayText, RelatedSpecimenURI, RelationType, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IsInternalRelationCache, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.RelatedSpecimenDisplayText, deleted.RelatedSpecimenURI, deleted.RelationType, deleted.RowGUID, deleted.SpecimenPartID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, IdentificationUnitID, IsInternalRelationCache, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RelatedSpecimenCollectionID, RelatedSpecimenDescription, RelatedSpecimenDisplayText, RelatedSpecimenURI, RelationType, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IsInternalRelationCache, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.RelatedSpecimenDisplayText, deleted.RelatedSpecimenURI, deleted.RelationType, deleted.RowGUID, deleted.SpecimenPartID, -1, 'D' 
FROM DELETED
end
end
GO


--#####################################################################################################################
--######   trgDelIdentification    ####################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelIdentification] ON [dbo].[Identification] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationCategory, IdentificationDate, IdentificationDateCategory, IdentificationDateSupplement, IdentificationDay, IdentificationMonth, IdentificationQualifier, IdentificationSequence, IdentificationUnitID, IdentificationYear, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, NameURI, Notes, ReferenceDetails, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, TaxonomicName, TermUri, TypeNotes, TypeStatus, VernacularTerm,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationCategory, deleted.IdentificationDate, deleted.IdentificationDateCategory, deleted.IdentificationDateSupplement, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationQualifier, deleted.IdentificationSequence, deleted.IdentificationUnitID, deleted.IdentificationYear, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.NameURI, deleted.Notes, deleted.ReferenceDetails, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.TaxonomicName, deleted.TermUri, deleted.TypeNotes, deleted.TypeStatus, deleted.VernacularTerm,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationCategory, IdentificationDate, IdentificationDateCategory, IdentificationDateSupplement, IdentificationDay, IdentificationMonth, IdentificationQualifier, IdentificationSequence, IdentificationUnitID, IdentificationYear, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, NameURI, Notes, ReferenceDetails, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, TaxonomicName, TermUri, TypeNotes, TypeStatus, VernacularTerm,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationCategory, deleted.IdentificationDate, deleted.IdentificationDateCategory, deleted.IdentificationDateSupplement, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationQualifier, deleted.IdentificationSequence, deleted.IdentificationUnitID, deleted.IdentificationYear, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.NameURI, deleted.Notes, deleted.ReferenceDetails, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.TaxonomicName, deleted.TermUri, deleted.TypeNotes, deleted.TypeStatus, deleted.VernacularTerm, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationCategory, IdentificationDate, IdentificationDateCategory, IdentificationDateSupplement, IdentificationDay, IdentificationMonth, IdentificationQualifier, IdentificationSequence, IdentificationUnitID, IdentificationYear, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, NameURI, Notes, ReferenceDetails, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, TaxonomicName, TermUri, TypeNotes, TypeStatus, VernacularTerm,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationCategory, deleted.IdentificationDate, deleted.IdentificationDateCategory, deleted.IdentificationDateSupplement, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationQualifier, deleted.IdentificationSequence, deleted.IdentificationUnitID, deleted.IdentificationYear, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.NameURI, deleted.Notes, deleted.ReferenceDetails, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.TaxonomicName, deleted.TermUri, deleted.TypeNotes, deleted.TypeStatus, deleted.VernacularTerm, -1, 'D' 
FROM DELETED
end
end


/* setting the LastIdentificationCache if the deleted dataset was the last identification*/
update IdentificationUnit
set LastIdentificationCache = 
case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end 
from IdentificationUnit, Identification a, deleted
where IdentificationUnit.CollectionSpecimenID = deleted.CollectionSpecimenID
and IdentificationUnit.IdentificationUnitID = deleted.IdentificationUnitID
and a.CollectionSpecimenID = deleted.CollectionSpecimenID
and a.IdentificationUnitID = deleted.IdentificationUnitID
and a.IdentificationSequence = 
(select max(b.IdentificationSequence) 
from Identification b
where b.CollectionSpecimenID = a.CollectionSpecimenID
and b.IdentificationUnitID = a.IdentificationUnitID
group by b.IdentificationUnitID, b.CollectionSpecimenID)
and LastIdentificationCache <> case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end

GO


--#####################################################################################################################
--######   trgDelIdentificationUnit    ################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelIdentificationUnit] ON [dbo].[IdentificationUnit] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnit_Log (Circumstances, CollectionSpecimenID, ColonisedSubstratePart, DataWithholdingReason, DisplayOrder, ExsiccataIdentification, ExsiccataNumber, FamilyCache, Gender, HierarchyCache, IdentificationUnitID, LastIdentificationCache, LifeStage, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, NumberOfUnits, NumberOfUnitsModifier, OnlyObserved, OrderCache, ParentUnitID, RelatedUnitID, RelationType, RowGUID, TaxonomicGroup, UnitDescription, UnitIdentifier,  LogVersion,  LogState) 
SELECT deleted.Circumstances, deleted.CollectionSpecimenID, deleted.ColonisedSubstratePart, deleted.DataWithholdingReason, deleted.DisplayOrder, deleted.ExsiccataIdentification, deleted.ExsiccataNumber, deleted.FamilyCache, deleted.Gender, deleted.HierarchyCache, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.LifeStage, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.NumberOfUnits, deleted.NumberOfUnitsModifier, deleted.OnlyObserved, deleted.OrderCache, deleted.ParentUnitID, deleted.RelatedUnitID, deleted.RelationType, deleted.RowGUID, deleted.TaxonomicGroup, deleted.UnitDescription, deleted.UnitIdentifier,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO IdentificationUnit_Log (Circumstances, CollectionSpecimenID, ColonisedSubstratePart, DataWithholdingReason, DisplayOrder, ExsiccataIdentification, ExsiccataNumber, FamilyCache, Gender, HierarchyCache, IdentificationUnitID, LastIdentificationCache, LifeStage, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, NumberOfUnits, NumberOfUnitsModifier, OnlyObserved, OrderCache, ParentUnitID, RelatedUnitID, RelationType, RowGUID, TaxonomicGroup, UnitDescription, UnitIdentifier,  LogVersion, LogState) 
SELECT deleted.Circumstances, deleted.CollectionSpecimenID, deleted.ColonisedSubstratePart, deleted.DataWithholdingReason, deleted.DisplayOrder, deleted.ExsiccataIdentification, deleted.ExsiccataNumber, deleted.FamilyCache, deleted.Gender, deleted.HierarchyCache, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.LifeStage, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.NumberOfUnits, deleted.NumberOfUnitsModifier, deleted.OnlyObserved, deleted.OrderCache, deleted.ParentUnitID, deleted.RelatedUnitID, deleted.RelationType, deleted.RowGUID, deleted.TaxonomicGroup, deleted.UnitDescription, deleted.UnitIdentifier, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO IdentificationUnit_Log (Circumstances, CollectionSpecimenID, ColonisedSubstratePart, DataWithholdingReason, DisplayOrder, ExsiccataIdentification, ExsiccataNumber, FamilyCache, Gender, HierarchyCache, IdentificationUnitID, LastIdentificationCache, LifeStage, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, NumberOfUnits, NumberOfUnitsModifier, OnlyObserved, OrderCache, ParentUnitID, RelatedUnitID, RelationType, RowGUID, TaxonomicGroup, UnitDescription, UnitIdentifier,  LogVersion, LogState) 
SELECT deleted.Circumstances, deleted.CollectionSpecimenID, deleted.ColonisedSubstratePart, deleted.DataWithholdingReason, deleted.DisplayOrder, deleted.ExsiccataIdentification, deleted.ExsiccataNumber, deleted.FamilyCache, deleted.Gender, deleted.HierarchyCache, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.LifeStage, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.NumberOfUnits, deleted.NumberOfUnitsModifier, deleted.OnlyObserved, deleted.OrderCache, deleted.ParentUnitID, deleted.RelatedUnitID, deleted.RelationType, deleted.RowGUID, deleted.TaxonomicGroup, deleted.UnitDescription, deleted.UnitIdentifier, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######   trgDelIdentificationUnitAnalysis    ########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelIdentificationUnitAnalysis] ON [dbo].[IdentificationUnitAnalysis] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnitAnalysis_Log (AnalysisDate, AnalysisID, AnalysisNumber, AnalysisResult, CollectionSpecimenID, ExternalAnalysisURI, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, ToolUsage,  LogVersion,  LogState) 
SELECT deleted.AnalysisDate, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.CollectionSpecimenID, deleted.ExternalAnalysisURI, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.ToolUsage,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO IdentificationUnitAnalysis_Log (AnalysisDate, AnalysisID, AnalysisNumber, AnalysisResult, CollectionSpecimenID, ExternalAnalysisURI, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, ToolUsage,  LogVersion, LogState) 
SELECT deleted.AnalysisDate, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.CollectionSpecimenID, deleted.ExternalAnalysisURI, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.ToolUsage, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO IdentificationUnitAnalysis_Log (AnalysisDate, AnalysisID, AnalysisNumber, AnalysisResult, CollectionSpecimenID, ExternalAnalysisURI, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID, ToolUsage,  LogVersion, LogState) 
SELECT deleted.AnalysisDate, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.CollectionSpecimenID, deleted.ExternalAnalysisURI, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, deleted.SpecimenPartID, deleted.ToolUsage, -1, 'D' 
FROM DELETED
end
end
GO



--#####################################################################################################################
--######   trgDelIdentificationUnitAnalysisMethod    ##################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelIdentificationUnitAnalysisMethod] ON [dbo].[IdentificationUnitAnalysisMethod] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnitAnalysisMethod_Log (AnalysisID, AnalysisNumber, CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, RowGUID,  LogVersion,  LogState) 
SELECT deleted.AnalysisID, deleted.AnalysisNumber, deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO IdentificationUnitAnalysisMethod_Log (AnalysisID, AnalysisNumber, CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, RowGUID,  LogVersion, LogState) 
SELECT deleted.AnalysisID, deleted.AnalysisNumber, deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethod_Log (AnalysisID, AnalysisNumber, CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, RowGUID,  LogVersion, LogState) 
SELECT deleted.AnalysisID, deleted.AnalysisNumber, deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.RowGUID, -1, 'D' 
FROM DELETED
end
end
GO


--#####################################################################################################################
--######   trgDelIdentificationUnitAnalysisMethodParameter    #########################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelIdentificationUnitAnalysisMethodParameter] ON [dbo].[IdentificationUnitAnalysisMethodParameter] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (AnalysisID, AnalysisNumber, CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ParameterID, RowGUID, Value,  LogVersion,  LogState) 
SELECT deleted.AnalysisID, deleted.AnalysisNumber, deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ParameterID, deleted.RowGUID, deleted.Value,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (AnalysisID, AnalysisNumber, CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ParameterID, RowGUID, Value,  LogVersion, LogState) 
SELECT deleted.AnalysisID, deleted.AnalysisNumber, deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ParameterID, deleted.RowGUID, deleted.Value, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (AnalysisID, AnalysisNumber, CollectionSpecimenID, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, MethodID, MethodMarker, ParameterID, RowGUID, Value,  LogVersion, LogState) 
SELECT deleted.AnalysisID, deleted.AnalysisNumber, deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.MethodID, deleted.MethodMarker, deleted.ParameterID, deleted.RowGUID, deleted.Value, -1, 'D' 
FROM DELETED
end
end
GO


--#####################################################################################################################
--######   trgDelIdentificationUnitGeoAnalysis    #####################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelIdentificationUnitGeoAnalysis] ON [dbo].[IdentificationUnitGeoAnalysis] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnitGeoAnalysis_Log (AnalysisDate, CollectionSpecimenID, Geography, Geometry, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion,  LogState) 
SELECT deleted.AnalysisDate, deleted.CollectionSpecimenID, deleted.Geography, deleted.Geometry, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO IdentificationUnitGeoAnalysis_Log (AnalysisDate, CollectionSpecimenID, Geography, Geometry, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion, LogState) 
SELECT deleted.AnalysisDate, deleted.CollectionSpecimenID, deleted.Geography, deleted.Geometry, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO IdentificationUnitGeoAnalysis_Log (AnalysisDate, CollectionSpecimenID, Geography, Geometry, IdentificationUnitID, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ResponsibleAgentURI, ResponsibleName, RowGUID,  LogVersion, LogState) 
SELECT deleted.AnalysisDate, deleted.CollectionSpecimenID, deleted.Geography, deleted.Geometry, deleted.IdentificationUnitID, deleted.LogCreatedBy, deleted.LogCreatedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.Notes, deleted.ResponsibleAgentURI, deleted.ResponsibleName, deleted.RowGUID, -1, 'D' 
FROM DELETED
end
end
GO

--#####################################################################################################################
--######   trgDelIdentificationUnitInPart    ##########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelIdentificationUnitInPart] ON [dbo].[IdentificationUnitInPart] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnitInPart_Log (CollectionSpecimenID, Description, DisplayOrder, IdentificationUnitID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, SpecimenPartID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Description, deleted.DisplayOrder, deleted.IdentificationUnitID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID, deleted.SpecimenPartID,  @Version,  'D'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, CollectionSpecimen WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO IdentificationUnitInPart_Log (CollectionSpecimenID, Description, DisplayOrder, IdentificationUnitID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Description, deleted.DisplayOrder, deleted.IdentificationUnitID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID, deleted.SpecimenPartID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO IdentificationUnitInPart_Log (CollectionSpecimenID, Description, DisplayOrder, IdentificationUnitID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Description, deleted.DisplayOrder, deleted.IdentificationUnitID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID, deleted.SpecimenPartID, -1, 'D' 
FROM DELETED
end
end
GO




--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.08' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.06'
END

GO


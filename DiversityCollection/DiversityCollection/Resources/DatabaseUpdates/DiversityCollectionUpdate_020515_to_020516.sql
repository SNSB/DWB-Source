--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

declare @Version varchar(10)
set @Version = (SELECT [dbo].[Version]())
IF (SELECT [dbo].[Version]()) <> '02.05.12'
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version 02.05.14. Current version = ' + @Version
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Description fuer Images   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO



ALTER TABLE CollectionEventImage ADD Description XML NULL
GO

ALTER TABLE CollectionEventImage_log ADD Description XML NULL
GO

ALTER TABLE CollectionEventSeriesImage ADD Description XML NULL
GO

ALTER TABLE CollectionEventSeriesImage_log ADD Description XML NULL
GO

ALTER TABLE CollectionSpecimenImage ADD Description XML NULL
GO

ALTER TABLE CollectionSpecimenImage_log ADD Description XML NULL
GO



--#####################################################################################################################
--######   Trigger fuer ImageDescription   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO



ALTER TRIGGER [dbo].[trgDelCollectionEventImage] ON [dbo].[CollectionEventImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 


/* setting the version in the main table */ 
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionEventID FROM deleted)
EXECUTE procSetVersionCollectionEvent @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end

GO






ALTER TRIGGER [dbo].[trgUpdCollectionEventImage] ON [dbo].[CollectionEventImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* setting the version in the main table */ 
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionEventID FROM deleted)
EXECUTE procSetVersionCollectionEvent @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)


/* updating the logging columns */
Update CollectionEventImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventImage, deleted 
where 1 = 1 
AND CollectionEventImage.CollectionEventID = deleted.CollectionEventID
AND CollectionEventImage.URI = deleted.URI

/* saving the original dataset in the logging table */ 
/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end

GO



ALTER TRIGGER [dbo].[trgDelCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 


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
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO





ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 

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
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenImage, deleted 
where 1 = 1 
AND CollectionSpecimenImage.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenImage.URI = deleted.URI

GO






ALTER TRIGGER [dbo].[trgDelCollectionEventSeriesImage] ON [dbo].[CollectionEventSeriesImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.4 */ 
/*  Date: 19.02.2008  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeriesImage_Log (SeriesID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.SeriesID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO





ALTER TRIGGER [dbo].[trgUpdCollectionEventSeriesImage] ON [dbo].[CollectionEventSeriesImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.4 */ 
/*  Date: 19.02.2008  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeriesImage_Log (SeriesID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.SeriesID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED


/* updating the logging columns */
Update CollectionEventSeriesImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventSeriesImage, deleted 
where 1 = 1 
AND CollectionEventSeriesImage.SeriesID = deleted.SeriesID
AND CollectionEventSeriesImage.URI = deleted.URI

GO




--#####################################################################################################################
--######   [CollectionImage]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO



ALTER TABLE CollectionImage ADD Description XML NULL
GO

ALTER TABLE CollectionImage_log ADD Description XML NULL
GO



ALTER TRIGGER [dbo].[trgDelCollectionImage] ON [dbo].[CollectionImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.6 */ 
/*  Date: 14.12.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionImage_Log (CollectionID, URI, ImageType, [Description], Notes, DataWithholdingReason, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionID, deleted.URI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO




ALTER TRIGGER [dbo].[trgUpdCollectionImage] ON [dbo].[CollectionImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.6 */ 
/*  Date: 14.12.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionImage_Log (CollectionID, URI, ImageType, [Description], Notes, DataWithholdingReason, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionID, deleted.URI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


/* updating the logging columns */
Update CollectionImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionImage, deleted 
where 1 = 1 
AND CollectionImage.CollectionID = deleted.CollectionID
AND CollectionImage.URI = deleted.URI

GO

--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

GRANT INSERT ON [dbo].ProjectProxy TO [DiversityCollectionAdministrator] AS [dbo]
GO

GRANT SELECT ON [dbo].ProjectProxy TO [DiversityCollectionAdministrator] AS [dbo]
GO





--#####################################################################################################################
--######   [RecordingMethod]   ######################################################################################
--#####################################################################################################################




--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

ALTER TABLE CollectionEventLocalisation ADD RecordingMethod nvarchar(500) NULL
GO

ALTER TABLE CollectionEventLocalisation_log ADD RecordingMethod nvarchar(500) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The method or device used for the recording of the localisation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventLocalisation', @level2type=N'COLUMN',@level2name=N'RecordingMethod'
GO



ALTER TRIGGER [dbo].[trgDelCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 


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
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, 
LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, 
ResponsibleAgentURI, RecordingMethod, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, 
deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RecordingMethod, 
deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, 
DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, RecordingMethod, 
Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, 
deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RecordingMethod, 
deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end

GO






ALTER TRIGGER [dbo].[trgUpdCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 

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
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, 
LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, 
ResponsibleAgentURI, RecordingMethod, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, 
deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RecordingMethod, 
deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, 
DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, RecordingMethod, 
Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, 
deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RecordingMethod, 
deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end


/* updating the logging columns */
Update CollectionEventLocalisation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventLocalisation, deleted 
where 1 = 1 
AND CollectionEventLocalisation.CollectionEventID = deleted.CollectionEventID
AND CollectionEventLocalisation.LocalisationSystemID = deleted.LocalisationSystemID

/* updating the geography column */
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
--AND inserted.Geography IS NULL

GO




--#####################################################################################################################
--######   [Collectionspecimenpart]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

ALTER TABLE CollectionSpecimenPart ALTER COLUMN Stock float
GO

ALTER TABLE CollectionSpecimenPart ADD StorageContainer nvarchar(500) NULL
GO

ALTER TABLE CollectionSpecimenPart ADD StockUnit nvarchar(50) NULL
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The container in which the part is stored' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPart', @level2type=N'COLUMN',@level2name=N'StorageContainer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If empty the stock is given as a count, else it contains the unit in which stock is expressed, e.g. µl, ml, kg etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPart', @level2type=N'COLUMN',@level2name=N'StockUnit'
GO


ALTER TABLE CollectionSpecimenPart_log ADD StorageContainer nvarchar(500) NULL
GO

ALTER TABLE CollectionSpecimenPart_log ADD StockUnit nvarchar(50) NULL
GO



CREATE ROLE [DiversityCollectionPartEditor] AUTHORIZATION [dbo]
GO

GRANT INSERT ON CollectionSpecimenPart TO [DiversityCollectionPartEditor] AS [dbo]
GO
GRANT UPDATE ON CollectionSpecimenPart TO [DiversityCollectionPartEditor] AS [dbo]
GO
GRANT DELETE ON CollectionSpecimenPart TO [DiversityCollectionPartEditor] AS [dbo]
GO

GRANT INSERT ON CollectionSpecimenProcessing TO [DiversityCollectionPartEditor] AS [dbo]
GO
GRANT UPDATE ON CollectionSpecimenProcessing TO [DiversityCollectionPartEditor] AS [dbo]
GO
GRANT DELETE ON CollectionSpecimenProcessing TO [DiversityCollectionPartEditor] AS [dbo]
GO

GRANT INSERT ON CollectionSpecimenTransaction TO [DiversityCollectionPartEditor] AS [dbo]
GO
GRANT UPDATE ON CollectionSpecimenTransaction TO [DiversityCollectionPartEditor] AS [dbo]
GO






ALTER TRIGGER [dbo].[trgDelCollectionSpecimenPart] ON [dbo].[CollectionSpecimenPart] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 


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
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, Notes, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, Notes, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenPart] ON [dbo].[CollectionSpecimenPart] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 

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
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, 
Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, 
Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenPart
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenPart, deleted 
where 1 = 1 
AND CollectionSpecimenPart.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenPart.SpecimenPartID = deleted.SpecimenPartID

GO

--#####################################################################################################################
--######   [CollectionSpecimenPart_Core]   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO


ALTER VIEW [dbo].[CollectionSpecimenPart_Core]
AS
SELECT     P.CollectionSpecimenID, P.SpecimenPartID, P.DerivedFromSpecimenPartID, P.PreparationMethod, P.PreparationDate, P.AccessionNumber, P.PartSublabel, 
                      P.CollectionID, P.MaterialCategory, P.StorageLocation, P.StorageContainer, P.Stock, P.StockUnit, P.Notes
FROM         CollectionSpecimenID_UserAvailable AS U INNER JOIN
                      CollectionSpecimenPart AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################




--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO



CREATE ROLE [DiversityCollectionStorageManager] AUTHORIZATION [dbo]
GO

GRANT INSERT ON CollectionSpecimenPart TO [DiversityCollectionStorageManager] AS [dbo]
GO
GRANT UPDATE ON CollectionSpecimenPart TO [DiversityCollectionStorageManager] AS [dbo]
GO
GRANT DELETE ON CollectionSpecimenPart TO [DiversityCollectionStorageManager] AS [dbo]
GO

GRANT INSERT ON CollectionSpecimenProcessing TO [DiversityCollectionStorageManager] AS [dbo]
GO
GRANT UPDATE ON CollectionSpecimenProcessing TO [DiversityCollectionStorageManager] AS [dbo]
GO
GRANT DELETE ON CollectionSpecimenProcessing TO [DiversityCollectionStorageManager] AS [dbo]
GO

GRANT INSERT ON CollectionSpecimenTransaction TO [DiversityCollectionStorageManager] AS [dbo]
GO
GRANT UPDATE ON CollectionSpecimenTransaction TO [DiversityCollectionStorageManager] AS [dbo]
GO





--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

/****** Object:  DatabaseRole [DiversityCollectionPartEditor]    Script Date: 12/29/2011 18:40:28 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionPartEditor' AND type = 'R')
DROP ROLE [DiversityCollectionPartEditor]
GO




--#####################################################################################################################
--######   Annotation   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO


CREATE TABLE [dbo].[AnnotationType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AnnotationType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AnnotationType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AnnotationType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AnnotationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AnnotationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AnnotationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AnnotationType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AnnotationType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

ALTER TABLE [dbo].[AnnotationType_Enum] ADD  CONSTRAINT [DF_AnnotationType_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO



INSERT INTO [AnnotationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes]
           ,[ParentCode]
           ,[RowGUID])

SELECT [Code]
      ,[Description]
      ,[DisplayText]
      ,[DisplayOrder]
      ,[DisplayEnable]
      ,[InternalNotes]
      ,[ParentCode]
      ,[RowGUID]
  FROM [DiversityCollection_Test].[dbo].[AnnotationType_Enum]
  
  


CREATE TABLE [dbo].[Annotation](
	[AnnotationID] [int] IDENTITY(1,1) NOT NULL,
	[ReferencedAnnotationID] [int] NULL,
	[AnnotationType] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[Annotation] [nvarchar](max) NOT NULL,
	[URI] [varchar](255) NULL,
	[ReferenceDisplayText] [nvarchar](500) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[SourceDisplayText] [nvarchar](500) NULL,
	[SourceURI] [varchar](255) NULL,
	[IsInternal] [bit] NULL,
	[ReferencedID] [int] NOT NULL,
	[ReferencedTable] [nvarchar](500) NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Annotation] PRIMARY KEY CLUSTERED 
(
	[AnnotationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the annotation (Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'AnnotationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If an annotation refers to another annotation, the ID of the referred annotation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'ReferencedAnnotationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the annotation as defined in AnnotationType_Enum, e.g. Reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'AnnotationType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title of the annotation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'Title'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The annotation entered by the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'Annotation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The complete URI address of a resource related to the annotation. May be link to a module, e.g. for the annotation type reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'URI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the reference. If the entry is linked to an external module like DiversityReferences, the cached display text of the referenced dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'ReferenceDisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the entry is linked to an external module like DiversityReferences, the link to the referenced dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source. If the entry is linked to an external module like DiversityAgents, the cached display text of the referenced dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'SourceDisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the entry is linked to an external module like DiversityAgents, the link to the referenced dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'SourceURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If an annotation is restricted to authorized users of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'IsInternal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the dataset in the table the annotation refers to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'ReferencedID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the table the annotation refers to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'ReferencedTable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Annotations to datasets in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Annotation'
GO

ALTER TABLE [dbo].[Annotation]  WITH CHECK ADD  CONSTRAINT [FK_Annotation_AnnotationType_Enum] FOREIGN KEY([AnnotationType])
REFERENCES [dbo].[AnnotationType_Enum] ([Code])
GO

ALTER TABLE [dbo].[Annotation] CHECK CONSTRAINT [FK_Annotation_AnnotationType_Enum]
GO

ALTER TABLE [dbo].[Annotation] ADD  CONSTRAINT [DF_Annotation_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[Annotation] ADD  CONSTRAINT [DF_Annotation_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[Annotation] ADD  CONSTRAINT [DF_Annotation_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[Annotation] ADD  CONSTRAINT [DF_Annotation_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[Annotation] ADD  CONSTRAINT [DF_Annotation_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

  

CREATE TRIGGER [dbo].[trgUpdAnnotation] ON [dbo].[Annotation] 
FOR UPDATE AS

/* updating the logging columns */
Update Annotation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Annotation, deleted 
where Annotation.AnnotationID = deleted.AnnotationID

GO


CREATE TRIGGER [dbo].[trgInsAnnotation] ON [dbo].[Annotation] 
FOR INSERT AS

/* updating the logging columns */
Update Annotation
set LogCreatedWhen = case when inserted.LogCreatedWhen IS NULL THEN getdate() else inserted.LogCreatedWhen end
, LogUpdatedWhen = case when inserted.LogUpdatedWhen IS NULL THEN getdate() else inserted.LogUpdatedWhen end
, LogCreatedBy = case when inserted.LogCreatedBy IS NULL THEN current_user else inserted.LogCreatedBy end
, LogUpdatedBy = case when inserted.LogUpdatedBy IS NULL THEN current_user else inserted.LogUpdatedBy end
FROM Annotation, inserted 
where Annotation.AnnotationID = inserted.AnnotationID

GO




GRANT SELECT ON [dbo].[AnnotationType_Enum] TO [DiversityCollectionUser] AS [dbo]
GO

GRANT INSERT ON [dbo].Annotation TO [DiversityCollectionUser] AS [dbo]
GO

GRANT SELECT ON [dbo].Annotation TO [DiversityCollectionUser] AS [dbo]
GO




--#####################################################################################################################
--######   [DiversityCollectionDataManager]   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO



IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DataManager' AND type = 'R')
AND  NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionDataManager' AND type = 'R')
	CREATE ROLE [DiversityCollectionDataManager] AUTHORIZATION [dbo]
GO

GRANT DELETE ON CollectionSpecimen TO [DiversityCollectionDataManager] AS [dbo]
GO

GRANT DELETE ON Annotation TO [DiversityCollectionDataManager] AS [dbo]
GO

GRANT UPDATE ON Annotation TO [DiversityCollectionDataManager] AS [dbo]
GO

EXEC sp_addrolemember 'DiversityCollectionDataManager', 'DiversityCollectionAdministrator'
GO

EXEC sp_addrolemember 'DiversityCollectionEditor', 'DiversityCollectionDataManager'
GO

EXEC sp_addrolemember 'DiversityCollectionUser', 'DiversityCollectionStorageManager'
GO



--#####################################################################################################################
--######   [ImageDescription]   ######################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'Description'
GO


ALTER TABLE ProjectProxy ADD ImageDescriptionTemplate XML NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Template for the description of images' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ImageDescriptionTemplate'
GO


GRANT UPDATE ON ProjectProxy(ImageDescriptionTemplate) TO [DiversityCollectionDataManager] AS [dbo]
GO

GRANT UPDATE ON ProjectProxy(ImageDescriptionTemplate) TO DiversityCollectionAdministrator AS [dbo]
GO


/****** Object:  DatabaseRole [DiversityCollectionCurator]    Script Date: 12/29/2011 18:40:28 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionCurator' AND type = 'R')
DROP ROLE [DiversityCollectionCurator]
GO


/****** Object:  DatabaseRole [DiversityCollectionCurator]    Script Date: 12/29/2011 18:40:28 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'wb_functions' AND type = 'R')
DROP ROLE [wb_functions]
GO






















--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO




/****** Object:  DatabaseRole [DiversityCollectionEntityEditor]    Script Date: 01/12/2012 12:56:59 ******/
/****** Object:  DatabaseRole [DiversityCollectionCurator]    Script Date: 12/29/2011 18:40:28 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionEntityEditor' AND type = 'R')
ALTER ROLE DiversityCollectionEntityEditor WITH NAME = DescriptionEditor
ELSE
BEGIN

CREATE ROLE DescriptionEditor AUTHORIZATION [dbo]

GRANT DELETE ON [dbo].[EntityRepresentation] TO [DescriptionEditor] AS [dbo]
GRANT INSERT ON [dbo].[EntityRepresentation] TO [DescriptionEditor] AS [dbo]
GRANT SELECT ON [dbo].[EntityRepresentation] TO [DescriptionEditor] AS [dbo]
GRANT UPDATE ON [dbo].[EntityRepresentation] TO [DescriptionEditor] AS [dbo]

GRANT DELETE ON [dbo].[EntityUsage] TO [DescriptionEditor] AS [dbo]
GRANT INSERT ON [dbo].[EntityUsage] TO [DescriptionEditor] AS [dbo]
GRANT SELECT ON [dbo].[EntityUsage] TO [DescriptionEditor] AS [dbo]
GRANT UPDATE ON [dbo].[EntityUsage] TO [DescriptionEditor] AS [dbo]

GRANT DELETE ON [dbo].[EntityContext_Enum] TO [DescriptionEditor] AS [dbo]
GRANT INSERT ON [dbo].[EntityContext_Enum] TO [DescriptionEditor] AS [dbo]
GRANT SELECT ON [dbo].[EntityContext_Enum] TO [DescriptionEditor] AS [dbo]
GRANT UPDATE ON [dbo].[EntityContext_Enum] TO [DescriptionEditor] AS [dbo]

GRANT DELETE ON [dbo].[Entity] TO [DescriptionEditor] AS [dbo]
GRANT INSERT ON [dbo].[Entity] TO [DescriptionEditor] AS [dbo]
GRANT SELECT ON [dbo].[Entity] TO [DescriptionEditor] AS [dbo]
GRANT UPDATE ON [dbo].[Entity] TO [DescriptionEditor] AS [dbo]

END
GO




IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionAdministrator' AND type = 'R')
	ALTER ROLE DiversityCollectionAdministrator WITH NAME = Administrator
ELSE
	BEGIN
	CREATE ROLE Administrator AUTHORIZATION [dbo]
	END
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionManager' AND type = 'R')
	ALTER ROLE DiversityCollectionManager WITH NAME = CollectionManager
ELSE
	BEGIN
	CREATE ROLE CollectionManager AUTHORIZATION [dbo]
	GRANT INSERT ON [dbo].[Collection_log] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[Collection_log] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionImage_log] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionImage_log] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenTransaction_log] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionSpecimenTransaction_log] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[TransactionDocument_log] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionDocument_log] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[Transaction_log] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[Transaction_log] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionRequestSpecimenParts] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[UserGroups] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionUserRequest] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionRequest] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionForeignRequest] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[Collection] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[Collection] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[Collection] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionChildNodes] TO [CollectionManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionRequester] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionRequester] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionRequester] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionRequester] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionHierarchy] TO [CollectionManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionImage] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionImage] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionImage] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionImage] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionManager] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[ManagerCollectionList] TO [CollectionManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionSpecimenTransaction] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenTransaction] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionSpecimenTransaction] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionSpecimenTransaction] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[Transaction] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[Transaction] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[Transaction] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionHierarchy] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[TransactionDocument] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionDocument] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[TransactionDocument] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionBalance] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[Collection_log] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[Collection] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[Collection] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[Collection] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenTransaction] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionSpecimenTransaction] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionSpecimenTransaction] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[Transaction_log] TO [CollectionManager] AS [dbo]
	GRANT ALTER ON [dbo].[Transaction] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[Transaction] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[Transaction] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[Transaction] TO [CollectionManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionRequester] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenTransaction_log] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[TransactionDocument_log] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionManager] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionImage_log] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionImage_log] TO [CollectionManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionImage] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionImage] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CollectionImage] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionImage] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[ManagerCollectionList] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CuratorCollectionList] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[CuratorCollectionHierarchyList] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionList] TO [CollectionManager] AS [dbo]
	GRANT INSERT ON [dbo].[TransactionDocument] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionDocument] TO [CollectionManager] AS [dbo]
	GRANT UPDATE ON [dbo].[TransactionDocument] TO [CollectionManager] AS [dbo]
	GRANT SELECT ON [dbo].[TransactionChildNodes] TO [CollectionManager] AS [dbo]
	END
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionDataManager' AND type = 'R')
	ALTER ROLE DiversityCollectionDataManager WITH NAME = DataManager
ELSE
	BEGIN
	CREATE ROLE DataManager AUTHORIZATION [dbo]
	GRANT DELETE ON [dbo].[CollectionSpecimen] TO [DataManager] AS [dbo]
	GRANT UPDATE ON [dbo].[ProjectProxy] ([ImageDescriptionTemplate]) TO [DataManager] AS [dbo]
	GRANT DELETE ON [dbo].[Annotation] TO [DataManager] AS [dbo]
	GRANT UPDATE ON [dbo].[Annotation] TO [DataManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionSpecimen] TO [DataManager] AS [dbo]
	GRANT DELETE ON [dbo].[Annotation] TO [DataManager] AS [dbo]
	GRANT UPDATE ON [dbo].[Annotation] TO [DataManager] AS [dbo]
	GRANT UPDATE ON [dbo].[ProjectProxy] ([ImageDescriptionTemplate]) TO [DataManager] AS [dbo]
	END
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionEditor' AND type = 'R')
	ALTER ROLE DiversityCollectionEditor WITH NAME = Editor
ELSE
	BEGIN
	CREATE ROLE Editor AUTHORIZATION [dbo]
	END
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionRequester' AND type = 'R')
	ALTER ROLE DiversityCollectionRequester WITH NAME = Requester
ELSE
	BEGIN
	CREATE ROLE Requester AUTHORIZATION [dbo]
	END
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionStorageManager' AND type = 'R')
	ALTER ROLE DiversityCollectionStorageManager WITH NAME = StorageManager
ELSE
	BEGIN
	CREATE ROLE StorageManager AUTHORIZATION [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenTransaction] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionSpecimenTransaction] TO [StorageManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionSpecimenPart] TO [StorageManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenPart] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionSpecimenPart] TO [StorageManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenTransaction] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionSpecimenTransaction] TO [StorageManager] AS [dbo]
	GRANT DELETE ON [dbo].[IdentificationUnitAnalysis] TO [StorageManager] AS [dbo]
	GRANT INSERT ON [dbo].[IdentificationUnitAnalysis] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[IdentificationUnitAnalysis] TO [StorageManager] AS [dbo]
	GRANT DELETE ON [dbo].[IdentificationUnitInPart] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[IdentificationUnitInPart] TO [StorageManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionSpecimenProcessing] TO [StorageManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenProcessing] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionSpecimenProcessing] TO [StorageManager] AS [dbo]
	GRANT DELETE ON [dbo].[IdentificationUnitInPart] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[IdentificationUnitInPart] TO [StorageManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionSpecimenProcessing] TO [StorageManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenProcessing] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionSpecimenProcessing] TO [StorageManager] AS [dbo]
	GRANT DELETE ON [dbo].[CollectionSpecimenPart] TO [StorageManager] AS [dbo]
	GRANT INSERT ON [dbo].[CollectionSpecimenPart] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[CollectionSpecimenPart] TO [StorageManager] AS [dbo]
	GRANT DELETE ON [dbo].[IdentificationUnitAnalysis] TO [StorageManager] AS [dbo]
	GRANT INSERT ON [dbo].[IdentificationUnitAnalysis] TO [StorageManager] AS [dbo]
	GRANT UPDATE ON [dbo].[IdentificationUnitAnalysis] TO [StorageManager] AS [dbo]
	END
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionTypist' AND type = 'R')
	ALTER ROLE DiversityCollectionTypist WITH NAME = Typist
ELSE
	BEGIN
	CREATE ROLE Typist AUTHORIZATION [dbo]
	END
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DiversityCollectionUser' AND type = 'R')
	ALTER ROLE DiversityCollectionUser WITH NAME = [User]
ELSE
	BEGIN
	CREATE ROLE [User] AUTHORIZATION [dbo]
	END
GO

EXEC sp_addrolemember 'StorageManager', 'CollectionManager'
GO


GRANT UPDATE ON IdentificationUnitInPart TO StorageManager AS [dbo]
GO

GRANT DELETE ON IdentificationUnitInPart TO StorageManager AS [dbo]
GO

GRANT UPDATE ON IdentificationUnitAnalysis TO StorageManager AS [dbo]
GO

GRANT DELETE ON IdentificationUnitAnalysis TO StorageManager AS [dbo]
GO

GRANT INSERT ON IdentificationUnitAnalysis TO StorageManager AS [dbo]
GO


UPDATE [CollTaxonomicGroup_Enum]
   SET [DisplayEnable] = 1
 WHERE [Code] = 'soil'
GO




--#####################################################################################################################
--######   [FirstLinesPart]   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO




ALTER FUNCTION [dbo].[FirstLinesPart] 
(@CollectionSpecimenIDs varchar(4000), 
@AnalysisIDs varchar(4000), @AnalysisStartDate date, @AnalysisEndDate date, 
@ProcessingID int, @ProcessingStartDate datetime, @ProcessingEndDate datetime)   
RETURNS @List TABLE (
	[SpecimenPartID] [int] Primary key,
	[CollectionSpecimenID] [int], --
	[Accession_number] [nvarchar](50) NULL, --
-- WITHHOLDINGREASONS
	[Data_withholding_reason] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collector] [nvarchar](255) NULL, --
--CollectionEvent
	[Collectors_event_number] [nvarchar](50) NULL, --
	[Collection_day] [tinyint] NULL, --
	[Collection_month] [tinyint] NULL, --
	[Collection_year] [smallint] NULL, --
	[Collection_date_supplement] [nvarchar](100) NULL, --
	[Collection_time] [varchar](50) NULL, --
	[Collection_time_span] [varchar](50) NULL, --
	[Country] [nvarchar](50) NULL, --
	[Locality_description] [nvarchar](255) NULL, --
	[Habitat_description] [nvarchar](255) NULL, -- 
	[Collecting_method] [nvarchar](255) NULL, --
	[Collection_event_notes] [nvarchar](255) NULL, --
--Localisation
	[Named_area] [nvarchar](255) NULL, -- 
	[NamedAreaLocation2] [nvarchar](255) NULL, --
	[Remove_link_to_gazetteer] [int] NULL,
	[Distance_to_location] [varchar](50) NULL, --
	[Direction_to_location] [varchar](50) NULL, --
	[Longitude] [nvarchar](255) NULL, --
	[Latitude] [nvarchar](255) NULL, --
	[Coordinates_accuracy] [nvarchar](50) NULL, --
	[Link_to_GoogleMaps] [int] NULL,
	[Altitude_from] [nvarchar](255) NULL, --
	[Altitude_to] [nvarchar](255) NULL, --
	[Altitude_accuracy] [nvarchar](50) NULL, --
	[Notes_for_Altitude] [nvarchar](255) NULL, --
	[MTB] [nvarchar](255) NULL, --
	[Quadrant] [nvarchar](255) NULL, --
	[Notes_for_MTB] [nvarchar](255) NULL, --
	[Sampling_plot] [nvarchar](255) NULL, --
	[Link_to_SamplingPlots] [nvarchar](255) NULL, --
	[Remove_link_to_SamplingPlots] [int] NULL,
	[Accuracy_of_sampling_plot] [nvarchar](50) NULL, --
	[Latitude_of_sampling_plot] [real] NULL, --
	[Longitude_of_sampling_plot] [real] NULL, --
--Properties
	[Geographic_region] [nvarchar](255) NULL, --
	[Lithostratigraphy] [nvarchar](255) NULL, --
	[Chronostratigraphy] [nvarchar](255) NULL, --
--Agent
	[Collectors_name] [nvarchar](255) NULL, --
	[Link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_collector] [int] NULL,
	[Collectors_number] [nvarchar](50) NULL, --
	[Notes_about_collector] [nvarchar](max) NULL, --
--Accession
	[Accession_day] [tinyint] NULL, --
	[Accession_month] [tinyint] NULL, --
	[Accession_year] [smallint] NULL, --
	[Accession_date_supplement] [nvarchar](255) NULL, --
--Depositor
	[Depositors_name] [nvarchar](255) NULL, --
	[Depositors_link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_Depositor] [int] NULL,
	[Depositors_accession_number] [nvarchar](50) NULL, --
--Exsiccate
	[Exsiccata_abbreviation] [nvarchar](255) NULL, --
	[Link_to_DiversityExsiccatae] [varchar](255) NULL, --
	[Remove_link_to_exsiccatae] [int] NULL,
	[Exsiccata_number] [nvarchar](50) NULL, --
--Notes
	[Original_notes] [nvarchar](max) NULL, --
	[Additional_notes] [nvarchar](max) NULL, --
	[Internal_notes] [nvarchar](max) NULL, --
--Label
	[Label_title] [nvarchar](255) NULL, --
	[Label_type] [nvarchar](50) NULL, --
	[Label_transcription_state] [nvarchar](50) NULL, --
	[Label_transcription_notes] [nvarchar](255) NULL, --
	[Problems] [nvarchar](255) NULL, --
--Organism
	[Taxonomic_group] [nvarchar](50) NULL, --
	[Relation_type] [nvarchar](50) NULL, --
	[Colonised_substrate_part] [nvarchar](255) NULL, --
	[Related_organism] [nvarchar] (200) NULL,
	[Life_stage] [nvarchar](255) NULL, --
	[Gender] [nvarchar](50) NULL, --
	[Number_of_units] [smallint] NULL, --
	[Circumstances] [nvarchar](50) NULL, -- 
	[Order_of_taxon] [nvarchar](255) NULL, --
	[Family_of_taxon] [nvarchar](255) NULL, --
	[Identifier_of_organism] [nvarchar](50) NULL, --
	[Description_of_organism] [nvarchar](50) NULL, --
	[Only_observed] [bit] NULL, --
	[Notes_for_organism] [nvarchar](max) NULL, --
--Identification
	[Taxonomic_name] [nvarchar](255) NULL, --
	[Link_to_DiversityTaxonNames] [varchar](255) NULL, --
	[Remove_link_for_identification] [int] NULL, 
	[Vernacular_term] [nvarchar](255) NULL, --
	[Identification_day] [tinyint] NULL, -- 
	[Identification_month] [tinyint] NULL, --
	[Identification_year] [smallint] NULL, --
	[Identification_category] [nvarchar](50) NULL, --
	[Identification_qualifier] [nvarchar](50) NULL, --
	[Type_status] [nvarchar](50) NULL, --
	[Type_notes] [nvarchar](max) NULL, --
	[Notes_for_identification] [nvarchar](max) NULL, --
	[Reference_title] [nvarchar](255) NULL, --
	[Link_to_DiversityReferences] [varchar](255) NULL, --
	[Remove_link_for_reference] [int] NULL,
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, --
	[Remove_link_for_determiner] [int] NULL,
--Analysis
	[Analysis_0] [nvarchar](50) NULL, --
	[AnalysisID_0] [int] NULL, --
	[Analysis_number_0] [nvarchar](50) NULL, --
	[Analysis_result_0] [nvarchar](max) NULL, --
	
	[Analysis_1] [nvarchar](50) NULL, --
	[AnalysisID_1] [int] NULL, --
	[Analysis_number_1] [nvarchar](50) NULL, --
	[Analysis_result_1] [nvarchar](max) NULL, --
	
	[Analysis_2] [nvarchar](50) NULL, --
	[AnalysisID_2] [int] NULL, --
	[Analysis_number_2] [nvarchar](50) NULL, --
	[Analysis_result_2] [nvarchar](max) NULL, --
	
	[Analysis_3] [nvarchar](50) NULL, --
	[AnalysisID_3] [int] NULL, --
	[Analysis_number_3] [nvarchar](50) NULL, --
	[Analysis_result_3] [nvarchar](max) NULL, --
	
	[Analysis_4] [nvarchar](50) NULL, --
	[AnalysisID_4] [int] NULL, --
	[Analysis_number_4] [nvarchar](50) NULL, --
	[Analysis_result_4] [nvarchar](max) NULL, --
	
	[Analysis_5] [nvarchar](50) NULL, --
	[AnalysisID_5] [int] NULL, --
	[Analysis_number_5] [nvarchar](50) NULL, --
	[Analysis_result_5] [nvarchar](max) NULL, --
	
	[Analysis_6] [nvarchar](50) NULL, --
	[AnalysisID_6] [int] NULL, --
	[Analysis_number_6] [nvarchar](50) NULL, --
	[Analysis_result_6] [nvarchar](max) NULL, --
	
	[Analysis_7] [nvarchar](50) NULL, --
	[AnalysisID_7] [int] NULL, --
	[Analysis_number_7] [nvarchar](50) NULL, --
	[Analysis_result_7] [nvarchar](max) NULL, --
	
	[Analysis_8] [nvarchar](50) NULL, --
	[AnalysisID_8] [int] NULL, --
	[Analysis_number_8] [nvarchar](50) NULL, --
	[Analysis_result_8] [nvarchar](max) NULL, --
	
	[Analysis_9] [nvarchar](50) NULL, --
	[AnalysisID_9] [int] NULL, --
	[Analysis_number_9] [nvarchar](50) NULL, --
	[Analysis_result_9] [nvarchar](max) NULL, --
	
--Storage	
	[Preparation_method] [nvarchar](max) NULL, --
	[Preparation_date] [datetime] NULL, --
	[Part_accession_number] [nvarchar](50) NULL, --
	[Part_sublabel] [nvarchar](50) NULL, --
	[Collection] [int] NULL, --
	[Material_category] [nvarchar](50) NULL, --
	[Storage_location] [nvarchar](255) NULL, --
	[Storage_container] [nvarchar](500) NULL, --
	[Stock] [float] NULL, --
	[Stock_unit] [nvarchar](50) NULL, --
	[Notes_for_part] [nvarchar](max) NULL, --
	
--IdentificationUnitInPart	
	[Description_of_unit_in_part] [nvarchar](500) NULL, --
	
--Processing
	[Processing_date_1] [datetime] NULL,
	[ProcessingID_1] [int] NULL,
	[Processing_Protocoll_1] [nvarchar](100) NULL,
	[Processing_duration_1] [varchar](50) NULL,
	[Processing_notes_1] [nvarchar](max) NULL,

	[Processing_date_2] [datetime] NULL,
	[ProcessingID_2] [int] NULL,
	[Processing_Protocoll_2] [nvarchar](100) NULL,
	[Processing_duration_2] [varchar](50) NULL,
	[Processing_notes_2] [nvarchar](max) NULL,

	[Processing_date_3] [datetime] NULL,
	[ProcessingID_3] [int] NULL,
	[Processing_Protocoll_3] [nvarchar](100) NULL,
	[Processing_duration_3] [varchar](50) NULL,
	[Processing_notes_3] [nvarchar](max) NULL,

	[Processing_date_4] [datetime] NULL,
	[ProcessingID_4] [int] NULL,
	[Processing_Protocoll_4] [nvarchar](100) NULL,
	[Processing_duration_4] [varchar](50) NULL,
	[Processing_notes_4] [nvarchar](max) NULL,

	[Processing_date_5] [datetime] NULL,
	[ProcessingID_5] [int] NULL,
	[Processing_Protocoll_5] [nvarchar](100) NULL,
	[Processing_duration_5] [varchar](50) NULL,
	[Processing_notes_5] [nvarchar](max) NULL,

--Transaction
	[_TransactionID] [int] NULL, --
	[_Transaction] [nvarchar](200) NULL, --
	[On_loan] [int] NULL, --
--Hidden fields
	[_CollectionEventID] [int] NULL, --
	[_IdentificationUnitID] [int] NULL, --
	[_IdentificationSequence] [smallint] NULL, --
	[_SpecimenPartID] [int] NULL, --
	[_CoordinatesAverageLatitudeCache] [real] NULL, --
	[_CoordinatesAverageLongitudeCache] [real] NULL, --
	[_CoordinatesLocationNotes] [nvarchar](255) NULL, --
	[_GeographicRegionPropertyURI] [varchar](255) NULL, --
	[_LithostratigraphyPropertyURI] [varchar](255) NULL, --
	[_ChronostratigraphyPropertyURI] [varchar](255) NULL, --
	[_NamedAverageLatitudeCache] [real] NULL, --
	[_NamedAverageLongitudeCache] [real] NULL, --
	[_LithostratigraphyPropertyHierarchyCache] [nvarchar](255) NULL, --
	[_ChronostratigraphyPropertyHierarchyCache] [nvarchar](255) NULL, --
	[_AverageAltitudeCache] [real] NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
MW 18.08.2011 
TEST: 
Select * from dbo.FirstLinesPart('3251, 3252', '34', null, null, null, null) order by CollectionSpecimenID, SpecimenPartID
Select * from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '1/1/2000', '12/12/2010') order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '1/1/2000', '12/12/2010') P order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '2000/2/1', '2010/12/31') P order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, null, null) P order by CollectionSpecimenID, SpecimenPartID
select * from CollectionSpecimenProcessing
Select * from dbo.FirstLinesPart('177930', '26,39,41,44,45', null, null, null, null, null) P order by CollectionSpecimenID, SpecimenPartID
Select * from dbo.FirstLinesPart('177930', '26,39,41,44,45', null, null, 8, null, null) P order by CollectionSpecimenID, SpecimenPartID
*/ 
AS 
BEGIN 

declare @IDs table (ID int  Primary key)
declare @sID varchar(50)
while @CollectionSpecimenIDs <> ''
begin
	if (CHARINDEX(',', @CollectionSpecimenIDs) > 0)
	begin
	set @sID = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, 1, CHARINDEX(',', @CollectionSpecimenIDs) -1)))
	set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 2, 4000)))
	if (isnumeric(@sID) = 1)
		begin
		insert into @IDs 
		values( @sID )
		end
	end
	else
	begin
	set @sID = rtrim(ltrim(@CollectionSpecimenIDs))
	set @CollectionSpecimenIDs = ''
	if (isnumeric(@sID) = 1)
		begin
		insert into @IDs 
		values( @sID )
		end
	end
end





--- Specimen
insert into @List (
SpecimenPartID
, CollectionSpecimenID
, Accession_number
, Data_withholding_reason
, _CollectionEventID
, Accession_day
, Accession_month
, Accession_year
, Accession_date_supplement
, Depositors_name
, Depositors_link_to_DiversityAgents
, Depositors_accession_number
, Exsiccata_abbreviation
, Link_to_DiversityExsiccatae
, Original_notes
, Additional_notes
, Internal_notes
, Label_title
, Label_type
, Label_transcription_state
, Label_transcription_notes
, Problems
, Part_accession_number
, [Collection]
, Material_category
, Notes_for_part
, Part_sublabel
, Preparation_date
, Preparation_method
, Stock
, Stock_unit
, Storage_location
, Storage_container
)
select 
P.SpecimenPartID
, S.CollectionSpecimenID
, S.AccessionNumber
, S.DataWithholdingReason
, S.CollectionEventID 
, AccessionDay
, AccessionMonth
, AccessionYear
, AccessionDateSupplement
, DepositorsName
, DepositorsAgentURI
, DepositorsAccessionNumber
, ExsiccataAbbreviation
, ExsiccataURI
, OriginalNotes
, AdditionalNotes
, InternalNotes
, LabelTitle
, LabelType
, LabelTranscriptionState
, LabelTranscriptionNotes
, Problems
, P.AccessionNumber
, P.CollectionID
, P.MaterialCategory
, P.Notes
, P.PartSublabel
, P.PreparationDate
, P.PreparationMethod
, P.Stock
, P.StockUnit
, P.StorageLocation
, P.StorageContainer
from dbo.CollectionSpecimen S, dbo.CollectionSpecimenPart P, dbo.CollectionSpecimenID_UserAvailable A
where S.CollectionSpecimenID in (select ID from @IDs) 
and P.CollectionSpecimenID = S.CollectionSpecimenID 
and S.CollectionSpecimenID = A.CollectionSpecimenID



--- Event

update L
set L.Collection_day = E.CollectionDay
, L.Collection_month = E.CollectionMonth
, L.Collection_year = E.CollectionYear
, L.Collection_date_supplement = E.CollectionDateSupplement
, L.Collection_time = E.CollectionTime
, L.Collection_time_span = E.CollectionTimeSpan
, L.Country = E.CountryCache
, L.Locality_description = cast(E.LocalityDescription as nvarchar(255))
, L.Habitat_description = cast(E.HabitatDescription as nvarchar(255))
, L.Collecting_method = cast(E.CollectingMethod as nvarchar(255))
, L.Collection_event_notes = cast(E.Notes as nvarchar(255))
, L.Data_withholding_reason_for_collection_event = E.DataWithholdingReason
, L.Collectors_event_number = E.CollectorsEventNumber
from @List L,
CollectionEvent E
where L._CollectionEventID = E.CollectionEventID



--- Named Area

update L
set L.Named_area = E.Location1
, L.NamedAreaLocation2 = E.Location2
, L.Distance_to_location = E.DistanceToLocation
, L.Direction_to_location = E.DirectionToLocation
, L._NamedAverageLatitudeCache = E.AverageLatitudeCache
, L._NamedAverageLongitudeCache = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 7


--- Coordinates

update L
set L.Longitude = E.Location1
, L.Latitude = E.Location2
, L.Coordinates_accuracy = E.LocationAccuracy
, L._CoordinatesAverageLatitudeCache = E.AverageLatitudeCache
, L._CoordinatesAverageLongitudeCache = E.AverageLongitudeCache
, L._CoordinatesLocationNotes = cast(E.LocationNotes as nvarchar (255))
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 8


--- Altitude

update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4



--- MTB

update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = cast(E.LocationNotes as nvarchar(255))
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 3



--- SamplingPlots

update L
set L.Sampling_plot = E.Location1
, L.Link_to_SamplingPlots = E.Location2
, L.Accuracy_of_sampling_plot = E.LocationAccuracy
, L.Latitude_of_sampling_plot = E.AverageLatitudeCache
, L.Longitude_of_sampling_plot = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 13



--- GeographicRegions

update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 10


--- Lithostratigraphy

update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 30



--- Chronostratigraphy

update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 20



--- Collector

update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
--,dbo.CollectionAgent Amin
where L.CollectionSpecimenID = A.CollectionSpecimenID
--and A.CollectionSpecimenID = Amin.CollectionSpecimenID
and EXISTS (SELECT CollectionSpecimenID
	FROM dbo.CollectionAgent AS Amin
	GROUP BY CollectionSpecimenID
	HAVING (A.CollectionSpecimenID = Amin.CollectionSpecimenID) 
	AND (MIN(Amin.CollectorsSequence) = A.CollectorsSequence))

update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
where L.CollectionSpecimenID = A.CollectionSpecimenID
and L.Collectors_name is null
and A.CollectorsSequence is null
and EXISTS (SELECT CollectionSpecimenID
	FROM dbo.CollectionAgent AS Amin
	GROUP BY CollectionSpecimenID
	HAVING (A.CollectionSpecimenID = Amin.CollectionSpecimenID) 
	AND (MIN(Amin.LogCreatedWhen) = A.LogCreatedWhen))


--- IdentificationUnit
--- IdentificationUnitInPart
-- getting the unit IDs of the part
declare @AllUnitIDs table (UnitID int, ID int, DisplayOrder smallint, PartID int)
declare @UnitIDs table (UnitID int, ID int, DisplayOrder smallint, PartID int)

insert into @AllUnitIDs (UnitID, ID, DisplayOrder, PartID)
select P.IdentificationUnitID, P.CollectionSpecimenID, P.DisplayOrder, P.SpecimenPartID
from @IDs as IDs, IdentificationUnitInPart as P, @List as L
where P.DisplayOrder > 0
and IDs.ID = P.CollectionSpecimenID 
and P.SpecimenPartID = L.SpecimenPartID

insert into @UnitIDs (UnitID, ID, DisplayOrder, PartID)
select U.UnitID, U.ID, U.DisplayOrder, U.PartID
from @AllUnitIDs as U
where exists (select * from @AllUnitIDs aU group by aU.ID having min(aU.DisplayOrder) = U.DisplayOrder)


update L
set L.Taxonomic_group = IU.TaxonomicGroup
, L._IdentificationUnitID = IU.IdentificationUnitID
, L.Relation_type = IU.RelationType
, L.Colonised_substrate_part = IU.ColonisedSubstratePart
, L.Life_stage = IU.LifeStage
, L.Gender = IU.Gender
, L.Number_of_units = IU.NumberOfUnits
, L.Circumstances = IU.Circumstances
, L.Order_of_taxon = IU.OrderCache
, L.Family_of_taxon = IU.FamilyCache
, L.Identifier_of_organism = IU.UnitIdentifier
, L.Description_of_organism = IU.UnitDescription
, L.Only_observed = IU.OnlyObserved
, L.Notes_for_organism = IU.Notes
, L.Exsiccata_number = IU.ExsiccataNumber
, L.Description_of_unit_in_part = UP.[Description]
from @List L,
IdentificationUnitInPart UP,
dbo.IdentificationUnit IU,
@UnitIDs U
where L.CollectionSpecimenID = UP.CollectionSpecimenID
and L.SpecimenPartID = UP.SpecimenPartID
and L.CollectionSpecimenID = IU.CollectionSpecimenID
and UP.CollectionSpecimenID = IU.CollectionSpecimenID
and UP.IdentificationUnitID = IU.IdentificationUnitID
and U.UnitID = UP.IdentificationUnitID
and U.PartID = UP.SpecimenPartID



--- Identification

update L
set L._IdentificationSequence = I.IdentificationSequence
, L.Taxonomic_name = I.TaxonomicName
, L.Link_to_DiversityTaxonNames = I.NameURI
, L.Vernacular_term = I.VernacularTerm
, L.Identification_day = I.IdentificationDay
, L.Identification_month = I.IdentificationMonth
, L.Identification_year = I.IdentificationYear
, L.Identification_category = I.IdentificationCategory
, L.Identification_qualifier = I.IdentificationQualifier
, L.Type_status = I.TypeStatus
, L.Type_notes = I.TypeNotes
, L.Notes_for_identification = I.Notes
, L.Reference_title = I.ReferenceTitle
, L.Link_to_DiversityReferences = I.ReferenceURI
, L.Determiner = I.ResponsibleName
, L.Link_to_DiversityAgents_for_determiner = I.ResponsibleAgentURI
from @List L,
dbo.Identification I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.Identification AS Imax
	GROUP BY CollectionSpecimenID, IdentificationUnitID
	HAVING (Imax.CollectionSpecimenID = I.CollectionSpecimenID) AND (Imax.IdentificationUnitID = I.IdentificationUnitID) AND 
	(MAX(Imax.IdentificationSequence) = I.IdentificationSequence))



-- ANALYSIS --- ###############################################################################################################

-- getting the AnalysisID's that should be shown

declare @AnalysisID_0 int
declare @AnalysisID_1 int
declare @AnalysisID_2 int
declare @AnalysisID_3 int
declare @AnalysisID_4 int
declare @AnalysisID_5 int
declare @AnalysisID_6 int
declare @AnalysisID_7 int
declare @AnalysisID_8 int
declare @AnalysisID_9 int


if (not @AnalysisIDs is null and @AnalysisIDs <> '')
begin
	declare @AnalysisID table (ID int Identity(0,1), AnalysisID int Primary key)
	declare @sAnalysisID varchar(50)
	declare @iAnalysis int
	set @iAnalysis = 0
	while @AnalysisIDs <> '' and @iAnalysis < 10
	begin
		if (CHARINDEX(',', @AnalysisIDs) > 0)
		begin
		set @sAnalysisID = rtrim(ltrim(SUBSTRING(@AnalysisIDs, 1, CHARINDEX(',', @AnalysisIDs) -1)))
		set @AnalysisIDs = rtrim(ltrim(SUBSTRING(@AnalysisIDs, CHARINDEX(',', @AnalysisIDs) + 1, 4000)))
		if (isnumeric(@sID) = 1 and (select count(*) from @AnalysisID where AnalysisID = @sID) = 0)
			begin
			insert into @AnalysisID (AnalysisID)
			values( @sAnalysisID )
			end
		end
		else
		begin
		set @sAnalysisID = rtrim(ltrim(@AnalysisIDs))
		set @AnalysisIDs = ''
		if (isnumeric(@sAnalysisID) = 1 and (select count(*) from @AnalysisID where AnalysisID = @sID) = 0)
			begin
			insert into @AnalysisID (AnalysisID)
			values( @sAnalysisID )
			end
		end
		set @iAnalysis = (select count(*) from @AnalysisID)
	end
		
	set @AnalysisID_0 = (select AnalysisID from @AnalysisID where ID = 0)
	set @AnalysisID_1 = (select AnalysisID from @AnalysisID where ID = 1)
	set @AnalysisID_2 = (select AnalysisID from @AnalysisID where ID = 2)
	set @AnalysisID_3 = (select AnalysisID from @AnalysisID where ID = 3)
	set @AnalysisID_4 = (select AnalysisID from @AnalysisID where ID = 4)
	set @AnalysisID_5 = (select AnalysisID from @AnalysisID where ID = 5)
	set @AnalysisID_6 = (select AnalysisID from @AnalysisID where ID = 6)
	set @AnalysisID_7 = (select AnalysisID from @AnalysisID where ID = 7)
	set @AnalysisID_8 = (select AnalysisID from @AnalysisID where ID = 8)
	set @AnalysisID_9 = (select AnalysisID from @AnalysisID where ID = 9)
end


--############### ANALYSIS 0 ###############


update L
set L.AnalysisID_0 = I.AnalysisID
, L.Analysis_number_0 = I.AnalysisNumber
, L.Analysis_result_0 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_0
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_0)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_0 = null
	, L.Analysis_number_0 = null
	, L.Analysis_result_0 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_0
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_0 = null
	, L.Analysis_number_0 = null
	, L.Analysis_result_0 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_0
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end

update L
set L.Analysis_0 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_0 = A.AnalysisID



--############### ANALYSIS 1 ###############

update L
set L.AnalysisID_1 = I.AnalysisID
, L.Analysis_number_1 = I.AnalysisNumber
, L.Analysis_result_1 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_1
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_1)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_1 = null
	, L.Analysis_number_1 = null
	, L.Analysis_result_1 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_1
	and I.AnalysisID = @AnalysisID_1
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_1 = null
	, L.Analysis_number_1 = null
	, L.Analysis_result_1 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_1
	and I.AnalysisID = @AnalysisID_1
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_1 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_1 = A.AnalysisID


--############### ANALYSIS 2 ###############

update L
set L.AnalysisID_2 = I.AnalysisID
, L.Analysis_number_2 = I.AnalysisNumber
, L.Analysis_result_2 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_2
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_2
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_2)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_2 = null
	, L.Analysis_number_2 = null
	, L.Analysis_result_2 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_2
	and I.AnalysisID = @AnalysisID_2
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_2 = null
	, L.Analysis_number_2 = null
	, L.Analysis_result_2 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_2
	and I.AnalysisID = @AnalysisID_2
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_2 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_2 = A.AnalysisID



--############### ANALYSIS 3 ###############

update L
set L.AnalysisID_3 = I.AnalysisID
, L.Analysis_number_3 = I.AnalysisNumber
, L.Analysis_result_3 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_3
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_3
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_3)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_3 = null
	, L.Analysis_number_3 = null
	, L.Analysis_result_3 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_3
	and I.AnalysisID = @AnalysisID_3
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_3 = null
	, L.Analysis_number_3 = null
	, L.Analysis_result_3 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_3
	and I.AnalysisID = @AnalysisID_3
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_3 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_3 = A.AnalysisID


--############### ANALYSIS 4 ###############

update L
set L.AnalysisID_4 = I.AnalysisID
, L.Analysis_number_4 = I.AnalysisNumber
, L.Analysis_result_4 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_4
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_4
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_4)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_4 = null
	, L.Analysis_number_4 = null
	, L.Analysis_result_4 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_4
	and I.AnalysisID = @AnalysisID_4
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_4 = null
	, L.Analysis_number_4 = null
	, L.Analysis_result_4 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_4
	and I.AnalysisID = @AnalysisID_4
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_4 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_4 = A.AnalysisID



--############### ANALYSIS 5 ###############

update L
set L.AnalysisID_5 = I.AnalysisID
, L.Analysis_number_5 = I.AnalysisNumber
, L.Analysis_result_5 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_5
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_5
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_5)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_5 = null
	, L.Analysis_number_5 = null
	, L.Analysis_result_5 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_5
	and I.AnalysisID = @AnalysisID_5
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_5 = null
	, L.Analysis_number_5 = null
	, L.Analysis_result_5 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_5
	and I.AnalysisID = @AnalysisID_5
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_5 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_5 = A.AnalysisID




--############### ANALYSIS 6 ###############

update L
set L.AnalysisID_6 = I.AnalysisID
, L.Analysis_number_6 = I.AnalysisNumber
, L.Analysis_result_6 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_6
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_6
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_6)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_6 = null
	, L.Analysis_number_6 = null
	, L.Analysis_result_6 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_6
	and I.AnalysisID = @AnalysisID_6
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_6 = null
	, L.Analysis_number_6 = null
	, L.Analysis_result_6 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_6
	and I.AnalysisID = @AnalysisID_6
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_6 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_6 = A.AnalysisID



--############### ANALYSIS 7 ###############

update L
set L.AnalysisID_7 = I.AnalysisID
, L.Analysis_number_7 = I.AnalysisNumber
, L.Analysis_result_7 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_7
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_7
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_7)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_7 = null
	, L.Analysis_number_7 = null
	, L.Analysis_result_7 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_7
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_7 = null
	, L.Analysis_number_7 = null
	, L.Analysis_result_7 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_7
	and I.AnalysisID = @AnalysisID_7
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_7 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_7 = A.AnalysisID


--############### ANALYSIS 8 ###############

update L
set L.AnalysisID_8 = I.AnalysisID
, L.Analysis_number_8 = I.AnalysisNumber
, L.Analysis_result_8 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_8
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_8
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_8)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_8 = null
	, L.Analysis_number_8 = null
	, L.Analysis_result_8 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_8
	and I.AnalysisID = @AnalysisID_8
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_8 = null
	, L.Analysis_number_8 = null
	, L.Analysis_result_8 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_8
	and I.AnalysisID = @AnalysisID_8
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_8 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_8 = A.AnalysisID



--############### ANALYSIS 9 ###############

update L
set L.AnalysisID_9 = I.AnalysisID
, L.Analysis_number_9 = I.AnalysisNumber
, L.Analysis_result_9 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_9
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_9
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_9)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_9 = null
	, L.Analysis_number_9 = null
	, L.Analysis_result_9 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_9
	and I.AnalysisID = @AnalysisID_9
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_9 = null
	, L.Analysis_number_9 = null
	, L.Analysis_result_9 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_9
	and I.AnalysisID = @AnalysisID_9
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_9 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_9 = A.AnalysisID




--- Related Organism
Update L
set L.Related_Organism = Urel.LastIdentificationCache
from  @List L
, dbo.IdentificationUnit U
, dbo.IdentificationUnit Urel
where U.IdentificationUnitID = L._IdentificationUnitID
and U.RelatedUnitID = Urel.IdentificationUnitID	
and U.CollectionSpecimenID = Urel.CollectionSpecimenID
	
	
	
-- PROCESSING --- ###############################################################################################################


-- getting all ProcessingID's

declare @ProcessingIDs varchar(4000)
if (@ProcessingID is null)
begin
	declare @Processing table (ProcessingID int Primary key)
	insert into @Processing
	select ProcessingID from Processing
end
else
begin
	insert into @Processing
	select @ProcessingID
end



-- setting the Processing date range that should be shown

declare @StartDate datetime
declare @EndDate datetime

if (ISDATE(@ProcessingStartDate) = 1) begin set @StartDate = @ProcessingStartDate end
else begin set @StartDate = (select MIN(ProcessingDate) from CollectionSpecimenProcessing) end

if (ISDATE(@ProcessingEndDate) = 1) begin set @EndDate = @ProcessingEndDate end
else begin set @EndDate = (select MAX(ProcessingDate) from CollectionSpecimenProcessing) end


-- PROCESSING 1 --- ###############################################################################################################

update L
set L.Processing_date_1 = P.ProcessingDate
, L.ProcessingID_1 = P.ProcessingID
, L.Processing_duration_1 = P.ProcessingDuration
, L.Processing_notes_1 = P.Notes
, L.Processing_Protocoll_1 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID



-- PROCESSING 2 --- ###############################################################################################################

update L
set L.Processing_date_2 = P.ProcessingDate
, L.ProcessingID_2 = P.ProcessingID
, L.Processing_duration_2 = P.ProcessingDuration
, L.Processing_notes_2 = P.Notes
, L.Processing_Protocoll_2 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_1
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID


-- PROCESSING 3 --- ###############################################################################################################

update L
set L.Processing_date_3 = P.ProcessingDate
, L.ProcessingID_3 = P.ProcessingID
, L.Processing_duration_3 = P.ProcessingDuration
, L.Processing_notes_3 = P.Notes
, L.Processing_Protocoll_3 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_2
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID




-- PROCESSING 4 --- ###############################################################################################################

update L
set L.Processing_date_4 = P.ProcessingDate
, L.ProcessingID_4 = P.ProcessingID
, L.Processing_duration_4 = P.ProcessingDuration
, L.Processing_notes_4 = P.Notes
, L.Processing_Protocoll_4 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_3
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID

-- PROCESSING 5 --- ###############################################################################################################


update L
set L.Processing_date_5 = P.ProcessingDate
, L.ProcessingID_5 = P.ProcessingID
, L.Processing_duration_5 = P.ProcessingDuration
, L.Processing_notes_5 = P.Notes
, L.Processing_Protocoll_5 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S 
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.ProcessingDate > L.Processing_date_4
AND S.SpecimenPartID = P.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID




--- Transaction

update L
set L._TransactionID = P.TransactionID
, L.On_loan = P.IsOnLoan
from @List L,
dbo.CollectionSpecimenTransaction P
where L.CollectionSpecimenID = P.CollectionSpecimenID
and L.SpecimenPartID = P.SpecimenPartID
and EXISTS
	(SELECT Tmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenTransaction AS Tmin
	GROUP BY Tmin.CollectionSpecimenID, Tmin.SpecimenPartID
	HAVING (Tmin.CollectionSpecimenID = P.CollectionSpecimenID) 
	AND Tmin.SpecimenPartID = P.SpecimenPartID
	AND (MIN(Tmin.TransactionID) = P.TransactionID))


update L
set L._Transaction = T.TransactionTitle
from @List L,
dbo.[Transaction] T
where L._TransactionID = T.TransactionID
	
                  
RETURN 
END   

GO


















































--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


/*

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.13'
END

GO
*/

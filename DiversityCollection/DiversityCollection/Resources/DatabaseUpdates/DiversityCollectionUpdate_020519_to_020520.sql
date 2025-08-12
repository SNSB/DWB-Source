declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.19'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
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


--#####################################################################################################################
--######   ApplicationEntityDescription   ######################################################################################
--#####################################################################################################################



IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ApplicationEntityDescription_LanguageCode_Enum]') AND parent_object_id = OBJECT_ID(N'[dbo].[ApplicationEntityDescription]'))
ALTER TABLE [dbo].[ApplicationEntityDescription] DROP CONSTRAINT [FK_ApplicationEntityDescription_LanguageCode_Enum]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ApplicationEntityDescription_LogCreatedWhen]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ApplicationEntityDescription] DROP CONSTRAINT [DF_ApplicationEntityDescription_LogCreatedWhen]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ApplicationEntityDescription_LogCreatedBy]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ApplicationEntityDescription] DROP CONSTRAINT [DF_ApplicationEntityDescription_LogCreatedBy]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ApplicationEntityDescription_LogUpdatedWhen]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ApplicationEntityDescription] DROP CONSTRAINT [DF_ApplicationEntityDescription_LogUpdatedWhen]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ApplicationEntityDescription_LogUpdatedBy]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ApplicationEntityDescription] DROP CONSTRAINT [DF_ApplicationEntityDescription_LogUpdatedBy]
END

GO

/****** Object:  Table [dbo].[ApplicationEntityDescription]    Script Date: 04/03/2012 11:38:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationEntityDescription]') AND type in (N'U'))
DROP TABLE [dbo].[ApplicationEntityDescription]
GO



--#####################################################################################################################
--######   Add RowGUID to Log tables and adapt triggers for Replication  ######################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   Analysis   ######################################################################################
--#####################################################################################################################

ALTER TABLE Analysis_log ADD [OnlyHierarchy] [bit] NULL
GO

ALTER TABLE Analysis_log ADD [RowGUID] [uniqueidentifier] NULL
GO


ALTER TRIGGER [trgDelAnalysis] ON [dbo].[Analysis] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 02.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Analysis_Log (AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.AnalysisID, deleted.AnalysisParentID, deleted.DisplayText, deleted.Description, deleted.MeasurementUnit, deleted.Notes, deleted.AnalysisURI, deleted.OnlyHierarchy, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED
GO

ALTER TRIGGER [trgUpdAnalysis] ON [dbo].[Analysis] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 02.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Analysis_Log (AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.AnalysisID, deleted.AnalysisParentID, deleted.DisplayText, deleted.Description, deleted.MeasurementUnit, deleted.Notes, deleted.AnalysisURI, deleted.OnlyHierarchy, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update Analysis
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Analysis, deleted 
where 1 = 1 
AND Analysis.AnalysisID = deleted.AnalysisID
GO


--#####################################################################################################################
--######   Collection   ######################################################################################
--#####################################################################################################################

ALTER TABLE Collection_log ADD [RowGUID] [uniqueidentifier] NULL
GO

ALTER TRIGGER [trgUpdCollection] ON [Collection] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 02.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionID, deleted.CollectionParentID, deleted.CollectionName, deleted.CollectionAcronym, deleted.AdministrativeContactName, deleted.AdministrativeContactAgentURI, deleted.Description, deleted.Location, deleted.CollectionOwner, deleted.DisplayOrder, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update Collection
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Collection, deleted 
where 1 = 1 
AND Collection.CollectionID = deleted.CollectionID
GO


ALTER TRIGGER [trgDelCollection] ON [dbo].[Collection] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 02.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionID, deleted.CollectionParentID, deleted.CollectionName, deleted.CollectionAcronym, deleted.AdministrativeContactName, deleted.AdministrativeContactAgentURI, deleted.Description, deleted.Location, deleted.CollectionOwner, deleted.DisplayOrder, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED
GO

--#####################################################################################################################
--######   CollectionAgent   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionAgent_log ADD [RowGUID] [uniqueidentifier] NULL
GO


ALTER TRIGGER [dbo].[trgDelCollectionAgent] ON [CollectionAgent] 
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
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionAgent_Log (CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, 
DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.CollectorsName, deleted.CollectorsAgentURI, deleted.CollectorsSequence, deleted.CollectorsNumber, 
deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionAgent_Log (CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, 
DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.CollectorsName, deleted.CollectorsAgentURI, deleted.CollectorsSequence, deleted.CollectorsNumber, 
deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



ALTER TRIGGER [dbo].[trgUpdCollectionAgent] ON [dbo].[CollectionAgent] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* setting the version in the main table */ 
DECLARE @Version int
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

/* updating the logging columns */
Update CollectionAgent
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionAgent, deleted 
where 1 = 1 
AND CollectionAgent.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionAgent.CollectorsName = deleted.CollectorsName

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionAgent_Log (CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.CollectorsName, deleted.CollectorsAgentURI, deleted.CollectorsSequence, deleted.CollectorsNumber, deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionAgent_Log (CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.CollectorsName, deleted.CollectorsAgentURI, deleted.CollectorsSequence, deleted.CollectorsNumber, deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



--#####################################################################################################################
--######   CollectionEvent   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionEvent_log ADD [RowGUID] [uniqueidentifier] NULL
GO


ALTER TRIGGER [dbo].[trgDelCollectionEvent] ON [dbo].[CollectionEvent] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.1 */ 
/*  Date: 11.01.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEvent_Log (CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, 
CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionDateCategory, CollectionTime, CollectionTimeSpan, 
LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, Notes, CountryCache, 
DataWithholdingReason, RowGUID, ReferenceDetails, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, 
 LogState) 
SELECT D.CollectionEventID, D.Version, D.SeriesID, D.CollectorsEventNumber, D.CollectionDate, D.CollectionDay, 
D.CollectionMonth, D.CollectionYear, D.CollectionDateSupplement, D.CollectionDateCategory, D.CollectionTime, D.CollectionTimeSpan, 
D.LocalityDescription, D.HabitatDescription, D.ReferenceTitle, D.ReferenceURI, D.CollectingMethod, D.Notes, D.CountryCache, 
D.DataWithholdingReason, D.RowGUID, D.ReferenceDetails, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, 
 'D'
FROM DELETED D

GO


ALTER TRIGGER [dbo].[trgUpdCollectionEvent] ON [dbo].[CollectionEvent]  
 FOR UPDATE AS 
 /*  Created by DiversityWorkbench Administration.  */  
 /*  DiversityWorkbenchMaintenance  2.0.0.3 */  
 /*  Date: 30.08.2007  */  
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
	 LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, LogState)  
	 SELECT D.CollectionEventID, D.Version, D.SeriesID, D.CollectorsEventNumber, D.CollectionDate, 
	 D.CollectionDay, D.CollectionMonth, D.CollectionYear, D.CollectionDateSupplement, D.CollectionDateCategory, D.CollectionTime, 
	 D.CollectionTimeSpan, D.LocalityDescription, D.HabitatDescription, D.ReferenceTitle, D.ReferenceURI, D.ReferenceDetails, 
	 D.CollectingMethod, D.Notes, D.CountryCache, D.DataWithholdingReason, D.RowGUID,  D.LogCreatedWhen, 
	 D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, 'U' 
	 FROM DELETED  D
 END  
 
 Update CollectionEvent set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user 
 FROM CollectionEvent, deleted  
 where 1 = 1  AND CollectionEvent.CollectionEventID = deleted.CollectionEventID

GO



--#####################################################################################################################
--######   CollectionEventImage   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionEventImage_log ADD [RowGUID] [uniqueidentifier] NULL
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
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'D' 
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
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end


GO



--#####################################################################################################################
--######   CollectionEventLocalisation   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionEventLocalisation_log ADD [RowGUID] [uniqueidentifier] NULL
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
ResponsibleAgentURI, RecordingMethod, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, 
deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RecordingMethod, 
deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, 
DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, RecordingMethod, 
Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, 
deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RecordingMethod, 
deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
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
ResponsibleAgentURI, RecordingMethod, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, 
deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RecordingMethod, 
deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, 
DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, RecordingMethod, 
Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, 
deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RecordingMethod, 
deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
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
--######   CollectionEventProperty   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionEventProperty_log ADD [RowGUID] [uniqueidentifier] NULL
GO




ALTER TRIGGER [dbo].[trgDelCollectionEventProperty] ON [dbo].[CollectionEventProperty] 
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
INSERT INTO CollectionEventProperty_Log (CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.PropertyID, deleted.DisplayText, deleted.PropertyURI, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventProperty_Log (CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.PropertyID, deleted.DisplayText, deleted.PropertyURI, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
GO




ALTER TRIGGER [dbo].[trgUpdCollectionEventProperty] ON [dbo].[CollectionEventProperty] 
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
INSERT INTO CollectionEventProperty_Log (CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.PropertyID, deleted.DisplayText, deleted.PropertyURI, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventProperty_Log (CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.PropertyID, deleted.DisplayText, deleted.PropertyURI, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'U' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end

/* updating the logging columns */
Update CollectionEventProperty
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventProperty, deleted 
where 1 = 1 
AND CollectionEventProperty.CollectionEventID = deleted.CollectionEventID
AND CollectionEventProperty.PropertyID = deleted.PropertyID
GO



--#####################################################################################################################
--######   CollectionEventSeries   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionEventSeries_log ADD [RowGUID] [uniqueidentifier] NULL
GO


ALTER TRIGGER [trgUpdCollectionEventSeries] ON [dbo].[CollectionEventSeries] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 05.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeries_Log (SeriesID, SeriesParentID, Description, SeriesCode, Notes, Geography, DateStart, DateEnd, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.SeriesID, deleted.SeriesParentID, deleted.Description, deleted.SeriesCode, deleted.Notes, deleted.Geography, deleted.DateStart, deleted.DateEnd, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionEventSeries
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventSeries, deleted 
where 1 = 1 
AND CollectionEventSeries.SeriesID = deleted.SeriesID
GO

ALTER TRIGGER [trgDelCollectionEventSeries] ON [dbo].[CollectionEventSeries] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 05.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeries_Log (SeriesID, SeriesParentID, Description, SeriesCode, Notes, Geography, DateStart, DateEnd, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.SeriesID, deleted.SeriesParentID, deleted.Description, deleted.SeriesCode, deleted.Notes, deleted.Geography, deleted.DateStart, deleted.DateEnd, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED
GO


--#####################################################################################################################
--######   CollectionEventSeriesImage   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionEventSeriesImage_log ADD [RowGUID] [uniqueidentifier] NULL
GO






ALTER TRIGGER [dbo].[trgDelCollectionEventSeriesImage] ON [dbo].[CollectionEventSeriesImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.4 */ 
/*  Date: 19.02.2008  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeriesImage_Log (SeriesID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.SeriesID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED


GO




ALTER TRIGGER [dbo].[trgUpdCollectionEventSeriesImage] ON [dbo].[CollectionEventSeriesImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.4 */ 
/*  Date: 19.02.2008  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeriesImage_Log (SeriesID, URI, ResourceURI, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.SeriesID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
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
--######   CollectionExternalDatasource   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionExternalDatasource ADD [LogCreatedWhen] [datetime] NULL
GO
ALTER TABLE CollectionExternalDatasource ADD [LogCreatedBy] [nvarchar](50) NULL
GO
ALTER TABLE CollectionExternalDatasource ADD [LogUpdatedWhen] [datetime] NULL
GO
ALTER TABLE CollectionExternalDatasource ADD [LogUpdatedBy] [nvarchar](50) NULL
GO

ALTER TABLE [dbo].[CollectionExternalDatasource] ADD  CONSTRAINT [DF_CollectionExternalDatasource_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[CollectionExternalDatasource] ADD  CONSTRAINT [DF_CollectionExternalDatasource_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[CollectionExternalDatasource] ADD  CONSTRAINT [DF_CollectionExternalDatasource_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionExternalDatasource] ADD  CONSTRAINT [DF_CollectionExternalDatasource_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionExternalDatasource', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionExternalDatasource', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionExternalDatasource', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionExternalDatasource', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO


CREATE TABLE [dbo].[CollectionExternalDatasource_log] 
([ExternalDatasourceID] [int] NULL,
 [ExternalDatasourceName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ExternalDatasourceVersion] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [Rights] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
 [ExternalDatasourceAuthors] [nvarchar] (200) COLLATE Latin1_General_CI_AS NULL,
 [ExternalDatasourceURI] [nvarchar] (300) COLLATE Latin1_General_CI_AS NULL,
 [ExternalDatasourceInstitution] [nvarchar] (300) COLLATE Latin1_General_CI_AS NULL,
 [InternalNotes] [nvarchar] (1500) COLLATE Latin1_General_CI_AS NULL,
 [ExternalAttribute_NameID] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [PreferredSequence] [tinyint] NULL,
 [Disabled] [bit] NULL,
 [RowGUID] [uniqueidentifier] NULL,
 [LogCreatedWhen] [datetime] NULL,
 [LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [datetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_CollectionExternalDatasource_Log_LogState]  DEFAULT ('U'), 
[LogDate] [datetime] NOT NULL CONSTRAINT [DF_CollectionExternalDatasource_Log_LogDate]  DEFAULT (getdate()), 
[LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_CollectionExternalDatasource_Log_LogUser]  DEFAULT (user_name()), 
[LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_CollectionExternalDatasource_Log] PRIMARY KEY CLUSTERED 
([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 

GO




CREATE TRIGGER [trgUpdCollectionExternalDatasource] ON [dbo].[CollectionExternalDatasource] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 03.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionExternalDatasource_Log (ExternalDatasourceID, ExternalDatasourceName, ExternalDatasourceVersion, Rights, ExternalDatasourceAuthors, ExternalDatasourceURI, ExternalDatasourceInstitution, InternalNotes, ExternalAttribute_NameID, PreferredSequence, Disabled, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.ExternalDatasourceID, deleted.ExternalDatasourceName, deleted.ExternalDatasourceVersion, deleted.Rights, deleted.ExternalDatasourceAuthors, deleted.ExternalDatasourceURI, deleted.ExternalDatasourceInstitution, deleted.InternalNotes, deleted.ExternalAttribute_NameID, deleted.PreferredSequence, deleted.Disabled, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionExternalDatasource
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionExternalDatasource, deleted 
where 1 = 1 
AND CollectionExternalDatasource.ExternalDatasourceID = deleted.ExternalDatasourceID

GO



CREATE TRIGGER [trgDelCollectionExternalDatasource] ON [dbo].[CollectionExternalDatasource] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 03.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionExternalDatasource_Log (ExternalDatasourceID, ExternalDatasourceName, ExternalDatasourceVersion, Rights, ExternalDatasourceAuthors, ExternalDatasourceURI, ExternalDatasourceInstitution, InternalNotes, ExternalAttribute_NameID, PreferredSequence, Disabled, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.ExternalDatasourceID, deleted.ExternalDatasourceName, deleted.ExternalDatasourceVersion, deleted.Rights, deleted.ExternalDatasourceAuthors, deleted.ExternalDatasourceURI, deleted.ExternalDatasourceInstitution, deleted.InternalNotes, deleted.ExternalAttribute_NameID, deleted.PreferredSequence, deleted.Disabled, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   CollectionImage   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionImage_log ADD [RowGUID] [uniqueidentifier] NULL
GO



ALTER TRIGGER [dbo].[trgDelCollection] ON [dbo].[Collection] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.CollectionID, deleted.CollectionParentID, deleted.CollectionName, deleted.CollectionAcronym, deleted.AdministrativeContactName, deleted.AdministrativeContactAgentURI, deleted.Description, deleted.Location, deleted.CollectionOwner, deleted.DisplayOrder, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO





ALTER TRIGGER [dbo].[trgUpdCollection] ON [dbo].[Collection] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* updating the logging columns */
Update Collection
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Collection, deleted 
where 1 = 1 
AND Collection.CollectionID = deleted.CollectionID

/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.CollectionID, deleted.CollectionParentID, deleted.CollectionName, deleted.CollectionAcronym, deleted.AdministrativeContactName, deleted.AdministrativeContactAgentURI, deleted.Description, deleted.Location, deleted.CollectionOwner, deleted.DisplayOrder, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED

GO




--#####################################################################################################################
--######   CollectionProject   ######################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   CollectionSpecimen   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimen_log ADD [RowGUID] [uniqueidentifier] NULL
GO


ALTER TRIGGER [trgDelCollectionSpecimen] ON [dbo].[CollectionSpecimen] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 03.04.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimen_Log (CollectionSpecimenID, Version, CollectionEventID, CollectionID, AccessionNumber, AccessionDate, AccessionDay, 
AccessionMonth, AccessionYear, AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, 
LabelTitle, LabelType, LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, 
ReferenceTitle, ReferenceURI, Problems, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, InternalNotes, 
ExternalDatasourceID, ExternalIdentifier, RowGUID, ReferenceDetails,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Version, deleted.CollectionEventID, deleted.CollectionID, deleted.AccessionNumber, deleted.AccessionDate, 
deleted.AccessionDay, deleted.AccessionMonth, deleted.AccessionYear, deleted.AccessionDateSupplement, deleted.AccessionDateCategory, deleted.DepositorsName, 
deleted.DepositorsAgentURI, deleted.DepositorsAccessionNumber, deleted.LabelTitle, deleted.LabelType, deleted.LabelTranscriptionState, 
deleted.LabelTranscriptionNotes, deleted.ExsiccataURI, deleted.ExsiccataAbbreviation, deleted.OriginalNotes, deleted.AdditionalNotes, deleted.ReferenceTitle, 
deleted.ReferenceURI, deleted.Problems, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, 
deleted.InternalNotes, deleted.ExternalDatasourceID, deleted.ExternalIdentifier, deleted.RowGUID, deleted.ReferenceDetails,  'D'
FROM DELETED

GO



ALTER TRIGGER [trgUpdCollectionSpecimen] ON [dbo].[CollectionSpecimen] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 03.04.2012  */ 

if not update(Version) 
BEGIN

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
END 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimen_Log (CollectionSpecimenID, Version, CollectionEventID, CollectionID, AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, ReferenceTitle, ReferenceURI, Problems, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, InternalNotes, ExternalDatasourceID, ExternalIdentifier, RowGUID, ReferenceDetails,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Version, deleted.CollectionEventID, deleted.CollectionID, deleted.AccessionNumber, deleted.AccessionDate, deleted.AccessionDay, deleted.AccessionMonth, deleted.AccessionYear, deleted.AccessionDateSupplement, deleted.AccessionDateCategory, deleted.DepositorsName, deleted.DepositorsAgentURI, deleted.DepositorsAccessionNumber, deleted.LabelTitle, deleted.LabelType, deleted.LabelTranscriptionState, deleted.LabelTranscriptionNotes, deleted.ExsiccataURI, deleted.ExsiccataAbbreviation, deleted.OriginalNotes, deleted.AdditionalNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.Problems, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.InternalNotes, deleted.ExternalDatasourceID, deleted.ExternalIdentifier, deleted.RowGUID, deleted.ReferenceDetails,  'U'
FROM DELETED

END

/* updating the logging columns */
Update CollectionSpecimen
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimen, deleted 
where 1 = 1 
AND CollectionSpecimen.CollectionSpecimenID = deleted.CollectionSpecimenID

GO

--#####################################################################################################################
--######   CollectionSpecimenImage   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenImage_log ADD [RowGUID] [uniqueidentifier] NULL
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
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
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
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, [Description], Notes, DataWithholdingReason, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.[Description], deleted.Notes, deleted.DataWithholdingReason, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
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



--#####################################################################################################################
--######   CollectionSpecimenPart   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenPart_log ADD [RowGUID] [uniqueidentifier] NULL
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
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, Notes, RowGUID,
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, deleted.RowGUID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, Notes, RowGUID, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, deleted.RowGUID, 
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
Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, deleted.RowGUID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, 
Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, deleted.RowGUID, 
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
--######   CollectionSpecimenProcessing   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenProcessing_log ADD [RowGUID] [uniqueidentifier] NULL
GO





ALTER TRIGGER [dbo].[trgDelCollectionSpecimenProcessing] ON [dbo].[CollectionSpecimenProcessing] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


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
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO





ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenProcessing] ON [dbo].[CollectionSpecimenProcessing] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 

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
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenProcessing
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenProcessing, deleted 
where 1 = 1 
AND CollectionSpecimenProcessing.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenProcessing.ProcessingDate = deleted.ProcessingDate
GO


--#####################################################################################################################
--######   CollectionSpecimenRelation   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenRelation_log ADD [RowGUID] [uniqueidentifier] NULL
GO



ALTER TRIGGER [dbo].[trgDelCollectionSpecimenRelation] ON [dbo].[CollectionSpecimenRelation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 18.09.2007  */ 


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
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.RelatedSpecimenURI, deleted.RelatedSpecimenDisplayText, deleted.RelationType, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.Notes, deleted.IsInternalRelationCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.RelatedSpecimenURI, deleted.RelatedSpecimenDisplayText, deleted.RelationType, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.Notes, deleted.IsInternalRelationCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO





ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenRelation] ON [dbo].[CollectionSpecimenRelation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 18.09.2007  */ 

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
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.RelatedSpecimenURI, deleted.RelatedSpecimenDisplayText, deleted.RelationType, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.Notes, deleted.IsInternalRelationCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.RelatedSpecimenURI, deleted.RelatedSpecimenDisplayText, deleted.RelationType, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.Notes, deleted.IsInternalRelationCache, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenRelation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenRelation, deleted 
where 1 = 1 
AND CollectionSpecimenRelation.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenRelation.RelatedSpecimenURI = deleted.RelatedSpecimenURI
GO



--#####################################################################################################################
--######   CollectionSpecimenTransaction   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenTransaction_log ADD [RowGUID] [uniqueidentifier] NULL
GO




ALTER TRIGGER [dbo].[trgDelCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenTransaction_Log (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.TransactionID, deleted.SpecimenPartID, deleted.IsOnLoan, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED

GO




ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenTransaction_Log (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.TransactionID, deleted.SpecimenPartID, deleted.IsOnLoan, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionSpecimenTransaction
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenTransaction, deleted 
where 1 = 1 
AND CollectionSpecimenTransaction.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenTransaction.TransactionID = deleted.TransactionID
AND CollectionSpecimenTransaction.SpecimenPartID = deleted.SpecimenPartID
GO




--#####################################################################################################################
--######   Identification   ######################################################################################
--#####################################################################################################################

ALTER TABLE Identification_log ADD [RowGUID] [uniqueidentifier] NULL
GO





ALTER TRIGGER [dbo].[trgDelIdentification] ON [dbo].[Identification] 
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
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IdentificationSequence, deleted.IdentificationDate, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationYear, deleted.IdentificationDateSupplement, deleted.IdentificationDateCategory, deleted.VernacularTerm, deleted.TaxonomicName, deleted.NameURI, deleted.IdentificationCategory, deleted.IdentificationQualifier, deleted.TypeStatus, deleted.TypeNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IdentificationSequence, deleted.IdentificationDate, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationYear, deleted.IdentificationDateSupplement, deleted.IdentificationDateCategory, deleted.VernacularTerm, deleted.TaxonomicName, deleted.NameURI, deleted.IdentificationCategory, deleted.IdentificationQualifier, deleted.TypeStatus, deleted.TypeNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
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





ALTER TRIGGER [dbo].[trgUpdIdentification] ON [dbo].[Identification] 
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
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)

/* updating the LastIdentificationCache in IdentificationUnit */
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
from IdentificationUnit, Identification a, inserted
where IdentificationUnit.CollectionSpecimenID = inserted.CollectionSpecimenID
and IdentificationUnit.IdentificationUnitID = inserted.IdentificationUnitID
and a.CollectionSpecimenID = inserted.CollectionSpecimenID
and a.IdentificationUnitID = inserted.IdentificationUnitID
and a.IdentificationSequence = 
(select max(b.IdentificationSequence) 
from Identification b
where b.CollectionSpecimenID = a.CollectionSpecimenID
and b.IdentificationUnitID = a.IdentificationUnitID
group by b.IdentificationUnitID, b.CollectionSpecimenID)
and (LastIdentificationCache is null or LastIdentificationCache <> case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end )

/* updating the logging columns */
Update Identification
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Identification, deleted 
where 1 = 1 
AND Identification.CollectionSpecimenID = deleted.CollectionSpecimenID
AND Identification.IdentificationSequence = deleted.IdentificationSequence
AND Identification.IdentificationUnitID = deleted.IdentificationUnitID

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IdentificationSequence, deleted.IdentificationDate, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationYear, deleted.IdentificationDateSupplement, deleted.IdentificationDateCategory, deleted.VernacularTerm, deleted.TaxonomicName, deleted.NameURI, deleted.IdentificationCategory, deleted.IdentificationQualifier, deleted.TypeStatus, deleted.TypeNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IdentificationSequence, deleted.IdentificationDate, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationYear, deleted.IdentificationDateSupplement, deleted.IdentificationDateCategory, deleted.VernacularTerm, deleted.TaxonomicName, deleted.NameURI, deleted.IdentificationCategory, deleted.IdentificationQualifier, deleted.TypeStatus, deleted.TypeNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end


GO



--#####################################################################################################################
--######   [IdentificationUnit]   ######################################################################################
--#####################################################################################################################


ALTER TABLE [dbo].[IdentificationUnit] ADD  [HierarchyCache] [nvarchar](500) NULL
GO

ALTER TABLE [dbo].[IdentificationUnit_log] ADD  [HierarchyCache] [nvarchar](500) NULL
GO

ALTER TABLE [dbo].[IdentificationUnit_log] ADD  [RowGUID] [uniqueidentifier] NULL
GO




ALTER TRIGGER [dbo].[trgDelIdentificationUnit] ON [dbo].[IdentificationUnit] 
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
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState)  
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO





ALTER TRIGGER [dbo].[trgUpdIdentificationUnit] ON [dbo].[IdentificationUnit] 
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
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)


/* updating the logging columns */
Update IdentificationUnit
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnit, deleted 
where 1 = 1 
AND IdentificationUnit.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnit.IdentificationUnitID = deleted.IdentificationUnitID


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState)  
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



--#####################################################################################################################
--######   IdentificationUnitAnalysis   ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[IdentificationUnitAnalysis_log] ADD  [RowGUID] [uniqueidentifier] NULL
GO



ALTER TRIGGER [dbo].[trgDelIdentificationUnitAnalysis] ON [dbo].[IdentificationUnitAnalysis] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 31.08.2007  */ 


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
INSERT INTO IdentificationUnitAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.ExternalAnalysisURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.AnalysisDate, deleted.SpecimenPartID, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.ExternalAnalysisURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.AnalysisDate, deleted.SpecimenPartID, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO





ALTER TRIGGER [dbo].[trgUpdIdentificationUnitAnalysis] ON [dbo].[IdentificationUnitAnalysis] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 31.08.2007  */ 

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
INSERT INTO IdentificationUnitAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.ExternalAnalysisURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.AnalysisDate, deleted.SpecimenPartID, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.ExternalAnalysisURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.AnalysisDate, deleted.SpecimenPartID, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update IdentificationUnitAnalysis
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnitAnalysis, deleted 
where 1 = 1 
AND IdentificationUnitAnalysis.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnitAnalysis.IdentificationUnitID = deleted.IdentificationUnitID
AND IdentificationUnitAnalysis.AnalysisID = deleted.AnalysisID
AND IdentificationUnitAnalysis.AnalysisNumber = deleted.AnalysisNumber

GO



--#####################################################################################################################
--######   IdentificationUnitGeoAnalysis   ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[IdentificationUnitGeoAnalysis_log] ADD  [RowGUID] [uniqueidentifier] NULL
GO


ALTER TRIGGER [dbo].[trgDelIdentificationUnitGeoAnalysis] ON [dbo].[IdentificationUnitGeoAnalysis] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.1 */ 
/*  Date: 22.02.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO IdentificationUnitGeoAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisDate, Geography, Geometry, ResponsibleName, ResponsibleAgentURI, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisDate, deleted.Geography, deleted.Geometry, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, 'D'
FROM DELETED

GO



ALTER TRIGGER [dbo].[trgUpdIdentificationUnitGeoAnalysis] ON [dbo].[IdentificationUnitGeoAnalysis] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.1 */ 
/*  Date: 22.02.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO IdentificationUnitGeoAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisDate, Geography, Geometry, ResponsibleName, ResponsibleAgentURI, Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisDate, deleted.Geography, deleted.Geometry, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, 'U'
FROM DELETED


/* updating the logging columns */
Update IdentificationUnitGeoAnalysis
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnitGeoAnalysis, deleted 
where 1 = 1 
AND IdentificationUnitGeoAnalysis.AnalysisDate = deleted.AnalysisDate
AND IdentificationUnitGeoAnalysis.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnitGeoAnalysis.IdentificationUnitID = deleted.IdentificationUnitID
GO


--#####################################################################################################################
--######   IdentificationUnitInPart   ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[IdentificationUnitInPart_log] ADD  [RowGUID] [uniqueidentifier] NULL
GO


ALTER TRIGGER [dbo].[trgDelIdentificationUnitInPart] ON [dbo].[IdentificationUnitInPart] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


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
INSERT INTO IdentificationUnitInPart_Log (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, DisplayOrder, Description, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.DisplayOrder, deleted.Description, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitInPart_Log (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, DisplayOrder, Description, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.DisplayOrder, deleted.Description, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO




ALTER TRIGGER [dbo].[trgUpdIdentificationUnitInPart] ON [dbo].[IdentificationUnitInPart] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 

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
INSERT INTO IdentificationUnitInPart_Log (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, DisplayOrder, Description, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.DisplayOrder, deleted.Description, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitInPart_Log (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, DisplayOrder, Description, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.DisplayOrder, deleted.Description, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update IdentificationUnitInPart
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnitInPart, deleted 
where 1 = 1 
AND IdentificationUnitInPart.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnitInPart.IdentificationUnitID = deleted.IdentificationUnitID
AND IdentificationUnitInPart.SpecimenPartID = deleted.SpecimenPartID

GO



--#####################################################################################################################
--######   Processing   ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[Processing_log] ADD  [RowGUID] [uniqueidentifier] NULL
GO

ALTER TABLE [dbo].[Processing_log] ADD  [OnlyHierarchy] [bit] NULL
GO



ALTER TRIGGER [dbo].[trgDelProcessing] ON [dbo].[Processing] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO Processing_Log (ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI, OnlyHierarchy, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.ProcessingID, deleted.ProcessingParentID, deleted.DisplayText, deleted.Description, deleted.Notes, deleted.ProcessingURI, deleted.OnlyHierarchy, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO




ALTER TRIGGER [dbo].[trgUpdProcessing] ON [dbo].[Processing] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* updating the logging columns */
Update Processing
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Processing, deleted 
where 1 = 1 
AND Processing.ProcessingID = deleted.ProcessingID

/* saving the original dataset in the logging table */ 
INSERT INTO Processing_Log (ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI, OnlyHierarchy, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.ProcessingID, deleted.ProcessingParentID, deleted.DisplayText, deleted.Description, deleted.Notes, deleted.ProcessingURI, deleted.OnlyHierarchy, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED

GO




--#####################################################################################################################
--######   Transaction   ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[Transaction_log] ADD  [RowGUID] [uniqueidentifier] NULL
GO




ALTER TRIGGER [dbo].[trgDelTransaction] ON [dbo].[Transaction] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Transaction_Log (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, ActualEndDate, 
InternalNotes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.ParentTransactionID, deleted.TransactionType, deleted.TransactionTitle, deleted.ReportingCategory, deleted.AdministratingCollectionID, 
deleted.MaterialDescription, deleted.MaterialCategory, deleted.MaterialCollectors, deleted.FromCollectionID, deleted.FromTransactionPartnerName, 
deleted.FromTransactionPartnerAgentURI, deleted.FromTransactionNumber, deleted.ToCollectionID, deleted.ToTransactionPartnerName, deleted.ToTransactionPartnerAgentURI, 
deleted.ToTransactionNumber, deleted.NumberOfUnits, deleted.Investigator, deleted.TransactionComment, deleted.BeginDate, deleted.AgreedEndDate, deleted.ActualEndDate, 
deleted.InternalNotes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO




ALTER TRIGGER [dbo].[trgUpdTransaction] ON [dbo].[Transaction] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Transaction_Log (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, ActualEndDate, 
InternalNotes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.ParentTransactionID, deleted.TransactionType, deleted.TransactionTitle, deleted.ReportingCategory, deleted.AdministratingCollectionID, 
deleted.MaterialDescription, deleted.MaterialCategory, deleted.MaterialCollectors, deleted.FromCollectionID, deleted.FromTransactionPartnerName, 
deleted.FromTransactionPartnerAgentURI, deleted.FromTransactionNumber, deleted.ToCollectionID, deleted.ToTransactionPartnerName, deleted.ToTransactionPartnerAgentURI, 
deleted.ToTransactionNumber, deleted.NumberOfUnits, deleted.Investigator, deleted.TransactionComment, deleted.BeginDate, deleted.AgreedEndDate, deleted.ActualEndDate, 
deleted.InternalNotes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update [Transaction]
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM [Transaction], deleted 
where 1 = 1 
AND [Transaction].TransactionID = deleted.TransactionID

GO



--#####################################################################################################################
--######   TransactionDocument   ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[TransactionDocument_log] ADD  [RowGUID] [uniqueidentifier] NULL
GO



ALTER TRIGGER [dbo].[trgDelTransactionDocument] ON [dbo].[TransactionDocument] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionDocument_Log (TransactionID, Date, TransactionText, InternalNotes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.Date, deleted.TransactionText, deleted.InternalNotes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO




ALTER TRIGGER [dbo].[trgUpdTransactionDocument] ON [dbo].[TransactionDocument] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionDocument_Log (TransactionID, Date, TransactionText, InternalNotes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.Date, deleted.TransactionText, deleted.InternalNotes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update TransactionDocument
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM TransactionDocument, deleted 
where 1 = 1 
AND TransactionDocument.TransactionID = deleted.TransactionID
AND TransactionDocument.Date = deleted.Date
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.20'
END

GO

select [dbo].[Version] ()  


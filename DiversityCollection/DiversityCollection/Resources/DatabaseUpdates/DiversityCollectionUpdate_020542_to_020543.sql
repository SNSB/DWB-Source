
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.42'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Adding Fields to image tables   ############################################################################
--######   CollectionEventImage   #####################################################################################
--#####################################################################################################################

ALTER TABLE CollectionEventImage ADD Title nvarchar(500) NULL
GO
ALTER TABLE CollectionEventImage_log ADD Title nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'Title'
GO


ALTER TABLE CollectionEventImage ADD IPR nvarchar(500) NULL
GO
ALTER TABLE CollectionEventImage_log ADD IPR nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Intellectual Property Rights; the rights given to persons over the creations of their minds' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'IPR'
GO


ALTER TABLE CollectionEventImage ADD CreatorAgent nvarchar(500) NULL
GO
ALTER TABLE CollectionEventImage_log ADD CreatorAgent nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person or organization originally creating the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'CreatorAgent'
GO


ALTER TABLE CollectionEventImage ADD CreatorAgentURI varchar(255) NULL
GO
ALTER TABLE CollectionEventImage_log ADD CreatorAgentURI varchar(255) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'CreatorAgentURI'
GO


ALTER TABLE CollectionEventImage ADD CopyrightStatement nvarchar(500) NULL
GO
ALTER TABLE CollectionEventImage_log ADD CopyrightStatement nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notice about rights held in and over the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'CopyrightStatement'
GO


ALTER TABLE CollectionEventImage ADD LicenseType nvarchar(500) NULL
GO
ALTER TABLE CollectionEventImage_log ADD LicenseType nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of an official or legal permission to do or own a specified thing, e. g. Creative Common licenses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'LicenseType'
GO


ALTER TABLE CollectionEventImage ADD InternalNotes nvarchar(500) NULL
GO
ALTER TABLE CollectionEventImage_log ADD InternalNotes nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes that should not be published e.g. on websites' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO



ALTER TABLE CollectionEventImage ADD LicenseHolder  nvarchar(500) NULL
GO
ALTER TABLE CollectionEventImage_log ADD LicenseHolder  nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'LicenseHolder'
GO

ALTER TABLE CollectionEventImage ADD LicenseHolderAgentURI  nvarchar(500) NULL
GO
ALTER TABLE CollectionEventImage_log ADD LicenseHolderAgentURI  nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to a module containing futher information about the person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'LicenseHolderAgentURI'
GO

ALTER TABLE CollectionEventImage ADD LicenseYear  nvarchar(50) NULL
GO
ALTER TABLE CollectionEventImage_log ADD LicenseYear  nvarchar(50) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of license declaration' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventImage', @level2type=N'COLUMN',@level2name=N'LicenseYear'
GO




ALTER TRIGGER [dbo].[trgDelCollectionEventImage] ON [dbo].[CollectionEventImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 15.11.2013  */ 


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
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  
@Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, 
CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
GO



ALTER TRIGGER [dbo].[trgUpdCollectionEventImage] ON [dbo].[CollectionEventImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 15.11.2013  */ 

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
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  
@Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventImage_Log (CollectionEventID, URI, ResourceURI, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, 
CollectionEvent.Version, 'U' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end

/* updating the logging columns */
Update CollectionEventImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventImage, deleted 
where 1 = 1 
AND CollectionEventImage.CollectionEventID = deleted.CollectionEventID
AND CollectionEventImage.URI = deleted.URI
GO





--#####################################################################################################################
--######   CollectionImage   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionImage ADD Title nvarchar(500) NULL
GO
ALTER TABLE CollectionImage_log ADD Title nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'Title'
GO


ALTER TABLE CollectionImage ADD IPR nvarchar(500) NULL
GO
ALTER TABLE CollectionImage_log ADD IPR nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Intellectual Property Rights; the rights given to persons over the creations of their minds' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'IPR'
GO


ALTER TABLE CollectionImage ADD CreatorAgent nvarchar(500) NULL
GO
ALTER TABLE CollectionImage_log ADD CreatorAgent nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person or organization originally creating the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'CreatorAgent'
GO


ALTER TABLE CollectionImage ADD CreatorAgentURI varchar(255) NULL
GO
ALTER TABLE CollectionImage_log ADD CreatorAgentURI varchar(255) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'CreatorAgentURI'
GO


ALTER TABLE CollectionImage ADD CopyrightStatement nvarchar(500) NULL
GO
ALTER TABLE CollectionImage_log ADD CopyrightStatement nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notice about rights held in and over the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'CopyrightStatement'
GO


ALTER TABLE CollectionImage ADD LicenseType nvarchar(500) NULL
GO
ALTER TABLE CollectionImage_log ADD LicenseType nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of an official or legal permission to do or own a specified thing, e. g. Creative Common licenses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LicenseType'
GO


ALTER TABLE CollectionImage ADD InternalNotes nvarchar(500) NULL
GO
ALTER TABLE CollectionImage_log ADD InternalNotes nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes that should not be published e.g. on websites' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO



ALTER TABLE CollectionImage ADD LicenseHolder  nvarchar(500) NULL
GO
ALTER TABLE CollectionImage_log ADD LicenseHolder  nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LicenseHolder'
GO

ALTER TABLE CollectionImage ADD LicenseHolderAgentURI  nvarchar(500) NULL
GO
ALTER TABLE CollectionImage_log ADD LicenseHolderAgentURI  nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to a module containing futher information about the person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LicenseHolderAgentURI'
GO

ALTER TABLE CollectionImage ADD LicenseYear  nvarchar(50) NULL
GO
ALTER TABLE CollectionImage_log ADD LicenseYear  nvarchar(50) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of license declaration' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LicenseYear'
GO



ALTER TRIGGER [dbo].[trgDelCollectionImage] ON [dbo].[CollectionImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 15.11.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionImage_Log (CollectionID, URI, ImageType, Notes, DataWithholdingReason, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, Description, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogState) 
SELECT deleted.CollectionID, deleted.URI, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.Description, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  'D'
FROM DELETED

GO





ALTER TRIGGER [dbo].[trgUpdCollectionImage] ON [dbo].[CollectionImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 15.11.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionImage_Log (CollectionID, URI, ImageType, Notes, DataWithholdingReason, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, Description, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogState) 
SELECT deleted.CollectionID, deleted.URI, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.Description, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionImage, deleted 
where 1 = 1 
AND CollectionImage.CollectionID = deleted.CollectionID
AND CollectionImage.URI = deleted.URI
GO


--#####################################################################################################################
--######   CollectionEventSeriesImage   ######################################################################################
--#####################################################################################################################




ALTER TABLE CollectionEventSeriesImage ADD Title nvarchar(500) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD Title nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'Title'
GO


ALTER TABLE CollectionEventSeriesImage ADD IPR nvarchar(500) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD IPR nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Intellectual Property Rights; the rights given to persons over the creations of their minds' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'IPR'
GO


ALTER TABLE CollectionEventSeriesImage ADD CreatorAgent nvarchar(500) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD CreatorAgent nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person or organization originally creating the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'CreatorAgent'
GO


ALTER TABLE CollectionEventSeriesImage ADD CreatorAgentURI varchar(255) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD CreatorAgentURI varchar(255) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'CreatorAgentURI'
GO


ALTER TABLE CollectionEventSeriesImage ADD CopyrightStatement nvarchar(500) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD CopyrightStatement nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notice about rights held in and over the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'CopyrightStatement'
GO


ALTER TABLE CollectionEventSeriesImage ADD LicenseType nvarchar(500) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD LicenseType nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of an official or legal permission to do or own a specified thing, e. g. Creative Common licenses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'LicenseType'
GO


ALTER TABLE CollectionEventSeriesImage ADD InternalNotes nvarchar(500) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD InternalNotes nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes that should not be published e.g. on websites' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO


ALTER TABLE CollectionEventSeriesImage ADD LicenseHolder  nvarchar(500) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD LicenseHolder  nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'LicenseHolder'
GO

ALTER TABLE CollectionEventSeriesImage ADD LicenseHolderAgentURI  nvarchar(500) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD LicenseHolderAgentURI  nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to a module containing futher information about the person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'LicenseHolderAgentURI'
GO

ALTER TABLE CollectionEventSeriesImage ADD LicenseYear  nvarchar(50) NULL
GO
ALTER TABLE CollectionEventSeriesImage_log ADD LicenseYear  nvarchar(50) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of license declaration' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesImage', @level2type=N'COLUMN',@level2name=N'LicenseYear'
GO


/****** Object:  Trigger [dbo].[trgDelCollectionEventSeriesImage]    Script Date: 11/18/2013 10:40:43 ******/

ALTER TRIGGER [dbo].[trgDelCollectionEventSeriesImage] ON [dbo].[CollectionEventSeriesImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.11.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeriesImage_Log (SeriesID, URI, ResourceURI, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogState) 
SELECT deleted.SeriesID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  'D'
FROM DELETED

GO



/****** Object:  Trigger [dbo].[trgUpdCollectionEventSeriesImage]    Script Date: 11/18/2013 10:40:59 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionEventSeriesImage] ON [dbo].[CollectionEventSeriesImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.11.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeriesImage_Log (SeriesID, URI, ResourceURI, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogState) 
SELECT deleted.SeriesID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  'U'
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
--######   CollectionSpecimenImage   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionSpecimenImage ADD Title nvarchar(500) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD Title nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'Title'
GO


ALTER TABLE CollectionSpecimenImage ADD IPR nvarchar(500) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD IPR nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Intellectual Property Rights; the rights given to persons over the creations of their minds' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'IPR'
GO


ALTER TABLE CollectionSpecimenImage ADD CreatorAgent nvarchar(500) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD CreatorAgent nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person or organization originally creating the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'CreatorAgent'
GO


ALTER TABLE CollectionSpecimenImage ADD CreatorAgentURI varchar(255) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD CreatorAgentURI varchar(255) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'CreatorAgentURI'
GO


ALTER TABLE CollectionSpecimenImage ADD CopyrightStatement nvarchar(500) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD CopyrightStatement nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notice about rights held in and over the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'CopyrightStatement'
GO


ALTER TABLE CollectionSpecimenImage ADD LicenseType nvarchar(500) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD LicenseType nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of an official or legal permission to do or own a specified thing, e. g. Creative Common licenses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'LicenseType'
GO


ALTER TABLE CollectionSpecimenImage ADD InternalNotes nvarchar(500) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD InternalNotes nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes that should not be published e.g. on websites' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO


ALTER TABLE CollectionSpecimenImage ADD LicenseHolder  nvarchar(500) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD LicenseHolder  nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'LicenseHolder'
GO

ALTER TABLE CollectionSpecimenImage ADD LicenseHolderAgentURI  nvarchar(500) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD LicenseHolderAgentURI  nvarchar(500) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to a module containing futher information about the person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'LicenseHolderAgentURI'
GO

ALTER TABLE CollectionSpecimenImage ADD LicenseYear  nvarchar(50) NULL
GO
ALTER TABLE CollectionSpecimenImage_log ADD LicenseYear  nvarchar(50) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of license declaration' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'LicenseYear'
GO




/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenImage]    Script Date: 11/18/2013 10:38:21 ******/

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.11.2013  */ 


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
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenImage]    Script Date: 11/18/2013 10:38:45 ******/


ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.11.2013  */ 

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
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, CollectionSpecimen.Version, 'U' 
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
--######   CollSpecimenImageType_Enum   ######################################################################################
--#####################################################################################################################


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'a still image of a document or paperwork associated with a specimen, observation or unit, e. g. herbarium label, file card'
      ,[DisplayText] = 'documentation'
      ,[DisplayEnable] = 1
      ,[InternalNotes] = 'replacement for label'
 WHERE Code = 'label'
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'photographic still image of a specimen, observation or unit; e. g. macro images of an object'
 WHERE Code = 'photograph'
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [DisplayEnable] = 0
 WHERE Code = 'photography'
GO


INSERT INTO [CollSpecimenImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes]
           ,[ParentCode])
     VALUES
           ('LM photograph'
           ,'photographic still image of a specimen, observation or unit from light microscopy'
           ,'LM photograph'
           ,6
           ,1
           ,'replacement for label'
           ,'image')
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'photographic still image of a specimen, observation or unit from scanning electron microscopy'
      ,[DisplayText] = 'SEM photograph'
 WHERE Code = 'SEM image'
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'photographic still image of a specimen, observation or unit from transmission electron microscopy'
      ,[DisplayText] = 'TEM photograph'
 WHERE Code = 'TEM image'
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'original line or color drawing and painting of a specimen, observation or unit'
      ,[DisplayText] = 'drawing, painting'
 WHERE Code = 'drawing'
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'still image of any raster or vector  format without any further information regarding the content type, e. g. graphic designs, plans and maps, vector graphics 2D, 3D models'
 WHERE Code = 'image'
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'e. g. audio file related to a specimen, observation or unit'
      ,[DisplayText] = 'sound'
 WHERE Code = 'audio'
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'e. g. audio file of spoken comments related to a specimen, observation or unit that might be deleted after transcription into the database'
      ,[DisplayText] = 'sound /recorded speech'
 WHERE Code = 'audio (for transcription)'
GO


UPDATE [CollSpecimenImageType_Enum]
   SET [Description] = 'e. g. video file related to an specimen, observation or unit'
      ,[DisplayText] = 'moving image'
 WHERE Code = 'video'
GO


INSERT INTO [CollSpecimenImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('supporting files'
           ,'text-xml or text-WKT encoded files, assigned to multimedia objects'
           ,'supporting files'
           ,14
           ,1)
GO


--#####################################################################################################################
--######   CollEventImageType_Enum   ######################################################################################
--#####################################################################################################################

INSERT INTO [CollEventImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('documentation'
           ,'a still image of a document or paperwork associated with an event, e. g. field books'
           ,'documentation'
           ,6
           ,1)
GO


UPDATE [CollEventImageType_Enum]
   SET [Description] = 'photographic still image of an event; e. g. trap, trawl net'
 WHERE Code = 'photograph'
GO


UPDATE [CollEventImageType_Enum]
   SET [Description] = 'photographic still image of an event taken from the air / an elevated position'
      ,[DisplayText] = 'aerial photograph'
 WHERE Code = 'air photograph'
GO

UPDATE [CollEventImageType_Enum]
   SET [Description] = 'photographic still image of an event'
      ,[DisplayText] = 'landscape photograph'
 WHERE Code = 'landscape photograph'
GO



INSERT INTO [CollEventImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('drawing,  painting'
           ,'original line or color drawing of an event'
           ,'drawing,  painting'
           ,8
           ,1)
GO


UPDATE [CollEventImageType_Enum]
   SET [Description] = 'still image of any raster or vector format without any further information regarding the content type, e. g. graphic designs, plans and maps, vector graphics 2D, 3D models'
 WHERE Code = 'image'
GO


UPDATE [CollEventImageType_Enum]
   SET [Description] = 'audio file related to an event'
      ,[DisplayText] = 'sound'
 WHERE Code = 'audio'
GO


UPDATE [CollEventImageType_Enum]
   SET [Description] = 'audio file of spoken comments related to an event that might be deleted after transcription into the database'
      ,[DisplayText] = 'sound /recorded speech'
 WHERE Code = 'audio (for transcription)'
GO


UPDATE [CollEventImageType_Enum]
   SET [Description] = 'e. g. video file related to an event'
      ,[DisplayText] = 'moving image'
 WHERE Code = 'video'
GO


INSERT INTO [CollEventImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('supporting files'
           ,'text-xml or text-WKT encoded files, assigned to multimedia objects'
           ,'supporting files'
           ,14
           ,1)
GO



UPDATE [CollEventImageType_Enum]
   SET [DisplayEnable] = 0
 WHERE Code = 'map'
GO



--#####################################################################################################################
--######   CollEventSeriesImageType_Enum   ######################################################################################
--#####################################################################################################################


INSERT INTO [CollEventSeriesImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('documentation'
           ,'a still image of a document or paperwork associated with an event series, e. g. field books'
           ,'documentation'
           ,6
           ,1)
GO


INSERT INTO [CollEventSeriesImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('photograph'
           ,'photographic still image of an event series; e. g. trap, trawl net'
           ,'photograph'
           ,1
           ,1)
GO


UPDATE [CollEventSeriesImageType_Enum]
   SET [Description] = 'photographic still image of an event series taken from the air / an elevated position'
      ,[DisplayText] = 'aerial photograph'
 WHERE Code = 'air photograph'
GO


UPDATE [CollEventSeriesImageType_Enum]
   SET [Description] = 'photographic still image of an event series'
      ,[DisplayText] = 'landscape photograph'
 WHERE Code = 'landscape photograph'
GO



INSERT INTO [CollEventSeriesImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('drawing,  painting'
           ,'original line or color drawing of an event series'
           ,'drawing,  painting'
           ,8
           ,1)
GO


UPDATE [CollEventSeriesImageType_Enum]
   SET [Description] = 'still image of any raster or vector format without any further information regarding the content type, e. g. graphic designs, plans and maps, vector graphics 2D, 3D models'
 WHERE Code = 'image'
GO


UPDATE [CollEventSeriesImageType_Enum]
   SET [Description] = 'audio file related to an event or event series'
      ,[DisplayText] = 'sound'
 WHERE Code = 'audio'
GO


UPDATE [CollEventSeriesImageType_Enum]
   SET [Description] = 'audio file of spoken comments related to an event or event series that might be deleted after transcription into the database'
      ,[DisplayText] = 'sound /recorded speech'
 WHERE Code = 'audio (for transcription)'
GO


INSERT INTO [CollEventSeriesImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('moving image'
           ,'e. g. video file related to an event series'
           ,'moving image'
           ,12
           ,1)
GO



INSERT INTO [CollEventSeriesImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('supporting files'
           ,'text-xml or text-WKT encoded files, assigned to multimedia objects'
           ,'supporting files'
           ,14
           ,1)
GO



UPDATE [CollEventSeriesImageType_Enum]
   SET [DisplayEnable] = 0
 WHERE Code = 'map'
GO


--#####################################################################################################################
--######   [Entity]   ######################################################################################
--#####################################################################################################################

update e set [DisplayText] = 'Content type', [Description] = 'information assigned to a multimedia resource, e.g. document, photograph, sound, video, supporting files'
--select *
  FROM [dbo].[EntityRepresentation]
  e where e.Entity like '%specimenimage%type'
  and e.EntityContext = 'General'
  and e.LanguageCode = 'en-US'

GO


update e set [DisplayText] = 'Content type', [Description] = 'information assigned to a multimedia resource of an event, e.g. document/ field book, air photograph, sound, video, supporting files '
--select *
  FROM [dbo].[EntityRepresentation]
  e where e.Entity like '%eventimage%type'
  and e.EntityContext = 'General'
  and e.LanguageCode = 'en-US'
  
GO


update e set [DisplayText] = 'Content type', [Description] = 'information assigned to a multimedia resource of an event series, e.g. document/ field book, air photograph, sound, video, supporting files '
--select *
  FROM [dbo].[EntityRepresentation]
  e where e.Entity like '%eventseriesimage%type'
  and e.EntityContext = 'General'
  and e.LanguageCode = 'en-US'
  
GO


update e set [DisplayText] = 'Resources of the collection event', Abbreviation = 'Resource', [Description] = 'The resources, e.g., multimedia files, assigned to the site of the collection event'
      --select *
  FROM [dbo].[EntityRepresentation]
  e where e.Entity like 'CollectionEventImage'
  and e.EntityContext = 'General'
  and e.LanguageCode = 'en-US'
  and Not e.Entity like '%.%'

GO


update e set [DisplayText] = 'Resources of the collection event series', Abbreviation = 'Resource', [Description] = 'The resources, e.g., multimedia files, assigned of a collection event series, e.g. an expedition'
      --select *
  FROM [dbo].[EntityRepresentation]
  e where e.Entity like 'CollectionEventSeriesImage'
  and e.EntityContext = 'General'
  and e.LanguageCode = 'en-US'
  and Not e.Entity like '%.%'

GO


update e set [DisplayText] = 'Resources of the collection', Abbreviation = 'Resource', [Description] = 'The resources showing the collection'
      --select *
  FROM [dbo].[EntityRepresentation]
  e where e.Entity like 'CollectionImage'
  and e.EntityContext = 'General'
  and e.LanguageCode = 'en-US'
  and Not e.Entity like '%.%'

GO


update e set [DisplayText] = 'Resources of the specimen', Abbreviation = 'Resource', [Description] = 'The resources, e.g., multimedia files, assigned of a specimen, specimen part or of an identification unit within this specimen'
      --select *
  FROM [dbo].[EntityRepresentation]
  e where e.Entity like 'CollectionSpecimenImage'
  and e.EntityContext = 'General'
  and e.LanguageCode = 'en-US'
  and Not e.Entity like '%.%'

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.43'
END

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.93'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   ProjectList  ###############################################################################################
--#####################################################################################################################


ALTER VIEW [dbo].[ProjectList]
AS
SELECT        TOP (100) PERCENT dbo.ProjectProxy.Project, dbo.ProjectUser.ProjectID, CAST(dbo.ProjectUser.ReadOnly AS int) AS ReadOnly
FROM            dbo.ProjectUser INNER JOIN
                         dbo.ProjectProxy ON dbo.ProjectUser.ProjectID = dbo.ProjectProxy.ProjectID
WHERE        (dbo.ProjectUser.LoginName = USER_NAME())
GROUP BY dbo.ProjectProxy.Project, dbo.ProjectUser.ProjectID, CAST(dbo.ProjectUser.ReadOnly AS int)
ORDER BY dbo.ProjectProxy.Project
GO

--#####################################################################################################################
--######   CollectionSpecimenPartDescription  #########################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenPartDescription](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenPartID] [int] NOT NULL,
	[PartDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionTermURI] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL CONSTRAINT [DF_CollectionSpecimenPartDescription_LogCreatedWhen]  DEFAULT (getdate()),
	[LogCreatedBy] [nvarchar](50) NULL CONSTRAINT [DF_CollectionSpecimenPartDescription_LogCreatedBy]  DEFAULT (suser_sname()),
	[LogUpdatedWhen] [datetime] NULL CONSTRAINT [DF_CollectionSpecimenPartDescription_LogUpdatedWhen]  DEFAULT (getdate()),
	[LogUpdatedBy] [nvarchar](50) NULL CONSTRAINT [DF_CollectionSpecimenPartDescription_LogUpdatedBy]  DEFAULT (suser_sname()),
	[RowGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_CollectionSpecimenPartDescription_RowGUID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_CollectionSpecimenPartDescription] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenPartID] ASC,
	[PartDescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CollectionSpecimenPartDescription]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenPartDescription_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionSpecimenPartDescription] CHECK CONSTRAINT [FK_CollectionSpecimenPartDescription_CollectionSpecimenPart]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of CollectionSpecimen (= foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID of the part of the collection specimen (= part of primary key). ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'SpecimenPartID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the description (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'PartDescriptionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The descrition of the part. Cached value if DescriptionTermURI is used' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to a external datasource like a webservice or the module DiversityScientificTerms where the description is documented' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'DescriptionTermURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about this description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO


--#####################################################################################################################
--######   CollectionSpecimenPartDescription_log  #####################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenPartDescription_log](
	[CollectionSpecimenID] [int] NULL,
	[SpecimenPartID] [int] NULL,
	[PartDescriptionID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionTermURI] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenPartDescription_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CollectionSpecimenPartDescription_log] ADD  CONSTRAINT [DF_CollectionSpecimenPartDescription_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionSpecimenPartDescription_log] ADD  CONSTRAINT [DF_CollectionSpecimenPartDescription_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionSpecimenPartDescription_log] ADD  CONSTRAINT [DF_CollectionSpecimenPartDescription_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


--#####################################################################################################################
--######   trgDelCollectionSpecimenPartDescription    #################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionSpecimenPartDescription] ON [dbo].[CollectionSpecimenPartDescription] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.7 */ 
/*  Date: 7/25/2016  */ 


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
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.PartDescriptionID, deleted.Description, deleted.DescriptionTermURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.PartDescriptionID, deleted.Description, deleted.DescriptionTermURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO


--#####################################################################################################################
--######   trgUpdCollectionSpecimenPartDescription    #################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionSpecimenPartDescription] ON [dbo].[CollectionSpecimenPartDescription] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.7 */ 
/*  Date: 7/25/2016  */ 

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
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.PartDescriptionID, deleted.Description, deleted.DescriptionTermURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.PartDescriptionID, deleted.Description, deleted.DescriptionTermURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenPartDescription
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM CollectionSpecimenPartDescription, deleted 
where 1 = 1 
AND CollectionSpecimenPartDescription.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenPartDescription.PartDescriptionID = deleted.PartDescriptionID
AND CollectionSpecimenPartDescription.SpecimenPartID = deleted.SpecimenPartID
GO


--#####################################################################################################################
--######   CollectionEventID_AvailableNotReadOnly    ##################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionEventID_AvailableNotReadOnly]
AS
SELECT        S.CollectionEventID
FROM            dbo.CollectionSpecimen AS S INNER JOIN
                         dbo.CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN
                         dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
WHERE        (U.LoginName = USER_NAME()) AND (U.ReadOnly = 0) AND (NOT (S.CollectionEventID IS NULL))
GROUP BY S.CollectionEventID
UNION
SELECT        E.CollectionEventID
FROM       dbo.CollectionEvent E LEFT OUTER JOIN dbo.CollectionSpecimen AS S ON S.CollectionEventID = E.CollectionEventID 
WHERE       (S.CollectionEventID IS NULL)
GO





--#####################################################################################################################
--######   DocumentType in TransactionDocument    #####################################################################
--#####################################################################################################################

ALTER Table [dbo].[TransactionDocument] ADD DocumentType nvarchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the document' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionDocument', @level2type=N'COLUMN',@level2name=N'DocumentType'
GO

ALTER Table [dbo].[TransactionDocument_log] ADD DocumentType nvarchar(255) NULL
GO


/****** Object:  Trigger [dbo].[trgDelTransactionDocument]    Script Date: 26.07.2016 13:56:17 ******/
ALTER TRIGGER [dbo].[trgDelTransactionDocument] ON [dbo].[TransactionDocument] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionDocument_Log (TransactionID, Date, TransactionText, DocumentURI, DocumentType, DisplayText, InternalNotes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.Date, deleted.TransactionText, deleted.DocumentURI, deleted.DocumentType, deleted.DisplayText, deleted.InternalNotes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO


/****** Object:  Trigger [dbo].[trgUpdTransactionDocument]    Script Date: 07/08/2014 11:58:35 ******/
ALTER TRIGGER [dbo].[trgUpdTransactionDocument] ON [dbo].[TransactionDocument] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionDocument_Log (TransactionID, Date, TransactionText, DocumentURI, DocumentType, DisplayText, InternalNotes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.Date, deleted.TransactionText, deleted.DocumentURI, deleted.DocumentType, deleted.DisplayText, deleted.InternalNotes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update TransactionDocument
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM TransactionDocument, deleted 
where 1 = 1 
AND TransactionDocument.TransactionID = deleted.TransactionID
AND TransactionDocument.Date = deleted.Date

GO


--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.06' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.94'
END

GO


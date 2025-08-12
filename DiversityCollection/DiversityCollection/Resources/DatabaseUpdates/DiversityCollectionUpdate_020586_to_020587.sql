declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.86'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   CollectionSpecimenReference   ##############################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenReference](
	[CollectionSpecimenID] [int] NOT NULL,
	[ReferenceID] [int]  IDENTITY(1,1) NOT NULL,
	[ReferenceTitle] [nvarchar](400) NOT NULL,
	[ReferenceURI] [varchar](500) NULL,
	[IdentificationUnitID] [int] NULL,
	[SpecimenPartID] [int] NULL,
	[ReferenceDetails] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenReference] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[ReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] ADD  CONSTRAINT [DF_CollectionSpecimenReference_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] ADD  CONSTRAINT [DF__Collectio__LogCr__0F6ECE9E]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] ADD  CONSTRAINT [DF_CollectionSpecimenReference_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] ADD  CONSTRAINT [DF__Collectio__LogUp__11571710]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] ADD  CONSTRAINT [DF__Collectio__RowGU__124B3B49]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenReference_CollectionSpecimen] FOREIGN KEY([CollectionSpecimenID])
REFERENCES [dbo].[CollectionSpecimen] ([CollectionSpecimenID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] CHECK CONSTRAINT [FK_CollectionSpecimenReference_CollectionSpecimen]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenReference_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] CHECK CONSTRAINT [FK_CollectionSpecimenReference_CollectionSpecimenPart]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenReference_IdentificationUnit] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID])
REFERENCES [dbo].[IdentificationUnit] ([CollectionSpecimenID], [IdentificationUnitID])
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] CHECK CONSTRAINT [FK_CollectionSpecimenReference_IdentificationUnit]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to unique ID of collection specimen record (part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique reference ID for the reference record (part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'ReferenceID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the publication related to the specimen or parts of it. Note that this is only a cached value where ReferenceURI is present' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the reference, e.g. a connection to the module DiversityReferences' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If relation refers to a certain organism within a specimen, the ID of an IdentificationUnit (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'IdentificationUnitID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the relation refers to a part of a specimen, the ID of a related part (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'SpecimenPartID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The exact location within the reference, e.g. pages, plates' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent (person or organization) responsible for this entry.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'ResponsibleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or organisation responsible for the data (see e.g. module DiversityAgents)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A reference related to the collection specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference'
GO



GRANT INSERT ON CollectionSpecimenReference TO [Editor]
GO
GRANT UPDATE ON CollectionSpecimenReference TO [Editor]
GO
GRANT DELETE ON CollectionSpecimenReference TO [Editor]
GO
GRANT SELECT ON CollectionSpecimenReference TO [User]
GO


--#####################################################################################################################
--######   CollectionSpecimenReference_log   ##########################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenReference_log](
	[CollectionSpecimenID] [int] NULL,
	[ReferenceID] [int] NULL,
	[ReferenceTitle] [nvarchar](400) NULL,
	[ReferenceURI] [varchar](500) NULL,
	[IdentificationUnitID] [int] NULL,
	[SpecimenPartID] [int] NULL,
	[ReferenceDetails] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
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
 CONSTRAINT [PK_CollectionSpecimenReference_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CollectionSpecimenReference_log] ADD  CONSTRAINT [DF_CollectionSpecimenReference_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference_log] ADD  CONSTRAINT [DF_CollectionSpecimenReference_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference_log] ADD  CONSTRAINT [DF_CollectionSpecimenReference_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT INSERT ON CollectionSpecimenReference_Log TO [Editor]
GO
GRANT SELECT ON CollectionSpecimenReference_Log TO [Editor]
GO

--#####################################################################################################################
--######   trgDelCollectionSpecimenReference   ########################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionSpecimenReference] ON [dbo].[CollectionSpecimenReference] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/20/2016  */ 


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
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ReferenceTitle, deleted.ReferenceID, deleted.ReferenceURI, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ReferenceID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO


--#####################################################################################################################
--######   trgUpdCollectionSpecimenReference   ########################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionSpecimenReference] ON [dbo].[CollectionSpecimenReference] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/20/2016  */ 

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
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ReferenceID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ReferenceID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM CollectionSpecimenReference, deleted 
where 1 = 1 
AND CollectionSpecimenReference.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenReference.ReferenceTitle = deleted.ReferenceTitle
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.87'
END

GO


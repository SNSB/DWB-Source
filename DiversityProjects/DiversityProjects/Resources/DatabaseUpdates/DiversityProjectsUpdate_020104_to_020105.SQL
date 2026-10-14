
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.04'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ProjectReference Citation   ################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectReference] ADD [Citation] nvarchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the reference should be cited' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'Citation'
GO

ALTER TABLE [dbo].[ProjectReference_log] ADD [Citation] nvarchar(255) NULL
GO

/****** Object:  Trigger [dbo].[trgDelProjectReference]    Script Date: 10.06.2016 15:05:24 ******/
ALTER TRIGGER [dbo].[trgDelProjectReference] ON [dbo].[ProjectReference] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Citation, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Citation, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO

/****** Object:  Trigger [dbo].[trgUpdProjectReference]    Script Date: 10.06.2016 15:05:54 ******/
ALTER TRIGGER [dbo].[trgUpdProjectReference] ON [dbo].[ProjectReference] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Citation, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Citation, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectReference, deleted 
where 1 = 1 
AND ProjectReference.ProjectID = deleted.ProjectID
AND ProjectReference.ReferenceTitle = deleted.ReferenceTitle
GO



--#####################################################################################################################
--######   ProjectIdentifierType   ####################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectIdentifierType](
	[Type] [nvarchar](50) NOT NULL,
	[ParentType] [nvarchar](50) NULL,
	[URL] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectIdentifierType] PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_LogUpdatedWhen_1]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_LogUpdatedBy_1]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_RowGUID]  DEFAULT (newid()) FOR [RowGUID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of project identifiers (primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The superior type of this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'ParentType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL providing with further informations about this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description of this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes about the type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of an external identier, e.g. DOI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType'
GO



--#####################################################################################################################
--######   ProjectIdentifierType_log   ################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ProjectIdentifierType_log](
	[Type] [nvarchar](50) NULL,
	[ParentType] [nvarchar](50) NULL,
	[URL] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectIdentifierType_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProjectIdentifierType_log] ADD  CONSTRAINT [DF_ProjectIdentifierType_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[ProjectIdentifierType_log] ADD  CONSTRAINT [DF_ProjectIdentifierType_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ProjectIdentifierType_log] ADD  CONSTRAINT [DF_ProjectIdentifierType_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


--#####################################################################################################################
--######   ProjectIdentifierType Trigger   ############################################################################
--#####################################################################################################################


/****** Object:  Trigger [dbo].[trgDelProjectIdentifierType]    Script Date: 10.06.2016 15:09:53 ******/
CREATE TRIGGER [dbo].[trgDelProjectIdentifierType] ON [dbo].[ProjectIdentifierType] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectIdentifierType_Log (Type, ParentType, URL, Description, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.Type, deleted.ParentType, deleted.URL, deleted.Description, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

/****** Object:  Trigger [dbo].[trgUpdProjectIdentifierType]    Script Date: 10.06.2016 15:10:31 ******/
CREATE TRIGGER [dbo].[trgUpdProjectIdentifierType] ON [dbo].[ProjectIdentifierType] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectIdentifierType_Log (Type, ParentType, URL, Description, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.Type, deleted.ParentType, deleted.URL, deleted.Description, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectIdentifierType
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectIdentifierType, deleted 
where 1 = 1 
AND ProjectIdentifierType.Type = deleted.Type
GO



--#####################################################################################################################
--######   ProjectIdentifier   ########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectIdentifier](
	[ProjectID] [int] NOT NULL,
	[Identifier] [nvarchar](255) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[URL] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectIdentifier] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[ProjectIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProjectIdentifier_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectIdentifier] CHECK CONSTRAINT [FK_ProjectIdentifier_Project]
GO

ALTER TABLE [dbo].[ProjectIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProjectIdentifier_ProjectIdentifierType] FOREIGN KEY([Type])
REFERENCES [dbo].[ProjectIdentifierType] ([Type])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ProjectIdentifier] CHECK CONSTRAINT [FK_ProjectIdentifier_ProjectIdentifierType]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The identifier itself' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'Identifier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the identifier as defined in table ExternalIdentifierType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further informations about the identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO


--#####################################################################################################################
--######   ProjectIdentifier_log   ####################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectIdentifier_log](
	[ProjectID] [int] NULL,
	[Identifier] [nvarchar](255) NULL,
	[Type] [nvarchar](50) NULL,
	[URL] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectIdentifier_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProjectIdentifier_log] ADD  CONSTRAINT [DF_ProjectIdentifier_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[ProjectIdentifier_log] ADD  CONSTRAINT [DF_ProjectIdentifier_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ProjectIdentifier_log] ADD  CONSTRAINT [DF_ProjectIdentifier_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO

--#####################################################################################################################
--######   ProjectIdentifier Trigger   ################################################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelProjectIdentifier]    Script Date: 10.06.2016 15:13:23 ******/
CREATE TRIGGER [dbo].[trgDelProjectIdentifier] ON [dbo].[ProjectIdentifier] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectIdentifier_Log (ProjectID, Identifier, Type, URL, Notes, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Identifier, deleted.Type, deleted.URL, deleted.Notes, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO

/****** Object:  Trigger [dbo].[trgUpdProjectIdentifier]    Script Date: 10.06.2016 15:13:37 ******/
CREATE TRIGGER [dbo].[trgUpdProjectIdentifier] ON [dbo].[ProjectIdentifier] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectIdentifier_Log (ProjectID, Identifier, Type, URL, Notes, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Identifier, deleted.Type, deleted.URL, deleted.Notes, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectIdentifier
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectIdentifier, deleted 
where 1 = 1 
AND ProjectIdentifier.Identifier = deleted.Identifier
AND ProjectIdentifier.ProjectID = deleted.ProjectID
GO



--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.02' END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.05'
END

GO


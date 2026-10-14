
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.00.05'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [ProjectReference]   ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[ProjectReference](
	[ProjectID] [int] NOT NULL,
	[ReferenceTitle] [nvarchar](255) NOT NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[Notes] [nvarchar](255) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectReference] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReferenceTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_LogInsertedBy]  DEFAULT (user_name()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[ProjectReference]  WITH CHECK ADD  CONSTRAINT [FK_ProjectReference_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectReference] CHECK CONSTRAINT [FK_ProjectReference_Project]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of table Project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the publication where information about the project was published. Note this is only a cached value where ReferenceURI is present' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of reference where information about the project, e.g. referring to the module DiversityReferences' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details within the reference, e.g. pages' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The references related to the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference'
GO





/****** Object:  Table [dbo].[ProjectReference_log]    Script Date: 05.09.2014 11:27:51 ******/

CREATE TABLE [dbo].[ProjectReference_log](
	[ProjectID] [int] NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[Notes] [nvarchar](255) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectReference_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProjectReference_log] ADD  CONSTRAINT [DF_ProjectReference_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ProjectReference_log] ADD  CONSTRAINT [DF_ProjectReference_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


/****** Object:  Trigger [dbo].[trgDelProjectReference]    Script Date: 05.09.2014 11:28:21 ******/
CREATE TRIGGER [dbo].[trgDelProjectReference] ON [dbo].[ProjectReference] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.0.7 */ 
/*  Date: 05.09.2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED

GO


/****** Object:  Trigger [dbo].[trgUpdProjectReference]    Script Date: 05.09.2014 11:28:37 ******/
CREATE TRIGGER [dbo].[trgUpdProjectReference] ON [dbo].[ProjectReference] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.0.7 */ 
/*  Date: 05.09.2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM ProjectReference, deleted 
where 1 = 1 
AND ProjectReference.ProjectID = deleted.ProjectID
AND ProjectReference.ReferenceTitle = deleted.ReferenceTitle
GO





--#####################################################################################################################
--######   [VersionClient]   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.00.08' END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.00.06'
END

GO


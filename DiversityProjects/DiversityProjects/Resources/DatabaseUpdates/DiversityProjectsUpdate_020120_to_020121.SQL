
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.20'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   ProjectArchiveMimeType_Enum   ##############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectArchiveMimeType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[URL] [nvarchar](500) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectArchiveMimeType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectArchiveMimeType_Enum] ADD  CONSTRAINT [DF_ProjectArchiveMimeType_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further information about this entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO



--#####################################################################################################################
--######   ProjectArchiveFormat_Enum   ################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectArchiveFormat_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[URL] [nvarchar](500) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectArchiveFormat_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectArchiveFormat_Enum] ADD  CONSTRAINT [DF_ProjectArchiveFormat_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further information about this entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO

--#####################################################################################################################
--######   ProjectArchive   ###########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectArchive](
	[ProjectID] [int] NOT NULL,
	[ArchiveID] [int] IDENTITY(1,1) NOT NULL,
	[ParentArchiveID] [int] NULL,
	[Format] [nvarchar](50) NULL,
	[MimeType] [nvarchar](50) NULL,
	[StorageLocation] [nvarchar](500) NULL,
	[ExternalStorageLocation] [nvarchar](500) NULL,
	[FileSHA512Hash] [nvarchar](529) NULL,
	[FileSize] [int] NULL,
	[TransferDate] [datetime] NULL,
	[PublicationDate] [date] NULL,
	[DataWithholdingReason] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[Published] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectArchive] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ArchiveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_TransferDate]  DEFAULT (getdate()) FOR [TransferDate]
GO

ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_Published]  DEFAULT ((0)) FOR [Published]
GO

ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[ProjectArchive]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchive_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectArchive] CHECK CONSTRAINT [FK_ProjectArchive_Project]
GO

ALTER TABLE [dbo].[ProjectArchive]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchive_ProjectArchive] FOREIGN KEY([ProjectID], [ParentArchiveID])
REFERENCES [dbo].[ProjectArchive] ([ProjectID], [ArchiveID])
GO

ALTER TABLE [dbo].[ProjectArchive] CHECK CONSTRAINT [FK_ProjectArchive_ProjectArchive]
GO

ALTER TABLE [dbo].[ProjectArchive]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchive_ProjectArchiveFormat_Enum] FOREIGN KEY([Format])
REFERENCES [dbo].[ProjectArchiveFormat_Enum] ([Code])
GO

ALTER TABLE [dbo].[ProjectArchive] CHECK CONSTRAINT [FK_ProjectArchive_ProjectArchiveFormat_Enum]
GO

ALTER TABLE [dbo].[ProjectArchive]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchive_ProjectArchiveMimeType_Enum] FOREIGN KEY([MimeType])
REFERENCES [dbo].[ProjectArchiveMimeType_Enum] ([Code])
GO

ALTER TABLE [dbo].[ProjectArchive] CHECK CONSTRAINT [FK_ProjectArchive_ProjectArchiveMimeType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'ArchiveID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the parent archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'ParentArchiveID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The format of the archive,e.g. SDD, EML, ABCD etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'Format'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The mime type of the archive, e.g. zip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'MimeType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The path or URI of the archive file' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'StorageLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The path or URI of the archive file as stored on an external server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'ExternalStorageLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The hash value generated from the file' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'FileSHA512Hash'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Size of the file in bytes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'FileSize'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the data were transferred from the productive database (Last date and time when changes may have happend)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date when the archive is published' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'PublicationDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The reason for locking the data from publication. If empty data may be published' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is published and no further changes are allowed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'Published'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Archives of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive'
GO


--#####################################################################################################################
--######   ProjectArchive_log   #######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectArchive_log](
	[ProjectID] [int] NULL,
	[ArchiveID] [int] NULL,
	[ParentArchiveID] [int] NULL,
	[Format] [nvarchar](50) NULL,
	[MimeType] [nvarchar](50) NULL,
	[StorageLocation] [nvarchar](500) NULL,
	[ExternalStorageLocation] [nvarchar](500) NULL,
	[FileSHA512Hash] [nvarchar](529) NULL,
	[FileSize] [int] NULL,
	[TransferDate] [datetime] NULL,
	[PublicationDate] [date] NULL,
	[DataWithholdingReason] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[Published] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectArchive_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectArchive_log] ADD  CONSTRAINT [DF_ProjectArchive_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[ProjectArchive_log] ADD  CONSTRAINT [DF_ProjectArchive_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ProjectArchive_log] ADD  CONSTRAINT [DF_ProjectArchive_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO

--#####################################################################################################################
--######   trgDelProjectArchive   #####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectArchive] ON [dbo].[ProjectArchive] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  4.1.11 */ 
/*  Date: 21.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectArchive_Log (ProjectID, ArchiveID, ParentArchiveID, Format, MimeType, StorageLocation, ExternalStorageLocation, FileSHA512Hash, FileSize, TransferDate, PublicationDate, DataWithholdingReason, Notes, Published, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.ProjectID, D.ArchiveID, D.ParentArchiveID, D.Format, D.MimeType, D.StorageLocation, D.ExternalStorageLocation, D.FileSHA512Hash, D.FileSize, D.TransferDate, D.PublicationDate, D.DataWithholdingReason, D.Notes, D.Published, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
GO

ALTER TABLE [dbo].[ProjectArchive] ENABLE TRIGGER [trgDelProjectArchive]
GO

--#####################################################################################################################
--######   trgUpdProjectArchive   #####################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgUpdProjectArchive] ON [dbo].[ProjectArchive] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  4.1.11 */ 
/*  Date: 21.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectArchive_Log (ProjectID, ArchiveID, ParentArchiveID, Format, MimeType, StorageLocation, ExternalStorageLocation, FileSHA512Hash, FileSize, TransferDate, PublicationDate, DataWithholdingReason, Notes, Published, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.ProjectID, D.ArchiveID, D.ParentArchiveID, D.Format, D.MimeType, D.StorageLocation, D.ExternalStorageLocation, D.FileSHA512Hash, D.FileSize, D.TransferDate, D.PublicationDate, D.DataWithholdingReason, D.Notes, D.Published, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM ProjectArchive T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.ArchiveID = D.ArchiveID
AND T.ProjectID = D.ProjectID
GO

ALTER TABLE [dbo].[ProjectArchive] ENABLE TRIGGER [trgUpdProjectArchive]
GO


--#####################################################################################################################
--######   ProjectArchiveIdentifier   #################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectArchiveIdentifier](
	[ProjectID] [int] NOT NULL,
	[ArchiveID] [int] NOT NULL,
	[Identifier] [nvarchar](255) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectArchiveIdentifier] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ArchiveID] ASC,
	[Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchiveIdentifier_ProjectArchive] FOREIGN KEY([ProjectID], [ArchiveID])
REFERENCES [dbo].[ProjectArchive] ([ProjectID], [ArchiveID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] CHECK CONSTRAINT [FK_ProjectArchiveIdentifier_ProjectArchive]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchiveIdentifier_ProjectIdentifier] FOREIGN KEY([ProjectID], [Identifier])
REFERENCES [dbo].[ProjectIdentifier] ([ProjectID], [Identifier])
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] CHECK CONSTRAINT [FK_ProjectArchiveIdentifier_ProjectIdentifier]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'ArchiveID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The identifier if archive is linked to an identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'Identifier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier of archives of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier'
GO


--#####################################################################################################################
--######   ProjectArchiveIdentifier_log   #############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectArchiveIdentifier_log](
	[ProjectID] [int] NULL,
	[ArchiveID] [int] NULL,
	[Identifier] [nvarchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectArchiveIdentifier_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier_log] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier_log] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier_log] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO

--#####################################################################################################################
--######   trgDelProjectArchiveIdentifier   ###########################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectArchiveIdentifier] ON [dbo].[ProjectArchiveIdentifier] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  4.1.11 */ 
/*  Date: 21.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectArchiveIdentifier_Log (ProjectID, ArchiveID, Identifier, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.ProjectID, D.ArchiveID, D.Identifier, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] ENABLE TRIGGER [trgDelProjectArchiveIdentifier]
GO


--#####################################################################################################################
--######   trgUpdProjectArchiveIdentifier   ###########################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectArchiveIdentifier] ON [dbo].[ProjectArchiveIdentifier] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  4.1.11 */ 
/*  Date: 21.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectArchiveIdentifier_Log (ProjectID, ArchiveID, Identifier, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.ProjectID, D.ArchiveID, D.Identifier, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM ProjectArchiveIdentifier T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.ArchiveID = D.ArchiveID
AND T.Identifier = D.Identifier
AND T.ProjectID = D.ProjectID
GO

ALTER TABLE [dbo].[ProjectArchiveIdentifier] ENABLE TRIGGER [trgUpdProjectArchiveIdentifier]
GO


--#####################################################################################################################
--######   New columns for Project (Archive)   ########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'Project' and c.COLUMN_NAME = 'EmbargoDate') = 0
begin
	alter table Project add [EmbargoDate] [date] NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date until which the data should not be published, made available to the public' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'EmbargoDate'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'Project' and c.COLUMN_NAME = 'CreateArchive') = 0
begin
	alter table Project add [CreateArchive] [bit] NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If an archive e.g. by a task schedule should be created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'CreateArchive'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'Project' and c.COLUMN_NAME = 'ArchiveProtocol') = 0
begin
	alter table Project add [ArchiveProtocol] [nvarchar](max) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the last archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ArchiveProtocol'
end


--#####################################################################################################################
--######   New roles for Agent    #####################################################################################
--#####################################################################################################################

if (select count(*) from ProjectAgentRole_Enum where Code = 'Content Contact') = 0
begin
INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Content Contact'
           ,'Content Contact'
           ,'Content Contact'
           ,1)
end

if (select count(*) from ProjectAgentRole_Enum where Code = 'Technical Contact') = 0
begin
INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Technical Contact'
           ,'Technical Contact'
           ,'Technical Contact'
           ,1)
end

GO

if (select count(*) from ProjectAgentRole_Enum where Code = 'Source Institution') = 0
begin
INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Source Institution'
           ,'Source Institution'
           ,'Source Institution'
           ,1)
end

GO

--#####################################################################################################################
--######   ProjectChildNodes   ########################################################################################
--#####################################################################################################################


ALTER  FUNCTION [dbo].[ProjectChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL
)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL
)
declare @i int;
   RETURN
END


GO


ALTER  FUNCTION [dbo].[ProjectChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL
)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL
)
declare @i int;
/*
   */
INSERT @TempItem (  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol) 
	SELECT  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol
	FROM Project WHERE ProjectParentID = @ID 
   DECLARE HierarchyCursor  CURSOR for
   select ProjectID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem ( ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol) 
	select  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol
	from dbo.ProjectChildNodes (@ParentID) where ProjectID not in (select ProjectID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor 
 INSERT @ItemList ( ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol) 
   SELECT distinct  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol
   FROM @TempItem ORDER BY ProjectTitle
   RETURN
END


GO

--#####################################################################################################################
--######   ProjectHierarchy   #########################################################################################
--#####################################################################################################################


ALTER  FUNCTION [dbo].[ProjectHierarchy] (@ProjectID int)  
RETURNS @ProjectList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL
)
/*
Returns a table that lists all the items related to the given item.
SELECT * FROM dbo.ProjectHierarchy(3)
MW 02.01.2006
*/
AS
BEGIN
declare @TopID int
declare @i int
set @TopID = (select ProjectParentID from Project where ProjectID = @ProjectID) 
set @i = (select count(*) from Project where ProjectID = @ProjectID)
if (@TopID is null )
	set @TopID =  @ProjectID
else	
	begin
	while (@i > 0)
		begin
		set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
		set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
		end
	set @TopID = @ProjectID
	end
   INSERT @ProjectList
   SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol
   FROM Project
   WHERE Project.ProjectID = @TopID

   INSERT @ProjectList (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol)
   SELECT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol 
	FROM dbo.ProjectChildNodes (@TopID)
   RETURN
END


GO




--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.03.02' END
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.21'
END

GO


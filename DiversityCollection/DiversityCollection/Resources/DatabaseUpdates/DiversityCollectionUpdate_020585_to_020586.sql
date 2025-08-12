declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.85'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   ProjectDataLastChanges        ##############################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[ProjectDataLastChanges] (@ProjectID int)  
RETURNS datetime
AS  
/*
retrieval of the last update in data of a project
--Test
select dbo.[ProjectDataLastChanges](3)
*/
BEGIN 
declare @LastChanges datetime
declare @Temp datetime

-- last changes in table CollectionSpecimen
set @LastChanges = (select max(S.[LogUpdatedWhen]) 
from [dbo].[CollectionSpecimen] S, [dbo].[CollectionProject] P
where S.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProjectID = @ProjectID)

-- last changes in table CollectionEvent
set @LastChanges = (select max(E.[LogUpdatedWhen]) 
from [dbo].[CollectionEvent] E, [dbo].[CollectionSpecimen] S, [dbo].[CollectionProject] P
where S.CollectionSpecimenID = P.CollectionSpecimenID
and S.CollectionEventID = E.CollectionEventID
and P.ProjectID = @ProjectID)
if (@Temp > @LastChanges)
	set @LastChanges = @Temp

-- last adding to the project
set @Temp = (select max(P.[LogUpdatedWhen]) 
from [dbo].[CollectionProject] P
where P.ProjectID = @ProjectID)
if (@Temp > @LastChanges)
	set @LastChanges = @Temp

-- last removal from the project
set @Temp = (select max(P.[LogDate]) 
from [dbo].[CollectionProject_log] P
where P.ProjectID = @ProjectID)
if (@Temp > @LastChanges)
	set @LastChanges = @Temp

return @LastChanges
END

GO

grant execute on dbo.[ProjectDataLastChanges] to [User] 
go




--#####################################################################################################################
--######   ExternalIdentifierType        ##############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ExternalIdentifierType](
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
 CONSTRAINT [PK_ExternalIdentifierType] PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ExternalIdentifierType] ADD  CONSTRAINT [DF_ExternalIdentifierType_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[ExternalIdentifierType] ADD  CONSTRAINT [DF_ExternalIdentifierType_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[ExternalIdentifierType] ADD  CONSTRAINT [DF_ExternalIdentifierType_LogUpdatedWhen_1]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ExternalIdentifierType] ADD  CONSTRAINT [DF_ExternalIdentifierType_LogUpdatedBy_1]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ExternalIdentifierType] ADD  CONSTRAINT [DF_ExternalIdentifierType_RowGUID]  DEFAULT (newid()) FOR [RowGUID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of external identifiers (primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The superior type of this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'ParentType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL providing further informations about this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description of this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes about the type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of an external identier, e.g. DOI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifierType'
GO


GRANT INSERT ON ExternalIdentifierType TO [Administrator]
GO
GRANT UPDATE ON ExternalIdentifierType TO [Administrator]
GO
GRANT DELETE ON ExternalIdentifierType TO [Administrator]
GO
GRANT SELECT ON ExternalIdentifierType TO [User]
GO

--#####################################################################################################################
--######   ExternalIdentifierType_log    ##############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ExternalIdentifierType_log](
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
 CONSTRAINT [PK_ExternalIdentifierType_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ExternalIdentifierType_log] ADD  CONSTRAINT [DF_ExternalIdentifierType_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[ExternalIdentifierType_log] ADD  CONSTRAINT [DF_ExternalIdentifierType_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ExternalIdentifierType_log] ADD  CONSTRAINT [DF_ExternalIdentifierType_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT INSERT ON ExternalIdentifierType_log TO [Administrator]
GO
GRANT SELECT ON ExternalIdentifierType_log TO [Editor]
GO


--#####################################################################################################################
--######   trgDelExternalIdentifierType              ##################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelExternalIdentifierType] ON [dbo].[ExternalIdentifierType] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/24/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ExternalIdentifierType_Log (Type, ParentType, URL, Description, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.Type, deleted.ParentType, deleted.URL, deleted.Description, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO



--#####################################################################################################################
--######   trgUpdExternalIdentifierType              ##################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdExternalIdentifierType] ON [dbo].[ExternalIdentifierType] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/24/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ExternalIdentifierType_Log (Type, ParentType, URL, Description, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.Type, deleted.ParentType, deleted.URL, deleted.Description, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ExternalIdentifierType
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ExternalIdentifierType, deleted 
where 1 = 1 
AND ExternalIdentifierType.Type = deleted.Type
GO




--#####################################################################################################################
--######   ExternalIdentifier            ##############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ExternalIdentifier](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ReferencedTable] [nvarchar](128) NOT NULL,
	[ReferencedID] [int] NOT NULL,
	[Type] [nvarchar](50) NULL,
	[Identifier] [nvarchar](500) NULL,
	[URL] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ExternalIdentifier] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ExternalIdentifier] ADD  CONSTRAINT [DF_ExternalIdentifier_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[ExternalIdentifier] ADD  CONSTRAINT [DF_ExternalIdentifier_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[ExternalIdentifier] ADD  CONSTRAINT [DF_ExternalIdentifier_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ExternalIdentifier] ADD  CONSTRAINT [DF_ExternalIdentifier_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ExternalIdentifier] ADD  CONSTRAINT [DF_ExternalIdentifier_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[ExternalIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ExternalIdentifier_ExternalIdentifierType] FOREIGN KEY([Type])
REFERENCES [dbo].[ExternalIdentifierType] ([Type])
GO

ALTER TABLE [dbo].[ExternalIdentifier] CHECK CONSTRAINT [FK_ExternalIdentifier_ExternalIdentifierType]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the identifier (Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the table the external identifier refers to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'ReferencedTable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the data set in the table the external identifier refers to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'ReferencedID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the identifier as defined in table ExternalIdentifierType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'Identifier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further informations about the identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An external identier related to a dataset, e.g. a DOI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExternalIdentifier'
GO



GRANT INSERT ON ExternalIdentifierType TO [Editor]
GO
GRANT UPDATE ON ExternalIdentifierType TO [Editor]
GO
GRANT DELETE ON ExternalIdentifierType TO [Editor]
GO
GRANT SELECT ON ExternalIdentifierType TO [User]
GO


--#####################################################################################################################
--######   ExternalIdentifier_log        ##############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ExternalIdentifier_log](
	[ID] [int] NULL,
	[ReferencedTable] [nvarchar](128) NULL,
	[ReferencedID] [int] NULL,
	[Type] [nvarchar](50) NULL,
	[Identifier] [nvarchar](500) NULL,
	[URL] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ExternalIdentifier_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ExternalIdentifier_log] ADD  CONSTRAINT [DF_ExternalIdentifier_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[ExternalIdentifier_log] ADD  CONSTRAINT [DF_ExternalIdentifier_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ExternalIdentifier_log] ADD  CONSTRAINT [DF_ExternalIdentifier_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT INSERT ON ExternalIdentifier_log TO [Editor]
GO
GRANT SELECT ON ExternalIdentifier_log TO [Editor]
GO




--#####################################################################################################################
--######   trgDelExternalIdentifier                  ##################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelExternalIdentifier] ON [dbo].[ExternalIdentifier] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/24/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ExternalIdentifier_Log (ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.ID, deleted.ReferencedTable, deleted.ReferencedID, deleted.Type, deleted.Identifier, deleted.URL, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   trgUpdExternalIdentifier                  ##################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdExternalIdentifier] ON [dbo].[ExternalIdentifier] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/24/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ExternalIdentifier_Log (ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.ID, deleted.ReferencedTable, deleted.ReferencedID, deleted.Type, deleted.Identifier, deleted.URL, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ExternalIdentifier
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ExternalIdentifier, deleted 
where 1 = 1 
AND ExternalIdentifier.ID = deleted.ID
GO



--#####################################################################################################################
--######   Transfer of ExternalIdentifierType        ##################################################################
--#####################################################################################################################

DELETE FROM ExternalIdentifierType
GO

INSERT INTO [dbo].[ExternalIdentifierType]
           ([Type]
           ,[ParentType]
           ,[Description]
           ,[InternalNotes])
SELECT [Code]
      ,[ParentCode]
      ,[Description]
      ,[InternalNotes]
  FROM [dbo].[AnnotationType_Enum]
  WHERE Code not in ('Annotation', 'Problem', 'Reference')
  AND [ParentCode] IS NULL
GO

INSERT INTO [dbo].[ExternalIdentifierType]
           ([Type]
           ,[ParentType]
           ,[Description]
           ,[InternalNotes])
SELECT [Code]
      ,[ParentCode]
      ,[Description]
      ,[InternalNotes]
  FROM [dbo].[AnnotationType_Enum]
  WHERE Code not in ('Annotation', 'Problem', 'Reference')
  AND NOT [ParentCode] IS NULL


--#####################################################################################################################
--######   Transfer of Annotation into ExternalIdentifier     #########################################################
--#####################################################################################################################
DELETE FROM ExternalIdentifier
GO

begin try
	BEGIN TRAN
		INSERT INTO [dbo].[ExternalIdentifier]
				   ([ReferencedTable]
				   ,[ReferencedID]
				   ,[Type]
				   ,[Identifier])
		select A.[ReferencedTable], a.ReferencedID, A.AnnotationType, A.Annotation from [dbo].[Annotation] A
		WHERE A.AnnotationType not in ('Annotation', 'Problem', 'Reference')

		DELETE A from [dbo].[Annotation] A
		WHERE A.AnnotationType not in ('Annotation', 'Problem', 'Reference')
	COMMIT TRAN 
end try
begin catch
	ROLLBACK TRAN
end catch
GO

--#####################################################################################################################
--######   Clear AnnotationType_Enum if possible     ##################################################################
--#####################################################################################################################

IF (SELECT COUNT(*) FROM Annotation A WHERE A.AnnotationType NOT IN ('Annotation', 'Problem', 'Reference')) = 0
begin
	DELETE A FROM [AnnotationType_Enum] A WHERE CODE NOT IN ('Annotation', 'Problem', 'Reference')
end

GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.86'
END

GO


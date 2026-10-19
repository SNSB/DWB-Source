
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.13'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   AgentExternalDatabase - Logging columns  ###################################################################
--#####################################################################################################################


ALTER TABLE [dbo].[AgentExternalDatabase] ADD [LogInsertedBy] [nvarchar](50) NULL;
GO
ALTER TABLE [dbo].[AgentExternalDatabase] ADD [LogInsertedWhen] [smalldatetime] NULL;
GO
ALTER TABLE [dbo].[AgentExternalDatabase] ADD [LogUpdatedBy] [nvarchar](50) NULL;
GO
ALTER TABLE [dbo].[AgentExternalDatabase] ADD [LogUpdatedWhen] [smalldatetime] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO


--#####################################################################################################################
--######   AgentExternalDatabase_log  #################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentExternalDatabase_log](
	[ExternalDatabaseID] [int] NULL,
	[ExternalDatabaseName] [nvarchar](100) NULL,
	[ExternalDatabaseVersion] [nvarchar](255) NULL,
	[Rights] [nvarchar](500) NULL,
	[ExternalDatabaseAuthors] [nvarchar](200) NULL,
	[ExternalDatabaseURI] [nvarchar](300) NULL,
	[ExternalDatabaseInstitution] [nvarchar](300) NULL,
	[InternalNotes] [nvarchar](1500) NULL,
	[ExternalAttribute_AgentID] [nvarchar](255) NULL,
	[PreferredSequence] [tinyint] NULL,
	[Disabled] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentExternalDatabase_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentExternalDatabase_log] ADD  CONSTRAINT [DF_AgentExternalDatabase_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[AgentExternalDatabase_log] ADD  CONSTRAINT [DF_AgentExternalDatabase_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[AgentExternalDatabase_log] ADD  CONSTRAINT [DF_AgentExternalDatabase_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


--#####################################################################################################################
--######   trgDelAgentExternalDatabase  ###############################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelAgentExternalDatabase] ON [dbo].[AgentExternalDatabase] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 13.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO AgentExternalDatabase_Log (ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, ExternalDatabaseURI, ExternalDatabaseInstitution, InternalNotes, ExternalAttribute_AgentID, PreferredSequence, Disabled, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogState) 
SELECT D.ExternalDatabaseID, D.ExternalDatabaseName, D.ExternalDatabaseVersion, D.Rights, D.ExternalDatabaseAuthors, D.ExternalDatabaseURI, D.ExternalDatabaseInstitution, D.InternalNotes, D.ExternalAttribute_AgentID, D.PreferredSequence, D.Disabled, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) ,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
GO

ALTER TABLE [dbo].[AgentExternalDatabase] ENABLE TRIGGER [trgDelAgentExternalDatabase]
GO

--#####################################################################################################################
--######   trgUpdAgentExternalDatabase  ###############################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdAgentExternalDatabase] ON [dbo].[AgentExternalDatabase] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 13.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO AgentExternalDatabase_Log (ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, ExternalDatabaseURI, ExternalDatabaseInstitution, InternalNotes, ExternalAttribute_AgentID, PreferredSequence, Disabled, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogState) 
SELECT D.ExternalDatabaseID, D.ExternalDatabaseName, D.ExternalDatabaseVersion, D.Rights, D.ExternalDatabaseAuthors, D.ExternalDatabaseURI, D.ExternalDatabaseInstitution, D.InternalNotes, D.ExternalAttribute_AgentID, D.PreferredSequence, D.Disabled, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) ,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM AgentExternalDatabase T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.ExternalDatabaseID = D.ExternalDatabaseID
GO

ALTER TABLE [dbo].[AgentExternalDatabase] ENABLE TRIGGER [trgUpdAgentExternalDatabase]
GO


--#####################################################################################################################
--######   AgentExternalID_log  #######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentExternalID_log](
	[AgentID] [int] NULL,
	[ExternalDatabaseID] [int] NULL,
	[ExternalAgentURI] [varchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentExternalID_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentExternalID_log] ADD  CONSTRAINT [DF_AgentExternalID_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[AgentExternalID_log] ADD  CONSTRAINT [DF_AgentExternalID_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[AgentExternalID_log] ADD  CONSTRAINT [DF_AgentExternalID_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO

--#####################################################################################################################
--######   trgDelAgentExternalID  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelAgentExternalID] ON [dbo].[AgentExternalID] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 13.02.2020  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) ,  @Version,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) , V.Version, 'D' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) , -1, 'D' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end
GO

ALTER TABLE [dbo].[AgentExternalID] ENABLE TRIGGER [trgDelAgentExternalID]
GO

--#####################################################################################################################
--######   trgUpdAgentExternalID  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdAgentExternalID] ON [dbo].[AgentExternalID] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 13.02.2020  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) ,  @Version,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) , V.Version, 'U' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) , -1, 'U' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM AgentExternalID T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.AgentID = D.AgentID
AND T.ExternalDatabaseID = D.ExternalDatabaseID
GO

ALTER TABLE [dbo].[AgentExternalID] ENABLE TRIGGER [trgUpdAgentExternalID]
GO




--#####################################################################################################################
--######   AgentHierarchyOther  #######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentHierarchyOther](
	[AgentID] [int] NOT NULL,
	[HierarchyID] [int] IDENTITY(1,1) NOT NULL,
	[AgentParentID] [int] NOT NULL,
	[ValidFrom] [datetime] NULL,
	[ValidUntil] [datetime] NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_AgentHierarchyOther] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[HierarchyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[AgentHierarchyOther]  WITH CHECK ADD  CONSTRAINT [FK_AgentHierarchyOther_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AgentHierarchyOther] CHECK CONSTRAINT [FK_AgentHierarchyOther_Agent]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of Agent (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'AgentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier of the hierarchy, part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'HierarchyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the superior agent if agents are organized within a hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'AgentParentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The start time of the hierarchy, e.g. when a person was employed in an institution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'ValidFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The end of a hierarchy, e.g. when an employment ended' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'ValidUntil'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the other hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Other hierarchies of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther'
GO

GRANT SELECT ON [AgentHierarchyOther] TO [DiversityWorkbenchUser] 
GO
GRANT INSERT ON [AgentHierarchyOther] TO [DiversityWorkbenchEditor]
GO
GRANT UPDATE ON [AgentHierarchyOther] TO [DiversityWorkbenchEditor]
GO
GRANT DELETE ON [AgentHierarchyOther] TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######   AgentHierarchyOther_log  ###################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentHierarchyOther_log](
	[AgentID] [int] NULL,
	[HierarchyID] [int] NULL,
	[AgentParentID] [int] NULL,
	[ValidFrom] [datetime] NULL,
	[ValidUntil] [datetime] NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentHierarchyOther_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentHierarchyOther_log] ADD  CONSTRAINT [DF_AgentHierarchyOther_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[AgentHierarchyOther_log] ADD  CONSTRAINT [DF_AgentHierarchyOther_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[AgentHierarchyOther_log] ADD  CONSTRAINT [DF_AgentHierarchyOther_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO

GRANT SELECT ON [AgentHierarchyOther_log] TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON [AgentHierarchyOther_log] TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######   trgDelAgentHierarchyOther  #################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelAgentHierarchyOther] ON [dbo].[AgentHierarchyOther] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 31.01.2020  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  @Version,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , V.Version, 'D' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , -1, 'D' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end
GO

ALTER TABLE [dbo].[AgentHierarchyOther] ENABLE TRIGGER [trgDelAgentHierarchyOther]
GO


--#####################################################################################################################
--######   trgUpdAgentHierarchyOther  #################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdAgentHierarchyOther] ON [dbo].[AgentHierarchyOther] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 31.01.2020  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  @Version,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , V.Version, 'U' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , -1, 'U' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM AgentHierarchyOther T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.AgentID = D.AgentID
AND T.HierarchyID = D.HierarchyID
GO

ALTER TABLE [dbo].[AgentHierarchyOther] ENABLE TRIGGER [trgUpdAgentHierarchyOther]
GO


--#####################################################################################################################
--######   AgentIdentifierType_Enum  ##################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentIdentifierType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[URL] [nvarchar](500) NULL,
 CONSTRAINT [PK_AgentIdentifierType_Enum_1] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further information about this entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO

GRANT SELECT ON AgentIdentifierType_Enum TO [DiversityWorkbenchUser] 
GO
GRANT INSERT ON AgentIdentifierType_Enum TO [DiversityWorkbenchAdministrator]
GO
GRANT UPDATE ON AgentIdentifierType_Enum TO [DiversityWorkbenchAdministrator]
GO
GRANT DELETE ON AgentIdentifierType_Enum TO [DiversityWorkbenchAdministrator]
GO

--#####################################################################################################################
--######   AgentIdentifierType_Enum - adding types if missing    ######################################################
--#####################################################################################################################

if (select count(*) from AgentIdentifierType_Enum where Code = 'ORCID') = 0
begin
	INSERT INTO AgentIdentifierType_Enum (Code, Description, DisplayText, DisplayEnable, URL)
	VALUES ('ORCID', 'Open Researcher Contributor Identification', 'ORCID', 1, 'https://orcid.org/')
end

if (select count(*) from AgentIdentifierType_Enum where Code = 'ROR') = 0
begin
	INSERT INTO AgentIdentifierType_Enum (Code, Description, DisplayText, DisplayEnable, URL)
	VALUES ('ROR', 'Research Organization Registry', 'ROR', 1, 'https://ror.org/')
end

if (select count(*) from AgentIdentifierType_Enum where Code = 'ISNI') = 0
begin
	INSERT INTO AgentIdentifierType_Enum (Code, Description, DisplayText, DisplayEnable, URL)
	VALUES ('ISNI', 'International Standard Name Identifier (ISO 27729)', 'ISNI', 1, 'http://www.isni.org/')
end

--#####################################################################################################################
--######   AgentIdentifier  ###########################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[AgentIdentifier](
	[AgentID] [int] NOT NULL,
	[Identifier] [nvarchar](400) NOT NULL,
	[IdentifierURI] [varchar](500) NULL,
	[Type] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_AgentIdentifier] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[AgentIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_AgentIdentifier_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AgentIdentifier] CHECK CONSTRAINT [FK_AgentIdentifier_Agent]
GO

ALTER TABLE [dbo].[AgentIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_AgentIdentifier_AgentIdentifierType_Enum] FOREIGN KEY([Type])
REFERENCES [dbo].[AgentIdentifierType_Enum] ([Code])
GO

ALTER TABLE [dbo].[AgentIdentifier] CHECK CONSTRAINT [FK_AgentIdentifier_AgentIdentifierType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of Agent (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'AgentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier for the agent, part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'Identifier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of Identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'IdentifierURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the identifier as defined in table AgentIdentifierType_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier for the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier'
GO

GRANT SELECT ON AgentIdentifier TO [DiversityWorkbenchUser] 
GO
GRANT INSERT ON AgentIdentifier TO [DiversityWorkbenchEditor]
GO
GRANT UPDATE ON AgentIdentifier TO [DiversityWorkbenchEditor]
GO
GRANT DELETE ON AgentIdentifier TO [DiversityWorkbenchEditor]
GO

--#####################################################################################################################
--######   AgentIdentifier_log  #######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentIdentifier_log](
	[AgentID] [int] NULL,
	[Identifier] [nvarchar](400) NULL,
	[IdentifierURI] [varchar](500) NULL,
	[Type] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentIdentifier_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentIdentifier_log] ADD  CONSTRAINT [DF_AgentIdentifier_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[AgentIdentifier_log] ADD  CONSTRAINT [DF_AgentIdentifier_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[AgentIdentifier_log] ADD  CONSTRAINT [DF_AgentIdentifier_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON AgentIdentifier_log TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON AgentIdentifier_log TO [DiversityWorkbenchEditor]
GO

--#####################################################################################################################
--######   trgDelAgentIdentifier  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelAgentIdentifier] ON [dbo].[AgentIdentifier] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 10.02.2020  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  @Version,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , V.Version, 'D' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , -1, 'D' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end
GO

ALTER TABLE [dbo].[AgentIdentifier] ENABLE TRIGGER [trgDelAgentIdentifier]
GO


--#####################################################################################################################
--######   trgUpdAgentIdentifier  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdAgentIdentifier] ON [dbo].[AgentIdentifier] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 10.02.2020  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  @Version,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , V.Version, 'U' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , -1, 'U' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM AgentIdentifier T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.AgentID = D.AgentID
AND T.Identifier = D.Identifier
GO

ALTER TABLE [dbo].[AgentIdentifier] ENABLE TRIGGER [trgUpdAgentIdentifier]
GO



--#####################################################################################################################
--######   ProjectProxy - add ProjectURI if missing    ################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectProxy' and C.COLUMN_NAME = 'ProjectURI') = 0
begin
	ALTER TABLE ProjectProxy ADD [ProjectURI] [varchar](255) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectURI'
end

GO

--#####################################################################################################################
--######   ProjectProxy - add CreateArchive if missing    #############################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectProxy' and C.COLUMN_NAME = 'CreateArchive') = 0
begin
	ALTER TABLE ProjectProxy ADD [CreateArchive] bit NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If an archive e.g. by a task schedule should be created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'CreateArchive'
end

GO

--#####################################################################################################################
--######   ProjectProxy - add ArchiveProtocol if missing    ###########################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectProxy' and C.COLUMN_NAME = 'ArchiveProtocol') = 0
begin
	ALTER TABLE ProjectProxy ADD [ArchiveProtocol] [nvarchar](MAX) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the last archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ArchiveProtocol'
end

GO

--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.01.02' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.14'
END

GO

select [dbo].[Version] ()  



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.00.00'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   [ProjectAgentRole_Enum]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ProjectAgentRole_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectAgentRole_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

ALTER TABLE [dbo].[ProjectAgentRole_Enum] ADD  CONSTRAINT [DF_ProjectAgentRole_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

GRANT SELECT ON [ProjectAgentRole_Enum] TO DiversityWorkbenchUser
GO


INSERT INTO [ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('editor'
           ,'editor'
           ,'editor'
           ,1
           ,1)
GO

INSERT INTO [ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('institution'
           ,'institution'
           ,'institution'
           ,1
           ,1)
GO


--#####################################################################################################################
--######   [ProjectAgent]   ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[ProjectAgent](
	[ProjectID] [int] NOT NULL,
	[AgentName] [nvarchar](255) NOT NULL,
	[AgentURI] [varchar](255) NOT NULL,
	[AgentRole] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectAgent] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[AgentName] ASC,
	[AgentURI] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent. Only cached value where AgentURI is given' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An URL linked to e.g. the module DiversityAgents within the Diversity Workbench' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of the agent within the project as defined in ProjectAgentRole_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentRole'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

ALTER TABLE [dbo].[ProjectAgent]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgent_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectAgent] CHECK CONSTRAINT [FK_ProjectAgent_Project]
GO

ALTER TABLE [dbo].[ProjectAgent]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgent_ProjectAgentRole_Enum] FOREIGN KEY([AgentRole])
REFERENCES [dbo].[ProjectAgentRole_Enum] ([Code])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ProjectAgent] CHECK CONSTRAINT [FK_ProjectAgent_ProjectAgentRole_Enum]
GO

ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_AgentURI]  DEFAULT ('') FOR [AgentURI]
GO

ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_LogInsertedBy]  DEFAULT (user_name()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO


GRANT SELECT ON [ProjectAgent] TO DiversityWorkbenchUser
GO

GRANT UPDATE ON [ProjectAgent] TO DiversityWorkbenchEditor
GO

GRANT DELETE ON [ProjectAgent] TO DiversityWorkbenchEditor
GO




INSERT INTO [ProjectAgent]
           ([ProjectID]
           ,[AgentName]
           ,[AgentRole])
SELECT     ProjectID, ProjectEditors, 'editor'
FROM         Project
WHERE     (ProjectEditors <> N'')

GO

INSERT INTO [ProjectAgent]
           ([ProjectID]
           ,[AgentName]
           ,[AgentRole])
SELECT     ProjectID, ProjectInstitution, 'institution'
FROM         Project
WHERE     (ProjectInstitution <> N'')

GO


--#####################################################################################################################
--######   [ProjectAgent_log]   ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[ProjectAgent_log] 
([ProjectID] [int] NULL,
 [AgentName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [AgentURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [AgentRole] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [Notes] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [LogInsertedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogInsertedWhen] [smalldatetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [smalldatetime] NULL,
 [RowGUID] [uniqueidentifier] NULL,
 [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ProjectAgent_Log_LogState]  DEFAULT ('U'), 
[LogDate] [datetime] NOT NULL CONSTRAINT [DF_ProjectAgent_Log_LogDate]  DEFAULT (getdate()), 
[LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ProjectAgent_Log_LogUser]  DEFAULT (user_name()), 
[LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_ProjectAgent_Log] PRIMARY KEY CLUSTERED 
([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 

GO

--#####################################################################################################################
--######   [trgUpdProjectAgent]   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [trgUpdProjectAgent] ON [dbo].[ProjectAgent] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 30.11.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, Notes, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.Notes, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectAgent
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM ProjectAgent, deleted 
where 1 = 1 
AND ProjectAgent.AgentName = deleted.AgentName
AND ProjectAgent.AgentURI = deleted.AgentURI
AND ProjectAgent.ProjectID = deleted.ProjectID

GO


--#####################################################################################################################
--######   [trgDelProjectAgent]   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [trgDelProjectAgent] ON [dbo].[ProjectAgent] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 30.11.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, Notes, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.Notes, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######   [procInsertProjectCopy]   ######################################################################################
--#####################################################################################################################



ALTER PROCEDURE [dbo].[procInsertProjectCopy] 
	(@ProjectID int output,
	 @CopyID int,
	 @ProjectTitle nvarchar  (50)  )
AS
BEGIN TRANSACTION

INSERT INTO Project (ProjectParentID, ProjectTitle, ProjectDescription, ProjectNotes, ProjectEditors, ProjectCopyright, ProjectVersion)
SELECT ProjectParentID, @ProjectTitle, ProjectDescription, ProjectNotes, ProjectEditors, ProjectCopyright, ProjectVersion FROM Project WHERE ProjectID = @CopyID
SELECT @ProjectID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

INSERT INTO ProjectAgent(ProjectID, AgentName, AgentURI, AgentRole, Notes)
SELECT @ProjectID, AgentName, AgentURI, AgentRole, Notes
FROM ProjectAgent WHERE ProjectID = @CopyID

COMMIT

SELECT @ProjectID

GO


--#####################################################################################################################
--######   [DiversityWorkbenchModule]   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[DiversityWorkbenchModule] ()   RETURNS nvarchar(50) AS BEGIN RETURN 'DiversityProjects' END  ; 

GO

grant execute on [DiversityWorkbenchModule] to DiversityWorkbenchUser
GO


CREATE FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.00.01' END
GO

grant execute on [VersionClient] to DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.00.01'
END

GO


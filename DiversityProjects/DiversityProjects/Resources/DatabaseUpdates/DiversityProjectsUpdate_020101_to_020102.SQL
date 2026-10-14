
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.01'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--#####################################################################################################################
--######   [Project_log]  column ProjectURL may be missing  ######################################################################################
--#####################################################################################################################
--#####################################################################################################################


IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C
where C.TABLE_NAME = 'Project_log'
and C.COLUMN_NAME = 'ProjectURL') = 0
begin
alter table Project_log add ProjectURL varchar(255) NULL
end

GO


--#####################################################################################################################
--#####################################################################################################################
--######   ProjectAgentRole_Enum - neue Rollen  ######################################################################################
--#####################################################################################################################
--#####################################################################################################################


INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Owner'
           ,'Owner'
           ,'Owner'
           ,1)
GO


INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Principal investigator'
           ,'Principal investigator'
           ,'Principal investigator'
           ,1)
GO


INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Researcher'
           ,'Researcher'
           ,'Researcher'
           ,1)
GO


UPDATE [dbo].[ProjectAgentRole_Enum]
   SET [Description] = 'Curator / Editor'
      ,[DisplayText] = 'Curator / Editor'
 WHERE code = 'editor'
GO

UPDATE [dbo].[ProjectAgentRole_Enum]
   SET [Description] = 'Editing institution'
      ,[DisplayText] = 'Editing institution'
 WHERE code = 'institution'
GO

GRANT SELECT ON  ProjectAgentRole_Enum TO [DiversityWorkbenchUser] 
GO


--#####################################################################################################################
--#####################################################################################################################
--######   ProjectAgent  ######################################################################################
--#####################################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectAgent' and C.COLUMN_NAME = 'AgentType') = 0
begin
	ALTER TABLE ProjectAgent ADD AgentType [nvarchar](50) NULL

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the agent, e.g. person or organisation.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentType'

	ALTER TABLE ProjectAgent_log ADD AgentType [nvarchar](50) NULL
end



--#####################################################################################################################
--######   ProjectAgentType_Enum  ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[ProjectAgentType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProjectAgentType_Enum_RowGUID]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_ProjectAgentType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

ALTER TABLE [dbo].[ProjectAgent]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgent_ProjectAgentType_Enum] FOREIGN KEY([AgentType])
REFERENCES [dbo].[ProjectAgentType_Enum] ([Code])
ON UPDATE CASCADE
GO


GRANT SELECT ON ProjectAgentType_Enum TO [DiversityWorkbenchUser]
GO

INSERT INTO [dbo].[ProjectAgentType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Person'
           ,'Person'
           ,'Person'
           ,1)
GO

INSERT INTO [dbo].[ProjectAgentType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Institution'
           ,'Institution'
           ,'Institution'
           ,1)
GO

--#####################################################################################################################
--######   [Trigger]   ######################################################################################
--#####################################################################################################################



/****** Object:  Trigger [dbo].[trgDelProjectAgent]    Script Date: 11.05.2015 18:00:29 ******/

ALTER TRIGGER [dbo].[trgDelProjectAgent] ON [dbo].[ProjectAgent] 
FOR DELETE AS 
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.AgentType, deleted.Notes, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO


/****** Object:  Trigger [dbo].[trgUpdProjectAgent]    Script Date: 11.05.2015 18:01:04 ******/

ALTER TRIGGER [dbo].[trgUpdProjectAgent] ON [dbo].[ProjectAgent] 
FOR UPDATE AS
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.AgentType, deleted.Notes, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED
Update ProjectAgent
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM ProjectAgent, deleted 
where 1 = 1 
AND ProjectAgent.AgentName = deleted.AgentName
AND ProjectAgent.AgentURI = deleted.AgentURI
AND ProjectAgent.ProjectID = deleted.ProjectID

GO


--#####################################################################################################################
--######   [ProjectParents]   ######################################################################################
--#####################################################################################################################



CREATE  FUNCTION [dbo].[ProjectParents] (@ProjectID int)  
RETURNS @ProjectList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectCopyright] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectRights] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectLicenseURI] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[HierarchyLevel] int
)
/*
Returns a table that lists all the parents of the given project.
SELECT * FROM dbo.ProjectParents(55)
MW 09.02.2015
*/
AS
BEGIN
--declare @TopID int
declare @i int
declare @HierarchyLevel int
set @HierarchyLevel = 1

INSERT @ProjectList (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, HierarchyLevel)
SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, @HierarchyLevel
FROM Project
WHERE Project.ProjectID = @ProjectID

set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
while (@i > 0)
begin
	set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
	set @HierarchyLevel = @HierarchyLevel + 1
	INSERT @ProjectList (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, HierarchyLevel)
	SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
		ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, @HierarchyLevel
	FROM Project
	WHERE Project.ProjectID = @ProjectID
	set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
end

RETURN
END


GO

GRANT SELECT ON ProjectParents TO [DiversityWorkbenchUser]
GO





--#####################################################################################################################
--######   [VersionClient]   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.00' END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.02'
END

GO


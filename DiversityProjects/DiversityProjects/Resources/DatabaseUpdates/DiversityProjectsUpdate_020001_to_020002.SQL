
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.00.01'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Project   ######################################################################################
--#####################################################################################################################

ALTER TABLE Project ADD [ProjectRights] nvarchar(255) NULL
GO

ALTER TABLE Project ADD [ProjectLicenseURI] nvarchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The rights, e.g. the copyright of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectRights'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The license under which the project is re-used or may be re-used' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectLicenseURI'
GO

ALTER TABLE Project ADD [RowGUID] [uniqueidentifier] NULL
GO

ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO






GRANT INSERT ON [Project] TO DiversityWorkbenchEditor
GO

GRANT UPDATE ON [Project] TO DiversityWorkbenchEditor
GO

GRANT DELETE ON [Project] TO DiversityWorkbenchAdministrator
GO


--#####################################################################################################################
--######   Project_log   ######################################################################################
--#####################################################################################################################

ALTER TABLE Project_log ADD [ProjectRights] nvarchar(255) NULL
GO

ALTER TABLE Project_log ADD [ProjectLicenseURI] nvarchar(255) NULL
GO

ALTER TABLE Project_log ADD [RowGUID] [uniqueidentifier] NULL
GO

GRANT INSERT ON [Project_log] TO DiversityWorkbenchEditor
GO


--#####################################################################################################################
--######   [trgDelProject]   ######################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelProject] ON [dbo].[Project] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.1 */ 
/*  Date: 29.12.2006  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectDescription, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED
GO


--#####################################################################################################################
--######   [trgUpdProject]   ######################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdProject] ON [dbo].[Project] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.1 */ 
/*  Date: 29.12.2006  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectDescription, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED


/* updating the logging columns */
Update Project
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Project, deleted 
where 1 = 1 
AND Project.ProjectID = deleted.ProjectID

GO


--#####################################################################################################################
--######   ProjectAgent   ######################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The persons or institutions involved in the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent'
GO

GRANT INSERT ON [ProjectAgent_log] TO DiversityWorkbenchEditor
GO


--#####################################################################################################################
--######   [ProjectResource]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectResource](
	[ProjectID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[SpecificRights] [nvarchar](500) NULL,
	[SpecificLicenseURI] [nvarchar](500) NULL,
	[ResourceURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectResource] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[URI] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of the project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The complete URI address of the resource. ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'URI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The rights, e.g. the copyright of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'SpecificRights'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The license under which the resource is re-used or may be re-used' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'SpecificLicenseURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the image, e.g. as stored in the module DiversityResources. ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'ResourceURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Resources of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource'
GO

ALTER TABLE [dbo].[ProjectResource]  WITH CHECK ADD  CONSTRAINT [FK_ProjectResource_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectResource] CHECK CONSTRAINT [FK_ProjectResource_Project]
GO

ALTER TABLE [dbo].[ProjectResource] ADD  CONSTRAINT [DF_ProjectResource_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[ProjectResource] ADD  CONSTRAINT [DF_ProjectResource_LogInsertedBy]  DEFAULT (user_name()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[ProjectResource] ADD  CONSTRAINT [DF_ProjectResource_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ProjectResource] ADD  CONSTRAINT [DF_ProjectResource_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectResource] ADD  CONSTRAINT [DF_ProjectResource_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT INSERT ON [ProjectResource] TO DiversityWorkbenchEditor
GO

GRANT UPDATE ON [ProjectResource] TO DiversityWorkbenchEditor
GO

GRANT DELETE ON [ProjectResource] TO DiversityWorkbenchEditor
GO

GRANT SELECT ON [ProjectResource] TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   [ProjectResource_log]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ProjectResource_log](
	[ProjectID] [int] NULL,
	[URI] [varchar](255) NULL,
	[SpecificRights] [nvarchar](500) NULL,
	[SpecificLicenseURI] [nvarchar](500) NULL,
	[ResourceURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectResource_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProjectResource_log] ADD  CONSTRAINT [DF_ProjectResource_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[ProjectResource_log] ADD  CONSTRAINT [DF_ProjectResource_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ProjectResource_log] ADD  CONSTRAINT [DF_ProjectResource_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT INSERT ON [ProjectResource_log] TO DiversityWorkbenchEditor
GO

GRANT SELECT ON [ProjectResource_log] TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   [trgDelProjectResource]   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelProjectResource] ON [dbo].[ProjectResource] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 17.12.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectResource_Log (ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.URI, deleted.SpecificRights, deleted.SpecificLicenseURI, deleted.ResourceURI, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######   [trgUpdProjectResource]   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectResource] ON [dbo].[ProjectResource] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 17.12.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectResource_Log (ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.URI, deleted.SpecificRights, deleted.SpecificLicenseURI, deleted.ResourceURI, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectResource
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM ProjectResource, deleted 
where 1 = 1 
AND ProjectResource.ProjectID = deleted.ProjectID
AND ProjectResource.URI = deleted.URI
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
--######   [ProjectChildNodes]   ######################################################################################
--#####################################################################################################################


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectChildNodes]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ProjectChildNodes]
GO



CREATE  FUNCTION [dbo].[ProjectChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
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
	[ProjectSettings] [xml] NULL
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
	[ProjectCopyright] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectRights] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectLicenseURI] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectSettings] [xml] NULL)

INSERT @TempItem (  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings) 
	SELECT  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings
	FROM Project WHERE ProjectParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select ProjectID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings
	from dbo.ProjectChildNodes (@ParentID) where ProjectID not in (select ProjectID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor 
 INSERT @ItemList ( ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL) 
   SELECT distinct  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL
   FROM @TempItem ORDER BY ProjectTitle
   
   -- update XML   
   update IL set IL.ProjectSettings = P.ProjectSettings
   from @ItemList IL, Project P
   WHERE IL.ProjectID = P.ProjectID

   RETURN
END

GO

GRANT SELECT ON [dbo].[ProjectChildNodes] TO DiversityWorkbenchUser
GO



--#####################################################################################################################
--######   [ProjectHierarchy]   ######################################################################################
--#####################################################################################################################



ALTER  FUNCTION [dbo].[ProjectHierarchy] (@ProjectID int)  
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
	[ProjectSettings] [xml] NULL
)

/*
Returns a table that lists all the items related to the given item.
MW 02.01.2006
*/
AS
BEGIN

-- getting the TopID
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

-- copy the root node in the result list
   INSERT @ProjectList
   SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, NULL
   FROM Project
   WHERE Project.ProjectID = @TopID
   
-- update XML   
   update PL set PL.ProjectSettings = P.ProjectSettings
   from @ProjectList PL, Project P
   WHERE P.ProjectID = @TopID
   and PL.ProjectID = P.ProjectID
	
-- copy the child nodes into the result list
   INSERT @ProjectList
   SELECT * FROM dbo.ProjectChildNodes (@TopID)

   RETURN
END


GO


--#####################################################################################################################
--######   [UserProxy]   ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[UserProxy](
	[LoginName] [nvarchar](50) NOT NULL,
	[CombinedNameCache] [nvarchar](255) NOT NULL,
	[AgentURI] [varchar](255) NULL,
	[Queries] [xml] NULL,
	[Settings] [xml] NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_UserProxy] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A login name of the user for access to the database server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'LoginName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The short name of the user, e.g. P. Smith. A cached value derived from an entry in the module DiversityAgents if AgentURI is filled.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'CombinedNameCache'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of a user in the remote module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Queries defined by the user to access the data in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Queries'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The settings for the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Settings'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The users with access to the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy'
GO

ALTER TABLE [dbo].[UserProxy] ADD  CONSTRAINT [DF__UserProxy__RowGU__4B58D01C]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT INSERT ON [UserProxy] TO DiversityWorkbenchAdministrator
GO

GRANT UPDATE ON [UserProxy] TO DiversityWorkbenchAdministrator
GO

GRANT DELETE ON [UserProxy] TO DiversityWorkbenchAdministrator
GO

GRANT SELECT ON [UserProxy] TO DiversityWorkbenchUser
GO





--#####################################################################################################################
--######   [ProjectUser]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ProjectUser](
	[LoginName] [nvarchar](50) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_ProjectUser] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A login name which the user uses  for access the DivesityWorkbench, Microsoft domains, etc..' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'LoginName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project to which the specimen belongs (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The projects available for a user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser'
GO

ALTER TABLE [dbo].[ProjectUser]  WITH CHECK ADD  CONSTRAINT [FK_ProjectUser_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_Project]
GO

ALTER TABLE [dbo].[ProjectUser]  WITH NOCHECK ADD  CONSTRAINT [FK_ProjectUser_UserProxy] FOREIGN KEY([LoginName])
REFERENCES [dbo].[UserProxy] ([LoginName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_UserProxy]
GO

ALTER TABLE [dbo].[ProjectUser] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT INSERT ON [ProjectUser] TO DiversityWorkbenchEditor
GO

GRANT UPDATE ON [ProjectUser] TO DiversityWorkbenchAdministrator
GO

GRANT DELETE ON [UserProxy] TO DiversityWorkbenchAdministrator
GO

GRANT SELECT ON [ProjectUser] TO DiversityWorkbenchUser
GO



--#####################################################################################################################
--######   [ProjectList]   ######################################################################################
--#####################################################################################################################




CREATE  FUNCTION [dbo].[ProjectList] ()  
RETURNS @ProjectList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL)

/*
Returns a table that lists all the projects available for a user
MW 18.12.2012
*/
AS
BEGIN

IF (SELECT PERMISSIONS(OBJECT_ID('Project'))&16) = 16
	BEGIN
		INSERT INTO @ProjectList
		SELECT ProjectID FROM Project
	END
ELSE
	BEGIN
		INSERT INTO @ProjectList
		SELECT [ProjectID]
		FROM [ProjectUser]
		WHERE LoginName = USER_NAME()
	END
RETURN
END

GO


GRANT SELECT ON [ProjectList] TO DiversityWorkbenchUser
GO

--#####################################################################################################################
--######   [Project_Core]   ######################################################################################
--#####################################################################################################################



CREATE VIEW [dbo].[Project_Core]
AS
SELECT     P.ProjectID, P.ProjectParentID, P.Project, P.ProjectTitle, P.ProjectDescription, P.ProjectNotes, P.ProjectCopyright, P.ProjectRights, P.ProjectLicenseURI, 
                      P.ProjectVersion, P.ProjectURL, P.ProjectSettings, P.LogInsertedBy, P.LogInsertedWhen, P.LogUpdatedBy, P.LogUpdatedWhen, P.RowGUID
FROM         dbo.Project AS P INNER JOIN
                      dbo.ProjectList() AS L ON P.ProjectID = L.ProjectID

GO

GRANT SELECT ON [Project_Core] TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   [ProjectAgent_Core]   ######################################################################################
--#####################################################################################################################



CREATE VIEW [dbo].[ProjectAgent_Core]
AS
SELECT     A.ProjectID, A.AgentName, A.AgentURI, A.AgentRole, A.Notes, A.LogInsertedBy, A.LogInsertedWhen, A.LogUpdatedBy, A.LogUpdatedWhen, A.RowGUID
FROM         dbo.ProjectAgent AS A INNER JOIN
                      dbo.ProjectList() AS L ON A.ProjectID = L.ProjectID

GO

GRANT SELECT ON [ProjectAgent_Core] TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   [ProjectResource_Core]   ######################################################################################
--#####################################################################################################################


CREATE VIEW [dbo].[ProjectResource_Core]
AS
SELECT     R.ProjectID, R.URI, R.SpecificRights, R.SpecificLicenseURI, R.ResourceURI, R.Notes, R.LogInsertedWhen, R.LogInsertedBy, R.LogUpdatedWhen, R.LogUpdatedBy, 
                      R.RowGUID
FROM         dbo.ProjectResource AS R INNER JOIN
                      dbo.ProjectList() AS L ON R.ProjectID = L.ProjectID

GO

GRANT SELECT ON [ProjectResource_Core] TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   [ProjectProxy]   ######################################################################################
--#####################################################################################################################



CREATE VIEW [dbo].[ProjectProxy]
AS
SELECT     ProjectID, Project, dbo.BaseURL() + CAST(ProjectID AS varchar) AS ProjectURI
FROM         dbo.Project_Core

GO

GRANT SELECT ON [ProjectProxy] TO DiversityWorkbenchUser
GO

--#####################################################################################################################
--######   [DiversityWorkbenchModule]   ######################################################################################
--#####################################################################################################################



GRANT EXECUTE ON dbo.DiversityWorkbenchModule TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   [VersionClient]   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.00.02' END
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.00.02'
END

GO


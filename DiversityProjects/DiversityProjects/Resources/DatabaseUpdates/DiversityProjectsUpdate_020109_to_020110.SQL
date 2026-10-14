
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.09'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  ProjectDescription - new description  #######################################################################
--#####################################################################################################################

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'A text, describing characteristic features, e. g. the purpose and scope, of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectDescription'
GO


--#####################################################################################################################
--######  Project: adding InternalDescription   #######################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'Project' and C.COLUMN_NAME = 'InternalDescription') = 0
begin
	ALTER TABLE [dbo].[Project] ADD [InternalDescription] [nvarchar](MAX) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text, describing characteristic features, e. g. the purpose and scope, of the project for internal use only' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'InternalDescription'

	ALTER TABLE [dbo].[Project_log] ADD [InternalDescription] [nvarchar](MAX) NULL
end
GO


--#####################################################################################################################
--######  Project: adding PublicDescription     #######################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'Project' and C.COLUMN_NAME = 'PublicDescription') = 0
begin
	ALTER TABLE [dbo].[Project] ADD [PublicDescription] [nvarchar](MAX) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text, describing characteristic features, e. g. the purpose and scope, of the project to be published' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'PublicDescription'

	ALTER TABLE [dbo].[Project_log] ADD [PublicDescription] [nvarchar](MAX) NULL
end
GO


--#####################################################################################################################
--######  Adaption of triggers for table Project    ###################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelProject] ON [dbo].[Project] 
FOR DELETE AS 
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, InternalDescription, ProjectDescription, PublicDescription, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectType, deleted.InternalDescription, deleted.ProjectDescription, deleted.PublicDescription, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED

GO


ALTER TRIGGER [dbo].[trgUpdProject] ON [dbo].[Project] 
FOR UPDATE AS
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, InternalDescription, ProjectDescription, PublicDescription, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectType, deleted.InternalDescription, deleted.ProjectDescription, deleted.PublicDescription, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED
Update Project
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM Project, deleted 
where 1 = 1 
AND Project.ProjectID = deleted.ProjectID

GO


--#####################################################################################################################
--######  ViewProject: adding new columns   ###########################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[ViewProject]
AS
SELECT [ProjectID]
      ,[ProjectParentID]
      ,[Project]
      ,[ProjectTitle]
      ,[ProjectDescription]
      ,[ProjectNotes]
      ,[ProjectVersion]
      ,[ProjectURL]
      ,[ProjectRights]
      ,[ProjectCopyright]
      ,[ProjectLicenseURI]
      ,[LogInsertedBy]
      ,[LogInsertedWhen]
      ,[LogUpdatedBy]
      ,[LogUpdatedWhen]
      ,[RowGUID]
	  ,dbo.StableIdentifier(ProjectID) as StableIdentifier
	  ,ProjectType
	  ,InternalDescription
	  ,PublicDescription
  FROM [dbo].[Project]
  GO


--#####################################################################################################################
--######  ProjectReference: adding IsCitation   #######################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'ProjectReference' and C.COLUMN_NAME = 'IsCitation') = 0
begin
	ALTER TABLE [dbo].[ProjectReference] ADD [IsCitation] bit NULL CONSTRAINT [DF_ProjectReference_IsCitation]  DEFAULT ((0))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If a reference is a citation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'IsCitation'

	ALTER TABLE [dbo].[ProjectReference_log] ADD [IsCitation] bit NULL
end
GO


--#####################################################################################################################
--######  ProjectReference: adding Uri   ##############################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'ProjectReference' and C.COLUMN_NAME = 'URI') = 0
begin
	ALTER TABLE [dbo].[ProjectReference] ADD URI [nvarchar](500) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of a reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'URI'

end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'ProjectReference_log' and C.COLUMN_NAME = 'URI') = 0
begin
	ALTER TABLE [dbo].[ProjectReference_log] ADD URI [nvarchar](500) NULL
end
GO


--#####################################################################################################################
--######  Adaption of trigger of table ProjectReference   #############################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelProjectReference]    Script Date: 10.06.2016 15:05:24 ******/
ALTER TRIGGER [dbo].[trgDelProjectReference] ON [dbo].[ProjectReference] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 
/*  changed: 21.07.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, ReferenceType, Notes, URI, IsCitation, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ReferenceType, deleted.Notes, deleted.URI, deleted.IsCitation, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO


DISABLE TRIGGER dbo.[trgUpdProjectReference] ON dbo.[ProjectReference]
GO

UPDATE [dbo].[ProjectReference] SET IsCitation = 0
GO

ALTER TABLE [dbo].[ProjectReference] ALTER COLUMN [IsCitation] bit NOT NULL
GO

/****** Object:  Trigger [dbo].[trgUpdProjectReference]    Script Date: 10.06.2016 15:05:54 ******/
ALTER TRIGGER [dbo].[trgUpdProjectReference] ON [dbo].[ProjectReference] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 
/*  changed: 21.07.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, ReferenceType, Notes, URI, IsCitation, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ReferenceType, deleted.Notes, deleted.URI, deleted.IsCitation, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update ProjectReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectReference, deleted 
where 1 = 1 
AND ProjectReference.ProjectID = deleted.ProjectID
AND ProjectReference.ReferenceTitle = deleted.ReferenceTitle

GO

ENABLE TRIGGER dbo.[trgUpdProjectReference]  ON dbo.[ProjectReference]
GO


--#####################################################################################################################
--######   ProjectChildNodes - adding new columns   ###################################################################
--#####################################################################################################################


ALTER  FUNCTION [dbo].[ProjectChildNodes] (@ID int)  
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
	[ProjectSettings] [xml] NULL,
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL
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
	[ProjectSettings] [xml] NULL,
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL)
/* 
removing content to enable self reference
*/
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
	[ProjectCopyright] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectRights] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectLicenseURI] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectSettings] [xml] NULL,
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL
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
	[ProjectSettings] [xml] NULL,
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL)
	
INSERT @TempItem (  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, ProjectType, InternalDescription, PublicDescription) 
	SELECT  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, ProjectType, InternalDescription, PublicDescription
	FROM Project WHERE ProjectParentID = @ID 
   DECLARE HierarchyCursor  CURSOR for
   select ProjectID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, ProjectType, InternalDescription, PublicDescription
	from dbo.ProjectChildNodes (@ParentID) where ProjectID not in (select ProjectID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor 
 INSERT @ItemList ( ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription) 
   SELECT distinct  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription
   FROM @TempItem ORDER BY ProjectTitle
   update IL set IL.ProjectSettings = P.ProjectSettings
   from @ItemList IL, Project P
   WHERE IL.ProjectID = P.ProjectID
   RETURN
END

GO


--#####################################################################################################################
--######   ProjectHierarchy - adding new columns   ####################################################################
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
	[ProjectSettings] [xml] NULL,
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL
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
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, NULL, ProjectType, InternalDescription, PublicDescription
   FROM Project
   WHERE Project.ProjectID = @TopID
   update PL set PL.ProjectSettings = P.ProjectSettings
   from @ProjectList PL, Project P
   WHERE P.ProjectID = @TopID
   and PL.ProjectID = P.ProjectID
   INSERT @ProjectList
   SELECT * FROM dbo.ProjectChildNodes (@TopID)
   RETURN
END

GO

--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.04' END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.10'
END

GO


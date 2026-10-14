
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.23'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Disabling Regulation in ProjectType_Enum  ###################################################################
--#####################################################################################################################

UPDATE [dbo].[ProjectType_Enum]
   SET [DisplayEnable] = 0
 WHERE [Code] = 'Regulation'
GO


--#####################################################################################################################
--######  setting Project.ProjectDescription to nvarchar(MAX)  ########################################################
--#####################################################################################################################

ALTER TABLE [dbo].[Project] ALTER COLUMN [ProjectDescription] nvarchar(MAX) NULL;
GO

ALTER TABLE [dbo].[Project_log] ALTER COLUMN [ProjectDescription] nvarchar(MAX) NULL;
GO


--#####################################################################################################################
--######  setting ProjectDescription in function ProjectParents to nvarchar(MAX)  #####################################
--#####################################################################################################################



ALTER  FUNCTION [dbo].[ProjectParents] (@ProjectID int)  
RETURNS @ProjectList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	--[ProjectCopyright] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	--[ProjectRights] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	--[ProjectLicenseURI] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
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
	ProjectNotes, /*ProjectCopyright, ProjectRights, ProjectLicenseURI,*/ ProjectVersion, ProjectURL, HierarchyLevel)
SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, /*ProjectCopyright, ProjectRights, ProjectLicenseURI,*/ ProjectVersion, ProjectURL, @HierarchyLevel
FROM Project
WHERE Project.ProjectID = @ProjectID

set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
while (@i > 0)
begin
	set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
	set @HierarchyLevel = @HierarchyLevel + 1
	INSERT @ProjectList (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, /*ProjectCopyright, ProjectRights, ProjectLicenseURI,*/ ProjectVersion, ProjectURL, HierarchyLevel)
	SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
		ProjectNotes, /*ProjectCopyright, ProjectRights, ProjectLicenseURI,*/ ProjectVersion, ProjectURL, @HierarchyLevel
	FROM Project
	WHERE Project.ProjectID = @ProjectID
	set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
end

RETURN
END

GO


--#####################################################################################################################
--######  setting ProjectDescription in function ProjectHierarchy to nvarchar(MAX)  ###################################
--#####################################################################################################################


ALTER  FUNCTION [dbo].[ProjectHierarchy] (@ProjectID int)  
RETURNS @ProjectList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
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
--######  setting ProjectDescription in function ProjectChildNodes to nvarchar(MAX)  ##################################
--#####################################################################################################################



ALTER  FUNCTION [dbo].[ProjectChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
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
	[ProjectDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
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
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '04.01.56' END
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.24'
END

GO


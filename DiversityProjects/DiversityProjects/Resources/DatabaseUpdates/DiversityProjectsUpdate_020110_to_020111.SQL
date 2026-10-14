
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.10'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  ProjectLabel including log table and trigger  ###############################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectLabel') = 0
BEGIN
	CREATE TABLE [dbo].[ProjectLabel](
		[ProjectID] [int] NOT NULL,
		[Label] [nvarchar](255) NOT NULL,
		[LogInsertedBy] [nvarchar](50) NULL CONSTRAINT [DF_ProjectLabel_LogInsertedBy]  DEFAULT (suser_sname()),
		[LogInsertedWhen] [smalldatetime] NULL CONSTRAINT [DF_ProjectLabel_LogInsertedWhen]  DEFAULT (getdate()),
		[LogUpdatedBy] [nvarchar](50) NULL CONSTRAINT [DF_ProjectLabel_LogUpdatedBy]  DEFAULT (suser_sname()),
		[LogUpdatedWhen] [smalldatetime] NULL CONSTRAINT [DF_ProjectLabel_LogUpdatedWhen]  DEFAULT (getdate()),
		[RowGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProjectLabel_RowGUID]  DEFAULT (newsequentialid()),
	 CONSTRAINT [PK_ProjectLabel] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[Label] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[ProjectLabel]  WITH CHECK ADD  CONSTRAINT [FK_ProjectLabel_Project] FOREIGN KEY([ProjectID])
	REFERENCES [dbo].[Project] ([ProjectID])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ProjectLabel] CHECK CONSTRAINT [FK_ProjectLabel_Project]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'ProjectID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Label' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'Label'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Labels for the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel'
END
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectLabel_log') = 0
BEGIN
	CREATE TABLE [dbo].[ProjectLabel_log](
		[ProjectID] [int] NULL,
		[Label] [nvarchar](255) NULL,
		[LogInsertedBy] [nvarchar](50) NULL,
		[LogInsertedWhen] [smalldatetime] NULL,
		[LogUpdatedBy] [nvarchar](50) NULL,
		[LogUpdatedWhen] [smalldatetime] NULL,
		[RowGUID] [uniqueidentifier] NULL,
		[LogState] [char](1) NULL CONSTRAINT [DF_ProjectLabel_Log_LogState]  DEFAULT ('U'),
		[LogDate] [datetime] NOT NULL CONSTRAINT [DF_ProjectLabel_Log_LogDate]  DEFAULT (getdate()),
		[LogUser] [nvarchar](50) NULL CONSTRAINT [DF_ProjectLabel_Log_LogUser]  DEFAULT (user_name()),
		[LogID] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_ProjectLabel_Log] PRIMARY KEY CLUSTERED 
	(
		[LogID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]



END
GO

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE TRIGGER [dbo].[trgDelProjectLabel] ON [dbo].[ProjectLabel] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.5 */ 
/*  Date: 25.08.2017  */ 
/* saving the original dataset in the logging table */ 
INSERT INTO ProjectLabel_Log (ProjectID, Label, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Label, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  ''D''
FROM DELETED')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



declare @SQL nvarchar(max)
set @SQL = (select 'CREATE TRIGGER [dbo].[trgUpdProjectLabel] ON [dbo].[ProjectLabel] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.5 */ 
/*  Date: 25.08.2017  */ 
/* saving the original dataset in the logging table */ 
INSERT INTO ProjectLabel_Log (ProjectID, Label, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Label, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  ''U''
FROM DELETED
/* updating the logging columns */
Update ProjectLabel
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectLabel, deleted 
where 1 = 1 
AND ProjectLabel.Label = deleted.Label
AND ProjectLabel.ProjectID = deleted.ProjectID
')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######  New roles for Agent  ########################################################################################
--#####################################################################################################################

IF (SELECT COUNT(*) FROM ProjectAgentRole_Enum E WHERE E.Code = 'AUTHOR') = 0
BEGIN
	INSERT INTO ProjectAgentRole_Enum
	(Code, Description, DisplayText, DisplayEnable)
	VALUES ('Author', 'Author', 'Author', 1)
	END
	ELSE
	BEGIN
	UPDATE E SET Code = 'Author', Description = 'Author', DisplayText = 'Author', DisplayEnable = 1
	FROM ProjectAgentRole_Enum E
	WHERE E.Code = 'AUTHOR'
END
GO


IF (SELECT COUNT(*) FROM ProjectAgentRole_Enum E WHERE E.Code = 'Publisher') = 0
BEGIN
	INSERT INTO ProjectAgentRole_Enum
	(Code, Description, DisplayText, DisplayEnable)
	VALUES ('Publisher', 'Publisher', 'Publisher', 1)
	END
	ELSE
	BEGIN
	UPDATE E SET Code = 'Publisher', Description = 'Publisher', DisplayText = 'Publisher', DisplayEnable = 1
	FROM ProjectAgentRole_Enum E
	WHERE E.Code = 'Publisher'
END
GO


--#####################################################################################################################
--######  Project - new description  ##################################################################################
--#####################################################################################################################

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Information on the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project'
GO


--#####################################################################################################################
--######  new column ProjectDescriptionType in table Project  #########################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Project' and c.COLUMN_NAME = 'ProjectDescriptionType') = 0
begin
	ALTER TABLE [dbo].[Project] ADD [ProjectDescriptionType] [nvarchar](50) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'classification of the public description, e. g. abstract' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectDescriptionType'

	ALTER TABLE [dbo].[Project_log] ADD [ProjectDescriptionType] [nvarchar](50) NULL
end
GO

--#####################################################################################################################
--######  add column ProjectType in table Project_log if missing  #####################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Project_log' and c.COLUMN_NAME = 'ProjectType') = 0
begin
	ALTER TABLE [dbo].[Project_log] ADD [ProjectType] [nvarchar](50) NULL
end
GO


--#####################################################################################################################
--######  add column ProjectDescriptionType in table Project_log if missing  ##########################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Project_log' and c.COLUMN_NAME = 'ProjectDescriptionType') = 0
begin
	ALTER TABLE [dbo].[Project_log] ADD [ProjectDescriptionType] [nvarchar](50) NULL
end
GO

--#####################################################################################################################
--######  Adaption of triggers for table Project    ###################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelProject] ON [dbo].[Project] 
FOR DELETE AS 
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, InternalDescription, ProjectDescription, PublicDescription, ProjectDescriptionType, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectType, deleted.InternalDescription, deleted.ProjectDescription, deleted.PublicDescription, deleted.ProjectDescriptionType, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED

GO


ALTER TRIGGER [dbo].[trgUpdProject] ON [dbo].[Project] 
FOR UPDATE AS
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, InternalDescription, ProjectDescription, PublicDescription, ProjectDescriptionType, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectType, deleted.InternalDescription, deleted.ProjectDescription, deleted.PublicDescription, deleted.ProjectDescriptionType, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED
Update Project
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM Project, deleted 
where 1 = 1 
AND Project.ProjectID = deleted.ProjectID

GO


--#####################################################################################################################
--######  ViewProject: adding new column   ############################################################################
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
	  ,ProjectDescriptionType
  FROM [dbo].[Project]
  GO

  
--#####################################################################################################################
--######   ProjectChildNodes - adding new column    ###################################################################
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
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL
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
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL)
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
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL
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
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL)
	
INSERT @TempItem (  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType) 
	SELECT  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType
	FROM Project WHERE ProjectParentID = @ID 
   DECLARE HierarchyCursor  CURSOR for
   select ProjectID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem ( ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType) 
	select  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType
	from dbo.ProjectChildNodes (@ParentID) where ProjectID not in (select ProjectID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor 
 INSERT @ItemList ( ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType) 
   SELECT distinct  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType
   FROM @TempItem ORDER BY ProjectTitle
   update IL set IL.ProjectSettings = P.ProjectSettings
   from @ItemList IL, Project P
   WHERE IL.ProjectID = P.ProjectID
   RETURN
END

GO


--#####################################################################################################################
--######   ProjectHierarchy - adding new column    ####################################################################
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
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL
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
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, NULL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType
   FROM Project
   WHERE Project.ProjectID = @TopID
   update PL set PL.ProjectSettings = P.ProjectSettings
   from @ProjectList PL, Project P
   WHERE P.ProjectID = @TopID
   and PL.ProjectID = P.ProjectID
   INSERT @ProjectList (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType)
   SELECT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType 
	FROM dbo.ProjectChildNodes (@TopID)
   RETURN
END

GO

--#####################################################################################################################
--######  ProjectReference: adding ReferenceCitation   ################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'ProjectReference' and c.COLUMN_NAME = 'ReferenceCitation') = 0
begin
	ALTER TABLE [dbo].[ProjectReference] ADD [ReferenceCitation] [nvarchar](500) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the reference should be cited; suggested citation style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceCitation'

	ALTER TABLE [dbo].[ProjectReference_log] ADD [ReferenceCitation] [nvarchar](500) NULL
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
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, ReferenceType, Notes, URI, IsCitation, ReferenceCitation, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ReferenceType, deleted.Notes, deleted.URI, deleted.IsCitation, deleted.ReferenceCitation, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO


/****** Object:  Trigger [dbo].[trgUpdProjectReference]    Script Date: 10.06.2016 15:05:54 ******/
ALTER TRIGGER [dbo].[trgUpdProjectReference] ON [dbo].[ProjectReference] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 
/*  changed: 21.07.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, ReferenceType, Notes, URI, IsCitation, ReferenceCitation, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ReferenceType, deleted.Notes, deleted.URI, deleted.IsCitation, deleted.ReferenceCitation, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
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
--######  ProjectReference: new description for IsCitation   ##########################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'ProjectReference' and C.COLUMN_NAME = 'IsCitation') = 1
begin
	EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'If a reference is the citation for the project data, i.e. the way the project data should be cited' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'IsCitation'
end
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
RETURN '02.01.11'
END

GO


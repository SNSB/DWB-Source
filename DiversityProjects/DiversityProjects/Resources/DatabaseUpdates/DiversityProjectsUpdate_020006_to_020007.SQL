
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.00.06'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ProjectReference - missing grants  ######################################################################################
--#####################################################################################################################

GRANT SELECT ON ProjectReference TO [DiversityWorkbenchUser]
GO

GRANT INSERT ON ProjectReference TO [DiversityWorkbenchEditor]
GO

GRANT UPDATE ON ProjectReference TO [DiversityWorkbenchEditor]
GO

GRANT DELETE ON ProjectReference TO [DiversityWorkbenchEditor]
GO


GRANT SELECT ON ProjectReference_log TO [DiversityWorkbenchUser]
GO

GRANT INSERT ON ProjectReference_log TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######   [Project_log]  column ProjectURL may be missing  ######################################################################################
--#####################################################################################################################

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'Project_log'
and C.COLUMN_NAME = 'ProjectURL') = 0
begin
ALTER TABLE Project_log ADD [ProjectURL] [varchar](255) NULL
end

/*
IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'Project_log'
and C.COLUMN_NAME = 'ProjectInstitution') = 0
begin
ALTER TABLE Project_log ADD [ProjectInstitution] [nvarchar](500) NULL
end
*/
GO


ALTER TRIGGER [dbo].[trgDelProject] ON [dbo].[Project] 
FOR DELETE AS 
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectDescription, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED

GO


/****** Object:  Trigger [dbo].[trgUpdProject]    Script Date: 09.10.2014 17:40:49 ******/

ALTER TRIGGER [dbo].[trgUpdProject] ON [dbo].[Project] 
FOR UPDATE AS
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectDescription, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED
Update Project
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Project, deleted 
where 1 = 1 
AND Project.ProjectID = deleted.ProjectID

GO



--#####################################################################################################################
--######   [VersionClient]   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.00.10' END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.00.07'
END

GO


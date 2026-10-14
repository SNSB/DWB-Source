
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.02'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--#####################################################################################################################
--######   missing permissions for ProjectLicense etc.  ######################################################################################
--#####################################################################################################################
--#####################################################################################################################

GRANT SELECT ON ProjectLicense TO [DiversityWorkbenchUser]
GO
GRANT INSERT ON ProjectLicense TO [DiversityWorkbenchEditor]
GO
GRANT UPDATE ON ProjectLicense TO [DiversityWorkbenchEditor]
GO
GRANT DELETE ON ProjectLicense TO [DiversityWorkbenchAdministrator]
GO


GRANT SELECT ON ProjectLicense_log TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON ProjectLicense_log TO [DiversityWorkbenchEditor]
GO


GRANT SELECT ON [Setting_log] TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON [Setting_log] TO [DiversityWorkbenchEditor]
GO



--#####################################################################################################################
--#####################################################################################################################
--######   Trigger fuer [dbo].[ProjectReference]  ######################################################################################
--#####################################################################################################################
--#####################################################################################################################


/****** Object:  Trigger [dbo].[trgDelProjectReference]    Script Date: 12.05.2015 17:46:15 ******/
ALTER TRIGGER [dbo].[trgDelProjectReference] ON [dbo].[ProjectReference] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.0.7 */ 
/*  Date: 05.09.2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO

/****** Object:  Trigger [dbo].[trgUpdProjectReference]    Script Date: 12.05.2015 17:47:01 ******/
ALTER TRIGGER [dbo].[trgUpdProjectReference] ON [dbo].[ProjectReference] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.0.7 */ 
/*  Date: 05.09.2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM ProjectReference, deleted 
where 1 = 1 
AND ProjectReference.ProjectID = deleted.ProjectID
AND ProjectReference.ReferenceTitle = deleted.ReferenceTitle
GO


--#####################################################################################################################
--######   [procInsertProjectCopy]   ######################################################################################
--#####################################################################################################################

ALTER PROCEDURE [dbo].[procInsertProjectCopy] 
	(@ProjectID int output,
	 @CopyID int,
	 @ProjectTitle nvarchar  (200)  )
AS
BEGIN TRANSACTION
-- Insert Project
INSERT INTO Project (ProjectParentID, Project, ProjectTitle, ProjectDescription, ProjectNotes, ProjectURL, ProjectVersion)
SELECT ProjectParentID, substring(@ProjectTitle, 1, 50), @ProjectTitle, ProjectDescription, ProjectNotes, ProjectURL, ProjectVersion 
FROM Project WHERE ProjectID = @CopyID
SELECT @ProjectID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

-- Insert agents
INSERT INTO ProjectAgent(ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes)
SELECT @ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes
FROM ProjectAgent WHERE ProjectID = @CopyID

-- Insert Licenses
INSERT INTO ProjectLicense
	  (ProjectID, DisplayText, LicenseURI, LicenseType, LicenseHolder, LicenseHolderAgentURI, LicenseYear, Context, LicenseDetails, IPR, IPRHolder, 
IPRHolderAgentURI, CopyrightStatement, CopyrightHolder, CopyrightHolderAgentURI, Notes)
SELECT @ProjectID, DisplayText, LicenseURI, LicenseType, LicenseHolder, LicenseHolderAgentURI, LicenseYear, Context, LicenseDetails, IPR, IPRHolder, 
IPRHolderAgentURI, CopyrightStatement, CopyrightHolder, CopyrightHolderAgentURI, Notes
FROM            ProjectLicense WHERE ProjectID = @CopyID

-- Insert References
INSERT INTO ProjectReference
(ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes)
SELECT        @ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes
FROM            ProjectReference WHERE ProjectID = @CopyID

-- Insert Resources
INSERT INTO ProjectResource
(ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes)
SELECT        @ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes
FROM            ProjectResource WHERE ProjectID = @CopyID

-- Insert Settings
INSERT INTO ProjectSetting
(ProjectID, SettingID, Value)
SELECT        @ProjectID, SettingID, Value
FROM            ProjectSetting WHERE ProjectID = @CopyID

-- Insert User
INSERT INTO [dbo].[ProjectUser]
           ([LoginName]
           ,[ProjectID])
SELECT  USER_NAME(),  @ProjectID

COMMIT
SELECT @ProjectID

GO

GRANT exec on [dbo].[procInsertProjectCopy] TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######   [ProjectLicense]   ######################################################################################
--#####################################################################################################################

GRANT SELECT ON ProjectLicense TO [DiversityWorkbenchUser]
GO

GRANT UPDATE ON ProjectLicense TO [DiversityWorkbenchEditor]
GO

GRANT INSERT ON ProjectLicense TO [DiversityWorkbenchEditor]
GO

GRANT DELETE ON ProjectLicense TO [DiversityWorkbenchEditor]
GO

--#####################################################################################################################
--######   [Enums]   ######################################################################################
--#####################################################################################################################


GRANT SELECT ON ProjectAgentRole_Enum TO [DiversityWorkbenchUser]
GO

GRANT SELECT ON ProjectAgentType_Enum TO [DiversityWorkbenchUser]
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
RETURN '02.01.03'
END

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.07'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Grants for ProjectIdentifier   ##############################################################################
--#####################################################################################################################

GRANT DELETE ON [dbo].[ProjectIdentifier] TO [DiversityWorkbenchAdministrator]
GO
GRANT INSERT ON [dbo].[ProjectIdentifier] TO [DiversityWorkbenchEditor]
GO
GRANT UPDATE ON [dbo].[ProjectIdentifier] TO [DiversityWorkbenchEditor]
GO
GRANT SELECT ON [dbo].[ProjectIdentifier] TO [DiversityWorkbenchUser]
GO

GRANT INSERT ON [dbo].[ProjectIdentifier_log] TO [DiversityWorkbenchEditor]
GO
GRANT SELECT ON [dbo].[ProjectIdentifier_log] TO [DiversityWorkbenchUser]
GO



--#####################################################################################################################
--######  StableIdentifier   ##########################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[StableIdentifier] (@ProjectID int)
RETURNS varchar (500)
/*
Returns the stable identifier for the project.
Test
select dbo.StableIdentifier (1234)
*/
AS
BEGIN
declare @StableIdentifier varchar(500)
set @StableIdentifier = ''
set @StableIdentifier = (select dbo.StableIdentifierBase() + cast(@ProjectID as varchar) from Project P WHERE P.ProjectID = @ProjectID)
return @StableIdentifier
END

GO

GRANT EXECUTE ON dbo.StableIdentifier TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   Adding ViewBaseURL for linked server   #####################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[ViewBaseURL]
	AS
	SELECT        dbo.BaseURL() AS BaseURL
GO

GRANT SELECT ON [dbo].[ViewBaseURL] TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   Adding ViewDiversityWorkbenchModule for linked server   ####################################################
--#####################################################################################################################

CREATE VIEW [dbo].[ViewDiversityWorkbenchModule]
AS
SELECT dbo.DiversityWorkbenchModule() AS DiversityWorkbenchModule
GO

GRANT SELECT ON [dbo].[ViewDiversityWorkbenchModule] TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   Adding ViewProject for linked server   #####################################################################
--#####################################################################################################################


CREATE VIEW dbo.ViewProject
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
  FROM [dbo].[Project]
GO

GRANT SELECT ON [dbo].ViewProject TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   Project_Core: cast(P.ProjectSettings as nvarchar(max)) for use in linked server  ###########################
--#####################################################################################################################


ALTER VIEW [dbo].[Project_Core]
AS
SELECT     P.ProjectID, P.ProjectParentID, P.Project, P.ProjectTitle, P.ProjectDescription, P.ProjectNotes, P.ProjectCopyright, P.ProjectRights, P.ProjectLicenseURI, 
                      P.ProjectVersion, P.ProjectURL, cast(P.ProjectSettings as nvarchar(max)) AS ProjectSettings, P.LogInsertedBy, P.LogInsertedWhen, P.LogUpdatedBy, P.LogUpdatedWhen, P.RowGUID
FROM         dbo.Project AS P INNER JOIN
                      dbo.ProjectList() AS L ON P.ProjectID = L.ProjectID

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
RETURN '02.01.08'
END

GO


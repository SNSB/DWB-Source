
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.00'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ProjectProxy   ######################################################################################
--#####################################################################################################################

ALTER TABLE ProjectProxy ADD [ProjectURI] varchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectURI'
GO


--#####################################################################################################################
--######   UserProxy   ######################################################################################
--#####################################################################################################################

ALTER TABLE UserProxy ADD [Settings] xml NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The settings for the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Settings'
GO



ALTER TABLE UserProxy ADD [AgentURI] varchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A link to a DiversityAgents module where further informations about the user is available.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO



--#####################################################################################################################
--######   [VersionClient]   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.01.01' END
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.01'
END

GO


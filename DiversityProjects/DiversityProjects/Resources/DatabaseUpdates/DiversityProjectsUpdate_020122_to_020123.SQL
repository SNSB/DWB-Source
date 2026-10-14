
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.22'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  Missing Descriptions  #######################################################################################
--#####################################################################################################################

if(select count(*) from INFORMATION_SCHEMA.VIEWS r where r.TABLE_NAME = 'Applications') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'Applications'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'Applications'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table ProjectAgent restricted to user accessible datasets',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectAgent_Core'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table ProjectAgent restricted to user accessible datasets',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectAgent_Core'
END CATCH;
GO

if(select count(*) from INFORMATION_SCHEMA.VIEWS r where r.TABLE_NAME = 'ProjectApplications') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectApplications'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectApplications'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Project including information of parent project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectDefinition'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Project including information of parent project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectDefinition'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table ProjectResource restricted to those accessible for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectResource_Core'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table ProjectResource restricted to those accessible for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectResource_Core'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Value of the function DiversityWorkbenchModule provided via a view for usage e.g. by linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDiversityWorkbenchModule'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Value of the function DiversityWorkbenchModule provided via a view for usage e.g. by linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDiversityWorkbenchModule'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Project including the StableIdentifier',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Project including the StableIdentifier',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewProject'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The StableIdentifier for the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewProject',
@level2type=N'COLUMN', @level2name=N'StableIdentifier'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The StableIdentifier for the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewProject',
@level2type=N'COLUMN', @level2name=N'StableIdentifier'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ApplicationSearchItemPrimaryKeyColumn'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ApplicationSearchItemPrimaryKeyColumn'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Hierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Hierarchy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyChildNodes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyTopID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyTopID'
END CATCH;
GO

if(select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'HierarchyTopID_Main20' and r.ROUTINE_TYPE = 'FUNCTION') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyTopID_Main20'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyTopID_Main20'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Metadata_ABCD'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Metadata_ABCD'
END CATCH;
GO

if(select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'Metadata_ABCD_2' and r.ROUTINE_TYPE = 'FUNCTION') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Metadata_ABCD_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Metadata_ABCD_2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectSettings'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectSettings'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectSettings_ABCD'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectSettings_ABCD'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuColumns'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuColumns'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuForeignKeys'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuForeignKeys'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuInferiorTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuInferiorTables'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuOrderColumn'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuOrderColumn'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuSuperiorTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuSuperiorTables'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuTableTabs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuTableTabs'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectChildren'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectChildren'
END CATCH;
GO


if(select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'procInsertProjectCopy_Main20' and r.ROUTINE_TYPE = 'PROCEDURE') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertProjectCopy_Main20'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertProjectCopy_Main20'
END CATCH;
GO

if(select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'procInsertProjectNew_Main20' and r.ROUTINE_TYPE = 'PROCEDURE') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertProjectNew_Main20'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertProjectNew_Main20'
END CATCH;
GO

if(select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'ProjectSettings_Defaults' and r.ROUTINE_TYPE = 'FUNCTION') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectSettings_Defaults'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectSettings_Defaults'
END CATCH;
GO


if(select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'ProjectSettings_Media' and r.ROUTINE_TYPE = 'FUNCTION') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectSettings_Media'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectSettings_Media'
END CATCH;
GO

--######  Enums  #####################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Language codes used within the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Language_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Language codes used within the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Language_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Roles of the agents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectAgentRole_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Roles of the agents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectAgentRole_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Types of the agents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectAgentType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Types of the agents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectAgentType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Standards for the created archives, e.g. EML',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectArchiveFormat_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Standards for the created archives, e.g. EML',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectArchiveFormat_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The MIME (accoridig to the Multipurpose Internet Mail Extensions specification) types of the created archives',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectArchiveMimeType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The MIME (accoridig to the Multipurpose Internet Mail Extensions specification) types of the created archives',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectArchiveMimeType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Type of the project, e.g. corresponding to a DWB module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Type of the project, e.g. corresponding to a DWB module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectType_Enum'
END CATCH;
GO


--#####################################################################################################################
--######  View_ProjectDescriptor  #####################################################################################
--#####################################################################################################################

CREATE VIEW View_ProjectDescriptor AS
SELECT      D.ProjectID, E.DisplayText + ': ' + D.[Content] AS Descriptor
FROM            ProjectDescriptor AS D INNER JOIN
                         ProjectDescriptorElement AS E ON D.ElementID = E.ElementID
GO

GRANT SELECT ON View_ProjectDescriptor TO [DiversityWorkbenchUser]
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Project descriptors for query',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'View_ProjectDescriptor'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Project descriptors for query',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'View_ProjectDescriptor'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Descriptor of the project (Type + Value)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'View_ProjectDescriptor',
@level2type=N'COLUMN', @level2name=N'Descriptor'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Descriptor of the project (Type + Value)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'View_ProjectDescriptor',
@level2type=N'COLUMN', @level2name=N'Descriptor'
END CATCH;
GO

--#####################################################################################################################
--######  View_ProjectSetting  ########################################################################################
--#####################################################################################################################

CREATE VIEW View_ProjectSetting AS
SELECT        TOP (1000) P.ProjectID, S.DisplayText + ': ' + P.Value AS Setting
FROM            ProjectSetting AS P INNER JOIN
Setting AS S ON P.SettingID = S.SettingID
order by Setting
GO

GRANT SELECT ON View_ProjectSetting TO [DiversityWorkbenchUser]
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Project settings for query',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'View_ProjectSetting'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Project settings for query',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'View_ProjectSetting'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting of the project (Type + Value)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'View_ProjectSetting',
@level2type=N'COLUMN', @level2name=N'Setting'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting of the project (Type + Value)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'View_ProjectSetting',
@level2type=N'COLUMN', @level2name=N'Setting'
END CATCH;
GO


--#####################################################################################################################
--######  Generic SetUserProjects  ####################################################################################
--#####################################################################################################################

/****** Object:  StoredProcedure [dbo].[SetUserProjects]    Script Date: 04.02.2021 15:23:35 ******/
IF 1 = (SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE [SPECIFIC_NAME]='SetUserProjects')
DROP PROCEDURE [dbo].[SetUserProjects]
GO

-- =============================================
-- Author:		Anton Link
-- Create date: 20210205
-- Description:	Create database user and assign
--              training projects     
-- =============================================
CREATE PROCEDURE [dbo].[SetUserProjects] 
	-- Add the parameters for the stored procedure here
	@User VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @Result NVARCHAR(200);
	DECLARE @DatabaseName NVARCHAR(MAX);
	DECLARE @DatabaseRole VARCHAR(100);
	DECLARE @DatabaseUser VARCHAR(100);
	DECLARE @Workshop INT;
	DECLARE @Windows INT;
	SET @DatabaseName=(SELECT DB_NAME());
	SET @DatabaseRole=(SELECT TOP 1 [name] FROM [sys].[database_principals] WHERE [type]='R' AND [name] LIKE '%Administrator');
	SET @Workshop=(SELECT COUNT(*) FROM [UserProxy] WHERE [LoginName]='Workshop');
	SET @Windows=CHARINDEX('\', REVERSE(@User));
	SET @Result=''; 

	IF @Windows=0
	BEGIN
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''S'' AND [name]=''' + @User + ''' ');
		EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;

		IF @DatabaseUser IS NULL
		BEGIN
  			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''U'' AND [name] LIKE ''%\' + @User + ''' ');
			EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;
		END
	END
	ELSE
	BEGIN
		SET @User=REVERSE(SUBSTRING(REVERSE(@User), 1, @Windows));
  		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''U'' AND [name] LIKE ''%' + @User + ''' ');
		EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;
	END
	
	IF @DatabaseUser IS NULL
	BEGIN
		SET @Result='User ' + @User + ' is not present in database ' + @DatabaseName;
	END
	ELSE
	BEGIN
		IF NOT @DatabaseRole IS NULL
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'ALTER ROLE [' + @DatabaseRole + '] ADD MEMBER [' + @DatabaseUser + ']; ')
			EXEC sp_executesql @SQL
			SET @Result='Database role ' + @DatabaseRole + ' set for user ' + @DatabaseUser;
		END
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'DELETE FROM [dbo].[ProjectUser] WHERE [LoginName]=''' + @DatabaseUser + ''' ')
		EXEC sp_executesql @SQL
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'DELETE FROM [dbo].[UserProxy] WHERE [LoginName]=''' + @DatabaseUser + ''' ')
		EXEC sp_executesql @SQL

 		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'INSERT INTO [dbo].[UserProxy] ([LoginName], [CombinedNameCache]) VALUES (''' + @DatabaseUser + ''', ''' + @DatabaseUser + '''); ')
		EXEC sp_executesql @SQL

		IF @Workshop > 0
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'INSERT INTO [dbo].[ProjectUser] ([LoginName], [ProjectID]) ' +
							 'SELECT ''' + @DatabaseUser + ''', [ProjectID] FROM [dbo].[ProjectUser] WHERE [LoginName]=''Workshop''; ')
			EXEC sp_executesql @SQL
			SET @Result=@Result + '; Workshop projects assigned for user ' + @DatabaseUser;
		END
		ELSE
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'INSERT INTO [dbo].[ProjectUser] ([LoginName], [ProjectID]) ' +
							 'SELECT ''' + @DatabaseUser + ''', [ProjectID] FROM [dbo].[ProjectProxy] WHERE [ProjectID]>0; ')
			EXEC sp_executesql @SQL
			SET @Result=@Result + '; Available projects assigned for user ' + @DatabaseUser;
		END
	END
	SELECT @Result
END
GO



--#####################################################################################################################
--######   ProjectAgentRole_Enum - Data Owner and Source Institution   ################################################
--#####################################################################################################################

if (select count(*) from ProjectAgentRole_Enum r where r.Code = 'Data Owner') = 0
begin
	INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Data Owner'
           ,'Person or institution owning or managing property rights, including intellectual property rights over the dataset or data package of a project'
           ,'Data Owner'
           ,1)
end
GO

if (select count(*) from ProjectAgentRole_Enum r where r.Code = 'Source Institution') = 0
begin
	INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Source Institution'
           ,'Organisation with license rights over the datasets or data packages of a project (might be identical with the data owner)'
           ,'Data Source Organisation'
           ,1)
end

update [dbo].[ProjectAgentRole_Enum]
set [DisplayText] = 'Data Source Organisation'
where [Code] = 'Source Institution'

GO


--#####################################################################################################################
--######  ProjectDataLastChanges  #####################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[ProjectDataLastChanges] (@ProjectID int)  
RETURNS datetime
AS  
/*
retrieval of the last update in data of a project
--Test
select dbo.[ProjectDataLastChanges](3)
*/
BEGIN 
declare @LastChanges datetime
declare @Temp datetime

-- last changes in table CollectionSpecimen
set @LastChanges = (select max(P.[LogUpdatedWhen]) 
from [dbo].[Project] P
where  P.ProjectID = @ProjectID)

return @LastChanges
END

GO

GRANT EXEC ON [dbo].[ProjectDataLastChanges] TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######  Set types if missing  #######################################################################################
--#####################################################################################################################

INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Checklist'
           ,'Checklist as used e.g. in DiversityTaxonNames'
           ,'Checklist'
           ,1)
GO

INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Projects'
           ,'Project for the module DiversityProjects'
           ,'Projects'
           ,1)
GO


/*
Agents
Checklist
Collection
Descriptions
Gazetteer
References
Regulation
SamplingPlots
ScientificTerms
TaxonNames
*/

UPDATE P SET P.ProjectType = 'Collection'
FROM [Project] P
WHERE P.Project LIKE '%coll' AND P.ProjectType IS NULL
GO

UPDATE P SET P.ProjectType = 'TaxonNames'
FROM [Project] P
WHERE P.Project LIKE '%names' AND P.ProjectType IS NULL
GO

UPDATE P SET P.ProjectType = 'Checklist'
FROM [Project] P
WHERE P.Project LIKE 'TaxRef_%' AND P.ProjectType IS NULL
GO



--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.03.02' END
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.23'
END

GO


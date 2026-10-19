
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.12'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Missing Descriptions    ####################################################################################
--#####################################################################################################################

--######   VIEWS

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Agents that are available for a user and are not ignored',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'Agent_Core'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Agents that are available for a user and are not ignored',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'Agent_Core' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The name of the agent including indentation for synonyms',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'Agent_Core',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The name of the agent including indentation for synonyms',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'Agent_Core',
@level2type=N'COLUMN', @level2name=N'DisplayText' 
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'IDs of the agent available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_UserAvailable'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'IDs of the agent available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_UserAvailable' 
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Projects available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Projects available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectList' 
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Information about the user as stored in table UserProxy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'UserInfo'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Information about the user as stored in table UserProxy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'UserInfo' 
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accumulated address of an agent within a hierarchy of up to 4 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewAgentAddress'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accumulated address of an agent within a hierarchy of up to 4 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewAgentAddress' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the superior agent within the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewAgentAddress',
@level2type=N'COLUMN', @level2name=N'ParentName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the superior agent within the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewAgentAddress',
@level2type=N'COLUMN', @level2name=N'ParentName' 
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The names of the agents as retrieved for the default display type with function AgentNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewAgentNames'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The names of the agents as retrieved for the default display type with function AgentNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewAgentNames' 
END CATCH;
GO


--######  ViewBaseURL

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Provides the basic address for accessing the database as defined in function BaseURL',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Provides the basic address for accessing the database as defined in function BaseURL',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Basic address for accessing the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL',
@level2type=N'COLUMN', @level2name=N'BaseURL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Basic address for accessing the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL',
@level2type=N'COLUMN', @level2name=N'BaseURL' 
END CATCH;
GO


--######  ViewDefaultAgentNameDisplayType

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The default display type as set for the default project as defined in function DefaultAgentNameDisplayType',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDefaultAgentNameDisplayType'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The default display type as set for the default project as defined in function DefaultAgentNameDisplayType',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDefaultAgentNameDisplayType' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The default display type',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDefaultAgentNameDisplayType',
@level2type=N'COLUMN', @level2name=N'DefaultAgentNameDisplayType'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The default display type',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDefaultAgentNameDisplayType',
@level2type=N'COLUMN', @level2name=N'DefaultAgentNameDisplayType' 
END CATCH;
GO

--######  ViewDiversityWorkbenchModule

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Provides the name of the DiversityWorkbench module as defined in function DiversityWorkbenchModule',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDiversityWorkbenchModule'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Provides the name of the DiversityWorkbench module as defined in function DiversityWorkbenchModule',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDiversityWorkbenchModule' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The name of the DiversityWorkbench module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDiversityWorkbenchModule',
@level2type=N'COLUMN', @level2name=N'DiversityWorkbenchModule'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The name of the DiversityWorkbench module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDiversityWorkbenchModule',
@level2type=N'COLUMN', @level2name=N'DiversityWorkbenchModule' 
END CATCH;
GO

--######   FUNCTIONS

--######  AgentAddress

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The address of the agent as collected from the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentAddress'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The address of the agent as collected from the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentAddress' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The name of the superior agent within the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentAddress',
@level2type=N'COLUMN', @level2name=N'ParentName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The name of the superior agent within the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentAddress',
@level2type=N'COLUMN', @level2name=N'ParentName' 
END CATCH;
GO


--######  AgentChildNodes

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the list of agents within the hierarchy underneath the agent with the given ID (=AgentID)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the list of agents within the hierarchy underneath the agent with the given ID (=AgentID)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentChildNodes' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent for which the hierarchy should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent for which the hierarchy should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID' 
END CATCH;
GO

--######  AgentHierarchy

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the list of agents within the hierarchy starting at the topmost agent related to the agent with the given AgentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the list of agents within the hierarchy starting at the topmost agent related to the agent with the given AgentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentHierarchy' 
END CATCH;
GO

--######  AgentNames

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the agents with their names according to the selected display type',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentNames'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the agents with their names according to the selected display type',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentNames' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The type of the display as documented in the table dbo.AgentNameDisplayType_Enum',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentNames',
@level2type=N'PARAMETER', @level2name=N'@DisplayType'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The type of the display as documented in the table dbo.AgentNameDisplayType_Enum',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentNames',
@level2type=N'PARAMETER', @level2name=N'@DisplayType' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentNames',
@level2type=N'COLUMN', @level2name=N'AgentID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentNames',
@level2type=N'COLUMN', @level2name=N'AgentID' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The name of the agent according to the selected display type',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentNames',
@level2type=N'COLUMN', @level2name=N'AgentName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The name of the agent according to the selected display type',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentNames',
@level2type=N'COLUMN', @level2name=N'AgentName' 
END CATCH;
GO

--###### AgentSuperiorList

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the list of superior agents as retrieved from the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSuperiorList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the list of superior agents as retrieved from the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSuperiorList' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The display order according to the hierarchy where 1 corresponds to the selected agent with increasing values for higher entries within the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSuperiorList',
@level2type=N'COLUMN', @level2name=N'DisplayOrder'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The display order according to the hierarchy where 1 corresponds to the selected agent with increasing values for higher entries within the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSuperiorList',
@level2type=N'COLUMN', @level2name=N'DisplayOrder' 
END CATCH;
GO

--###### AgentSynonymTopID

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the top AgentID within the synyonym hierarchy for a given AgentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSynonymTopID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the top AgentID within the synyonym hierarchy for a given AgentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSynonymTopID' 
END CATCH;
GO

--###### AgentSynonymy

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns all agents within the synonymy of the given agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSynonymy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns all agents within the synonymy of the given agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSynonymy' 
END CATCH;
GO

--###### AgentSynonymyChildNodes

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The depending synonymys of a given agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSynonymyChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The depending synonymys of a given agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSynonymyChildNodes' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent for which the depending synonymys should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSynonymyChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent for which the depending synonymys should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSynonymyChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID' 
END CATCH;
GO


--###### AgentTopID

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the top ID within the hierarchy for a given AgentID from the table Agent.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentTopID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the top ID within the hierarchy for a given AgentID from the table Agent.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentTopID' 
END CATCH;
GO

--###### DefaultAgentNameDisplayType

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The default display type as set for the default project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DefaultAgentNameDisplayType'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The default display type as set for the default project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DefaultAgentNameDisplayType' 
END CATCH;
GO

--###### BaseURL

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Provides the basic address for accessing the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'BaseURL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Provides the basic address for accessing the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'BaseURL' 
END CATCH;
GO

--###### DefaultProjectID

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the default ProjectID for a user as set in table UserProxy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DefaultProjectID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the default ProjectID for a user as set in table UserProxy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DefaultProjectID' 
END CATCH;
GO

--###### DiversityWorkbenchModule

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The name of the DiversityWorkbench module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityWorkbenchModule'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The name of the DiversityWorkbench module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityWorkbenchModule' 
END CATCH;
GO

--###### Version

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Version'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Version' 
END CATCH;
GO

--###### VersionClient

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Version of the client software compatible with the version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'VersionClient'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Version of the client software compatible with the version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'VersionClient' 
END CATCH;
GO


--###### PROCEDURES

--###### procInsertAgentCopy

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Creates a copy of a given agent and returns the AgentID of the new agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertAgentCopy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Creates a copy of a given agent and returns the AgentID of the new agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertAgentCopy' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the created agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertAgentCopy',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the created agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertAgentCopy',
@level2type=N'PARAMETER', @level2name=N'@AgentID' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'A temporary display text for the created agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertAgentCopy',
@level2type=N'PARAMETER', @level2name=N'@AgentName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'A temporary display text for the created agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertAgentCopy',
@level2type=N'PARAMETER', @level2name=N'@AgentName' 
END CATCH;
GO

--###### procSetVersionAgent

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of a dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionAgent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of a dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionAgent' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionAgent',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionAgent',
@level2type=N'PARAMETER', @level2name=N'@ID' 
END CATCH;
GO

--###### ROLES

--###### DiversityWorkbenchAdministrator


BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role for the adminstration of the database',
@level0type = N'USER', @level0name = 'DiversityWorkbenchAdministrator';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role for the adminstration of the database',
@level0type = N'USER', @level0name = 'DiversityWorkbenchAdministrator';
END CATCH;
GO

--###### DiversityWorkbenchEditor

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role with write access for the database',
@level0type = N'USER', @level0name = 'DiversityWorkbenchEditor';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role with write access for the database',
@level0type = N'USER', @level0name = 'DiversityWorkbenchEditor';
END CATCH;
GO

--###### DiversityWorkbenchUser

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role with read only access to the database',
@level0type = N'USER', @level0name = 'DiversityWorkbenchUser';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role with read only access to the database',
@level0type = N'USER', @level0name = 'DiversityWorkbenchUser';
END CATCH;
GO

--#####################################################################################################################
--######   Missing grants    ##########################################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.UserInfo TO DiversityWorkbenchUser
GO

GRANT SELECT ON dbo.AgentChildNodes TO DiversityWorkbenchUser
GO

GRANT EXEC ON dbo.AgentTopID TO DiversityWorkbenchUser
GO

GRANT SELECT ON dbo.SuperiorAgentList TO DiversityWorkbenchUser
GO


GRANT SELECT ON AgentImage_log TO DiversityWorkbenchEditor
GO
GRANT INSERT ON AgentImage_log TO DiversityWorkbenchEditor
GO

GRANT SELECT ON AgentKeyword_log TO DiversityWorkbenchEditor
GO
GRANT INSERT ON AgentKeyword_log TO DiversityWorkbenchEditor
GO

GRANT SELECT ON AgentProject_log TO DiversityWorkbenchEditor
GO
GRANT INSERT ON AgentProject_log TO DiversityWorkbenchEditor
GO

GRANT SELECT ON AgentReference_log TO DiversityWorkbenchEditor
GO
GRANT INSERT ON AgentReference_log TO DiversityWorkbenchEditor
GO

GRANT SELECT ON AgentRelation_log TO DiversityWorkbenchEditor
GO
GRANT INSERT ON AgentRelation_log TO DiversityWorkbenchEditor
GO

GRANT EXEC ON dbo.procInsertAgentCopy TO DiversityWorkbenchEditor
GO

GRANT EXEC ON dbo.procSetVersionAgent TO DiversityWorkbenchEditor
GO



GRANT INSERT ON ApplicationSearchSelectionStrings TO DiversityWorkbenchAdministrator
GO
GRANT UPDATE ON ApplicationSearchSelectionStrings TO DiversityWorkbenchAdministrator
GO
GRANT DELETE ON ApplicationSearchSelectionStrings TO DiversityWorkbenchAdministrator
GO



--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.01.02' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.13'
END

GO

select [dbo].[Version] ()  


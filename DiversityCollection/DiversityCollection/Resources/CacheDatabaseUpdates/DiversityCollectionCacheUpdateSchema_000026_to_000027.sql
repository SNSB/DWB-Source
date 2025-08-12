--#####################################################################################################################
--######   CacheProjectAgentRole - new table for agent roles  #########################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CacheProjectAgentRole' and T.TABLE_SCHEMA = '#project#') = 0
begin
	CREATE TABLE [#project#].[CacheProjectAgentRole](
		[ProjectID] [int] NOT NULL,
		[AgentName] [nvarchar](255) NOT NULL,
		[AgentURI] [varchar](255) NOT NULL,
		[AgentRole] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_ProjectAgentRole] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[AgentName] ASC,
		[AgentURI] ASC,
		[AgentRole] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheProjectAgentRole', @level2type=N'COLUMN',@level2name=N'ProjectID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent. Only cached value where AgentURI is given' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentName'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An URL linked to e.g. the module DiversityAgents within the Diversity Workbench' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentURI'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of the agent within the project as defined in ProjectAgentRole_Enum' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentRole'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of the person or institution involved in the project' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheProjectAgentRole'
end
GO 

GRANT SELECT ON [#project#].[CacheProjectAgentRole] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheProjectAgentRole] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheProjectAgentRole] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheProjectAgentRole] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   procPublishProjectAgentRole - transfer in new table for agent roles  #######################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishProjectAgentRole] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProjectAgentRole]
*/
truncate table [#project#].CacheProjectAgentRole

INSERT INTO [#project#].[CacheProjectAgentRole]
           ([ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole])
SELECT DISTINCT [ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole]
  FROM ' + dbo.ProjectsDatabase() + '.DBO.ProjectAgentRole A
  where A.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


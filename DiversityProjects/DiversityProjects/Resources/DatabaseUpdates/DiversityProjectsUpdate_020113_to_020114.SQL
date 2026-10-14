
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.13'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ProjectAgentRole  ##########################################################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectAgentRole') = 0
begin
	CREATE TABLE [dbo].[ProjectAgentRole](
		[ProjectID] [int] NOT NULL,
		[AgentName] [nvarchar](255) NOT NULL,
		[AgentURI] [varchar](255) NOT NULL,
		[AgentRole] [nvarchar](50) NOT NULL,
		[LogInsertedBy] [nvarchar](50) NULL,
		[LogInsertedWhen] [smalldatetime] NULL,
		[LogUpdatedBy] [nvarchar](50) NULL,
		[LogUpdatedWhen] [smalldatetime] NULL,
		[RowGUID] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_ProjectAgentRole] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[AgentName] ASC,
		[AgentURI] ASC,
		[AgentRole] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[ProjectAgentRole] ADD  DEFAULT (suser_sname()) FOR [LogInsertedBy]
	ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [DF_ProjectAgentRole_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
	ALTER TABLE [dbo].[ProjectAgentRole] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
	ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [DF_ProjectAgentRole_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
	ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [DF_ProjectAgentRole_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]

	ALTER TABLE [dbo].[ProjectAgentRole]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgentRole_ProjectAgent] FOREIGN KEY([ProjectID], [AgentName], [AgentURI])
	REFERENCES [dbo].[ProjectAgent] ([ProjectID], [AgentName], [AgentURI])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ProjectAgentRole] CHECK CONSTRAINT [FK_ProjectAgentRole_ProjectAgent]

	ALTER TABLE [dbo].[ProjectAgentRole]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgentRole_ProjectAgentRole_Enum] FOREIGN KEY([AgentRole])
	REFERENCES [dbo].[ProjectAgentRole_Enum] ([Code])

	ALTER TABLE [dbo].[ProjectAgentRole] CHECK CONSTRAINT [FK_ProjectAgentRole_ProjectAgentRole_Enum]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'ProjectID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent. Only cached value where AgentURI is given' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentName'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An URL linked to e.g. the module DiversityAgents within the Diversity Workbench' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentURI'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of the agent within the project as defined in ProjectAgentRole_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentRole'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of the person or institution involved in the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole'

end
GO

GRANT SELECT ON [ProjectAgentRole] TO [DiversityWorkbenchUser]
GO
GRANT UPDATE ON [ProjectAgentRole] TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON [ProjectAgentRole] TO [DiversityWorkbenchEditor]
GO
GRANT DELETE ON [ProjectAgentRole] TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######   ProjectAgentRole_log    ####################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectAgentRole_log') = 0
begin
	CREATE TABLE [dbo].[ProjectAgentRole_log](
		[ProjectID] [int] NULL,
		[AgentName] [nvarchar](255) NULL,
		[AgentURI] [varchar](255) NULL,
		[AgentRole] [nvarchar](50) NULL,
		[LogInsertedBy] [nvarchar](50) NULL,
		[LogInsertedWhen] [smalldatetime] NULL,
		[LogUpdatedBy] [nvarchar](50) NULL,
		[LogUpdatedWhen] [smalldatetime] NULL,
		[RowGUID] [uniqueidentifier] NULL,
		[LogState] [char](1) NULL,
		[LogDate] [datetime] NOT NULL,
		[LogUser] [nvarchar](50) NULL,
		[LogID] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_ProjectAgentRole_Log] PRIMARY KEY CLUSTERED 
	(
		[LogID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ProjectAgentRole_log] ADD  CONSTRAINT [DF_ProjectAgentRole_Log_LogState]  DEFAULT ('U') FOR [LogState]
	ALTER TABLE [dbo].[ProjectAgentRole_log] ADD  CONSTRAINT [DF_ProjectAgentRole_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
	ALTER TABLE [dbo].[ProjectAgentRole_log] ADD  CONSTRAINT [DF_ProjectAgentRole_Log_LogUser]  DEFAULT (suser_sname()) FOR [LogUser]
end
GO

GRANT SELECT ON [ProjectAgentRole_log] TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON [ProjectAgentRole_log] TO [DiversityWorkbenchEditor]
GO



--#####################################################################################################################
--######   trgDelProjectAgentRole    ##################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectAgentRole] ON [dbo].[ProjectAgentRole] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.7 */ 
/*  Date: 03.01.2018  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectAgentRole_Log (ProjectID, AgentName, AgentURI, AgentRole, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   trgUpdProjectAgentRole    ##################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectAgentRole] ON [dbo].[ProjectAgentRole] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.7 */ 
/*  Date: 03.01.2018  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectAgentRole_Log (ProjectID, AgentName, AgentURI, AgentRole, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectAgentRole
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectAgentRole, deleted 
where 1 = 1 
AND ProjectAgentRole.AgentName = deleted.AgentName
AND ProjectAgentRole.AgentRole = deleted.AgentRole
AND ProjectAgentRole.AgentURI = deleted.AgentURI
AND ProjectAgentRole.ProjectID = deleted.ProjectID
GO


--#####################################################################################################################
--######  Transfer of data into new table   ###########################################################################
--#####################################################################################################################

INSERT INTO [dbo].[ProjectAgentRole]
           ([ProjectID]
           ,[AgentName]
           ,[AgentURI]
           ,[AgentRole]
           ,[LogInsertedBy]
           ,[LogInsertedWhen]
           ,[LogUpdatedBy]
           ,[LogUpdatedWhen])
SELECT [ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole]
      ,[LogInsertedBy]
      ,[LogInsertedWhen]
      ,[LogUpdatedBy]
      ,[LogUpdatedWhen]
  FROM [dbo].[ProjectAgent] A 
  WHERE NOT A.AgentRole IS NULL
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
RETURN '02.01.14'
END

GO


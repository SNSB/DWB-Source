
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.21'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ProjectAgentType_Enum - Removing Submitter   ###############################################################
--#####################################################################################################################

UPDATE [dbo].[ProjectAgent]
   SET [AgentType] = 'Person'
 WHERE [AgentType] = 'Submitter'
GO

DELETE FROM [dbo].[ProjectAgentType_Enum]
 WHERE [Code] = 'Submitter'
GO



--#####################################################################################################################
--######   ProjectAgent - Changing PK   ###############################################################################
--#####################################################################################################################

-- Adding the new column
if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectAgent' and C.COLUMN_NAME = 'ProjectAgentID') = 0
begin
	ALTER TABLE ProjectAgent ADD [ProjectAgentID] [int] IDENTITY(1,1) NOT NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent, Primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'ProjectAgentID'
	ALTER TABLE ProjectAgent_log ADD [ProjectAgentID] [int] NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectAgentRole' and C.COLUMN_NAME = 'ProjectAgentID') = 0
begin
	ALTER TABLE ProjectAgentRole ADD [ProjectAgentID] [int] NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent, Part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'ProjectAgentID'
	ALTER TABLE ProjectAgentRole_log ADD [ProjectAgentID] [int] NULL;
end
GO


-- Updating the ProjectAgentID
UPDATE R SET  R.ProjectAgentID = A.ProjectAgentID
FROM ProjectAgentRole AS R INNER JOIN
ProjectAgent AS A ON R.ProjectID = A.ProjectID AND R.AgentName = A.AgentName AND R.AgentURI = A.AgentURI
GO

-- alter ProjectAgentID
ALTER TABLE ProjectAgentRole ALTER COLUMN ProjectAgentID int NOT NULL
GO

-- Removing the link to ProjectAgentRole_Enum
if (select count(*) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE K where K.CONSTRAINT_NAME = 'FK_ProjectAgent_ProjectAgentRole_Enum') > 0
begin
	ALTER TABLE [dbo].[ProjectAgent] DROP CONSTRAINT [FK_ProjectAgent_ProjectAgentRole_Enum]
end
GO
ALTER TABLE [dbo].[ProjectAgent]  DROP  CONSTRAINT [FK_ProjectAgent_Project]
GO

-- Removing the relation
ALTER TABLE [dbo].[ProjectAgentRole] DROP CONSTRAINT [FK_ProjectAgentRole_ProjectAgent]
GO


-- Removing the Primary keys
ALTER TABLE [dbo].[ProjectAgent] DROP CONSTRAINT [PK_ProjectAgent] WITH ( ONLINE = OFF )
GO
ALTER TABLE [dbo].[ProjectAgentRole] DROP CONSTRAINT [PK_ProjectAgentRole] WITH ( ONLINE = OFF )
GO

-- Adding the new primary keys
ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [PK_ProjectAgent] PRIMARY KEY CLUSTERED 
(
	[ProjectAgentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [PK_ProjectAgentRole] PRIMARY KEY CLUSTERED 
(
	[ProjectAgentID] ASC,
	[AgentRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Adding the relations
ALTER TABLE [dbo].[ProjectAgent]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgent_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectAgent] CHECK CONSTRAINT [FK_ProjectAgent_Project]
GO

ALTER TABLE [dbo].[ProjectAgentRole]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgentRole_ProjectAgent] FOREIGN KEY([ProjectAgentID])
REFERENCES [dbo].[ProjectAgent] ([ProjectAgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectAgentRole] CHECK CONSTRAINT [FK_ProjectAgentRole_ProjectAgent]
GO


--#####################################################################################################################
--######   trgDelProjectAgent   #######################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelProjectAgent] ON [dbo].[ProjectAgent] 
FOR DELETE AS 
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes, AgentSequence, ProjectAgentID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.AgentType, deleted.Notes, deleted.AgentSequence, deleted.ProjectAgentID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO

--#####################################################################################################################
--######   trgUpdProjectAgent   #######################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdProjectAgent] ON [dbo].[ProjectAgent] 
FOR UPDATE AS
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes, AgentSequence, ProjectAgentID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.AgentType, deleted.Notes, deleted.AgentSequence, deleted.ProjectAgentID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED
Update ProjectAgent
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM ProjectAgent, deleted 
where 1 = 1 
AND ProjectAgent.ProjectAgentID = deleted.ProjectAgentID
GO

--#####################################################################################################################
--######   trgDelProjectAgentRole   ###################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelProjectAgentRole] ON [dbo].[ProjectAgentRole] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.7 */ 
/*  Date: 03.01.2018  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectAgentRole_Log (ProjectID, AgentName, AgentURI, AgentRole, ProjectAgentID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.ProjectAgentID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO


--#####################################################################################################################
--######   trgUpdProjectAgentRole   ###################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdProjectAgentRole] ON [dbo].[ProjectAgentRole] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.7 */ 
/*  Date: 03.01.2018  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectAgentRole_Log (ProjectID, AgentName, AgentURI, AgentRole, ProjectAgentID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.ProjectAgentID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectAgentRole
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectAgentRole, deleted 
where 1 = 1 
AND ProjectAgentRole.ProjectAgentID = deleted.ProjectAgentID
AND ProjectAgentRole.AgentRole = deleted.AgentRole
GO


--#####################################################################################################################
--######   ProjectAgentRole - ProjectAgentRole_Enum: Change relation to UPDATE CASCADE   ##############################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectAgentRole] DROP CONSTRAINT [FK_ProjectAgentRole_ProjectAgentRole_Enum]
GO

ALTER TABLE [dbo].[ProjectAgentRole]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgentRole_ProjectAgentRole_Enum] FOREIGN KEY([AgentRole])
REFERENCES [dbo].[ProjectAgentRole_Enum] ([Code])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ProjectAgentRole] CHECK CONSTRAINT [FK_ProjectAgentRole_ProjectAgentRole_Enum]
GO


--#####################################################################################################################
--######   ProjectAgentRole_Enum - Author   ###########################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Author/Creator',
[Description] = 'Main researcher involved in producing the dataset or data package of a project, or the author of the (data) publication (may be a corporate/institutional or personal name)'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Author'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Content Contact   ##################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Content Contact',
[Description] = 'Person with knowledge of how to access, troubleshoot, or otherwise field issues related to the content of the dataset or data package in a project'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Content Contact'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - curator   ##########################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Data Curator'
, [Description] = 'Person tasked with curating, reviewing, enhancing, cleaning, or standardizing the content of a dataset or data package managed in a project or submitted for storage, use, and maintenance within a data repository'
, [Code] = 'Data Curator'
FROM ProjectAgentRole_Enum R
WHERE Code = 'curator'


--#####################################################################################################################
--######   ProjectAgentRole_Enum - editor   ###########################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Data Editor/Data Manager'
, [Description] = 'Person (or organisation with a staff of data managers) responsible for maintaining the finished dataset or data package'
, [Code] = 'Data Editor'
FROM ProjectAgentRole_Enum R
WHERE Code = 'editor'


--#####################################################################################################################
--######   ProjectAgentRole_Enum - institution   ######################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Data Owner'
, [Description] = 'Person or institution owning or managing property rights, including intellectual property rights over the dataset or data package of a project'
, [Code] = 'Data Owner'
FROM ProjectAgentRole_Enum R
WHERE Code = 'institution'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Owner   ############################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Data Owner Contact'
, [Description] = 'Representative for person or institution owning or managing property rights, including intellectual property rights over the dataset or data package of a project'
, [Code] = 'Data Owner Contact'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Owner'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Principal investigator   ###########################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Principal Investigator'
, [Description] = 'Person leading an investigation, research study or research group'
, [Code] = 'Principal Investigator'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Principal investigator'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Publisher   ########################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Data Publisher',
[Description] = 'Organisation publishing  datasets or data packages'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Publisher'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Researcher   #######################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Researcher',
[Description] = 'Person involved in analyzing data or the results of an experiment or formal study. May indicate an intern or assistant to one of the authors who helped with research but who was not so “key” as to be listed as an author'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Researcher'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Source Institution   ###############################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Data Source Organisation',
[Description] = 'Organisation with license rights over the datasets or data packages of a project (might be identical with the data owner)'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Source Institution'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Submitter   ########################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Submitter',
[Description] = 'Person submitting a dataset or data package to a data repository or data publisher'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Submitter'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Technical Contact   ################################################################
--#####################################################################################################################

UPDATE R SET [DisplayText] = 'Technical Contact',
[Description] = 'Person or institution (e.g., data repository) with knowledge of how to access or troubleshoot technical issues related to the datasets or data packages of a project'
FROM ProjectAgentRole_Enum R
WHERE Code = 'Technical Contact'

--#####################################################################################################################
--######   ProjectAgentRole_Enum - Other   ############################################################################
--#####################################################################################################################

if (select count(*) from ProjectAgentRole_Enum r where r.Code = 'Other') = 0
begin
	INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Other'
           ,'Person or institution making a significant contribution to the development and/or maintenance of the dataset or data package of a project, but whose contribution does not "fit"'
           ,'Other'
           ,1)
end


--#####################################################################################################################
--######   Correction of descriptions   ###############################################################################
--#####################################################################################################################

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Public description of the project if different from the description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'PublicDescription'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Internal description of the project, not to be published' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'InternalDescription'
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
RETURN '02.01.22'
END

GO


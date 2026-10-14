
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
--######   ProjectReference - ReferenceTitle -> nvarchar(500)   #######################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectReference] DROP CONSTRAINT [PK_ProjectReference]
GO

ALTER TABLE ProjectReference ALTER Column ReferenceTitle nvarchar(500) NOT NULL;
GO

ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [PK_ProjectReference] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReferenceTitle] ASC
)
GO

ALTER TABLE ProjectReference_log ALTER Column ReferenceTitle nvarchar(500) NOT NULL;


--#####################################################################################################################
--######   ProjectKeyword    ##########################################################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectKeyword') = 0
begin
	CREATE TABLE [dbo].[ProjectKeyword](
		[ProjectID] [int] NOT NULL,
		[Keyword] [nvarchar](255) NOT NULL,
		[LogInsertedBy] [nvarchar](50) NULL CONSTRAINT [DF_ProjectKeyword_LogInsertedBy]  DEFAULT (suser_sname()),
		[LogInsertedWhen] [smalldatetime] NULL CONSTRAINT [DF_ProjectKeyword_LogInsertedWhen]  DEFAULT (getdate()),
		[LogUpdatedBy] [nvarchar](50) NULL CONSTRAINT [DF_ProjectKeyword_LogUpdatedBy]  DEFAULT (suser_sname()),
		[LogUpdatedWhen] [smalldatetime] NULL CONSTRAINT [DF_ProjectKeyword_LogUpdatedWhen]  DEFAULT (getdate()),
		[RowGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProjectKeyword_RowGUID]  DEFAULT (newsequentialid()),
	 CONSTRAINT [PK_ProjectKeyword] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[Keyword] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ProjectKeyword]  WITH CHECK ADD  CONSTRAINT [FK_ProjectKeyword_Project] FOREIGN KEY([ProjectID])
	REFERENCES [dbo].[Project] ([ProjectID])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ProjectKeyword] CHECK CONSTRAINT [FK_ProjectKeyword_Project]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'ProjectID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The keyword related to the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'Keyword'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Keywords for the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword'
end
GO

GRANT SELECT ON [ProjectKeyword] TO [DiversityWorkbenchUser]
GO
GRANT UPDATE ON [ProjectKeyword] TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON [ProjectKeyword] TO [DiversityWorkbenchEditor]
GO
GRANT DELETE ON [ProjectKeyword] TO [DiversityWorkbenchEditor]
GO



--#####################################################################################################################
--######   ProjectKeyword_log    ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectKeyword_log') = 0
begin
	CREATE TABLE [dbo].[ProjectKeyword_log](
		[ProjectID] [int] NULL,
		[Keyword] [nvarchar](255) NULL,
		[LogInsertedBy] [nvarchar](50) NULL,
		[LogInsertedWhen] [smalldatetime] NULL,
		[LogUpdatedBy] [nvarchar](50) NULL,
		[LogUpdatedWhen] [smalldatetime] NULL,
		[RowGUID] [uniqueidentifier] NULL,
		[LogState] [char](1) NULL,
		[LogDate] [datetime] NOT NULL,
		[LogUser] [nvarchar](50) NULL,
		[LogID] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_ProjectKeyword_Log] PRIMARY KEY CLUSTERED 
	(
		[LogID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ProjectKeyword_log] ADD  CONSTRAINT [DF_ProjectKeyword_Log_LogState]  DEFAULT ('U') FOR [LogState]
	ALTER TABLE [dbo].[ProjectKeyword_log] ADD  CONSTRAINT [DF_ProjectKeyword_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
	ALTER TABLE [dbo].[ProjectKeyword_log] ADD  CONSTRAINT [DF_ProjectKeyword_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
end
GO

GRANT SELECT ON [ProjectKeyword_log] TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON [ProjectKeyword_log] TO [DiversityWorkbenchEditor]
GO



--#####################################################################################################################
--######   trgDelProjectKeyword    ####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectKeyword] ON [dbo].[ProjectKeyword] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.5 */ 
/*  Date: 08.08.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectKeyword_Log (ProjectID, Keyword, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Keyword, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   trgUpdProjectKeyword    ####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectKeyword] ON [dbo].[ProjectKeyword] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.5 */ 
/*  Date: 08.08.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectKeyword_Log (ProjectID, Keyword, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Keyword, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update ProjectKeyword
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectKeyword, deleted 
where 1 = 1 
AND ProjectKeyword.Keyword = deleted.Keyword
AND ProjectKeyword.ProjectID = deleted.ProjectID

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
RETURN '02.01.13'
END

GO


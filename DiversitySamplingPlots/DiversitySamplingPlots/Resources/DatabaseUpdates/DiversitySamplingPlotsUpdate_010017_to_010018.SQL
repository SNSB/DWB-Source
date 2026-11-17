
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.17'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   [Check DiversitySamplingPlots database for inconsistency]   ################################################
--#####################################################################################################################


-- Add PropertyGeometry column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotProperty_log' AND T.COLUMN_NAME = 'PropertyGeometry') = 0
BEGIN
   ALTER TABLE SamplingPlotProperty_log ADD [PropertyGeometry] [geometry] NULL;
END
GO

-- Add table ReplicationPublisher if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'ReplicationPublisher') = 0
BEGIN
  CREATE TABLE [dbo].[ReplicationPublisher](
    [DatabaseName] [varchar](255) NOT NULL, 
    [Server] [varchar](255) NOT NULL, 
    [Port] [smallint] NULL, 
    [LogCreatedWhen] [datetime] NULL, 
    [LogCreatedBy] [nvarchar](50) NULL, 
    [LogUpdatedWhen] [datetime] NULL, 
    [LogUpdatedBy] [nvarchar](50) NULL, 
    CONSTRAINT [PK_ReplicationPublisher] PRIMARY KEY CLUSTERED  
    ([DatabaseName] ASC, 
    [Server] ASC 
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
    ) ON [PRIMARY]; 

  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the publishing database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'DatabaseName';
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or address of the server where the publishing database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Server';
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The port used by the server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Port';
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen';
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogCreatedBy';
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen';
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy';

  ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen];
  ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy];
  ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen];
  ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy];
END 
GO  

GRANT DELETE ON dbo.ReplicationPublisher TO DiversityWorkbenchAdministrator
GO 
GRANT INSERT ON dbo.ReplicationPublisher TO DiversityWorkbenchAdministrator
GO 
GRANT UPDATE ON dbo.ReplicationPublisher TO DiversityWorkbenchAdministrator
GO 
GRANT SELECT ON dbo.ReplicationPublisher TO DiversityWorkbenchAdministrator
GO 

--################################################################################################
--  LocalisationSystem  ##########################################################################
--################################################################################################

-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'LocalisationSystem' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [LocalisationSystem] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocalisationSystem', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocalisationSystem', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocalisationSystem', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocalisationSystem', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'LocalisationSystem' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [LocalisationSystem] ADD CONSTRAINT DF_LocalisationSystem_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [LocalisationSystem] ADD CONSTRAINT DF_LocalisationSystem_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [LocalisationSystem] ADD CONSTRAINT DF_LocalisationSystem_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [LocalisationSystem] ADD CONSTRAINT DF_LocalisationSystem_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdLocalisationSystem]')) DROP TRIGGER [dbo].[trgUpdLocalisationSystem]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelLocalisationSystem]')) DROP TRIGGER [dbo].[trgDelLocalisationSystem]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [LocalisationSystem] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [LocalisationSystem] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [LocalisationSystem] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'LocalisationSystem' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [LocalisationSystem] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [LocalisationSystem] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'LocalisationSystem' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [LocalisationSystem] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'LocalisationSystem_log') = 0
BEGIN
CREATE TABLE [dbo].[LocalisationSystem_log] 
([LocalisationSystemID] [int] NULL,
 [LocalisationSystemParentID] [int] NULL,
 [LocalisationSystemName] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
 [DefaultAccuracyOfLocalisation] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [DefaultMeasurementUnit] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [ParsingMethodName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [DisplayEnable] [bit] NULL,
 [DisplayOrder] [smallint] NULL,
 [Description] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [DisplayTextLocation1] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [DescriptionLocation1] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [DisplayTextLocation2] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [DescriptionLocation2] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [RowGUID] [uniqueidentifier] NULL,
 [LogCreatedWhen] [datetime],
[LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS,
[LogUpdatedWhen] [datetime],
[LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS,
 [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_LocalisationSystem_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_LocalisationSystem_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_LocalisationSystem_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_LocalisationSystem_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'LocalisationSystem_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [LocalisationSystem_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'LocalisationSystem_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [LocalisationSystem_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'LocalisationSystem_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[LocalisationSystem_RowGUID]([LocalisationSystemID] [int],
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_LocalisationSystem_RowGUID] PRIMARY KEY CLUSTERED
([LocalisationSystemID] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[LocalisationSystem_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [LocalisationSystem_RowGUID] ([LocalisationSystemID]) 
SELECT [LocalisationSystemID]
FROM [LocalisationSystem] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [LocalisationSystem] AS T  INNER JOIN [LocalisationSystem_RowGUID] AS R ON 
T.[LocalisationSystemID] = R.[LocalisationSystemID]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [LocalisationSystem_log] AS T INNER JOIN [LocalisationSystem_RowGUID] AS R ON 
T.[LocalisationSystemID] = R.[LocalisationSystemID]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[LocalisationSystem_RowGUID]
GO



-- Create update trigger

CREATE TRIGGER [trgUpdLocalisationSystem] ON [dbo].[LocalisationSystem] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO LocalisationSystem_Log (LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, DefaultAccuracyOfLocalisation, DefaultMeasurementUnit, ParsingMethodName, DisplayText, DisplayEnable, DisplayOrder, Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2, DescriptionLocation2, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.LocalisationSystemID, deleted.LocalisationSystemParentID, deleted.LocalisationSystemName, deleted.DefaultAccuracyOfLocalisation, deleted.DefaultMeasurementUnit, deleted.ParsingMethodName, deleted.DisplayText, deleted.DisplayEnable, deleted.DisplayOrder, deleted.Description, deleted.DisplayTextLocation1, deleted.DescriptionLocation1, deleted.DisplayTextLocation2, deleted.DescriptionLocation2, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update LocalisationSystem
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM LocalisationSystem, deleted 
where 1 = 1 
AND LocalisationSystem.LocalisationSystemID = deleted.LocalisationSystemID

GO



-- Create delete trigger

CREATE TRIGGER [trgDelLocalisationSystem] ON [dbo].[LocalisationSystem] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO LocalisationSystem_Log (LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, DefaultAccuracyOfLocalisation, DefaultMeasurementUnit, ParsingMethodName, DisplayText, DisplayEnable, DisplayOrder, Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2, DescriptionLocation2, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.LocalisationSystemID, deleted.LocalisationSystemParentID, deleted.LocalisationSystemName, deleted.DefaultAccuracyOfLocalisation, deleted.DefaultMeasurementUnit, deleted.ParsingMethodName, deleted.DisplayText, deleted.DisplayEnable, deleted.DisplayOrder, deleted.Description, deleted.DisplayTextLocation1, deleted.DescriptionLocation1, deleted.DisplayTextLocation2, deleted.DescriptionLocation2, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED


GO

--##########################################################################################
--  ProjectProxy  ##########################################################################
--##########################################################################################

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'ProjectProxy' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [ProjectProxy] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [ProjectProxy] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectProxy' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [ProjectProxy] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectProxy_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[ProjectProxy_RowGUID]([ProjectID] [int],
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProjectProxy_RowGUID] PRIMARY KEY CLUSTERED
([ProjectID] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[ProjectProxy_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [ProjectProxy_RowGUID] ([ProjectID]) 
SELECT [ProjectID]
FROM [ProjectProxy] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [ProjectProxy] AS T  INNER JOIN [ProjectProxy_RowGUID] AS R ON 
T.[ProjectID] = R.[ProjectID]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[ProjectProxy_RowGUID]
GO

--#########################################################################################
--  ProjectUser  ##########################################################################
--#########################################################################################

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'ProjectUser' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [ProjectUser] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [ProjectUser] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectUser' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [ProjectUser] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectUser_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[ProjectUser_RowGUID]([LoginName] [nvarchar](50) NOT NULL ,
 [ProjectID] [int],
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProjectUser_RowGUID] PRIMARY KEY CLUSTERED
([LoginName] ASC , [ProjectID] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[ProjectUser_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [ProjectUser_RowGUID] ([LoginName], [ProjectID]) 
SELECT [LoginName], [ProjectID]
FROM [ProjectUser] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [ProjectUser] AS T  INNER JOIN [ProjectUser_RowGUID] AS R ON 
T.[LoginName] = R.[LoginName] AND T.[ProjectID] = R.[ProjectID]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[ProjectUser_RowGUID]
GO

--######################################################################################
--  Property  ##########################################################################
--######################################################################################

-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'Property' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [Property] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Property', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Property', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Property', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Property', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'Property' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [Property] ADD CONSTRAINT DF_Property_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [Property] ADD CONSTRAINT DF_Property_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [Property] ADD CONSTRAINT DF_Property_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [Property] ADD CONSTRAINT DF_Property_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdProperty]')) DROP TRIGGER [dbo].[trgUpdProperty]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelProperty]')) DROP TRIGGER [dbo].[trgDelProperty]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [Property] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [Property] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [Property] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'Property' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [Property] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [Property] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'Property' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [Property] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'Property_log') = 0
BEGIN
CREATE TABLE [dbo].[Property_log] 
([PropertyID] [int] NULL,
 [DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [Description] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [PropertyParentID] [int] NULL,
 [PropertyName] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
 [DefaultAccuracyOfProperty] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [DefaultMeasurementUnit] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [ParsingMethodName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [DisplayEnabled] [bit] NULL,
 [DisplayOrder] [smallint] NULL,
 [RowGUID] [uniqueidentifier],
[LogCreatedWhen] [datetime],
[LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS,
[LogUpdatedWhen] [datetime],
[LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS,
 [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_Property_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_Property_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_Property_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_Property_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'Property_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [Property_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'Property_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [Property_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'Property_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[Property_RowGUID]([PropertyID] [int],
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Property_RowGUID] PRIMARY KEY CLUSTERED
([PropertyID] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[Property_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [Property_RowGUID] ([PropertyID]) 
SELECT [PropertyID]
FROM [Property] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [Property] AS T  INNER JOIN [Property_RowGUID] AS R ON 
T.[PropertyID] = R.[PropertyID]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [Property_log] AS T INNER JOIN [Property_RowGUID] AS R ON 
T.[PropertyID] = R.[PropertyID]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[Property_RowGUID]
GO



-- Create update trigger

CREATE TRIGGER [trgUpdProperty] ON [dbo].[Property] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Property_Log (PropertyID, DisplayText, Description, PropertyParentID, PropertyName, DefaultAccuracyOfProperty, DefaultMeasurementUnit, ParsingMethodName, DisplayEnabled, DisplayOrder, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.PropertyID, deleted.DisplayText, deleted.Description, deleted.PropertyParentID, deleted.PropertyName, deleted.DefaultAccuracyOfProperty, deleted.DefaultMeasurementUnit, deleted.ParsingMethodName, deleted.DisplayEnabled, deleted.DisplayOrder, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update Property
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Property, deleted 
where 1 = 1 
AND Property.PropertyID = deleted.PropertyID

GO



-- Create delete trigger

CREATE TRIGGER [trgDelProperty] ON [dbo].[Property] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Property_Log (PropertyID, DisplayText, Description, PropertyParentID, PropertyName, DefaultAccuracyOfProperty, DefaultMeasurementUnit, ParsingMethodName, DisplayEnabled, DisplayOrder, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.PropertyID, deleted.DisplayText, deleted.Description, deleted.PropertyParentID, deleted.PropertyName, deleted.DefaultAccuracyOfProperty, deleted.DefaultMeasurementUnit, deleted.ParsingMethodName, deleted.DisplayEnabled, deleted.DisplayOrder, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED


GO

--##########################################################################################
--  SamplingPlot  ##########################################################################
--##########################################################################################

-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlot' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlot] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlot', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlot', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlot', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlot', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlot' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlot] ADD CONSTRAINT DF_SamplingPlot_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [SamplingPlot] ADD CONSTRAINT DF_SamplingPlot_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [SamplingPlot] ADD CONSTRAINT DF_SamplingPlot_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [SamplingPlot] ADD CONSTRAINT DF_SamplingPlot_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdSamplingPlot]')) DROP TRIGGER [dbo].[trgUpdSamplingPlot]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelSamplingPlot]')) DROP TRIGGER [dbo].[trgDelSamplingPlot]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [SamplingPlot] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [SamplingPlot] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [SamplingPlot] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlot' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlot] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [SamplingPlot] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlot' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlot] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlot_log') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlot_log] 
([PlotID] [int] NULL,
 [PartOfPlotID] [int] NULL,
 [PlotIdentifier] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
 [PlotGeography_Cache] [geography] NULL,
 [PlotGeometry_Cache] [geometry] NULL,
 [PlotDescription] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [PlotType] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [InternalNotes] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [LogCreatedWhen] [datetime] NULL,
 [LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [datetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [CountryCache] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [RowGUID] [uniqueidentifier] NULL,
  [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlot_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_SamplingPlot_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlot_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_SamplingPlot_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlot_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlot_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlot_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlot_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'SamplingPlot_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlot_RowGUID]([PlotID] [int],
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SamplingPlot_RowGUID] PRIMARY KEY CLUSTERED
([PlotID] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[SamplingPlot_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [SamplingPlot_RowGUID] ([PlotID]) 
SELECT [PlotID]
FROM [SamplingPlot] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [SamplingPlot] AS T  INNER JOIN [SamplingPlot_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [SamplingPlot_log] AS T INNER JOIN [SamplingPlot_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[SamplingPlot_RowGUID]
GO



-- Create update trigger

-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgUpdSamplingPlot] ON [dbo].[SamplingPlot] 
--FOR UPDATE AS
--INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, PlotType, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, CountryCache, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.PlotType, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.CountryCache, deleted.RowGUID,  'U'
--FROM DELETED
--Update SamplingPlot
--set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
--FROM SamplingPlot, deleted 
--where 1 = 1 
--AND SamplingPlot.PlotID = deleted.PlotID
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgUpdSamplingPlot] ON [dbo].[SamplingPlot] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, PlotType, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, CountryCache, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.PlotType, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.CountryCache, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlot
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlot, deleted 
where 1 = 1 
AND SamplingPlot.PlotID = deleted.PlotID

GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingPlot] ON [dbo].[SamplingPlot] 
--FOR DELETE AS 
--INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, PlotType, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, CountryCache, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.PlotType, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.CountryCache, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgDelSamplingPlot] ON [dbo].[SamplingPlot] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, PlotType, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, CountryCache, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.PlotType, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.CountryCache, deleted.RowGUID,  'D'
FROM DELETED


GO

--###############################################################################################
--  SamplingPlotImage  ##########################################################################
--###############################################################################################

-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotImage' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlotImage] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotImage', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotImage', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotImage', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotImage', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotImage' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlotImage] ADD CONSTRAINT DF_SamplingPlotImage_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [SamplingPlotImage] ADD CONSTRAINT DF_SamplingPlotImage_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [SamplingPlotImage] ADD CONSTRAINT DF_SamplingPlotImage_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [SamplingPlotImage] ADD CONSTRAINT DF_SamplingPlotImage_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdSamplingPlotImage]')) DROP TRIGGER [dbo].[trgUpdSamplingPlotImage]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelSamplingPlotImage]')) DROP TRIGGER [dbo].[trgDelSamplingPlotImage]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [SamplingPlotImage] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [SamplingPlotImage] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [SamplingPlotImage] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotImage' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlotImage] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [SamplingPlotImage] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotImage' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlotImage] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotImage_log') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlotImage_log] 
([PlotID] [int] NULL,
 [URI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ResourceURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ImageType] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [Notes] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [LogCreatedWhen] [datetime] NULL,
 [LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [datetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [RowGUID] [uniqueidentifier] NULL,
  [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlotImage_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_SamplingPlotImage_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlotImage_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_SamplingPlotImage_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotImage_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlotImage_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotImage_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlotImage_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'SamplingPlotImage_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlotImage_RowGUID]([PlotID] [int],
 [URI] [varchar](255) NOT NULL ,
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SamplingPlotImage_RowGUID] PRIMARY KEY CLUSTERED
([PlotID] ASC , [URI] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[SamplingPlotImage_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [SamplingPlotImage_RowGUID] ([PlotID], [URI]) 
SELECT [PlotID], [URI]
FROM [SamplingPlotImage] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [SamplingPlotImage] AS T  INNER JOIN [SamplingPlotImage_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[URI] = R.[URI]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [SamplingPlotImage_log] AS T INNER JOIN [SamplingPlotImage_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[URI] = R.[URI]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[SamplingPlotImage_RowGUID]
GO



-- Create update trigger

-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgUpdSamplingPlotImage] ON [dbo].[SamplingPlotImage] 
--FOR UPDATE AS
--INSERT INTO SamplingPlotImage_Log (PlotID, URI, ResourceURI, ImageType, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
--FROM DELETED
--Update SamplingPlotImage
--set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
--FROM SamplingPlotImage, deleted 
--where 1 = 1 
--AND SamplingPlotImage.PlotID = deleted.PlotID
--AND SamplingPlotImage.URI = deleted.URI
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgUpdSamplingPlotImage] ON [dbo].[SamplingPlotImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotImage_Log (PlotID, URI, ResourceURI, ImageType, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotImage, deleted 
where 1 = 1 
AND SamplingPlotImage.PlotID = deleted.PlotID
AND SamplingPlotImage.URI = deleted.URI

GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingPlotImage] ON [dbo].[SamplingPlotImage] 
--FOR DELETE AS 
--INSERT INTO SamplingPlotImage_Log (PlotID, URI, ResourceURI, ImageType, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgDelSamplingPlotImage] ON [dbo].[SamplingPlotImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotImage_Log (PlotID, URI, ResourceURI, ImageType, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.URI, deleted.ResourceURI, deleted.ImageType, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


GO

--######################################################################################################
--  SamplingPlotLocalisation  ##########################################################################
--######################################################################################################

-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotLocalisation' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlotLocalisation] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotLocalisation', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotLocalisation', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotLocalisation', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotLocalisation', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotLocalisation' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlotLocalisation] ADD CONSTRAINT DF_SamplingPlotLocalisation_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [SamplingPlotLocalisation] ADD CONSTRAINT DF_SamplingPlotLocalisation_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [SamplingPlotLocalisation] ADD CONSTRAINT DF_SamplingPlotLocalisation_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [SamplingPlotLocalisation] ADD CONSTRAINT DF_SamplingPlotLocalisation_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdSamplingPlotLocalisation]')) DROP TRIGGER [dbo].[trgUpdSamplingPlotLocalisation]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelSamplingPlotLocalisation]')) DROP TRIGGER [dbo].[trgDelSamplingPlotLocalisation]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [SamplingPlotLocalisation] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [SamplingPlotLocalisation] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [SamplingPlotLocalisation] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotLocalisation' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlotLocalisation] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [SamplingPlotLocalisation] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotLocalisation' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlotLocalisation] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotLocalisation_log') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlotLocalisation_log] 
([PlotID] [int] NULL,
 [LocalisationSystemID] [int] NULL,
 [Location1] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [Location2] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [LocationAccuracy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LocationNotes] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [DeterminationDate] [smalldatetime] NULL,
 [DistanceToLocation] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [DirectionToLocation] [varchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [ResponsibleName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ResponsibleAgentURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [Geography] [geography] NULL,
 [AverageAltitudeCache] [float] NULL,
 [AverageLatitudeCache] [float] NULL,
 [AverageLongitudeCache] [float] NULL,
 [LogCreatedWhen] [datetime] NULL,
 [LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [datetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [RowGUID] [uniqueidentifier] NULL,
  [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_SamplingPlotLocalisation_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotLocalisation_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlotLocalisation_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotLocalisation_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlotLocalisation_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'SamplingPlotLocalisation_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlotLocalisation_RowGUID]([PlotID] [int],
 [LocalisationSystemID] [int],
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SamplingPlotLocalisation_RowGUID] PRIMARY KEY CLUSTERED
([PlotID] ASC , [LocalisationSystemID] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[SamplingPlotLocalisation_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [SamplingPlotLocalisation_RowGUID] ([PlotID], [LocalisationSystemID]) 
SELECT [PlotID], [LocalisationSystemID]
FROM [SamplingPlotLocalisation] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [SamplingPlotLocalisation] AS T  INNER JOIN [SamplingPlotLocalisation_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[LocalisationSystemID] = R.[LocalisationSystemID]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [SamplingPlotLocalisation_log] AS T INNER JOIN [SamplingPlotLocalisation_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[LocalisationSystemID] = R.[LocalisationSystemID]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[SamplingPlotLocalisation_RowGUID]
GO



-- Create update trigger

-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgUpdSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
--FOR UPDATE AS
--INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
--FROM DELETED
--Update SamplingPlotLocalisation
--set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
--FROM SamplingPlotLocalisation, deleted 
--where 1 = 1 
--AND SamplingPlotLocalisation.LocalisationSystemID = deleted.LocalisationSystemID
--AND SamplingPlotLocalisation.PlotID = deleted.PlotID
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgUpdSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotLocalisation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotLocalisation, deleted 
where 1 = 1 
AND SamplingPlotLocalisation.LocalisationSystemID = deleted.LocalisationSystemID
AND SamplingPlotLocalisation.PlotID = deleted.PlotID

GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
--FOR DELETE AS 
--INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgDelSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


GO

--##################################################################################################
--  SamplingPlotProperty  ##########################################################################
--##################################################################################################

-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotProperty' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlotProperty] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotProperty', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotProperty', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotProperty', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotProperty', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotProperty' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlotProperty] ADD CONSTRAINT DF_SamplingPlotProperty_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [SamplingPlotProperty] ADD CONSTRAINT DF_SamplingPlotProperty_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [SamplingPlotProperty] ADD CONSTRAINT DF_SamplingPlotProperty_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [SamplingPlotProperty] ADD CONSTRAINT DF_SamplingPlotProperty_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdSamplingPlotProperty]')) DROP TRIGGER [dbo].[trgUpdSamplingPlotProperty]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelSamplingPlotProperty]')) DROP TRIGGER [dbo].[trgDelSamplingPlotProperty]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [SamplingPlotProperty] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [SamplingPlotProperty] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [SamplingPlotProperty] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotProperty' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlotProperty] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [SamplingPlotProperty] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotProperty' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlotProperty] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotProperty_log') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlotProperty_log] 
([PlotID] [int] NULL,
 [PropertyID] [int] NULL,
 [PropertyGeometry] [geometry] NULL,
 [LogCreatedWhen] [datetime] NULL,
 [LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [datetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [PropertyURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [DisplayText] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [PropertyHierarchyCache] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [PropertyValue] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ResponsibleName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ResponsibleAgentURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [Notes] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [AverageValueCache] [float] NULL,
 [RowGUID] [uniqueidentifier] NULL,
  [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlotProperty_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_SamplingPlotProperty_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlotProperty_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_SamplingPlotProperty_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotProperty_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlotProperty_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotProperty_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlotProperty_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'SamplingPlotProperty_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlotProperty_RowGUID]([PlotID] [int],
 [PropertyID] [int],
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SamplingPlotProperty_RowGUID] PRIMARY KEY CLUSTERED
([PlotID] ASC , [PropertyID] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[SamplingPlotProperty_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [SamplingPlotProperty_RowGUID] ([PlotID], [PropertyID]) 
SELECT [PlotID], [PropertyID]
FROM [SamplingPlotProperty] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [SamplingPlotProperty] AS T  INNER JOIN [SamplingPlotProperty_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[PropertyID] = R.[PropertyID]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [SamplingPlotProperty_log] AS T INNER JOIN [SamplingPlotProperty_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[PropertyID] = R.[PropertyID]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[SamplingPlotProperty_RowGUID]
GO



-- Create update trigger

CREATE TRIGGER [trgUpdSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotProperty_Log (PlotID, PropertyID, PropertyGeometry, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, PropertyURI, DisplayText, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.PropertyID, deleted.PropertyGeometry, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.PropertyURI, deleted.DisplayText, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotProperty
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotProperty, deleted 
where 1 = 1 
AND SamplingPlotProperty.PlotID = deleted.PlotID
AND SamplingPlotProperty.PropertyID = deleted.PropertyID

GO



-- Create delete trigger

CREATE TRIGGER [trgDelSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotProperty_Log (PlotID, PropertyID, PropertyGeometry, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, PropertyURI, DisplayText, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.PropertyID, deleted.PropertyGeometry, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.PropertyURI, deleted.DisplayText, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID,  'D'
FROM DELETED


GO

--##################################################################################################
--  SamplingPlotResource  ##########################################################################
--##################################################################################################

-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotResource' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlotResource] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotResource', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotResource', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotResource', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingPlotResource', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotResource' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlotResource] ADD CONSTRAINT DF_SamplingPlotResource_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [SamplingPlotResource] ADD CONSTRAINT DF_SamplingPlotResource_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [SamplingPlotResource] ADD CONSTRAINT DF_SamplingPlotResource_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [SamplingPlotResource] ADD CONSTRAINT DF_SamplingPlotResource_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdSamplingPlotResource]')) DROP TRIGGER [dbo].[trgUpdSamplingPlotResource]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelSamplingPlotResource]')) DROP TRIGGER [dbo].[trgDelSamplingPlotResource]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [SamplingPlotResource] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [SamplingPlotResource] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [SamplingPlotResource] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotResource' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlotResource] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [SamplingPlotResource] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotResource' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingPlotResource] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotResource_log') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlotResource_log] 
([PlotID] [int] NULL,
 [ResourceURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ResponsibleName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ResponsibleAgentURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [Notes] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [LogCreatedWhen] [datetime] NULL,
 [LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [datetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [RowGUID] [uniqueidentifier] NULL,
  [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlotResource_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_SamplingPlotResource_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingPlotResource_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_SamplingPlotResource_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotResource_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingPlotResource_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotResource_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingPlotResource_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'SamplingPlotResource_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[SamplingPlotResource_RowGUID]([PlotID] [int],
 [ResourceURI] [varchar](255) NOT NULL ,
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SamplingPlotResource_RowGUID] PRIMARY KEY CLUSTERED
([PlotID] ASC , [ResourceURI] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[SamplingPlotResource_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [SamplingPlotResource_RowGUID] ([PlotID], [ResourceURI]) 
SELECT [PlotID], [ResourceURI]
FROM [SamplingPlotResource] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [SamplingPlotResource] AS T  INNER JOIN [SamplingPlotResource_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[ResourceURI] = R.[ResourceURI]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [SamplingPlotResource_log] AS T INNER JOIN [SamplingPlotResource_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[ResourceURI] = R.[ResourceURI]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[SamplingPlotResource_RowGUID]
GO



-- Create update trigger

-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgUpdSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
--FOR UPDATE AS
--INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
--FROM DELETED
--Update SamplingPlotResource
--set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
--FROM SamplingPlotResource, deleted 
--where 1 = 1 
--AND SamplingPlotResource.PlotID = deleted.PlotID
--AND SamplingPlotResource.ResourceURI = deleted.ResourceURI
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgUpdSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotResource
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotResource, deleted 
where 1 = 1 
AND SamplingPlotResource.PlotID = deleted.PlotID
AND SamplingPlotResource.ResourceURI = deleted.ResourceURI

GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
--FOR DELETE AS 
--INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgDelSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


GO

--#############################################################################################
--  SamplingProject  ##########################################################################
--#############################################################################################

-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingProject' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingProject] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingProject', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingProject', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingProject', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SamplingProject', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingProject' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingProject] ADD CONSTRAINT DF_SamplingProject_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [SamplingProject] ADD CONSTRAINT DF_SamplingProject_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [SamplingProject] ADD CONSTRAINT DF_SamplingProject_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [SamplingProject] ADD CONSTRAINT DF_SamplingProject_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdSamplingProject]')) DROP TRIGGER [dbo].[trgUpdSamplingProject]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelSamplingProject]')) DROP TRIGGER [dbo].[trgDelSamplingProject]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [SamplingProject] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [SamplingProject] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [SamplingProject] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingProject' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingProject] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [SamplingProject] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingProject' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [SamplingProject] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingProject_log') = 0
BEGIN
CREATE TABLE [dbo].[SamplingProject_log] 
([PlotID] [int] NULL,
 [ProjectID] [int] NULL,
 [LogCreatedWhen] [datetime] NULL,
 [LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [datetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [RowGUID] [uniqueidentifier] NULL,
  [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingProject_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_SamplingProject_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_SamplingProject_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_SamplingProject_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingProject_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [SamplingProject_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingProject_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [SamplingProject_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'SamplingProject_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[SamplingProject_RowGUID]([PlotID] [int],
 [ProjectID] [int],
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SamplingProject_RowGUID] PRIMARY KEY CLUSTERED
([PlotID] ASC , [ProjectID] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[SamplingProject_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [SamplingProject_RowGUID] ([PlotID], [ProjectID]) 
SELECT [PlotID], [ProjectID]
FROM [SamplingProject] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [SamplingProject] AS T  INNER JOIN [SamplingProject_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[ProjectID] = R.[ProjectID]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [SamplingProject_log] AS T INNER JOIN [SamplingProject_RowGUID] AS R ON 
T.[PlotID] = R.[PlotID] AND T.[ProjectID] = R.[ProjectID]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[SamplingProject_RowGUID]
GO



-- Create update trigger

-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgUpdSamplingProject] ON [dbo].[SamplingProject] 
--FOR UPDATE AS
--INSERT INTO SamplingProject_Log (PlotID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
--FROM DELETED
--Update SamplingProject
--set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
--FROM SamplingProject, deleted 
--where 1 = 1 
--AND SamplingProject.PlotID = deleted.PlotID
--AND SamplingProject.ProjectID = deleted.ProjectID
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgUpdSamplingProject] ON [dbo].[SamplingProject] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingProject_Log (PlotID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingProject
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingProject, deleted 
where 1 = 1 
AND SamplingProject.PlotID = deleted.PlotID
AND SamplingProject.ProjectID = deleted.ProjectID

GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingProject] ON [dbo].[SamplingProject] 
--FOR DELETE AS 
--INSERT INTO SamplingProject_Log (PlotID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [trgDelSamplingProject] ON [dbo].[SamplingProject] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingProject_Log (PlotID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


GO

--#######################################################################################
--  UserProxy  ##########################################################################
--#######################################################################################

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'UserProxy' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [UserProxy] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [UserProxy] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [UserProxy] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'UserProxy_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[UserProxy_RowGUID]([LoginName] [nvarchar](50) NOT NULL ,
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_UserProxy_RowGUID] PRIMARY KEY CLUSTERED
([LoginName] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[UserProxy_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temorary table for RowGUID

INSERT INTO [UserProxy_RowGUID] ([LoginName]) 
SELECT [LoginName]
FROM [UserProxy] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [UserProxy] AS T  INNER JOIN [UserProxy_RowGUID] AS R ON 
T.[LoginName] = R.[LoginName]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[UserProxy_RowGUID]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.06.03' 
END

GO
ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.18'
END


GO

select [dbo].[Version] ()  



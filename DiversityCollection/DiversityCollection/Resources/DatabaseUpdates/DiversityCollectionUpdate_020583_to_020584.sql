declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.83'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  ParameterValue_Enum Replication   ###########################################################################
--#####################################################################################################################


-- Add log columns if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'ParameterValue_Enum' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [ParameterValue_Enum] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

   DECLARE @v sql_variant; 
   SET @v = N'Name of user who first entered (typed or imported) the data.';
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ParameterValue_Enum', N'COLUMN', N'LogCreatedBy';
   SET @v = N'Date and time when the data were first entered (typed or imported) into this database.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ParameterValue_Enum', N'COLUMN', N'LogCreatedWhen';
   SET @v = N'Name of user who last updated the data.';
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ParameterValue_Enum', N'COLUMN', N'LogUpdatedBy';
   SET @v = N'Date and time when the data were last updated.'; 
   EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ParameterValue_Enum', N'COLUMN', N'LogUpdatedWhen';

END
GO

-- Add log columns defaults if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ParameterValue_Enum' and C.COLUMN_NAME like 'Log%tedWhen' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [ParameterValue_Enum] ADD CONSTRAINT DF_ParameterValue_Enum_LogCreatedBy DEFAULT (user_name()) FOR LogCreatedBy; 
   ALTER TABLE [ParameterValue_Enum] ADD CONSTRAINT DF_ParameterValue_Enum_LogCreatedWhen DEFAULT (getdate()) FOR LogCreatedWhen; 
   ALTER TABLE [ParameterValue_Enum] ADD CONSTRAINT DF_ParameterValue_Enum_LogUpdatedBy DEFAULT (user_name()) FOR LogUpdatedBy; 
   ALTER TABLE [ParameterValue_Enum] ADD CONSTRAINT DF_ParameterValue_Enum_LogUpdatedWhen DEFAULT (getdate()) FOR LogUpdatedWhen; 
END
GO

-- Deleting the update trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgUpdParameterValue_Enum]')) DROP TRIGGER [dbo].[trgUpdParameterValue_Enum]
GO

-- Deleting the delete trigger

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgDelParameterValue_Enum]')) DROP TRIGGER [dbo].[trgDelParameterValue_Enum]
GO

-- Fill log columns if empty

IF (select COUNT(*) from [ParameterValue_Enum] T where T.logUpdatedWhen IS NULL) > 0
BEGIN
   UPDATE T SET LogCreatedWhen = '1900-01-01' FROM [ParameterValue_Enum] T WHERE T.LogCreatedWhen  IS NULL;
   UPDATE T SET LogUpdatedWhen = '1900-01-01' FROM [ParameterValue_Enum] T WHERE T.LogUpdatedWhen  IS NULL;
END
GO

-- Add RowGUID column if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'ParameterValue_Enum' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [ParameterValue_Enum] ADD [RowGUID] [uniqueidentifier] NULL;
   ALTER TABLE [ParameterValue_Enum] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add default for RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ParameterValue_Enum' and C.COLUMN_NAME = 'RowGUID' and C.COLUMN_DEFAULT is null) > 0
BEGIN
   ALTER TABLE [ParameterValue_Enum] ADD DEFAULT (newsequentialid()) FOR [RowGUID];
END
GO

-- Add log table if missing

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'ParameterValue_Enum_log') = 0
BEGIN
CREATE TABLE [dbo].[ParameterValue_Enum_log] 
([MethodID] [int] NULL,
 [ParameterID] [int] NULL,
 [Value] [nvarchar] (400) COLLATE Latin1_General_CI_AS NULL,
 [DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [Description] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [URI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [RowGUID] [uniqueidentifier],
[LogCreatedWhen] [datetime],
[LogCreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS,
[LogUpdatedWhen] [datetime],
[LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS,
 [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ParameterValue_Enum_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_ParameterValue_Enum_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ParameterValue_Enum_Log_LogUser]  DEFAULT (user_name()), 
 [LogVersion] [int] NULL, 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_ParameterValue_Enum_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
END;

-- Add log columns to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'ParameterValue_Enum_log' AND T.COLUMN_NAME LIKE 'Log%tedWhen') = 0
BEGIN
   ALTER TABLE [ParameterValue_Enum_log] ADD
      LogCreatedBy nvarchar(50) NULL,
      LogCreatedWhen datetime NULL,
      LogUpdatedBy nvarchar(50) NULL,
      LogUpdatedWhen datetime NULL

END
GO

-- Add RowGUID column to log table if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'ParameterValue_Enum_log' AND T.COLUMN_NAME = 'RowGUID') = 0
BEGIN
   ALTER TABLE [ParameterValue_Enum_log] ADD [RowGUID] [uniqueidentifier] NULL;
END
GO

-- Create temporary table containing RowGUID if missing

IF (select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ParameterValue_Enum_RowGUID') = 0
BEGIN
CREATE TABLE [dbo].[ParameterValue_Enum_RowGUID]([MethodID] [int],
 [ParameterID] [int],
 [Value] [nvarchar](400) NOT NULL ,
  [RowGUID] [uniqueidentifier] NULL,
  [LogCreatedWhen] [datetime] NULL,
  [LogCreatedBy] [nvarchar](50) NULL,
  [LogUpdatedWhen] [datetime] NULL,
  [LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ParameterValue_Enum_RowGUID] PRIMARY KEY CLUSTERED
([MethodID] ASC , [ParameterID] ASC , [Value] ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]; 
ALTER TABLE [dbo].[ParameterValue_Enum_RowGUID] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]; 
END


-- Transfer data to temporary table for RowGUID

INSERT INTO [ParameterValue_Enum_RowGUID] ([MethodID], [ParameterID], [Value]) 
SELECT [MethodID], [ParameterID], [Value]
FROM [ParameterValue_Enum] 
WHERE [RowGUID] IS NULL;



-- Write RowGUID in data

UPDATE T SET [RowGUID] = R.[RowGUID] 
FROM [ParameterValue_Enum] AS T  INNER JOIN [ParameterValue_Enum_RowGUID] AS R ON 
T.[MethodID] = R.[MethodID] AND T.[ParameterID] = R.[ParameterID] AND T.[Value] = R.[Value]; 
UPDATE T SET [RowGUID] = R.[RowGUID]
FROM [ParameterValue_Enum_log] AS T INNER JOIN [ParameterValue_Enum_RowGUID] AS R ON 
T.[MethodID] = R.[MethodID] AND T.[ParameterID] = R.[ParameterID] AND T.[Value] = R.[Value]; 


-- Deleting temorary table for RowGUID

DROP TABLE [dbo].[ParameterValue_Enum_RowGUID]
GO



-- Create update trigger

CREATE TRIGGER [trgUpdParameterValue_Enum] ON [dbo].[ParameterValue_Enum] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/13/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ParameterValue_Enum_Log (MethodID, ParameterID, Value, DisplayText, Description, URI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.DisplayText, deleted.Description, deleted.URI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update ParameterValue_Enum
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME() 
FROM ParameterValue_Enum, deleted 
where 1 = 1 
AND ParameterValue_Enum.MethodID = deleted.MethodID
AND ParameterValue_Enum.ParameterID = deleted.ParameterID
AND ParameterValue_Enum.Value = deleted.Value

GO



-- Create delete trigger

CREATE TRIGGER [trgDelParameterValue_Enum] ON [dbo].[ParameterValue_Enum] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/13/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ParameterValue_Enum_Log (MethodID, ParameterID, Value, DisplayText, Description, URI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.DisplayText, deleted.Description, deleted.URI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED


GO

--#####################################################################################################################
--######  ProjectProxy - CreateArchive   ##############################################################################
--#####################################################################################################################

ALTER TABLE ProjectProxy ADD CreateArchive bit NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If an archive e.g. by a task schedule should be created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'CreateArchive'
GO

--#####################################################################################################################
--######  ProjectProxy - ArchiveProtocol   ############################################################################
--#####################################################################################################################

ALTER TABLE ProjectProxy ADD [ArchiveProtocol] [nvarchar](max) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the last archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ArchiveProtocol'
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.84'
END

GO


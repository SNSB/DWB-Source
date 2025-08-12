
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.68'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Removing experimental tables     ###########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionSpecimenProcessingMethodParameter_log') > 0
begin
	drop table CollectionSpecimenProcessingMethodParameter_log
end

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionSpecimenProcessingMethodParameter') > 0
begin
	drop table CollectionSpecimenProcessingMethodParameter
end


if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionSpecimenProcessingMethod_log') > 0
begin
	drop table CollectionSpecimenProcessingMethod_log
end


if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionSpecimenProcessingMethod') > 0
begin
	drop table CollectionSpecimenProcessingMethod
end

--#####################################################################################################################
--######   [CollectionSpecimenProcessing]   ###########################################################################
--#####################################################################################################################

-- Removing the keys

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessing_CollectionSpecimen]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessing]'))
ALTER TABLE [dbo].[CollectionSpecimenProcessing] DROP CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimen]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessing_CollectionSpecimenPart]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessing]'))
ALTER TABLE [dbo].[CollectionSpecimenProcessing] DROP CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessing]') AND name = N'PK_CollectionSpecimenProcessing')
ALTER TABLE [dbo].[CollectionSpecimenProcessing] DROP CONSTRAINT [PK_CollectionSpecimenProcessing]
GO

-- Adding the unique ID

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionSpecimenProcessing' and C.COLUMN_NAME = 'SpecimenProcessingID') = 0
begin
	ALTER TABLE [dbo].[CollectionSpecimenProcessing] ADD [SpecimenProcessingID] [int] IDENTITY(1,1) NOT NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID of the processing of a specimen or part of a specimen, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessing', @level2type=N'COLUMN',@level2name=N'SpecimenProcessingID'
end


-- Adding the keys

ALTER TABLE [dbo].[CollectionSpecimenProcessing] ADD  CONSTRAINT [PK_CollectionSpecimenProcessing] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenProcessingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessing]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimen] FOREIGN KEY([CollectionSpecimenID])
REFERENCES [dbo].[CollectionSpecimen] ([CollectionSpecimenID])
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessing] CHECK CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimen]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessing]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessing] CHECK CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart]
GO


-- log table

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionSpecimenProcessing_log' and C.COLUMN_NAME = 'SpecimenProcessingID') = 0
begin
	ALTER TABLE [dbo].[CollectionSpecimenProcessing_log] ADD [SpecimenProcessingID] [int] NULL
end

GO


/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenProcessing]    Script Date: 10/22/2013 13:23:57 ******/

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenProcessing] ON [dbo].[CollectionSpecimenProcessing] 
FOR DELETE AS 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
set @i = (select count(*) from deleted) 
if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenProcessing]    Script Date: 10/22/2013 13:29:47 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenProcessing] ON [dbo].[CollectionSpecimenProcessing] 
FOR UPDATE AS
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
set @i = (select count(*) from deleted) 
if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
Update CollectionSpecimenProcessing
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionSpecimenProcessing, deleted 
where 1 = 1 
AND CollectionSpecimenProcessing.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenProcessing.ProcessingDate = deleted.ProcessingDate

GO

--#####################################################################################################################
--######   [MethodForProcessing]   ####################################################################################
--#####################################################################################################################

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MethodForProcessing]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MethodForProcessing](
	[ProcessingID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_MethodForProcessing] PRIMARY KEY CLUSTERED 
(
	[ProcessingID] ASC,
	[MethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
--GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the processing (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'ProcessingID'
--GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Method (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'MethodID'
--GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
--GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
--GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
--GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
--GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Methods available for a processing' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing'
--GO

ALTER TABLE [dbo].[MethodForProcessing]  WITH CHECK ADD  CONSTRAINT [FK_MethodForProcessing_Method] FOREIGN KEY([MethodID])
REFERENCES [dbo].[Method] ([MethodID])
--GO

ALTER TABLE [dbo].[MethodForProcessing] CHECK CONSTRAINT [FK_MethodForProcessing_Method]
--GO

ALTER TABLE [dbo].[MethodForProcessing]  WITH CHECK ADD  CONSTRAINT [FK_MethodForProcessing_Processing] FOREIGN KEY([ProcessingID])
REFERENCES [dbo].[Processing] ([ProcessingID])
--GO

ALTER TABLE [dbo].[MethodForProcessing] CHECK CONSTRAINT [FK_MethodForProcessing_Processing]
--GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
--GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
--GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
--GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
--GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
--GO

END

GRANT SELECT ON [MethodForProcessing] TO [User]
GO

GRANT UPDATE ON [MethodForProcessing] TO [Administrator]
GO

GRANT INSERT ON [MethodForProcessing] TO [Administrator]
GO

GRANT DELETE ON [MethodForProcessing] TO [Administrator]
GO


--#####################################################################################################################
--######   CollectionSpecimenProcessingMethod   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenProcessingMethod](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenProcessingID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[ProcessingID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenProcessingMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenProcessingID] ASC,
	[MethodID] ASC,
	[ProcessingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to ID of CollectionSpecimen (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of the specimen processing (= foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'SpecimenProcessingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the method, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the processing. Refers to ProcessingID in table Processing (foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'ProcessingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The methods used for a processing of a specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod'
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessingMethod_CollectionSpecimenProcessing] FOREIGN KEY([CollectionSpecimenID], [SpecimenProcessingID])
REFERENCES [dbo].[CollectionSpecimenProcessing] ([CollectionSpecimenID], [SpecimenProcessingID])
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] CHECK CONSTRAINT [FK_CollectionSpecimenProcessingMethod_CollectionSpecimenProcessing]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessingMethod_MethodForProcessing] FOREIGN KEY([ProcessingID], [MethodID])
REFERENCES [dbo].[MethodForProcessing] ([ProcessingID], [MethodID])
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] CHECK CONSTRAINT [FK_CollectionSpecimenProcessingMethod_MethodForProcessing]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_ProcessingID]  DEFAULT ((1)) FOR [ProcessingID]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO



GRANT SELECT ON CollectionSpecimenProcessingMethod TO [User]
GO

GRANT UPDATE ON CollectionSpecimenProcessingMethod TO [Editor]
GO

GRANT INSERT ON CollectionSpecimenProcessingMethod TO [Editor]
GO

GRANT DELETE ON CollectionSpecimenProcessingMethod TO [Editor]
GO


--#####################################################################################################################
--######   CollectionSpecimenProcessingMethod_log   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenProcessingMethod_log](
	[CollectionSpecimenID] [int] NULL,
	[SpecimenProcessingID] [int] NULL,
	[MethodID] [int] NULL,
	[ProcessingID] [int] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenProcessingMethod_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod_log] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod_log] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod_log] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethod_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON CollectionSpecimenProcessingMethod_log TO [Editor]
GO

GRANT INSERT ON CollectionSpecimenProcessingMethod_log TO [Editor]
GO

GRANT DELETE ON CollectionSpecimenProcessingMethod_log TO [Administrator]
GO



/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenProcessingMethod]    Script Date: 03/05/2014 12:19:48 ******/

CREATE TRIGGER [dbo].[trgDelCollectionSpecimenProcessingMethod] ON [dbo].[CollectionSpecimenProcessingMethod] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 22.10.2013  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.MethodID, deleted.ProcessingID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.MethodID, deleted.ProcessingID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenProcessingMethod]    Script Date: 03/05/2014 12:20:13 ******/

CREATE TRIGGER [dbo].[trgUpdCollectionSpecimenProcessingMethod] ON [dbo].[CollectionSpecimenProcessingMethod] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 22.10.2013  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.MethodID, deleted.ProcessingID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.MethodID, deleted.ProcessingID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenProcessingMethod
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionSpecimenProcessingMethod, deleted 
where 1 = 1 
AND CollectionSpecimenProcessingMethod.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenProcessingMethod.MethodID = deleted.MethodID
AND CollectionSpecimenProcessingMethod.ProcessingID = deleted.ProcessingID
AND CollectionSpecimenProcessingMethod.SpecimenProcessingID = deleted.SpecimenProcessingID
GO




--#####################################################################################################################
--######   CollectionSpecimenProcessingMethodParameter   ##############################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenProcessingMethodParameter](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenProcessingID] [int] NOT NULL,
	[ProcessingID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_CollectionSpecimenProcessingMethodParameter] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenProcessingID] ASC,
	[ProcessingID] ASC,
	[MethodID] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to ID of CollectionSpecimen (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to ID of CollectionSpecimenProcessing (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'SpecimenProcessingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the processing. Refers to ProcessingID in table Processing  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'ProcessingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the method  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the parameter. Referes to table Parameter  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'ParameterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of the parameter if different of the default value as documented in the table Parameter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The parameter values of a method used for the processing of a specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter'
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_CollectionSpecimenProcessingMethod] FOREIGN KEY([CollectionSpecimenID], [SpecimenProcessingID], [MethodID], [ProcessingID])
REFERENCES [dbo].[CollectionSpecimenProcessingMethod] ([CollectionSpecimenID], [SpecimenProcessingID], [MethodID], [ProcessingID])
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] CHECK CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_CollectionSpecimenProcessingMethod]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_Parameter] FOREIGN KEY([ParameterID], [MethodID])
REFERENCES [dbo].[Parameter] ([ParameterID], [MethodID])
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] CHECK CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_Parameter]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethodParameter_ProcessingID]  DEFAULT ((1)) FOR [ProcessingID]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethodParameter_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethodParameter_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethodParameter_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethodParameter_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO



GRANT SELECT ON CollectionSpecimenProcessingMethodParameter TO [User]
GO

GRANT UPDATE ON CollectionSpecimenProcessingMethodParameter TO [Editor]
GO

GRANT INSERT ON CollectionSpecimenProcessingMethodParameter TO [Editor]
GO

GRANT DELETE ON CollectionSpecimenProcessingMethodParameter TO [Editor]
GO



--#####################################################################################################################
--######   CollectionSpecimenProcessingMethodParameter_log   ##########################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionSpecimenProcessingMethodParameter_log](
	[CollectionSpecimenID] [int] NULL,
	[SpecimenProcessingID] [int] NULL,
	[ProcessingID] [int] NULL,
	[MethodID] [int] NULL,
	[ParameterID] [int] NULL,
	[Value] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenProcessingMethodParameter_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter_log] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethodParameter_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter_log] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethodParameter_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter_log] ADD  CONSTRAINT [DF_CollectionSpecimenProcessingMethodParameter_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO



GRANT SELECT ON CollectionSpecimenProcessingMethod_log TO [Editor]
GO

GRANT INSERT ON CollectionSpecimenProcessingMethod_log TO [Editor]
GO

GRANT DELETE ON CollectionSpecimenProcessingMethod_log TO [Administrator]
GO


/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenProcessingMethodParameter]    Script Date: 03/05/2014 12:31:48 ******/

CREATE TRIGGER [dbo].[trgDelCollectionSpecimenProcessingMethodParameter] ON [dbo].[CollectionSpecimenProcessingMethodParameter] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 22.10.2013  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenProcessingMethodParameter]    Script Date: 03/05/2014 12:32:11 ******/

CREATE TRIGGER [dbo].[trgUpdCollectionSpecimenProcessingMethodParameter] ON [dbo].[CollectionSpecimenProcessingMethodParameter] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 22.10.2013  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenProcessingMethodParameter
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionSpecimenProcessingMethodParameter, deleted 
where 1 = 1 
AND CollectionSpecimenProcessingMethodParameter.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenProcessingMethodParameter.MethodID = deleted.MethodID
AND CollectionSpecimenProcessingMethodParameter.ParameterID = deleted.ParameterID
AND CollectionSpecimenProcessingMethodParameter.ProcessingID = deleted.ProcessingID
AND CollectionSpecimenProcessingMethodParameter.SpecimenProcessingID = deleted.SpecimenProcessingID
GO


--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.00' 
END

GO





--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.69'
END

GO



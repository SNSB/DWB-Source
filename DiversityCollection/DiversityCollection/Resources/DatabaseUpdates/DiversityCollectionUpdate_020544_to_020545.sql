
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.44'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [MethodForAnalysis]   ######################################################################################
--#####################################################################################################################

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MethodForAnalysis]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MethodForAnalysis](
	[AnalysisID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_MethodForAnalysis] PRIMARY KEY CLUSTERED 
(
	[AnalysisID] ASC,
	[MethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Analysis (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'AnalysisID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Method (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'MethodID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Methods available for a Analysis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis'

ALTER TABLE [dbo].[MethodForAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_MethodForAnalysis_Analysis] FOREIGN KEY([AnalysisID])
REFERENCES [dbo].[Analysis] ([AnalysisID])

ALTER TABLE [dbo].[MethodForAnalysis] CHECK CONSTRAINT [FK_MethodForAnalysis_Analysis]

ALTER TABLE [dbo].[MethodForAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_MethodForAnalysis_Method] FOREIGN KEY([MethodID])
REFERENCES [dbo].[Method] ([MethodID])

ALTER TABLE [dbo].[MethodForAnalysis] CHECK CONSTRAINT [FK_MethodForAnalysis_Method]

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
END



GRANT SELECT ON MethodForAnalysis TO [User]
GO

GRANT UPDATE ON [MethodForProcessing] TO [Administrator]
GO

GRANT INSERT ON [MethodForProcessing] TO [Administrator]
GO

GRANT DELETE ON [MethodForProcessing] TO [Administrator]
GO



--#####################################################################################################################
--######   [IdentificationUnitAnalysisMethod]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[IdentificationUnitAnalysisMethod](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[AnalysisID] [int] NOT NULL,
	[AnalysisNumber] [nvarchar](50) NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_IdentificationUnitAnalysisMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[MethodID] ASC,
	[AnalysisID] ASC,
	[AnalysisNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to ID of CollectionSpecimen (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of IdentficationUnit (= foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'IdentificationUnitID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the method, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the processing. Refers to AnalysisID in table Processing (foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'AnalysisID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of the analysis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'AnalysisNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The methods used for an analysis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod'
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID], [AnalysisID], [AnalysisNumber])
REFERENCES [dbo].[IdentificationUnitAnalysis] ([CollectionSpecimenID], [IdentificationUnitID], [AnalysisID], [AnalysisNumber])
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethod_MethodForAnalysis] FOREIGN KEY([AnalysisID], [MethodID])
REFERENCES [dbo].[MethodForAnalysis] ([AnalysisID], [MethodID])
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethod_MethodForAnalysis]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_AnalysisID]  DEFAULT ((1)) FOR [AnalysisID]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

GRANT SELECT ON IdentificationUnitAnalysisMethod TO [User]
GO

GRANT UPDATE ON IdentificationUnitAnalysisMethod TO [Editor]
GO

GRANT INSERT ON IdentificationUnitAnalysisMethod TO [Editor]
GO

GRANT DELETE ON IdentificationUnitAnalysisMethod TO [Editor]
GO

GRANT VIEW DEFINITION ON IdentificationUnitAnalysisMethod TO Editor
GO

--#####################################################################################################################
--######   [IdentificationUnitAnalysisMethod_log]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[IdentificationUnitAnalysisMethod_log](
	[CollectionSpecimenID] [int] NULL,
	[IdentificationUnitID] [int] NULL,
	[MethodID] [int] NULL,
	[AnalysisID] [int] NULL,
	[AnalysisNumber] [nvarchar](50) NULL,
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
 CONSTRAINT [PK_IdentificationUnitAnalysisMethod_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod_log] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod_log] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod_log] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethod_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON [IdentificationUnitAnalysisMethod_log] TO [Editor]
GO

GRANT INSERT ON [IdentificationUnitAnalysisMethod_log] TO [Editor]
GO

GRANT DELETE ON [IdentificationUnitAnalysisMethod_log] TO [Administrator]
GO



/****** Object:  Trigger [dbo].[trgDelIdentificationUnitAnalysisMethod]    Script Date: 03/05/2014 12:38:21 ******/

CREATE TRIGGER [dbo].[trgDelIdentificationUnitAnalysisMethod] ON [dbo].[IdentificationUnitAnalysisMethod] 
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
INSERT INTO IdentificationUnitAnalysisMethod_Log (CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.MethodID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethod_Log (CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.MethodID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO


/****** Object:  Trigger [dbo].[trgUpdIdentificationUnitAnalysisMethod]    Script Date: 03/05/2014 12:38:30 ******/

CREATE TRIGGER [dbo].[trgUpdIdentificationUnitAnalysisMethod] ON [dbo].[IdentificationUnitAnalysisMethod] 
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
INSERT INTO IdentificationUnitAnalysisMethod_Log (CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.MethodID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethod_Log (CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.MethodID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update IdentificationUnitAnalysisMethod
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnitAnalysisMethod, deleted 
where 1 = 1 
AND IdentificationUnitAnalysisMethod.AnalysisID = deleted.AnalysisID
AND IdentificationUnitAnalysisMethod.AnalysisNumber = deleted.AnalysisNumber
AND IdentificationUnitAnalysisMethod.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnitAnalysisMethod.IdentificationUnitID = deleted.IdentificationUnitID
AND IdentificationUnitAnalysisMethod.MethodID = deleted.MethodID
GO


--#####################################################################################################################
--######   IdentificationUnitAnalysisMethodParameter   ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[IdentificationUnitAnalysisMethodParameter](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[AnalysisID] [int] NOT NULL,
	[AnalysisNumber] [nvarchar](50) NOT NULL,
	[MethodID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_IdentificationUnitAnalysisMethodParameter] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[AnalysisID] ASC,
	[AnalysisNumber] ASC,
	[MethodID] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to ID of CollectionSpecimen (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the identification unit  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'IdentificationUnitID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the analysis. Refers to AnalysisID in table Analysis  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'AnalysisID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of the analysis  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'AnalysisNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the method  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the parameter tool. Referes to table Parameter  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'ParameterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of the parameter if different of the default value as documented in the table Parameter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The parameter values of a method used for an analysis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter'
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID], [MethodID], [AnalysisID], [AnalysisNumber])
REFERENCES [dbo].[IdentificationUnitAnalysisMethod] ([CollectionSpecimenID], [IdentificationUnitID], [MethodID], [AnalysisID], [AnalysisNumber])
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_Parameter] FOREIGN KEY([ParameterID], [MethodID])
REFERENCES [dbo].[Parameter] ([ParameterID], [MethodID])
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_Parameter]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_AnalysisID]  DEFAULT ((1)) FOR [AnalysisID]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT SELECT ON [IdentificationUnitAnalysisMethodParameter] TO [User]
GO

GRANT UPDATE ON [IdentificationUnitAnalysisMethodParameter] TO [Editor]
GO

GRANT INSERT ON [IdentificationUnitAnalysisMethodParameter] TO [Editor]
GO

GRANT DELETE ON [IdentificationUnitAnalysisMethodParameter] TO [Editor]
GO

GRANT VIEW DEFINITION ON [IdentificationUnitAnalysisMethodParameter] TO Editor
GO



--#####################################################################################################################
--######   IdentificationUnitAnalysisMethodParameter_log  ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[IdentificationUnitAnalysisMethodParameter_log](
	[CollectionSpecimenID] [int] NULL,
	[IdentificationUnitID] [int] NULL,
	[AnalysisID] [int] NULL,
	[AnalysisNumber] [nvarchar](50) NULL,
	[MethodID] [int] NULL,
	[ParameterID] [int] NULL,
	[Value] [nvarchar](max) NULL,
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
 CONSTRAINT [PK_IdentificationUnitAnalysisMethodParameter_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter_log] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter_log] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter_log] ADD  CONSTRAINT [DF_IdentificationUnitAnalysisMethodParameter_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON [IdentificationUnitAnalysisMethodParameter_log] TO [Editor]
GO

GRANT INSERT ON [IdentificationUnitAnalysisMethodParameter_log] TO [Editor]
GO

GRANT DELETE ON [IdentificationUnitAnalysisMethodParameter_log] TO [Administrator]
GO



/****** Object:  Trigger [dbo].[trgDelIdentificationUnitAnalysisMethodParameter]    Script Date: 03/05/2014 12:52:15 ******/

CREATE TRIGGER [dbo].[trgDelIdentificationUnitAnalysisMethodParameter] ON [dbo].[IdentificationUnitAnalysisMethodParameter] 
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
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, Value, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, Value, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO



/****** Object:  Trigger [dbo].[trgUpdIdentificationUnitAnalysisMethodParameter]    Script Date: 03/05/2014 12:52:23 ******/

CREATE TRIGGER [dbo].[trgUpdIdentificationUnitAnalysisMethodParameter] ON [dbo].[IdentificationUnitAnalysisMethodParameter] 
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
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, Value, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, Value, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update IdentificationUnitAnalysisMethodParameter
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnitAnalysisMethodParameter, deleted 
where 1 = 1 
AND IdentificationUnitAnalysisMethodParameter.AnalysisID = deleted.AnalysisID
AND IdentificationUnitAnalysisMethodParameter.AnalysisNumber = deleted.AnalysisNumber
AND IdentificationUnitAnalysisMethodParameter.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnitAnalysisMethodParameter.IdentificationUnitID = deleted.IdentificationUnitID
AND IdentificationUnitAnalysisMethodParameter.MethodID = deleted.MethodID
AND IdentificationUnitAnalysisMethodParameter.ParameterID = deleted.ParameterID
GO





--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.45'
END

GO



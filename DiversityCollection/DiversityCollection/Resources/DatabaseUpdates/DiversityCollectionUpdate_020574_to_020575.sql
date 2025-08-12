declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.74'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   IdentificationUnitAnalysisMethod       #####################################################################
--#####################################################################################################################

-- Removing the keys

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitAnalysisMethod]'))
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] DROP CONSTRAINT [FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitAnalysisMethod_MethodForAnalysis]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitAnalysisMethod]'))
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] DROP CONSTRAINT [FK_IdentificationUnitAnalysisMethod_MethodForAnalysis]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitAnalysisMethodParameter]'))
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] DROP CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitAnalysisMethodParameter_Parameter]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitAnalysisMethodParameter]'))
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] DROP CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_Parameter]
GO


IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IdentificationUnitAnalysisMethod]') AND name = N'PK_IdentificationUnitAnalysisMethod')
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] DROP CONSTRAINT [PK_IdentificationUnitAnalysisMethod]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IdentificationUnitAnalysisMethodParameter]') AND name = N'PK_IdentificationUnitAnalysisMethodParameter')
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] DROP CONSTRAINT [PK_IdentificationUnitAnalysisMethodParameter]
GO

-- Adding the additional key column

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'IdentificationUnitAnalysisMethod' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE IdentificationUnitAnalysisMethod ADD MethodMarker nvarchar(50) NOT NULL DEFAULT '1'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A marker for the method, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethod', @level2type=N'COLUMN',@level2name=N'MethodMarker'
end
GO

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'IdentificationUnitAnalysisMethodParameter' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE IdentificationUnitAnalysisMethodParameter ADD MethodMarker nvarchar(50) NOT NULL DEFAULT '1'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A marker for the method, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysisMethodParameter', @level2type=N'COLUMN',@level2name=N'MethodMarker'
end
GO

-- Adding the primary keys

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] ADD  CONSTRAINT [PK_IdentificationUnitAnalysisMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[MethodID] ASC,
	[AnalysisID] ASC,
	[AnalysisNumber] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] ADD  CONSTRAINT [PK_IdentificationUnitAnalysisMethodParameter] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[AnalysisID] ASC,
	[AnalysisNumber] ASC,
	[MethodID] ASC,
	[ParameterID] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-- Adding the foreign keys

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis] 
FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID], [AnalysisID], [AnalysisNumber])
REFERENCES [dbo].[IdentificationUnitAnalysis] ([CollectionSpecimenID], [IdentificationUnitID], [AnalysisID], [AnalysisNumber])
GO
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethod_MethodForAnalysis] FOREIGN KEY([AnalysisID], [MethodID])
REFERENCES [dbo].[MethodForAnalysis] ([AnalysisID], [MethodID])
GO
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethod_MethodForAnalysis]
GO


ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod] 
FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID], [MethodID], [AnalysisID], [AnalysisNumber], [MethodMarker])
REFERENCES [dbo].[IdentificationUnitAnalysisMethod] ([CollectionSpecimenID], [IdentificationUnitID], [MethodID], [AnalysisID], [AnalysisNumber], [MethodMarker])
GO
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_Parameter] FOREIGN KEY([ParameterID], [MethodID])
REFERENCES [dbo].[Parameter] ([ParameterID], [MethodID])
GO
ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_Parameter]
GO


-- Logtables

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'IdentificationUnitAnalysisMethod_log' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE IdentificationUnitAnalysisMethod_log ADD MethodMarker nvarchar(50)
end
GO

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'IdentificationUnitAnalysisMethodParameter_log' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE IdentificationUnitAnalysisMethodParameter_log ADD MethodMarker nvarchar(50) 
end
GO

--Trigger for IdentificationUnitAnalysisMethod

/****** Object:  Trigger [dbo].[trgDelIdentificationUnitAnalysisMethod]    Script Date: 16.02.2016 15:45:24 ******/

ALTER TRIGGER [dbo].[trgDelIdentificationUnitAnalysisMethod] ON [dbo].[IdentificationUnitAnalysisMethod] 
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
INSERT INTO IdentificationUnitAnalysisMethod_Log (CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.MethodID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethod_Log (CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.MethodID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO


/****** Object:  Trigger [dbo].[trgUpdIdentificationUnitAnalysisMethod]    Script Date: 16.02.2016 15:46:37 ******/

ALTER TRIGGER [dbo].[trgUpdIdentificationUnitAnalysisMethod] ON [dbo].[IdentificationUnitAnalysisMethod] 
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
INSERT INTO IdentificationUnitAnalysisMethod_Log (CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.MethodID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethod_Log (CollectionSpecimenID, IdentificationUnitID, MethodID, AnalysisID, AnalysisNumber, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.MethodID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
Update IdentificationUnitAnalysisMethod
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM IdentificationUnitAnalysisMethod, deleted 
where 1 = 1 
AND IdentificationUnitAnalysisMethod.AnalysisID = deleted.AnalysisID
AND IdentificationUnitAnalysisMethod.AnalysisNumber = deleted.AnalysisNumber
AND IdentificationUnitAnalysisMethod.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnitAnalysisMethod.IdentificationUnitID = deleted.IdentificationUnitID
AND IdentificationUnitAnalysisMethod.MethodID = deleted.MethodID
AND IdentificationUnitAnalysisMethod.MethodMarker = deleted.MethodMarker

GO


--Trigger for IdentificationUnitAnalysisMethodParameter

/****** Object:  Trigger [dbo].[trgDelIdentificationUnitAnalysisMethodParameter]    Script Date: 16.02.2016 15:49:37 ******/

ALTER TRIGGER [dbo].[trgDelIdentificationUnitAnalysisMethodParameter] ON [dbo].[IdentificationUnitAnalysisMethodParameter] 
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
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, Value, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, Value, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO


/****** Object:  Trigger [dbo].[trgUpdIdentificationUnitAnalysisMethodParameter]    Script Date: 16.02.2016 15:50:32 ******/

ALTER TRIGGER [dbo].[trgUpdIdentificationUnitAnalysisMethodParameter] ON [dbo].[IdentificationUnitAnalysisMethodParameter] 
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
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, Value, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysisMethodParameter_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, MethodID, ParameterID, Value, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
Update IdentificationUnitAnalysisMethodParameter
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM IdentificationUnitAnalysisMethodParameter, deleted 
where 1 = 1 
AND IdentificationUnitAnalysisMethodParameter.AnalysisID = deleted.AnalysisID
AND IdentificationUnitAnalysisMethodParameter.AnalysisNumber = deleted.AnalysisNumber
AND IdentificationUnitAnalysisMethodParameter.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnitAnalysisMethodParameter.IdentificationUnitID = deleted.IdentificationUnitID
AND IdentificationUnitAnalysisMethodParameter.MethodID = deleted.MethodID
AND IdentificationUnitAnalysisMethodParameter.ParameterID = deleted.ParameterID
AND IdentificationUnitAnalysisMethodParameter.MethodMarker = deleted.MethodMarker

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.75'
END

GO


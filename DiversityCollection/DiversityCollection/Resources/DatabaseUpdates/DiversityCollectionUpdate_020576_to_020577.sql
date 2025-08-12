declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.76'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   CollectionEventMethod       ################################################################################
--#####################################################################################################################

-- Removing the keys

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionEventMethod_CollectionEvent]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionEventMethod]'))
ALTER TABLE [dbo].[CollectionEventMethod] DROP CONSTRAINT [FK_CollectionEventMethod_CollectionEvent]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionEventMethod_Method]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionEventMethod]'))
ALTER TABLE [dbo].[CollectionEventMethod] DROP CONSTRAINT [FK_CollectionEventMethod_Method]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionEventParameterValue_CollectionEventMethod]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionEventParameterValue]'))
ALTER TABLE [dbo].[CollectionEventParameterValue] DROP CONSTRAINT [FK_CollectionEventParameterValue_CollectionEventMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionEventParameterValue_Parameter]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionEventParameterValue]'))
ALTER TABLE [dbo].[CollectionEventParameterValue] DROP CONSTRAINT [FK_CollectionEventParameterValue_Parameter]
GO


IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CollectionEventMethod]') AND name = N'PK_CollectionEventMethod')
ALTER TABLE [dbo].[CollectionEventMethod] DROP CONSTRAINT [PK_CollectionEventMethod]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CollectionEventParameterValue]') AND name = N'PK_CollectionEventParameterValue')
ALTER TABLE [dbo].[CollectionEventParameterValue] DROP CONSTRAINT [PK_CollectionEventParameterValue]
GO

-- Adding the additional key column

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionEventMethod' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE CollectionEventMethod ADD MethodMarker nvarchar(50) NOT NULL DEFAULT '1'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A marker for the method, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventMethod', @level2type=N'COLUMN',@level2name=N'MethodMarker'
end
GO

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionEventParameterValue' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE CollectionEventParameterValue ADD MethodMarker nvarchar(50) NOT NULL DEFAULT '1'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A marker for the method, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'MethodMarker'
end
GO

-- Adding the primary keys

ALTER TABLE [dbo].[CollectionEventMethod] ADD  CONSTRAINT [PK_CollectionEventMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[MethodID] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


ALTER TABLE [dbo].[CollectionEventParameterValue] ADD  CONSTRAINT [PK_CollectionEventParameterValue] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[MethodID] ASC,
	[ParameterID] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-- Adding the foreign keys

ALTER TABLE [dbo].[CollectionEventMethod]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventMethod_CollectionEvent] FOREIGN KEY([CollectionEventID])
REFERENCES [dbo].[CollectionEvent] ([CollectionEventID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollectionEventMethod] CHECK CONSTRAINT [FK_CollectionEventMethod_CollectionEvent]
GO

ALTER TABLE [dbo].[CollectionEventMethod]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventMethod_Method] FOREIGN KEY([MethodID])
REFERENCES [dbo].[Method] ([MethodID])
GO
ALTER TABLE [dbo].[CollectionEventMethod] CHECK CONSTRAINT [FK_CollectionEventMethod_Method]
GO


ALTER TABLE [dbo].[CollectionEventParameterValue]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventParameterValue_CollectionEventMethod] 
FOREIGN KEY([CollectionEventID], [MethodID], [MethodMarker])
REFERENCES [dbo].[CollectionEventMethod] ([CollectionEventID], [MethodID], [MethodMarker])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollectionEventParameterValue] CHECK CONSTRAINT [FK_CollectionEventParameterValue_CollectionEventMethod]
GO

ALTER TABLE [dbo].[CollectionEventParameterValue]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventParameterValue_Parameter] FOREIGN KEY([ParameterID], [MethodID])
REFERENCES [dbo].[Parameter] ([ParameterID], [MethodID])
GO
ALTER TABLE [dbo].[CollectionEventParameterValue] CHECK CONSTRAINT [FK_CollectionEventParameterValue_Parameter]
GO



-- Logtables

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionEventMethod_log' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE CollectionEventMethod_log ADD MethodMarker nvarchar(50)
end
GO

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionEventParameterValue_log' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE CollectionEventParameterValue_log ADD MethodMarker nvarchar(50) 
end
GO

--Trigger for CollectionEventMethod

/****** Object:  Trigger [dbo].[trgDelCollectionEventMethod]    Script Date: 28.12.2015 14:36:08 ******/
ALTER TRIGGER [dbo].[trgDelCollectionEventMethod] ON [dbo].[CollectionEventMethod] 
FOR DELETE AS 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
set @i = (select count(*) from deleted) 
if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM deleted)
   EXECUTE procSetVersionCollectionEvent @ID
   SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)
END 
if (not @Version is null) 
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end


GO

/****** Object:  Trigger [dbo].[trgUpdCollectionEventMethod]    Script Date: 28.12.2015 14:37:28 ******/
ALTER TRIGGER [dbo].[trgUpdCollectionEventMethod] ON [dbo].[CollectionEventMethod] 
FOR UPDATE AS
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
set @i = (select count(*) from deleted) 
if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM deleted)
   EXECUTE procSetVersionCollectionEvent @ID
   SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)
END 
if (not @Version is null) 
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionEvent.Version, 'U' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
Update CollectionEventMethod
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionEventMethod, deleted 
where 1 = 1 
AND CollectionEventMethod.CollectionEventID = deleted.CollectionEventID
AND CollectionEventMethod.MethodID = deleted.MethodID
AND CollectionEventMethod.MethodMarker = deleted.MethodMarker


GO




--Trigger for CollectionEventParameterValue

/****** Object:  Trigger [dbo].[trgDelCollectionEventParameterValue]    Script Date: 16.02.2016 16:28:45 ******/

ALTER TRIGGER [dbo].[trgDelCollectionEventParameterValue] ON [dbo].[CollectionEventParameterValue] 
FOR DELETE AS 
INSERT INTO CollectionEventParameterValue_Log (CollectionEventID, MethodID, ParameterID, Value, MethodMarker, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO


/****** Object:  Trigger [dbo].[trgUpdCollectionEventParameterValue]    Script Date: 16.02.2016 16:30:01 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionEventParameterValue] ON [dbo].[CollectionEventParameterValue] 
FOR UPDATE AS
INSERT INTO CollectionEventParameterValue_Log (CollectionEventID, MethodID, ParameterID, Value, MethodMarker, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED
Update CollectionEventParameterValue
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionEventParameterValue, deleted 
where 1 = 1 
AND CollectionEventParameterValue.CollectionEventID = deleted.CollectionEventID
AND CollectionEventParameterValue.MethodID = deleted.MethodID
AND CollectionEventParameterValue.ParameterID = deleted.ParameterID
AND CollectionEventParameterValue.MethodMarker = deleted.MethodMarker

GO





--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.77'
END

GO


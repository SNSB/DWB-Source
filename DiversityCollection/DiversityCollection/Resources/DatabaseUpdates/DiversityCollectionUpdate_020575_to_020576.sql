declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.75'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   CollectionSpecimenProcessingMethod       ###################################################################
--#####################################################################################################################

-- Removing the keys

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessingMethod_CollectionSpecimenProcessing]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessingMethod]'))
ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] DROP CONSTRAINT [FK_CollectionSpecimenProcessingMethod_CollectionSpecimenProcessing]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessingMethod_MethodForProcessing]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessingMethod]'))
ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] DROP CONSTRAINT [FK_CollectionSpecimenProcessingMethod_MethodForProcessing]
GO

--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessingMethodParameter_CollectionSpecimenProcessingMethod]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessingMethodParameter]'))
ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] DROP CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_CollectionSpecimenProcessingMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessingMethodParameter_Parameter]') 
AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessingMethodParameter]'))
ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] DROP CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_Parameter]
GO

--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessingMethod]') AND name = N'PK_CollectionSpecimenProcessingMethod')
ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] DROP CONSTRAINT [PK_CollectionSpecimenProcessingMethod]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessingMethodParameter]') AND name = N'PK_CollectionSpecimenProcessingMethodParameter')
ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] DROP CONSTRAINT [PK_CollectionSpecimenProcessingMethodParameter]
GO

--#####################################################################################################################
-- Adding the additional key column

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionSpecimenProcessingMethod' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE CollectionSpecimenProcessingMethod ADD MethodMarker nvarchar(50) NOT NULL DEFAULT '1'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A marker for the method, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethod', @level2type=N'COLUMN',@level2name=N'MethodMarker'
end
GO

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionSpecimenProcessingMethodParameter' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE CollectionSpecimenProcessingMethodParameter ADD MethodMarker nvarchar(50) NOT NULL DEFAULT '1'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A marker for the method, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessingMethodParameter', @level2type=N'COLUMN',@level2name=N'MethodMarker'
end
GO

--#####################################################################################################################
-- Adding the primary keys

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethod] ADD  CONSTRAINT [PK_CollectionSpecimenProcessingMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenProcessingID] ASC,
	[MethodID] ASC,
	[ProcessingID] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] ADD  CONSTRAINT [PK_CollectionSpecimenProcessingMethodParameter] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenProcessingID] ASC,
	[ProcessingID] ASC,
	[MethodID] ASC,
	[ParameterID] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

--#####################################################################################################################
-- Adding the foreign keys
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


--#####################################################################################################################
ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_CollectionSpecimenProcessingMethod] 
FOREIGN KEY([CollectionSpecimenID], [SpecimenProcessingID], [MethodID], [ProcessingID], [MethodMarker])
REFERENCES [dbo].[CollectionSpecimenProcessingMethod] ([CollectionSpecimenID], [SpecimenProcessingID], [MethodID], [ProcessingID], [MethodMarker])
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] CHECK CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_CollectionSpecimenProcessingMethod]
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_Parameter] FOREIGN KEY([ParameterID], [MethodID])
REFERENCES [dbo].[Parameter] ([ParameterID], [MethodID])
GO

ALTER TABLE [dbo].[CollectionSpecimenProcessingMethodParameter] CHECK CONSTRAINT [FK_CollectionSpecimenProcessingMethodParameter_Parameter]
GO



--#####################################################################################################################

-- Logtables

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionSpecimenProcessingMethod_log' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE CollectionSpecimenProcessingMethod_log ADD MethodMarker nvarchar(50)
end
GO

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionSpecimenProcessingMethodParameter_log' and C.COLUMN_NAME = 'MethodMarker') = 0
begin
	ALTER TABLE CollectionSpecimenProcessingMethodParameter_log ADD MethodMarker nvarchar(50) 
end
GO

--#####################################################################################################################
--Trigger for CollectionSpecimenProcessingMethod

/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenProcessingMethod]    Script Date: 16.02.2016 15:45:24 ******/

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenProcessingMethod] ON [dbo].[CollectionSpecimenProcessingMethod] 
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
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.MethodID, deleted.ProcessingID, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.MethodID, deleted.ProcessingID, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO

/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenProcessingMethod]    Script Date: 03/05/2014 12:20:13 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenProcessingMethod] ON [dbo].[CollectionSpecimenProcessingMethod] 
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
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.MethodID, deleted.ProcessingID, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethod_Log (CollectionSpecimenID, SpecimenProcessingID, MethodID, ProcessingID, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.MethodID, deleted.ProcessingID, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U' 
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
AND CollectionSpecimenProcessingMethod.MethodMarker = deleted.MethodMarker

GO




--Trigger for CollectionSpecimenProcessingMethodParameter

/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenProcessingMethodParameter]    Script Date: 03/05/2014 12:31:48 ******/

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenProcessingMethodParameter] ON [dbo].[CollectionSpecimenProcessingMethodParameter] 
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
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenProcessingMethodParameter]    Script Date: 03/05/2014 12:32:11 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenProcessingMethodParameter] ON [dbo].[CollectionSpecimenProcessingMethodParameter] 
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
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessingMethodParameter_Log (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, ParameterID, Value, MethodMarker, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenProcessingID, deleted.ProcessingID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
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
AND CollectionSpecimenProcessingMethodParameter.MethodMarker = deleted.MethodMarker

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.76'
END

GO


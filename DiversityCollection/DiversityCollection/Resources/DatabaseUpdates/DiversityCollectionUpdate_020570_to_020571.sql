declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.70'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Foreign key for Relation to CollectionID   #################################################################
--#####################################################################################################################

if (SELECT COUNT(*) FROM [CollectionSpecimenRelation] R WHERE R.RelatedSpecimenCollectionID NOT IN (SELECT CollectionID FROM [Collection])) > 0
begin
	UPDATE R SET R.RelatedSpecimenCollectionID = NULL
	FROM [CollectionSpecimenRelation] R
	WHERE R.RelatedSpecimenCollectionID NOT IN (SELECT CollectionID FROM [Collection])
end

if (select count(*) from INFORMATION_SCHEMA.TABLE_CONSTRAINTS T where T.CONSTRAINT_NAME = 'FK_CollectionSpecimenRelation_Collection') = 0
begin
	ALTER TABLE [dbo].[CollectionSpecimenRelation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenRelation_Collection] FOREIGN KEY([RelatedSpecimenCollectionID])
	REFERENCES [dbo].[Collection] ([CollectionID])
	ALTER TABLE [dbo].[CollectionSpecimenRelation] CHECK CONSTRAINT [FK_CollectionSpecimenRelation_Collection]
end

GO

--#####################################################################################################################
--######   CollectionEventMethod   ####################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.COLUMN_NAME = 'RowGUID' and C.TABLE_NAME = 'CollectionEventMethod') = 0
begin
	ALTER TABLE [dbo].[CollectionEventMethod] ADD RowGUID [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_CollectionEventMethod_RowGUID] DEFAULT (newsequentialid());
	ALTER TABLE [dbo].[CollectionEventMethod_log] ADD RowGUID [uniqueidentifier] ROWGUIDCOL  NOT NULL;
end
GO

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
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionEvent.Version, 'D' 
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
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionEvent.Version, 'U' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
Update CollectionEventMethod
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionEventMethod, deleted 
where 1 = 1 
AND CollectionEventMethod.CollectionEventID = deleted.CollectionEventID
AND CollectionEventMethod.MethodID = deleted.MethodID

GO

--#####################################################################################################################
--######   CollectionEventParameterValue   ############################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[CollectionEventParameterValue] ADD CONSTRAINT [DF_CollectionEventParameterValue_RowGUID] DEFAULT (newsequentialid()) FOR RowGUID;
GO


--#####################################################################################################################
--######   HierarchyCache Description  ################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A cached value fo the superior taxonomy of the last identification as derived from a taxonomic data provider' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnit', @level2type=N'COLUMN',@level2name=N'HierarchyCache'
GO


--#####################################################################################################################
--######   ParameterValue_Enum  #######################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of the parameter (part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The text as e.g. shown in user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Description of the parameter value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'URI referring to an external documentation of the parameter value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'URI'
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.71'
END

GO


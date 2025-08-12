--use DiversityCollection_Test

declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.47'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   revision   ######################################################################################
--#####################################################################################################################

if (Select COUNT(*) from [CollIdentificationCategory_Enum] C where C.Code = 'revision') = 0
begin
INSERT INTO [CollIdentificationCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('revision'
           ,'The correction of a former determination of the material'
           ,'revision'
           ,10
           ,1)
end
GO




--#####################################################################################################################
--######   [CollTransactionType_Enum]   ######################################################################################
--#####################################################################################################################

INSERT INTO [CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('return'
           ,'a partial return of a loan'
           ,'return'
           ,1)
GO


INSERT INTO [CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('forwarding'
           ,'a forwarding of specimens of a loan to another institution'
           ,'forwarding'
           ,1)
GO


INSERT INTO [CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('permit'
           ,'permit or certificate for the collection of specimen'
           ,'permit'
           ,1)
GO


UPDATE [CollTransactionType_Enum]
   SET [DisplayText] = 'borrowing'
 WHERE Code = 'borrow'
GO

--#####################################################################################################################
--######   CollectionSpecimenTransaction - TransactionReturnID  ######################################################################################
--#####################################################################################################################

ALTER TABLE dbo.CollectionSpecimenTransaction ADD TransactionReturnID int NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the table Transaction (= foreign key) for the return of a part that has been on loan' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenTransaction', @level2type=N'COLUMN',@level2name=N'TransactionReturnID'
GO

ALTER TABLE dbo.CollectionSpecimenTransaction_log ADD TransactionReturnID int NULL
GO


/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenTransaction]    Script Date: 04/11/2014 14:14:50 ******/

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR DELETE AS 
INSERT INTO CollectionSpecimenTransaction_Log (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, AccessionNumber, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState, TransactionReturnID) 
SELECT deleted.CollectionSpecimenID, deleted.TransactionID, deleted.SpecimenPartID, deleted.IsOnLoan, deleted.AccessionNumber, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D', deleted.TransactionReturnID
FROM DELETED

GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenTransaction]    Script Date: 04/11/2014 14:15:11 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR UPDATE AS
INSERT INTO CollectionSpecimenTransaction_Log (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, AccessionNumber, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState, TransactionReturnID) 
SELECT deleted.CollectionSpecimenID, deleted.TransactionID, deleted.SpecimenPartID, deleted.IsOnLoan, deleted.AccessionNumber, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U', deleted.TransactionReturnID
FROM DELETED
Update CollectionSpecimenTransaction
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenTransaction, deleted 
where 1 = 1 
AND CollectionSpecimenTransaction.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenTransaction.TransactionID = deleted.TransactionID
AND CollectionSpecimenTransaction.SpecimenPartID = deleted.SpecimenPartID

GO



--#####################################################################################################################
--######   CollectionSpecimenImage - DisplayOrder  ######################################################################################
--#####################################################################################################################

ALTER TABLE dbo.CollectionSpecimenImage ADD DisplayOrder int NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the images should be shown in a interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

ALTER TABLE dbo.CollectionSpecimenImage_log ADD DisplayOrder int NULL
GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenImage]    Script Date: 04/09/2014 13:41:38 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
FOR UPDATE AS
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
set @i = (select count(*) from deleted) 

GO


declare @DisplayOrder int
set @DisplayOrder = 1;
while (@DisplayOrder < 100 AND (select COUNT(*) from CollectionSpecimenImage where DisplayOrder IS NULL) > 0)
begin
	UPDATE I SET I.DisplayOrder = @DisplayOrder
	FROM [CollectionSpecimenImage] I
	where I.DisplayOrder IS NULL 
	AND exists(select * from [CollectionSpecimenImage] 
	L where I.CollectionSpecimenID = L.CollectionSpecimenID 
	and I.DisplayOrder is null
	and L.DisplayOrder is null
	group by L.CollectionSpecimenID having max(L.LogUpdatedWhen) = I.LogUpdatedWhen)
	set @DisplayOrder = @DisplayOrder +  1;
end

GO


UPDATE I SET I.LogCreatedWhen = I.LogUpdatedWhen
FROM [CollectionSpecimenImage] I
WHERE  I.LogCreatedWhen IS NULL
AND NOT I.LogUpdatedWhen IS NULL

GO

/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenImage]    Script Date: 04/09/2014 13:41:38 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
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
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogVersion,  LogState, DisplayOrder) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  @Version,  'U', deleted.DisplayOrder
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogVersion, LogState, DisplayOrder) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, CollectionSpecimen.Version, 'U', deleted.DisplayOrder 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
Update CollectionSpecimenImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenImage, deleted 
where 1 = 1 
AND CollectionSpecimenImage.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenImage.URI = deleted.URI

GO





/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenImage]    Script Date: 04/09/2014 13:52:15 ******/

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
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
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogVersion,  LogState, DisplayOrder) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  @Version,  'D', deleted.DisplayOrder
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogVersion, LogState, DisplayOrder) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, CollectionSpecimen.Version, 'D' , deleted.DisplayOrder
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



--#####################################################################################################################
--######   [CollectionSpecimenImageProperty]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionSpecimenImageProperty](
	[CollectionSpecimenID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[Property] [varchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenImageProperty] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[URI] ASC,
	[Property] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of CollectionSpecimen (= foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The complete URI address of the image. This is only a cached value, if ResourceID is available and referring to the module DiversityResources' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'URI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The property of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'Property'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If description of the property of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO


ALTER TABLE [dbo].[CollectionSpecimenImageProperty]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenImageProperty_CollectionSpecimenImage] FOREIGN KEY([CollectionSpecimenID], [URI])
REFERENCES [dbo].[CollectionSpecimenImage] ([CollectionSpecimenID], [URI])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty] CHECK CONSTRAINT [FK_CollectionSpecimenImageProperty_CollectionSpecimenImage]
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty] ADD  CONSTRAINT [DF_CollectionSpecimenImageProperty_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty] ADD  CONSTRAINT [DF_CollectionSpecimenImageProperty_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty] ADD  CONSTRAINT [DF_CollectionSpecimenImageProperty_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty] ADD  CONSTRAINT [DF_CollectionSpecimenImageProperty_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty] ADD  CONSTRAINT [DF__Collectio__RowGU__206E72C7]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

GRANT SELECT ON [CollectionSpecimenImageProperty] TO [User]
GO

GRANT INSERT ON [CollectionSpecimenImageProperty] TO Editor
GO

GRANT UPDATE ON [CollectionSpecimenImageProperty] TO Editor
GO

GRANT DELETE ON [CollectionSpecimenImageProperty] TO Editor
GO



/****** Object:  Table [dbo].[CollectionSpecimenImageProperty_log]    Script Date: 04/09/2014 15:05:31 ******/

CREATE TABLE [dbo].[CollectionSpecimenImageProperty_log](
	[CollectionSpecimenID] [int] NULL,
	[URI] [varchar](255) NULL,
	[Property] [varchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenImageProperty_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[CollectionSpecimenImageProperty_log] ADD  CONSTRAINT [DF_CollectionSpecimenImageProperty_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty_log] ADD  CONSTRAINT [DF_CollectionSpecimenImageProperty_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty_log] ADD  CONSTRAINT [DF_CollectionSpecimenImageProperty_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON [CollectionSpecimenImageProperty_log] TO Editor
GO

GRANT INSERT ON [CollectionSpecimenImageProperty_log] TO Editor
GO




/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenImageProperty]    Script Date: 04/09/2014 15:05:52 ******/

CREATE TRIGGER [dbo].[trgDelCollectionSpecimenImageProperty] ON [dbo].[CollectionSpecimenImageProperty] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 09.04.2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenImageProperty_Log (CollectionSpecimenID, URI, Property, Description, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.Property, deleted.Description, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO



/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenImageProperty]    Script Date: 04/09/2014 15:06:04 ******/

CREATE TRIGGER [dbo].[trgUpdCollectionSpecimenImageProperty] ON [dbo].[CollectionSpecimenImageProperty] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 09.04.2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenImageProperty_Log (CollectionSpecimenID, URI, Property, Description, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.Property, deleted.Description, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionSpecimenImageProperty
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenImageProperty, deleted 
where 1 = 1 
AND CollectionSpecimenImageProperty.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenImageProperty.Property = deleted.Property
AND CollectionSpecimenImageProperty.URI = deleted.URI
GO


--#####################################################################################################################
--######   [UserCollectionList]  ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[UserCollectionList] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint])
/*
Returns a table that lists all the collections a User has access to, including the child collections.
MW 20.02.2012
Test:
select * from dbo.UserCollectionList()
*/
AS
BEGIN
	DECLARE @CollectionID INT
	DECLARE @TempAdminCollectionID TABLE (CollectionID int primary key)
	DECLARE @TempCollectionID TABLE (CollectionID int primary key)
	IF (SELECT COUNT(*) FROM CollectionUser WHERE (LoginName = USER_NAME())) > 0
	BEGIN
		IF (SELECT COUNT(*) FROM CollectionManager WHERE (LoginName = USER_NAME())) > 0
		BEGIN
			INSERT @TempCollectionID (CollectionID) 
				SELECT CollectionID FROM ManagerCollectionList()
		END
		INSERT @TempAdminCollectionID (CollectionID) 
		SELECT CollectionID FROM CollectionUser WHERE (LoginName = USER_NAME()) 
		INSERT @TempCollectionID (CollectionID) 
		SELECT CollectionID FROM CollectionUser WHERE (LoginName = USER_NAME()) 
		DECLARE HierarchyCursor  CURSOR for
		select CollectionID from @TempAdminCollectionID
		open HierarchyCursor
		FETCH next from HierarchyCursor into @CollectionID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			insert into @TempCollectionID select CollectionID 
			from dbo.CollectionChildNodes (@CollectionID) where CollectionID not in (select CollectionID from @TempCollectionID)
			FETCH NEXT FROM HierarchyCursor into @CollectionID
		END
		CLOSE HierarchyCursor
		DEALLOCATE HierarchyCursor
	END
	IF (select COUNT(*) from @TempCollectionID) = 0
	BEGIN
		INSERT @TempCollectionID (CollectionID) 
			SELECT CollectionID FROM Collection
	END
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder FROM dbo.Collection
	WHERE CollectionID in (SELECT CollectionID FROM @TempCollectionID)
	RETURN
END

GO


--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.03' 
END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.48'
END

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.90'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  MaterialSource     #######################################################################################
--#####################################################################################################################

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.COLUMN_NAME = 'MaterialSource' AND C.TABLE_NAME = 'Transaction') = 0
BEGIN
	ALTER TABLE [dbo].[Transaction] ADD [MaterialSource] nvarchar(500) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The source of the material within a transaction, e.g. a excavation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Transaction', @level2type=N'COLUMN',@level2name=N'MaterialSource'
	ALTER TABLE [dbo].[Transaction_log] ADD MaterialSource nvarchar(500) NULL
END
GO

/****** Object:  Trigger [dbo].[trgDelTransaction]    Script Date: 27.06.2016 11:23:51 ******/
ALTER TRIGGER [dbo].[trgDelTransaction] ON [dbo].[Transaction] 
FOR DELETE AS 
INSERT INTO Transaction_Log (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, ActualEndDate, 
InternalNotes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState, ToRecipient, MaterialSource) 
SELECT deleted.TransactionID, deleted.ParentTransactionID, deleted.TransactionType, deleted.TransactionTitle, deleted.ReportingCategory, deleted.AdministratingCollectionID, 
deleted.MaterialDescription, deleted.MaterialCategory, deleted.MaterialCollectors, deleted.FromCollectionID, deleted.FromTransactionPartnerName, 
deleted.FromTransactionPartnerAgentURI, deleted.FromTransactionNumber, deleted.ToCollectionID, deleted.ToTransactionPartnerName, deleted.ToTransactionPartnerAgentURI, 
deleted.ToTransactionNumber, deleted.NumberOfUnits, deleted.Investigator, deleted.TransactionComment, deleted.BeginDate, deleted.AgreedEndDate, deleted.ActualEndDate, 
deleted.InternalNotes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D', 
deleted.ToRecipient, deleted.MaterialSource
FROM DELETED

GO

/****** Object:  Trigger [dbo].[trgUpdTransaction]    Script Date: 27.06.2016 11:26:42 ******/
ALTER TRIGGER [dbo].[trgUpdTransaction] ON [dbo].[Transaction] 
FOR UPDATE AS
INSERT INTO Transaction_Log (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, ActualEndDate, 
InternalNotes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState, ToRecipient, MaterialSource) 
SELECT deleted.TransactionID, deleted.ParentTransactionID, deleted.TransactionType, deleted.TransactionTitle, deleted.ReportingCategory, deleted.AdministratingCollectionID, 
deleted.MaterialDescription, deleted.MaterialCategory, deleted.MaterialCollectors, deleted.FromCollectionID, deleted.FromTransactionPartnerName, 
deleted.FromTransactionPartnerAgentURI, deleted.FromTransactionNumber, deleted.ToCollectionID, deleted.ToTransactionPartnerName, deleted.ToTransactionPartnerAgentURI, 
deleted.ToTransactionNumber, deleted.NumberOfUnits, deleted.Investigator, deleted.TransactionComment, deleted.BeginDate, deleted.AgreedEndDate, deleted.ActualEndDate, 
deleted.InternalNotes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U', 
deleted.ToRecipient, deleted.MaterialSource
FROM DELETED
Update [Transaction]
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM [Transaction], deleted 
where 1 = 1 
AND [Transaction].TransactionID = deleted.TransactionID

GO



--#####################################################################################################################
--######  TransactionList          ####################################################################################
--#####################################################################################################################


ALTER VIEW [dbo].[TransactionList]
AS
SELECT     TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
                      MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
                      ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
                      BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
FROM         dbo.[Transaction]
WHERE     (AdministratingCollectionID IS NULL)
UNION
SELECT     T.TransactionID, T.ParentTransactionID, T.TransactionType, T.TransactionTitle, 
                      T.ReportingCategory, T.AdministratingCollectionID, T.MaterialDescription, T.MaterialSource, T.MaterialCategory, 
                      T.MaterialCollectors, T.FromCollectionID, T.FromTransactionPartnerName, 
                      T.FromTransactionPartnerAgentURI, T.FromTransactionNumber, T.ToCollectionID, 
                      T.ToTransactionPartnerName, T.ToTransactionPartnerAgentURI, T.ToTransactionNumber, T.ToRecipient, 
                      T.NumberOfUnits, T.Investigator, T.TransactionComment, T.BeginDate, 
                      T.AgreedEndDate, T.ActualEndDate, T.InternalNotes, T.ResponsibleName, 
                      T.ResponsibleAgentURI
FROM         dbo.[Transaction] AS T INNER JOIN
                      dbo.CollectionManager ON T.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID
WHERE     (dbo.CollectionManager.LoginName = USER_NAME())

GO




--#####################################################################################################################
--######  TransactionChildNodes    ####################################################################################
--#####################################################################################################################

ALTER  FUNCTION [dbo].[TransactionChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL
)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
	RETURN 
END

GO


ALTER  FUNCTION [dbo].[TransactionChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL
)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL
)
INSERT @TempItem ( TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI) 
	SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
FROM [TransactionList]   
WHERE ParentTransactionID = @ID 
  DECLARE HierarchyCursor  CURSOR for
   select TransactionID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
	from dbo.TransactionChildNodes (@ParentID) where TransactionID not in (select TransactionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
INSERT @ItemList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI) 
   SELECT distinct TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
   FROM @TempItem ORDER BY TransactionTitle
   RETURN
END

GO



--#####################################################################################################################
--######  TransactionChildNodesAccess     #############################################################################
--#####################################################################################################################


IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES R WHERE R.ROUTINE_NAME = 'TransactionChildNodesAccess') = 1
BEGIN
	DROP FUNCTION [dbo].[TransactionChildNodesAccess]
END

GO


CREATE FUNCTION [dbo].[TransactionChildNodesAccess] (@ID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Accessible] tinyint NULL
)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item 
where the current user has no access according to the restriction in TransactionList.
MW02.01.2006
*/
AS
BEGIN
	declare @ParentID int
	RETURN
END
GO


GRANT SELECT ON [dbo].[TransactionChildNodesAccess] TO [User]
GO


ALTER  FUNCTION [dbo].[TransactionChildNodesAccess] (@ID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Accessible] tinyint NULL
)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item 
where the current user has no access according to the restriction in TransactionList.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Accessible] tinyint NULL
)
INSERT @TempItem ( TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, Accessible) 
	SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, 0
FROM [Transaction]   
WHERE ParentTransactionID = @ID 
  DECLARE HierarchyCursor  CURSOR for
   select TransactionID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
	  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
	  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
	  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, Accessible
	from dbo.TransactionChildNodesAccess (@ParentID) where TransactionID not in (select TransactionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor

INSERT @ItemList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, Accessible) 
   SELECT distinct TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, Accessible
   FROM @TempItem ORDER BY TransactionTitle
   RETURN
END

GO


--#####################################################################################################################
--######  TransactionHierarchy     ####################################################################################
--#####################################################################################################################

ALTER  FUNCTION [dbo].[TransactionHierarchy] (@TransactionID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL
)
/*
Returns a table that lists all the transactions related to the given transaction.
MW 02.01.2006
*/
AS
BEGIN
declare @TopID int
declare @i int
set @TopID = (select ParentTransactionID from [TransactionList] where TransactionID = @TransactionID) 
set @i = (select count(*) from [TransactionList] where TransactionID = @TransactionID)
if (@TopID is null )
	set @TopID =  @TransactionID
else	
	begin
	while (@i > 0)
		begin
		set @TransactionID = (select ParentTransactionID from [TransactionList] where TransactionID = @TransactionID and not ParentTransactionID is null) 
		set @i = (select count(*) from [TransactionList] where TransactionID = @TransactionID and not ParentTransactionID is null)
		end
	set @TopID = @TransactionID
	end
INSERT @ItemList
SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
FROM [TransactionList]
WHERE TransactionID = @TopID
INSERT @ItemList
SELECT * FROM dbo.TransactionChildNodes (@TopID)
   RETURN
END

GO


--#####################################################################################################################
--######  TransactionHierarchyAccess     ##############################################################################
--#####################################################################################################################

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES R WHERE R.ROUTINE_NAME = 'TransactionHierarchyAccess') = 1
BEGIN
	DROP FUNCTION [dbo].[TransactionHierarchyAccess]
END

GO

CREATE  FUNCTION [dbo].[TransactionHierarchyAccess] (@TransactionID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Accessible] tinyint NULL
)
/*
Returns a table that lists all the transactions related to the given transaction.
MW 02.01.2006
*/
AS
BEGIN

-- finding the top ID
declare @TopID int
declare @i int
set @TopID = (select ParentTransactionID from [Transaction] where TransactionID = @TransactionID) 
set @i = (select count(*) from [Transaction] where TransactionID = @TransactionID)
if (@TopID is null )
	set @TopID =  @TransactionID
else	
	begin
	while (@i > 0)
		begin
		set @TransactionID = (select ParentTransactionID from [Transaction] where TransactionID = @TransactionID and not ParentTransactionID is null) 
		set @i = (select count(*) from [Transaction] where TransactionID = @TransactionID and not ParentTransactionID is null)
		end
	set @TopID = @TransactionID
	end

-- insert the top dataset
INSERT @ItemList
SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, 0
FROM [Transaction]
WHERE TransactionID = @TopID

-- insert the depending datasets
INSERT @ItemList
SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, 0 FROM dbo.TransactionChildNodesAccess (@TopID)

-- setting the Accessibility marker
UPDATE I SET I.Accessible = 1 from @ItemList I, TransactionList L
WHERE I.TransactionID = L.TransactionID
   RETURN
END

GO


GRANT SELECT ON [dbo].[TransactionHierarchyAccess] TO [User]
GO



--#####################################################################################################################
--######  TransactionHierarchyAll        ##############################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[TransactionHierarchyAll] ()  
RETURNS @TransactionList TABLE ([TransactionID] [int] Primary key ,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NOT NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialSource] [nvarchar](500) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[ToRecipient] [nvarchar](255) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](200) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[DisplayText] [nvarchar](500),
	[HierarchyDisplayText] [nvarchar](500))
/*
Returns a table that lists all the transactions items related to the given analysis.
MW 02.01.2006
TEST:
SELECT * FROM DBO.TransactionHierarchyAll()
*/
AS
BEGIN
INSERT @TransactionList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, MaterialCategory, 
MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText)
SELECT DISTINCT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, MaterialCategory, 
MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToRecipient, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
, TransactionTitle
FROM [Transaction]
WHERE [Transaction].ParentTransactionID IS NULL
declare @i int
declare @iNext int
set @i = (select count(*) from [Transaction] where TransactionID not IN (select TransactionID from  @TransactionList))
while (@i > 0)
	begin
	INSERT @TransactionList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, MaterialCategory, 
		MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
		ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
		ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText)
	SELECT DISTINCT C.TransactionID, C.ParentTransactionID, C.TransactionType, C.TransactionTitle, C.ReportingCategory, C.AdministratingCollectionID, C.MaterialDescription, C.MaterialSource, C.MaterialCategory, 
		C.MaterialCollectors, C.FromCollectionID, C.FromTransactionPartnerName, C.FromTransactionPartnerAgentURI, C.FromTransactionNumber, C.ToCollectionID, 
		C.ToTransactionPartnerName, C.ToTransactionPartnerAgentURI, C.ToTransactionNumber, C.ToRecipient, C.NumberOfUnits, C.Investigator, C.TransactionComment, C.BeginDate, C.AgreedEndDate, 
		C.ActualEndDate, C.InternalNotes, C.ResponsibleName, C.ResponsibleAgentURI, L.DisplayText + ' | ' + C.TransactionTitle
	FROM [Transaction] C, @TransactionList L
	WHERE C.ParentTransactionID = L.TransactionID
	AND C.TransactionID NOT IN (select TransactionID from  @TransactionList)
	set @iNext = (select count(*) from [Transaction] where TransactionID not IN (select TransactionID from  @TransactionList))
	if (@iNext <> @i)
	begin
		set @i = @iNext;
	end
	else 
	begin
		set @i = 0;
	end
end
update T set HierarchyDisplayText = DisplayText
from @TransactionList T
   RETURN
END

GO



--#####################################################################################################################
--######  Transaction type removal    #################################################################################
--#####################################################################################################################

INSERT INTO [dbo].[CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[InternalNotes])
     VALUES
           ('removal'
           ,'the removal of specimens from an institution, e.g. by damage, loss etc.'
           ,'removal'
           ,1
           ,'BeginDate')
GO



--#####################################################################################################################
--######   trgDelCollectionSpecimenReference   ########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenReference] ON [dbo].[CollectionSpecimenReference] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/20/2016  */ 


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
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ReferenceID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ReferenceID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.91'
END

GO


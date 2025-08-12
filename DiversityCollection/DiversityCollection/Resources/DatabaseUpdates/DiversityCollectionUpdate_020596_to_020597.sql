declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.96'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######  TransactionList          ####################################################################################
--#####################################################################################################################


ALTER VIEW [dbo].[TransactionList]
AS
SELECT     TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
                      MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
                      ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
                      BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
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
                      T.ResponsibleAgentURI, T.DateSupplement
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
	[ResponsibleAgentURI] [varchar](255) NULL,
	[DateSupplement] [nvarchar](100) NULL
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
	[ResponsibleAgentURI] [varchar](255) NULL,
	[DateSupplement] [nvarchar](100) NULL
)
INSERT @TempItem ( TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement) 
	SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
FROM [TransactionList]   
WHERE ParentTransactionID = @ID 
/*
  DECLARE HierarchyCursor  CURSOR for
   select TransactionID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
	from dbo.TransactionChildNodes (@ParentID) where TransactionID not in (select TransactionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
   */
INSERT @ItemList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement) 
   SELECT distinct TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
   FROM @TempItem ORDER BY TransactionTitle
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
	[ResponsibleAgentURI] [varchar](255) NULL,
	[DateSupplement] [nvarchar](100) NULL
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
	[ResponsibleAgentURI] [varchar](255) NULL,
	[DateSupplement] [nvarchar](100) NULL
)
INSERT @TempItem ( TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement) 
	SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
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
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
	from dbo.TransactionChildNodes (@ParentID) where TransactionID not in (select TransactionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
   
INSERT @ItemList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement) 
   SELECT distinct TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
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
	[ResponsibleAgentURI] [varchar](255) NULL,
	[DateSupplement] [nvarchar](100) NULL
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
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
FROM [TransactionList]
WHERE TransactionID = @TopID
INSERT @ItemList
SELECT * FROM dbo.TransactionChildNodes (@TopID)
   RETURN
END

GO



--#####################################################################################################################
--######   DNA, lyophilised              ##############################################################################
--#####################################################################################################################

if (select count(*) from [dbo].CollMaterialCategory_Enum T where t.Code = 'DNA lyophilised') = 0
begin
	INSERT INTO [dbo].CollMaterialCategory_Enum
			   (Code, Description, ParentCode, DisplayText, DisplayEnable, DisplayOrder)
		 VALUES
			   ('DNA lyophilised', 'DNA lyophilised', 'DNA sample', 'DNA lyophilised', 1, 91)
end
GO


--#####################################################################################################################
--######   ExternalIdentifierType        ##############################################################################
--#####################################################################################################################

if (select count(*) from [dbo].[ExternalIdentifierType] T where t.Type = 'ID') = 0
begin
	INSERT INTO [dbo].[ExternalIdentifierType]
			   ([Type])
		 VALUES
			   ('ID')
end
GO


UPDATE [dbo].[ExternalIdentifierType]
   SET [ParentType] = 'ID'
 WHERE Type <> 'ID' and ParentType is null
GO


if (select count(*) from [dbo].[ExternalIdentifierType] T where t.Type = 'Regulation') = 0
begin
	INSERT INTO [dbo].[ExternalIdentifierType]
			   ([Type])
		 VALUES
			   ('Regulation')
end
GO



--#####################################################################################################################
--######   CollectionHierarchySuperior   ##############################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionHierarchySuperior] (@CollectionID int)  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL)
/*
Returns a table that lists the given and all the items superior to the given collection.
MW 02.09.2016
Test:
SELECT * FROM dbo.CollectionHierarchySuperior(3)
SELECT * FROM dbo.CollectionHierarchySuperior(1015)
SELECT * FROM dbo.CollectionHierarchySuperior(0)
*/
AS
BEGIN
declare @ParentID int
declare @i int

-- insert the given collection
   INSERT @CollectionList
   SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
   FROM Collection
   WHERE Collection.CollectionID = @CollectionID

-- insert the superior collections
-- check if there is any superior collection
if (select count(*) from @CollectionList where CollectionID = @CollectionID and CollectionParentID is null) = 1
	RETURN

-- getting the superiors
set @ParentID = (select MAX(CollectionParentID) from @CollectionList where not CollectionParentID is null AND CollectionParentID NOT IN (SELECT CollectionID FROM @CollectionList)) 
set @i = 1
while (@i > 0)
begin
	set @ParentID = (select MAX(CollectionParentID) from @CollectionList where not CollectionParentID is null AND CollectionParentID NOT IN (SELECT CollectionID FROM @CollectionList)) 
	--set @ParentID = @CollectionID
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
	FROM Collection
	WHERE Collection.CollectionID = @ParentID
	AND Collection.CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	set @i = (select count(*) from @CollectionList where not CollectionParentID is null AND CollectionParentID NOT IN (SELECT CollectionID FROM @CollectionList))
end
RETURN

END

GO

GRANT SELECT ON dbo.CollectionHierarchySuperior TO [User]
GO



--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.06' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.97'
END

GO


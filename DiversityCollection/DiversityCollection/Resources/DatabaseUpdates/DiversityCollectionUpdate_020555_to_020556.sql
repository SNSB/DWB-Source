
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.55'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   TransactionDocument   ######################################################################################
--######   Adding the column DocumentURI allowing the include documents from the web   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TransactionDocument' and C.COLUMN_NAME = 'DocumentURI') = 0
begin
ALTER TABLE TransactionDocument ADD DocumentURI varchar(1000) NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A link to a web resource of the document the document' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionDocument', @level2type=N'COLUMN',@level2name=N'DocumentURI'
ALTER TABLE TransactionDocument_log ADD DocumentURI varchar(1000) NULL
end
GO



/****** Object:  Trigger [dbo].[trgDelTransactionDocument]    Script Date: 07/08/2014 11:56:51 ******/
ALTER TRIGGER [dbo].[trgDelTransactionDocument] ON [dbo].[TransactionDocument] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionDocument_Log (TransactionID, Date, TransactionText, DocumentURI, DisplayText, InternalNotes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.Date, deleted.TransactionText, deleted.DocumentURI, deleted.DisplayText, deleted.InternalNotes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED
GO



/****** Object:  Trigger [dbo].[trgUpdTransactionDocument]    Script Date: 07/08/2014 11:58:35 ******/
ALTER TRIGGER [dbo].[trgUpdTransactionDocument] ON [dbo].[TransactionDocument] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionDocument_Log (TransactionID, Date, TransactionText, DocumentURI, DisplayText, InternalNotes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.Date, deleted.TransactionText, deleted.DocumentURI, deleted.DisplayText, deleted.InternalNotes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update TransactionDocument
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM TransactionDocument, deleted 
where 1 = 1 
AND TransactionDocument.TransactionID = deleted.TransactionID
AND TransactionDocument.Date = deleted.Date


GO


--#####################################################################################################################
--######   TransactionHierarchyAccess   ######################################################################################
--#####################################################################################################################


CREATE  FUNCTION [dbo].[TransactionHierarchyAccess] (@TransactionID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
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
Corresponding function as [dbo].[TransactionHierarchy] but returns accessible and inaccessible transactions and adds a column containing the accessiblity
1 = Accessible
0 = Not accessible
MW 16.03.2015
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
SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, 0
FROM [Transaction]
WHERE TransactionID = @TopID

-- insert the depending datasets
INSERT @ItemList
SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, 0 FROM dbo.TransactionChildNodesAccess (@TopID)

-- setting the Accessibility marker
UPDATE I SET I.Accessible = 1 from @ItemList I, TransactionList L
WHERE I.TransactionID = L.TransactionID
   RETURN
END



GO

GRANT SELECT ON dbo.TransactionHierarchyAccess TO [User]
GO



--#####################################################################################################################
--######   TransactionChildNodesAccess  ######################################################################################
--#####################################################################################################################



CREATE  FUNCTION [dbo].[TransactionChildNodesAccess] (@ID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] NOT NULL,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[MaterialDescription] [nvarchar](max) NULL,
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
including the indication where the current user has access according to the restriction in TransactionList.
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
INSERT @TempItem ( TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, Accessible) 
	SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
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
	insert into @TempItem select TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
	  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
	  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
	  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, Accessible
	from dbo.TransactionChildNodesAccess (@ParentID) where TransactionID not in (select TransactionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor

INSERT @ItemList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, Accessible) 
   SELECT distinct TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, Accessible
   FROM @TempItem ORDER BY TransactionTitle
   RETURN
END

GO


GRANT SELECT ON dbo.TransactionChildNodesAccess TO [User]
GO



--#####################################################################################################################
--######   Forwarding - enalbe this type  ######################################################################################
--#####################################################################################################################


UPDATE [dbo].[CollTransactionType_Enum]
   SET [DisplayEnable] = 1
 WHERE [Code] = 'Forwarding'
GO





-- PK um Identity ergaenzen und Datum aus PK


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.56'
END

GO



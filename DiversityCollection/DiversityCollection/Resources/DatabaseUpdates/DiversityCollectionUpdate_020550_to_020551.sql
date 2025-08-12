
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.50'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Receiver for Transaction  ######################################################################################
--#####################################################################################################################


ALTER TABLE [dbo].[Transaction] ADD [ToRecipient] [nvarchar](255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The recipient receiving the transaction e.g. if not derived from the link to DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Transaction', @level2type=N'COLUMN',@level2name=N'ToRecipient'
GO

ALTER TABLE [dbo].[Transaction_log] ADD [ToRecipient] [nvarchar](255) NULL
GO


--#####################################################################################################################
/****** Object:  Trigger [dbo].[trgDelTransaction]    Script Date: 21.08.2014 12:49:12 ******/
ALTER TRIGGER [dbo].[trgDelTransaction] ON [dbo].[Transaction] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Transaction_Log (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, ActualEndDate, 
InternalNotes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState, ToRecipient) 
SELECT deleted.TransactionID, deleted.ParentTransactionID, deleted.TransactionType, deleted.TransactionTitle, deleted.ReportingCategory, deleted.AdministratingCollectionID, 
deleted.MaterialDescription, deleted.MaterialCategory, deleted.MaterialCollectors, deleted.FromCollectionID, deleted.FromTransactionPartnerName, 
deleted.FromTransactionPartnerAgentURI, deleted.FromTransactionNumber, deleted.ToCollectionID, deleted.ToTransactionPartnerName, deleted.ToTransactionPartnerAgentURI, 
deleted.ToTransactionNumber, deleted.NumberOfUnits, deleted.Investigator, deleted.TransactionComment, deleted.BeginDate, deleted.AgreedEndDate, deleted.ActualEndDate, 
deleted.InternalNotes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D', deleted.ToRecipient
FROM DELETED

GO


--#####################################################################################################################
/****** Object:  Trigger [dbo].[trgUpdTransaction]    Script Date: 21.08.2014 12:50:20 ******/
ALTER TRIGGER [dbo].[trgUpdTransaction] ON [dbo].[Transaction] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Transaction_Log (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, ActualEndDate, 
InternalNotes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState, ToRecipient) 
SELECT deleted.TransactionID, deleted.ParentTransactionID, deleted.TransactionType, deleted.TransactionTitle, deleted.ReportingCategory, deleted.AdministratingCollectionID, 
deleted.MaterialDescription, deleted.MaterialCategory, deleted.MaterialCollectors, deleted.FromCollectionID, deleted.FromTransactionPartnerName, 
deleted.FromTransactionPartnerAgentURI, deleted.FromTransactionNumber, deleted.ToCollectionID, deleted.ToTransactionPartnerName, deleted.ToTransactionPartnerAgentURI, 
deleted.ToTransactionNumber, deleted.NumberOfUnits, deleted.Investigator, deleted.TransactionComment, deleted.BeginDate, deleted.AgreedEndDate, deleted.ActualEndDate, 
deleted.InternalNotes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U', deleted.ToRecipient
FROM DELETED


/* updating the logging columns */
Update [Transaction]
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM [Transaction], deleted 
where 1 = 1 
AND [Transaction].TransactionID = deleted.TransactionID

GO



--#####################################################################################################################
/****** Object:  UserDefinedFunction [dbo].[TransactionHierarchyAll]    Script Date: 25.08.2014 15:16:43 ******/

ALTER FUNCTION [dbo].[TransactionHierarchyAll] ()  
RETURNS @TransactionList TABLE ([TransactionID] [int] Primary key ,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NOT NULL,
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
INSERT @TransactionList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, 
MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText)
SELECT DISTINCT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, 
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
	INSERT @TransactionList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, 
		MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
		ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
		ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText)
	SELECT DISTINCT C.TransactionID, C.ParentTransactionID, C.TransactionType, C.TransactionTitle, C.ReportingCategory, C.AdministratingCollectionID, C.MaterialDescription, C.MaterialCategory, 
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
--#####################################################################################################################
ALTER VIEW [dbo].[TransactionList]
AS
SELECT     TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
                      MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
                      ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
                      BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
FROM         dbo.[Transaction]
WHERE     (AdministratingCollectionID IS NULL)
UNION
SELECT     T.TransactionID, T.ParentTransactionID, T.TransactionType, T.TransactionTitle, 
                      T.ReportingCategory, T.AdministratingCollectionID, T.MaterialDescription, T.MaterialCategory, 
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
ALTER  FUNCTION [dbo].[TransactionChildNodes] (@ID int)  
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
	[ResponsibleAgentURI] [varchar](255) NULL
)
BEGIN
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
INSERT @TempItem ( TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI) 
	SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
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
	insert into @TempItem select TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
	from dbo.TransactionChildNodes (@ParentID) where TransactionID not in (select TransactionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
INSERT @ItemList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI) 
   SELECT distinct TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
   FROM @TempItem ORDER BY TransactionTitle
   RETURN
END

GO







/****** Object:  UserDefinedFunction [dbo].[TransactionHierarchy]    Script Date: 25.08.2014 15:16:35 ******/

ALTER  FUNCTION [dbo].[TransactionHierarchy] (@TransactionID int)  
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
SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, 
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
--######   [TransactionDocument]   ######################################################################################
--#####################################################################################################################



ALTER TABLE [dbo].[TransactionDocument] ADD	[DisplayText] [nvarchar](255) NULL
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A display text as shown e.g. in a user interface to characterize the document' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionDocument', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

ALTER TABLE [dbo].[TransactionDocument_log] ADD	[DisplayText] [nvarchar](255) NULL
GO



/****** Object:  Trigger [dbo].[trgDelTransactionDocument]    Script Date: 07/08/2014 11:56:51 ******/
ALTER TRIGGER [dbo].[trgDelTransactionDocument] ON [dbo].[TransactionDocument] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionDocument_Log (TransactionID, Date, TransactionText, DisplayText, InternalNotes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.Date, deleted.TransactionText, deleted.DisplayText, deleted.InternalNotes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED



GO



/****** Object:  Trigger [dbo].[trgUpdTransactionDocument]    Script Date: 07/08/2014 11:58:35 ******/
ALTER TRIGGER [dbo].[trgUpdTransactionDocument] ON [dbo].[TransactionDocument] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 29.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionDocument_Log (TransactionID, Date, TransactionText, DisplayText, InternalNotes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.TransactionID, deleted.Date, deleted.TransactionText, deleted.DisplayText, deleted.InternalNotes, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
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
--######   amphibian   ######################################################################################
--#####################################################################################################################


INSERT INTO [dbo].[CollTaxonomicGroup_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('amphibian'
           ,'amphibian'
           ,'amphibian'
           ,505
           ,1
           ,'vertebrate')
GO




--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.06' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.51'
END

GO



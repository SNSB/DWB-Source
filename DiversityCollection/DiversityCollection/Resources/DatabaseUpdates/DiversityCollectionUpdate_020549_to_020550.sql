
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.49'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   ActualEndDate for prolonation  ######################################################################################
--#####################################################################################################################



EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Actual end of the transaction after a prolonation when e.g. the date of return for a loan was prolonged by the owner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Transaction', @level2type=N'COLUMN',@level2name=N'ActualEndDate'
GO

UPDATE e SET [Description] = 'Actual end of the transaction after a prolonation when e.g. the date of return for a loan was prolonged by the owner'
FROM [EntityRepresentation]
e where e.Entity = 'Transaction.ActualEndDate' and e.LanguageCode = 'en-US' AND e.EntityContext = 'General'
GO

UPDATE e SET [Description] = 'Ende einer Transaktion nach einer Verlängerung wenn e.g. für eine Ausleihe eine Verlängerung bis zu diesem Datum gewährt wurde.'
FROM [EntityRepresentation]
e where e.Entity = 'Transaction.ActualEndDate' and e.LanguageCode = 'de-DE' AND e.EntityContext = 'General'
GO

  
--#####################################################################################################################
--######   Permit and transaction group  ######################################################################################
--#####################################################################################################################

IF (SELECT COUNT(*) FROM [CollTransactionType_Enum] E WHERE E.[Code] = 'permit') = 0
BEGIN
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
END
GO


IF (SELECT COUNT(*) FROM [CollTransactionType_Enum] E WHERE E.[Code] = 'transaction group') = 0
BEGIN
	INSERT INTO [CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('transaction group'
           ,'the superior dataset for a group of transactions'
           ,'transaction group'
           ,1)
END
GO


/*
SELECT 'UPDATE [CollTransactionType_Enum] SET  [DisplayEnable] = ' + cast([DisplayEnable] as varchar) 
+ ', DisplayOrder = ' + case when [DisplayOrder] is null then 'NULL' else  cast([DisplayOrder] as varchar) end + ', [InternalNotes] = ' + CHAR(39) + [InternalNotes] + CHAR(39)
+ ' WHERE Code = ' +  CHAR(39) + Code + CHAR(39)  
FROM [CollTransactionType_Enum]

SELECT 'UPDATE [CollTransactionType_Enum] SET DisplayOrder = ' + case when [DisplayOrder] is null then 'NULL' else  cast([DisplayOrder] as varchar) end + ', [InternalNotes] = ' + CHAR(39) + [InternalNotes] + CHAR(39)
+ ' WHERE Code = ' +  CHAR(39) + Code + CHAR(39)  
FROM [CollTransactionType_Enum]
*/

UPDATE [CollTransactionType_Enum] SET DisplayOrder = 60, [InternalNotes] = 'BeginDate' WHERE Code = 'borrow'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = 80, [InternalNotes] = 'BeginDate' WHERE Code = 'embargo'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = 40, [InternalNotes] = 'BeginDate' WHERE Code = 'exchange'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = NULL, [InternalNotes] = 'BeginDate' WHERE Code = 'forwarding'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = 20, [InternalNotes] = 'BeginDate' WHERE Code = 'gift'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = 10, [InternalNotes] = 'AccessionNumber' WHERE Code = 'inventory'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = 50, [InternalNotes] = 'BeginDate' WHERE Code = 'loan'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = 70, [InternalNotes] = 'BeginDate' WHERE Code = 'permit'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = 30, [InternalNotes] = 'BeginDate' WHERE Code = 'purchase'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = 10, [InternalNotes] = 'BeginDate' WHERE Code = 'request'
UPDATE [CollTransactionType_Enum] SET DisplayOrder = NULL, [InternalNotes] = 'BeginDate' WHERE Code = 'return'

IF (SELECT COUNT(*) AS Expr1 FROM [Transaction] WHERE (TransactionType = N'request')) = 0
begin
	UPDATE [CollTransactionType_Enum] SET  [DisplayEnable] = 0 WHERE Code = 'request'
end

IF (SELECT COUNT(*) AS Expr1 FROM [Transaction] WHERE (TransactionType = N'borrow')) = 0
begin
	UPDATE [CollTransactionType_Enum] SET  [DisplayEnable] = 0 WHERE Code = 'borrow'
end

IF (SELECT COUNT(*) AS Expr1 FROM [Transaction] WHERE (TransactionType = N'forwarding')) = 0
begin
	UPDATE [CollTransactionType_Enum] SET  [DisplayEnable] = 0 WHERE Code = 'forwarding'
end

GO



--#####################################################################################################################
--######   [TransactionComment]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[TransactionComment](
	[Comment] [nvarchar](400) NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TransactionComment] PRIMARY KEY CLUSTERED 
(
	[Comment] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Text as transferred into the comment of a transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionComment', @level2type=N'COLUMN',@level2name=N'Comment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionComment', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionComment', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionComment', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionComment', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

ALTER TABLE [dbo].[TransactionComment] ADD  CONSTRAINT [DF_TransactionComment_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[TransactionComment] ADD  CONSTRAINT [DF_TransactionComment_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[TransactionComment] ADD  CONSTRAINT [DF_TransactionComment_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[TransactionComment] ADD  CONSTRAINT [DF_TransactionComment_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[TransactionComment] ADD  CONSTRAINT [DF_TransactionComment_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


INSERT INTO [TransactionComment]
           ([Comment])
     VALUES
           ('Determinavit labels are partly lacking.')
GO

INSERT INTO [TransactionComment]
           ([Comment])
     VALUES
           ('Determinavit labels are completely lacking.')
GO


INSERT INTO [TransactionComment]
           ([Comment])
     VALUES
           ('Please send a formal request for prolongation should this date be already passed.')
GO

GRANT SELECT ON dbo.TransactionComment TO [User]
GO

GRANT INSERT ON dbo.TransactionComment TO [Administrator]
GO

GRANT UPDATE ON dbo.TransactionComment TO [Administrator]
GO

GRANT DELETE ON dbo.TransactionComment TO [Administrator]
GO



--#####################################################################################################################
--######   TransactionHierarchyAll - Schleifenstopp   ######################################################################################
--#####################################################################################################################



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
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText)
SELECT DISTINCT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, 
MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
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
		ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
		ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText)
	SELECT DISTINCT C.TransactionID, C.ParentTransactionID, C.TransactionType, C.TransactionTitle, C.ReportingCategory, C.AdministratingCollectionID, C.MaterialDescription, C.MaterialCategory, 
		C.MaterialCollectors, C.FromCollectionID, C.FromTransactionPartnerName, C.FromTransactionPartnerAgentURI, C.FromTransactionNumber, C.ToCollectionID, 
		C.ToTransactionPartnerName, C.ToTransactionPartnerAgentURI, C.ToTransactionNumber, C.NumberOfUnits, C.Investigator, C.TransactionComment, C.BeginDate, C.AgreedEndDate, 
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
	--set @i = (select count(*) from [Transaction] where TransactionID not IN (select TransactionID from  @TransactionList))
end
update T set HierarchyDisplayText = DisplayText
from @TransactionList T
   RETURN
END
GO

--#####################################################################################################################
--######   EntityDetermination_Enum   ######################################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.EntityDetermination_Enum TO [User]
GO


--#####################################################################################################################
--######   trgDelCollectionSpecimenTransaction   ######################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR DELETE AS 
INSERT INTO CollectionSpecimenTransaction_Log (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, AccessionNumber, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState, TransactionReturnID) 
SELECT deleted.CollectionSpecimenID, deleted.TransactionID, deleted.SpecimenPartID, deleted.IsOnLoan, deleted.AccessionNumber, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D', deleted.TransactionReturnID
FROM DELETED

UPDATE T SET T.TransactionReturnID = NULL 
FROM [CollectionSpecimenTransaction] T, DELETED D
WHERE T.CollectionSpecimenID = D.CollectionSpecimenID
AND T.SpecimenPartID = D.SpecimenPartID
AND T.TransactionReturnID = D.TransactionID

GO




--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.05' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.50'
END

GO



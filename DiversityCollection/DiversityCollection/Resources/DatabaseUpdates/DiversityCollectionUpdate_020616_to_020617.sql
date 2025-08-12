declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.16'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   CollTransactionType_Enum: new type regulation  #############################################################
--#####################################################################################################################

If (select count(*) from [CollTransactionType_Enum] where Code = 'regulation') = 0
begin
INSERT INTO [dbo].[CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('regulation'
           ,'a regulation, provisio or stipulation e.g. in the context of the Nagoya protocoll'
           ,'regulation'
           ,1)
end
GO



--#####################################################################################################################
--######   CollectionSpecimenTransaction: new column TransactionTitle for validation  #################################
--#####################################################################################################################


ALTER TABLE [dbo].[CollectionSpecimenTransaction] ADD [TransactionTitle] [nvarchar](200) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title as in related table Transaction. Used for validation of correct entry of transaction with type regulation (see insert trigger)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenTransaction', @level2type=N'COLUMN',@level2name=N'TransactionTitle'
GO

ALTER TABLE [dbo].[CollectionSpecimenTransaction_log] ADD [TransactionTitle] [nvarchar](200) NULL;
GO

--#####################################################################################################################
--######   CollectionSpecimenTransaction: trgInsCollectionSpecimenTransaction for validation  #########################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgInsCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR INSERT AS
if (select count(*) from inserted I, [Transaction] T 
where I.TransactionID = T.TransactionID 
and T.TransactionType = 'regulation') = 1
begin
	if (select count(*) from inserted I, [Transaction] T 
	where I.TransactionID = T.TransactionID 
	and I.TransactionTitle = T.TransactionTitle
	and T.TransactionType = 'regulation') = 0
	begin
		rollback tran
	end
end
GO

--#####################################################################################################################
--######   CollectionSpecimenTransaction: trgDelCollectionSpecimenTransaction  ########################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  altered for inclusion of TransactionTitle */ 
/*  Date: 1/2/2019  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenTransaction_Log (AccessionNumber, TransactionTitle, CollectionSpecimenID, IsOnLoan, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, SpecimenPartID, TransactionID, TransactionReturnID,  LogState) 
SELECT deleted.AccessionNumber, deleted.TransactionTitle, deleted.CollectionSpecimenID, deleted.IsOnLoan, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID, deleted.SpecimenPartID, deleted.TransactionID, deleted.TransactionReturnID,  'D'
FROM DELETED
GO

--#####################################################################################################################
--######   CollectionSpecimenTransaction: trgDelCollectionSpecimenTransaction  ########################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  altered for inclusion of TransactionTitle */ 
/*  Date: 1/2/2019  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenTransaction_Log (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, AccessionNumber, TransactionTitle, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState, TransactionReturnID) 
SELECT deleted.CollectionSpecimenID, deleted.TransactionID, deleted.SpecimenPartID, deleted.IsOnLoan, deleted.AccessionNumber, deleted.TransactionTitle, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U', deleted.TransactionReturnID
FROM DELETED
Update CollectionSpecimenTransaction
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionSpecimenTransaction, deleted 
where 1 = 1 
AND CollectionSpecimenTransaction.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenTransaction.TransactionID = deleted.TransactionID
AND CollectionSpecimenTransaction.SpecimenPartID = deleted.SpecimenPartID
GO


--#####################################################################################################################
--######   TransactionRegulation - access to regluations for user  ####################################################
--#####################################################################################################################

CREATE VIEW [dbo].[TransactionRegulation]
AS
SELECT        TransactionTitle, TransactionID
FROM            dbo.[Transaction]
WHERE        (TransactionType IN (N'regulation'))
GO

GRANT SELECT ON [TransactionRegulation] TO [USER]
GO


--#####################################################################################################################
--######   TransactionPermit - access to permits for user  ############################################################
--#####################################################################################################################

CREATE VIEW [dbo].[TransactionPermit]
AS
SELECT        TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, MaterialCollectors, MaterialSource, FromTransactionPartnerName, FromTransactionNumber, 
                         ToTransactionPartnerName, ToTransactionNumber, NumberOfUnits, TransactionComment, BeginDate, AgreedEndDate, DateSupplement, InternalNotes, ToRecipient, ResponsibleName, TransactionID
FROM            dbo.[Transaction]
WHERE        (TransactionType IN (N'permit'))
GO

GRANT SELECT ON [TransactionPermit] TO [USER]
GO


--#####################################################################################################################
--######   TransactionList_H7 - optimizing access to transaction including hierarchy  #################################
--#####################################################################################################################

CREATE VIEW [dbo].[TransactionList_H7]
AS

SELECT        TransactionID, ParentTransactionID, TransactionType, TransactionTitle, 
TransactionTitle AS HierarchyDisplayText,
ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, MaterialCategory, MaterialCollectors, FromCollectionID, 
                         FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, 
                         Investigator, TransactionComment, BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
FROM            dbo.[Transaction]
WHERE        (AdministratingCollectionID IS NULL)
UNION
SELECT    T .TransactionID, T .ParentTransactionID, T .TransactionType, 
T .TransactionTitle,
case when T7.TransactionTitle is null then '' else T7.TransactionTitle + ' | ' end +
case when T6.TransactionTitle is null then '' else T6.TransactionTitle + ' | ' end +
case when T5.TransactionTitle is null then '' else T5.TransactionTitle + ' | ' end +
case when T4.TransactionTitle is null then '' else T4.TransactionTitle + ' | ' end +
case when T3.TransactionTitle is null then '' else T3.TransactionTitle + ' | ' end + 
case when T2.TransactionTitle is null then '' else T2.TransactionTitle + ' | ' end + 
case when T1.TransactionTitle is null then '' else T1.TransactionTitle + ' | ' end +
T .TransactionTitle 
AS HierarchyDisplayText, 
T .ReportingCategory, T .AdministratingCollectionID, T .MaterialDescription, T .MaterialSource, T .MaterialCategory, T .MaterialCollectors, 
                         T .FromCollectionID, T .FromTransactionPartnerName, T .FromTransactionPartnerAgentURI, T .FromTransactionNumber, T .ToCollectionID, T .ToTransactionPartnerName, T .ToTransactionPartnerAgentURI, 
                         T .ToTransactionNumber, T .ToRecipient, T .NumberOfUnits, T .Investigator, T .TransactionComment, T .BeginDate, T .AgreedEndDate, T .ActualEndDate, T .InternalNotes, T .ResponsibleName, T .ResponsibleAgentURI, 
                         T .DateSupplement
FROM            dbo.[Transaction] AS T INNER JOIN
                         dbo.CollectionManager ON T.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
left outer join 
dbo.[Transaction] AS T1 ON T.ParentTransactionID = T1.TransactionID and T1.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
left outer join 
dbo.[Transaction] AS T2 ON T1.ParentTransactionID = T2.TransactionID and T2.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
left outer join 
dbo.[Transaction] AS T3 ON T2.ParentTransactionID = T3.TransactionID and T3.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
left outer join 
dbo.[Transaction] AS T4 ON T3.TransactionID = T4.ParentTransactionID and T4.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
left outer join 
dbo.[Transaction] AS T5 ON T4.TransactionID = T5.ParentTransactionID and T5.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
left outer join 
dbo.[Transaction] AS T6 ON T5.TransactionID = T6.ParentTransactionID and T6.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
left outer join 
dbo.[Transaction] AS T7 ON T6.TransactionID = T7.ParentTransactionID and T7.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()        
GO

GRANT SELECT ON [dbo].[TransactionList_H7] TO [User]
GO


--#####################################################################################################################
--######   trgUpdCollectionSpecimen - writing date into LastChanges in ProjectProxy   #################################
--#####################################################################################################################
ALTER TRIGGER [dbo].[trgUpdCollectionSpecimen] ON [dbo].[CollectionSpecimen] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 03.04.2012  */ 

if not update(Version) 
BEGIN

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
END 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimen_Log (CollectionSpecimenID, Version, CollectionEventID, CollectionID, AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, ReferenceTitle, ReferenceURI, Problems, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, InternalNotes, ExternalDatasourceID, ExternalIdentifier, RowGUID, ReferenceDetails,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Version, deleted.CollectionEventID, deleted.CollectionID, deleted.AccessionNumber, deleted.AccessionDate, deleted.AccessionDay, deleted.AccessionMonth, deleted.AccessionYear, deleted.AccessionDateSupplement, deleted.AccessionDateCategory, deleted.DepositorsName, deleted.DepositorsAgentURI, deleted.DepositorsAccessionNumber, deleted.LabelTitle, deleted.LabelType, deleted.LabelTranscriptionState, deleted.LabelTranscriptionNotes, deleted.ExsiccataURI, deleted.ExsiccataAbbreviation, deleted.OriginalNotes, deleted.AdditionalNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.Problems, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.InternalNotes, deleted.ExternalDatasourceID, deleted.ExternalIdentifier, deleted.RowGUID, deleted.ReferenceDetails,  'U'
FROM DELETED

END

/* updating the logging columns */
Update CollectionSpecimen
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionSpecimen, deleted 
where 1 = 1 
AND CollectionSpecimen.CollectionSpecimenID = deleted.CollectionSpecimenID

/* setting the date in ProjectProxy if the DataWithholdingReason is not filled or kept filled */ 
Update P 
set P.LastChanges = getdate()
FROM CollectionProject C, ProjectProxy P, deleted D, inserted I
where C.CollectionSpecimenID = @ID
AND C.ProjectID = P.ProjectID
AND C.CollectionSpecimenID = D.CollectionSpecimenID
AND (
((I.DataWithholdingReason IS NULL OR I.DataWithholdingReason = '') AND D.DataWithholdingReason <> '') 
OR
((D.DataWithholdingReason IS NULL OR D.DataWithholdingReason = '') AND I.DataWithholdingReason <> '') 
OR
((D.DataWithholdingReason IS NULL OR D.DataWithholdingReason = '') AND (I.DataWithholdingReason IS NULL OR I.DataWithholdingReason = '')) 
)


--#####################################################################################################################
--######   ApplicationSearchSelectionStrings - adaption to DSGVO   ####################################################
--#####################################################################################################################

update a set [UserName] = u.ID
  FROM [ApplicationSearchSelectionStrings] a, UserProxy U
  where U.LoginName = A.UserName
GO

--#####################################################################################################################
--######   ApplicationSearchSelectionStrings_Core - adaption to DSGVO   ###############################################
--#####################################################################################################################

ALTER VIEW [dbo].[ApplicationSearchSelectionStrings_Core]
AS
SELECT        SQLStringIdentifier, ItemTable, SQLString, Description
FROM            dbo.ApplicationSearchSelectionStrings
WHERE        (UserName = dbo.UserID())
GO



--#####################################################################################################################
--######   REVOKE SELECT on log tables from role User   ###############################################################
--#####################################################################################################################


REVOKE SELECT ON [dbo].[Method_log] TO [User] AS [dbo]
GO
REVOKE SELECT ON [dbo].[CollectionEventMethod_log] TO [User]
GO
REVOKE SELECT ON [dbo].[CollectionImage_log] TO [User]
GO
REVOKE SELECT ON [dbo].[IdentificationUnitGeoAnalysis_log] TO [User]
GO
REVOKE SELECT ON [dbo].[Method_log] TO [User]
GO
REVOKE SELECT ON [dbo].[Tool_log] TO [User]
GO


--#####################################################################################################################
--######   CollectionChildNodes optimized   ###########################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[CollectionChildNodes] (@ID int)  
RETURNS @ItemList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 03.12.2018
Test
select * from dbo.CollectionChildNodes(1)
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL)
INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]
	FROM Collection WHERE CollectionParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type])
		select C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.CollectionOwner, C.DisplayOrder, C.[Type]
		from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem)
		set @i = (select count(*) from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	end

 --  DECLARE HierarchyCursor  CURSOR for
 --  select CollectionID from @TempItem
 --  open HierarchyCursor
 --  FETCH next from HierarchyCursor into @ParentID
 --  WHILE @@FETCH_STATUS = 0
 --  BEGIN
	--insert into @TempItem select CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]
	--from dbo.CollectionChildNodes (@ParentID) where CollectionID not in (select CollectionID from @TempItem)
 --  	FETCH NEXT FROM HierarchyCursor into @ParentID
 --  END
 --  CLOSE HierarchyCursor
 --  DEALLOCATE HierarchyCursor

 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]
   FROM @TempItem ORDER BY CollectionName
   RETURN
END
GO



--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.09.00' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.17'
END

GO


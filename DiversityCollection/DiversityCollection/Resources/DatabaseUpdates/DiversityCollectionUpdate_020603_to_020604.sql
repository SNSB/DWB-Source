declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.03'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  TransactionChildNodes - missing children included (in @IDs)   ###############################################
--#####################################################################################################################

ALTER  FUNCTION [dbo].[TransactionChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
	[TransactionID] [int] primary key NOT NULL,
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
	[NumberOfUnits] [int] NULL,
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
MW 12.12.2016: NumberOfUnits -> int
select * FROM dbo.[TransactionChildNodes](3918)
*/
AS
BEGIN

declare @IDs TABLE (
	[TransactionID] [int] primary key NOT NULL,
	[ParentTransactionID] [int] NULL)

   --declare @ParentID int

   insert into @IDs(TransactionID, ParentTransactionID) 
   Select distinct T.TransactionID, t.ParentTransactionID 
   from [Transaction] T 
   where t.ParentTransactionID = @ID
   and T.TransactionID <> @ID
   and T.TransactionID <> T.ParentTransactionID

   declare @i int
   set @i = (select count(*) from @IDs I where I.ParentTransactionID not in (select TransactionID from @IDs))
   while @i > 0
   begin
		insert into @IDs(TransactionID, ParentTransactionID) 
			Select distinct T.TransactionID, t.ParentTransactionID 
			from [Transaction] T, @IDs I where t.TransactionID = I.ParentTransactionID
			and T.TransactionID not in (select TransactionID from @IDs)
		insert into @IDs(TransactionID, ParentTransactionID) 
			Select distinct T.TransactionID, t.ParentTransactionID 
			from [Transaction] T, @IDs I where t.ParentTransactionID = I.TransactionID
			and T.TransactionID not in (select TransactionID from @IDs)
		set @i = (select count(*) from @IDs I where I.ParentTransactionID not in (select TransactionID from @IDs))
   end

INSERT @ItemList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
	MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
	ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
	BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement) 
SELECT distinct T.TransactionID, T.ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
	MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
	ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
	BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
   FROM @IDs I, [Transaction] T 
   where I.TransactionID = T.TransactionID
   ORDER BY TransactionTitle

   RETURN
END

GO


--#####################################################################################################################
--######  CollectionSpecimenPartRegulation: Add Responsible    ########################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenPartRegulation ADD ResponsibleName nvarchar(500) NULL;
GO

ALTER TABLE CollectionSpecimenPartRegulation ADD ResponsibleAgentURI nvarchar(500) NULL;
GO


--#####################################################################################################################
--######  StableIdentifier: Use StableIdentifierBase if present    ####################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[StableIdentifier] (@ProjectID int, @CollectionSpecimenID int, @IdentificationUnitID int, @SpecimenPartID int)
RETURNS varchar (500)
/*
Returns a stable identfier for a dataset.
Relies on an entry in ProjectProxy
*/
AS
BEGIN
declare @StableIdentifierBase varchar(500)
if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_TYPE = 'FUNCTION'
and r.ROUTINE_NAME = 'StableIdentifierBase') = 1
begin 
	set @StableIdentifierBase = (SELECT dbo.[StableIdentifierBase]())
end
else 
begin
	set @StableIdentifierBase = NULL
end
set @StableIdentifierBase = (SELECT case when [StableIdentifierBase] is null or [StableIdentifierBase] = ''
	then NULL -- dbo.BaseURL() -- former version used BaseURL as a default
	else [StableIdentifierBase]
	end
  FROM [dbo].[ProjectProxy]
p where p.ProjectID = @ProjectID)
if (@StableIdentifierBase not like '%/')
begin
	set @StableIdentifierBase = @StableIdentifierBase + '/'
end
declare @StableIdentifier varchar(500)
set @StableIdentifier = @StableIdentifierBase + cast(@CollectionSpecimenID as varchar) 
if (@IdentificationUnitID IS NOT NULL)
begin
	set @StableIdentifier = @StableIdentifier + '/' + cast(@IdentificationUnitID as varchar) 
	if (@SpecimenPartID IS NOT NULL)
	begin
		set @StableIdentifier = @StableIdentifier + '/' + cast(@SpecimenPartID as varchar) 
	end
end
return @StableIdentifier
END


GO

--#####################################################################################################################
--######   Rename EUNIS    ############################################################################################
--#####################################################################################################################

UPDATE P 
set [DisplayText] = 'EUNIS 2003 (European Nature Information System)'
from [dbo].[Property] P
where P.PropertyID = 1
GO

--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.08' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.04'
END

GO


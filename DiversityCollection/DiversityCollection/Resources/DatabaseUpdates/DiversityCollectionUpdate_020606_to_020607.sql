declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.06'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  TransactionChildNodes - more missing children included (in @IDs)   ##########################################
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
		set @i = (select count(*) from @IDs I, [Transaction] T where I.TransactionID = T.ParentTransactionID and T.TransactionID not in (select TransactionID from @IDs))
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
--#####################################################################################################################
--######  Delete trigger - ensure transfer in logtables for cascading deletes   #######################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  NextFreeAccNumber: Bugfix getting the prefix    #############################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[NextFreeAccNumber] (@AccessionNumber nvarchar(50), @IncludeSpecimen bit, @IncludePart bit)  
/*
returns next free accession number for parts or specimen
optionally including either parts or specimen
assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453
with a leading string and a numeric end
MW 05.09.2013
TEST:
select dbo.[NextFreeAccNumber] ('0033933', 1, 1)
select dbo.[NextFreeAccNumber] ('0033933', 0, 1)
select dbo.[NextFreeAccNumber] ('00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 0, 1)
select dbo.[NextFreeAccNumber] ('ZSM_DIP-000', 1, 1)
select dbo.[NextFreeAccNumber] ('1907/9', 1, 1)
select dbo.[NextFreeAccNumber] ('ZSM-MA-9', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0013622', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014900', 1, 1)
select dbo.[NextFreeAccNumber] ('MB-FA-000001', 1, 1) 
select dbo.[NextFreeAccNumber] ('MB-FA-000101', 1, 1) 
select dbo.[NextFreeAccNumber] ('SMNS-B-PH-2017/942', 1, 1) 
*/
RETURNS nvarchar (50)
AS
BEGIN 
declare @NextAcc nvarchar(50)
set @NextAcc = ''
declare @Start int
declare @Position tinyint
declare @Prefix nvarchar(50)
set @Position = len(@AccessionNumber) 
if (isnumeric(@AccessionNumber) = 1)
begin
	set @Prefix = substring(@AccessionNumber, 1, len(@AccessionNumber) - len(cast(cast(@AccessionNumber as int) as varchar)))
	set @Start = cast(@AccessionNumber as int)
end
else
begin
while (substring(@AccessionNumber, @Position, 1) LIKE '[0-9]')--(isnumeric(rtrim(substring(@AccessionNumber, @Position, len(@AccessionNumber)))) = 1)
begin
	set @Start = CAST(substring(@AccessionNumber, @Position, len(@AccessionNumber)) as int)
	set @Position = @Position - 1
	set @Prefix = substring(@AccessionNumber, 1, @Position)
end
end
if (@Start < 0) 
begin 
	set @Start = @Start * -1;
end
declare @Space nvarchar(1)
set @Space = ''
if (SUBSTRING(@AccessionNumber, @Position + 1, 1) = ' ')
begin
	set @Space = '_'
end
if (LEN(@Prefix) = LEN(@AccessionNumber))
begin
	set @Prefix = SUBSTRING(@Prefix, 1, len(@Prefix) - 1)
end
declare @T Table (ID int identity(1, 1),
	NumericPart int NULL,
    AccessionNumber nvarchar(50) NULL)
if (@IncludeSpecimen = 1)
begin    
INSERT INTO @T (AccessionNumber)
SELECT AccessionNumber 
FROM CollectionSpecimen  
WHERE AccessionNumber LIKE @Prefix + '%'
AND AccessionNumber >= @AccessionNumber
end
if (@IncludePart = 1)
begin    
INSERT INTO @T (AccessionNumber)
SELECT AccessionNumber 
FROM CollectionSpecimenPart  
WHERE AccessionNumber LIKE @Prefix + '%'
AND AccessionNumber >= @AccessionNumber
end
if (select COUNT(*) from @T) = 0
begin
INSERT INTO @T (AccessionNumber)
SELECT @AccessionNumber 
end
UPDATE @T SET NumericPart = ID + @Start;
UPDATE @T SET AccessionNumber = @Prefix 
+ case when (len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) > 0 
then replicate('0', len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) else '' end
+ CAST(NumericPart as varchar);
if (@IncludeSpecimen = 1)
begin
	Delete T from @T T inner join  CollectionSpecimen S on S.AccessionNumber = T.AccessionNumber
	where not S.AccessionNumber is null
end	
if (@IncludePart = 1)
begin
	Delete T from @T T inner join  CollectionSpecimenPart S on S.AccessionNumber = T.AccessionNumber
	where not S.AccessionNumber is null
end	
set @NextAcc = (SELECT MIN(T.AccessionNumber) from @T T)
return (@NextAcc)
END
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
RETURN '02.06.07'
END

GO


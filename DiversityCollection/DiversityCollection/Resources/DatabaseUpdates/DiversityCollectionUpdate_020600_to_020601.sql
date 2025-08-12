declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.00'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   sp_TransactionHierarchyAll   ###############################################################################
--######   replaces function TransactionHierarchyAll ##################################################################
--#####################################################################################################################


CREATE  PROCEDURE [dbo].[sp_TransactionHierarchyAll] (@Mode int)
AS
/*
Returns a table that lists all the transactions items related to the given analysis.
MW 29.12.2016
@Mode 0: Only content of original table
@Mode 1: Including a NULL value
TEST:
exec DBO.sp_TransactionHierarchyAll 0
exec DBO.sp_TransactionHierarchyAll 1
*/

begin try
CREATE TABLE #TransactionList ([TransactionID] [int] Primary key ,
	[ParentTransactionID] [int] NULL,
	[DisplayText] [nvarchar](500))

	INSERT #TransactionList (
	TransactionID, ParentTransactionID, DisplayText)
	SELECT 
	TransactionID, ParentTransactionID, TransactionTitle
	FROM [Transaction]
	CREATE NONCLUSTERED INDEX [IdxTransactionList_ParentTransactionID] ON #TransactionList
	(
		[ParentTransactionID] ASC
	)
	while (select count(*) from #TransactionList L where not L.ParentTransactionID is null) > 0
	begin
		update L set L.DisplayText = T.TransactionTitle + ' | ' + L.DisplayText, L.ParentTransactionID = T.ParentTransactionID
		from #TransactionList L, [Transaction] T
		where L.ParentTransactionID = T.TransactionID
	end
end try
begin catch
end catch
if @Mode = 0
begin
	select T.TransactionID, T.ParentTransactionID, T.TransactionType, T.TransactionTitle, T.ReportingCategory, T.AdministratingCollectionID, T.MaterialDescription, T.MaterialSource, T.MaterialCategory, 
	T.MaterialCollectors, T.FromCollectionID, T.FromTransactionPartnerName, T.FromTransactionPartnerAgentURI, T.FromTransactionNumber, T.ToCollectionID, 
	T.ToTransactionPartnerName, T.ToTransactionPartnerAgentURI, T.ToRecipient, T.ToTransactionNumber, T.NumberOfUnits, T.Investigator, T.TransactionComment, T.BeginDate, T.AgreedEndDate, 
	T.ActualEndDate, T.InternalNotes, T.ResponsibleName, T.ResponsibleAgentURI, L.DisplayText
	from #TransactionList L, [Transaction] T 
	where L.TransactionID = T.TransactionID
end
else if @Mode = 1
begin
	select NULL AS TransactionID, NULL AS ParentTransactionID, NULL AS TransactionType, NULL AS TransactionTitle, NULL AS ReportingCategory, NULL AS AdministratingCollectionID, NULL AS MaterialDescription, NULL AS MaterialSource, NULL AS MaterialCategory, 
	NULL AS MaterialCollectors, NULL AS FromCollectionID, NULL AS FromTransactionPartnerName, NULL AS FromTransactionPartnerAgentURI, NULL AS FromTransactionNumber, NULL AS ToCollectionID, 
	NULL AS ToTransactionPartnerName, NULL AS ToTransactionPartnerAgentURI, NULL AS ToRecipient, NULL AS ToTransactionNumber, NULL AS NumberOfUnits, NULL AS Investigator, NULL AS TransactionComment, NULL AS BeginDate, NULL AS AgreedEndDate, 
	NULL AS ActualEndDate, NULL AS InternalNotes, NULL AS ResponsibleName, NULL AS ResponsibleAgentURI, NULL AS DisplayText
	union
	select T.TransactionID, T.ParentTransactionID, T.TransactionType, T.TransactionTitle, T.ReportingCategory, T.AdministratingCollectionID, T.MaterialDescription, T.MaterialSource, T.MaterialCategory, 
	T.MaterialCollectors, T.FromCollectionID, T.FromTransactionPartnerName, T.FromTransactionPartnerAgentURI, T.FromTransactionNumber, T.ToCollectionID, 
	T.ToTransactionPartnerName, T.ToTransactionPartnerAgentURI, T.ToRecipient, T.ToTransactionNumber, T.NumberOfUnits, T.Investigator, T.TransactionComment, T.BeginDate, T.AgreedEndDate, 
	T.ActualEndDate, T.InternalNotes, T.ResponsibleName, T.ResponsibleAgentURI, L.DisplayText
	from #TransactionList L, [Transaction] T 
	where L.TransactionID = T.TransactionID
end

drop index [IdxTransactionList_ParentTransactionID] ON #TransactionList;
drop table #TransactionList;

GO

GRANT exec on [dbo].[sp_TransactionHierarchyAll] to [User]
GO


--#####################################################################################################################
--######   CollectionSpecimenPartDescription    #######################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.[CollectionSpecimenPartDescription] TO [User];
GRANT INSERT ON dbo.[CollectionSpecimenPartDescription] TO [Editor];
GRANT UPDATE ON dbo.[CollectionSpecimenPartDescription] TO [Editor];
GRANT DELETE ON dbo.[CollectionSpecimenPartDescription] TO [Editor];
GRANT SELECT ON dbo.[CollectionSpecimenPartDescription_log] TO [Editor];
GRANT INSERT ON dbo.[CollectionSpecimenPartDescription_log] TO [Editor];
GO

--#####################################################################################################################
--######  TransactionHierarchy     ####################################################################################
--#####################################################################################################################

ALTER  FUNCTION [dbo].[TransactionHierarchy] (@TransactionID int)  
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
Returns a table that lists all the transactions related to the given transaction.
MW 12.12.2016: NumberOfUnits -> int
SELECT  * FROM dbo.TransactionHierarchy(3918)
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
		set @i = (select count(*) from [TransactionList] where TransactionID = @TransactionID and not ParentTransactionID is null and ParentTransactionID <> @TransactionID)
		end
	set @TopID = @TransactionID
	end
INSERT @ItemList
SELECT Distinct TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, 
  MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, 
  ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, 
  BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
FROM [TransactionList]
WHERE TransactionID = @TopID
INSERT @ItemList
SELECT * FROM dbo.TransactionChildNodes (@TopID) WHERE TransactionID <> @TopID
   RETURN
END

GO

--#####################################################################################################################
--######  TransactionChildNodes    ####################################################################################
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
--######  TransactionHierarchyAccess     ##############################################################################
--#####################################################################################################################

ALTER  FUNCTION [dbo].[TransactionHierarchyAccess] (@TransactionID int)  
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
	[NumberOfUnits] [int] NULL,
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
MW 12.12.2016: NumberOfUnits -> int
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


--#####################################################################################################################
--######  TransactionChildNodesAccess     #############################################################################
--#####################################################################################################################


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
	[NumberOfUnits] [int] NULL,
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
MW 12.12.2016: NumberOfUnits -> int
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
	[NumberOfUnits] [int] NULL,
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
--######  CacheAdmin inherits grants of CacheUser  ####################################################################
--#####################################################################################################################

EXEC sp_addrolemember N'CacheUser', N'CacheAdmin';
GO

EXEC sp_addrolemember N'CacheAdmin', N'Administrator';
GO




--#####################################################################################################################
--######   trgUpdCollectionEventLocalisation      #####################################################################
--#####################################################################################################################
 
ALTER TRIGGER [dbo].[trgUpdCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR UPDATE AS
 /*  
 Changed: 01.12.2015 - sysuser instead of user and DoUpdate for check if there are real changes 
 Changed: 29.12.2016 - U for LogState (was D)
 */
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
DECLARE @DoUpdate int
set @DoUpdate = 0

SET @DoUpdate = (SELECT count(*)
FROM DELETED D, INSERTED I
WHERE D.CollectionEventID = I.CollectionEventID AND I.LocalisationSystemID = D.LocalisationSystemID
AND (
(I.Location1 <> D.Location1 OR I.Location1 IS NULL AND NOT D.Location1 IS NULL OR NOT I.Location1 IS NULL AND D.Location1 IS NULL)
OR (I.Location2 <> D.Location2 OR I.Location2 IS NULL AND NOT D.Location2 IS NULL OR NOT I.Location2 IS NULL AND D.Location2 IS NULL)
OR (I.LocationAccuracy <> D.LocationAccuracy OR I.LocationAccuracy IS NULL AND NOT D.LocationAccuracy IS NULL OR NOT I.LocationAccuracy IS NULL AND D.LocationAccuracy IS NULL)
OR (I.LocationNotes <> D.LocationNotes OR I.LocationNotes IS NULL AND NOT D.LocationNotes IS NULL OR NOT I.LocationNotes IS NULL AND D.LocationNotes IS NULL)
OR (I.DeterminationDate <> D.DeterminationDate OR I.DeterminationDate IS NULL AND NOT D.DeterminationDate IS NULL OR NOT I.DeterminationDate IS NULL AND D.DeterminationDate IS NULL)
OR (I.DistanceToLocation <> D.DistanceToLocation OR I.DistanceToLocation IS NULL AND NOT D.DistanceToLocation IS NULL OR NOT I.DistanceToLocation IS NULL AND D.DistanceToLocation IS NULL)
OR (I.DirectionToLocation <> D.DirectionToLocation OR I.DirectionToLocation IS NULL AND NOT D.DirectionToLocation IS NULL OR NOT I.DirectionToLocation IS NULL AND D.DirectionToLocation IS NULL)
OR (I.ResponsibleName <> D.ResponsibleName OR I.ResponsibleName IS NULL AND NOT D.ResponsibleName IS NULL OR NOT I.ResponsibleName IS NULL AND D.ResponsibleName IS NULL)
OR (I.ResponsibleAgentURI <> D.ResponsibleAgentURI OR I.ResponsibleAgentURI IS NULL AND NOT D.ResponsibleAgentURI IS NULL OR NOT I.ResponsibleAgentURI IS NULL AND D.ResponsibleAgentURI IS NULL)
OR (I.RecordingMethod <> D.RecordingMethod OR I.RecordingMethod IS NULL AND NOT D.RecordingMethod IS NULL OR NOT I.RecordingMethod IS NULL AND D.RecordingMethod IS NULL)
OR (I.Geography.ToString() <> D.Geography.ToString() OR I.Geography IS NULL AND NOT D.Geography IS NULL OR NOT I.Geography IS NULL AND D.Geography IS NULL)
OR (I.AverageAltitudeCache <> D.AverageAltitudeCache OR I.AverageAltitudeCache IS NULL AND NOT D.AverageAltitudeCache IS NULL OR NOT I.AverageAltitudeCache IS NULL AND D.AverageAltitudeCache IS NULL)
OR (I.AverageLatitudeCache <> D.AverageLatitudeCache OR I.AverageLatitudeCache IS NULL AND NOT D.AverageLatitudeCache IS NULL OR NOT I.AverageLatitudeCache IS NULL AND D.AverageLatitudeCache IS NULL)
OR (I.AverageLongitudeCache <> D.AverageLongitudeCache OR I.AverageLongitudeCache IS NULL AND NOT D.AverageLongitudeCache IS NULL OR NOT I.AverageLongitudeCache IS NULL AND D.AverageLongitudeCache IS NULL)
))

set @i = (select count(*) from deleted) 
if @i = 1 AND @DoUpdate > 0
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM deleted)
   EXECUTE procSetVersionCollectionEvent @ID
   SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)
END 
if (not @Version is null) 
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, 
LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, 
ResponsibleAgentURI, RecordingMethod, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion,  LogState) 
SELECT D.CollectionEventID, D.LocalisationSystemID, D.Location1, D.Location2, D.LocationAccuracy, D.LocationNotes, 
D.DeterminationDate, D.DistanceToLocation, D.DirectionToLocation, D.ResponsibleName, D.ResponsibleAgentURI, D.RecordingMethod, 
D.Geography, D.AverageAltitudeCache, D.AverageLatitudeCache, D.AverageLongitudeCache, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, 
D.LogUpdatedWhen, D.LogUpdatedBy,  @Version,  'U'
FROM DELETED D, INSERTED I
WHERE D.CollectionEventID = I.CollectionEventID AND I.LocalisationSystemID = D.LocalisationSystemID
AND (
(I.Location1 <> D.Location1 OR I.Location1 IS NULL AND NOT D.Location1 IS NULL OR NOT I.Location1 IS NULL AND D.Location1 IS NULL)
OR (I.Location2 <> D.Location2 OR I.Location2 IS NULL AND NOT D.Location2 IS NULL OR NOT I.Location2 IS NULL AND D.Location2 IS NULL)
OR (I.LocationAccuracy <> D.LocationAccuracy OR I.LocationAccuracy IS NULL AND NOT D.LocationAccuracy IS NULL OR NOT I.LocationAccuracy IS NULL AND D.LocationAccuracy IS NULL)
OR (I.LocationNotes <> D.LocationNotes OR I.LocationNotes IS NULL AND NOT D.LocationNotes IS NULL OR NOT I.LocationNotes IS NULL AND D.LocationNotes IS NULL)
OR (I.DeterminationDate <> D.DeterminationDate OR I.DeterminationDate IS NULL AND NOT D.DeterminationDate IS NULL OR NOT I.DeterminationDate IS NULL AND D.DeterminationDate IS NULL)
OR (I.DistanceToLocation <> D.DistanceToLocation OR I.DistanceToLocation IS NULL AND NOT D.DistanceToLocation IS NULL OR NOT I.DistanceToLocation IS NULL AND D.DistanceToLocation IS NULL)
OR (I.DirectionToLocation <> D.DirectionToLocation OR I.DirectionToLocation IS NULL AND NOT D.DirectionToLocation IS NULL OR NOT I.DirectionToLocation IS NULL AND D.DirectionToLocation IS NULL)
OR (I.ResponsibleName <> D.ResponsibleName OR I.ResponsibleName IS NULL AND NOT D.ResponsibleName IS NULL OR NOT I.ResponsibleName IS NULL AND D.ResponsibleName IS NULL)
OR (I.ResponsibleAgentURI <> D.ResponsibleAgentURI OR I.ResponsibleAgentURI IS NULL AND NOT D.ResponsibleAgentURI IS NULL OR NOT I.ResponsibleAgentURI IS NULL AND D.ResponsibleAgentURI IS NULL)
OR (I.RecordingMethod <> D.RecordingMethod OR I.RecordingMethod IS NULL AND NOT D.RecordingMethod IS NULL OR NOT I.RecordingMethod IS NULL AND D.RecordingMethod IS NULL)
OR (I.Geography.ToString() <> D.Geography.ToString() OR I.Geography IS NULL AND NOT D.Geography IS NULL OR NOT I.Geography IS NULL AND D.Geography IS NULL)
OR (I.AverageAltitudeCache <> D.AverageAltitudeCache OR I.AverageAltitudeCache IS NULL AND NOT D.AverageAltitudeCache IS NULL OR NOT I.AverageAltitudeCache IS NULL AND D.AverageAltitudeCache IS NULL)
OR (I.AverageLatitudeCache <> D.AverageLatitudeCache OR I.AverageLatitudeCache IS NULL AND NOT D.AverageLatitudeCache IS NULL OR NOT I.AverageLatitudeCache IS NULL AND D.AverageLatitudeCache IS NULL)
OR (I.AverageLongitudeCache <> D.AverageLongitudeCache OR I.AverageLongitudeCache IS NULL AND NOT D.AverageLongitudeCache IS NULL OR NOT I.AverageLongitudeCache IS NULL AND D.AverageLongitudeCache IS NULL)
)
end
else
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, 
DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, RecordingMethod, 
Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion, LogState) 
SELECT D.CollectionEventID, D.LocalisationSystemID, D.Location1, D.Location2, D.LocationAccuracy, D.LocationNotes, 
D.DeterminationDate, D.DistanceToLocation, D.DirectionToLocation, D.ResponsibleName, D.ResponsibleAgentURI, D.RecordingMethod, 
D.Geography, D.AverageAltitudeCache, D.AverageLatitudeCache, D.AverageLongitudeCache, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, 
D.LogUpdatedWhen, D.LogUpdatedBy, E.Version, 'U' 
FROM DELETED D, CollectionEvent E
WHERE D.CollectionEventID = E.CollectionEventID
end

if (@DoUpdate > 0)
begin

	Update L
	set LogUpdatedWhen = getdate(), LogUpdatedBy = suser_sname()
	FROM CollectionEventLocalisation L, deleted D
	where 1 = 1 
	AND L.CollectionEventID = D.CollectionEventID
	AND L.LocalisationSystemID = D.LocalisationSystemID

	Update L
	set Geography = 
	case when I.Geography IS null then 
		geography::STPointFromText('POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+' ' +replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')', 4326)
	else I.Geography end
	FROM CollectionEventLocalisation L, inserted I
	where 1 = 1 
	AND L.CollectionEventID = I.CollectionEventID
	AND L.LocalisationSystemID = I.LocalisationSystemID
	AND I.AverageLatitudeCache between -90 and 90
	AND I.AverageLongitudeCache between -180 and 180
	AND (L.Geography.ToString() <> I.Geography.ToString()
	OR L.Geography IS NULL)

	Update L
	set AverageLatitudeCache = I.Geography.EnvelopeCenter().Lat,
	AverageLongitudeCache = I.Geography.EnvelopeCenter().Long
	FROM CollectionEventLocalisation L, inserted I
	where 1 = 1 
	AND L.CollectionEventID = I.CollectionEventID
	AND L.LocalisationSystemID = I.LocalisationSystemID
	AND I.AverageLatitudeCache IS NULL
	AND I.AverageLongitudeCache IS NULL
	AND L.AverageLatitudeCache IS NULL
	AND L.AverageLongitudeCache IS NULL
	AND NOT I.Geography IS NULL


	Update L
	set Geography = geography::STPointFromText('POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+' ' +replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')', 4326)
	FROM CollectionEventLocalisation L, inserted I
	where 1 = 1 
	AND L.CollectionEventID = I.CollectionEventID
	AND L.LocalisationSystemID = I.LocalisationSystemID
	AND I.AverageLatitudeCache between -90 and 90
	AND I.AverageLongitudeCache between -180 and 360
	AND (L.Geography.ToString() LIKE 'POINT%')
	AND @i = 1
end

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
RETURN '02.06.01'
END

GO


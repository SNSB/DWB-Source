declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.34'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   EventSeriesChildNodes - Bugfix and optimizing   ############################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[EventSeriesChildNodes] (@ID int)  
RETURNS @ItemList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (500)  NULL ,
   Notes nvarchar (500) ,
   [Geography] geography)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 19.10.2022
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (500)  NULL ,
   Notes nvarchar (500)  ,
   [Geography] geography)

-- insert the first childs into the table
 INSERT @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes, [Geography]) 
	SELECT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes, [Geography]
	FROM CollectionEventSeries WHERE SeriesParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, CollectionEventSeries C where C.SeriesParentID = T.SeriesID and C.SeriesID not in (select SeriesID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes, [Geography])
		select S.SeriesID, S.SeriesParentID, S.DateStart, S.DateEnd, S.SeriesCode, S.[Description],  S.Notes, S.[Geography]
		from @TempItem T, CollectionEventSeries S where S.SeriesParentID = T.SeriesID and S.SeriesID not in (select SeriesID from @TempItem)
		set @i = (select count(*) from @TempItem T, CollectionEventSeries C where C.SeriesParentID = T.SeriesID and C.SeriesID not in (select SeriesID from @TempItem))
	end

	/*
-- for each child get the childs
   DECLARE HierarchyCursor  CURSOR for
   select SeriesID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   AND @ParentID not in (select SeriesID from @TempItem)
   AND @ParentID not in (select SeriesParentID from @TempItem)
   BEGIN
		insert into @TempItem select SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes, [Geography] 
		from dbo.EventSeriesChildNodes (@ParentID) where SeriesID not in (select SeriesID from @TempItem)
   		FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
   */

 INSERT @ItemList (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes) 
   SELECT distinct SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes
   FROM @TempItem ORDER BY DateStart
 UPDATE L SET [Geography] = E.[Geography]
 FROM @ItemList L, CollectionEventSeries E
 WHERE E.SeriesID = L.SeriesID

   RETURN
END

GO



--#####################################################################################################################
--######   trgUpdCollectionSpecimen - Bugfix including trailing commands   ############################################
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
set LogUpdatedWhen = getdate(), LogUpdatedBy = cast([dbo].[UserID]() as varchar)
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

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.35'
END

GO


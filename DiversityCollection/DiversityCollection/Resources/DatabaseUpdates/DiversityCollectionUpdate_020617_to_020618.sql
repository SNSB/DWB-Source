declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.17'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   trgUpdCollectionEvent - Bugfix for setting date   ##########################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionEvent] ON [dbo].[CollectionEvent]  
 FOR UPDATE AS 
 /*  Created by DiversityWorkbench Administration.  */  
 /*  DiversityWorkbenchMaintenance  2.0.0.3 */  
 /*  Date: 30.08.2007  */  
 /*  Changed: 06.11.2018 - Bugfix for setting date with missing parts */
 if not update(Version)  
 BEGIN  /* setting the version in the main table */  
	 DECLARE @i int  
	 DECLARE @ID int 
	 DECLARE @Version int  
	 set @i = (select count(*) from deleted)   
	 if @i = 1  
	 BEGIN     
		 SET  @ID = (SELECT CollectionEventID FROM deleted)    
		 EXECUTE procSetVersionCollectionEvent @ID 
	 END   

	 -- setting the column CollectionDate for valid dates derived from the columns CollectionDay, CollectionMonth and CollectionYear
	Update CollectionEvent set CollectionDate = case when E.CollectionMonth is null or E.CollectionDay is null or E.CollectionYear is null 
	then null 
	else 
	case when ISDATE(convert(varchar(40), cast(E.CollectionYear as varchar) + '-' 
		+ case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar)  + '-' 
		+ case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000Z', 127)) = 1
	then cast(convert(varchar(40), cast(E.CollectionYear as varchar) + '-' 
		+ case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar)  + '-' 
		+ case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000Z', 127) as datetime)	 
	else null end  
	end
	FROM CollectionEvent E, deleted D 
	where E.CollectionEventID = D.CollectionEventID   

	 /* saving the original dataset in the logging table */  
	 INSERT INTO CollectionEvent_Log (
	 CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, 
	 CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionDateCategory, CollectionTime, 
	 CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, ReferenceDetails, 
	 CollectingMethod, Notes, CountryCache, DataWithholdingReason, RowGUID,  LogCreatedWhen, 
	 LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, LogState, LocalityVerbatim, CollectionEndDay, CollectionEndMonth, CollectionEndYear, DataWithholdingReasonDate)  
	 SELECT D.CollectionEventID, D.Version, D.SeriesID, D.CollectorsEventNumber, D.CollectionDate, 
	 D.CollectionDay, D.CollectionMonth, D.CollectionYear, D.CollectionDateSupplement, D.CollectionDateCategory, D.CollectionTime, 
	 D.CollectionTimeSpan, D.LocalityDescription, D.HabitatDescription, D.ReferenceTitle, D.ReferenceURI, D.ReferenceDetails, 
	 D.CollectingMethod, D.Notes, D.CountryCache, D.DataWithholdingReason, D.RowGUID,  D.LogCreatedWhen, 
	 D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, 'U', D.LocalityVerbatim, D.CollectionEndDay, D.CollectionEndMonth, D.CollectionEndYear, D.DataWithholdingReasonDate
	 FROM DELETED  D
 END  
 
 Update CollectionEvent set LogUpdatedWhen = getdate(), LogUpdatedBy =  suser_sname()
 FROM CollectionEvent, deleted  
 where 1 = 1  AND CollectionEvent.CollectionEventID = deleted.CollectionEventID

 GO

--#####################################################################################################################
--######   AnalysisHierarchyAll() - ensure AnalysisID <> AnalysisParentID  ############################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AnalysisHierarchyAll] ()  
RETURNS @AnalysisList TABLE ([AnalysisID] [int] Primary key ,
	[AnalysisParentID] [int] NULL ,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[MeasurementUnit] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[AnalysisURI] [varchar](255) NULL,
	[OnlyHierarchy] [bit] NULL,
	[HierarchyDisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the analysis items related to the given analysis.
MW 02.01.2006
TEST:
SELECT * FROM DBO.AnalysisHierarchyAll()
*/
AS
BEGIN
INSERT @AnalysisList (AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy, HierarchyDisplayText)
SELECT DISTINCT AnalysisID, case when AnalysisParentID = AnalysisID then null else AnalysisParentID end as AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy
, DisplayText
FROM Analysis
WHERE Analysis.AnalysisParentID IS NULL or Analysis.AnalysisParentID = Analysis.AnalysisID
declare @i int
set @i = (select count(*) from Analysis where AnalysisID not IN (select AnalysisID from  @AnalysisList))
while (@i > 0)
	begin
	INSERT @AnalysisList (AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy, HierarchyDisplayText)
	SELECT DISTINCT C.AnalysisID, case when C.AnalysisParentID = C.AnalysisID then null else C.AnalysisParentID end as AnalysisParentID, C.DisplayText, C.Description, C.MeasurementUnit, C.Notes, C.AnalysisURI, C.OnlyHierarchy, L.HierarchyDisplayText + ' | ' + C.DisplayText
	FROM Analysis C, @AnalysisList L
	WHERE C.AnalysisParentID = L.AnalysisID
	AND C.AnalysisID NOT IN (select AnalysisID from  @AnalysisList)
	set @i = (select count(*) from Analysis where AnalysisID not IN (select AnalysisID from  @AnalysisList))
end
   RETURN
END
GO

--#####################################################################################################################
--######   UserID() - if SUSER_SNAME is not found, try USER_NAME  #####################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[UserID] () RETURNS int AS 
BEGIN  
declare @ID int;  
SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = SUSER_SNAME()) 
IF @ID IS NULL
begin
	SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = USER_NAME()) 
end
RETURN @ID  
END 
GO

--#####################################################################################################################
--######   expert assignment - new category for CollIdentificationCategory_Enum  ######################################
--#####################################################################################################################

if (select count(*) from [dbo].[CollIdentificationCategory_Enum] c where c.Code = 'expert assignment') = 0
begin
INSERT INTO [dbo].[CollIdentificationCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayOrder
           ,[DisplayEnable])
     VALUES
           ('expert assignment'
           ,'The assignment of occurrence data to a new taxon following expert opinion, without study of material'
           ,'expert assignment'
		   ,11
           ,1)
end
GO


--#####################################################################################################################
--######   CollectionEventRegulation - remove link to obsolete table Regulation  ######################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE T
where T.TABLE_NAME = 'CollectionEventRegulation'
and T.COLUMN_NAME = 'Regulation'
and T.CONSTRAINT_NAME = 'FK_CollectionEventRegulation_Regulation') > 0
begin
	ALTER TABLE [dbo].[CollectionEventRegulation] DROP CONSTRAINT [FK_CollectionEventRegulation_Regulation]
end
GO
if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CollectionEventRegulation' and c.COLUMN_NAME = 'RegulationID') > 0
begin
	ALTER TABLE [dbo].[CollectionEventRegulation] DROP COLUMN [RegulationID]
end
GO


--#####################################################################################################################
--######   deactivate trgInsteadOfInsertCollectionSpecimenTransaction if present  #####################################
--#####################################################################################################################

if (select count(*) from sys.objects o where o.name = 'trgInsteadOfInsertCollectionSpecimenTransaction') = 1
begin try
ALTER TABLE [dbo].[CollectionSpecimenTransaction] DISABLE TRIGGER [trgInsteadOfInsertCollectionSpecimenTransaction]
end try
begin catch
end catch
GO


--#####################################################################################################################
--######   activate trgInsCollectionSpecimenTransaction ###############################################################
--#####################################################################################################################

ALTER TABLE [dbo].[CollectionSpecimenTransaction] ENABLE TRIGGER [trgInsCollectionSpecimenTransaction]
GO


--#####################################################################################################################
--######   Alter TransactionRegulation - hide title for user with missing permission  #################################
--#####################################################################################################################

ALTER VIEW [dbo].[TransactionRegulation]
AS
SELECT        CASE WHEN L.TransactionID IS NULL THEN R.TransactionType ELSE R.TransactionTitle END AS TransactionTitle, R.TransactionID
FROM            dbo.[Transaction] AS R LEFT OUTER JOIN
                         dbo.TransactionList AS L ON R.TransactionID = L.TransactionID
WHERE        (R.TransactionType IN (N'regulation'))
GO


--#####################################################################################################################
--######   ApplicationSearchSelectionStrings_Core - adaption to DSGVO   ###############################################
--######   bugfix for remnants with user name instead of user ID   ####################################################
--#####################################################################################################################

ALTER VIEW [dbo].[ApplicationSearchSelectionStrings_Core]
AS
SELECT      SQLStringIdentifier, ItemTable, SQLString, Description
FROM            dbo.ApplicationSearchSelectionStrings
WHERE        (UserName = dbo.UserID())
and ISNUMERIC(UserName) = 1
union
SELECT        SQLStringIdentifier, ItemTable, SQLString, Description
FROM            dbo.ApplicationSearchSelectionStrings
WHERE        (UserName = User_Name())
and ISNUMERIC(UserName) = 0
GO


--#####################################################################################################################
--######   TransactionPermit - adding column Investigator  ############################################################
--#####################################################################################################################

ALTER VIEW [dbo].[TransactionPermit]
AS
SELECT        TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, MaterialCollectors, MaterialSource, FromTransactionPartnerName, FromTransactionNumber, 
                         ToTransactionPartnerName, ToTransactionNumber, NumberOfUnits, TransactionComment, Investigator, BeginDate, AgreedEndDate, DateSupplement, InternalNotes, ToRecipient, ResponsibleName, TransactionID
FROM            dbo.[Transaction]
WHERE        (TransactionType IN (N'permit'))
GO


--#####################################################################################################################
--######   Descriptions for database roles  ###########################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Administrator of the database. Read/write access to all objects',
@level0type = N'USER', @level0name = 'Administrator';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Administrator of the database. Read/write access to all objects',
@level0type = N'USER', @level0name = 'Administrator';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Reading access to objects related to the cache database',
@level0type = N'USER', @level0name = 'CacheUser';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Reading access to objects related to the cache database',
@level0type = N'USER', @level0name = 'CacheUser';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Read/write access to objects related to cache database',
@level0type = N'USER', @level0name = 'CacheAdmin';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Read/write access to objects related to cache database',
@level0type = N'USER', @level0name = 'CacheAdmin';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role for the administration of collections and References',
@level0type = N'USER', @level0name = 'CollectionManager';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role for the administration of collections and References',
@level0type = N'USER', @level0name = 'CollectionManager';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Can delete data from tables CollectionReference and CollectionReference, otherwise same permissions as Editor',
@level0type = N'USER', @level0name = 'DataManager';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Can delete data from tables CollectionReference and CollectionReference, otherwise same permissions as Editor',
@level0type = N'USER', @level0name = 'DataManager';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role with write access to the tables for the desprition of the database objects',
@level0type = N'USER', @level0name = 'DescriptionEditor';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role with write access to the tables for the desprition of the database objects',
@level0type = N'USER', @level0name = 'DescriptionEditor';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Standard role for most users. Write permissions for most tables but can not delete data from tables CollectionReference and CollectionReference',
@level0type = N'USER', @level0name = 'Editor';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Standard role for most users. Write permissions for most tables but can not delete data from tables CollectionReference and CollectionReference',
@level0type = N'USER', @level0name = 'Editor';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role with special rights needed for replication',
@level0type = N'USER', @level0name = 'Replicator';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role with special rights needed for replication',
@level0type = N'USER', @level0name = 'Replicator';
END CATCH;
GO

BEGIN TRY
REVOKE SELECT ON [dbo].[ReferenceGeoAnalysis_log] TO [Replicator] AS [dbo]
END TRY
BEGIN CATCH
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role for handling Reference References and related information',
@level0type = N'USER', @level0name = 'StorageManager';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role for handling Reference References and related information',
@level0type = N'USER', @level0name = 'StorageManager';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Database Role with reading accesss to Transaction information.',
@level0type = N'USER', @level0name = 'TransactionUser';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Database Role with reading accesss to Transaction information.',
@level0type = N'USER', @level0name = 'TransactionUser';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Write access to most objects. Can not delete data and not insert data into tables CollectionReference and CollectionReference',
@level0type = N'USER', @level0name = 'Typist';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Write access to most objects. Can not delete data and not insert data into tables CollectionReference and CollectionReference',
@level0type = N'USER', @level0name = 'Typist';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Restricted to read access to all tables. Can insert data into table Annotation',
@level0type = N'USER', @level0name = 'User';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Restricted to read access to all tables. Can insert data into table Annotation',
@level0type = N'USER', @level0name = 'User';
END CATCH;
GO


--#####################################################################################################################
--######   CollectionSpecimenTransaction: trgInsCollectionSpecimenTransaction for validation  #########################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgInsCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR INSERT AS
if (select count(*) from inserted I, [Transaction] T 
where I.TransactionID = T.TransactionID 
and T.TransactionType = 'regulation') = 1
begin
	if (select count(*) from inserted I, [Transaction] T, CollectionSpecimen S, CollectionEventRegulation R 
	where I.TransactionID = T.TransactionID 
	and I.TransactionTitle = T.TransactionTitle
	and I.CollectionSpecimenID = S.CollectionSpecimenID
	and S.CollectionEventID = R.CollectionEventID
	and I.TransactionTitle = R.Regulation
	and T.TransactionType = 'regulation') = 0
	begin
		rollback tran
	end
end
GO


--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.09.06' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.18'
END

GO


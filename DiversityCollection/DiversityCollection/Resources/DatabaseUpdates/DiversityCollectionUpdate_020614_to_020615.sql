declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.14'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Grant for CollectionManager   ##############################################################################
--#####################################################################################################################

GRANT INSERT ON [dbo].[CollectionManager] TO CollectionManager;
GO


--#####################################################################################################################
--######   New column LastChanges in ProjectProxy   ###################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectProxy' and C.COLUMN_NAME = 'LastChanges') = 0
begin
	ALTER TABLE [dbo].ProjectProxy ADD LastChanges datetime NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The recent date when data within the project had been changed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'LastChanges'
end
GO

--#####################################################################################################################
--######   Setting the new column LastChanges in ProjectProxy to the last date of changes  ############################
--#####################################################################################################################

DECLARE @PP TABLE ([ProjectID] [int] Primary key ,
	[LastChanges] [datetime] NULL)
insert into @PP(ProjectID, LastChanges)
select P.ProjectID, max(S.LogUpdatedWhen)
FROM CollectionProject C, ProjectProxy P, CollectionSpecimen S
where S.CollectionSpecimenID = C.CollectionSpecimenID
group by P.ProjectID, C.ProjectID
having C.ProjectID = P.ProjectID
--select * from @PP
UPDATE P 
set P.LastChanges = PP.LastChanges
FROM ProjectProxy P, @PP PP
WHERE P.ProjectID = PP.ProjectID
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

/* setting the date in ProjectProxy */ 
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
)

GO


--#####################################################################################################################
--######   trgInsCollectionProject - writing date into LastChanges in ProjectProxy   ##################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgInsCollectionProject] ON [dbo].[CollectionProject] 
FOR INSERT AS

/*  By: M. Weiss */ 
/*  Date: 08.06.2018  */ 

/* setting the date in ProjectProxy */ 
Update P 
set P.LastChanges = getdate()
FROM inserted I, ProjectProxy P, CollectionSpecimen S
where S.CollectionSpecimenID = I.CollectionSpecimenID
AND P.ProjectID = I.ProjectID
AND (S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '')

GO


--#####################################################################################################################
--######   trgDelCollectionProject - writing date into LastChanges in ProjectProxy   ##################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionProject] ON [dbo].[CollectionProject] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.0 */ 
/*  Date: 24.11.2011  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionProject_Log (CollectionSpecimenID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

/* setting the date in ProjectProxy */ 
/* setting the date in ProjectProxy */ 
Update P 
set P.LastChanges = getdate()
FROM deleted D, ProjectProxy P, CollectionSpecimen S
where D.CollectionSpecimenID = S.CollectionSpecimenID
AND P.ProjectID = D.ProjectID
AND (S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '')
GO


--#####################################################################################################################
--######   procSetVersionCollectionSpecimen - writing date into LastChanges in ProjectProxy   #########################
--#####################################################################################################################

ALTER PROCEDURE [dbo].[procSetVersionCollectionSpecimen]  (@ID int) 
AS 
/*  Setting the version of a dataset.  */ 
/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

DECLARE @NextVersion int 
DECLARE @CurrentVersion int 
DECLARE @LastUser nvarchar(500) 
DECLARE @LastUpdate datetime 
DECLARE @UpdatePeriod int 

set @LastUpdate = (SELECT LogUpdatedWhen FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID) 
set @UpdatePeriod = (SELECT DateDiff(hour, @LastUpdate, getdate())) 
set @LastUser = (SELECT LogUpdatedBy FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID) 

if not @ID is null and (@LastUser <> User_name() or @UpdatePeriod > 24) 
begin  
    set @CurrentVersion = (select Version from CollectionSpecimen where CollectionSpecimenID = @ID) 
    if @CurrentVersion is null begin set @CurrentVersion = 0 end 
    set @NextVersion = @CurrentVersion + 1 
    update CollectionSpecimen set Version = @NextVersion 
    where CollectionSpecimenID = @ID 

/* setting the date in ProjectProxy */ 
Update P 
set P.LastChanges = getdate()
FROM CollectionProject C, ProjectProxy P, CollectionSpecimen S 
where C.CollectionSpecimenID = @ID
AND C.ProjectID = P.ProjectID
AND C.CollectionSpecimenID = S.CollectionSpecimenID
AND (S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '')

select @NextVersion  
END
GO


--#####################################################################################################################
--######   procSetVersionCollectionEvent - writing date into LastChanges in ProjectProxy   ############################
--#####################################################################################################################

ALTER  PROCEDURE [dbo].[procSetVersionCollectionEvent]  (@ID int)
AS 
/*  Setting the version of a dataset.  */ 
/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

DECLARE @NextVersion int 
DECLARE @CurrentVersion int 
DECLARE @LastUser nvarchar(500) 
DECLARE @LastUpdate datetime 
DECLARE @UpdatePeriod int 

set @LastUpdate = (SELECT LogUpdatedWhen FROM CollectionEvent WHERE CollectionEventID = @ID) 
set @UpdatePeriod = (SELECT DateDiff(hour, @LastUpdate, getdate())) 
set @LastUser = (SELECT LogUpdatedBy FROM CollectionEvent WHERE CollectionEventID = @ID) 
set @CurrentVersion = (select Version from CollectionEvent where CollectionEventID = @ID) 
 if @CurrentVersion is null begin set @CurrentVersion = 0 end 
set @NextVersion = @CurrentVersion

if not @ID is null and ((@LastUser <> User_name() and @LastUser <> SUSER_SNAME()) or @UpdatePeriod > 24)
begin  
    set @NextVersion = @CurrentVersion + 1 
    update CollectionEvent set Version = @NextVersion 
    where CollectionEventID = @ID 

/* setting the date in ProjectProxy */ 
Update P 
set P.LastChanges = getdate()
FROM CollectionProject C, CollectionSpecimen S, ProjectProxy P
where S.CollectionEventID = @ID
AND C.CollectionSpecimenID = S.CollectionSpecimenID
AND C.ProjectID = P.ProjectID
AND (S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '')

select @NextVersion  
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
RETURN '02.06.15'
END

GO


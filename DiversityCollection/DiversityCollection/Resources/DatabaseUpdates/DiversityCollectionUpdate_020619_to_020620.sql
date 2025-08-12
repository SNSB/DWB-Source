declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.19'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  CollectionSpecimenReference - Add column IdentificationSequence  ############################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'CollectionSpecimenReference' and c.COLUMN_NAME = 'IdentificationSequence') = 0
begin
	ALTER TABLE CollectionSpecimenReference ADD [IdentificationSequence] [smallint] NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Referes to table Identification: The sequence of the identifications.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenReference', @level2type=N'COLUMN',@level2name=N'IdentificationSequence'
	ALTER TABLE CollectionSpecimenReference_log ADD [IdentificationSequence] [smallint] NULL;
end
GO

--#####################################################################################################################
--######  trgDelCollectionSpecimenReference    ########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenReference] ON [dbo].[CollectionSpecimenReference] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ReferenceDetails, ReferenceID, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID,  LogVersion,  LogState) 
SELECT d.CollectionSpecimenID, d.IdentificationUnitID, d.IdentificationSequence, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.ReferenceDetails, d.ReferenceID, d.ReferenceTitle, d.ReferenceURI, d.ResponsibleAgentURI, d.ResponsibleName, d.RowGUID, d.SpecimenPartID,  @Version,  'D'
FROM DELETED d
end
else
begin
if (select count(*) FROM DELETED D, CollectionSpecimen WHERE d.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ReferenceDetails, ReferenceID, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.IdentificationUnitID, d.IdentificationSequence, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.ReferenceDetails, d.ReferenceID, d.ReferenceTitle, d.ReferenceURI, d.ResponsibleAgentURI, d.ResponsibleName, d.RowGUID, d.SpecimenPartID, CollectionSpecimen.Version, 'D' 
FROM DELETED d, CollectionSpecimen
WHERE d.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, ReferenceDetails, ReferenceID, ReferenceTitle, ReferenceURI, ResponsibleAgentURI, ResponsibleName, RowGUID, SpecimenPartID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.IdentificationUnitID, d.IdentificationSequence, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.ReferenceDetails, d.ReferenceID, d.ReferenceTitle, d.ReferenceURI, d.ResponsibleAgentURI, d.ResponsibleName, d.RowGUID, d.SpecimenPartID, -1, 'D' 
FROM DELETED D
end
end
GO



--#####################################################################################################################
--######   trgUpdCollectionSpecimenReference   ########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenReference] ON [dbo].[CollectionSpecimenReference] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 5/20/2016  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, IdentificationSequence, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT d.CollectionSpecimenID, d.ReferenceID, d.ReferenceTitle, d.ReferenceURI, d.IdentificationUnitID, d.IdentificationSequence, d.SpecimenPartID, d.ReferenceDetails, d.Notes, d.ResponsibleName, d.ResponsibleAgentURI, d.LogCreatedWhen, d.LogCreatedBy, d.LogUpdatedWhen, d.LogUpdatedBy, d.RowGUID,  @Version,  'U'
FROM DELETED D
end
else
begin
INSERT INTO CollectionSpecimenReference_Log (CollectionSpecimenID, ReferenceID, ReferenceTitle, ReferenceURI, IdentificationUnitID, IdentificationSequence, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.ReferenceID, d.ReferenceTitle, d.ReferenceURI, d.IdentificationUnitID, d.IdentificationSequence, d.SpecimenPartID, d.ReferenceDetails, d.Notes, d.ResponsibleName, d.ResponsibleAgentURI, d.LogCreatedWhen, d.LogCreatedBy, d.LogUpdatedWhen, d.LogUpdatedBy, d.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED D, CollectionSpecimen
WHERE d.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM CollectionSpecimenReference R, deleted d
where 1 = 1 
AND R.CollectionSpecimenID = d.CollectionSpecimenID
AND R.ReferenceTitle = d.ReferenceTitle

GO


--#####################################################################################################################
--######  Add potential missing obsolete columns to Identification for compatibility ##################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Identification' and c.COLUMN_NAME = 'ReferenceTitle') = 0
begin
	ALTER TABLE Identification ADD ReferenceTitle nvarchar(255) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Obsolete; Publications or authoritative opinions of scientist used during the identification process. Example: enter ''Schmeil-Fitschen 1995'', if this field flora was used.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Identification', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Identification' and c.COLUMN_NAME = 'ReferenceURI') = 0
begin
	ALTER TABLE Identification ADD ReferenceURI varchar(255) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Obsolete; The URI of the reference e.g. as provided by the module DiversityReferences' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Identification', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Identification' and c.COLUMN_NAME = 'ReferenceDetails') = 0
begin
	ALTER TABLE Identification ADD ReferenceDetails nvarchar(50) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Obsolete; The exact location within the reference, e.g. pages, plates' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Identification', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
end
GO

--#####################################################################################################################
--######  Transfer data from Identification to CollectionSpecimenReference ############################################
--#####################################################################################################################

INSERT INTO [dbo].[CollectionSpecimenReference]
        ([CollectionSpecimenID]
        ,[IdentificationUnitID]
        ,[IdentificationSequence]
        ,[ReferenceTitle]
        ,[ReferenceURI]
        ,[ReferenceDetails]
        ,[ResponsibleName]
        ,[ResponsibleAgentURI]
        ,[LogCreatedWhen]
        ,[LogCreatedBy]
        ,[LogUpdatedWhen]
        ,[LogUpdatedBy])
SELECT [CollectionSpecimenID]
    ,[IdentificationUnitID]
    ,[IdentificationSequence]
    ,[ReferenceTitle]
    ,[ReferenceURI]
    ,[ReferenceDetails]
    ,[ResponsibleName]
    ,[ResponsibleAgentURI]
    ,[LogCreatedWhen]
    ,[LogCreatedBy]
    ,[LogUpdatedWhen]
    ,[LogUpdatedBy]
FROM [dbo].[Identification]
WHERE [ReferenceTitle] <> ''
GO






--#####################################################################################################################
--######  ExternalIdentifier - GRANT VIEW DEFINITION ##################################################################
--#####################################################################################################################

GRANT VIEW DEFINITION ON dbo.ExternalIdentifier TO [Editor];
GO

--#####################################################################################################################
--######  CollectionAgent_Core: CollectionSpecimenID_Available instead of CollectionSpecimenID_UserAvailable  #########
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionAgent_Core]
AS
SELECT        dbo.CollectionAgent.CollectionSpecimenID, dbo.CollectionAgent.CollectorsName, dbo.CollectionAgent.CollectorsAgentURI, dbo.CollectionAgent.CollectorsSequence, dbo.CollectionAgent.CollectorsNumber, 
                         dbo.CollectionAgent.Notes, dbo.CollectionAgent.DataWithholdingReason
FROM            dbo.CollectionAgent INNER JOIN
                         dbo.CollectionSpecimenID_Available ON dbo.CollectionAgent.CollectionSpecimenID = dbo.CollectionSpecimenID_Available.CollectionSpecimenID
GO

--#####################################################################################################################
--######  CollIdentificationCategory_Enum: changed descriptions for negative and dubious  #############################
--#####################################################################################################################

UPDATE [dbo].[CollIdentificationCategory_Enum]
   SET [Description] = 'The material is identified as not belonging to the identified taxon or the observation is verified as not belonging  to the identified taxon'
 WHERE Code = 'negative'

 UPDATE [dbo].[CollIdentificationCategory_Enum]
   SET [Description] = 'The material is assumed as dubiously belonging to the identified taxon or the observation is assumed as dubiously belonging  to the identified taxon'
 WHERE Code = 'dubious'

 GO

--#####################################################################################################################
--######   CollectionSpecimenPartDescription: add IdentificationUnitID for optional link to unit  #####################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.COLUMN_NAME = 'IdentificationUnitID' and c.TABLE_NAME = 'CollectionSpecimenPartDescription') = 0
begin
	ALTER TABLE [CollectionSpecimenPartDescription] ADD [IdentificationUnitID] [int] NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of IdentficationUnit (= foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'IdentificationUnitID'
	ALTER TABLE [CollectionSpecimenPartDescription_LOG] ADD [IdentificationUnitID] [int] NULL;
end
GO

--#####################################################################################################################
--######  trgDelCollectionSpecimenPartDescription   ###################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelCollectionSpecimenPartDescription] ON [dbo].[CollectionSpecimenPartDescription] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID, IdentificationUnitID,  LogVersion,  LogState) 
SELECT d.CollectionSpecimenID, d.Description, d.DescriptionTermURI, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.PartDescriptionID, d.RowGUID, d.SpecimenPartID, d.IdentificationUnitID,  @Version,  'D'
FROM DELETED D
end
else
begin
if (select count(*) FROM DELETED D, CollectionSpecimen S WHERE d.CollectionSpecimenID = S.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID, IdentificationUnitID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.Description, d.DescriptionTermURI, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.PartDescriptionID, d.RowGUID, d.SpecimenPartID, d.IdentificationUnitID, CollectionSpecimen.Version, 'D' 
FROM DELETED D, CollectionSpecimen
WHERE d.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID, IdentificationUnitID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.Description, d.DescriptionTermURI, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.PartDescriptionID, d.RowGUID, d.SpecimenPartID, d.IdentificationUnitID, -1, 'D' 
FROM DELETED D
end
end
GO


--#####################################################################################################################
--######   trgUpdCollectionSpecimenPartDescription    #################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenPartDescription] ON [dbo].[CollectionSpecimenPartDescription] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.7 */ 
/*  Date: 7/25/2016  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, Notes, IdentificationUnitID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT d.CollectionSpecimenID, d.SpecimenPartID, d.PartDescriptionID, d.Description, d.DescriptionTermURI, d.Notes, d.IdentificationUnitID, d.LogCreatedWhen, d.LogCreatedBy, d.LogUpdatedWhen, d.LogUpdatedBy, d.RowGUID,  @Version,  'U'
FROM DELETED D
end
else
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, Notes, IdentificationUnitID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.SpecimenPartID, d.PartDescriptionID, d.Description, d.DescriptionTermURI, d.Notes, d.IdentificationUnitID, d.LogCreatedWhen, d.LogCreatedBy, d.LogUpdatedWhen, d.LogUpdatedBy, d.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED D, CollectionSpecimen
WHERE d.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenPartDescription
set LogUpdatedWhen = getdate(), LogUpdatedBy = cast([dbo].[UserID]() as varchar)
FROM CollectionSpecimenPartDescription, deleted D
where 1 = 1 
AND CollectionSpecimenPartDescription.CollectionSpecimenID = d.CollectionSpecimenID
AND CollectionSpecimenPartDescription.PartDescriptionID = d.PartDescriptionID
AND CollectionSpecimenPartDescription.SpecimenPartID = d.SpecimenPartID
GO


--#####################################################################################################################
--######  Add potential missing obsolete column to IdentificationUnitInPart for compatibility #########################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'IdentificationUnitInPart' and c.COLUMN_NAME = 'Description') = 0
begin
	ALTER TABLE IdentificationUnitInPart ADD [Description] nvarchar(500) NULL;
end
GO

--#####################################################################################################################
--######   Transfer of description from table IdentificationUnitInPart into CollectionSpecimenPartDescription   #######
--#####################################################################################################################

if (SELECT count(*)
	FROM [dbo].[IdentificationUnitInPart]
	UiP where UiP.[Description] <> '') > 0
begin
INSERT INTO [dbo].[CollectionSpecimenPartDescription]
           ([CollectionSpecimenID]
           ,[SpecimenPartID]
           ,[Description]
           ,[IdentificationUnitID]
           ,[LogCreatedWhen]
           ,[LogCreatedBy]
           ,[LogUpdatedWhen]
           ,[LogUpdatedBy]
           ,[RowGUID])
SELECT [CollectionSpecimenID]
      ,[SpecimenPartID]
      ,[Description]
      ,[IdentificationUnitID]
      ,[LogInsertedWhen]
      ,[LogInsertedBy]
      ,[LogUpdatedWhen]
      ,[LogUpdatedBy]
      ,[RowGUID]
  FROM [dbo].[IdentificationUnitInPart]
  UiP where UiP.[Description] <> ''

end
GO

--#####################################################################################################################
--######   Adaption of description of column Description in table IdentificationUnitInPart ############################
--#####################################################################################################################
begin try
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Obsolete - please use table CollectionSpecimenPartDescription instead' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitInPart', @level2type=N'COLUMN',@level2name=N'Description'
end try
begin catch
	EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Obsolete - please use table CollectionSpecimenPartDescription instead' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitInPart', @level2type=N'COLUMN',@level2name=N'Description'
end catch
GO


--#####################################################################################################################
--######  Add potential missing obsolete columns to CollectionSpecimen for compatibility ##############################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CollectionSpecimen' and c.COLUMN_NAME = 'ReferenceTitle') = 0
begin
	ALTER TABLE CollectionSpecimen ADD ReferenceTitle nvarchar(255) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Obsolete - please use table CollectionSpecimenReference instead' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimen', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CollectionSpecimen' and c.COLUMN_NAME = 'ReferenceURI') = 0
begin
	ALTER TABLE CollectionSpecimen ADD ReferenceURI varchar(255) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Obsolete - please use table CollectionSpecimenReference instead' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimen', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CollectionSpecimen' and c.COLUMN_NAME = 'ReferenceDetails') = 0
begin
	ALTER TABLE CollectionSpecimen ADD ReferenceDetails nvarchar(50) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Obsolete - please use table CollectionSpecimenReference instead' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimen', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
end
GO


--#####################################################################################################################
--######   Transfer of reference from table CollectionSpecimen into CollectionSpecimenReference   #####################
--#####################################################################################################################

INSERT INTO [dbo].[CollectionSpecimenReference]
           ([CollectionSpecimenID]
           ,[ReferenceTitle]
           ,[ReferenceURI]
           ,[ReferenceDetails]
           ,[LogCreatedWhen]
           ,[LogCreatedBy]
           ,[LogUpdatedWhen]
           ,[LogUpdatedBy])
SELECT [CollectionSpecimenID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[LogCreatedWhen]
      ,[LogCreatedBy]
      ,[LogUpdatedWhen]
      ,[LogUpdatedBy]
  FROM [dbo].[CollectionSpecimen] S
WHERE S.[ReferenceTitle] <> ''


GO

--#####################################################################################################################
--######   Adaption of description of reference columns in table CollectionSpecimen ###################################
--#####################################################################################################################

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Obsolete - please use table CollectionSpecimenReference instead' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimen', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Obsolete - please use table CollectionSpecimenReference instead' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimen', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Obsolete - please use table CollectionSpecimenReference instead' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimen', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO


--#####################################################################################################################
--######   UserID() - if SUSER_SNAME is not found, try USER_NAME, overwise -1  ########################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[UserID] () RETURNS int AS 
BEGIN  
declare @ID int;  
SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = SUSER_SNAME()) 
IF @ID IS NULL
begin
	SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = USER_NAME()) 
end
IF @ID IS NULL
begin
	set @ID = -1
end
RETURN @ID  
END 
GO



--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '04.01.03' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.20'
END

GO


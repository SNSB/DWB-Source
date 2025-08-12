
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.28'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   DataWithholdingReason   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionSpecimenPart ADD DataWithholdingReason nvarchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the specimen part is withhold, the reason for withholding the data, otherwise null.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPart', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO


ALTER TABLE CollectionSpecimenPart_log ADD DataWithholdingReason nvarchar(255) NULL
GO


--#####################################################################################################################
--######   trgDelCollectionSpecimenPart   ######################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelCollectionSpecimenPart] ON [dbo].[CollectionSpecimenPart] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 


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
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, 
PreparationDate, AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, 
Stock, StockUnit, ResponsibleName, ResponsibleAgentURI, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, 
LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, 
deleted.PreparationDate, deleted.AccessionNumber, deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, 
deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.ResponsibleName, 
deleted.ResponsibleAgentURI, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, 
PreparationDate, AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, 
Stock, StockUnit, ResponsibleName, ResponsibleAgentURI, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, 
LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, 
deleted.PreparationDate, deleted.AccessionNumber, deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, 
deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.ResponsibleName, deleted.ResponsibleAgentURI, 
deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, 
deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D'
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO

--#####################################################################################################################
--######   trgUpdCollectionSpecimenPart   ######################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenPart] ON [dbo].[CollectionSpecimenPart] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 

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
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, 
PreparationDate, AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, 
Stock, StockUnit, ResponsibleName, ResponsibleAgentURI, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, 
LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, 
deleted.PreparationDate, deleted.AccessionNumber, deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, 
deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.ResponsibleName, 
deleted.ResponsibleAgentURI, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, 
PreparationDate, AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, 
Stock, StockUnit, ResponsibleName, ResponsibleAgentURI, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, 
LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, 
deleted.PreparationDate, deleted.AccessionNumber, deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, 
deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.ResponsibleName, deleted.ResponsibleAgentURI, 
deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, 
deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U'
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenPart
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenPart, deleted 
where 1 = 1 
AND CollectionSpecimenPart.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenPart.SpecimenPartID = deleted.SpecimenPartID

GO


--#####################################################################################################################
--######   setting the VersionClient   ######################################################################################
--#####################################################################################################################

 ALTER FUNCTION [dbo].[VersionClient] () RETURNS nvarchar(11) AS BEGIN RETURN '03.00.05.07' END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.29'
END

GO



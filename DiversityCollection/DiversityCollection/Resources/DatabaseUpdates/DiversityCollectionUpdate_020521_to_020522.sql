declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.21'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   Rechte   ######################################################################################
--#####################################################################################################################

--GRANT INSERT ON [Transaction] TO [StorageManager]
--GO

--GRANT UPDATE ON [Transaction] TO [StorageManager]
--GO

GRANT INSERT ON [CollectionManager] TO [Administrator]
GO

GRANT UPDATE ON [CollectionManager] TO [Administrator]
GO

GRANT DELETE ON [CollectionManager] TO [Administrator]
GO

GRANT SELECT ON [CollectionManager] TO [CollectionManager]
GO



GRANT DELETE ON [CollectionEvent] TO [DataManager]
GO

GRANT SELECT ON [IdentificationUnitGeoAnalysis_log] TO [Replicator]
GO


--#####################################################################################################################
--######    Mutant   ######################################################################################
--#####################################################################################################################


INSERT INTO [CollUnitRelationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('Mutant of'
           ,'A mutant of the parent organism'
           ,'Mutant of'
           ,200
           ,1)
GO


--#####################################################################################################################
--######   CollectionSpecimenPart   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionSpecimenPart ADD ResponsibleName nvarchar(255) NULL
GO

ALTER TABLE CollectionSpecimenPart_log ADD ResponsibleName nvarchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person or institution responsible for the preparation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPart', @level2type=N'COLUMN',@level2name=N'ResponsibleName'
GO


ALTER TABLE CollectionSpecimenPart ADD ResponsibleAgentURI  varchar(255) NULL
GO

ALTER TABLE CollectionSpecimenPart_log ADD ResponsibleAgentURI  varchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or institution responsible for the preparation (= foreign key) as stored in the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPart', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'
GO



/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenPart]    Script Date: 06/26/2012 16:08:56 ******/

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
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, ResponsibleName, ResponsibleAgentURI, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, Notes, RowGUID,
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, deleted.RowGUID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, ResponsibleName, ResponsibleAgentURI, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, Notes, RowGUID, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, deleted.RowGUID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end



GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenPart]    Script Date: 06/26/2012 16:11:13 ******/

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
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, ResponsibleName, ResponsibleAgentURI, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, 
Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, deleted.RowGUID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, ResponsibleName, ResponsibleAgentURI, PreparationMethod, PreparationDate, AccessionNumber, 
PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, Stock, StockUnit, 
Notes, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.PreparationMethod, deleted.PreparationDate, deleted.AccessionNumber, 
deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.Notes, deleted.RowGUID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
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
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.22'
END

GO

select [dbo].[Version] ()  


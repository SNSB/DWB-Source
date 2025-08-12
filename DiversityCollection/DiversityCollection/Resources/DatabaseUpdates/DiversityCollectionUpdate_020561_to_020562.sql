
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.61'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Default Datawithholding for Specimen   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C
where C.TABLE_NAME = 'CollectionSpecimen'
and C.COLUMN_NAME = 'DataWithholdingReason'
and C.COLUMN_DEFAULT is null) = 1
begin
	ALTER TABLE CollectionSpecimen ADD CONSTRAINT [DF_CollectionSpecimen_DataWithholdingReason]  DEFAULT (N'Withold by default') FOR DataWithholdingReason
end
GO

--#####################################################################################################################
--######   Datawithholding for Unit   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C
where C.TABLE_NAME = 'IdentificationUnit'
and C.COLUMN_NAME = 'DataWithholdingReason') = 0
begin
	ALTER TABLE IdentificationUnit ADD DataWithholdingReason nvarchar(255) NULL
	ALTER TABLE IdentificationUnit_log ADD DataWithholdingReason nvarchar(255) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the data set is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnit', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
end
GO



ALTER TRIGGER [dbo].[trgDelIdentificationUnit] ON [dbo].[IdentificationUnit] 
FOR DELETE AS 
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 
DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
if (not @Version is null) 
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, DataWithholdingReason,
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, deleted.DataWithholdingReason, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, DataWithholdingReason, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState)  
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, deleted.DataWithholdingReason, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



/****** Object:  Trigger [dbo].[trgUpdIdentificationUnit]    Script Date: 21.07.2015 17:30:50 ******/

ALTER TRIGGER [dbo].[trgUpdIdentificationUnit] ON [dbo].[IdentificationUnit] 
FOR UPDATE AS
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 
DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
Update IdentificationUnit
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnit, deleted 
where 1 = 1 
AND IdentificationUnit.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnit.IdentificationUnitID = deleted.IdentificationUnitID
if (not @Version is null) 
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, DataWithholdingReason, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, deleted.DataWithholdingReason, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, DataWithholdingReason, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState)  
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, deleted.DataWithholdingReason, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO


--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.09' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.62'
END

GO



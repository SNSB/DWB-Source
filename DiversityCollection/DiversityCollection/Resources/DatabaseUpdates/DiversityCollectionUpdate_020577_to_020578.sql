declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.77'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   IdentificationUnit - NumberOfUnitsModifier    ##############################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'IdentificationUnit' and C.COLUMN_NAME = 'NumberOfUnitsModifier') = 0
begin
	alter table IdentificationUnit ADD NumberOfUnitsModifier nvarchar(50) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A modifier for the number of units of this organism, e.g. ca. 400 beetles in a bottle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnit', @level2type=N'COLUMN',@level2name=N'NumberOfUnitsModifier'

	alter table IdentificationUnit_log ADD NumberOfUnitsModifier nvarchar(50) NULL;
end

GO

/****** Object:  Trigger [dbo].[trgDelIdentificationUnit]    Script Date: 18.02.2016 16:17:22 ******/

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
Gender, NumberOfUnits, NumberOfUnitsModifier, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, DataWithholdingReason,
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.NumberOfUnitsModifier, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, deleted.DataWithholdingReason, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, NumberOfUnitsModifier, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, DataWithholdingReason, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState)  
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.NumberOfUnitsModifier, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
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
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM IdentificationUnit, deleted 
where 1 = 1 
AND IdentificationUnit.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnit.IdentificationUnitID = deleted.IdentificationUnitID
if (not @Version is null) 
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, NumberOfUnitsModifier, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, DataWithholdingReason, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.NumberOfUnitsModifier, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, deleted.DataWithholdingReason, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, NumberOfUnitsModifier, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, DataWithholdingReason, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState)  
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.NumberOfUnitsModifier, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, deleted.DataWithholdingReason, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end


GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.78'
END

GO


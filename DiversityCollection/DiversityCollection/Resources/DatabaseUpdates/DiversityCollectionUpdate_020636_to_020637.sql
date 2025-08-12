declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.36'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Removing user defined types to ensure compatiblity to SIARD   ##############################################
--#####################################################################################################################

ALTER TABLE CollectionEvent ALTER COLUMN CollectionDay tinyint NULL;

ALTER TABLE CollectionEvent ALTER COLUMN CollectionMonth tinyint NULL;

ALTER TABLE CollectionEvent ALTER COLUMN CollectionEndDay tinyint NULL;

ALTER TABLE CollectionEvent ALTER COLUMN CollectionEndMonth tinyint NULL;

ALTER TABLE CollectionEvent_log ALTER COLUMN CollectionDay tinyint NULL;

ALTER TABLE CollectionEvent_log ALTER COLUMN CollectionMonth tinyint NULL;

ALTER TABLE CollectionEvent_log ALTER COLUMN CollectionEndDay tinyint NULL;

ALTER TABLE CollectionEvent_log ALTER COLUMN CollectionEndMonth tinyint NULL;


--#####################################################################################################################
--######   Missing descriptions   #####################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   Tables   ###################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Table for temperary storage of description of database objects derived e.g. from tables Entity, EntityRepresentation etc.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CacheDescription'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Table for temperary storage of description of database objects derived e.g. from tables Entity, EntityRepresentation etc.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CacheDescription'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The type of accessibility of database objects within a certain context',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'EntityAccessibility_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The type of accessibility of database objects within a certain context',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'EntityAccessibility_Enum'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The way a value of an entry is determined, e.g. calculated, defined by the user etc.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'EntityDetermination_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The way a value of an entry is determined, e.g. calculated, defined by the user etc.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'EntityDetermination_Enum'
END CATCH;
GO

--#####################################################################################################################
--######   Trigger   ##################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionAgent',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionAgent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionAgent',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionAgent'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the date in case of valid date columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEvent',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionEvent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the date in case of valid date columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEvent',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionEvent'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventImage',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionEventImage'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventImage',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionEventImage'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting missing geographical values on base of given values',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventLocalisation',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionEventLocalisation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting missing geographical values on base of given values',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventLocalisation',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionEventLocalisation'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventProperty',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionEventProperty'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventProperty',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionEventProperty'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting LastChanges in table ProjectProxy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionProject',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting LastChanges in table ProjectProxy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionProject',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionProject'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',
@level2type=N'TRIGGER', @level2name=N'CollectionSpecimenImage_URI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',
@level2type=N'TRIGGER', @level2name=N'CollectionSpecimenImage_URI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenImage'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenImage'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPart',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenPart'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPart',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenPart'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenProcessing',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenProcessing'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenProcessing',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenProcessing'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenRelation',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenRelation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenRelation',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenRelation'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Rollback of regulations if corresponding datasets are missing in table CollectionEventRegulation ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenTransaction',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenTransaction'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Rollback of regulations if corresponding datasets are missing in table CollectionEventRegulation ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenTransaction',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionSpecimenTransaction'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Inserting CollectionID of parent entry if missing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionTask',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionTask'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Inserting CollectionID of parent entry if missing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionTask',
@level2type=N'TRIGGER', @level2name=N'trgInsCollectionTask'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'setting the display oder for the new unit to the next number if none or an already present on was given',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnit',
@level2type=N'TRIGGER', @level2name=N'trgInsIdentificationUnit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'setting the display oder for the new unit to the next number if none or an already present on was given',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnit',
@level2type=N'TRIGGER', @level2name=N'trgInsIdentificationUnit'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnitAnalysis',
@level2type=N'TRIGGER', @level2name=N'trgInsIdentificationUnitAnalysis'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnitAnalysis',
@level2type=N'TRIGGER', @level2name=N'trgInsIdentificationUnitAnalysis'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnitInPart',
@level2type=N'TRIGGER', @level2name=N'trgInsIdentificationUnitInPart'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnitInPart',
@level2type=N'TRIGGER', @level2name=N'trgInsIdentificationUnitInPart'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Updating the LastIdentificationCache in IdentificationUnit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Identification',
@level2type=N'TRIGGER', @level2name=N'trgIdentificationInsert'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Updating the LastIdentificationCache in IdentificationUnit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Identification',
@level2type=N'TRIGGER', @level2name=N'trgIdentificationInsert'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Updating empty date columns depending on a given date',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Identification',
@level2type=N'TRIGGER', @level2name=N'trgInsIdentification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Updating empty date columns depending on a given date',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Identification',
@level2type=N'TRIGGER', @level2name=N'trgInsIdentification'
END CATCH;
GO


--#####################################################################################################################
--######   Views   ####################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Annotations linked to table CollectionEvent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationEvent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Annotations linked to table CollectionEvent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationEvent'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Annotations linked to table CollectionSpecimenPart',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationPart'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Annotations linked to table CollectionSpecimenPart',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationPart'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Annotations linked to table CollectionSpecimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationSpecimen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Annotations linked to table CollectionSpecimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationSpecimen'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Annotations linked to table IdentificationUnit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationUnit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Annotations linked to table IdentificationUnit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationUnit'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionSpecimenPart restricted to those avaliable for a user including the collection hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenPart_Core2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionSpecimenPart restricted to those avaliable for a user including the collection hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenPart_Core2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the collection separated by "|" followed by StorageLocation resp. AccessionNumber and Sublabel',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenPart_Core2',
@level2type=N'COLUMN', @level2name=N'Collection'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the collection separated by "|" followed by StorageLocation resp. AccessionNumber and Sublabel',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenPart_Core2',
@level2type=N'COLUMN', @level2name=N'Collection'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Identification restricted to those available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'Identification_Core2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Identification restricted to those available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'Identification_Core2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentificationSequenceMax'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentificationSequenceMax'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table IdentificationUnit restricted to those available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentificationUnit_Core2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table IdentificationUnit restricted to those available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentificationUnit_Core2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table IdentificationUnit restricted to those available for a user and DisplayOrder = 1',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentificationUnitDisplayOrder1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table IdentificationUnit restricted to those available for a user and DisplayOrder = 1',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentificationUnitDisplayOrder1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierEvent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierEvent'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierPart'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierPart'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierSpecimen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierSpecimen'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierTransaction'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierTransaction'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierUnit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'IdentifierUnit'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List based on content of table CollectionSpecimenPart restricted to those with an AccessionNumber and available for a CollectionManager',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ManagerSpecimenPartList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List based on content of table CollectionSpecimenPart restricted to those with an AccessionNumber and available for a CollectionManager',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ManagerSpecimenPartList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List of projects available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List of projects available for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List of projects a user can edit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectListNotReadOnly'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List of projects a user can edit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectListNotReadOnly'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List of possible request for a user based on table CollectionSpecimenPart ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'RequesterSpecimenPartList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List of possible request for a user based on table CollectionSpecimenPart ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'RequesterSpecimenPartList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction missing a AdministratingCollectionID resp. those available for a CollectionManager',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction missing a AdministratingCollectionID resp. those available for a CollectionManager',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList'
END CATCH;
GO


if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'TransactionList_H7') > 0

begin
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction missing a AdministratingCollectionID resp. those available for a CollectionManager including a hierarchy with up to 7 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList_H7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction missing a AdministratingCollectionID resp. those available for a CollectionManager including a hierarchy with up to 7 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList_H7'
END CATCH;


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the transaction represented by the TransactionTitle separated by "|" for up to 7 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList_H7',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the transaction represented by the TransactionTitle separated by "|" for up to 7 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList_H7',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END CATCH;

end
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction restricted to those with TransactionType = permit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionPermit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction restricted to those with TransactionType = permit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionPermit'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction restricted to those with TransactionType = regulation a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionRegulation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction restricted to those with TransactionType = regulation a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionRegulation'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List of the groups the users are asigned to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'UserGroups'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List of the groups the users are asigned to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'UserGroups'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of function dbo.BaseURL() ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of function dbo.BaseURL() ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionEventImage converting XML to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewCollectionEventImage'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionEventImage converting XML to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewCollectionEventImage'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionEventSeriesImage converting XML to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewCollectionEventSeriesImage'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionEventSeriesImage converting XML to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewCollectionEventSeriesImage'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Collection converting XML to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewCollectionImage'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Collection converting XML to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewCollectionImage'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionSpecimenImage converting XML to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewCollectionSpecimenImage'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionSpecimenImage converting XML to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewCollectionSpecimenImage'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of function dbo.DiversityWorkbenchModule()',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDiversityWorkbenchModule'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of function dbo.DiversityWorkbenchModule()',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewDiversityWorkbenchModule'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table IdentificationUnitGeoAnalysis converting geography and geometry to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewIdentificationUnitGeoAnalysis'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table IdentificationUnitGeoAnalysis converting geography and geometry to nvarchar and excluding Log columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewIdentificationUnitGeoAnalysis'
END CATCH;
GO



--#####################################################################################################################
--######   Functions   ################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Analysis containing all children for a given AnalysisID defined via the relation in column AnalysisParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Analysis containing all children for a given AnalysisID defined via the relation in column AnalysisParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisChildNodes'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The AnalysisID for which the children should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The AnalysisID for which the children should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.ROUTINES t where t.ROUTINE_NAME = 'AnalysisHierarchy') > 0
begin

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Analysis containing all data for a given AnalysisID including the children defined via the relation in column AnalysisParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Analysis containing all data for a given AnalysisID including the children defined via the relation in column AnalysisParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisHierarchy'
END CATCH;

end
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Analysis including the hierarchy separated via "|" defined via the relation in column AnalysisParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisHierarchyAll'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Analysis including the hierarchy separated via "|" defined via the relation in column AnalysisParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisHierarchyAll'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the content of table Analysis related to the given project and a taxonomic group',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the content of table Analysis related to the given project and a taxonomic group',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the content of table Analysis available for a IdentificationUnit given by the IdentificationUnitID. The list depends upon the analysis types available for a taxonomic group and the projects available for an analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisListForUnit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the content of table Analysis available for a IdentificationUnit given by the IdentificationUnitID. The list depends upon the analysis types available for a taxonomic group and the projects available for an analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisListForUnit'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns content of table Analysis related to the given project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisProjectList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns content of table Analysis related to the given project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisProjectList'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the content of the table AnalysisTaxonomicGroup used in a project including the whole hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisTaxonomicGroupForProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the content of the table AnalysisTaxonomicGroup used in a project including the whole hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisTaxonomicGroupForProject'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ApplicationSearchItemPrimaryKeyColumn'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ApplicationSearchItemPrimaryKeyColumn'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The average altitude for the points of a geography where the z value is given ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AverageAltitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The average altitude for the points of a geography where the z value is given ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AverageAltitude'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The geography for which the average altitude should be extracted',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AverageAltitude',
@level2type=N'PARAMETER', @level2name=N'@Geography'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The geography for which the average altitude should be extracted',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AverageAltitude',
@level2type=N'PARAMETER', @level2name=N'@Geography'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the basic URL for the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'BaseURL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the basic URL for the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'BaseURL'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionChildNodes'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the collection (= CollectionID, PK)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the collection (= CollectionID, PK)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Series related to the given Series.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionEventSeriesHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Series related to the given Series.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionEventSeriesHierarchy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the analysis items related to the given analysis.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the analysis items related to the given analysis.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections including a display text with the whole hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchyAll'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections including a display text with the whole hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchyAll'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections related to the given collection in the list.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchyMulti'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections related to the given collection in the list.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchyMulti'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of IDs of the collections that should be included.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchyMulti',
@level2type=N'PARAMETER', @level2name=N'@CollectionIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of IDs of the collections that should be included.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchyMulti',
@level2type=N'PARAMETER', @level2name=N'@CollectionIDs'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists the given and all the items superior to the given collection',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchySuperior'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists the given and all the items superior to the given collection',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchySuperior'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The CollectionTaskID to which the inferior datasets are link to via column CollectionTaskParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The CollectionTaskID to which the inferior datasets are link to via column CollectionTaskParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the transaction to which the collection task is linked',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskCollectionHierarchyAll',
@level2type=N'COLUMN', @level2name=N'TransactionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the transaction to which the collection task is linked',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskCollectionHierarchyAll',
@level2type=N'COLUMN', @level2name=N'TransactionID'
END CATCH;


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Display text including the hierarchy of the collection separated by the string defined in function TaskCollectionHierarchySeparator followed by the hierarchy of the collection task separated by the string defined in function TaskHierarchySeparator followed by optional information about start, end and result of the collection task',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskCollectionHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Display text including the hierarchy of the collection separated by the string defined in function TaskCollectionHierarchySeparator followed by the hierarchy of the collection task separated by the string defined in function TaskHierarchySeparator followed by optional information about start, end and result of the collection task',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskCollectionHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The CollectionTaskID for which the hierarchy should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchy',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The CollectionTaskID for which the hierarchy should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchy',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the transaction to which the collection task is linked',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchy',
@level2type=N'COLUMN', @level2name=N'TransactionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the transaction to which the collection task is linked',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchy',
@level2type=N'COLUMN', @level2name=N'TransactionID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Display text of the task the collection task is linked to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'Task'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Display text of the task the collection task is linked to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'Task'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the transaction to which the collection task is linked',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'TransactionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the transaction to which the collection task is linked',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'TransactionID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'CollectionHierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'CollectionHierarchyDisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Display text including the hierarchy of the collection task separated by the string defined in function TaskHierarchySeparator followed by the hierarchy of the collection separated by the string defined in function TaskCollectionHierarchySeparator',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Display text including the hierarchy of the collection task separated by the string defined in function TaskHierarchySeparator followed by the hierarchy of the collection separated by the string defined in function TaskCollectionHierarchySeparator',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Display text including the hierarchy of the collection separated by the string defined in function TaskCollectionHierarchySeparator followed by the hierarchy of the collection task separated by the string defined in function TaskHierarchySeparator',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'CollectionHierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Display text including the hierarchy of the collection separated by the string defined in function TaskCollectionHierarchySeparator followed by the hierarchy of the collection task separated by the string defined in function TaskHierarchySeparator',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'CollectionHierarchyDisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'TaskHierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'TaskHierarchyDisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Display text of the collection task based on diverse content in tables Task and CollectionTask',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'TaskDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Display text of the collection task based on diverse content in tables Task and CollectionTask',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'TaskDisplayText'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The CollectionTaskID for which the parent hierarchy should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskParentNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The CollectionTaskID for which the parent hierarchy should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionTaskParentNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO



--#####################################################################################################################
--######   Procedures   ###############################################################################################
--#####################################################################################################################


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deleting a XML node in a column of datatype XML',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deleting a XML node in a column of datatype XML',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the table containing the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode',
@level2type=N'PARAMETER', @level2name=N'@Table'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the table containing the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode',
@level2type=N'PARAMETER', @level2name=N'@Table'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode',
@level2type=N'PARAMETER', @level2name=N'@Column'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode',
@level2type=N'PARAMETER', @level2name=N'@Column'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Path of the XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode',
@level2type=N'PARAMETER', @level2name=N'@Path'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Path of the XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode',
@level2type=N'PARAMETER', @level2name=N'@Path'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Where clause to select the data within the table',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode',
@level2type=N'PARAMETER', @level2name=N'@WhereClause'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Where clause to select the data within the table',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlNode',
@level2type=N'PARAMETER', @level2name=N'@WhereClause'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Copy a collection specimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimen2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Copy a collection specimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimen2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The CollectionSpecimenID of the CollectionSpecimen that should be copied',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimen2',
@level2type=N'PARAMETER', @level2name=N'@OriginalCollectionSpecimenID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The CollectionSpecimenID of the CollectionSpecimen that should be copied',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimen2',
@level2type=N'PARAMETER', @level2name=N'@OriginalCollectionSpecimenID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'3 Options: -1 = dont copy the event, leave the entry in table CollectionSpecimen empty; 0 =  take same event as original specimen; 1 =  create new event with the same data as the old specimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimen2',
@level2type=N'PARAMETER', @level2name=N'@EventCopyMode'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'3 Options: -1 = dont copy the event, leave the entry in table CollectionSpecimen empty; 0 =  take same event as original specimen; 1 =  create new event with the same data as the old specimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimen2',
@level2type=N'PARAMETER', @level2name=N'@EventCopyMode'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Contains list of tables that are copied according to the users choice',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimen2',
@level2type=N'PARAMETER', @level2name=N'@IncludedTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Contains list of tables that are copied according to the users choice',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimen2',
@level2type=N'PARAMETER', @level2name=N'@IncludedTables'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Copy a collection specimen part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimenPart'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Copy a collection specimen part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimenPart'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The SpecimenPartID of the CollectionSpecimenPart that should be copied',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimenPart',
@level2type=N'PARAMETER', @level2name=N'@OriginalSpecimenPartID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The SpecimenPartID of the CollectionSpecimenPart that should be copied',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimenPart',
@level2type=N'PARAMETER', @level2name=N'@OriginalSpecimenPartID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Contains list of tables that are copied according to the users choice',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimenPart',
@level2type=N'PARAMETER', @level2name=N'@IncludedTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Contains list of tables that are copied according to the users choice',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procCopyCollectionSpecimenPart',
@level2type=N'PARAMETER', @level2name=N'@IncludedTables'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Filling table CacheDescription',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procFillCacheDescription'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Filling table CacheDescription',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procFillCacheDescription'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Copy a collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertCollectionEventCopy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Copy a collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertCollectionEventCopy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The CollectionEventID of the CollectionEvent that should be copied',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertCollectionEventCopy',
@level2type=N'PARAMETER', @level2name=N'@OriginalCollectionEventID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The CollectionEventID of the CollectionEvent that should be copied',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertCollectionEventCopy',
@level2type=N'PARAMETER', @level2name=N'@OriginalCollectionEventID'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertCollectionSpecimenCopy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertCollectionSpecimenCopy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertCollectionSpecimenEventCopy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertCollectionSpecimenEventCopy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of a dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionCollectionEvent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of a dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionCollectionEvent'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionEventID of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionCollectionEvent',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionEventID of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionCollectionEvent',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of a dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionCollectionSpecimen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the version of a dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionCollectionSpecimen'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenID of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionCollectionSpecimen',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenID of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procSetVersionCollectionSpecimen',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects'
END CATCH;
GO




BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting a value of an XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting a value of an XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the table containing the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Table'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the table containing the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Table'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Column'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Column'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Path of the XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Path'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Path of the XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Path'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Attribute that should be set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Attribute'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Attribute that should be set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Attribute'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Where clause to select the data within the table',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@WhereClause'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Where clause to select the data within the table',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@WhereClause'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The value for the attribute that should be set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Value'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The value for the attribute that should be set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Value'
END CATCH;
GO




BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting a value of an XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting a value of an XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the table containing the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@Table'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the table containing the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@Table'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@Column'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@Column'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Path of the XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@Path'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Path of the XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@Path'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The value for the node that should be set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@Value'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The value for the node that should be set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@Value'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Where clause to select the data within the table',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@WhereClause'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Where clause to select the data within the table',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetXmlValue',
@level2type=N'PARAMETER', @level2name=N'@WhereClause'
END CATCH;
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.37'
END

GO


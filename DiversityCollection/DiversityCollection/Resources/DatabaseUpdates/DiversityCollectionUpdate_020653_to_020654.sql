declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.53'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Beschreibungen korrigieren  #################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'A context e.g. as defined in table EntityContext_Enum',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CacheDescription',
@level2type=N'COLUMN', @level2name=N'Context'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'A context e.g. as defined in table EntityContext_Enum',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CacheDescription',
@level2type=N'COLUMN', @level2name=N'Context'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The cached date of the collection event calculated from the entries in CollectionDay, -Month and -Year.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEvent',
@level2type=N'COLUMN', @level2name=N'CollectionDate'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The cached date of the collection event calculated from the entries in CollectionDay, -Month and -Year.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEvent',
@level2type=N'COLUMN', @level2name=N'CollectionDate'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Part of primary key, refers to unique ID for the table CollectionEvent (= foreign key)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventImage',
@level2type=N'COLUMN', @level2name=N'CollectionEventID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Part of primary key, refers to unique ID for the table CollectionEvent (= foreign key)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventImage',
@level2type=N'COLUMN', @level2name=N'CollectionEventID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the parameter tool. Refers to table Parameter (= foreign key and part of primary key)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventParameterValue',
@level2type=N'COLUMN', @level2name=N'ParameterID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the parameter tool. Refers to table Parameter (= foreign key and part of primary key)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventParameterValue',
@level2type=N'COLUMN', @level2name=N'ParameterID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes on the property of the collection site.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventProperty',
@level2type=N'COLUMN', @level2name=N'Notes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes on the property of the collection site.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventProperty',
@level2type=N'COLUMN', @level2name=N'Notes'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Part of primary key, refers to unique ID for the table CollectionEvent (= foreign key).',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventRegulation',
@level2type=N'COLUMN', @level2name=N'CollectionEventID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Part of primary key, refers to unique ID for the table CollectionEvent (= foreign key).',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventRegulation',
@level2type=N'COLUMN', @level2name=N'CollectionEventID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventSeriesImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventSeriesImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'For selection in e.g. pick lists: of several equal names only the name from the source with the lowest preferred sequence will be provided.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionExternalDatasource',
@level2type=N'COLUMN', @level2name=N'PreferredSequence'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'For selection in e.g. pick lists: of several equal names only the name from the source with the lowest preferred sequence will be provided.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionExternalDatasource',
@level2type=N'COLUMN', @level2name=N'PreferredSequence'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If this source should be disabled for selection of names e.g. in pick lists.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionExternalDatasource',
@level2type=N'COLUMN', @level2name=N'Disabled'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If this source should be disabled for selection of names e.g. in pick lists.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionExternalDatasource',
@level2type=N'COLUMN', @level2name=N'Disabled'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The order in which the images should be shown in an interface.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',
@level2type=N'COLUMN', @level2name=N'DisplayOrder'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The order in which the images should be shown in an interface.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',
@level2type=N'COLUMN', @level2name=N'DisplayOrder'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The description of the part. Cached value if DescriptionTermURI is used.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartDescription',
@level2type=N'COLUMN', @level2name=N'Description'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The description of the part. Cached value if DescriptionTermURI is used.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartDescription',
@level2type=N'COLUMN', @level2name=N'Description'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to an external data source like a web-service or the module DiversityScientificTerms where the description is documented.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartDescription',
@level2type=N'COLUMN', @level2name=N'DescriptionTermURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to an external data source like a web-service or the module DiversityScientificTerms where the description is documented.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartDescription',
@level2type=N'COLUMN', @level2name=N'DescriptionTermURI'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the parameter. Refers to table Parameter (= Foreign key and part of primary key).',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenProcessingMethodParameter',
@level2type=N'COLUMN', @level2name=N'ParameterID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the parameter. Refers to table Parameter (= Foreign key and part of primary key).',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenProcessingMethodParameter',
@level2type=N'COLUMN', @level2name=N'ParameterID'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Refers to table Identification: The sequence of the identifications.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenReference',
@level2type=N'COLUMN', @level2name=N'IdentificationSequence'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Refers to table Identification: The sequence of the identifications.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenReference',
@level2type=N'COLUMN', @level2name=N'IdentificationSequence'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accession number which has been assigned to the part of the specimen, e.g. in connection with a former inventory.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenTransaction',
@level2type=N'COLUMN', @level2name=N'AccessionNumber'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accession number which has been assigned to the part of the specimen, e.g. in connection with a former inventory.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenTransaction',
@level2type=N'COLUMN', @level2name=N'AccessionNumber'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of user to first enter (typed or imported) the data.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenTransaction',
@level2type=N'COLUMN', @level2name=N'LogInsertedBy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of user to first enter (typed or imported) the data.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenTransaction',
@level2type=N'COLUMN', @level2name=N'LogInsertedBy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionTaskImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link to a module containing further information on the person or institution holding the license.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionTaskImage',
@level2type=N'COLUMN', @level2name=N'LicenseHolderAgentURI'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the access of entity is restricted to e.g. read only or it can be edited without restrictions.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'EntityUsage',
@level2type=N'COLUMN', @level2name=N'Accessibility'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the access of entity is restricted to e.g. read only or it can be edited without restrictions.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'EntityUsage',
@level2type=N'COLUMN', @level2name=N'Accessibility'
END CATCH;
GO




BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the parameter tool. Refers to table Parameter (= Foreign key and part of primary key).',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnitAnalysisMethodParameter',
@level2type=N'COLUMN', @level2name=N'ParameterID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the parameter tool. Refers to table Parameter (= Foreign key and part of primary key).',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnitAnalysisMethodParameter',
@level2type=N'COLUMN', @level2name=N'ParameterID'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The geography where the organism resp. object was located according to WGS84, e.g. a point (latitude, longitude and altitude).',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnitGeoAnalysis',
@level2type=N'COLUMN', @level2name=N'Geography'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The geography where the organism resp. object was located according to WGS84, e.g. a point (latitude, longitude and altitude).',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'IdentificationUnitGeoAnalysis',
@level2type=N'COLUMN', @level2name=N'Geography'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The default measurement unit for the localisation system, e.g. m, geographic coordinates.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'LocalisationSystem',
@level2type=N'COLUMN', @level2name=N'DefaultMeasurementUnit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The default measurement unit for the localisation system, e.g. m, geographic coordinates.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'LocalisationSystem',
@level2type=N'COLUMN', @level2name=N'DefaultMeasurementUnit'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of a preceding transaction of a superior transaction, if transactions are organized in a hierarchy.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'ParentTransactionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of a preceding transaction of a superior transaction, if transactions are organized in a hierarchy.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'ParentTransactionID'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The source of the material within a transaction, e.g. an excavation.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'MaterialSource'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The source of the material within a transaction, e.g. an excavation.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'MaterialSource'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the collection from which the specimen were transferred, e.g. the donating collection of a gift.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'FromCollectionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the collection from which the specimen were transferred, e.g. the donating collection of a gift.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'FromCollectionID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the person or institution from which the specimen were transferred, e.g. the donator of a gift.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'FromTransactionPartnerName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the person or institution from which the specimen were transferred, e.g. the donator of a gift.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'FromTransactionPartnerName'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the collection to which the specimen were transferred, e.g. the receiver of a gift.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'ToCollectionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the collection to which the specimen were transferred, e.g. the receiver of a gift.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'ToCollectionID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the person or institution to which the specimen were transferred, e.g. the receiver of a gift.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'ToTransactionPartnerName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the person or institution to which the specimen were transferred, e.g. the receiver of a gift.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'ToTransactionPartnerName'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Actual end of the transaction after a prolongation when e.g. the date of return for a loan was prolonged by the owner.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'ActualEndDate'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Actual end of the transaction after a prolongation when e.g. the date of return for a loan was prolonged by the owner.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Transaction',
@level2type=N'COLUMN', @level2name=N'ActualEndDate'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'An identifier for the payment like a booking number or invoice number.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'Identifier'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'An identifier for the payment like a booking number or invoice number.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'Identifier'
END CATCH;
GO




BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the payment was not in the default currency as defined in TransactionCurrency, the amount of the payment in foreign currency.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'ForeignAmount'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the payment was not in the default currency as defined in TransactionCurrency, the amount of the payment in foreign currency.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'ForeignAmount'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the payment was not in the default currency as defined in TransactionCurrency, the foreign currency of the payment.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'ForeignCurrency'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the payment was not in the default currency as defined in TransactionCurrency, the foreign currency of the payment.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'ForeignCurrency'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to the source for further information about the payer, e.g. in the module DiversityAgents.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'PayerAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to the source for further information about the payer, e.g. in the module DiversityAgents.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'PayerAgentURI'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to the source for further information about the recipient of the payment, e.g. in the module DiversityAgents.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'RecipientAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to the source for further information about the recipient of the payment, e.g. in the module DiversityAgents.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TransactionPayment',
@level2type=N'COLUMN', @level2name=N'RecipientAgentURI'
END CATCH;
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.54'
END

GO
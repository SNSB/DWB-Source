declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.38'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Missing grants for definition   ############################################################################
--#####################################################################################################################

--solving import problem where the definition of table CollectionEventRegulation (as the only table with this problem) was not visible for the Editor
GRANT VIEW DEFINITION ON schema::[dbo] TO [Editor];

--#####################################################################################################################
--######   Missing descriptions in enums   ############################################################################
--#####################################################################################################################

--######   CollEventImageType_Enum   ############################################################################

UPDATE [dbo].[CollEventImageType_Enum]
   SET [Description] = 'audio record of e.g. the song of an observed bird'
 WHERE Code = 'audio' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventImageType_Enum]
   SET [Description] = 'photography from an elevated position e.g. an aerial drone'
 WHERE Code = 'aerial photography' AND ([Description] = '' OR  [Description] IS NULL)

GO


UPDATE [dbo].[CollEventImageType_Enum]
   SET [Description] = 'photography of a whole biotope where the specimen are observed resp. collected'
 WHERE Code = 'biotope photography' AND ([Description] = '' OR  [Description] IS NULL)

GO


UPDATE [dbo].[CollEventImageType_Enum]
   SET [Description] = 'photography of the landscape where the specimen are observed resp. collected'
 WHERE Code = 'landscape photography' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventImageType_Enum]
   SET [Description] = 'image of a map where the specimen are observed resp. collected'
 WHERE Code = 'map' AND ([Description] = '' OR  [Description] IS NULL)

GO

--######   CollEventSeriesImageType_Enum   ############################################################################

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'audio record of e.g. the song of an observed bird'
 WHERE Code = 'audio'

GO

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'photography from an elevated position e.g. an aerial drone'
 WHERE Code = 'aerial photography' AND ([Description] = '' OR  [Description] IS NULL)

GO


UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'photography of the landscape where the specimen are observed resp. collected'
 WHERE Code = 'landscape photography' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'image of a map where the specimen are observed resp. collected'
 WHERE Code = 'map' AND ([Description] = '' OR  [Description] IS NULL)

GO


--######   CollTransactionType_Enum   ############################################################################

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'if the objects included in the transaction are borrowed from another instution'
 WHERE Code = 'borrow' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'if the objects included in the transaction are part of an exchange with another instution'
 WHERE Code = 'exchange' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'if the objects included in the transaction are received as a gift'
 WHERE Code = 'gift' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'if the objects included in the transaction are included in an inventory'
 WHERE Code = 'inventory' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'if the objects included in the transaction are given as a loan to another instution'
 WHERE Code = 'loan' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'if the objects included in the transaction were purchased'
 WHERE Code = 'purchase' AND ([Description] = '' OR  [Description] IS NULL)

GO

UPDATE [dbo].[CollEventSeriesImageType_Enum]
   SET [Description] = 'if the objects included in the transaction are part of an external loan request'
 WHERE Code = 'request' AND ([Description] = '' OR  [Description] IS NULL)

GO

--#####################################################################################################################
--######   Missing descriptions   #####################################################################################
--#####################################################################################################################
--#####################################################################################################################
--######   Trigger  ###################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'updating the logging columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Annotation',
@level2type=N'TRIGGER', @level2name=N'trgInsAnnotation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'updating the logging columns',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Annotation',
@level2type=N'TRIGGER', @level2name=N'trgInsAnnotation'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting ReadOnly in dependence of locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectUser',
@level2type=N'TRIGGER', @level2name=N'trgInsProjectUser'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting ReadOnly in dependence of locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectUser',
@level2type=N'TRIGGER', @level2name=N'trgInsProjectUser'
END CATCH;
GO


--#####################################################################################################################
--######   Functions   ################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   FirstLinesEvent_2   ########################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collection event are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collection_event'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collection event are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collection_event'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The country where the collection event took place. Cached value derived from an geographic entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Country'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The country where the collection event took place. Cached value derived from an geographic entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Country'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes about the collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Collection_event_notes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes about the collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Collection_event_notes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Named_area'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Named_area'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'NamedAreaLocation2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'NamedAreaLocation2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove a link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_gazetteer'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove a link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_gazetteer'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Longitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Longitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Latitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Latitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Coordinates_accuracy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Coordinates_accuracy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for retrieval of WGS84 coordinates via e.g. GoogleMaps corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Link_to_GoogleMaps'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for retrieval of WGS84 coordinates via e.g. GoogleMaps corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Link_to_GoogleMaps'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Lower value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Altitude_from'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Lower value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Altitude_from'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Upper value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Altitude_to'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Upper value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Altitude_to'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Altitude_accuracy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Altitude_accuracy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'MTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'MTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Quadrant of TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Quadrant'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Quadrant of TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Quadrant'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_MTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_MTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Link_to_SamplingPlots'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Link_to_SamplingPlots'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserverd to remove link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_SamplingPlots'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserverd to remove link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_SamplingPlots'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Accuracy_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Accuracy_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Latitude_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Latitude_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Longitude_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Longitude_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Geographic region corresponding to LocalisationSystemID = 10',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Geographic_region'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Geographic region corresponding to LocalisationSystemID = 10',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Geographic_region'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Lithostratigraphy corresponding to LocalisationSystemID = 30',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Lithostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Lithostratigraphy corresponding to LocalisationSystemID = 30',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Lithostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Chronostratigraphy corresponding to LocalisationSystemID = 20',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Chronostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Chronostratigraphy corresponding to LocalisationSystemID = 20',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'Chronostratigraphy'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLatitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLatitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLongitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLongitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI of Geographic Region',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_GeographicRegionPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI of Geographic Region',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_GeographicRegionPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLatitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLatitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLongitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLongitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyHierarchyCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyHierarchyCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyHierarchyCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyHierarchyCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Altitude',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_AverageAltitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Altitude',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesEvent_2',
@level2type=N'COLUMN', @level2name=N'_AverageAltitudeCache'
END CATCH;
GO



--#####################################################################################################################
--######   FirstLinesPart_2   ################################################################################################
--#####################################################################################################################



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of AnalysisIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@AnalysisIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of AnalysisIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@AnalysisIDs'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The start date of the range for analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@AnalysisStartDate'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The start date of the range for analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@AnalysisStartDate'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The end date of the range for analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@AnalysisEndDate'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The end date of the range for analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@AnalysisEndDate'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The start date of the range for processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@ProcessingStartDate'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The start date of the range for processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@ProcessingStartDate'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The end date of the range for processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@ProcessingEndDate'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The end date of the range for processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'PARAMETER', @level2name=N'@ProcessingEndDate'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collection event are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collection_event'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collection event are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collection_event'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collector are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collector are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collector'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The country where the collection event took place. Cached value derived from an geographic entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Country'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The country where the collection event took place. Cached value derived from an geographic entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Country'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes about the collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Collection_event_notes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes about the collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Collection_event_notes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Named_area'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Named_area'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'NamedAreaLocation2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'NamedAreaLocation2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove a link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_gazetteer'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove a link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_gazetteer'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Longitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Longitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Latitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Latitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Coordinates_accuracy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Coordinates_accuracy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for retrieval of WGS84 coordinates via e.g. GoogleMaps corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_GoogleMaps'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for retrieval of WGS84 coordinates via e.g. GoogleMaps corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_GoogleMaps'
END CATCH;
GO






BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Lower value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Altitude_from'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Lower value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Altitude_from'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Upper value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Altitude_to'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Upper value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Altitude_to'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Altitude_accuracy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Altitude_accuracy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_Altitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_Altitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'MTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'MTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Quadrant of TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Quadrant'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Quadrant of TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Quadrant'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_MTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_MTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_SamplingPlots'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_SamplingPlots'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserverd to remove link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_SamplingPlots'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserverd to remove link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_SamplingPlots'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Accuracy_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Accuracy_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Latitude_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Latitude_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Longitude_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Longitude_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Geographic region corresponding to LocalisationSystemID = 10',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Geographic_region'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Geographic region corresponding to LocalisationSystemID = 10',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Geographic_region'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Lithostratigraphy corresponding to LocalisationSystemID = 30',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Lithostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Lithostratigraphy corresponding to LocalisationSystemID = 30',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Lithostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Chronostratigraphy corresponding to LocalisationSystemID = 20',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Chronostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Chronostratigraphy corresponding to LocalisationSystemID = 20',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Chronostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link for the first collector to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link for the first collector to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes about the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_about_collector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes about the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_about_collector'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Depositors_link_to_DiversityAgents'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Depositors_link_to_DiversityAgents'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_Depositor'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_Depositor'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityExsiccatae'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityExsiccatae'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_exsiccatae'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_exsiccatae'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Last identification of the related organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Related_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Last identification of the related organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Related_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Order of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Order_of_taxon'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Order of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Order_of_taxon'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Family of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Family_of_taxon'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Family of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Family_of_taxon'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Identifier of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Identifier_of_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Identifier of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Identifier_of_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Description_of_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Description of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Description_of_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityTaxonNames'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityTaxonNames'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column to remove link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_identification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column to remove link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_identification'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_identification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_identification'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Agent responsible for first identification of first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Agent responsible for first identification of first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Determiner'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents_for_determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents_for_determiner'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reservet to remove link for agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reservet to remove link for agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_determiner'
END CATCH;
GO

--######   Analysis_0   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_0'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_0'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_0'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_0'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_0'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_0'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_0'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_0'
END CATCH;
GO

--######   Analysis_1   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_1'
END CATCH;
GO

--######   Analysis_2   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_2'
END CATCH;
GO

--######   Analysis_3   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_3'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_3'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_3'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_3'
END CATCH;
GO

--######   Analysis_4   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_4'
END CATCH;
GO

--######   Analysis_5   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_5'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_5'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_5'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_5'
END CATCH;
GO

--######   Analysis_6   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_6'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_6'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_6'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_6'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_6'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_6'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_6'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_6'
END CATCH;
GO

--######   Analysis_7   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_7'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_7'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_7'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_7'
END CATCH;
GO

--######   Analysis_8   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_8'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_8'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_8'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_8'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_8'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_8'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_8'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_8'
END CATCH;
GO

--######   Analysis_9   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_9'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_9'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_9'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'AnalysisID_9'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_9'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_number_9'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_9'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Analysis_result_9'
END CATCH;
GO

--############################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accession number of part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Part_accession_number'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accession number of part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Part_accession_number'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the Collection',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Collection'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the Collection',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Collection'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for specimen part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_part'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for specimen part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Notes_for_part'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of identification unit in specimen part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Description_of_unit_in_part'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Description of identification unit in specimen part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Description_of_unit_in_part'
END CATCH;
GO

--######   Processing_1   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_1'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_1'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_1 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_1'
END CATCH;
GO

--######   Processing_2   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_2 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_2'
END CATCH;
GO

--######   Processing_3   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_3'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_3'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_3'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_3'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_3 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_3'
END CATCH;
GO

--######   Processing_4   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_4'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_4'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_4 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_4'
END CATCH;
GO

--######   Processing_5   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Date of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_date_5'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'ProcessingID_5'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Protocoll of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_Protocoll_5'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Duration of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_duration_5'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes of Processing_5 according to list of ProcessingIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'Processing_notes_5'
END CATCH;
GO

--############################################################################################


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Title of first transaction',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_Transaction'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Title of first transaction',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_Transaction'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If part is on loan according to first transaction',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'On_loan'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If part is on loan according to first transaction',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'On_loan'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLatitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLatitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLongitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLongitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI of Geographic Region',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_GeographicRegionPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI of Geographic Region',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_GeographicRegionPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLatitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLatitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLongitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLongitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyHierarchyCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyHierarchyCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyHierarchyCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyHierarchyCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Calculated average altitude as parsed from the location fields',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_AverageAltitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Calculated average altitude as parsed from the location fields',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesPart_2',
@level2type=N'COLUMN', @level2name=N'_AverageAltitudeCache'
END CATCH;
GO


--#####################################################################################################################
--######   FirstLinesSeries   ################################################################################################
--#####################################################################################################################



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesSeries',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesSeries',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END CATCH;
GO

--#####################################################################################################################
--######   FirstLinesUnit_4   ################################################################################################
--#####################################################################################################################



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of AnalysisIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'PARAMETER', @level2name=N'@AnalysisIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of AnalysisIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'PARAMETER', @level2name=N'@AnalysisIDs'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The start date of the range for analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'PARAMETER', @level2name=N'@AnalysisStartDate'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The start date of the range for analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'PARAMETER', @level2name=N'@AnalysisStartDate'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The end date of the range for analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'PARAMETER', @level2name=N'@AnalysisEndDate'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The end date of the range for analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'PARAMETER', @level2name=N'@AnalysisEndDate'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collection event are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collection_event'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collection event are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collection_event'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collector are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collector are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collector'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes about the collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Collection_event_notes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes about the collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Collection_event_notes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Named_area'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Named_area'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'NamedAreaLocation2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'NamedAreaLocation2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove a link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_gazetteer'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove a link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_gazetteer'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Longitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Longitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Latitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Latitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Coordinates_accuracy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Coordinates_accuracy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for retrieval of WGS84 coordinates via e.g. GoogleMaps corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_GoogleMaps'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for retrieval of WGS84 coordinates via e.g. GoogleMaps corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_GoogleMaps'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Lower value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Altitude_from'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Lower value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Altitude_from'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Upper value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Altitude_to'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Upper value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Altitude_to'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Altitude_accuracy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Altitude_accuracy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_Altitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_Altitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'MTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'MTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Quadrant of TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Quadrant'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Quadrant of TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Quadrant'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_MTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_MTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_SamplingPlots'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_SamplingPlots'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserverd to remove link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_SamplingPlots'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserverd to remove link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_SamplingPlots'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Accuracy_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Accuracy_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Latitude_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Latitude_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Longitude_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Longitude_of_sampling_plot'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Geographic region corresponding to LocalisationSystemID = 10',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Geographic_region'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Geographic region corresponding to LocalisationSystemID = 10',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Geographic_region'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Lithostratigraphy corresponding to LocalisationSystemID = 30',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Lithostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Lithostratigraphy corresponding to LocalisationSystemID = 30',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Lithostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Chronostratigraphy corresponding to LocalisationSystemID = 20',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Chronostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Chronostratigraphy corresponding to LocalisationSystemID = 20',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Chronostratigraphy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link for the first collector to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link for the first collector to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for removal of ink for the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_collector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for removal of ink for the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_collector'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes about the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_about_collector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes about the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_about_collector'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Depositors_link_to_DiversityAgents'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Depositors_link_to_DiversityAgents'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_Depositor'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_Depositor'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityExsiccatae'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityExsiccatae'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_exsiccatae'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_exsiccatae'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Last identification of the related organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Related_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Last identification of the related organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Related_organism'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Order of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Order_of_taxon'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Order of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Order_of_taxon'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Family of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Family_of_taxon'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Family of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Family_of_taxon'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Identifier of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Identifier_of_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Identifier of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Identifier_of_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Description_of_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Description of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Description_of_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityTaxonNames'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityTaxonNames'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column to remove link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_identification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column to remove link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_identification'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_identification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_identification'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Agent responsible for first identification of first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Agent responsible for first identification of first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Determiner'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents_for_determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents_for_determiner'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reservet to remove link for agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reservet to remove link for agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_determiner'
END CATCH;
GO


--######   Analysis_0   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_0'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_0'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_0'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_0'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_0'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_0'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_0'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis 0 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_0'
END CATCH;
GO

--######   Analysis_1   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_1'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_1 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_1'
END CATCH;
GO

--######   Analysis_2   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_2 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_2'
END CATCH;
GO

--######   Analysis_3   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_3'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_3'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_3'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_3 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_3'
END CATCH;
GO

--######   Analysis_4   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_4 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_4'
END CATCH;
GO

--######   Analysis_5   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_5'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_5'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_5'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_5 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_5'
END CATCH;
GO

--######   Analysis_6   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_6'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_6'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_6'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_6'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_6'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_6'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_6'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_6 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_6'
END CATCH;
GO

--######   Analysis_7   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_7'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_7'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_7'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_7 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_7'
END CATCH;
GO

--######   Analysis_8   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_8'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_8'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_8'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_8'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_8'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_8'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_8'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_8 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_8'
END CATCH;
GO

--######   Analysis_9   ######################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_9'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_9'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_9'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'AnalysisID_9'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_9'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_number_9'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_9'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Result of analysis_9 according to list of AnalysisIDs',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Analysis_result_9'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for specimen part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_part'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for specimen part',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_part'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Title of first transaction',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_Transaction'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Title of first transaction',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_Transaction'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If part is on loan according to first transaction',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'On_loan'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If part is on loan according to first transaction',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'On_loan'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLatitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLatitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLongitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude of WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_CoordinatesAverageLongitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for WGS84 Coordinates',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI of Geographic Region',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_GeographicRegionPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI of Geographic Region',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_GeographicRegionPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLatitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Latitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLatitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLongitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Longitude for Named Area',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_NamedAverageLongitudeCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyHierarchyCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Lithostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_LithostratigraphyPropertyHierarchyCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyHierarchyCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy for Chronostratigraphy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_ChronostratigraphyPropertyHierarchyCache'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Average Altitude',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_AverageAltitudeCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Average Altitude',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLinesUnit_4',
@level2type=N'COLUMN', @level2name=N'_AverageAltitudeCache'
END CATCH;
GO




BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'GatheringSuperiorList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'GatheringSuperiorList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'getFieldDescription'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'getFieldDescription'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Hierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Hierarchy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyChildNodes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchySuperiorList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchySuperiorList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyTopID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyTopID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ItemHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ItemHierarchy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ItemHierarchyChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ItemHierarchyChildNodes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ItemHierarchyTopID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ItemHierarchyTopID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ItemSuperiorList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ItemSuperiorList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyTopID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'HierarchyTopID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a Manager has access to, including the child collections.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ManagerCollectionList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a Manager has access to, including the child collections.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ManagerCollectionList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'MethodID for which the depending methods should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'MethodChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'MethodID for which the depending methods should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'MethodChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the method separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'MethodHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the method separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'MethodHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'MultiColumnQuery'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'MultiColumnQuery'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Namespace'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Namespace'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'returns next free accession number similar to given parameter. Assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453 with a leading string and a numeric end',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'NextFreeAccNr'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'returns next free accession number similar to given parameter. Assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453 with a leading string and a numeric end',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'NextFreeAccNr'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If specimen should be included in search',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'NextFreeAccNumber',
@level2type=N'PARAMETER', @level2name=N'@IncludeSpecimen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If specimen should be included in search',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'NextFreeAccNumber',
@level2type=N'PARAMETER', @level2name=N'@IncludeSpecimen'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If parts should be included in search',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'NextFreeAccNumber',
@level2type=N'PARAMETER', @level2name=N'@IncludePart'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If parts should be included in search',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'NextFreeAccNumber',
@level2type=N'PARAMETER', @level2name=N'@IncludePart'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Provides a link to common information about the DiversityWorkbench',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'PrivacyConsentInfo'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Provides a link to common information about the DiversityWorkbench',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'PrivacyConsentInfo'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the processings within a hierarchy starting at the topmost processing related to the given item',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the processings within a hierarchy starting at the topmost processing related to the given item',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingChildNodes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the parent processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the parent processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the analysis items related to the given processing.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the analysis items related to the given processing.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingHierarchy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the parent processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingHierarchy',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the parent processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingHierarchy',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Processings including their hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingHierarchyAll'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Processings including their hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingHierarchyAll'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the processings as DisplayText separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the processings as DisplayText separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the processing items related to the given part.Tthe list depends upon the processing types available for a material category and the projects available for a processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingListForPart'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the processing items related to the given part.Tthe list depends upon the processing types available for a material category and the projects available for a processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingListForPart'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the processings as DisplayText separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingListForPart',
@level2type=N'COLUMN', @level2name=N'DisplayTextHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the processings as DisplayText separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingListForPart',
@level2type=N'COLUMN', @level2name=N'DisplayTextHierarchy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Processing items related to the given project. ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingProjectList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Processing items related to the given project. ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProcessingProjectList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectChildNodes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'retrieval of the last update in data of a project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectDataLastChanges'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'retrieval of the last update in data of a project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectDataLastChanges'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a requester has access to, including the child collections if allowed',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'RequesterCollectionList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a requester has access to, including the child collections if allowed',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'RequesterCollectionList'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuColumns'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuColumns'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuForeignKeys'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuForeignKeys'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuInferiorTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuInferiorTables'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuOrderColumn'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuOrderColumn'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuPrimaryKeyColumns'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuPrimaryKeyColumns'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuSuperiorTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuSuperiorTables'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuTables'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuTableTabs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuTableTabs'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a stable identfier for a dataset. Relies on an entry in ProjectProxy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a stable identfier for a dataset. Relies on an entry in ProjectProxy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenID that should be included in the StableIdentifier',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenID that should be included in the StableIdentifier',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'IdentificationUnitID that should be included in the StableIdentifier',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier',
@level2type=N'PARAMETER', @level2name=N'@IdentificationUnitID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'IdentificationUnitID that should be included in the StableIdentifier',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier',
@level2type=N'PARAMETER', @level2name=N'@IdentificationUnitID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'SpecimenPartID that should be included in the StableIdentifier',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier',
@level2type=N'PARAMETER', @level2name=N'@SpecimenPartID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'SpecimenPartID that should be included in the StableIdentifier',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier',
@level2type=N'PARAMETER', @level2name=N'@SpecimenPartID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'TaskParentID for which all child nodes of a given Task should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaskChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'TaskParentID for which all child nodes of a given Task should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaskChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the task starting at top Task separated by string defined in dbo.TaskHierarchySeparator()',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the task starting at top Task separated by string defined in dbo.TaskHierarchySeparator()',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the task ending at top Task separated by string defined in dbo.TaskHierarchySeparator()',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayTextInvers'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the task ending at top Task separated by string defined in dbo.TaskHierarchySeparator()',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaskHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayTextInvers'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Taxonomic name used as base for transformation',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaxonWithQualifier',
@level2type=N'PARAMETER', @level2name=N'@Taxon'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Taxonomic name used as base for transformation',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaxonWithQualifier',
@level2type=N'PARAMETER', @level2name=N'@Taxon'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Qualifier used as base for transformation',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaxonWithQualifier',
@level2type=N'PARAMETER', @level2name=N'@Qualifier'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Qualifier used as base for transformation',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TaxonWithQualifier',
@level2type=N'PARAMETER', @level2name=N'@Qualifier'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TopCollectionEventID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TopCollectionEventID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the transactions within a hierarchy starting at the topmost transaction related to the given transaction.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the transactions within a hierarchy starting at the topmost transaction related to the given transaction.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'TransactionID of the topmost transaction within the hierarchy that should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'TransactionID of the topmost transaction within the hierarchy that should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item where the current user has no access according to the restriction in TransactionList.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodesAccess'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item where the current user has no access according to the restriction in TransactionList.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodesAccess'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'TransactionID of the topmost transaction within the hierarchy that should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodesAccess',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'TransactionID of the topmost transaction within the hierarchy that should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodesAccess',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If a user has access to this dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodesAccess',
@level2type=N'COLUMN', @level2name=N'Accessible'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If a user has access to this dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionChildNodesAccess',
@level2type=N'COLUMN', @level2name=N'Accessible'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the transactions related to the given transaction.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the transactions related to the given transaction.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the transactions related to the given transaction including the accessiblity.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAccess'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the transactions related to the given transaction including the accessiblity.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAccess'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If a user has access to this dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAccess',
@level2type=N'COLUMN', @level2name=N'Accessible'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If a user has access to this dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAccess',
@level2type=N'COLUMN', @level2name=N'Accessible'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the transactions including their hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAll'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the transactions including their hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAll'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the transaction starting at top transaction separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAll',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the transaction starting at top transaction separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAll',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the transaction starting at top transaction separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the transaction starting at top transaction separated by |',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'TransactionHierarchyAll',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a User has access to, including the child collections.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'UserCollectionList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a User has access to, including the child collections.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'UserCollectionList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Version'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Version'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Minimal version of the client compatible with the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'VersionClient'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Minimal version of the client compatible with the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'VersionClient'
END CATCH;
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.39'
END

GO


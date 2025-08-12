declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.35'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Missing descriptions   #####################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   Tables   ###################################################################################################
--#####################################################################################################################


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The types of an annotation as used in table Annotation',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnnotationType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The types of an annotation as used in table Annotation',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnnotationType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Anonyms for collectors of whom the names should not be published',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnonymCollector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Anonyms for collectors of whom the names should not be published',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnonymCollector'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The name of the collector, PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnonymCollector',
@level2type=N'COLUMN', @level2name=N'CollectorsName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The name of the collector, PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnonymCollector',
@level2type=N'COLUMN', @level2name=N'CollectorsName'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The anonymisation phrase for the collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnonymCollector',
@level2type=N'COLUMN', @level2name=N'Anonymisation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The anonymisation phrase for the collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnonymCollector',
@level2type=N'COLUMN', @level2name=N'Anonymisation'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The anonymisation phrase for the collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnonymCollector',
@level2type=N'COLUMN', @level2name=N'Anonymisation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The anonymisation phrase for the collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AnonymCollector',
@level2type=N'COLUMN', @level2name=N'Anonymisation'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the person responsible for the entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'ResponsibleName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the person responsible for the entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'ResponsibleName'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI of the person responsible for the entry as e.g. retrieved from DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'ResponsibleAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI of the person responsible for the entry as e.g. retrieved from DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'ResponsibleAgentURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI of the person responsible for the entry as e.g. retrieved from DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'ResponsibleAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI of the person responsible for the entry as e.g. retrieved from DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'ResponsibleAgentURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI of the person responsible for the entry as e.g. retrieved from DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'ResponsibleAgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI of the person responsible for the entry as e.g. retrieved from DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'ResponsibleAgentURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The types of the images taken from a collection event series',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollEventSeriesImageType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The types of the images taken from a collection event series',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollEventSeriesImageType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollExchangeType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollExchangeType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Type of visibility of an entity as used in table EntityUsage',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'EntityVisibility_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Type of visibility of an entity as used in table EntityUsage',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'EntityVisibility_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Databases providing data via replication',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ReplicationPublisher'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Databases providing data via replication',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ReplicationPublisher'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Data from DiversityWorkbench modules used used within a task',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Data from DiversityWorkbench modules used used within a task',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the task, part of PK, relates to PK of table Task',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'TaskID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the task, part of PK, relates to PK of table Task',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'TaskID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Display text as provided by the module, part of PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Display text as provided by the module, part of PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI linking the dataset of the module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'URI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI linking the dataset of the module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'URI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Optional description of the linked data, e.g. the common name for taxa',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'Description'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Optional description of the linked data, e.g. the common name for taxa',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'Description'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes related to the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'Notes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes related to the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'Notes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Point in time when this data set was created',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'LogCreatedWhen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Point in time when this data set was created',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'LogCreatedWhen'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the creator of this data set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'LogCreatedBy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the creator of this data set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'LogCreatedBy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Point in time when this data set was updated last',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'LogUpdatedWhen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Point in time when this data set was updated last',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'LogUpdatedWhen'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the person to update this data set last',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'LogUpdatedBy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the person to update this data set last',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskModule',
@level2type=N'COLUMN', @level2name=N'LogUpdatedBy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List or results provided for the CollectionTask of the corresponding type',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List or results provided for the CollectionTask of the corresponding type',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the task, part of PK, relates to PK of table Task',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'TaskID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the task, part of PK, relates to PK of table Task',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'TaskID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The result, part of PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'Result'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The result, part of PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'Result'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'A URI of the result providing further information',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'URI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'A URI of the result providing further information',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'URI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The description of the entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'Description'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The description of the entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'Description'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes about the entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'Notes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes about the entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'Notes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Point in time when this data set was created',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'LogCreatedWhen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Point in time when this data set was created',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'LogCreatedWhen'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the creator of this data set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'LogCreatedBy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the creator of this data set',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'LogCreatedBy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Point in time when this data set was updated last',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'LogUpdatedWhen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Point in time when this data set was updated last',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'LogUpdatedWhen'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the person to update this data set last',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'LogUpdatedBy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the person to update this data set last',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'TaskResult',
@level2type=N'COLUMN', @level2name=N'LogUpdatedBy'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'Tool') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Tools used within the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Tool'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Tools used within the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Tool'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ToolForAnalysis') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Tools available for an analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ToolForAnalysis'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Tools available for an analysis',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ToolForAnalysis'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ToolForProcessing') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Tools available for a processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ToolForProcessing'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Tools available for a processing',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ToolForProcessing'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. URI of a user in a remote module, e.g. refering to UserInfo.UserID in database DiversityUsers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'UserProxy',
@level2type=N'COLUMN', @level2name=N'UserURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. URI of a user in a remote module, e.g. refering to UserInfo.UserID in database DiversityUsers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'UserProxy',
@level2type=N'COLUMN', @level2name=N'UserURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URI of a agent in the module DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'UserProxy',
@level2type=N'COLUMN', @level2name=N'AgentURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URI of a agent in the module DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'UserProxy',
@level2type=N'COLUMN', @level2name=N'AgentURI'
END CATCH;
GO

--#####################################################################################################################
--######   Views   ####################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the collection event. Refers to PK of table CollectionEvent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationEvent',
@level2type=N'COLUMN', @level2name=N'CollectionEventID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the collection event. Refers to PK of table CollectionEvent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationEvent',
@level2type=N'COLUMN', @level2name=N'CollectionEventID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the collection specimen part. Refers to PK of table CollectionSpecimenPart',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationPart',
@level2type=N'COLUMN', @level2name=N'SpecimenPartID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the collection specimen part. Refers to PK of table CollectionSpecimenPart',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationPart',
@level2type=N'COLUMN', @level2name=N'SpecimenPartID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the collection specimen. Refers to PK of table CollectionSpecimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationSpecimen',
@level2type=N'COLUMN', @level2name=N'CollectionSpecimenID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the collection specimen. Refers to PK of table CollectionSpecimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationSpecimen',
@level2type=N'COLUMN', @level2name=N'CollectionSpecimenID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the IdentificationUnit. Refers to PK of table IdentificationUnit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationUnit',
@level2type=N'COLUMN', @level2name=N'IdentificationUnitID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the IdentificationUnit. Refers to PK of table IdentificationUnit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AnnotationUnit',
@level2type=N'COLUMN', @level2name=N'IdentificationUnitID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ApplicationSearchItemPrimaryKeyColumns'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ApplicationSearchItemPrimaryKeyColumns'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ApplicationSearchMenuFields'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ApplicationSearchMenuFields'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Restriction of the content of table ApplicationSearchSelectionStrings to the current user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ApplicationSearchSelectionStrings_Core'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Restriction of the content of table ApplicationSearchSelectionStrings to the current user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ApplicationSearchSelectionStrings_Core'
END CATCH;
GO



if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CacheDB_CollectionAgent') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionAgent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionAgent'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CacheDB_CollectionEvent') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionEvent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionEvent'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CacheDB_CollectionEventChronostratigraphy') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionEventChronostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionEventChronostratigraphy'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CacheDB_CollectionEventLithostratigraphy') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionEventLithostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionEventLithostratigraphy'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CacheDB_CollectionProject') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_CollectionProject'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CacheDB_Identification_First') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_Identification_First'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_Identification_First'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CacheDB_Identification_FirstEntry') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_Identification_FirstEntry'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_Identification_FirstEntry'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CacheDB_Identification_Last') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_Identification_Last'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CacheDB_Identification_Last'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Table CollectionAgent restricted to available datasets',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionAgent_Core'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Table CollectionAgent restricted to available datasets',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionAgent_Core'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. List for specimen with first 6 collector names',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionAgentCollectorsNameList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. List for specimen with first 6 collector names',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionAgentCollectorsNameList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List for accessible content of table CollectionEvent with preformated date column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEvent_Core2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List for accessible content of table CollectionEvent with preformated date column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEvent_Core2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventID_AvailableNotReadOnly'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventID_AvailableNotReadOnly'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List for CollectionEventID (PK of table CollectionEvent) a user can edit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventID_CanEdit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List for CollectionEventID (PK of table CollectionEvent) a user can edit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventID_CanEdit'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List for CollectionEventID (PK of table CollectionEvent) a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventID_UserAvailable'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List for CollectionEventID (PK of table CollectionEvent) a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventID_UserAvailable'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CollectionEventList') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List of accessible dataset of table CollectionSpecimen containing additional data from table CollectionEvent ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimen_Core2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List of accessible dataset of table CollectionSpecimen containing additional data from table CollectionEvent ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimen_Core2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'LocalityDescription from table CollectionEvent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimen_Core2',
@level2type=N'COLUMN', @level2name=N'Locality'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'LocalityDescription from table CollectionEvent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimen_Core2',
@level2type=N'COLUMN', @level2name=N'Locality'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'HabitatDescription from table CollectionEvent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimen_Core2',
@level2type=N'COLUMN', @level2name=N'Habitat'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'HabitatDescription from table CollectionEvent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimen_Core2',
@level2type=N'COLUMN', @level2name=N'Habitat'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has access to including CollectionSpecimenIDs not linked to a project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_Available'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has access to including CollectionSpecimenIDs not linked to a project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_Available'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has read only access to including CollectionSpecimenIDs from locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_AvailableReadOnly'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has read only access to including CollectionSpecimenIDs from locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_AvailableReadOnly'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has access to excluding those with read only access',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_CanEdit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has access to excluding those with read only access',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_CanEdit'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs of locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_Locked'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs of locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_Locked'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs of locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_Locked'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs of locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_Locked'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has read only access to including CollectionSpecimenIDs from locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_ReadOnly'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has read only access to including CollectionSpecimenIDs from locked projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_ReadOnly'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has access to including CollectionSpecimenIDs not linked to a project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_UserAvailable'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenIDs a user has access to including CollectionSpecimenIDs not linked to a project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenID_UserAvailable'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CollectionSpecimenList') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List of specimen where another specimen within the database has defined a relation to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenRelationInvers'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List of specimen where another specimen within the database has defined a relation to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenRelationInvers'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction with TransactionType = request a CollectionManager has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionForeignRequest'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction with TransactionType = request a CollectionManager has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionForeignRequest'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction with TransactionType loan or request a CollectionRequester has created and access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionUserRequest'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction with TransactionType loan or request a CollectionRequester has created and access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionUserRequest'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction with TransactionType loan or request the user has access to as a CollectionManager or the user has created and access to as a CollectionRequester ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionRequest'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction with TransactionType loan or request the user has access to as a CollectionManager or the user has created and access to as a CollectionRequester ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionRequest'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'PK and supplementary columns of Table CollectionSpecimenTransaction with TransactionType loan or request the user has access to as a CollectionManager or the user has created and access to as a CollectionRequester ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenTransactionRequest'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'PK and supplementary columns of Table CollectionSpecimenTransaction with TransactionType loan or request the user has access to as a CollectionManager or the user has created and access to as a CollectionRequester ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenTransactionRequest'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CollectorsNumber_Core') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectorsNumber_Core'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectorsNumber_Core'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CuratorCollectionList') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CuratorCollectionList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CuratorCollectionList'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CuratorSpecimenList') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CuratorSpecimenList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CuratorSpecimenList'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CuratorSpecimenPartList') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CuratorSpecimenPartList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CuratorSpecimenPartList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstCollectionAgent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstCollectionAgent'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Identification restricted to last identification of a IdentificationUnit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesIdentification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Identification restricted to last identification of a IdentificationUnit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesIdentification'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesAltitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesAltitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesChronostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesChronostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionAgent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionAgent'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionSpecimen'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionSpecimen'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionSpecimenOnLoan'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionSpecimenOnLoan'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionSpecimenPart'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionSpecimenPart'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionSpecimenTransaction'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCollectionSpecimenTransaction'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCoordinatesWGS84'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesCoordinatesWGS84'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesGeographicRegion'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesGeographicRegion'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesIdenitificationUnitDisplayOrder1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesIdenitificationUnitDisplayOrder1'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesIdentificationUnit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesIdentificationUnit'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesIdentificationUnitAnalysis'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesIdentificationUnitAnalysis'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesLithostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesLithostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesLoan'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesLoan'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesMTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesMTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesNamedArea'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesNamedArea'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesSamplingPlot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesSamplingPlot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesSecondUnit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesSecondUnit'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesSecondUnitIdentification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesSecondUnitIdentification'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesTransaction'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'FirstLinesTransaction'
END CATCH;
GO






--#####################################################################################################################
--######   Functions   ################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.ROUTINES t where t.ROUTINE_NAME = 'AgentID') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Returns true if the ID exists in the database DiversityAgent, else false.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Returns true if the ID exists in the database DiversityAgent, else false.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentID'
END CATCH;
GO


if (select count(*) from INFORMATION_SCHEMA.ROUTINES t where t.ROUTINE_NAME = 'AgentIDPresent') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentIDPresent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentIDPresent'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the collection with acronym if present, otherwise name',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionLocationAll',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the collection with acronym if present, otherwise name',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionLocationAll',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END CATCH;
GO


--#####################################################################################################################
--######   Procedures   ###############################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deleting an attribute of an XML node in a column of datatype XML',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deleting an attribute of an XML node in a column of datatype XML',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the table containing the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Table'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the table containing the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Table'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Column'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the XML column',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Column'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Path of the XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Path'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Path of the XML node',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Path'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Attribute that should be removed',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Attribute'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Attribute that should be removed',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@Attribute'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Where clause to select the data within the table',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@WhereClause'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Where clause to select the data within the table',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'DeleteXmlAttribute',
@level2type=N'PARAMETER', @level2name=N'@WhereClause'
END CATCH;
GO


--#####################################################################################################################
--######   Roles   ####################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role with the permission to place requests for loans',
@level0type = N'USER', @level0name = 'Requester';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role with the permission to place requests for loans',
@level0type = N'USER', @level0name = 'Requester';
END CATCH;
GO


--#####################################################################################################################
--######   Adding defaults in table TaskModule if missing  ############################################################
--#####################################################################################################################


IF object_id('DF_TaskModule_LogCreatedBy', 'D') IS NULL BEGIN
ALTER TABLE [dbo].TaskModule ADD  CONSTRAINT [DF_TaskModule_LogCreatedBy]  DEFAULT ([dbo].[UserID]()) FOR [LogCreatedBy]
END
GO

IF object_id('DF_TaskModule_LogUpdatedBy', 'D') IS NULL BEGIN
ALTER TABLE [dbo].TaskModule ADD  CONSTRAINT [DF_TaskModule_LogUpdatedBy]  DEFAULT ([dbo].[UserID]()) FOR [LogUpdatedBy]
END
GO


--#####################################################################################################################
--######   New entry Prometheus in TaskModuleType_Enum for IPM:   #####################################################
--#####################################################################################################################

if (select count(*) from TaskModuleType_Enum t where t.Code = 'Prometheus') = 0
begin
INSERT INTO [dbo].[TaskModuleType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Prometheus'
           ,'Prometheus time series database'
           ,'Prometheus'
           ,1)
end
GO


--#####################################################################################################################
--######   New column PropertyURI in table CollectionSpecimenImageProperty  ###########################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'CollectionSpecimenImageProperty' AND T.COLUMN_NAME = 'PropertyURI') = 0
begin
	ALTER TABLE CollectionSpecimenImageProperty ADD [PropertyURI] [varchar](500) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the property of the image, e.g. a link to module DiversityScientificTerms' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'PropertyURI'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = 'CollectionSpecimenImageProperty_log' AND T.COLUMN_NAME = 'PropertyURI') = 0
	ALTER TABLE CollectionSpecimenImageProperty_log ADD [PropertyURI] [varchar](500) NULL;
GO


ALTER TRIGGER [dbo].[trgDelCollectionSpecimenImageProperty] ON [dbo].[CollectionSpecimenImageProperty] 
FOR DELETE AS 
INSERT INTO CollectionSpecimenImageProperty_Log (CollectionSpecimenID, URI, Property, PropertyURI, Description, ImageArea, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.Property, deleted.PropertyURI, deleted.Description, deleted.ImageArea, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED
GO


ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenImageProperty] ON [dbo].[CollectionSpecimenImageProperty] 
FOR UPDATE AS
INSERT INTO CollectionSpecimenImageProperty_Log (CollectionSpecimenID, URI, Property, PropertyURI, Description, ImageArea, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.Property, deleted.PropertyURI, deleted.Description, deleted.ImageArea, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED
Update CollectionSpecimenImageProperty
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionSpecimenImageProperty, deleted 
where 1 = 1 
AND CollectionSpecimenImageProperty.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenImageProperty.Property = deleted.Property
AND CollectionSpecimenImageProperty.URI = deleted.URI
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.36'
END

GO


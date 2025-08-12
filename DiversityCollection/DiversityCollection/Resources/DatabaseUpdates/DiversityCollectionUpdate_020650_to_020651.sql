declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.50'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  ISSUE #101  ##################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  Missing GRANT SELECTS #####################################
--#####################################################################################################################

GRANT SELECT ON [dbo].[ManagerSpecimenPartList] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[RequesterSpecimenPartList] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[TransactionForeignRequest] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[TransactionUserRequest] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[TransactionRequest] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionEventSeriesDescriptor] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollEventSeriesDescriptorType_Enum] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionEventSeriesDescriptor_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionEventSeriesImage_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionExternalDatasource_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionSpecimenProcessingMethodParameter_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollEventSeriesDescriptorType_Enum] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[EntityAccessibility_Enum] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[ExternalIdentifierType_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[IdentificationUnitGeoAnalysis_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[LanguageCode_Enum] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[MeasurementUnit_Enum] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[Method_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Parameter_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectUser] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[PropertyType_Enum] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[TransactionPayment_log] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionSpecimenTransactionRequest] TO [User] AS [dbo]
GO
GRANT SELECT ON [dbo].[UserGroups] TO [User] AS [dbo]
GO


--#####################################################################################################################
--######   CollTaxonomicGroup_Enum. New entry organism #115   #########################################################
--#####################################################################################################################

if (select count(*) from [dbo].[CollTaxonomicGroup_Enum] where Code = 'organism') = 0
begin
	INSERT INTO [dbo].[CollTaxonomicGroup_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('organism'
           ,'organism'
           ,'organism'
           ,1)
		   end
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.51'
END

GO


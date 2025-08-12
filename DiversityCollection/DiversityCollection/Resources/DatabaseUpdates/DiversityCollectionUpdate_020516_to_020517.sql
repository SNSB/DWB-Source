--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

declare @Version varchar(10)
set @Version = (SELECT [dbo].[Version]())
IF (SELECT [dbo].[Version]()) <> '02.05.16'
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version 02.05.17. Current version = ' + @Version
RAISERROR (@Message, 18, 1) 
END
GO




--#####################################################################################################################
--######   [Collectionspecimenpart]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

ALTER TABLE CollectionSpecimenPart_log ALTER COLUMN Stock float
GO


--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO


INSERT INTO [CollIdentificationCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes]
           ,[ParentCode])
SELECT [Code]
      ,[Description]
      ,[DisplayText]
      ,[DisplayOrder]
      ,[DisplayEnable]
      ,[InternalNotes]
      ,[ParentCode]
  FROM [DiversityCollection_Test].[dbo].[CollIdentificationQualifier_Enum]
q where q.code like 'type %'
GO


UPDATE [Identification]
   SET [IdentificationCategory] = [IdentificationQualifier], [IdentificationQualifier] = NULL
 WHERE [IdentificationQualifier] like 'type %'
GO




delete q 
FROM [CollIdentificationQualifier_Enum]
q where q.code like 'type %'
GO



--#####################################################################################################################
--######   [Collectionspecimenpart]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

GRANT SELECT ON dbo.CollectionEventID_UserAvailable TO [User]
GO



--#####################################################################################################################
--######   [Collectionspecimenpart]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The exact location within the reference, e.g. pages, plates' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEvent', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geography of the localisation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventLocalisation', @level2type=N'COLUMN',@level2name=N'Geography'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the collection event series started' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeries', @level2type=N'COLUMN',@level2name=N'DateStart'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the collection event series ended' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeries', @level2type=N'COLUMN',@level2name=N'DateEnd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geography of the collection event series' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeries', @level2type=N'COLUMN',@level2name=N'Geography'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The exact location within the reference, e.g. pages, plates' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimen', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the collection specimen part (= part of Primary key). ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPart', @level2type=N'COLUMN',@level2name=N'SpecimenPartID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The exact location within the reference, e.g. pages, plates' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Identification', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default measurement unit for the characterisation system, e.g. pH' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DefaultMeasurementUnit'
GO




--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The images showing the collection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geographical position or region of an organism at a certain time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitGeoAnalysis'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The types of the analysis that are available for a project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAnalysis'
GO

--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the access of entity is resticted to e.g. read only or it can be edited without restrictions' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityUsage', @level2type=N'COLUMN',@level2name=N'Accessibility'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If a value is determined e.g. by the system or the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityUsage', @level2type=N'COLUMN',@level2name=N'Determination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the entity is visible or hidden from e.g. a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityUsage', @level2type=N'COLUMN',@level2name=N'Visibility'
GO



















































--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO


ALTER FUNCTION [dbo].[VersionClient] () RETURNS nvarchar(11) AS BEGIN RETURN '03.00.03.07' END
GO





--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.17'
END

GO


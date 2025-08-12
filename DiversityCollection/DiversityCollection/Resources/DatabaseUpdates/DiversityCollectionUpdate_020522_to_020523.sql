declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.22'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_JMRC
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


--#####################################################################################################################
--######   Part   ######################################################################################
--#####################################################################################################################


EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The label for a part of a specimen, e.g. "cone", or a number attached to a duplicate of a specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPart', @level2type=N'COLUMN',@level2name=N'PartSublabel'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'ID of the processing method. Refers to ProcessingID in table Processing (foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessing', @level2type=N'COLUMN',@level2name=N'ProcessingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional: If the dataset is related to a part of a specimen, the ID of a related part (= foreign key, see table CollectionSpecimenPart)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessing', @level2type=N'COLUMN',@level2name=N'SpecimenPartID'
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.23'
END

GO

select [dbo].[Version] ()  


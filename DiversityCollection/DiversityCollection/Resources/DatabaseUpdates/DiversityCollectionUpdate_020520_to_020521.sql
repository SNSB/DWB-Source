declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.20'
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
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
--use DiversityCollection_JMRC

GO


--#####################################################################################################################
--######   Annotation   ######################################################################################
--#####################################################################################################################

GRANT UPDATE ON Annotation TO [User]
GO


--#####################################################################################################################
--######    Rechte User   ######################################################################################
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

GRANT SELECT ON dbo.UserProxy To Administrator
GO

GRANT INSERT ON dbo.UserProxy To Administrator
GO

GRANT DELETE ON dbo.UserProxy To Administrator
GO

GRANT SELECT ON dbo.ProjectUser To Administrator
GO

GRANT INSERT ON dbo.ProjectUser To Administrator
GO

GRANT DELETE ON dbo.ProjectUser To Administrator
GO

EXEC sp_addrolemember 'db_accessadmin', 'Administrator';
GO

EXEC sp_addrolemember 'db_securityadmin', 'Administrator';
GO



--#####################################################################################################################
--######    Gruppe Replicator   ######################################################################################
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

/****** Object:  DatabaseRole [Editor]    Script Date: 04/27/2012 15:02:23 ******/
CREATE ROLE [Replicator] AUTHORIZATION [dbo]
GO

GRANT Alter On Analysis to Replicator;
GO

GRANT Alter On dbo.Annotation to Replicator;
GO

GRANT Alter On dbo.Collection to Replicator;
GO

GRANT Alter On CollectionEventSeries to Replicator;
GO

GRANT Alter On CollectionEvent to Replicator;
GO

GRANT Alter On CollectionSpecimen to Replicator;
GO

GRANT Alter On dbo.CollectionSpecimenPart to Replicator;
GO

GRANT Alter On dbo.IdentificationUnit to Replicator;
GO

GRANT Alter On dbo.Processing to Replicator;
GO

GRANT Alter On dbo.[Transaction] to Replicator;
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
--use DiversityCollection_JMRC
GO



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.21'
END

GO

select [dbo].[Version] ()  


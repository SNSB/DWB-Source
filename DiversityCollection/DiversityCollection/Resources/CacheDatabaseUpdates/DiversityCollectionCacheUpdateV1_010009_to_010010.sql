
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.09'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--#####################################################################################################################
--######      AnalysisCache        ######################################################################
--#####################################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'AnalysisCache') = 0
begin
	CREATE TABLE [dbo].[AnalysisCache](
		[AnalysisID] [int] NOT NULL,
		[AnalysisParentID] [int] NULL,
		[DisplayText] [nvarchar](50) NULL,
		[Description] [nvarchar](max) NULL,
		[MeasurementUnit] [nvarchar](50) NULL,
		[Notes] [nvarchar](max) NULL,
		[AnalysisURI] [varchar](255) NULL,
		[OnlyHierarchy] [bit] NULL CONSTRAINT [DF_AnalysisCache_OnlyHierarchy]  DEFAULT ((0)),
	 CONSTRAINT [PK_AnalysisCache] PRIMARY KEY CLUSTERED 
	(
		[AnalysisID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end


GRANT SELECT ON [AnalysisCache] TO [CollectionCacheUser]
GO
GRANT DELETE ON [AnalysisCache] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [AnalysisCache] TO [CacheAdministrator]
GO
GRANT INSERT ON [AnalysisCache] TO [CacheAdministrator]
GO




--#####################################################################################################################
--######   procTransferAnalysis      #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferAnalysis] 
AS
/*
-- TEST
EXECUTE  [dbo].[procTransferAnalysis]
*/
truncate table AnalysisCache

INSERT INTO [dbo].[AnalysisCache]
           ([AnalysisID]
		   ,[AnalysisParentID]
           ,[DisplayText]
           ,[Description]
           ,[MeasurementUnit]
           ,[Notes]
           ,[AnalysisURI]
           ,[OnlyHierarchy])
SELECT A.AnalysisID, A.AnalysisParentID, A.DisplayText, A.Description, A.MeasurementUnit, A.Notes, A.AnalysisURI, A.OnlyHierarchy
FROM ' + dbo.SourceDatebase() + '.dbo.Analysis AS A INNER JOIN
' + dbo.SourceDatebase() + '.dbo.ProjectAnalysis AS PA ON A.AnalysisID = PA.AnalysisID INNER JOIN
ProjectPublished AS P ON PA.ProjectID = P.ProjectID

DECLARE @U int
SET @U = (SELECT COUNT(*) FROM AnalysisCache)
SELECT CAST(@U AS VARCHAR) + '' analysis imported''')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO

GRANT EXECUTE ON [dbo].[procTransferAnalysis] TO [CacheAdministrator]
GO


--#####################################################################################################################
--#####################################################################################################################
--######      ProjectAnalysisCache        ######################################################################
--#####################################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectAnalysisCache') = 0
begin
	CREATE TABLE [dbo].[ProjectAnalysisCache](
		[AnalysisID] [int] IDENTITY(1,1) NOT NULL,
		[ProjectID] [int] NULL,
		[DisplayText] [nvarchar](50) NULL,
		[Description] [nvarchar](max) NULL,
		[MeasurementUnit] [nvarchar](50) NULL,
		[Notes] [nvarchar](max) NULL,
		[AnalysisURI] [varchar](255) NULL,
		[OnlyHierarchy] [bit] NULL CONSTRAINT [DF_ProjectAnalysisCache_OnlyHierarchy]  DEFAULT ((0)),
	 CONSTRAINT [PK_ProjectAnalysisCache] PRIMARY KEY CLUSTERED 
	(
		[AnalysisID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end


GRANT SELECT ON [ProjectAnalysisCache] TO [CollectionCacheUser]
GO
GRANT DELETE ON [ProjectAnalysisCache] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [ProjectAnalysisCache] TO [CacheAdministrator]
GO
GRANT INSERT ON [ProjectAnalysisCache] TO [CacheAdministrator]
GO




--#####################################################################################################################
--######   procTransferAnalysis      #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferAnalysis] 
AS
/*
-- TEST
EXECUTE  [dbo].[procTransferAnalysis]
*/
truncate table AnalysisCache

INSERT INTO [dbo].[AnalysisCache]
           ([AnalysisID]
		   ,[AnalysisParentID]
           ,[DisplayText]
           ,[Description]
           ,[MeasurementUnit]
           ,[Notes]
           ,[AnalysisURI]
           ,[OnlyHierarchy])
SELECT A.AnalysisID, A.AnalysisParentID, A.DisplayText, A.Description, A.MeasurementUnit, A.Notes, A.AnalysisURI, A.OnlyHierarchy
FROM ' + dbo.SourceDatebase() + '.dbo.Analysis AS A INNER JOIN
' + dbo.SourceDatebase() + '.dbo.ProjectAnalysis AS PA ON A.AnalysisID = PA.AnalysisID INNER JOIN
ProjectPublished AS P ON PA.ProjectID = P.ProjectID

DECLARE @U int
SET @U = (SELECT COUNT(*) FROM AnalysisCache)
SELECT CAST(@U AS VARCHAR) + '' analysis imported''')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO

GRANT EXECUTE ON [dbo].[procTransferAnalysis] TO [CacheAdministrator]
GO


--#####################################################################################################################
--######   IdentificationUnitAnalysisCache   ######################################################################################
--#####################################################################################################################



if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'IdentificationUnitAnalysisCache') = 0
begin
	CREATE TABLE [dbo].[IdentificationUnitAnalysisCache](
		[CollectionSpecimenID] [int] NOT NULL,
		[IdentificationUnitID] [int] NOT NULL,
		[AnalysisID] [int] NOT NULL,
		[AnalysisNumber] [nvarchar](50) NOT NULL,
		[AnalysisResult] [nvarchar](max) NULL,
		[ExternalAnalysisURI] [varchar](255) NULL,
		[ResponsibleName] [nvarchar](255) NULL,
		[ResponsibleAgentURI] [varchar](255) NULL,
		[AnalysisDate] [nvarchar](50) NULL,
		[SpecimenPartID] [int] NULL,
		[Notes] [nvarchar](max) NULL,
	 CONSTRAINT [PK_IdentificationUnitAnalysisCache] PRIMARY KEY CLUSTERED 
	(
		[CollectionSpecimenID] ASC,
		[IdentificationUnitID] ASC,
		[AnalysisID] ASC,
		[AnalysisNumber] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end


GRANT SELECT ON [IdentificationUnitAnalysisCache] TO [CollectionCacheUser]
GO
GRANT DELETE ON [IdentificationUnitAnalysisCache] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [IdentificationUnitAnalysisCache] TO [CacheAdministrator]
GO
GRANT INSERT ON [IdentificationUnitAnalysisCache] TO [CacheAdministrator]
GO



--#####################################################################################################################
--######   procTransferIdentificationUnitAnalysis      #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [dbo].[procTransferIdentificationUnitAnalysis] AS
begin

/*Loeschen der Daten in der Tabelle*/
truncate table IdentificationUnitAnalysisCache

/* Einlesen der Daten fuer Units ohne Part*/      
INSERT INTO [dbo].[IdentificationUnitAnalysisCache]
           ([CollectionSpecimenID]
           ,[IdentificationUnitID]
           ,[AnalysisID]
           ,[AnalysisNumber]
           ,[AnalysisResult]
           ,[ExternalAnalysisURI]
           ,[ResponsibleName]
           ,[ResponsibleAgentURI]
           ,[AnalysisDate]
           ,[SpecimenPartID]
           ,[Notes])
SELECT   DISTINCT     A.CollectionSpecimenID, A.IdentificationUnitID, A.AnalysisID, A.AnalysisNumber, A.AnalysisResult, A.ExternalAnalysisURI, A.ResponsibleName, 
                         A.ResponsibleAgentURI, A.AnalysisDate, A.SpecimenPartID, A.Notes
FROM            ' + dbo.SourceDatebase() + '.dbo.IdentificationUnitAnalysis AS A INNER JOIN
                         CollectionSpecimenCache AS S ON A.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN
                         IdentificationUnitPartCache AS U ON A.CollectionSpecimenID = U.CollectionSpecimenID AND A.IdentificationUnitID = U.IdentificationUnitID
WHERE        (A.SpecimenPartID IS NULL)

/* Einlesen der Daten fuer Units mit Part*/      
INSERT INTO [dbo].[IdentificationUnitAnalysisCache]
           ([CollectionSpecimenID]
           ,[IdentificationUnitID]
           ,[AnalysisID]
           ,[AnalysisNumber]
           ,[AnalysisResult]
           ,[ExternalAnalysisURI]
           ,[ResponsibleName]
           ,[ResponsibleAgentURI]
           ,[AnalysisDate]
           ,[SpecimenPartID]
           ,[Notes])
SELECT   DISTINCT     A.CollectionSpecimenID, A.IdentificationUnitID, A.AnalysisID, A.AnalysisNumber, A.AnalysisResult, A.ExternalAnalysisURI, A.ResponsibleName, 
                         A.ResponsibleAgentURI, A.AnalysisDate, A.SpecimenPartID, A.Notes
FROM            ' + dbo.SourceDatebase() + '.dbo.IdentificationUnitAnalysis AS A INNER JOIN
                         CollectionSpecimenCache AS S ON A.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN
                         IdentificationUnitPartCache AS U ON A.CollectionSpecimenID = U.CollectionSpecimenID AND A.IdentificationUnitID = U.IdentificationUnitID AND 
                         A.SpecimenPartID = U.SpecimenPartID INNER JOIN
                         CollectionSpecimenPartCache AS P ON A.CollectionSpecimenID = P.CollectionSpecimenID AND A.SpecimenPartID = P.SpecimenPartID
WHERE        (NOT (A.SpecimenPartID IS NULL))

DECLARE @U int
SET @U = (SELECT COUNT(*) FROM IdentificationUnitAnalysisCache)
SELECT CAST(@U AS VARCHAR) + '' analysis imported''
end')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO

GRANT EXECUTE ON [dbo].[procTransferIdentificationUnitAnalysis] TO [CacheAdministrator]
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.10'
END

GO



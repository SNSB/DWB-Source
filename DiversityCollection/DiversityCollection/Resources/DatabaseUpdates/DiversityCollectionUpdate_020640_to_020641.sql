declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.40'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   IdentifierSpecimen: Extension with column ReferencedTable to enable insert  ################################
--#####################################################################################################################


ALTER VIEW [dbo].[IdentifierSpecimen]
AS
SELECT        ID, ReferencedID AS CollectionSpecimenID, Type, Identifier, URL, Notes, ReferencedTable, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
FROM            ExternalIdentifier
WHERE        (ReferencedTable = N'CollectionSpecimen')

GO



--#####################################################################################################################
--######   CollectionEventLocalisation_Core: Excluding log columns and geography as String  ###########################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionEventLocalisation_Core]
AS
SELECT        CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, 
                         AverageLatitudeCache, AverageLongitudeCache, RecordingMethod, Geography.ToString() AS Geography
FROM            dbo.CollectionEventLocalisation
GO

GRANT SELECT ON [dbo].[CollectionEventLocalisation_Core] TO [User]
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionEventLocalisation excluding log columns and geography',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventLocalisation_Core'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table CollectionEventLocalisation excluding log columns and geography',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionEventLocalisation_Core'
END CATCH;
GO

--#####################################################################################################################
--######   CollectionSpecimenRelationInversList: Listing all internal relations in reverse perspective  ###############
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionSpecimenRelationInversList] ()   
RETURNS @TempCollectionSpecimenRelationInvers TABLE
([CollectionSpecimenID] [int] NOT NULL , 	
[RelatedSpecimenCollectionSpecimenID] [int] NOT NULL , 	
[RelatedSpecimenAccessionNumber] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[RelationType] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[RelatedSpecimenCollectionID] int NULL , 	
[Notes] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL)    
/* 
TEST: Select * from CollectionSpecimenRelationInversList()  
*/ 
AS 
BEGIN  
INSERT INTO @TempCollectionSpecimenRelationInvers            
([CollectionSpecimenID]
      ,[RelatedSpecimenCollectionSpecimenID]
      ,[RelatedSpecimenAccessionNumber]
      ,[RelationType]
      ,[RelatedSpecimenCollectionID]
      ,[Notes]) 
SELECT TOP 100 PERCENT 
cast(substring(R.RelatedSpecimenURI, len(dbo.baseURL()) + 1, 255) as int) AS CollectionSpecimenID,
R.CollectionSpecimenID AS RelatedSpecimenCollectionSpecimenID, 
S.AccessionNumber AS RelatedSpecimenAccessionNumber, 
R.RelationType, 
R.RelatedSpecimenCollectionID AS RelatedSpecimenCollectionID, 
R.Notes
FROM CollectionSpecimenRelation R, CollectionSpecimen S  
WHERE (R.IsInternalRelationCache = 1)  
AND S.CollectionSpecimenID = R.CollectionSpecimenID 
and not R.RelatedSpecimenURI IS NULL
and R.RelatedSpecimenURI like dbo.BaseURL() + '%'
and ISNUMERIC(substring(R.RelatedSpecimenURI, len(dbo.baseURL()) + 1, 255)) = 1
ORDER BY RelatedSpecimenDisplayText
RETURN 
END   
GO

GRANT SELECT ON [dbo].[CollectionSpecimenRelationInversList]  TO [User]
GO

if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'CollectionSpecimenRelationInversList') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Listing all internal relations in reverse perspective',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionSpecimenRelationInversList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Listing all internal relations in reverse perspective',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionSpecimenRelationInversList'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'CollectionSpecimenRelationInversList') > 0
 and (select count(*) from INFORMATION_SCHEMA.ROUTINE_COLUMNS c where c.TABLE_NAME = 'CollectionSpecimenRelationInversList' and c.COLUMN_NAME = 'RelatedSpecimenCollectionSpecimenID') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The CollectionSpecimenID of the dataset that refers to the current dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionSpecimenRelationInversList',
@level2type=N'COLUMN', @level2name=N'RelatedSpecimenCollectionSpecimenID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The CollectionSpecimenID of the dataset that refers to the current dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionSpecimenRelationInversList',
@level2type=N'COLUMN', @level2name=N'RelatedSpecimenCollectionSpecimenID'
END CATCH;
GO


if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'CollectionSpecimenRelationInversList') > 0
 and (select count(*) from INFORMATION_SCHEMA.ROUTINE_COLUMNS c where c.TABLE_NAME = 'CollectionSpecimenRelationInversList' and c.COLUMN_NAME = 'RelatedSpecimenAccessionNumber') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The AccessionNumber of the dataset that refers to the current dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionSpecimenRelationInversList',
@level2type=N'COLUMN', @level2name=N'RelatedSpecimenAccessionNumber'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The AccessionNumber of the dataset that refers to the current dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionSpecimenRelationInversList',
@level2type=N'COLUMN', @level2name=N'RelatedSpecimenAccessionNumber'
END CATCH;
GO


--#####################################################################################################################
--######   CollectionSpecimenRelationInvers: Content of function CollectionSpecimenRelationInversList  ################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionSpecimenRelationInvers]
AS 
Select [CollectionSpecimenID]
      ,[RelatedSpecimenCollectionSpecimenID]
      ,[RelatedSpecimenAccessionNumber]
      ,[RelationType]
      ,[RelatedSpecimenCollectionID]
      ,[Notes] from CollectionSpecimenRelationInversList()
/*
SELECT TOP 100 PERCENT cast(substring(R.RelatedSpecimenURI, len(dbo.baseURL()) + 1, 255) as int) AS CollectionSpecimenID,
R.CollectionSpecimenID AS RelatedSpecimenCollectionSpecimenID, S.AccessionNumber AS RelatedSpecimenAccessionNumber, 
R.RelationType, R.RelatedSpecimenCollectionID AS RelatedSpecimenCollectionID, 
R.Notes
FROM CollectionSpecimenRelation R, CollectionSpecimen S  
WHERE (R.IsInternalRelationCache = 1)  
AND S.CollectionSpecimenID = R.CollectionSpecimenID 
and not R.RelatedSpecimenURI IS NULL
and R.RelatedSpecimenURI like dbo.BaseURL() + '%'
ORDER BY RelatedSpecimenDisplayText
*/
GO

if (select count(*) from INFORMATION_SCHEMA.VIEWS r where r.TABLE_NAME = 'CollectionSpecimenRelationInvers') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of function CollectionSpecimenRelationInversList: List of specimen where another specimen within the database has defined a relation to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenRelationInvers'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of function CollectionSpecimenRelationInversList: List of specimen where another specimen within the database has defined a relation to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenRelationInvers'
END CATCH;
GO


--#####################################################################################################################
--######   CollectionSpecimenRelationInternal: All internal relations irrespective of the direction  ##################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionSpecimenRelationInternal]
AS
SELECT TOP 100 PERCENT [CollectionSpecimenID],
cast(substring(R.RelatedSpecimenURI, len(dbo.baseURL()) + 1, 255) as int) AS RelatedSpecimen_CollectionSpecimenID
,[RelatedSpecimenDisplayText]
,[RelationType]
,[RelatedSpecimenCollectionID]
,[RelatedSpecimenDescription]
,[Notes]
FROM [CollectionSpecimenRelation] R
WHERE ISNUMERIC(substring(R.RelatedSpecimenURI, len(dbo.baseURL()) + 1, 255)) = 1
AND R.IsInternalRelationCache = 1
UNION
SELECT 
cast(substring(R.RelatedSpecimenURI, len(dbo.baseURL()) + 1, 255) as int) AS CollectionSpecimenID,
R.CollectionSpecimenID AS RelatedSpecimen_CollectionSpecimenID, 
S.AccessionNumber AS [RelatedSpecimenDisplayText], 
R.RelationType, 
R.RelatedSpecimenCollectionID, 
R.[RelatedSpecimenDescription],
R.Notes
FROM CollectionSpecimenRelation R, CollectionSpecimen S  
WHERE (R.IsInternalRelationCache = 1)  
AND S.CollectionSpecimenID = R.CollectionSpecimenID 
and not R.RelatedSpecimenURI IS NULL
and R.RelatedSpecimenURI like dbo.BaseURL() + '%'
and ISNUMERIC(substring(R.RelatedSpecimenURI, len(dbo.baseURL()) + 1, 255)) = 1
and R.IsInternalRelationCache = 1
         
GO

GRANT SELECT ON [dbo].[CollectionSpecimenRelationInternal] TO [User]
GO


if (select count(*) from INFORMATION_SCHEMA.VIEWS r where r.TABLE_NAME = 'CollectionSpecimenRelationInternal') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List of specimen with a relation to a specimen within the same database or where another specimen within the database has defined a relation to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenRelationInternal'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List of specimen with a relation to a specimen within the same database or where another specimen within the database has defined a relation to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenRelationInternal'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.VIEWS r where r.TABLE_NAME = 'CollectionSpecimenRelationInternal') > 0
 and (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CollectionSpecimenRelationInternal' and c.COLUMN_NAME = 'RelatedSpecimen_CollectionSpecimenID') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenID of the related specimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenRelationInternal',
@level2type=N'COLUMN', @level2name=N'RelatedSpecimen_CollectionSpecimenID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'CollectionSpecimenID of the related specimen',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'CollectionSpecimenRelationInternal',
@level2type=N'COLUMN', @level2name=N'RelatedSpecimen_CollectionSpecimenID'
END CATCH;
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.41'
END

GO


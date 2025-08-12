
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.19'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Missing GRANTS ##############################################################################################
--#####################################################################################################################


EXEC sp_addrolemember N'db_owner', N'CacheAdmin'



--#####################################################################################################################
--######   extending views with LogUpdatedWhen to enable comparision  #################################################
--#####################################################################################################################



--#####################################################################################################################
--######   ViewAnalysis   #############################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAnalysis]
AS
SELECT DISTINCT A.[AnalysisID]
,[AnalysisParentID]
,[DisplayText]
,[Description]
,[MeasurementUnit]
,[Notes]
,[AnalysisURI]
,[OnlyHierarchy]
,A.[LogUpdatedWhen]
FROM            
' +  dbo.SourceDatabase() + '.[dbo].[ProjectAnalysis] P,  ' +  dbo.SourceDatabase() + '.dbo.Analysis AS A 
where P.AnalysisID = A.AnalysisID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   ViewAnnotation   ###########################################################################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAnnotation]
AS
SELECT DISTINCT A.AnnotationID, A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI, 
A.SourceDisplayText, A.SourceURI, A.IsInternal, A.ReferencedID, A.ReferencedTable, A.LogUpdatedWhen
FROM ' +  dbo.SourceDatabase() + '.dbo.Annotation AS A')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewAnnotation') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all annotations' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewAnnotation'
GO

GRANT SELECT ON ViewAnnotation TO [CacheUser]
GO


--#####################################################################################################################
--######   ViewCollection   ###########################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollection]
AS
SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, LogUpdatedWhen
FROM            
' +  dbo.SourceDatabase() + '.[dbo].[Collection]')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollection') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collections' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollection'
GO

GRANT SELECT ON ViewCollection TO [CacheUser]
GO




--#####################################################################################################################
--######   ViewCollectionAgent  LogUpdatedWhen         ################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ViewCollectionAgent]
AS
SELECT    DISTINCT     TOP (100) PERCENT A.CollectionSpecimenID, A.CollectorsName, A.CollectorsSequence, A.CollectorsNumber, A.CollectorsAgentURI, A.LogUpdatedWhen
FROM            dbo.ViewCollectionSpecimen AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionAgent AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID
WHERE        (A.DataWithholdingReason = N'''') OR
                         (A.DataWithholdingReason IS NULL)
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 7, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   ViewCollectionEvent   ######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionEvent]
AS
SELECT   DISTINCT     E.CollectionEventID, E.Version, E.CollectorsEventNumber, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionDate END AS CollectionDate, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionDay END AS CollectionDay, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionMonth END AS CollectionMonth, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionYear END AS CollectionYear, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionDateSupplement END AS CollectionDateSupplement, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionTime END AS CollectionTime, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionTimeSpan END AS CollectionTimeSpan, 
E.LocalityDescription, E.HabitatDescription, E.ReferenceTitle, E.CollectingMethod, E.Notes, E.CountryCache, 
E.ReferenceDetails, E.LocalityVerbatim, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionEndDay END AS CollectionEndDay, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionEndMonth END AS CollectionEndMonth, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionEndYear END AS CollectionEndYear
, E.LogUpdatedWhen
FROM dbo.ViewCollectionSpecimen AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID
WHERE        (E.DataWithholdingReason = '''') OR
(E.DataWithholdingReason IS NULL)')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   ViewCollectionEventLocalisation LogUpdatedWhen  ############################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ViewCollectionEventLocalisation]
AS
SELECT   DISTINCT     L.CollectionEventID, L.LocalisationSystemID, L.Location1, L.Location2, L.LocationAccuracy, L.LocationNotes, L.DeterminationDate, L.DistanceToLocation, 
L.DirectionToLocation, L.ResponsibleName, L.ResponsibleAgentURI, L.AverageAltitudeCache, L.AverageLatitudeCache, L.AverageLongitudeCache, 
L.RecordingMethod, L.[Geography].ToString() AS [Geography], L.LogUpdatedWhen
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionEventLocalisation AS L INNER JOIN
dbo.ViewCollectionEvent AS E ON L.CollectionEventID = E.CollectionEventID
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 7, 80000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   ViewCollectionEventProperty   ##############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionEventProperty]
AS
SELECT    DISTINCT    P.CollectionEventID, P.PropertyID, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.ResponsibleName, P.ResponsibleAgentURI, 
P.Notes, P.AverageValueCache, P.LogUpdatedWhen
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionEventProperty AS P INNER JOIN
dbo.ViewCollectionEvent AS E ON P.CollectionEventID = E.CollectionEventID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   ViewCollectionProject   ####################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionProject]
AS
SELECT        C.CollectionSpecimenID, C.ProjectID, C.LogUpdatedWhen
FROM            dbo.ViewCollectionSpecimen INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionProject AS C INNER JOIN
dbo.ProjectPublished AS PP ON C.ProjectID = PP.ProjectID ON dbo.ViewCollectionSpecimen.CollectionSpecimenID = C.CollectionSpecimenID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   ViewCollectionSpecimen   ###################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimen]
AS
SELECT    DISTINCT    S.CollectionSpecimenID, LabelTranscriptionNotes, OriginalNotes, S.LogUpdatedWhen, CollectionEventID, AccessionNumber, AccessionDate, AccessionDay, 
AccessionMonth, AccessionYear, DepositorsName, DepositorsAccessionNumber, ExsiccataURI, ExsiccataAbbreviation, AdditionalNotes, ReferenceTitle, 
ReferenceURI, ExternalDatasourceID
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimen AS S INNER JOIN
dbo.ProjectPublished AS PP INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionProject AS P ON PP.ProjectID = P.ProjectID ON S.CollectionSpecimenID = P.CollectionSpecimenID
WHERE        (DataWithholdingReason = '''' OR
DataWithholdingReason IS NULL) AND (S.CollectionSpecimenID NOT IN
(SELECT        CST.CollectionSpecimenID
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenTransaction AS CST INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.[Transaction] AS TR ON CST.TransactionID = TR.TransactionID
WHERE        (TR.TransactionType = N''embargo'') AND (TR.BeginDate IS NULL OR
TR.BeginDate <= GETDATE()) AND (TR.AgreedEndDate IS NULL OR
TR.AgreedEndDate >= GETDATE())))')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   ViewCollectionExternalDatasource  ##########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionExternalDatasource]
AS
SELECT   E.ExternalDatasourceID, E.ExternalDatasourceName, E.ExternalDatasourceVersion, E.Rights, E.ExternalDatasourceAuthors, E.ExternalDatasourceURI, 
E.ExternalDatasourceInstitution, E.InternalNotes, E.ExternalAttribute_NameID, E.PreferredSequence, E.Disabled, E.LogUpdatedWhen
FROM            dbo.ViewCollectionSpecimen C INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionExternalDatasource E ON E.ExternalDatasourceID = C.ExternalDatasourceID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionExternalDatasource') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all external datasources' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionExternalDatasource'
GO

GRANT SELECT ON ViewCollectionExternalDatasource TO [CacheUser]
GO

--#####################################################################################################################
--######   ViewCollectionSpecimenImage   ##############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenImage]
AS
SELECT        I.CollectionSpecimenID, I.URI, I.ResourceURI, I.SpecimenPartID, I.IdentificationUnitID, I.ImageType, I.Notes, I.LicenseURI, I.LicenseNotes, I.DisplayOrder, 
I.LicenseYear, I.LicenseHolderAgentURI, I.LicenseHolder, I.LicenseType, I.CopyrightStatement, I.CreatorAgentURI, I.CreatorAgent, I.IPR, I.Title, I.LogUpdatedWhen
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenImage AS I 
WHERE        (I.DataWithholdingReason IS NULL OR
I.DataWithholdingReason = N'''')')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   ViewCollectionSpecimenPart   ###############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenPart]
AS
SELECT   DISTINCT      P.SpecimenPartID, P.DerivedFromSpecimenPartID, P.PreparationMethod, P.PreparationDate, P.PartSublabel, P.CollectionID, P.MaterialCategory, P.StorageLocation, 
P.Stock, P.Notes, P.CollectionSpecimenID, P.AccessionNumber, P.StorageContainer, P.StockUnit, P.ResponsibleName, P.LogUpdatedWhen
FROM            dbo.ViewCollectionSpecimen AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenPart AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID
WHERE        (P.DataWithholdingReason IS NULL) OR
(P.DataWithholdingReason = N'''')')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   ViewCollectionSpecimenProcessing   #########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ViewCollectionSpecimenProcessing]
AS
SELECT   DISTINCT   P.CollectionSpecimenID, P.SpecimenProcessingID, P.ProcessingDate, P.ProcessingID, P.Protocoll, P.SpecimenPartID, P.ProcessingDuration, P.ResponsibleName, 
P.ResponsibleAgentURI, P.Notes, P.LogUpdatedWhen
FROM            dbo.ViewCollectionSpecimenPart AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenProcessing AS P 
ON S.CollectionSpecimenID = P.CollectionSpecimenID
AND S.SpecimenPartID = P.SpecimenPartID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   ViewCollectionSpecimenReference  ###########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenReference]
AS
SELECT DISTINCT R.CollectionSpecimenID, R.ReferenceID, R.ReferenceTitle, R.ReferenceURI, R.IdentificationUnitID, 
R.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI, R.LogUpdatedWhen
FROM dbo.ViewCollectionSpecimen AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenReference AS R ON S.CollectionSpecimenID = R.CollectionSpecimenID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionSpecimenReference') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld references' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionSpecimenReference'
GO

GRANT SELECT ON ViewCollectionSpecimenReference TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewCollectionSpecimenRelation   ###########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenRelation]
AS
SELECT   DISTINCT      R.CollectionSpecimenID, R.RelatedSpecimenURI, R.RelatedSpecimenDisplayText, R.RelationType, R.RelatedSpecimenCollectionID, 
R.RelatedSpecimenDescription, R.Notes, R.IdentificationUnitID, R.SpecimenPartID, R.LogUpdatedWhen
FROM            dbo.ViewCollectionSpecimen AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenRelation AS R ON S.CollectionSpecimenID = R.CollectionSpecimenID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   ViewExternalIdentifier   ###################################################################################
--#####################################################################################################################




declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewExternalIdentifier]
AS
SELECT DISTINCT E.ID, E.Type, E.Identifier, E.URL, E.Notes, E.ReferencedTable, E.ReferencedID, E.LogUpdatedWhen
FROM ' +  dbo.SourceDatabase() + '.dbo.ExternalIdentifier AS E')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewExternalIdentifier') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all external identifiers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewExternalIdentifier'
GO

GRANT SELECT ON ViewExternalIdentifier TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewIdentification   #######################################################################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewIdentification]
AS
SELECT DISTINCT 
CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, 
IdentificationCategory, IdentificationQualifier, VernacularTerm, TaxonomicName, NameURI, Notes, TypeStatus, TypeNotes, ReferenceTitle, ReferenceDetails, 
ResponsibleName, LogUpdatedWhen
FROM ' +  dbo.SourceDatabase() + '.dbo.Identification AS I')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   ViewIdentificationUnit   ###################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ViewIdentificationUnit]
AS
SELECT  DISTINCT       U.CollectionSpecimenID, U.IdentificationUnitID, U.LastIdentificationCache, U.TaxonomicGroup, U.RelatedUnitID, U.RelationType, U.ExsiccataNumber, 
U.DisplayOrder, U.ColonisedSubstratePart, U.FamilyCache, U.OrderCache, U.LifeStage, U.Gender, U.HierarchyCache, U.UnitIdentifier, U.UnitDescription, 
U.Circumstances, U.Notes, U.NumberOfUnits, U.OnlyObserved, U.RetrievalType, U.LogUpdatedWhen
FROM            ' +  dbo.SourceDatabase() + '.dbo.IdentificationUnit AS U INNER JOIN
dbo.ViewCollectionSpecimen AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID
WHERE        (U.DataWithholdingReason IS NULL) OR
(U.DataWithholdingReason = N'''')')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   ViewIdentificationUnitAnalysis   ###########################################################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewIdentificationUnitAnalysis]
AS
SELECT   DISTINCT      A.AnalysisID, A.AnalysisNumber, A.AnalysisResult, A.ExternalAnalysisURI, A.ResponsibleName, A.ResponsibleAgentURI, A.AnalysisDate, A.SpecimenPartID, 
A.Notes, A.CollectionSpecimenID, A.IdentificationUnitID, A.LogUpdatedWhen
FROM            dbo.ViewIdentificationUnit AS U INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.IdentificationUnitAnalysis AS A ON U.CollectionSpecimenID = A.CollectionSpecimenID AND U.IdentificationUnitID = A.IdentificationUnitID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   ViewIdentificationUnitGeoAnalysis   ########################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewIdentificationUnitGeoAnalysis]
AS
SELECT   DISTINCT    A.CollectionSpecimenID, A.IdentificationUnitID, A.AnalysisDate, A.[Geography].ToString() AS [Geography], A.Geometry.ToString() AS Geometry, A.ResponsibleName, A.ResponsibleAgentURI, A.Notes, A.LogUpdatedWhen
FROM            dbo.ViewIdentificationUnit AS U INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.IdentificationUnitGeoAnalysis AS A ON  U.CollectionSpecimenID = A.CollectionSpecimenID AND U.IdentificationUnitID = A.IdentificationUnitID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewIdentificationUnitGeoAnalysis') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld geo analysis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewIdentificationUnitGeoAnalysis'
GO

GRANT SELECT ON ViewIdentificationUnitGeoAnalysis TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewIdentificationUnitInPart   #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewIdentificationUnitInPart]
AS
SELECT  DISTINCT       I.Description, I.CollectionSpecimenID, I.IdentificationUnitID, I.SpecimenPartID, I.DisplayOrder, I.LogUpdatedWhen
FROM            dbo.ViewCollectionSpecimenPart AS P INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.IdentificationUnitInPart AS I ON P.SpecimenPartID = I.SpecimenPartID INNER JOIN
dbo.ViewIdentificationUnit AS U ON I.IdentificationUnitID = U.IdentificationUnitID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   ViewMetadata   #############################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewMetadata]
AS
SELECT  DISTINCT  ProjectID, SettingID, Value, LogUpdatedWhen
FROM  ' +  dbo.ProjectsDatabase() + '.dbo.ProjectSetting')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewMetadata') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for metadata' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewMetadata'
GO

GRANT SELECT ON ViewMetadata TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewProcessing   ###########################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ViewProcessing]
AS
SELECT DISTINCT P.ProcessingID, P.ProcessingParentID, P.DisplayText, P.Description, P.Notes, P.ProcessingURI, P.OnlyHierarchy, P.LogUpdatedWhen
FROM ' +  dbo.SourceDatabase() + '.dbo.Processing AS P INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.ProjectProcessing AS PP 
ON P.ProcessingID = PP.ProcessingID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



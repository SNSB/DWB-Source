
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.03'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--#####################################################################################################################
--######   Views for published data   ###################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   ViewCollectionSpecimen   ###################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimen]
AS
SELECT    DISTINCT    S.CollectionSpecimenID, LabelTranscriptionNotes, OriginalNotes, S.LogUpdatedWhen, CollectionEventID, AccessionNumber, AccessionDate, AccessionDay, 
AccessionMonth, AccessionYear, DepositorsName, DepositorsAccessionNumber, ExsiccataURI, ExsiccataAbbreviation, AdditionalNotes, ReferenceTitle, 
ReferenceURI
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


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionSpecimen') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionSpecimen'
GO


GRANT SELECT ON ViewCollectionSpecimen TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewCollectionAgent   ######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionAgent]
AS
SELECT    DISTINCT     TOP (100) PERCENT A.CollectionSpecimenID, A.CollectorsName, A.CollectorsSequence, A.CollectorsNumber
FROM            dbo.ViewCollectionSpecimen AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionAgent AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID
WHERE        (A.DataWithholdingReason = N'''') OR
                         (A.DataWithholdingReason IS NULL)')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionAgent') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collectors' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionAgent'
GO


GRANT SELECT ON ViewCollectionAgent TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewCollectionEvent   ######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionEvent]
AS
SELECT   DISTINCT     E.CollectionEventID, E.Version, E.CollectorsEventNumber, E.CollectionDate, E.CollectionDay, E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, 
E.CollectionTime, E.CollectionTimeSpan, E.LocalityDescription, E.HabitatDescription, E.ReferenceTitle, E.CollectingMethod, E.Notes, E.CountryCache, 
E.ReferenceDetails, E.LocalityVerbatim, E.CollectionEndDay, E.CollectionEndMonth, E.CollectionEndYear
FROM            dbo.ViewCollectionSpecimen AS S INNER JOIN
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



IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionEvent') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collection events' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionEvent'
GO


GRANT SELECT ON ViewCollectionEvent TO [CacheUser]
GO


--#####################################################################################################################
--######   ViewCollectionEventLocalisation   ##########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionEventLocalisation]
AS
SELECT   DISTINCT     L.CollectionEventID, L.LocalisationSystemID, L.Location1, L.Location2, L.LocationAccuracy, L.LocationNotes, L.DeterminationDate, L.DistanceToLocation, 
L.DirectionToLocation, L.ResponsibleName, L.ResponsibleAgentURI, L.AverageAltitudeCache, L.AverageLatitudeCache, L.AverageLongitudeCache, 
L.RecordingMethod
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionEventLocalisation AS L INNER JOIN
dbo.ViewCollectionEvent AS E ON L.CollectionEventID = E.CollectionEventID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionEventLocalisation') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld localisations' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionEventLocalisation'
GO


GRANT SELECT ON ViewCollectionEventLocalisation TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewCollectionEventProperty   ##############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionEventProperty]
AS
SELECT    DISTINCT    P.CollectionEventID, P.PropertyID, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.ResponsibleName, P.ResponsibleAgentURI, 
P.Notes, P.AverageValueCache
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


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionEventProperty') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collection site properties' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionEventProperty'
GO


GRANT SELECT ON ViewCollectionEventProperty TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewCollectionProject   ####################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionProject]
AS
SELECT        C.CollectionSpecimenID, C.ProjectID
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


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionProject') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collection specimen within the projects' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionProject'
GO

GRANT SELECT ON ViewCollectionProject TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewCollectionSpecimenPart   ###############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenPart]
AS
SELECT   DISTINCT      P.SpecimenPartID, P.DerivedFromSpecimenPartID, P.PreparationMethod, P.PreparationDate, P.PartSublabel, P.CollectionID, P.MaterialCategory, P.StorageLocation, 
P.Stock, P.Notes, P.CollectionSpecimenID, P.AccessionNumber, P.StorageContainer, P.StockUnit, P.ResponsibleName
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


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionSpecimenPart') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collection specimen parts' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionSpecimenPart'
GO

GRANT SELECT ON ViewCollectionSpecimenPart TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewCollectionSpecimenProcessing   #########################################################################
--#####################################################################################################################

-- erst nach korrektur der aufbaus

--declare @SQL nvarchar(max)
--set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenProcessing]
--AS
--SELECT   DISTINCT      P.CollectionSpecimenID, P.ProcessingDate, P.ProcessingID, P.Protocoll, P.ProcessingDuration, S.ResponsibleName, S.Notes, P.SpecimenPartID, P.Notes AS Expr1, 
--P.ResponsibleName AS Expr2
--FROM            dbo.ViewCollectionSpecimenPart AS S INNER JOIN
--' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenProcessing AS P ON S.SpecimenPartID = P.SpecimenPartID')

--begin try
--exec sp_executesql @SQL
--end try
--begin catch
--set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
--exec sp_executesql @SQL
--end catch

--GO

--IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionSpecimenProcessing') AND [name] = N'MS_Description' AND [minor_id] = 0)
--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collection specimen processings' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionSpecimenProcessing'
--GO

--GRANT SELECT ON ViewCollectionSpecimenProcessing TO [CacheUser]
--GO




--#####################################################################################################################
--######   ViewCollectionSpecimenRelation   ###########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenRelation]
AS
SELECT   DISTINCT      R.CollectionSpecimenID, R.RelatedSpecimenURI, R.RelatedSpecimenDisplayText, R.RelationType, R.RelatedSpecimenCollectionID, 
R.RelatedSpecimenDescription, R.Notes, R.IdentificationUnitID, R.SpecimenPartID
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


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionSpecimenRelation') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collection specimen processings' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionSpecimenRelation'
GO

GRANT SELECT ON ViewCollectionSpecimenRelation TO [CacheUser]
GO




--#####################################################################################################################
--######   ViewIdentificationUnit   ###################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewIdentificationUnit]
AS
SELECT  DISTINCT       U.CollectionSpecimenID, U.IdentificationUnitID, U.LastIdentificationCache, U.TaxonomicGroup, U.RelatedUnitID, U.RelationType, U.ExsiccataNumber, 
U.DisplayOrder, U.ColonisedSubstratePart, U.FamilyCache, U.OrderCache, U.LifeStage, U.Gender, U.HierarchyCache, U.UnitIdentifier, U.UnitDescription, 
U.Circumstances, U.Notes, U.NumberOfUnits, U.OnlyObserved
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


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewIdentificationUnit') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld identification units' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewIdentificationUnit'
GO

GRANT SELECT ON ViewIdentificationUnit TO [CacheUser]
GO


--#####################################################################################################################
--######   ViewIdentification   #########################################################################################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewIdentification]
AS
SELECT DISTINCT 
                         CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, 
                         IdentificationCategory, IdentificationQualifier, VernacularTerm, TaxonomicName, NameURI, Notes, TypeStatus, TypeNotes, ReferenceTitle, ReferenceDetails, 
                         ResponsibleName
FROM            ' +  dbo.SourceDatabase() + '.dbo.Identification AS I')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewIdentification') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all identifications' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewIdentification'
GO

GRANT SELECT ON ViewIdentification TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewTaxononmy   #########################################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewTaxononmy]
AS
SELECT   DISTINCT      I.CollectionSpecimenID, I.IdentificationUnitID, I.IdentificationSequence, I.IdentificationDay, I.IdentificationMonth, I.IdentificationYear, 
I.IdentificationDateSupplement, I.IdentificationCategory, I.IdentificationQualifier, I.VernacularTerm, I.TaxonomicName, I.NameURI, I.Notes, 
I.TypeStatus, I.TypeNotes, I.ReferenceTitle, I.ReferenceDetails, /*T.TaxonName,*/ T.AcceptedName, 
T.TaxonomicRank, T.NameURI AS AcceptedNameURI, CASE WHEN T .GenusOrSupragenericName IS NULL THEN CASE WHEN I.TaxonomicName IS NULL OR
len(rtrim(I.TaxonomicName)) = 0 THEN NULL ELSE CASE WHEN charindex('' '', I.TaxonomicName) = 0 THEN I.TaxonomicName ELSE substring(I.TaxonomicName COLLATE DATABASE_DEFAULT, 1, 
charindex('' '', I.TaxonomicName)) END END ELSE T .GenusOrSupragenericName END AS GenusOrSupragenericName, CASE WHEN T .TaxonNameSinAuthor IS NULL 
THEN CASE WHEN I.TaxonomicName IS NULL OR')

set @SQL = (@SQL + ' len(rtrim(I.TaxonomicName)) = 0 THEN NULL ELSE CASE WHEN I.IdentificationQualifier IN (''sp.'', ''sp. nov.'', ''spp.'') OR
T .TaxonomicRank = ''gen.'' THEN CASE WHEN charindex('' '', I.TaxonomicName) = 0 THEN I.TaxonomicName + '' sp.'' ELSE substring(I.TaxonomicName COLLATE DATABASE_DEFAULT, 1, 
charindex('' '', I.TaxonomicName)) + ''sp.'' END ELSE CASE WHEN charindex('' '', I.TaxonomicName) 
= 0 THEN I.TaxonomicName + '' sp.'' ELSE substring(I.TaxonomicName, 1, charindex('' '', I.TaxonomicName) - 1) + substring(I.TaxonomicName, charindex('' '', 
I.TaxonomicName), CASE WHEN charindex('' '', I.TaxonomicName, charindex('' '', I.TaxonomicName) + 1) > charindex('' '', I.TaxonomicName) THEN charindex('' '', 
I.TaxonomicName, charindex('' '', I.TaxonomicName) + 1) - charindex('' '', I.TaxonomicName) ELSE len(I.TaxonomicName) END) 
END END END ELSE T .TaxonNameSinAuthor END AS TaxonNameSinAuthor, CASE WHEN T .AcceptedName IS NULL 
THEN CASE WHEN I.TaxonomicName IS NULL OR')

set @SQL = (@SQL + ' I.TaxonomicName = '''' THEN I.VernacularTerm ELSE I.TaxonomicName COLLATE DATABASE_DEFAULT END ELSE T .AcceptedName END AS ValidTaxonName, 
CASE WHEN T .AcceptedName IS NULL THEN CASE WHEN I.IdentificationQualifier IS NULL OR
I.IdentificationQualifier = '''' THEN I.TaxonomicName ELSE CASE WHEN I.IdentificationQualifier = ''?'' THEN I.IdentificationQualifier + '' '' + I.TaxonomicName ELSE CASE WHEN
I.IdentificationQualifier IN (''sp.'', ''sp. nov.'', ''spp.'') THEN CASE WHEN CHARINDEX('' '', I.TaxonomicName) 
= 0 THEN I.TaxonomicName ELSE substring(I.TaxonomicName, 1, charindex('' '', I.TaxonomicName) - 1) 
END + '' '' + I.IdentificationQualifier ELSE CASE WHEN (I.IdentificationQualifier LIKE ''cf.%'' OR
I.IdentificationQualifier LIKE ''aff.%'') THEN CASE WHEN I.IdentificationQualifier LIKE ''% gen.'' THEN substring(I.IdentificationQualifier, 1, charindex(''.'', 
I.IdentificationQualifier)) + '' '' + CASE WHEN charindex('' '', I.TaxonomicName) = 0 THEN I.TaxonomicName ELSE substring(I.TaxonomicName COLLATE DATABASE_DEFAULT, 1, charindex('' '', 
I.TaxonomicName)) END ELSE CASE WHEN I.IdentificationQualifier LIKE ''% sp.'' OR')

set @SQL = (@SQL + ' I.IdentificationQualifier LIKE ''% ssp.'' THEN rtrim(substring(I.TaxonomicName, 1, charindex('' '', I.TaxonomicName)) + substring(I.IdentificationQualifier, 1, charindex(''.'', 
I.IdentificationQualifier)) + substring(I.TaxonomicName, charindex('' '', I.TaxonomicName), len(I.TaxonomicName))) 
ELSE CASE WHEN I.IdentificationQualifier LIKE ''% var.'' AND patindex(''%var.%'', I.TaxonomicName) > 0 THEN rtrim(substring(I.TaxonomicName, 1, patindex(''%var.%'', 
I.TaxonomicName) - 1) + I.IdentificationQualifier + substring(I.TaxonomicName, patindex(''%var.%'', I.TaxonomicName) + 4, len(I.TaxonomicName))) 
ELSE CASE WHEN I.IdentificationQualifier LIKE ''% fm.'' AND patindex(''%fm.%'', I.TaxonomicName) > 0 THEN rtrim(substring(I.TaxonomicName, 1, patindex(''%fm.%'', 
I.TaxonomicName) - 1) + I.IdentificationQualifier + substring(I.TaxonomicName, patindex(''%fm.%'', I.TaxonomicName) + 3, len(I.TaxonomicName))) 
ELSE I.TaxonomicName END END END END ELSE I.TaxonomicName END END END END ELSE CASE WHEN I.IdentificationQualifier IS NULL OR')

set @SQL = (@SQL + ' I.IdentificationQualifier = '''' THEN T .AcceptedName ELSE CASE WHEN I.IdentificationQualifier = ''?'' THEN I.IdentificationQualifier + '' '' + T .AcceptedName ELSE CASE WHEN
I.IdentificationQualifier IN (''sp.'', ''sp. nov.'', ''spp.'') THEN CASE WHEN CHARINDEX('' '', T .AcceptedName) = 0 THEN T .AcceptedName ELSE substring(T .AcceptedName,
1, charindex('' '', T .AcceptedName) - 1) END + '' '' + I.IdentificationQualifier ELSE CASE WHEN (I.IdentificationQualifier LIKE ''cf.%'' OR
I.IdentificationQualifier LIKE ''aff.%'') THEN CASE WHEN I.IdentificationQualifier LIKE ''% gen.'' THEN substring(I.IdentificationQualifier, 1, charindex(''.'', 
I.IdentificationQualifier)) + '' '' + substring(T .AcceptedName, 1, charindex('' '', T .AcceptedName)) ELSE CASE WHEN I.IdentificationQualifier LIKE ''% sp.'' OR
I.IdentificationQualifier LIKE ''% ssp.'' THEN rtrim(substring(T .AcceptedName, 1, charindex('' '', T .AcceptedName)) + substring(I.IdentificationQualifier, 1, charindex(''.'', 
I.IdentificationQualifier)) + substring(T .AcceptedName, charindex('' '', I.TaxonomicName), len(T .AcceptedName))) 
ELSE CASE WHEN I.IdentificationQualifier LIKE ''% var.'' AND patindex(''%var.%'', T .AcceptedName) > 0 THEN rtrim(substring(T .AcceptedName, 1, patindex(''%var.%'', 
T .AcceptedName) - 1) + I.IdentificationQualifier + substring(T .AcceptedName, patindex(''%var.%'', T .AcceptedName) + 4, len(T .AcceptedName))) 
ELSE CASE WHEN I.IdentificationQualifier LIKE ''% fm.'' AND patindex(''%fm.%'', T .AcceptedName) > 0 THEN rtrim(substring(T .AcceptedName, 1, patindex(''%fm.%'', 
T .AcceptedName) - 1) + I.IdentificationQualifier + substring(T .AcceptedName, patindex(''%fm.%'', T .AcceptedName) + 3, len(T .AcceptedName))) 
ELSE T .AcceptedName END END END END ELSE I.TaxonomicName END END END END END AS ValidTaxonWithQualifier, 
CASE WHEN T .TaxonNameSinAuthor IS NULL THEN CASE WHEN I.TaxonomicName IS NULL OR ')

set @SQL = (@SQL + 'len(rtrim(I.TaxonomicName)) = 0 THEN NULL ELSE CASE WHEN charindex('' '', I.TaxonomicName) = 0 THEN I.TaxonomicName ELSE substring(I.TaxonomicName COLLATE DATABASE_DEFAULT, 1, 
charindex('' '', I.TaxonomicName) - 1) + substring(I.TaxonomicName, charindex('' '', I.TaxonomicName), CASE WHEN charindex('' '', I.TaxonomicName, charindex('' '', 
I.TaxonomicName) + 1) > charindex('' '', I.TaxonomicName) THEN charindex('' '', I.TaxonomicName, charindex('' '', I.TaxonomicName) + 1) - charindex('' '', 
I.TaxonomicName) ELSE len(I.TaxonomicName) END) END END ELSE T .TaxonNameSinAuthor END AS ValidTaxonNameSinAuthor, 
CASE WHEN T .AcceptedName IS NULL THEN CASE WHEN I.IdentificationQualifier IN (''sp.'', ''cf. gen.'') OR
T .TaxonomicRank = ''gen.'' THEN CASE WHEN CHARINDEX('' '', I.TaxonomicName) = 0 THEN I.TaxonomicName + '' sp.'' ELSE substring(I.TaxonomicName COLLATE DATABASE_DEFAULT, 1, 
charindex('' '', I.TaxonomicName) - 1) + '' sp.'' END ELSE I.TaxonomicName END ELSE CASE WHEN I.IdentificationQualifier IN (''sp.'', ''cf. gen.'') OR
T .TaxonomicRank = ''gen.'' THEN CASE WHEN CHARINDEX('' '', T .AcceptedName) = 0 THEN T .AcceptedName + '' sp.'' ELSE substring(T .AcceptedName, 1, 
charindex('' '', T .AcceptedName) - 1) + '' sp.'' END ELSE T .AcceptedName END END AS ValidTaxonIndex, CASE WHEN I.IdentificationQualifier IN (''sp.'', 
''cf. gen.'') OR
T .TaxonomicRank = ''gen.'' THEN 1 ELSE 0 END AS ValidTaxonListOrder, I.ResponsibleName
FROM            dbo.TaxonSynonymy AS T RIGHT OUTER JOIN
' +  dbo.SourceDatabase() + '.dbo.Identification AS I INNER JOIN
dbo.ViewIdentificationUnit AS U ON I.IdentificationUnitID = U.IdentificationUnitID ON T.NameURI = I.NameURI COLLATE DATABASE_DEFAULT')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 8000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewTaxononmy') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all identifications including the accepted names and synonyms as derived from the table TaxonSynonymy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewTaxononmy'
GO

GRANT SELECT ON ViewTaxononmy TO [CacheUser]
GO




--#####################################################################################################################
--######   ViewAnalysis   #########################################################################################################
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


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewAnalysis') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for analysis used in the published data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewAnalysis'
GO

GRANT SELECT ON ViewAnalysis TO [CacheUser]
GO




--#####################################################################################################################
--######   ViewIdentificationUnitAnalysis   #########################################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewIdentificationUnitAnalysis]
AS
SELECT   DISTINCT      A.AnalysisID, A.AnalysisNumber, A.AnalysisResult, A.ExternalAnalysisURI, A.ResponsibleName, A.ResponsibleAgentURI, A.AnalysisDate, A.SpecimenPartID, 
U.Notes, A.CollectionSpecimenID, A.IdentificationUnitID, A.Notes AS Expr1
FROM            dbo.ViewIdentificationUnit AS U INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.IdentificationUnitAnalysis AS A ON U.IdentificationUnitID = A.IdentificationUnitID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewIdentificationUnitAnalysis') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld identification units' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewIdentificationUnitAnalysis'
GO

GRANT SELECT ON ViewIdentificationUnitAnalysis TO [CacheUser]
GO




--#####################################################################################################################
--######   ViewIdentificationUnitInPart   #########################################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewIdentificationUnitInPart]
AS
SELECT  DISTINCT       I.Description, I.CollectionSpecimenID, I.IdentificationUnitID, I.SpecimenPartID, I.DisplayOrder
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


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewIdentificationUnitInPart') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld identification units in a specimen part' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewIdentificationUnitInPart'
GO

GRANT SELECT ON ViewIdentificationUnitInPart TO [CacheUser]
GO




--#####################################################################################################################
--######   ViewWithholdAgent   #########################################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewWithholdAgent]
AS
SELECT        A.CollectionSpecimenID
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionAgent AS A INNER JOIN
dbo.ViewCollectionSpecimen AS S ON A.CollectionSpecimenID = S.CollectionSpecimenID
WHERE        (A.DataWithholdingReason <> N'''')')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewWithholdAgent') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all withheld collectors of published specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewWithholdAgent'
GO

GRANT SELECT ON ViewWithholdAgent TO [CacheUser]
GO



--#####################################################################################################################
--######   ViewWithholdPart   #########################################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewWithholdPart]
AS
SELECT        P.SpecimenPartID
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenPart AS P INNER JOIN
dbo.ViewCollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID
WHERE        (P.DataWithholdingReason <> N'''')')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewWithholdPart') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all withheld parts of published specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewWithholdPart'
GO

GRANT SELECT ON ViewWithholdPart TO [CacheUser]
GO




--#####################################################################################################################
--######   ViewWithholdUnit   #########################################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewWithholdUnit]
AS
SELECT        U.IdentificationUnitID
FROM            ' +  dbo.SourceDatabase() + '.dbo.IdentificationUnit AS U INNER JOIN
dbo.ViewCollectionSpecimen AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID
WHERE        (U.DataWithholdingReason <> N'''')')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewWithholdUnit') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all withheld units of published specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewWithholdUnit'
GO

GRANT SELECT ON ViewWithholdUnit TO [CacheUser]
GO




--#####################################################################################################################
--######   ViewCollectionSpecimenImage   #########################################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenImage]
AS
SELECT        I.CollectionSpecimenID, I.URI, I.ResourceURI, I.SpecimenPartID, I.IdentificationUnitID, I.ImageType, I.Notes, I.LicenseURI, I.LicenseNotes, I.DisplayOrder, 
I.LicenseYear, I.LicenseHolderAgentURI, I.LicenseHolder, I.LicenseType, I.CopyrightStatement, I.CreatorAgentURI, I.CreatorAgent, I.IPR, I.Title
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenImage AS I INNER JOIN
dbo.ViewCollectionSpecimen AS S ON I.CollectionSpecimenID = S.CollectionSpecimenID
WHERE        (I.DataWithholdingReason IS NULL OR
I.DataWithholdingReason = N'''') AND (I.IdentificationUnitID NOT IN
(SELECT        IdentificationUnitID
FROM            dbo.ViewWithholdUnit)) AND (I.CollectionSpecimenID NOT IN
(SELECT        CollectionSpecimenID
FROM            dbo.ViewWithholdAgent)) AND (I.SpecimenPartID NOT IN
(SELECT        SpecimenPartID
FROM            dbo.ViewWithholdPart))')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionSpecimenImage') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collection specimen images' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionSpecimenImage'
GO

GRANT SELECT ON ViewCollectionSpecimenImage TO [CacheUser]
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.04'
END

GO



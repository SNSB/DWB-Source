
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.12'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO




--#####################################################################################################################
--######   [CollectionSpecimen] - including restriction to embargos     #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[CollectionSpecimen]
AS
SELECT        S.CollectionSpecimenID, NULL AS Version, LTRIM(RTRIM(S.AccessionNumber)) AS AccessionNumber, CASE WHEN 1 = 1 THEN CASE WHEN E.CollectionYear IS NULL
THEN NULL ELSE CAST(E.CollectionYear AS VARCHAR) + CASE WHEN E.CollectionMonth IS NULL THEN '''' ELSE + ''-'' + REPLICATE(''0'', 
2 - LEN(CAST(E.CollectionMonth AS VARCHAR))) + CAST(E.CollectionMonth AS VARCHAR) + CASE WHEN E.CollectionDay IS NULL THEN '''' ELSE ''-'' + REPLICATE(''0'', 
2 - LEN(CAST(E.CollectionDay AS VARCHAR))) + CAST(E.CollectionDay AS VARCHAR) END END END ELSE NULL END AS CollectionDate, E.CollectionDay, 
E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, E.LocalityDescription, E.CountryCache, S.LabelTranscriptionNotes, 
CASE WHEN S.ExsiccataAbbreviation LIKE ''<%>'' THEN NULL ELSE S.ExsiccataAbbreviation END AS ExsiccataAbbreviation, S.OriginalNotes, 
E.CollectorsEventNumber, E.DataWithholdingReason AS EventDataWithholdingReason, dbo.CollectionEventChronostratigraphy.Display AS Chronostratigraphy, 
dbo.CollectionEventLithostratigraphy.DisplayText AS Lithostratigraphy, S.LogUpdatedWhen
FROM            dbo.CollectionEventLithostratigraphy RIGHT OUTER JOIN
dbo.CollectionEvent AS E ON dbo.CollectionEventLithostratigraphy.CollectionEventID = E.CollectionEventID LEFT OUTER JOIN
dbo.CollectionEventChronostratigraphy ON E.CollectionEventID = dbo.CollectionEventChronostratigraphy.CollectionEventID RIGHT OUTER JOIN
' + dbo.SourceDatebase() + '.dbo.CollectionSpecimen AS S INNER JOIN
dbo.CollectionProject ON S.CollectionSpecimenID = dbo.CollectionProject.CollectionSpecimenID ON E.CollectionEventID = S.CollectionEventID
WHERE        (S.DataWithholdingReason = '''' OR
S.DataWithholdingReason IS NULL) AND (S.CollectionSpecimenID NOT IN
(SELECT        CST.CollectionSpecimenID
FROM            ' + dbo.SourceDatebase() + '.dbo.CollectionSpecimenTransaction AS CST INNER JOIN
' + dbo.SourceDatebase() + '.dbo.[Transaction] AS TR ON CST.TransactionID = TR.TransactionID
WHERE        (TR.TransactionType = N''embargo'') AND (TR.BeginDate IS NULL OR
TR.BeginDate <= GETDATE()) AND (TR.AgreedEndDate IS NULL OR
TR.AgreedEndDate >= GETDATE())))
GROUP BY S.CollectionSpecimenID, S.AccessionNumber, E.CollectionDay, E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, E.LocalityDescription, 
E.CountryCache, S.LabelTranscriptionNotes, S.OriginalNotes, E.CollectorsEventNumber, E.DataWithholdingReason, dbo.CollectionEventChronostratigraphy.Display, 
dbo.CollectionEventLithostratigraphy.DisplayText, S.ExsiccataAbbreviation, S.LogUpdatedWhen')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch



GO


--#####################################################################################################################
--######   CollectionSpecimenPart - including DataWithholdingReason     #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[CollectionSpecimenPart]
AS
SELECT        CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, PartSublabel, CollectionID, 
                         MaterialCategory, StorageLocation, Stock, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
FROM            DiversityCollection_Test.dbo.CollectionSpecimenPart AS CollectionSpecimenPart_1')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO


--#####################################################################################################################
--######  procTransferIdentificationUnit - Datawithholding in Part  #############################################################################
--#####################################################################################################################




declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [dbo].[procTransferIdentificationUnit] AS
/*
-- TEST

declare @T1 datetime
declare @T2 datetime
set @T1 = (select GETDATE() )                     

execute dbo.procTransferIdentificationUnit

set @T2 = (select GETDATE())
select DATEDIFF(ms, @t1, @t2)

--select * from IdentificationUnitPartCache

*/
declare @i int
set @i = (select count(*) from dbo.Test_IdentificationUnit_AcceptedName_Duplicates)
if (@i > 0)
begin
   RAISERROR (''Problems with the taxon synonymy. Please check dbo.Test_IdentificationUnit_AcceptedName_Duplicates'', 16, 1)
end
else
begin

/*Loeschen der Daten in der Tabelle*/
delete from IdentificationUnitPartCache

/* Einlesen der Daten*/      
INSERT INTO IdentificationUnitPartCache
      (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, LastIdentificationCache, 
      U.TaxonomicGroup, SubstrateID, ExsiccataNumber, DisplayOrder, 
      ColonisedSubstratePart, IdentificationQualifier, VernacularTerm, TaxonomicName, Notes, 
      TypeStatus, SynonymName, AcceptedName, AcceptedNameURI, TaxonomicRank, 
      GenusOrSupragenericName, TaxonNameSinAuthor, TaxonomicNameFirstEntry, LastValidTaxonName, LastValidTaxonWithQualifier, 
      LastValidTaxonIndex, LastValidTaxonListOrder, LastValidTaxonSinAuthor, FamilyCache, OrderCache, 
      LifeStage, Gender, Relation
      , UnitAssociation_AssociatedUnitSourceInstitutionCode, UnitAssociation_AssociatedUnitSourceName
      , UnitAssociation_AssociatedUnitID, UnitAssociation_AssociationType
      ,UnitID, 
      UnitGUID,  
      MaterialCategory, 
      AccNrUnitID)
SELECT DISTINCT  U.CollectionSpecimenID, U.IdentificationUnitID, P.SpecimenPartID, RTRIM(LastIdentificationCache), 
	U.TaxonomicGroup, RelatedUnitID, ExsiccataNumber, U.DisplayOrder, 
	ColonisedSubstratePart, '''', '''', '''', CAST(U.Notes AS nvarchar(600)),  
	'''' AS TypeStatus, RTRIM(''''), RTRIM(''''), '''', '''', 
	RTRIM('''') AS GenusOrSupragenericName, RTRIM(''''), RTRIM(''''), RTRIM(''''), RTRIM(''''), 
	RTRIM('''') AS LastValidTaxonIndex, '''', '''', FamilyCache, OrderCache, 
	LifeStage, Gender, RelationType, 
	''' + dbo.ServerURL() + ''', ''Collection''
	, RelatedUnitID, RelationType
	, cast(U.CollectionSpecimenID as varchar) + ''-'' + cast(U.IdentificationUnitID as varchar) + ''-'' + cast(P.SpecimenPartID as varchar)
	, ''' + dbo.BaseURLofSource() + ''' + cast(U.CollectionSpecimenID as varchar) + ''-'' + cast(U.IdentificationUnitID as varchar) + ''-'' + cast(P.SpecimenPartID as varchar)
	, ''''
	, CASE WHEN S.AccessionNumber <> '''' THEN S.AccessionNumber + '' / '' ELSE '''' END + CAST(U.IdentificationUnitID AS varchar(90)) + '' / '' + cast(P.SpecimenPartID  AS varchar(90))
FROM ' + dbo.SourceDatebase() + '.dbo.IdentificationUnit U, dbo.IdentificationUnitInPart P, dbo.CollectionSpecimen S
, dbo.TaxonomicGroupInProject T, dbo.CollectionProject CP, dbo.EnumMaterialCategory M, dbo.CollectionSpecimenPart SP
where P.CollectionSpecimenID = U.CollectionSpecimenID and U.IdentificationUnitID = P.IdentificationUnitID
and S.CollectionSpecimenID = U.CollectionSpecimenID and S.CollectionSpecimenID = P.CollectionSpecimenID
and T.ProjectID = CP.ProjectID
and CP.CollectionSpecimenID = S.CollectionSpecimenID
and T.TaxonomicGroup = U.TaxonomicGroup
and M.Code = SP.MaterialCategory
and SP.CollectionSpecimenID = P.CollectionSpecimenID
and SP.SpecimenPartID = P.SpecimenPartID
and (SP.DatawithholdingReason = '''' or SP.DatawithholdingReason is null)')

set @SQL = (@SQL + '

/* Einlesen der Daten fuer Units ohne Part*/      
INSERT INTO IdentificationUnitPartCache
      (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, LastIdentificationCache, 
      TaxonomicGroup, SubstrateID, ExsiccataNumber, DisplayOrder, 
      ColonisedSubstratePart, IdentificationQualifier, VernacularTerm, TaxonomicName, Notes, 
      TypeStatus, SynonymName, AcceptedName, AcceptedNameURI, TaxonomicRank, 
      GenusOrSupragenericName, TaxonNameSinAuthor, TaxonomicNameFirstEntry, LastValidTaxonName, LastValidTaxonWithQualifier, 
      LastValidTaxonIndex, LastValidTaxonListOrder, LastValidTaxonSinAuthor, FamilyCache, OrderCache, 
      LifeStage, Gender, Relation
      , UnitAssociation_AssociatedUnitSourceInstitutionCode, UnitAssociation_AssociatedUnitSourceName
      , UnitAssociation_AssociatedUnitID, UnitAssociation_AssociationType
      ,UnitID, 
      UnitGUID,  
      MaterialCategory, 
      AccNrUnitID)
SELECT DISTINCT  U.CollectionSpecimenID, U.IdentificationUnitID, -1, RTRIM(LastIdentificationCache), 
	U.TaxonomicGroup, RelatedUnitID, ExsiccataNumber, U.DisplayOrder, 
	ColonisedSubstratePart, '''', '''', '''', CAST(U.Notes AS nvarchar(600)), 
	'''' AS TypeStatus, RTRIM(''''), RTRIM(''''), '''', '''', 
	RTRIM('''') AS GenusOrSupragenericName, RTRIM(''''), RTRIM(''''), RTRIM(''''), RTRIM(''''), 
	RTRIM('''') AS LastValidTaxonIndex, '''', '''', FamilyCache, OrderCache, 
	LifeStage, Gender, RelationType, 
	''' + dbo.ServerURL() + ''', ''Collection''
	, RelatedUnitID, RelationType
	, cast(U.CollectionSpecimenID as varchar) + ''-'' + cast(U.IdentificationUnitID as varchar) + ''-''
	, ''' + dbo.BaseURLofSource() + ''' + cast(U.CollectionSpecimenID as varchar) + ''-'' + cast(U.IdentificationUnitID as varchar) + ''-''
	, ''''
	, CASE WHEN S.AccessionNumber <> '''' THEN S.AccessionNumber + '' / '' ELSE '''' END + CAST(U.IdentificationUnitID AS varchar(90))
FROM CollectionProject AS CP INNER JOIN
TaxonomicGroupInProject AS T ON CP.ProjectID = T.ProjectID INNER JOIN
' + dbo.SourceDatebase() + '.dbo.IdentificationUnit AS U INNER JOIN
CollectionSpecimen AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID ON CP.CollectionSpecimenID = S.CollectionSpecimenID AND 
T.TaxonomicGroup = U.TaxonomicGroup LEFT OUTER JOIN
IdentificationUnitInPart AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID AND U.IdentificationUnitID = P.IdentificationUnitID
WHERE (P.CollectionSpecimenID IS NULL)

/* Korrektur von  First entry */
update U set U.IdentificationSequenceFirstEntry = I.IdentificationSequence
FROM  ' + dbo.SourceDatebase() + '.dbo.Identification I, IdentificationUnitPartCache U
WHERE I.IdentificationUnitID = U.IdentificationUnitID 
AND I.CollectionSpecimenID = U.CollectionSpecimenID
AND I.IdentificationSequence = (
Select min(M.IdentificationSequence) from ' + dbo.SourceDatebase() + '.dbo.Identification M
where M.CollectionSpecimenID = I.CollectionSpecimenID and M.IdentificationUnitID = I.IdentificationUnitID
GROUP BY M.CollectionSpecimenID, M.IdentificationUnitID)

update U set U.TaxonomicNameFirstEntry = I.TaxonomicName
FROM  ' + dbo.SourceDatebase() + '.dbo.Identification I, IdentificationUnitPartCache U
WHERE I.IdentificationUnitID = U.IdentificationUnitID 
AND I.CollectionSpecimenID = U.CollectionSpecimenID
AND I.IdentificationSequence = U.IdentificationSequenceFirstEntry')

set @SQL = (@SQL + '

-- Last entry
update U set U.LastValidIdentificationSequence = I.IdentificationSequence
FROM  ' + dbo.SourceDatebase() + '.dbo.Identification I, IdentificationUnitPartCache U
WHERE I.IdentificationUnitID = U.IdentificationUnitID 
AND I.CollectionSpecimenID = U.CollectionSpecimenID
AND I.IdentificationSequence = (
Select max(M.IdentificationSequence) from ' + dbo.SourceDatebase() + '.dbo.Identification M
where M.CollectionSpecimenID = I.CollectionSpecimenID and M.IdentificationUnitID = I.IdentificationUnitID
GROUP BY M.CollectionSpecimenID, M.IdentificationUnitID)

--Identification
update U set U.TaxonomicName = cast(rtrim(ltrim(I.TaxonomicName)) as nvarchar(255))
, U.IdentificationQualifier = I.IdentificationQualifier
, U.VernacularTerm = I.VernacularTerm
, U.Notes = cast(rtrim(ltrim(I.Notes)) as nvarchar(600))
, U.TypeStatus = I.TypeStatus
, U.SynNameURI = I.NameURI
FROM  ' + dbo.SourceDatebase() + '.dbo.Identification I, IdentificationUnitPartCache U
WHERE I.IdentificationUnitID = U.IdentificationUnitID 
AND I.CollectionSpecimenID = U.CollectionSpecimenID
AND I.IdentificationSequence = U.LastValidIdentificationSequence')

set @SQL = (@SQL + '

--TaxonNames
UPDATE UP SET
UP.SynonymName = T.SynonymName, 
UP.AcceptedName = T.AcceptedName, 
UP.TaxonomicRank = T.TaxonomicRank, 
UP.AcceptedNameURI = T.NameURI, 
UP.GenusOrSupragenericName = CASE WHEN T .GenusOrSupragenericName IS NULL THEN CASE WHEN UP.TaxonomicName IS NULL OR len(rtrim(UP.TaxonomicName)) = 0 THEN NULL 
	ELSE CASE WHEN charindex('' '', UP.TaxonomicName) = 0 THEN UP.TaxonomicName 
	ELSE substring(UP.TaxonomicName, 1, charindex('' '', UP.TaxonomicName)) END END ELSE T.GenusOrSupragenericName END, 
UP.TaxonNameSinAuthor =  CASE WHEN T .TaxonNameSinAuthor IS NULL 
	THEN CASE WHEN UP.TaxonomicName IS NULL OR len(rtrim(UP.TaxonomicName)) = 0 THEN NULL 
	ELSE CASE WHEN UP.IdentificationQualifier IN (''sp.'', ''sp. nov.'', ''spp.'') OR T .TaxonomicRank = ''gen.'' 
	THEN CASE WHEN charindex('' '', UP.TaxonomicName) = 0 THEN UP.TaxonomicName + '' sp.'' 
	ELSE substring(UP.TaxonomicName, 1, charindex('' '', UP.TaxonomicName)) + ''sp.'' END 
	ELSE CASE WHEN charindex('' '', UP.TaxonomicName) = 0 THEN UP.TaxonomicName + '' sp.'' 
	ELSE substring(UP.TaxonomicName, 1, charindex('' '', UP.TaxonomicName) - 1) + substring(UP.TaxonomicName, charindex('' '', UP.TaxonomicName), 
	CASE WHEN charindex('' '', UP.TaxonomicName, charindex('' '', UP.TaxonomicName) + 1) > charindex('' '', UP.TaxonomicName) 
	THEN charindex('' '', UP.TaxonomicName, charindex('' '', UP.TaxonomicName) + 1) - charindex('' '', UP.TaxonomicName) 
	ELSE len(UP.TaxonomicName) END) END END END ELSE T .TaxonNameSinAuthor END, 
UP.LastValidTaxonName =  CASE WHEN T .AcceptedName IS NULL THEN CASE WHEN UP.TaxonomicName IS NULL OR UP.TaxonomicName = '''' THEN UP.VernacularTerm 
	ELSE UP.TaxonomicName END ELSE T .AcceptedName END , 
UP.LastValidTaxonWithQualifier = CASE WHEN T .AcceptedName IS NULL THEN CASE WHEN UP.IdentificationQualifier IS NULL OR UP.IdentificationQualifier = '''' 
	THEN UP.TaxonomicName ELSE CASE WHEN UP.IdentificationQualifier = ''?'' THEN UP.IdentificationQualifier + '' '' + UP.TaxonomicName 
	ELSE CASE WHEN UP.IdentificationQualifier IN (''sp.'', ''sp. nov.'', ''spp.'') 
	THEN CASE WHEN CHARINDEX('' '', UP.TaxonomicName) = 0 THEN UP.TaxonomicName 
	ELSE substring(UP.TaxonomicName, 1, charindex('' '', UP.TaxonomicName) - 1) END + '' '' + UP.IdentificationQualifier 
	ELSE CASE WHEN (UP.IdentificationQualifier LIKE ''cf.%'' OR UP.IdentificationQualifier LIKE ''aff.%'') 
	THEN CASE WHEN UP.IdentificationQualifier LIKE ''% gen.'' 
	THEN substring(UP.IdentificationQualifier, 1, charindex(''.'', UP.IdentificationQualifier)) + '' '' + 
	CASE WHEN charindex('' '', UP.TaxonomicName) = 0 THEN UP.TaxonomicName 
	ELSE substring(UP.TaxonomicName, 1, charindex('' '', UP.TaxonomicName)) END ELSE ')

set @SQL = (@SQL + 'CASE WHEN UP.IdentificationQualifier LIKE ''% sp.'' OR UP.IdentificationQualifier LIKE ''% ssp.'' 
	THEN rtrim(substring(UP.TaxonomicName, 1, charindex('' '', UP.TaxonomicName)) + 
	substring(UP.IdentificationQualifier, 1, charindex(''.'', UP.IdentificationQualifier)) + 
	substring(UP.TaxonomicName, charindex('' '', UP.TaxonomicName), len(UP.TaxonomicName))) ELSE 
	CASE WHEN UP.IdentificationQualifier LIKE ''% var.'' AND patindex(''%var.%'', UP.TaxonomicName) > 0 
	THEN rtrim(substring(UP.TaxonomicName, 1, patindex(''%var.%'', UP.TaxonomicName) - 1) + UP.IdentificationQualifier 
	+ substring(UP.TaxonomicName, patindex(''%var.%'', UP.TaxonomicName) + 4, len(UP.TaxonomicName))) 
	ELSE CASE WHEN UP.IdentificationQualifier LIKE ''% fm.'' AND patindex(''%fm.%'', UP.TaxonomicName) > 0 
	THEN rtrim(substring(UP.TaxonomicName, 1, patindex(''%fm.%'', UP.TaxonomicName) - 1) + UP.IdentificationQualifier 
	+ substring(UP.TaxonomicName, patindex(''%fm.%'', UP.TaxonomicName) + 3, len(UP.TaxonomicName))) 
	ELSE UP.TaxonomicName END END END END ELSE UP.TaxonomicName END END END END ELSE 
	CASE WHEN UP.IdentificationQualifier IS NULL OR UP.IdentificationQualifier = '''' THEN T .AcceptedName ELSE 
	CASE WHEN UP.IdentificationQualifier = ''?'' THEN UP.IdentificationQualifier + '' '' + T .AcceptedName ELSE 
	CASE WHEN UP.IdentificationQualifier IN (''sp.'', ''sp. nov.'', ''spp.'') THEN CASE WHEN CHARINDEX('' '', T .AcceptedName) = 0 
	THEN T .AcceptedName ELSE substring(T .AcceptedName, 1, charindex('' '', T .AcceptedName) - 1) END + '' '' + UP.IdentificationQualifier 
	ELSE CASE WHEN (UP.IdentificationQualifier LIKE ''cf.%'' OR UP.IdentificationQualifier LIKE ''aff.%'') THEN 
	CASE WHEN UP.IdentificationQualifier LIKE ''% gen.'' 
	THEN substring(UP.IdentificationQualifier, 1, charindex(''.'', UP.IdentificationQualifier)) + '' '' 
	+ substring(T .AcceptedName, 1, charindex('' '', T .AcceptedName)) ELSE ')

set @SQL = (@SQL + 'CASE WHEN UP.IdentificationQualifier LIKE ''% sp.'' OR UP.IdentificationQualifier LIKE ''% ssp.'' 
	THEN rtrim(substring(T .AcceptedName, 1, charindex('' '', T .AcceptedName)) 
	+ substring(UP.IdentificationQualifier, 1, charindex(''.'', UP.IdentificationQualifier)) 
	+ substring(T .AcceptedName, charindex('' '', UP.TaxonomicName), len(T .AcceptedName))) 
	ELSE CASE WHEN UP.IdentificationQualifier LIKE ''% var.'' AND patindex(''%var.%'', T .AcceptedName) > 0 
	THEN rtrim(substring(T .AcceptedName, 1, patindex(''%var.%'',  T .AcceptedName) - 1) + UP.IdentificationQualifier 
	+ substring(T .AcceptedName, patindex(''%var.%'', T .AcceptedName) + 4, len(T .AcceptedName))) 
	ELSE CASE WHEN UP.IdentificationQualifier LIKE ''% fm.'' AND patindex(''%fm.%'', T .AcceptedName) > 0 
	THEN rtrim(substring(T .AcceptedName, 1, patindex(''%fm.%'', T .AcceptedName) - 1) + UP.IdentificationQualifier 
	+ substring(T .AcceptedName, patindex(''%fm.%'', T .AcceptedName) + 3, len(T .AcceptedName))) ELSE T .AcceptedName END END END END 
	ELSE UP.TaxonomicName END END END END END , 
UP.LastValidTaxonSinAuthor = CASE WHEN T .TaxonNameSinAuthor IS NULL THEN CASE WHEN UP.TaxonomicName IS NULL OR len(rtrim(UP.TaxonomicName)) = 0 THEN NULL ELSE 
	CASE WHEN charindex('' '', UP.TaxonomicName) = 0 THEN UP.TaxonomicName ELSE substring(UP.TaxonomicName, 1, charindex('' '', UP.TaxonomicName) - 1) 
	+ substring(UP.TaxonomicName, charindex('' '', UP.TaxonomicName), 
	CASE WHEN charindex('' '', UP.TaxonomicName, charindex('' '', UP.TaxonomicName) + 1) > charindex('' '', UP.TaxonomicName) 
	THEN charindex('' '', UP.TaxonomicName, charindex('' '', UP.TaxonomicName) + 1) - charindex('' '', UP.TaxonomicName) 
	ELSE len(UP.TaxonomicName) END) END END ELSE T .TaxonNameSinAuthor END , 
UP.LastValidTaxonIndex = CASE WHEN T .AcceptedName IS NULL THEN CASE WHEN UP.IdentificationQualifier IN (''sp.'', ''cf. gen.'') OR T .TaxonomicRank = ''gen.'' THEN 
	CASE WHEN CHARINDEX('' '', UP.TaxonomicName) = 0 THEN UP.TaxonomicName + '' sp.'' 
	ELSE substring(UP.TaxonomicName, 1, charindex('' '', UP.TaxonomicName) - 1) + '' sp.'' END ELSE UP.TaxonomicName END ELSE 
	CASE WHEN UP.IdentificationQualifier IN (''sp.'', ''cf. gen.'') OR T .TaxonomicRank = ''gen.'' THEN 
	CASE WHEN CHARINDEX('' '', T .AcceptedName) = 0 THEN T .AcceptedName + '' sp.'' 
	ELSE substring(T .AcceptedName, 1, charindex('' '', T .AcceptedName) - 1) + '' sp.'' END ELSE T .AcceptedName END END , 
UP.LastValidTaxonListOrder = CASE WHEN UP.IdentificationQualifier IN (''sp.'', ''cf. gen.'') OR T .TaxonomicRank = ''gen.'' THEN 1 ELSE 0 END 
FROM   IdentificationUnitPartCache AS UP
LEFT OUTER JOIN dbo.TaxonSynonymy AS T ON UP.SynNameURI = T.SynNameURI')

set @SQL = (@SQL + '

-- set relation
UPDATE C SET C.Relation 
= RTRIM(RTRIM(C.Relation) + '' '' + CASE WHEN C.Relation LIKE ''% on'' THEN '''' ELSE ''on '' END + CASE WHEN P.LastIdentificationCache = '''' THEN P.TaxonomicGroup ELSE P.LastIdentificationCache END )
, C.UnitAssociation_Comment 
= RTRIM(RTRIM(C.Relation) + '' '' + CASE WHEN C.Relation LIKE ''% on'' THEN '''' ELSE ''on '' END + CASE WHEN P.LastIdentificationCache = '''' THEN P.TaxonomicGroup ELSE P.LastIdentificationCache END )
FROM IdentificationUnitPartCache C, IdentificationUnit U, IdentificationUnit P
WHERE C.CollectionSpecimenID = U.CollectionSpecimenID
AND C.CollectionSpecimenID = P.CollectionSpecimenID
AND C.IdentificationUnitID = U.IdentificationUnitID
AND U.RelatedUnitID = P.IdentificationUnitID

-- set Typification
UPDATE C SET C.NomenclaturalTypeDesignation_TypifiedName = I.TaxonomicName
, C.NomenclaturalTypeDesignation_TypeStatus = I.TypeStatus
FROM IdentificationUnitPartCache C, Identification I
WHERE C.CollectionSpecimenID = I.CollectionSpecimenID
AND C.IdentificationUnitID = I.IdentificationUnitID
AND I.TypeStatus <> ''''

--MaterialCategory
UPDATE C SET C.MaterialCategory = P.MaterialCategory
FROM IdentificationUnitPartCache AS C INNER JOIN
CollectionSpecimenPart AS P ON C.CollectionSpecimenID = P.CollectionSpecimenID AND C.SpecimenPartID = P.SpecimenPartID

DECLARE @U int
SET @U = (SELECT COUNT(*) FROM IdentificationUnitPartCache)
SELECT CAST(@U AS VARCHAR) + '' identification units imported''
end')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch


GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.13'
END

GO



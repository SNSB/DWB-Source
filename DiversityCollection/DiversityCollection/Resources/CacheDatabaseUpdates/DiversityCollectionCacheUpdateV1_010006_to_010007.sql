
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.06'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   procTransferCoordinates  Bugfix      #############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferCoordinates] 
AS

-- setting the CollectionEventID to ease subsequent update
UPDATE C SET C.CollectionEventID = E.[CollectionEventID] 
FROM [dbo].[CollectionSpecimenCache] C, CollectionSpecimenEventID E
WHERE C.CollectionSpecimenID = E.CollectionSpecimenID

DECLARE @LocalisationSystemID int

-- the sequence of the LocalisationSystems that should be scanned to insert the coordiates
DECLARE @TempLocalisationSystem TABLE (OrderSequence int primary key, LocalisationSystemID int)
INSERT INTO @TempLocalisationSystem (OrderSequence, LocalisationSystemID)
SELECT [Sequence]
      ,[LocalisationSystemID]
  FROM [dbo].[EnumLocalisationSystem]

-- inserting the coordinates
while ((select count (*) from @TempLocalisationSystem) > 0) 
begin
	set @LocalisationSystemID = (select min(LocalisationSystemID) from @TempLocalisationSystem where OrderSequence = (select min(OrderSequence) from @TempLocalisationSystem))
	
	UPDATE CC
	SET CC.Latitude = CAST(L.AverageLatitudeCache AS float),
	CC.Longitude = L.AverageLongitudeCache
	FROM CollectionSpecimenCache CC, 
	dbo.CollectionEventLocalisation L
	WHERE CC.CollectionEventID = L.CollectionEventID
	AND L.LocalisationSystemID = @LocalisationSystemID
	AND CC.Latitude IS NULL
	AND L.AverageLatitudeCache <> 0
	
	delete @TempLocalisationSystem where LocalisationSystemID = @LocalisationSystemID

end

-- reducing the precision of the coordinates for those project where a reduction is demanded
DECLARE @CoordinatePrecision TABLE(ProjectID int primary key, CoordinatePrecision tinyint)

-- getting all projects where the accuracy of the coordinates should be reduced
insert into @CoordinatePrecision (ProjectID, CoordinatePrecision)
select ProjectID, CoordinatePrecision from [dbo].[ProjectPublished] PP
where not PP.CoordinatePrecision is null

declare @ProjectID int
set @ProjectID = (select min(ProjectID) from @CoordinatePrecision)
-- reducing the precision
while (select count(*) from @CoordinatePrecision) > 0
begin
	UPDATE CC
	SET CC.Latitude = round(CC.Latitude, CA.CoordinatePrecision),
	CC.Longitude = round(CC.Longitude, CA.CoordinatePrecision)
	FROM CollectionSpecimenCache CC, CollectionProject P, @CoordinatePrecision CA
	where CC.CollectionSpecimenID = P.CollectionSpecimenID and P.ProjectID = CA.ProjectID
	and (CC.Latitude <> 0 or CC.Longitude <> 0)
	and CA.ProjectID = @ProjectID

	set @ProjectID = (select min(ProjectID) from @CoordinatePrecision)

	delete CA from @CoordinatePrecision CA
	where CA.ProjectID = @ProjectID

end')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO


--#####################################################################################################################
--######  procTransferIdentificationUnit - no / behind AccNrUnitID if part is missing  #############################################################################
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
and SP.SpecimenPartID = P.SpecimenPartID')

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
RETURN '01.00.07'
END

GO



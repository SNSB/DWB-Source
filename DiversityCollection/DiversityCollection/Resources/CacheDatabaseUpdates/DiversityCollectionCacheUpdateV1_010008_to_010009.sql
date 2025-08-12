
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.08'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--#####################################################################################################################
--######      IdentificationUnitPartCache        ######################################################################
--#####################################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'IdentificationUnitPartCache') = 0
begin
	CREATE TABLE [dbo].[IdentificationUnitPartCache](
		[CollectionSpecimenID] [int] NOT NULL,
		[IdentificationUnitID] [int] NOT NULL,
		[SpecimenPartID] [int] NOT NULL,
		[LastIdentificationCache] [nvarchar](255) NULL,
		[TaxonomicGroup] [nvarchar](50) NULL,
		[SubstrateID] [int] NULL,
		[ExsiccataNumber] [nvarchar](50) NULL,
		[DisplayOrder] [tinyint] NOT NULL,
		[ColonisedSubstratePart] [nvarchar](255) NULL,
		[IdentificationQualifier] [nvarchar](50) NULL,
		[VernacularTerm] [nvarchar](255) NULL,
		[TaxonomicName] [nvarchar](255) NULL,
		[Notes] [nvarchar](600) NULL,
		[TypeStatus] [nvarchar](50) NULL,
		[SynonymName] [nvarchar](255) NULL,
		[AcceptedName] [nvarchar](255) NULL,
		[AcceptedNameURI] [varchar](255) NULL,
		[SynNameURI] [varchar](255) NULL,
		[TaxonomicRank] [nvarchar](50) NULL,
		[GenusOrSupragenericName] [nvarchar](200) NULL,
		[TaxonNameSinAuthor] [nvarchar](255) NULL,
		[IdentificationSequenceFirstEntry] [smallint] NULL,
		[TaxonomicNameFirstEntry] [nvarchar](255) NULL,
		[LastValidIdentificationSequence] [smallint] NULL,
		[LastValidTaxonName] [nvarchar](255) NULL,
		[LastValidTaxonWithQualifier] [nvarchar](255) NULL,
		[LastValidTaxonSinAuthor] [nvarchar](255) NULL,
		[LastValidTaxonIndex] [nvarchar](255) NULL,
		[LastValidTaxonListOrder] [tinyint] NULL,
		[FamilyCache] [nvarchar](255) NULL,
		[OrderCache] [nvarchar](255) NULL,
		[LifeStage] [nvarchar](255) NULL,
		[Gender] [nvarchar](50) NULL,
		[Relation] [nvarchar](500) NULL,
		[UnitAssociation_AssociatedUnitSourceInstitutionCode] [nvarchar](50) NULL,
		[UnitAssociation_AssociatedUnitSourceName] [nvarchar](50) NULL,
		[UnitAssociation_AssociatedUnitID] [int] NULL,
		[UnitAssociation_AssociationType] [nvarchar](50) NULL,
		[UnitAssociation_Comment] [nvarchar](500) NULL,
		[UnitID] [nvarchar](90) NULL,
		[UnitGUID] [nvarchar](150) NULL,
		[AccNrUnitID] [nvarchar](90) NULL,
		[NomenclaturalTypeDesignation_TypifiedName] [nvarchar](500) NULL,
		[NomenclaturalTypeDesignation_TypeStatus] [nvarchar](50) NULL,
		[MaterialCategory] [nvarchar](50) NULL,
		[LogInsertedWhen] [smalldatetime] NULL CONSTRAINT [DF_IdentificationUnitPartCache_LogInsertedWhen]  DEFAULT (getdate()),
	 CONSTRAINT [PK_IdentificationUnitPartCache] PRIMARY KEY CLUSTERED 
	(
		[CollectionSpecimenID] ASC,
		[IdentificationUnitID] ASC,
		[SpecimenPartID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]
end
GO


GRANT SELECT ON [IdentificationUnitPartCache] TO [CollectionCacheUser]
GO
GRANT DELETE ON [IdentificationUnitPartCache] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [IdentificationUnitPartCache] TO [CacheAdministrator]
GO
GRANT INSERT ON [IdentificationUnitPartCache] TO [CacheAdministrator]
GO



--#####################################################################################################################
--######  procTransferIdentificationUnit  - DatawithholdingReason #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = ''
declare @SQL1 nvarchar(max)
declare @SQL2 nvarchar(max)
declare @SQL3 nvarchar(max)
declare @SQL4 nvarchar(max)
declare @SQL5 nvarchar(max)
declare @SQL6 nvarchar(max)
declare @SQL7 nvarchar(max)

set @SQL1 = (select 'CREATE PROCEDURE [dbo].[procTransferIdentificationUnit] AS
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
FROM ' + dbo.SourceDatebase() + '.dbo.IdentificationUnit U, ' + dbo.SourceDatebase() + '.dbo.IdentificationUnitInPart P, CollectionSpecimen S
, dbo.TaxonomicGroupInProject T, CollectionProject CP, dbo.EnumMaterialCategory M, ' + dbo.SourceDatebase() + '.dbo.CollectionSpecimenPart SP
where P.CollectionSpecimenID = U.CollectionSpecimenID and U.IdentificationUnitID = P.IdentificationUnitID
and S.CollectionSpecimenID = U.CollectionSpecimenID and S.CollectionSpecimenID = P.CollectionSpecimenID
and T.ProjectID = CP.ProjectID
and CP.CollectionSpecimenID = S.CollectionSpecimenID
and T.TaxonomicGroup = U.TaxonomicGroup
and (U.DatawithholdingReason = '''' or U.DatawithholdingReason is null)
and M.Code = SP.MaterialCategory
and SP.CollectionSpecimenID = P.CollectionSpecimenID
and SP.SpecimenPartID = P.SpecimenPartID')

set @SQL2 = (@SQL + '

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
	, CASE WHEN S.AccessionNumber <> '''' THEN S.AccessionNumber + '' / '' ELSE '''' END + CAST(U.IdentificationUnitID AS varchar(90)) + '' /''
FROM CollectionProject AS CP INNER JOIN
TaxonomicGroupInProject AS T ON CP.ProjectID = T.ProjectID INNER JOIN
' + dbo.SourceDatebase() + '.dbo.IdentificationUnit AS U INNER JOIN
CollectionSpecimen AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID ON CP.CollectionSpecimenID = S.CollectionSpecimenID 
AND (U.DatawithholdingReason = '''' or U.DatawithholdingReason is null)
and T.TaxonomicGroup = U.TaxonomicGroup LEFT OUTER JOIN
' + dbo.SourceDatebase() + '.dbo.IdentificationUnitInPart AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID AND U.IdentificationUnitID = P.IdentificationUnitID
WHERE (P.CollectionSpecimenID IS NULL)
and (U.DatawithholdingReason = '''' or U.DatawithholdingReason is null)

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

set @SQL3 = (@SQL + '

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

set @SQL4 = (@SQL + '

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

set @SQL5 = (@SQL + 'CASE WHEN UP.IdentificationQualifier LIKE ''% sp.'' OR UP.IdentificationQualifier LIKE ''% ssp.'' 
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

set @SQL6 = (@SQL + 'CASE WHEN UP.IdentificationQualifier LIKE ''% sp.'' OR UP.IdentificationQualifier LIKE ''% ssp.'' 
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

set @SQL7 = (@SQL + '

-- set relation
UPDATE C SET C.Relation 
= RTRIM(RTRIM(C.Relation) + '' '' + CASE WHEN C.Relation LIKE ''% on'' THEN '''' ELSE ''on '' END + CASE WHEN P.LastIdentificationCache = '''' THEN P.TaxonomicGroup ELSE P.LastIdentificationCache END )
, C.UnitAssociation_Comment 
= RTRIM(RTRIM(C.Relation) + '' '' + CASE WHEN C.Relation LIKE ''% on'' THEN '''' ELSE ''on '' END + CASE WHEN P.LastIdentificationCache = '''' THEN P.TaxonomicGroup ELSE P.LastIdentificationCache END )
FROM IdentificationUnitPartCache C, ' + dbo.SourceDatebase() + '.dbo.IdentificationUnit U, ' + dbo.SourceDatebase() + '.dbo.IdentificationUnit P
WHERE C.CollectionSpecimenID = U.CollectionSpecimenID
AND C.CollectionSpecimenID = P.CollectionSpecimenID
AND C.IdentificationUnitID = U.IdentificationUnitID
AND U.RelatedUnitID = P.IdentificationUnitID

-- set Typification
UPDATE C SET C.NomenclaturalTypeDesignation_TypifiedName = I.TaxonomicName
, C.NomenclaturalTypeDesignation_TypeStatus = I.TypeStatus
FROM IdentificationUnitPartCache C, ' + dbo.SourceDatebase() + '.dbo.Identification I
WHERE C.CollectionSpecimenID = I.CollectionSpecimenID
AND C.IdentificationUnitID = I.IdentificationUnitID
AND I.TypeStatus <> ''''

--MaterialCategory
UPDATE C SET C.MaterialCategory = P.MaterialCategory
FROM IdentificationUnitPartCache AS C INNER JOIN
' + dbo.SourceDatebase() + '.dbo.CollectionSpecimenPart AS P ON C.CollectionSpecimenID = P.CollectionSpecimenID AND C.SpecimenPartID = P.SpecimenPartID

DECLARE @U int
SET @U = (SELECT COUNT(*) FROM IdentificationUnitPartCache)
SELECT CAST(@U AS VARCHAR) + '' identification units imported''
end')


begin try
exec ( @SQL1 + @SQL2 + @SQL3 + @SQL4 + @SQL5 + @SQL6 + @SQL7)
end try
begin catch
set @SQL1 = 'ALTER ' + SUBSTRING(@SQL1, 8, 4000)
exec (  @SQL1 + @SQL2 + @SQL3 + @SQL4 + @SQL5 + @SQL6 + @SQL7)
end catch

GO

GRANT EXEC ON [dbo].procTransferIdentificationUnit TO [CollectionCacheUser]
GO





--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.09'
END

GO



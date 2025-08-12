
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.38'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [TaxonWithQualifier]   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[TaxonWithQualifier]
(
	@Taxon nvarchar(500),
	@Qualifier nvarchar(50)
)
RETURNS nvarchar(500)
AS
BEGIN
-- =======================
-- Author:		Markus Weiss
-- Create date: 2013/09/16
-- Description:	Gibt Taxonomischen Namen mit verarbeitetem Qualifier zurueck
-- =======================
/*
Test
select dbo.TaxonWithQualifier('Erysiphe DC.', 'sp.')
1. ? = vor dem Namen, ob Gattung oder intragenerische Namen

2. aff. forma (bzw.für cf. forma das gleiche):
Cribraria vulgaris aff. f. (cf. f.) similis (L.) Fr.
select dbo.TaxonWithQualifier('Cribraria vulgaris f. similis (L.) Fr.', 'cf. forma')
select dbo.TaxonWithQualifier('Cribraria vulgaris f. similis (L.) Fr.', 'aff. forma')

aff. gen (cf. gen.):
aff. (cf.) Cribraria (L.) Fr.
select dbo.TaxonWithQualifier('Cribraria (L.) Fr.', 'cf. gen.')
select dbo.TaxonWithQualifier('Cribraria (L.) Fr.', 'aff. gen.')

aff. sp. (cf. sp.)
Cribraria aff. (cf.) vulgaris (L.) Fr.
select dbo.TaxonWithQualifier('Cribraria vulgaris (L.) Fr.', 'cf. sp.')
select dbo.TaxonWithQualifier('Cribraria vulgaris (L.) Fr.', 'aff. sp.')

aff. ssp. (cf. ssp.)
Cribraria vulgaris aff. ssp. (cf. ssp.) similis (L.) Fr.
select dbo.TaxonWithQualifier('Cribraria vulgaris ssp. similis (L.) Fr.', 'cf. ssp.')
select dbo.TaxonWithQualifier('Cribraria vulgaris ssp. similis (L.) Fr.', 'aff. ssp.')

aff. var. (cf. var.)
Cribraria vulgaris aff. var. (cf. var.) similis (L.) Fr.
select dbo.TaxonWithQualifier('Cribraria vulgaris var. similis (L.) Fr.', 'cf. var.')
select dbo.TaxonWithQualifier('Cribraria vulgaris var. similis (L.) Fr.', 'aff. var.')

3. agg. (Autoren weglassen)
Cribraria splendens agg.
Cribraria spendens ssp. vulgaris agg.
select dbo.TaxonWithQualifier('Cribraria splendens', 'agg.')
select dbo.TaxonWithQualifier('Cribraria spendens ssp. vulgaris', 'agg.')


4. s.l. und s.str. (mit Autoren, ansonsten wie unter 3.)
Cribraria splendens (L.) Fr. s.str.
select dbo.TaxonWithQualifier('Cribraria splendens (L.) Fr.', 's. str.')
Cribraria splendens (L.) Fr. s.l.
select dbo.TaxonWithQualifier('Cribraria splendens (L.) Fr.', 's. l.')
Cribraria spendens ssp. vulgaris (L.) Fr. s.l..
select dbo.TaxonWithQualifier('Cribraria spendens ssp. vulgaris (L.) Fr.', 's. str.')
Cribraria spendens f. vulgaris (L.) Fr. s.str.
select dbo.TaxonWithQualifier('Cribraria spendens ssp. vulgaris (L.) Fr.', 's. l.')

5. sp. nur bei Gattungen (Autoren weglassen)
Cribraria sp.
select dbo.TaxonWithQualifier('Cribraria Fr.', 'sp.')

6. sp. nov. hinten an Namen (mit oder ohne Autoren) oder besser weglassen, würde ich eigentlich meinen
Cribraria splendens Mayr sp. nov.
select dbo.TaxonWithQualifier('Cribraria splendens Mayr', 'sp. nov.')

Cribraria spendens ssp. vulgaris sp. nov.
select dbo.TaxonWithQualifier('Cribraria splendens ssp. vulgaris Mayr', 'sp. nov.')
--##Cribraria splendens vulgaris Mayr sp. nov. -- etwas unklar ob Rank mit ausgegeben werden soll, vorerst keine Aenderung

Cribraria spendens f. vulgaris Mayr & Müller sp. nov. 
select dbo.TaxonWithQualifier('Cribraria splendens f. vulgaris Mayr', 'sp. nov.')
---## Cribraria splendens vulgaris Mayr sp. nov. -- etwas unklar ob Rank mit ausgegeben werden soll, vorerst keine Aenderung

*/
            if (@Qualifier = '') return @Taxon;

            set @Taxon = rtrim(ltrim(@Taxon));
            declare @_Taxon nvarchar(500);
            set @_Taxon = @Taxon;
            declare @Rank nvarchar(50);
            set @Rank = '';
            declare @Genus nvarchar(50);
            set @Genus = '';
            declare @Authors nvarchar(250);
            set @Authors = '';
            declare @IntraSpecificEpithet nvarchar(150);
            set @IntraSpecificEpithet = '';
            declare @SpeciesEpithet nvarchar(150);
            set @SpeciesEpithet = '';
            declare @QualifierText nvarchar(50);
            set @QualifierText = '';
            declare @QualifierRank nvarchar(50);
            set @QualifierRank = '';
            BEGIN
                if (len(@Taxon) > 0)
                BEGIN
                    if (charindex(' ', @Taxon) > 0)
                    BEGIN
                        SET @Rank = 'sp.';
                        SET @Genus = ltrim(rtrim(Substring(@Taxon, 1, charindex(' ', @Taxon))));
                        SET @Taxon = ltrim(rtrim(Substring(@Taxon, charindex(' ', @Taxon), 500)));
                        if (@Taxon LIKE ('(%')
                            OR upper(Substring(@Taxon, 1, 1)) COLLATE Latin1_General_CS_AS = Substring(@Taxon, 1, 1) COLLATE Latin1_General_CS_AS 
                            )
							BEGIN
								SET @Authors = ltrim(rtrim(@Taxon));
								SET @Taxon = '';
								if (@Qualifier = 'sp.')
									begin
									SET @Rank = @Qualifier;
									end
								else
									begin
									SET @Rank = 'gen.';
									end
							END
                        else
                        BEGIN
                            if (charindex(' ', @Taxon) > 0)
                            BEGIN
                                SET @SpeciesEpithet = ltrim(rtrim(Substring(@Taxon, 1, charindex(' ', @Taxon))));
                                SET @Taxon = ltrim(rtrim(Substring(@Taxon, charindex(' ', @Taxon), 500)));
                                if (PATINDEX('% var. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0 OR
									PATINDEX('% ssp. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0 OR
									PATINDEX('% fm. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0 OR
									PATINDEX('% subvar. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0 OR
									PATINDEX('% f. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0)
									BEGIN
										if (PATINDEX('% var. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0)
											SET @Rank = 'var.';
										else if (PATINDEX('% ssp. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0)
											SET @Rank = 'ssp.';
										else if (PATINDEX('% fm. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0)
											SET @Rank = 'fm.';
										else if (PATINDEX('% subvar. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0)
											SET @Rank = 'subvar.';
										else if (PATINDEX('% f. %' COLLATE Latin1_General_CS_AS, @Taxon) > 0)
											SET @Rank = 'f.';
										SET @Authors = rtrim(ltrim(Substring(@Taxon, 1, patindex('% ' + @Rank + ' %', @Taxon))));
										SET @Taxon = rtrim(ltrim(Substring(@Taxon, patindex('% ' + @Rank + ' %', @Taxon) + len(@Rank) + 2, 500)));
										SET @IntraSpecificEpithet = @Taxon;
									END/**/
                                else
                                BEGIN
                                if (@Taxon like 'var.%' OR
									@Taxon like  'ssp.%' OR
									@Taxon like 'fm. %'  COLLATE Latin1_General_CS_AS OR
									@Taxon  like 'subvar. %' OR
									@Taxon like 'f. %'  COLLATE Latin1_General_CS_AS)
                                    BEGIN
                                        SET @Rank = ltrim(rtrim(Substring(@Taxon, 1, charindex('.', @Taxon) + 1)));
                                        SET @Taxon = ltrim(rtrim(Substring(@Taxon, charindex('.', @Taxon) + 1, 500)));
                                        if (charindex(' ' , @Taxon) > 0)
                                        BEGIN
                                            SET @IntraSpecificEpithet = rtrim(ltrim(Substring(@Taxon, 1, charindex(' ', @Taxon))));
                                            SET @Taxon = ltrim(rtrim(Substring(@Taxon, charindex(' ' , @Taxon), 500)));
                                        END
                                        else
                                        BEGIN
                                            SET @IntraSpecificEpithet = @Taxon;
                                            SET @Taxon = '';
                                        END
                                    END
                                    if (len(@Taxon) > 0 AND Substring(@Taxon, 1, 1) COLLATE Latin1_General_CS_AS = upper(Substring(@Taxon, 1, 1)) COLLATE Latin1_General_CS_AS)
                                        SET @Authors = @Taxon;
                                    else if (len(@Taxon) > 0)
                                    BEGIN
                                        SET @IntraSpecificEpithet = @Taxon;
                                        SET @Rank = 'ssp.';
                                    END
                                    if (len(@Authors) > 0 AND
                                        (@Authors LIKE ('%;')
                                        OR @Authors LIKE('%,')))
                                    BEGIN
                                        SET @Authors = ltrim(rtrim(Substring(@Authors, 1, len(@Authors) - 1)));
                                    END
                                END
                            END
                            else
                                SET @SpeciesEpithet = @Taxon;
                        END
                    END
                    else
                    BEGIN
                        SET @Rank = 'gen.';
                        SET @Genus = @Taxon;
                    END
                END

                IF (@Qualifier = 'aff. forma' 
                   OR @Qualifier = 'aff. gen.' 
                   OR @Qualifier = 'aff. sp.' 
                   OR @Qualifier = 'aff. ssp.'
                   OR @Qualifier = 'aff. var.')
                   BEGIN
                        SET @QualifierRank = ltrim(rtrim(Substring(@Qualifier, 6, 50)));
                        if (@QualifierRank = 'forma') set @QualifierRank = 'f.'
                        SET @QualifierText = 'aff.';
                    END
                    ELSE
                    IF (@Qualifier = 'cf. forma'
                   OR @Qualifier = 'cf. gen.'
                   OR @Qualifier = 'cf. sp.'
                   OR @Qualifier = 'cf. ssp.'
                   OR @Qualifier = 'cf. var.')
                   BEGIN
                        SET @QualifierRank = ltrim(rtrim(Substring(@Qualifier, 5, 50)));
                        if (@QualifierRank = 'forma') set @QualifierRank = 'f.'
                        SET @QualifierText = 'cf.';
                   END;
                   else
                    IF (@Qualifier = 'cf. hybrid')
                    begin
                        SET @QualifierRank = 'hybrid';
                        SET @QualifierText = 'cf.';
                        end;
                        else
                    IF (@Qualifier = '?'
                   OR @Qualifier = 'agg.'
                   OR @Qualifier = 's. l.'
                   OR @Qualifier = 's. str.'
                   OR @Qualifier = 'sp.'
                   OR @Qualifier = 'sp. nov.' -- #1 (von 2) falls Rank bei sp. nov. mit ausgegeben werden soll auskommentieren
                   OR @Qualifier = 'spp.')
                   begin
                        SET @QualifierRank = '';
                        SET @QualifierText = @Qualifier;
                        end;
                END
                SET @Taxon = '';
                if (@QualifierText = '?') SET @Taxon = '? ';
                if ((@QualifierRank = 'gen.' OR @QualifierRank = 'hybrid') AND (@QualifierText = 'aff.' OR @QualifierText = 'cf.')) 
                SET @Taxon = @QualifierText + ' ';
                SET @Taxon += @Genus + ' ';
                if (@QualifierRank = 'sp.' AND len(@QualifierText) > 0)
                    SET @Taxon += @QualifierText + ' ';
                if (len(@SpeciesEpithet) > 0)
                    SET @Taxon = @Taxon + @SpeciesEpithet + ' ';
                if (@QualifierRank <> 'sp.' AND
                    @QualifierRank <> 'gen.' AND
                    len(@QualifierRank) > 0 AND
                    len(@QualifierText) > 0 AND
                    @QualifierText <> '?' AND
                    @IntraSpecificEpithet <> @SpeciesEpithet)
                    SET @Taxon += @QualifierText + ' ' + @QualifierRank + ' ';
                if (len(@IntraSpecificEpithet) = 0)
                BEGIN
                    if (len(@Authors) > 0 AND @QualifierText <> 'agg.' AND @QualifierText <> 'sp.')
                        SET @Taxon += @Authors + ' ';
                END
                else
                BEGIN
                    if (@SpeciesEpithet = @IntraSpecificEpithet AND len(@Authors) > 0 AND @QualifierText <> 'agg.' AND @QualifierText <> 'sp.')
                        SET @Taxon += @Authors + ' ';
                    if (@QualifierRank <> 'sp.' AND
                       @QualifierRank <> 'gen.' AND
                       len(@QualifierRank) > 0 AND
                       len(@QualifierText) > 0 AND
                       @QualifierText <> '?' AND
                       @IntraSpecificEpithet = @SpeciesEpithet)
                       SET @Taxon += @QualifierText + ' ';
                    SET @Taxon += @IntraSpecificEpithet + ' ';
                    if (@SpeciesEpithet <> @IntraSpecificEpithet AND len(@Authors) > 0 AND @QualifierText <> 'agg.' AND @QualifierText <> 'sp.')
                        SET @Taxon += @Authors + ' ';
                END
                if (len(@QualifierText) > 0 AND len(@QualifierRank) = 0 AND @QualifierText <> '?')
                    SET @Taxon += @QualifierText;

            if (@Taxon = '' OR (LEN(@Taxon) < Len(@_Taxon) AND @QualifierText <> 'agg.' AND @QualifierText <> 'sp.' AND @QualifierText <> '?'))
                SET @Taxon = @_Taxon;
                -- #2 von 2 falls Rank bei sp. nov. mit ausgegeben werden soll aktivieren
				-- if (@Qualifier = 'sp. nov.' AND @QualifierText = '' AND @Rank <> 'sp.' and @Rank <> 'gen.' and @QualifierRank = '')
				--		set @Taxon += ' ' + @Qualifier;
                return @Taxon;
END

GO

GRANT EXEC ON dbo.TaxonWithQualifier TO [User]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Generates a valid name for a taxon using the taxonomic name and the identification qualifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'TaxonWithQualifier'
GO



-- Entfernung der CollectionID aus der Tabelle CollectionSpecimen - ausschliesslich verfuegbar in CollectionSpecimenPart
--#####################################################################################################################
--######   [CollectionID]   ######################################################################################
--#####################################################################################################################


UPDATE P SET P.CollectionID = S.CollectionSpecimenID
FROM         CollectionSpecimen AS S INNER JOIN
                      Collection AS C ON S.CollectionID = C.CollectionID INNER JOIN
                      CollectionSpecimenPart AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID
WHERE     (P.CollectionID IS NULL) AND (NOT (S.CollectionID IS NULL))
GO

INSERT INTO [CollectionSpecimenPart]
           ([CollectionSpecimenID]
           ,[CollectionID]
           ,[MaterialCategory]
           ,[StorageLocation]
           ,[Stock])
SELECT     S.CollectionSpecimenID, S.CollectionID, 'specimen' AS MaterialCategory, CASE WHEN MAX(U.LastIdentificationCache) IS NULL 
                      THEN '' ELSE CASE WHEN MAX(U.LastIdentificationCache) = '' then MAX(U.TaxonomicGroup) else MAX(U.LastIdentificationCache) end END AS StorageLocation, 1 AS Expr3
FROM         CollectionSpecimen AS S INNER JOIN
                      Collection AS C ON S.CollectionID = C.CollectionID LEFT OUTER JOIN
                      IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID LEFT OUTER JOIN
                      CollectionSpecimenPart AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID
WHERE     (NOT (S.CollectionID IS NULL)) AND (P.CollectionSpecimenID IS NULL) and(P.CollectionID IS NULL)
GROUP BY S.CollectionSpecimenID, S.CollectionID
GO

--#####################################################################################################################
--######  [CollectionDateCategory] 'collection date'   ######################################################################################
--#####################################################################################################################


UPDATE E SET [CollectionDateCategory] = 'actual'
  FROM [CollectionEvent]
E where E.CollectionDateCategory = 'collection date'
GO


delete e
FROM [CollEventDateCategory_Enum]
e where e.Code = 'collection date'
GO




--#####################################################################################################################
--######   [NextFreeAccNumber]  ######################################################################################
-- beruecksichtigt optional auch Parts
--#####################################################################################################################

/****** Object:  UserDefinedFunction [dbo].[NextFreeAccNumber]    Script Date: 10/02/2013 16:13:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NextFreeAccNumber]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[NextFreeAccNumber]
GO



CREATE FUNCTION [dbo].[NextFreeAccNumber] (@AccessionNumber nvarchar(50), @IncludeSpecimen bit, @IncludePart bit)  
/*
returns next free accession number for parts or specimen
optionally including either parts or specimen
assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453
with a leading string and a numeric end
MW 05.09.2013
TEST:
select dbo.[NextFreeAccNumber] ('0033933', 1, 1)
select dbo.[NextFreeAccNumber] ('0033933', 0, 1)
select dbo.[NextFreeAccNumber] ('00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 0, 1)
select dbo.[NextFreeAccNumber] ('ZSM_DIP-000', 1, 1)
select dbo.[NextFreeAccNumber] ('1907/9', 1, 1)
select dbo.[NextFreeAccNumber] ('ZSM-MA-9', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0013622', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014900', 1, 1)
select dbo.[NextFreeAccNumber] ('MB-FA-000001', 1, 1) 
select dbo.[NextFreeAccNumber] ('MB-FA-000101', 1, 1) 
*/
RETURNS nvarchar (50)
AS
BEGIN 
declare @NextAcc nvarchar(50)
set @NextAcc = ''
declare @Start int
declare @Position tinyint
declare @Prefix nvarchar(50)
set @Position = len(@AccessionNumber) 
if (isnumeric(@AccessionNumber) = 1)
begin
	set @Prefix = substring(@AccessionNumber, 1, len(@AccessionNumber) - len(cast(cast(@AccessionNumber as int) as varchar)))
	set @Start = cast(@AccessionNumber as int)
end
else
begin
while (isnumeric(rtrim(substring(@AccessionNumber, @Position, len(@AccessionNumber)))) = 1)
begin
	set @Start = CAST(substring(@AccessionNumber, @Position, len(@AccessionNumber)) as int)
	set @Prefix = substring(@AccessionNumber, 1, @Position)
	set @Position = @Position - 1
end
end
if (@Start < 0) 
begin 
	set @Start = @Start * -1;
end
declare @Space nvarchar(1)
set @Space = ''
if (SUBSTRING(@AccessionNumber, @Position + 1, 1) = ' ')
begin
	set @Space = '_'
end
if (LEN(@Prefix) = LEN(@AccessionNumber))
begin
	set @Prefix = SUBSTRING(@Prefix, 1, len(@Prefix) - 1)
end
declare @T Table (ID int identity(1, 1),
	NumericPart int NULL,
    AccessionNumber nvarchar(50) NULL)
if (@IncludeSpecimen = 1)
begin    
INSERT INTO @T (AccessionNumber)
SELECT AccessionNumber 
FROM CollectionSpecimen  
WHERE AccessionNumber LIKE @Prefix + '%'
AND AccessionNumber >= @AccessionNumber
end
if (@IncludePart = 1)
begin    
INSERT INTO @T (AccessionNumber)
SELECT AccessionNumber 
FROM CollectionSpecimenPart  
WHERE AccessionNumber LIKE @Prefix + '%'
AND AccessionNumber >= @AccessionNumber
end
if (select COUNT(*) from @T) = 0
begin
INSERT INTO @T (AccessionNumber)
SELECT @AccessionNumber 
end
UPDATE @T SET NumericPart = ID + @Start;
UPDATE @T SET AccessionNumber = @Prefix 
+ case when (len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) > 0 
then replicate('0', len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) else '' end
+ CAST(NumericPart as varchar);
if (@IncludeSpecimen = 1)
begin
	Delete T from @T T inner join  CollectionSpecimen S on S.AccessionNumber = T.AccessionNumber
	where not S.AccessionNumber is null
end	
if (@IncludePart = 1)
begin
	Delete T from @T T inner join  CollectionSpecimenPart S on S.AccessionNumber = T.AccessionNumber
	where not S.AccessionNumber is null
end	
set @NextAcc = (SELECT MIN(T.AccessionNumber) from @T T)
return (@NextAcc)
END

GO

GRANT EXEC ON dbo.NextFreeAccNumber TO [Editor]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The next free accession number starting like parameter 1, optional inclusion of specimen (parameter 2) and parts (parameter 3)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'NextFreeAccNumber'
GO


--#####################################################################################################################
--######   http://mfnpaleo.paleodatabase.de/cgi-bin/bridge.pl   ######################################################################################
-- Webservice existiert an alter Stelle nicht mehr
--#####################################################################################################################


UPDATE I SET [NameURI] = REPLACE([NameURI], 'http://mfnpaleo.paleodatabase.de/cgi-bin/bridge.pl', 'http://paleodb.org/cgi-bin/bridge.pl')
  FROM [Identification]
I where I.NameURI like 'http://mfnpaleo.paleodatabase.de/cgi-bin/bridge.pl%'

GO


--#####################################################################################################################
--######   Method   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[Method](
	[MethodID] [int] IDENTITY(1,1) NOT NULL,
	[MethodParentID] [int] NULL,
	[OnlyHierarchy] [bit] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[MethodURI] [varchar](255) NULL,
	[ForCollectionEvent] [bit] NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_Method] PRIMARY KEY CLUSTERED 
(
	[MethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Method (Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MethodID of the parent Method if it belongs to a certain type documented in this table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'MethodParentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the entry is only used for the hierarchical arrangement of the entries' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'OnlyHierarchy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the Method as e.g. shown in user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the Method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to an external documentation of the Method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'MethodURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If a tool may be used during a collection event' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'ForCollectionEvent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes concerning this analysis Method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Methods used within the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Method'
GO

ALTER TABLE [dbo].[Method]  WITH NOCHECK ADD  CONSTRAINT [FK_Method_Method] FOREIGN KEY([MethodParentID])
REFERENCES [dbo].[Method] ([MethodID])
GO

ALTER TABLE [dbo].[Method] CHECK CONSTRAINT [FK_Method_Method]
GO

ALTER TABLE [dbo].[Method] ADD  CONSTRAINT [DF_Method_OnlyHierarchy]  DEFAULT ((0)) FOR [OnlyHierarchy]
GO

ALTER TABLE [dbo].[Method] ADD  CONSTRAINT [DF_Method_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[Method] ADD  CONSTRAINT [DF_Method_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[Method] ADD  CONSTRAINT [DF_Method_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[Method] ADD  CONSTRAINT [DF_Method_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[Method] ADD  CONSTRAINT [DF_Method_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT SELECT ON Method TO [User]
GO

GRANT UPDATE ON Method TO [Administrator]
GO

GRANT INSERT ON Method TO [Administrator]
GO

GRANT DELETE ON Method TO [Administrator]
GO


--#####################################################################################################################
--######   Method_log   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Method_log](
	[MethodID] [int] NULL,
	[MethodParentID] [int] NULL,
	[OnlyHierarchy] [bit] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[MethodURI] [varchar](255) NULL,
	[ForCollectionEvent] [bit] NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Method_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[Method_log] ADD  CONSTRAINT [DF_Method_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[Method_log] ADD  CONSTRAINT [DF_Method_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[Method_log] ADD  CONSTRAINT [DF_Method_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON [Method_log] TO [User]
GO

GRANT INSERT ON [Method_log] TO [Administrator]
GO

--#####################################################################################################################
--######   [trgDelMethod]   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelMethod] ON [dbo].[Method] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.10.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Method_Log (MethodID, MethodParentID, OnlyHierarchy, DisplayText, Description, MethodURI, ForCollectionEvent, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.MethodID, deleted.MethodParentID, deleted.OnlyHierarchy, deleted.DisplayText, deleted.Description, deleted.MethodURI, deleted.ForCollectionEvent, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######   [trgUpdMethod]   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdMethod] ON [dbo].[Method] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.10.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Method_Log (MethodID, MethodParentID, OnlyHierarchy, DisplayText, Description, MethodURI, ForCollectionEvent, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.MethodID, deleted.MethodParentID, deleted.OnlyHierarchy, deleted.DisplayText, deleted.Description, deleted.MethodURI, deleted.ForCollectionEvent, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update Method
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Method, deleted 
where 1 = 1 
AND Method.MethodID = deleted.MethodID
GO


--#####################################################################################################################
--######   [MethodChildNodes]   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[MethodChildNodes] (@ID int)  
RETURNS @ItemList TABLE (MethodID int primary key,
	MethodParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	ForCollectionEvent [bit]   NULL ,
	Notes nvarchar  (1000)   NULL ,
	MethodURI varchar  (255)   NULL ,
	OnlyHierarchy [bit] NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (MethodID int primary key,
	MethodParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	ForCollectionEvent [bit]   NULL ,
	Notes nvarchar  (1000)   NULL ,
	MethodURI varchar  (255)   NULL,
	OnlyHierarchy [bit] NULL ,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)

 INSERT @TempItem (MethodID , MethodParentID, DisplayText , Description , ForCollectionEvent, Notes , MethodURI, OnlyHierarchy, RowGUID) 
	SELECT MethodID , MethodParentID, DisplayText , Description , ForCollectionEvent, Notes , MethodURI, OnlyHierarchy, RowGUID
	FROM Method WHERE MethodParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select MethodID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select MethodID , MethodParentID, DisplayText , Description , ForCollectionEvent, Notes , MethodURI, OnlyHierarchy, RowGUID
	from dbo.MethodChildNodes (@ParentID) where MethodID not in (select MethodID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (MethodID , MethodParentID, DisplayText , Description , ForCollectionEvent, Notes , MethodURI, OnlyHierarchy, RowGUID) 
   SELECT distinct MethodID , MethodParentID, DisplayText , Description , ForCollectionEvent, Notes , MethodURI, OnlyHierarchy, RowGUID
   FROM @TempItem ORDER BY DisplayText
   RETURN
END

GO

GRANT SELECT ON [dbo].[MethodChildNodes] TO [User]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'All child nodes of a given method related via the MethodParentID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'MethodChildNodes'
GO



--#####################################################################################################################
--######   MethodHierarchy   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[MethodHierarchy] (@MethodID int)  
RETURNS @MethodList TABLE ([MethodID] [int] Primary key ,
	[MethodParentID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[MethodURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[ForCollectionEvent] [bit] NULL,
	[OnlyHierarchy] [bit] NULL)


/*
Returns a table that lists all the Method items related to the given Method.
MW 02.01.2006
Test
SELECT  *  FROM dbo.MethodHierarchy(82)

*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int

set @TopID = (select MethodParentID from Method where MethodID = @MethodID) 

set @i = (select count(*) from Method where MethodID = @MethodID)

if (@TopID is null )
	set @TopID =  @MethodID
else	
	begin
	while (@i > 0)
		begin
		set @MethodID = (select MethodParentID from Method where MethodID = @MethodID and not MethodParentID is null) 
		set @i = (select count(*) from Method where MethodID = @MethodID and not MethodParentID is null)
		end
	set @TopID = @MethodID
	end

-- copy the root node in the result list
   INSERT @MethodList
   SELECT DISTINCT MethodID, MethodParentID, DisplayText, Description, Notes, MethodURI, ForCollectionEvent, OnlyHierarchy
   FROM Method
   WHERE Method.MethodID = @TopID

-- copy the child nodes into the result list
   INSERT @MethodList
   SELECT MethodID, MethodParentID, DisplayText, Description, Notes, MethodURI, ForCollectionEvent, OnlyHierarchy
   FROM dbo.MethodChildNodes (@TopID)
   
-- delete the heaeders
   /*DELETE A FROM @MethodList A
   WHERE A.OnlyHierarchy = 1*/

   RETURN
END



GO

GRANT SELECT ON [dbo].[MethodHierarchy] TO [User]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The hierarchy of a given method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'MethodHierarchy'
GO



--#####################################################################################################################
--######   [MethodHierarchyAll]   ######################################################################################
--#####################################################################################################################




CREATE FUNCTION [dbo].[MethodHierarchyAll] ()  
RETURNS @MethodList TABLE ([MethodID] [int] Primary key ,
	[MethodParentID] [int] NULL ,
	[DisplayText] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[MethodURI] [varchar](255) NULL,
	[ForCollectionEvent] [bit] NULL,
	[OnlyHierarchy] [bit] NULL,
	[HierarchyDisplayText] [nvarchar] (900) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the Method items related to the given Method.
MW 02.01.2006
TEST:
SELECT * FROM DBO.MethodHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @MethodList (MethodID, MethodParentID, DisplayText, Description, ForCollectionEvent, Notes, MethodURI, OnlyHierarchy, HierarchyDisplayText)
SELECT DISTINCT MethodID, MethodParentID, DisplayText, Description, ForCollectionEvent, Notes, MethodURI, OnlyHierarchy, DisplayText
FROM Method
WHERE Method.MethodParentID IS NULL

declare @i int
set @i = (select count(*) from Method where MethodID not IN (select MethodID from  @MethodList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @MethodList (MethodID, MethodParentID, DisplayText, Description, ForCollectionEvent, Notes, MethodURI, OnlyHierarchy, HierarchyDisplayText)
	SELECT DISTINCT C.MethodID, C.MethodParentID, C.DisplayText, C.Description, C.ForCollectionEvent, C.Notes, C.MethodURI, C.OnlyHierarchy, L.HierarchyDisplayText + ' | ' + C.DisplayText
	FROM Method C, @MethodList L
	WHERE C.MethodParentID = L.MethodID
	AND C.MethodID NOT IN (select MethodID from  @MethodList)

	set @i = (select count(*) from Method where MethodID not IN (select MethodID from  @MethodList))
end


   RETURN
END

GO

GRANT SELECT ON [dbo].[MethodHierarchyAll] TO [User]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'All methods including a column displaying the hierarchy of the method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'MethodHierarchyAll'
GO



--#####################################################################################################################
--######   CollectionEventMethod   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionEventMethod](
	[CollectionEventID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_CollectionEventMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[MethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to ID of CollectionSpecimen (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventMethod', @level2type=N'COLUMN',@level2name=N'CollectionEventID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the setting, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventMethod', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventMethod', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventMethod', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventMethod', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventMethod', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The methods used within a collection event' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventMethod'
GO


ALTER TABLE [dbo].[CollectionEventMethod]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventMethod_CollectionEvent] FOREIGN KEY([CollectionEventID])
REFERENCES [dbo].[CollectionEvent] ([CollectionEventID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionEventMethod] CHECK CONSTRAINT [FK_CollectionEventMethod_CollectionEvent]
GO

ALTER TABLE [dbo].[CollectionEventMethod]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventMethod_Method] FOREIGN KEY([MethodID])
REFERENCES [dbo].[Method] ([MethodID])
GO

ALTER TABLE [dbo].[CollectionEventMethod] CHECK CONSTRAINT [FK_CollectionEventMethod_Method]
GO

ALTER TABLE [dbo].[CollectionEventMethod] ADD  CONSTRAINT [DF_CollectionEventMethod_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[CollectionEventMethod] ADD  CONSTRAINT [DF_CollectionEventMethod_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[CollectionEventMethod] ADD  CONSTRAINT [DF_CollectionEventMethod_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionEventMethod] ADD  CONSTRAINT [DF_CollectionEventMethod_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO


GRANT SELECT ON CollectionEventMethod TO [User]
GO

GRANT UPDATE ON CollectionEventMethod TO [Editor]
GO

GRANT INSERT ON CollectionEventMethod TO [Editor]
GO

GRANT DELETE ON CollectionEventMethod TO [Editor]
GO

--#####################################################################################################################
--######   CollectionEventMethod_log   ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[CollectionEventMethod_log](
	[CollectionEventID] [int] NULL,
	[MethodID] [int] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionEventMethod_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CollectionEventMethod_log] ADD  CONSTRAINT [DF_CollectionEventMethod_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionEventMethod_log] ADD  CONSTRAINT [DF_CollectionEventMethod_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionEventMethod_log] ADD  CONSTRAINT [DF_CollectionEventMethod_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON [CollectionEventMethod_log] TO [User]
GO

GRANT INSERT ON [CollectionEventMethod_log] TO [Administrator]
GO


--#####################################################################################################################
--######   [trgDelCollectionEventMethod]   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelCollectionEventMethod] ON [dbo].[CollectionEventMethod] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.10.2013  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM deleted)
   EXECUTE procSetVersionCollectionEvent @ID
   SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'D' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end
GO


--#####################################################################################################################
--######   [trgUpdCollectionEventMethod]   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionEventMethod] ON [dbo].[CollectionEventMethod] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.10.2013  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM deleted)
   EXECUTE procSetVersionCollectionEvent @ID
   SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionEventMethod_Log (CollectionEventID, MethodID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionEvent.Version, 'U' 
FROM DELETED, CollectionEvent
WHERE deleted.CollectionEventID = CollectionEvent.CollectionEventID
end

/* updating the logging columns */
Update CollectionEventMethod
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventMethod, deleted 
where 1 = 1 
AND CollectionEventMethod.CollectionEventID = deleted.CollectionEventID
AND CollectionEventMethod.MethodID = deleted.MethodID
GO



--#####################################################################################################################
--######   Parameter   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Parameter](
	[MethodID] [int] NOT NULL,
	[ParameterID] [int] IDENTITY(1,1) NOT NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[ParameterURI] [varchar](255) NULL,
	[DefaultValue] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Parameter] PRIMARY KEY CLUSTERED 
(
	[ParameterID] ASC,
	[MethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Method (Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Parameter (Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'ParameterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the parameter as e.g. shown in user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the parameter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to an external documentation of the Parameter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'ParameterURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default value of the parameter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'DefaultValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes concerning this parameter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The variable parameters within a method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parameter'
GO

ALTER TABLE [dbo].[Parameter]  WITH CHECK ADD  CONSTRAINT [FK_Parameter_Method] FOREIGN KEY([MethodID])
REFERENCES [dbo].[Method] ([MethodID])
GO

ALTER TABLE [dbo].[Parameter] CHECK CONSTRAINT [FK_Parameter_Method]
GO

ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_Parameter_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_Parameter_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_Parameter_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_Parameter_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_Parameter_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT SELECT ON Parameter TO [User]
GO

GRANT UPDATE ON Parameter TO [Administrator]
GO

GRANT INSERT ON Parameter TO [Administrator]
GO

GRANT DELETE ON Parameter TO [Administrator]
GO

--#####################################################################################################################
--######   [Parameter_log]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Parameter_log](
	[MethodID] [int] NULL,
	[ParameterID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[ParameterURI] [varchar](255) NULL,
	[DefaultValue] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Parameter_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Parameter_log] ADD  CONSTRAINT [DF_Parameter_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[Parameter_log] ADD  CONSTRAINT [DF_Parameter_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[Parameter_log] ADD  CONSTRAINT [DF_Parameter_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO

--#####################################################################################################################
--######   [trgDelParameter]   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelParameter] ON [dbo].[Parameter] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.10.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Parameter_Log (MethodID, ParameterID, DisplayText, Description, ParameterURI, DefaultValue, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.MethodID, deleted.ParameterID, deleted.DisplayText, deleted.Description, deleted.ParameterURI, deleted.DefaultValue, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######   [trgUpdParameter]   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdParameter] ON [dbo].[Parameter] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 18.10.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Parameter_Log (MethodID, ParameterID, DisplayText, Description, ParameterURI, DefaultValue, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.MethodID, deleted.ParameterID, deleted.DisplayText, deleted.Description, deleted.ParameterURI, deleted.DefaultValue, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update Parameter
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Parameter, deleted 
where 1 = 1 
AND Parameter.MethodID = deleted.MethodID
AND Parameter.ParameterID = deleted.ParameterID
GO



--#####################################################################################################################
--######   CollectionEventParameterValue   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionEventParameterValue](
	[CollectionEventID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_CollectionEventParameterValue] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[ParameterID] ASC,
	[MethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the collection event   (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'CollectionEventID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the method tool. Referes to table Method  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the parameter tool. Referes to table Parameter  (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'ParameterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of the parameter if different of the default value as documented in the table Parameter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The values of the parameter of the methods used within a collection event' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue'
GO

ALTER TABLE [dbo].[CollectionEventParameterValue]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventParameterValue_CollectionEventMethod] FOREIGN KEY([CollectionEventID], [MethodID])
REFERENCES [dbo].[CollectionEventMethod] ([CollectionEventID], [MethodID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionEventParameterValue] CHECK CONSTRAINT [FK_CollectionEventParameterValue_CollectionEventMethod]
GO

ALTER TABLE [dbo].[CollectionEventParameterValue]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventParameterValue_Parameter] FOREIGN KEY([ParameterID], [MethodID])
REFERENCES [dbo].[Parameter] ([ParameterID], [MethodID])
GO

ALTER TABLE [dbo].[CollectionEventParameterValue] CHECK CONSTRAINT [FK_CollectionEventParameterValue_Parameter]
GO


GRANT SELECT ON CollectionEventParameterValue TO [User]
GO

GRANT UPDATE ON CollectionEventParameterValue TO [Editor]
GO

GRANT INSERT ON CollectionEventParameterValue TO [Editor]
GO

GRANT DELETE ON CollectionEventParameterValue TO [Editor]
GO


--#####################################################################################################################
--######   ParameterValue_Enum   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ParameterValue_Enum](
	[MethodID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[Value] [nvarchar](400) NOT NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[URI] [varchar](255) NULL,
 CONSTRAINT [PK_ParameterValue_Enum] PRIMARY KEY CLUSTERED 
(
	[MethodID] ASC,
	[ParameterID] ASC,
	[Value] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Method (Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Method (Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'ParameterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the Method as e.g. shown in user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the Method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to an external documentation of the Method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum', @level2type=N'COLUMN',@level2name=N'URI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Distinct values for a parameter of a method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParameterValue_Enum'
GO

ALTER TABLE [dbo].[ParameterValue_Enum]  WITH CHECK ADD  CONSTRAINT [FK_ParameterValue_Enum_Parameter] FOREIGN KEY([ParameterID], [MethodID])
REFERENCES [dbo].[Parameter] ([ParameterID], [MethodID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ParameterValue_Enum] CHECK CONSTRAINT [FK_ParameterValue_Enum_Parameter]
GO



GRANT SELECT ON ParameterValue_Enum TO [User]
GO

GRANT UPDATE ON ParameterValue_Enum TO [Administrator]
GO

GRANT INSERT ON ParameterValue_Enum TO [Administrator]
GO

GRANT DELETE ON ParameterValue_Enum TO [Administrator]
GO

--#####################################################################################################################
--######   setting the Version of the client   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.06.20' 
END
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.39'
END

GO



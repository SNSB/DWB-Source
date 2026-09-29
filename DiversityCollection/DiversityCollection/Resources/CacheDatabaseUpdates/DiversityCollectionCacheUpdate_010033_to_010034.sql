
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.33'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   TaxonSynonymy - Add new columns for Name Details ###########################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'InfragenericEpithet') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [InfragenericEpithet] [nvarchar](200) NULl;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'SpeciesEpithet') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [SpeciesEpithet] [nvarchar](100) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'InfraspecificEpithet') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [InfraspecificEpithet] [nvarchar](100) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'Authors') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [Authors] [nvarchar](500) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'BasionymAuthors') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [BasionymAuthors] [nvarchar](100) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'CombiningAuthors') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [CombiningAuthors] [nvarchar](255) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'SanctioningAuthor') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [SanctioningAuthor] [nvarchar](100) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'NonNomenclaturalNameSuffix') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [NonNomenclaturalNameSuffix] [nvarchar](200) NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'IsRecombination') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [IsRecombination] [bit] NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'YearOfPubl') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [YearOfPubl] [smallint] NULL;
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS N where N.TABLE_NAME = 'TaxonSynonymy' AND N.COLUMN_NAME = 'NomenclaturalCode') = 0
begin
	ALTER TABLE [dbo].[TaxonSynonymy] ADD [NomenclaturalCode] [nvarchar](50) NULL;
end
GO



--#####################################################################################################################
--######   Adaption of procedure procTransferTaxonSynonymy   ##########################################################
--#####################################################################################################################

-- wird nicht mehr benötigt, da Transfer anders erfolgt

/*
declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [dbo].[procTransferTaxonSynonymy] 
@Source nvarchar(50),
@View nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @SQL nvarchar(4000)
	set @SQL = (select ''CREATE PROCEDURE [dbo].[procTransferTaxonSynonymy] 
		@Source nvarchar(50),
		@View nvarchar(50)
		AS
		BEGIN
		INSERT INTO TaxonSynonymy
			(NameID, BaseURL, TaxonName, AcceptedNameID, AcceptedName, TaxonomicRank, SpeciesGenusNameID, GenusOrSupragenericName, NameParentID, 
			TaxonNameSinAuthor, ProjectID, Source, InfragenericEpithet, SpeciesEpithet, InfraspecificEpithet, Authors
			,BasionymAuthors, CombiningAuthors, SanctioningAuthor, NonNomenclaturalNameSuffix, IsRecombination, YearOfPubl, N.NomenclaturalCode)
		SELECT     N.NameID, N.BaseURL, N.TaxonName, N.AcceptedNameID, N.AcceptedName, N.TaxonomicRank, N.SpeciesGenusNameID, N.GenusOrSupragenericName, 
                H.NameParentID, N.TaxonNameSinAuthor, N.ProjectID, ''' + @Source + ''', N.InfragenericEpithet, N.SpeciesEpithet, N.InfraspecificEpithet, 
			case when N.NomenclaturalCode = 3 -- Zoology 
			then 
				case when  N.BasionymAuthors is null or N.BasionymAuthors = '' 
				then '' 
				else 
					case when N.IsRecombination = 1 then '' ('' else '' '' end +
					RTRIM(N.BasionymAuthors) +
					case when N.IsRecombination = 1 and NOT N.BasionymAuthorsYear IS null 
						then '', '' + cast(N.BasionymAuthorsYear AS varchar) 
						else case when N.IsRecombination = 0 and not N.YearOfPubl is null 
							then '', '' + cast(N.YearOfPubl AS varchar) 
							else '''' end
					end
					+ case when N.IsRecombination = 1 then '')'' else '''' end
				end +
				case when N.NonNomenclaturalNameSuffix IS NULL then '''' else '' '' + RTRIM(N.NonNomenclaturalNameSuffix) end
			else
				case when  N.BasionymAuthors is null or N.BasionymAuthors = '''' 
				then '''' 
				else 	case when N.CombiningAuthors is null or N.CombiningAuthors = '''' 
					then  	'' '' + RTRIM(N.BasionymAuthors) +
						case when  N.SanctioningAuthor is null or N.SanctioningAuthor = ''''  then '''' else '' : '' + RTRIM(N.SanctioningAuthor) end 
					else 	'' ('' + RTRIM(N.BasionymAuthors) +
						case when  N.SanctioningAuthor is null or N.SanctioningAuthor = ''''  then '''' else '' : '' + RTRIM(N.SanctioningAuthor) end
						+ '') '' 
					end 
				end +
				case when  N.CombiningAuthors is null or N.CombiningAuthors = ''''  then '''' else RTRIM(N.CombiningAuthors) end +
				case when N.NonNomenclaturalNameSuffix IS NULL then '''' else '' '' + RTRIM(N.NonNomenclaturalNameSuffix) end
			end AS Authors
			,N.BasionymAuthors, N.CombiningAuthors, N.SanctioningAuthor, N.NonNomenclaturalNameSuffix, N.IsRecombination, N.YearOfPubl, N.NomenclaturalCode
	FROM ' + @View + ' AS N LEFT OUTER JOIN
         ' + @View + '_H AS H ON N.NameID = H.NameID; 	DECLARE @i int
	SET @i = (SELECT COUNT(*) FROM TaxonSynonymy)
	SELECT CAST(@i AS VARCHAR) + '' taxonomic names imported''
END')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch


GO
*/


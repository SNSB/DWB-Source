
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.02'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   TaxonSynonymy   ############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'TaxonSynonymy') = 0
begin

CREATE TABLE [dbo].[TaxonSynonymy](
	[NameURI] [varchar](255) NULL,
	[AcceptedName] [nvarchar](255) NULL,
	[SynNameURI] [varchar](255) NULL,
	[SynonymName] [nvarchar](255) NULL,
	[TaxonomicRank] [nvarchar](50) NULL,
	[GenusOrSupragenericName] [nvarchar](200) NULL,
	[SpeciesGenusNameURI] [varchar](255) NULL,
	[TaxonNameSinAuthor] [nvarchar](2000) NULL,
	[LogInsertedWhen] [smalldatetime] NULL CONSTRAINT [DF_TaxonSynonymy_LogInsertedWhen_1]  DEFAULT (getdate()),
	[ProjectID] [int] NULL,
	[Source] [nvarchar](100) NULL
) ON [PRIMARY]



EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymy', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The source of the data, e.g. the name of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymy', @level2type=N'COLUMN',@level2name=N'Source'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds cached data from DiversityTaxonNames as base for other procedures.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymy'

END


GRANT SELECT ON TaxonSynonymy TO [CacheUser]
GO
GRANT DELETE ON TaxonSynonymy TO [CacheAdmin] 
GO
GRANT UPDATE ON TaxonSynonymy TO [CacheAdmin]
GO
GRANT INSERT ON TaxonSynonymy TO [CacheAdmin]
GO


--#####################################################################################################################
--######   TaxonSynonymySource   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'TaxonSynonymySource') = 0
begin

CREATE TABLE [dbo].[TaxonSynonymySource](
	[DatabaseName] [nvarchar](400) NOT NULL,
	[SourceName] [nvarchar](400) NULL,
 CONSTRAINT [PK_TaxonSynonymySource] PRIMARY KEY CLUSTERED 
(
	[DatabaseName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database where the data are taken from' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'DatabaseName'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'the name of the view retrieving the data from the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'SourceName'

END

GRANT SELECT ON TaxonSynonymySource TO [CacheUser]
GO
GRANT DELETE ON TaxonSynonymySource TO [CacheAdmin] 
GO
GRANT UPDATE ON TaxonSynonymySource TO [CacheAdmin]
GO
GRANT INSERT ON TaxonSynonymySource TO [CacheAdmin]
GO



--#####################################################################################################################
--######   procTransferTaxonSynonymy      #############################################################################
--#####################################################################################################################

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
	set @SQL = ''INSERT INTO TaxonSynonymy
						  (NameID, BaseURL, TaxonName, AcceptedNameID, AcceptedName, TaxonomicRank, SpeciesGenusNameID, GenusOrSupragenericName, NameParentID, 
                         TaxonNameSinAuthor, ProjectID, Source)
	SELECT     N.NameID, N.BaseURL, N.TaxonName, N.AcceptedNameID, N.AcceptedName, N.TaxonomicRank, N.SpeciesGenusNameID, N.GenusOrSupragenericName, 
                         H.NameParentID, N.TaxonNameSinAuthor, N.ProjectID, '''''' + @Source + ''''''
	FROM   '' + @View + '' AS N LEFT OUTER JOIN
                         '' + @View + ''_H AS H ON N.NameID = H.NameID    ''

	exec sp_executesql @SQL
	
	-- Bereinigung doppelter Eintraege
	declare @Duplicate int
	set @Duplicate = (SELECT COUNT(*) FROM TaxonSynonymy T WHERE EXISTS (
			SELECT     * FROM         TaxonSynonymy
			WHERE T.Source = @Source
			GROUP BY SynNameURI
			HAVING      (COUNT(*) > 1) AND (MIN(NameURI) <> MAX(NameURI)) AND (NOT (MIN(ProjectID) IS NULL))
			and SynNameURI <> MIN(NameURI) and SynNameURI <> MAX(NameURI)
			and T.ProjectID = MAX(ProjectID) AND T.NameURI = MAX(NameURI)))
	while @Duplicate > 0
	begin		
		delete T
		FROM TaxonSynonymy T
		WHERE T.Source = @Source
		AND EXISTS (
		SELECT     *
		FROM         TaxonSynonymy S
		WHERE S.Source = @Source
		GROUP BY SynNameURI
		HAVING      (COUNT(*) > 1) AND (MIN(NameURI) <> MAX(NameURI)) AND (NOT (MIN(ProjectID) IS NULL))
		and SynNameURI <> MIN(NameURI) and SynNameURI <> MAX(NameURI)
		and T.ProjectID = MAX(ProjectID) AND T.NameURI = MAX(NameURI))

	set @Duplicate = (SELECT COUNT(*) FROM TaxonSynonymy T WHERE T.Source = @Source 
			AND EXISTS (
			SELECT     * FROM         TaxonSynonymy S
			WHERE S.Source = @Source
			GROUP BY SynNameURI
			HAVING      (COUNT(*) > 1) AND (MIN(NameURI) <> MAX(NameURI)) AND (NOT (MIN(ProjectID) IS NULL))
			and SynNameURI <> MIN(NameURI) and SynNameURI <> MAX(NameURI)
			and T.ProjectID = MAX(ProjectID) AND T.NameURI = MAX(NameURI)))
	end


	DECLARE @i int
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


GRANT EXEC ON [dbo].[procTransferTaxonSynonymy] TO [CacheAdmin]
GO


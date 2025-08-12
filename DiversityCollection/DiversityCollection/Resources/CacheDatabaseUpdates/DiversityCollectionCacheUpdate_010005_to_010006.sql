
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.05'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ScientificTerm    ##########################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ScientificTerm') = 0
begin

CREATE TABLE [dbo].[ScientificTerm](
	[RepresentationURI] [varchar](255) NULL,
	[DisplayText] [nvarchar](255) NULL,
	[HierarchyCache] [varchar](900) NULL,
	[HierarchyCacheDown] [nvarchar](900) NULL,
	[RankingTerm] [nvarchar](200) NULL,
	[LogInsertedWhen] [smalldatetime] NULL CONSTRAINT [DF_ScientificTerm_LogInsertedWhen]  DEFAULT (getdate()),
	[SourceView] [nvarchar](400) NULL
) ON [PRIMARY]



EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTerm', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The source of the data,i.e. the name of the view in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTerm', @level2type=N'COLUMN',@level2name=N'SourceView'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds cached data from DiversityScientificTerms as base for other procedures.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTerm'

END


GRANT SELECT ON ScientificTerm TO [CacheUser]
GO
GRANT DELETE ON ScientificTerm TO [CacheAdmin] 
GO
GRANT UPDATE ON ScientificTerm TO [CacheAdmin]
GO
GRANT INSERT ON ScientificTerm TO [CacheAdmin]
GO


--#####################################################################################################################
--######   ScientificTermSource   #####################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ScientificTermSource') = 0
begin

CREATE TABLE [dbo].[ScientificTermSource](
	[SourceView] [nvarchar](400) NOT NULL,
	[Source] [nvarchar](50) NULL,
	[SourceID] [int] NULL,
	[ServerName] [nvarchar](400) NULL,
	[DatabaseName] [nvarchar](400) NULL,
 CONSTRAINT [PK_ScientificTermSource] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database where the data are taken from' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'DatabaseName'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'the name of the view retrieving the data from the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScientificTermSource', @level2type=N'COLUMN',@level2name=N'SourceView'

END

GRANT SELECT ON ScientificTermSource TO [CacheUser]
GO
GRANT DELETE ON ScientificTermSource TO [CacheAdmin] 
GO
GRANT UPDATE ON ScientificTermSource TO [CacheAdmin]
GO
GRANT INSERT ON ScientificTermSource TO [CacheAdmin]
GO



--#####################################################################################################################
--######   procTransferScientificTerm     #############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [dbo].[procTransferScientificTerm] 
@View nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @SQL nvarchar(4000)
	set @SQL = ''INSERT INTO ScientificTerm
						  (RepresentationURI, DisplayText, HierarchyCache, HierarchyCacheDown, RankingTerm, SourceView)
	SELECT     RepresentationURI, DisplayText, HierarchyCache, HierarchyCacheDown, RankingTerm, '''''' + @View + ''''''
	FROM         '' + @View

	exec sp_executesql @SQL
	
	DECLARE @i int
	SET @i = (SELECT COUNT(*) FROM ScientificTerm)
	SELECT CAST(@i AS VARCHAR) + '' scientific terms imported''

END')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch


GO


GRANT EXEC ON [dbo].[procTransferScientificTerm] TO [CacheAdmin]
GO



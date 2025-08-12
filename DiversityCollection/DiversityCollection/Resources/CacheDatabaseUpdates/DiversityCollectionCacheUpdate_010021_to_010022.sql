
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.21'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   ReferenceSource  ###########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ReferenceSource](
	[SourceView] [nvarchar](200) NOT NULL,
	[Source] [nvarchar](500) NULL,
	[SourceID] [int] NULL,
	[LinkedServerName] [nvarchar](500) NULL,
	[DatabaseName] [nvarchar](50) NULL,
	[Subsets] [nvarchar](500) NULL,
	[TransferProtocol] [nvarchar](max) NULL,
	[IncludeInTransfer] [bit] NULL,
 CONSTRAINT [PK_ReferenceSource] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'the name of the view retrieving the data from the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source is located on a linked server, the name of the linked server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'LinkedServerName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database where the data are taken from' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subsets of a source: The names of the tables included in the transfer separted by "|"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'Subsets'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the source should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReferenceSource', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO


GRANT SELECT ON ReferenceSource TO [CacheUser]
GO
GRANT DELETE ON ReferenceSource TO [CacheAdmin] 
GO
GRANT UPDATE ON ReferenceSource TO [CacheAdmin]
GO
GRANT INSERT ON ReferenceSource TO [CacheAdmin]
GO



--#####################################################################################################################
--######   ReferenceTitle   ###########################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ReferenceTitle](
	[BaseURL] [varchar] (500) NOT NULL,
	[RefType] [nvarchar](10) NOT NULL,
	[RefID] int NOT NULL,
	[RefDescription_Cache] nvarchar(255) NOT NULL,
	[Title] nvarchar(4000) NOT NULL,
	[DateYear] [smallint] NULL,
	[DateMonth] [smallint] NULL,
	[DateDay] [smallint] NULL,
	[DateSuppl] nvarchar(255) NOT NULL,
	[SourceTitle] nvarchar(4000) NOT NULL,
	[SeriesTitle] nvarchar(255) NOT NULL,
	[Periodical] nvarchar(255) NOT NULL,
	[Volume] nvarchar(255) NOT NULL,
	[Issue] nvarchar(255) NOT NULL,
	[Pages] nvarchar(255) NOT NULL,
	[Publisher] nvarchar(255) NOT NULL,
	[PublPlace] nvarchar(255) NOT NULL,
	[Edition] [smallint] NULL,
	[DateYear2] [smallint] NULL,
	[DateMonth2] [smallint] NULL,
	[DateDay2] [smallint] NULL,
	[DateSuppl2] nvarchar(255) NOT NULL,
	[ISSN_ISBN] [nvarchar](18) NOT NULL,
	[Miscellaneous1] nvarchar(255) NOT NULL,
	[Miscellaneous2] nvarchar(255) NOT NULL,
	[Miscellaneous3] nvarchar(255) NOT NULL,
	[UserDef1] nvarchar(4000) NOT NULL,
	[UserDef2] nvarchar(4000) NOT NULL,
	[UserDef3] nvarchar(4000) NOT NULL,
	[UserDef4] nvarchar(4000) NOT NULL,
	[UserDef5] nvarchar(4000) NOT NULL,
	[WebLinks] nvarchar(4000) NOT NULL,
	[LinkToPDF] nvarchar(4000) NOT NULL,
	[LinkToFullText] nvarchar(4000) NOT NULL,
	[RelatedLinks] nvarchar(4000) NOT NULL,
	[LinkToImages] nvarchar(4000) NOT NULL,
	[SourceRefID] int NULL,
	[Language] nvarchar(25) NOT NULL,
	[CitationText] nvarchar(1000) NOT NULL,
	[CitationFrom] nvarchar(255) NOT NULL,
	[LogInsertedWhen] [datetime] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[SourceView] nvarchar(128) NOT NULL,
 CONSTRAINT [ReferenceTitle_PK] PRIMARY KEY NONCLUSTERED 
(
	[BaseURL] ASC,
	[RefID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) 

GO

ALTER TABLE [dbo].[ReferenceTitle] ADD  CONSTRAINT [DF_ReferenceTitle_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO


GRANT SELECT ON ReferenceTitle TO [CacheUser]
GO
GRANT DELETE ON ReferenceTitle TO [CacheAdmin] 
GO
GRANT UPDATE ON ReferenceTitle TO [CacheAdmin]
GO
GRANT INSERT ON ReferenceTitle TO [CacheAdmin]
GO


--#####################################################################################################################
--######   procTransferReferenceTitle     #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [dbo].[procTransferReferenceTitle] 
@View nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @SQL nvarchar(4000)
	set @SQL = ''INSERT INTO ReferenceTitle
			  (BaseURL, RefType, RefID,RefDescription_Cache, Title, DateYear, DateMonth, DateDay, DateSuppl, SourceTitle, SeriesTitle, Periodical, Volume, Issue, Pages, Publisher,
               PublPlace, Edition, DateYear2, DateMonth2, DateDay2, DateSuppl2, ISSN_ISBN, Miscellaneous1, Miscellaneous2, Miscellaneous3, UserDef1, UserDef2, UserDef3, UserDef4,
               UserDef5, WebLinks, LinkToPDF, LinkToFullText, RelatedLinks, LinkToImages, SourceRefID, Language, CitationText, CitationFrom, LogUpdatedWhen, ProjectID, SourceView)
	SELECT     BaseURL, RefType, RefID,RefDescription_Cache, Title, DateYear, DateMonth, DateDay, DateSuppl, SourceTitle, SeriesTitle, Periodical, Volume, Issue, Pages, Publisher,
               PublPlace, Edition, DateYear2, DateMonth2, DateDay2, DateSuppl2, ISSN_ISBN, Miscellaneous1, Miscellaneous2, Miscellaneous3, UserDef1, UserDef2, UserDef3, UserDef4,
               UserDef5, WebLinks, LinkToPDF, LinkToFullText, RelatedLinks, LinkToImages, SourceRefID, Language, CitationText, CitationFrom, LogUpdatedWhen, ProjectID, '''''' + @View + ''''''
	FROM         '' + @View

	exec sp_executesql @SQL
	
	DECLARE @i int
	SET @i = (SELECT COUNT(*) FROM ReferenceTitle)
	SELECT CAST(@i AS VARCHAR) + '' references imported''

END')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   ReferenceRelator   #########################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ReferenceRelator](
	[RefID] int NOT NULL,
	[Role] [nvarchar](3) NOT NULL,
	[Sequence] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[AgentURI] [varchar](255) NULL,
	[SortLabel] [nvarchar](255) NULL,
	[Address] [nvarchar](1000) NULL,
	[SourceView] nvarchar(128) NOT NULL,
 CONSTRAINT [ReferenceRelator_PK] PRIMARY KEY NONCLUSTERED 
(
	[RefID] ASC,
	[Role] ASC,
	[Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO


GRANT SELECT ON ReferenceRelator TO [CacheUser]
GO
GRANT DELETE ON ReferenceRelator TO [CacheAdmin] 
GO
GRANT UPDATE ON ReferenceRelator TO [CacheAdmin]
GO
GRANT INSERT ON ReferenceRelator TO [CacheAdmin]
GO



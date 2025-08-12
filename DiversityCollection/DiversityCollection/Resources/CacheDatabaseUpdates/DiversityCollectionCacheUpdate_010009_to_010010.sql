
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.09'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   GazetteerSource   ##########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[GazetteerSource](
	[SourceView] [nvarchar](200) NOT NULL,
	[Source] [nvarchar](500) NULL,
	[SourceID] [int] NULL,
	[LinkedServerName] [nvarchar](500) NULL,
	[DatabaseName] [nvarchar](50) NULL,
 CONSTRAINT [PK_GazetteerSource] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'the name of the view retrieving the data from the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database where the data are taken from' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GazetteerSource', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO

GRANT SELECT ON GazetteerSource TO [CacheUser]
GO
GRANT DELETE ON GazetteerSource TO [CacheAdmin] 
GO
GRANT UPDATE ON GazetteerSource TO [CacheAdmin]
GO
GRANT INSERT ON GazetteerSource TO [CacheAdmin]
GO



--#####################################################################################################################
--######   Gazetteer   ################################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[Gazetteer](
	[BaseURL] [varchar](255) NOT NULL,
	[NameID] [int] NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[LanguageCode] [nvarchar](50) NULL,
	[PlaceID] int NOT NULL,
	[PlaceType] [nvarchar](50) NULL,
	[PreferredName] [nvarchar](400) NOT NULL,
	[PreferredNameID] [int] NOT NULL,
	[PreferredNameLanguageCode] [nvarchar](50) NULL,
	[ProjectID] [int] NOT NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[SourceView] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Gazetteer] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[NameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Gazetteer] ADD  CONSTRAINT [DF_Gazetteer_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gazetteer', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gazetteer', @level2type=N'COLUMN',@level2name=N'SourceView'
GO


GRANT SELECT ON Gazetteer TO [CacheUser]
GO
GRANT DELETE ON Gazetteer TO [CacheAdmin] 
GO
GRANT UPDATE ON Gazetteer TO [CacheAdmin]
GO
GRANT INSERT ON Gazetteer TO [CacheAdmin]
GO



--#####################################################################################################################
--######   procTransferGazetteer          #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [dbo].[procTransferGazetteer] 
@View nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @SQL nvarchar(4000)
	set @SQL = ''INSERT INTO Gazetteer
						  (BaseURL, NameID, Name, LanguageCode, PlaceID, PlaceType, PreferredName, PreferredNameID, PreferredNameLanguageCode, ProjectID, SourceView)
	SELECT     BaseURL, NameID, Name, LanguageCode, PlaceID, PlaceType, PreferredName, PreferredNameID, PreferredNameLanguageCode, ProjectID, '''''' + @View + ''''''
	FROM         '' + @View

	exec sp_executesql @SQL
	
	DECLARE @i int
	SET @i = (SELECT COUNT(*) FROM Gazetteer)
	SELECT CAST(@i AS VARCHAR) + '' geographic names imported''

END')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


GRANT EXEC ON [dbo].[procTransferGazetteer] TO [CacheAdmin]
GO



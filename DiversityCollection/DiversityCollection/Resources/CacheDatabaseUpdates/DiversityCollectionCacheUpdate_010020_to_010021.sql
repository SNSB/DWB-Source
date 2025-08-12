
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.20'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   TaxonNameExternalDatabase  #################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaxonNameExternalDatabase](
	[ExternalDatabaseID] [int] NOT NULL,
	[ExternalDatabaseName] [nvarchar](255) NULL,
	[ExternalDatabaseVersion] [nvarchar](255) NULL,
	[Rights] [nvarchar](500) NULL,
	[ExternalDatabaseAuthors] [nvarchar](200) NULL,
	[ExternalDatabaseURI] [nvarchar](300) NULL,
	[ExternalDatabaseInstitution] [nvarchar](300) NULL,
	[ExternalAttribute_NameID] [nvarchar](255) NULL,
	[SourceView] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TaxonNameExternalDatabase] PRIMARY KEY CLUSTERED 
(
	[ExternalDatabaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GRANT SELECT ON TaxonNameExternalDatabase TO [CacheUser]
GO
GRANT DELETE ON TaxonNameExternalDatabase TO [CacheAdmin] 
GO
GRANT UPDATE ON TaxonNameExternalDatabase TO [CacheAdmin]
GO
GRANT INSERT ON TaxonNameExternalDatabase TO [CacheAdmin]
GO


--#####################################################################################################################
--######   TaxonNameExternalID  #######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaxonNameExternalID](
	[NameID] [int] NOT NULL,
	[ExternalDatabaseID] [int] NOT NULL,
	[ExternalNameURI] [varchar](255) NULL,
	[SourceView] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TaxonNameExternalID] PRIMARY KEY CLUSTERED 
(
	[NameID] ASC,
	[ExternalDatabaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GRANT SELECT ON [TaxonNameExternalID] TO [CacheUser]
GO
GRANT DELETE ON [TaxonNameExternalID] TO [CacheAdmin] 
GO
GRANT UPDATE ON [TaxonNameExternalID] TO [CacheAdmin]
GO
GRANT INSERT ON [TaxonNameExternalID] TO [CacheAdmin]
GO


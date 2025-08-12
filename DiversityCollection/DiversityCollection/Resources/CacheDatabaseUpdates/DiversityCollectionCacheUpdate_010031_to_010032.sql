
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.31'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   TaxonCommonName Add columns ReferenceTitle, LogUpdatedWhen ################################################
--#####################################################################################################################

ALTER TABLE [dbo].[TaxonCommonName] DROP CONSTRAINT [PK_TaxonCommonName] WITH ( ONLINE = OFF )
GO

ALTER TABLE [dbo].[TaxonCommonName] ALTER COLUMN [CommonName] [nvarchar](220) NOT NULL;
GO

ALTER TABLE [dbo].[TaxonCommonName] ALTER COLUMN [BaseURL] [varchar](255) NOT NULL;
GO


ALTER TABLE [dbo].[TaxonCommonName] ADD [ReferenceTitle] [nvarchar](220) NULL;
GO

ALTER TABLE [dbo].[TaxonCommonName] ADD [LogUpdatedWhen] [smalldatetime] NOT NULL DEFAULT '1900-01-01 00:00:00';
GO

ALTER TABLE [dbo].[TaxonCommonName] ADD  CONSTRAINT [PK_TaxonCommonName] PRIMARY KEY CLUSTERED 
(
	[NameID] ASC,
	[BaseURL] ASC,
	[CommonName] ASC,
	[LanguageCode] ASC,
	[CountryCode] ASC,
	[LogUpdatedWhen] ASC
) ON [PRIMARY]
GO


--#####################################################################################################################
--######   ScientificTerm: Add column ExternalID. Issue #49  ##########################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ScientificTerm] ADD [ExternalID] [nvarchar](50) NULL;
GO

--#####################################################################################################################
--######   SourceTransfer: Adaption of size of column SourceView #49  #################################################
--#####################################################################################################################

/****** Object:  Index [PK_SourceTransfer]    Script Date: 08.04.2025 13:03:51 ******/
ALTER TABLE [dbo].[SourceTransfer] DROP CONSTRAINT [PK_SourceTransfer] WITH ( ONLINE = OFF )
GO

ALTER TABLE [dbo].[SourceTransfer] ALTER COLUMN [SourceView] nvarchar(390) NOT NULL
GO

/****** Object:  Index [PK_SourceTransfer]    Script Date: 08.04.2025 13:03:53 ******/
ALTER TABLE [dbo].[SourceTransfer] ADD  CONSTRAINT [PK_SourceTransfer] PRIMARY KEY CLUSTERED 
(
	[Source] ASC,
	[SourceView] ASC,
	[TransferDate] ASC
) ON [PRIMARY]
GO






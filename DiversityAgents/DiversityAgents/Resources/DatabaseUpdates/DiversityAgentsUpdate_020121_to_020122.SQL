
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.21'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  new table LanguageCode_Enum  ################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[LanguageCode_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[ParentCode] [nvarchar](50) NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_LanguageCode_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LanguageCode_Enum] ADD  CONSTRAINT [DF_LanguageCode_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT DELETE ON [dbo].[LanguageCode_Enum] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[LanguageCode_Enum] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[LanguageCode_Enum] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[LanguageCode_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO

INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayEnable]) VALUES (N'', N'', N'', 1)
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'af', N'Afrikaans', N'Afrikaans', NULL, 1, NULL, N'0x0036', N'dcf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ar', N'Arabisch', N'Arabisch', NULL, 1, NULL, N'0x0001', N'ddf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'az', N'Aserbaidschanisch', N'Aserbaidschanisch', NULL, 1, NULL, N'0x002C', N'def9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'be', N'Belarussisch', N'Belarussisch', NULL, 1, NULL, N'0x0023', N'dff9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'bg', N'Bulgarisch', N'Bulgarisch', NULL, 1, NULL, N'0x0002', N'e0f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ca', N'Katalanisch', N'Katalanisch', NULL, 1, NULL, N'0x0003', N'e1f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'cs', N'Tschechisch', N'Tschechisch', NULL, 1, NULL, N'0x0005', N'e2f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'da', N'Dänisch', N'Dänisch', NULL, 1, NULL, N'0x0006', N'e3f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'de', N'Deutsch', N'Deutsch', NULL, 1, NULL, N'0x0007', N'e4f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'el', N'Griechisch', N'Griechisch', NULL, 1, NULL, N'0x0008', N'e5f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'en', N'Englisch', N'Englisch', NULL, 1, NULL, N'0x0009', N'e6f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'es', N'Spanisch', N'Spanisch', NULL, 1, NULL, N'0x000A', N'e7f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'et', N'Estnisch', N'Estnisch', NULL, 1, NULL, N'0x0025', N'e8f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'eu', N'Baskisch', N'Baskisch', NULL, 1, NULL, N'0x002D', N'e9f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'fa', N'Farsi', N'Farsi', NULL, 1, NULL, N'0x0029', N'eaf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'fi', N'Finnisch', N'Finnisch', NULL, 1, NULL, N'0x000B', N'ebf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'fo', N'Färingisch', N'Färingisch', NULL, 1, NULL, N'0x0038', N'ecf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'fr', N'Französisch', N'Französisch', NULL, 1, NULL, N'0x000C', N'edf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'gl', N'Galizisch', N'Galizisch', NULL, 1, NULL, N'0x0056', N'eef9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'gu', N'Gujarati', N'Gujarati', NULL, 1, NULL, N'0x0047', N'eff9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'he', N'Hebräisch', N'Hebräisch', NULL, 1, NULL, N'0x000D', N'f0f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'hi', N'Hindi', N'Hindi', NULL, 1, NULL, N'0x0039', N'f1f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'hr', N'Kroatisch', N'Kroatisch', NULL, 1, NULL, N'0x001A', N'f2f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'hu', N'Ungarisch', N'Ungarisch', NULL, 1, NULL, N'0x000E', N'f3f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'hy', N'Armenisch', N'Armenisch', NULL, 1, NULL, N'0x002B', N'f4f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'id', N'Indonesisch', N'Indonesisch', NULL, 1, NULL, N'0x0021', N'f5f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'is', N'Isländisch', N'Isländisch', NULL, 1, NULL, N'0x000F', N'f6f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'it', N'Italienisch', N'Italienisch', NULL, 1, NULL, N'0x0010', N'f7f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ja', N'Japanisch', N'Japanisch', NULL, 1, NULL, N'0x0011', N'f8f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ka', N'Georgisch', N'Georgisch', NULL, 1, NULL, N'0x0037', N'f9f9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'kk', N'Kasachisch', N'Kasachisch', NULL, 1, NULL, N'0x003F', N'faf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'kn', N'Kannada', N'Kannada', NULL, 1, NULL, N'0x004B', N'fbf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ko', N'Koreanisch', N'Koreanisch', NULL, 1, NULL, N'0x0012', N'fcf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ky', N'Kirgisisch', N'Kirgisisch', NULL, 1, NULL, N'0x0040', N'fdf9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'lt', N'Litauisch', N'Litauisch', NULL, 1, NULL, N'0x0027', N'fef9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'lv', N'Lettisch', N'Lettisch', NULL, 1, NULL, N'0x0026', N'fff9403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'mk', N'Mazedonisch', N'Mazedonisch', NULL, 1, NULL, N'0x002F', N'00fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'mn', N'Mongolisch', N'Mongolisch', NULL, 1, NULL, N'0x0050', N'01fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'mr', N'Marathi', N'Marathi', NULL, 1, NULL, N'0x004E', N'02fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ms', N'Malaiisch', N'Malaiisch', NULL, 1, NULL, N'0x003E', N'03fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'nl', N'Niederländisch', N'Niederländisch', NULL, 1, NULL, N'0x0013', N'04fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'no', N'Norwegisch', N'Norwegisch', NULL, 1, NULL, N'0x0014', N'05fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'pa', N'Punjabi', N'Punjabi', NULL, 1, NULL, N'0x0046', N'06fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'pl', N'Polnisch', N'Polnisch', NULL, 1, NULL, N'0x0015', N'07fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'pt', N'Portugiesisch', N'Portugiesisch', NULL, 1, NULL, N'0x0016', N'08fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ro', N'Rumänisch', N'Rumänisch', NULL, 1, NULL, N'0x0018', N'09fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ru', N'Russisch', N'Russisch', NULL, 1, NULL, N'0x0019', N'0afa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'sa', N'Sanskrit', N'Sanskrit', NULL, 1, NULL, N'0x004F', N'0bfa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'sk', N'Slowakisch', N'Slowakisch', NULL, 1, NULL, N'0x001B', N'0cfa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'sl', N'Slowenisch', N'Slowenisch', NULL, 1, NULL, N'0x0024', N'0dfa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'sq', N'Albanisch', N'Albanisch', NULL, 1, NULL, N'0x001C', N'0efa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'sv', N'Schwedisch', N'Schwedisch', NULL, 1, NULL, N'0x001D', N'0ffa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'sw', N'Swahili', N'Swahili', NULL, 1, NULL, N'0x0041', N'10fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ta', N'Tamil', N'Tamil', NULL, 1, NULL, N'0x0049', N'11fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'te', N'Telugu', N'Telugu', NULL, 1, NULL, N'0x004A', N'12fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'th', N'Thai', N'Thai', NULL, 1, NULL, N'0x001E', N'13fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'tr', N'Türkisch', N'Türkisch', NULL, 1, NULL, N'0x001F', N'14fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'tt', N'Tatarisch', N'Tatarisch', NULL, 1, NULL, N'0x0044', N'15fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'uk', N'Ukrainisch', N'Ukrainisch', NULL, 1, NULL, N'0x0022', N'16fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'ur', N'Urdu', N'Urdu', NULL, 1, NULL, N'0x0020', N'17fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'uz', N'Usbekisch', N'Usbekisch', NULL, 1, NULL, N'0x0043', N'18fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [ParentCode], [InternalNotes], [RowGUID]) VALUES (N'vi', N'Vietnamesisch', N'Vietnamesisch', NULL, 1, NULL, N'0x002A', N'19fa403e-2f6d-de11-871c-001ec9d7ae4b')
GO
INSERT [dbo].[LanguageCode_Enum] ([Code], [Description], [DisplayText], [DisplayEnable]) VALUES (N'zh', N'Chinesisch', N'Chinesisch', 1)
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The codes for the languages',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'LanguageCode_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The codes for the languages',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'LanguageCode_Enum'
END CATCH;
GO



--#####################################################################################################################
--######  Agent - new column Language  ################################################################################
--#####################################################################################################################


ALTER TABLE Agent ADD ContentLanguage nvarchar(50) NULL;
GO

ALTER TABLE Agent_log ADD ContentLanguage nvarchar(50) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The language of the content' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ContentLanguage'
GO


ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [FK_Agent_LanguageCode_Enum] FOREIGN KEY([ContentLanguage])
REFERENCES [dbo].[LanguageCode_Enum] ([Code])
GO

ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_LanguageCode_Enum]
GO

--#####################################################################################################################
--######  Agent - trgDelAgent  ########################################################################################
--#####################################################################################################################
ALTER TRIGGER [dbo].[trgDelAgent] ON [dbo].[Agent] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.7 */ 
/*  Date: 01.02.2011  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Agent_Log (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, RevisionLevel, DataWithholdingReason, IgnoreButKeepForReference, ContentLanguage, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT D.AgentID, D.AgentParentID, D.AgentName, D.Version, D.AgentTitle, D.GivenName, D.GivenNamePostfix, D.InheritedNamePrefix, D.InheritedName, D.InheritedNamePostfix, D.Abbreviation, D.AgentType, D.AgentRole, D.AgentGender, D.Description, D.OriginalSpelling, D.Notes, D.ValidFromDate, D.ValidFromDay, D.ValidFromMonth, D.ValidFromYear, D.ValidUntilDate, D.ValidUntilDay, D.ValidUntilMonth, D.ValidUntilYear, D.ValidDateSupplement, D.PlaceOfBirth, D.PlaceOfDeath, D.SynonymToAgentID, D.SynonymisationType, D.RevisionLevel, D.DataWithholdingReason, D.IgnoreButKeepForReference, D.ContentLanguage, D.LogUpdatedBy, D.LogUpdatedWhen,  'D'
FROM DELETED D
GO


--#####################################################################################################################
--######  Agent - trgUpdAgent  ########################################################################################
--#####################################################################################################################
ALTER TRIGGER [dbo].[trgUpdAgent] ON [dbo].[Agent] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.7 */ 
/*  Date: 01.02.2011  */ 

if not update(Version) 
BEGIN

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
END 


/* saving the original dataset in the logging table */ 
INSERT INTO Agent_Log (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, RevisionLevel, DataWithholdingReason, IgnoreButKeepForReference, ContentLanguage, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT D.AgentID, D.AgentParentID, D.AgentName, D.Version, D.AgentTitle, D.GivenName, D.GivenNamePostfix, D.InheritedNamePrefix, D.InheritedName, D.InheritedNamePostfix, D.Abbreviation, D.AgentType, D.AgentRole, D.AgentGender, D.Description, D.OriginalSpelling, D.Notes, D.ValidFromDate, D.ValidFromDay, D.ValidFromMonth, D.ValidFromYear, D.ValidUntilDate, D.ValidUntilDay, D.ValidUntilMonth, D.ValidUntilYear, D.ValidDateSupplement, D.PlaceOfBirth, D.PlaceOfDeath, D.SynonymToAgentID, D.SynonymisationType, D.RevisionLevel, D.DataWithholdingReason, D.IgnoreButKeepForReference, D.ContentLanguage, D.LogUpdatedBy, D.LogUpdatedWhen,  'U'
FROM DELETED D

END

/* updating the logging columns */
Update Agent
set Agent.AgentName = 
case when i.InheritedNamePrefix is null or i.InheritedNamePrefix = '' then '' else i.InheritedNamePrefix + ' ' end +
case when i.Inheritedname is null then '' else i.Inheritedname end + 
case when i.GivenName is null or i.GivenName = '' then '' else 
	case when i.Inheritedname is null or i.Inheritedname = '' then '' else ', ' end + i.GivenName end +
case when i.GivenNamePostfix is null or i.GivenNamePostfix = '' then '' else ' ' + i.GivenNamePostfix end +
case when i.InheritedNamePostfix is null or i.InheritedNamePostfix = '' then '' else ', ' + i.InheritedNamePostfix end +
case when i.AgentTitle is null then '' else ', ' + i.AgentTitle end,
LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM Agent, inserted i 
where 1 = 1 
AND Agent.AgentID = i.AgentID
GO


--#####################################################################################################################
--######  procInsertAgentCopy - new column ContentLanguage  ###########################################################
--#####################################################################################################################

ALTER PROCEDURE [dbo].[procInsertAgentCopy] 
	(@AgentID int output,
	 @CopyID int,
	 @AgentName nvarchar  (200))
AS

-- Agent
INSERT INTO Agent
	(AgentParentID, AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType,
	AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, 
	ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, RevisionLevel, 
	DataWithholdingReason, ContentLanguage)
SELECT     AgentParentID, @AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, 
	AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, 
	ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, 'unchecked' AS RevisionLevel, 
	DataWithholdingReason, ContentLanguage
FROM         Agent
WHERE     (AgentID = @CopyID)

SELECT @AgentID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

-- AgentContactInformation
INSERT INTO AgentContactInformation (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, 
                      ValidFrom, ValidUntil)
SELECT @AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, 
                      ValidFrom, ValidUntil
FROM         AgentContactInformation 
WHERE AgentID = @CopyID

-- AgentProject
INSERT INTO AgentProject
                      (AgentID, ProjectID)
SELECT     @AgentID, ProjectID
FROM         AgentProject
WHERE AgentID = @CopyID

-- AgentReference
INSERT INTO AgentReference
                      (AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, ContainsImage, ContainsReferencelist, Notes)
SELECT     @AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, ContainsImage, ContainsReferencelist, Notes
FROM         AgentReference
WHERE AgentID = @CopyID

-- AgentKeyword
INSERT INTO AgentKeyword
                      (AgentID, Keyword)
SELECT     @AgentID, Keyword
FROM         AgentKeyword
WHERE AgentID = @CopyID

-- AgentImage
INSERT INTO AgentImage
                      (AgentID, URI)
SELECT     @AgentID, URI
FROM         AgentImage
WHERE AgentID = @CopyID

--AgentExternalID
INSERT INTO AgentExternalID
                      (AgentID, ExternalDatabaseID, ExternalAgentURI)
SELECT     @AgentID, ExternalDatabaseID, ExternalAgentURI
FROM         AgentExternalID
WHERE AgentID = @CopyID

-- AgentRelation
INSERT INTO AgentRelation
                      (AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason)
SELECT     @AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason
FROM         AgentRelation
WHERE AgentID = @CopyID

SELECT @AgentID

GO



--#####################################################################################################################
--######  AgentChildNodes - new column ContentLanguage  ###############################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[ContentLanguage] [nvarchar](50) NULL)

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 06.08.2022
SELECT * FROM [dbo].[AgentChildNodes] (41781)
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL	)

	INSERT @TempItem (AgentID, AgentParentID) 
	SELECT AgentID, AgentParentID
	FROM Agent WHERE AgentParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, Agent C where C.AgentParentID = T.AgentID and C.AgentID not in (select AgentID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (AgentID, AgentParentID)
		select C.AgentID, C.AgentParentID
		from @TempItem T, Agent C where C.AgentParentID = T.AgentID and (C.IgnoreButKeepForReference is null or c.IgnoreButKeepForReference = '') and C.AgentID not in (select AgentID from @TempItem)
		set @i = (select count(*) from @TempItem T, Agent C where C.AgentParentID = T.AgentID and C.AgentID not in (select AgentID from @TempItem))
	end

 INSERT @ItemList (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, ContentLanguage) 
   SELECT distinct T.AgentID, T.AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, ContentLanguage
   FROM @TempItem T INNER JOIN Agent A ON T.AgentID = A.AgentID ORDER BY AgentName
   RETURN
END

GO



--#####################################################################################################################
--######   AgentHierarchy - new column ContentLanguage   ##############################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentHierarchy] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[ContentLanguage] [nvarchar](50) NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 08.08.2022
SELECT * FROM [dbo].[AgentHierarchy] (41781)
SELECT * FROM [dbo].[AgentHierarchy] (2821)
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.AgentTopID(@AgentID) )

-- get the ID's of the child nodes
DECLARE @TempItem TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL)

	INSERT @TempItem (AgentID, AgentParentID) 
	SELECT AgentID, AgentParentID FROM dbo.AgentChildNodes (@TopID)

-- copy the root node in the result list
INSERT @TempItem
   SELECT DISTINCT AgentID, AgentParentID
   FROM Agent A
   WHERE A.AgentID = @TopID AND A.AgentID NOT IN (SELECT AgentID FROM @TempItem)

-- copy the content into the result list
INSERT @AgentList
   SELECT DISTINCT A.AgentID, A.AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, ContentLanguage
   FROM Agent A INNER JOIN @TempItem T ON A.AgentID = T.AgentID
   ORDER BY AgentName

   RETURN
END

GO



--#####################################################################################################################
--######   AgentSynonymTopID - ensure result despite of loops   #######################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentSynonymTopID] (@AgentID int)
RETURNS int
/*
Returns the top ID within the synyonym hierarchy for a given ID from the table Agent.
MW 22.08.2022
*/
AS
BEGIN
declare @TopID int
declare @i int
declare @AgentIDs TABLE ([AgentID] [int] primary key)
declare @StartID int
set @StartID = @AgentID

set @TopID = (select SynonymToAgentID from Agent where AgentID = @AgentID) 

set @i = (select count(*) from Agent where AgentID = @AgentID)

if (@TopID is null )
	set @TopID =  @AgentID
else	
	begin
	while (@i > 0)
		begin
		set @AgentID = (select SynonymToAgentID from Agent A where AgentID = @AgentID and not SynonymToAgentID is null and (A.IgnoreButKeepForReference is null or A.IgnoreButKeepForReference = '')) 
		set @i = (select count(*) from Agent A where AgentID = @AgentID and not SynonymToAgentID is null and (A.IgnoreButKeepForReference is null or A.IgnoreButKeepForReference = ''))
		if @StartID = @AgentID set @i = 0
		if (select count(*) from @AgentIDs A where A.AgentID = @AgentID) > 0
			begin set @i = 0 end
		else
			begin insert into @AgentIDs(AgentID) select @AgentID end
		end
	set @TopID = @AgentID
	end
return @TopID
END

GO

--#####################################################################################################################
--######   AgentSynonymyChildNodes - Optimized, ensure result despite of loops and new column ContentLanguage   #######
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentSynonymyChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[ContentLanguage] [nvarchar](50) NULL)

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 22.08.2022
*/
AS
BEGIN
   declare @SynonymToAgentID int
   RETURN
END


GO

ALTER FUNCTION [dbo].[AgentSynonymyChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[ContentLanguage] [nvarchar](50) NULL)

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 22.08.2022
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
[AgentID] [int] primary key,
	[SynonymToAgentID] [int] NULL	)

	INSERT @TempItem (AgentID, SynonymToAgentID) 
	SELECT AgentID, SynonymToAgentID
	FROM Agent WHERE SynonymToAgentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, Agent C where C.SynonymToAgentID = T.AgentID and C.AgentID not in (select AgentID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (AgentID, SynonymToAgentID)
		select C.AgentID, C.SynonymToAgentID
		from @TempItem T, Agent C where C.SynonymToAgentID = T.AgentID and (C.IgnoreButKeepForReference is null or c.IgnoreButKeepForReference = '') and C.AgentID not in (select AgentID from @TempItem)
		set @i = (select count(*) from @TempItem T, Agent C where C.SynonymToAgentID = T.AgentID and C.AgentID not in (select AgentID from @TempItem))
	end

 INSERT @ItemList (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, ContentLanguage) 
   SELECT distinct T.AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	T.SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, ContentLanguage
   FROM @TempItem T INNER JOIN Agent A ON T.AgentID = A.AgentID ORDER BY AgentName
   RETURN
END

GO


--#####################################################################################################################
--######   AgentSynonymy - ensure result despite of loops and new column ContentLanguage   ############################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentSynonymy] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[ContentLanguage] [nvarchar](50) NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 22.08.2022
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.[AgentSynonymTopID](@AgentID) )

-- get the ID's of the child nodes
DECLARE @TempItem TABLE (
    [AgentID] [int] primary key,
	[SynonymToAgentID] [int] NULL)

	INSERT @TempItem (AgentID, SynonymToAgentID) 
	SELECT AgentID, SynonymToAgentID FROM dbo.AgentSynonymyChildNodes (@TopID)

-- copy the root node in the result list
INSERT @TempItem
   SELECT DISTINCT AgentID, SynonymToAgentID
   FROM Agent A
   WHERE A.AgentID = @TopID AND A.AgentID NOT IN (SELECT AgentID FROM @TempItem)

-- copy the content into the result list
INSERT @AgentList
   SELECT DISTINCT A.AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	A.SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, ContentLanguage
   FROM Agent A INNER JOIN @TempItem T ON A.AgentID = T.AgentID
   ORDER BY AgentName

   RETURN
END

GO


--#####################################################################################################################
--######   AgentSuperiorList  - new column ContentLanguage  ###########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentSuperiorList] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[ContentLanguage] [nvarchar](50) NULL,
	DisplayOrder int NOT NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 22.08.2022
Test:
select * from dbo.AgentSuperiorList(12665)
*/
AS
BEGIN
	declare @i int
	set @i = 1
	while (not @AgentID is null)
		begin

			INSERT @AgentList
			SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
			ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
			SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, ContentLanguage, @i
			FROM Agent
			WHERE Agent.AgentID = @AgentID

			set @AgentID = (select AgentParentID from Agent where AgentID = @AgentID)
			if(select count(*) from @AgentList where AgentID = @AgentID) > 0 set @AgentID = null;

			set @i = (select @i + 1)

		end

   RETURN
END

GO




--#####################################################################################################################
--######  AgentTopID: ensure result despite of loops  #################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentTopID] (@AgentID int)
RETURNS int
/*
Returns the top ID within the hierarchy for a given ID from the table Agent.
MW 08.08.2022
SELECT [dbo].[AgentTopID] (41781)
*/
AS
BEGIN
declare @TopID int
declare @i int
declare @StartID int
declare @AgentIDs TABLE ([AgentID] [int] primary key)

set @StartID = @AgentID

set @TopID = (select AgentParentID from Agent where AgentID = @AgentID) 

set @i = (select count(*) from Agent where AgentID = @AgentID)

if (@TopID is null )
	set @TopID =  @AgentID
else	
	begin
	while (@i > 0)
		begin
		set @AgentID = (select AgentParentID from Agent A where AgentID = @AgentID and (A.IgnoreButKeepForReference is null or A.IgnoreButKeepForReference = '') and not AgentParentID is null) 
		set @i = (select count(*) from Agent where AgentID = @AgentID and not AgentParentID is null)
		if @StartID = @AgentID set @i = 0
		if (select count(*) from @AgentIDs A where A.AgentID = @AgentID) > 0
			begin set @i = 0 end
		else
			begin insert into @AgentIDs(AgentID) select @AgentID end
		end
	set @TopID = @AgentID
	end
return @TopID
END

GO


--#####################################################################################################################
--######  AgentSuperiorList: ensure result despite of loops and not IgnoreButKeepForReference  ########################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentSuperiorList] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[ContentLanguage] [nvarchar](50) NULL,
	DisplayOrder int NOT NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 22.08.2022
Test:
select * from dbo.AgentSuperiorList(12665)
*/
AS
BEGIN
	declare @i int
	set @i = 1
	while (not @AgentID is null)
		begin

			INSERT @AgentList
			SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
			ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
			SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, ContentLanguage, @i
			FROM Agent
			WHERE Agent.AgentID = @AgentID

			set @AgentID = (select AgentParentID from Agent a where AgentID = @AgentID and (a.IgnoreButKeepForReference = 0 OR a.IgnoreButKeepForReference IS NULL))
			if(select count(*) from @AgentList where AgentID = @AgentID) > 0 set @AgentID = null;

			set @i = (select @i + 1)

		end

   RETURN
END
GO


--#####################################################################################################################
--######  AgentHierarchyAllSuperior: ensure result despite of loops  ##################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentHierarchyAllSuperior] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] NOT NULL,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL)

/*
Returns a table that lists all the agents superior to the given agent, including all hierarchy data.
MW 08.08.2022
SELECT * FROM dbo.AgentHierarchyAllSuperior(37687)
SELECT * FROM dbo.AgentHierarchyAllSuperior(1)
SELECT * FROM dbo.AgentHierarchyAllSuperior(4)
SELECT * FROM dbo.AgentHierarchyAllSuperior(5)
SELECT * FROM dbo.AgentHierarchyAllSuperior(8)
SELECT AgentID, AgentParentID, AgentName FROM dbo.AgentHierarchyAllSuperior(41782)
*/
AS
BEGIN

DECLARE @Top TABLE (
    [AgentID] [int] NOT NULL,
	[AgentParentID] [int] NULL)
	
DECLARE @TempItem TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL);

DECLARE @All TABLE (
    [AgentID] [int] NOT NULL,
	[AgentParentID] [int] NULL);

declare @Check TABLE (
    [AgentID] [int] NOT NULL);

-- getting the TopIDs
INSERT @TOP(AgentID, AgentParentID)
SELECT A.AgentID, A.AgentParentID
FROM Agent A 
WHERE (A.IgnoreButKeepForReference = 0 OR A.IgnoreButKeepForReference IS NULL)
AND A.SynonymToAgentID IS NULL
AND A.AgentID = @AgentID
UNION
SELECT  A.AgentID, S.AgentParentID
FROM Agent AS A INNER JOIN
Agent AS S ON A.AgentID = S.SynonymToAgentID
WHERE (A.IgnoreButKeepForReference = 0 OR A.IgnoreButKeepForReference IS NULL) 
AND (S.IgnoreButKeepForReference = 0 OR S.IgnoreButKeepForReference IS NULL)
AND S.AgentID = @AgentID
UNION
SELECT H.AgentID, H.AgentParentID
FROM AgentHierarchyOther AS H INNER JOIN
Agent A ON H.AgentID = A.AgentID
WHERE (A.IgnoreButKeepForReference = 0 OR A.IgnoreButKeepForReference IS NULL)
AND H.AgentID = @AgentID

INSERT @TOP(AgentID)
select AgentParentID from @Top t where t.AgentParentID is not null

declare @ID int
set @ID = (select Min(AgentID) from @TOP)

while (select count(*) from @TOP t where t.AgentID not in (select AgentID from @Check)) > 0
begin
insert @All (AgentID, AgentParentID) select AgentID, AgentParentID from [dbo].[AgentSuperiorList](@ID)

--delete s from @TOP s where s.AgentID = @ID
insert @Check (AgentID) select AgentID from [dbo].[AgentSuperiorList](@ID) s
set @ID = (select Min(AgentID) from @TOP t where t.AgentID not in (select AgentID from @Check))
end

-- copy the child nodes into the result list
INSERT @AgentList
SELECT DISTINCT AA.AgentID, AA.AgentParentID, A.AgentName
FROM Agent A, @All AA
WHERE A.AgentID = AA.AgentID

   RETURN
END

GO




--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '04.01.00' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.22'
END

GO

select [dbo].[Version] ()  


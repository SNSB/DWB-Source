
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.17'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Language_Enum including content   ##########################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[Language_Enum](
	[DisplayOrder] [int] NOT NULL,
	[Code] [varchar](5) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Language_Enum] PRIMARY KEY CLUSTERED 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (0, N'--', N'not language specific/not applicable')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (1, N'?', N'unknown (not checked)')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (2, N'en', N'English')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (3, N'de', N'German')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (4, N'es', N'Spanish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (5, N'fr', N'French')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (6, N'ru', N'Russian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (7, N'ja', N'Japanese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (8, N'zh', N'Chinese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (9, N'pt', N'Portuguese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (10, N'it', N'Italian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (11, N'la', N'Latin')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (12, N'ar', N'Arabic')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (13, N'aa', N'Afar')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (14, N'ab', N'Abkhazian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (15, N'af', N'Afrikaans')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (16, N'am', N'Amharic')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (17, N'as', N'Assamese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (18, N'ay', N'Aymara')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (19, N'az', N'Azerbaijani')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (20, N'ba', N'Bashkir')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (21, N'be', N'Byelorussian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (22, N'bg', N'Bulgarian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (23, N'bh', N'Bihari')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (24, N'bi', N'Bislama')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (25, N'bn', N'Bengali')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (26, N'bo', N'Tibetan')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (27, N'br', N'Breton')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (28, N'ca', N'Catalan')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (29, N'co', N'Corsican')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (30, N'cs', N'Czech')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (31, N'cy', N'Welsh')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (32, N'da', N'Danish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (33, N'dz', N'Bhutani')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (34, N'el', N'Greek')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (35, N'eo', N'Esperanto')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (36, N'et', N'Estonian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (37, N'eu', N'Basque')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (38, N'fa', N'Persian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (39, N'fi', N'Finnish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (40, N'fj', N'Fiji')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (41, N'fo', N'Faeroese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (42, N'fy', N'Frisian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (43, N'ga', N'Irish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (44, N'gd', N'Scots')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (45, N'gl', N'Galician')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (46, N'gn', N'Guarani')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (47, N'gu', N'Gujarati')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (48, N'ha', N'Hausa')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (49, N'hi', N'Hindi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (50, N'hr', N'Croatian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (51, N'hu', N'Hungarian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (52, N'hy', N'Armenian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (53, N'ia', N'Interlingua')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (54, N'ie', N'Interlingue')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (55, N'ik', N'Inupiak')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (56, N'in', N'Indonesian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (57, N'is', N'Icelandic')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (58, N'iw', N'Hebrew')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (59, N'ji', N'Yiddish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (60, N'jw', N'Javanese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (61, N'ka', N'Georgian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (62, N'kk', N'Kazakh')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (63, N'kl', N'Greenlandic')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (64, N'km', N'Cambodian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (65, N'kn', N'Kannada')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (66, N'ko', N'Korean')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (67, N'ks', N'Kashmiri')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (68, N'ku', N'Kurdish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (69, N'ky', N'Kirghiz')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (70, N'ln', N'Lingala')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (71, N'lo', N'Laothian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (72, N'lt', N'Lithuanian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (73, N'lv', N'Latvian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (74, N'mg', N'Malagasy')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (75, N'mi', N'Maori')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (76, N'mk', N'Macedonian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (77, N'ml', N'Malayalam')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (78, N'mn', N'Mongolian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (79, N'mo', N'Moldavian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (80, N'mr', N'Marathi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (81, N'ms', N'Malay')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (82, N'mt', N'Maltese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (83, N'my', N'Burmese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (84, N'na', N'Nauru')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (85, N'ne', N'Nepali')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (86, N'nl', N'Dutch')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (87, N'no', N'Norwegian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (88, N'oc', N'Occitan')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (89, N'or', N'Oriya')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (90, N'pa', N'Punjabi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (91, N'pl', N'Polish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (92, N'ps', N'Pashto')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (93, N'qu', N'Quechua')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (94, N'rm', N'Rhaeto-Romance')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (95, N'rn', N'Kirundi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (96, N'ro', N'Romanian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (97, N'rw', N'Kinyarwanda')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (98, N'sa', N'Sanskrit')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (99, N'sd', N'Sindhi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (100, N'sg', N'Sangro')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (101, N'sh', N'Serbo-Croatian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (102, N'si', N'Singhalese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (103, N'sk', N'Slovak')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (104, N'sl', N'Slovenian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (105, N'sm', N'Samoan')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (106, N'sn', N'Shona')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (107, N'so', N'Somali')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (108, N'sq', N'Albanian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (109, N'sr', N'Serbian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (110, N'ss', N'Siswati')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (111, N'st', N'Sesotho')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (112, N'su', N'Sundanese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (113, N'sv', N'Swedish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (114, N'sw', N'Swahili')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (115, N'ta', N'Tamil')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (116, N'te', N'Tegulu')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (117, N'tg', N'Tajik')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (118, N'th', N'Thai')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (119, N'ti', N'Tigrinya')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (120, N'tk', N'Turkmen')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (121, N'tl', N'Tagalog')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (122, N'tn', N'Setswana')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (123, N'to', N'Tonga')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (124, N'tr', N'Turkish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (125, N'ts', N'Tsonga')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (126, N'tt', N'Tatar')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (127, N'tw', N'Twi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (128, N'uk', N'Ukrainian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (129, N'ur', N'Urdu')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (130, N'uz', N'Uzbek')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (131, N'vi', N'Vietnamese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (132, N'vo', N'Volapuk')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (133, N'wo', N'Wolof')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (134, N'xh', N'Xhosa')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (135, N'yo', N'Yoruba')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (136, N'zu', N'Zulu')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (1000, N'(?)!', N'unknown (checked but still unknown)')
GO

GRANT SELECT ON [dbo].[Language_Enum] TO [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   ProjectDescriptorElement   #################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectDescriptorElement](
	[ElementID] [int] NOT NULL,
	[ParentElementID] [int] NULL,
	[Abbreviation] [nvarchar](25) NOT NULL CONSTRAINT [DF__ProjectDe__Eleme__2A563856]  DEFAULT (''),
	[DisplayText] [nvarchar](80) NOT NULL CONSTRAINT [DF__ProjectDe__Eleme__2B4A5C8F]  DEFAULT (''),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF__ProjectDe__Displ__2C3E80C8]  DEFAULT ((0)),
	[Description] [nvarchar](1000) NOT NULL CONSTRAINT [DF__ProjectDe__Eleme__2D32A501]  DEFAULT (''),
	[URL] [varchar](500) NOT NULL CONSTRAINT [DF__ProjectDe__Eleme__2E26C93A]  DEFAULT (''),
	[InternalNotes] [nvarchar](1000) NOT NULL CONSTRAINT [DF__ProjectDe__Inter__2F1AED73]  DEFAULT (''),
	[DisplayEnabled] [bit] NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL CONSTRAINT [DF_ProjectDescriptorElement_LogInsertedBy]  DEFAULT (suser_sname()),
	[LogInsertedWhen] [smalldatetime] NULL CONSTRAINT [DF_ProjectDescriptorElement_LogInsertedWhen]  DEFAULT (getdate()),
	[LogUpdatedBy] [nvarchar](50) NULL CONSTRAINT [DF_ProjectDescriptorElement_LogUpdatedBy]  DEFAULT (suser_sname()),
	[LogUpdatedWhen] [smalldatetime] NULL CONSTRAINT [DF_ProjectDescriptorElement_LogUpdatedWhen]  DEFAULT (getdate()),
	[RowGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProjectDescriptorElement_RowGUID]  DEFAULT (newsequentialid()),
 CONSTRAINT [ProjectDescriptorElement_PK] PRIMARY KEY CLUSTERED 
(
	[ElementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numeric identifier (primary key).' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'ElementID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the parent element' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'ParentElementID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated name for descriptor element.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'Abbreviation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concise English label of a descriptor element definition.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order in which elements are displayed independently of a resource collection (for order within a collection see ProjectDescriptorAssociation.DisplayOrder).' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A free-form text that may be displayed in user interfaces as explanatory text.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of a conceptual ontological resource considered equivalent with this descriptor element.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'URL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes and remarks. Although normally not published in public reports, this should not be used for truly confidential information.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Whether this DescriptorElement is to be displayed in the user interface.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'DisplayEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Examples of descriptor elements (= concepts for variables) are keyword, taxon name, pathogen name, host name, or host feature. Association with ResourceCollections is defined in ProjectDescriptorAssociation, relations in Res.DescriptorElementRelation.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement'
GO

GRANT SELECT ON [dbo].[ProjectDescriptorElement] TO [DiversityWorkbenchUser]
GRANT INSERT ON [dbo].[ProjectDescriptorElement] TO [DiversityWorkbenchAdministrator]
GRANT UPDATE ON [dbo].[ProjectDescriptorElement] TO [DiversityWorkbenchAdministrator]
GRANT DELETE ON [dbo].[ProjectDescriptorElement] TO [DiversityWorkbenchAdministrator]
GO

INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled]) VALUES (0, N'Any', N'Any concept (unspecified)', 0, N'The most general base concept of all other concepts', N'', N'The ID=0 may be used in client software and should not be changed. All other IDs may be freely changed.', 1)
GO
INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled]) VALUES (1, N'Keyword', N'Keyword', 200, N'Keywords are essentially unspecified concepts and may be used so in imported data. For new data entry, keywords should be used if no more appropriate concept is present.', N'', N'', 1)
GO
INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled]) VALUES (2, N'Label', N'Label', 240, N'', N'', N'', 1)
GO

--#####################################################################################################################
--######   ProjectDescriptorElement_log  ##############################################################################
--#####################################################################################################################
CREATE TABLE [dbo].[ProjectDescriptorElement_log] 
([ElementID] [int] NULL,
 [ParentElementID] [int] NULL,
 [Abbreviation] [nvarchar] (25) COLLATE Latin1_General_CI_AS NULL,
 [DisplayText] [nvarchar] (80) COLLATE Latin1_General_CI_AS NULL,
 [DisplayOrder] [int] NULL,
 [Description] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL,
 [URL] [varchar] (500) COLLATE Latin1_General_CI_AS NULL,
 [InternalNotes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL,
 [DisplayEnabled] [bit] NULL,
 [LogInsertedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogInsertedWhen] [smalldatetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [smalldatetime] NULL,
 [RowGUID] [uniqueidentifier] NULL,
 [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ProjectDescriptorElement_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_ProjectDescriptorElement_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ProjectDescriptorElement_Log_LogUser]  DEFAULT (user_name()), 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_ProjectDescriptorElement_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
GO

GRANT INSERT ON dbo.[ProjectDescriptorElement_log] TO [DiversityWorkbenchEditor];
GRANT SELECT ON dbo.[ProjectDescriptorElement_log] TO [DiversityWorkbenchEditor];
GO

--#####################################################################################################################
--######   trgUpdProjectDescriptorElement   ###########################################################################
--#####################################################################################################################
CREATE TRIGGER [trgUpdProjectDescriptorElement] ON [dbo].[ProjectDescriptorElement] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 30.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptorElement_Log (ElementID, ParentElementID, Abbreviation, DisplayText, DisplayOrder, Description, URL, InternalNotes, DisplayEnabled, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ElementID, deleted.ParentElementID, deleted.Abbreviation, deleted.DisplayText, deleted.DisplayOrder, deleted.Description, deleted.URL, deleted.InternalNotes, deleted.DisplayEnabled, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  LogUpdatedBy = SUSER_NAME()
FROM ProjectDescriptorElement T, deleted D 
where 1 = 1 
AND T.ElementID = D.ElementID
GO

--#####################################################################################################################
--######   trgDelProjectDescriptor  ###################################################################################
--#####################################################################################################################
CREATE TRIGGER [trgDelProjectDescriptorElement] ON [dbo].[ProjectDescriptorElement] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 30.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptorElement_Log (ElementID, ParentElementID, Abbreviation, DisplayText, DisplayOrder, Description, URL, InternalNotes, DisplayEnabled, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ElementID, deleted.ParentElementID, deleted.Abbreviation, deleted.DisplayText, deleted.DisplayOrder, deleted.Description, deleted.URL, deleted.InternalNotes, deleted.DisplayEnabled, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO

--#####################################################################################################################
--######   ProjectDescriptor  #########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectDescriptor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Language] [varchar](5) NOT NULL,
	[ElementID] [int] NOT NULL,
	[Content] [nvarchar](255) NOT NULL,
	[ContentURI] [varchar](500) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [ProjectDescriptor_PK] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ProjectDescriptor] ADD  DEFAULT ('en') FOR [Language]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  DEFAULT ((0)) FOR [ElementID]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  DEFAULT ('') FOR [Content]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  DEFAULT ('') FOR [ContentURI]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[ProjectDescriptor]  WITH CHECK ADD  CONSTRAINT [FK_ProjectDescriptor_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
GO
ALTER TABLE [dbo].[ProjectDescriptor] CHECK CONSTRAINT [FK_ProjectDescriptor_Project]
GO
ALTER TABLE [dbo].[ProjectDescriptor]  WITH CHECK ADD  CONSTRAINT [FK_ProjectDescriptor_ProjectDescriptorElement] FOREIGN KEY([ElementID])
REFERENCES [dbo].[ProjectDescriptorElement] ([ElementID])
GO
ALTER TABLE [dbo].[ProjectDescriptor] CHECK CONSTRAINT [FK_ProjectDescriptor_ProjectDescriptorElement]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal system generated primary key. Note that multiple values for a descriptor concept may be added (e.g. keywords)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project, foreign key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Language in which element content is expressed. Necessary even for numeric or date content (because expressed through string using language-specific conventions)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'Language'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of a descriptor element concept (foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'ElementID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A name, state, or value text for the descriptor element.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of a conceptual ontological resource considered equivalent with the content, especially URIs for taxon names or keywords from ontologies.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'ContentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Object names, event names, keywords, etc., providing indexing information for a resource.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor'
GO

GRANT SELECT ON [dbo].[ProjectDescriptor] TO [DiversityWorkbenchUser]
GRANT INSERT ON [dbo].[ProjectDescriptor] TO [DiversityWorkbenchEditor]
GRANT UPDATE ON [dbo].[ProjectDescriptor] TO [DiversityWorkbenchEditor]
GRANT DELETE ON [dbo].[ProjectDescriptor] TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######   ProjectDescriptor_log   ####################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectDescriptor_log] 
([ID] [int] NULL,
 [ProjectID] [int] NULL,
 [Language] [varchar] (5) COLLATE Latin1_General_CI_AS NULL,
 [ElementID] [int] NULL,
 [Content] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
 [ContentURI] [varchar] (500) COLLATE Latin1_General_CI_AS NULL,
 [LogInsertedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogInsertedWhen] [smalldatetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [smalldatetime] NULL,
 [RowGUID] [uniqueidentifier] NULL,
  [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ProjectDescriptor_Log_LogState]  DEFAULT ('U'), 
 [LogDate] [datetime] NOT NULL CONSTRAINT [DF_ProjectDescriptor_Log_LogDate]  DEFAULT (getdate()), 
 [LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ProjectDescriptor_Log_LogUser]  DEFAULT (user_name()), 
 [LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_ProjectDescriptor_Log] PRIMARY KEY CLUSTERED 
 ([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 
GO

GRANT INSERT ON dbo.[ProjectDescriptor_log] TO [DiversityWorkbenchEditor];
GRANT SELECT ON dbo.[ProjectDescriptor_log] TO [DiversityWorkbenchEditor];
GO

--#####################################################################################################################
--######   trgUpdProjectDescriptor   ##################################################################################
--#####################################################################################################################

CREATE TRIGGER [trgUpdProjectDescriptor] ON [dbo].[ProjectDescriptor] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 27.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptor_Log (ID, ProjectID, Language, ElementID, Content, ContentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ID, deleted.ProjectID, deleted.Language, deleted.ElementID, deleted.Content, deleted.ContentURI, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  LogUpdatedBy = SUSER_NAME()
FROM ProjectDescriptor T, deleted D 
where 1 = 1 
AND T.ID = D.ID
GO

--#####################################################################################################################
--######   trgDelProjectDescriptor  ###################################################################################
--#####################################################################################################################

CREATE TRIGGER [trgDelProjectDescriptor] ON [dbo].[ProjectDescriptor] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 27.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptor_Log (ID, ProjectID, Language, ElementID, Content, ContentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ID, deleted.ProjectID, deleted.Language, deleted.ElementID, deleted.Content, deleted.ContentURI, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO

--#####################################################################################################################
--######   Transfer data from ProjectKeyword to ProjectDescriptor  ####################################################
--#####################################################################################################################
IF (SELECT COUNT(*) FROM [ProjectDescriptor] D, [ProjectKeyword] K 
	WHERE D.ProjectID = K.ProjectID AND D.Content = K.Keyword AND D.ElementID = 1) = 0
BEGIN
  INSERT INTO [dbo].[ProjectDescriptor]
           ([ProjectID]
           ,[ElementID]
           ,[Content]
           ,[LogInsertedBy]
           ,[LogInsertedWhen]
           ,[LogUpdatedBy]
           ,[LogUpdatedWhen]
           ,[RowGUID])
  SELECT [ProjectID]
      ,1
	  ,[Keyword]
      ,[LogInsertedBy]
      ,[LogInsertedWhen]
      ,[LogUpdatedBy]
      ,[LogUpdatedWhen]
      ,[RowGUID]
  FROM [dbo].[ProjectKeyword]
END
GO


--#####################################################################################################################
--######   Transfer data from ProjectLabel to ProjectDescriptor  ######################################################
--#####################################################################################################################
IF (SELECT COUNT(*) FROM [ProjectDescriptor] D, [ProjectLabel] L
	WHERE D.ProjectID = L.ProjectID AND D.Content = L.Label AND D.ElementID = 2) = 0
BEGIN
  INSERT INTO [dbo].[ProjectDescriptor]
           ([ProjectID]
           ,[ElementID]
           ,[Content]
           ,[LogInsertedBy]
           ,[LogInsertedWhen]
           ,[LogUpdatedBy]
           ,[LogUpdatedWhen]
           ,[RowGUID])
  SELECT [ProjectID]
      ,2
	  ,[Label]
      ,[LogInsertedBy]
      ,[LogInsertedWhen]
      ,[LogUpdatedBy]
      ,[LogUpdatedWhen]
      ,[RowGUID]
  FROM [dbo].[ProjectLabel]
END
GO


--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.03.00' END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.18'
END

GO


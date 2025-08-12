
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.03'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--#####################################################################################################################
--######   Adding new tables for Taxonomic groups etc.   ###################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   TaxonSynonymySource   #########################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'TaxonSynonymySource') = 0
begin

CREATE TABLE [dbo].[TaxonSynonymySource](
	[DatabaseName] [nvarchar](50) NOT NULL,
	[SourceName] [nvarchar](50) NULL,
 CONSTRAINT [PK_TaxonSynonymySource] PRIMARY KEY CLUSTERED 
(
	[DatabaseName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database where the data are taken from' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'DatabaseName'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'the name of the view retrieving the data from the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxonSynonymySource', @level2type=N'COLUMN',@level2name=N'SourceName'

END

GRANT SELECT ON TaxonSynonymySource TO [CollectionCacheUser]
GO
GRANT DELETE ON TaxonSynonymySource TO [CacheAdministrator] 
GO
GRANT UPDATE ON TaxonSynonymySource TO [CacheAdministrator]
GO
GRANT INSERT ON TaxonSynonymySource TO [CacheAdministrator]
GO




--#####################################################################################################################
--######   EnumKingdom   #########################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'EnumKingdom') = 0
begin
	CREATE TABLE [dbo].[EnumKingdom](
		[Kingdom] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_Kingdom_Enum] PRIMARY KEY CLUSTERED 
	(
		[Kingdom] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'Animalia') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Animalia')
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'Archaea') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Archaea')
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'Bacteria') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Bacteria')
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'Chromista') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Chromista')
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'Fungi') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Fungi')
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'incertae sedis') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'incertae sedis')
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'Plantae') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Plantae')
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'Protozoa') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Protozoa')
end
GO

if (select count(*) from EnumKingdom E where E.Kingdom = 'Viruses') = 0
begin
	INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Viruses')
end
GO


GRANT SELECT ON [EnumKingdom] TO [CollectionCacheUser]
GO
GRANT DELETE ON [EnumKingdom] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [EnumKingdom] TO [CollectionCacheUser]
GO
GRANT INSERT ON [EnumKingdom] TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   EnumLocalisationSystem   #########################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'EnumLocalisationSystem') = 0
begin
	CREATE TABLE [dbo].[EnumLocalisationSystem](
		[LocalisationSystemID] [int] NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
		[Sequence] [int] NOT NULL,
	 CONSTRAINT [PK_EnumLocalisationSystem] PRIMARY KEY CLUSTERED 
	(
		[LocalisationSystemID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO

if (select count(*) from EnumLocalisationSystem E where E.LocalisationSystemID = 2) = 0
begin
	INSERT [dbo].[EnumLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (2, N'Gauss-Krüger coordinates', 2)
end
GO

if (select count(*) from EnumLocalisationSystem E where E.LocalisationSystemID = 3) = 0
begin
	INSERT [dbo].[EnumLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (3, N'MTB (A, CH, D)', 5)
end
GO

if (select count(*) from EnumLocalisationSystem E where E.LocalisationSystemID = 7) = 0
begin
	INSERT [dbo].[EnumLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (7, N'Named area (DiversityGazetteer)', 4)
end
GO

if (select count(*) from EnumLocalisationSystem E where E.LocalisationSystemID = 8) = 0
begin
	INSERT [dbo].[EnumLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (8, N'Coord. WGS84', -1)
end
GO

if (select count(*) from EnumLocalisationSystem E where E.LocalisationSystemID = 13) = 0
begin
	INSERT [dbo].[EnumLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (13, N'Sampling plot (DiversitySamplingPlots)', 3)
end
GO

GRANT SELECT ON [EnumLocalisationSystem] TO [CollectionCacheUser]
GO
GRANT DELETE ON [EnumLocalisationSystem] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [EnumLocalisationSystem] TO [CollectionCacheUser]
GO
GRANT INSERT ON [EnumLocalisationSystem] TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   EnumMaterialCategory   #########################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'EnumMaterialCategory') = 0
begin
	CREATE TABLE [dbo].[EnumMaterialCategory](
		[Code] [nvarchar](50) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
		[RecordBasis] [nvarchar](50) NULL,
		[PreparationType] [nvarchar](50) NULL,
		[CategoryOrder] [int] NULL,
	 CONSTRAINT [PK_EnumMaterialCategory] PRIMARY KEY CLUSTERED 
	(
		[Code] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'bones', N'bones', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'complete skeleton', N'complete skeleton', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'cultures', N'cultures', N'LivingSpecimen', NULL, 6)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'DNA sample', N'DNA sample', N'PreservedSpecimen', NULL, 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'dried specimen', N'dried specimen', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil bones', N'fossil bones', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil complete skeleton', N'fossil complete skeleton', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil incomplete skeleton', N'fossil incomplete skeleton', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil otolith', N'fossil otolith', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil postcranial skeleton', N'fossil postcranial skeleton', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil scales', N'fossil scales', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil shell', N'fossil shell', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil single bones', N'fossil single bones', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil skull', N'fossil skull', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil specimen', N'fossil specimen', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'fossil tooth', N'fossil tooth', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'herbarium sheets', N'herbarium sheets', N'PreservedSpecimen', N'dried and pressed', 1)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'icones', N'icones', N'DrawingOrPhotograph', NULL, 3)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'incomplete skeleton', N'incomplete skeleton', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'micr. slide', N'micr. slide', N'PreservedSpecimen', N'microscopic preparation', 7)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'mould', N'mould', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'nest', N'nest', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'other specimen', N'other specimen', N'PreservedSpecimen', NULL, 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'otolith', N'otolith', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'pelt', N'pelt', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'photogr. print', N'photogr. print', N'DrawingOrPhotograph', NULL, 4)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'postcranial skeleton', N'postcranial skeleton', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'preserved specimen', N'preserved specimen', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'SEM table', N'SEM table', N'PreservedSpecimen', N'microscopic preparation', 8)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'single bones', N'single bones', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'skull', N'skull', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'specimen', N'specimen', N'PreservedSpecimen', N'dried', 0)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'tooth', N'tooth', N'PreservedSpecimen', N'dried', 100)
	INSERT [dbo].[EnumMaterialCategory] ([Code], [DisplayText], [RecordBasis], [PreparationType], [CategoryOrder]) VALUES (N'vial', N'vial', N'PreservedSpecimen', NULL, 100)

end
GO



GRANT SELECT ON [EnumMaterialCategory] TO [CollectionCacheUser]
GO
GRANT DELETE ON [EnumMaterialCategory] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [EnumMaterialCategory] TO [CollectionCacheUser]
GO
GRANT INSERT ON [EnumMaterialCategory] TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   EnumPreparationType   #########################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'EnumPreparationType') = 0
begin
	CREATE TABLE [dbo].[EnumPreparationType](
		[PreparationType] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_EnumPreparationType] PRIMARY KEY CLUSTERED 
	(
		[PreparationType] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	INSERT [dbo].[EnumPreparationType] ([PreparationType]) VALUES (N'dried')
	INSERT [dbo].[EnumPreparationType] ([PreparationType]) VALUES (N'dried and pressed')
	INSERT [dbo].[EnumPreparationType] ([PreparationType]) VALUES (N'microscopic preparation')
	INSERT [dbo].[EnumPreparationType] ([PreparationType]) VALUES (N'no treatment')
end
GO


GRANT SELECT ON [EnumPreparationType] TO [CollectionCacheUser]
GO
GRANT DELETE ON [EnumPreparationType] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [EnumPreparationType] TO [CollectionCacheUser]
GO
GRANT INSERT ON [EnumPreparationType] TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   EnumRecordBasis   #########################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'EnumRecordBasis') = 0
begin
	CREATE TABLE [dbo].[EnumRecordBasis](
		[RecordBasis] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_EnumRecordBasis] PRIMARY KEY CLUSTERED 
	(
		[RecordBasis] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	INSERT [dbo].[EnumRecordBasis] ([RecordBasis]) VALUES (N'DrawingOrPhotograph')
	INSERT [dbo].[EnumRecordBasis] ([RecordBasis]) VALUES (N'LivingSpecimen')
	INSERT [dbo].[EnumRecordBasis] ([RecordBasis]) VALUES (N'PreservedSpecimen')
end
GO

GRANT SELECT ON [EnumRecordBasis] TO [CollectionCacheUser]
GO
GRANT DELETE ON [EnumRecordBasis] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [EnumRecordBasis] TO [CollectionCacheUser]
GO
GRANT INSERT ON [EnumRecordBasis] TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   EnumTaxonomicGroup   #########################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'EnumTaxonomicGroup') = 0
begin
	CREATE TABLE [dbo].[EnumTaxonomicGroup](
		[Code] [nvarchar](50) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
		[Kingdom] [nvarchar](50) NULL,
	 CONSTRAINT [PK_TaxonomicGroup] PRIMARY KEY CLUSTERED 
	(
		[Code] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'fungus', N'fungus', N'Fungi')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'lichen', N'lichen', N'Fungi')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'myxomycete', N'slime mould', N'Protozoa')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'bryophyte', N'bryophyte', N'Plantae')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'plant', N'plant', N'Plantae')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'alga', N'alga', N'Plantae')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'mollusc', N'mollusc', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'animal', N'animal', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'arthropod', N'arthropod', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'insect', N'insect', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'echinoderm', N'echinoderm', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'vertebrate', N'vertebrate', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'fish', N'fish', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'amphibian', N'amphibian', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'reptile', N'reptile', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'mammal', N'mammal', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'cnidaria', N'cnidaria', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'evertebrate', N'evertebrate', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'bird', N'bird', N'Animalia')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'bacterium', N'bacterium', N'Bacteria')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'virus', N'virus', N'Viruses')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'chromista', N'chromista', N'Chromista')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'archaea', N'archaea', N'Archaea')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'unknown', N'unknown', N'incertae sedis')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'soil', N'soil', N'incertae sedis')
	INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'other', N'other', N'incertae sedis')
end
GO



GRANT SELECT ON [EnumTaxonomicGroup] TO [CollectionCacheUser]
GO
GRANT DELETE ON [EnumTaxonomicGroup] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [EnumTaxonomicGroup] TO [CollectionCacheUser]
GO
GRANT INSERT ON [EnumTaxonomicGroup] TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   TaxonomicGroupInProject   #########################################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TaxonomicGroupInProject') = 0
begin
	CREATE TABLE [dbo].[TaxonomicGroupInProject](
		[TaxonomicGroup] [nvarchar](50) NOT NULL,
		[ProjectID] [int] NOT NULL,
	 CONSTRAINT [PK_TaxonomicGroupInProject] PRIMARY KEY CLUSTERED 
	(
		[TaxonomicGroup] ASC,
		[ProjectID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO


GRANT SELECT ON [TaxonomicGroupInProject] TO [CollectionCacheUser]
GO
GRANT DELETE ON [TaxonomicGroupInProject] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [TaxonomicGroupInProject] TO [CollectionCacheUser]
GO
GRANT INSERT ON [TaxonomicGroupInProject] TO [CollectionCacheUser]
GO




--#####################################################################################################################
--######  RELATIONS  ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r where r.CONSTRAINT_NAME = 'FK_EnumMaterialCategory_EnumPreparationType') = 0
begin
	ALTER TABLE [dbo].[EnumMaterialCategory]  WITH CHECK ADD  CONSTRAINT [FK_EnumMaterialCategory_EnumPreparationType] FOREIGN KEY([PreparationType])
	REFERENCES [dbo].[EnumPreparationType] ([PreparationType])

	ALTER TABLE [dbo].[EnumMaterialCategory] CHECK CONSTRAINT [FK_EnumMaterialCategory_EnumPreparationType]
end
GO

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r where r.CONSTRAINT_NAME = 'FK_EnumMaterialCategory_EnumRecordBasis') = 0
begin

	ALTER TABLE [dbo].[EnumMaterialCategory]  WITH CHECK ADD  CONSTRAINT [FK_EnumMaterialCategory_EnumRecordBasis] FOREIGN KEY([RecordBasis])
	REFERENCES [dbo].[EnumRecordBasis] ([RecordBasis])

	ALTER TABLE [dbo].[EnumMaterialCategory] CHECK CONSTRAINT [FK_EnumMaterialCategory_EnumRecordBasis]
end
GO

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r where r.CONSTRAINT_NAME = 'FK_EnumTaxonomicGroup_EnumKingdom') = 0
begin
	ALTER TABLE [dbo].[EnumTaxonomicGroup]  WITH CHECK ADD  CONSTRAINT [FK_EnumTaxonomicGroup_EnumKingdom] FOREIGN KEY([Kingdom])
	REFERENCES [dbo].[EnumKingdom] ([Kingdom])

	ALTER TABLE [dbo].[EnumTaxonomicGroup] CHECK CONSTRAINT [FK_EnumTaxonomicGroup_EnumKingdom]
end
GO

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r where r.CONSTRAINT_NAME = 'FK_TaxonomicGroupInProject_EnumTaxonomicGroup') = 0
begin
	ALTER TABLE [dbo].[TaxonomicGroupInProject]  WITH CHECK ADD  CONSTRAINT [FK_TaxonomicGroupInProject_EnumTaxonomicGroup] FOREIGN KEY([TaxonomicGroup])
	REFERENCES [dbo].[EnumTaxonomicGroup] ([Code])

	ALTER TABLE [dbo].[TaxonomicGroupInProject] CHECK CONSTRAINT [FK_TaxonomicGroupInProject_EnumTaxonomicGroup]
end
GO

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r where r.CONSTRAINT_NAME = 'FK_TaxonomicGroupInProject_ProjectPublished') = 0
begin
	ALTER TABLE [dbo].[TaxonomicGroupInProject]  WITH CHECK ADD  CONSTRAINT [FK_TaxonomicGroupInProject_ProjectPublished] FOREIGN KEY([ProjectID])
	REFERENCES [dbo].[ProjectPublished] ([ProjectID])

	ALTER TABLE [dbo].[TaxonomicGroupInProject] CHECK CONSTRAINT [FK_TaxonomicGroupInProject_ProjectPublished]
end
GO


--#####################################################################################################################
--#####################################################################################################################
--######   Creating the generic basic functions etc.   ###################################
--#####################################################################################################################
--#####################################################################################################################




--#####################################################################################################################
--######   CollectionEventLocalisation  ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW CollectionEventLocalisation 
AS SELECT L.* 
FROM ' +  dbo.SourceDatebase() + '.dbo.CollectionEventLocalisation AS L INNER JOIN 
' +  dbo.SourceDatebase() + '.dbo.CollectionEvent AS E ON L.CollectionEventID = E.CollectionEventID 
WHERE (E.DataWithholdingReason = '''') OR 
(E.DataWithholdingReason IS NULL)')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON dbo.[CollectionEventLocalisation] TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   CollectionSpecimenEventID  ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[CollectionSpecimenEventID] 
AS 
SELECT S.CollectionSpecimenID, S.CollectionEventID 
FROM ' +  dbo.SourceDatebase() + '.dbo.CollectionSpecimen AS S WHERE NOT S.CollectionEventID IS NULL')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON dbo.[CollectionSpecimenEventID] TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######  TESTING   ######################################################################################
--#####################################################################################################################


declare @AllTestPassed char(3)
set @AllTestPassed = 'YES'
declare @Message nvarchar (999)
declare @SQL nvarchar (999)

--#####################################################################################################################
--######  TESTING CollectionSpecimenEventID  ######################################################################################
--#####################################################################################################################

begin try
	select count(*) from CollectionEventLocalisation
end try
begin catch
	set @AllTestPassed = 'NO'
	set @Message = 'The view CollectionEventLocalisation did not return a result. Please turn to your administrator to fix this before continuing with the update of the database'
	RAISERROR (@Message, 18, 1) 
end catch

--#####################################################################################################################
--######  TESTING CollectionSpecimenEventID  ######################################################################################
--#####################################################################################################################

begin try
	select count(*) from CollectionSpecimenEventID
end try
begin catch
	set @AllTestPassed = 'NO'
	set @Message = 'The view CollectionSpecimenEventID did not return a result. Please turn to your administrator to fix this before continuing with the update of the database'
	RAISERROR (@Message, 18, 1) 
end catch


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

--set @AllTestPassed = 'YES'

--select @AllTestPassed

if 	( @AllTestPassed = 'YES')
begin
	set @SQL = 'ALTER FUNCTION [dbo].[Version] ()  
	RETURNS nvarchar(8)
	AS
	BEGIN
	RETURN ''01.00.04''
	END'
	exec sp_executesql @SQL
end

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

/*
ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.04'
END

GO
*/


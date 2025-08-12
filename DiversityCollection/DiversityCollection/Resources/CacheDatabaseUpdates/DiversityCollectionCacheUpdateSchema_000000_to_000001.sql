
--#####################################################################################################################
--######   Roles   #########################################################################################################
--#####################################################################################################################

if (select count(*) from sysusers where issqlrole = 1 and name = 'CacheAdmin_#project#') = 0
begin
/****** Object:  DatabaseRole [Admin_#project#]    Script Date: 26.08.2015 17:20:59 ******/
	CREATE ROLE [CacheAdmin_#project#]
end
GO

if (select count(*) from sysusers where issqlrole = 1 and name = 'CacheUser_#project#') = 0
begin
	/****** Object:  DatabaseRole [User_#project#]    Script Date: 26.08.2015 17:21:16 ******/
	CREATE ROLE [CacheUser_#project#]
end
GO

EXEC sp_addrolemember 'CacheUser_#project#', 'CacheAdmin_#project#'

EXEC sp_addrolemember 'CacheUser_#project#', 'CacheUser'

EXEC sp_addrolemember 'CacheAdmin_#project#', 'CacheAdmin'

GO

GRANT EXEC on [#project#].ProjectID TO [CacheUser_#project#]
GO

--#####################################################################################################################
--######   Basic tables for configuration of the project   ############################################################
--######   and transfer of entries form previous version   ############################################################
--#####################################################################################################################


--#####################################################################################################################
--######   ProjectLocalisationSystem   ###################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA ='#project#' AND C.TABLE_NAME = 'ProjectLocalisationSystem') = 0
begin
	CREATE TABLE [#project#].[ProjectLocalisationSystem](
		[LocalisationSystemID] [int] NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
		[Sequence] [int] NOT NULL,
	 CONSTRAINT [PK_ProjectLocalisationSystem] PRIMARY KEY CLUSTERED 
	(
		[LocalisationSystemID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO

--#####################################################################################################################
--######  Adding default entries to  ProjectLocalisationSystem   #########################################################
--#####################################################################################################################


if (select count(*) from [#project#].ProjectLocalisationSystem E where E.LocalisationSystemID = 2) = 0
begin
	INSERT [#project#].[ProjectLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (2, N'Gauss-Krüger coordinates', 2)
end
GO

if (select count(*) from [#project#].ProjectLocalisationSystem E where E.LocalisationSystemID = 3) = 0
begin
	INSERT [#project#].[ProjectLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (3, N'MTB (A, CH, D)', 5)
end
GO

if (select count(*) from [#project#].ProjectLocalisationSystem E where E.LocalisationSystemID = 7) = 0
begin
	INSERT [#project#].[ProjectLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (7, N'Named area (DiversityGazetteer)', 4)
end
GO

if (select count(*) from [#project#].ProjectLocalisationSystem E where E.LocalisationSystemID = 8) = 0
begin
	INSERT [#project#].[ProjectLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (8, N'Coord. WGS84', -1)
end
GO

if (select count(*) from [#project#].ProjectLocalisationSystem E where E.LocalisationSystemID = 13) = 0
begin
	INSERT [#project#].[ProjectLocalisationSystem] ([LocalisationSystemID], [DisplayText], [Sequence]) VALUES (13, N'Sampling plot (DiversitySamplingPlots)', 3)
end
GO

GRANT SELECT ON [#project#].[ProjectLocalisationSystem] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[ProjectLocalisationSystem] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[ProjectLocalisationSystem] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[ProjectLocalisationSystem] TO [CacheAdmin_#project#]
GO




--#####################################################################################################################
--######   ProjectTaxonomicGroup   ####################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA ='#project#' AND C.TABLE_NAME = 'ProjectTaxonomicGroup') = 0
begin
	CREATE TABLE [#project#].[ProjectTaxonomicGroup](
		[TaxonomicGroup] [nvarchar](50) NOT NULL
	 CONSTRAINT [PK_ProjectTaxonomicGroup] PRIMARY KEY CLUSTERED 
	(
		[TaxonomicGroup] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO

GRANT SELECT ON [#project#].[ProjectTaxonomicGroup] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[ProjectTaxonomicGroup] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[ProjectTaxonomicGroup] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[ProjectTaxonomicGroup] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   Transfer data from previous table into ProjectTaxonomicGroup   #############################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES C where C.TABLE_SCHEMA ='dbo' AND C.TABLE_NAME = 'TaxonomicGroupInProject') > 0
BEGIN
	INSERT INTO [#project#].[ProjectTaxonomicGroup]
			   ([TaxonomicGroup])
	SELECT [TaxonomicGroup]
	  FROM [dbo].[TaxonomicGroupInProject]
	  P WHERE P.ProjectID = [#project#].ProjectID()
END



--#####################################################################################################################
--######   ProjectMaterialCategory   ##################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA ='#project#' AND C.TABLE_NAME = 'ProjectMaterialCategory') = 0
begin
	CREATE TABLE [#project#].[ProjectMaterialCategory](
		[MaterialCategory] [nvarchar](50) NOT NULL
	 CONSTRAINT [PK_ProjectMaterialCategory] PRIMARY KEY CLUSTERED 
	(
		[MaterialCategory] 
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO

GRANT SELECT ON [#project#].[ProjectMaterialCategory] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[ProjectMaterialCategory] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[ProjectMaterialCategory] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[ProjectMaterialCategory] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   Transfer data from previous table into ProjectMaterialCategory   ###########################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES C where C.TABLE_SCHEMA ='dbo' AND C.TABLE_NAME = 'EnumMaterialCategory') > 0
BEGIN
	INSERT INTO [#project#].[ProjectMaterialCategory]
			   ([MaterialCategory])
	SELECT Code
	  FROM [dbo].[EnumMaterialCategory]
	  E WHERE E.ProjectID = [#project#].ProjectID()
END


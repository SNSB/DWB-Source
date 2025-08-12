
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.07'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   CollectionAgentCache      #############################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionAgentCache') = 0
begin
	CREATE TABLE [dbo].[CollectionAgentCache](
		[CollectionSpecimenID] [int] NOT NULL,
		[CollectorsName] [nvarchar](255) NOT NULL,
		[CollectorsSequence] [datetime2](7) NULL,
		[CollectorsNumber] [nvarchar](50) NULL,
	 CONSTRAINT [PK_CollectionAgentCache] PRIMARY KEY CLUSTERED 
	(
		[CollectionSpecimenID] ASC,
		[CollectorsName] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

GRANT SELECT ON [CollectionAgentCache] TO [CollectionCacheUser]
GO
GRANT DELETE ON [CollectionAgentCache] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [CollectionAgentCache] TO [CacheAdministrator]
GO
GRANT INSERT ON [CollectionAgentCache] TO [CacheAdministrator]
GO



--#####################################################################################################################
--######   procTransferCollectionAgent      #############################################################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferCollectionAgent] 
AS
/*
-- TEST
EXECUTE  [dbo].[procTransferCollectionAgent]
*/
truncate table CollectionAgentCache

INSERT INTO [dbo].[CollectionAgentCache]
           ([CollectionSpecimenID]
           ,[CollectorsName]
           ,[CollectorsSequence]
           ,[CollectorsNumber])
SELECT    A.CollectionSpecimenID, CollectorsName, CollectorsSequence, CollectorsNumber
FROM  ' + dbo.SourceDatebase() + '.dbo.CollectionAgent AS A, CollectionSpecimen S
WHERE   A.CollectionSpecimenID = S.CollectionSpecimenID AND     
((DataWithholdingReason = N'''') OR
(DataWithholdingReason IS NULL))
ORDER BY CollectorsName')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO

GRANT EXECUTE ON [dbo].[procTransferCollectionAgent] TO [CacheAdministrator]
GO




--#####################################################################################################################
--######   CollectionProjectCache      #############################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionProjectCache') = 0
begin
	CREATE TABLE [dbo].[CollectionProjectCache](
		[CollectionSpecimenID] [int] NOT NULL,
		[ProjectID] [int] NOT NULL,
	 CONSTRAINT [PK_CollectionProjectCache] PRIMARY KEY CLUSTERED 
	(
		[CollectionSpecimenID] ASC,
		[ProjectID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO


GRANT SELECT ON [CollectionProjectCache] TO [CollectionCacheUser]
GO
GRANT DELETE ON [CollectionProjectCache] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [CollectionProjectCache] TO [CacheAdministrator]
GO
GRANT INSERT ON [CollectionProjectCache] TO [CacheAdministrator]
GO



--#####################################################################################################################
--######   procTransferCollectionProject      #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferCollectionProject] 
AS
truncate table CollectionProjectCache


INSERT INTO CollectionProjectCache
                      (CollectionSpecimenID, ProjectID)
SELECT     C.CollectionSpecimenID, C.ProjectID
FROM         CollectionProject C, [dbo].[ProjectPublished] P, CollectionSpecimen S
where C.ProjectID = P.ProjectID
and C.CollectionSpecimenID = S.CollectionSpecimenID

DECLARE @i int
SET @i = (SELECT COUNT(*) FROM CollectionProjectCache)
SELECT CAST(@i AS VARCHAR) + '' project entries imported''')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch

GO

GRANT EXECUTE ON [dbo].[procTransferCollectionProject] TO [CacheAdministrator]
GO



--#####################################################################################################################
--######   CollectionSpecimenImageCache      #############################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionSpecimenImageCache') = 0
begin
	CREATE TABLE [dbo].[CollectionSpecimenImageCache](
		[CollectionSpecimenID] [int] NOT NULL,
		[URI] [varchar](255) NOT NULL,
		[MediaFormat] [varchar](255) NULL,
		[AccNrUnitID] [nvarchar](90) NOT NULL,
		[IdentificationUnitID] [int] NULL,
		[DisplayOrder] [bigint] NULL,
		[ProductURI] [varchar](255) NULL,
		[ImageSpecimenPartID] [int] NULL,
		[ImageIdentificationUnitID] [int] NULL,
		[LicenseType] [nvarchar](500) NULL,
		[LicenseNotes] [nvarchar](500) NULL,
		[LicenseURI] [varchar](500) NULL,
		[CreatorAgent] [nvarchar](500) NULL,
	 CONSTRAINT [PK_CollectionSpecimenImageCache] PRIMARY KEY CLUSTERED 
	(
		[CollectionSpecimenID] ASC,
		[URI] ASC,
		[AccNrUnitID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO

GRANT SELECT ON [CollectionSpecimenImageCache] TO [CollectionCacheUser]
GO
GRANT DELETE ON [CollectionSpecimenImageCache] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [CollectionSpecimenImageCache] TO [CacheAdministrator]
GO
GRANT INSERT ON [CollectionSpecimenImageCache] TO [CacheAdministrator]
GO

--#####################################################################################################################
--######   CollectionSpecimenImage_DisplayOrder      #############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[CollectionSpecimenImage_DisplayOrder]
AS
SELECT t.CollectionSpecimenID, URI
, ROW_NUMBER() OVER(ORDER BY (SELECT 1))  AS DisplayOrder
FROM (
    SELECT TOP 100 PERCENT [CollectionSpecimenID], URI from ' + dbo.SourceDatebase() + '.dbo.CollectionSpecimenImage 
    ORDER BY [CollectionSpecimenID], URI
) AS t')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON [dbo].[CollectionSpecimenImage_DisplayOrder] TO [CacheAdministrator]
GO


--#####################################################################################################################
--######   procTransferCollectionSpecimenImage      #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferCollectionSpecimenImage] 
AS
/*
-- TEST
EXECUTE  [dbo].[procTransferCollectionSpecimenImage]
*/
truncate table CollectionSpecimenImageCache

INSERT INTO [dbo].[CollectionSpecimenImageCache]
           ([CollectionSpecimenID]
           ,[URI]
           ,[MediaFormat]
           ,[AccNrUnitID]
           ,[IdentificationUnitID]
           ,[DisplayOrder]
           ,[ProductURI]
           ,[ImageSpecimenPartID]
           ,[ImageIdentificationUnitID]
           ,[LicenseType]
           ,[LicenseNotes]
           ,[LicenseURI]
           ,[CreatorAgent])
SELECT DISTINCT 
TOP (100) PERCENT I.CollectionSpecimenID, 
CASE WHEN S.EventDataWithholdingReason IS NULL OR
S.EventDataWithholdingReason = '''' THEN I.URI ELSE NULL END AS URI, SUBSTRING(I.URI, LEN(I.URI) - 2, 255) AS MediaFormat, 
U.AccNrUnitID, 
U.IdentificationUnitID, 
CASE WHEN I.DisplayOrder IS NULL THEN D .DisplayOrder ELSE I.DisplayOrder END AS DisplayOrder, 
dbo.HighResolutionImagePath(I.URI) AS ProductURI, 
I.SpecimenPartID AS ImageSpecimenPartID, 
I.IdentificationUnitID AS ImageIdentificationUnitID, 
I.LicenseType, 
I.LicenseNotes, 
I.LicenseURI, 
I.CreatorAgent
FROM            ' + dbo.SourceDatebase() + '.dbo.CollectionSpecimenImage AS I INNER JOIN
IdentificationUnitPartCache AS U ON I.CollectionSpecimenID = U.CollectionSpecimenID INNER JOIN
CollectionSpecimenImage_DisplayOrder AS D ON I.CollectionSpecimenID = D.CollectionSpecimenID AND I.URI = D.URI INNER JOIN
CollectionSpecimen AS S ON I.CollectionSpecimenID = S.CollectionSpecimenID
WHERE       ( (I.DataWithholdingReason = N'''') AND (I.SpecimenPartID IS NULL) OR
(I.DataWithholdingReason IS NULL) AND (I.SpecimenPartID IS NULL)) 

UNION 

SELECT DISTINCT 
TOP (100) PERCENT S.CollectionSpecimenID, CASE WHEN S.EventDataWithholdingReason IS NULL OR
S.EventDataWithholdingReason = '''' THEN I.URI ELSE NULL END AS URI, SUBSTRING(I.URI, LEN(I.URI) - 2, 255) AS MediaFormat, U.AccNrUnitID, 
U.IdentificationUnitID, CASE WHEN I.DisplayOrder IS NULL THEN D .DisplayOrder ELSE I.DisplayOrder END AS DisplayOrder, dbo.HighResolutionImagePath(I.URI) 
AS ProductURI, I.SpecimenPartID AS ImageSpecimenPartID, I.IdentificationUnitID AS ImageIdentificationUnitID, I.LicenseType, I.LicenseNotes, I.LicenseURI, 
I.CreatorAgent
FROM ' + dbo.SourceDatebase() + '.dbo.CollectionSpecimenImage AS I INNER JOIN
CollectionSpecimen AS S ON I.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN
IdentificationUnitPartCache AS U ON I.CollectionSpecimenID = U.CollectionSpecimenID AND I.SpecimenPartID = U.SpecimenPartID INNER JOIN
CollectionSpecimenImage_DisplayOrder AS D ON I.CollectionSpecimenID = D.CollectionSpecimenID AND I.URI = D.URI
WHERE (I.DataWithholdingReason = N'''') OR
(I.DataWithholdingReason IS NULL) ')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO

GRANT EXECUTE ON [dbo].[procTransferCollectionSpecimenImage] TO [CacheAdministrator]
GO




--#####################################################################################################################
--######   CollectionSpecimenPartCache      #############################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionSpecimenPartCache') = 0
begin
	CREATE TABLE [dbo].[CollectionSpecimenPartCache](
		[CollectionSpecimenID] [int] NOT NULL,
		[MaterialCategory] [nvarchar](50) NOT NULL,
		[RecordBasis] [varchar](19) NOT NULL,
		[PreparationType] [varchar](23) NULL,
		[MaterialCategoryOrder] [int] NOT NULL,
		[StorageLocation] [nvarchar](255) NULL,
		[SpecimenPartID] [int] NULL,
		[AccNrUnitID] [nvarchar](200) NULL
	) ON [PRIMARY]
end
GO

GRANT SELECT ON [CollectionSpecimenPartCache] TO [CollectionCacheUser]
GO
GRANT DELETE ON [CollectionSpecimenPartCache] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [CollectionSpecimenPartCache] TO [CacheAdministrator]
GO
GRANT INSERT ON [CollectionSpecimenPartCache] TO [CacheAdministrator]
GO



--#####################################################################################################################
--######   procTransferCollectionSpecimenPart      #############################################################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferCollectionSpecimenPart] 
AS
/*
-- TEST
EXECUTE  [dbo].[procTransferCollectionSpecimenPart]
*/
truncate table CollectionSpecimenPartCache

INSERT INTO [dbo].[CollectionSpecimenPartCache]
           ([CollectionSpecimenID]
           ,[MaterialCategory]
           ,[RecordBasis]
           ,[PreparationType]
           ,[MaterialCategoryOrder]
           ,[StorageLocation]
		   ,SpecimenPartID
		   ,AccNrUnitID)
SELECT P.CollectionSpecimenID
, P.MaterialCategory
, CASE WHEN M.RecordBasis IS NULL THEN '''' ELSE M.RecordBasis END AS RecordBasis
, M.PreparationType
, CASE WHEN M.CategoryOrder IS NULL THEN '''' ELSE M.CategoryOrder END AS MaterialCategoryOrder
, MAX(P.StorageLocation) AS StorageLocation
, P.SpecimenPartID
, CASE WHEN S.AccessionNumber IS NULL THEN CAST(P.CollectionSpecimenID AS varchar) ELSE S.AccessionNumber END + '' / '' + CAST(U.IdentificationUnitID AS varchar) + '' / '' + CAST(U.SpecimenPartID AS varchar) AS AccNrUnitID
FROM        CollectionSpecimen AS S INNER JOIN
            ' + dbo.SourceDatebase() + '.dbo.CollectionSpecimenPart AS P INNER JOIN
            ' + dbo.SourceDatebase() + '.dbo.IdentificationUnitInPart AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID AND P.SpecimenPartID = U.SpecimenPartID ON 
            S.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN
            EnumMaterialCategory AS M ON P.MaterialCategory = M.Code
GROUP BY P.CollectionSpecimenID, P.MaterialCategory, P.SpecimenPartID, U.IdentificationUnitID, S.AccessionNumber, U.SpecimenPartID, M.RecordBasis, M.PreparationType, 
                         M.CategoryOrder')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO

GRANT EXECUTE ON [dbo].[procTransferCollectionSpecimenPart] TO [CacheAdministrator]
GO

--#####################################################################################################################
--######   [ABCD_Metadata]      #############################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ABCD_Metadata') = 0
begin
	CREATE TABLE [dbo].[ABCD_Metadata](
		[ProjectID] [int] NOT NULL,
		[ProjectTitleCode] [nvarchar](254) NULL,
		[TaxonomicGroup] [nvarchar](254) NULL,
		[DatasetGUID] [nvarchar](254) NULL,
		[TechnicalContactName] [nvarchar](254) NULL,
		[TechnicalContactEmail] [nvarchar](254) NULL,
		[TechnicalContactPhone] [nvarchar](254) NULL,
		[TechnicalContactAddress] [nvarchar](254) NULL,
		[ContentContactName] [nvarchar](254) NULL,
		[ContentContactEmail] [nvarchar](254) NULL,
		[ContentContactPhone] [nvarchar](254) NULL,
		[ContentContactAddress] [nvarchar](254) NULL,
		[OtherProviderUDDI] [nvarchar](254) NULL,
		[DatasetTitle] [nvarchar](254) NULL,
		[DatasetDetails] [nvarchar](254) NULL,
		[DatasetCoverage] [nvarchar](254) NULL,
		[DatasetURI] [nvarchar](254) NULL,
		[DatasetIconURI] [nvarchar](254) NULL,
		[DatasetVersionMajor] [nvarchar](254) NULL,
		[DatasetCreators] [nvarchar](254) NULL,
		[DatasetContributors] [nvarchar](254) NULL,
		[DateCreated] [nvarchar](254) NULL,
		[DateModified] [nvarchar](254) NULL,
		[SourceID] [nvarchar](254) NULL,
		[SourceInstitutionID] [nvarchar](254) NULL,
		[OwnerOrganizationName] [nvarchar](254) NULL,
		[OwnerOrganizationAbbrev] [nvarchar](254) NULL,
		[OwnerContactPerson] [nvarchar](254) NULL,
		[OwnerContactRole] [nvarchar](254) NULL,
		[OwnerAddress] [nvarchar](254) NULL,
		[OwnerTelephone] [nvarchar](254) NULL,
		[OwnerEmail] [nvarchar](254) NULL,
		[OwnerURI] [nvarchar](254) NULL,
		[OwnerLogoURI] [nvarchar](254) NULL,
		[IPRText] [nvarchar](254) NULL,
		[IPRDetails] [nvarchar](254) NULL,
		[IPRURI] [nvarchar](254) NULL,
		[CopyrightText] [nvarchar](254) NULL,
		[CopyrightDetails] [nvarchar](254) NULL,
		[CopyrightURI] [nvarchar](254) NULL,
		[TermsOfUseText] [nvarchar](254) NULL,
		[TermsOfUseDetails] [nvarchar](254) NULL,
		[TermsOfUseURI] [nvarchar](254) NULL,
		[DisclaimersText] [nvarchar](254) NULL,
		[DisclaimersDetails] [nvarchar](254) NULL,
		[DisclaimersURI] [nvarchar](254) NULL,
		[LicenseText] [nvarchar](254) NULL,
		[LicensesDetails] [nvarchar](254) NULL,
		[LicenseURI] [nvarchar](254) NULL,
		[AcknowledgementsText] [nvarchar](254) NULL,
		[AcknowledgementsDetails] [nvarchar](254) NULL,
		[AcknowledgementsURI] [nvarchar](254) NULL,
		[CitationsText] [nvarchar](254) NULL,
		[CitationsDetails] [nvarchar](254) NULL,
		[CitationsURI] [nvarchar](254) NULL,
		[RecordBasis] [nvarchar](254) NULL,
		[KindOfUnit] [nvarchar](254) NULL,
		[HigherTaxonRank] [nvarchar](254) NULL,
		[RecordURI] [nvarchar](254) NULL,
	 CONSTRAINT [PK_ABCD_Metadata] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO


GRANT SELECT ON [ABCD_Metadata] TO [CollectionCacheUser]
GO
GRANT DELETE ON [ABCD_Metadata] TO [CacheAdministrator] 
GO
GRANT UPDATE ON [ABCD_Metadata] TO [CacheAdministrator]
GO
GRANT INSERT ON [ABCD_Metadata] TO [CacheAdministrator]
GO

--#####################################################################################################################
--######   ABCD_Metadata      #############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = ''
declare @SQL1 nvarchar(max)
declare @SQL2 nvarchar(max)
declare @SQL3 nvarchar(max)
declare @SQL4 nvarchar(max)
declare @SQL5 nvarchar(max)
declare @SQL6 nvarchar(max)

set @SQL1 = (select 'CREATE FUNCTION [dbo].[Metadata] (@ProjectID int)  
RETURNS @Metadata TABLE (
 ProjectID integer NULL,
  ProjectTitleCode nvarchar(254),
  TaxonomicGroup nvarchar(254),
  DatasetGUID nvarchar(254),
  TechnicalContactName nvarchar(254),
  TechnicalContactEmail nvarchar(254),
  TechnicalContactPhone nvarchar(254),
  TechnicalContactAddress nvarchar(254),
  ContentContactName nvarchar(254),
  ContentContactEmail nvarchar(254),
  ContentContactPhone nvarchar(254),
  ContentContactAddress nvarchar(254),
  OtherProviderUDDI nvarchar(254),
  DatasetTitle nvarchar(254),
  DatasetDetails nvarchar(254),
  DatasetCoverage nvarchar(254),
  DatasetURI nvarchar(254),
  DatasetIconURI nvarchar(254),
  DatasetVersionMajor nvarchar(254),
  DatasetCreators nvarchar(254),
  DatasetContributors nvarchar(254),
  DateCreated nvarchar(254),
  DateModified nvarchar(254),
  SourceID nvarchar(254),
  SourceInstitutionID nvarchar(254),
  OwnerOrganizationName nvarchar(254),
  OwnerOrganizationAbbrev nvarchar(254),
  OwnerContactPerson nvarchar(254),
  OwnerContactRole nvarchar(254),
  OwnerAddress nvarchar(254),
  OwnerTelephone nvarchar(254),
  OwnerEmail nvarchar(254),
  OwnerURI nvarchar(254),
  OwnerLogoURI nvarchar(254),
  IPRText nvarchar(254),
  IPRDetails nvarchar(254),
  IPRURI nvarchar(254),
  CopyrightText nvarchar(500),
  CopyrightDetails nvarchar(254),
  CopyrightURI nvarchar(254),
  TermsOfUseText nvarchar(254),
  TermsOfUseDetails nvarchar(254),
  TermsOfUseURI nvarchar(254),
  DisclaimersText nvarchar(254),
  DisclaimersDetails nvarchar(254),
  DisclaimersURI nvarchar(254),
  LicenseText nvarchar(254),
  LicensesDetails nvarchar(254),
  LicenseURI nvarchar(254),
  AcknowledgementsText nvarchar(254),
  AcknowledgementsDetails nvarchar(254),
  AcknowledgementsURI nvarchar(254),
  CitationsText nvarchar(254),
  CitationsDetails nvarchar(254),
  CitationsURI nvarchar(254),
  RecordBasis nvarchar(254),
  KindOfUnit nvarchar(254),
  HigherTaxonRank nvarchar(254),
  RecordURI nvarchar(254)
)
begin')

set @SQL2 = (@SQL + '
insert into @Metadata (ProjectID) values (@ProjectID)

declare  @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectSetting] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)

	INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
	SELECT SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, 
	case when isdate(value) = 1 and ProjectSetting like ''%Date%'' then convert(varchar(20), cast(value as datetime), 126) else Value end as Value 
	FROM ' + dbo.ProjectsDatabase() + '.DBO.SettingsForProject(@ProjectID, ''ABCD | %'', ''.'', 2)
	IF (SELECT COUNT(*) FROM @SettingList L WHERE ProjectSetting = ''DateCreated'') = 0
	BEGIN
		INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
		SELECT -1, null, ''DateCreated'', ''DateCreated'', ''DateCreated'', 
		convert(varchar(19), min(LogCreatedWhen), 126) 
		from ' + dbo.SourceDatebase() + '.dbo.CollectionProject 
		where ProjectID = @ProjectID 
	END
	IF (SELECT COUNT(*) FROM @SettingList L WHERE ProjectSetting = ''DateModified'') = 0
	BEGIN
		INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
		SELECT -2, null, ''DateModified'', ''DateModified'', ''DateModified'', 
		convert(varchar(19), max(LogInsertedWhen), 126) 
		from CollectionSpecimenCache
	END')

set @SQL3 = (@SQL + '
UPDATE @Metadata SET ProjectTitleCode  = (SELECT P.Project FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = @ProjectID)
UPDATE @Metadata SET LicenseText  =   (SELECT MIN(L.LicenseType)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = @ProjectID
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID)))

UPDATE @Metadata SET LicensesDetails  =   (SELECT MIN(L.LicenseDetails)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = @ProjectID
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID)))

UPDATE @Metadata SET LicenseURI  =   (SELECT MIN(L.LicenseURI)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = @ProjectID
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID)))
')

set @SQL4 = (@SQL + '
UPDATE @Metadata SET TaxonomicGroup  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TaxonomicGroup'')
UPDATE @Metadata SET DatasetGUID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetGUID'')
UPDATE @Metadata SET TechnicalContactName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactName'')
UPDATE @Metadata SET TechnicalContactEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactEmail'')
UPDATE @Metadata SET TechnicalContactPhone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactPhone'')
UPDATE @Metadata SET TechnicalContactAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactAddress'')
UPDATE @Metadata SET ContentContactName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactName'')
UPDATE @Metadata SET ContentContactEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactEmail'')
UPDATE @Metadata SET ContentContactPhone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactPhone'')
UPDATE @Metadata SET ContentContactAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactAddress'')
UPDATE @Metadata SET OtherProviderUDDI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OtherProviderUDDI'')
UPDATE @Metadata SET DatasetTitle  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetTitle'')
UPDATE @Metadata SET DatasetDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetDetails'')
UPDATE @Metadata SET DatasetCoverage  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetCoverage'')
UPDATE @Metadata SET DatasetURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetURI'')
UPDATE @Metadata SET DatasetIconURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetIconURI'')
UPDATE @Metadata SET DatasetVersionMajor  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetVersionMajor'')
UPDATE @Metadata SET DatasetCreators  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetCreators'')
')
set @SQL5 = (@SQL + '
UPDATE @Metadata SET DatasetContributors  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetContributors'')
UPDATE @Metadata SET DateCreated  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DateCreated'')
UPDATE @Metadata SET DateModified  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DateModified'')
UPDATE @Metadata SET SourceID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''SourceID'')
UPDATE @Metadata SET SourceInstitutionID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''SourceInstitutionID'')
UPDATE @Metadata SET OwnerOrganizationName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerOrganizationName'')
UPDATE @Metadata SET OwnerOrganizationAbbrev  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerOrganizationAbbrev'')
UPDATE @Metadata SET OwnerContactPerson  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerContactPerson'')
UPDATE @Metadata SET OwnerContactRole  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerContactRole'')
UPDATE @Metadata SET OwnerAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerAddress'')
UPDATE @Metadata SET OwnerTelephone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerTelephone'')
UPDATE @Metadata SET OwnerEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerEmail'')
UPDATE @Metadata SET OwnerURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerURI'')
UPDATE @Metadata SET OwnerLogoURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerLogoURI'')
UPDATE @Metadata SET IPRText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRText'')
UPDATE @Metadata SET IPRDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRDetails'')
UPDATE @Metadata SET IPRURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRURI'')
UPDATE @Metadata SET CopyrightText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightText'')
')
set @SQL6 = (@SQL + '
UPDATE @Metadata SET CopyrightDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightDetails'')
UPDATE @Metadata SET CopyrightURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightURI'')
UPDATE @Metadata SET TermsOfUseText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseText'')
UPDATE @Metadata SET TermsOfUseDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseDetails'')
UPDATE @Metadata SET TermsOfUseURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseURI'')
UPDATE @Metadata SET DisclaimersText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersText'')
UPDATE @Metadata SET DisclaimersDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersDetails'')
UPDATE @Metadata SET DisclaimersURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersURI'')
UPDATE @Metadata SET AcknowledgementsText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsText'')
UPDATE @Metadata SET AcknowledgementsDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsDetails'')
UPDATE @Metadata SET AcknowledgementsURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsURI'')
UPDATE @Metadata SET CitationsText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsText'')
UPDATE @Metadata SET CitationsDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsDetails'')
UPDATE @Metadata SET CitationsURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsURI'')
UPDATE @Metadata SET RecordBasis  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''RecordBasis'')
UPDATE @Metadata SET KindOfUnit  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''KindOfUnit'')
UPDATE @Metadata SET HigherTaxonRank  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''HigherTaxonRank'')
UPDATE @Metadata SET RecordURI = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''RecordURI'')

return
END')

begin try
exec ( @SQL1 + @SQL2 + @SQL3 + @SQL4 + @SQL5 + @SQL6)
end try
begin catch
set @SQL1 = 'ALTER ' + SUBSTRING(@SQL1, 8, 4000)
exec (  @SQL1 + @SQL2 + @SQL3 + @SQL4 + @SQL5 + @SQL6)
end catch


GO

GRANT SELECT ON dbo.Metadata TO [CacheAdministrator] 
GO


--#####################################################################################################################
--######   ----------------      #############################################################################
--#####################################################################################################################




declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferMetadata] 
AS
/*
-- TEST
EXECUTE  [dbo].[procTransferMetadata] 
*/
truncate table ABCD_Metadata
declare @ProjectIDs table (ProjectID int NOT NULL)
insert into @ProjectIDs (ProjectID) SELECT ProjectID from ProjectPublished
declare @ProjectID int
set @ProjectID = (select min(ProjectID) from @ProjectIDs)
WHILE (Select count(*) from @ProjectIDs) > 0
begin
	INSERT INTO ABCD_Metadata
                         (ProjectID, ProjectTitleCode, TaxonomicGroup, DatasetGUID, TechnicalContactName, TechnicalContactEmail, TechnicalContactPhone, TechnicalContactAddress, 
                         ContentContactName, ContentContactEmail, ContentContactPhone, ContentContactAddress, OtherProviderUDDI, DatasetTitle, DatasetDetails, DatasetCoverage, 
                         DatasetURI, DatasetIconURI, DatasetVersionMajor, DatasetCreators, DatasetContributors, DateCreated, DateModified, SourceID, SourceInstitutionID, 
                         OwnerOrganizationName, OwnerOrganizationAbbrev, OwnerContactPerson, OwnerContactRole, OwnerAddress, OwnerTelephone, OwnerEmail, OwnerURI, 
                         OwnerLogoURI, IPRText, IPRDetails, IPRURI, CopyrightText, CopyrightDetails, CopyrightURI, TermsOfUseText, TermsOfUseDetails, TermsOfUseURI, 
                         DisclaimersText, DisclaimersDetails, DisclaimersURI, LicenseText, LicensesDetails, LicenseURI, AcknowledgementsText, AcknowledgementsDetails, 
                         AcknowledgementsURI, CitationsText, CitationsDetails, CitationsURI, RecordBasis, KindOfUnit, HigherTaxonRank, RecordURI)
	SELECT        ProjectID, ProjectTitleCode, TaxonomicGroup, DatasetGUID, TechnicalContactName, TechnicalContactEmail, TechnicalContactPhone, TechnicalContactAddress, 
                         ContentContactName, ContentContactEmail, ContentContactPhone, ContentContactAddress, OtherProviderUDDI, DatasetTitle, DatasetDetails, DatasetCoverage, 
                         DatasetURI, DatasetIconURI, DatasetVersionMajor, DatasetCreators, DatasetContributors, DateCreated, DateModified, SourceID, SourceInstitutionID, 
                         OwnerOrganizationName, OwnerOrganizationAbbrev, OwnerContactPerson, OwnerContactRole, OwnerAddress, OwnerTelephone, OwnerEmail, OwnerURI, 
                         OwnerLogoURI, IPRText, IPRDetails, IPRURI, CopyrightText, CopyrightDetails, CopyrightURI, TermsOfUseText, TermsOfUseDetails, TermsOfUseURI, 
                         DisclaimersText, DisclaimersDetails, DisclaimersURI, LicenseText, LicensesDetails, LicenseURI, AcknowledgementsText, AcknowledgementsDetails, 
                         AcknowledgementsURI, CitationsText, CitationsDetails, CitationsURI, RecordBasis, KindOfUnit, HigherTaxonRank, RecordURI
	FROM            dbo.Metadata(@ProjectID)
	DELETE P FROM @ProjectIDs P WHERE P.ProjectID = @ProjectID
	set @ProjectID = (select min(ProjectID) from @ProjectIDs)
end

DECLARE @i int
SET @i = (SELECT COUNT(*) FROM ABCD_Metadata)
SELECT CAST(@i AS VARCHAR) + '' metadata imported''')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch


GO

GRANT EXECUTE ON [dbo].[procTransferMetadata] TO [CacheAdministrator]
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.08'
END

GO



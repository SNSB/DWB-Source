--#####################################################################################################################
--######   CacheProjectReference - add URI   ##########################################################################
--#####################################################################################################################


ALTER TABLE [#project#].CacheProjectReference ADD URI [nvarchar](500) NULL
GO


--#####################################################################################################################
--######   procPublishProjectReference - add URI  #####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishProjectReference] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProjectReference]
*/
truncate table [#project#].CacheProjectReference

INSERT INTO [#project#].[CacheProjectReference]
           ([ProjectID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[ReferenceType]
      ,[Notes]
	  ,[URI])
SELECT DISTINCT [ProjectID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[ReferenceType]
      ,[Notes]
	  ,[URI]
  FROM ' + dbo.ProjectsDatabase() + '.DBO.ProjectReference R
  where R.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO
--#####################################################################################################################
--######   CacheMetadata - CopyrightText -> 500  ######################################################################
--#####################################################################################################################


ALTER TABLE [#project#].CacheMetadata ALTER COLUMN CopyrightText [nvarchar](500) NULL
GO

--#####################################################################################################################
--######   CacheMetadata - add ProjectTitle  ##########################################################################
--#####################################################################################################################


ALTER TABLE [#project#].CacheMetadata ADD ProjectTitle [nvarchar](400) NULL
GO


--#####################################################################################################################
--######   procPublishMetadata - add ProjectTitle   ###################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishMetadata] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishMetadata] 
*/
truncate table  [#project#].[CacheMetadata]
-- temporary table
insert into [#project#].[CacheMetadata] (ProjectID) values ([#project#].ProjectID())

declare  @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectSetting] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
')

set @SQL = (@SQL + '
	INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
	SELECT SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, 
	case when isdate(value) = 1 and ProjectSetting like ''%Date%'' then convert(varchar(20), cast(value as datetime), 126) else Value end as Value 
	FROM ' + dbo.ProjectsDatabase() + '.DBO.SettingsForProject([#project#].ProjectID(), ''ABCD | %'', ''.'', 2)
	IF (SELECT COUNT(*) FROM @SettingList L WHERE ProjectSetting = ''DateCreated'') = 0
	BEGIN
		INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
		SELECT -1, null, ''DateCreated'', ''DateCreated'', ''DateCreated'', 
		convert(varchar(19), min(LogCreatedWhen), 126) 
		from ' + dbo.SourceDatabase() + '.dbo.CollectionProject 
		where ProjectID = [#project#].ProjectID() 
	END
	')

set @SQL = (@SQL + '
	IF (SELECT COUNT(*) FROM @SettingList L WHERE ProjectSetting = ''DateModified'') = 0
	BEGIN
		INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
		SELECT -2, null, ''DateModified'', ''DateModified'', ''DateModified'', 
		convert(varchar(19), max(LastUpdatedWhen), 126) 
		from [dbo].ProjectPublished P WHERE P.ProjectID = [#project#].ProjectID()
	END
	')

set @SQL = (@SQL + '
UPDATE [#project#].[CacheMetadata] SET ProjectTitleCode  = (SELECT P.Project FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = [#project#].ProjectID())
UPDATE [#project#].[CacheMetadata] SET ProjectTitle  = (SELECT P.ProjectTitle FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = [#project#].ProjectID())
UPDATE [#project#].[CacheMetadata] SET LicenseText  =   (SELECT MIN(L.LicenseType)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = [#project#].ProjectID()
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID)))

UPDATE [#project#].[CacheMetadata] SET LicensesDetails  =   (SELECT MIN(L.LicenseDetails)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = [#project#].ProjectID()
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID)))

UPDATE [#project#].[CacheMetadata] SET LicenseURI  =   (SELECT MIN(L.LicenseURI)
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
  WHERE L.ProjectID = [#project#].ProjectID()
  AND EXISTS (SELECT MIN(F.LicenseID) FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID))); 
')

-- StableIdentifier - optional
set @SQL = (@SQL + '
begin try
UPDATE [#project#].[CacheMetadata] SET StableIdentifier = (SELECT ' + dbo.ProjectsDatabase() + '.dbo.StableIdentifier([#project#].ProjectID()));
end try
begin catch
end catch
')

set @SQL = (@SQL + '
UPDATE [#project#].[CacheMetadata] SET TaxonomicGroup  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TaxonomicGroup'')
UPDATE [#project#].[CacheMetadata] SET DatasetGUID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetGUID'')
UPDATE [#project#].[CacheMetadata] SET TechnicalContactName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactName'')
UPDATE [#project#].[CacheMetadata] SET TechnicalContactEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactEmail'')
UPDATE [#project#].[CacheMetadata] SET TechnicalContactPhone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactPhone'')
UPDATE [#project#].[CacheMetadata] SET TechnicalContactAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TechnicalContactAddress'')
UPDATE [#project#].[CacheMetadata] SET ContentContactName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactName'')
UPDATE [#project#].[CacheMetadata] SET ContentContactEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactEmail'')
UPDATE [#project#].[CacheMetadata] SET ContentContactPhone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactPhone'')
UPDATE [#project#].[CacheMetadata] SET ContentContactAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''ContentContactAddress'')
UPDATE [#project#].[CacheMetadata] SET OtherProviderUDDI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OtherProviderUDDI'')
UPDATE [#project#].[CacheMetadata] SET DatasetTitle  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetTitle'')
UPDATE [#project#].[CacheMetadata] SET DatasetDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetDetails'')
UPDATE [#project#].[CacheMetadata] SET DatasetCoverage  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetCoverage'')
UPDATE [#project#].[CacheMetadata] SET DatasetURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetURI'')
UPDATE [#project#].[CacheMetadata] SET DatasetIconURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetIconURI'')
UPDATE [#project#].[CacheMetadata] SET DatasetVersionMajor  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetVersionMajor'')
UPDATE [#project#].[CacheMetadata] SET DatasetCreators  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetCreators'')
')

set @SQL = (@SQL + '
UPDATE [#project#].[CacheMetadata] SET DatasetContributors  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DatasetContributors'')
UPDATE [#project#].[CacheMetadata] SET DateCreated  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DateCreated'')
UPDATE [#project#].[CacheMetadata] SET DateModified  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DateModified'')
UPDATE [#project#].[CacheMetadata] SET SourceID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''SourceID'')
UPDATE [#project#].[CacheMetadata] SET SourceInstitutionID  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''SourceInstitutionID'')
UPDATE [#project#].[CacheMetadata] SET OwnerOrganizationName  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerOrganizationName'')
UPDATE [#project#].[CacheMetadata] SET OwnerOrganizationAbbrev  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerOrganizationAbbrev'')
UPDATE [#project#].[CacheMetadata] SET OwnerContactPerson  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerContactPerson'')
UPDATE [#project#].[CacheMetadata] SET OwnerContactRole  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerContactRole'')
UPDATE [#project#].[CacheMetadata] SET OwnerAddress  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerAddress'')
UPDATE [#project#].[CacheMetadata] SET OwnerTelephone  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerTelephone'')
UPDATE [#project#].[CacheMetadata] SET OwnerEmail  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerEmail'')
UPDATE [#project#].[CacheMetadata] SET OwnerURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerURI'')
UPDATE [#project#].[CacheMetadata] SET OwnerLogoURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''OwnerLogoURI'')
UPDATE [#project#].[CacheMetadata] SET IPRText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRText'')
UPDATE [#project#].[CacheMetadata] SET IPRDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRDetails'')
UPDATE [#project#].[CacheMetadata] SET IPRURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''IPRURI'')
UPDATE [#project#].[CacheMetadata] SET CopyrightText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightText'')
')

set @SQL = (@SQL + '
UPDATE [#project#].[CacheMetadata] SET CopyrightDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightDetails'')
UPDATE [#project#].[CacheMetadata] SET CopyrightURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CopyrightURI'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseText'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseDetails'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseURI'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersText'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersDetails'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersURI'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsText'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsDetails'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsURI'')
UPDATE [#project#].[CacheMetadata] SET CitationsText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsText'')
UPDATE [#project#].[CacheMetadata] SET CitationsDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsDetails'')
UPDATE [#project#].[CacheMetadata] SET CitationsURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''CitationsURI'')
UPDATE [#project#].[CacheMetadata] SET RecordBasis  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''RecordBasis'')
UPDATE [#project#].[CacheMetadata] SET KindOfUnit  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''KindOfUnit'')
UPDATE [#project#].[CacheMetadata] SET HigherTaxonRank  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''HigherTaxonRank'')
UPDATE [#project#].[CacheMetadata] SET RecordURI = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''RecordURI'')
')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch


GO


--#####################################################################################################################
--######   CacheProjectAgent   ########################################################################################
--#####################################################################################################################

CREATE TABLE [#project#].[CacheProjectAgent](
	[ProjectID] [int] NOT NULL,
	[AgentName] [nvarchar](255) NOT NULL,
	[AgentURI] [varchar](255) NOT NULL,
	[AgentRole] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[AgentSequence] [int] NULL,
 CONSTRAINT [PK_CacheProjectAgent] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[AgentName] ASC,
	[AgentURI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO



--#####################################################################################################################
--######   procPublishProjectAgent  ###################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishProjectAgent] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProjectAgent]
*/
truncate table [#project#].CacheProjectAgent

INSERT INTO [#project#].[CacheProjectAgent]
           ([ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole]
      ,[AgentType]
      ,[Notes]
      ,[AgentSequence])
SELECT DISTINCT [ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole]
      ,[AgentType]
      ,[Notes]
      ,[AgentSequence]
  FROM ' + dbo.ProjectsDatabase() + '.DBO.ProjectAgent A
  where A.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



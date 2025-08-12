
--#####################################################################################################################
--######   procPublishMetadata - DatasetDetails from PublicDescription, Agents from DA etc. via ProjectAgentID   ######
--######   and join with CacheCollectionProject and OwnerContactRole only for valid entry   ###########################
--######   ignore Stable identifier if missing  #######################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishMetadata] 
AS
/*
EXECUTE  [#project#].[procPublishMetadata] 
*/
truncate table  [#project#].[CacheMetadata]
-- insert IDs
insert into [#project#].[CacheMetadata] (ProjectID) 
select distinct ProjectID from [#project#].CacheCollectionProject  --values ([#project#].ProjectID())

-- Data from table Project

UPDATE M SET M.ProjectTitleCode =  P.Project, M.ProjectTitle = P.ProjectTitle, 
	M.DatasetTitle = P.ProjectTitle, M.DatasetDetails = P.PublicDescription, M.DatasetURI = P.ProjectURL
FROM [#project#].[CacheMetadata] M INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.Project P ON P.ProjectID = M.ProjectID

UPDATE M SET M.DateModified = convert(varchar(19), P.LastUpdatedWhen, 126)
FROM [#project#].[CacheMetadata] M
INNER JOIN [dbo].ProjectPublished P ON M.ProjectID = P.ProjectID
')

set @SQL = (@SQL + '
-- Data from table ProjectLicense

UPDATE M SET M.LicenseText =  P.LicenseType, M.LicensesDetails = P.LicenseDetails, 
	M.LicenseURI = P.LicenseURI, CopyrightText = CopyrightHolder, CopyrightURI = CopyrightStatement
FROM [#project#].[CacheMetadata] M INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense P ON P.ProjectID = M.ProjectID
AND EXISTS(SELECT L.ProjectID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L GROUP BY L.ProjectID 
	HAVING MIN(L.LicenseID) = P.LicenseID)
	')

set @SQL = (@SQL + '
-- Data from DiversityAgents
-- Technical contact

UPDATE M SET M.TechnicalContactName =  A.AgentName
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Technical contact''

UPDATE M SET M.TechnicalContactEmail = C.Email
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation C 
	ON (C.DataWithholdingReason = '''' OR C.DataWithholdingReason IS NULL)
	AND A.AgentID = C.AgentID
	AND EXISTS (SELECT L.AgentID FROM ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation L 
	WHERE L.AgentID = C.AgentID 
	GROUP BY L.AgentID 
	HAVING MIN(L.DisplayOrder) = C.DisplayOrder)
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Technical contact''
')

set @SQL = (@SQL + '
-- Content contact

UPDATE M SET M.ContentContactName =  A.AgentName
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Content contact''

UPDATE M SET M.ContentContactEmail = C.Email
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation C 
	ON (C.DataWithholdingReason = '''' OR C.DataWithholdingReason IS NULL)
	AND A.AgentID = C.AgentID
	AND EXISTS (SELECT L.AgentID FROM ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation L 
	WHERE L.AgentID = C.AgentID 
	GROUP BY L.AgentID 
	HAVING MIN(L.DisplayOrder) = C.DisplayOrder)
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Content contact''
')


set @SQL = (@SQL + '-- Data Owner

UPDATE M SET M.OwnerOrganizationName =  A.AgentName, M.OwnerOrganizationAbbrev =  A.Abbreviation
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Data Owner''


UPDATE M SET M.OwnerURI = C.URI
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation C 
	ON (C.DataWithholdingReason = '''' OR C.DataWithholdingReason IS NULL)
	AND A.AgentID = C.AgentID
	AND EXISTS (SELECT L.AgentID FROM ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation L 
	WHERE L.AgentID = C.AgentID 
	GROUP BY L.AgentID 
	HAVING MIN(L.DisplayOrder) = C.DisplayOrder)
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Data Owner''

UPDATE M SET M.OwnerLogoURI = I.URI
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.AgentImage I
	ON (I.DataWithholdingReason = '''' OR I.DataWithholdingReason IS NULL)
	AND A.AgentID = I.AgentID
	AND I.Type = ''Logo''
	AND EXISTS (SELECT L.AgentID FROM ' + dbo.AgentsDatabase() + '.dbo.AgentImage L 
	WHERE L.AgentID = I.AgentID 
	AND L.Type = ''Logo''
	GROUP BY L.AgentID 
	HAVING MIN(L.Sequence) = I.Sequence)
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Data Owner''
')


set @SQL = (@SQL + '-- Data Owner Contact

UPDATE M SET M.OwnerContactPerson =  A.GivenName + '' '' + A.InheritedName
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Data Owner Contact''

UPDATE [#project#].[CacheMetadata] SET OwnerContactRole = ''Data Owner Contact''
	WHERE OwnerContactPerson <> ''''


UPDATE M SET M.OwnerEmail = C.Email
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation C 
	ON (C.DataWithholdingReason = '''' OR C.DataWithholdingReason IS NULL)
	AND A.AgentID = C.AgentID
	AND EXISTS (SELECT L.AgentID FROM ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation L 
	WHERE L.AgentID = C.AgentID 
	GROUP BY L.AgentID 
	HAVING MIN(L.DisplayOrder) = C.DisplayOrder)
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Data Owner Contact''

')


set @SQL = (@SQL + '
-- Source institution

UPDATE M SET M.SourceInstitutionID =  CASE WHEN A.Abbreviation IS NULL OR RTRIM(A.Abbreviation) = '''' THEN A.AgentName ELSE A.Abbreviation END
FROM [#project#].[CacheMetadata] M 
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P 
	ON M.ProjectID = P.ProjectID
	AND EXISTS (SELECT L.ProjectAgentID FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent L 
	WHERE L.ProjectAgentID = P.ProjectAgentID 
	AND L.ProjectID = P.ProjectID 
	GROUP BY L.ProjectAgentID 
	HAVING MIN(L.AgentSequence) = P.AgentSequence)
INNER JOIN ' + dbo.AgentsDatabase() + '.dbo.Agent A 
	ON (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
	AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
INNER JOIN ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R 
	ON R.ProjectID = P.ProjectID 
	AND P.ProjectAgentID = R.ProjectAgentID
	AND R.AgentRole = ''Source institution''
')

set @SQL = (@SQL + '
-- Settings


declare  @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectSetting] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL);

declare @ProjectList TABLE([ProjectID] [int] Primary key);

INSERT @ProjectList (ProjectID) SELECT ProjectID FROM [#project#].[CacheMetadata];

declare @ProjectID int;

while (select count(*) from @ProjectList) > 0
begin

set @ProjectID = (SELECT MIN(ProjectID) FROM @ProjectList)

delete from @SettingList;

INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
	SELECT SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, 
	case when isdate(value) = 1 and ProjectSetting like ''%Date%'' then convert(varchar(20), cast(value as datetime), 126) else Value end as Value 
	FROM ' + dbo.ProjectsDatabase() + '.DBO.SettingsForProject(@ProjectID, ''ABCD | %'', ''.'', 2)

UPDATE [#project#].[CacheMetadata] SET RecordURI = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''RecordURI'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET TermsOfUseText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseText'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET TermsOfUseDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseDetails'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET TermsOfUseURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseURI'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET DisclaimersText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersText'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET DisclaimersDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersDetails'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET DisclaimersURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersURI'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsText'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsDetails'') WHERE ProjectID = @ProjectID
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsURI'') WHERE ProjectID = @ProjectID

begin try
UPDATE M SET StableIdentifier 
= (SELECT ' + dbo.ProjectsDatabase() + '.dbo.StableIdentifier(@ProjectID))
FROM [#project#].[CacheMetadata] M
WHERE M.ProjectID = @ProjectID;
end try
begin catch
end catch

delete from @ProjectList where ProjectID = @ProjectID

end
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
--######   procPublishCollectionProject redesigned  ###################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionProject] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionProject] 
*/
truncate table  [#project#].[CacheCollectionProject]
-- insert Data
insert into [#project#].[CacheCollectionProject] (ProjectID, CollectionSpecimenID) 
  SELECT P.ProjectID, P.CollectionSpecimenID 
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionProject] P
  inner join [#project#].[CacheCollectionSpecimen] S
  on P.CollectionSpecimenID = S.CollectionSpecimenID
  Order by P.CollectionSpecimenID

')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].procPublishCollectionProject TO [CacheAdmin] 
GO

--#####################################################################################################################
--######   procPublishCollectionSpecimen - inclusion of column Restriction in table ProjectPublished  #################
--#####################################################################################################################

ALTER  PROCEDURE [#project#].[procPublishCollectionSpecimen] 
AS
TRUNCATE TABLE [#project#].CacheCollectionSpecimen

DECLARE @InsertCommand nvarchar(max)
DECLARE @WhereClause nvarchar(max)
SET @WhereClause = (SELECT case when [Restriction] is null then '' else [Restriction] end FROM [dbo].[ProjectPublished] WHERE ProjectID = [#project#].ProjectID())

IF @WhereClause <> ''
BEGIN
   SET @WhereClause = (SELECT REPLACE(@WhereClause, 'WHERE CollectionSpecimenID IN', 'WHERE S.CollectionSpecimenID IN'))
   SET @WhereClause = (SELECT REPLACE(@WhereClause, ' CollectionSpecimen_Core2 ', ' [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimen] '))
   SET @WhereClause = (SELECT REPLACE(@WhereClause, ' CollectionProject ', ' [' +  dbo.SourceDatabase() + '].[dbo].[CollectionProject] '))
END

SET @InsertCommand = (SELECT '
INSERT INTO [#project#].CacheCollectionSpecimen
      ([CollectionSpecimenID]
      ,[LabelTranscriptionNotes]
      ,[OriginalNotes]
      ,[LogUpdatedWhen]
      ,[CollectionEventID]
      ,[AccessionNumber]
      ,[AccessionDate]
      ,[AccessionDay]
      ,[AccessionMonth]
      ,[AccessionYear]
      ,[DepositorsName]
      ,[DepositorsAccessionNumber]
      ,[ExsiccataURI]
      ,[ExsiccataAbbreviation]
      ,[AdditionalNotes]
      ,[ReferenceTitle]
      ,[ReferenceURI]
	  ,[ExternalDatasourceID])
SELECT DISTINCT S.[CollectionSpecimenID]
      ,[LabelTranscriptionNotes]
      ,[OriginalNotes]
      ,S.[LogUpdatedWhen]
      ,[CollectionEventID]
      ,[AccessionNumber]
      ,[AccessionDate]
      ,[AccessionDay]
      ,[AccessionMonth]
      ,[AccessionYear]
      ,[DepositorsName]
      ,[DepositorsAccessionNumber]
      ,[ExsiccataURI]
      ,[ExsiccataAbbreviation]
      ,[AdditionalNotes]
      ,[ReferenceTitle]
      ,[ReferenceURI]
	  ,[ExternalDatasourceID]
  FROM [dbo].[ViewCollectionSpecimen] S 
  inner join dbo.ViewCollectionProject P
  on s.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID() ' + @WhereClause)

exec sp_executesql @InsertCommand

GO




--#####################################################################################################################
--######   New table CacheCollectionSpecimenPartDescription  ##########################################################
--#####################################################################################################################

CREATE TABLE [#project#].[CacheCollectionSpecimenPartDescription](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenPartID] [int] NOT NULL,
	[PartDescriptionID] [int] NOT NULL,
	[IdentificationUnitID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionTermURI] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[DescriptionHierarchyCache] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheCollectionSpecimenPartDescription] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenPartID] ASC,
	[PartDescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO

--#####################################################################################################################
--######   PROCEDURE procPublishCollectionSpecimenPartDescription  ####################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionSpecimenPartDescription] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenPartDescription] 
*/
truncate table  [#project#].[CacheCollectionSpecimenPartDescription]
-- insert Data
insert into [#project#].[CacheCollectionSpecimenPartDescription] (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, IdentificationUnitID, Description, DescriptionTermURI, Notes, DescriptionHierarchyCache) 
  SELECT D.CollectionSpecimenID, D.SpecimenPartID, D.PartDescriptionID, D.IdentificationUnitID, D.Description, D.DescriptionTermURI, D.Notes, D.DescriptionHierarchyCache 
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimenPartDescription] D, [#project#].[CacheCollectionSpecimenPart] P
  where D.SpecimenPartID = P.SpecimenPartID
  and P.CollectionSpecimenID = D.CollectionSpecimenID

')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].procPublishCollectionSpecimenPartDescription TO [CacheAdmin] 
GO


--#####################################################################################################################
--######   New table CacheCollectionSpecimenProcessingMethod  #########################################################
--#####################################################################################################################

CREATE TABLE [#project#].[CacheCollectionSpecimenProcessingMethod](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenProcessingID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[MethodMarker] [nvarchar](50) NOT NULL,
	[ProcessingID] [int] NOT NULL,
 CONSTRAINT [PK_CacheCollectionSpecimenProcessingMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenProcessingID] ASC,
	[MethodID] ASC,
	[ProcessingID] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



--#####################################################################################################################
--######   PROCEDURE procPublishCollectionSpecimenProcessingMethod  ###################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionSpecimenProcessingMethod] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenProcessingMethod] 
*/
truncate table  [#project#].[CacheCollectionSpecimenProcessingMethod]
-- insert Data
insert into [#project#].[CacheCollectionSpecimenProcessingMethod] (CollectionSpecimenID, SpecimenProcessingID, MethodID, MethodMarker, ProcessingID) 
  SELECT M.CollectionSpecimenID, M.SpecimenProcessingID, M.MethodID, M.MethodMarker, M.ProcessingID 
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimenProcessingMethod] M, [#project#].[CacheCollectionSpecimenProcessing] P
  where M.SpecimenProcessingID = P.SpecimenProcessingID
  and P.CollectionSpecimenID = M.CollectionSpecimenID

')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].procPublishCollectionSpecimenProcessingMethod TO [CacheAdmin] 
GO



--#####################################################################################################################
--######   New table CacheCollectionSpecimenProcessingMethodParameter  ################################################
--#####################################################################################################################

CREATE TABLE [#project#].[CacheCollectionSpecimenProcessingMethodParameter](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenProcessingID] [int] NOT NULL,
	[ProcessingID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[MethodMarker] [nvarchar](50) NOT NULL,
	[ParameterID] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CacheCollectionSpecimenProcessingMethodParameterParameter] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenProcessingID] ASC,
	[ProcessingID] ASC,
	[MethodID] ASC,
	[ParameterID] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO


--#####################################################################################################################
--######   PROCEDURE procPublishCollectionSpecimenProcessingMethodParameter  ##########################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionSpecimenProcessingMethodParameter] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenProcessingMethodParameter] 
*/
truncate table  [#project#].[CacheCollectionSpecimenProcessingMethodParameter]
-- insert Data
insert into [#project#].[CacheCollectionSpecimenProcessingMethodParameter] (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, MethodMarker, ParameterID, Value) 
  SELECT P.CollectionSpecimenID, P.SpecimenProcessingID, P.ProcessingID, P.MethodID, P.MethodMarker, P.ParameterID, P.Value 
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionSpecimenProcessingMethodParameter] P
  inner join [#project#].[CacheCollectionSpecimenProcessingMethod] M
  on M.SpecimenProcessingID = P.SpecimenProcessingID
  and P.CollectionSpecimenID = M.CollectionSpecimenID
  and P.MethodID = M.MethodID
  and P.MethodMarker = M.MethodMarker
  and P.ProcessingID = M.ProcessingID

')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].procPublishCollectionSpecimenProcessingMethodParameter TO [CacheAdmin] 
GO




--#####################################################################################################################
--######   New table CacheIdentificationUnitAnalysisMethod  ###########################################################
--#####################################################################################################################

CREATE TABLE [#project#].[CacheIdentificationUnitAnalysisMethod](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[AnalysisID] [int] NOT NULL,
	[AnalysisNumber] [nvarchar](50) NOT NULL,
	[MethodMarker] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CacheIdentificationUnitAnalysisMethod] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[MethodID] ASC,
	[AnalysisID] ASC,
	[AnalysisNumber] ASC,
	[MethodMarker] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO



--#####################################################################################################################
--######   PROCEDURE procPublishIdentificationUnitAnalysisMethod  #####################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishIdentificationUnitAnalysisMethod] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnitAnalysisMethod] 
*/
truncate table  [#project#].[CacheIdentificationUnitAnalysisMethod]
-- insert Data
insert into [#project#].[CacheIdentificationUnitAnalysisMethod] (CollectionSpecimenID, IdentificationUnitID, MethodID, MethodMarker, AnalysisID, AnalysisNumber) 
  SELECT P.CollectionSpecimenID, P.IdentificationUnitID, P.MethodID, P.MethodMarker, P.AnalysisID, P.AnalysisNumber 
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[IdentificationUnitAnalysisMethod] P
  inner join [#project#].[CacheIdentificationUnitAnalysis] M
  on M.IdentificationUnitID = P.IdentificationUnitID
  and P.CollectionSpecimenID = M.CollectionSpecimenID
  and P.AnalysisID = M.AnalysisID
  and P.AnalysisNumber = M.AnalysisNumber
')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].procPublishIdentificationUnitAnalysisMethod TO [CacheAdmin] 
GO


--#####################################################################################################################
--######   New table CacheIdentificationUnitAnalysisMethodParameter  ##################################################
--#####################################################################################################################

CREATE TABLE [#project#].[CacheIdentificationUnitAnalysisMethodParameter](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[AnalysisID] [int] NOT NULL,
	[AnalysisNumber] [nvarchar](50) NOT NULL,
	[MethodID] [int] NOT NULL,
	[MethodMarker] [nvarchar](50) NOT NULL,
	[ParameterID] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheIdentificationUnitAnalysisMethodParameter] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[AnalysisID] ASC,
	[AnalysisNumber] ASC,
	[MethodID] ASC,
	[MethodMarker] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO



--#####################################################################################################################
--######   PROCEDURE procPublishIdentificationUnitAnalysisMethodParameter  ############################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishIdentificationUnitAnalysisMethodParameter] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnitAnalysisMethodParameter] 
*/
truncate table  [#project#].[CacheIdentificationUnitAnalysisMethodParameter]
-- insert Data
insert into [#project#].[CacheIdentificationUnitAnalysisMethodParameter] (CollectionSpecimenID, IdentificationUnitID, MethodID, MethodMarker, AnalysisID, AnalysisNumber, ParameterID, Value) 
  SELECT P.CollectionSpecimenID, P.IdentificationUnitID, P.MethodID, P.MethodMarker, P.AnalysisID, P.AnalysisNumber, P.ParameterID, P.Value
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[IdentificationUnitAnalysisMethodParameter] P
  inner join [#project#].[CacheIdentificationUnitAnalysisMethod] M
  on M.IdentificationUnitID = P.IdentificationUnitID
  and P.CollectionSpecimenID = M.CollectionSpecimenID
  and P.MethodID = M.MethodID
  and P.MethodMarker = M.MethodMarker
  and P.AnalysisID = M.AnalysisID
  and P.AnalysisNumber = M.AnalysisNumber
')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].procPublishIdentificationUnitAnalysisMethod TO [CacheAdmin] 
GO


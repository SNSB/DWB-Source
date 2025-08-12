
--#####################################################################################################################
--######   New table CacheCollectionProject  ##########################################################################
--#####################################################################################################################

CREATE TABLE [#project#].[CacheCollectionProject](
	[ProjectID] [int] NOT NULL,
	[CollectionSpecimenID] [int] NOT NULL,
 CONSTRAINT [PK_CacheCollectionProject] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[ProjectID] ASC
)
)
GO

GRANT SELECT ON [#project#].[CacheCollectionProject] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheCollectionProject] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheCollectionProject] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheCollectionProject] TO [CacheAdmin_#project#]
GO
 

--#####################################################################################################################
--######   procPublishCollectionProject   #############################################################################
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
  FROM [' +  dbo.SourceDatabase() + '].[dbo].[CollectionProject] P, [' +  dbo.SourceDatabase() + '].[dbo].[CollectionProject] O
  where O.ProjectID = [#project#].ProjectID()
  and P.CollectionSpecimenID = O.CollectionSpecimenID
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
--######   ProjectAgent - add ProjectAgentID  #########################################################################
--#####################################################################################################################

TRUNCATE TABLE [#project#].[CacheProjectAgent];
GO

ALTER TABLE [#project#].[CacheProjectAgent] ADD [ProjectAgentID] int NOT NULL;
GO

--#####################################################################################################################
--######   ProjectAgent - change PK to ProjectAgentID  ################################################################
--#####################################################################################################################

ALTER TABLE [#project#].[CacheProjectAgent] DROP CONSTRAINT [PK_CacheProjectAgent] --WITH ( ONLINE = OFF )
GO

ALTER TABLE [#project#].[CacheProjectAgent] ADD  CONSTRAINT [PK_CacheProjectAgent] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ProjectAgentID] ASC
)--WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--#####################################################################################################################
--######   procPublishProjectAgent - include ProjectAgentID  ##########################################################
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
      ,[AgentSequence]
	  ,[ProjectAgentID])
SELECT DISTINCT [ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole]
      ,[AgentType]
      ,[Notes]
      ,[AgentSequence]
	  ,[ProjectAgentID]
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



--#####################################################################################################################
--######   ProjectAgentRole - add ProjectAgentID  #####################################################################
--#####################################################################################################################

TRUNCATE TABLE [#project#].[CacheProjectAgentRole];
GO

ALTER TABLE [#project#].[CacheProjectAgentRole] ADD [ProjectAgentID] int NOT NULL;
GO

--#####################################################################################################################
--######   ProjectAgentRole - change PK to ProjectAgentID  ############################################################
--#####################################################################################################################

ALTER TABLE [#project#].[CacheProjectAgentRole] DROP CONSTRAINT [PK_ProjectAgentRole] --WITH ( ONLINE = OFF )
GO

ALTER TABLE [#project#].[CacheProjectAgentRole] ADD  CONSTRAINT [PK_ProjectAgentRole] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ProjectAgentID] ASC,
	[AgentRole] ASC
) --WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


--#####################################################################################################################
--######   procPublishProjectAgentRole - include ProjectAgentID  ######################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishProjectAgentRole] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProjectAgentRole]
*/
truncate table [#project#].CacheProjectAgentRole

INSERT INTO [#project#].[CacheProjectAgentRole]
           ([ProjectID]
      ,[AgentName]
      ,[AgentURI]
      ,[AgentRole]
	  ,[ProjectAgentID])
SELECT DISTINCT       
A.ProjectID, 
A.AgentName, 
A.AgentURI,
R.AgentRole,
R.[ProjectAgentID]
FROM  ' + dbo.ProjectsDatabase() + '.DBO.ProjectAgentRole AS R 
INNER JOIN ' + dbo.ProjectsDatabase() + '.DBO.ProjectAgent AS A ON R.ProjectAgentID = A.ProjectAgentID
WHERE A.ProjectID = [#project#].ProjectID()
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   procPublishMetadata - DatasetDetails from PublicDescription, Agents from DA etc. via ProjectAgentID   ######
--######   and join with CacheCollectionProject and OwnerContactRole only for valid entry   ###########################
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

UPDATE M SET StableIdentifier 
= (SELECT ' + dbo.ProjectsDatabase() + '.dbo.StableIdentifier(@ProjectID))
FROM [#project#].[CacheMetadata] M
WHERE M.ProjectID = @ProjectID;

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
--######   procPublishCount - include CacheCollectionProject  #########################################################
--#####################################################################################################################

ALTER PROCEDURE [#project#].[procPublishCount] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCount]
-- CREATION
SELECT 'INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT '' + T.TABLE_NAME + '', COUNT(*) FROM [#project#].['  + T.TABLE_NAME + '];'  
FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_SCHEMA = '#project#' AND T.TABLE_TYPE = 'BASE TABLE' ORDER BY T.TABLE_NAME
*/
truncate table [#project#].[CacheCount]

INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheAnalysis', COUNT(*) FROM [#project#].[CacheAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheAnnotation', COUNT(*) FROM [#project#].[CacheAnnotation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollection', COUNT(*) FROM [#project#].[CacheCollection];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionAgent', COUNT(*) FROM [#project#].[CacheCollectionAgent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEvent', COUNT(*) FROM [#project#].[CacheCollectionEvent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEventLocalisation', COUNT(*) FROM [#project#].[CacheCollectionEventLocalisation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEventMethod', COUNT(*) FROM [#project#].[CacheCollectionEventMethod];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEventParameterValue', COUNT(*) FROM [#project#].[CacheCollectionEventParameterValue];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionEventProperty', COUNT(*) FROM [#project#].[CacheCollectionEventProperty];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionExternalDatasource', COUNT(*) FROM [#project#].[CacheCollectionExternalDatasource];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimen', COUNT(*) FROM [#project#].[CacheCollectionSpecimen];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionProject', COUNT(*) FROM [#project#].[CacheCollectionProject];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenImage', COUNT(*) FROM [#project#].[CacheCollectionSpecimenImage];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenPart', COUNT(*) FROM [#project#].[CacheCollectionSpecimenPart];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenProcessing', COUNT(*) FROM [#project#].[CacheCollectionSpecimenProcessing];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenReference', COUNT(*) FROM [#project#].[CacheCollectionSpecimenReference];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCollectionSpecimenRelation', COUNT(*) FROM [#project#].[CacheCollectionSpecimenRelation];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheExternalIdentifier', COUNT(*) FROM [#project#].[CacheExternalIdentifier];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentification', COUNT(*) FROM [#project#].[CacheIdentification];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentificationUnit', COUNT(*) FROM [#project#].[CacheIdentificationUnit];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentificationUnitAnalysis', COUNT(*) FROM [#project#].[CacheIdentificationUnitAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentificationUnitGeoAnalysis', COUNT(*) FROM [#project#].[CacheIdentificationUnitGeoAnalysis];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheIdentificationUnitInPart', COUNT(*) FROM [#project#].[CacheIdentificationUnitInPart];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheLocalisationSystem', COUNT(*) FROM [#project#].[CacheLocalisationSystem];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheMetadata', COUNT(*) FROM [#project#].[CacheMetadata];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheMethod', COUNT(*) FROM [#project#].[CacheMethod];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheProcessing', COUNT(*) FROM [#project#].[CacheProcessing];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheProjectAgent', COUNT(*) FROM [#project#].[CacheProjectAgent];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheProjectReference', COUNT(*) FROM [#project#].[CacheProjectReference];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'ProjectMaterialCategory', COUNT(*) FROM [#project#].[ProjectMaterialCategory];
INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'ProjectTaxonomicGroup', COUNT(*) FROM [#project#].[ProjectTaxonomicGroup];

INSERT INTO [#project#].[CacheCount] (Tablename, TotalCount) SELECT 'CacheCount', COUNT(*) FROM [#project#].[CacheCount];

GO



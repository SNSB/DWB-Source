
--#####################################################################################################################
--######   procPublishCollection - Getting the CollectionAcronym  #####################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollection] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollection]
*/
truncate table [#project#].CacheCollection

-- Collection
INSERT INTO [#project#].[CacheCollection]
(CollectionID, CollectionParentID, CollectionName, CollectionAcronym, 
AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, 
CollectionOwner, DisplayOrder)
SELECT    DISTINCT    
C.CollectionID, CollectionParentID, CollectionName, CollectionAcronym, 
AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, 
CollectionOwner, DisplayOrder
FROM  ' +  dbo.SourceDatabase() + '.dbo.Collection C, 
[#project#].CacheCollectionSpecimenPart AS P
WHERE P.CollectionID = C.CollectionID

-- Getting the CollectionAcronym
IF (SELECT COUNT(*) FROM [#project#].[CacheCollection] CC WHERE CC.CollectionAcronym IS NULL) > 0
BEGIN	
	DECLARE @I int;
	SET @I = (SELECT COUNT(*) FROM [#project#].[CacheCollection] CC WHERE CC.CollectionAcronym IS NULL)
	DECLARE @ID int;
	WHILE @I > 0
	BEGIN
		SET @ID = (SELECT TOP 1 CC.CollectionID FROM [#project#].[CacheCollection] CC WHERE CC.CollectionAcronym IS NULL)
		UPDATE CC SET CC.CollectionAcronym = (SELECT TOP 1 H.CollectionAcronym 
			FROM ' +  dbo.SourceDatabase() + '.[dbo].[CollectionHierarchy](@ID) H 
			WHERE H.CollectionAcronym <> '''' ORDER BY H.CollectionID)
		FROM [#project#].[CacheCollection] CC  WHERE CC.CollectionID = @ID
		UPDATE CC SET CC.CollectionAcronym = ''''
		FROM [#project#].[CacheCollection] CC WHERE CC.CollectionID = @ID AND CC.CollectionAcronym IS NULL
		SET @I = (SELECT COUNT(*) FROM [#project#].[CacheCollection] CC WHERE CC.CollectionAcronym IS NULL)
	END
END
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
--######   procPublishCollectionEventLocalisation - reducing quadrant precision according to settings  ################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionEventLocalisation] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEventLocalisation]
*/
truncate table [#project#].CacheCollectionEventLocalisation

INSERT INTO [#project#].[CacheCollectionEventLocalisation]
           ([CollectionEventID]
      ,[LocalisationSystemID]
      ,[Location1]
      ,[Location2]
      ,[LocationAccuracy]
      ,[LocationNotes]
      ,[DeterminationDate]
      ,[DistanceToLocation]
      ,[DirectionToLocation]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AverageAltitudeCache]
      ,[AverageLatitudeCache]
      ,[AverageLongitudeCache]
      ,[RecordingMethod]
	  ,[Geography])
SELECT DISTINCT L.[CollectionEventID]
      ,L.[LocalisationSystemID]
      ,[Location1]
      ,[Location2]
      ,[LocationAccuracy]
      ,[LocationNotes]
      ,[DeterminationDate]
      ,[DistanceToLocation]
      ,[DirectionToLocation]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AverageAltitudeCache]
      ,[AverageLatitudeCache]
      ,[AverageLongitudeCache]
      ,[RecordingMethod]
	  ,[Geography].ToString()
  FROM ' +  dbo.SourceDatabase() + '.[dbo].[CollectionEventLocalisation] L,
[#project#].[CacheCollectionEvent] E
  where E.CollectionEventID = L.CollectionEventID 

if not (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())  is null
begin
	declare @Precision int
	set @Precision = (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())

	-- rounding average values for all entries
	update L 
	set [AverageLatitudeCache] = round(L.[AverageLatitudeCache], @Precision ),
	[AverageLongitudeCache] = round([AverageLongitudeCache], @Precision)
	from [#project#].[CacheCollectionEventLocalisation] L

	-- rounding location values for coordinate entries
	update E 
	set [Location2] = round(E.[AverageLatitudeCache], @Precision ),
	[Location1] = round(E.[AverageLongitudeCache], @Precision)
	from [#project#].[CacheCollectionEventLocalisation] E,
	[#project#].[CacheLocalisationSystem] L
	WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
	AND L.ParsingMethodName = ''Coordinates''

	-- rounding location values for Gauss Krueger entries
	declare @GK int
	if @Precision < 5
	begin
		set @GK = 5 - @Precision
		update E 
		set [Location2] = substring([Location2], 1, 7 - @GK) + replicate(''0'', @GK)  ,
		[Location1] = substring([Location1], 1, 7 - @GK) + replicate(''0'', @GK)  
		from [#project#].[CacheCollectionEventLocalisation] E,
		[#project#].[CacheLocalisationSystem] L
		WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
		AND L.ParsingMethodName = ''GK''
	end

	-- rounding location values for TK25
	update E 
	set [Location2] = substring([Location2], 1, @Precision )
	from [#project#].[CacheCollectionEventLocalisation] E,
	[#project#].[CacheLocalisationSystem] L
	WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
	AND L.ParsingMethodName = ''MTB''

end
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
--######   CacheMetadata - DatasetDetails changed to nvarchar(MAX)  ##################################################
--#####################################################################################################################

ALTER TABLE [#project#].[CacheMetadata] ALTER COLUMN DatasetDetails nvarchar(MAX) NULL
GO

--#####################################################################################################################
--######   procPublishMetadata - DatasetDetails from PublicDescription, Agents from DA etc.   #########################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishMetadata] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishMetadata] 
*/
truncate table  [#project#].[CacheMetadata]
-- insert ID
insert into [#project#].[CacheMetadata] (ProjectID) values ([#project#].ProjectID())
-- Data from table Project
UPDATE [#project#].[CacheMetadata] SET ProjectTitleCode  = 
	(SELECT P.Project FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = [#project#].ProjectID())
UPDATE [#project#].[CacheMetadata] SET ProjectTitle  = 
	(SELECT P.ProjectTitle FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = [#project#].ProjectID())
UPDATE [#project#].[CacheMetadata] SET DatasetTitle  = 
	(SELECT P.ProjectTitle FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = [#project#].ProjectID())
begin try
UPDATE [#project#].[CacheMetadata] SET DatasetDetails  = 
	(SELECT P.PublicDescription FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = [#project#].ProjectID())
end try
begin catch
end catch
UPDATE [#project#].[CacheMetadata] SET DateModified = 
(SELECT convert(varchar(19), max(P.LastUpdatedWhen), 126) from [dbo].ProjectPublished P WHERE P.ProjectID = [#project#].ProjectID())
UPDATE [#project#].[CacheMetadata] SET DatasetURI = 
	(SELECT P.ProjectURL FROM ' + dbo.ProjectsDatabase() + '.dbo.Project P WHERE P.ProjectID = [#project#].ProjectID())
')

set @SQL = (@SQL + '
-- Data from table ProjectLicense
UPDATE [#project#].[CacheMetadata] SET LicenseText  =   
	(SELECT TOP 1 L.LicenseType
	FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
	WHERE L.ProjectID = [#project#].ProjectID()
	ORDER BY L.LicenseID)
UPDATE [#project#].[CacheMetadata] SET LicensesDetails  =   
	(SELECT TOP 1 L.LicenseDetails
	FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
	WHERE L.ProjectID = [#project#].ProjectID()
	ORDER BY L.LicenseID)
UPDATE [#project#].[CacheMetadata] SET LicenseURI  =   
	(SELECT TOP 1 L.LicenseURI
	FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
	WHERE L.ProjectID = [#project#].ProjectID()
	ORDER BY L.LicenseID); 
	')

set @SQL = (@SQL + '
-- Data from table ProjectCopyright
UPDATE [#project#].[CacheMetadata] SET CopyrightText  =   
	(SELECT TOP 1 CopyrightHolder
	FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
	WHERE L.ProjectID = [#project#].ProjectID()
	ORDER BY L.LicenseID)
UPDATE [#project#].[CacheMetadata] SET CopyrightURI  =   
	(SELECT TOP 1 CopyrightHolderAgentURI
	FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectLicense L
	WHERE L.ProjectID = [#project#].ProjectID()
	ORDER BY L.LicenseID); 
	') 

-- StableIdentifier - optional
set @SQL = (@SQL + '
-- Data from function StableIdentifier
begin try
UPDATE [#project#].[CacheMetadata] SET StableIdentifier = (SELECT ' + dbo.ProjectsDatabase() + '.dbo.StableIdentifier([#project#].ProjectID()));
end try
begin catch
end catch
')

set @SQL = (@SQL + '
-- Data from DiversityAgents
-- Technical contact
UPDATE [#project#].[CacheMetadata] SET TechnicalContactName  = (SELECT TOP 1 A.AgentName
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND R.AgentRole = ''Technical contact''
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence, P.ProjectAgentID)

UPDATE [#project#].[CacheMetadata] SET TechnicalContactEmail  = (SELECT TOP 1 C.Email
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R, 
  ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation C
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND A.AgentID = C.AgentID
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND (C.DataWithholdingReason = '''' OR C.DataWithholdingReason IS NULL)
  AND R.AgentRole = ''Technical contact''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY C.DisplayOrder, P.AgentSequence, P.ProjectAgentID)
')

set @SQL = (@SQL + '
-- Content contact
UPDATE [#project#].[CacheMetadata] SET ContentContactName  = (SELECT TOP 1 A.AgentName
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND R.AgentRole = ''Content contact''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence, P.ProjectAgentID)

UPDATE [#project#].[CacheMetadata] SET ContentContactEmail  = (SELECT TOP 1 C.Email
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R, 
  ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation C
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND A.AgentID = C.AgentID
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND (C.DataWithholdingReason = '''' OR C.DataWithholdingReason IS NULL)
  AND R.AgentRole = ''Content contact''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY C.DisplayOrder, P.AgentSequence, P.ProjectAgentID)
')


set @SQL = (@SQL + '-- Data Owner
UPDATE [#project#].[CacheMetadata] SET OwnerOrganizationName = (SELECT TOP 1 A.AgentName
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND R.AgentRole = ''Data Owner''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence)

UPDATE [#project#].[CacheMetadata] SET OwnerOrganizationAbbrev = (SELECT TOP 1 A.Abbreviation
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND R.AgentRole = ''Data Owner''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence)

UPDATE [#project#].[CacheMetadata] SET OwnerURI = (SELECT TOP 1 C.URI
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R, 
  ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation C
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND A.AgentID = C.AgentID
  AND R.AgentRole = ''Data Owner''
  AND P.AgentURI  COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence, C.DisplayOrder)

UPDATE [#project#].[CacheMetadata] SET OwnerLogoURI = (SELECT TOP 1 I.URI
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R, 
  ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.AgentsDatabase() + '.dbo.AgentImage I
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND (I.DataWithholdingReason = '''' OR I.DataWithholdingReason IS NULL)
  AND A.AgentID = I.AgentID
  AND I.Type = ''Logo''
  AND R.AgentRole = ''Data Owner''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence, I.Sequence)
')


set @SQL = (@SQL + '-- Data Owner Contact
UPDATE [#project#].[CacheMetadata] SET OwnerContactPerson = (SELECT TOP 1 A.GivenName + '' '' + A.InheritedName
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND R.AgentRole = ''Data Owner Contact''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence)

UPDATE [#project#].[CacheMetadata] SET OwnerContactRole = ''Data Owner Contact''

UPDATE [#project#].[CacheMetadata] SET OwnerEmail = (SELECT TOP 1 C.Email
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R, 
  ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.AgentsDatabase() + '.dbo.AgentContactInformation C
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND A.AgentID = C.AgentID
  AND (A.DataWithholdingReason = '''' OR A.DataWithholdingReason IS NULL)
  AND (C.DataWithholdingReason = '''' OR C.DataWithholdingReason IS NULL)
  AND R.AgentRole = ''Data Owner Contact''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence, C.DisplayOrder)
')

set @SQL = (@SQL + '
-- Source institution
UPDATE [#project#].[CacheMetadata] SET SourceInstitutionID  = 
(SELECT TOP 1 CASE WHEN A.Abbreviation IS NULL OR RTRIM(A.Abbreviation) = '''' THEN A.AgentName ELSE A.Abbreviation END
  FROM ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgent P, ' + dbo.AgentsDatabase() + '.dbo.Agent A, ' + dbo.ProjectsDatabase() + '.dbo.ProjectAgentRole R
  WHERE P.ProjectID = [#project#].ProjectID()
  AND P.ProjectID = R.ProjectID
  AND P.AgentName = R.AgentName
  AND P.AgentURI = R.AgentURI
  AND R.AgentRole = ''Source institution''
  AND P.AgentURI COLLATE DATABASE_DEFAULT = ' + dbo.AgentsDatabase() + '.dbo.BaseURL() + cast(A.AgentID as varchar) COLLATE DATABASE_DEFAULT
  ORDER BY P.AgentSequence)
')

set @SQL = (@SQL + '
-- Settings
declare  @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectSetting] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)

INSERT @SettingList (SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, Value)
	SELECT SettingID, ParentSettingID, DisplayText, Description, ProjectSetting, 
	case when isdate(value) = 1 and ProjectSetting like ''%Date%'' then convert(varchar(20), cast(value as datetime), 126) else Value end as Value 
	FROM ' + dbo.ProjectsDatabase() + '.DBO.SettingsForProject([#project#].ProjectID(), ''ABCD | %'', ''.'', 2)

UPDATE [#project#].[CacheMetadata] SET RecordURI = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''RecordURI'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseText'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseDetails'')
UPDATE [#project#].[CacheMetadata] SET TermsOfUseURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''TermsOfUseURI'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersText'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersDetails'')
UPDATE [#project#].[CacheMetadata] SET DisclaimersURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''DisclaimersURI'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsText  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsText'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsDetails  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsDetails'')
UPDATE [#project#].[CacheMetadata] SET AcknowledgementsURI  = (SELECT min(S.Value) FROM @SettingList S WHERE REPLACE(S.ProjectSetting, ''.'', '''') = ''AcknowledgementsURI'')
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
--######   ProjectTaxonomicGroup - Add RestrictToLinkedIdentifications  ###############################################
--#####################################################################################################################

ALTER TABLE [#project#].ProjectTaxonomicGroup ADD [RestrictToLinkedIdentifications] [bit] NULL
GO

ALTER TABLE [#project#].[ProjectTaxonomicGroup] ADD CONSTRAINT [DF_ProjectTaxonomicGroup_RestrictToLinkedIdentifications]  DEFAULT ((0)) FOR [RestrictToLinkedIdentifications]
GO

UPDATE T SET [RestrictToLinkedIdentifications] = 0
FROM [#project#].[ProjectTaxonomicGroup] AS T


--#####################################################################################################################
--######   procPublishIdentificationUnit - optional restriction to linked identifications  ############################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishIdentificationUnit] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnit]
*/
truncate table [#project#].CacheIdentificationUnit

-- insert basic data not linked to thesaurus
INSERT INTO [#project#].[CacheIdentificationUnit]
           ([CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[LastIdentificationCache]
      ,[TaxonomicGroup]
	  ,[DisplayOrder])
SELECT U.[CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,MAX([LastIdentificationCache])
      ,MAX(U.[TaxonomicGroup])
      ,MAX([DisplayOrder])
  FROM [' + dbo.SourceDatabase() + '].[dbo].[IdentificationUnit] U, 
  [' + dbo.SourceDatabase() + '].[dbo].[CollectionProject] CP, 
  [#project#].ProjectTaxonomicGroup T,
  [#project#].CacheCollectionSpecimen S
  where U.[CollectionSpecimenID] = CP.[CollectionSpecimenID]
  and U.[CollectionSpecimenID] = S.[CollectionSpecimenID]
  AND CP.[ProjectID] = [#project#].ProjectID()
  and T.TaxonomicGroup COLLATE DATABASE_DEFAULT = U.TaxonomicGroup COLLATE DATABASE_DEFAULT
  and (T.RestrictToLinkedIdentifications = 0 or T.RestrictToLinkedIdentifications is null)
  and  ((U.DataWithholdingReason IS NULL) OR (U.DataWithholdingReason = N''''))
  GROUP BY U.[CollectionSpecimenID],[IdentificationUnitID]')

  set @SQL = (@SQL + '
-- -- insert basic data linked to taxon thesaurus
INSERT INTO [#project#].[CacheIdentificationUnit]
           ([CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[LastIdentificationCache]
      ,[TaxonomicGroup]
	  ,[DisplayOrder])
SELECT U.[CollectionSpecimenID]
      ,U.[IdentificationUnitID]
      ,MAX([LastIdentificationCache])
      ,MAX(U.[TaxonomicGroup])
      ,MAX([DisplayOrder])
  FROM [' + dbo.SourceDatabase() + '].[dbo].[IdentificationUnit] U, 
  [' + dbo.SourceDatabase() + '].[dbo].[CollectionProject] CP, 
  [' + dbo.SourceDatabase() + '].[dbo].[Identification] I, 
  [#project#].ProjectTaxonomicGroup T,
  [#project#].CacheCollectionSpecimen S
  where U.[CollectionSpecimenID] = CP.[CollectionSpecimenID]
  and U.[CollectionSpecimenID] = S.[CollectionSpecimenID]
  AND CP.[ProjectID] = [#project#].ProjectID()
  and T.TaxonomicGroup COLLATE DATABASE_DEFAULT = U.TaxonomicGroup COLLATE DATABASE_DEFAULT
  and I.CollectionSpecimenID = U.CollectionSpecimenID
  and I.IdentificationUnitID = U.IdentificationUnitID
  and ((I.TaxonomicName = U.LastIdentificationCache and I.NameURI <> '''') 
  or (I.VernacularTerm = U.LastIdentificationCache and I.TermURI <> ''''))
  and T.RestrictToLinkedIdentifications = 1
  and  ((U.DataWithholdingReason IS NULL) OR (U.DataWithholdingReason = N''''))
  GROUP BY U.[CollectionSpecimenID], U.[IdentificationUnitID]')
  
set @SQL = (@SQL + '
-- setting the stable identifier
DECLARE @StableIdentifierBase nvarchar(255), @ProjectID int
set @ProjectID = (select [#project#].ProjectID())
  set @StableIdentifierBase = (select [' + dbo.SourceDatabase() + '].[dbo].StableIdentifier(@ProjectID, 0, NULL, NULL))
  set @StableIdentifierBase = (select substring(@StableIdentifierBase, 1, len(@StableIdentifierBase) - 1))
  select @StableIdentifierBase

UPDATE U SET U.StableIdentifier = @StableIdentifierBase + cast( U.CollectionSpecimenID as varchar) + ''/'' + cast( U.IdentificationUnitID as varchar)
  FROM [#project#].[CacheIdentificationUnit] U')

set @SQL = (@SQL + '
-- setting the still missing values
UPDATE CU SET
       CU.RelatedUnitID            = U.[RelatedUnitID]
      ,CU.[RelationType]           = U.[RelationType]
      ,CU.[ExsiccataNumber]        = U.[ExsiccataNumber]
      ,CU.[ColonisedSubstratePart] = U.[ColonisedSubstratePart]
      ,CU.[FamilyCache]            = U.[FamilyCache]
      ,CU.[OrderCache]             = U.[OrderCache]
      ,CU.[LifeStage]              = U.[LifeStage]
      ,CU.[Gender]                 = U.[Gender]
      ,CU.[HierarchyCache]         = U.[HierarchyCache]
      ,CU.[UnitIdentifier]         = U.[UnitIdentifier]
      ,CU.[UnitDescription]        = U.[UnitDescription]
      ,CU.[Circumstances]          = U.[Circumstances]
      ,CU.[Notes]                  = U.[Notes]
      ,CU.[NumberOfUnits]          = U.[NumberOfUnits]
      ,CU.[OnlyObserved]           = U.[OnlyObserved]
	  ,CU.[RetrievalType]          = U.[RetrievalType]
  FROM [' + dbo.SourceDatabase() + '].[dbo].[IdentificationUnit] U,
  [#project#].[CacheIdentificationUnit] CU
  WHERE U.CollectionSpecimenID = CU.CollectionSpecimenID
  AND U.IdentificationUnitID = CU.IdentificationUnitID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO
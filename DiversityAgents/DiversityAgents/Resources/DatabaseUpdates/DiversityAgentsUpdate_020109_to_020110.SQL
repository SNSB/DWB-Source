
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.09'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   UserProxy - Add ID - according to EU-DSGVO   ###############################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'ID') = 0 
begin 
   ALTER TABLE UserProxy ADD [ID] [int] IDENTITY(1,1) NOT NULL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'ID' end 
GO

--#####################################################################################################################
--######   UserProxy - Add - PrivacyConsent and PrivacyConsentDate   ##################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'PrivacyConsent') = 0 
begin 
   ALTER TABLE UserProxy ADD [PrivacyConsent] [bit] NULL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user consents the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsent' end 
GO

GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsent]) TO [DiversityWorkbenchUser]
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'PrivacyConsentDate') = 0 
begin 
   ALTER TABLE UserProxy ADD [PrivacyConsentDate] [datetime] NULL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time and date when the user consented or refused the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsentDate' end 
GO

GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsentDate]) TO [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   UserProxy - Trigger for setting PrivacyConsentDate   #######################################################
--#####################################################################################################################
CREATE TRIGGER [dbo].[trgUpdUserProxy] ON [dbo].[UserProxy] 
FOR UPDATE AS 
declare @PC bit 
if (select count(*) from deleted) = 1 
begin 
  set @PC = (select case when I.PrivacyConsent <> D.PrivacyConsent 
    or (I.PrivacyConsent is null and not D.PrivacyConsent is null) 
    or (not I.PrivacyConsent is null and D.PrivacyConsent is null) 
    then 1 else 0 end from inserted I, deleted D) 
  if (@PC = 1) 
  begin 
    UPDATE U SET PrivacyConsentDate = GETDATE()  
    FROM UserProxy U, deleted D 
    WHERE U.ID = D.ID 
  end 
end 
GO

--#####################################################################################################################
--######   Function providing the ID of the user from UserProxy  ######################################################
--#####################################################################################################################

declare @SQL nvarchar(max) set @SQL = (select ' CREATE FUNCTION [dbo].[UserID] () RETURNS int AS 
BEGIN  
declare @ID int;  
SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = SUSER_SNAME()) 
RETURN @ID  
END ') 
begin try 
   exec sp_executesql @SQL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserID' 
end try 
begin catch 
end catch 
GO 
GRANT EXEC ON [dbo].[UserID] TO [DiversityWorkbenchUser] 
GO 

BEGIN TRY 
  EXEC sys.sp_addextendedproperty  
    @name=N'MS_Description', @value=N'Providing the ID of the current user from UserProxy', 
    @level0type=N'SCHEMA', @level0name=N'dbo', 
    @level1type=N'FUNCTION', @level1name=N'UserID'; 
END TRY 
BEGIN CATCH 
  EXEC sys.sp_updateextendedproperty  
    @name=N'MS_Description', @value=N'Providing the ID of the current user from UserProxy', 
    @level0type=N'SCHEMA', @level0name=N'dbo', 
    @level1type=N'FUNCTION', @level1name=N'UserID'; 
END CATCH 
GO 

--#####################################################################################################################
--######   Function providing the name of the user from UserProxy  ####################################################
--#####################################################################################################################

declare @SQL nvarchar(max) set @SQL = (select ' CREATE FUNCTION [dbo].[UserName] (@ID varchar(10)) RETURNS nvarchar(50) AS 
BEGIN  
declare @User nvarchar(50);  
SET @User = (SELECT MIN(CASE WHEN U.CombinedNameCache <> '''' THEN substring(U.CombinedNameCache, 1, 50) ELSE CASE WHEN U.LoginName <> '''' THEN U.LoginName ELSE cast(U.ID as varchar) END END) FROM UserProxy U WHERE cast(U.ID as varchar) = @ID)  
RETURN @User  
END ') 
begin try 
   exec sp_executesql @SQL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserName' 
end try 
begin catch 
end catch 
GO 
GRANT EXEC ON [dbo].[UserName] TO [DiversityWorkbenchUser] 
GO 

BEGIN TRY 
  EXEC sys.sp_addextendedproperty  
    @name=N'MS_Description', @value=N'Providing the name of the user from UserProxy', 
    @level0type=N'SCHEMA', @level0name=N'dbo', 
    @level1type=N'FUNCTION', @level1name=N'UserName'; 
END TRY 
BEGIN CATCH 
  EXEC sys.sp_updateextendedproperty  
    @name=N'MS_Description', @value=N'Providing the name of the user from UserProxy', 
    @level0type=N'SCHEMA', @level0name=N'dbo', 
    @level1type=N'FUNCTION', @level1name=N'UserName'; 
END CATCH 
BEGIN TRY 
;   EXEC sys.sp_addextendedproperty  
    @name=N'MS_Description', @value=N'ID of the user according to table UserProxy', 
    @level0type=N'SCHEMA', @level0name=N'dbo', 
    @level1type=N'FUNCTION', @level1name=N'UserName', 
    @level2type=N'PARAMETER', @level2name=N'@ID'; 
END TRY 
BEGIN CATCH 
  EXEC sys.sp_updateextendedproperty  
    @name=N'MS_Description', @value=N'ID of the user according to table UserProxy', 
    @level0type=N'SCHEMA', @level0name=N'dbo', 
    @level1type=N'FUNCTION', @level1name=N'UserName', 
    @level2type=N'PARAMETER', @level2name=N'@ID';
 END CATCH
GO 

--#####################################################################################################################
--######   Function PrivacyConsentInfo providing common information within the DiversityWorkbench  ####################
--#####################################################################################################################

CREATE FUNCTION [dbo].[PrivacyConsentInfo] () 
RETURNS varchar (900) 
AS  
BEGIN return 'http://diversityworkbench.net/Portal/Default_Agreement_on_Processing_of_Personal_Data_in_DWB_Software'  
END; 
GO 
GRANT EXEC ON [dbo].[PrivacyConsentInfo] TO [DiversityWorkbenchUser] 
GO 
GRANT ALTER ON [dbo].[PrivacyConsentInfo] TO [DiversityWorkbenchAdministrator] 
GO 

BEGIN TRY 
  EXEC sys.sp_addextendedproperty  
    @name=N'MS_Description', @value=N'Providing common information about the storage and processing of personal data within the DiversityWorkbench', 
    @level0type=N'SCHEMA', @level0name=N'dbo', 
    @level1type=N'FUNCTION', @level1name=N'PrivacyConsentInfo'; 
END TRY 
BEGIN CATCH 
  EXEC sys.sp_updateextendedproperty  
    @name=N'MS_Description', @value=N'Providing common information about the storage and processing of personal data within the DiversityWorkbench', 
    @level0type=N'SCHEMA', @level0name=N'dbo', 
    @level1type=N'FUNCTION', @level1name=N'PrivacyConsentInfo'; 
END CATCH 
GO 


--#####################################################################################################################
--######   Agent: Adding column IgnoreButKeepForReference    ##########################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'Agent' and C.COLUMN_NAME = 'IgnoreButKeepForReference') = 0
begin
	ALTER TABLE Agent ADD [IgnoreButKeepForReference] [bit] NULL CONSTRAINT [DF_Agent_IgnoreButKeepForReference]  DEFAULT ((0));
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If true, the record is ignored for all purposes of evaluation (because contradicted). It is kept only to maintain the cited reference. If no reference is given, it may be deleted instead.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'IgnoreButKeepForReference'
	ALTER TABLE Agent_log ADD [IgnoreButKeepForReference] [bit] NULL;
end
GO

GO

DISABLE TRIGGER [dbo].[trgUpdAgent] ON [dbo].[Agent] 
GO

UPDATE [dbo].[Agent] SET [IgnoreButKeepForReference] = 0
GO

ENABLE TRIGGER [dbo].[trgUpdAgent] ON [dbo].[Agent] 
GO

ALTER TRIGGER [dbo].[trgDelAgent] ON [dbo].[Agent] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.7 */ 
/*  Date: 01.02.2011  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Agent_Log (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, RevisionLevel, DataWithholdingReason, IgnoreButKeepForReference, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT D.AgentID, D.AgentParentID, D.AgentName, D.Version, D.AgentTitle, D.GivenName, D.GivenNamePostfix, D.InheritedNamePrefix, D.InheritedName, D.InheritedNamePostfix, D.Abbreviation, D.AgentType, D.AgentRole, D.AgentGender, D.Description, D.OriginalSpelling, D.Notes, D.ValidFromDate, D.ValidFromDay, D.ValidFromMonth, D.ValidFromYear, D.ValidUntilDate, D.ValidUntilDay, D.ValidUntilMonth, D.ValidUntilYear, D.ValidDateSupplement, D.PlaceOfBirth, D.PlaceOfDeath, D.SynonymToAgentID, D.SynonymisationType, D.RevisionLevel, D.DataWithholdingReason, D.IgnoreButKeepForReference, D.LogUpdatedBy, D.LogUpdatedWhen,  'D'
FROM DELETED D

GO


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
INSERT INTO Agent_Log (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, RevisionLevel, DataWithholdingReason, IgnoreButKeepForReference, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT D.AgentID, D.AgentParentID, D.AgentName, D.Version, D.AgentTitle, D.GivenName, D.GivenNamePostfix, D.InheritedNamePrefix, D.InheritedName, D.InheritedNamePostfix, D.Abbreviation, D.AgentType, D.AgentRole, D.AgentGender, D.Description, D.OriginalSpelling, D.Notes, D.ValidFromDate, D.ValidFromDay, D.ValidFromMonth, D.ValidFromYear, D.ValidUntilDate, D.ValidUntilDay, D.ValidUntilMonth, D.ValidUntilYear, D.ValidDateSupplement, D.PlaceOfBirth, D.PlaceOfDeath, D.SynonymToAgentID, D.SynonymisationType, D.RevisionLevel, D.DataWithholdingReason, D.IgnoreButKeepForReference, D.LogUpdatedBy, D.LogUpdatedWhen,  'U'
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
--######   Agent_Core: Adding column DisplayText with indented synonyms    ############################################
--#####################################################################################################################

ALTER VIEW [dbo].[Agent_Core]
AS
SELECT        A.AgentID, A.AgentParentID, A.AgentName, CASE WHEN A.SynonymToAgentID IS NULL THEN '' ELSE '    ' END + A.AgentName AS DisplayText, A.Version, A.AgentTitle, A.GivenName, A.GivenNamePostfix, A.InheritedNamePrefix, 
                         A.InheritedName, A.InheritedNamePostfix, A.Abbreviation, A.AgentType, A.AgentRole, A.AgentGender, A.Description, A.OriginalSpelling, A.Notes, A.ValidFromDate, A.ValidFromDay, A.ValidFromMonth, A.ValidFromYear, 
                         A.ValidUntilDate, A.ValidUntilDay, A.ValidUntilMonth, A.ValidUntilYear, A.ValidDateSupplement, A.PlaceOfBirth, A.PlaceOfDeath, A.SynonymToAgentID, A.SynonymisationType, A.RevisionLevel, A.DataWithholdingReason,
						 A.IgnoreButKeepForReference
FROM            dbo.Agent AS A INNER JOIN
                         dbo.AgentID_UserAvailable AS U ON A.AgentID = U.AgentID
						 WHERE A.IgnoreButKeepForReference = 0
GO

GRANT SELECT ON [dbo].[Agent_Core] TO [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   missing Table descriptions    ##############################################################################
--#####################################################################################################################

if (SELECT count(*)
FROM sys.extended_properties AS ep
INNER JOIN  sys.tables AS t ON ep.major_id = t.object_id 
left outer JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id
WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = 'AgentReference' AND c.column_id IS NULL) = 0
begin
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'References containing informations about the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference'
end
GO

if (SELECT count(*)
FROM sys.extended_properties AS ep
INNER JOIN  sys.tables AS t ON ep.major_id = t.object_id 
left outer JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id
WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = 'AgentRelation' AND c.column_id IS NULL) = 0
begin
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relations of the agent to other agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation'
end
GO


--#####################################################################################################################
--######   Setting grants for Admin      ##############################################################################
--#####################################################################################################################

EXEC sp_addrolemember N'DiversityWorkbenchEditor', N'DiversityWorkbenchAdministrator';
GO

--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.00.22' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.10'
END

GO

select [dbo].[Version] ()  


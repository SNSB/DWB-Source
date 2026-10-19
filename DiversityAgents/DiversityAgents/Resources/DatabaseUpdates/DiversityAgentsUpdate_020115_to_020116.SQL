
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.15'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   AgentHierarchyAllSuperior  #################################################################################
--#####################################################################################################################
CREATE FUNCTION [dbo].[AgentHierarchyAllSuperior] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] NOT NULL,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL)

/*
Returns a table that lists all the agents superior to the given agent, including all hierarchy data.
MW 02.01.2006
SELECT * FROM dbo.AgentHierarchyAllSuperior(37687)
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

INSERT @All (AgentID, AgentParentID) 
SELECT AgentID, AgentParentID
FROM @Top T;

declare @i int
set @i = (select count(*) from @Top where AgentParentID is not null)
while @i > 0
begin
	UPDATE T SET T.AgentID = A.AgentID, T.AgentParentID = A.AgentParentID
	FROM @Top T, Agent A
	WHERE T.AgentParentID = A.AgentID
	AND T.AgentParentID IS NOT NULL

	INSERT @All (AgentID, AgentParentID) 
	SELECT AgentID, AgentParentID
	FROM @Top T WHERE NOT EXISTS (SELECT * FROM @All A WHERE A.AgentID = T.AgentID AND A.AgentParentID = T.AgentParentID);

	set @i = (select count(*) from @Top where AgentParentID is not null)
end


   -- copy the child nodes into the result list
   INSERT @AgentList
   SELECT DISTINCT AA.AgentID, AA.AgentParentID, A.AgentName
   FROM Agent A, @All AA
   WHERE A.AgentID = AA.AgentID

   RETURN
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns the list of all superior agents within the hierarchy and other hierarchies related to the agent with the given AgentID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentHierarchyAllSuperior'
GO

GRANT SELECT ON  [dbo].[AgentHierarchyAllSuperior] TO [DiversityWorkbenchUser];
GO


--#####################################################################################################################
--######   AgentHierarchyOther - add FOREIGN KEY for AgentParentID  ###################################################
--#####################################################################################################################

ALTER TABLE [dbo].[AgentHierarchyOther]  WITH CHECK ADD  CONSTRAINT [FK_AgentHierarchyOther_Agent1] FOREIGN KEY([AgentParentID])
REFERENCES [dbo].[Agent] ([AgentID])
GO

ALTER TABLE [dbo].[AgentHierarchyOther] CHECK CONSTRAINT [FK_AgentHierarchyOther_Agent1]
GO


--#####################################################################################################################
--######   Agent - add IdxAgentIgnoreButKeepForReference  #############################################################
--#####################################################################################################################

CREATE NONCLUSTERED INDEX IdxIgnoreButKeepForReference
ON [dbo].[Agent] ([IgnoreButKeepForReference])
INCLUDE ([AgentID],[SynonymToAgentID])
GO

--#####################################################################################################################
--######   AgentImage - add DataWithholdingReason  ####################################################################
--#####################################################################################################################

IF(SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = 'AgentImage' AND C.COLUMN_NAME = 'DataWithholdingReason') = 0
BEGIN
	ALTER TABLE AgentImage ADD DataWithholdingReason [nvarchar](255) NULL;
	ALTER TABLE [dbo].[AgentImage] ADD  CONSTRAINT [DF_AgentImage_DataWithholdingReason]  DEFAULT (N'Withhold by default') FOR [DataWithholdingReason]
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
	ALTER TABLE AgentImage_log ADD DataWithholdingReason [nvarchar](255) NULL;
END
GO


--#####################################################################################################################
--######   trgDelAgentImage - add DataWithholdingReason  ##############################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelAgentImage] ON [dbo].[AgentImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.5 */ 
/*  Date: 18.08.2010  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], DataWithholdingReason, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Type], deleted.[Sequence], deleted.DataWithholdingReason, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], DataWithholdingReason, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Type], deleted.[Sequence], deleted.DataWithholdingReason, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'D' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end

GO

--#####################################################################################################################
--######   trgUpdAgentImage - add DataWithholdingReason  ##############################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdAgentImage] ON [dbo].[AgentImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  3.0.1.0 */ 
/*  Date: 07.05.2018  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], DataWithholdingReason, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Type], deleted.[Sequence], deleted.DataWithholdingReason, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'U'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, Agent WHERE deleted.AgentID = Agent.AgentID) > 0 
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], DataWithholdingReason, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Type], deleted.[Sequence], deleted.DataWithholdingReason, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'U' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end
else
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], DataWithholdingReason, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Type], deleted.[Sequence], deleted.DataWithholdingReason, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, -1, 'U' 
FROM DELETED
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  LogUpdatedBy = SUSER_NAME()
FROM AgentImage T, deleted D 
where 1 = 1 
AND T.AgentID = D.AgentID
AND T.URI = D.URI
GO

--#####################################################################################################################
--######   AgentImage - setting the default  ##########################################################################
--#####################################################################################################################

UPDATE [dbo].[AgentImage]
   SET [DataWithholdingReason] = N'Withhold by default'
GO


--#####################################################################################################################
--######   PublicAgent  ###############################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicAgent]
AS
SELECT        N.AgentName AS DisplayText, dbo.BaseURL() + CAST(A.AgentID AS VARCHAR) AS URI, A.AgentID, A.AgentParentID, A.AgentName, A.Version, A.AgentTitle, A.GivenName, A.GivenNamePostfix, A.InheritedNamePrefix, 
                         A.InheritedName, A.InheritedNamePostfix, A.Abbreviation, A.AgentType, A.AgentRole, A.AgentGender, A.Description, A.OriginalSpelling, A.Notes, A.ValidFromDate, A.ValidFromDay, A.ValidFromMonth, A.ValidFromYear, 
                         A.ValidUntilDate, A.ValidUntilDay, A.ValidUntilMonth, A.ValidUntilYear, A.ValidDateSupplement, A.PlaceOfBirth, A.PlaceOfDeath, A.SynonymToAgentID, A.SynonymisationType, A.RevisionLevel, A.LogUpdatedWhen
FROM            Agent AS A INNER JOIN
                         dbo.AgentNames(dbo.DefaultAgentNameDisplayType()) AS N ON A.AgentID = N.AgentID
WHERE        (A.IgnoreButKeepForReference = 0) AND (A.DataWithholdingReason = N'' OR
                         A.DataWithholdingReason IS NULL)
UNION
SELECT        N.AgentName AS DisplayText, dbo.BaseURL() + CAST(A.AgentID AS VARCHAR) AS URI, S.AgentID, A.AgentParentID, A.AgentName, A.Version, A.AgentTitle, A.GivenName, A.GivenNamePostfix, A.InheritedNamePrefix, 
                         A.InheritedName, A.InheritedNamePostfix, A.Abbreviation, A.AgentType, A.AgentRole, A.AgentGender, A.Description, A.OriginalSpelling, A.Notes, A.ValidFromDate, A.ValidFromDay, A.ValidFromMonth, A.ValidFromYear, 
                         A.ValidUntilDate, A.ValidUntilDay, A.ValidUntilMonth, A.ValidUntilYear, A.ValidDateSupplement, A.PlaceOfBirth, A.PlaceOfDeath, A.SynonymToAgentID, A.SynonymisationType, A.RevisionLevel, A.LogUpdatedWhen
FROM            dbo.AgentNames(dbo.DefaultAgentNameDisplayType()) AS N INNER JOIN
                         Agent AS S INNER JOIN
                         Agent AS A ON S.SynonymToAgentID = A.AgentID ON N.AgentID = A.AgentID
WHERE        (S.IgnoreButKeepForReference = 1) AND (A.IgnoreButKeepForReference = 0) AND (A.DataWithholdingReason = N'' OR
                         A.DataWithholdingReason IS NULL) AND (S.SynonymisationType = 'replaced with')

GO

grant select on [PublicAgent] to [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   PublicImage  ###############################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicImage]
AS
SELECT        I.AgentID, I.URI, I.Description, I.Type, I.Sequence, I.LogUpdatedWhen
FROM            dbo.AgentImage AS I INNER JOIN
                         dbo.Agent AS A ON A.AgentID = I.AgentID
WHERE        (A.DataWithholdingReason = N'') OR
                         (I.DataWithholdingReason = N'') AND (A.DataWithholdingReason IS NULL) OR
                         (I.DataWithholdingReason IS NULL)
GO

grant select on [PublicImage] to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   [PublicReference]  #########################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicReference]
AS
SELECT        R.AgentID, R.ReferenceTitle, R.ReferenceURI, R.ReferenceDetails, R.ContainsImage, R.ContainsReferencelist, R.Notes, R.LogUpdatedWhen
FROM            dbo.AgentReference AS R INNER JOIN
                         dbo.Agent AS A ON R.AgentID = A.AgentID
WHERE        (A.DataWithholdingReason = N'') OR
                         (A.DataWithholdingReason IS NULL)
GO

grant select on [PublicReference] to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   [PublicRelation]  ##########################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicRelation]
AS
SELECT        R.AgentID, R.RelatedAgentID, R.RelationType, R.Notes, R.LogUpdatedWhen
FROM            dbo.AgentRelation AS R INNER JOIN
                         dbo.Agent AS A ON R.AgentID = A.AgentID
WHERE        (A.DataWithholdingReason = N'') OR
                         (A.DataWithholdingReason IS NULL) AND (R.DataWithholdingReason = N'') OR
                         (R.DataWithholdingReason IS NULL)
GO

grant select on [PublicRelation] to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   PublicContactInformation4  #################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicContactInformation4]
AS
SELECT        A1.AgentID, C.DisplayOrder, A4.AgentName AS ParentName, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, C.CellularPhone, C.Telefax, C.Email, C.URI, 
                         C.Notes, C.ValidFrom, C.ValidUntil, C.LogUpdatedWhen
FROM            dbo.Agent AS A1 INNER JOIN
                         dbo.Agent AS A2 ON A1.AgentParentID = A2.AgentID INNER JOIN
                         dbo.Agent AS A3 ON A2.AgentParentID = A3.AgentID INNER JOIN
                         dbo.Agent AS A4 ON A3.AgentParentID = A4.AgentID INNER JOIN
                         dbo.AgentContactInformation AS C ON A4.AgentParentID = C.AgentID
WHERE    EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE   (M.DataWithholdingReason IS NULL OR M.DataWithholdingReason = '')
AND     (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO


--#####################################################################################################################
--######   PublicContactInformation3  #################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicContactInformation3]
AS
SELECT        A1.AgentID, C.DisplayOrder, A3.AgentName AS ParentName, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, C.CellularPhone, C.Telefax, C.Email, C.URI, 
                         C.Notes, C.ValidFrom, C.ValidUntil, C.LogUpdatedWhen
FROM            dbo.Agent AS A1 INNER JOIN
                         dbo.Agent AS A2 ON A1.AgentParentID = A2.AgentID INNER JOIN
                         dbo.Agent AS A3 ON A2.AgentParentID = A3.AgentID INNER JOIN
                         dbo.AgentContactInformation AS C ON A3.AgentParentID = C.AgentID
WHERE      EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE      (M.DataWithholdingReason IS NULL OR M.DataWithholdingReason = '')
AND       (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO


--#####################################################################################################################
--######   PublicContactInformation2  #################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicContactInformation2]
AS
SELECT        A.AgentID, C.DisplayOrder, A2.AgentName AS ParentName, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, C.CellularPhone, C.Telefax, C.Email, C.URI, 
                         C.Notes, C.ValidFrom, C.ValidUntil, C.LogUpdatedWhen
FROM            dbo.Agent AS A INNER JOIN
                         dbo.Agent AS A2 ON A.AgentParentID = A2.AgentID INNER JOIN
                         dbo.AgentContactInformation AS C ON A2.AgentParentID = C.AgentID
WHERE     EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE     (M.DataWithholdingReason IS NULL OR M.DataWithholdingReason = '')
AND         (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO


--#####################################################################################################################
--######   PublicContactInformation1  #################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicContactInformation1]
AS
SELECT        A.AgentID, C.DisplayOrder, A.AgentName AS ParentName, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, C.CellularPhone, C.Telefax, C.Email, C.URI, 
                         C.Notes, C.ValidFrom, C.ValidUntil, C.LogUpdatedWhen
FROM            dbo.Agent AS A INNER JOIN
                         dbo.AgentContactInformation AS C ON A.AgentParentID = C.AgentID
WHERE     EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE      (M.DataWithholdingReason IS NULL OR M.DataWithholdingReason = '')
AND        (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO


--#####################################################################################################################
--######   PublicContactInformation0  #################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicContactInformation0]
AS
SELECT        AgentID, DisplayOrder, CAST(NULL AS varchar(200)) AS ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, 
                         ValidUntil, C.LogUpdatedWhen
FROM            dbo.AgentContactInformation AS C
WHERE      EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE     (M.DataWithholdingReason IS NULL OR M.DataWithholdingReason = '')
AND        (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO


--#####################################################################################################################
--######   PublicContactInformation  ##################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicContactInformation]
AS
SELECT        
CASE WHEN A0.AgentID IS NULL THEN CASE WHEN A1.AgentID IS NULL THEN case when a2.AgentID is null then case when a3.AgentID is null then a4.AgentID else a3.AgentID end else a2.AgentID end ELSE A1.AgentID end ELSE A0.AgentID END AS AgentID, 
CASE WHEN A0.DisplayOrder IS NULL THEN CASE WHEN A1.DisplayOrder IS NULL THEN case when a2.DisplayOrder is null then case when a3.DisplayOrder is null then a4.DisplayOrder else a3.DisplayOrder end else a2.DisplayOrder end ELSE A1.DisplayOrder end ELSE A0.DisplayOrder END AS DisplayOrder, 
CASE WHEN A4.ParentName IS NULL THEN CASE WHEN A3.ParentName IS NULL THEN case when a2.ParentName is null then case when a1.ParentName is null then a0.ParentName else a1.ParentName end else a2.ParentName end ELSE A3.ParentName end ELSE A4.ParentName END AS ParentName, 
CASE WHEN A0.AddressType IS NULL THEN CASE WHEN A1.AddressType IS NULL THEN case when a2.AddressType is null then case when a3.AddressType is null then a4.AddressType else a3.AddressType end else a2.AddressType end ELSE A1.AddressType end ELSE A0.AddressType END AS AddressType, 
CASE WHEN A0.Country IS NULL THEN CASE WHEN A1.Country IS NULL THEN case when a2.Country is null then case when a3.Country is null then a4.Country else a3.Country end else a2.Country end ELSE A1.Country end ELSE A0.Country END AS Country, 
CASE WHEN A0.City IS NULL THEN CASE WHEN A1.City IS NULL THEN case when a2.City is null then case when a3.City is null then a4.City else a3.City end else a2.City end ELSE A1.City end ELSE A0.City END AS City, 
CASE WHEN A0.PostalCode IS NULL THEN CASE WHEN A1.PostalCode IS NULL THEN case when a2.PostalCode is null then case when a3.PostalCode is null then a4.PostalCode else a3.PostalCode end else a2.PostalCode end ELSE A1.PostalCode end ELSE A0.PostalCode END AS PostalCode, 

CASE WHEN A0.Streetaddress IS NULL THEN CASE WHEN A1.Streetaddress IS NULL THEN case when a2.Streetaddress is null then case when a3.Streetaddress is null then a4.Streetaddress else a3.Streetaddress end else a2.Streetaddress end ELSE A1.Streetaddress end ELSE A0.Streetaddress END AS Streetaddress, 
CASE WHEN A0.Address IS NULL THEN CASE WHEN A1.Address IS NULL THEN case when a2.Address is null then case when a3.Address is null then a4.Address else a3.Address end else a2.Address end ELSE A1.Address end ELSE A0.Address END AS Address, 
CASE WHEN A0.Telephone IS NULL THEN CASE WHEN A1.Telephone IS NULL THEN case when a2.Telephone is null then case when a3.Telephone is null then a4.Telephone else a3.Telephone end else a2.Telephone end ELSE A1.Telephone end ELSE A0.Telephone END AS Telephone, 
CASE WHEN A0.CellularPhone IS NULL THEN CASE WHEN A1.CellularPhone IS NULL THEN case when a2.CellularPhone is null then case when a3.CellularPhone is null then a4.CellularPhone else a3.CellularPhone end else a2.CellularPhone end ELSE A1.CellularPhone end ELSE A0.CellularPhone END AS CellularPhone, 
CASE WHEN A0.Telefax IS NULL THEN CASE WHEN A1.Telefax IS NULL THEN case when a2.Telefax is null then case when a3.Telefax is null then a4.Telefax else a3.Telefax end else a2.Telefax end ELSE A1.Telefax end ELSE A0.Telefax END AS Telefax, 
CASE WHEN A0.Email IS NULL THEN CASE WHEN A1.Email IS NULL THEN case when a2.Email is null then case when a3.Email is null then a4.Email else a3.Email end else a2.Email end ELSE A1.Email end ELSE A0.Email END AS Email, 
CASE WHEN A0.URI IS NULL THEN CASE WHEN A1.URI IS NULL THEN case when a2.URI is null then case when a3.URI is null then a4.URI else a3.URI end else a2.URI end ELSE A1.URI end ELSE A0.URI END AS URI, 
CASE WHEN A0.Notes IS NULL THEN CASE WHEN A1.Notes IS NULL THEN case when a2.Notes is null then case when a3.Notes is null then a4.Notes else a3.Notes end else a2.Notes end ELSE A1.Notes end ELSE A0.Notes END AS Notes, 
CASE WHEN A0.ValidFrom IS NULL THEN CASE WHEN A1.ValidFrom IS NULL THEN case when a2.ValidFrom is null then case when a3.ValidFrom is null then a4.ValidFrom else a3.ValidFrom end else a2.ValidFrom end ELSE A1.ValidFrom end ELSE A0.ValidFrom END AS ValidFrom, 
CASE WHEN A0.ValidUntil IS NULL THEN CASE WHEN A1.ValidUntil IS NULL THEN case when a2.ValidUntil is null then case when a3.ValidUntil is null then a4.ValidUntil else a3.ValidUntil end else a2.ValidUntil end ELSE A1.ValidUntil end ELSE A0.ValidUntil END AS ValidUntil,
CASE WHEN A0.LogUpdatedWhen IS NULL THEN CASE WHEN A1.LogUpdatedWhen IS NULL THEN case when a2.LogUpdatedWhen is null then case when a3.LogUpdatedWhen is null then a4.LogUpdatedWhen else a3.LogUpdatedWhen end else a2.LogUpdatedWhen end ELSE A1.LogUpdatedWhen end ELSE A0.LogUpdatedWhen END AS LogUpdatedWhen
FROM dbo.PublicContactInformation4 AS A4 FULL OUTER JOIN
dbo.PublicContactInformation3 AS A3 ON A4.AgentID = A3.AgentID FULL OUTER JOIN
dbo.PublicContactInformation2 AS A2 ON A3.AgentID = A2.AgentID FULL OUTER JOIN
dbo.PublicContactInformation1 AS A1 ON A2.AgentID = A1.AgentID FULL OUTER JOIN
dbo.PublicContactInformation0 AS A0 ON A1.AgentID = A0.AgentID
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the superior agent within the hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublicContactInformation', @level2type=N'COLUMN',@level2name=N'ParentName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Accumulated public address of an agent within a hierarchy of up to 4 levels' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublicContactInformation'
GO

--#####################################################################################################################
--######   Grants for views   #########################################################################################
--#####################################################################################################################


grant select on [PublicContactInformation] to [DiversityWorkbenchUser]
grant select on [PublicContactInformation0] to [DiversityWorkbenchUser]
grant select on [PublicContactInformation1] to [DiversityWorkbenchUser]
grant select on [PublicContactInformation2] to [DiversityWorkbenchUser]
grant select on [PublicContactInformation3] to [DiversityWorkbenchUser]
grant select on [PublicContactInformation4] to [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.01.02' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.16'
END

GO

select [dbo].[Version] ()  


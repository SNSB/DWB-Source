
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.22'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   AgentContactInformation_Public - avoiding loops   ##########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentContactInformation_Public] ()  
RETURNS @ItemList TABLE (
	[AgentID] [int] NOT NULL,
	[DisplayOrder] [tinyint] NOT NULL,
	[ParentName] [nvarchar](200) NULL,
	[AddressType] [nvarchar](50) NULL,
	[Country] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[PostalCode] [nvarchar](50) NULL,
	[Streetaddress] [nvarchar](255) NULL,
	[Address] [nvarchar](255) NULL,
	[Telephone] [nvarchar](50) NULL,
	[CellularPhone] [nvarchar](50) NULL,
	[Telefax] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[URI] [nvarchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[ValidFrom] [datetime] NULL,
	[ValidUntil] [datetime] NULL,
	[LogUpdatedWhen] [smalldatetime] NULL)

/*
Returns a result set that lists the public contact information of every agent.
MW 06.08.2020

SELECT * FROM [dbo].[AgentContactInformation_Public] ()
WHERE(AgentID = 37687)
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
	[AgentID] [int] NOT NULL,
	[DisplayOrder] [tinyint] NOT NULL,
	[ParentName] [nvarchar](200) NULL,
	[AddressType] [nvarchar](50) NULL,
	[Country] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[PostalCode] [nvarchar](50) NULL,
	[Streetaddress] [nvarchar](255) NULL,
	[Address] [nvarchar](255) NULL,
	[Telephone] [nvarchar](50) NULL,
	[CellularPhone] [nvarchar](50) NULL,
	[Telefax] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[URI] [nvarchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[ValidFrom] [datetime] NULL,
	[ValidUntil] [datetime] NULL,
	[LogUpdatedWhen] [smalldatetime] NULL)

	-- Getting all public agents with a superior agent (and if present the display order of the first address)
	INSERT @TempItem (AgentID, DisplayOrder)
	SELECT   A.AgentID,    CASE WHEN  MIN(C.DisplayOrder) IS NULL THEN 0 ELSE  MIN(C.DisplayOrder) END AS DisplayOrder
	FROM            AgentContactInformation AS C RIGHT OUTER JOIN
		Agent AS A ON C.AgentID = A.AgentID 
		and  (C.DataWithholdingReason IS NULL OR
		C.DataWithholdingReason = '') AND (C.ValidFrom <= GETDATE() OR
		C.ValidFrom IS NULL) AND (C.ValidUntil IS NULL OR
		C.ValidUntil >= GETDATE())/**/
	WHERE (A.DataWithholdingReason IS NULL OR
		A.DataWithholdingReason = '')
		AND NOT A.AgentParentID IS NULL
	GROUP BY A.AgentID

	-- Getting all public agents without a superior agent and the display order of the first address
	INSERT @TempItem (AgentID, DisplayOrder)
	SELECT   A.AgentID,    CASE WHEN  MIN(C.DisplayOrder) IS NULL THEN 0 ELSE  MIN(C.DisplayOrder) END AS DisplayOrder
	FROM            AgentContactInformation AS C INNER JOIN
		Agent AS A ON C.AgentID = A.AgentID and  (C.DataWithholdingReason IS NULL OR
		C.DataWithholdingReason = '') AND (C.ValidFrom <= GETDATE() OR
		C.ValidFrom IS NULL) AND (C.ValidUntil IS NULL OR
		C.ValidUntil >= GETDATE())
	WHERE (A.DataWithholdingReason IS NULL OR
		A.DataWithholdingReason = '')
		AND A.AgentParentID IS NULL
	GROUP BY A.AgentID


	-- setting the values for the basic address
	UPDATE T SET T.AddressType = A.AddressType, 
		T.Country = A.Country, 
		T.City = A.City, 
		T.PostalCode = A.PostalCode, 
		T.Streetaddress = A.Streetaddress, 
		T.Address = A.Address, 
		T.Telephone = A.Telephone, 
		T.CellularPhone = A.CellularPhone, 
		T.Telefax = A.Telefax, 
		T.Email = A.Email, 
		T.URI = A.URI, 
		T.Notes = A.Notes, 
		T.ValidFrom = A.ValidFrom, 
		T.ValidUntil = A.ValidUntil,
		T.LogUpdatedWhen = A.LogUpdatedWhen
	FROM @TempItem T, AgentContactInformation A
	WHERE T.AgentID = A.AgentID
	AND T.DisplayOrder = A.DisplayOrder
	AND (A.DataWithholdingReason IS NULL OR
		A.DataWithholdingReason = '') AND (A.ValidFrom <= GETDATE() OR
		A.ValidFrom IS NULL) AND (A.ValidUntil IS NULL OR
		A.ValidUntil >= GETDATE())

	-- getting the hierarchy of the agents
	DECLARE @Hierarchy TABLE (
	[AgentID] [int] NOT NULL,
	[AgentParentID] [int] NULL,
	[ParentName] [nvarchar](200),
	[DataWithholdingReason] [nvarchar](255))

	INSERT INTO @Hierarchy (AgentID, AgentParentID, ParentName, DataWithholdingReason)
	SELECT A.AgentID, A.AgentParentID, P.AgentName, A.DataWithholdingReason
	FROM Agent A, Agent P
	WHERE A.AgentParentID = P.AgentID

	DECLARE @CheckLoop int

	WHILE (SELECT COUNT(*) FROM @Hierarchy H WHERE NOT H.AgentParentID IS NULL) > 0
	BEGIN
		SET @CheckLoop = (SELECT COUNT(*) FROM @Hierarchy H WHERE NOT H.AgentParentID IS NULL)

		-- writing the address infos of the superior agent into the inferior agent
		UPDATE T0 SET
		T0.ParentName = CASE WHEN T0.ParentName IS NULL OR T0.ParentName = '' THEN H.ParentName ELSE T0.ParentName END, 
		T0.AddressType = CASE WHEN T0.AddressType IS NULL OR T0.AddressType = '' THEN Tn.AddressType ELSE T0.AddressType END, 
		T0.Country = CASE WHEN T0.Country IS NULL OR T0.Country = '' THEN Tn.Country ELSE T0.Country END, 
		T0.City = CASE WHEN T0.City IS NULL OR T0.City = '' THEN Tn.City ELSE T0.City END, 
		T0.PostalCode = CASE WHEN T0.PostalCode IS NULL OR T0.PostalCode = '' THEN Tn.PostalCode ELSE T0.PostalCode END, 
		T0.Streetaddress = CASE WHEN T0.Streetaddress IS NULL OR T0.Streetaddress = '' THEN Tn.Streetaddress ELSE T0.Streetaddress END, 
		T0.Address = CASE WHEN T0.Address IS NULL OR T0.Address = '' THEN Tn.Address ELSE T0.Address END, 
		T0.Telephone = CASE WHEN T0.Telephone IS NULL OR T0.Telephone = '' THEN Tn.Telephone ELSE T0.Telephone END, 
		T0.CellularPhone = CASE WHEN T0.CellularPhone IS NULL OR T0.CellularPhone = '' THEN Tn.CellularPhone ELSE T0.CellularPhone END, 
		T0.Telefax = CASE WHEN T0.Telefax IS NULL OR T0.Telefax = '' THEN Tn.Telefax ELSE T0.Telefax END, 
		T0.Email = CASE WHEN T0.Email IS NULL OR T0.Email = '' THEN Tn.Email ELSE T0.Email END, 
		T0.URI = CASE WHEN T0.URI IS NULL OR T0.URI = '' THEN Tn.URI ELSE T0.URI END, 
		T0.Notes = CASE WHEN T0.Notes IS NULL OR T0.Notes = '' THEN Tn.Notes ELSE T0.Notes END, 
		T0.ValidFrom = CASE WHEN T0.ValidFrom IS NULL OR T0.ValidFrom = '' THEN Tn.ValidFrom ELSE T0.ValidFrom END, 
		T0.ValidUntil = CASE WHEN T0.ValidUntil IS NULL OR T0.ValidUntil = '' THEN Tn.ValidUntil ELSE T0.ValidUntil END,
		T0.LogUpdatedWhen = CASE WHEN T0.LogUpdatedWhen IS NULL OR T0.LogUpdatedWhen = '' THEN Tn.LogUpdatedWhen ELSE T0.LogUpdatedWhen END
		FROM @TempItem T0, @TempItem Tn, @Hierarchy H
		WHERE T0.AgentID = H.AgentID and Tn.AgentID = H.AgentParentID
		AND (H.DataWithholdingReason IS NULL OR H.DataWithholdingReason = '')

		-- getting the next hierarchy step
		UPDATE H SET H.AgentParentID = A.AgentParentID, H.DataWithholdingReason = A.DataWithholdingReason
		FROM @Hierarchy H, Agent A
		WHERE H.AgentParentID = A.AgentID

		-- removing failed parents
		UPDATE H SET H.AgentParentID = NULL
		FROM @Hierarchy H left outer join Agent A on H.AgentParentID = A.AgentID
		WHERE A.AgentID is null

		-- Avoid loops
		IF (@CheckLoop = (SELECT COUNT(*) FROM @Hierarchy H WHERE NOT H.AgentParentID IS NULL))
		BEGIN BREAK END
	END

	INSERT INTO @ItemList 
	(AgentID, DisplayOrder, ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, LogUpdatedWhen)
	SELECT 
	AgentID, DisplayOrder, ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, LogUpdatedWhen
	FROM @TempItem

RETURN
END

GO


--#####################################################################################################################
--######  Missing relation between [SynonymToAgentID] and [AgentID] in [dbo].[Agent]  #################################
--#####################################################################################################################

-- Remove invalid relations
UPDATE S SET S.[SynonymToAgentID] = NULL
  FROM [dbo].[Agent] S left outer join [dbo].[Agent] A on S.SynonymToAgentID = A.AgentID and a.AgentID is null
  where not S.SynonymToAgentID is null

-- Add relation
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [FK_Agent_SynonymToAgentID] FOREIGN KEY([SynonymToAgentID])
REFERENCES [dbo].[Agent] ([AgentID])
GO

ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_SynonymToAgentID]
GO



--#####################################################################################################################
--######   Agent_Core: Adding column ValidFrom and ValidUntil    ######################################################
--#####################################################################################################################

ALTER VIEW [dbo].[Agent_Core]
AS
SELECT        A.AgentID, A.AgentParentID, A.AgentName, CASE WHEN A.SynonymToAgentID IS NULL THEN '' ELSE '    ' END + A.AgentName AS DisplayText, A.Version, A.AgentTitle, A.GivenName, A.GivenNamePostfix, A.InheritedNamePrefix, 
                         A.InheritedName, A.InheritedNamePostfix, A.Abbreviation, A.AgentType, A.AgentRole, A.AgentGender, A.Description, A.OriginalSpelling, A.Notes, A.ValidFromDate, A.ValidFromDay, A.ValidFromMonth, A.ValidFromYear, 
                         A.ValidUntilDate, A.ValidUntilDay, A.ValidUntilMonth, A.ValidUntilYear, A.ValidDateSupplement, A.PlaceOfBirth, A.PlaceOfDeath, A.SynonymToAgentID, A.SynonymisationType, A.RevisionLevel, A.DataWithholdingReason, 
                         A.IgnoreButKeepForReference, CASE WHEN A.ValidFromYear IS NULL THEN '' ELSE CAST(A.ValidFromYear AS varchar) + '-' + CASE WHEN A.ValidFromMonth IS NULL THEN '' ELSE CAST(A.ValidFromMonth AS varchar) 
                         END + '-' + CASE WHEN A.ValidFromDay IS NULL THEN '' ELSE CAST(A.ValidFromDay AS varchar) END END AS ValidFrom, CASE WHEN A.ValidUntilYear IS NULL THEN '' ELSE CAST(A.ValidUntilYear AS varchar) 
                         + '-' + CASE WHEN A.ValidUntilMonth IS NULL THEN '' ELSE CAST(A.ValidUntilMonth AS varchar) END + '-' + CASE WHEN A.ValidUntilDay IS NULL THEN '' ELSE CAST(A.ValidUntilDay AS varchar) END END AS ValidUntil
FROM            dbo.Agent AS A INNER JOIN
                         dbo.AgentID_UserAvailable AS U ON A.AgentID = U.AgentID
WHERE        (A.IgnoreButKeepForReference = 0)
GO



--#####################################################################################################################
--######  Missing descriptions  #######################################################################################
--#####################################################################################################################
--######  Tables  #####################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Unique ID for the descriptor, Part of PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentDescriptor',
@level2type=N'COLUMN', @level2name=N'DescriptorID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Unique ID for the descriptor, Part of PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentDescriptor',
@level2type=N'COLUMN', @level2name=N'DescriptorID'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.TABLES r where r.TABLE_NAME = 'AgentIdentifier') > 0
 and (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'AgentIdentifier' and c.COLUMN_NAME = 'IdentifierID') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Unique ID for the identifier, Part of PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentIdentifier',
@level2type=N'COLUMN', @level2name=N'IdentifierID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Unique ID for the identifier, Part of PK',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentIdentifier',
@level2type=N'COLUMN', @level2name=N'IdentifierID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Sex of the agent: female or male',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentGender_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Sex of the agent: female or male',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentGender_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Types of the agent identifier, e.g. ISNI',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentIdentifierType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Types of the agent identifier, e.g. ISNI',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentIdentifierType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Types of images in table AgentImage, e.g. Logo',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentImageType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Types of images in table AgentImage, e.g. Logo',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentImageType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Display types of the names of the agents, defining e.g. the position of title, given and inherited part of the name and their abbreviation',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentNameDisplayType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Display types of the names of the agents, defining e.g. the position of title, given and inherited part of the name and their abbreviation',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentNameDisplayType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Relation types of agents, e.g. Child of',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentRelationType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Relation types of agents, e.g. Child of',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentRelationType_Enum'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Synonymisation types of agents, e.g. synonym to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentSynonymisationType_Enum'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Synonymisation types of agents, e.g. synonym to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'AgentSynonymisationType_Enum'
END CATCH;
GO

--#####################################################################################################################
--######  Trigger  ######################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Setting the value of column AgentName according to standard derived from content of given values in columns InheritedNamePrefix, Inheritedname, GivenName, GivenNamePostfix, InheritedNamePostfix and AgentTitle',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Agent',
@level2type=N'TRIGGER', @level2name=N'trgInsAgent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Setting the value of column AgentName according to standard derived from content of given values in columns InheritedNamePrefix, Inheritedname, GivenName, GivenNamePostfix, InheritedNamePostfix and AgentTitle',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Agent',
@level2type=N'TRIGGER', @level2name=N'trgInsAgent'
END CATCH;
GO

--#####################################################################################################################
--######  Views  ######################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Corresponds to content of column AgentName',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicAgent',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Corresponds to content of column AgentName',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicAgent',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Combines BaseURL of the database and AgentID to provide unique identifier of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicAgent',
@level2type=N'COLUMN', @level2name=N'URI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Combines BaseURL of the database and AgentID to provide unique identifier of the dataset',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicAgent',
@level2type=N'COLUMN', @level2name=N'URI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Agent available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicAgent'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Agent available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicAgent'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table AgentIdentifier available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicIdentifier'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table AgentIdentifier available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicIdentifier'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table AgentImage available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicImage'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table AgentImage available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicImage'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table AgentReference available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicReference'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table AgentReference available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicReference'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table AgentRelation available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicRelation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table AgentRelation available for the public',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PublicRelation'
END CATCH;
GO

--#####################################################################################################################
--######  Functions  ##################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Display order according to the hierarchy with 1 for the current dataset und increasing values for higher levels in the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSuperiorList',
@level2type=N'COLUMN', @level2name=N'DisplayOrder'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Display order according to the hierarchy with 1 for the current dataset und increasing values for higher levels in the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentSuperiorList',
@level2type=N'COLUMN', @level2name=N'DisplayOrder'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ReplacedWithAgentID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ReplacedWithAgentID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuColumns'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuColumns'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuForeignKeys'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuForeignKeys'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuInferiorTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuInferiorTables'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuOrderColumn'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuOrderColumn'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuSuperiorTables'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuSuperiorTables'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuTableTabs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SearchMenuTableTabs'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SuperiorAgentList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SuperiorAgentList'
END CATCH;
GO

--#####################################################################################################################
--######  PROCEDURES  #################################################################################################
--#####################################################################################################################



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertAgentNew'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertAgentNew'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Create database user and assign training projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Create database user and assign training projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'LoginName of the User',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects',
@level2type=N'PARAMETER', @level2name=N'@User'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'LoginName of the User',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects',
@level2type=N'PARAMETER', @level2name=N'@User'
END CATCH;
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.23'
END

GO

select [dbo].[Version] ()  


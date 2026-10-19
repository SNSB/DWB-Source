
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.17'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  procInsertAgentCopy - Bugfix and extension to new tables  ###################################################
--#####################################################################################################################


ALTER PROCEDURE [dbo].[procInsertAgentCopy] 
	(@AgentID int output,
	 @CopyID int,
	 @AgentName nvarchar  (200))
AS

-- Agent
INSERT INTO Agent
	(AgentParentID, AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType,
	AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, 
	ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, RevisionLevel, 
	DataWithholdingReason)
SELECT     AgentParentID, @AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, 
	AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, 
	ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, 'unchecked' AS RevisionLevel, 
	DataWithholdingReason
FROM         Agent
WHERE     (AgentID = @CopyID)

SELECT @AgentID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

-- AgentContactInformation
INSERT INTO AgentContactInformation (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, 
                      ValidFrom, ValidUntil)
SELECT @AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, 
                      ValidFrom, ValidUntil
FROM         AgentContactInformation 
WHERE AgentID = @CopyID

-- AgentProject
INSERT INTO AgentProject
                      (AgentID, ProjectID)
SELECT     @AgentID, ProjectID
FROM         AgentProject
WHERE AgentID = @CopyID

-- AgentReference
INSERT INTO AgentReference
                      (AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, ContainsImage, ContainsReferencelist, Notes)
SELECT     @AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, ContainsImage, ContainsReferencelist, Notes
FROM         AgentReference
WHERE AgentID = @CopyID

-- AgentKeyword
INSERT INTO AgentKeyword
                      (AgentID, Keyword)
SELECT     @AgentID, Keyword
FROM         AgentKeyword
WHERE AgentID = @CopyID

-- AgentImage
INSERT INTO AgentImage
                      (AgentID, URI)
SELECT     @AgentID, URI
FROM         AgentImage
WHERE AgentID = @CopyID

--AgentExternalID
INSERT INTO AgentExternalID
                      (AgentID, ExternalDatabaseID, ExternalAgentURI)
SELECT     @AgentID, ExternalDatabaseID, ExternalAgentURI
FROM         AgentExternalID
WHERE AgentID = @CopyID

-- AgentRelation
INSERT INTO AgentRelation
                      (AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason)
SELECT     @AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason
FROM         AgentRelation
WHERE AgentID = @CopyID

SELECT @AgentID

GO




--#####################################################################################################################
--######  generic SetUserProjects  ####################################################################################
--#####################################################################################################################

/****** Object:  StoredProcedure [dbo].[SetUserProjects]    Script Date: 04.02.2021 15:23:35 ******/
IF 1 = (SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE [SPECIFIC_NAME]='SetUserProjects')
DROP PROCEDURE [dbo].[SetUserProjects]
GO

-- =============================================
-- Author:		Anton Link
-- Create date: 20210205
-- Description:	Create database user and assign
--              training projects     
-- =============================================
CREATE PROCEDURE [dbo].[SetUserProjects] 
	-- Add the parameters for the stored procedure here
	@User VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @Result NVARCHAR(200);
	DECLARE @DatabaseName NVARCHAR(MAX);
	DECLARE @DatabaseRole VARCHAR(100);
	DECLARE @DatabaseUser VARCHAR(100);
	DECLARE @Workshop INT;
	DECLARE @Windows INT;
	SET @DatabaseName=(SELECT DB_NAME());
	SET @DatabaseRole=(SELECT TOP 1 [name] FROM [sys].[database_principals] WHERE [type]='R' AND [name] LIKE '%Administrator');
	SET @Workshop=(SELECT COUNT(*) FROM [UserProxy] WHERE [LoginName]='Workshop');
	SET @Windows=CHARINDEX('\', REVERSE(@User));
	SET @Result=''; 

	IF @Windows=0
	BEGIN
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''S'' AND [name]=''' + @User + ''' ');
		EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;

		IF @DatabaseUser IS NULL
		BEGIN
  			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''U'' AND [name] LIKE ''%\' + @User + ''' ');
			EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;
		END
	END
	ELSE
	BEGIN
		SET @User=REVERSE(SUBSTRING(REVERSE(@User), 1, @Windows));
  		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''U'' AND [name] LIKE ''%' + @User + ''' ');
		EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;
	END
	
	IF @DatabaseUser IS NULL
	BEGIN
		SET @Result='User ' + @User + ' is not present in database ' + @DatabaseName;
	END
	ELSE
	BEGIN
		IF NOT @DatabaseRole IS NULL
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'ALTER ROLE [' + @DatabaseRole + '] ADD MEMBER [' + @DatabaseUser + ']; ')
			EXEC sp_executesql @SQL
			SET @Result='Database role ' + @DatabaseRole + ' set for user ' + @DatabaseUser;
		END
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'DELETE FROM [dbo].[ProjectUser] WHERE [LoginName]=''' + @DatabaseUser + ''' ')
		EXEC sp_executesql @SQL
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'DELETE FROM [dbo].[UserProxy] WHERE [LoginName]=''' + @DatabaseUser + ''' ')
		EXEC sp_executesql @SQL

 		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'INSERT INTO [dbo].[UserProxy] ([LoginName], [CombinedNameCache]) VALUES (''' + @DatabaseUser + ''', ''' + @DatabaseUser + '''); ')
		EXEC sp_executesql @SQL

		IF @Workshop > 0
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'INSERT INTO [dbo].[ProjectUser] ([LoginName], [ProjectID]) ' +
							 'SELECT ''' + @DatabaseUser + ''', [ProjectID] FROM [dbo].[ProjectUser] WHERE [LoginName]=''Workshop''; ')
			EXEC sp_executesql @SQL
			SET @Result=@Result + '; Workshop projects assigned for user ' + @DatabaseUser;
		END
		ELSE
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'INSERT INTO [dbo].[ProjectUser] ([LoginName], [ProjectID]) ' +
							 'SELECT ''' + @DatabaseUser + ''', [ProjectID] FROM [dbo].[ProjectProxy] WHERE [ProjectID]>0; ')
			EXEC sp_executesql @SQL
			SET @Result=@Result + '; Available projects assigned for user ' + @DatabaseUser;
		END
	END
	SELECT @Result
END
GO



--#####################################################################################################################
--######   AgentContactInformation_Public - bugfix Telefax and removing failed parents    #############################
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

	WHILE (SELECT COUNT(*) FROM @Hierarchy H WHERE NOT H.AgentParentID IS NULL) > 0
	BEGIN
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
--######   ViewAgentNames - create view if missing   ##################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentNames]
AS
SELECT * FROM [dbo].[AgentNames] (
  [dbo].[DefaultAgentNameDisplayType] ())
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON [ViewAgentNames] TO [DiversityWorkbenchUser]
GO



--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '04.01.00' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.18'
END

GO

select [dbo].[Version] ()  


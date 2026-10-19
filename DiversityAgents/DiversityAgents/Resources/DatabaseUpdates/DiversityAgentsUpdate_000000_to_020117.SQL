
/****** Object:  DatabaseRole [DiversityWorkbenchAdministrator]    Script Date: 07.04.2021 12:23:26 ******/
CREATE ROLE [DiversityWorkbenchAdministrator]
GO
/****** Object:  DatabaseRole [DiversityWorkbenchEditor]    Script Date: 07.04.2021 12:23:26 ******/
CREATE ROLE [DiversityWorkbenchEditor]
GO
/****** Object:  DatabaseRole [DiversityWorkbenchUser]    Script Date: 07.04.2021 12:23:26 ******/
CREATE ROLE [DiversityWorkbenchUser]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [DiversityWorkbenchAdministrator]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [DiversityWorkbenchAdministrator]
GO
ALTER ROLE [DiversityWorkbenchEditor] ADD MEMBER [DiversityWorkbenchAdministrator]
GO
ALTER ROLE [DiversityWorkbenchUser] ADD MEMBER [DiversityWorkbenchEditor]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentAddress]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   AgentAddress    ############################################################################################
--#####################################################################################################################

-- =============================================
-- Author:		Markus Weiss
-- Create date: 13.03.2007
-- Description:	returns a table containing the address values including values of superior entries for missing values
-- Test: 
-- SELECT * FROM dbo.AgentAddress(37687)
-- SELECT * FROM dbo.AgentAddress(37677)
-- SELECT * FROM dbo.AgentAddress(37686)
-- =============================================
CREATE FUNCTION [dbo].[AgentAddress] 
(
@AgentID int
)
/*
Testing:
SELECT * FROM dbo.AgentAddress(18685);
SELECT * FROM dbo.AgentAddress(37687)
SELECT * FROM dbo.AgentAddress(37677)
SELECT * FROM dbo.AgentAddress(37686)
SELECT * FROM dbo.AgentContactInformation WHERE AgentID = 37686
*/
RETURNS 
@AddressTable TABLE 
(
	[AgentID] [int] NOT NULL Primary key,
	[DisplayOrder] [tinyint] NULL,
	[ParentName] [nvarchar](4000) COLLATE Latin1_General_CI_AS NULL,
	[AddressType] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Country] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[City] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[PostalCode] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Streetaddress] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[Address] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[Telephone] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[CellularPhone] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Telefax] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Email] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[URI] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[Notes] [nvarchar](Max) NULL,
	[ValidFrom] [datetime] NULL,
	[ValidUntil] [datetime] NULL
)
AS
BEGIN

INSERT INTO @AddressTable (AgentID) VALUES (@AgentID)
DECLARE @DisplayOrder tinyint
SET @DisplayOrder = (SELECT MIN(DisplayOrder) FROM AgentContactInformation 
	WHERE AgentID = @AgentID 
	AND (ValidFrom <= GETDATE() OR ValidFrom IS NULL)  
	AND (ValidUntil IS NULL OR ValidUntil >= GETDATE() ))

UPDATE @AddressTable SET 
	[AddressType] = AgentContactInformation.AddressType,
	[DisplayOrder] = 0,
	[Country] = AgentContactInformation.Country,
	[City] = AgentContactInformation.City,
	[PostalCode] = AgentContactInformation.PostalCode,
	[Streetaddress] = AgentContactInformation.Streetaddress,
	[Address] = AgentContactInformation.Address,
	[Telephone] = AgentContactInformation.Telephone,
	[CellularPhone] = AgentContactInformation.CellularPhone,
	[Telefax] = AgentContactInformation.Telefax,
	[Email] = AgentContactInformation.Email,
	[URI] = AgentContactInformation.URI,
	[Notes]  = AgentContactInformation.Notes,
	[ValidFrom] = AgentContactInformation.ValidFrom,
	[ValidUntil] = AgentContactInformation.ValidUntil
	FROM AgentContactInformation
	WHERE AgentContactInformation.AgentID = @AgentID AND AgentContactInformation.DisplayOrder = @DisplayOrder

DECLARE @ParentName [nvarchar](4000)
DECLARE @AddressType [nvarchar](50)
DECLARE @Country [nvarchar](255)
DECLARE @City [nvarchar](255) 
DECLARE @PostalCode [nvarchar](50)
DECLARE @Streetaddress [nvarchar](255) 
DECLARE @Address [nvarchar](255)
DECLARE @Telephone [nvarchar](50) 
DECLARE @CellularPhone [nvarchar](50)
DECLARE @Telefax [nvarchar](50)
DECLARE @Email [nvarchar](255)
DECLARE @URI [nvarchar](255)

DECLARE @ParentID int
SET @ParentID = (SELECT AgentParentID FROM dbo.Agent WHERE AgentID = @AgentID)

WHILE NOT @ParentID IS NULL
BEGIN
	SET @DisplayOrder = (SELECT MIN(DisplayOrder) FROM AgentContactInformation 
	WHERE AgentID = @ParentID 
	AND (ValidFrom <= GETDATE() OR ValidFrom IS NULL)  
	AND (ValidUntil IS NULL OR ValidUntil >= GETDATE() ))

	SET @ParentName = (SELECT ParentName FROM @AddressTable)
	IF @ParentName IS NULL OR LEN(@ParentName) = 0
	BEGIN 
		UPDATE @AddressTable SET [ParentName] = Agent.AgentName--, DisplayOrder = @DisplayOrder
		FROM Agent
		WHERE Agent.AgentID = @ParentID
	END

	SET @AddressType = (SELECT AddressType FROM @AddressTable)
	IF @AddressType <> (SELECT AddressType FROM AgentContactInformation WHERE AgentContactInformation.AgentID = @ParentID AND DisplayOrder = @DisplayOrder)
		BEGIN UPDATE @AddressTable SET AddressType = '?' END
	IF @AddressType IS NULL OR LEN(@AddressType) = 0
	BEGIN 
		UPDATE @AddressTable SET AddressType = AgentContactInformation.AddressType
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END
	UPDATE @AddressTable SET AddressType = AddressType + ' - cumulative'


	SET @Country = (SELECT Country FROM @AddressTable)
	IF @Country IS NULL OR LEN(@Country) = 0
	BEGIN 
		UPDATE @AddressTable SET [Country] = AgentContactInformation.Country
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @City = (SELECT City FROM @AddressTable)
	IF @City IS NULL OR LEN(@City) = 0
	BEGIN 
		UPDATE @AddressTable SET [City] = AgentContactInformation.City
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @PostalCode = (SELECT PostalCode FROM @AddressTable)
	IF @PostalCode IS NULL OR LEN(@PostalCode) = 0
	BEGIN 
		UPDATE @AddressTable SET PostalCode = AgentContactInformation.PostalCode
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @Streetaddress = (SELECT Streetaddress FROM @AddressTable)
	IF @Streetaddress IS NULL OR LEN(@Streetaddress) = 0 
	BEGIN 
		UPDATE @AddressTable SET Streetaddress = AgentContactInformation.Streetaddress
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @Address = (SELECT Address FROM @AddressTable)
	IF @Address IS NULL OR LEN(@Address) = 0 
	BEGIN 
		UPDATE @AddressTable SET Address = AgentContactInformation.Address
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @Telephone = (SELECT Telephone FROM @AddressTable)
	IF @Telephone IS NULL OR LEN(@Telephone) = 0 
	BEGIN 
		UPDATE @AddressTable SET Telephone = AgentContactInformation.Telephone
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @CellularPhone = (SELECT CellularPhone FROM @AddressTable)
	IF @CellularPhone IS NULL OR LEN(@CellularPhone) = 0 
	BEGIN 
		UPDATE @AddressTable SET CellularPhone = AgentContactInformation.CellularPhone
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @Telefax = (SELECT Telefax FROM @AddressTable)
	IF @Telefax IS NULL OR LEN(@Telefax) = 0 
	BEGIN 
		UPDATE @AddressTable SET Telefax = AgentContactInformation.Telefax
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @Email = (SELECT Email FROM @AddressTable)
	IF @Email IS NULL OR LEN(@Email) = 0 
	BEGIN 
		UPDATE @AddressTable SET Email = AgentContactInformation.Email
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @URI = (SELECT URI FROM @AddressTable)
	IF @URI IS NULL OR LEN(@URI) = 0 
	BEGIN 
		UPDATE @AddressTable SET URI = AgentContactInformation.URI
		FROM AgentContactInformation
		WHERE AgentContactInformation.AgentID = @ParentID AND AgentContactInformation.DisplayOrder = @DisplayOrder
	END

	SET @ParentID = (SELECT AgentParentID FROM dbo.Agent WHERE AgentID = @ParentID)
END

DECLARE @I INT
SET @I = (SELECT COUNT(*) FROM @AddressTable WHERE DisplayOrder IS NULL 
AND ParentName IS NULL AND AddressType IS NULL AND Country IS NULL
AND City IS NULL AND PostalCode IS NULL AND Streetaddress IS NULL
AND [Address] IS NULL AND Telephone IS NULL AND CellularPhone IS NULL
AND Telefax IS NULL AND Email IS NULL AND URI IS NULL)

IF @I > 0 BEGIN DELETE FROM @AddressTable END 

SET @DisplayOrder = (SELECT MIN(DisplayOrder) FROM AgentContactInformation WHERE AgentID = @AgentID)

SET @I = (SELECT COUNT(*) FROM @AddressTable A, AgentContactInformation C 
WHERE (A.Country = C.Country OR A.Country IS NULL)
AND (A.City = C.City OR A.City IS NULL)
AND (A.PostalCode = C.PostalCode OR C.PostalCode IS NULL)
AND (A.Streetaddress = C.Streetaddress OR A.Streetaddress IS NULL)
AND (A.[Address] = C.[Address] OR A.[Address] IS NULL)
AND (A.Telephone = C.Telephone OR A.Telephone IS NULL)
AND (A.CellularPhone = C.CellularPhone OR A.CellularPhone IS NULL)
AND (A.Telefax = C.Telefax OR A.Telefax IS NULL)
AND (A.Email = C.Email OR A.Email IS NULL)
AND (A.URI  = C.URI OR A.URI IS NULL)
AND C.AgentID = @AgentID
AND A.AgentID = @AgentID
AND C.DisplayOrder = @DisplayOrder)

IF @I > 0 
BEGIN 

	UPDATE @AddressTable 
	SET AddressType = (SELECT AddressType FROM AgentContactInformation WHERE AgentID = @AgentID AND DisplayOrder = @DisplayOrder)

	UPDATE @AddressTable 
	SET DisplayOrder = (SELECT MIN(DisplayOrder) FROM AgentContactInformation WHERE AgentID = @AgentID )

END	

RETURN 
 
END

GO
GRANT SELECT ON [dbo].[AgentAddress] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentChildNodes]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[AgentChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)


	INSERT @TempItem (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason) 
	SELECT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
	FROM Agent WHERE AgentParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select AgentID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason 
	from dbo.AgentChildNodes (@ParentID) where AgentID not in (select AgentID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason) 
   SELECT distinct AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
   FROM @TempItem ORDER BY AgentName
   RETURN
END

GO
GRANT SELECT ON [dbo].[AgentChildNodes] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentContactInformation_Public]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[AgentContactInformation_Public] ()  
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
	UPDATE T SET T.AddressType = A.AddressType, T.Country = A.Country, T.City = A.City, 
		T.PostalCode = A.PostalCode, T.Streetaddress = A.Streetaddress, T.Address = A.Address, 
		T.Telephone = A.Telephone, T.CellularPhone = A.CellularPhone, T.Telefax = T.Telefax, 
		T.Email = A.Email, T.URI = A.URI, T.Notes = A.Notes, T.ValidFrom = A.ValidFrom, T.ValidUntil = A.ValidUntil,
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

	END

	INSERT INTO @ItemList 
	(AgentID, DisplayOrder, ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, LogUpdatedWhen)
	SELECT 
	AgentID, DisplayOrder, ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, LogUpdatedWhen
	FROM @TempItem

RETURN
END


GO
GRANT SELECT ON [dbo].[AgentContactInformation_Public] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentHierarchy]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





--#####################################################################################################################
--######   AgentHierarchy   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[AgentHierarchy] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.AgentTopID(@AgentID) )

-- get the ID's of the child nodes
DECLARE @TempItem TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)

	INSERT @TempItem (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason) 
	SELECT * FROM dbo.AgentChildNodes (@TopID)

-- copy the root node in the result list
   INSERT @AgentList
   SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
   FROM Agent
   WHERE Agent.AgentID = @TopID

   -- copy the child nodes into the result list
   INSERT @AgentList
   SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
   FROM Agent
   WHERE Agent.AgentID in (select AgentID from @TempItem)
   ORDER BY AgentName

   RETURN
END






GO
GRANT SELECT ON [dbo].[AgentHierarchy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentHierarchyAllSuperior]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[AgentHierarchyAllSuperior] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentNames]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




--#####################################################################################################################
--######   AgentNames   ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[AgentNames] (@DisplayType nvarchar(50))  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentName] [nvarchar](200) NOT NULL)

/*
Returns a table that lists all the agents with their names according to the selected display type.
The types are documented in the table dbo.AgentNameDisplayType_Enum
MW 02.01.2006
Test:
select * from dbo.AgentNames ('Ii, G.') where AgentID in (24, 38, 41, 2760, 64, 94,149, 39427)

declare @AgentID int
set @AgentID = 25745--39466
select * from dbo.Agent where AgentID = @AgentID
select * from dbo.AgentNames ('Ii, G.') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Gg Ii') where AgentID in (@AgentID)
select * from dbo.AgentNames ('G. Ii') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Ii, Gg') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Gg Ii') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Tt. Gg Ii') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Tt. G. Ii') where AgentID in (@AgentID)

select * from dbo.Agent where AgentID = 39526
select * from dbo.Agent where AgentID in  (24, 38, 41, 2760, 64, 94,149)
select * from dbo.Agent where Givenname like '% % %'
select * from dbo.Agent where agentname like 'abel, g%'
select * from dbo.AgentNames ('Tt Gg Ii') order by AgentName
*/
AS
BEGIN
	INSERT @AgentList (AgentID, AgentName) 
	SELECT A.AgentID, 
	CASE WHEN A.AgentType <> 'Person' 
	THEN A.AgentName
	ELSE
	case when @DisplayType = 'Ii, G.'
	then 
		case when not A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
		+ case when A.InheritedName is null then '' else A.InheritedName end 
		+ case when not A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then A.InheritedNamePostfix + ' ' else '' end
		+ case when rtrim(A.GivenName) <> '' 
		then
			+ case when A.GivenName is null or rtrim(A.GivenName) = '' then ''  else ', ' end
				+ case when A.GivenName like '_._._._._._.' or A.GivenName like '_._._._._.' or A.GivenName like '_._._._.' or A.GivenName like '_._._.' or A.GivenName like '_._.' or A.GivenName like '_.' 
				or A.GivenName like '_. _. _.' or A.GivenName like '_. _.' 
				then A.GivenName 
				else
				case when A.GivenName like '%-%' then substring(A.GivenName, 1, 1) + '.' + '-' + substring(A.GivenName, charindex('-', A.GivenName) + 1, 1) + '.' 
				else
					case when not A.GivenName like '% %' 
					then substring(A.GivenName, 1, 1) + '.' 
					else
						case when not A.GivenName like '% % %' 
						then substring(A.GivenName, 1, 1) + '.' + substring(A.GivenName, charindex(' ', A.GivenName) + 1, 1) + '.' 
						else
							case when substring(A.GivenName, 1, 1) <> '(' then substring(A.GivenName, 1, 1) + '.'else '' end
							+ case when substring(A.GivenName, charindex(' ', A.GivenName) + 1, 1) <> '(' 
							then substring(A.GivenName, charindex(' ', A.GivenName) + 1, 1) + '.' 
							else '' end
							+ case when substring(substring(A.GivenName, charindex(' ', A.GivenName) + 1, 255), charindex(' ', substring(A.GivenName, charindex(' ', A.GivenName) + 1, 255)) + 1, 1) <> '(' 
							then substring(substring(A.GivenName, charindex(' ', A.GivenName) + 1, 255), charindex(' ', substring(A.GivenName, charindex(' ', A.GivenName) + 1, 255)) + 1, 1) + '.'
							else '' end
						end
					end
				end
			end
		else ''
		end
		+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' 
			then case when rtrim(A.GivenName) <> '' then '' else ',' end + ' ' + A.GivenNamePostfix
			else '' end
	else
		case when @DisplayType = 'Ii, Gg'
		then
			ltrim(case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
			+ case when A.InheritedName is null then '' else ' ' + A.InheritedName end 
			+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
			+ case when A.GivenName is null then '' else ', ' + GivenName end
			+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then case when rtrim(A.GivenName) <> '' then '' else ',' end + ' ' + A.GivenNamePostfix else '' end)
		else
			case when @DisplayType = 'Gg Ii'
			then
				case when A.GivenName is null then '' else GivenName + ' ' end
				+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then A.GivenNamePostfix + ' ' else '' end
				+ case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
				+ case when A.InheritedName is null then '' else A.InheritedName + ' ' end 
				+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
			else
				case when @DisplayType = 'G. Ii'
				then
					case when A.GivenName is null then '' else substring(A.GivenName, 1, 1) + '.' + ' ' end
					+ case when A.GivenName like '% %' and Not a.GivenName like '% (%' then substring(A.GivenName, charindex(' ', A.GivenName) + 1, 1) + '. ' else '' end
					+ case when A.GivenName like '% % %' and Not a.GivenName like '% % % %' and Not a.GivenName like '% % (%' then substring(reverse(A.GivenName), charindex(' ', reverse(A.GivenName)) - 1, 1) + '. 'else '' end
					+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then A.GivenNamePostfix + ' ' else '' end
					+ case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
					+ case when A.InheritedName is null then '' else A.InheritedName + ' ' end 
					+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
				else
					case when @DisplayType = 'Tt. Gg Ii'
					then 
						case when A.AgentTitle is null then '' else A.AgentTitle + ' ' end
						+ case when A.GivenName is null then '' else A.GivenName + ' ' end
						+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then A.GivenNamePostfix + ' ' else '' end
						+ case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
						+ case when A.InheritedName is null then '' else A.InheritedName end 
						+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
					else
						case when @DisplayType = 'Tt. G. Ii'
						then
							case when A.AgentTitle is null then '' else A.AgentTitle + ' ' end
							+ case when A.GivenName is null then '' else substring(A.GivenName, 1, 1) + '. ' end
							+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then A.GivenNamePostfix + ' ' else '' end
							+ case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
							+ case when A.InheritedName is null then '' else A.InheritedName end
							+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
						else
							case when @DisplayType = 'Abbr' and (not A.Abbreviation is null or A.Abbreviation = '')
							then A.Abbreviation
							else A.AgentName
							end
						end
					end
				end
			end
		end
	end
	END
	FROM dbo.Agent A

   RETURN
END

GO
GRANT SELECT ON [dbo].[AgentNames] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentSuperiorList]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




--#####################################################################################################################
--######   AgentSuperiorList  ######################################################################################
--#####################################################################################################################




CREATE FUNCTION [dbo].[AgentSuperiorList] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	DisplayOrder int NOT NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
Test:
select * from dbo.AgentSuperiorList(12665)
*/
AS
BEGIN
	declare @i int
	set @i = 1
	while (not @AgentID is null)
		begin

			INSERT @AgentList
			SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
			ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
			SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, @i
			FROM Agent
			WHERE Agent.AgentID = @AgentID

			set @AgentID = (select AgentParentID from Agent where AgentID = @AgentID)

			set @i = (select @i + 1)

		end

   RETURN
END

GO
GRANT SELECT ON [dbo].[AgentSuperiorList] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentSynonymTopID]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[AgentSynonymTopID] (@AgentID int)
RETURNS int
/*
Returns the top ID within the synyonym hierarchy for a given ID from the table Agent.
MW 03.01.2006
*/
AS
BEGIN
declare @TopID int
declare @i int

set @TopID = (select SynonymToAgentID from Agent where AgentID = @AgentID) 

set @i = (select count(*) from Agent where AgentID = @AgentID)

if (@TopID is null )
	set @TopID =  @AgentID
else	
	begin
	while (@i > 0)
		begin
		set @AgentID = (select SynonymToAgentID from Agent where AgentID = @AgentID and not SynonymToAgentID is null) 
		set @i = (select count(*) from Agent where AgentID = @AgentID and not SynonymToAgentID is null)
		end
	set @TopID = @AgentID
	end
return @TopID
END


GO
GRANT EXECUTE ON [dbo].[AgentSynonymTopID] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentSynonymy]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[AgentSynonymy] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.AgentSynonymTopID(@AgentID) )

-- get the ID's of the child nodes
DECLARE @TempItem TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL)

	INSERT @TempItem (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel) 
	SELECT * FROM dbo.AgentSynonymyChildNodes (@TopID)

-- copy the root node in the result list
   INSERT @AgentList
   SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel
   FROM Agent
   WHERE Agent.AgentID = @TopID

   -- copy the child nodes into the result list
   INSERT @AgentList
   SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel
   FROM Agent
   WHERE Agent.AgentID in (select AgentID from @TempItem)
   ORDER BY AgentName

   RETURN
END






GO
GRANT SELECT ON [dbo].[AgentSynonymy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentSynonymyChildNodes]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[AgentSynonymyChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL)

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @SynonymToAgentID int
   DECLARE @TempItem TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL)


	INSERT @TempItem (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel) 
	SELECT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel
	FROM Agent WHERE SynonymToAgentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select AgentID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @SynonymToAgentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel 
	from dbo.AgentSynonymyChildNodes (@SynonymToAgentID) where AgentID not in (select AgentID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @SynonymToAgentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel) 
   SELECT distinct AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel
   FROM @TempItem ORDER BY AgentName
   RETURN
END


GO
GRANT SELECT ON [dbo].[AgentSynonymyChildNodes] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[AgentTopID]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[AgentTopID] (@AgentID int)
RETURNS int
/*
Returns the top ID within the hierarchy for a given ID from the table Agent.
MW 03.01.2006
*/
AS
BEGIN
declare @TopID int
declare @i int

set @TopID = (select AgentParentID from Agent where AgentID = @AgentID) 

set @i = (select count(*) from Agent where AgentID = @AgentID)

if (@TopID is null )
	set @TopID =  @AgentID
else	
	begin
	while (@i > 0)
		begin
		set @AgentID = (select AgentParentID from Agent where AgentID = @AgentID and not AgentParentID is null) 
		set @i = (select count(*) from Agent where AgentID = @AgentID and not AgentParentID is null)
		end
	set @TopID = @AgentID
	end
return @TopID
END

GO
GRANT EXECUTE ON [dbo].[AgentTopID] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ApplicationSearchItemPrimaryKeyColumn]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ApplicationSearchItemPrimaryKeyColumn] (@TableName varchar(50))  
/* returns the name of the first column of the primary key. Items in the database (e.g. Collection habe a IdentityColumn as primary key */
RETURNS nvarchar(300)
AS  
BEGIN 
DECLARE @ColumnName nvarchar(300)

set @ColumnName =
(
SELECT      MAX(c.Column_Name) 
FROM         INFORMATION_SCHEMA.TABLE_CONSTRAINTS t INNER JOIN
                      INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c ON t.Constraint_Name = c.Constraint_Name
WHERE     (t.Constraint_Type = 'Primary key') 
AND (t.Table_Name = @TableName)
)

RETURN @ColumnName
END





GO
/****** Object:  UserDefinedFunction [dbo].[BaseURL]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE ON [dbo].[BaseURL] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[DefaultAgentNameDisplayType]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   [DefaultAgentNameDisplayType]    ###########################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[DefaultAgentNameDisplayType] ()
RETURNS nvarchar(50)
AS
BEGIN
	declare @Type nvarchar(50)
	set @Type = (SELECT [AgentNameDisplayType]
	FROM [ProjectProxy]
	where ProjectID = dbo.DefaultProjectID ())
	if @Type is null
	begin
	set @Type = ''
	end
	RETURN @Type
END

GO
GRANT EXECUTE ON [dbo].[DefaultAgentNameDisplayType] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[DefaultProjectID]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Markus Weiss>
-- Create date: <27.12.2006>
-- Description:	<Returns the project id of the default project like the CombinedThesaurus>
-- =============================================
CREATE FUNCTION [dbo].[DefaultProjectID] ()
RETURNS int
AS
BEGIN
	declare @i int
	set @i = (select ProjectID from UserProxy where loginname = User_Name())
	if @i is null
	begin
	set @i = (select min(ProjectID) from ProjectUser where loginname = User_Name())
	end
	RETURN @i
END


GO
GRANT EXECUTE ON [dbo].[DefaultProjectID] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[DiversityWorkbenchModule]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	
CREATE FUNCTION [dbo].[DiversityWorkbenchModule] ()   RETURNS nvarchar(50) AS BEGIN RETURN 'DiversityAgents' END  ; 
GO
GRANT EXECUTE ON [dbo].[DiversityWorkbenchModule] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ReplacedWithAgentID]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ReplacedWithAgentID] (@AgentID int)
RETURNS int
/*
Returns the replacement ID for an ID that has been replaced.
MW 13.07.2008
*/
AS
BEGIN
declare @MaxLoop int
declare @ReplacementID int
declare @i int

set @i = (select count(*) from Agent where AgentID = @AgentID and not ReplacedWithAgentID is null)
if (@i = 0)
	return @AgentID

set @ReplacementID = (select ReplacedWithAgentID from Agent where AgentID = @AgentID) 
/*
set @i = (select count(*) from Agent where AgentID = @AgentID)

if (@TopID is null )
	set @TopID =  @AgentID
else	
	begin
	while (@i > 0)
		begin
		set @AgentID = (select AgentParentID from Agent where AgentID = @AgentID and not AgentParentID is null) 
		set @i = (select count(*) from Agent where AgentID = @AgentID and not AgentParentID is null)
		end
	set @TopID = @AgentID
	end
return @TopID
*/
return @ReplacementID

END

GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuColumns]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuColumns] (@TableName varchar(50))  
RETURNS @MenuList TABLE (
   TableName varchar(50) not null,
   ColumnName varchar(50) not null,
   DataType varchar(50) not null,
   ColumnLength int null,
   ColumnDescription nvarchar(1000) null,
   ForeignKeyTable varchar(50) null,
   ForeignKeyColumn varchar(50) null,
   MenuTab varchar(50) null)
/*Returns a result set that lists all the Tables and Collections connected to the querried item.*/
AS
BEGIN
/* inserting the Data into the table */
-- table variable to hold accumulated results
   DECLARE @ItemList TABLE (
   TableName varchar(50) not null,
   ColumnName varchar(50) not null,
   DataType varchar(50) not null,
   ColumnLength int null,
   ColumnDescription nvarchar(1000) null,
   ForeignKeyTable varchar(50) null,
   ForeignKeyColumn varchar(50) null,
   MenuTab varchar(50) null)
insert into @ItemList (TableName, ColumnName, DataType, ColumnLength, MenuTab)
SELECT     TOP 100 PERCENT dbo.sysobjects.name AS TableName, dbo.syscolumns.name AS ColumnName, dbo.systypes.name AS DataType, 
                      	dbo.syscolumns.length, case when dbo.sysobjects.name like 'Collection%' and len(dbo.sysobjects.name) > 10  then RTRIM( substring(dbo.sysobjects.name, 11, 50)) else 
		case when dbo.sysobjects.name like 'Identification%' then  RTRIM( 'Identification ' + lower(substring(dbo.sysobjects.name, 15, 50)))  else dbo.sysobjects.name end end
FROM         dbo.syscolumns INNER JOIN
                      dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id INNER JOIN
                      dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE     (dbo.sysobjects.xtype = 'U')
		AND (dbo.systypes.name <> N'sysname') 
		AND (NOT (dbo.syscolumns.name LIKE N'%ID')) 
		AND (NOT (dbo.syscolumns.name LIKE N'%Sequence')) 
		AND (dbo.sysobjects.name IN (SELECT Tablename FROM dbo.SearchMenuInferiorTables (@TableName)) 
			OR dbo.sysobjects.name = @TableName 
			OR dbo.sysobjects.name IN (SELECT Tablename FROM dbo.SearchMenuSuperiorTables (@TableName)) )
		AND dbo.systypes.name IN ('char', 'nchar', 'varchar', 'nvarchar', 'text', 'ntext','bit', 'tinyint', 'smallint', 'int', 'bigint', 'real', 'float', 'money','smallmoney','decimal', 'numeric', 'datetime', 'smalldatetime')
ORDER BY dbo.sysobjects.name, dbo.syscolumns.name
/* update of the descriptions */
DECLARE @Table as varchar(50)
DECLARE @OldTable as varchar(50)
DECLARE @Column as varchar(50)
DECLARE @Description as nvarchar(1000)
DECLARE @fktable_name nvarchar(257) /* 2*128 + 1 */
DECLARE @fktable_id	int
DECLARE @fkfull_table_name	nvarchar(257) /* 2*128 + 1 */
DECLARE DescriptionCursor  CURSOR for
select TableName, ColumnName from @ItemList
open DescriptionCursor
FETCH next from DescriptionCursor into @Table, @Column
SET @OldTable = (select @Table)
WHILE @@FETCH_STATUS = 0
BEGIN
   -- This is executed as long as the previous fetch succeeds.
	set @Description = (SELECT     max(CONVERT(nvarchar(1000), [value]))
	FROM         ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @Table, 'column', @Column) [::fn_listextendedproperty_1]) 
	update @ItemList set ColumnDescription = @Description where TableName = @Table and ColumnName = @Column
	/* setting the foreign-key values for lookup tables */
	IF  @OldTable <>  @Table
	BEGIN
		SET @fkfull_table_name = (SELECT quotename(@Table))
		SET @fktable_id = (SELECT object_id(@fkfull_table_name))
		UPDATE @ItemList SET ForeignKeyTable = convert(sysname,o1.name), ForeignKeyColumn = convert(sysname,c1.name)
		from @ItemList a, sysreferences r,  sysindexes i,
			sysobjects o1, sysobjects o2,
			syscolumns c1, syscolumns c2
		where	r.rkeyindid = i.indid
			AND r.rkeyid = i.id
			AND r.fkeyid = @fktable_id
			AND o1.id = r.rkeyid
			AND o2.id = r.fkeyid
			AND c1.id = r.rkeyid
			AND c2.id = r.fkeyid
			AND c1.colid = r.rkey1
			AND c2.colid = r.fkey1
			and o1.name like '%_Enum'
			AND convert(sysname,c2.name) = a.ColumnName
		/*print @Table
		print @OldTable
		print @Column*/
	END
	set @OldTable = (select @Table)
	
   	FETCH NEXT FROM DescriptionCursor into @Table, @Column
END
CLOSE DescriptionCursor
DEALLOCATE DescriptionCursor
/* transferring the data */
INSERT @MenuList
SELECT TableName,  ColumnName, DataType, ColumnLength, ColumnDescription, ForeignKeyTable, ForeignKeyColumn, MenuTab
FROM @ItemList
order by TableName, ColumnName
return
END


GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuForeignKeys]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuForeignKeys] (@TableName varchar(50))  
RETURNS @MenuList TABLE (
   TableName varchar(50) not null,
   ColumnName varchar(50) not null,
   ForeignKeyTable varchar(50) null,
   ForeignKeyColumn varchar(50) null)
/*Returns a result set that lists all the Tables and Foreign keys connected to the querried item.*/
AS
BEGIN
-- table variable to hold accumulated results
   DECLARE @ItemList TABLE (
   TableName varchar(50) not null,
   ColumnName varchar(50) not null,
   ForeignKeyTable varchar(50) null,
   ForeignKeyColumn varchar(50) null)
/* inserting the Data into the table */
insert into @ItemList (TableName, ColumnName)
SELECT     TOP 100 PERCENT dbo.sysobjects.name AS TableName, dbo.syscolumns.name AS ColumnName
FROM         dbo.syscolumns INNER JOIN
                   dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id INNER JOIN
                   dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE     (dbo.sysobjects.xtype = 'U') AND (dbo.systypes.name <> N'sysname') 
	AND (NOT (dbo.syscolumns.name LIKE N'%Sequence')) 
	AND (
		dbo.sysobjects.name IN (SELECT Tablename FROM SearchMenuSuperiorTables ( @TableName) )
		OR dbo.sysobjects.name IN (SELECT Tablename FROM SearchMenuInferiorTables ( @TableName) )
		OR dbo.sysobjects.name = @TableName 
		)/*# ApplicationCollSearchMenuTables - Austauschen gegen Funktion !!! #*/
ORDER BY dbo.sysobjects.name, dbo.syscolumns.name
/* update of the descriptions */
DECLARE @Table as varchar(50)
DECLARE @OldTable as varchar(50)
DECLARE @Column as varchar(50)
--DECLARE @Description as nvarchar(1000)
DECLARE @fktable_name nvarchar(257) /* 2*128 + 1 */
DECLARE @fktable_id	int
DECLARE @fkfull_table_name	nvarchar(257) /* 2*128 + 1 */
DECLARE DescriptionCursor  CURSOR for
select TableName, ColumnName from @ItemList
open DescriptionCursor
FETCH next from DescriptionCursor into @Table, @Column
SET @OldTable = (select @Table)
WHILE @@FETCH_STATUS = 0
BEGIN
   -- This is executed as long as the previous fetch succeeds.
	/* setting the foreign-key values for lookup tables */
	IF  @OldTable <>  @Table
	BEGIN
		SET @fkfull_table_name = (SELECT quotename(@Table))
		SET @fktable_id = (SELECT object_id(@fkfull_table_name))
		UPDATE @ItemList SET ForeignKeyTable = convert(sysname,o1.name), ForeignKeyColumn = convert(sysname,c1.name)
		from @ItemList a, sysreferences r,  sysindexes i,
			sysobjects o1, sysobjects o2,
			syscolumns c1, syscolumns c2
		where	r.rkeyindid = i.indid
			AND r.rkeyid = i.id
			AND r.fkeyid = @fktable_id
			AND o1.id = r.rkeyid
			AND o2.id = r.fkeyid
			AND c1.id = r.rkeyid
			AND c2.id = r.fkeyid
			AND c1.colid = r.rkey1
			AND c2.colid = r.fkey1
			and o1.name not like '%_Enum'
			AND convert(sysname,c2.name) = a.ColumnName
	END
	set @OldTable = (select @Table)
	
   	FETCH NEXT FROM DescriptionCursor into @Table, @Column
END
CLOSE DescriptionCursor
DEALLOCATE DescriptionCursor
/* selecting the data */
DELETE FROM @ItemList  WHERE ForeignKeyTable IS NULL
/* transferring data to result table */
insert into @MenuList (TableName, ColumnName,  ForeignKeyTable, ForeignKeyColumn)
SELECT     TableName, ColumnName,  ForeignKeyTable, ForeignKeyColumn FROM @ItemList
WHERE ForeignKeyTable  NOT LIKE '[_]%'
AND ForeignKeyTable <> TableName
return
END

GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuInferiorTables]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuInferiorTables] (@TableName varchar(50))  
RETURNS @TableList TABLE (
   TableName varchar(50) )
/*Returns a result set that lists all the Tables which are chiild tables to the querried table.*/
AS
BEGIN
-- table variable to hold accumulated results
   DECLARE @TempList TABLE (
   TableName varchar(50) not null)
/* inserting the Data into the table */
insert into @TempList (TableName)
select i.Table_name from information_Schema.Table_Constraints s,  information_Schema.Table_Constraints i, information_Schema.Referential_Constraints r
where s.Constraint_Type = 'Primary key'
and i.Constraint_name = r.Constraint_name
and r.Unique_constraint_name = s.Constraint_name
and i.Table_Name not like '%Enum'
and i.Table_Name not like '[_]%'
and i.Table_Name <> s.Table_Name
and s.Table_name = @TableName
DECLARE @Table as varchar(50)
DECLARE TableCursor  CURSOR for
select TableName from @TempList
open TableCursor
FETCH next from TableCursor into @Table
WHILE @@FETCH_STATUS = 0
BEGIN
   	-- This is executed as long as the previous fetch succeeds.
	--insert into @TempList (TableName) select TableName from SearchMenuInferiorTables (@Table) -- Fehler bei Schachtelungstiefe
   	FETCH NEXT FROM TableCursor into @Table
END
CLOSE TableCursor
DEALLOCATE TableCursor
--insert into @TempList (TableName) values (@TableName)
/* transferring data to result table */
insert into @TableList (TableName)
SELECT   distinct  TableName FROM @TempList
return
END


GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuOrderColumn]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuOrderColumn] (@TableName varchar(50))  
RETURNS @ColumnList TABLE (
   ColumnName varchar(50) not null)
/*Returns a result set that lists all the Tables and Collections connected to the querried item.*/
AS
BEGIN
/* inserting the Data into the table */
INSERT INTO @ColumnList (ColumnName)
SELECT Column_Name FROM information_Schema.Columns 
WHERE Table_Name = @TableName
AND Data_Type IN ('char', 'nchar', 'varchar', 'nvarchar', 'text', 'ntext','bit', 'tinyint', 'smallint', 'int', 'bigint', 'real', 'float', 'money','smallmoney','decimal', 'numeric', 'datetime', 'smalldatetime')
union select ''
ORDER BY Column_Name
return
END



GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuSuperiorTables]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuSuperiorTables] (@TableName varchar(50))  
RETURNS @TableList TABLE (
   TableName varchar(50) )
/*Returns a result set that lists all the Tables which are chiild tables to the querried table.*/
AS
BEGIN
-- table variable to hold accumulated results
   DECLARE @TempList TABLE (
   TableName varchar(50) not null)
/* inserting the Data into the table */
insert into @TempList (TableName) 
select i.Table_name from information_Schema.Table_Constraints s,  information_Schema.Table_Constraints i, information_Schema.Referential_Constraints r
where s.Constraint_Type = 'Foreign key'
and s.Constraint_name = r.Constraint_name
and r.Unique_constraint_name = i.Constraint_name
and i.Table_Name not like '%Enum'
and i.Table_Name not like '[_]%'
and i.Table_Name <> s.Table_Name
and s.Table_name = @TableName
DECLARE @Table as varchar(50)
DECLARE TableCursor  CURSOR for
select TableName from @TempList
open TableCursor

-- Fehler durch Schachtelungstiefe
declare @i int
set @i = 0

FETCH next from TableCursor into @Table
WHILE @@FETCH_STATUS = 0
BEGIN
   	-- This is executed as long as the previous fetch succeeds.
	if @i = 0 begin
	--insert into @TempList (TableName) select TableName from SearchMenuSuperiorTables (@Table) -- Fehler durch Schachtelungstiefe?
	set @i = @i + 1
	end
   	FETCH NEXT FROM TableCursor into @Table
END
CLOSE TableCursor
DEALLOCATE TableCursor

/* transferring data to result table */
insert into @TableList (TableName)
SELECT   distinct  TableName FROM @TempList
return
END


GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuTableTabs]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuTableTabs] (@TableName varchar(50))  
RETURNS @TableList TABLE (
   TableName varchar(50),
   TableTab nvarchar(50) )
/*Returns a result set that lists all the Tables which are chiild tables to the querried table.*/
AS
BEGIN
-- table variable to hold accumulated results
   DECLARE @TempList TABLE (
   TableName varchar(50) not null,
   TableTab nvarchar(50) null)
/* inserting the Data into the table */
insert into @TempList (TableName) select TableName from SearchMenuInferiorTables (@TableName) -- Fehler bei Schachtelungstiefe
insert into @TempList (TableName) select TableName from SearchMenuSuperiorTables (@TableName)
insert into @TempList (TableName) values (@TableName)
update @TempList set TableTab = case when TableName like 'Collection%' and len(TableName) > 10  then RTRIM( substring(TableName, 11, 50)) else 
		case when TableName like 'Identification%' then  RTRIM( 'Identification ' + lower(substring(TableName, 15, 50)))  else TableName end end
/* transferring data to result table */
insert into @TableList (TableName, TableTab)
SELECT   distinct  TableName, TableTab FROM @TempList
return
END



GO
/****** Object:  UserDefinedFunction [dbo].[SuperiorAgentList]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[SuperiorAgentList] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[ReplacedWithAgentID] [int] NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	DisplayOrder int NOT NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
Test:
select * from dbo.SuperiorAgentList(12665)
*/
AS
BEGIN
	declare @i int
	set @i = 1
	while (not @AgentID is null)
		begin

			INSERT @AgentList
			SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
			ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
			ReplacedWithAgentID, RevisionLevel, @i
			FROM Agent
			WHERE Agent.AgentID = @AgentID

			set @AgentID = (select AgentParentID from Agent where AgentID = @AgentID)

			set @i = (select @i + 1)

		end

   RETURN
END
GO
GRANT SELECT ON [dbo].[SuperiorAgentList] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[UserID]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE FUNCTION [dbo].[UserID] () RETURNS int AS 
BEGIN  
declare @ID int;  
SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = SUSER_SNAME()) 
RETURN @ID  
END 
GO
GRANT EXECUTE ON [dbo].[UserID] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[Version]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE ON [dbo].[Version] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[VersionClient]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '04.01.00' 
END

GO
GRANT EXECUTE ON [dbo].[VersionClient] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AddressType_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_AddressType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[AddressType_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[Agent]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agent](
	[AgentID] [int] IDENTITY(1,1) NOT NULL,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[GivenNamePostfix] [nvarchar](50) NULL,
	[InheritedNamePrefix] [nvarchar](50) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[InheritedNamePostfix] [nvarchar](50) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[AgentGender] [nvarchar](50) NULL,
	[Description] [nvarchar](1000) NULL,
	[OriginalSpelling] [nvarchar](200) NULL,
	[Notes] [nvarchar](max) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[IgnoreButKeepForReference] [bit] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[Agent] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[Agent] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Agent] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Agent] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[Agent_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agent_log](
	[AgentID] [int] NULL,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NULL,
	[Version] [smallint] NULL,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[GivenNamePostfix] [nvarchar](50) NULL,
	[InheritedNamePrefix] [nvarchar](50) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[InheritedNamePostfix] [nvarchar](50) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[AgentGender] [nvarchar](50) NULL,
	[Description] [nvarchar](1000) NULL,
	[OriginalSpelling] [nvarchar](200) NULL,
	[Notes] [nvarchar](max) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[IgnoreButKeepForReference] [bit] NULL,
 CONSTRAINT [PK_Agent_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[Agent_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Agent_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentContactInformation]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentContactInformation](
	[AgentID] [int] NOT NULL,
	[DisplayOrder] [tinyint] NOT NULL,
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
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_AgentAddress] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentContactInformation] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentContactInformation] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentContactInformation] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentContactInformation] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentContactInformation_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentContactInformation_log](
	[AgentID] [int] NULL,
	[DisplayOrder] [tinyint] NULL,
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
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
 CONSTRAINT [PK_AgentContactInformation_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[AgentContactInformation_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentContactInformation_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentExternalDatabase]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentExternalDatabase](
	[ExternalDatabaseID] [int] NOT NULL,
	[ExternalDatabaseName] [nvarchar](100) NULL,
	[ExternalDatabaseVersion] [nvarchar](255) NULL,
	[Rights] [nvarchar](500) NULL,
	[ExternalDatabaseAuthors] [nvarchar](200) NULL,
	[ExternalDatabaseURI] [nvarchar](300) NULL,
	[ExternalDatabaseInstitution] [nvarchar](300) NULL,
	[InternalNotes] [nvarchar](1500) NULL,
	[ExternalAttribute_AgentID] [nvarchar](255) NULL,
	[PreferredSequence] [tinyint] NULL,
	[Disabled] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_AgentExternalDatabase] PRIMARY KEY CLUSTERED 
(
	[ExternalDatabaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[AgentExternalDatabase] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentExternalDatabase_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentExternalDatabase_log](
	[ExternalDatabaseID] [int] NULL,
	[ExternalDatabaseName] [nvarchar](100) NULL,
	[ExternalDatabaseVersion] [nvarchar](255) NULL,
	[Rights] [nvarchar](500) NULL,
	[ExternalDatabaseAuthors] [nvarchar](200) NULL,
	[ExternalDatabaseURI] [nvarchar](300) NULL,
	[ExternalDatabaseInstitution] [nvarchar](300) NULL,
	[InternalNotes] [nvarchar](1500) NULL,
	[ExternalAttribute_AgentID] [nvarchar](255) NULL,
	[PreferredSequence] [tinyint] NULL,
	[Disabled] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentExternalDatabase_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgentExternalID]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentExternalID](
	[AgentID] [int] NOT NULL,
	[ExternalDatabaseID] [int] NOT NULL,
	[ExternalAgentURI] [varchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_AgentExternalID] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[ExternalDatabaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentExternalID] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentExternalID] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentExternalID] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentExternalID] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentExternalID_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentExternalID_log](
	[AgentID] [int] NULL,
	[ExternalDatabaseID] [int] NULL,
	[ExternalAgentURI] [varchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentExternalID_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgentGender_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentGender_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_AgentGender_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[AgentGender_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentHierarchyOther]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentHierarchyOther](
	[AgentID] [int] NOT NULL,
	[HierarchyID] [int] IDENTITY(1,1) NOT NULL,
	[AgentParentID] [int] NOT NULL,
	[ValidFrom] [datetime] NULL,
	[ValidUntil] [datetime] NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_AgentHierarchyOther] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[HierarchyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentHierarchyOther] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentHierarchyOther] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentHierarchyOther] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentHierarchyOther] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentHierarchyOther_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentHierarchyOther_log](
	[AgentID] [int] NULL,
	[HierarchyID] [int] NULL,
	[AgentParentID] [int] NULL,
	[ValidFrom] [datetime] NULL,
	[ValidUntil] [datetime] NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentHierarchyOther_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[AgentHierarchyOther_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentHierarchyOther_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentIdentifier]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentIdentifier](
	[AgentID] [int] NOT NULL,
	[Identifier] [nvarchar](400) NOT NULL,
	[IdentifierURI] [varchar](500) NULL,
	[Type] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_AgentIdentifier] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentIdentifier] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentIdentifier] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentIdentifier] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentIdentifier] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentIdentifier_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentIdentifier_log](
	[AgentID] [int] NULL,
	[Identifier] [nvarchar](400) NULL,
	[IdentifierURI] [varchar](500) NULL,
	[Type] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentIdentifier_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[AgentIdentifier_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentIdentifier_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentIdentifierType_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentIdentifierType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[URL] [nvarchar](500) NULL,
 CONSTRAINT [PK_AgentIdentifierType_Enum_1] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentIdentifierType_Enum] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentIdentifierType_Enum] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentIdentifierType_Enum] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentIdentifierType_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentImage]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentImage](
	[AgentID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[Sequence] [int] NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
 CONSTRAINT [PK_AgentImage] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[URI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentImage] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentImage] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentImage] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentImage] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentImage_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentImage_log](
	[AgentID] [int] NULL,
	[URI] [varchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[Sequence] [int] NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
 CONSTRAINT [PK_AgentImage_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[AgentImage_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentImage_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentImageType_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentImageType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[URL] [nvarchar](500) NULL,
 CONSTRAINT [PK_AgentImageType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgentKeyword]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentKeyword](
	[AgentID] [int] NOT NULL,
	[Keyword] [nvarchar](200) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_AgentKeyword] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[Keyword] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentKeyword] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentKeyword] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentKeyword] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentKeyword] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentKeyword_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentKeyword_log](
	[AgentID] [int] NULL,
	[Keyword] [nvarchar](200) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentKeyword_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[AgentKeyword_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentKeyword_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentNameDisplayType_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentNameDisplayType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_AgentNameDisplayType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[AgentNameDisplayType_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentProject]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentProject](
	[AgentID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_AgentProject] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentProject] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT DELETE ON [dbo].[AgentProject] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentProject] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentProject] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentProject_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentProject_log](
	[AgentID] [int] NULL,
	[ProjectID] [int] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentProject_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[AgentProject_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentProject_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentReference]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentReference](
	[AgentID] [int] NOT NULL,
	[ReferenceTitle] [nvarchar](255) NOT NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[ContainsImage] [tinyint] NULL,
	[ContainsReferencelist] [tinyint] NULL,
	[Notes] [nvarchar](255) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_AgentReference] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[ReferenceTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentReference] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentReference] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentReference] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentReference] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentReference_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentReference_log](
	[AgentID] [int] NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[ContainsImage] [tinyint] NULL,
	[ContainsReferencelist] [tinyint] NULL,
	[Notes] [nvarchar](255) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentReference_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[AgentReference_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentReference_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentRelation]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentRelation](
	[AgentID] [int] NOT NULL,
	[RelatedAgentID] [int] NOT NULL,
	[RelationType] [nvarchar](50) NULL,
	[Notes] [nvarchar](255) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
 CONSTRAINT [PK_AgentRelation_1] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[RelatedAgentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[AgentRelation] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[AgentRelation] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AgentRelation] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentRelation] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentRelation_log]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentRelation_log](
	[AgentID] [int] NULL,
	[RelatedAgentID] [int] NULL,
	[RelationType] [nvarchar](50) NULL,
	[Notes] [nvarchar](255) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentRelation_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[AgentRelation_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[AgentRelation_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentRelationType_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentRelationType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_AgentRelationType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[AgentRelationType_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentSynonymisationType_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentSynonymisationType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_AgentSynonymisationType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[AgentSynonymisationType_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentTitle_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentTitle_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_AgentTitle_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[AgentTitle_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[AgentType_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_AgentType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[AgentType_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ApplicationEntityDescription]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationEntityDescription](
	[TableName] [varchar](50) NOT NULL,
	[ColumnName] [varchar](50) NOT NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](1000) NULL,
	[LanguageCode] [nvarchar](50) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ApplicationEntityDescription] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationSearchSelectionStrings]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationSearchSelectionStrings](
	[UserName] [varchar](50) NOT NULL,
	[SQLStringIdentifier] [varchar](50) NOT NULL,
	[ItemTable] [varchar](50) NULL,
	[SQLString] [varchar](2000) NULL,
	[Description] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ApplicationSearchSelectionStrings] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC,
	[SQLStringIdentifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ApplicationSearchSelectionStrings] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[ApplicationSearchSelectionStrings] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ApplicationSearchSelectionStrings] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[ApplicationSearchSelectionStrings] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectProxy]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectProxy](
	[ProjectID] [int] NOT NULL,
	[Project] [nvarchar](50) NOT NULL,
	[ProjectURI] [varchar](255) NULL,
	[AgentNameDisplayType] [nvarchar](50) NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL,
 CONSTRAINT [PK_ProjetProxy] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectProxy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectUser]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectUser](
	[LoginName] [nvarchar](50) NOT NULL,
	[ProjectID] [int] NOT NULL,
 CONSTRAINT [PK_ProjectUser] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectUser] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectUser] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectUser] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectUser] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[RevisionLevel_Enum]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RevisionLevel_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_RevisionLevel_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[RevisionLevel_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[UserProxy]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProxy](
	[LoginName] [nvarchar](50) NOT NULL,
	[CombinedNameCache] [nvarchar](50) NULL,
	[UserURI] [varchar](255) NULL,
	[ProjectID] [int] NULL,
	[Queries] [xml] NULL,
	[AgentURI] [varchar](255) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PrivacyConsent] [bit] NULL,
	[PrivacyConsentDate] [datetime] NULL,
 CONSTRAINT [PK_UserProxy] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[UserProxy] TO [DiversityWorkbenchUser] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserProxy] ([Queries]) TO [DiversityWorkbenchUser] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsent]) TO [DiversityWorkbenchUser] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsentDate]) TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewAgentAddress0]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewAgentAddress0]
AS
SELECT        AgentID, DisplayOrder, CAST(NULL AS varchar(200)) AS ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, 
                         ValidUntil
FROM            dbo.AgentContactInformation AS C
WHERE        EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE        (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO
GRANT SELECT ON [dbo].[ViewAgentAddress0] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewAgentAddress1]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewAgentAddress1]
AS
SELECT        A.AgentID, C.DisplayOrder, A.AgentName AS ParentName, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, C.CellularPhone, C.Telefax, C.Email, C.URI, 
                         C.Notes, C.ValidFrom, C.ValidUntil
FROM            dbo.Agent AS A INNER JOIN
                         dbo.AgentContactInformation AS C ON A.AgentParentID = C.AgentID
WHERE        EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE        (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO
GRANT SELECT ON [dbo].[ViewAgentAddress1] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewAgentAddress2]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewAgentAddress2]
AS
SELECT        A.AgentID, C.DisplayOrder, A2.AgentName AS ParentName, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, C.CellularPhone, C.Telefax, C.Email, C.URI, 
                         C.Notes, C.ValidFrom, C.ValidUntil
FROM            dbo.Agent AS A INNER JOIN
                         dbo.Agent AS A2 ON A.AgentParentID = A2.AgentID INNER JOIN
                         dbo.AgentContactInformation AS C ON A2.AgentParentID = C.AgentID
WHERE        EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE        (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO
GRANT SELECT ON [dbo].[ViewAgentAddress2] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewAgentAddress3]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewAgentAddress3]
AS
SELECT        A1.AgentID, C.DisplayOrder, A3.AgentName AS ParentName, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, C.CellularPhone, C.Telefax, C.Email, C.URI, 
                         C.Notes, C.ValidFrom, C.ValidUntil
FROM            dbo.Agent AS A1 INNER JOIN
                         dbo.Agent AS A2 ON A1.AgentParentID = A2.AgentID INNER JOIN
                         dbo.Agent AS A3 ON A2.AgentParentID = A3.AgentID INNER JOIN
                         dbo.AgentContactInformation AS C ON A3.AgentParentID = C.AgentID
WHERE        EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE        (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO
GRANT SELECT ON [dbo].[ViewAgentAddress3] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewAgentAddress4]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewAgentAddress4]
AS
SELECT        A1.AgentID, C.DisplayOrder, A4.AgentName AS ParentName, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, C.CellularPhone, C.Telefax, C.Email, C.URI, 
                         C.Notes, C.ValidFrom, C.ValidUntil
FROM            dbo.Agent AS A1 INNER JOIN
                         dbo.Agent AS A2 ON A1.AgentParentID = A2.AgentID INNER JOIN
                         dbo.Agent AS A3 ON A2.AgentParentID = A3.AgentID INNER JOIN
                         dbo.Agent AS A4 ON A3.AgentParentID = A4.AgentID INNER JOIN
                         dbo.AgentContactInformation AS C ON A4.AgentParentID = C.AgentID
WHERE        EXISTS
                             (SELECT        AgentID
                               FROM            dbo.AgentContactInformation AS M
                               WHERE        (AgentID = C.AgentID) AND (ValidFrom <= GETDATE() OR
                                                         ValidFrom IS NULL) AND (ValidUntil IS NULL OR
                                                         ValidUntil >= GETDATE())
                               GROUP BY AgentID
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))
GO
GRANT SELECT ON [dbo].[ViewAgentAddress4] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewAgentAddress]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewAgentAddress]
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
CASE WHEN A0.ValidUntil IS NULL THEN CASE WHEN A1.ValidUntil IS NULL THEN case when a2.ValidUntil is null then case when a3.ValidUntil is null then a4.ValidUntil else a3.ValidUntil end else a2.ValidUntil end ELSE A1.ValidUntil end ELSE A0.ValidUntil END AS ValidUntil
FROM dbo.ViewAgentAddress4 AS A4 FULL OUTER JOIN
dbo.ViewAgentAddress3 AS A3 ON A4.AgentID = A3.AgentID FULL OUTER JOIN
dbo.ViewAgentAddress2 AS A2 ON A3.AgentID = A2.AgentID FULL OUTER JOIN
dbo.ViewAgentAddress1 AS A1 ON A2.AgentID = A1.AgentID FULL OUTER JOIN
dbo.ViewAgentAddress0 AS A0 ON A1.AgentID = A0.AgentID
GO
GRANT SELECT ON [dbo].[ViewAgentAddress] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[AgentID_UserAvailable]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   AgentID_UserAvailable  ######################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[AgentID_UserAvailable]
AS
SELECT dbo.AgentProject.AgentID
FROM         dbo.AgentProject INNER JOIN
                      dbo.ProjectUser ON dbo.AgentProject.ProjectID = dbo.ProjectUser.ProjectID
WHERE     (dbo.ProjectUser.LoginName = USER_NAME())
UNION
SELECT AgentID FROM Agent 
WHERE AgentID NOT IN (SELECT AgentID FROM AgentProject)

GO
GRANT SELECT ON [dbo].[AgentID_UserAvailable] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[Agent_Core]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   Agent_Core: Adding column DisplayText with indented synonyms    ############################################
--#####################################################################################################################

CREATE VIEW [dbo].[Agent_Core]
AS
SELECT        A.AgentID, A.AgentParentID, A.AgentName, CASE WHEN A.SynonymToAgentID IS NULL THEN '' ELSE '    ' END + A.AgentName AS DisplayText, A.Version, A.AgentTitle, A.GivenName, A.GivenNamePostfix, A.InheritedNamePrefix, 
                         A.InheritedName, A.InheritedNamePostfix, A.Abbreviation, A.AgentType, A.AgentRole, A.AgentGender, A.Description, A.OriginalSpelling, A.Notes, A.ValidFromDate, A.ValidFromDay, A.ValidFromMonth, A.ValidFromYear, 
                         A.ValidUntilDate, A.ValidUntilDay, A.ValidUntilMonth, A.ValidUntilYear, A.ValidDateSupplement, A.PlaceOfBirth, A.PlaceOfDeath, A.SynonymToAgentID, A.SynonymisationType, A.RevisionLevel, A.DataWithholdingReason,
						 A.IgnoreButKeepForReference
FROM            dbo.Agent AS A INNER JOIN
                         dbo.AgentID_UserAvailable AS U ON A.AgentID = U.AgentID
						 WHERE A.IgnoreButKeepForReference = 0
GO
GRANT SELECT ON [dbo].[Agent_Core] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ApplicationSearchSelectionStrings_Core]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ApplicationSearchSelectionStrings_Core]
AS
SELECT     SQLStringIdentifier, ItemTable, SQLString, Description
FROM         dbo.ApplicationSearchSelectionStrings
WHERE     (UserName = USER_NAME())
GO
GRANT SELECT ON [dbo].[ApplicationSearchSelectionStrings_Core] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ProjectList]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProjectList]
AS
SELECT     dbo.ProjectProxy.ProjectID, dbo.ProjectProxy.Project
FROM         dbo.ProjectUser INNER JOIN
                      dbo.ProjectProxy ON dbo.ProjectUser.ProjectID = dbo.ProjectProxy.ProjectID
WHERE     (dbo.ProjectUser.LoginName = USER_NAME())
GO
GRANT SELECT ON [dbo].[ProjectList] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicAgent]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicAgent] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicContactInformation]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   PublicContactInformation - Change source to new function AgentContactInformation_Public  ###################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicContactInformation]
AS
SELECT        AgentID, DisplayOrder, ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, LogUpdatedWhen
FROM            dbo.AgentContactInformation_Public() AS AgentContactInformation_Public_1
GO
GRANT SELECT ON [dbo].[PublicContactInformation] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicContactInformation0]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicContactInformation0] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicContactInformation1]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicContactInformation1] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicContactInformation2]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicContactInformation2] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicContactInformation3]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicContactInformation3] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicContactInformation4]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicContactInformation4] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicImage]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicImage] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicReference]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicReference] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PublicRelation]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GRANT SELECT ON [dbo].[PublicRelation] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[UserInfo]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[UserInfo]
AS
SELECT     LoginName, CombinedNameCache, UserURI, ProjectID
FROM         dbo.UserProxy
WHERE     (LoginName = USER_NAME())
GO
GRANT SELECT ON [dbo].[UserInfo] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserInfo] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[UserInfo] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewAgentNames]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewAgentNames]
AS
SELECT * FROM [dbo].[AgentNames] (
  [dbo].[DefaultAgentNameDisplayType] ())
GO
GRANT SELECT ON [dbo].[ViewAgentNames] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewBaseURL]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewBaseURL]
	AS
	SELECT        dbo.BaseURL() AS BaseURL
GO
GRANT SELECT ON [dbo].[ViewBaseURL] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewDefaultAgentNameDisplayType]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewDefaultAgentNameDisplayType]
AS
SELECT [dbo].[DefaultAgentNameDisplayType] () AS DefaultAgentNameDisplayType
GO
GRANT SELECT ON [dbo].[ViewDefaultAgentNameDisplayType] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewDiversityWorkbenchModule]    Script Date: 07.04.2021 12:23:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewDiversityWorkbenchModule]
AS
SELECT dbo.DiversityWorkbenchModule() AS DiversityWorkbenchModule
GO
GRANT SELECT ON [dbo].[ViewDiversityWorkbenchModule] TO [DiversityWorkbenchUser] AS [dbo]
GO
INSERT [dbo].[AddressType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'office', N'office address', N'office', 0, 1, NULL, NULL)
GO
INSERT [dbo].[AddressType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'private', N'private address', N'private', 1, 1, NULL, NULL)
GO
INSERT [dbo].[AgentGender_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'female', NULL, N'female', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentGender_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'male', NULL, N'male', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentIdentifierType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [URL]) VALUES (N'ISNI', N'International Standard Name Identifier (ISO 27729)', N'ISNI', NULL, 1, NULL, NULL, N'http://www.isni.org/')
GO
INSERT [dbo].[AgentIdentifierType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [URL]) VALUES (N'ORCID', N'Open Researcher Contributor Identification', N'ORCID', NULL, 1, NULL, NULL, N'https://orcid.org/')
GO
INSERT [dbo].[AgentIdentifierType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [URL]) VALUES (N'ROR', N'Research Organization Registry', N'ROR', NULL, 1, NULL, NULL, N'https://ror.org/')
GO
INSERT [dbo].[AgentImageType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [URL]) VALUES (N'Logo', N'Logo', N'Logo', NULL, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[AgentImageType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [URL]) VALUES (N'Portrait', N'Portrait', N'Portrait', NULL, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[AgentNameDisplayType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Abbr', N'The international defined standard arbbreviation; e.g. L. for Linne', N'Abbreviation', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentNameDisplayType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'G. Ii', N'Given name. Inherited name; e.g. H. Hertel', N'Given. InheritedName', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentNameDisplayType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Gg Ii', N'Given name InheritedName; e.g. Hannes Hertel', N'GivenName InheritedName', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentNameDisplayType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Ii, G.', N'Inherited name, GivenName; e.g. Hertel, H.', N'InheritedName, Given.', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentNameDisplayType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Ii, Gg', N'Inherited name, GivenName; e.g. Hertel, Hannes', N'InheritedName, GivenName', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentNameDisplayType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Tt. G. Ii', N'Titel Given name. Inherited name; e.g. Prof. H. Hertel', N'Titel Given. InheritedName', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentNameDisplayType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Tt. Gg Ii', N'Titel Given name Inherited name; e.g. Prof. Hannes Hertel', N'Titel GivenName InheritedName', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentRelationType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Child of', NULL, N'Child of', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentRelationType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Married to', NULL, N'Married to', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentRelationType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Parent of', NULL, N'Parent of', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentSynonymisationType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'replaced with', N'if the agent was replaced by another entry', N'replaced with', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentSynonymisationType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'synonym to', N'if the agent is synonym to another agent', N'synonym to', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentTitle_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'', NULL, N'', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentTitle_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Dr.', NULL, N'Dr.', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentTitle_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Frater', NULL, N'Frater', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentTitle_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Pater', NULL, N'Pater', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentTitle_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Prof.', NULL, N'Prof.', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentTitle_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Prof. Dr.', NULL, N'Prof. Dr.', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Collection', NULL, N'Collection', 2, 1, NULL, NULL)
GO
INSERT [dbo].[AgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Company', NULL, N'Company', 4, 1, NULL, NULL)
GO
INSERT [dbo].[AgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Department', NULL, N'Department', 7, 1, NULL, NULL)
GO
INSERT [dbo].[AgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Group', NULL, N'Group', 5, 1, NULL, NULL)
GO
INSERT [dbo].[AgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Institution', NULL, N'Institution', 3, 1, NULL, NULL)
GO
INSERT [dbo].[AgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'Person', NULL, N'Person', 1, 1, NULL, NULL)
GO
INSERT [dbo].[AgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'University', NULL, N'University', 6, 1, NULL, NULL)
GO
INSERT [dbo].[ProjectProxy] ([ProjectID], [Project], [ProjectURI], [AgentNameDisplayType], [CreateArchive], [ArchiveProtocol]) VALUES (0, N'DiversityWorkbench', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[ProjectUser] ([LoginName], [ProjectID]) VALUES (N'dbo', 0)
GO
INSERT [dbo].[RevisionLevel_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'checked', N'checked', N'checked', 20, 1, NULL, NULL)
GO
INSERT [dbo].[RevisionLevel_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'final revision', N'final revision', N'final revision', 40, 1, NULL, NULL)
GO
INSERT [dbo].[RevisionLevel_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'review required', N'review required', N'review required', 10, 1, NULL, NULL)
GO
INSERT [dbo].[RevisionLevel_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'to be deleted', N'to be deleted', N'to be deleted', 50, 1, NULL, NULL)
GO
INSERT [dbo].[RevisionLevel_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode]) VALUES (N'unchecked', N'unchecked', N'unchecked', 1, 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[UserProxy] ON 
;
INSERT [dbo].[UserProxy] ([LoginName], [CombinedNameCache], [UserURI], [ProjectID], [Queries], [AgentURI], [ID], [PrivacyConsent], [PrivacyConsentDate]) VALUES (N'dbo', N'Owner of database', NULL, NULL, NULL, NULL, 1, NULL, NULL)
;
SET IDENTITY_INSERT [dbo].[UserProxy] OFF
GO
/****** Object:  Index [IdxIgnoreButKeepForReference]    Script Date: 07.04.2021 12:23:26 ******/
CREATE NONCLUSTERED INDEX [IdxIgnoreButKeepForReference] ON [dbo].[Agent]
(
	[IgnoreButKeepForReference] ASC
)
INCLUDE([AgentID],[SynonymToAgentID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AgentName]    Script Date: 07.04.2021 12:23:26 ******/
CREATE NONCLUSTERED INDEX [IX_AgentName] ON [dbo].[Agent]
(
	[AgentName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_AgentName]  DEFAULT ('') FOR [AgentName]
GO
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_Version]  DEFAULT ((1)) FOR [Version]
GO
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_AgentType]  DEFAULT (N'person') FOR [AgentType]
GO
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_RevisionLevel]  DEFAULT (N'unchecked') FOR [RevisionLevel]
GO
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_IgnoreButKeepForReference]  DEFAULT ((0)) FOR [IgnoreButKeepForReference]
GO
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF__Agent__LogUpdate__14D10B8B]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[Agent_log] ADD  CONSTRAINT [DF_Agent_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[Agent_log] ADD  CONSTRAINT [DF_Agent_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[Agent_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentContactInformation] ADD  CONSTRAINT [DF_AgentContactInformation_DataWithholdingReason]  DEFAULT (N'Withhold by default') FOR [DataWithholdingReason]
GO
ALTER TABLE [dbo].[AgentContactInformation] ADD  CONSTRAINT [DF__AgentCont__LogUp__16B953FD]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[AgentContactInformation] ADD  CONSTRAINT [DF_AgentContactInformation_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[AgentContactInformation_log] ADD  CONSTRAINT [DF_AgentContactInformation_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentContactInformation_log] ADD  CONSTRAINT [DF_AgentContactInformation_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentContactInformation_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentExternalDatabase_log] ADD  CONSTRAINT [DF_AgentExternalDatabase_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentExternalDatabase_log] ADD  CONSTRAINT [DF_AgentExternalDatabase_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentExternalDatabase_log] ADD  CONSTRAINT [DF_AgentExternalDatabase_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentExternalID] ADD  CONSTRAINT [DF_AgentExternalID_ExternalDatabaseID]  DEFAULT ('') FOR [ExternalDatabaseID]
GO
ALTER TABLE [dbo].[AgentExternalID] ADD  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[AgentExternalID] ADD  CONSTRAINT [DF_AgentExternalID_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[AgentExternalID] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[AgentExternalID] ADD  CONSTRAINT [DF_AgentExternalID_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[AgentExternalID_log] ADD  CONSTRAINT [DF_AgentExternalID_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentExternalID_log] ADD  CONSTRAINT [DF_AgentExternalID_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentExternalID_log] ADD  CONSTRAINT [DF_AgentExternalID_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[AgentHierarchyOther] ADD  CONSTRAINT [DF_AgentHierarchyOther_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[AgentHierarchyOther_log] ADD  CONSTRAINT [DF_AgentHierarchyOther_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentHierarchyOther_log] ADD  CONSTRAINT [DF_AgentHierarchyOther_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentHierarchyOther_log] ADD  CONSTRAINT [DF_AgentHierarchyOther_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[AgentIdentifier] ADD  CONSTRAINT [DF_AgentIdentifier_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[AgentIdentifier_log] ADD  CONSTRAINT [DF_AgentIdentifier_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentIdentifier_log] ADD  CONSTRAINT [DF_AgentIdentifier_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentIdentifier_log] ADD  CONSTRAINT [DF_AgentIdentifier_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentImage] ADD  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[AgentImage] ADD  CONSTRAINT [DF_AgentImage_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[AgentImage] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[AgentImage] ADD  CONSTRAINT [DF_AgentImage_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[AgentImage] ADD  CONSTRAINT [DF_Agent_Sequence]  DEFAULT ((1)) FOR [Sequence]
GO
ALTER TABLE [dbo].[AgentImage] ADD  CONSTRAINT [DF_AgentImage_DataWithholdingReason]  DEFAULT (N'Withhold by default') FOR [DataWithholdingReason]
GO
ALTER TABLE [dbo].[AgentImage_log] ADD  CONSTRAINT [DF_AgentImage_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentImage_log] ADD  CONSTRAINT [DF_AgentImage_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentImage_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentKeyword] ADD  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[AgentKeyword] ADD  CONSTRAINT [DF_AgentKeyword_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[AgentKeyword] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[AgentKeyword] ADD  CONSTRAINT [DF_AgentKeyword_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[AgentKeyword_log] ADD  CONSTRAINT [DF_AgentKeyword_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentKeyword_log] ADD  CONSTRAINT [DF_AgentKeyword_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentKeyword_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentProject] ADD  CONSTRAINT [DF_AgentProject_ProjectID]  DEFAULT (0) FOR [ProjectID]
GO
ALTER TABLE [dbo].[AgentProject] ADD  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[AgentProject] ADD  CONSTRAINT [DF_AgentProject_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[AgentProject] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[AgentProject] ADD  CONSTRAINT [DF_AgentProject_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[AgentProject_log] ADD  CONSTRAINT [DF_AgentProject_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentProject_log] ADD  CONSTRAINT [DF_AgentProject_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentProject_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentReference_log] ADD  CONSTRAINT [DF_AgentReference_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentReference_log] ADD  CONSTRAINT [DF_AgentReference_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentReference_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[AgentRelation_log] ADD  CONSTRAINT [DF_AgentRelation_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[AgentRelation_log] ADD  CONSTRAINT [DF_AgentRelation_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AgentRelation_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[Agent]  WITH NOCHECK ADD  CONSTRAINT [FK_Agent_Agent] FOREIGN KEY([AgentParentID])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_Agent]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [FK_Agent_AgentGender_Enum] FOREIGN KEY([AgentGender])
REFERENCES [dbo].[AgentGender_Enum] ([Code])
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_AgentGender_Enum]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [FK_Agent_AgentSynonymisationType_Enum] FOREIGN KEY([SynonymisationType])
REFERENCES [dbo].[AgentSynonymisationType_Enum] ([Code])
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_AgentSynonymisationType_Enum]
GO
ALTER TABLE [dbo].[Agent]  WITH NOCHECK ADD  CONSTRAINT [FK_Agent_AgentTitle_Enum] FOREIGN KEY([AgentTitle])
REFERENCES [dbo].[AgentTitle_Enum] ([Code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_AgentTitle_Enum]
GO
ALTER TABLE [dbo].[Agent]  WITH NOCHECK ADD  CONSTRAINT [FK_Agent_AgentType_Enum] FOREIGN KEY([AgentType])
REFERENCES [dbo].[AgentType_Enum] ([Code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_AgentType_Enum]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [FK_Agent_RevisionLevel_Enum] FOREIGN KEY([RevisionLevel])
REFERENCES [dbo].[RevisionLevel_Enum] ([Code])
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [FK_Agent_RevisionLevel_Enum]
GO
ALTER TABLE [dbo].[AgentContactInformation]  WITH CHECK ADD  CONSTRAINT [FK_AgentContactInformation_AddressType_Enum] FOREIGN KEY([AddressType])
REFERENCES [dbo].[AddressType_Enum] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[AgentContactInformation] CHECK CONSTRAINT [FK_AgentContactInformation_AddressType_Enum]
GO
ALTER TABLE [dbo].[AgentContactInformation]  WITH CHECK ADD  CONSTRAINT [FK_AgentContactInformation_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentContactInformation] CHECK CONSTRAINT [FK_AgentContactInformation_Agent]
GO
ALTER TABLE [dbo].[AgentExternalID]  WITH NOCHECK ADD  CONSTRAINT [FK_AgentExternalID_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentExternalID] CHECK CONSTRAINT [FK_AgentExternalID_Agent]
GO
ALTER TABLE [dbo].[AgentExternalID]  WITH CHECK ADD  CONSTRAINT [FK_AgentExternalID_AgentExternalDatabase] FOREIGN KEY([ExternalDatabaseID])
REFERENCES [dbo].[AgentExternalDatabase] ([ExternalDatabaseID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentExternalID] CHECK CONSTRAINT [FK_AgentExternalID_AgentExternalDatabase]
GO
ALTER TABLE [dbo].[AgentHierarchyOther]  WITH CHECK ADD  CONSTRAINT [FK_AgentHierarchyOther_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentHierarchyOther] CHECK CONSTRAINT [FK_AgentHierarchyOther_Agent]
GO
ALTER TABLE [dbo].[AgentHierarchyOther]  WITH CHECK ADD  CONSTRAINT [FK_AgentHierarchyOther_Agent1] FOREIGN KEY([AgentParentID])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[AgentHierarchyOther] CHECK CONSTRAINT [FK_AgentHierarchyOther_Agent1]
GO
ALTER TABLE [dbo].[AgentIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_AgentIdentifier_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentIdentifier] CHECK CONSTRAINT [FK_AgentIdentifier_Agent]
GO
ALTER TABLE [dbo].[AgentIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_AgentIdentifier_AgentIdentifierType_Enum] FOREIGN KEY([Type])
REFERENCES [dbo].[AgentIdentifierType_Enum] ([Code])
GO
ALTER TABLE [dbo].[AgentIdentifier] CHECK CONSTRAINT [FK_AgentIdentifier_AgentIdentifierType_Enum]
GO
ALTER TABLE [dbo].[AgentImage]  WITH CHECK ADD  CONSTRAINT [FK_AgentImage_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentImage] CHECK CONSTRAINT [FK_AgentImage_Agent]
GO
ALTER TABLE [dbo].[AgentImage]  WITH NOCHECK ADD  CONSTRAINT [FK_AgentImage_AgentImageType_Enum] FOREIGN KEY([Type])
REFERENCES [dbo].[AgentImageType_Enum] ([Code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentImage] CHECK CONSTRAINT [FK_AgentImage_AgentImageType_Enum]
GO
ALTER TABLE [dbo].[AgentKeyword]  WITH NOCHECK ADD  CONSTRAINT [FK_AgentKeyword_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentKeyword] CHECK CONSTRAINT [FK_AgentKeyword_Agent]
GO
ALTER TABLE [dbo].[AgentNameDisplayType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_AgentNameDisplayType_Enum_AgentNameDisplayType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[AgentNameDisplayType_Enum] ([Code])
GO
ALTER TABLE [dbo].[AgentNameDisplayType_Enum] CHECK CONSTRAINT [FK_AgentNameDisplayType_Enum_AgentNameDisplayType_Enum]
GO
ALTER TABLE [dbo].[AgentProject]  WITH NOCHECK ADD  CONSTRAINT [FK_AgentProject_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentProject] CHECK CONSTRAINT [FK_AgentProject_Agent]
GO
ALTER TABLE [dbo].[AgentProject]  WITH CHECK ADD  CONSTRAINT [FK_AgentProject_ProjetProxy] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectProxy] ([ProjectID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[AgentProject] CHECK CONSTRAINT [FK_AgentProject_ProjetProxy]
GO
ALTER TABLE [dbo].[AgentReference]  WITH CHECK ADD  CONSTRAINT [FK_AgentReference_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentReference] CHECK CONSTRAINT [FK_AgentReference_Agent]
GO
ALTER TABLE [dbo].[AgentRelation]  WITH CHECK ADD  CONSTRAINT [FK_AgentRelation_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentRelation] CHECK CONSTRAINT [FK_AgentRelation_Agent]
GO
ALTER TABLE [dbo].[AgentRelation]  WITH CHECK ADD  CONSTRAINT [FK_AgentRelation_AgentRelationType_Enum] FOREIGN KEY([RelationType])
REFERENCES [dbo].[AgentRelationType_Enum] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[AgentRelation] CHECK CONSTRAINT [FK_AgentRelation_AgentRelationType_Enum]
GO
ALTER TABLE [dbo].[AgentSynonymisationType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_AgentSynonymisationType_Enum_AgentSynonymisationType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[AgentSynonymisationType_Enum] ([Code])
GO
ALTER TABLE [dbo].[AgentSynonymisationType_Enum] CHECK CONSTRAINT [FK_AgentSynonymisationType_Enum_AgentSynonymisationType_Enum]
GO
ALTER TABLE [dbo].[ProjectProxy]  WITH CHECK ADD  CONSTRAINT [FK_ProjectProxy_AgentNameDisplayType_Enum] FOREIGN KEY([AgentNameDisplayType])
REFERENCES [dbo].[AgentNameDisplayType_Enum] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ProjectProxy] CHECK CONSTRAINT [FK_ProjectProxy_AgentNameDisplayType_Enum]
GO
ALTER TABLE [dbo].[ProjectUser]  WITH NOCHECK ADD  CONSTRAINT [FK_ProjectUser_ProjetProxy] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectProxy] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_ProjetProxy]
GO
ALTER TABLE [dbo].[ProjectUser]  WITH CHECK ADD  CONSTRAINT [FK_ProjectUser_UserProxy] FOREIGN KEY([LoginName])
REFERENCES [dbo].[UserProxy] ([LoginName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_UserProxy]
GO
ALTER TABLE [dbo].[UserProxy]  WITH CHECK ADD  CONSTRAINT [FK_UserProxy_ProjectProxy] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectProxy] ([ProjectID])
GO
ALTER TABLE [dbo].[UserProxy] CHECK CONSTRAINT [FK_UserProxy_ProjectProxy]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidFromDay] CHECK  (([ValidFromDay]>(0) AND [ValidFromDay]<(32)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidFromDay]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidFromMonth] CHECK  (([ValidFromMonth]>(0) AND [ValidFromMonth]<(13)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidFromMonth]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidFromYear] CHECK  (([ValidFromYear]>(0) AND [ValidFromYear]<(datepart(year,getdate())+(2))))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidFromYear]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidUntilDay] CHECK  (([ValidUntilDay]>(0) AND [ValidUntilDay]<(32)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidUntilDay]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidUntilMonth] CHECK  (([ValidUntilMonth]>(0) AND [ValidUntilMonth]<(13)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidUntilMonth]
GO
ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidUntilYear] CHECK  (([ValidUntilYear]>(0) AND [ValidUntilYear]<(datepart(year,getdate())+(100))))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidUntilYear]
GO
/****** Object:  StoredProcedure [dbo].[procInsertAgentCopy]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[procInsertAgentCopy] 
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
GRANT EXECUTE ON [dbo].[procInsertAgentCopy] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  StoredProcedure [dbo].[procInsertAgentNew]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[procInsertAgentNew] 
	(@AgentID int output,
	 @AgentName nvarchar  (500)  )
AS
INSERT INTO Agent (AgentName)
VALUES (@AgentName)
SELECT @AgentID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])
SELECT @AgentID
GO
/****** Object:  StoredProcedure [dbo].[procSetVersionAgent]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[procSetVersionAgent]  (@ID int) 
AS 
/*  Setting the version of a dataset.  */ 
/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.1 */ 
/*  Date: 20.09.2006  */ 

DECLARE @NextVersion int 
DECLARE @CurrentVersion int 
DECLARE @LastUser nvarchar(500) 
DECLARE @LastUpdate datetime 
DECLARE @UpdatePeriod int 

set @LastUpdate = (SELECT LogUpdatedWhen FROM Agent WHERE AgentID = @ID) 
set @UpdatePeriod = (SELECT DateDiff(hour, @LastUpdate, getdate())) 
set @LastUser = (SELECT LogUpdatedBy FROM Agent WHERE AgentID = @ID) 

set @CurrentVersion = (select Version from Agent where AgentID = @ID) 
if @CurrentVersion is null begin set @CurrentVersion = 0 end 
set @NextVersion = @CurrentVersion 
if not @ID is null and (@LastUser <> User_name() or @UpdatePeriod > 24) 
begin  
    set @NextVersion = @CurrentVersion + 1 
    update Agent set Version = @NextVersion 
    where AgentID = @ID 
select @NextVersion  
END
GO
GRANT EXECUTE ON [dbo].[procSetVersionAgent] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Trigger [dbo].[trgAgentName]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgAgentName] ON [dbo].[Agent] 
FOR INSERT
AS
update Agent
set Agent.AgentName = 
case when inserted.InheritedNamePrefix is null or inserted.InheritedNamePrefix = '' then '' else inserted.InheritedNamePrefix + ' ' end +
case when inserted.Inheritedname is null then '' else inserted.Inheritedname end + 
case when inserted.InheritedNamePostfix is null or inserted.InheritedNamePostfix = '' then '' else ' ' + inserted.InheritedNamePostfix end +
case when inserted.GivenName is null or inserted.GivenName = '' then '' else 
	case when inserted.Inheritedname is null or inserted.Inheritedname = '' then '' else ', ' end + inserted.GivenName end +
case when inserted.GivenNamePostfix is null or inserted.GivenNamePostfix = '' then '' else ' ' + inserted.GivenNamePostfix end +
case when inserted.AgentTitle is null then '' else ', ' + inserted.AgentTitle end
from Agent, inserted
where inserted.AgentType = 'person'
and Agent.AgentID = inserted.AgentID
GO
ALTER TABLE [dbo].[Agent] DISABLE TRIGGER [trgAgentName]
GO
/****** Object:  Trigger [dbo].[trgDelAgent]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelAgent] ON [dbo].[Agent] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.7 */ 
/*  Date: 01.02.2011  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Agent_Log (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath, SynonymToAgentID, SynonymisationType, RevisionLevel, DataWithholdingReason, IgnoreButKeepForReference, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT D.AgentID, D.AgentParentID, D.AgentName, D.Version, D.AgentTitle, D.GivenName, D.GivenNamePostfix, D.InheritedNamePrefix, D.InheritedName, D.InheritedNamePostfix, D.Abbreviation, D.AgentType, D.AgentRole, D.AgentGender, D.Description, D.OriginalSpelling, D.Notes, D.ValidFromDate, D.ValidFromDay, D.ValidFromMonth, D.ValidFromYear, D.ValidUntilDate, D.ValidUntilDay, D.ValidUntilMonth, D.ValidUntilYear, D.ValidDateSupplement, D.PlaceOfBirth, D.PlaceOfDeath, D.SynonymToAgentID, D.SynonymisationType, D.RevisionLevel, D.DataWithholdingReason, D.IgnoreButKeepForReference, D.LogUpdatedBy, D.LogUpdatedWhen,  'D'
FROM DELETED D
GO
ALTER TABLE [dbo].[Agent] ENABLE TRIGGER [trgDelAgent]
GO
/****** Object:  Trigger [dbo].[trgInsAgent]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--#####################################################################################################################
--######   trgInsAgent  ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgInsAgent] ON [dbo].[Agent] 
FOR INSERT AS

/*  MW 04.05.2015  */ 

BEGIN

/* setting the AgentName */ 
Update Agent
set Agent.AgentName = 
case when i.InheritedNamePrefix is null or i.InheritedNamePrefix = '' then '' else i.InheritedNamePrefix + ' ' end +
case when i.Inheritedname is null then '' else i.Inheritedname end + 
case when i.GivenName is null or i.GivenName = '' then '' else 
	case when i.Inheritedname is null or i.Inheritedname = '' then '' else ', ' end + i.GivenName end +
case when i.GivenNamePostfix is null or i.GivenNamePostfix = '' then '' else ' ' + i.GivenNamePostfix end +
case when i.InheritedNamePostfix is null or i.InheritedNamePostfix = '' then '' else ', ' + i.InheritedNamePostfix end +
case when i.AgentTitle is null then '' else ', ' + i.AgentTitle end
FROM Agent, inserted i 
where 1 = 1 
AND Agent.AgentID = i.AgentID
END
GO
ALTER TABLE [dbo].[Agent] ENABLE TRIGGER [trgInsAgent]
GO
/****** Object:  Trigger [dbo].[trgUpdAgent]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdAgent] ON [dbo].[Agent] 
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
ALTER TABLE [dbo].[Agent] ENABLE TRIGGER [trgUpdAgent]
GO
/****** Object:  Trigger [dbo].[trgDelAgentContactInformation]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Trigger [dbo].[trgDelAgentContactInformation]    Script Date: 16.06.2016 16:35:38 ******/
CREATE TRIGGER [dbo].[trgDelAgentContactInformation] ON [dbo].[AgentContactInformation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.1 */ 
/*  Date: 20.09.2006  */ 


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
INSERT INTO AgentContactInformation_Log (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.DisplayOrder, deleted.AddressType, deleted.Country, deleted.City, deleted.PostalCode, deleted.Streetaddress, deleted.Address, deleted.Telephone, deleted.CellularPhone, deleted.Telefax, deleted.Email, deleted.URI, deleted.Notes, deleted.ValidFrom, deleted.ValidUntil, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO AgentContactInformation_Log (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.DisplayOrder, deleted.AddressType, deleted.Country, deleted.City, deleted.PostalCode, deleted.Streetaddress, deleted.Address, deleted.Telephone, deleted.CellularPhone, deleted.Telefax, deleted.Email, deleted.URI, deleted.Notes, deleted.ValidFrom, deleted.ValidUntil, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'D' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end
GO
ALTER TABLE [dbo].[AgentContactInformation] ENABLE TRIGGER [trgDelAgentContactInformation]
GO
/****** Object:  Trigger [dbo].[trgInsAgentContactInformation]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgInsAgentContactInformation] ON [dbo].[AgentContactInformation] 
FOR INSERT AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.1 */ 
/*  Date: 20.09.2006  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from inserted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM inserted)
   EXECUTE procSetVersionAgent @ID
END
GO
ALTER TABLE [dbo].[AgentContactInformation] ENABLE TRIGGER [trgInsAgentContactInformation]
GO
/****** Object:  Trigger [dbo].[trgUpdAgentContactInformation]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Trigger [dbo].[trgUpdAgentContactInformation]    Script Date: 16.06.2016 16:35:57 ******/
CREATE TRIGGER [dbo].[trgUpdAgentContactInformation] ON [dbo].[AgentContactInformation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.1 */ 
/*  Date: 20.09.2006  */ 

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
INSERT INTO AgentContactInformation_Log (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.DisplayOrder, deleted.AddressType, deleted.Country, deleted.City, deleted.PostalCode, deleted.Streetaddress, deleted.Address, deleted.Telephone, deleted.CellularPhone, deleted.Telefax, deleted.Email, deleted.URI, deleted.Notes, deleted.ValidFrom, deleted.ValidUntil, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO AgentContactInformation_Log (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.DisplayOrder, deleted.AddressType, deleted.Country, deleted.City, deleted.PostalCode, deleted.Streetaddress, deleted.Address, deleted.Telephone, deleted.CellularPhone, deleted.Telefax, deleted.Email, deleted.URI, deleted.Notes, deleted.ValidFrom, deleted.ValidUntil, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'U' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end

/* updating the logging columns */
Update AgentContactInformation
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM AgentContactInformation, deleted 
where 1 = 1 
AND AgentContactInformation.AgentID = deleted.AgentID
AND AgentContactInformation.DisplayOrder = deleted.DisplayOrder
GO
ALTER TABLE [dbo].[AgentContactInformation] ENABLE TRIGGER [trgUpdAgentContactInformation]
GO
/****** Object:  Trigger [dbo].[trgDelAgentExternalDatabase]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelAgentExternalDatabase] ON [dbo].[AgentExternalDatabase] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 13.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO AgentExternalDatabase_Log (ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, ExternalDatabaseURI, ExternalDatabaseInstitution, InternalNotes, ExternalAttribute_AgentID, PreferredSequence, Disabled, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogState) 
SELECT D.ExternalDatabaseID, D.ExternalDatabaseName, D.ExternalDatabaseVersion, D.Rights, D.ExternalDatabaseAuthors, D.ExternalDatabaseURI, D.ExternalDatabaseInstitution, D.InternalNotes, D.ExternalAttribute_AgentID, D.PreferredSequence, D.Disabled, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) ,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
GO
ALTER TABLE [dbo].[AgentExternalDatabase] ENABLE TRIGGER [trgDelAgentExternalDatabase]
GO
/****** Object:  Trigger [dbo].[trgUpdAgentExternalDatabase]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdAgentExternalDatabase] ON [dbo].[AgentExternalDatabase] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 13.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO AgentExternalDatabase_Log (ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, ExternalDatabaseURI, ExternalDatabaseInstitution, InternalNotes, ExternalAttribute_AgentID, PreferredSequence, Disabled, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogState) 
SELECT D.ExternalDatabaseID, D.ExternalDatabaseName, D.ExternalDatabaseVersion, D.Rights, D.ExternalDatabaseAuthors, D.ExternalDatabaseURI, D.ExternalDatabaseInstitution, D.InternalNotes, D.ExternalAttribute_AgentID, D.PreferredSequence, D.Disabled, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) ,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM AgentExternalDatabase T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.ExternalDatabaseID = D.ExternalDatabaseID
GO
ALTER TABLE [dbo].[AgentExternalDatabase] ENABLE TRIGGER [trgUpdAgentExternalDatabase]
GO
/****** Object:  Trigger [dbo].[trgDelAgentExternalID]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelAgentExternalID] ON [dbo].[AgentExternalID] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 13.02.2020  */ 


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
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) ,  @Version,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) , V.Version, 'D' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) , -1, 'D' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end
GO
ALTER TABLE [dbo].[AgentExternalID] ENABLE TRIGGER [trgDelAgentExternalID]
GO
/****** Object:  Trigger [dbo].[trgUpdAgentExternalID]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdAgentExternalID] ON [dbo].[AgentExternalID] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 13.02.2020  */ 

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
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) ,  @Version,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) , V.Version, 'U' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentExternalID_Log (AgentID, ExternalDatabaseID, ExternalAgentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.ExternalDatabaseID, D.ExternalAgentURI, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, cast(U.ID as varchar) , -1, 'U' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM AgentExternalID T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.AgentID = D.AgentID
AND T.ExternalDatabaseID = D.ExternalDatabaseID
GO
ALTER TABLE [dbo].[AgentExternalID] ENABLE TRIGGER [trgUpdAgentExternalID]
GO
/****** Object:  Trigger [dbo].[trgDelAgentHierarchyOther]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   trgDelAgentHierarchyOther  #################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelAgentHierarchyOther] ON [dbo].[AgentHierarchyOther] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 31.01.2020  */ 


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
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  @Version,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , V.Version, 'D' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , -1, 'D' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end
GO
ALTER TABLE [dbo].[AgentHierarchyOther] ENABLE TRIGGER [trgDelAgentHierarchyOther]
GO
/****** Object:  Trigger [dbo].[trgUpdAgentHierarchyOther]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   trgUpdAgentHierarchyOther  #################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdAgentHierarchyOther] ON [dbo].[AgentHierarchyOther] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 31.01.2020  */ 

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
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  @Version,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , V.Version, 'U' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentHierarchyOther_Log (AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.HierarchyID, D.AgentParentID, D.ValidFrom, D.ValidUntil, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , -1, 'U' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM AgentHierarchyOther T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.AgentID = D.AgentID
AND T.HierarchyID = D.HierarchyID
GO
ALTER TABLE [dbo].[AgentHierarchyOther] ENABLE TRIGGER [trgUpdAgentHierarchyOther]
GO
/****** Object:  Trigger [dbo].[trgDelAgentIdentifier]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   trgDelAgentIdentifier  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelAgentIdentifier] ON [dbo].[AgentIdentifier] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 10.02.2020  */ 


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
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  @Version,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , V.Version, 'D' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , -1, 'D' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end
GO
ALTER TABLE [dbo].[AgentIdentifier] ENABLE TRIGGER [trgDelAgentIdentifier]
GO
/****** Object:  Trigger [dbo].[trgUpdAgentIdentifier]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   trgUpdAgentIdentifier  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdAgentIdentifier] ON [dbo].[AgentIdentifier] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.3 */ 
/*  Date: 10.02.2020  */ 

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
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  @Version,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , V.Version, 'U' 
FROM DELETED D, UserProxy U, Agent V
WHERE U.LoginName = suser_sname() AND D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentIdentifier_Log (AgentID, Identifier, IdentifierURI, Type, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.Identifier, D.IdentifierURI, D.Type, D.Notes, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) , -1, 'U' 
FROM DELETED D, UserProxy U WHERE U.LoginName = suser_sname() 
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM AgentIdentifier T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.AgentID = D.AgentID
AND T.Identifier = D.Identifier
GO
ALTER TABLE [dbo].[AgentIdentifier] ENABLE TRIGGER [trgUpdAgentIdentifier]
GO
/****** Object:  Trigger [dbo].[trgDelAgentImage]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   trgDelAgentImage - add DataWithholdingReason  ##############################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelAgentImage] ON [dbo].[AgentImage] 
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
ALTER TABLE [dbo].[AgentImage] ENABLE TRIGGER [trgDelAgentImage]
GO
/****** Object:  Trigger [dbo].[trgInsAgentImage]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgInsAgentImage] ON [dbo].[AgentImage] 
FOR INSERT AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.5 */ 
/*  Date: 18.08.2010  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from inserted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM inserted)
   EXECUTE procSetVersionAgent @ID
END 
GO
ALTER TABLE [dbo].[AgentImage] ENABLE TRIGGER [trgInsAgentImage]
GO
/****** Object:  Trigger [dbo].[trgUpdAgentImage]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   trgUpdAgentImage - add DataWithholdingReason  ##############################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdAgentImage] ON [dbo].[AgentImage] 
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
ALTER TABLE [dbo].[AgentImage] ENABLE TRIGGER [trgUpdAgentImage]
GO
/****** Object:  Trigger [dbo].[trgDelAgentKeyword]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelAgentKeyword] ON [dbo].[AgentKeyword] 
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
INSERT INTO AgentKeyword_Log (AgentID, Keyword, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.Keyword, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO AgentKeyword_Log (AgentID, Keyword, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.Keyword, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'D' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end
GO
ALTER TABLE [dbo].[AgentKeyword] ENABLE TRIGGER [trgDelAgentKeyword]
GO
/****** Object:  Trigger [dbo].[trgInsAgentKeyword]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgInsAgentKeyword] ON [dbo].[AgentKeyword] 
FOR INSERT AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.5 */ 
/*  Date: 18.08.2010  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from inserted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM inserted)
   EXECUTE procSetVersionAgent @ID
END 
GO
ALTER TABLE [dbo].[AgentKeyword] ENABLE TRIGGER [trgInsAgentKeyword]
GO
/****** Object:  Trigger [dbo].[trgDelAgentProject]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelAgentProject] ON [dbo].[AgentProject] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.5 */ 
/*  Date: 18.08.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO AgentProject_Log (AgentID, ProjectID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.AgentID, deleted.ProjectID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[AgentProject] ENABLE TRIGGER [trgDelAgentProject]
GO
/****** Object:  Trigger [dbo].[trgDelAgentReference]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelAgentReference] ON [dbo].[AgentReference] 
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
INSERT INTO AgentReference_Log (AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, ContainsImage, ContainsReferencelist, Notes, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ContainsImage, deleted.ContainsReferencelist, deleted.Notes, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO AgentReference_Log (AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, ContainsImage, ContainsReferencelist, Notes, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ContainsImage, deleted.ContainsReferencelist, deleted.Notes, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'D' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end
GO
ALTER TABLE [dbo].[AgentReference] ENABLE TRIGGER [trgDelAgentReference]
GO
/****** Object:  Trigger [dbo].[trgInsAgentReference]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgInsAgentReference] ON [dbo].[AgentReference] 
FOR INSERT AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.5 */ 
/*  Date: 18.08.2010  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from inserted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM inserted)
   EXECUTE procSetVersionAgent @ID
END 
GO
ALTER TABLE [dbo].[AgentReference] ENABLE TRIGGER [trgInsAgentReference]
GO
/****** Object:  Trigger [dbo].[trgUpdAgentReference]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdAgentReference] ON [dbo].[AgentReference] 
FOR UPDATE AS

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
INSERT INTO AgentReference_Log (AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, ContainsImage, ContainsReferencelist, Notes, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ContainsImage, deleted.ContainsReferencelist, deleted.Notes, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO AgentReference_Log (AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, ContainsImage, ContainsReferencelist, Notes, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ContainsImage, deleted.ContainsReferencelist, deleted.Notes, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'U' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end

/* updating the logging columns */
Update AgentReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM AgentReference, deleted 
where 1 = 1 
AND AgentReference.AgentID = deleted.AgentID
AND AgentReference.ReferenceTitle = deleted.ReferenceTitle
GO
ALTER TABLE [dbo].[AgentReference] ENABLE TRIGGER [trgUpdAgentReference]
GO
/****** Object:  Trigger [dbo].[trgDelAgentRelation]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelAgentRelation] ON [dbo].[AgentRelation] 
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
INSERT INTO AgentRelation_Log (AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.RelatedAgentID, deleted.RelationType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO AgentRelation_Log (AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.RelatedAgentID, deleted.RelationType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'D' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end
GO
ALTER TABLE [dbo].[AgentRelation] ENABLE TRIGGER [trgDelAgentRelation]
GO
/****** Object:  Trigger [dbo].[trgInsAgentRelation]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgInsAgentRelation] ON [dbo].[AgentRelation] 
FOR INSERT AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.5 */ 
/*  Date: 18.08.2010  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from inserted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM inserted)
   EXECUTE procSetVersionAgent @ID
END 
GO
ALTER TABLE [dbo].[AgentRelation] ENABLE TRIGGER [trgInsAgentRelation]
GO
/****** Object:  Trigger [dbo].[trgUpdAgentRelation]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdAgentRelation] ON [dbo].[AgentRelation] 
FOR UPDATE AS

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
INSERT INTO AgentRelation_Log (AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.RelatedAgentID, deleted.RelationType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO AgentRelation_Log (AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.RelatedAgentID, deleted.RelationType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'U' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end

/* updating the logging columns */
Update AgentRelation
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM AgentRelation, deleted 
where 1 = 1 
AND AgentRelation.AgentID = deleted.AgentID
AND AgentRelation.RelatedAgentID = deleted.RelatedAgentID
GO
ALTER TABLE [dbo].[AgentRelation] ENABLE TRIGGER [trgUpdAgentRelation]
GO
/****** Object:  Trigger [dbo].[trgUpdUserProxy]    Script Date: 07.04.2021 12:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   trgUpdUserProxy - remove PrivacyConsentDate if no consent is given   #######################################
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
  else
  begin
    UPDATE U SET PrivacyConsentDate = NULL
    FROM UserProxy U, deleted D 
    WHERE U.ID = D.ID 
  end
end 
GO
ALTER TABLE [dbo].[UserProxy] ENABLE TRIGGER [trgUpdUserProxy]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Creates a copy of a given agent and returns the AgentID of the new agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertAgentCopy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the created agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertAgentCopy', @level2type=N'PARAMETER',@level2name=N'@AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the agent that should be copied' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertAgentCopy', @level2type=N'PARAMETER',@level2name=N'@CopyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A temporary display text for the created agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertAgentCopy', @level2type=N'PARAMETER',@level2name=N'@AgentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Setting the version of a dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procSetVersionAgent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procSetVersionAgent', @level2type=N'PARAMETER',@level2name=N'@ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The address of the agent as collected from the hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the agent for which the hierarchy should be retrieved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentChildNodes', @level2type=N'PARAMETER',@level2name=N'@ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns the list of agents within the hierarchy underneath the agent with the given ID (=AgentID)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentChildNodes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides the first contact information that is not withheld and valid including contact information of parent agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentContactInformation_Public'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns the list of agents within the hierarchy starting at the topmost agent related to the agent with the given AgentID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentHierarchy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns the list of all superior agents within the hierarchy and other hierarchies related to the agent with the given AgentID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentHierarchyAllSuperior'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the display as documented in the table dbo.AgentNameDisplayType_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentNames', @level2type=N'PARAMETER',@level2name=N'@DisplayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the agents with their names according to the selected display type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentNames'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns the list of superior agents as retrieved from the hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentSuperiorList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns the top AgentID within the synyonym hierarchy for a given AgentID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentSynonymTopID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns all agents within the synonymy of the given agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentSynonymy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the agent for which the depending synonymys should be retrieved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentSynonymyChildNodes', @level2type=N'PARAMETER',@level2name=N'@ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The depending synonymys of a given agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentSynonymyChildNodes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns the top ID within the hierarchy for a given AgentID from the table Agent.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentTopID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides the basic address for accessing the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'BaseURL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default display type as set for the default project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'DefaultAgentNameDisplayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns the default ProjectID for a user as set in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'DefaultProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the DiversityWorkbench module' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'DiversityWorkbenchModule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'Version'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Version of the client software compatible with the version of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'VersionClient'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Role for the adminstration of the database' , @level0type=N'USER',@level0name=N'DiversityWorkbenchAdministrator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Role with write access for the database' , @level0type=N'USER',@level0name=N'DiversityWorkbenchEditor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Role with read only access to the database' , @level0type=N'USER',@level0name=N'DiversityWorkbenchUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddressType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddressType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddressType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddressType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddressType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddressType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddressType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lookup table for the types of the addresses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddressType_Enum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent  (= Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the superior agent if agents are organized within a hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'AgentParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The whole name of the agent as shown e.g. for selection in an user interface. For persons this entry will be generated as follows: LastName, FirstNames, AgentTitle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'AgentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The version of a agent record (revision number, internally filled by system)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'Version'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the agent, e.g. Dr., Prof.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'AgentTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The first names of the agent (if a person) or the name of e.g. an institution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'GivenName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Variable part of name, correctly placed at end of given names' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'GivenNamePostfix'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Variable part of name, correctly placed at the beginning of the inherited names' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'InheritedNamePrefix'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last names of the agent (if a person)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'InheritedName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Additions after inherited name, like generation (Jr., III.) or names of religious orders' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'InheritedNamePostfix'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Abbreviation of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'Abbreviation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the agent, e.g. person, company' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'AgentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of an agent esp. a person within an organization. e.g. "Database Administrator" or "Curator"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'AgentRole'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The gender resp. sex of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'AgentGender'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A description of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name as originally written in e.g. chinese or cyrillic letters' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'OriginalSpelling'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the begin of the exsistence of the agent, e.g. the birthday of a person or the founding of an institution, calculated from ValidFromDay, - Month and -Year' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidFromDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The day of the begin of the exsistence of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidFromDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The month of the begin of the exsistence of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidFromMonth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of the begin of the exsistence of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidFromYear'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the end of the exsistence of the agent, e.g. death of a person or closing of an institute, calculated from ValidUntilDay, - Month and -Year' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidUntilDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The day of the end of the exsistence of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidUntilDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The month of the end of the exsistence of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidUntilMonth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of the end of the exsistence of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidUntilYear'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Verbal or additional  date information, e.g.  ''end of summer 1985'', ''first quarter''. The time of the valid date  if necessary.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'ValidDateSupplement'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The place (e.g. a city) where a person was born or an institution was founded' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'PlaceOfBirth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The place (e.g.a city) where the person died' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'PlaceOfDeath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the agent which was selected as a replacement for the current agent, e.g. if to equal datasets were imported from different sources' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'SynonymToAgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the synonymisation like "replaced with", "synonym to"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'SynonymisationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The level of the revision of the agent, e.g. ''unchecked'', ''final revision''' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'RevisionLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If true, the record is ignored for all purposes of evaluation (because contradicted). It is kept only to maintain the cited reference. If no reference is given, it may be deleted instead.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'IgnoreButKeepForReference'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The main table with the data of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Variable part of name, correctly placed at end of given names' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent_log', @level2type=N'COLUMN',@level2name=N'GivenNamePostfix'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Variable part of name, correctly placed at the beginning of the inherited names' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent_log', @level2type=N'COLUMN',@level2name=N'InheritedNamePrefix'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Additions after inherited name, like generation (Jr., III.) or names of religious orders' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent_log', @level2type=N'COLUMN',@level2name=N'InheritedNamePostfix'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name as originally written in e.g. chinese or cyrillic letters' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent_log', @level2type=N'COLUMN',@level2name=N'OriginalSpelling'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The place (e.g. a city) where a person was born or an institution was founded' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent_log', @level2type=N'COLUMN',@level2name=N'PlaceOfBirth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The place (e.g.a city) where the person died' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent_log', @level2type=N'COLUMN',@level2name=N'PlaceOfDeath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent_log', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of Agent (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Display order of records in user interface. DisplayOrder 1 corresponds to the preferred address  (= part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the adress, e.g. private' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'AddressType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Country of the address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'Country'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'City of the address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'City'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ZIP or postcode of the address (usually output before or after the city)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'PostalCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Usually street name and number, but may also contain post office box' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'Streetaddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Free text postal address of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phone number, including area code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'Telephone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The number of a mobile telephone device of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'CellularPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fax number, including area code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'Telefax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'E-mail address of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI pointing to a homepage containing further information' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'URI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about this address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date when this address became valid as date according to ISO 8601' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'ValidFrom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the expiration of the validity of this address as date according to ISO 8601' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'ValidUntil'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The contact information resp. addresses of the agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An ID to identify an external data collection of plant names (primary key, the ID has no meaning outside of the DiversityWorkbench system)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'ExternalDatabaseID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the data collection that has been integrated or can be linked to for further analysis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'ExternalDatabaseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The version of this data collection (either official version number, or dates when the collection was integrated)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'ExternalDatabaseVersion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A description of copyright agreements or permission to use data from the external database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'Rights'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The persons or institutions responsible for the external database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'ExternalDatabaseAuthors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to the database provider resp. the external database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'ExternalDatabaseURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The institution responsible for the external database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'ExternalDatabaseInstitution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Additional notes concerning this data collection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The table and field name in the external data collection to which PlantNameAnalysis.ExternalNameID refers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'ExternalAttribute_AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'For selection in e.g. picklists: of several equal names only the name from the source with the lowest preferred sequence will be provided.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'PreferredSequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If this source should be disabled for selection of names e.g. in picklists' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'Disabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The external databases from which data in the database may have been imported from' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalDatabase'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent (foreign key + part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalID', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of an external taxonomic data collection as defined in AgentExternalDatabase (foreign key + part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalID', @level2type=N'COLUMN',@level2name=N'ExternalDatabaseID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI (e.g. LSID) of the external agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalID', @level2type=N'COLUMN',@level2name=N'ExternalAgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalID', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalID', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalID', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalID', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID''s of data that were imported from foreign souces' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentExternalID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentGender_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentGender_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentGender_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentGender_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentGender_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentGender_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentGender_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of Agent (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier of the hierarchy, part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'HierarchyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the superior agent if agents are organized within a hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'AgentParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The start time of the hierarchy, e.g. when a person was employed in an institution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'ValidFrom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The end of a hierarchy, e.g. when an employment ended' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'ValidUntil'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the other hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Other hierarchies of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentHierarchyOther'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of Agent (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier for the agent, part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'Identifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of Identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'IdentifierURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the identifier as defined in table AgentIdentifierType_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier for the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further information about this entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentIdentifierType_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent  (foreign key + part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The complete URI address of the image. This is only a cached value if ResourceID is available referring to the module DiversityResources' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'URI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The sequence of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'Sequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The images of the agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further information about this entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent  (foreign key + part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentKeyword', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The keyword' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentKeyword', @level2type=N'COLUMN',@level2name=N'Keyword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentKeyword', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentKeyword', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentKeyword', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentKeyword', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The keywords for the agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentKeyword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentNameDisplayType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentNameDisplayType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentNameDisplayType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentNameDisplayType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentNameDisplayType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentNameDisplayType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentNameDisplayType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent  (= Foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentProject', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentProject', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentProject', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentProject', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentProject', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentProject', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The projects of the agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentProject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of Agent (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the publication where information about the agent was published. Note this is only a cached value where ReferenceURI is present' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of reference where information about the agent, e.g. referring to the module DiversityReferences' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details within the reference, e.g. pages' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the reference contains an image of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'ContainsImage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the reference contains a publication list of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'ContainsReferencelist'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'References containing informations about the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentReference'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of Agent (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The AgentID of the related agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation', @level2type=N'COLUMN',@level2name=N'RelatedAgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the relation between the agents, e.g. "parent of", "child of", "married to"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation', @level2type=N'COLUMN',@level2name=N'RelationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the relation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relations of the agent to other agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentRelationType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSynonymisationType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSynonymisationType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSynonymisationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSynonymisationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSynonymisationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSynonymisationType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSynonymisationType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentTitle_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lookup table for the titles of the agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentTitle_Enum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lookup table for the types of the agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentType_Enum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the resultset and the purpose of the query' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationSearchSelectionStrings', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the project, Primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the project as stored in the module DiversityProjects' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'Project'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The display type of the name of the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'AgentNameDisplayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If an archive e.g. by a task schedule should be created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'CreateArchive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the last archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ArchiveProtocol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The projects - refers to database DiversityProjects' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The login name of the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the default project of the user as stored in table ProjectProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The projects to which users have access to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lookup table for the revision level' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RevisionLevel_Enum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The login name of the user, Primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A combined name of the user, created on the base of an entry in the module DiversityUsers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'CombinedNameCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to UserInfo.UserID in database DiversityUsers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'UserURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the default project of the user as stored in table ProjectProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Queries created by the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Queries'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A link to a DiversityAgents module where further informations about the user is available.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user consents the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time and date when the user consented or refused the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsentDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The user logins - refers to database DiversityUsers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent including indentation for synonyms' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Agent_Core', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Agents that are available for a user and are not ignored' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Agent_Core'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IDs of the agent available for a user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AgentID_UserAvailable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Projects available for a user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ProjectList'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the superior agent within the hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublicContactInformation', @level2type=N'COLUMN',@level2name=N'ParentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Accumulated public address of an agent within a hierarchy of up to 4 levels' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublicContactInformation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Information about the user as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'UserInfo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the superior agent within the hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewAgentAddress', @level2type=N'COLUMN',@level2name=N'ParentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Accumulated address of an agent within a hierarchy of up to 4 levels' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewAgentAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The names of the agents as retrieved for the default display type with function AgentNames' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewAgentNames'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Basic address for accessing the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewBaseURL', @level2type=N'COLUMN',@level2name=N'BaseURL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides the basic address for accessing the database as defined in function BaseURL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewBaseURL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default display type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDefaultAgentNameDisplayType', @level2type=N'COLUMN',@level2name=N'DefaultAgentNameDisplayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default display type as set for the default project as defined in function DefaultAgentNameDisplayType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDefaultAgentNameDisplayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the DiversityWorkbench module' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDiversityWorkbenchModule', @level2type=N'COLUMN',@level2name=N'DiversityWorkbenchModule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides the name of the DiversityWorkbench module as defined in function DiversityWorkbenchModule' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDiversityWorkbenchModule'
GO


declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.20'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  Grants for AgentImageType_Enum  #############################################################################
--#####################################################################################################################

GRANT SELECT ON AgentImageType_Enum TO [DiversityWorkbenchUser];
GRANT INSERT ON AgentImageType_Enum TO [DiversityWorkbenchAdministrator];
GRANT UPDATE ON AgentImageType_Enum TO [DiversityWorkbenchAdministrator];
GRANT DELETE ON AgentImageType_Enum TO [DiversityWorkbenchAdministrator];
GO


--#####################################################################################################################
--######   AgentAddress    ############################################################################################
--#####################################################################################################################

-- =============================================
-- Author:		Markus Weiss
-- Create date: 08.08.2022
-- Description:	returns a table containing the address values including values of superior entries for missing values
-- Test: 
-- SELECT * FROM dbo.AgentAddress(37687)
-- SELECT * FROM dbo.AgentAddress(37677)
-- SELECT * FROM dbo.AgentAddress(37686)
-- =============================================

ALTER FUNCTION [dbo].[AgentAddress] 
(
@AgentID int
)
/*
Testing:
SELECT * FROM dbo.AgentAddress(18685);
SELECT * FROM dbo.AgentAddress(37687)
SELECT * FROM dbo.AgentAddress(37677)
SELECT * FROM dbo.AgentAddress(37686)
SELECT * FROM dbo.AgentAddress(41781)
SELECT * FROM dbo.AgentAddress(2821)
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

DECLARE @AddressLoop TABLE ([AgentID] [int] NOT NULL Primary key)

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

	if (not @ParentID is null)
	begin
	if (select count(*) from @AddressLoop l where l.AgentID = @ParentID) > 0 
		set @ParentID = null
	else
		INSERT INTO @AddressLoop SELECT @ParentID
	end

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

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the Agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentAddress',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the Agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentAddress',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END CATCH;
GO


--#####################################################################################################################
--######  AgentHierarchyAllSuperior: ensure result despite of loops  ##################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentHierarchyAllSuperior] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] NOT NULL,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL)

/*
Returns a table that lists all the agents superior to the given agent, including all hierarchy data.
MW 08.08.2022
SELECT * FROM dbo.AgentHierarchyAllSuperior(37687)
SELECT AgentID, AgentParentID, AgentName FROM dbo.AgentHierarchyAllSuperior(41782)
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
	if (select count(*) from @TOP a where a.AgentParentID = @AgentID) > 0 set @i = 0
end


   -- copy the child nodes into the result list
   INSERT @AgentList
   SELECT DISTINCT AA.AgentID, AA.AgentParentID, A.AgentName
   FROM Agent A, @All AA
   WHERE A.AgentID = AA.AgentID

   RETURN
END

GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the Agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentHierarchyAllSuperior',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the Agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentHierarchyAllSuperior',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END CATCH;
GO


--#####################################################################################################################
--######  AgentChildNodes: ensure result despite of loops  ############################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentChildNodes] (@ID int)  
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
MW 06.08.2022
SELECT * FROM [dbo].[AgentChildNodes] (41781)
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL	)

	INSERT @TempItem (AgentID, AgentParentID) 
	SELECT AgentID, AgentParentID
	FROM Agent WHERE AgentParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, Agent C where C.AgentParentID = T.AgentID and C.AgentID not in (select AgentID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (AgentID, AgentParentID)
		select C.AgentID, C.AgentParentID
		from @TempItem T, Agent C where C.AgentParentID = T.AgentID and C.AgentID not in (select AgentID from @TempItem)
		set @i = (select count(*) from @TempItem T, Agent C where C.AgentParentID = T.AgentID and C.AgentID not in (select AgentID from @TempItem))
	end

 INSERT @ItemList (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason) 
   SELECT distinct T.AgentID, T.AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
   FROM @TempItem T INNER JOIN Agent A ON T.AgentID = A.AgentID ORDER BY AgentName
   RETURN
END

GO

--#####################################################################################################################
--######  AgentTopID: ensure result despite of loops  #################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentTopID] (@AgentID int)
RETURNS int
/*
Returns the top ID within the hierarchy for a given ID from the table Agent.
MW 08.08.2022
SELECT [dbo].[AgentTopID] (41781)
*/
AS
BEGIN
declare @TopID int
declare @i int
declare @StartID int
set @StartID = @AgentID

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
		if @StartID = @AgentID set @i = 0
		end
	set @TopID = @AgentID
	end
return @TopID
END

GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent for which the top ID should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentTopID',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent for which the top ID should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentTopID',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END CATCH;
GO


--#####################################################################################################################
--######   AgentHierarchy: ensure result despite of loops   ###########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentHierarchy] (@AgentID int)  
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
MW 08.08.2022
SELECT * FROM [dbo].[AgentHierarchy] (41781)
SELECT * FROM [dbo].[AgentHierarchy] (2821)
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
	[AgentParentID] [int] NULL)

	INSERT @TempItem (AgentID, AgentParentID) 
	SELECT AgentID, AgentParentID FROM dbo.AgentChildNodes (@TopID)

-- copy the root node in the result list
INSERT @TempItem
   SELECT DISTINCT AgentID, AgentParentID
   FROM Agent A
   WHERE A.AgentID = @TopID AND A.AgentID NOT IN (SELECT AgentID FROM @TempItem)

-- copy the content into the result list
INSERT @AgentList
   SELECT DISTINCT A.AgentID, A.AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
   FROM Agent A INNER JOIN @TempItem T ON A.AgentID = T.AgentID
   ORDER BY AgentName

   RETURN
END

GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent for which the hierarchy should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentHierarchy',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The AgentID of the agent for which the hierarchy should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AgentHierarchy',
@level2type=N'PARAMETER', @level2name=N'@AgentID'
END CATCH;
GO


--#####################################################################################################################
--######  ProjectProxy: new columns [RowGUID], [LastChanges], [IsLocked] ###############################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectProxy] ADD [RowGUID] [uniqueidentifier] ROWGUIDCOL NOT NULL DEFAULT (newsequentialid());
ALTER TABLE [dbo].[ProjectProxy] ADD [LastChanges] [datetime] NULL;
ALTER TABLE [dbo].[ProjectProxy] ADD [IsLocked] [bit] NULL;

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The recent date when data within the project had been changed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'LastChanges'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the data within the project should not be changeed and the access for all users is restricted to read only' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'IsLocked'
GO


--#####################################################################################################################
--######   AgentID_FullAccess  ########################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[AgentID_FullAccess]
AS
SELECT        A.AgentID
FROM            dbo.AgentProject AS A 
INNER JOIN dbo.ProjectUser AS U ON A.ProjectID = U.ProjectID AND (U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) AND (U.ReadOnly IS NULL OR U.ReadOnly = 0)
INNER JOIN dbo.ProjectProxy P ON U.ProjectID = P.ProjectID AND (P.IsLocked = 0 OR P.IsLocked IS NULL)

GO

GRANT SELECT ON [AgentID_FullAccess] TO [DiversityWorkbenchUser];
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of agents with full access (neither read only or locked)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_FullAccess'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of agents with full access (neither read only or locked)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_FullAccess'
END CATCH;
GO


--#####################################################################################################################
--######   AgentID_UserAvailable  ######################################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[AgentID_UserAvailable]
AS
SELECT p.AgentID
FROM         dbo.AgentProject P INNER JOIN
                      dbo.ProjectUser U ON p.ProjectID = u.ProjectID
WHERE     ((U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()))
UNION
SELECT AgentID FROM Agent 
WHERE AgentID NOT IN (SELECT AgentID FROM AgentProject)

GO



--#####################################################################################################################
--######   AgentID_Locked #############################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[AgentID_Locked]
AS
SELECT        C.AgentID
FROM            dbo.AgentProject AS C INNER JOIN
dbo.ProjectProxy AS P ON P.ProjectID = C.ProjectID AND (P.IsLocked = 1) 
GROUP BY C.AgentID
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'AgentIDs that are locked due to locking of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_Locked'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'AgentIDs that are locked due to locking of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_Locked'
END CATCH;
GO

GRANT SELECT ON [AgentID_Locked] TO [DiversityWorkbenchUser];
GO

--#####################################################################################################################
--######   AgentID_ReadOnly  ##########################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[AgentID_ReadOnly]
AS
SELECT        P.AgentID
FROM            dbo.AgentProject AS P INNER JOIN
dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
WHERE        ((U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) AND (U.ReadOnly = 1) 
	OR 
	P.AgentID IN (SELECT L.AgentID FROM [AgentID_Locked] L)
	)
GROUP BY P.AgentID
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'AgentIDs that are ReadOnly for a User or are locked due to locking of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_ReadOnly'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'AgentIDs that are ReadOnly for a User or are locked due to locking of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_ReadOnly'
END CATCH;
GO

GRANT SELECT ON [AgentID_ReadOnly] TO [DiversityWorkbenchUser];
GO


--#####################################################################################################################
--######   AgentID_AvailableReadOnly  #################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[AgentID_AvailableReadOnly]
AS
SELECT        P.AgentID
FROM            dbo.AgentProject AS P INNER JOIN
                         dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
WHERE        (U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) 
AND 
(
	(U.ReadOnly = 1
		AND (P.AgentID NOT IN
			(SELECT P.AgentID
			FROM dbo.AgentProject AS P INNER JOIN
			dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
			WHERE (U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) 
			AND (U.ReadOnly = 0))
		)
	)
	OR
	P.AgentID IN (SELECT L.AgentID FROM [AgentID_Locked] L)
)
GROUP BY P.AgentID

GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Available AgentIDs that are ReadOnly or Locked',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_AvailableReadOnly'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Available AgentIDs that are ReadOnly or Locked',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'AgentID_AvailableReadOnly'
END CATCH;
GO

GRANT SELECT ON [AgentID_AvailableReadOnly] TO [DiversityWorkbenchUser];
GO


--#####################################################################################################################
--######  ProjectList: Add ReadOnly  ##################################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[ProjectList]
AS
SELECT     P.ProjectID, P.Project, CASE WHEN P.IsLocked = 1 THEN 1 ELSE U.ReadOnly END AS ReadOnly, P.IsLocked
FROM         dbo.ProjectUser U INNER JOIN
                      dbo.ProjectProxy P ON U.ProjectID = P.ProjectID
WHERE     ((U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()))
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
RETURN '02.01.21'
END

GO

select [dbo].[Version] ()  


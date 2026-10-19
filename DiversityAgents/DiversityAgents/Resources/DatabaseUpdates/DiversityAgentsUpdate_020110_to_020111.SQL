
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.10'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   missing description    #####################################################################################
--#####################################################################################################################

if (SELECT count(*) 
FROM sys.extended_properties AS ep 
INNER JOIN  sys.tables AS t ON ep.major_id = t.object_id  
left outer JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id 
WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = 'Agent' AND c.name = 'AgentGender' ) = 0 
begin 
   EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The gender resp. sex of the agent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Agent',
@level2type=N'COLUMN', @level2name=N'AgentGender' 
end;
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


--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.00.16' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.11'
END

GO

select [dbo].[Version] ()  


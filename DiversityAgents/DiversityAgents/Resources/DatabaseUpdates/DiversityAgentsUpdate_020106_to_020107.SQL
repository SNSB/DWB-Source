
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.06'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   ViewBaseURL to enable access by linked server on function BaseURL()   ######################################
--#####################################################################################################################



declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewBaseURL]
	AS
	SELECT        dbo.BaseURL() AS BaseURL')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

grant select on ViewBaseURL to [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   [ViewDiversityWorkbenchModule]   ###########################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewDiversityWorkbenchModule]
AS
SELECT dbo.DiversityWorkbenchModule() AS DiversityWorkbenchModule')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

grant select on [ViewDiversityWorkbenchModule] to [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   [DefaultAgentNameDisplayType]    ###########################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[DefaultAgentNameDisplayType] ()
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


--#####################################################################################################################
--######   [ViewDefaultAgentNameDisplayType]   ########################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewDefaultAgentNameDisplayType]
AS
SELECT [dbo].[DefaultAgentNameDisplayType] () AS DefaultAgentNameDisplayType')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

grant select on ViewDefaultAgentNameDisplayType to [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   [ViewAgentNames]   #########################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentNames]
AS
SELECT * FROM [dbo].[AgentNames] (
  [dbo].[DefaultAgentNameDisplayType] ())')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentNames]
	AS SELECT * FROM [dbo].[AgentNames] ('''')')
	exec sp_executesql @SQL
	end catch
end catch

GO

grant select on ViewAgentNames to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   [ViewAgentAddress0]   ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentAddress0]
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
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

grant select on [ViewAgentAddress0] to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   [ViewAgentAddress1]   ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentAddress1]
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
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

grant select on [ViewAgentAddress1] to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   [ViewAgentAddress2]   ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentAddress2]
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
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

grant select on [ViewAgentAddress2] to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   [ViewAgentAddress3]   ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentAddress3]
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
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


grant select on [ViewAgentAddress3] to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   [ViewAgentAddress4]   ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentAddress4]
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
                               HAVING         (MIN(DisplayOrder) = C.DisplayOrder))')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


grant select on [ViewAgentAddress4] to [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   [ViewAgentAddress]   ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(MAX)
set @SQL = (select 'CREATE VIEW [dbo].[ViewAgentAddress]
AS
SELECT        
CASE WHEN A0.AgentID IS NULL THEN CASE WHEN A1.AgentID IS NULL THEN case when a2.AgentID is null then case when a3.AgentID is null then a4.AgentID else a3.AgentID end else a2.AgentID end ELSE A1.AgentID end ELSE A0.AgentID END AS AgentID, 
CASE WHEN A0.DisplayOrder IS NULL THEN CASE WHEN A1.DisplayOrder IS NULL THEN case when a2.DisplayOrder is null then case when a3.DisplayOrder is null then a4.DisplayOrder else a3.DisplayOrder end else a2.DisplayOrder end ELSE A1.DisplayOrder end ELSE A0.DisplayOrder END AS DisplayOrder, 
CASE WHEN A4.ParentName IS NULL THEN CASE WHEN A3.ParentName IS NULL THEN case when a2.ParentName is null then case when a1.ParentName is null then a0.ParentName else a1.ParentName end else a2.ParentName end ELSE A3.ParentName end ELSE A4.ParentName END AS ParentName, 
CASE WHEN A0.AddressType IS NULL THEN CASE WHEN A1.AddressType IS NULL THEN case when a2.AddressType is null then case when a3.AddressType is null then a4.AddressType else a3.AddressType end else a2.AddressType end ELSE A1.AddressType end ELSE A0.AddressType END AS AddressType, 
CASE WHEN A0.Country IS NULL THEN CASE WHEN A1.Country IS NULL THEN case when a2.Country is null then case when a3.Country is null then a4.Country else a3.Country end else a2.Country end ELSE A1.Country end ELSE A0.Country END AS Country, 
CASE WHEN A0.City IS NULL THEN CASE WHEN A1.City IS NULL THEN case when a2.City is null then case when a3.City is null then a4.City else a3.City end else a2.City end ELSE A1.City end ELSE A0.City END AS City, 
CASE WHEN A0.PostalCode IS NULL THEN CASE WHEN A1.PostalCode IS NULL THEN case when a2.PostalCode is null then case when a3.PostalCode is null then a4.PostalCode else a3.PostalCode end else a2.PostalCode end ELSE A1.PostalCode end ELSE A0.PostalCode END AS PostalCode, 
')

set @SQL = (@SQL + '
CASE WHEN A0.Streetaddress IS NULL THEN CASE WHEN A1.Streetaddress IS NULL THEN case when a2.Streetaddress is null then case when a3.Streetaddress is null then a4.Streetaddress else a3.Streetaddress end else a2.Streetaddress end ELSE A1.Streetaddress end ELSE A0.Streetaddress END AS Streetaddress, 
CASE WHEN A0.Address IS NULL THEN CASE WHEN A1.Address IS NULL THEN case when a2.Address is null then case when a3.Address is null then a4.Address else a3.Address end else a2.Address end ELSE A1.Address end ELSE A0.Address END AS Address, 
CASE WHEN A0.Telephone IS NULL THEN CASE WHEN A1.Telephone IS NULL THEN case when a2.Telephone is null then case when a3.Telephone is null then a4.Telephone else a3.Telephone end else a2.Telephone end ELSE A1.Telephone end ELSE A0.Telephone END AS Telephone, 
CASE WHEN A0.CellularPhone IS NULL THEN CASE WHEN A1.CellularPhone IS NULL THEN case when a2.CellularPhone is null then case when a3.CellularPhone is null then a4.CellularPhone else a3.CellularPhone end else a2.CellularPhone end ELSE A1.CellularPhone end ELSE A0.CellularPhone END AS CellularPhone, 
CASE WHEN A0.Telefax IS NULL THEN CASE WHEN A1.Telefax IS NULL THEN case when a2.Telefax is null then case when a3.Telefax is null then a4.Telefax else a3.Telefax end else a2.Telefax end ELSE A1.Telefax end ELSE A0.Telefax END AS Telefax, 
CASE WHEN A0.Email IS NULL THEN CASE WHEN A1.Email IS NULL THEN case when a2.Email is null then case when a3.Email is null then a4.Email else a3.Email end else a2.Email end ELSE A1.Email end ELSE A0.Email END AS Email, 
')

set @SQL = (@SQL + 'CASE WHEN A0.URI IS NULL THEN CASE WHEN A1.URI IS NULL THEN case when a2.URI is null then case when a3.URI is null then a4.URI else a3.URI end else a2.URI end ELSE A1.URI end ELSE A0.URI END AS URI, 
CASE WHEN A0.Notes IS NULL THEN CASE WHEN A1.Notes IS NULL THEN case when a2.Notes is null then case when a3.Notes is null then a4.Notes else a3.Notes end else a2.Notes end ELSE A1.Notes end ELSE A0.Notes END AS Notes, 
CASE WHEN A0.ValidFrom IS NULL THEN CASE WHEN A1.ValidFrom IS NULL THEN case when a2.ValidFrom is null then case when a3.ValidFrom is null then a4.ValidFrom else a3.ValidFrom end else a2.ValidFrom end ELSE A1.ValidFrom end ELSE A0.ValidFrom END AS ValidFrom, 
CASE WHEN A0.ValidUntil IS NULL THEN CASE WHEN A1.ValidUntil IS NULL THEN case when a2.ValidUntil is null then case when a3.ValidUntil is null then a4.ValidUntil else a3.ValidUntil end else a2.ValidUntil end ELSE A1.ValidUntil end ELSE A0.ValidUntil END AS ValidUntil
FROM dbo.ViewAgentAddress4 AS A4 FULL OUTER JOIN
dbo.ViewAgentAddress3 AS A3 ON A4.AgentID = A3.AgentID FULL OUTER JOIN
dbo.ViewAgentAddress2 AS A2 ON A3.AgentID = A2.AgentID FULL OUTER JOIN
dbo.ViewAgentAddress1 AS A1 ON A2.AgentID = A1.AgentID FULL OUTER JOIN
dbo.ViewAgentAddress0 AS A0 ON A1.AgentID = A0.AgentID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 8000)
exec sp_executesql @SQL
end catch

GO



grant select on [ViewAgentAddress] to [DiversityWorkbenchUser]
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
RETURN '02.01.07'
END

GO

select [dbo].[Version] ()  



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.03'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
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

--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
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
RETURN '02.01.04'
END

GO

select [dbo].[Version] ()  


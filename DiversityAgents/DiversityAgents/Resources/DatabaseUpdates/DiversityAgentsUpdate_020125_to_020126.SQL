
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.25'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

declare @Version int
set @Version = (select cast(SUBSTRING(@@Version, len('Microsoft SQL Server') + 2, 4) as int))
IF (@Version < 2016)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for SQL Server version 2016 or higher. Current version = ' + cast(@Version as varchar)
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######  Removing deprecated procedures ##############################################################################
--#####################################################################################################################

DROP PROCEDURE [dbo].[procInsertAgentAddressCopy_Main20]
GO

DROP PROCEDURE [dbo].[procInsertAgentAddressNew_Main20]
GO

DROP PROCEDURE [dbo].[procInsertAgentCopy_Main20]
GO

DROP PROCEDURE [dbo].[procInsertAgentNew_Main20]
GO

--#####################################################################################################################
--######  procInsertAgentCopy - fixing privilege escalation  ##########################################################
--#####################################################################################################################

DENY EXECUTE ON [dbo].[procInsertAgentCopy] TO [public]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.26'
END

GO

select [dbo].[Version] ()  


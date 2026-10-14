
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.00.02'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


GRANT INSERT ON [ProjectAgent] TO DiversityWorkbenchEditor
GO

GRANT UPDATE ON [ProjectAgent] TO DiversityWorkbenchEditor
GO

GRANT DELETE ON [ProjectAgent] TO DiversityWorkbenchEditor
GO

GRANT SELECT ON [ProjectAgent] TO DiversityWorkbenchUser
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.00.03'
END

GO


declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.73'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   GRANTS for CollRetrievalType_Enum       ####################################################################
--#####################################################################################################################

GRANT SELECT ON CollRetrievalType_Enum TO [User]
GO

GRANT INSERT ON CollRetrievalType_Enum TO [Administrator]
GO
GRANT UPDATE ON CollRetrievalType_Enum TO [Administrator]
GO
GRANT DELETE ON CollRetrievalType_Enum TO [Administrator]
GO


--#####################################################################################################################
--######   ViewBaseURL       ##########################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[ViewBaseURL]
	AS
	SELECT dbo.BaseURL() AS BaseURL
GO

GRANT SELECT ON ViewBaseURL TO [User]
GO


--#####################################################################################################################
--######   ViewDiversityWorkbenchModule     ###########################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[ViewDiversityWorkbenchModule]
AS
SELECT dbo.DiversityWorkbenchModule() AS DiversityWorkbenchModule
GO

GRANT SELECT ON [ViewDiversityWorkbenchModule] TO [User]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.74'
END

GO


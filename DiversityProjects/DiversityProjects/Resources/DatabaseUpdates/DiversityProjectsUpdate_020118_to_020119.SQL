
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.18'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   trgDelProjectDescriptor  ###################################################################################
--#####################################################################################################################

ALTER TRIGGER [trgDelProjectDescriptor] ON [dbo].[ProjectDescriptor] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 27.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptor_Log (ID, ProjectID, Language, ElementID, Content, ContentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ID, deleted.ProjectID, deleted.Language, deleted.ElementID, deleted.Content, deleted.ContentURI, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO


--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.03.00' END
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.19'
END

GO


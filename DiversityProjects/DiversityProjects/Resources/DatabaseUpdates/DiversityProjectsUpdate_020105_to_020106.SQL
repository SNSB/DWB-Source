
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.05'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Grants for ProjectIdentifier    ############################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.[ProjectIdentifier] TO [DiversityWorkbenchUser];
GO

GRANT INSERT ON dbo.[ProjectIdentifier] TO [DiversityWorkbenchEditor];
GRANT UPDATE ON dbo.[ProjectIdentifier] TO [DiversityWorkbenchEditor];
GRANT DELETE ON dbo.[ProjectIdentifier] TO [DiversityWorkbenchEditor];
GO

--#####################################################################################################################
--######   Grants for ProjectIdentifierType    ########################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.[ProjectIdentifierType] TO [DiversityWorkbenchUser];
GO

GRANT INSERT ON dbo.[ProjectIdentifierType] TO [DiversityWorkbenchAdministrator];
GRANT UPDATE ON dbo.[ProjectIdentifierType] TO [DiversityWorkbenchAdministrator];
GRANT DELETE ON dbo.[ProjectIdentifierType] TO [DiversityWorkbenchAdministrator];
GO


--#####################################################################################################################
--######   ProjectIdentifier           ################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'External identifiers for the project, e.g. a DOI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier'
GO


--#####################################################################################################################
--######   ProjectReference            ################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectReference] ALTER COLUMN [Notes] nvarchar(MAX) NULL
GO   

ALTER TABLE [dbo].[ProjectReference_log] ALTER COLUMN [Notes] nvarchar(MAX) NULL
GO  

--#####################################################################################################################
--######   ProjectReference Citation -> Type   ########################################################################
--#####################################################################################################################

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The type of the reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'Citation'
GO

EXEC sp_rename 'ProjectReference.Citation', 'ReferenceType', 'COLUMN';  
GO

EXEC sp_rename 'ProjectReference_log.Citation', 'ReferenceType', 'COLUMN';  
GO



/****** Object:  Trigger [dbo].[trgDelProjectReference]    Script Date: 10.06.2016 15:05:24 ******/
ALTER TRIGGER [dbo].[trgDelProjectReference] ON [dbo].[ProjectReference] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, ReferenceType, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ReferenceType, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO

/****** Object:  Trigger [dbo].[trgUpdProjectReference]    Script Date: 10.06.2016 15:05:54 ******/
ALTER TRIGGER [dbo].[trgUpdProjectReference] ON [dbo].[ProjectReference] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, ReferenceType, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ReferenceType, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectReference, deleted 
where 1 = 1 
AND ProjectReference.ProjectID = deleted.ProjectID
AND ProjectReference.ReferenceTitle = deleted.ReferenceTitle
GO
 

--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.04' END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.06'
END

GO


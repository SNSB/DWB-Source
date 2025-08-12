declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.52'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  ISSUE #84  ##################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  Revoke insert on [dbo].[CollectionManager] from Role CollectionManager  #####################################
--#####################################################################################################################

REVOKE INSERT ON [dbo].[CollectionManager] TO [CollectionManager] AS [dbo]
GO

CREATE TRIGGER [dbo].[trg_InsertCollectionManager]
ON [dbo].[Collection]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    -- Insert into CollectionManager for the user who inserted the row
    INSERT INTO CollectionManager (LoginName, AdministratingCollectionID)
    SELECT 
        SUSER_SNAME(), -- Gets the username of the user performing the insert
		i.CollectionID
    FROM 
        INSERTED i;
END;
GO

CREATE TRIGGER [dbo].[trg_DeleteCollectionManager]
ON [dbo].[Collection]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    -- Delete from CollectionManager where the CollectionID matches the deleted CollectionID
    DELETE FROM CollectionManager
    WHERE AdministratingCollectionID IN (
        SELECT d.CollectionID
        FROM DELETED d
    );
END;
GO

ALTER TABLE [dbo].[Collection] ENABLE TRIGGER [trg_InsertCollectionManager]
GO
ALTER TABLE [dbo].[Collection] ENABLE TRIGGER [trg_DeleteCollectionManager]
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.53'
END

GO
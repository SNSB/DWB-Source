declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.55'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Restrict trg_UpdateCollectionUpdateCollectionClosure to update of CollectionParentID                   ######
--#####################################################################################################################


ALTER TRIGGER [dbo].[trg_UpdateCollectionUpdateCollectionClosure]
ON [dbo].[Collection]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(CollectionParentID)
    BEGIN
        -- Delete old relationships for the updated collection and its descendents
        DELETE FROM CollectionClosure
        WHERE DescendentID IN (
            SELECT DescendentID
            FROM CollectionClosure
            WHERE AncestorID IN (SELECT CollectionID FROM DELETED)
        )
        AND AncestorID IN (
            SELECT AncestorID
            FROM CollectionClosure
            WHERE DescendentID IN (SELECT CollectionID FROM DELETED)
            AND AncestorID != DescendentID
        );
        -- Insert new relationships for the updated collection and its descendents
        INSERT INTO CollectionClosure (AncestorID, DescendentID, Depth)
        SELECT 
            supertree.AncestorID, -- New ancestor
            subtree.DescendentID, -- Descendant
            supertree.Depth + subtree.Depth + 1 -- New depth
        FROM CollectionClosure AS supertree
        CROSS JOIN CollectionClosure AS subtree
        INNER JOIN INSERTED i ON subtree.AncestorID = i.CollectionID
        WHERE supertree.DescendentID = i.CollectionParentID;
    END;
END;
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.56'
END

GO
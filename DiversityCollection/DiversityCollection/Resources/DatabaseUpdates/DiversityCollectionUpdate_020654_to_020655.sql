declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.54'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  ManagerCollectionList - Redesign with CollectionClosure                                                ######
--######  This function returns all collections for which the current user has edit permissions. If the user     ######
--######  is an administrator, all collections are returned.                                                     ######
--#####################################################################################################################

ALTER FUNCTION [dbo].[ManagerCollectionList]()
RETURNS @CollectionList TABLE (
    [CollectionID] [INT] PRIMARY KEY,
    [CollectionParentID] [INT] NULL,
    [CollectionName] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [CollectionAcronym] [NVARCHAR](50) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactName] [NVARCHAR](500) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactAgentURI] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [Description] [NVARCHAR](4000) COLLATE Latin1_General_CI_AS NULL,
    [Location] [NVARCHAR](1000) COLLATE Latin1_General_CI_AS NULL,
    [CollectionOwner] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [DisplayOrder] [VARCHAR](255) COLLATE Latin1_General_CI_AS NULL
)
AS
BEGIN
    -- Check if the user is an Administrator or sysadmin
    IF IS_MEMBER('Administrator') = 1 OR IS_SRVROLEMEMBER('sysadmin') = 1
    BEGIN
        -- Return all collections
        INSERT INTO @CollectionList
        SELECT DISTINCT
            C.CollectionID,
            C.CollectionParentID,
            C.CollectionName,
            C.CollectionAcronym,
            C.AdministrativeContactName,
            C.AdministrativeContactAgentURI,
            C.Description,
            C.Location,
            C.CollectionOwner,
            C.DisplayOrder
        FROM Collection C;
    END
    ELSE
    BEGIN
        -- Return collections the user has access to
        INSERT INTO @CollectionList
        SELECT DISTINCT
            C.CollectionID,
            C.CollectionParentID,
            C.CollectionName,
            C.CollectionAcronym,
            C.AdministrativeContactName,
            C.AdministrativeContactAgentURI,
            C.Description,
            C.Location,
            C.CollectionOwner,
            C.DisplayOrder
        FROM Collection C
        INNER JOIN CollectionClosure CC ON C.CollectionID = CC.DescendentID
        WHERE CC.AncestorID IN (
            SELECT AdministratingCollectionID 
            FROM CollectionManager 
            WHERE LoginName = USER_NAME()
        );
    END
    RETURN;
END;
GO

GRANT SELECT ON [dbo].[ManagerCollectionList] TO [User] AS [dbo]
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a Manager has access to, including the child collections. For Administrators, all collections are returned.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ManagerCollectionList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a Manager has access to, including the child collections. For Administrators, all collections are returned.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ManagerCollectionList'
END CATCH;
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.55'
END

GO
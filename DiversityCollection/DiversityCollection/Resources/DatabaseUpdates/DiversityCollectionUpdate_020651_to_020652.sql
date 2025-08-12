declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.51'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Table for Closure pattern to refactor  CollectionHiearchy  #########################################
--#####################################################################################################################
CREATE TABLE [dbo].[CollectionClosure] (
    [AncestorID] [int] NOT NULL,
    [DescendentID] [int] NOT NULL,
    [Depth] [int] NOT NULL, -- Depth indicates the distance between ancestor and descendent
    CONSTRAINT [PK_CollectionClosure] PRIMARY KEY CLUSTERED (
        [AncestorID] ASC,
        [DescendentID] ASC
    )
);
GO

GRANT SELECT ON [dbo].[CollectionClosure] TO [User] AS [dbo]
GO

GRANT INSERT ON [dbo].[CollectionClosure] TO [Administrator]
GO
GRANT UPDATE ON [dbo].[CollectionClosure] TO [Administrator]
GO
GRANT DELETE ON [dbo].[CollectionClosure] TO [Administrator]
GO
GRANT INSERT ON [dbo].[CollectionClosure] TO [CollectionManager]
GO
GRANT UPDATE ON [dbo].[CollectionClosure] TO [CollectionManager]
GO


INSERT INTO [dbo].[CollectionClosure] (AncestorID, DescendentID, Depth)
SELECT CollectionID AS AncestorID, CollectionID AS DescendentID, 0 AS Depth
FROM Collection;
GO

INSERT INTO [dbo].[CollectionClosure] (AncestorID, DescendentID, Depth)
SELECT p.CollectionID AS AncestorID, c.CollectionID AS DescendentID, 1 AS Depth
FROM Collection p
INNER JOIN Collection c ON c.CollectionParentID = p.CollectionID;
GO

;WITH InsertCollectionClosureRecursiveCTE AS (
    -- Anchor: Direct parent-child relationships
    SELECT 
        p.CollectionID AS AncestorID, 
        c.CollectionID AS DescendentID, 
        1 AS Depth
    FROM Collection p
    INNER JOIN Collection c ON c.CollectionParentID = p.CollectionID
    UNION ALL
    -- Recursive: Traverse the hierarchy
    SELECT 
        r.AncestorID, 
        c.CollectionID AS DescendentID, 
        r.Depth + 1 AS Depth
    FROM InsertCollectionClosureRecursiveCTE r
    INNER JOIN Collection c ON c.CollectionParentID = r.DescendentID
)

INSERT INTO [dbo].[CollectionClosure] (AncestorID, DescendentID, Depth)
SELECT AncestorID, DescendentID, Depth
FROM InsertCollectionClosureRecursiveCTE r
WHERE NOT EXISTS (
    SELECT 1
    FROM [dbo].[CollectionClosure] cc
    WHERE cc.AncestorID = r.AncestorID AND cc.DescendentID = r.DescendentID
);
GO

CREATE INDEX IX_CollectionClosure_DescendentID
ON CollectionClosure (DescendentID);
GO

CREATE INDEX IX_CollectionClosure_AncestorID
ON CollectionClosure (AncestorID);
GO

--#####################################################################################################################
--######  trigger for updating CollectionClosure  ########################################################
--##################################################################################################

CREATE TRIGGER trg_InsertCollectionUpdateCollectionClosure
ON Collection
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    -- Step 1: Insert self-referencing row for the newly inserted collection
    INSERT INTO CollectionClosure (AncestorID, DescendentID, Depth)
    SELECT 
        i.CollectionID, -- AncestorID
        i.CollectionID, -- DescendantID
        0               -- Depth
    FROM INSERTED i;
   -- Step 2: Insert parent-child relationships
    INSERT INTO CollectionClosure (AncestorID, DescendentID, Depth)
    SELECT 
        p.AncestorID,   -- AncestorID (all ancestors of the parent)
        i.CollectionID, -- DescendantID (the newly inserted collection)
        p.Depth + 1     -- Depth (increment depth by 1)
    FROM CollectionClosure p
    INNER JOIN INSERTED i ON p.DescendentID = i.CollectionParentID;
END;
GO

CREATE TRIGGER trg_UpdateCollectionUpdateCollectionClosure
ON Collection
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
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
GO

CREATE TRIGGER trg_DeleteCollectionUpdateCollectionClosure
ON Collection
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    -- Delete relationships for the deleted collection and its descendants
    DELETE FROM CollectionClosure
    WHERE DescendentID IN (
        SELECT DescendentID
        FROM CollectionClosure
        WHERE AncestorID IN (SELECT CollectionID FROM DELETED)
    );
END;
GO

--#####################################################################################################################
--######  CollectionHierarchy func - Redesign  ########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionHierarchy] (@CollectionID INT)
RETURNS @CollectionList TABLE (
    [CollectionID] [INT] PRIMARY KEY,
    [CollectionParentID] [INT] NULL,
    [CollectionName] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [CollectionAcronym] [NVARCHAR](10) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactName] [NVARCHAR](500) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactAgentURI] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [Description] [NVARCHAR](MAX) COLLATE Latin1_General_CI_AS NULL,
    [Location] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [CollectionOwner] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [DisplayOrder] [SMALLINT] NULL,
    [Type] [NVARCHAR](50) NULL,
    [LocationPlan] [VARCHAR](500) NULL,
    [LocationPlanWidth] [FLOAT] NULL,
    [LocationGeometry] [GEOMETRY] NULL,
    [LocationHeight] [FLOAT] NULL,
    [LocationParentID] [INT] NULL,
    [LocationPlanDate] [DATETIME] NULL
)
AS
BEGIN
    -- Step 1: Find the root ancestor (where CollectionParentID is NULL)
    DECLARE @RootID INT;
    SELECT TOP 1 @RootID = AncestorID
    FROM CollectionClosure
    WHERE DescendentID = @CollectionID
      AND AncestorID NOT IN (SELECT CollectionID FROM Collection WHERE CollectionParentID IS NOT NULL)
    ORDER BY Depth ASC;
    -- Step 2: Retrieve the entire hierarchy starting from the root ancestor
    INSERT INTO @CollectionList
    SELECT 
        C.CollectionID,
        C.CollectionParentID,
        C.CollectionName,
        C.CollectionAcronym,
        C.AdministrativeContactName,
        C.AdministrativeContactAgentURI,
        C.Description,
        C.Location,
        C.CollectionOwner,
        C.DisplayOrder,
        C.Type,
        C.LocationPlan,
        C.LocationPlanWidth,
        C.LocationGeometry,
        C.LocationHeight,
        C.LocationParentID,
        C.LocationPlanDate
    FROM Collection C
    INNER JOIN CollectionClosure CC ON C.CollectionID = CC.DescendentID
    WHERE CC.AncestorID = @RootID;
    RETURN;
END;
GO

--#####################################################################################################################
--######  CollectionHierarchyAll - Redesign  ########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionHierarchyAll]()
RETURNS @CollectionList TABLE (
    [CollectionID] [INT] PRIMARY KEY,
    [CollectionParentID] [INT] NULL,
    [CollectionName] [NVARCHAR](500) COLLATE Latin1_General_CI_AS NULL,
    [CollectionAcronym] [NVARCHAR](50) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactName] [NVARCHAR](500) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactAgentURI] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [Description] [NVARCHAR](MAX) COLLATE Latin1_General_CI_AS NULL,
    [Location] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [LocationPlan] [VARCHAR](500) NULL,
    [LocationPlanWidth] [FLOAT] NULL,
    [LocationPlanDate] [DATETIME] NULL,
    [LocationHeight] [FLOAT] NULL,
    [CollectionOwner] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [DisplayOrder] [VARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [Type] [NVARCHAR](50) NULL,
    [LocationParentID] [INT] NULL,
    [DisplayText] [VARCHAR](900) COLLATE Latin1_General_CI_AS NULL
)
AS
BEGIN
    -- Step 1: Insert all collections into the temporary table
    INSERT INTO @CollectionList (
        CollectionID, CollectionParentID, CollectionName, CollectionAcronym, 
        AdministrativeContactName, AdministrativeContactAgentURI, Description, 
        Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, 
        CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText
    )
    SELECT DISTINCT 
        C.CollectionID,
        C.CollectionParentID,
        C.CollectionName,
        C.CollectionAcronym,
        C.AdministrativeContactName,
        C.AdministrativeContactAgentURI,
        C.Description,
        C.Location,
        C.LocationPlan,
        C.LocationPlanWidth,
        C.LocationPlanDate,
        C.LocationHeight,
        C.CollectionOwner,
        C.DisplayOrder,
        C.[Type],
        C.LocationParentID,
        -- Dynamically calculate DisplayText
        STUFF((
            SELECT ' | ' + 
                CASE 
                    WHEN Parent.CollectionAcronym IS NULL OR Parent.CollectionAcronym = '' THEN Parent.CollectionName
                    ELSE Parent.CollectionAcronym
                END
            FROM CollectionClosure CC
            INNER JOIN Collection Parent ON CC.AncestorID = Parent.CollectionID
            WHERE CC.DescendentID = C.CollectionID
            ORDER BY CC.Depth DESC
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 3, '') AS DisplayText -- Remove leading ' | '
    FROM Collection C
    INNER JOIN CollectionClosure CC 
        ON C.CollectionID = CC.DescendentID
    WHERE CC.AncestorID = CC.DescendentID;
    -- Step 2: Delete collections that do not meet the conditions
    DELETE FROM @CollectionList
    WHERE CollectionID NOT IN (
        -- Filter by UserCollectionList
        SELECT CollectionID FROM [dbo].[UserCollectionList]()
    );
    RETURN;
END;
GO

GRANT SELECT ON [dbo].[CollectionHierarchyAll] TO [User] AS [dbo]
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the whole hierarchy for the collections based on column CollectionParentID and table CollectionClosure',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchyAll'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the whole hierarchy for the collections based on column CollectionParentID and table CollectionClosure',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CollectionHierarchyAll'
END CATCH;
GO


--#####################################################################################################################
--######  UserCollectionList - adding view CollectionListForManager  ##################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[UserCollectionList]()
RETURNS @CollectionList TABLE (
    [CollectionID] [INT] PRIMARY KEY,
    [CollectionParentID] [INT] NULL,
    [CollectionName] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [CollectionAcronym] [NVARCHAR](50) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactName] [NVARCHAR](500) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactAgentURI] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [Description] [NVARCHAR](MAX) COLLATE Latin1_General_CI_AS NULL,
    [Location] [NVARCHAR](1000) COLLATE Latin1_General_CI_AS NULL,
    [CollectionOwner] [NVARCHAR](255) COLLATE Latin1_General_CI_AS NULL,
    [DisplayOrder] [SMALLINT],
    [Type] [NVARCHAR](50) NULL,
    [LocationPlan] [VARCHAR](500) NULL,
    [LocationPlanWidth] [FLOAT] NULL,
    [LocationGeometry] [GEOMETRY] NULL,
    [LocationHeight] [FLOAT] NULL,
    [LocationParentID] [INT] NULL,
    [LocationPlanDate] [DATETIME] NULL
)
AS
BEGIN
    DECLARE @TempCollectionID TABLE (CollectionID INT PRIMARY KEY);
    -- Check if the user has specific access rights
    IF (SELECT COUNT(*) FROM CollectionUser WHERE LoginName = USER_NAME()) > 0
    BEGIN
        -- If the user is a manager, include their managed collections
        IF (SELECT COUNT(*) FROM CollectionManager WHERE LoginName = USER_NAME()) > 0
        BEGIN
            INSERT INTO @TempCollectionID (CollectionID)
            SELECT DISTINCT CC.DescendentID
            FROM CollectionManager CM
            INNER JOIN CollectionClosure CC ON CM.AdministratingCollectionID = CC.AncestorID
            WHERE CM.LoginName = USER_NAME();
        END
        -- Include collections explicitly assigned to the user
        INSERT INTO @TempCollectionID (CollectionID)
        SELECT DISTINCT CC.DescendentID
        FROM CollectionUser CU
        INNER JOIN CollectionClosure CC ON CU.CollectionID = CC.AncestorID
        WHERE CU.LoginName = USER_NAME()
          AND CC.DescendentID NOT IN (SELECT CollectionID FROM @TempCollectionID);
    END
    -- If no specific access rights, grant access to all collections
    IF (SELECT COUNT(*) FROM @TempCollectionID) = 0
    BEGIN
        INSERT INTO @TempCollectionID (CollectionID)
        SELECT CollectionID FROM Collection;
    END
    -- Populate the result table with collection details
    INSERT INTO @CollectionList
    SELECT 
        C.CollectionID,
        C.CollectionParentID,
        C.CollectionName,
        C.CollectionAcronym,
        C.AdministrativeContactName,
        C.AdministrativeContactAgentURI,
        C.Description,
        C.Location,
        C.CollectionOwner,
        C.DisplayOrder,
        C.Type,
        C.LocationPlan,
        C.LocationPlanWidth,
        C.LocationGeometry,
        C.LocationHeight,
        C.LocationParentID,
        C.LocationPlanDate
    FROM Collection C
    WHERE C.CollectionID IN (SELECT CollectionID FROM @TempCollectionID);
    RETURN;
END;
GO
GRANT SELECT ON [dbo].[UserCollectionList] TO [User] AS [dbo]
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a User has access to, including the child collections.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'UserCollectionList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a User has access to, including the child collections.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'UserCollectionList'
END CATCH;
GO

--#####################################################################################################################
--######  CollectionUser_log - for logging changes  ###################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionUser_log](
	[LoginName] [nvarchar](50) NULL,
	[CollectionID] [int] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionUser_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionUser_log] ADD  CONSTRAINT [DF_CollectionUser_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionUser_log] ADD  CONSTRAINT [DF_CollectionUser_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionUser_log] ADD  CONSTRAINT [DF_CollectionUser_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID](),0)) FOR [LogUser]
GO

GRANT SELECT ON dbo.CollectionUser_log TO [CollectionManager]
GO
GRANT SELECT ON dbo.CollectionUser_log TO [Administrator]
GO

--#####################################################################################################################
--######  trgDelCollectionUser - for logging changes  #################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionUser] ON [dbo].[CollectionUser] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.4.13 */ 
/*  Date: 1/29/2025  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionUser_Log (CollectionID, LoginName, RowGUID, LogUser,  LogState) 
SELECT D.CollectionID, D.LoginName, D.RowGUID, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D 
GO

ALTER TABLE [dbo].[CollectionUser] ENABLE TRIGGER [trgDelCollectionUser]
GO


--#####################################################################################################################
--######  ManagerCollectionList - Redesign for cached values  #########################################################
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
    -- Insert all collections a manager has access to, including child collections
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
    RETURN;
END;
GO

GRANT SELECT ON [dbo].[ManagerCollectionList] TO [User] AS [dbo]
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a Manager has access to, including the child collections.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ManagerCollectionList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collections a Manager has access to, including the child collections.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ManagerCollectionList'
END CATCH;
GO

--#####################################################################################################################
--######  ManagerCollection_log for logging changes  ##################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionManager_log](
	[LoginName] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionManager_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionManager_log] ADD  CONSTRAINT [DF_CollectionManager_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionManager_log] ADD  CONSTRAINT [DF_CollectionManager_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionManager_log] ADD  CONSTRAINT [DF_CollectionManager_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID](),0)) FOR [LogUser]
GO

GRANT SELECT ON dbo.CollectionManager_log TO [CollectionManager]
GO
GRANT SELECT ON dbo.CollectionManager_log TO [Administrator]
GO

--#####################################################################################################################
--######  trgDelCollectionManager for logging changes  ################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionManager] ON [dbo].[CollectionManager] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.4.13 */ 
/*  Date: 1/29/2025  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionManager_Log (AdministratingCollectionID, LoginName, RowGUID, LogUser,  LogState) 
SELECT D.AdministratingCollectionID, D.LoginName, D.RowGUID, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D 
GO

ALTER TABLE [dbo].[CollectionManager] ENABLE TRIGGER [trgDelCollectionManager]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.52'
END

GO



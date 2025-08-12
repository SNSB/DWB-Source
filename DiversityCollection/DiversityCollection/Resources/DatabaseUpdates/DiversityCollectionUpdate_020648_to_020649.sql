declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.48'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   New material category core sample  #########################################################################
--#####################################################################################################################

IF(SELECT COUNT(*) FROM [CollMaterialCategory_Enum] WHERE [Code] = 'core sample') = 0
BEGIN
INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes]
           ,[ParentCode])
     VALUES
           ('core sample'
           ,'core sample like an ice core, a drill core sample from soil, wood or rock etc.'
           ,'core sample'
           ,830
           ,1
           ,'especially for wood core samples'
           ,'other specimen')

END
GO



--#####################################################################################################################
--######   CollectionChildNodes - optimized   #########################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[CollectionChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
    [CollectionID] [int] PRIMARY KEY,
    [CollectionParentID] [int] NULL,
    [CollectionName] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
    [CollectionAcronym] [nvarchar](10) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactName] [nvarchar](500) COLLATE Latin1_General_CI_AS NULL,
    [AdministrativeContactAgentURI] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
    [Description] [nvarchar](max) COLLATE Latin1_General_CI_AS NULL,
    [Location] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
    [LocationPlan] [varchar](500) NULL,
    [LocationPlanWidth] [float] NULL,
    [LocationPlanDate] [datetime] NULL,
    [LocationHeight] [float] NULL,
    [CollectionOwner] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
    [DisplayOrder] [smallint] NULL,
    [Type] [nvarchar](50) NULL,
    [LocationParentID] [int] NULL)
AS
BEGIN
    ;WITH RecursiveCTE AS (
        SELECT
            CollectionID, CollectionParentID, CollectionName, CollectionAcronym,
            AdministrativeContactName, AdministrativeContactAgentURI, Description,
            Location, LocationPlan, LocationPlanWidth, LocationPlanDate,
            LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
        FROM Collection
        WHERE CollectionParentID = @ID
        UNION ALL
        SELECT
            c.CollectionID, c.CollectionParentID, c.CollectionName, c.CollectionAcronym,
            c.AdministrativeContactName, c.AdministrativeContactAgentURI, c.Description,
            c.Location, c.LocationPlan, c.LocationPlanWidth, c.LocationPlanDate,
            c.LocationHeight, c.CollectionOwner, c.DisplayOrder, c.[Type], c.LocationParentID
        FROM Collection c
        INNER JOIN RecursiveCTE r ON c.CollectionParentID = r.CollectionID
    )
    INSERT INTO @ItemList
    SELECT * FROM RecursiveCTE
    RETURN
END
GO

--#####################################################################################################################
--######   CollectionHierarchyAll - optimized ?  #########################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionHierarchyAll] ()  
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
    ;WITH CollectionHierarchyCT AS (
        -- Anchor member: Select root collections
        SELECT 
            CollectionID,
            CollectionParentID,
            CollectionName,
            CollectionAcronym,
            AdministrativeContactName,
            AdministrativeContactAgentURI,
            Description,
            Location,
            LocationPlan,
            LocationPlanWidth,
            LocationPlanDate,
            LocationHeight,
            CollectionOwner,
            DisplayOrder,
            [Type],
            LocationParentID,
            CAST(CASE WHEN CollectionAcronym IS NULL OR CollectionAcronym = '' THEN CollectionName ELSE CollectionAcronym END AS VARCHAR(900)) AS DisplayText
        FROM Collection
        WHERE CollectionParentID IS NULL
        UNION ALL
        -- Recursive member: Select child collections
        SELECT 
            c.CollectionID,
            c.CollectionParentID,
            c.CollectionName,
            c.CollectionAcronym,
            c.AdministrativeContactName,
            c.AdministrativeContactAgentURI,
            c.Description,
            c.Location,
            c.LocationPlan,
            c.LocationPlanWidth,
            c.LocationPlanDate,
            c.LocationHeight,
            c.CollectionOwner,
            c.DisplayOrder,
            c.[Type],
            c.LocationParentID,
            CAST(r.DisplayText + ' | ' + 
                CASE 
                    WHEN c.CollectionAcronym IS NULL OR c.CollectionAcronym = '' OR 
                         (c.CollectionAcronym <> '' AND c.CollectionAcronym = r.CollectionAcronym) 
                    THEN c.CollectionName 
                    ELSE c.CollectionAcronym 
                END AS VARCHAR(900)) AS DisplayText
        FROM Collection c
        INNER JOIN CollectionHierarchyCT r ON c.CollectionParentID = r.CollectionID
    )
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
        CH.DisplayText
    FROM CollectionHierarchyCT CH
    INNER JOIN Collection C ON CH.CollectionID = C.CollectionID
    WHERE 
        -- Include all collections if the user is an administrator or dbo
        (
            (SELECT COUNT(*) 
             FROM sys.database_principals pR
             INNER JOIN sys.database_role_members rm ON rm.role_principal_id = pR.principal_id
             INNER JOIN sys.database_principals pU ON rm.member_principal_id = pU.principal_id
             WHERE pU.type <> 'R'
               AND pU.Name = USER_NAME()
               AND pR.name = 'Administrator') > 0
            OR USER_NAME() = 'dbo'
        )
        -- Otherwise, filter by UserCollectionList
        OR CH.CollectionID IN (SELECT CollectionID FROM [dbo].[UserCollectionList]());
    RETURN;
END
GO

/****** Object:  Index [Index_CollectionID_CollectionParentID]    Script Date: 19.11.2024 17:25:43 ******/
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'Index_CollectionID_CollectionParentID' AND object_id = OBJECT_ID('Collection'))
BEGIN
CREATE NONCLUSTERED INDEX  [Index_CollectionID_CollectionParentID] ON [dbo].[Collection]
(
	[CollectionID] ASC,
	[CollectionParentID] ASC
);
END
GO


IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'Index_CollectionParentID' AND object_id = OBJECT_ID('Collection'))
BEGIN
CREATE NONCLUSTERED INDEX [Index_CollectionParentID] ON [dbo].[Collection]
(
	[CollectionParentID] ASC
);
END

GO
-- Before we can add an index ProjectID on CollectionProject we have to change the View CollectionSpecimenID_AvailableReadOnly
-- because with the index the "SELECT CollectionSpecimenID FROM CollectionSpecimenID_AvailableReadOnly" runs into a timeout
-- Please check if this view could be changed to: (this would reduce time, especially when we also add the index on Project_Id)
-----------------------
--WITH UserProjects AS (
--    SELECT P.CollectionSpecimenID, U.ReadOnly
--    FROM dbo.CollectionProject AS P
--    INNER JOIN dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
--    WHERE U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()
--),
--NonReadOnlySpecimens AS (
--    SELECT P.CollectionSpecimenID
--    FROM dbo.CollectionProject AS P
--    INNER JOIN dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
--    WHERE (U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) AND U.ReadOnly = 0
--),
--LockedSpecimens AS (
--    SELECT CollectionSpecimenID
--    FROM [CollectionSpecimenID_Locked]
--)
--SELECT DISTINCT P.CollectionSpecimenID
--FROM UserProjects AS P
--LEFT JOIN NonReadOnlySpecimens AS NR ON P.CollectionSpecimenID = NR.CollectionSpecimenID
--LEFT JOIN LockedSpecimens AS L ON P.CollectionSpecimenID = L.CollectionSpecimenID
--WHERE (P.ReadOnly = 1 AND NR.CollectionSpecimenID IS NULL) OR L.CollectionSpecimenID IS NOT NULL
--ORDER BY P.CollectionSpecimenID;
-------------------------------
--CREATE NONCLUSTERED INDEX [Index_ProjectID] ON [dbo].[CollectionProject]
--(
--	[ProjectID] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.49'
END

GO


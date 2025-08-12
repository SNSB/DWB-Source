
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.34'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   isoparatype   ######################################################################################
--#####################################################################################################################

UPDATE [CollTypeStatus_Enum]
   SET [DisplayEnable] = 1
 WHERE Code = 'isoparatype'
GO



--#####################################################################################################################
--######   [CollectionHierarchy]   ######################################################################################
--#####################################################################################################################


/****** Object:  UserDefinedFunction [dbo].[CollectionHierarchy]    Script Date: 07/08/2013 17:07:22 ******/

ALTER FUNCTION [dbo].[CollectionHierarchy] (@CollectionID int)  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL)

/*
Returns a table that lists all the analysis items related to the given analysis.
MW 02.01.2006
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int

set @TopID = (select CollectionParentID from Collection where CollectionID = @CollectionID) 

set @i = (select count(*) from Collection where CollectionID = @CollectionID)

if (@TopID is null )
	set @TopID =  @CollectionID
else	
	begin
	while (@i > 0)
		begin
		set @CollectionID = (select CollectionParentID from Collection where CollectionID = @CollectionID and not CollectionParentID is null) 
		set @i = (select count(*) from Collection where CollectionID = @CollectionID and not CollectionParentID is null)
		end
	set @TopID = @CollectionID
	end

-- copy the root node in the result list

   INSERT @CollectionList
   SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
   FROM Collection
   WHERE Collection.CollectionID = @TopID

-- copy the child nodes into the result list
  INSERT @CollectionList
   SELECT * FROM dbo.CollectionChildNodes (@TopID)

   RETURN
END

GO


/****** Object:  UserDefinedFunction [dbo].[CollectionChildNodes]    Script Date: 07/08/2013 17:07:39 ******/

ALTER FUNCTION [dbo].[CollectionChildNodes] (@ID int)  
RETURNS @ItemList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL)

INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
	FROM Collection WHERE CollectionParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select CollectionID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
	from dbo.CollectionChildNodes (@ParentID) where CollectionID not in (select CollectionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
   FROM @TempItem ORDER BY CollectionName
   RETURN
END

GO

--#####################################################################################################################
--######   ProjectUser ADD [ReadOnly]  ######################################################################################
--#####################################################################################################################


ALTER TABLE ProjectUser ADD [ReadOnly] [bit] NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user has only read access to data of this project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'ReadOnly'
GO

ALTER TABLE [dbo].[ProjectUser] ADD  CONSTRAINT [DF_ProjectUser_ReadOnly]  DEFAULT ((0)) FOR [ReadOnly]
GO

UPDATE ProjectUser SET [ReadOnly] = 0
GO

--#####################################################################################################################
--######   [CollectionSpecimenID_ReadOnly]   ######################################################################################
--#####################################################################################################################


CREATE VIEW [dbo].[CollectionSpecimenID_ReadOnly]
AS
SELECT     P.CollectionSpecimenID
FROM         dbo.CollectionProject P INNER JOIN
                      dbo.ProjectUser U ON P.ProjectID = U.ProjectID
WHERE     (U.LoginName = USER_NAME() and U.ReadOnly = 1)
and P.CollectionSpecimenID not in (SELECT [CollectionSpecimenID]
  FROM [DiversityCollection_Test].[dbo].[CollectionSpecimenID_UserAvailable])
GROUP BY P.CollectionSpecimenID

GO

GRANT SELECT ON [CollectionSpecimenID_ReadOnly] TO [USER]
GO


--#####################################################################################################################
--######   [CollectionSpecimenID_UserAvailable]   ######################################################################################
--#####################################################################################################################


ALTER VIEW [dbo].[CollectionSpecimenID_UserAvailable]
AS
SELECT     dbo.CollectionProject.CollectionSpecimenID
FROM         dbo.CollectionProject INNER JOIN
                      dbo.ProjectUser ON dbo.CollectionProject.ProjectID = dbo.ProjectUser.ProjectID
WHERE     (dbo.ProjectUser.LoginName = USER_NAME() and dbo.ProjectUser.ReadOnly = 0)
GROUP BY dbo.CollectionProject.CollectionSpecimenID
UNION
SELECT     TOP (100) PERCENT dbo.CollectionSpecimen.CollectionSpecimenID
FROM         dbo.CollectionProject AS CollectionProject_1 RIGHT OUTER JOIN
                      dbo.CollectionSpecimen ON CollectionProject_1.CollectionSpecimenID = dbo.CollectionSpecimen.CollectionSpecimenID
GROUP BY dbo.CollectionSpecimen.CollectionSpecimenID, CollectionProject_1.ProjectID
HAVING      (CollectionProject_1.ProjectID IS NULL)
ORDER BY CollectionSpecimenID

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.35'
END

GO



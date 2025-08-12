declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.15'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   CollTransactionType_Enum. New entry permanent loan   #######################################################
--#####################################################################################################################

if (select count(*) from [dbo].[CollTransactionType_Enum] where Code = 'permanent loan') = 0
begin
	INSERT INTO [dbo].[CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('permanent loan'
           ,'permanent loan'
           ,'permanent loan'
           ,1)
		   end
GO



--#####################################################################################################################
--######   CollCollectionType_Enum. New table for the type of a collection   ##########################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'CollCollectionType_Enum') = 0
begin
	CREATE TABLE [dbo].[CollCollectionType_Enum](
		[Code] [nvarchar](50) NOT NULL,
		[Description] [nvarchar](500) NULL,
		[DisplayText] [nvarchar](50) NULL,
		[DisplayOrder] [smallint] NULL,
		[DisplayEnable] [bit] NULL,
		[InternalNotes] [nvarchar](500) NULL,
		[ParentCode] [nvarchar](50) NULL,
		[Icon] [image] NULL,
		[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	 CONSTRAINT [PK_CollCollectionType_Enum] PRIMARY KEY CLUSTERED 
	(
		[Code] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[CollCollectionType_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]

	ALTER TABLE [dbo].[CollCollectionType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_CollCollectionType_Enum_CollCollectionType_Enum] FOREIGN KEY([ParentCode])
	REFERENCES [dbo].[CollCollectionType_Enum] ([Code])

	ALTER TABLE [dbo].[CollCollectionType_Enum] CHECK CONSTRAINT [FK_CollCollectionType_Enum_CollCollectionType_Enum]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum', @level2type=N'COLUMN',@level2name=N'Code'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum', @level2type=N'COLUMN',@level2name=N'Description'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A symbol representing this entry in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum', @level2type=N'COLUMN',@level2name=N'Icon'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The types of a collection, e.g. cupboard, drawer, box, rack etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionType_Enum'
end

GRANT SELECT ON [CollCollectionType_Enum] TO [User]
GO
GRANT INSERT ON [CollCollectionType_Enum] TO [Administrator]
GO
GRANT UPDATE ON [CollCollectionType_Enum] TO [Administrator]
GO
GRANT DELETE ON [CollCollectionType_Enum] TO [Administrator]
GO

--#####################################################################################################################
--######   CollCollectionType_Enum. Insert new types   ################################################################
--#####################################################################################################################

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'cupboard') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
			   ([Code]
			   ,[Description]
			   ,[DisplayText]
			   ,DisplayEnable)
		 VALUES
			   ('cupboard'
			   ,'cupboard'
			   ,'cupboard'
			   ,1)
	end
GO

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'drawer') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('drawer'
           ,'drawer'
           ,'drawer'
		   ,1)
	end
GO

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'box') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('box'
           ,'box'
           ,'box'
		   ,1)
	end
GO

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'institution') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('institution'
           ,'institution'
           ,'institution'
		   ,1)
	end
GO

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'room') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('room'
           ,'room'
           ,'room'
		   ,1)
	end
GO

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'steel locker') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('steel locker'
           ,'steel locker'
           ,'steel locker'
		   ,1)
	end
GO

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'radioactive') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('radioactive'
           ,'Place where radioactive objects are stored'
           ,'radioactive'
		   ,1)
	end
GO

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'freezer') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('freezer'
           ,'freezer'
           ,'freezer'
		   ,1)
	end
GO

if (select count(*) from [dbo].[CollCollectionType_Enum] where Code = 'fridge') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('fridge'
           ,'fridge'
           ,'fridge'
		   ,1)
	end
GO


--#####################################################################################################################
--######   Collection: Add column Type and adapt trigger   ############################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Collection' and C.COLUMN_NAME = 'Type') = 0
begin
	ALTER TABLE [Collection] ADD [Type] [nvarchar](50) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the collection, e.g. cupboard, drawer etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Collection', @level2type=N'COLUMN',@level2name=N'Type'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Collection_log' and C.COLUMN_NAME = 'Type') = 0
begin
	ALTER TABLE [Collection_log] ADD [Type] [nvarchar](50) NULL
end
GO

ALTER TABLE [dbo].[Collection]  WITH CHECK ADD  CONSTRAINT [FK_Collection_CollCollectionType_Enum] FOREIGN KEY([Type])
REFERENCES [dbo].[CollCollectionType_Enum] ([Code])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Collection] CHECK CONSTRAINT [FK_Collection_CollCollectionType_Enum]
GO


ALTER TRIGGER [dbo].[trgDelCollection] ON [dbo].[Collection] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 
/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type], RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.CollectionID, deleted.CollectionParentID, deleted.CollectionName, deleted.CollectionAcronym, deleted.AdministrativeContactName, deleted.AdministrativeContactAgentURI, deleted.Description, deleted.Location, deleted.CollectionOwner, deleted.DisplayOrder, deleted.[Type], deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED
GO


ALTER TRIGGER [dbo].[trgUpdCollection] ON [dbo].[Collection] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 
/* updating the logging columns */
Update Collection
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM Collection, deleted 
where 1 = 1 
AND Collection.CollectionID = deleted.CollectionID
/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type], RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.CollectionID, deleted.CollectionParentID, deleted.CollectionName, deleted.CollectionAcronym, deleted.AdministrativeContactName, deleted.AdministrativeContactAgentURI, deleted.Description, deleted.Location, deleted.CollectionOwner, deleted.DisplayOrder, deleted.[Type], deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED
GO

--#####################################################################################################################
--######   CollectionChildNodes - New column Type   ###################################################################
--#####################################################################################################################


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
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   RETURN
END
GO

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
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL)
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
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL)
INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]
	FROM Collection WHERE CollectionParentID = @ID 
   DECLARE HierarchyCursor  CURSOR for
   select CollectionID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]
	from dbo.CollectionChildNodes (@ParentID) where CollectionID not in (select CollectionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]
   FROM @TempItem ORDER BY CollectionName
   RETURN
END

GO


--#####################################################################################################################
--######   CollectionHierarchy - New column Type   ####################################################################
--#####################################################################################################################


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
	[DisplayOrder] [smallint]  NULL,
	[Type] [nvarchar](50) NULL)
/*
Returns a table that lists all the analysis items related to the given analysis.
MW 02.01.2006
*/
AS
BEGIN
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
   INSERT @CollectionList
   SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]
   FROM Collection
   WHERE Collection.CollectionID = @TopID
  INSERT @CollectionList
   SELECT * FROM dbo.CollectionChildNodes (@TopID)
   RETURN
END
GO

--#####################################################################################################################
--######   CollectionHierarchyAll - New column Type   #################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[CollectionHierarchyAll] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL)
/*
Returns a table that lists all the collections including a display text with the whole hierarchy.
MW 20.02.2012
TEST:
SELECT * FROM DBO.CollectionHierarchyAll()
*/
AS
BEGIN
	INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type], DisplayText)
	SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type]
	, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
	FROM Collection C
	WHERE C.CollectionParentID IS NULL
	declare @i int
	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @CollectionList))
	while (@i > 0)
		begin
		INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, [Type], DisplayText)
		SELECT DISTINCT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.CollectionOwner
		, C.DisplayOrder, C.[Type], L.DisplayText + ' | ' + C.CollectionName
		FROM Collection C, @CollectionList L
		WHERE C.CollectionParentID = L.CollectionID
		AND C.CollectionID NOT IN (select CollectionID from  @CollectionList)
		set @i = (select count(*) from Collection C where CollectionID not IN (select CollectionID from  @CollectionList))
	end
	Declare @A int
	set @A = (select COUNT(*) 
	from sys.database_principals pR, sys.database_role_members, sys.database_principals pU  
	where sys.database_role_members.role_principal_id = pR.principal_id  
	and sys.database_role_members.member_principal_id = pU.principal_id 
	and pU.type <> 'R' 
	and pU.Name = User_Name()
	and pR.name = 'Administrator'
	)
	if (@A = 0 and User_Name() = 'dbo')
		begin 	set @A = 1 end
	if @A= 0
	begin
		DELETE L FROM @CollectionList L WHERE L.CollectionID NOT IN ( SELECT CollectionID FROM [dbo].[UserCollectionList] ())
	end
   RETURN
END

GO


--#####################################################################################################################
--######   CollectionHierarchyMulti - New column Type   ###############################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[CollectionHierarchyMulti] (@CollectionIDs varchar(255))  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL)

/*
Returns a table that lists all the collections related to the given collection in the list.
MW 02.01.2006
*/

AS
BEGIN

SET @CollectionIDs = rtrim(ltrim(@CollectionIDs))
if @CollectionIDs = '' 
begin return end
else
while rtrim(@CollectionIDs) <> '' 
begin
	declare @CollectionID int
	if charindex(' ', @CollectionIDs) = 0
	begin
	set @CollectionID = cast(@CollectionIDs as int)
	set @CollectionIDs = ''
	end
	else
	begin
	set @CollectionID = cast(substring(@CollectionIDs, 1, charindex(' ', @CollectionIDs)) as int)
	end
	
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
	Description, Location, CollectionOwner, DisplayOrder, [Type]
	FROM Collection
	WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	AND CollectionID = @CollectionID
	declare @TopID int
	set @TopID = (SELECT CollectionParentID FROM Collection WHERE CollectionID = @CollectionID)
	while not @TopID is null
	begin
		INSERT @CollectionList
		SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
		Description, Location, CollectionOwner, DisplayOrder, [Type]
		FROM Collection
		WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
		AND CollectionID = @TopID
		set @TopID = (SELECT CollectionParentID FROM Collection WHERE CollectionID = @TopID)
	end

	SET @CollectionIDs = ltrim(substring(@CollectionIDs, charindex(' ', @CollectionIDs), 255))
END

   RETURN
END
GO




--#####################################################################################################################
--######   EntityRepresentation - Trigger for update and defaults   ###################################################
--#####################################################################################################################

if (select count(*)
from sys.default_constraints chk
inner join sys.columns col
    on chk.parent_object_id = col.object_id
inner join sys.tables st
    on chk.parent_object_id = st.object_id
where 
st.name = 'EntityRepresentation'
and col.name = 'LogUpdatedBy'
and col.column_id = chk.parent_column_id) = 0
begin
	ALTER TABLE [dbo].[EntityRepresentation] ADD  DEFAULT ([dbo].[UserID]()) FOR [LogUpdatedBy]
end
GO

if (select count(*)
from sys.default_constraints chk
inner join sys.columns col
    on chk.parent_object_id = col.object_id
inner join sys.tables st
    on chk.parent_object_id = st.object_id
where 
st.name = 'EntityRepresentation'
and col.name = 'LogCreatedBy'
and col.column_id = chk.parent_column_id) = 0
begin
	ALTER TABLE [dbo].[EntityRepresentation] ADD  DEFAULT ([dbo].[UserID]()) FOR [LogCreatedBy]
end
GO

if (select count(*)
from sys.default_constraints chk
inner join sys.columns col
    on chk.parent_object_id = col.object_id
inner join sys.tables st
    on chk.parent_object_id = st.object_id
where 
st.name = 'EntityRepresentation'
and col.name = 'LogUpdatedWhen'
and col.column_id = chk.parent_column_id) = 0
begin
	ALTER TABLE [dbo].[EntityRepresentation] ADD  DEFAULT (getdate()) FOR [LogUpdatedWhen]
end
GO

if (select count(*)
from sys.default_constraints chk
inner join sys.columns col
    on chk.parent_object_id = col.object_id
inner join sys.tables st
    on chk.parent_object_id = st.object_id
where 
st.name = 'EntityRepresentation'
and col.name = 'LogCreatedWhen'
and col.column_id = chk.parent_column_id) = 0
begin
	ALTER TABLE [dbo].[EntityRepresentation] ADD  DEFAULT (getdate()) FOR [LogCreatedWhen]
end
GO

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE TRIGGER [trgUpdEntityRepresentation] ON [dbo].[EntityRepresentation] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.9.3 */ 
/*  Date: 11/20/2018  */ 
/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM EntityRepresentation T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.Entity = D.Entity
AND T.EntityContext = D.EntityContext
AND T.LanguageCode = D.LanguageCode')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   CollectionEvent - CollectionDate: Updated description   ####################################################
--#####################################################################################################################

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The cached date of the collection event calulated from the entries in CollectionDay, -Month and -Year.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEvent', @level2type=N'COLUMN',@level2name=N'CollectionDate'
GO

--#################################################################################################################################
--######   trgInsCollectionEvent - Restricting the setting of the CollectionDate to valid dates retrieved from the parts   ########
--#################################################################################################################################

ALTER TRIGGER [dbo].[trgInsCollectionEvent] ON [dbo].[CollectionEvent] 
FOR INSERT AS

/*  Date: 06.11.2018  */ 

/* setting the date fields */ 
Update CollectionEvent set CollectionDate = 
case when E.CollectionMonth is null or E.CollectionDay is null or E.CollectionYear is null then null 
else case when ISDATE(convert(varchar(40), cast(E.CollectionYear as varchar) + '-' 
	+ case when I.CollectionMonth < 10 then '0' else '' end + cast(I.CollectionMonth as varchar)  + '-' 
	+ case when I.CollectionDay < 10 then '0' else '' end + cast(I.CollectionDay as varchar) + 'T00:00:00.000Z', 127)) = 1
	AND I.CollectionYear > 1760
	AND I.CollectionMonth between 1 and 12
	AND I.CollectionDay between 1 and 31
then cast(convert(varchar(40), cast(I.CollectionYear as varchar) + '-' 
	+ case when I.CollectionMonth < 10 then '0' else '' end + cast(I.CollectionMonth as varchar)  + '-' 
	+ case when I.CollectionDay < 10 then '0' else '' end + cast(I.CollectionDay as varchar) + 'T00:00:00.000Z', 127) as datetime)
else null end end 
FROM CollectionEvent E, inserted I  
where E.CollectionEventID = I.CollectionEventID   

GO


--#####################################################################################################################
--######   trgUpdCollectionEvent - Bugfix for setting date with missing parts   #######################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionEvent] ON [dbo].[CollectionEvent]  
 FOR UPDATE AS 
 /*  Created by DiversityWorkbench Administration.  */  
 /*  DiversityWorkbenchMaintenance  2.0.0.3 */  
 /*  Date: 30.08.2007  */  
 /*  Changed: 06.11.2018 - Bugfix for setting date with missing parts */
 if not update(Version)  
 BEGIN  /* setting the version in the main table */  
	 DECLARE @i int  
	 DECLARE @ID int 
	 DECLARE @Version int  
	 set @i = (select count(*) from deleted)   
	 if @i = 1  
	 BEGIN     
		 SET  @ID = (SELECT CollectionEventID FROM deleted)    
		 EXECUTE procSetVersionCollectionEvent @ID 
	 END   

	 -- setting the column CollectionDate for valid dates derived from the columns CollectionDay, CollectionMonth and CollectionYear
	Update CollectionEvent set CollectionDate = case when E.CollectionMonth is null or E.CollectionDay is null or E.CollectionYear is null 
	then null 
	else 
	case when ISDATE(convert(varchar(40), cast(E.CollectionYear as varchar) + '-' 
		+ case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar)  + '-' 
		+ case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000Z', 127)) = 1
	then cast(convert(varchar(40), cast(D.CollectionYear as varchar) + '-' 
		+ case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar)  + '-' 
		+ case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000Z', 127) as datetime)	 
	else null end  
	end
	FROM CollectionEvent E, deleted D 
	where E.CollectionEventID = D.CollectionEventID   

	 /* saving the original dataset in the logging table */  
	 INSERT INTO CollectionEvent_Log (
	 CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, 
	 CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionDateCategory, CollectionTime, 
	 CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, ReferenceDetails, 
	 CollectingMethod, Notes, CountryCache, DataWithholdingReason, RowGUID,  LogCreatedWhen, 
	 LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, LogState, LocalityVerbatim, CollectionEndDay, CollectionEndMonth, CollectionEndYear, DataWithholdingReasonDate)  
	 SELECT D.CollectionEventID, D.Version, D.SeriesID, D.CollectorsEventNumber, D.CollectionDate, 
	 D.CollectionDay, D.CollectionMonth, D.CollectionYear, D.CollectionDateSupplement, D.CollectionDateCategory, D.CollectionTime, 
	 D.CollectionTimeSpan, D.LocalityDescription, D.HabitatDescription, D.ReferenceTitle, D.ReferenceURI, D.ReferenceDetails, 
	 D.CollectingMethod, D.Notes, D.CountryCache, D.DataWithholdingReason, D.RowGUID,  D.LogCreatedWhen, 
	 D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, 'U', D.LocalityVerbatim, D.CollectionEndDay, D.CollectionEndMonth, D.CollectionEndYear, D.DataWithholdingReasonDate
	 FROM DELETED  D
 END  
 
 Update CollectionEvent set LogUpdatedWhen = getdate(), LogUpdatedBy =  suser_sname()
 FROM CollectionEvent, deleted  
 where 1 = 1  AND CollectionEvent.CollectionEventID = deleted.CollectionEventID

 GO



--#####################################################################################################################
--######  EntityInformation_2: bugfix for entities with several "."  ##################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[EntityInformation_2] (@Entity [varchar] (500), @Language nvarchar(50), @Context nvarchar(50))  
RETURNS @EntityList TABLE ([Entity] [varchar] (500) Primary key ,
	[DisplayGroup] [nvarchar](50) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Abbreviation] [nvarchar](20) NULL,
	[Description] [nvarchar](max) NULL,
	[Accessibility] [nvarchar](50) NULL,
	[Determination] [nvarchar](50) NULL,
	[Visibility] [nvarchar](50) NULL,
	[PresetValue] [nvarchar](500) NULL,
	[UsageNotes] [nvarchar](4000) NULL,
	[DoesExist] [bit] NULL,
	[DisplayTextOK] [bit] NULL,
	[AbbreviationOK] [bit] NULL,
	[DescriptionOK] [bit] NULL)
/*
Returns the information to an entity.
MW 26.09.2011
Test:
select * from [dbo].[EntityInformation_2] ('CollCircumstances_Enum.Code.bred', 'de-DE', 'General')
select * from [dbo].[EntityInformation_2] ('CollLabelType_Enum', 'de-DE', 'Observation.mobile')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'de-DE', 'Observation')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'de-DE', '')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'en-US', 'Observation')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'en-US', '')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', '', '')
select * from [dbo].[EntityInformation_2] ('EntityUsage.PresetValue', '', '')
select * from [dbo].[EntityInformation_2] ('Identification.CollectionSpecimenID', 'en-US', 'Observation')
select * from [dbo].[EntityInformation_2] ('Identification.CollectionSpecimenID', 'en-US', 'General')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen.CollectionSpecimenID', 'en-US', 'General')
select * from [dbo].[EntityInformation_2] ('Identification.IdentificationDate', 'de-DE', 'Observation')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'de-DE', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('Test', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('CollMaterialCategory_Enum.Code.drawing or photograph', 'de-DE', 'General')
*/

AS
BEGIN

if @Context = '' begin set @Context = 'General' end

-- fill the list with the basic data from the table Entity

insert @EntityList (Entity, [DisplayGroup]
	, [DoesExist], [DisplayTextOK], [AbbreviationOK], [DescriptionOK]) 
SELECT TOP 1 [Entity]
      ,[DisplayGroup]
      ,1, 0, 0, 0
  FROM [Entity]
WHERE Entity = @Entity


-- if nothing is found, fill in the values according to the parameters of the function

if (select count(*) from @EntityList) = 0
begin
	insert @EntityList (Entity, DisplayText, Abbreviation, DoesExist, [DisplayTextOK], [AbbreviationOK], [DescriptionOK]) 
	Values (@Entity, 
	case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 
	substring(case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 1, 20)
	, 0, 0, 0, 0)

	declare @Table nvarchar(50)
	declare @Column nvarchar(50)
	if (@Entity not like '%.%')
	begin
	set @Table = @Entity
	update E set E.[Description] = (SELECT max(CONVERT(nvarchar(MAX), [value]))
	FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @Table, default, NULL) WHERE name =  'MS_Description')
	, E.[DescriptionOK] = 1
	from @EntityList E 
	end
	
	if (@Entity like '%.%' and @Entity not like '%.%.%')
	begin
	set @Table = (select rtrim(substring(@Entity, 1, charindex('.', @Entity)-1)))
	set @Column = (select rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)))
	update E set E.[Description] = (SELECT max(CONVERT(nvarchar(MAX), [value])) 
	FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @Table, 'column', @Column) WHERE name =  'MS_Description')
	, E.[DescriptionOK] = 1
	from @EntityList E 
	end
	
	if (select count(*) from @EntityList where len([Description]) > 0) = 0
	begin
	update E set E.[Description] = case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) 
	+ case when @Entity like '%.%' then ' - ' + rtrim(substring(@Entity, 1, charindex('.', @Entity)-1)) else '' end
	else @Entity end
	from @EntityList E 
	end
	
	if (@Entity like '%_Enum.Code.%')
	begin
	declare @EnumTable nvarchar(50)
	set @EnumTable = (select substring(@Entity, 1, charindex('.', @Entity) - 1))
	declare @Value nvarchar(50)
	set @Value = (select rtrim(substring(@Entity, charindex('.', @Entity) + 6, 255)))
	update E set E.[Description] = @Value, E.DisplayText = substring(@Value, 1, 50), E.Abbreviation = substring(@Value, 1, 20)
	from @EntityList E 
	end
	return
end


-- set the details for usage and representation of the entry

declare @ParentContext nvarchar(50)
declare @CurrentContext nvarchar(50)


-- set the Accessibility, Determination, Visibility and PresetValue if there is one --################################################################
update E set
E.Accessibility = U.Accessibility,
E.Determination = U.Determination, 
E.Visibility = U.Visibility, 
E.PresetValue = U.PresetValue, 
E.UsageNotes = U.Notes
from dbo.EntityUsage U, @EntityList E
where U.Entity = @Entity
and U.EntityContext = @Context

-- search for usage information in parent datasets
set @CurrentContext = @Context
set @ParentContext = @Context
while not @ParentContext is null
begin
	update E set 
	E.Accessibility = case when E.Accessibility is null then U.Accessibility else E.Accessibility end,
	E.Determination = case when E.Determination is null then U.Determination else E.Determination end,
	E.Visibility = case when E.Visibility is null then U.Visibility else E.Visibility end,
	E.PresetValue = case when E.PresetValue is null then U.PresetValue else E.PresetValue end,
	E.UsageNotes = case when E.UsageNotes is null then U.Notes else E.UsageNotes end
	from dbo.EntityUsage U, @EntityList E
	where U.Entity = @Entity
	and U.EntityContext = @ParentContext
	
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end


-- set the representation values --################################################################
update E set E.[DisplayText] = R.DisplayText, 
E.Abbreviation = R.Abbreviation, 
E.[Description] = R.[Description],
E.[DisplayTextOK] = case when R.DisplayText is null or R.DisplayText = '' then 0 else 1 end, 
E.[AbbreviationOK] = case when R.Abbreviation is null or R.Abbreviation = '' then 0 else 1 end, 
E.[DescriptionOK] = case when R.[Description] is null or R.[Description] = '' then 0 else 1 end
from dbo.EntityRepresentation R, @EntityList E
where R.Entity = @Entity
and R.EntityContext = @Context
and R.LanguageCode = @Language

-- search for representation values in parent datasets in the same language if nothing is found
set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @Context)
while not @ParentContext is null
begin
	update E set 
	E.[DisplayText] = case when E.DisplayText is null then R.DisplayText else E.DisplayText end, 
	E.Abbreviation = case when E.Abbreviation is null then R.Abbreviation else E.Abbreviation end, 
	E.[Description] = case when E.[Description] is null then R.[Description] else E.[Description] end,
	E.[DisplayTextOK] = case when E.[DisplayTextOK] = 0 and R.[DisplayText] <> '' then 1 else E.[DisplayTextOK] end, 
	E.[AbbreviationOK] = case when E.[AbbreviationOK] = 0 and R.[Abbreviation] <> '' then 1 else E.[AbbreviationOK] end, 
	E.[DescriptionOK] = case when E.[DescriptionOK] = 0 and R.[Description] <> '' then 1 else E.[DescriptionOK] end
	from dbo.EntityRepresentation R, @EntityList E
	where R.Entity = @Entity
	and R.EntityContext = @ParentContext
	and R.LanguageCode = @Language
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end


-- search for representation values in parent datasets in the default language if nothing is found
set @ParentContext = @Context
while not @ParentContext is null
begin
	update E set 
	E.[DisplayText] = case when E.DisplayText is null then R.DisplayText else E.DisplayText end, 
	E.Abbreviation = case when E.Abbreviation is null then R.Abbreviation else E.Abbreviation end, 
	E.[Description] = case when E.[Description] is null then R.[Description] else E.[Description] end,
	E.[DisplayTextOK] = case when E.[DisplayTextOK] = 0 and R.[DisplayText] <> '' then 1 else E.[DisplayTextOK] end, 
	E.[AbbreviationOK] = case when E.[AbbreviationOK] = 0 and R.[Abbreviation] <> '' then 1 else E.[AbbreviationOK] end, 
	E.[DescriptionOK] = case when E.[DescriptionOK] = 0 and R.[Description] <> '' then 1 else E.[DescriptionOK] end
	from dbo.EntityRepresentation R, @EntityList E
	where R.Entity = @Entity
	and R.EntityContext = @ParentContext
	and R.LanguageCode = 'en-US'
	
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end

update E set E.[DisplayText] = substring(case when @Entity like '%.%' then rtrim(reverse(substring(reverse(@Entity), 1, charindex('.', reverse(@Entity))-1))) else @Entity end, 1, 20)
from @EntityList E 
where E.DisplayText is null

update E set E.Abbreviation = substring(case when @Entity like '%.%' then rtrim(reverse(substring(reverse(@Entity), 1, charindex('.', reverse(@Entity))-1))) else @Entity end, 1, 20)
from @EntityList E 
where E.Abbreviation is null


   RETURN
END


GO



--#####################################################################################################################
--######  CollTypeStatus_Enum: insert new type conserved type  ########################################################
--#####################################################################################################################

if (select count(*) from CollTypeStatus_Enum where Code = 'conserved type') = 0
begin
	INSERT INTO [dbo].[CollTypeStatus_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('conserved type'
           ,'The one specimen or other element conserved as type for a name of a subdivision of a genus or of an infraspecific taxonand listed in ICN, App. III and IV (typ. cons.)'
           ,'conserved type'
           ,40
           ,1)
end
GO


--#####################################################################################################################
--######  ManagerCollectionList: CollectionName expanded to 255  ######################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[ManagerCollectionList] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the collections a Manager has access to, including the child collections.
MW 02.10.2018

Test:
select * from dbo.ManagerCollectionList()
*/
AS
BEGIN
	-- Filling the AdmininstratingCollections in the temp list
	DECLARE @CollectionID INT
	DECLARE @TempAdminCollectionID TABLE (CollectionID int primary key)
	DECLARE @TempCollectionID TABLE (CollectionID int primary key)
	INSERT @TempAdminCollectionID (CollectionID) 
		SELECT AdministratingCollectionID FROM CollectionManager WHERE (LoginName = USER_NAME()) 
	INSERT @TempCollectionID (CollectionID) 
		SELECT AdministratingCollectionID FROM CollectionManager WHERE (LoginName = USER_NAME()) 

	DECLARE HierarchyCursor  CURSOR for
	select CollectionID from @TempAdminCollectionID
	open HierarchyCursor
	FETCH next from HierarchyCursor into @CollectionID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		insert into @TempCollectionID select CollectionID 
		from dbo.CollectionChildNodes (@CollectionID) where @CollectionID not in (select CollectionID from @TempCollectionID)
		FETCH NEXT FROM HierarchyCursor into @CollectionID
	END
	CLOSE HierarchyCursor
	DEALLOCATE HierarchyCursor

	-- copy the child nodes into the result list
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder FROM dbo.Collection
	WHERE CollectionID in (SELECT CollectionID FROM @TempCollectionID)
	

	RETURN
END
GO


--#####################################################################################################################
--######  procCopyCollectionSpecimen2: Copy hierarchy for taxa  #######################################################
--#####################################################################################################################


ALTER  PROCEDURE [dbo].[procCopyCollectionSpecimen2] 
	(@CollectionSpecimenID int output ,
	@OriginalCollectionSpecimenID int ,
	@AccessionNumber  nvarchar(50),
	@EventCopyMode int,
	@IncludedTables nvarchar(4000))
AS
declare @count int
declare @EventID int

/*
Copy a collection specimen
@EventCopyMode
-1: dont copy the event, leave the entry in table CollectionSpecimen empty
0:  take same event as original specimen
1:  create new event with the same data as the old specimen

@IncludedTables contains list of tables that are copied according to the users choice
*/

if (@EventCopyMode = 0) set @EventID = (SELECT CollectionEventID FROM CollectionSpecimen WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID)
if (@EventCopyMode < 0) set @EventID = null

DECLARE @CollectionEventID int
DECLARE @OriginalCollectionEventID int

if (@EventCopyMode > 0) 
begin
	DECLARE @RC int
	set @OriginalCollectionEventID = (SELECT CollectionEventID FROM CollectionSpecimen WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID)

	-- CollectionEvent
	INSERT INTO CollectionEvent
		  (SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, 
		  CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, 
		  Notes, CountryCache, DataWithholdingReason)
	SELECT SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, 
		  CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, 
		  Notes, CountryCache, DataWithholdingReason
	FROM  CollectionEvent
	WHERE CollectionEventID = @OriginalCollectionEventID

	SET @CollectionEventID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

	-- CollectionEventLocalisation
	INSERT INTO CollectionEventLocalisation
		(CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, 
		DirectionToLocation, ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, [Geography])
	SELECT @CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, 
		DirectionToLocation, ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, [Geography]
	FROM CollectionEventLocalisation
	WHERE (CollectionEventID = @OriginalCollectionEventID)

	-- CollectionEventProperty
	INSERT INTO CollectionEventProperty
		(CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue,  
		ResponsibleName, ResponsibleAgentURI, Notes,AverageValueCache)
	SELECT @CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue,  
		ResponsibleName, ResponsibleAgentURI, Notes,AverageValueCache
	FROM CollectionEventProperty
	WHERE (CollectionEventID = @OriginalCollectionEventID)

	-- CollectionEventImage
	IF (@IncludedTables LIKE '%|CollectionEventImage|%')
	BEGIN
		INSERT INTO CollectionEventImage
			(CollectionEventID, URI, ResourceURI, ImageType, Notes)
		SELECT @CollectionEventID, URI, ResourceURI, ImageType, Notes
		FROM CollectionEventImage
		WHERE (CollectionEventID = @OriginalCollectionEventID)
	END
	set @EventID = @CollectionEventID
end

-- CollectionSpecimen
INSERT INTO CollectionSpecimen (CollectionEventID, CollectionID, AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, 
	AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, 
	LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, InternalNotes, ReferenceTitle, ReferenceURI, 
	Problems, DataWithholdingReason)
SELECT @EventID, CollectionID, @AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, 
	AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, 
	LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, InternalNotes, ReferenceTitle, ReferenceURI, 
	Problems, DataWithholdingReason
FROM CollectionSpecimen
WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID

SET @CollectionSpecimenID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

-- CollectionProject
INSERT INTO CollectionProject (CollectionSpecimenID, ProjectID)
SELECT @CollectionSpecimenID, ProjectID
FROM CollectionProject
WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID

-- CollectionAgent
IF (@IncludedTables LIKE '%|CollectionAgent|%')
BEGIN
	INSERT INTO CollectionAgent ( CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason)
	SELECT   @CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason
	FROM CollectionAgent
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
END

-- IdentificationUnit
DECLARE @TempUnitTable TABLE (IdentificationUnitID int primary key,
	OriginalIdentificationUnitID int NULL)
DECLARE @IdentificationUnitID int
DECLARE @OriginalIdentificationUnitID int

DECLARE UnitCursor CURSOR FOR
SELECT IdentificationUnitID FROM IdentificationUnit WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
OPEN UnitCursor
FETCH NEXT FROM UnitCursor INTO @OriginalIdentificationUnitID
WHILE @@FETCH_STATUS = 0
BEGIN
	-- IdentificationUnit
	INSERT INTO IdentificationUnit (CollectionSpecimenID, LastIdentificationCache, FamilyCache, OrderCache, HierarchyCache, TaxonomicGroup, 
		OnlyObserved, RelationType, ColonisedSubstratePart, LifeStage, Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, 
		Circumstances, DisplayOrder, Notes)
	SELECT @CollectionSpecimenID, LastIdentificationCache, FamilyCache, OrderCache, HierarchyCache, TaxonomicGroup, 
		OnlyObserved, RelationType, ColonisedSubstratePart, LifeStage, Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, 
		Circumstances, DisplayOrder, Notes
	FROM IdentificationUnit
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND IdentificationUnitID = @OriginalIdentificationUnitID

	SET @IdentificationUnitID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])
	INSERT INTO @TempUnitTable (IdentificationUnitID, OriginalIdentificationUnitID) VALUES (@IdentificationUnitID, @OriginalIdentificationUnitID)

	-- Identification
	IF (@IncludedTables LIKE '%|Identification|%')
	BEGIN
		INSERT INTO Identification (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermURI, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, Notes, ResponsibleName, ResponsibleAgentURI)
		SELECT @CollectionSpecimenID, @IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, Notes, ResponsibleName, ResponsibleAgentURI
		FROM Identification
		WHERE Identification.CollectionSpecimenID = @OriginalCollectionSpecimenID
		and Identification.IdentificationUnitID = @OriginalIdentificationUnitID
	END

   	FETCH NEXT FROM UnitCursor INTO @OriginalIdentificationUnitID
END
CLOSE UnitCursor
DEALLOCATE UnitCursor

-- Fixing the relations between the units
DECLARE @I INT
SET @I = (SELECT COUNT(*) FROM @TempUnitTable)
IF @I > 0
BEGIN
	UPDATE N SET N.RelatedUnitID = NH.IdentificationUnitID
	FROM IdentificationUnit N, IdentificationUnit NH, @TempUnitTable T, @TempUnitTable TH, IdentificationUnit O--, IdentificationUnit OH
	WHERE N.IdentificationUnitID = T.IdentificationUnitID
	AND O.IdentificationUnitID = T.OriginalIdentificationUnitID
	AND O.RelatedUnitID = TH.OriginalIdentificationUnitID
	AND TH.IdentificationUnitID = NH.IdentificationUnitID

	UPDATE N SET N.ParentUnitID = NH.IdentificationUnitID
	FROM IdentificationUnit N, IdentificationUnit NH, @TempUnitTable T, @TempUnitTable TH, IdentificationUnit O--, IdentificationUnit OH
	WHERE N.IdentificationUnitID = T.IdentificationUnitID
	AND O.IdentificationUnitID = T.OriginalIdentificationUnitID
	AND O.ParentUnitID = TH.OriginalIdentificationUnitID
	AND TH.IdentificationUnitID = NH.IdentificationUnitID
END

-- CollectionSpecimenPart
DECLARE @TempPartTable TABLE (SpecimenPartID int primary key,
	OriginalSpecimenPartID int NULL)
DECLARE @SpecimenPartID int
DECLARE @OriginalSpecimenPartID int

DECLARE @FetchStatusPartCursor int
DECLARE @FetchStatusProcessingCursor int
DECLARE PartCursor CURSOR FOR
SELECT SpecimenPartID FROM CollectionSpecimenPart WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
OPEN PartCursor
FETCH NEXT FROM PartCursor INTO @OriginalSpecimenPartID
SET @FetchStatusPartCursor = @@FETCH_STATUS
WHILE @FetchStatusPartCursor = 0
BEGIN
	-- CollectionSpecimenPart
	INSERT INTO CollectionSpecimenPart ( CollectionSpecimenID, PreparationMethod, PreparationDate, 
		AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, Stock, Notes)
	SELECT @CollectionSpecimenID, PreparationMethod, PreparationDate, 
		AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, Stock, Notes
	FROM CollectionSpecimenPart
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND SpecimenPartID = @OriginalSpecimenPartID

	SET @SpecimenPartID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])
	INSERT INTO @TempPartTable (SpecimenPartID, OriginalSpecimenPartID) VALUES (@SpecimenPartID, @OriginalSpecimenPartID)



	--CollectionSpecimenProcessing
	--CollectionSpecimenProcessingMethod
	--CollectionSpecimenProcessingMethodParameter
	IF (@IncludedTables LIKE '%|CollectionSpecimenProcessing|%')
	BEGIN
		DECLARE @OriginalSpecimenProcessingID int
		DECLARE @SpecimenProcessingID int
		DECLARE ProcessingCursor CURSOR FOR
		SELECT SpecimenProcessingID FROM CollectionSpecimenProcessing WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID AND SpecimenPartID = @OriginalSpecimenPartID
		OPEN ProcessingCursor
		FETCH NEXT FROM ProcessingCursor INTO @OriginalSpecimenProcessingID
		SET @FetchStatusProcessingCursor = @@FETCH_STATUS
		WHILE @FetchStatusProcessingCursor = 0
		BEGIN
			-- CollectionSpecimenProcessing
			INSERT INTO CollectionSpecimenProcessing
				(CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration,  
				ResponsibleName, ResponsibleAgentURI, Notes)
			SELECT @CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, @SpecimenPartID, ProcessingDuration,  
				ResponsibleName, ResponsibleAgentURI, Notes
			FROM CollectionSpecimenProcessing P
			WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
			AND P.SpecimenPartID = @OriginalSpecimenPartID
			AND P.SpecimenProcessingID = @OriginalSpecimenProcessingID

			SET @SpecimenProcessingID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

			IF (@IncludedTables LIKE '%|CollectionSpecimenProcessingMethod|%')
			BEGIN
				-- CollectionSpecimenProcessingMethod
				INSERT INTO CollectionSpecimenProcessingMethod
					(CollectionSpecimenID, SpecimenProcessingID, MethodID, MethodMarker, ProcessingID)
				SELECT @CollectionSpecimenID, @SpecimenProcessingID, M.MethodID, M.MethodMarker, M.ProcessingID
				FROM CollectionSpecimenProcessingMethod M
				WHERE M.CollectionSpecimenID = @OriginalCollectionSpecimenID
				AND M.SpecimenProcessingID = @OriginalSpecimenProcessingID

				-- CollectionSpecimenProcessingMethodParameter
				INSERT INTO CollectionSpecimenProcessingMethodParameter
					(CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, MethodMarker, ParameterID, Value)
				SELECT @CollectionSpecimenID, @SpecimenProcessingID, M.ProcessingID, M.MethodID, M.MethodMarker, M.ParameterID, M.Value
				FROM CollectionSpecimenProcessingMethodParameter M
				WHERE M.CollectionSpecimenID = @OriginalCollectionSpecimenID
				AND M.SpecimenProcessingID = @OriginalSpecimenProcessingID
			END
			FETCH NEXT FROM ProcessingCursor INTO @OriginalSpecimenProcessingID
			SET @FetchStatusProcessingCursor = @@FETCH_STATUS
		END
		CLOSE ProcessingCursor
		DEALLOCATE ProcessingCursor
	END




	FETCH NEXT FROM PartCursor INTO @OriginalSpecimenPartID
	SET @FetchStatusPartCursor = @@FETCH_STATUS
END
CLOSE PartCursor
DEALLOCATE PartCursor

-- Fixing the relations between the parts
DECLARE @P INT
SET @P = (SELECT COUNT(*) FROM @TempUnitTable)
IF @P > 0
BEGIN
	UPDATE N SET N.DerivedFromSpecimenPartID = NH.SpecimenPartID
	FROM CollectionSpecimenPart N, CollectionSpecimenPart NH, @TempPartTable T, @TempPartTable TH, CollectionSpecimenPart O--, IdentificationUnit OH
	WHERE N.SpecimenPartID = T.SpecimenPartID
	AND O.SpecimenPartID = T.OriginalSpecimenPartID
	AND O.DerivedFromSpecimenPartID = TH.OriginalSpecimenPartID
	AND TH.SpecimenPartID = NH.SpecimenPartID
END

--IdentificationUnitInPart
--IF (@IncludedTables LIKE '%|IdentificationUnit|%' AND @IncludedTables LIKE '%|CollectionSpecimenPart|%')
BEGIN
	INSERT INTO IdentificationUnitInPart (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, DisplayOrder)
	SELECT @CollectionSpecimenID, U.IdentificationUnitID, P.SpecimenPartID, I.DisplayOrder
	FROM IdentificationUnitInPart I, @TempPartTable P, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID
END


-- CollectionRelation
IF (@IncludedTables LIKE '%|CollectionSpecimenRelation|%')
BEGIN
	INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache
	FROM CollectionSpecimenRelation
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND [IdentificationUnitID] IS NULL
	AND [SpecimenPartID] IS NULL

	INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, U.IdentificationUnitID
	FROM CollectionSpecimenRelation R, @TempUnitTable U
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND [SpecimenPartID] IS NULL

	INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, SpecimenPartID)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, P.SpecimenPartID
	FROM CollectionSpecimenRelation R, @TempPartTable P
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND [IdentificationUnitID] IS NULL
	AND R.[SpecimenPartID] = P.OriginalSpecimenPartID

	INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID, SpecimenPartID)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, U.IdentificationUnitID, P.SpecimenPartID
	FROM CollectionSpecimenRelation R, @TempPartTable P, @TempUnitTable U
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND R.[SpecimenPartID] = P.OriginalSpecimenPartID

END


--IdentificationUnitAnalysis
--IdentificationUnitAnalysisMethod
--IdentificationUnitAnalysisMethodParameter
IF (@IncludedTables LIKE '%|IdentificationUnitAnalysis|%')
BEGIN
	-- for parts
	INSERT INTO [dbo].[IdentificationUnitAnalysis]
           ([CollectionSpecimenID]
           ,[IdentificationUnitID]
           ,[AnalysisID]
           ,[AnalysisNumber]
           ,[AnalysisResult]
           ,[ExternalAnalysisURI]
           ,[ResponsibleName]
           ,[ResponsibleAgentURI]
           ,[AnalysisDate]
           ,[SpecimenPartID]
           ,[Notes])
	SELECT DISTINCT @CollectionSpecimenID, U.IdentificationUnitID
	, I.AnalysisID, I.AnalysisNumber, I.AnalysisResult, I.ExternalAnalysisURI, I.ResponsibleName, I.ResponsibleAgentURI, I.AnalysisDate
	, P.SpecimenPartID, I.Notes
	FROM IdentificationUnitAnalysis I, @TempPartTable P, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	-- for non parts
	INSERT INTO [dbo].[IdentificationUnitAnalysis]
           ([CollectionSpecimenID]
           ,[IdentificationUnitID]
           ,[AnalysisID]
           ,[AnalysisNumber]
           ,[AnalysisResult]
           ,[ExternalAnalysisURI]
           ,[ResponsibleName]
           ,[ResponsibleAgentURI]
           ,[AnalysisDate]
           ,[SpecimenPartID]
           ,[Notes])
	SELECT DISTINCT @CollectionSpecimenID, U.IdentificationUnitID
	, I.AnalysisID, I.AnalysisNumber, I.AnalysisResult, I.ExternalAnalysisURI, I.ResponsibleName, I.ResponsibleAgentURI, I.AnalysisDate
	, I.SpecimenPartID, I.Notes
	FROM IdentificationUnitAnalysis I, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID IS NULL
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	IF (@IncludedTables LIKE '%|IdentificationUnitAnalysisMethod|%')
	BEGIN
		INSERT INTO [dbo].[IdentificationUnitAnalysisMethod]
			   ([CollectionSpecimenID]
			   ,[IdentificationUnitID]
			   ,[MethodID]
			   ,[AnalysisID]
			   ,[AnalysisNumber]
			   ,[MethodMarker])
		SELECT @CollectionSpecimenID, U.IdentificationUnitID, I.MethodID
		, I.AnalysisID, I.AnalysisNumber, I.MethodMarker
		FROM IdentificationUnitAnalysisMethod I, @TempUnitTable U
		WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
		AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

		INSERT INTO [dbo].[IdentificationUnitAnalysisMethodParameter]
			   ([CollectionSpecimenID]
			   ,[IdentificationUnitID]
			   ,[AnalysisID]
			   ,[AnalysisNumber]
			   ,[MethodID]
			   ,[ParameterID]
			   ,[Value]
			   ,[MethodMarker])
		SELECT @CollectionSpecimenID, U.IdentificationUnitID, I.AnalysisID, I.AnalysisNumber, I.MethodID
		, I.ParameterID, I.Value, I.MethodMarker
		FROM IdentificationUnitAnalysisMethodParameter I, @TempUnitTable U
		WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
		AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID
	END
END

--IdentificationGoeUnitAnalysis
IF (@IncludedTables LIKE '%|IdentificationUnitGeoAnalysis|%')
BEGIN
	INSERT INTO [dbo].[IdentificationUnitGeoAnalysis]
			   ([CollectionSpecimenID]
			   ,[IdentificationUnitID]
			   ,[AnalysisDate]
			   ,[Geography]
			   ,[Geometry]
			   ,[ResponsibleName]
			   ,[ResponsibleAgentURI]
			   ,[Notes])
	SELECT @CollectionSpecimenID
		  ,[IdentificationUnitID]
		  ,[AnalysisDate]
		  ,[Geography]
		  ,[Geometry]
		  ,[ResponsibleName]
		  ,[ResponsibleAgentURI]
		  ,[Notes]
	  FROM [dbo].[IdentificationUnitGeoAnalysis] I
	WHERE I.CollectionSpecimenID = @OriginalCollectionSpecimenID
END

--CollectionSpecimenImage
IF (@IncludedTables LIKE '%|CollectionSpecimenImage|%')
BEGIN
	INSERT INTO CollectionSpecimenImage
		(CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI)
	SELECT @CollectionSpecimenID, URI, ResourceURI, P.SpecimenPartID, U.IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI
	FROM CollectionSpecimenImage I, @TempPartTable P, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenImage
		(CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI)
	SELECT @CollectionSpecimenID, URI, ResourceURI, P.SpecimenPartID, I.IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI
	FROM CollectionSpecimenImage I, @TempPartTable P
	WHERE I.IdentificationUnitID IS NULL
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenImage
		(CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI)
	SELECT @CollectionSpecimenID, URI, ResourceURI, I.SpecimenPartID, U.IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI
	FROM CollectionSpecimenImage I, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID IS NULL
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenImage
		(CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI)
	SELECT @CollectionSpecimenID, URI, ResourceURI, I.SpecimenPartID, I.IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI
	FROM CollectionSpecimenImage I
	WHERE I.IdentificationUnitID IS NULL
	AND I.SpecimenPartID IS NULL
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

END


--CollectionSpecimenImageProperty
IF (@IncludedTables LIKE '%|CollectionSpecimenImageProperty|%')
BEGIN
	INSERT INTO CollectionSpecimenImageProperty
		   (CollectionSpecimenID, URI, Property, Description, ImageArea)
	SELECT @CollectionSpecimenID, URI, Property, Description, ImageArea
	FROM CollectionSpecimenImageProperty I
	WHERE I.CollectionSpecimenID = @OriginalCollectionSpecimenID
END


--CollectionSpecimenReference
IF (@IncludedTables LIKE '%|CollectionSpecimenReference|%')
BEGIN
	INSERT INTO CollectionSpecimenReference
		(CollectionSpecimenID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, U.IdentificationUnitID, P.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R, @TempPartTable P, @TempUnitTable U
	WHERE R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND R.SpecimenPartID = P.OriginalSpecimenPartID
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		(CollectionSpecimenID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, R.IdentificationUnitID, P.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R, @TempPartTable P
	WHERE R.IdentificationUnitID IS NULL
	AND R.SpecimenPartID = P.OriginalSpecimenPartID
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		(CollectionSpecimenID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, U.IdentificationUnitID, R.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R, @TempUnitTable U
	WHERE R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND R.SpecimenPartID IS NULL
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		(CollectionSpecimenID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, R.IdentificationUnitID, R.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R
	WHERE R.IdentificationUnitID IS NULL
	AND R.SpecimenPartID IS NULL
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

END

--CollectionSpecimenTransaction
IF (@IncludedTables LIKE '%|CollectionSpecimenTransaction|%')
BEGIN
	INSERT INTO CollectionSpecimenTransaction
		(CollectionSpecimenID, TransactionID, SpecimenPartID, AccessionNumber, TransactionReturnID)
	SELECT @CollectionSpecimenID, TransactionID, P.SpecimenPartID, AccessionNumber, TransactionReturnID
	FROM CollectionSpecimenTransaction T, @TempPartTable P
	WHERE T.SpecimenPartID = P.OriginalSpecimenPartID
	AND T.CollectionSpecimenID = @OriginalCollectionSpecimenID
END

--Annotation
IF (@IncludedTables LIKE '%|Annotation|%')
BEGIN
	INSERT INTO [dbo].[Annotation]
           ([ReferencedAnnotationID]
           ,[AnnotationType]
           ,[Title]
           ,[Annotation]
           ,[URI]
           ,[ReferenceDisplayText]
           ,[ReferenceURI]
           ,[SourceDisplayText]
           ,[SourceURI]
           ,[IsInternal]
           ,[ReferencedID]
           ,[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
	, A.SourceDisplayText, A.SourceURI, A.IsInternal, @CollectionSpecimenID, A.ReferencedTable
	FROM Annotation A
	WHERE A.ReferencedTable = 'CollectionSpecimen' AND A.ReferencedID = @OriginalCollectionSpecimenID

	INSERT INTO [dbo].[Annotation]
           ([ReferencedAnnotationID]
           ,[AnnotationType]
           ,[Title]
           ,[Annotation]
           ,[URI]
           ,[ReferenceDisplayText]
           ,[ReferenceURI]
           ,[SourceDisplayText]
           ,[SourceURI]
           ,[IsInternal]
           ,[ReferencedID]
           ,[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
	, A.SourceDisplayText, A.SourceURI, A.IsInternal, U.IdentificationUnitID, A.ReferencedTable
	FROM Annotation A, @TempUnitTable U
	WHERE A.ReferencedTable = 'IdentificationUnit' AND A.ReferencedID = U.OriginalIdentificationUnitID

	INSERT INTO [dbo].[Annotation]
           ([ReferencedAnnotationID]
           ,[AnnotationType]
           ,[Title]
           ,[Annotation]
           ,[URI]
           ,[ReferenceDisplayText]
           ,[ReferenceURI]
           ,[SourceDisplayText]
           ,[SourceURI]
           ,[IsInternal]
           ,[ReferencedID]
           ,[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
	, A.SourceDisplayText, A.SourceURI, A.IsInternal, P.SpecimenPartID, A.ReferencedTable
	FROM Annotation A, @TempPartTable P
	WHERE A.ReferencedTable = 'CollectionSpecimenPart' AND A.ReferencedID = P.OriginalSpecimenPartID

	if @EventCopyMode = 1 AND NOT @CollectionEventID IS NULL
	BEGIN
		INSERT INTO [dbo].[Annotation]
			   ([ReferencedAnnotationID]
			   ,[AnnotationType]
			   ,[Title]
			   ,[Annotation]
			   ,[URI]
			   ,[ReferenceDisplayText]
			   ,[ReferenceURI]
			   ,[SourceDisplayText]
			   ,[SourceURI]
			   ,[IsInternal]
			   ,[ReferencedID]
			   ,[ReferencedTable])
		SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
		, A.SourceDisplayText, A.SourceURI, A.IsInternal, @CollectionEventID, A.ReferencedTable
		FROM Annotation A
		WHERE A.ReferencedTable = 'CollectionEvent' AND A.ReferencedID = @OriginalCollectionEventID
	END

END

--ExternalIdentifier
IF (@IncludedTables LIKE '%|ExternalIdentifier|%')
BEGIN
	INSERT INTO [dbo].[ExternalIdentifier]
           ([ReferencedTable]
           ,[ReferencedID]
           ,[Type]
           ,[Identifier]
           ,[URL]
           ,[Notes])
	SELECT T.ReferencedTable, @CollectionSpecimenID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T
	WHERE T.ReferencedTable = 'CollectionSpecimen' AND T.ReferencedID = @OriginalCollectionSpecimenID

	INSERT INTO [dbo].[ExternalIdentifier]
           ([ReferencedTable]
           ,[ReferencedID]
           ,[Type]
           ,[Identifier]
           ,[URL]
           ,[Notes])
	SELECT T.ReferencedTable, U.IdentificationUnitID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T, @TempUnitTable U
	WHERE T.ReferencedTable = 'IdentificationUnit' AND T.ReferencedID = U.OriginalIdentificationUnitID

	INSERT INTO [dbo].[ExternalIdentifier]
           ([ReferencedTable]
           ,[ReferencedID]
           ,[Type]
           ,[Identifier]
           ,[URL]
           ,[Notes])
	SELECT T.ReferencedTable, P.SpecimenPartID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T, @TempPartTable P
	WHERE T.ReferencedTable = 'CollectionSpecimenPart' AND T.ReferencedID = P.OriginalSpecimenPartID

	if @EventCopyMode = 1 AND NOT @CollectionEventID IS NULL
	BEGIN
		INSERT INTO [dbo].[ExternalIdentifier]
			   ([ReferencedTable]
			   ,[ReferencedID]
			   ,[Type]
			   ,[Identifier]
			   ,[URL]
			   ,[Notes])
		SELECT T.ReferencedTable, @CollectionEventID, T.Type, T.Identifier, T.URL, T.Notes
		FROM ExternalIdentifier T
		WHERE T.ReferencedTable = 'CollectionEvent' AND T.ReferencedID = @OriginalCollectionEventID
	END

END

SELECT @CollectionSpecimenID



GO




--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.09.00' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.16'
END

GO


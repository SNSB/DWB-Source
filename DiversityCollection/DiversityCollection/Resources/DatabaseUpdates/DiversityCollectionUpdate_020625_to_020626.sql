declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.25'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Task  #######################################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[Task] ADD [MetricUnit] nvarchar(50) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The unit of the metric, e.g. °C for temperature' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'MetricUnit'
GO

ALTER TABLE [dbo].[Task_log] ADD [MetricUnit] nvarchar(50) NULL;
GO


--#####################################################################################################################
--######  TaskChildNodes  #############################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[TaskChildNodes] (@ID int)  
RETURNS @ItemList TABLE (TaskID int primary key,
	TaskParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	Notes nvarchar  (1000)   NULL ,
	TaskURI varchar  (255)   NULL ,
	Type varchar  (50) NULL,
	ModuleTitle varchar  (50) NULL,
	ModuleType varchar  (50) NULL,
	[SpecimenPartType] [nvarchar](50) NULL,
	[TransactionType] [nvarchar](50) NULL,
	ResultType varchar  (50) NULL,
	DateType varchar  (50) NULL,
	[DateBeginType] [nvarchar](50) NULL,
	[DateEndType] [nvarchar](50) NULL,
	NumberType varchar  (50) NULL,
	BoolType varchar  (50) NULL,
	[MetricType] [nvarchar](50) NULL,
	[MetricUnit] [nvarchar](50) NULL,
	[ResponsibleType] [nvarchar](50) NULL,
	DescriptionType varchar  (50) NULL,
	NotesType varchar  (50) NULL,
	UriType varchar  (50) NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 15.07.2021
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (TaskID int primary key,
	TaskParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	Notes nvarchar  (1000)   NULL ,
	TaskURI varchar  (255)   NULL,
	Type varchar  (50) NULL,
	ModuleTitle varchar  (50) NULL,
	ModuleType varchar  (50) NULL,
	[SpecimenPartType] [nvarchar](50) NULL,
	[TransactionType] [nvarchar](50) NULL,
	ResultType varchar  (50) NULL,
	DateType varchar  (50) NULL,
	[DateBeginType] [nvarchar](50) NULL,
	[DateEndType] [nvarchar](50) NULL,
	NumberType varchar  (50) NULL,
	BoolType varchar  (50) NULL,
	[MetricType] [nvarchar](50) NULL,
	[MetricUnit] [nvarchar](50) NULL,
	[ResponsibleType] [nvarchar](50) NULL,
	DescriptionType varchar  (50) NULL,
	NotesType varchar  (50) NULL,
	UriType varchar  (50) NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)


 INSERT @TempItem (
		   TaskID , TaskParentID, DisplayText, Description,  Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID) 
	SELECT TaskID , TaskParentID, DisplayText , Description, Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID
	FROM Task WHERE TaskParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, Task C where C.TaskParentID = T.TaskID and C.TaskID not in (select TaskID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (TaskID , TaskParentID, DisplayText, Description, Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID)
		select C.TaskID, C.TaskParentID, C.DisplayText, C.Description, C.Notes , C.TaskURI, C.Type, C.ModuleTitle, C.ModuleType, C.SpecimenPartType, C.TransactionType, C.ResultType, C.DateType, C.DateBeginType, C.DateEndType, C.NumberType, C.BoolType, C.MetricType, C.MetricUnit, C.ResponsibleType, C.DescriptionType, C.NotesType, C.UriType, C.RowGUID
		from @TempItem T, Task C where C.TaskParentID = T.TaskID and C.TaskID not in (select TaskID from @TempItem)
		set @i = (select count(*) from @TempItem T, Task C where C.TaskParentID = T.TaskID and C.TaskID not in (select TaskID from @TempItem))
	end

 INSERT @ItemList (TaskID , TaskParentID, DisplayText, Description, Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID) 
   SELECT distinct TaskID , TaskParentID, DisplayText, Description, Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID
   FROM @TempItem ORDER BY DisplayText
   RETURN


   RETURN
END
GO

--#####################################################################################################################
--######  TaskHierarchy  ##############################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[TaskHierarchy] (@TaskID int)  
RETURNS @TaskList TABLE ([TaskID] [int] Primary key ,
	[TaskParentID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[TaskURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	Type varchar  (50) NULL,
	ModuleTitle varchar  (50) NULL,
	ModuleType varchar  (50) NULL,
	[SpecimenPartType] [nvarchar](50) NULL,
	[TransactionType] [nvarchar](50) NULL,
	ResultType varchar  (50) NULL,
	DateType varchar  (50) NULL,
	[DateBeginType] [nvarchar](50) NULL,
	[DateEndType] [nvarchar](50) NULL,
	NumberType varchar  (50) NULL,
	BoolType varchar  (50) NULL,
	[MetricType] [nvarchar](50) NULL,
	[MetricUnit] [nvarchar](50) NULL,
	[ResponsibleType] [nvarchar](50) NULL,
	DescriptionType varchar  (50) NULL,
	NotesType varchar  (50) NULL,
	UriType varchar  (50) NULL
)
/*
Returns a table that lists all the Task items related to the given Task.
MW 02.01.2006
Test
SELECT  *  FROM dbo.TaskHierarchy(82)
*/
AS
BEGIN
declare @TopID int
declare @i int
set @TopID = (select TaskParentID from Task where TaskID = @TaskID) 
set @i = (select count(*) from Task where TaskID = @TaskID)
if (@TopID is null )
	set @TopID =  @TaskID
else	
	begin
	while (@i > 0)
		begin
		set @TaskID = (select TaskParentID from Task where TaskID = @TaskID and not TaskParentID is null) 
		set @i = (select count(*) from Task where TaskID = @TaskID and not TaskParentID is null)
		end
	set @TopID = @TaskID
	end
   INSERT @TaskList
   SELECT DISTINCT TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle,  ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType
   FROM Task
   WHERE Task.TaskID = @TopID
   INSERT @TaskList
            SELECT TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle,  ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType
   FROM dbo.TaskChildNodes (@TopID)
   RETURN
END
GO


--#####################################################################################################################
--######  TaskHierarchyAll  ###########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[TaskHierarchyAll] ()  
RETURNS @TaskList TABLE ([TaskID] [int] Primary key ,
	[TaskParentID] [int] NULL ,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[TaskURI] [varchar](255) NULL,
	Type varchar  (50) NULL,
	ModuleTitle varchar  (50) NULL,
	ModuleType varchar  (50) NULL,
	[SpecimenPartType] [nvarchar](50) NULL,
	[TransactionType] [nvarchar](50) NULL,
	ResultType varchar  (50) NULL,
	DateType varchar  (50) NULL,
	[DateBeginType] [nvarchar](50) NULL,
	[DateEndType] [nvarchar](50) NULL,
	NumberType varchar  (50) NULL,
	BoolType varchar  (50) NULL,
	[MetricType] [nvarchar](50) NULL,
	[MetricUnit] [nvarchar](50) NULL,
	[ResponsibleType] [nvarchar](50) NULL,
	DescriptionType varchar  (50) NULL,
	NotesType varchar  (50) NULL,
	UriType varchar  (50) NULL,
	[HierarchyDisplayText] [nvarchar] (900) COLLATE Latin1_General_CI_AS NULL,
	[HierarchyDisplayTextInvers] [nvarchar] (900))
/*
Returns a table that lists all the Task items related to the given Task.
MW 02.01.2006
TEST:
SELECT * FROM DBO.TaskHierarchyAll()
*/
AS
BEGIN
declare @S nchar(3);
set @S = (select dbo.TaskHierarchySeparator());
INSERT @TaskList (TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType, HierarchyDisplayText, HierarchyDisplayTextInvers)
SELECT DISTINCT   TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, MetricUnit, ResponsibleType, DescriptionType, NotesType, UriType, DisplayText,          DisplayText
FROM Task
WHERE Task.TaskParentID IS NULL
declare @i int
set @i = (select count(*) from Task where TaskID not IN (select TaskID from  @TaskList))
while (@i > 0)
	begin
	INSERT @TaskList (TaskID,   TaskParentID,   DisplayText,   Description,    Notes,   TaskURI,   Type,   ModuleTitle,   ModuleType, SpecimenPartType,     TransactionType,   ResultType,   DateType, DateBeginType, DateEndType,   NumberType,   BoolType, MetricType, MetricUnit, ResponsibleType,   DescriptionType,   NotesType,   UriType, HierarchyDisplayText, HierarchyDisplayTextInvers)
	SELECT DISTINCT C.TaskID, C.TaskParentID, C.DisplayText, C.Description,  C.Notes, C.TaskURI, C.Type, C.ModuleTitle, C.ModuleType, C.SpecimenPartType, C.TransactionType, C.ResultType, C.DateType, C.DateBeginType, C.DateEndType, C.NumberType, C.BoolType, C.MetricType, C.MetricUnit, C.ResponsibleType, C.DescriptionType, C.NotesType, C.UriType, L.HierarchyDisplayText + @S + C.DisplayText, C.DisplayText + @S + L.HierarchyDisplayTextInvers  
	FROM Task C, @TaskList L
	WHERE C.TaskParentID = L.TaskID
	AND C.TaskID NOT IN (select TaskID from  @TaskList)
	set @i = (select count(*) from Task where TaskID not IN (select TaskID from  @TaskList))
end
   RETURN
END
GO




--#####################################################################################################################
--######  trgDelTask  #################################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelTask] ON [dbo].[Task] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Task_Log (MetricType, MetricUnit, ResponsibleType, BoolType, DateType, Description, DescriptionType, DisplayText, ModuleTitle, ModuleType, Notes, NotesType, NumberType, ResultType, SpecimenPartType, TaskID, TaskParentID, TaskURI, TransactionType, Type, UriType, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.MetricType, D.MetricUnit, D.ResponsibleType, D.BoolType, D.DateType, D.Description, D.DescriptionType, D.DisplayText, D.ModuleTitle, D.ModuleType, D.Notes, D.NotesType, D.NumberType, D.ResultType, D.SpecimenPartType, D.TaskID, D.TaskParentID, D.TaskURI, D.TransactionType, D.Type, D.UriType, D.LogCreatedBy, D.LogCreatedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D

GO

--#####################################################################################################################
--######  trgDelTask  #################################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdTask] ON [dbo].[Task] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Task_Log (MetricType, MetricUnit, ResponsibleType, BoolType, DateType, Description, DescriptionType, DisplayText, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, ModuleTitle, ModuleType, Notes, NotesType, NumberType, ResultType, RowGUID, SpecimenPartType, TaskID, TaskParentID, TaskURI, TransactionType, Type, UriType, LogUser,  LogState) 
SELECT D.MetricType, D.MetricUnit, D.ResponsibleType, D.BoolType, D.DateType, D.Description, D.DescriptionType, D.DisplayText, D.LogCreatedBy, D.LogCreatedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.ModuleTitle, D.ModuleType, D.Notes, D.NotesType, D.NumberType, D.ResultType, D.RowGUID, D.SpecimenPartType, D.TaskID, D.TaskParentID, D.TaskURI, D.TransactionType, D.Type, D.UriType, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM Task T, deleted D where 1 = 1 
AND T.TaskID = D.TaskID

GO

--#####################################################################################################################
--######  Collection types for handling locations  ####################################################################
--#####################################################################################################################


INSERT INTO [dbo].[CollCollectionType_Enum] (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('department', 'department', 'department', 1, '', 'institution');
GO

INSERT INTO [dbo].[CollCollectionType_Enum] (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('container', 'container', 'container', 1, '', NULL);
GO

if(select count(*) from [CollCollectionType_Enum] where Code = 'freezer') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum] (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('freezer', 'freezer', 'freezer', 1, '', 'container');
end
if(select count(*) from [CollCollectionType_Enum] where Code = 'fridge') = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum] (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('fridge', 'fridge', 'fridge', 1, '', 'container');
end
GO

UPDATE T SET ParentCode = 'container' FROM [CollCollectionType_Enum] T WHERE Code IN ('box', 'freezer', 'drawer', 'cupboard', 'steel locker', 'subdivided container', 'fridge')
GO

INSERT INTO [dbo].[CollCollectionType_Enum] (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('location', 'location', 'location', 1, '', NULL);
GO

UPDATE T SET ParentCode = 'location' FROM [CollCollectionType_Enum] T WHERE Code IN ('room', 'trap', 'sensor')
GO

--#####################################################################################################################
--######  Task types for treatment  ###################################################################################
--#####################################################################################################################

--INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Usage', 'Usage', 'Usage', 1, '');

--INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Wet collection', 'Wet collection', 'Wet collection', 1, 'Wet collection containing inflammable solvents', 'Usage');
--INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Dry collection', 'Dry collection', 'Dry collection', 1, 'Dry collection not containing inflammable solvents', 'Usage');
--INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Skins', 'Skins', 'Skins', 1, '', 'Usage');
--INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Bones', 'Bones', 'Bones', 1, '', 'Usage');
--INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Department', 'Department', 'Department', 1, '', 'Usage');

INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Treatment', 'Treatment', 'Treatment', 1, '');
UPDATE T SET ParentCode = 'Treatment' FROM TaskType_Enum T WHERE Code IN ('Cleaning', 'Freezing', 'Gas', 'Poison', 'Processing', 'Radiation', 'Repair')

INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Beneficial organism', 'Beneficial organism', 'Beneficial organism', 1, '', 'Treatment');
UPDATE T SET ParentCode = 'IPM' FROM TaskType_Enum T WHERE Code IN ('Pest', 'Trap')

GO

UPDATE T SET DisplayEnable = 0 FROM TaskType_Enum T WHERE Code IN ('Sensor')
GO

if(select count(*) from TaskType_Enum where Code = 'Temperature') = 0
begin
	INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, ParentCode) VALUES ('Temperature', 'Temperature', 'Temperature', 1, 'Sensor');
end
GO

if(select count(*) from TaskType_Enum where Code = 'Humidity') = 0
begin
	INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, ParentCode) VALUES ('Humidity', 'Humidity', 'Humidity', 1, 'Sensor');
end
GO


--#####################################################################################################################
--######  ManagerCollectionList: optimizing and bugfix for child nodes  ###############################################
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

	set @CollectionID = (select min(CollectionID) from @TempAdminCollectionID)
	while (select count(*) from @TempAdminCollectionID) > 0
	begin
		insert into @TempCollectionID select CollectionID from dbo.CollectionChildNodes (@CollectionID) where CollectionID not in (select CollectionID from @TempCollectionID)
		delete from @TempAdminCollectionID where CollectionID = @CollectionID
		set @CollectionID = (select min(CollectionID) from @TempAdminCollectionID)
	end


	--DECLARE HierarchyCursor  CURSOR for
	--select CollectionID from @TempAdminCollectionID
	--open HierarchyCursor
	--FETCH next from HierarchyCursor into @CollectionID
	--WHILE @@FETCH_STATUS = 0
	--BEGIN
	--	insert into @TempCollectionID select CollectionID 
	--	from dbo.CollectionChildNodes (@CollectionID) where CollectionID not in (select CollectionID from @TempCollectionID)
	--	FETCH NEXT FROM HierarchyCursor into @CollectionID
	--END
	--CLOSE HierarchyCursor
	--DEALLOCATE HierarchyCursor

	-- copy the child nodes into the result list
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder FROM dbo.Collection
	WHERE CollectionID in (SELECT CollectionID FROM @TempCollectionID)
	

	RETURN
END
GO


--#####################################################################################################################
--######  trgInsCollectionTask - use CollectionID if provided  ########################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgInsCollectionTask] ON [dbo].[CollectionTask] 
INSTEAD OF INSERT AS

/*  Date: 20.07.2021  */ 
INSERT INTO [dbo].[CollectionTask]
           ([CollectionTaskParentID]
		  ,[CollectionID]
		  ,[TaskID]
		  ,[DisplayOrder]
		  ,[DisplayText]
		  ,[SpecimenPartID]
		  ,[TransactionID]
		  ,[ModuleUri]
		  ,[TaskStart]
		  ,[TaskEnd]
		  ,[Result]
		  ,[URI]
		  ,[NumberValue]
		  ,[BoolValue], MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI
		  ,[Description]
           ,[Notes])
SELECT i.[CollectionTaskParentID]
      ,case when c.[CollectionID] is null then i.CollectionID else case when i.CollectionID is null then c.CollectionID else i.CollectionID end end
      ,i.[TaskID]
	,i.[DisplayOrder]
      ,i.[DisplayText]
		  ,i.[SpecimenPartID]
	,i.[TransactionID]
		  ,i.[ModuleUri]
      ,i.[TaskStart]
      ,i.[TaskEnd]
      ,i.[Result]
      ,i.[URI]
		  ,i.[NumberValue]
		  ,i.[BoolValue], i.MetricDescription, i.MetricSource, i.MetricUnit, i.ResponsibleAgent, i.ResponsibleAgentURI
      ,i.[Description]
      ,i.[Notes]
  FROM inserted i left outer join [dbo].[CollectionTask] c ON i.CollectionTaskParentID = c.CollectionTaskID

  GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.26'
END

GO


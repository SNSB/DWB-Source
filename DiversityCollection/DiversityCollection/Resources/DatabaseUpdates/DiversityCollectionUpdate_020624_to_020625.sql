declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.24'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Update content in [dbo].[TaskType_Enum]  ###################################################################
--#####################################################################################################################

UPDATE [dbo].[TaskType_Enum]
   SET [Description] = 'Specimen stored e.g. within a DiversityCollection database'
      ,[DisplayEnable] = 1
 WHERE Code = 'Collection'
GO


UPDATE [dbo].[TaskType_Enum]
   SET [Description] = 'Part of a specimen e.g. affected by pests and stored within a DiversityCollection database'
      ,[DisplayEnable] = 1
 WHERE Code = 'Part'
GO



--#####################################################################################################################
--######   CollectionTaskHierarchyAll including new column TaskDisplayText  ###########################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionTaskHierarchyAll] ()  
RETURNS @TaskList TABLE ([CollectionTaskID] [int] Primary key ,
	[CollectionTaskParentID] [int] NULL ,
	[CollectionID] [int] NULL ,
	[TaskID] [int] NULL ,
	[DisplayOrder] [int] NULL ,
	[Task] [nvarchar] (400) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [nvarchar] (400) COLLATE Latin1_General_CI_AS NULL ,
	[ModuleUri] [varchar](500) NULL,
	[CollectionSpecimenID] [int] NULL,
	[SpecimenPartID] [int] NULL,
	[TransactionID] [int] NULL,
	[TaskStart] [datetime] NULL,
	[TaskEnd] [datetime] NULL,
	[Result] [nvarchar](400) NULL,
	[URI] [varchar](500) NULL,
	[NumberValue] [real] NULL,
	[BoolValue] [bit] NULL,
	[MetricDescription] [nvarchar](500) NULL,
	[MetricSource] [varchar](4000) NULL,
	[MetricUnit] [nvarchar](50) NULL,
	[ResponsibleAgent] [nvarchar](500) NULL,
	[ResponsibleAgentURI] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[HierarchyDisplayText] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[CollectionHierarchyDisplayText] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[TaskHierarchyDisplayText] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[TaskDisplayText] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the CollectionTask items related to the given CollectionTask.
MW 02.01.2021
TEST:
SELECT * FROM DBO.CollectionTaskHierarchyAll()
*/
AS
BEGIN

-- Separator for Tasks
declare @S nchar(3);
set @S = ([dbo].[TaskHierarchySeparator] ());
declare @C nchar(3);
set @C = (select dbo.TaskCollectionHierarchySeparator())

INSERT @TaskList (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, Task , DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, 
Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes, HierarchyDisplayText)
SELECT DISTINCT CollectionTaskID, CollectionTaskParentID, CollectionID, C.TaskID, C.DisplayOrder, T.DisplayText, C.DisplayText, C.ModuleUri, C.CollectionSpecimenID, C.SpecimenPartID, C.TransactionID, C.TaskStart, C.TaskEnd, 
C.Result, C.URI, C.NumberValue, C.BoolValue, C.MetricDescription, C.MetricSource, C.MetricUnit, C.ResponsibleAgent, C.ResponsibleAgentURI, C.Description, C.Notes, T.DisplayText
FROM CollectionTask C INNER JOIN Task T ON C.TaskID = T.TaskID
WHERE C.CollectionTaskParentID IS NULL
declare @i int
set @i = (select count(*) from CollectionTask where CollectionTaskID not IN (select CollectionTaskID from  @TaskList))
while (@i > 0)
	begin
	INSERT @TaskList (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, Task, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, 
	Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes, HierarchyDisplayText)
	SELECT DISTINCT C.CollectionTaskID, C.CollectionTaskParentID, C.CollectionID, C.TaskID, C.DisplayOrder, T.DisplayText, C.DisplayText, C.ModuleUri, C.CollectionSpecimenID, C.SpecimenPartID, C.TransactionID, C.TaskStart, C.TaskEnd, 
	C.Result, C.URI, C.NumberValue, C.BoolValue, C.MetricDescription, C.MetricSource, C.MetricUnit, C.ResponsibleAgent, C.ResponsibleAgentURI, C.Description,  C.Notes, L.HierarchyDisplayText +  @S  + T.DisplayText
	FROM CollectionTask C 
	INNER JOIN Task T ON C.TaskID = T.TaskID
	INNER JOIN @TaskList L ON C.CollectionTaskParentID = L.CollectionTaskID 
	WHERE C.CollectionTaskID NOT IN (select CollectionTaskID from  @TaskList)
	set @i = (select count(*) from CollectionTask where CollectionTaskID not IN (select CollectionTaskID from  @TaskList))
end

-- adding the date and result if present
update T set T.HierarchyDisplayText = T.HierarchyDisplayText 
+ case when C.TaskStart is null then '' else ' ' + convert(varchar(10), C.TaskStart, 23) 
	+ case when C.TaskEnd is null then '' else @S + convert(varchar(10), C.TaskEnd, 23) end
end
+ case when C.Result is null or rtrim(C.Result) = '' then '' else  @C + C.Result end  
from @TaskList T inner join CollectionTask C on T.CollectionTaskID = C.CollectionTaskID

-- adding the display text if different from task
update L set L.HierarchyDisplayText = L.HierarchyDisplayText + ' : ' + rtrim(L.DisplayText)  
from @TaskList L inner join Task T on T.TaskID = L.TaskID and T.DisplayText <> L.DisplayText and rtrim(T.DisplayText) <> ''
and (L.Result is null or rtrim(L.Result) = '') and rtrim(L.DisplayText) <> ''

-- setting CollectionHierarchyDisplayText and adding collection hierarchy to HierarchyDisplayText
update L set L.CollectionHierarchyDisplayText = C.DisplayText + @C + Replace(L.HierarchyDisplayText, ' | ', @S),
L.HierarchyDisplayText = L.HierarchyDisplayText + @C + C.DisplayText
from @TaskList L inner join [dbo].[CollectionHierarchyAll]() AS C on C.CollectionID = L.CollectionID 

-- setting TaskDisplayText
update L set L.TaskDisplayText = 
CASE WHEN L.DisplayText IS NULL OR L.DisplayText = '' THEN 
CASE WHEN L.MetricDescription <> '' AND L.MetricUnit <> '' THEN L.MetricDescription + ' ' + L.MetricUnit 
ELSE CASE WHEN T.DisplayText IS NULL THEN T.Type ELSE T.DisplayText END END
ELSE
CASE WHEN T.DisplayText <> L.DisplayText THEN T.DisplayText + ': ' ELSE '' END
+ L.DisplayText
+ CASE WHEN NOT L.TaskStart IS NULL THEN ' ' + CONVERT(varchar(10), L.TaskStart, 102) ELSE '' END
+ CASE WHEN NOT L.TaskStart IS NULL AND NOT L.TaskEnd IS NULL THEN ' -' ELSE '' END
+ CASE WHEN NOT L.TaskEnd IS NULL THEN ' ' + CONVERT(varchar(10), L.TaskEnd, 102) ELSE '' END
+ CASE WHEN NOT L.Result IS NULL THEN ' ' + L.Result ELSE '' END
+ CASE WHEN NOT L.NumberValue IS NULL THEN ': ' + CAST(L.NumberValue AS varchar) ELSE '' END
END 
from @TaskList L inner join Task AS T on T.TaskID = L.TaskID 

RETURN
END

GO

--#####################################################################################################################
--######   CollectionHierarchySuperior including column Type   ########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionHierarchySuperior] (@CollectionID int)  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationHeight] [float] NULL,
	[LocationGeometry] geometry NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL,
	[Type] [nvarchar](50) NULL)
/*
Returns a table that lists the given and all the items superior to the given collection.
MW 02.09.2016
Test:
SELECT * FROM dbo.CollectionHierarchySuperior(3)
SELECT * FROM dbo.CollectionHierarchySuperior(1015)
SELECT * FROM dbo.CollectionHierarchySuperior(0)
*/
AS
BEGIN
declare @ParentID int
declare @i int

-- insert the given collection
   INSERT @CollectionList
   SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type
   FROM Collection
   WHERE Collection.CollectionID = @CollectionID

-- insert the superior collections
-- check if there is any superior collection
if (select count(*) from @CollectionList where CollectionID = @CollectionID and CollectionParentID is null) = 1
	RETURN

-- getting the superiors
set @ParentID = (select MAX(CollectionParentID) from @CollectionList where not CollectionParentID is null AND CollectionParentID NOT IN (SELECT CollectionID FROM @CollectionList)) 
set @i = 1
while (@i > 0)
begin
	set @ParentID = (select MAX(CollectionParentID) from @CollectionList where not CollectionParentID is null AND CollectionParentID NOT IN (SELECT CollectionID FROM @CollectionList)) 
	--set @ParentID = @CollectionID
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type
	FROM Collection
	WHERE Collection.CollectionID = @ParentID
	AND Collection.CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	set @i = (select count(*) from @CollectionList where not CollectionParentID is null AND CollectionParentID NOT IN (SELECT CollectionID FROM @CollectionList))
end

declare @LocationPlan [varchar](500);
declare @LocationPlanWidth [float];
declare @LocationGeometry geometry;
set @LocationPlan = (select LocationPlan from @CollectionList where CollectionID = @ParentID)
set @LocationPlanWidth = (select LocationPlanWidth from @CollectionList where CollectionID = @ParentID)
set @LocationGeometry = (select LocationGeometry from @CollectionList where CollectionID = @ParentID)
while (@ParentID <> @CollectionID)
begin
	update C set C.LocationPlan = @LocationPlan from @CollectionList C where C.CollectionParentID = @ParentID and (C.LocationPlan is null or C.LocationPlan = '')
	update C set C.LocationPlanWidth = @LocationPlanWidth from @CollectionList C where C.CollectionParentID = @ParentID and C.LocationPlanWidth is null
	update C set C.LocationGeometry = @LocationGeometry from @CollectionList C where C.CollectionParentID = @ParentID and C.LocationGeometry is null

	set @ParentID = (select CollectionID from @CollectionList where CollectionParentID = @ParentID)
	
	set @LocationPlan = (select LocationPlan from @CollectionList where CollectionID = @ParentID)
	set @LocationPlanWidth = (select LocationPlanWidth from @CollectionList where CollectionID = @ParentID)
	set @LocationGeometry = (select LocationGeometry from @CollectionList where CollectionID = @ParentID)
end

RETURN

END

GO

--#####################################################################################################################
--######   CollectionHierarchyAll using Acronym if present  ###########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionHierarchyAll] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationHeight] [float] NULL,
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
declare @Temp TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL)

	INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type], DisplayText)
	SELECT DISTINCT CollectionID, case when CollectionParentID = CollectionID then null else CollectionParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type]
	, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
	FROM Collection C
	WHERE C.CollectionParentID IS NULL
	declare @i int
	declare @i2 int
	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @Temp))
	set @i2 = @i
	while (@i > 0)
		begin
		INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type], DisplayText)
		SELECT DISTINCT C.CollectionID, case when C.CollectionParentID = C.CollectionID then null else C.CollectionParentID end, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
		C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], L.DisplayText + ' | ' + case when C.CollectionAcronym IS NULL OR C.CollectionAcronym = '' then C.CollectionName else C.CollectionAcronym end
		FROM Collection C, @Temp L
		WHERE C.CollectionParentID = L.CollectionID
		AND C.CollectionID NOT IN (select CollectionID from  @Temp)
		set @i = (select count(*) from Collection C where CollectionID not IN (select CollectionID from  @Temp))
		if @i2 > 0 and @i = @i2
		begin
			INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type], DisplayText)
			SELECT DISTINCT C.CollectionID, NULL, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
			C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.CollectionName
			FROM Collection C, @Temp L
			WHERE C.CollectionID NOT IN (select CollectionID from  @Temp)
			set @i = 0
		end
		set @i2 = @i
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
		begin 	
		INSERT @CollectionList (
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type], DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type], DisplayText
		FROM @Temp L
		end
	else
	begin
		INSERT @CollectionList (
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type], DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type], DisplayText
		FROM @Temp L WHERE L.CollectionID IN ( SELECT CollectionID FROM [dbo].[UserCollectionList] ())
	end
   RETURN
END

GO

--#####################################################################################################################
--######  New collection type trap  ###################################################################################
--#####################################################################################################################

INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('trap'
           ,'A trap for monitoring pest e.g. for IPM'
           ,'trap'
           ,1)
GO



--#####################################################################################################################
--######   New task Types IPM and Pest  ###############################################################################
--#####################################################################################################################

INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('IPM', 'Integrated Pest Management', 'IPM', 1, '');
GO

INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Pest', 'A pest recorded e.g. during Integrated Pest Management', 'Pest', 1, '');
GO

--#####################################################################################################################
--######   Disabling not used Task Types  #############################################################################
--#####################################################################################################################

UPDATE TaskType_Enum SET DisplayEnable = 0
WHERE Code NOT IN ('IPM', 'Pest', 'Trap', 'Monitoring', 'Sensor', 'Collection', 'Part')

--#####################################################################################################################
--######   Recreate CacheDescription  #################################################################################
--#####################################################################################################################


EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'Context'
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'LanguageCode'
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'TableName'
GO

ALTER TABLE [dbo].[CacheDescription] DROP CONSTRAINT [DF_CacheDescription_Context]
GO

ALTER TABLE [dbo].[CacheDescription] DROP CONSTRAINT [DF_CacheDescription_LanguageCode]
GO

/****** Object:  Table [dbo].[CacheDescription]    Script Date: 03.12.2021 07:02:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CacheDescription]') AND type in (N'U'))
DROP TABLE [dbo].[CacheDescription]
GO

/****** Object:  Table [dbo].[CacheDescription]    Script Date: 03.12.2021 07:02:27 ******/
CREATE TABLE [dbo].[CacheDescription](
	[TableName] [varchar](50) NOT NULL,
	[ColumnName] [varchar](50) NOT NULL,
	[LanguageCode] [varchar](50) NOT NULL,
	[Context] [nvarchar](50) NOT NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Abbreviation] [nvarchar](20) NULL,
	[Description] [nvarchar](max) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CacheDescription] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC,
	[ColumnName] ASC,
	[LanguageCode] ASC,
	[Context] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CacheDescription] ADD  CONSTRAINT [DF_CacheDescription_LanguageCode]  DEFAULT ('en-US') FOR [LanguageCode]
GO

ALTER TABLE [dbo].[CacheDescription] ADD  CONSTRAINT [DF_CacheDescription_Context]  DEFAULT ('General') FOR [Context]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'TableName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the table column' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The language code for the description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'LanguageCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A context e.g. as definded in table EntityContext_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'Context'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The text for the table or column as shown e.g. in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The abbreviation for the table or column as shown e.g. in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'Abbreviation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description for the table column' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A unique ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'ID'
GO

GRANT SELECT ON [dbo].[CacheDescription] TO [User]
GO
GRANT UPDATE ON [dbo].[CacheDescription] TO [User]
GO
GRANT DELETE ON [dbo].[CacheDescription] TO [User]
GO
GRANT INSERT ON [dbo].[CacheDescription] TO [User]
GO



--#####################################################################################################################
--######   procFillCacheDescription - include entity   ################################################################
--#####################################################################################################################

ALTER  PROCEDURE [dbo].[procFillCacheDescription] 
AS
/*
exec dbo.procFillCacheDescription
select * from CacheDescription
*/

truncate table CacheDescription;

-- filling the columns
insert into CacheDescription (TableName, ColumnName)
select T.TABLE_NAME, C.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.TABLES T
where C.TABLE_NAME = T.TABLE_NAME
and T.TABLE_TYPE = 'BASE TABLE'
and T.TABLE_SCHEMA = 'dbo'
and T.TABLE_NAME not like '%_log'
and T.TABLE_NAME not like '%_Enum'

declare @i int;
declare @table varchar(50)
declare @column varchar(50)
declare @ID int
declare @description nvarchar(4000)

set @i = (select count(*) from CacheDescription where Description is null)
while @i > 0
begin
	set @ID = (select min(ID) from CacheDescription where Description is null)
	set @table = (select TableName from CacheDescription where ID = @ID)
	set @column = (select ColumnName from CacheDescription where ID = @ID)
	set @description = (SELECT max(CONVERT(nvarchar(MAX), [value]))  FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @table, 'column', @column) WHERE name =  'MS_Description')
	if @description is null begin set @description = '' end
	update CacheDescription set DisplayText = @column, Description = @description where ID = @ID
	set @i = (select count(*) from CacheDescription where Description is null)
end

-- filling the tables
insert into CacheDescription (TableName, ColumnName)
select T.TABLE_NAME, '' from INFORMATION_SCHEMA.TABLES T
where T.TABLE_TYPE = 'BASE TABLE'
and T.TABLE_SCHEMA = 'dbo'
and T.TABLE_NAME not like '%_log'
and T.TABLE_NAME not like '%_Enum'

set @i = (select count(*) from CacheDescription where Description is null)
while @i > 0
begin
	set @ID = (select min(ID) from CacheDescription where Description is null)
	set @table = (select TableName from CacheDescription where ID = @ID)
	set @description = (SELECT max(CONVERT(nvarchar(MAX), [value]))  FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @table, NULL, NULL) WHERE name =  'MS_Description')
	if @description is null begin set @description = '' end
	update CacheDescription set DisplayText = @table, Description = @description where ID = @ID
	set @i = (select count(*) from CacheDescription where Description is null)
end

-- update the tables according to entities for existing entries
  UPDATE C SET C.DisplayText = case when E.[DisplayText] <> '' then E.[DisplayText] else C.DisplayText end
      ,C.[Abbreviation] = case when E.[Abbreviation] <> '' then E.[Abbreviation] else C.[Abbreviation] end
      ,C.[Description] = case when E.[Description] <> '' then E.[Description] else C.[Description] end
  FROM [dbo].[EntityRepresentation] E INNER JOIN CacheDescription C ON C.TableName = E.Entity AND C.ColumnName = '' AND E.EntityContext = C.Context AND C.LanguageCode = E.LanguageCode
  WHERE E.Entity NOT LIKE '%.%'

-- update the columns according to entities for existing entries
  UPDATE C SET C.DisplayText = case when E.[DisplayText] <> '' then E.[DisplayText] else C.DisplayText end
      ,C.[Abbreviation] = case when E.[Abbreviation] <> '' then E.[Abbreviation] else C.[Abbreviation] end
      ,C.[Description] = case when E.[Description] <> '' then E.[Description] else C.[Description] end
  FROM [dbo].[EntityRepresentation] E 
  INNER JOIN CacheDescription C ON C.TableName = SUBSTRING([Entity], 1, CHARINDEX('.', Entity)-1) AND C.ColumnName = SUBSTRING([Entity], CHARINDEX('.', Entity)+1, 255) AND E.EntityContext = C.Context AND C.LanguageCode = E.LanguageCode
  WHERE E.Entity LIKE '%.%'

  --insert missing tables
INSERT INTO [dbo].[CacheDescription] ([TableName],[ColumnName],[LanguageCode],[Context],[DisplayText],[Abbreviation],[Description])
SELECT [Entity] AS TableName
	  ,'' AS ColumnName
      ,[LanguageCode]
      ,[EntityContext] AS Context
      ,[DisplayText]
      ,[Abbreviation]
      ,[Description]
  FROM [dbo].[EntityRepresentation] E INNER JOIN INFORMATION_SCHEMA.TABLES T ON T.TABLE_NAME = E.Entity
  WHERE E.Entity NOT LIKE '%.%'
AND NOT EXISTS (SELECT * FROM [CacheDescription] C WHERE C.TableName = E.[Entity] AND C.ColumnName = '' AND C.LanguageCode = E.LanguageCode AND C.Context = E.EntityContext)


  -- insert missing columns
INSERT INTO [dbo].[CacheDescription] ([TableName],[ColumnName],[LanguageCode],[Context],[DisplayText],[Abbreviation],[Description])
SELECT SUBSTRING([Entity], 1, CHARINDEX('.', Entity)-1) AS TableName
	  ,SUBSTRING([Entity], CHARINDEX('.', Entity)+1, 255) AS ColumnName
      ,[LanguageCode]
      ,[EntityContext] AS Context
      ,[DisplayText]
      ,[Abbreviation]
      ,[Description]
  FROM [dbo].[EntityRepresentation] E INNER JOIN INFORMATION_SCHEMA.COLUMNS T ON T.TABLE_NAME = SUBSTRING([Entity], 1, CHARINDEX('.', Entity)-1) AND T.COLUMN_NAME = SUBSTRING([Entity], CHARINDEX('.', Entity)+1, 255)
  WHERE E.Entity LIKE '%.%'
AND NOT EXISTS (SELECT * FROM [CacheDescription] C WHERE E.Entity LIKE '%.%' AND C.TableName = SUBSTRING([Entity], 1, CHARINDEX('.', Entity)-1) AND C.ColumnName = SUBSTRING([Entity], CHARINDEX('.', Entity)+1, 255) AND C.LanguageCode = E.LanguageCode AND C.Context = E.EntityContext)


GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.25'
END

GO


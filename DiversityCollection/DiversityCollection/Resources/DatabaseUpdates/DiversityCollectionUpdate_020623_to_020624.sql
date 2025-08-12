declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.23'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   new table CacheDescription   ###############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CacheDescription](
	[TableName] [varchar](50) NOT NULL,
	[ColumnName] [varchar](50) NOT NULL,
	[LanguageCode] [char](2) NOT NULL,
	[Context] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](4000) NULL,
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

ALTER TABLE [dbo].[CacheDescription] ADD  CONSTRAINT [DF_CacheDescription_LanguageCode]  DEFAULT ('en') FOR [LanguageCode]
GO

ALTER TABLE [dbo].[CacheDescription] ADD  CONSTRAINT [DF_CacheDescription_Context]  DEFAULT ('') FOR [Context]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'TableName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the table column' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The language code for the description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'LanguageCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A context e.g. as definded in table EntityContext_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'Context'
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
--######   procFillCacheDescription   #################################################################################
--#####################################################################################################################

CREATE  PROCEDURE [dbo].[procFillCacheDescription] 
AS
/*
exec dbo.procFillCacheDescription
select * from CacheDescription
*/

truncate table CacheDescription;
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
	update CacheDescription set Description = @description where ID = @ID
	set @i = (select count(*) from CacheDescription where Description is null)
end

GO

GRANT EXEC ON [dbo].[procFillCacheDescription] TO [User]
GO

--#####################################################################################################################
--######  New collection types subdivided container and sensor ########################################################
--#####################################################################################################################

INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('subdivided container'
           ,'A container subdivided into chambers'
           ,'subdivided container'
           ,1)
GO

INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('sensor'
           ,'A sensor for e.g. temperature or pests'
           ,'sensor'
           ,1)
GO

if (SELECT COUNT(*) FROM [CollCollectionType_Enum] WHERE Code = 'collection') = 0
begin
INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('collection'
           ,'A collection of objects'
           ,'collection'
           ,1)
end
GO


--#####################################################################################################################
--######  New taxonomic group artefact ################################################################################
--#####################################################################################################################

INSERT INTO [dbo].[CollTaxonomicGroup_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('artefact'
           ,'An item made or given shape by humans, such as a tool or a work of art'
           ,'artefact'
           ,1)
GO


--#####################################################################################################################
--######  New transaction type for warnings  ##########################################################################
--#####################################################################################################################

INSERT INTO [dbo].[CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('warning'
           ,'A warning e.g. against biocides in samples'
           ,'warning'
           ,1)
GO



--#####################################################################################################################
--######  New columns in Collection for geometry  #####################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[Collection] ADD [LocationPlan] [varchar](500) NULL;
ALTER TABLE [dbo].[Collection_log] ADD [LocationPlan] [varchar](500) NULL;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI or file name including path of the floor plan of the collection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Collection', @level2type=N'COLUMN',@level2name=N'LocationPlan'
GO

ALTER TABLE [dbo].[Collection] ADD [LocationPlanWidth] [float] NULL;
ALTER TABLE [dbo].[Collection_log] ADD [LocationPlanWidth] [float] NULL;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Width of location plan in meter for calculation of size by provided geometry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Collection', @level2type=N'COLUMN',@level2name=N'LocationPlanWidth'
GO

ALTER TABLE [dbo].[Collection] ADD [LocationGeometry] [geometry] NULL;
ALTER TABLE [dbo].[Collection_log] ADD [LocationGeometry] [geometry] NULL;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Geometry of the collection within the floor plan' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Collection', @level2type=N'COLUMN',@level2name=N'LocationGeometry'
GO

ALTER TABLE [dbo].[Collection] ADD [LocationHeight] [float] NULL;
ALTER TABLE [dbo].[Collection_log] ADD [LocationHeight] [float] NULL;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Height from ground level, e.g. for the position of sensors' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Collection', @level2type=N'COLUMN',@level2name=N'LocationHeight'
GO


--#####################################################################################################################
--######  New columns in trgDelCollection  ############################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollection] ON [dbo].[Collection] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 
/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
Location, LocationPlan, LocationPlanWidth, LocationGeometry, LocationHeight, CollectionOwner, DisplayOrder, [Type], RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.CollectionID, deleted.CollectionParentID, deleted.CollectionName, deleted.CollectionAcronym, deleted.AdministrativeContactName, deleted.AdministrativeContactAgentURI, deleted.Description, 
deleted.Location, deleted.LocationPlan, deleted.LocationPlanWidth, deleted.LocationGeometry, deleted.LocationHeight, deleted.CollectionOwner, deleted.DisplayOrder, deleted.[Type], deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED
GO


--#####################################################################################################################
--######  New columns in trgUpdCollection  ############################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollection] ON [dbo].[Collection] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 
/* updating the logging columns */
Update Collection
set LogUpdatedWhen = getdate(), LogUpdatedBy = cast(dbo.UserID() as varchar)
FROM Collection, deleted 
where 1 = 1 
AND Collection.CollectionID = deleted.CollectionID
/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
Location, LocationPlan, LocationPlanWidth, LocationGeometry, LocationHeight, CollectionOwner, DisplayOrder, [Type], RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.CollectionID, deleted.CollectionParentID, deleted.CollectionName, deleted.CollectionAcronym, deleted.AdministrativeContactName, deleted.AdministrativeContactAgentURI, deleted.Description, 
deleted.Location, deleted.LocationPlan, deleted.LocationPlanWidth, deleted.LocationGeometry, deleted.LocationHeight, deleted.CollectionOwner, deleted.DisplayOrder, deleted.[Type], deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED
GO



--#####################################################################################################################
--######   CollectionChildNodes including columns LocationPlan, LocationPlanWidth, LocationHeight  ####################
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
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 03.12.2018
Test
select * from dbo.CollectionChildNodes(1)
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
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL)
INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type]) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type]
	FROM Collection WHERE CollectionParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type])
		select C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type]
		from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem)
		set @i = (select count(*) from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	end

 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type]) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type]
   FROM @TempItem ORDER BY CollectionName
   RETURN
END
GO


--#####################################################################################################################
--######   CollectionHierarchy  including columns LocationPlan, LocationPlanWidth, LocationHeight   ###################
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
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationHeight] [float] NULL,
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
   SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type]
   FROM Collection
   WHERE Collection.CollectionID = @TopID
  INSERT @CollectionList
   SELECT * FROM dbo.CollectionChildNodes (@TopID)
   RETURN
END
GO



--#####################################################################################################################
--######   CollectionHierarchyAll including columns LocationPlan, LocationPlanWidth, LocationHeight  ##################
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
		C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], L.DisplayText + ' | ' + C.CollectionName
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
--######   CollectionHierarchyMulti including columns LocationPlan, LocationPlanWidth, LocationHeight   ###############
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionHierarchyMulti] (@CollectionIDs varchar(255))  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationHeight] [float] NULL,
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
	Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type]
	FROM Collection
	WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	AND CollectionID = @CollectionID
	declare @TopID int
	set @TopID = (SELECT CollectionParentID FROM Collection WHERE CollectionID = @CollectionID)
	while not @TopID is null
	begin
		INSERT @CollectionList
		SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
		Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, [Type]
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
--######   CollectionHierarchySuperior including columns LocationPlan, LocationPlanWidth, LocationHeight   ############
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
	[DisplayOrder] [smallint]  NULL)
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
   SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder
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
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder
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
--######  Grants for DataManager  #####################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Can insert, update and delete data from tables Analysis, Annotation, CollectionEvent, CollectionSpecimen, Method, MethodForAnalysis, MethodForProcessing, Processing, ProcessingMaterialCategory, ProjectAnalysis, ProjectProcessing',
@level0type = N'USER', @level0name = 'DataManager';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Can insert, update and delete data from tables Analysis, Annotation, CollectionEvent, CollectionSpecimen, Method, MethodForAnalysis, MethodForProcessing, Processing, ProcessingMaterialCategory, ProjectAnalysis, ProjectProcessing',
@level0type = N'USER', @level0name = 'DataManager';
END CATCH;
GO


GRANT INSERT ON [dbo].[Analysis] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[Analysis] TO [DataManager]
GO
GRANT DELETE ON [dbo].[Analysis] TO [DataManager]
GO
GRANT INSERT ON [dbo].[Analysis_log] TO [DataManager]
GO

GRANT INSERT ON [dbo].[AnalysisResult] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[AnalysisResult] TO [DataManager]
GO
GRANT DELETE ON [dbo].[AnalysisResult] TO [DataManager]
GO

GRANT INSERT ON [dbo].[AnalysisTaxonomicGroup] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[AnalysisTaxonomicGroup] TO [DataManager]
GO
GRANT DELETE ON [dbo].[AnalysisTaxonomicGroup] TO [DataManager]
GO

GRANT INSERT ON [dbo].[Method] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[Method] TO [DataManager]
GO
GRANT DELETE ON [dbo].[Method] TO [DataManager]
GO
GRANT INSERT ON [dbo].[Method_log] TO [DataManager]
GO

GRANT INSERT ON [dbo].[MethodForAnalysis] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[MethodForAnalysis] TO [DataManager]
GO
GRANT DELETE ON [dbo].[MethodForAnalysis] TO [DataManager]
GO

GRANT INSERT ON [dbo].[MethodForProcessing] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[MethodForProcessing] TO [DataManager]
GO
GRANT DELETE ON [dbo].[MethodForProcessing] TO [DataManager]
GO

GRANT INSERT ON [dbo].[Parameter] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[Parameter] TO [DataManager]
GO
GRANT DELETE ON [dbo].[Parameter] TO [DataManager]
GO

GRANT INSERT ON [dbo].[ParameterValue_Enum] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[ParameterValue_Enum] TO [DataManager]
GO
GRANT DELETE ON [dbo].[ParameterValue_Enum] TO [DataManager]
GO

GRANT INSERT ON [dbo].[CollectionEventMethod] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[CollectionEventMethod] TO [DataManager]
GO
GRANT DELETE ON [dbo].[CollectionEventMethod] TO [DataManager]
GO

GRANT INSERT ON [dbo].[CollectionEventParameterValue] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[CollectionEventParameterValue] TO [DataManager]
GO
GRANT DELETE ON [dbo].[CollectionEventParameterValue] TO [DataManager]
GO

GRANT INSERT ON [dbo].[Processing] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[Processing] TO [DataManager]
GO
GRANT DELETE ON [dbo].[Processing] TO [DataManager]
GO
GRANT INSERT ON [dbo].[Processing_log] TO [DataManager]
GO

GRANT INSERT ON [dbo].[ProcessingMaterialCategory] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[ProcessingMaterialCategory] TO [DataManager]
GO
GRANT DELETE ON [dbo].[ProcessingMaterialCategory] TO [DataManager]
GO

GRANT INSERT ON [dbo].[ProjectAnalysis] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[ProjectAnalysis] TO [DataManager]
GO
GRANT DELETE ON [dbo].[ProjectAnalysis] TO [DataManager]
GO

GRANT INSERT ON [dbo].[ProjectProcessing] TO [DataManager]
GO
GRANT UPDATE ON [dbo].[ProjectProcessing] TO [DataManager]
GO
GRANT DELETE ON [dbo].[ProjectProcessing] TO [DataManager]
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Full access for tables CollectionEventRegulation and Regulation',
@level0type = N'USER', @level0name = 'RegulationManager';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Full access for tables CollectionEventRegulation and Regulation',
@level0type = N'USER', @level0name = 'RegulationManager';
END CATCH;
GO



--#####################################################################################################################
--######  TaskType_Enum  ##############################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaskType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[Icon] [image] NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_TaskType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TaskType_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[TaskType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_TaskType_Enum_TaskType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[TaskType_Enum] ([Code])
GO

ALTER TABLE [dbo].[TaskType_Enum] CHECK CONSTRAINT [FK_TaskType_Enum_TaskType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A symbol representing this entry in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum', @level2type=N'COLUMN',@level2name=N'Icon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of a task, e.g. freezing etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskType_Enum'
GO


/****** Skript 
SELECT 'INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES (''' + 
[Code] + ''', '''
+ [Description] + ''', '''
+ [DisplayText] + ''', 1, '''
+ case when [InternalNotes] <> '' then InternalNotes else '' end + ''', '''
+ case when [ParentCode] <> '' then ParentCode else '' end + ''');', Code, ParentCode
  FROM [DiversityCollection].[dbo].[TaskType_Enum]
  Order by ParentCode, Code

  SELECT 'INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES (''' + 
[Code] + ''', '''
+ [Description] + ''', '''
+ [DisplayText] + ''', 1, '''
+ case when [InternalNotes] <> '' then InternalNotes else '' end + ''');', Code, ParentCode
  FROM [DiversityCollection].[dbo].[TaskType_Enum]
  where ParentCode Is null
  Order by Code
******/

INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('DiversityWorkbench', 'A module of the DiversityWorkbench', 'DiversityWorkbench', 1, 'Related to the DiversityWorkbench');

INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Agent', 'Agent', 'Agent', 1, 'Related to DiversityAgents', 'DiversityWorkbench');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Collection', 'Specimen part within a remote database', 'Collection', 0, 'Related to DiversityCollection', 'DiversityWorkbench');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Description', 'Description', 'Description', 1, 'Related to DiversityDescriptions', 'DiversityWorkbench');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Gazetteer', 'Gazetteer', 'Gazetteer', 1, 'Related to DiversityGazetteer', 'DiversityWorkbench');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Project', 'Project', 'Project', 1, 'Related to DiversityProjects', 'DiversityWorkbench');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Sampling plot', 'Sampling plot', 'Sampling plot', 1, 'Related to DiversitySamplingPlots', 'DiversityWorkbench');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Scientific term', 'Scientific term', 'Scientific term', 1, 'Related to DiversityScientificTerms', 'DiversityWorkbench');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes, ParentCode) VALUES ('Taxon name', 'Taxon name', 'Taxon name', 1, 'Related to DiversityTaxonNames', 'DiversityWorkbench');

INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Analysis', 'Analysis', 'Analysis', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Cleaning', 'Cleaning', 'Cleaning', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Damage', 'Damage', 'Damage', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Document', 'Document', 'Document', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Evaluation', 'Evaluation', 'Evaluation', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Freezing', 'Freezing', 'Freezing', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Gas', 'Gas', 'Gas', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Inspection', 'Inspection', 'Inspection', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Legislation', 'Legislation', 'Legislation', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Monitoring', 'Monitoring', 'Monitoring', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Part', 'Part', 'Part', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Payment', 'Payment', 'Payment', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Poison', 'Poison', 'Poison', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Problem', 'Problem', 'Problem', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Processing', 'Processing', 'Processing', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Query', 'Query', 'Query', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Radiation', 'Radiation', 'Radiation', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Repair', 'Repair', 'Repair', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Search', 'Search', 'Search', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Sensor', 'Sensor', 'Sensor', 0, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, ParentCode) VALUES ('Humidity', 'Humidity', 'Humidity', 1, 'Sensor');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, ParentCode) VALUES ('Temperature', 'Temperature', 'Temperature', 1, 'Sensor');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Task', 'Task', 'Task', 1, '');
INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable, InternalNotes) VALUES ('Trap', 'Trap', 'Trap', 1, '');
GO

GRANT SELECT ON [dbo].[TaskType_Enum] TO [User]
GO
GRANT INSERT ON [dbo].[TaskType_Enum] TO [Administrator]
GO
GRANT UPDATE ON [dbo].[TaskType_Enum] TO [Administrator]
GO
GRANT DELETE ON [dbo].[TaskType_Enum] TO [Administrator]
GO

--#####################################################################################################################
--######  TaskDateType_Enum  ##########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaskDateType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Icon] [image] NULL,
 CONSTRAINT [PK_TaskDateType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaskDateType_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[TaskDateType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_TaskDateType_Enum_TaskDateType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[TaskDateType_Enum] ([Code])
GO

ALTER TABLE [dbo].[TaskDateType_Enum] CHECK CONSTRAINT [FK_TaskDateType_Enum_TaskDateType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A symbol representing this entry in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum', @level2type=N'COLUMN',@level2name=N'Icon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of a task, e.g. freezing etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDateType_Enum'
GO

GRANT SELECT ON [dbo].[TaskDateType_Enum] TO [User]
GO

INSERT INTO [TaskDateType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('Date', 'Date', 'Date', 1);
INSERT INTO [TaskDateType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('Date from to', 'Date from to', 'Date from to', 1);
INSERT INTO [TaskDateType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('Date & Time', 'Date & Time', 'Date & Time', 1);
INSERT INTO [TaskDateType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('Date & Time from to', 'Date & Time from to', 'Date & Time from to', 1);
INSERT INTO [TaskDateType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('Time', 'Time', 'Time', 1);
INSERT INTO [TaskDateType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('Time from to', 'Time from to', 'Time from to', 1);


--#####################################################################################################################
--######  TaskModuleType_Enum  ########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaskModuleType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Icon] [image] NULL,
 CONSTRAINT [PK_TaskModuleType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaskModuleType_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[TaskModuleType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_TaskModuleType_Enum_TaskModuleType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[TaskModuleType_Enum] ([Code])
GO

ALTER TABLE [dbo].[TaskModuleType_Enum] CHECK CONSTRAINT [FK_TaskModuleType_Enum_TaskModuleType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A symbol representing this entry in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum', @level2type=N'COLUMN',@level2name=N'Icon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of a task, e.g. freezing etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskModuleType_Enum'
GO

GRANT SELECT ON [dbo].[TaskModuleType_Enum] TO [User]
GO

INSERT INTO [TaskModuleType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('DiversityAgents', 'DiversityAgents', 'DiversityAgents', 1);
INSERT INTO [TaskModuleType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('DiversityCollection', 'DiversityCollection', 'DiversityCollection', 1);
INSERT INTO [TaskModuleType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('DiversityGazetteer', 'DiversityGazetteer', 'DiversityGazetteer', 1);
INSERT INTO [TaskModuleType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('DiversityProjects', 'DiversityProjects', 'DiversityProjects', 1);
INSERT INTO [TaskModuleType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('DiversitySamplingPlots', 'DiversitySamplingPlots', 'DiversitySamplingPlots', 1);
INSERT INTO [TaskModuleType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('DiversityScientificTerms', 'DiversityScientificTerms', 'DiversityScientificTerms', 1);
INSERT INTO [TaskModuleType_Enum] (Code, Description, DisplayText, DisplayEnable) VALUES ('DiversityTaxonNames', 'DiversityTaxonNames', 'DiversityTaxonNames', 1);


--#####################################################################################################################
--######  Task  #######################################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[Task](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[TaskParentID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[TaskURI] [varchar](500) NULL,
	[Type] [nvarchar](50) NULL,
	[ModuleTitle] [nvarchar](50) NULL,
	[ModuleType] [nvarchar](50) NULL,
	[SpecimenPartType] [nvarchar](50) NULL,
	[TransactionType] [nvarchar](50) NULL,
	[ResultType] [nvarchar](50) NULL,
	[DateType] [nvarchar](50) NULL,
	[DateBeginType] [nvarchar](50) NULL,
	[DateEndType] [nvarchar](50) NULL,
	[NumberType] [nvarchar](50) NULL,
	[BoolType] [nvarchar](50) NULL,
	[MetricType] [nvarchar](50) NULL,
	[DescriptionType] [nvarchar](50) NULL,
	[NotesType] [nvarchar](50) NULL,
	[UriType] [nvarchar](50) NULL,
	[ResponsibleType] [nvarchar](50) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_Type]  DEFAULT ('Task') FOR [Type]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF__Task__LogCreated__78B651CF]  DEFAULT ([dbo].[UserID]()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF__Task__LogUpdated__79AA7608]  DEFAULT ([dbo].[UserID]()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[Task]  WITH NOCHECK ADD  CONSTRAINT [FK_Task_Task] FOREIGN KEY([TaskParentID])
REFERENCES [dbo].[Task] ([TaskID])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Task]
GO

ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskDateType_Enum] FOREIGN KEY([DateType])
REFERENCES [dbo].[TaskDateType_Enum] ([Code])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_TaskDateType_Enum]
GO

ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskModuleType_Enum] FOREIGN KEY([ModuleType])
REFERENCES [dbo].[TaskModuleType_Enum] ([Code])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_TaskModuleType_Enum]
GO

ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskType_Enum] FOREIGN KEY([Type])
REFERENCES [dbo].[TaskType_Enum] ([Code])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_TaskType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Task (primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TaskID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the superior type of the Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TaskParentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The display text of the Task as shown e.g. in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URI for a Task as defined in an external data source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TaskURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the task as defined in table TaskType_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title for module related data for collection tasks. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ModuleTitle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The DiversityWorkbench module to which a task is related. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ModuleType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description of the collection specimen part to which a task is related. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'SpecimenPartType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description of the transaction to which a task is related. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TransactionType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The display text for the results as shown in a user interface. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ResultType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time details defined for a task. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'DateType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition of the begin for date and time details defined for a task. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'DateBeginType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition of the end for date and time details defined for a task. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'DateEndType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition for the numeric value as shown in a user interface. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'NumberType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition for the boolean value as shown in a user interface. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'BoolType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition for the metric as shown in a user interface. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'MetricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition for the description as shown in a user interface. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'DescriptionType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition for the notes as shown in a user interface. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'NotesType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition for the URI as shown in a user interface. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'UriType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The definition for the responsible agent as shown in a user interface. Not available in depending collection task if empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ResponsibleType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Tasks of the collection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task'
GO

GRANT SELECT ON [dbo].[Task] TO [User]
GO
GRANT INSERT ON [dbo].[Task] TO [CollectionManager]
GO
GRANT UPDATE ON [dbo].[Task] TO [CollectionManager]
GO
GRANT DELETE ON [dbo].[Task] TO [Administrator]
GO


--#####################################################################################################################
--######  Task_log  ###################################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[Task_log](
	[TaskID] [int] NULL,
	[TaskParentID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[TaskURI] [varchar](500) NULL,
	[Type] [nvarchar](50) NULL,
	[ModuleTitle] [nvarchar](50) NULL,
	[ModuleType] [nvarchar](50) NULL,
	[SpecimenPartType] [nvarchar](50) NULL,
	[TransactionType] [nvarchar](50) NULL,
	[ResultType] [nvarchar](50) NULL,
	[DateType] [nvarchar](50) NULL,
	[DateBeginType] [nvarchar](50) NULL,
	[DateEndType] [nvarchar](50) NULL,
	[NumberType] [nvarchar](50) NULL,
	[BoolType] [nvarchar](50) NULL,
	[MetricType] [nvarchar](50) NULL,
	[DescriptionType] [nvarchar](50) NULL,
	[NotesType] [nvarchar](50) NULL,
	[UriType] [nvarchar](50) NULL,
	[ResponsibleType] [nvarchar](50) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Task_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Task_log] ADD  CONSTRAINT [DF_Task_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[Task_log] ADD  CONSTRAINT [DF_Task_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[Task_log] ADD  CONSTRAINT [DF_Task_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID]())) FOR [LogUser]
GO

GRANT SELECT ON [dbo].[Task_log] TO [CollectionManager]
GO
GRANT INSERT ON [dbo].[Task_log] TO [CollectionManager]
GO


--#####################################################################################################################
--######  trgDelTask  #################################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelTask] ON [dbo].[Task] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Task_Log (MetricType, ResponsibleType, BoolType, DateType, Description, DescriptionType, DisplayText, ModuleTitle, ModuleType, Notes, NotesType, NumberType, ResultType, SpecimenPartType, TaskID, TaskParentID, TaskURI, TransactionType, Type, UriType, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.MetricType, D.ResponsibleType, D.BoolType, D.DateType, D.Description, D.DescriptionType, D.DisplayText, D.ModuleTitle, D.ModuleType, D.Notes, D.NotesType, D.NumberType, D.ResultType, D.SpecimenPartType, D.TaskID, D.TaskParentID, D.TaskURI, D.TransactionType, D.Type, D.UriType, D.LogCreatedBy, D.LogCreatedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D
GO

ALTER TABLE [dbo].[Task] ENABLE TRIGGER [trgDelTask]
GO


--#####################################################################################################################
--######  trgDelTask  #################################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdTask] ON [dbo].[Task] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Task_Log (MetricType, ResponsibleType, BoolType, DateType, Description, DescriptionType, DisplayText, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, ModuleTitle, ModuleType, Notes, NotesType, NumberType, ResultType, RowGUID, SpecimenPartType, TaskID, TaskParentID, TaskURI, TransactionType, Type, UriType, LogUser,  LogState) 
SELECT D.MetricType, D.ResponsibleType, D.BoolType, D.DateType, D.Description, D.DescriptionType, D.DisplayText, D.LogCreatedBy, D.LogCreatedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.ModuleTitle, D.ModuleType, D.Notes, D.NotesType, D.NumberType, D.ResultType, D.RowGUID, D.SpecimenPartType, D.TaskID, D.TaskParentID, D.TaskURI, D.TransactionType, D.Type, D.UriType, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM Task T, deleted D where 1 = 1 
AND T.TaskID = D.TaskID
GO

ALTER TABLE [dbo].[Task] ENABLE TRIGGER [trgUpdTask]
GO


--#####################################################################################################################
--######   TaskHierarchySeparator   ###################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[TaskHierarchySeparator] () RETURNS  nchar(3) AS BEGIN return ' ' + nchar(8286) + ' ' END;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a separator used within a task hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'TaskHierarchySeparator'
GO

GRANT EXEC ON [dbo].[TaskHierarchySeparator] TO [User]
GO

/*
select dbo.TaskHierarchySeparator()
*/


--#####################################################################################################################
--######   TaskCollectionHierarchySeparator   #########################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[TaskCollectionHierarchySeparator] () RETURNS  nchar(3) AS BEGIN return ' ' + nchar(8212) + ' ' END;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a separator used between collection and task hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'TaskCollectionHierarchySeparator'
GO

GRANT EXEC ON [dbo].[TaskCollectionHierarchySeparator] TO [User]
GO

/*
select dbo.TaskCollectionHierarchySeparator()
*/


--#####################################################################################################################
--######  TaskChildNodes  #############################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[TaskChildNodes] (@ID int)  
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
	[ResponsibleType] [nvarchar](50) NULL,
	DescriptionType varchar  (50) NULL,
	NotesType varchar  (50) NULL,
	UriType varchar  (50) NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)


 INSERT @TempItem (
		   TaskID , TaskParentID, DisplayText, Description,  Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID) 
	SELECT TaskID , TaskParentID, DisplayText , Description, Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID
	FROM Task WHERE TaskParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, Task C where C.TaskParentID = T.TaskID and C.TaskID not in (select TaskID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (TaskID , TaskParentID, DisplayText, Description, Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID)
		select C.TaskID, C.TaskParentID, C.DisplayText, C.Description, C.Notes , C.TaskURI, C.Type, C.ModuleTitle, C.ModuleType, C.SpecimenPartType, C.TransactionType, C.ResultType, C.DateType, C.DateBeginType, C.DateEndType, C.NumberType, C.BoolType, C.MetricType, C.ResponsibleType, C.DescriptionType, C.NotesType, C.UriType, C.RowGUID
		from @TempItem T, Task C where C.TaskParentID = T.TaskID and C.TaskID not in (select TaskID from @TempItem)
		set @i = (select count(*) from @TempItem T, Task C where C.TaskParentID = T.TaskID and C.TaskID not in (select TaskID from @TempItem))
	end

 INSERT @ItemList (TaskID , TaskParentID, DisplayText, Description, Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID) 
   SELECT distinct TaskID , TaskParentID, DisplayText, Description, Notes , TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType, RowGUID
   FROM @TempItem ORDER BY DisplayText
   RETURN


   RETURN
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'All child nodes of a given Task related via the TaskParentID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'TaskChildNodes'
GO

GRANT SELECT ON TaskChildNodes TO [User]
GO

--#####################################################################################################################
--######  TaskHierarchy  ##############################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[TaskHierarchy] (@TaskID int)  
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
   SELECT DISTINCT TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle,  ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType
   FROM Task
   WHERE Task.TaskID = @TopID
   INSERT @TaskList
            SELECT TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle,  ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType
   FROM dbo.TaskChildNodes (@TopID)
   RETURN
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The hierarchy of a given Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'TaskHierarchy'
GO

GRANT SELECT ON TaskHierarchy TO [User]
GO

--#####################################################################################################################
--######  TaskHierarchyAll  ###########################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[TaskHierarchyAll] ()  
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
INSERT @TaskList (TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType, HierarchyDisplayText, HierarchyDisplayTextInvers)
SELECT DISTINCT   TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, MetricType, ResponsibleType, DescriptionType, NotesType, UriType, DisplayText,          DisplayText
FROM Task
WHERE Task.TaskParentID IS NULL
declare @i int
set @i = (select count(*) from Task where TaskID not IN (select TaskID from  @TaskList))
while (@i > 0)
	begin
	INSERT @TaskList (TaskID,   TaskParentID,   DisplayText,   Description,    Notes,   TaskURI,   Type,   ModuleTitle,   ModuleType, SpecimenPartType,     TransactionType,   ResultType,   DateType, DateBeginType, DateEndType,   NumberType,   BoolType, MetricType, ResponsibleType,   DescriptionType,   NotesType,   UriType, HierarchyDisplayText, HierarchyDisplayTextInvers)
	SELECT DISTINCT C.TaskID, C.TaskParentID, C.DisplayText, C.Description,  C.Notes, C.TaskURI, C.Type, C.ModuleTitle, C.ModuleType, C.SpecimenPartType, C.TransactionType, C.ResultType, C.DateType, C.DateBeginType, C.DateEndType, C.NumberType, C.BoolType, C.MetricType, C.ResponsibleType, C.DescriptionType, C.NotesType, C.UriType, L.HierarchyDisplayText + @S + C.DisplayText, C.DisplayText + @S + L.HierarchyDisplayTextInvers  
	FROM Task C, @TaskList L
	WHERE C.TaskParentID = L.TaskID
	AND C.TaskID NOT IN (select TaskID from  @TaskList)
	set @i = (select count(*) from Task where TaskID not IN (select TaskID from  @TaskList))
end
   RETURN
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'All Tasks including a column displaying the hierarchy of the Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'TaskHierarchyAll'
GO

GRANT SELECT ON TaskHierarchyAll TO [User]
GO



--#####################################################################################################################
--######  TaskResult  #################################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaskResult](
	[TaskID] [int] NOT NULL,
	[Result] [nvarchar](400) NOT NULL,
	[URI] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_TaskResult] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC,
	[Result] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaskResult] ADD  CONSTRAINT [DF_TaskResult_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[TaskResult] ADD  CONSTRAINT [DF_TaskResult_LogCreatedBy]  DEFAULT ([dbo].[UserID]()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[TaskResult] ADD  CONSTRAINT [DF_TaskResult_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[TaskResult] ADD  CONSTRAINT [DF_TaskResult_LogUpdatedBy]  DEFAULT ([dbo].[UserID]()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[TaskResult] ADD  CONSTRAINT [DF_TaskResult_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[TaskResult]  WITH CHECK ADD  CONSTRAINT [FK_TaskResult_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([TaskID])
GO

ALTER TABLE [dbo].[TaskResult] CHECK CONSTRAINT [FK_TaskResult_Task]
GO


GRANT SELECT ON [dbo].[TaskResult] TO [User]
GO
GRANT INSERT ON [dbo].[TaskResult] TO [CollectionManager]
GO
GRANT UPDATE ON [dbo].[TaskResult] TO [CollectionManager]
GO
GRANT DELETE ON [dbo].[TaskResult] TO [CollectionManager]
GO

--#####################################################################################################################
--######  TaskResult_log  #############################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[TaskResult_log](
	[TaskID] [int] NULL,
	[Result] [nvarchar](400) NULL,
	[URI] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TaskResult_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaskResult_log] ADD  CONSTRAINT [DF_TaskResult_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[TaskResult_log] ADD  CONSTRAINT [DF_TaskResult_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[TaskResult_log] ADD  CONSTRAINT [DF_TaskResult_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID]())) FOR [LogUser]
GO

GRANT SELECT ON [dbo].[TaskResult_log] TO [CollectionManager]
GO
GRANT INSERT ON [dbo].[TaskResult_log] TO [CollectionManager]
GO

--#####################################################################################################################
--######  trgDelTaskResult  ###########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelTaskResult] ON [dbo].[TaskResult] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TaskResult_Log (Description, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, Result, RowGUID, TaskID, URI, LogUser,  LogState) 
SELECT D.Description, D.LogCreatedBy, D.LogCreatedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.Notes, D.Result, D.RowGUID, D.TaskID, D.URI, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D 
GO

ALTER TABLE [dbo].[TaskResult] ENABLE TRIGGER [trgDelTaskResult]
GO


--#####################################################################################################################
--######  trgUpdTaskResult  ###########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdTaskResult] ON [dbo].[TaskResult] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TaskResult_Log (Description, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, Result, RowGUID, TaskID, URI, LogUser,  LogState) 
SELECT D.Description, D.LogCreatedBy, D.LogCreatedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.Notes, D.Result, D.RowGUID, D.TaskID, D.URI, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM TaskResult T, deleted D where 1 = 1 
AND T.Result = D.Result
AND T.TaskID = D.TaskID
GO

ALTER TABLE [dbo].[TaskResult] ENABLE TRIGGER [trgUpdTaskResult]
GO



--#####################################################################################################################
--######  TaskModule  #################################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaskModule](
	[TaskID] [int] NOT NULL,
	[DisplayText] [nvarchar](400) NOT NULL,
	[URI] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_TaskModule] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC,
	[DisplayText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaskModule] ADD  CONSTRAINT [DF_TaskModule_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[TaskModule] ADD  CONSTRAINT [DF_TaskModule_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[TaskModule] ADD  CONSTRAINT [DF_TaskModule_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[TaskModule]  WITH CHECK ADD  CONSTRAINT [FK_TaskModule_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([TaskID])
GO

ALTER TABLE [dbo].[TaskModule] CHECK CONSTRAINT [FK_TaskModule_Task]
GO

GRANT SELECT ON [dbo].[TaskModule] TO [User]
GO
GRANT INSERT ON [dbo].[TaskModule] TO [CollectionManager]
GO
GRANT UPDATE ON [dbo].[TaskModule] TO [CollectionManager]
GO
GRANT DELETE ON [dbo].[TaskModule] TO [CollectionManager]
GO

--#####################################################################################################################
--######  TaskModule_log  #############################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaskModule_log](
	[TaskID] [int] NULL,
	[DisplayText] [nvarchar](400) NULL,
	[URI] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TaskModule_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaskModule_log] ADD  CONSTRAINT [DF_TaskModule_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[TaskModule_log] ADD  CONSTRAINT [DF_TaskModule_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[TaskModule_log] ADD  CONSTRAINT [DF_TaskModule_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID]())) FOR [LogUser]
GO

GRANT SELECT ON [dbo].[TaskModule_log] TO [CollectionManager]
GO
GRANT INSERT ON [dbo].[TaskModule_log] TO [CollectionManager]
GO

--#####################################################################################################################
--######  trgDelTaskModule  ###########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelTaskModule] ON [dbo].[TaskModule] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TaskModule_Log (Description, DisplayText, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RowGUID, TaskID, URI, LogUser,  LogState) 
SELECT D.Description, D.DisplayText, D.LogCreatedBy, D.LogCreatedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.Notes, D.RowGUID, D.TaskID, D.URI, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D 
GO

ALTER TABLE [dbo].[TaskModule] ENABLE TRIGGER [trgDelTaskModule]
GO



--#####################################################################################################################
--######  trgUpdTaskModule  ###########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdTaskModule] ON [dbo].[TaskModule] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TaskModule_Log (Description, DisplayText, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RowGUID, TaskID, URI, LogUser,  LogState) 
SELECT D.Description, D.DisplayText, D.LogCreatedBy, D.LogCreatedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.Notes, D.RowGUID, D.TaskID, D.URI, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM TaskModule T, deleted D where 1 = 1 
AND T.DisplayText = D.DisplayText
AND T.TaskID = D.TaskID
GO

ALTER TABLE [dbo].[TaskModule] ENABLE TRIGGER [trgUpdTaskModule]
GO



--#####################################################################################################################
--######  CollectionTask  #############################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionTask](
	[CollectionTaskID] [int] IDENTITY(1,1) NOT NULL,
	[CollectionTaskParentID] [int] NULL,
	[CollectionID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[DisplayOrder] [int] NULL,
	[DisplayText] [nvarchar](400) NULL,
	[CollectionSpecimenID] [int] NULL,
	[SpecimenPartID] [int] NULL,
	[TransactionID] [int] NULL,
	[ModuleUri] [varchar](500) NULL,
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
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CollectionTask] PRIMARY KEY CLUSTERED 
(
	[CollectionTaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionTask] ADD  CONSTRAINT [DF_CollectionTask_DisplayOrder]  DEFAULT ((1)) FOR [DisplayOrder]
GO

ALTER TABLE [dbo].[CollectionTask] ADD  CONSTRAINT [DF_CollectionTask_LogInsertedBy]  DEFAULT ([dbo].[UserID]()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[CollectionTask] ADD  CONSTRAINT [DF_CollectionTask_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[CollectionTask] ADD  CONSTRAINT [DF_CollectionTask_LogUpdatedBy]  DEFAULT ([dbo].[UserID]()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[CollectionTask] ADD  CONSTRAINT [DF_CollectionTask_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionTask] ADD  CONSTRAINT [DF_CollectionTask_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[CollectionTask]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTask_Collection] FOREIGN KEY([CollectionID])
REFERENCES [dbo].[Collection] ([CollectionID])
GO

ALTER TABLE [dbo].[CollectionTask] CHECK CONSTRAINT [FK_CollectionTask_Collection]
GO

ALTER TABLE [dbo].[CollectionTask]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTask_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
GO

ALTER TABLE [dbo].[CollectionTask] CHECK CONSTRAINT [FK_CollectionTask_CollectionSpecimenPart]
GO

ALTER TABLE [dbo].[CollectionTask]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTask_CollectionTask] FOREIGN KEY([CollectionTaskParentID])
REFERENCES [dbo].[CollectionTask] ([CollectionTaskID])
GO

ALTER TABLE [dbo].[CollectionTask] CHECK CONSTRAINT [FK_CollectionTask_CollectionTask]
GO

ALTER TABLE [dbo].[CollectionTask]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTask_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([TaskID])
GO

ALTER TABLE [dbo].[CollectionTask] CHECK CONSTRAINT [FK_CollectionTask_Task]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK of the table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'CollectionTaskID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relation to PK for hierarchy within the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'CollectionTaskParentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relation to table Collection. Every ColletionTask needs a relation to a collection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'CollectionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relation to table Task where details for the collection task are defined' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'TaskID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Display order e.g. in a report. Data with value 0 will not be included' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The display text of the module related data as shown e.g. in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the CollectionSpecimenID of CollectionSpecimenPart (= foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID of the part of the collection specimen the task is related to. ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'SpecimenPartID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URL of module related data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'ModuleUri'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The start date and or time of the collection tasks. The type is defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'TaskStart'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The end date and or time of the collection tasks. The type is defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'TaskEnd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text result either taken from a list or entered. The type is defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the collection tasks. The type is defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'URI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The numeric value of the collection tasks. The type is defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'NumberValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The boolean of the collection tasks. The type is defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'BoolValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the metric e.g. imported from a time series database like Prometheus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'MetricDescription'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The source of the metric e.g. the PromQL statement for the import from a timeseries database Prometheus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'MetricSource'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The unit of the metric, e.g. °C for temperature' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'MetricUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the responsible person or institution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'ResponsibleAgent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or institution responsible for the determination (= foreign key) as stored in the module DiversityAgents.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description of the collection tasks. The type is defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The notes of the collection tasks. The type is defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A task for a collection. Details are defined in table Task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTask'
GO

GRANT SELECT ON [dbo].[CollectionTask] TO [User]
GO
GRANT INSERT ON [dbo].[CollectionTask] TO [CollectionManager]
GO
GRANT UPDATE ON [dbo].[CollectionTask] TO [CollectionManager]
GO
GRANT DELETE ON [dbo].[CollectionTask] TO [CollectionManager]
GO

--#####################################################################################################################
--######  CollectionTask_log  #########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionTask_log](
	[CollectionTaskID] [int] NULL,
	[CollectionTaskParentID] [int] NULL,
	[CollectionID] [int] NULL,
	[TaskID] [int] NULL,
	[DisplayOrder] [int] NULL,
	[DisplayText] [nvarchar](400) NULL,
	[SpecimenPartID] [int] NULL,
	[TransactionID] [int] NULL,
	[ModuleUri] [varchar](500) NULL,
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
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionTask_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionTask_log] ADD  CONSTRAINT [DF_CollectionTask_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionTask_log] ADD  CONSTRAINT [DF_CollectionTask_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionTask_log] ADD  CONSTRAINT [DF_CollectionTask_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID]())) FOR [LogUser]
GO


GRANT SELECT ON [dbo].[CollectionTask_log] TO [CollectionManager]
GO
GRANT INSERT ON [dbo].[CollectionTask_log] TO [CollectionManager]
GO

--#####################################################################################################################
--######  trgDelCollectionTask  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionTask] ON [dbo].[CollectionTask] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTask_Log (BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, SpecimenPartID, TransactionID,
	CollectionID, CollectionTaskID, CollectionTaskParentID, Description, DisplayOrder, DisplayText, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, ModuleUri, Notes, NumberValue, Result, RowGUID, TaskEnd, TaskID, TaskStart, URI, LogUser,  LogState) 
SELECT D.BoolValue, D.MetricDescription, D.MetricSource, D.MetricUnit, D.ResponsibleAgent, D.ResponsibleAgentURI, D.SpecimenPartID, D.TransactionID, 
	D.CollectionID, D.CollectionTaskID, D.CollectionTaskParentID, D.Description, D.DisplayOrder, D.DisplayText, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.ModuleUri, D.Notes, D.NumberValue, D.Result, D.RowGUID, D.TaskEnd, D.TaskID, D.TaskStart, D.URI, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D
GO

ALTER TABLE [dbo].[CollectionTask] ENABLE TRIGGER [trgDelCollectionTask]
GO


--#####################################################################################################################
--######  trgInsCollectionTask  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgInsCollectionTask] ON [dbo].[CollectionTask] 
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
      ,case when c.[CollectionID] is null then i.CollectionID else c.CollectionID end
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

ALTER TABLE [dbo].[CollectionTask] ENABLE TRIGGER [trgInsCollectionTask]
GO


ALTER TABLE [dbo].[CollectionTask] ENABLE TRIGGER [trgInsCollectionTask]
GO


--#####################################################################################################################
--######  trgUpdCollectionTask  #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionTask] ON [dbo].[CollectionTask] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTask_Log (BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, SpecimenPartID, TransactionID, CollectionID, CollectionTaskID, CollectionTaskParentID, Description, DisplayOrder, DisplayText, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, ModuleUri, Notes, NumberValue, Result, RowGUID, TaskEnd, TaskID, TaskStart, URI, LogUser,  LogState) 
SELECT D.BoolValue, D.MetricDescription, D.MetricSource, D.MetricUnit, D.ResponsibleAgent, D.ResponsibleAgentURI, D.SpecimenPartID, D.TransactionID, D.CollectionID, D.CollectionTaskID, D.CollectionTaskParentID, D.Description, D.DisplayOrder, D.DisplayText, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.ModuleUri, D.Notes, D.NumberValue, D.Result, D.RowGUID, D.TaskEnd, D.TaskID, D.TaskStart, D.URI, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM CollectionTask T, deleted D where 1 = 1 
AND T.CollectionTaskID = D.CollectionTaskID
GO

ALTER TABLE [dbo].[CollectionTask] ENABLE TRIGGER [trgUpdCollectionTask]
GO


--#####################################################################################################################
--######   CollectionTaskChildNodes ###################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionTaskChildNodes] (@ID int)  
RETURNS @ItemList TABLE ([CollectionTaskID] [int] Primary key ,
	[CollectionTaskParentID] [int] NULL ,
	[CollectionID] [int] NULL ,
	[TaskID] [int] NULL ,
	[DisplayOrder] [int] NULL ,
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
	[Notes] [nvarchar](max) NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 20.10.2021
Test
select * from dbo.CollectionTaskChildNodes(1)
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE ([CollectionTaskID] [int] Primary key ,
	[CollectionTaskParentID] [int] NULL ,
	[CollectionID] [int] NULL ,
	[TaskID] [int] NULL ,
	[DisplayOrder] [int] NULL ,
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
	[Notes] [nvarchar](max) NULL)
INSERT @TempItem (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes) 
	SELECT CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes
	FROM CollectionTask WHERE CollectionTaskParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, CollectionTask C where C.CollectionTaskParentID = T.CollectionTaskID and C.CollectionTaskID not in (select CollectionTaskID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes)
		select C.CollectionTaskID, C.CollectionTaskParentID, C.CollectionID, C.TaskID, C.DisplayOrder, C.DisplayText, C.ModuleUri, C.CollectionSpecimenID, C.SpecimenPartID, C.TransactionID, C.TaskStart, C.TaskEnd, C.Result, C.URI, C.NumberValue, C.BoolValue, C.MetricDescription, C.MetricSource, C.MetricUnit, C.ResponsibleAgent, C.ResponsibleAgentURI, C.Description, C.Notes
		from @TempItem T, CollectionTask C where C.CollectionTaskParentID = T.CollectionTaskID and C.CollectionTaskID not in (select CollectionTaskID from @TempItem)
		set @i = (select count(*) from @TempItem T, CollectionTask C where C.CollectionTaskParentID = T.CollectionTaskID and C.CollectionTaskID not in (select CollectionTaskID from @TempItem))
	end

 INSERT @ItemList (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes) 
   SELECT distinct CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes
   FROM @TempItem ORDER BY CollectionID
   RETURN
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a result set that lists all children of a collection task within a hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionTaskChildNodes'
GO


GRANT SELECT ON [dbo].[CollectionTaskChildNodes] TO [User]
GO

--#####################################################################################################################
--######   CollectionTaskCollectionHierarchyAll #######################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionTaskCollectionHierarchyAll] ()  
RETURNS @TaskList TABLE ([CollectionTaskID] [int] Primary key ,
	[CollectionTaskParentID] [int] NULL ,
	[CollectionID] [int] NULL ,
	[TaskID] [int] NULL ,
	[DisplayText] [nvarchar] (400) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
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
	[HierarchyDisplayText] [nvarchar] (900) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the CollectionTask items related to the given CollectionTask.
MW 02.01.2021
TEST:
SELECT * FROM DBO.CollectionTaskCollectionHierarchyAll()
*/
AS
BEGIN

-- Separator for Tasks
declare @S nchar(3);
set @S = ([dbo].[TaskHierarchySeparator] ());
declare @C nchar(3);
set @C = (select dbo.TaskCollectionHierarchySeparator())

INSERT @TaskList (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayText, DisplayOrder, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, 
NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes, HierarchyDisplayText)
SELECT DISTINCT CollectionTaskID, CollectionTaskParentID, CollectionID, C.TaskID, C.DisplayText, C.DisplayOrder, C.ModuleUri, C.CollectionSpecimenID, C.SpecimenPartID, C.TransactionID, C.TaskStart, C.TaskEnd, C.Result, C.URI, 
C.NumberValue, C.BoolValue, C.MetricDescription, C.MetricSource, C.MetricUnit, C.ResponsibleAgent, C.ResponsibleAgentURI, C.Description, C.Notes, T.DisplayText
FROM CollectionTask C INNER JOIN Task T ON C.TaskID = T.TaskID
WHERE C.CollectionTaskParentID IS NULL
declare @i int
set @i = (select count(*) from CollectionTask where CollectionTaskID not IN (select CollectionTaskID from  @TaskList))
while (@i > 0)
	begin
	INSERT @TaskList (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayText, DisplayOrder, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, 
	NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes, HierarchyDisplayText)
	SELECT DISTINCT C.CollectionTaskID, C.CollectionTaskParentID, C.CollectionID, C.TaskID, C.DisplayText, C.DisplayOrder, C.ModuleUri, C.CollectionSpecimenID, C.SpecimenPartID, C.TransactionID, C.TaskStart, C.TaskEnd, C.Result, C.URI, 
	C.NumberValue, C.BoolValue, C.MetricDescription, C.MetricSource, C.MetricUnit, C.ResponsibleAgent, C.ResponsibleAgentURI, C.Description,  C.Notes, L.HierarchyDisplayText + @S + T.DisplayText
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
+ case when C.Result is null or rtrim(C.Result) = '' then '' else  ' : ' + C.Result end  
from @TaskList T inner join CollectionTask C on T.CollectionTaskID = C.CollectionTaskID

-- adding the display text if different from task
update L set L.HierarchyDisplayText = L.HierarchyDisplayText + ' : ' + rtrim(L.DisplayText)  
from @TaskList L inner join Task T on T.TaskID = L.TaskID and T.DisplayText <> L.DisplayText and rtrim(T.DisplayText) <> ''
and (L.Result is null or rtrim(L.Result) = '')

-- adding the collection as leading part
update L set L.HierarchyDisplayText = C.DisplayText + @C + L.HierarchyDisplayText 
from @TaskList L inner join [dbo].[CollectionHierarchyAll]() AS C on C.CollectionID = L.CollectionID 

   RETURN
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a result set that lists all the collection tasks including a display text with the related collection as leading part' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionTaskCollectionHierarchyAll'
GO

GRANT SELECT ON [dbo].[CollectionTaskCollectionHierarchyAll] TO [User]
GO



--#####################################################################################################################
--######   CollectionTaskHierarchy  ###################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionTaskHierarchy] (@ID int)  
RETURNS @ItemList TABLE ([CollectionTaskID] [int] Primary key ,
	[CollectionTaskParentID] [int] NULL ,
	[CollectionID] [int] NULL ,
	[TaskID] [int] NULL ,
	[DisplayOrder] [int] NULL ,
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
	[Notes] [nvarchar](max) NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 03.12.2018
Test
select * from dbo.CollectionTaskHierarchy(1)
*/
AS
BEGIN
declare @TopID int
declare @i int
set @TopID = (select CollectionTaskParentID from CollectionTask where CollectionTaskID = @ID) 
set @i = (select count(*) from CollectionTask where CollectionTaskID = @ID)
if (@TopID is null )
	set @TopID =  @ID
else	
	begin
	while (@i > 0)
		begin
		set @ID = (select CollectionTaskParentID from CollectionTask where CollectionTaskID = @ID and not CollectionTaskParentID is null) 
		set @i = (select count(*) from CollectionTask where CollectionTaskID = @ID and not CollectionTaskParentID is null)
		end
	set @TopID = @ID
	end
INSERT @ItemList
SELECT DISTINCT CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes
FROM CollectionTask
WHERE CollectionTask.CollectionTaskID = @TopID
INSERT @ItemList
SELECT CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes
FROM dbo.CollectionTaskChildNodes (@TopID)

   RETURN
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a result set that lists all the collection tasks within a hierarchy starting at the topmost collection task related to the given collection task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionTaskHierarchy'
GO

GRANT SELECT ON [dbo].[CollectionTaskHierarchy] TO [User]
GO


--#####################################################################################################################
--######   CollectionTaskHierarchyAll  ################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionTaskHierarchyAll] ()  
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
	[CollectionHierarchyDisplayText] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
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
/*
-- setting CollectionHierarchyDisplayText
update L set L.CollectionHierarchyDisplayText = C.DisplayText + @C + Replace(L.HierarchyDisplayText, ' | ', @S) 
from @TaskList L inner join [dbo].[CollectionHierarchyAll]() AS C on C.CollectionID = L.CollectionID 

-- adding the collection if no other information is available
update L set L.HierarchyDisplayText = L.HierarchyDisplayText + @C + rtrim(C.CollectionName)  
from @TaskList L inner join Collection C on C.CollectionID = L.CollectionID 
and C.CollectionName <> ''
and (L.HierarchyDisplayText not like '%' + @C + ' %')
and (L.Result is null or rtrim(L.Result) = '')
*/
   RETURN
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'All CollectionTasks including a column displaying the hierarchy of the CollectionTask' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionTaskHierarchyAll'
GO

GRANT SELECT ON [dbo].[CollectionTaskHierarchyAll] TO [User]
GO



--#####################################################################################################################
--######   CollectionTaskParentNodes   ################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionTaskParentNodes] (@ID int)  
RETURNS @ItemList TABLE ([CollectionTaskID] [int] Primary key ,
	[CollectionTaskParentID] [int] NULL ,
	[CollectionID] [int] NULL ,
	[TaskID] [int] NULL ,
	[DisplayOrder] [int] NULL ,
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
	[Notes] [nvarchar](max) NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 03.12.2018
Test
select * from dbo.CollectionTaskParentNodes(1)
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE ([CollectionTaskID] [int] Primary key ,
	[CollectionTaskParentID] [int] NULL ,
	[CollectionID] [int] NULL ,
	[TaskID] [int] NULL ,
	[DisplayOrder] [int] NULL ,
	[DisplayText] [nvarchar] (400) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionSpecimenID] [int] NULL,
	[SpecimenPartID] [int] NULL,
	[TransactionID] [int] NULL,
	[ModuleUri] [varchar](500) NULL,
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
	[Notes] [nvarchar](max) NULL)
INSERT @TempItem (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes) 
	SELECT CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes
	FROM CollectionTask WHERE CollectionTaskID = @ID 

	set @ParentID = (select CollectionTaskParentID from @TempItem where CollectionTaskID = @ID)
	set @ID = @ParentID
	while not @ParentID is null
	begin
		insert into @TempItem (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes)
		select C.CollectionTaskID, C.CollectionTaskParentID, C.CollectionID, C.TaskID, C.DisplayOrder, C.DisplayText, C.ModuleUri, C.CollectionSpecimenID, C.SpecimenPartID, C.TransactionID, C.TaskStart, C.TaskEnd, C.Result, C.URI, C.NumberValue, C.BoolValue, C.MetricDescription, C.MetricSource, C.MetricUnit, C.ResponsibleAgent, C.ResponsibleAgentURI, C.Description, C.Notes
		from CollectionTask C where C.CollectionTaskID = @ParentID and C.CollectionTaskID not in (select CollectionTaskID from @TempItem)
		set @ParentID = (select CollectionTaskParentID from @TempItem where CollectionTaskID = @ID)
		set @ID = @ParentID
	end

 INSERT @ItemList (CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes) 
   SELECT distinct CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, ModuleUri, CollectionSpecimenID, SpecimenPartID, TransactionID, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI, Description, Notes
   FROM @TempItem ORDER BY CollectionID
   RETURN
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Parent CollectionTasks for a given CollectionTask in the hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionTaskParentNodes'
GO

GRANT SELECT ON [dbo].[CollectionTaskParentNodes] TO [User]
GO


--#####################################################################################################################
--######  CollectionTaskImage  ########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionTaskImage](
	[CollectionTaskID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[ImageType] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[Description] [xml] NULL,
	[Title] [nvarchar](500) NULL,
	[IPR] [nvarchar](500) NULL,
	[CreatorAgent] [nvarchar](500) NULL,
	[CreatorAgentURI] [varchar](255) NULL,
	[CopyrightStatement] [nvarchar](500) NULL,
	[LicenseType] [nvarchar](500) NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[LicenseHolder] [nvarchar](500) NULL,
	[LicenseHolderAgentURI] [nvarchar](500) NULL,
	[LicenseYear] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CollectionTaskImage] PRIMARY KEY CLUSTERED 
(
	[CollectionTaskID] ASC,
	[URI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionTaskImage] ADD  CONSTRAINT [DF_CollectionTaskImage_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[CollectionTaskImage] ADD  CONSTRAINT [DF__Collectio__LogIn__3434A84B]  DEFAULT ([dbo].[UserID]()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[CollectionTaskImage] ADD  CONSTRAINT [DF_CollectionTaskImage_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionTaskImage] ADD  CONSTRAINT [DF__Collectio__LogUp__361CF0BD]  DEFAULT ([dbo].[UserID]()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[CollectionTaskImage] ADD  CONSTRAINT [DF_CollectionTaskImage_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[CollectionTaskImage]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTaskImage_CollectionTask] FOREIGN KEY([CollectionTaskID])
REFERENCES [dbo].[CollectionTask] ([CollectionTaskID])
GO

ALTER TABLE [dbo].[CollectionTaskImage] CHECK CONSTRAINT [FK_CollectionTaskImage_CollectionTask]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of CollectionTask (= foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'CollectionTaskID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The complete URI address of the image. ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'URI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the image, e.g. label' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'ImageType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the CollectionTask image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'Title'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Intellectual Property Rights; the rights given to persons for their intellectual property' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'IPR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person or organization originally creating the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'CreatorAgent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'CreatorAgentURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notice on rights held in and for the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'CopyrightStatement'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of an official or legal permission to do or own a specified thing, e.g. Creative Common licenses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'LicenseType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes which should not be published e.g. on websites' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'LicenseHolder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to a module containing futher information on the person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'LicenseHolderAgentURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of license declaration' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'LicenseYear'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The display order of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the data set is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The images showing the CollectionTask' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage'
GO

GRANT SELECT ON [dbo].[CollectionTaskImage] TO [User]
GO
GRANT INSERT ON [dbo].[CollectionTaskImage] TO [CollectionManager]
GO
GRANT UPDATE ON [dbo].[CollectionTaskImage] TO [CollectionManager]
GO
GRANT DELETE ON [dbo].[CollectionTaskImage] TO [CollectionManager]
GO

--#####################################################################################################################
--######  CollectionTaskImage_log  ####################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionTaskImage_log](
	[CollectionTaskID] [int] NULL,
	[URI] [varchar](255) NULL,
	[ImageType] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[Description] [xml] NULL,
	[Title] [nvarchar](500) NULL,
	[IPR] [nvarchar](500) NULL,
	[CreatorAgent] [nvarchar](500) NULL,
	[CreatorAgentURI] [varchar](255) NULL,
	[CopyrightStatement] [nvarchar](500) NULL,
	[LicenseType] [nvarchar](500) NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[LicenseHolder] [nvarchar](500) NULL,
	[LicenseHolderAgentURI] [nvarchar](500) NULL,
	[LicenseYear] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionTaskImage_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionTaskImage_log] ADD  CONSTRAINT [DF_CollectionTaskImage_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionTaskImage_log] ADD  CONSTRAINT [DF_CollectionTaskImage_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionTaskImage_log] ADD  CONSTRAINT [DF_CollectionTaskImage_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID]())) FOR [LogUser]
GO



GRANT SELECT ON [dbo].[CollectionTaskImage_log] TO [CollectionManager]
GO
GRANT INSERT ON [dbo].[CollectionTaskImage_log] TO [CollectionManager]
GO

--#####################################################################################################################
--######  trgDelCollectionTaskImage  ##################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionTaskImage] ON [dbo].[CollectionTaskImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTaskImage_Log (CollectionTaskID, CopyrightStatement, CreatorAgent, CreatorAgentURI, DisplayOrder, DataWithholdingReason, Description, ImageType, InternalNotes, IPR, LicenseHolder, LicenseHolderAgentURI, LicenseType, LicenseYear, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RowGUID, Title, URI, LogUser,  LogState) 
SELECT D.CollectionTaskID, D.CopyrightStatement, D.CreatorAgent, D.CreatorAgentURI, D.DisplayOrder, D.DataWithholdingReason, D.Description, D.ImageType, D.InternalNotes, D.IPR, D.LicenseHolder, D.LicenseHolderAgentURI, D.LicenseType, D.LicenseYear, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.Notes, D.RowGUID, D.Title, D.URI, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D
GO

ALTER TABLE [dbo].[CollectionTaskImage] ENABLE TRIGGER [trgDelCollectionTaskImage]
GO


--#####################################################################################################################
--######  trgUpdCollectionTaskImage  ##################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionTaskImage] ON [dbo].[CollectionTaskImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTaskImage_Log (CollectionTaskID, CopyrightStatement, CreatorAgent, CreatorAgentURI, DisplayOrder, DataWithholdingReason, Description, ImageType, InternalNotes, IPR, LicenseHolder, LicenseHolderAgentURI, LicenseType, LicenseYear, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RowGUID, Title, URI, LogUser,  LogState) 
SELECT D.CollectionTaskID, D.CopyrightStatement, D.CreatorAgent, D.CreatorAgentURI, D.DisplayOrder, D.DataWithholdingReason, D.Description, D.ImageType, D.InternalNotes, D.IPR, D.LicenseHolder, D.LicenseHolderAgentURI, D.LicenseType, D.LicenseYear, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.Notes, D.RowGUID, D.Title, D.URI, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM CollectionTaskImage T, deleted D where 1 = 1 
AND T.CollectionTaskID = D.CollectionTaskID
AND T.URI = D.URI
GO

ALTER TABLE [dbo].[CollectionTaskImage] ENABLE TRIGGER [trgUpdCollectionTaskImage]
GO


--#####################################################################################################################
--######  CollectionTaskMetric  #######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionTaskMetric](
	[CollectionTaskID] [int] NOT NULL,
	[MetricDate] [datetime] NOT NULL,
	[MetricValue] [real] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CollectionTaskMetric] PRIMARY KEY CLUSTERED 
(
	[CollectionTaskID] ASC,
	[MetricDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionTaskMetric] ADD  CONSTRAINT [DF_CollectionTaskMetric_LogInsertedBy]  DEFAULT ([dbo].[UserID]()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[CollectionTaskMetric] ADD  CONSTRAINT [DF_CollectionTaskMetric_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[CollectionTaskMetric] ADD  CONSTRAINT [DF_CollectionTaskMetric_LogUpdatedBy]  DEFAULT ([dbo].[UserID]()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[CollectionTaskMetric] ADD  CONSTRAINT [DF_CollectionTaskMetric_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionTaskMetric] ADD  CONSTRAINT [DF_CollectionTaskMetric_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[CollectionTaskMetric]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTaskMetric_CollectionTask] FOREIGN KEY([CollectionTaskID])
REFERENCES [dbo].[CollectionTask] ([CollectionTaskID])
GO

ALTER TABLE [dbo].[CollectionTaskMetric] CHECK CONSTRAINT [FK_CollectionTaskMetric_CollectionTask]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of CollectionTask (= foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric', @level2type=N'COLUMN',@level2name=N'CollectionTaskID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time of the metric, part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric', @level2type=N'COLUMN',@level2name=N'MetricDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of the metric' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric', @level2type=N'COLUMN',@level2name=N'MetricValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The metric related to a collection task' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric'
GO

GRANT SELECT ON [dbo].[CollectionTaskMetric] TO [User]
GO
GRANT INSERT ON [dbo].[CollectionTaskMetric] TO [CollectionManager]
GO
GRANT UPDATE ON [dbo].[CollectionTaskMetric] TO [CollectionManager]
GO
GRANT DELETE ON [dbo].[CollectionTaskMetric] TO [CollectionManager]
GO


--#####################################################################################################################
--######  CollectionTaskMetric_log   ##################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionTaskMetric_log](
	[CollectionTaskID] [int] NULL,
	[MetricDate] [datetime] NULL,
	[MetricValue] [real] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionTaskMetric_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionTaskMetric_log] ADD  CONSTRAINT [DF_CollectionTaskMetric_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionTaskMetric_log] ADD  CONSTRAINT [DF_CollectionTaskMetric_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionTaskMetric_log] ADD  CONSTRAINT [DF_CollectionTaskMetric_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID]())) FOR [LogUser]
GO


GRANT SELECT ON [dbo].[CollectionTaskMetric_log] TO [CollectionManager]
GO
GRANT INSERT ON [dbo].[CollectionTaskMetric_log] TO [CollectionManager]
GO



--#####################################################################################################################
--######  trgDelCollectionTaskMetric  #################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionTaskMetric] ON [dbo].[CollectionTaskMetric] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.202 */ 
/*  Date: 11/10/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTaskMetric_Log (CollectionTaskID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, MetricDate, MetricValue, RowGUID, LogUser,  LogState) 
SELECT D.CollectionTaskID, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.MetricDate, D.MetricValue, D.RowGUID, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D 
GO

ALTER TABLE [dbo].[CollectionTaskMetric] ENABLE TRIGGER [trgDelCollectionTaskMetric]
GO


--#####################################################################################################################
--######  trgUpdCollectionTaskMetric  #################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionTaskMetric] ON [dbo].[CollectionTaskMetric] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.202 */ 
/*  Date: 11/10/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTaskMetric_Log (CollectionTaskID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, MetricDate, MetricValue, RowGUID, LogUser,  LogState) 
SELECT D.CollectionTaskID, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.MetricDate, D.MetricValue, D.RowGUID, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM CollectionTaskMetric T, deleted D where 1 = 1 
AND T.CollectionTaskID = D.CollectionTaskID
AND T.MetricDate = D.MetricDate
GO

ALTER TABLE [dbo].[CollectionTaskMetric] ENABLE TRIGGER [trgUpdCollectionTaskMetric]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.24'
END

GO


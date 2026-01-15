declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.56'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   CollectionHierarchyMulti - adaption of column CollectionName  ##############################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionHierarchyMulti] (@CollectionIDs varchar(255))  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)

/*
Returns a table that lists all the collections related to the given collection in the list.
MW 02.01.2006
SELECT * FROM [dbo].[CollectionHierarchyMulti] ('1 3 7') 
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
	Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection
	WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	AND CollectionID = @CollectionID
	declare @TopID int
	set @TopID = (SELECT CollectionParentID FROM Collection WHERE CollectionID = @CollectionID)
	while not @TopID is null
	begin
		INSERT @CollectionList
		SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
		Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
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
--######   CollectionLocationMulti - adaption of column CollectionName, add Level, inherit plan  ######################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionLocationMulti] (@CollectionIDs varchar(4000))  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL,
	[Level] [smallint] NULL)

/*
Returns a table that lists all the collections related to the given collection in the list.
MW 02.06.2022
SELECT * FROM dbo.[CollectionLocationMulti]('1 3 7')
SELECT * FROM dbo.[CollectionLocationMulti]('1 3 7 11177 11183')
*/

AS
BEGIN

SET @CollectionIDs = rtrim(ltrim(@CollectionIDs))
if @CollectionIDs = '' 
begin return end
else
while rtrim(@CollectionIDs) <> '' 
begin

	declare @Level [smallint]
	set @Level = 0

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
	
	INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
	Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level)

	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
	Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, @Level
	FROM Collection
	WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	AND CollectionID = @CollectionID

	declare @TopID int
	set @TopID = (SELECT LocationParentID FROM Collection WHERE CollectionID = @CollectionID)

	while not @TopID is null
	begin
		set @Level = @Level - 1

		INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
		Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level)

		SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
		Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, @Level
		FROM Collection
		WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
		AND CollectionID = @TopID

		set @TopID = (SELECT LocationParentID FROM Collection WHERE CollectionID = @TopID)
	end

	SET @CollectionIDs = ltrim(substring(@CollectionIDs, charindex(' ', @CollectionIDs), 4000))
END

set @Level = 0
declare @ParentID int;
set @ParentID = (select min(CollectionID) from @CollectionList where LocationPlan <> '');
while (not @ParentID is null)
begin
	set @Level = @Level - 1
	update c set LocationPlan = p.LocationPlan, Level = @Level from @CollectionList c inner join @CollectionList p on p.CollectionID = c.LocationParentID and p.CollectionID = @ParentID and c.LocationPlan is null
	set @ParentID = (select min(CollectionID) from @CollectionList where LocationPlan <> '' and CollectionID > @ParentID)
end

   RETURN
END
GO

--#####################################################################################################################
--######   CollectionLocationChildNodes - inherit plan & add Level   ##################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionLocationChildNodes] (@ID int)  
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL,
	[Level] [smallint] NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 03.06.2022
Test
select * from dbo.CollectionLocationChildNodes(1)
select * from dbo.CollectionLocationChildNodes(11177)
select * from dbo.CollectionLocationChildNodes(11183)
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL,
	[Level] [smallint] NULL)

	declare @LocationPlan varchar(500)
	declare @NextLocationPlan varchar(500)
	set @LocationPlan = (select LocationPlan from Collection where CollectionID = @ID)
	declare @Level [smallint]
	set @Level = -1


-- Insert locations
INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, case when LocationPlan is null then @LocationPlan else LocationPlan end as LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, @Level
	FROM Collection WHERE LocationParentID = @ID 

	declare @i int

	set @i = (select count(*) from @TempItem T, Collection C where C.LocationParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	while @i > 0
	begin
		set @Level = @Level - 1;

		-- Insert locations
		insert into @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
		Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level)
		select C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
		C.Location, case when C.LocationPlan is null then @LocationPlan else C.LocationPlan end as LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID, @Level
		from @TempItem T, Collection C where C.LocationParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem)

		set @NextLocationPlan = (select top 1 C.LocationPlan
		from @TempItem T, Collection C where C.LocationParentID = T.CollectionID and C.LocationPlan <> '' and  C.CollectionID not in (select CollectionID from @TempItem))

		if (@NextLocationPlan <> @LocationPlan) begin set @LocationPlan = @NextLocationPlan end

		-- setting the counter
		set @i = (select count(*) from @TempItem T, Collection C where C.LocationParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	end

 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
 Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level
   FROM @TempItem ORDER BY CollectionName
   RETURN
END
GO

--#####################################################################################################################
--######   CollectionLocationSuperior - inherit plan & add Level   ####################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionLocationSuperior] (@CollectionID int)  
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[LocationGeometry] geometry NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL,
	[Level] [smallint] NULL)
/*
Returns a table that lists the given and all the items superior to the given collection.
MW 02.06.2022
Test:
SELECT * FROM dbo.CollectionLocationSuperior(3)
SELECT * FROM dbo.CollectionLocationSuperior(1015)
SELECT * FROM dbo.CollectionLocationSuperior(11183)
*/
AS
BEGIN
	declare @ParentID int
	declare @i int

	declare @Level [smallint]
	set @Level = 0

-- insert the given collection
   INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID, Level)

   SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID, @Level
   FROM Collection
   WHERE Collection.CollectionID = @CollectionID

-- insert the superior collections
-- check if there is any superior collection
if (select count(*) from @CollectionList where CollectionID = @CollectionID and LocationParentID is null) = 1
	RETURN

-- getting the superiors
set @ParentID = (select MAX(LocationParentID) from @CollectionList where not LocationParentID is null AND LocationParentID NOT IN (SELECT CollectionID FROM @CollectionList)) 
set @i = 1
while (@i > 0)
begin
	set @Level = @Level + 1;

	set @ParentID = (select MAX(LocationParentID) from @CollectionList where not LocationParentID is null AND LocationParentID NOT IN (SELECT CollectionID FROM @CollectionList)) 
	INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID, Level)

	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID, @Level
	FROM Collection
	WHERE Collection.CollectionID = @ParentID
	AND Collection.CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)

	set @i = (select count(*) from @CollectionList where not LocationParentID is null AND LocationParentID NOT IN (SELECT CollectionID FROM @CollectionList))
end

declare @LocationPlan [varchar](500);
declare @LocationPlanWidth [float];
declare @LocationPlanDate [datetime];
declare @LocationGeometry geometry;
set @LocationPlan = (select LocationPlan from @CollectionList where CollectionID = @ParentID)
set @LocationPlanWidth = (select LocationPlanWidth from @CollectionList where CollectionID = @ParentID)
set @LocationPlanDate = (select LocationPlanDate from @CollectionList where CollectionID = @ParentID)
set @LocationGeometry = (select LocationGeometry from @CollectionList where CollectionID = @ParentID)
while (@ParentID <> @CollectionID)
begin
	update C set C.LocationPlan = @LocationPlan from @CollectionList C where C.LocationParentID = @ParentID and (C.LocationPlan is null or C.LocationPlan = '')
	update C set C.LocationPlanWidth = @LocationPlanWidth from @CollectionList C where C.LocationParentID = @ParentID and C.LocationPlanWidth is null
	update C set C.LocationPlanDate = @LocationPlanDate from @CollectionList C where C.LocationParentID = @ParentID and C.LocationPlanDate is null
	update C set C.LocationGeometry = @LocationGeometry from @CollectionList C where C.LocationParentID = @ParentID and C.LocationGeometry is null

	set @ParentID = (select CollectionID from @CollectionList where LocationParentID = @ParentID)
	
	set @LocationPlan = (select LocationPlan from @CollectionList where CollectionID = @ParentID)
	set @LocationPlanWidth = (select LocationPlanWidth from @CollectionList where CollectionID = @ParentID)
	set @LocationPlanDate = (select LocationPlanDate from @CollectionList where CollectionID = @ParentID)
	set @LocationGeometry = (select LocationGeometry from @CollectionList where CollectionID = @ParentID)
end

RETURN

END
GO

--#####################################################################################################################
--######   CollectionLocation - inherit plan & add Level ##############################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionLocation] (@CollectionID int)  
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL,
	[Level] [smallint] NULL)
/*
Returns a table that lists all the collection items related to the given collection.
MW 02.06.2022
SELECT * FROM dbo.[CollectionLocation](1)
SELECT * FROM dbo.[CollectionLocation](11183)
*/
AS
BEGIN
declare @LocationID int

declare @TopID int
set @TopID = (select LocationParentID from Collection where CollectionID = @CollectionID) 

declare @i int
set @i = (select count(*) from Collection where CollectionID = @CollectionID)

declare @Level [smallint]
set @Level = 0


if (@TopID is null )
	set @TopID =  @CollectionID
else	
	begin
	while (@i > 0)
		begin
		set @LocationID = (select LocationParentID from Collection 
		where CollectionID = @CollectionID and not LocationParentID is null) 
		if (@LocationID IS NULL)
		begin
			set @LocationID = (select LocationParentID from Collection 
			where CollectionID = @CollectionID and not LocationParentID is null) 
		end
		set @CollectionID = @LocationID

		set @i = (select count(*) from Collection where CollectionID = @CollectionID and not LocationParentID is null)
		end
	set @TopID = @CollectionID
	end
   INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level)

   SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, @Level
   FROM Collection
   WHERE Collection.CollectionID = @TopID

  INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level)

   SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type] , LocationParentID, Level
   FROM dbo.CollectionLocationChildNodes (@TopID)
   RETURN
END
GO

--#####################################################################################################################
--######   CollectionLocationAll - inherit plan & add Level  ##########################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[CollectionLocationAll] ()  
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL,
	[Level] [smallint] NULL)
/*
Returns a table that lists all the collections including a display text with the whole hierarchy.
MW 02.06.2022
TEST:
SELECT * FROM DBO.CollectionLocationAll()
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL,
	[Level] [smallint] NULL)

	declare @Level [smallint]
	set @Level = 0

	INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level,  DisplayText)

	SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, @Level
	, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
	FROM Collection C
	WHERE C.LocationParentID IS NULL AND (C.CollectionID IN (SELECT LocationParentID FROM Collection WHERE NOT LocationParentID IS NULL)
	OR C.Type IN ('collection', 'department', 'institution', 'location', 'room')
	)

	declare @i int
	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @Temp) AND  NOT LocationParentID IS NULL)

	declare @i2 int
	set @i2 = @i

	while (@i > 0)
		begin

		set @Level = @Level - 1;

		INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
		Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level,  DisplayText)

		SELECT DISTINCT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
		C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID, @Level, L.DisplayText + ' | ' + case when C.CollectionAcronym IS NULL OR C.CollectionAcronym = '' then C.CollectionName else C.CollectionAcronym end
		FROM Collection C, @Temp L
		WHERE C.LocationParentID = L.CollectionID
		AND C.CollectionID NOT IN (select CollectionID from  @Temp)
		set @i = (select count(*) from Collection C where CollectionID not IN (select CollectionID from  @Temp) AND  NOT LocationParentID IS NULL)
		if @i2 > 0 and @i = @i2
		begin
			INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level,  DisplayText)
			SELECT DISTINCT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
			C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], NULL, 0, C.CollectionName
			FROM Collection C, @Temp L
			WHERE C.CollectionID NOT IN (select CollectionID from  @Temp) AND  NOT C.LocationParentID IS NULL
			set @i = 0
		end
		set @i2 = @i
	end

	declare @ParentID int;
	set @ParentID = (select min(CollectionID) from @Temp where LocationPlan <> '');
	while (not @ParentID is null)
	begin
		update c set LocationPlan = p.LocationPlan from @Temp c inner join @Temp p on p.CollectionID = c.LocationParentID and p.CollectionID = @ParentID and c.LocationPlan is null
		set @ParentID = (select min(CollectionID) from @Temp where LocationPlan <> '' and CollectionID > @ParentID)
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
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level,  DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level,  DisplayText
		FROM @Temp L
		end
	else
	begin
		INSERT @CollectionList (
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level,  DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, Level,  DisplayText
		FROM @Temp L WHERE L.CollectionID IN ( SELECT CollectionID FROM [dbo].[UserCollectionList] ())
	end
   RETURN
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.57'
END

GO
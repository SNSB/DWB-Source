declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.45'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   Redesign of collection location  ###########################################################################
--#####################################################################################################################
--######   CollectionLocationChildNodes depending on column LocationParentID  #########################################
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
	[LocationParentID] [int] NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 03.06.2022
Test
select * from dbo.CollectionLocationChildNodes(1)
*/
AS
BEGIN
	declare @ParentID int
	RETURN
END
GO


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
	[LocationParentID] [int] NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 03.06.2022
Test
select * from dbo.CollectionLocationChildNodes(1)
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
	[LocationParentID] [int] NULL)

-- Insert locations
INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection WHERE LocationParentID = @ID 

-- Insert children missing a location
--INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
--	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
--	FROM Collection WHERE LocationParentID IS NULL AND CollectionParentID = @ID and CollectionID not in (select CollectionID from @TempItem)

	declare @i int
	--set @i = (select count(*) from @TempItem T, Collection C where case when C.LocationParentID is null then C.CollectionParentID else C.LocationParentID end = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	set @i = (select count(*) from @TempItem T, Collection C where C.LocationParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	while @i > 0
	begin
		-- Insert locations
		insert into @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
		Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
		select C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
		C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID
		from @TempItem T, Collection C where C.LocationParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem)

		-- Insert children missing a location
		--insert into @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
		--select C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID
		--from @TempItem T, Collection C where C.LocationParentID is null and C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem)

		-- setting the counter
		--set @i = (select count(*) from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
		set @i = (select count(*) from @TempItem T, Collection C where C.LocationParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	end

 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
 Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
   FROM @TempItem ORDER BY CollectionName
   RETURN
END
GO


--#####################################################################################################################
--######   CollectionLocation depending on column LocationParentID  ###################################################
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
	[LocationParentID] [int] NULL)
/*
Returns a table that lists all the collection items related to the given collection.
MW 02.06.2022
SELECT * FROM dbo.[CollectionLocation](1)
*/
AS
BEGIN
declare @TopID int
declare @i int
declare @LocationID int
--set @TopID = (select case when LocationParentID is null then CollectionParentID else LocationParentID end from Collection where CollectionID = @CollectionID) 
set @TopID = (select LocationParentID from Collection where CollectionID = @CollectionID) 
set @i = (select count(*) from Collection where CollectionID = @CollectionID)
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
		--set @i = (select count(*) from Collection where CollectionID = @CollectionID and not case when LocationParentID is null then CollectionParentID else LocationParentID end is null)
		set @i = (select count(*) from Collection where CollectionID = @CollectionID and not LocationParentID is null)
		end
	set @TopID = @CollectionID
	end
   INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
   --SELECT DISTINCT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
   FROM Collection
   WHERE Collection.CollectionID = @TopID
  INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
   SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type] , LocationParentID
   FROM dbo.CollectionLocationChildNodes (@TopID)
   RETURN
END
GO



--#####################################################################################################################
--######   CollectionLocationAll depending on column LocationParentID  ################################################
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
	[LocationParentID] [int] NULL)
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
	[LocationParentID] [int] NULL)

	INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID,  DisplayText)
	--SELECT DISTINCT CollectionID, case when LocationParentID is null then CollectionParentID else case when LocationParentID = CollectionID then null else LocationParentID end end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
	FROM Collection C
	WHERE C.LocationParentID IS NULL AND (C.CollectionID IN (SELECT LocationParentID FROM Collection WHERE NOT LocationParentID IS NULL)
	OR C.Type IN ('collection', 'department', 'institution', 'location', 'room')
	)
	declare @i int
	declare @i2 int
	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @Temp) AND  NOT LocationParentID IS NULL)
	set @i2 = @i
	while (@i > 0)
		begin
		INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
		Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID,  DisplayText)
		--SELECT DISTINCT C.CollectionID, case when C.LocationParentID is null then C.CollectionParentID else case when C.LocationParentID = C.CollectionID then null else C.LocationParentID end end, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
		SELECT DISTINCT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
		C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID, L.DisplayText + ' | ' + case when C.CollectionAcronym IS NULL OR C.CollectionAcronym = '' then C.CollectionName else C.CollectionAcronym end
		FROM Collection C, @Temp L
		WHERE C.LocationParentID = L.CollectionID
		AND C.CollectionID NOT IN (select CollectionID from  @Temp)
		set @i = (select count(*) from Collection C where CollectionID not IN (select CollectionID from  @Temp) AND  NOT LocationParentID IS NULL)
		if @i2 > 0 and @i = @i2
		begin
			INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID,  DisplayText)
			SELECT DISTINCT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
			C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], NULL, C.CollectionName
			FROM Collection C, @Temp L
			WHERE C.CollectionID NOT IN (select CollectionID from  @Temp) AND  NOT C.LocationParentID IS NULL
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
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID,  DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID,  DisplayText
		FROM @Temp L
		end
	else
	begin
		INSERT @CollectionList (
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID,  DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
			Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID,  DisplayText
		FROM @Temp L WHERE L.CollectionID IN ( SELECT CollectionID FROM [dbo].[UserCollectionList] ())
	end
   RETURN
END

GO




--#####################################################################################################################
--######   CollectionLocationMulti depending on column LocationParentID   #############################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionLocationMulti] (@CollectionIDs varchar(4000))  
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)

/*
Returns a table that lists all the collections related to the given collection in the list.
MW 02.06.2022
SELECT * FROM dbo.[CollectionLocationMulti]('1 3 7')
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
	
	INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
	Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
	--SELECT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
	Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection
	WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	AND CollectionID = @CollectionID
	declare @TopID int
	--set @TopID = (SELECT case when LocationParentID is null then CollectionParentID else LocationParentID end FROM Collection WHERE CollectionID = @CollectionID)
	set @TopID = (SELECT LocationParentID FROM Collection WHERE CollectionID = @CollectionID)
	while not @TopID is null
	begin
		INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
	Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
		--SELECT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
		SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
		Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
		FROM Collection
		WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
		AND CollectionID = @TopID
		--set @TopID = (SELECT case when LocationParentID is null then CollectionParentID else LocationParentID end FROM Collection WHERE CollectionID = @TopID)
		set @TopID = (SELECT LocationParentID FROM Collection WHERE CollectionID = @TopID)
	end

	SET @CollectionIDs = ltrim(substring(@CollectionIDs, charindex(' ', @CollectionIDs), 4000))
END

   RETURN
END
GO


--#####################################################################################################################
--######   CollectionLocationSuperior depending on column LocationParentID   ##########################################
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
	[LocationParentID] [int] NULL)
/*
Returns a table that lists the given and all the items superior to the given collection.
MW 02.06.2022
Test:
SELECT * FROM dbo.CollectionLocationSuperior(3)
SELECT * FROM dbo.CollectionLocationSuperior(1015)
SELECT * FROM dbo.CollectionLocationSuperior(0)
*/
AS
BEGIN
declare @ParentID int
declare @i int

-- insert the given collection
   INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID)
   --SELECT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID
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
	set @ParentID = (select MAX(LocationParentID) from @CollectionList where not LocationParentID is null AND LocationParentID NOT IN (SELECT CollectionID FROM @CollectionList)) 
	--set @ParentID = @CollectionID
	INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID)
	--SELECT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID
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
--######   grants for StableIdentifierBase and StableIdentifier if missing  ###########################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.SPECIFIC_NAME = 'StableIdentifierBase') = 1
begin
	grant exec on [dbo].[StableIdentifierBase] to [User]
end

if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.SPECIFIC_NAME = 'StableIdentifier') = 1
begin
	grant exec on [dbo].[StableIdentifier] to [User]
end

GO

--#####################################################################################################################
--######   grants for log tables if missing  ##########################################################################
--#####################################################################################################################

GRANT SELECT ON IdentificationUnitGeoAnalysis_log TO [Editor]
GO

GRANT SELECT ON Identification_log TO [Editor]
GO

GRANT SELECT ON CollectionSpecimenProcessingMethodParameter_log TO [Editor]
GO

GRANT SELECT ON CollectionEventMethod_log TO [Editor]
GO


--#####################################################################################################################
--######   Collection - new type rack  ################################################################################
--#####################################################################################################################

if (select count(*) from CollCollectionType_Enum c where c.code = 'rack') = 0
begin
INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('rack'
           ,'A rack in a collection, e.g. part of a mobile shelving system'
           ,'rack'
           ,1
           ,'location')
end

GO


--#####################################################################################################################
--######   procInsertCollectionEventCopy - include tables for Method  #################################################
--#####################################################################################################################

ALTER  PROCEDURE [dbo].[procInsertCollectionEventCopy]  	
(@CollectionEventID int output , 	
@OriginalCollectionEventID int) AS declare @count int  

-- CollectionEvent
INSERT INTO CollectionEvent
		(SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionEndDay, CollectionEndMonth, CollectionEndYear, 
		CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, 
		Notes, CountryCache, DataWithholdingReason, DataWithholdingReasonDate)
SELECT SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionEndDay, CollectionEndMonth, CollectionEndYear, 
		CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, 
		Notes, CountryCache, DataWithholdingReason, DataWithholdingReasonDate
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
INSERT INTO CollectionEventImage
	(CollectionEventID, URI, ResourceURI, ImageType, Notes)
SELECT @CollectionEventID, URI, ResourceURI, ImageType, Notes
FROM CollectionEventImage
WHERE (CollectionEventID = @OriginalCollectionEventID)

-- CollectionEventMethod
INSERT INTO CollectionEventMethod 
				(CollectionEventID, MethodID, MethodMarker)
SELECT       @CollectionEventID, MethodID, MethodMarker
FROM            CollectionEventMethod
WHERE (CollectionEventID = @OriginalCollectionEventID)

INSERT INTO CollectionEventParameterValue
            (CollectionEventID, MethodID, MethodMarker, ParameterID, Value, Notes)
SELECT      @CollectionEventID, MethodID, MethodMarker, ParameterID, Value, Notes
FROM            CollectionEventParameterValue
WHERE (CollectionEventID = @OriginalCollectionEventID)

SELECT @CollectionEventID; 

GO


--#####################################################################################################################
--######   Trigger handling deletion of data tables Annotation and ExternalIdentifier  ################################
--#####################################################################################################################
--#####################################################################################################################
--######   CollectionEvent  ###########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionEvent_RelTab] ON [dbo].[CollectionEvent] 
FOR DELETE AS 

DELETE FROM [dbo].[Annotation]
WHERE [ReferencedTable] = 'CollectionEvent'
AND [ReferencedID] = (SELECT D.CollectionEventID FROM DELETED D)

DELETE FROM [dbo].[ExternalIdentifier]
WHERE [ReferencedTable] = 'CollectionEvent'
AND [ReferencedID] = (SELECT D.CollectionEventID FROM DELETED D)

ALTER TABLE [dbo].[CollectionEvent] ENABLE TRIGGER [trgDelCollectionEvent_RelTab]
GO

--#####################################################################################################################
--######   CollectionSpecimen  ########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionSpecimen_RelTab] ON [dbo].[CollectionSpecimen] 
FOR DELETE AS 

DELETE FROM [dbo].[Annotation]
WHERE [ReferencedTable] = 'CollectionSpecimen'
AND [ReferencedID] = (SELECT D.CollectionSpecimenID FROM DELETED D)

DELETE FROM [dbo].[ExternalIdentifier]
WHERE [ReferencedTable] = 'CollectionSpecimen'
AND [ReferencedID] = (SELECT D.CollectionSpecimenID FROM DELETED D)

ALTER TABLE [dbo].[CollectionSpecimen] ENABLE TRIGGER [trgDelCollectionSpecimen_RelTab]
GO


--#####################################################################################################################
--######   IdentificationUnit  ########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelIdentificationUnit_RelTab] ON [dbo].[IdentificationUnit] 
FOR DELETE AS 

DELETE FROM [dbo].[Annotation]
WHERE [ReferencedTable] = 'IdentificationUnit'
AND [ReferencedID] = (SELECT D.IdentificationUnitID FROM DELETED D)

DELETE FROM [dbo].[ExternalIdentifier]
WHERE [ReferencedTable] = 'IdentificationUnit'
AND [ReferencedID] = (SELECT D.IdentificationUnitID FROM DELETED D)

ALTER TABLE [dbo].[IdentificationUnit] ENABLE TRIGGER [trgDelIdentificationUnit_RelTab]
GO

--#####################################################################################################################
--######   CollectionSpecimenPart  ####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionSpecimenPart_RelTab] ON [dbo].[CollectionSpecimenPart] 
FOR DELETE AS 

DELETE FROM [dbo].[Annotation]
WHERE [ReferencedTable] = 'CollectionSpecimenPart'
AND [ReferencedID] = (SELECT D.SpecimenPartID FROM DELETED D)

DELETE FROM [dbo].[ExternalIdentifier]
WHERE [ReferencedTable] = 'CollectionSpecimenPart'
AND [ReferencedID] = (SELECT D.SpecimenPartID FROM DELETED D)

ALTER TABLE [dbo].[CollectionSpecimenPart] ENABLE TRIGGER [trgDelCollectionSpecimenPart_RelTab]
GO

--#####################################################################################################################
--######   CollectionSpecimenReference  ###############################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionSpecimenReference_RelTab] ON [dbo].[CollectionSpecimenReference] 
FOR DELETE AS 

DELETE FROM [dbo].[Annotation]
WHERE [ReferencedTable] = 'CollectionSpecimenReference'
AND [ReferencedID] = (SELECT D.ReferenceID FROM DELETED D)

DELETE FROM [dbo].[ExternalIdentifier]
WHERE [ReferencedTable] = 'CollectionSpecimenReference'
AND [ReferencedID] = (SELECT D.ReferenceID FROM DELETED D)

ALTER TABLE [dbo].[CollectionSpecimenReference] ENABLE TRIGGER [trgDelCollectionSpecimenReference_RelTab]
GO

--#####################################################################################################################
--######   Transaction  ###############################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelTransaction_RelTab] ON [dbo].[Transaction] 
FOR DELETE AS 

DELETE FROM [dbo].[Annotation]
WHERE [ReferencedTable] = 'Transaction'
AND [ReferencedID] = (SELECT D.TransactionID FROM DELETED D)

DELETE FROM [dbo].[ExternalIdentifier]
WHERE [ReferencedTable] = 'Transaction'
AND [ReferencedID] = (SELECT D.TransactionID FROM DELETED D)

ALTER TABLE [dbo].[Transaction] ENABLE TRIGGER [trgDelTransaction_RelTab]
GO




--#####################################################################################################################
--######   FirstLines_4: Adaption of length of LableTitle in Table CollectionSpecimen   ###############################
--#####################################################################################################################


ALTER FUNCTION [dbo].[FirstLines_4] 
(@CollectionSpecimenIDs varchar(8000))   
RETURNS @List TABLE (
	--Specimen
	[CollectionSpecimenID] [int] Primary key, --
	[Accession_number] [nvarchar](50) NULL, --
	-- withholding
	[Data_withholding_reason] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collector] [nvarchar](255) NULL, --
	[Collectors_event_number] [nvarchar](50) NULL, --
	-- event
	[Collection_day] [tinyint] NULL, --
	[Collection_month] [tinyint] NULL, --
	[Collection_year] [smallint] NULL, --
	[Collection_date_supplement] [nvarchar](100) NULL, --
	[Collection_time] [varchar](50) NULL, --
	[Collection_time_span] [varchar](50) NULL, --
	[Country] [nvarchar](50) NULL, --
	[Locality_description] [nvarchar](max) NULL, --
	[Habitat_description] [nvarchar](max) NULL, -- 
	[Collecting_method] [nvarchar](max) NULL, --
	[Collection_event_notes] [nvarchar](max) NULL, --
	--localisation
	[Named_area] [nvarchar](255) NULL, -- 
	[NamedAreaLocation2] [nvarchar](255) NULL, --
	[Remove_link_to_gazetteer] [int] NULL,
	[Distance_to_location] [varchar](50) NULL, --
	[Direction_to_location] [varchar](50) NULL, --
	[Longitude] [nvarchar](255) NULL, --
	[Latitude] [nvarchar](255) NULL, --
	[Coordinates_accuracy] [nvarchar](50) NULL, --
	[Link_to_GoogleMaps] [int] NULL,
	[_CoordinatesLocationNotes] [nvarchar](max) NULL, --
	[Altitude_from] [nvarchar](255) NULL, --
	[Altitude_to] [nvarchar](255) NULL, --
	[Altitude_accuracy] [nvarchar](50) NULL, --
	[Notes_for_Altitude] [nvarchar](max) NULL, --
	[MTB] [nvarchar](255) NULL, --
	[Quadrant] [nvarchar](255) NULL, --
	[Notes_for_MTB] [nvarchar](max) NULL, --
	[Sampling_plot] [nvarchar](255) NULL, --
	[Link_to_SamplingPlots] [nvarchar](255) NULL, --
	[Remove_link_to_SamplingPlots] [int] NULL,
	[Accuracy_of_sampling_plot] [nvarchar](50) NULL, --
	[Latitude_of_sampling_plot] [real] NULL, --
	[Longitude_of_sampling_plot] [real] NULL, --
	--site properties
	[Geographic_region] [nvarchar](255) NULL, --
	[Lithostratigraphy] [nvarchar](255) NULL, --
	[Chronostratigraphy] [nvarchar](255) NULL, --
	[Biostratigraphy] [nvarchar](255) NULL, --
	--collector
	[Collectors_name] [nvarchar](255) NULL, --
	[Link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_collector] [int] NULL,
	[Collectors_number] [nvarchar](50) NULL, --
	[Notes_about_collector] [nvarchar](max) NULL, --
	--Accession etc.
	[Accession_day] [tinyint] NULL, --
	[Accession_month] [tinyint] NULL, --
	[Accession_year] [smallint] NULL, --
	[Accession_date_supplement] [nvarchar](255) NULL, --
	[Depositors_name] [nvarchar](255) NULL, --
	[Depositors_link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_Depositor] [int] NULL,
	[Depositors_accession_number] [nvarchar](50) NULL, --
	[Exsiccata_abbreviation] [nvarchar](255) NULL, --
	[Link_to_DiversityExsiccatae] [varchar](255) NULL, --
	[Remove_link_to_exsiccatae] [int] NULL,
	[Exsiccata_number] [nvarchar](50) NULL, --
	[Original_notes] [nvarchar](max) NULL, --
	[Additional_notes] [nvarchar](max) NULL, --
	[Internal_notes] [nvarchar](max) NULL, --
	[Label_title] [nvarchar](max) NULL, --
	[Label_type] [nvarchar](50) NULL, --
	[Label_transcription_state] [nvarchar](50) NULL, --
	[Label_transcription_notes] [nvarchar](255) NULL, --
	[Problems] [nvarchar](255) NULL, --
	[External_datasource] [int] NULL, --
	[External_identifier] [nvarchar](100) NULL, --

	-- unit
	[Taxonomic_group] [nvarchar](50) NULL, --
	[Relation_type] [nvarchar](50) NULL, --
	[Colonised_substrate_part] [nvarchar](255) NULL, --
	[Life_stage] [nvarchar](255) NULL, --
	[Gender] [nvarchar](50) NULL, --
	[Number_of_units] [smallint] NULL, --
	[Circumstances] [nvarchar](50) NULL, -- 
	[Order_of_taxon] [nvarchar](255) NULL, --
	[Family_of_taxon] [nvarchar](255) NULL, --
	[Identifier_of_organism] [nvarchar](50) NULL, --
	[Description_of_organism] [nvarchar](50) NULL, --
	[Only_observed] [bit] NULL, --
	[Notes_for_organism] [nvarchar](max) NULL, --
	-- identification
	[Taxonomic_name] [nvarchar](255) NULL, --
	[Link_to_DiversityTaxonNames] [varchar](255) NULL, --
	[Remove_link_for_identification] [int] NULL, 
	[Vernacular_term] [nvarchar](255) NULL, --
	[Identification_day] [tinyint] NULL, -- 
	[Identification_month] [tinyint] NULL, --
	[Identification_year] [smallint] NULL, --
	[Identification_category] [nvarchar](50) NULL, --
	[Identification_qualifier] [nvarchar](50) NULL, --
	[Type_status] [nvarchar](50) NULL, --
	[Type_notes] [nvarchar](max) NULL, --
	[Notes_for_identification] [nvarchar](max) NULL, --
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, --
	[Remove_link_for_determiner] [int] NULL,
	-- analysis
	[Analysis] [nvarchar](50) NULL, --
	[AnalysisID] [int] NULL, --
	[Analysis_number] [nvarchar](50) NULL, --
	[Analysis_result] [nvarchar](max) NULL, --
	-- 2. unit
	[Taxonomic_group_of_second_organism] [nvarchar](50) NULL, --
	[Life_stage_of_second_organism] [nvarchar](255) NULL, --
	[Gender_of_second_organism] [nvarchar](50) NULL, --
	[Number_of_units_of_second_organism] [smallint] NULL, --
	[Circumstances_of_second_organism] [nvarchar](50) NULL, -- 
	[Identifier_of_second_organism] [nvarchar](50) NULL, --
	[Description_of_second_organism] [nvarchar](50) NULL, --
	[Only_observed_of_second_organism] [bit] NULL, --
	[Notes_for_second_organism] [nvarchar](max) NULL, --
	-- 2. indent
	[Taxonomic_name_of_second_organism] [nvarchar](255) NULL, --
	[Link_to_DiversityTaxonNames_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_second_organism] [int] NULL,
	[Vernacular_term_of_second_organism] [nvarchar](255) NULL, --
	[Identification_day_of_second_organism] [tinyint] NULL, -- 
	[Identification_month_of_second_organism] [tinyint] NULL, --
	[Identification_year_of_second_organism] [smallint] NULL, --
	[Identification_category_of_second_organism] [nvarchar](50) NULL, --
	[Identification_qualifier_of_second_organism] [nvarchar](50) NULL, --
	[Type_status_of_second_organism] [nvarchar](50) NULL, --
	[Type_notes_of_second_organism] [nvarchar](max) NULL, --
	[Notes_for_identification_of_second_organism] [nvarchar](max) NULL, --
	[Determiner_of_second_organism] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_determiner_of_second_organism] [int] NULL,
	-- part
	[Collection] [int] NULL, --
	[Material_category] [nvarchar](50) NULL, --
	[Storage_location] [nvarchar](255) NULL, --
	[Stock] [float] NULL, --
	[Part_accession_number] [nvarchar](50) NULL, --
	[Storage_container] [nvarchar](500) NULL, --
	[Preparation_method] [nvarchar](max) NULL, --
	[Preparation_date] [datetime] NULL, --
	[Notes_for_part] [nvarchar](max) NULL, --
	--relation
	[Related_specimen_URL] [varchar](255) NULL, --
	[Related_specimen_display_text] [varchar](255) NULL, --
	[Link_to_DiversityCollection_for_relation] [varchar](255) NULL, --
	[Type_of_relation] [nvarchar](50) NULL, --
	[Related_specimen_description] [nvarchar](max) NULL, --
	[Related_specimen_notes] [nvarchar](max) NULL, --
	[Relation_is_internal] [bit] NULL, --
	-- hidden columns
	[_TransactionID] [int] NULL, --
	[_Transaction] [nvarchar](200) NULL, --
	[_CollectionEventID] [int] NULL, --
	[_IdentificationUnitID] [int] NULL, --
	[_IdentificationSequence] [smallint] NULL, --
	[_SecondUnitID] [int] NULL, --
	[_SecondSequence] [smallint] NULL, --
	[_SpecimenPartID] [int] NULL, --
	[_CoordinatesAverageLatitudeCache] [real] NULL, --
	[_CoordinatesAverageLongitudeCache] [real] NULL, --
	[_GeographicRegionPropertyURI] [varchar](255) NULL, --
	[_LithostratigraphyPropertyURI] [varchar](255) NULL, --
	[_ChronostratigraphyPropertyURI] [varchar](255) NULL, --
	[_BiostratigraphyPropertyURI] [varchar](255) NULL, --
	[_NamedAverageLatitudeCache] [real] NULL, --
	[_NamedAverageLongitudeCache] [real] NULL, --
	[_LithostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_ChronostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_BiostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_SecondUnitFamilyCache] [nvarchar](255) NULL, --
	[_SecondUnitOrderCache] [nvarchar](255) NULL, --
	[_AverageAltitudeCache] [real] NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
Adding Relations, 
Specimen: Reference, ExternalSource
Part: Container, Description
Biostratigraphy


MW 18.03.2015 
TEST: 
Select * from dbo.FirstLines_3('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select 	[Altitude_from],
	[Altitude_to],
	[Altitude_accuracy],
	[Notes_for_Altitude],
	[MTB] [nvarchar],
	[Quadrant] [nvarchar],
	[Notes_for_MTB],
	[Sampling_plot],
	[Link_to_SamplingPlots],
	[Remove_link_to_SamplingPlots]
 from dbo.FirstLines_3('3251, 3252')
 Select *
 from dbo.FirstLines_3('3251,3252')
 Select *
 from dbo.FirstLines_3('3251, 3252')
 Select *
 from dbo.FirstLines_3('3251, 3252,')
 Select *
 from dbo.FirstLines_3('3251,')
 Select *
 from dbo.FirstLines_3('3251')
*/ 
AS 
BEGIN 
declare @IDs table (ID int  Primary key)
declare @sID varchar(50)
declare @sLenth int
set @sLenth = len(@CollectionSpecimenIDs)
while @CollectionSpecimenIDs <> ''-- AND charindex(',',@CollectionSpecimenIDs) > 0
begin
	if (CHARINDEX(',', @CollectionSpecimenIDs) > 0)
		begin
		set @sID = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, 1, CHARINDEX(',', @CollectionSpecimenIDs) -1)))
		set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 1, 8000)))
		if (isnumeric(@sID) = 1)
			begin
				insert into @IDs 
				values( @sID )
			end
		end
	else
		begin
		if (isnumeric(@CollectionSpecimenIDs) = 1 AND ((select count(*) from @IDs) = 0 OR len(rtrim(ltrim(@CollectionSpecimenIDs))) >= len(@sID) OR @sLenth < 8000))
			begin
				set @sID = rtrim(ltrim(@CollectionSpecimenIDs))
				insert into @IDs 
				values( @sID )
			end
		set @CollectionSpecimenIDs = ''
		end
end

-- insert basic informations to specimen
insert into @List (CollectionSpecimenID
, Accession_number
, Data_withholding_reason
, _CollectionEventID
, Accession_day
, Accession_month
, Accession_year
, Accession_date_supplement
, Depositors_name
, Depositors_link_to_DiversityAgents
, Depositors_accession_number
, Exsiccata_abbreviation
, Link_to_DiversityExsiccatae
, Original_notes
, Additional_notes
, Internal_notes
, Label_title
, Label_type
, Label_transcription_state
, Label_transcription_notes
, Problems
, External_datasource
, External_identifier
--, Reference_title_for_specimen
--, Link_to_DiversityReferences_for_specimen
)
select S.CollectionSpecimenID
, S.AccessionNumber
, S.DataWithholdingReason
, S.CollectionEventID 
, AccessionDay
, AccessionMonth
, AccessionYear
, AccessionDateSupplement
, DepositorsName
, DepositorsAgentURI
, DepositorsAccessionNumber
, ExsiccataAbbreviation
, ExsiccataURI
, OriginalNotes
, AdditionalNotes
, InternalNotes
, LabelTitle
, LabelType
, LabelTranscriptionState
, LabelTranscriptionNotes
, Problems
, ExternalDatasourceID
, ExternalIdentifier
--, ReferenceTitle
--, ReferenceURI
from dbo.CollectionSpecimen S, dbo.CollectionSpecimenID_UserAvailable U
where S.CollectionSpecimenID in (select ID from @IDs)  
and U.CollectionSpecimenID = S.CollectionSpecimenID

-- insert information about collection event
update L
set L.Collection_day = E.CollectionDay
, L.Collection_month = E.CollectionMonth
, L.Collection_year = E.CollectionYear
, L.Collection_date_supplement = E.CollectionDateSupplement
, L.Collection_time = E.CollectionTime
, L.Collection_time_span = E.CollectionTimeSpan
, L.Country = E.CountryCache
, L.Locality_description = E.LocalityDescription
, L.Habitat_description = E.HabitatDescription
, L.Collecting_method = E.CollectingMethod
, L.Collection_event_notes = E.Notes
, L.Data_withholding_reason_for_collection_event = E.DataWithholdingReason
, L.Collectors_event_number = E.CollectorsEventNumber
from @List L,
CollectionEvent E
where L._CollectionEventID = E.CollectionEventID

-- insert gazetteer infos
update L
set L.Named_area = E.Location1
, L.NamedAreaLocation2 = E.Location2
, L.Distance_to_location = E.DistanceToLocation
, L.Direction_to_location = E.DirectionToLocation
, L._NamedAverageLatitudeCache = E.AverageLatitudeCache
, L._NamedAverageLongitudeCache = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 7

-- insert Coordinates 
update L
set L.Longitude = E.Location1
, L.Latitude = E.Location2
, L.Coordinates_accuracy = E.LocationAccuracy
, L._CoordinatesAverageLatitudeCache = E.AverageLatitudeCache
, L._CoordinatesAverageLongitudeCache = E.AverageLongitudeCache
, L._CoordinatesLocationNotes = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 8

-- Altitide
update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
, L.Notes_for_Altitude = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4

--MTB
update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 3

--Sampling Plot
update L
set L.Sampling_plot = E.Location1
, L.Link_to_SamplingPlots = E.Location2
, L.Accuracy_of_sampling_plot = E.LocationAccuracy
, L.Latitude_of_sampling_plot = E.AverageLatitudeCache
, L.Longitude_of_sampling_plot = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 13

--Geographic_region
update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 10

--Lithostratigraphy
update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 30

--Chronostratigraphy
update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 20

--Chronostratigraphy
update L
set L.Biostratigraphy = P.DisplayText
, L._BiostratigraphyPropertyURI = P.PropertyURI
, L._BiostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 60

--Collector
update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
where L.CollectionSpecimenID = A.CollectionSpecimenID
and EXISTS (SELECT CollectionSpecimenID
	FROM dbo.CollectionAgent AS Amin
	GROUP BY CollectionSpecimenID
	HAVING (A.CollectionSpecimenID = Amin.CollectionSpecimenID) 
	AND (MIN(Amin.CollectorsSequence) = A.CollectorsSequence))
update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
where L.CollectionSpecimenID = A.CollectionSpecimenID
and L.Collectors_name is null
and A.CollectorsSequence is null
and EXISTS (SELECT CollectionSpecimenID
	FROM dbo.CollectionAgent AS Amin
	GROUP BY CollectionSpecimenID
	HAVING (A.CollectionSpecimenID = Amin.CollectionSpecimenID) 
	AND (MIN(Amin.LogCreatedWhen) = A.LogCreatedWhen))

-- getting the units
declare @AllUnitIDs table (UnitID int  Primary key, ID int, DisplayOrder smallint, RelatedUnitID int)
declare @UnitIDs table (UnitID int  Primary key, ID int, DisplayOrder smallint, RelatedUnitID int)
insert into @AllUnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
select U.IdentificationUnitID, U.CollectionSpecimenID, U.DisplayOrder, U.RelatedUnitID
from IdentificationUnit as U, @IDs as IDs
where DisplayOrder > 0
and IDs.ID = U.CollectionSpecimenID 
insert into @UnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
select U.UnitID, U.ID, U.DisplayOrder, U.RelatedUnitID
from @AllUnitIDs as U,  @AllUnitIDs as M
where U.ID = M.ID
group by M.ID, U.DisplayOrder, U.ID, U.RelatedUnitID, U.UnitID
having U.DisplayOrder = MIN(M.DisplayOrder)

--Unit
update L
set L.Taxonomic_group = I.TaxonomicGroup
, L._IdentificationUnitID = I.IdentificationUnitID
, L.Relation_type = I.RelationType
, L.Colonised_substrate_part = I.ColonisedSubstratePart
, L.Life_stage = I.LifeStage
, L.Gender = I.Gender
, L.Number_of_units = I.NumberOfUnits
, L.Circumstances = I.Circumstances
, L.Order_of_taxon = I.OrderCache
, L.Family_of_taxon = I.FamilyCache
, L.Identifier_of_organism = I.UnitIdentifier
, L.Description_of_organism = I.UnitDescription
, L.Only_observed = I.OnlyObserved
, L.Notes_for_organism = I.Notes
, L.Exsiccata_number = I.ExsiccataNumber
, L._SecondUnitID = U1.RelatedUnitID
from @List L,
@UnitIDs U1,
dbo.IdentificationUnit I
where L.CollectionSpecimenID = U1.ID
and L.CollectionSpecimenID = I.CollectionSpecimenID
and U1.ID = I.CollectionSpecimenID
and U1.UnitID = I.IdentificationUnitID

--Identification
update L
set L._IdentificationSequence = I.IdentificationSequence
, L.Taxonomic_name = I.TaxonomicName
, L.Link_to_DiversityTaxonNames = I.NameURI
, L.Vernacular_term = I.VernacularTerm
, L.Identification_day = I.IdentificationDay
, L.Identification_month = I.IdentificationMonth
, L.Identification_year = I.IdentificationYear
, L.Identification_category = I.IdentificationCategory
, L.Identification_qualifier = I.IdentificationQualifier
, L.Type_status = I.TypeStatus
, L.Type_notes = I.TypeNotes
, L.Notes_for_identification = I.Notes
--, L.Reference_title = I.ReferenceTitle
--, L.Link_to_DiversityReferences = I.ReferenceURI
, L.Determiner = I.ResponsibleName
, L.Link_to_DiversityAgents_for_determiner = I.ResponsibleAgentURI
from @List L,
dbo.Identification I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.Identification AS Imax
	GROUP BY CollectionSpecimenID, IdentificationUnitID
	HAVING (Imax.CollectionSpecimenID = I.CollectionSpecimenID) AND (Imax.IdentificationUnitID = I.IdentificationUnitID) AND 
	(MAX(Imax.IdentificationSequence) = I.IdentificationSequence))

--Analysis
update L
set L.AnalysisID = I.AnalysisID
, L.Analysis_number = I.AnalysisNumber
, L.Analysis_result = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (MIN(Imin.AnalysisID) = I.AnalysisID) 
	AND (MIN(Imin.AnalysisNumber) = I.AnalysisNumber))

update L
set L.Analysis = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID = A.AnalysisID

-- 2. Unit
update L
set L.Taxonomic_group_of_second_organism = I.TaxonomicGroup
, L.Life_stage_of_second_organism = I.LifeStage
, L.Gender_of_second_organism = I.Gender
, L.Number_of_units_of_second_organism = I.NumberOfUnits
, L.Circumstances_of_second_organism = I.Circumstances
, L.Identifier_of_second_organism = I.UnitIdentifier
, L.Description_of_second_organism = I.UnitDescription
, L.Only_observed_of_second_organism = I.OnlyObserved
, L.Notes_for_second_organism = I.Notes
, L._SecondUnitFamilyCache = I.FamilyCache
, L._SecondUnitOrderCache = I.OrderCache
from @List L,
dbo.IdentificationUnit I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._SecondUnitID = I.IdentificationUnitID

-- 2. Ident
update L
set L._SecondSequence = I.IdentificationSequence
, L.Taxonomic_name_of_second_organism = I.TaxonomicName
, L.Link_to_DiversityTaxonNames_of_second_organism = I.NameURI
, L.Vernacular_term_of_second_organism = I.VernacularTerm
, L.Identification_day_of_second_organism = I.IdentificationDay
, L.Identification_month_of_second_organism = I.IdentificationMonth
, L.Identification_year_of_second_organism = I.IdentificationYear
, L.Identification_category_of_second_organism = I.IdentificationCategory
, L.Identification_qualifier_of_second_organism = I.IdentificationQualifier
, L.Type_status_of_second_organism = I.TypeStatus
, L.Type_notes_of_second_organism = I.TypeNotes
, L.Notes_for_identification_of_second_organism = I.Notes
--, L.Reference_title_of_second_organism = I.ReferenceTitle
--, L.Link_to_DiversityReferences_of_second_organism = I.ReferenceURI
, L.Determiner_of_second_organism = I.ResponsibleName
, L.Link_to_DiversityAgents_for_determiner_of_second_organism = I.ResponsibleAgentURI
from @List L,
dbo.Identification I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._SecondUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.Identification AS Imax
	GROUP BY CollectionSpecimenID, IdentificationUnitID
	HAVING (Imax.CollectionSpecimenID = I.CollectionSpecimenID) AND (Imax.IdentificationUnitID = I.IdentificationUnitID) AND 
	(MAX(Imax.IdentificationSequence) = I.IdentificationSequence))

-- Part
update L
set L._SpecimenPartID = P.SpecimenPartID
, L.Collection = P.CollectionID
, L.Material_category = P.MaterialCategory
, L.Storage_location = P.StorageLocation
, L.Stock = P.Stock
, L.Preparation_method = P.PreparationMethod
, L.Preparation_date = P.PreparationDate
, L.Notes_for_part = P.Notes
, L.Storage_container = P.StorageContainer
, L.Part_accession_number = P.AccessionNumber
from @List L,
dbo.CollectionSpecimenPart P
where L.CollectionSpecimenID = P.CollectionSpecimenID
and EXISTS
	(SELECT Pmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenPart AS Pmin
	GROUP BY Pmin.CollectionSpecimenID
	HAVING (Pmin.CollectionSpecimenID = P.CollectionSpecimenID) AND (MIN(Pmin.SpecimenPartID) = P.SpecimenPartID))

-- Transaction
update L
set L._TransactionID = P.TransactionID
--, L.On_loan = P.IsOnLoan
from @List L,
dbo.CollectionSpecimenTransaction P
where L.CollectionSpecimenID = P.CollectionSpecimenID
and L._SpecimenPartID = P.SpecimenPartID
and EXISTS
	(SELECT Tmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenTransaction AS Tmin
	GROUP BY Tmin.CollectionSpecimenID, Tmin.SpecimenPartID
	HAVING (Tmin.CollectionSpecimenID = P.CollectionSpecimenID) 
	AND Tmin.SpecimenPartID = P.SpecimenPartID
	AND (MIN(Tmin.TransactionID) = P.TransactionID))

update L
set L._Transaction = T.TransactionTitle
from @List L,
dbo.[Transaction] T
where L._TransactionID = T.TransactionID

--Relation
update L
set L.Related_specimen_URL = R.RelatedSpecimenURI
, L.Related_specimen_description = R.RelatedSpecimenDescription
, L.Relation_is_internal = R.IsInternalRelationCache
, L.Type_of_relation = R.RelationType
, L.Link_to_DiversityCollection_for_relation = case when R.IsInternalRelationCache = 1 then R.RelatedSpecimenURI else '' end
, L.Related_specimen_display_text = R.RelatedSpecimenDisplayText
, L.Related_specimen_notes = R.Notes
from @List L,
dbo.CollectionSpecimenRelation R
where L.CollectionSpecimenID = R.CollectionSpecimenID
and EXISTS
	(SELECT Rmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenRelation AS Rmin
	GROUP BY Rmin.CollectionSpecimenID
	HAVING (Rmin.CollectionSpecimenID = R.CollectionSpecimenID) AND (MIN(Rmin.RelatedSpecimenURI) = R.RelatedSpecimenURI))


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
RETURN '02.06.46'
END

GO


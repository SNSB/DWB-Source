declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.30'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  CollectionTaskImage: new column ObjectGeometry  #############################################################
--#####################################################################################################################

ALTER TABLE [dbo].[CollectionTaskImage] ADD [ObjectGeometry] [geometry] NULL;
ALTER TABLE [dbo].[CollectionTaskImage_log] ADD [ObjectGeometry] [geometry] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geometry of an object placed within the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskImage', @level2type=N'COLUMN',@level2name=N'ObjectGeometry'
GO



ALTER TRIGGER [dbo].[trgDelCollectionTaskImage] ON [dbo].[CollectionTaskImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTaskImage_Log (CollectionTaskID, CopyrightStatement, CreatorAgent, CreatorAgentURI, DisplayOrder, DataWithholdingReason, Description, ImageType, InternalNotes, IPR, LicenseHolder, LicenseHolderAgentURI, LicenseType, LicenseYear, ObjectGeometry, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RowGUID, Title, URI, LogUser,  LogState) 
SELECT D.CollectionTaskID, D.CopyrightStatement, D.CreatorAgent, D.CreatorAgentURI, D.DisplayOrder, D.DataWithholdingReason, D.Description, D.ImageType, D.InternalNotes, D.IPR, D.LicenseHolder, D.LicenseHolderAgentURI, D.LicenseType, D.LicenseYear, D.ObjectGeometry, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.Notes, D.RowGUID, D.Title, D.URI, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D

GO

ALTER TRIGGER [dbo].[trgUpdCollectionTaskImage] ON [dbo].[CollectionTaskImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.199 */ 
/*  Date: 8/20/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTaskImage_Log (CollectionTaskID, CopyrightStatement, CreatorAgent, CreatorAgentURI, DisplayOrder, DataWithholdingReason, Description, ImageType, InternalNotes, IPR, LicenseHolder, LicenseHolderAgentURI, LicenseType, LicenseYear, ObjectGeometry, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, RowGUID, Title, URI, LogUser,  LogState) 
SELECT D.CollectionTaskID, D.CopyrightStatement, D.CreatorAgent, D.CreatorAgentURI, D.DisplayOrder, D.DataWithholdingReason, D.Description, D.ImageType, D.InternalNotes, D.IPR, D.LicenseHolder, D.LicenseHolderAgentURI, D.LicenseType, D.LicenseYear, D.ObjectGeometry, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.Notes, D.RowGUID, D.Title, D.URI, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM CollectionTaskImage T, deleted D where 1 = 1 
AND T.CollectionTaskID = D.CollectionTaskID
AND T.URI = D.URI
GO



--#####################################################################################################################
--######  CollUnitRelationType_Enum: new entry gall inducing  #########################################################
--#####################################################################################################################

INSERT INTO [dbo].[CollUnitRelationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Gall inducing'
           ,'Growth movement or swelling growth of (pseudo-)tissue or organ of a host organism caused by an organism.'
           ,'Gall inducing'
           ,1)
GO


--#####################################################################################################################
--######  CollMaterialCategory_Enum: new entry pinned specimen  #######################################################
--#####################################################################################################################

INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('pinned specimen'
           ,'pinned specimen, e.g. insects in a insect case'
           ,'pinned specimen'
           ,1
           ,'dried specimen')
GO


--#####################################################################################################################
--######  Collection: new column LocationParentID and LocationPlanDate  ###############################################
--#####################################################################################################################

ALTER TABLE [dbo].[Collection] ADD [LocationParentID] [int] NULL;
ALTER TABLE [dbo].[Collection_log] ADD [LocationParentID] [int] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the hierarchy of the location does not match the logical hierarchy, the ID of the parent location' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Collection', @level2type=N'COLUMN',@level2name=N'LocationParentID'
GO

ALTER TABLE [dbo].[Collection]  WITH CHECK ADD  CONSTRAINT [FK_Collection_CollectionLocation] FOREIGN KEY([LocationParentID])
REFERENCES [dbo].[Collection] ([CollectionID])
GO

ALTER TABLE [dbo].[Collection] CHECK CONSTRAINT [FK_Collection_CollectionLocation]
GO


ALTER TABLE [dbo].[Collection] ADD [LocationPlanDate] [datetime] NULL;
ALTER TABLE [dbo].[Collection_log] ADD [LocationPlanDate] [datetime] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date when the plan for the collection has been created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Collection', @level2type=N'COLUMN',@level2name=N'LocationPlanDate'
GO


--#####################################################################################################################
--######  New column in trgUpdCollection   ############################################################################
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
Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationGeometry, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID , RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT D.CollectionID, D.CollectionParentID, D.CollectionName, D.CollectionAcronym, D.AdministrativeContactName, D.AdministrativeContactAgentURI, D.Description, 
D.Location, D.LocationPlan, D.LocationPlanWidth, D.LocationPlanDate, D.LocationGeometry, D.LocationHeight, D.CollectionOwner, D.DisplayOrder, D.[Type], D.LocationParentID, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy,  'U'
FROM DELETED D

GO

--#####################################################################################################################
--######  New column in trgDelCollection  #############################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollection] ON [dbo].[Collection] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 
/* saving the original dataset in the logging table */ 
INSERT INTO Collection_Log (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationGeometry, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT D.CollectionID, D.CollectionParentID, D.CollectionName, D.CollectionAcronym, D.AdministrativeContactName, D.AdministrativeContactAgentURI, D.Description, 
D.Location, D.LocationPlan, D.LocationPlanWidth, D.LocationPlanDate, D.LocationGeometry, D.LocationHeight, D.CollectionOwner, D.DisplayOrder, D.[Type], D.LocationParentID, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy,  'D'
FROM DELETED D

GO


--#####################################################################################################################
--######   CollectionChildNodes including column LocationPlanDate  ####################################################
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)
INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection WHERE CollectionParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
		select C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID
		from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem)
		set @i = (select count(*) from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	end

 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
   FROM @TempItem ORDER BY CollectionName
   RETURN
END
GO



--#####################################################################################################################
--######   CollectionHierarchy  including column LocationPlanDate   ###################################################
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)
/*
Returns a table that lists all the analysis items related to the given analysis.
MW 02.01.2006
SELECT * FROM dbo.CollectionHierarchy(1)
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
   SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
   FROM Collection
   WHERE Collection.CollectionID = @TopID
  INSERT @CollectionList
   SELECT * FROM dbo.CollectionChildNodes (@TopID)
   RETURN
END
GO


--#####################################################################################################################
--######   CollectionHierarchyAll including new column LocationPlanDate  ##############################################
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)

	INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
	SELECT DISTINCT CollectionID, case when CollectionParentID = CollectionID then null else CollectionParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
	FROM Collection C
	WHERE C.CollectionParentID IS NULL
	declare @i int
	declare @i2 int
	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @Temp))
	set @i2 = @i
	while (@i > 0)
		begin
		INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
		SELECT DISTINCT C.CollectionID, case when C.CollectionParentID = C.CollectionID then null else C.CollectionParentID end, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
		C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID, L.DisplayText + ' | ' + case when C.CollectionAcronym IS NULL OR C.CollectionAcronym = '' then C.CollectionName else C.CollectionAcronym end
		FROM Collection C, @Temp L
		WHERE C.CollectionParentID = L.CollectionID
		AND C.CollectionID NOT IN (select CollectionID from  @Temp)
		set @i = (select count(*) from Collection C where CollectionID not IN (select CollectionID from  @Temp))
		if @i2 > 0 and @i = @i2
		begin
			INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
			SELECT DISTINCT C.CollectionID, NULL, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
			C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID, C.CollectionName
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
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText
		FROM @Temp L
		end
	else
	begin
		INSERT @CollectionList (
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText
		FROM @Temp L WHERE L.CollectionID IN ( SELECT CollectionID FROM [dbo].[UserCollectionList] ())
	end
   RETURN
END

GO


--#####################################################################################################################
--######   CollectionHierarchyMulti including column LocationPlanDate   ###############################################
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
--######   CollectionHierarchySuperior including column LocationPlanDate   ############################################
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
	[LocationPlanDate] [datetime] NULL,
	[LocationHeight] [float] NULL,
	[LocationGeometry] geometry NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)
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
   SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, [Type], LocationParentID
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
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection
	WHERE Collection.CollectionID = @ParentID
	AND Collection.CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	set @i = (select count(*) from @CollectionList where not CollectionParentID is null AND CollectionParentID NOT IN (SELECT CollectionID FROM @CollectionList))
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
	update C set C.LocationPlan = @LocationPlan from @CollectionList C where C.CollectionParentID = @ParentID and (C.LocationPlan is null or C.LocationPlan = '')
	update C set C.LocationPlanWidth = @LocationPlanWidth from @CollectionList C where C.CollectionParentID = @ParentID and C.LocationPlanWidth is null
	update C set C.LocationPlanDate = @LocationPlanDate from @CollectionList C where C.CollectionParentID = @ParentID and C.LocationPlanDate is null
	update C set C.LocationGeometry = @LocationGeometry from @CollectionList C where C.CollectionParentID = @ParentID and C.LocationGeometry is null

	set @ParentID = (select CollectionID from @CollectionList where CollectionParentID = @ParentID)
	
	set @LocationPlan = (select LocationPlan from @CollectionList where CollectionID = @ParentID)
	set @LocationPlanWidth = (select LocationPlanWidth from @CollectionList where CollectionID = @ParentID)
	set @LocationPlanDate = (select LocationPlanDate from @CollectionList where CollectionID = @ParentID)
	set @LocationGeometry = (select LocationGeometry from @CollectionList where CollectionID = @ParentID)
end

RETURN
END

GO

--#####################################################################################################################
--######   CollectionLocationChildNodes depending on column LocationParentID  #########################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionLocationChildNodes] (@ID int)  
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
INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
	SELECT CollectionID, LocationParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection WHERE LocationParentID = @ID 

-- Insert children missing a location
INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection WHERE LocationParentID IS NULL AND CollectionParentID = @ID and CollectionID not in (select CollectionID from @TempItem)

	declare @i int
	set @i = (select count(*) from @TempItem T, Collection C where case when C.LocationParentID is null then C.CollectionParentID else C.LocationParentID end = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	while @i > 0
	begin
		-- Insert locations
		insert into @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
		select C.CollectionID, C.LocationParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID
		from @TempItem T, Collection C where C.LocationParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem)

		-- Insert children missing a location
		insert into @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID)
		select C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID
		from @TempItem T, Collection C where C.LocationParentID is null and C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem)

		-- setting the counter
		set @i = (select count(*) from @TempItem T, Collection C where C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @TempItem))
	end

 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
   FROM @TempItem ORDER BY CollectionName
   RETURN
END
GO

GRANT SELECT ON [dbo].[CollectionLocationChildNodes] TO [User] AS [dbo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the collection for which the hierarchy should be listed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocationChildNodes', @level2type=N'PARAMETER',@level2name=N'@ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocationChildNodes'
GO




--#####################################################################################################################
--######   CollectionLocationMulti depending on column LocationParentID   #############################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionLocationMulti] (@CollectionIDs varchar(4000))  
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
	
	INSERT @CollectionList
	SELECT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
	Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection
	WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	AND CollectionID = @CollectionID
	declare @TopID int
	set @TopID = (SELECT case when LocationParentID is null then CollectionParentID else LocationParentID end FROM Collection WHERE CollectionID = @CollectionID)
	while not @TopID is null
	begin
		INSERT @CollectionList
		SELECT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, 
		Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
		FROM Collection
		WHERE CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
		AND CollectionID = @TopID
		set @TopID = (SELECT case when LocationParentID is null then CollectionParentID else LocationParentID end FROM Collection WHERE CollectionID = @TopID)
	end

	SET @CollectionIDs = ltrim(substring(@CollectionIDs, charindex(' ', @CollectionIDs), 4000))
END

   RETURN
END
GO

GRANT SELECT ON [dbo].[CollectionLocationMulti] TO [User] AS [dbo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Space separated list of CollectionIDs with a maximal length of 4000' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocationMulti', @level2type=N'PARAMETER',@level2name=N'@CollectionIDs'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'List of collections according to the given CollectionIDs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocationMulti'
GO

--#####################################################################################################################
--######   CollectionLocation depending on column LocationParentID  ###################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionLocation] (@CollectionID int)  
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
set @TopID = (select case when LocationParentID is null then CollectionParentID else LocationParentID end from Collection where CollectionID = @CollectionID) 
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
			set @LocationID = (select CollectionParentID from Collection 
			where CollectionID = @CollectionID and not CollectionParentID is null) 
		end
		set @CollectionID = @LocationID
		set @i = (select count(*) from Collection where CollectionID = @CollectionID and not case when LocationParentID is null then CollectionParentID else LocationParentID end is null)
		end
	set @TopID = @CollectionID
	end
   INSERT @CollectionList
   SELECT DISTINCT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
   FROM Collection
   WHERE Collection.CollectionID = @TopID
  INSERT @CollectionList
   SELECT * FROM dbo.CollectionLocationChildNodes (@TopID)
   RETURN
END
GO

GRANT SELECT ON [dbo].[CollectionLocation] TO [User] AS [dbo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the collection for which the hierarchy should be listed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocation', @level2type=N'PARAMETER',@level2name=N'@CollectionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the collection items related to the given collection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocation'
GO





--#####################################################################################################################
--######   CollectionLocationAll depending on column LocationParentID  ################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionLocationAll] ()  
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

	INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
	SELECT DISTINCT CollectionID, case when LocationParentID is null then CollectionParentID else case when LocationParentID = CollectionID then null else LocationParentID end end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
	FROM Collection C
	WHERE C.CollectionParentID IS NULL
	declare @i int
	declare @i2 int
	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @Temp))
	set @i2 = @i
	while (@i > 0)
		begin
		INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
		SELECT DISTINCT C.CollectionID, case when C.LocationParentID is null then C.CollectionParentID else case when C.LocationParentID = C.CollectionID then null else C.LocationParentID end end, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
		C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID, L.DisplayText + ' | ' + case when C.CollectionAcronym IS NULL OR C.CollectionAcronym = '' then C.CollectionName else C.CollectionAcronym end
		FROM Collection C, @Temp L
		WHERE C.CollectionParentID = L.CollectionID
		AND C.CollectionID NOT IN (select CollectionID from  @Temp)
		set @i = (select count(*) from Collection C where CollectionID not IN (select CollectionID from  @Temp))
		if @i2 > 0 and @i = @i2
		begin
			INSERT @Temp (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
			SELECT DISTINCT C.CollectionID, NULL, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
			C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID, C.CollectionName
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
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText
		FROM @Temp L
		end
	else
	begin
		INSERT @CollectionList (
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText)
		SELECT DISTINCT 
			CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID, DisplayText
		FROM @Temp L WHERE L.CollectionID IN ( SELECT CollectionID FROM [dbo].[UserCollectionList] ())
	end
   RETURN
END

GO

GRANT SELECT ON [dbo].[CollectionLocationAll] TO [User] AS [dbo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the collections including a display text with the whole hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocationAll'
GO



--#####################################################################################################################
--######   CollectionLocationSuperior depending on column LocationParentID   ##########################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionLocationSuperior] (@CollectionID int)  
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
   INSERT @CollectionList
   SELECT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID
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
	SELECT CollectionID, case when LocationParentID is null then CollectionParentID else LocationParentID end, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
	Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, LocationGeometry, CollectionOwner, DisplayOrder, Type, LocationParentID
	FROM Collection
	WHERE Collection.CollectionID = @ParentID
	AND Collection.CollectionID NOT IN (SELECT CollectionID FROM @CollectionList)
	set @i = (select count(*) from @CollectionList where not CollectionParentID is null AND CollectionParentID NOT IN (SELECT CollectionID FROM @CollectionList))
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
	update C set C.LocationPlan = @LocationPlan from @CollectionList C where C.CollectionParentID = @ParentID and (C.LocationPlan is null or C.LocationPlan = '')
	update C set C.LocationPlanWidth = @LocationPlanWidth from @CollectionList C where C.CollectionParentID = @ParentID and C.LocationPlanWidth is null
	update C set C.LocationPlanDate = @LocationPlanDate from @CollectionList C where C.CollectionParentID = @ParentID and C.LocationPlanDate is null
	update C set C.LocationGeometry = @LocationGeometry from @CollectionList C where C.CollectionParentID = @ParentID and C.LocationGeometry is null

	set @ParentID = (select CollectionID from @CollectionList where CollectionParentID = @ParentID)
	
	set @LocationPlan = (select LocationPlan from @CollectionList where CollectionID = @ParentID)
	set @LocationPlanWidth = (select LocationPlanWidth from @CollectionList where CollectionID = @ParentID)
	set @LocationPlanDate = (select LocationPlanDate from @CollectionList where CollectionID = @ParentID)
	set @LocationGeometry = (select LocationGeometry from @CollectionList where CollectionID = @ParentID)
end

RETURN

END

GO

GRANT SELECT ON [dbo].[CollectionLocationSuperior] TO [User] AS [dbo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the collection for which the hierarchy should be listed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocationSuperior', @level2type=N'PARAMETER',@level2name=N'@CollectionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists the given and all the items superior to the given collection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionLocationSuperior'
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.31'
END

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.27'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   [ProjectURI]   ######################################################################################
--#####################################################################################################################


ALTER TABLE ProjectProxy ADD ProjectURI varchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectURI'
GO


--#####################################################################################################################
--######   [LocalisationSystem]   ######################################################################################
--#####################################################################################################################

UPDATE [LocalisationSystem]
   SET [DisplayText] = 'Sampling plot (DiversitySamplingPlots)'
      ,[Description] = 'A sampling plot. May be linked to the module DiversitySamplingPlots'
      ,[DisplayTextLocation1] = 'Sampling plot'
      ,[DescriptionLocation1] = 'The name of the sampling plot. A cached value if the entry is linked to the module DiversitySamplingPlots'
      ,[DisplayTextLocation2] = 'Link to module'
      ,[DescriptionLocation2] = 'The link to the module DiversitySamplingPlots'
 WHERE [LocalisationSystemID] = 13
GO


UPDATE [LocalisationSystem]
   SET [DescriptionLocation1] = 'Longitude (East-West)'
      ,[DescriptionLocation2] = 'Latitude (North-South)'
 WHERE [LocalisationSystemID] = 8
GO


UPDATE [LocalisationSystem]
   SET [DescriptionLocation1] = 'Exposition from'
      ,[DescriptionLocation2] = 'Exposition to'
 WHERE [LocalisationSystemID] = 10
 GO

UPDATE [LocalisationSystem]
   SET [DescriptionLocation1] = 'Slope from'
      ,[DescriptionLocation2] = 'Slope to'
 WHERE [LocalisationSystemID] = 11
 GO


--#####################################################################################################################
--######   [DiversityCollectionCacheDatabaseName]   ######################################################################################
--#####################################################################################################################


	
CREATE FUNCTION [dbo].[DiversityCollectionCacheDatabaseName] () RETURNS nvarchar(50) 
AS 
BEGIN 
/*
returns the name of the cache database containing the exported data for e.g. publication on the web
TEST
SELECT [dbo].[DiversityCollectionCacheDatabaseName] ()
*/

-- for all databases not linked to a cache database
-- RETURN '' remove to enable function below

declare @CacheDatabaseName nvarchar(50) 
set @CacheDatabaseName = (select Db_name())
if (select charindex('_', @CacheDatabaseName)) > 0
begin
set @CacheDatabaseName = REPLACE(@CacheDatabaseName, '_', 'Cache_')
end
if (select COUNT(*) from master.sys.databases where name = @CacheDatabaseName and name <> Db_name()) = 0
begin
set @CacheDatabaseName = (select Db_name() + 'Cache')
end
if (select COUNT(*) from master.sys.databases where name = @CacheDatabaseName and name <> Db_name()) = 0
begin
set @CacheDatabaseName = (select substring(Db_name(), 1, charindex('_', Db_name()) - 1) + 'Cache')
end
if (select COUNT(*) from master.sys.databases where name = @CacheDatabaseName and name <> Db_name()) = 0
begin
set @CacheDatabaseName = ''
end
-- set fixed names that do not fit in the procedure above here
-- set @CacheDatabaseName = 'DiversityCollectionCache'
RETURN @CacheDatabaseName
END; 



GO

GRANT EXEC ON [DiversityCollectionCacheDatabaseName] TO [User]
GO


--#####################################################################################################################
--######   CollectionUser   ######################################################################################
--#####################################################################################################################




CREATE TABLE [dbo].[CollectionUser](
	[LoginName] [nvarchar](50) NOT NULL,
	[CollectionID] [int] NOT NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_CollectionUser] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC,
	[CollectionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A login name which the user uses  for access the DivesityWorkbench, Microsoft domains, etc..' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionUser', @level2type=N'COLUMN',@level2name=N'LoginName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID for the collection for the User has access to administrate the transaction.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionUser', @level2type=N'COLUMN',@level2name=N'CollectionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Users of collections within DiversityCollection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionUser'
GO

ALTER TABLE [dbo].[CollectionUser]  WITH CHECK ADD  CONSTRAINT [FK_CollectionUser_Collection] FOREIGN KEY([CollectionID])
REFERENCES [dbo].[Collection] ([CollectionID])
GO

ALTER TABLE [dbo].[CollectionUser] CHECK CONSTRAINT [FK_CollectionUser_Collection]
GO

ALTER TABLE [dbo].[CollectionUser] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

GRANT SELECT ON [CollectionUser] TO [USER]
GO

GRANT INSERT ON [CollectionUser] TO [CollectionManager]
GO

GRANT DELETE ON [CollectionUser] TO [CollectionManager]
GO



--#####################################################################################################################
--######   [UserCollectionList]   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[UserCollectionList] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the collections a User has access to, including the child collections.
MW 20.02.2012

Test:
select * from dbo.UserCollectionList()
*/
AS
BEGIN
	DECLARE @CollectionID INT
	DECLARE @TempAdminCollectionID TABLE (CollectionID int primary key)
	DECLARE @TempCollectionID TABLE (CollectionID int primary key)
	
	-- getting the collections of the user
	IF (SELECT COUNT(*) FROM CollectionUser WHERE (LoginName = USER_NAME())) > 0
	BEGIN
		-- getting the collections of the manager
		IF (SELECT COUNT(*) FROM CollectionManager WHERE (LoginName = USER_NAME())) > 0
		BEGIN
			INSERT @TempCollectionID (CollectionID) 
				SELECT CollectionID FROM ManagerCollectionList()
		END

		-- Filling the Collections in the temp list
		INSERT @TempAdminCollectionID (CollectionID) 
		SELECT CollectionID FROM CollectionUser WHERE (LoginName = USER_NAME()) 
			
		INSERT @TempCollectionID (CollectionID) 
		SELECT CollectionID FROM CollectionUser WHERE (LoginName = USER_NAME()) 

		DECLARE HierarchyCursor  CURSOR for
		select CollectionID from @TempAdminCollectionID
		open HierarchyCursor
		FETCH next from HierarchyCursor into @CollectionID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			insert into @TempCollectionID select CollectionID 
			from dbo.CollectionChildNodes (@CollectionID) where CollectionID not in (select CollectionID from @TempCollectionID)
			FETCH NEXT FROM HierarchyCursor into @CollectionID
		END
		CLOSE HierarchyCursor
		DEALLOCATE HierarchyCursor
	END
	
	-- in nothing is found, get any collction
	IF (select COUNT(*) from @TempCollectionID) = 0
	BEGIN
		INSERT @TempCollectionID (CollectionID) 
			SELECT CollectionID FROM Collection
	END

	-- copy the child nodes into the result list
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder FROM dbo.Collection
	WHERE CollectionID in (SELECT CollectionID FROM @TempCollectionID)
	

	RETURN
END

GO

GRANT SELECT ON dbo.UserCollectionList TO [User]
GO

--#####################################################################################################################
--######   [CollectionHierarchyAll]   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[CollectionHierarchyAll] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the collections including a display text with the whole hierarchy.
MW 20.02.2012
TEST:
SELECT * FROM DBO.CollectionHierarchyAll()
*/
AS
BEGIN
	INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, DisplayText)
	SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
	, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
	FROM Collection C
	WHERE C.CollectionParentID IS NULL
	declare @i int
	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @CollectionList))
	while (@i > 0)
		begin
		INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, DisplayText)
		SELECT DISTINCT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.CollectionOwner
		, C.DisplayOrder, L.DisplayText + ' | ' + C.CollectionName
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
--######   setting the Version   ######################################################################################
--#####################################################################################################################


 ALTER FUNCTION [dbo].[VersionClient] () RETURNS nvarchar(11) AS BEGIN RETURN '03.00.05.06' END
 GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################




ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.28'
END

GO



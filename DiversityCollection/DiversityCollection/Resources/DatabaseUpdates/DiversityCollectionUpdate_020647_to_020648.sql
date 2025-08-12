declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.47'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  Granting delete for Annotation to editor  ###################################################################
--#####################################################################################################################

GRANT DELETE ON dbo.Annotation TO [Editor]
GO

GRANT UPDATE ON dbo.Annotation TO [Editor]
GO


--#####################################################################################################################
--######  Disable triggers for update of related tables Annotation and ExternalIdentifier  ############################
--#####################################################################################################################


ALTER TABLE [dbo].[CollectionEvent] DISABLE TRIGGER [trgDelCollectionEvent_RelTab]
GO

ALTER TABLE [dbo].[IdentificationUnit] DISABLE TRIGGER [trgDelIdentificationUnit_RelTab]
GO

ALTER TABLE [dbo].[CollectionSpecimen] DISABLE TRIGGER [trgDelCollectionSpecimen_RelTab]
GO

ALTER TABLE [dbo].[CollectionSpecimenPart] DISABLE TRIGGER [trgDelCollectionSpecimenPart_RelTab]
GO

ALTER TABLE [dbo].[CollectionSpecimenReference] DISABLE TRIGGER [trgDelCollectionSpecimenReference_RelTab]
GO

ALTER TABLE [dbo].[Transaction] DISABLE TRIGGER [trgDelTransaction_RelTab]
GO



--#####################################################################################################################
--######  UserCollectionList - Inclusion of missing columns  ##########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[UserCollectionList] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint],
	[Type] [nvarchar](50) NULL,
	[LocationPlan] [varchar](500) NULL,
	[LocationPlanWidth] [float] NULL,
	[LocationGeometry] [geometry] NULL,
	[LocationHeight] [float] NULL,
	[LocationParentID] [int] NULL,
	[LocationPlanDate] [datetime] NULL
)
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
	IF (SELECT COUNT(*) FROM CollectionUser WHERE (LoginName = USER_NAME())) > 0
	BEGIN
		IF (SELECT COUNT(*) FROM CollectionManager WHERE (LoginName = USER_NAME())) > 0
		BEGIN
			INSERT @TempCollectionID (CollectionID) 
				SELECT DISTINCT CollectionID FROM ManagerCollectionList()
		END
		INSERT @TempAdminCollectionID (CollectionID) 
		SELECT DISTINCT CollectionID FROM CollectionUser WHERE (LoginName = USER_NAME()) AND CollectionID NOT IN (SELECT CollectionID FROM @TempAdminCollectionID)

		INSERT @TempCollectionID (CollectionID) 
		SELECT CollectionID FROM CollectionUser WHERE (LoginName = USER_NAME())  AND CollectionID NOT IN (SELECT CollectionID FROM @TempCollectionID)

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
	IF (select COUNT(*) from @TempCollectionID) = 0
	BEGIN
		INSERT @TempCollectionID (CollectionID) 
			SELECT CollectionID FROM Collection
	END
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder 
	, Type, LocationPlan, LocationPlanWidth, LocationGeometry, LocationHeight, LocationParentID, LocationPlanDate
	FROM dbo.Collection
	WHERE CollectionID in (SELECT CollectionID FROM @TempCollectionID)
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
RETURN '02.06.48'
END

GO


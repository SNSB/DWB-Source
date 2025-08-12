declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.44'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   CollectionEventSeriesHierarchy: Add DateSupplement  ########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionEventSeriesHierarchy] (@SeriesID int)  
RETURNS @EventSeriesList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
Test
SELECT  SeriesID, SeriesParentID, Description, SeriesCode, Notes, DateStart, DateEnd  FROM dbo.CollectionEventSeriesHierarchy(-8064) 
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.EventSeriesTopID(@SeriesID) )

-- get the ID s of the child nodes
DECLARE @TempItem TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

	INSERT @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes, [Geography]) 
	SELECT * FROM dbo.EventSeriesChildNodes (@TopID)

-- copy the root node in the result list
   INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes)
   SELECT DISTINCT SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes
   FROM CollectionEventSeries
   WHERE CollectionEventSeries.SeriesID = @TopID
   AND SeriesID NOT IN (SELECT SeriesID FROM @EventSeriesList)

   -- copy the child nodes into the result list
   INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes)
   SELECT DISTINCT SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes
   FROM CollectionEventSeries
   WHERE CollectionEventSeries.SeriesID in (select SeriesID from @TempItem)
   AND SeriesID NOT IN (SELECT SeriesID FROM @EventSeriesList)
   ORDER BY DateStart
   
   -- set the geography
	UPDATE L SET [Geography] = E.[Geography]
	FROM @EventSeriesList L, CollectionEventSeries E
	WHERE E.SeriesID = L.SeriesID

   RETURN
END

GO



--#####################################################################################################################
--######   CollectionEventRegulation: Add column TransactionID  #######################################################
--#####################################################################################################################

ALTER TABLE CollectionEventRegulation ADD TransactionID int NULL;

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Refers to unique TransactionID for the table Transaction (= foreign key)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventRegulation',
@level2type=N'COLUMN', @level2name=N'TransactionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Refers to unique TransactionID for the table Transaction (= foreign key)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionEventRegulation',
@level2type=N'COLUMN', @level2name=N'TransactionID'
END CATCH;
GO

ALTER TABLE CollectionEventRegulation_log ADD TransactionID int NULL;
GO


--#####################################################################################################################
--######   Setting the relation for column TransactionID    ###########################################################
--#####################################################################################################################

ALTER TABLE [dbo].[CollectionEventRegulation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventRegulation_Transaction] FOREIGN KEY([TransactionID])
REFERENCES [dbo].[Transaction] ([TransactionID])
GO

ALTER TABLE [dbo].[CollectionEventRegulation] CHECK CONSTRAINT [FK_CollectionEventRegulation_Transaction]
GO


--#####################################################################################################################
--######   Setting the values for the new column TransactionID    #####################################################
--#####################################################################################################################

UPDATE R SET R.TransactionID = T.TransactionID
FROM CollectionEventRegulation AS R INNER JOIN
[Transaction] AS T ON R.Regulation = T.TransactionTitle AND R.TransactionID IS NULL AND 'Regulation' = T.TransactionType AND R.TransactionID IS NULL
AND T.TransactionID IN (
select min(TransactionID) from [Transaction] group by TransactionTitle having count(*) = 1
)
GO


--#####################################################################################################################
--######   trgDelCollectionEventRegulation    #########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE TRIGGER [dbo].[trgDelCollectionEventRegulation] ON [dbo].[CollectionEventRegulation] 
FOR DELETE AS 
/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventRegulation_Log (CollectionEventID, Regulation, TransactionID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.Regulation, deleted.TransactionID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  ''D''
FROM DELETED')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch
GO



--#####################################################################################################################
--######   trgUpdCollectionEventRegulation    #########################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  TRIGGER [dbo].[trgUpdCollectionEventRegulation] ON [dbo].[CollectionEventRegulation] 
FOR UPDATE AS

DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
		/* saving the original dataset in the logging table */ 
		INSERT INTO CollectionEventRegulation_Log (CollectionEventID, Regulation, TransactionID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
		SELECT deleted.CollectionEventID, deleted.Regulation, deleted.TransactionID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  ''U''
		FROM DELETED

		/* updating the logging columns */
		Update CollectionEventRegulation
		set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
		FROM CollectionEventRegulation, deleted 
		where 1 = 1 
		AND CollectionEventRegulation.CollectionEventID = deleted.CollectionEventID
		AND CollectionEventRegulation.Regulation = deleted.Regulation
	 END')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
exec sp_executesql @SQL
end catch
GO



--#####################################################################################################################
--######   Regulations: Deprecated objects in database  ###############################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Regulations e.g. concerning the collection of specimens',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Regulation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Regulations e.g. concerning the collection of specimens',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Regulation'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Part of primay key, refers to unique ID for the table Regulation (= foreign key)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'RegulationID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Part of primay key, refers to unique ID for the table Regulation (= foreign key)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',
@level2type=N'COLUMN', @level2name=N'RegulationID'
END CATCH;
GO

--#####################################################################################################################
--######   Relation between CollectionTask and Transaction   ##########################################################
--#####################################################################################################################



ALTER TABLE [dbo].[CollectionTask]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTask_Transaction] FOREIGN KEY([TransactionID])
REFERENCES [dbo].[Transaction] ([TransactionID])
GO

ALTER TABLE [dbo].[CollectionTask] CHECK CONSTRAINT [FK_CollectionTask_Transaction]
GO


--#####################################################################################################################
--######   CollectionID in table CollectionSpecimen deprecated   ######################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. ID of the collection as stored in table Collection (= foreign key, see table Collection)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimen',
@level2type=N'COLUMN', @level2name=N'CollectionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. ID of the collection as stored in table Collection (= foreign key, see table Collection)',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'CollectionSpecimen',
@level2type=N'COLUMN', @level2name=N'CollectionID'
END CATCH;
GO

ALTER TABLE [dbo].[CollectionSpecimen] DROP CONSTRAINT [FK_CollectionSpecimen_Collection]
GO



--#####################################################################################################################
--######   CollectionChildNodes - optimized   #########################################################################
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
	[DisplayOrder] [smallint]  NULL,
	[Type] [nvarchar](50) NULL,
	[LocationParentID] [int] NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 20.10.2023
*/
AS
BEGIN

INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID
	FROM Collection WHERE CollectionParentID = @ID 

declare @i int
set @i = (select count(*) from Collection A, @ItemList T where A.CollectionParentID = T.CollectionID and A.CollectionID not in (select CollectionID from @ItemList))
while @i > 0
begin
	INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, 
   Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, [Type], LocationParentID) 
	SELECT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, 
   C.Location, C.LocationPlan, C.LocationPlanWidth, C.LocationPlanDate, C.LocationHeight, C.CollectionOwner, C.DisplayOrder, C.[Type], C.LocationParentID
	FROM Collection C, @ItemList T WHERE C.CollectionParentID = T.CollectionID and C.CollectionID not in (select CollectionID from @ItemList)

	set @i = (select count(*) from Collection A, @ItemList T where A.CollectionParentID = T.CollectionID and A.CollectionID not in (select CollectionID from @ItemList))
end

   RETURN
END
GO


--#####################################################################################################################
--######   Setting the select permission on sql_expression_dependencies for the Editor to enable imports   ############
--#####################################################################################################################

GRANT SELECT ON sys.sql_expression_dependencies TO [Editor]
GO
GRANT VIEW DEFINITION ON ExternalIdentifier TO [Editor];
GO
GRANT INSERT ON [dbo].IdentifierEvent TO [Editor];
GRANT INSERT ON [dbo].IdentifierPart TO [Editor];
GRANT INSERT ON [dbo].IdentifierSpecimen TO [Editor];
GRANT INSERT ON [dbo].IdentifierTransaction TO [Editor];
GRANT INSERT ON [dbo].IdentifierUnit TO [Editor];
GO


--#####################################################################################################################
--######   EventSeriesChildNodes - Description and Notes to nvarchar(max)   ###########################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[EventSeriesChildNodes] (@ID int)  
RETURNS @ItemList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (MAX)  NULL ,
   Notes nvarchar (MAX) ,
   [Geography] geography)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 19.10.2022
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (MAX)  NULL ,
   Notes nvarchar (MAX)  ,
   [Geography] geography)

-- insert the first childs into the table
 INSERT @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes, [Geography]) 
	SELECT SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes, [Geography]
	FROM CollectionEventSeries WHERE SeriesParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, CollectionEventSeries C where C.SeriesParentID = T.SeriesID and C.SeriesID not in (select SeriesID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes, [Geography])
		select S.SeriesID, S.SeriesParentID, S.DateStart, S.DateEnd, S.DateSupplement, S.SeriesCode, S.[Description],  S.Notes, S.[Geography]
		from @TempItem T, CollectionEventSeries S where S.SeriesParentID = T.SeriesID and S.SeriesID not in (select SeriesID from @TempItem)
		set @i = (select count(*) from @TempItem T, CollectionEventSeries C where C.SeriesParentID = T.SeriesID and C.SeriesID not in (select SeriesID from @TempItem))
	end


 INSERT @ItemList (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes) 
   SELECT distinct SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes
   FROM @TempItem ORDER BY DateStart
 UPDATE L SET [Geography] = E.[Geography]
 FROM @ItemList L, CollectionEventSeries E
 WHERE E.SeriesID = L.SeriesID

   RETURN
END

GO



--#####################################################################################################################
--######   EventSeriesHierarchy - Description and Notes to nvarchar(max)   ############################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[EventSeriesHierarchy] (@SeriesID int)  
RETURNS @EventSeriesList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(MAX) NULL,
   Notes nvarchar(MAX) NULL ,
   [Geography] geography)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
Test
SELECT * FROM  dbo.EventSeriesHierarchy(-1733)
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.EventSeriesTopID(@SeriesID) )

declare @List TABLE (SeriesID int primary key,
   SeriesParentID int NULL)
   
-- inserting the start values  
	INSERT @List (SeriesID) Values(@TopID)
	INSERT @List (SeriesID, SeriesParentID) SELECT SeriesID, SeriesParentID FROM dbo.EventSeriesChildNodes (@TopID)
	
-- getting the whole hierarchy	
	set @i = (select COUNT(*) from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P))
	while @i > 0
	begin
		INSERT @List (SeriesID, SeriesParentID) 
			SELECT E.SeriesID, E.SeriesParentID from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P)
		set @i = (select COUNT(*) from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P))
	end

	INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes)
	SELECT E.SeriesID, E.SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes
	FROM CollectionEventSeries E, @List L where L.SeriesID = E.SeriesID
   
-- set the geography
	UPDATE L SET [Geography] = E.[Geography]
	FROM @EventSeriesList L, CollectionEventSeries E
	WHERE E.SeriesID = L.SeriesID

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
RETURN '02.06.45'
END

GO


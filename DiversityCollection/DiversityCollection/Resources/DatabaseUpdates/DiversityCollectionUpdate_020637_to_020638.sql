declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.37'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   CollectionLocation in case it is missing  ##################################################################
--#####################################################################################################################

if (select COUNT(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'CollectionLocation') = 0
begin

declare @SQL nvarchar(max)
	set @SQL = (select '
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
	')

	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
	exec sp_executesql @SQL
	end catch;

end
GO

GRANT SELECT ON [dbo].[CollectionLocation] TO [User] AS [dbo]


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the collection for which the hierarchy should be listed',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION',@level1name=N'CollectionLocation',
@level2type=N'PARAMETER',@level2name=N'@CollectionID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the collection for which the hierarchy should be listed',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION',@level1name=N'CollectionLocation',
@level2type=N'PARAMETER',@level2name=N'@CollectionID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collection items related to the given collection',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION',@level1name=N'CollectionLocation'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the collection items related to the given collection',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION',@level1name=N'CollectionLocation'
END CATCH;
GO



--#####################################################################################################################
--######  TransactionList_H7 in case it is missing  ##################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'TransactionList_H7') = 0
begin

	declare @SQL nvarchar(max)
	set @SQL = (select '
	CREATE VIEW [dbo].[TransactionList_H7]
	AS
	SELECT        TransactionID, ParentTransactionID, TransactionType, TransactionTitle, 
	TransactionTitle AS HierarchyDisplayText,
	ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialSource, MaterialCategory, MaterialCollectors, FromCollectionID, 
							 FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, 
							 Investigator, TransactionComment, BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DateSupplement
	FROM            dbo.[Transaction]
	WHERE        (AdministratingCollectionID IS NULL)
	UNION
	SELECT    T .TransactionID, T .ParentTransactionID, T .TransactionType, 
	T .TransactionTitle,
	case when T7.TransactionTitle is null then '''' else T7.TransactionTitle + '' | '' end +
	case when T6.TransactionTitle is null then '''' else T6.TransactionTitle + '' | '' end +
	case when T5.TransactionTitle is null then '''' else T5.TransactionTitle + '' | '' end +
	case when T4.TransactionTitle is null then '''' else T4.TransactionTitle + '' | '' end +
	case when T3.TransactionTitle is null then '''' else T3.TransactionTitle + '' | '' end + 
	case when T2.TransactionTitle is null then '''' else T2.TransactionTitle + '' | '' end + 
	case when T1.TransactionTitle is null then '''' else T1.TransactionTitle + '' | '' end +
	T .TransactionTitle 
	AS HierarchyDisplayText, 
	T .ReportingCategory, T .AdministratingCollectionID, T .MaterialDescription, T .MaterialSource, T .MaterialCategory, T .MaterialCollectors, 
							 T .FromCollectionID, T .FromTransactionPartnerName, T .FromTransactionPartnerAgentURI, T .FromTransactionNumber, T .ToCollectionID, T .ToTransactionPartnerName, T .ToTransactionPartnerAgentURI, 
							 T .ToTransactionNumber, T .ToRecipient, T .NumberOfUnits, T .Investigator, T .TransactionComment, T .BeginDate, T .AgreedEndDate, T .ActualEndDate, T .InternalNotes, T .ResponsibleName, T .ResponsibleAgentURI, 
							 T .DateSupplement
	FROM            dbo.[Transaction] AS T INNER JOIN
							 dbo.CollectionManager ON T.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
	left outer join 
	dbo.[Transaction] AS T1 ON T.ParentTransactionID = T1.TransactionID and T1.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
	left outer join 
	dbo.[Transaction] AS T2 ON T1.ParentTransactionID = T2.TransactionID and T2.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
	left outer join 
	dbo.[Transaction] AS T3 ON T2.ParentTransactionID = T3.TransactionID and T3.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
	left outer join 
	dbo.[Transaction] AS T4 ON T3.TransactionID = T4.ParentTransactionID and T4.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
	left outer join 
	dbo.[Transaction] AS T5 ON T4.TransactionID = T5.ParentTransactionID and T5.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
	left outer join 
	dbo.[Transaction] AS T6 ON T5.TransactionID = T6.ParentTransactionID and T6.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()
	left outer join 
	dbo.[Transaction] AS T7 ON T6.TransactionID = T7.ParentTransactionID and T7.AdministratingCollectionID = dbo.CollectionManager.AdministratingCollectionID and dbo.CollectionManager.LoginName = USER_NAME()        
	')

	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
	exec sp_executesql @SQL
	end catch;

end
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction missing a AdministratingCollectionID resp. those available for a CollectionManager including a hierarchy with up to 7 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList_H7'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of table Transaction missing a AdministratingCollectionID resp. those available for a CollectionManager including a hierarchy with up to 7 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList_H7'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the transaction represented by the TransactionTitle separated by "|" for up to 7 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList_H7',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Hierarchy of the transaction represented by the TransactionTitle separated by "|" for up to 7 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'TransactionList_H7',
@level2type=N'COLUMN', @level2name=N'HierarchyDisplayText'
END CATCH;
GO

GRANT SELECT ON TransactionList_H7 TO [USER]
GO


--#####################################################################################################################
--######   New Collection Type hardware for e.g. network components  ##################################################
--#####################################################################################################################
if (select count(*) from [CollCollectionType_Enum] e where e.code = 'hardware' ) = 0
begin
	INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[InternalNotes])
     VALUES
           ('hardware'
           ,'hardware'
           ,'hardware e.g. for sensor network'
           ,1
           ,'e.g. Raspi, LoRa transceiver etc. used for IPM sensor net')
end
GO



--#####################################################################################################################
--######   CacheDescription - add Columns Type and Schema  ############################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CacheDescription' and c.COLUMN_NAME = 'Type') = 0
begin
	ALTER TABLE [dbo].[CacheDescription] ADD [Type] varchar(20) NULL;
	ALTER TABLE [dbo].[CacheDescription] ADD  CONSTRAINT [DF_CacheDescription_Type]  DEFAULT ('COLUMN') FOR [Type]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'Type'
end
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CacheDescription' and c.COLUMN_NAME = 'Schema') = 0
	begin
	ALTER TABLE [dbo].[CacheDescription] ADD [Schema] varchar(100) NULL;
	ALTER TABLE [dbo].[CacheDescription] ADD  CONSTRAINT [DF_CacheDescription_Schema]  DEFAULT ('dbo') FOR [Schema]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Schema of the entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDescription', @level2type=N'COLUMN',@level2name=N'Schema'
end
GO


--#####################################################################################################################
--######   procFillCacheDescription - include System, Views, Procedures, Functions  ###################################
--#####################################################################################################################

ALTER  PROCEDURE [dbo].[procFillCacheDescription] 
AS
/*
exec dbo.procFillCacheDescription
select * from CacheDescription
*/

truncate table CacheDescription;

declare @i int;
declare @table varchar(50)
declare @column varchar(50)
declare @ID int
declare @description nvarchar(4000)

-- filling the columns
insert into CacheDescription (TableName, ColumnName, [LanguageCode])
select T.TABLE_NAME, C.COLUMN_NAME, 'System' from INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.TABLES T
where C.TABLE_NAME = T.TABLE_NAME
and T.TABLE_TYPE = 'BASE TABLE'
and T.TABLE_SCHEMA = 'dbo'
and T.TABLE_NAME not like '%_log'
and T.TABLE_NAME not like '%_Enum'

insert into CacheDescription (TableName, ColumnName)
select T.TABLE_NAME, C.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.TABLES T
where C.TABLE_NAME = T.TABLE_NAME
and T.TABLE_TYPE = 'BASE TABLE'
and T.TABLE_SCHEMA = 'dbo'
and T.TABLE_NAME not like '%_log'
and T.TABLE_NAME not like '%_Enum'

set @i = (select count(*) from CacheDescription where Description is null and [Type] = 'column')
while @i > 0
begin
	set @ID = (select min(ID) from CacheDescription where Description is null and [Type] = 'column')
	set @table = (select TableName from CacheDescription where ID = @ID)
	set @column = (select ColumnName from CacheDescription where ID = @ID)
	set @description = (SELECT max(CONVERT(nvarchar(MAX), [value]))  FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @table, 'column', @column) WHERE name =  'MS_Description')
	if @description is null begin set @description = '' end
	update CacheDescription set DisplayText = @column, Description = @description where ID = @ID
	set @i = (select count(*) from CacheDescription where Description is null and [Type] = 'column')
end


-- filling the tables
insert into CacheDescription (TableName, ColumnName, [LanguageCode], [Type])
select T.TABLE_NAME, '', 'System', 'Table' from INFORMATION_SCHEMA.TABLES T
where T.TABLE_TYPE = 'BASE TABLE'
and T.TABLE_SCHEMA = 'dbo'
and T.TABLE_NAME not like '%_log'
and T.TABLE_NAME not like '%_Enum'

insert into CacheDescription (TableName, ColumnName, [Type])
select T.TABLE_NAME, '', 'Table' from INFORMATION_SCHEMA.TABLES T
where T.TABLE_TYPE = 'BASE TABLE'
and T.TABLE_SCHEMA = 'dbo'
and T.TABLE_NAME not like '%_log'
and T.TABLE_NAME not like '%_Enum'

set @i = (select count(*) from CacheDescription where Description is null and [Type] = 'table')
while @i > 0
begin
	set @ID = (select min(ID) from CacheDescription where Description is null and [Type] = 'table')
	set @table = (select TableName from CacheDescription where ID = @ID)
	set @description = (SELECT max(CONVERT(nvarchar(MAX), [value]))  FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @table, NULL, NULL) WHERE name =  'MS_Description')
	if @description is null begin set @description = '' end
	update CacheDescription set DisplayText = @table, Description = @description where ID = @ID
	set @i = (select count(*) from CacheDescription where Description is null and [Type] = 'table')
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

-- filling the views
insert into CacheDescription (TableName, ColumnName, [LanguageCode], [Type])
select T.TABLE_NAME, '', 'System', 'VIEW' from INFORMATION_SCHEMA.TABLES T
where T.TABLE_TYPE = 'VIEW'
and T.TABLE_SCHEMA = 'dbo'
and T.TABLE_NAME not like '%_log'
and T.TABLE_NAME not like '%_Enum'

insert into CacheDescription (TableName, ColumnName, [Type])
select T.TABLE_NAME, '', 'VIEW' from INFORMATION_SCHEMA.TABLES T
where T.TABLE_TYPE = 'VIEW'
and T.TABLE_SCHEMA = 'dbo'
and T.TABLE_NAME not like '%_log'
and T.TABLE_NAME not like '%_Enum'

set @i = (select count(*) from CacheDescription where Description is null and [Type] = 'VIEW')
while @i > 0
begin
	set @ID = (select min(ID) from CacheDescription where Description is null and [Type] = 'VIEW')
	set @table = (select TableName from CacheDescription where ID = @ID)
	set @description = (SELECT max(CONVERT(nvarchar(MAX), [value]))  FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'VIEW', @table, NULL, NULL) WHERE name =  'MS_Description')
	if @description is null begin set @description = '' end
	update CacheDescription set DisplayText = @table, Description = @description where ID = @ID
	set @i = (select count(*) from CacheDescription where Description is null and [Type] = 'VIEW')
end


-- filling the FUNCTION
insert into CacheDescription (TableName, ColumnName, [LanguageCode], [Type])
select R.ROUTINE_NAME, '', 'System', R.ROUTINE_TYPE from INFORMATION_SCHEMA.ROUTINES R
where R.ROUTINE_TYPE = 'FUNCTION'


insert into CacheDescription (TableName, ColumnName, [LanguageCode], [Type])
select R.ROUTINE_NAME, '', '', R.ROUTINE_TYPE from INFORMATION_SCHEMA.ROUTINES R
where R.ROUTINE_TYPE = 'FUNCTION'

set @i = (select count(*) from CacheDescription where Description is null and [Type] in ('FUNCTION'))
while @i > 0
begin
	set @ID = (select min(ID) from CacheDescription where Description is null and [Type]  in ('FUNCTION'))
	set @table = (select TableName from CacheDescription where ID = @ID)
	set @description = (SELECT max(CONVERT(nvarchar(MAX), [value]))  FROM ::fn_listextendedproperty(N'MS_Description', N'USER', 'DBO', 'FUNCTION', @table, NULL, NULL))
	if @description is null begin set @description = '' end
	update CacheDescription set DisplayText = @table, Description = @description where ID = @ID
	set @i = (select count(*) from CacheDescription where Description is null and [Type]  in ('FUNCTION'))
end

-- filling the PROCEDURE
insert into CacheDescription (TableName, ColumnName, [LanguageCode], [Type])
select R.ROUTINE_NAME, '', 'System', R.ROUTINE_TYPE from INFORMATION_SCHEMA.ROUTINES R
where R.ROUTINE_TYPE = 'PROCEDURE'

insert into CacheDescription (TableName, ColumnName, [LanguageCode], [Type])
select R.ROUTINE_NAME, '', '', R.ROUTINE_TYPE from INFORMATION_SCHEMA.ROUTINES R
where R.ROUTINE_TYPE = 'PROCEDURE'

set @i = (select count(*) from CacheDescription where Description is null and [Type] in ('PROCEDURE'))
while @i > 0
begin
	set @ID = (select min(ID) from CacheDescription where Description is null and [Type]  in ('PROCEDURE'))
	set @table = (select TableName from CacheDescription where ID = @ID)
	set @description = (SELECT max(CONVERT(nvarchar(MAX), [value]))  FROM ::fn_listextendedproperty(N'MS_Description', N'USER', 'DBO', 'PROCEDURE', @table, NULL, NULL))
	if @description is null begin set @description = '' end
	update CacheDescription set DisplayText = @table, Description = @description where ID = @ID
	set @i = (select count(*) from CacheDescription where Description is null and [Type]  in ('PROCEDURE'))
end

GO


--#####################################################################################################################
--######   Removing extended properties that are not descriptions to ensure compatibility with SIARD   ################
--#####################################################################################################################
--Retrieal:
/*
SELECT
  'exec sys.sp_dropextendedproperty @name=N''' + p.name +''',
  @level0type=N''SCHEMA'', @level0name=N''' + SCHEMA_NAME(tbl.schema_id)
+ ''',
  @level1type=N''TABLE'', @level1name=N''' + tbl.name + ''',
  @level2type=N''COLUMN'',@level2name=N'''+clmns.name+''';GO'
  FROM
   sys.tables AS tbl
   INNER JOIN sys.all_columns AS clmns ON clmns.object_id=tbl.object_id
   INNER JOIN sys.extended_properties AS p ON p.major_id=tbl.object_id
AND p.minor_id=clmns.column_id AND p.class=1
where not  p.name like 'MS_Desc%' ;
*/

BEGIN TRY
exec sys.sp_dropextendedproperty @name=N'MS_DisplayControl',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionEventImage',    @level2type=N'COLUMN',@level2name=N'ResourceURI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_DisplayControl',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'IdentificationUnitAnalysis',    @level2type=N'COLUMN',@level2name=N'ExternalAnalysisURI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'URI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'URI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'URI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'ResourceURI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'ResourceURI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'ResourceURI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_DisplayControl',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'ResourceURI';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'IdentificationUnitID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'IdentificationUnitID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'IdentificationUnitID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'ImageType';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'ImageType';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenImage',    @level2type=N'COLUMN',@level2name=N'ImageType';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemParentID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemParentID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemParentID';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemName';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemName';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'LocalisationSystemName';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfLocalisation';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfLocalisation';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfLocalisation';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayText';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayText';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayText';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayEnable';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayEnable';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayEnable';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayOrder';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayOrder';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayOrder';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'Description';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'Description';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'Description';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayTextLocation1';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayTextLocation1';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayTextLocation1';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DescriptionLocation1';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DescriptionLocation1';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DescriptionLocation1';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayTextLocation2';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayTextLocation2';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DisplayTextLocation2';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DescriptionLocation2';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DescriptionLocation2';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'LocalisationSystem',    @level2type=N'COLUMN',@level2name=N'DescriptionLocation2';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_Regulation',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',    @level2type=N'COLUMN',@level2name=N'LogCreatedWhen';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_Regulation',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',    @level2type=N'COLUMN',@level2name=N'LogCreatedBy';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_Regulation',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',    @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_Regulation',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'CollectionSpecimenPartRegulation',    @level2type=N'COLUMN',@level2name=N'LogUpdatedBy';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'UserName';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'UserName';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'UserName';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'SQLStringIdentifier';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'SQLStringIdentifier';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'SQLStringIdentifier';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'ItemTable';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'ItemTable';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'ItemTable';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnHidden',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'SQLString';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnOrder',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'SQLString';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_ColumnWidth',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'ApplicationSearchSelectionStrings',    @level2type=N'COLUMN',@level2name=N'SQLString';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_DisplayControl',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'UserProxy',    @level2type=N'COLUMN',@level2name=N'LoginName';  
END TRY BEGIN CATCH END CATCH; BEGIN TRY 
exec sys.sp_dropextendedproperty @name=N'MS_Format',    @level0type=N'SCHEMA', @level0name=N'dbo',    @level1type=N'TABLE', @level1name=N'UserProxy',    @level2type=N'COLUMN',@level2name=N'LoginName';  
END TRY BEGIN CATCH END CATCH; 

--#####################################################################################################################
--######   Missing descriptions   #####################################################################################
--#####################################################################################################################
--#####################################################################################################################
--######   Functions   ################################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the information for the current user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CurrentUser'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the information for the current user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CurrentUser'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'retrieval of the name of the current user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CurrentUserName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'retrieval of the name of the current user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'CurrentUserName'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the ID of the current project used by the user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DefaultProjectID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the ID of the current project used by the user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DefaultProjectID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'returns the name of the cache database containing the exported data for e.g. publication on the web',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityCollectionCacheDatabaseName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'returns the name of the cache database containing the exported data for e.g. publication on the web',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityCollectionCacheDatabaseName'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the analysis items related to the given project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_AnalysisProjectList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the analysis items related to the given project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_AnalysisProjectList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the contents of the table AnalysisResult used in a project.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_AnalysisResultForProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the contents of the table AnalysisResult used in a project.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_AnalysisResultForProject'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_AnalysisResultForProject',
@level2type=N'PARAMETER', @level2name=N'@ProjectID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_AnalysisResultForProject',
@level2type=N'PARAMETER', @level2name=N'@ProjectID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the contents of the table AnalysisTaxonomicGroup used in a project including the whole hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_AnalysisTaxonomicGroupsForProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the contents of the table AnalysisTaxonomicGroup used in a project including the whole hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_AnalysisTaxonomicGroupsForProject'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the entries from the table relevant for a mobile application',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_EventImageTypes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the entries from the table relevant for a mobile application',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_EventImageTypes'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that the content of the table CollectionEvent related to the given project and a locality.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_EventsForProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that the content of the table CollectionEvent related to the given project and a locality.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_EventsForProject'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'A search string for comparision with the column LocalityDescription',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_EventsForProject',
@level2type=N'PARAMETER', @level2name=N'@Locality'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'A search string for comparision with the column LocalityDescription',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_EventsForProject',
@level2type=N'PARAMETER', @level2name=N'@Locality'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists the determination entry from the table CollIdentificationCategory_Enum relevant for a mobile application.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationCategories'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists the determination entry from the table CollIdentificationCategory_Enum relevant for a mobile application.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationCategories'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the entries from the table CollIdentificationQualifier_Enum relevant for a mobile application.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationQualifiers'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the entries from the table CollIdentificationQualifier_Enum relevant for a mobile application.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationQualifiers'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the first altitude of a unit as stored in table IdentificationUnitGeoAnalysis.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationUnitAltitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the first altitude of a unit as stored in table IdentificationUnitGeoAnalysis.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationUnitAltitude'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the first latitude of a unit as stored in table IdentificationUnitGeoAnalysis. ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationUnitLatitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the first latitude of a unit as stored in table IdentificationUnitGeoAnalysis. ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationUnitLatitude'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the first longitude of a unit as stored in table IdentificationUnitGeoAnalysis.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationUnitLongitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the first longitude of a unit as stored in table IdentificationUnitGeoAnalysis.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_IdentificationUnitLongitude'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the material categories as defined in the settings related to the given project as stored in DiversityProjects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_MaterialCategories'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the material categories as defined in the settings related to the given project as stored in DiversityProjects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_MaterialCategories'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the project for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_ProjectList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the project for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_ProjectList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_ProjectList',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_ProjectList',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the entries from the table CollSpecimenImageType_Enum relevant for a mobile application.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_SpecimenImageTypes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the entries from the table CollSpecimenImageType_Enum relevant for a mobile application.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_SpecimenImageTypes'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the taxonomic groups as defined in the settings related to the given project as stored in DiversityProjects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_TaxonomicGroups'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the taxonomic groups as defined in the settings related to the given project as stored in DiversityProjects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_TaxonomicGroups'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the entries from the table CollUnitRelationType_Enum relevant for a mobile application.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_UnitRelationTypes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the entries from the table CollUnitRelationType_Enum relevant for a mobile application.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_UnitRelationTypes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the projects for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_UserInfo'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the projects for a user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityMobile_UserInfo'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityWorkbenchModule'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the module',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityWorkbenchModule'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns the information about an entity',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns the information about an entity',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The language to which the information should be restricted',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'PARAMETER', @level2name=N'@Language'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The language to which the information should be restricted',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'PARAMETER', @level2name=N'@Language'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The context to which the information should be restricted',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'PARAMETER', @level2name=N'@Context'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The context to which the information should be restricted',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'PARAMETER', @level2name=N'@Context'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If DisplayText is present',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'DisplayTextOK'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If DisplayText is present',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'DisplayTextOK'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If Abbreviation is present',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'AbbreviationOK'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If Abbreviation is present',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'AbbreviationOK'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If Description is present',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'DescriptionOK'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If Description is present',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'DescriptionOK'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes from table EntityUsage is present',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'UsageNotes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes from table EntityUsage is present',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'UsageNotes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If an object does exist',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'DoesExist'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If an object does exist',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EntityInformation_2',
@level2type=N'COLUMN', @level2name=N'DoesExist'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'retrieval of the description including all superior events',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventDescription'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'retrieval of the description including all superior events',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventDescription'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'retrieval of the description of all superior events',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventDescriptionSuperior'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'retrieval of the description of all superior events',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventDescriptionSuperior'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesChildNodes'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The SeriesID for which the children should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The SeriesID for which the children should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Series related to the given Series',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Series related to the given Series',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesHierarchy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Series above the given Series.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesSuperiorList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the Series above the given Series.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesSuperiorList'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N' Returns the top ID within the hierarchy for a given ID from the table CollectionEventSeries',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesTopID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N' Returns the top ID within the hierarchy for a given ID from the table CollectionEventSeries',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSeriesTopID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'calculation of all specimen in the database that are assigned to this event, including the inferior events',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSpecimenNumber'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'calculation of all specimen in the database that are assigned to this event, including the inferior events',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSpecimenNumber'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the CollectionEventIDs superior to a given event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSuperiorList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the CollectionEventIDs superior to a given event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSuperiorList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ExchangeChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ExchangeChildNodes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ExchangeHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ExchangeHierarchy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The CollectionEventID for which the count should be calculated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSpecimenNumber',
@level2type=N'PARAMETER', @level2name=N'@EventID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The CollectionEventID for which the count should be calculated',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'EventSpecimenNumber',
@level2type=N'PARAMETER', @level2name=N'@EventID'
END CATCH;
GO

if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_NAME = 'AnalysisHierarchy') > 0
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Content of table Analysis containing all data for a given AnalysisID including the children defined via the relation in column AnalysisParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Deprecated. Content of table Analysis containing all data for a given AnalysisID including the children defined via the relation in column AnalysisParentID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'AnalysisHierarchy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the specimen with the first entries of related tables',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the specimen with the first entries of related tables',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Comma separated list of CollectionSpecimenIDs for which the data should be retrieved',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'PARAMETER', @level2name=N'@CollectionSpecimenIDs'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collector are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'If the data of the collector are withhold, the reason for withholding the data, otherwise null',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Data_withholding_reason_for_collector'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The country where the collection event took place. Cached value derived from an geographic entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Country'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The country where the collection event took place. Cached value derived from an geographic entry',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Country'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes about the collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Collection_event_notes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes about the collection event',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Collection_event_notes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Named_area'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Named_area'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'NamedAreaLocation2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'NamedAreaLocation2'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove a link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_gazetteer'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove a link to a named area as e.g. derived from DiversityGazetteer corresponding to LocalisationSystemID = 7',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_gazetteer'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Longitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Longitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Latitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Latitude'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Coordinates_accuracy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Coordinates_accuracy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for coordinates derived from WGS84 corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'_CoordinatesLocationNotes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for retrieval of WGS84 coordinates via e.g. GoogleMaps corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_GoogleMaps'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for retrieval of WGS84 coordinates via e.g. GoogleMaps corresponding to LocalisationSystemID = 8',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_GoogleMaps'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Lower value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Altitude_from'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Lower value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Altitude_from'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Upper value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Altitude_to'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Upper value of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Altitude_to'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Altitude_accuracy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Altitude_accuracy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_Altitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for altitude range corresponding to LocalisationSystemID = 4',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_Altitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'MTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'MTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Quadrant of TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Quadrant'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Quadrant of TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Quadrant'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_MTB'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for TK25 corresponding to LocalisationSystemID = 3',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_MTB'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Name of the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Name of the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_SamplingPlots'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_SamplingPlots'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserverd to remove link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_SamplingPlots'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserverd to remove link to the sampling plot corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_SamplingPlots'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Accuracy_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Accuracy of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Accuracy_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Latitude_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Latitude_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Longitude_of_sampling_plot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the sampling plot coordinates corresponding to LocalisationSystemID = 13',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Longitude_of_sampling_plot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Geographic region corresponding to LocalisationSystemID = 10',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Geographic_region'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Geographic region corresponding to LocalisationSystemID = 10',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Geographic_region'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Lithostratigraphy corresponding to LocalisationSystemID = 30',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Lithostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Lithostratigraphy corresponding to LocalisationSystemID = 30',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Lithostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Chronostratigraphy corresponding to LocalisationSystemID = 20',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Chronostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Chronostratigraphy corresponding to LocalisationSystemID = 20',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Chronostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Biostratigraphy corresponding to LocalisationSystemID = 60',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Biostratigraphy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Biostratigraphy corresponding to LocalisationSystemID = 60',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Biostratigraphy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link for the first collector to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link for the first collector to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for removal of ink for the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_collector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved for removal of ink for the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_collector'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes about the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_about_collector'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes about the first collector',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_about_collector'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Depositors_link_to_DiversityAgents'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Depositors_link_to_DiversityAgents'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_Depositor'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link for the depositor e.g. to DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_Depositor'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityExsiccatae'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityExsiccatae'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_exsiccatae'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reserved to remove the link to DiversityExsiccatae',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_to_exsiccatae'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Order of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Order_of_taxon'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Order of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Order_of_taxon'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Family of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Family_of_taxon'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Family of the first taxon',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Family_of_taxon'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Identifier of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identifier_of_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Identifier of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identifier_of_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Description_of_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Description of the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Description_of_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for the first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'External datasource ID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'External_datasource'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'External datasource ID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'External_datasource'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityTaxonNames'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityTaxonNames'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column to remove link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_identification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column to remove link for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_identification'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'First analysis of first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Analysis'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'First analysis of first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Analysis'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_identification'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for first identification of first organism to DiversityTaxonNames',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_identification'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Agent responsible for first identification of first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Agent responsible for first identification of first organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Determiner'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents_for_determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityAgents_for_determiner'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Empty column reservet to remove link for agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_determiner'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Empty column reservet to remove link for agent for responsible for first identification of first organism to e.g. DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_determiner'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Taxonomic group of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Taxonomic_group_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Taxonomic group of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Taxonomic_group_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Life stage of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Life_stage_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Life stage of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Life_stage_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Gender of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Gender_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Gender of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Gender_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Number of units of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Number_of_units_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Number of units of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Number_of_units_of_second_organism'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Circumstances of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Circumstances_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Circumstances of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Circumstances_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Identifier for second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identifier_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Identifier for second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identifier_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'UnitDescription of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Description_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'UnitDescription of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Description_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'OnlyObserved of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Only_observed_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'OnlyObserved of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Only_observed_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Notes for second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Notes for second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Notes_for_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Taxonomic name of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Taxonomic_name_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Taxonomic name of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Taxonomic_name_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Link to DiversityTaxonNames of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityTaxonNames_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Link to DiversityTaxonNames of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Link_to_DiversityTaxonNames_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Remove link to DiversityTaxonNames for second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Remove link to DiversityTaxonNames for second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Remove_link_for_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Vernacular term of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Vernacular_term_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Vernacular term of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Vernacular_term_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Identification day of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identification_day_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Identification day of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identification_day_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Identification month of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identification_month_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Identification month of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identification_month_of_second_organism'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Identification year of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identification_year_of_second_organism'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Identification year of second organism',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'FirstLines_4',
@level2type=N'COLUMN', @level2name=N'Identification_year_of_second_organism'
END CATCH;
GO









--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.38'
END

GO


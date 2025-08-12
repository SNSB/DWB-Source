
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.62'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Changing the default for the log columns   ######################################################################################
--#####################################################################################################################

declare @Tables table (TableName varchar(50) primary key not null)
declare @Columns table (ColumnName varchar(50) primary key not null)

insert into @Tables (TableName)
select distinct t.TABLE_NAME
from INFORMATION_SCHEMA.TABLES t 
, INFORMATION_SCHEMA.COLUMNS c
where t.TABLE_TYPE = 'BASE TABLE'
and t.TABLE_NAME = c.TABLE_NAME
and t.TABLE_NAME not like 'xx%'
and t.TABLE_NAME not like '%Enum'
and t.TABLE_NAME not like '%_log_%'
and c.COLUMN_NAME = 'LogUpdatedBy'
order by t.TABLE_NAME

select * from @Tables

declare @Default varchar(50)
declare @TableName varchar(50)
declare @SQL nvarchar(max)
declare @LogColumn varchar(50)

WHILE (select count(*) from @Tables) > 0
BEGIN
	SET @TableName = (SELECT MIN(TableName) FROM @Tables)
	IF (@TableName LIKE '%_log')
	begin
		INSERT INTO @Columns (ColumnName) VALUES ('LogUser')
	end
	else
	begin
		INSERT INTO @Columns (ColumnName) VALUES ('LogCreatedby')
		INSERT INTO @Columns (ColumnName) VALUES ('LogInsertedby')
		INSERT INTO @Columns (ColumnName) VALUES ('LogUpdatedby')
	end
	WHILE (select count(*) from @Columns) > 0
	begin
		SET @LogColumn = (SELECT MIN(ColumnName) FROM @Columns)
		SET @Default = (select D.name 
			from sys.default_constraints D, sys.tables T, sys.columns C
			where D.parent_object_id = T.object_id
			and C.object_id = T.object_id
			and D.parent_column_id = C.column_id
			and t.name = @TableName
			and C.name = @LogColumn)
		if (@Default <> '')
		begin
			set @SQL = (select 'alter table ' + @TableName + ' drop ' + @Default + '; alter table ' + @TableName + ' add default SYSTEM_USER for ' + @LogColumn)
			begin try
			exec sp_executesql @SQL
			end try
			begin catch
			end catch
		end
		DELETE C FROM @Columns C WHERE C.ColumnName = @LogColumn
	end

	DELETE T FROM @Tables T WHERE T.TableName = @TableName
END
GO


--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.09' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.63'
END

GO



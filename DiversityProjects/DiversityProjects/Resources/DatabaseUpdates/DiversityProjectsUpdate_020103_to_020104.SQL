
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.03'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Agent sequence   #######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectAgent] ADD [AgentSequence] int NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The sequence of an agent within a project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentSequence'
GO

ALTER TABLE [dbo].[ProjectAgent_log] ADD [AgentSequence] int NULL
GO

/****** Object:  Trigger [dbo].[trgDelProjectAgent]    Script Date: 11.05.2015 18:00:29 ******/

ALTER TRIGGER [dbo].[trgDelProjectAgent] ON [dbo].[ProjectAgent] 
FOR DELETE AS 
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes, AgentSequence, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.AgentType, deleted.Notes, deleted.AgentSequence, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO
/****** Object:  Trigger [dbo].[trgUpdProjectAgent]    Script Date: 11.05.2015 18:01:04 ******/

ALTER TRIGGER [dbo].[trgUpdProjectAgent] ON [dbo].[ProjectAgent] 
FOR UPDATE AS
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes, AgentSequence, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.AgentType, deleted.Notes, deleted.AgentSequence, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED
Update ProjectAgent
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM ProjectAgent, deleted 
where 1 = 1 
AND ProjectAgent.AgentName = deleted.AgentName
AND ProjectAgent.AgentURI = deleted.AgentURI
AND ProjectAgent.ProjectID = deleted.ProjectID
GO


--#####################################################################################################################
--######   Changing the default for the log columns   #################################################################
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
--######   Update Trigger:  SYSTEM_USER  instead of current_user   ####################################################
--#####################################################################################################################

declare @TriggerList table(trigger_name varchar(50))
insert into @TriggerList (trigger_name) 
SELECT
    [so].[name] AS [trigger_name]
FROM sysobjects AS [so]
INNER JOIN sysobjects AS so2 ON so.parent_obj = so2.Id
WHERE [so].[type] = 'TR'
and OBJECTPROPERTY( [so].[id], 'ExecIsUpdateTrigger') = 1
and [so].[name] like 'trgUpd%'

declare @TriggerName nvarchar(50)
set @TriggerName = (select min(Trigger_name) from @TriggerList)
while (select count(*) from @TriggerList) > 0
begin
	declare @Trigger nvarchar(max)
	set @Trigger = ''
	declare @TT table([definition] nvarchar(255), ID int identity)
	insert into @TT ([definition]) exec sp_helptext @TriggerName
	declare @ID int
	set @ID = (select min(ID) from @TT)
	while (Select count(*) from @TT) > 0
	begin
		set @Trigger = @Trigger + (select [definition] from @TT where ID = @ID)
		delete T from @TT T where T.ID = @ID
		set @ID = (select min(ID) from @TT)
	end
	if (@Trigger like '% = current_user%')
	begin
		set @Trigger = REPLACE(@Trigger, 'CREATE TRIGGER ', 'ALTER TRIGGER ')
		set @Trigger = replace(@Trigger, ' = current_user', ' = SYSTEM_USER')
		begin try
			exec sp_executesql @Trigger
		end try
		begin catch
			select 'Error: ' + cast(@@ERROR as varchar)
		end catch
	end
	delete L from @TriggerList L where L.trigger_name = @TriggerName
	set @TriggerName = (select min(Trigger_name) from @TriggerList) 
end
 GO




--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.01' END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.04'
END

GO


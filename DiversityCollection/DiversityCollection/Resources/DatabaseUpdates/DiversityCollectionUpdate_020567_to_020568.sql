
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.67'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Update Trigger:  SYSTEM_USER  instead of current_user   ##################################################
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
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.02' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.68'
END

GO



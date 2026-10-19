
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.23'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######  ProjectUser: Adding ReadOnly for locking a project  #########################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'ProjectUser' and c.COLUMN_NAME = 'ReadOnly') = 0
begin
ALTER TABLE [dbo].[ProjectUser] ADD [ReadOnly] [bit] NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user has only read access to data of this project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'ReadOnly'
end
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.24'
END

GO

select [dbo].[Version] ()  


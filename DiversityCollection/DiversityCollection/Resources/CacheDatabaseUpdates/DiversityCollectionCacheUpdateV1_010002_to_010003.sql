
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.02'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (500)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--###### Column in table ProjectPublished corresponding to HighResolutionImagePath  ###################################
--#####################################################################################################################

-- does not work as high resolution versions of images do not correspond to projects

--ALTER TABLE [dbo].[ProjectPublished] ADD [HtmlAvailable] [tinyint] NULL
--GO
--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If a high resolution html image is available' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'HtmlAvailable'
--GO


--#####################################################################################################################
--#####################################################################################################################
--######   Testing the basic functions to enable generic generation of views etc.   ###################################
--#####################################################################################################################
--#####################################################################################################################

declare @Message nvarchar (500)
declare @AllTestPassed char(3)
set @AllTestPassed = 'YES'

--#####################################################################################################################
--######   BaseURLofSource  ######################################################################################
--#####################################################################################################################


declare @BaseURLofSource nvarchar(500)
begin try
	set @BaseURLofSource = ( select dbo.BaseURLofSource())
	--select 'BaseURLofSource: ' + @BaseURLofSource
end try
begin catch
	set @AllTestPassed = 'NO'
	set @Message = 'The function BaseURLofSource did not return a result. Please turn to your administrator to fix this before continuing with the update of the database'
	RAISERROR (@Message, 18, 1) 
end catch


--#####################################################################################################################
--######   ServerURL  ######################################################################################
--#####################################################################################################################


declare @ServerURL nvarchar(500)
begin try
	set @ServerURL = ( select dbo.ServerURL())
	--select 'ServerURL: ' + @ServerURL
end try
begin catch
	set @AllTestPassed = 'NO'
	set @Message = 'The function ServerURL did not return a result. Please turn to your administrator to fix this before continuing with the update of the database'
	RAISERROR (@Message, 18, 1) 
end catch



--#####################################################################################################################
--######   DatebaseTrunk  ######################################################################################
--#####################################################################################################################

declare @SourceDatebaseTrunk nvarchar(500)
begin try
set @SourceDatebaseTrunk = ( select dbo.SourceDatebaseTrunk())
--select 'SourceDatebaseTrunk: ' + @SourceDatebaseTrunk
end try
begin catch
	set @AllTestPassed = 'NO'
	set @Message = 'The function SourceDatebaseTrunk did not return a result. Please turn to your administrator to fix this before continuing with the update of the database'
	RAISERROR (@Message, 18, 1) 
end catch




--#####################################################################################################################
--######   HighResolutionImagePath  ######################################################################################
--#####################################################################################################################


declare @HighResolutionImagePath nvarchar(500)
begin try
set @HighResolutionImagePath = ( select dbo.HighResolutionImagePath('http/images'))
--select 'SourceDatebaseTrunk: ' + @HighResolutionImagePath
end try
begin catch
	set @AllTestPassed = 'NO'
	set @Message = 'The function HighResolutionImagePath did not return a result. Please turn to your administrator to fix this before continuing with the update of the database'
	RAISERROR (@Message, 18, 1) 
end catch


--#####################################################################################################################
--######   Projects Database  ######################################################################################
--#####################################################################################################################

declare @ProjectsDatabase nvarchar(500)
begin try
set @ProjectsDatabase = ( select dbo.ProjectsDatabase())
--select 'ProjectsDatabase: ' + @ProjectsDatabase
end try
begin catch
	set @AllTestPassed = 'NO'
	set @Message = 'The function ProjectsDatabase did not return a result. Please turn to your administrator to fix this before continuing with the update of the database'
	RAISERROR (@Message, 18, 1) 
end catch
declare @SQL nvarchar(500)
set @SQL = 'DECLARE @i INT; SET @I = (SELECT COUNT(*) FROM ' + @ProjectsDatabase + '.dbo.SettingsForProject(0, ''ABCD | %'', ''.'', 2))'
begin try
	exec sp_executesql @SQL
end try
begin catch
	set @AllTestPassed = 'NO'
end catch






--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################
--select @AllTestPassed

if 	( @AllTestPassed = 'YES')
begin
	set @SQL = 'ALTER FUNCTION [dbo].[Version] ()  
	RETURNS nvarchar(8)
	AS
	BEGIN
	RETURN ''01.00.03''
	END'
	exec sp_executesql @SQL
end
else
begin
	set @Message = 'The test of the generic functions failed. Please turn to your administrator for the correction of these functions'
	RAISERROR (@Message, 18, 1) 
end

GO



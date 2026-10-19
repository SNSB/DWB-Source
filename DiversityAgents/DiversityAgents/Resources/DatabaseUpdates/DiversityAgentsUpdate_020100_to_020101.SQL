
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.00'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   AgentURI   ######################################################################################
--#####################################################################################################################

declare @i int
set @i = (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C
where C.TABLE_NAME = 'UserProxy'
and C.COLUMN_NAME = 'AgentURI')
if (@i = 0)
begin
ALTER TABLE UserProxy ADD [AgentURI] [varchar](255) NULL
end

GO

begin try
-- may be present - avoid error message
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A link to a DiversityAgents module where further informations about the user is available.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'AgentURI'
end try
begin catch
end catch
GO


--#####################################################################################################################
--######   [ProjectURI]   ######################################################################################
--#####################################################################################################################

declare @i int
set @i = (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C
where C.TABLE_NAME = 'ProjectProxy'
and C.COLUMN_NAME = 'ProjectURI')
if (@i = 0)
begin
ALTER TABLE ProjectProxy ADD ProjectURI varchar(255) NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectURI'
end
GO


begin try
	declare @BaseURL varchar(255)
	set @BaseURL = '';
	declare @ProjectDB nvarchar(50);
	set @ProjectDB = (select MIN(S.CATALOG_NAME) from INFORMATION_SCHEMA.SCHEMATA S)
	declare @Module nvarchar(50);
	set @Module = (select dbo.DiversityWorkbenchModule())
	set @ProjectDB = 'DiversityProjects' + SUBSTRING(@ProjectDB, len(@Module) + 1, 255)
	declare @Command nvarchar(500)
	DECLARE @ParmDefinition nvarchar(500);
	SET @ParmDefinition = N'@UrlOut varchar(60) OUTPUT';
	set @Command = (' Select @UrlOut = '  + @ProjectDB + '.dbo.BaseURL()');
	Execute sp_executesql @Command, @ParmDefinition,  @UrlOut=@BaseURL OUTPUT;
	UPDATE P SET P.ProjectURI = @BaseURL + cast(P.ProjectID as varchar)
	From ProjectProxy P
	where P.ProjectID > 0
end try
begin catch
	select 'Please set the link to DiversityProjects. The default name for the database could not be found: ' + ERROR_MESSAGE() as ErrorMessage
end catch

GO



--#####################################################################################################################
--######   [VersionClient]   ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[VersionClient] () RETURNS nvarchar(11) AS BEGIN RETURN '03.00.00.08' END
GO

GRANT EXEC ON [dbo].[VersionClient] TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.01'
END

GO

select [dbo].[Version] ()  


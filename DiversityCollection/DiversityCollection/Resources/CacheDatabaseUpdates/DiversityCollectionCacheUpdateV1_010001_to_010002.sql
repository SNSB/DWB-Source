
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.01'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--#####################################################################################################################
--######   Creating the basic functions to enable generic generation of views etc.  ###################################
--#####################################################################################################################
--#####################################################################################################################




--#####################################################################################################################
--######   BaseURLofSource  ######################################################################################
--#####################################################################################################################


declare @CacheDB nvarchar(100)
declare @SQL nvarchar(max)
DECLARE @ParmDefinition nvarchar(500);
SET @ParmDefinition = N'@DBout nvarchar(50) OUTPUT';
declare @DB nvarchar(50)
set @SQL = 'set @DBout = (select min(T.TABLE_CATALOG) from INFORMATION_SCHEMA.TABLES T)'
exec sp_executesql @SQL, @ParmDefinition, @DBout=@DB OUTPUT
set @CacheDB = @DB
set @DB = REPLACE(@DB, 'Cache', '')
begin try -- try simple variant that the name of the databases are corresponding
	set @SQL = 'use ' + @DB
	exec sp_executesql @SQL
end try
begin catch
	-- getting the names of all databases
	declare @DBs table (DBname nvarchar(100))
	insert into @DBs
	select name from sys.databases
	where name like 'DiversityCollection%'
	while (select count(*) from @DBs) > 0
	begin
		set @DB = (select min(DBname) from @DBs)
		delete D from @DBs D where D.DBname = @DB
		-- check every database: does it exist, does it contain a table CacheDatabase and does this table contain 1 entry with the correkt name
		set @SQL = 'use ' + @DB
		begin try
			exec sp_executesql @SQL
			SET @ParmDefinition = N'@Count int OUTPUT';
			declare @i int
			set @SQL = 'use ' + @DB + '; set @Count = (SELECT COUNT(*) FROM [dbo].[CacheDatabase] WHERE [DatabaseName] = ''' + @CacheDB + ''')';
			exec sp_executesql @SQL, @ParmDefinition, @Count=@i OUTPUT
			if (@i = 1)
			begin
				delete D from @DBs D
			end
			else
			begin
				set @DB = ''
			end
		end try
		begin catch
		end catch
	end
end catch

if @DB = ''
begin
	declare @Message nvarchar(500)
	set @Message = 'No source database could be found. Please turn to your administrator'
	RAISERROR (@Message, 18, 1) 
end


declare @BaseURL nvarchar(500)
set @SQL = 'use ' + @DB + '; set @BaseUrlOut = (select dbo.BaseURL())'
SET @ParmDefinition = N'@BaseUrlOut nvarchar(500) OUTPUT';
exec sp_executesql @SQL, @ParmDefinition, @BaseUrlOut=@BaseURL OUTPUT

set @SQL = 'CREATE FUNCTION [dbo].[BaseURLofSource] ()  
RETURNS  nvarchar (500) AS  
BEGIN RETURN ''' + @BaseURL + ''' END'
select 'ALTER ' + SUBSTRING(@SQL, 9, 900)
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 900)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [dbo].[BaseURLofSource] TO [CollectionCacheUser]
GO

--#####################################################################################################################
--######   ServerURL  ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[ServerURL] ()  
RETURNS  nvarchar (255) AS  
BEGIN 
-- Returns the URL of the server as found in the BaseURL of the source
-- TEST: select dbo.ServerURL()
declare @URL nvarchar(255)
declare @Pos int
set @URL =  dbo.BaseURLofSource()
if (@URL LIKE '%/') set @URL = substring(@URL, 1, len(@URL) - 1)
set @URL = REVERSE(@URL)
set @Pos = len(@URL) - charindex('/', @URL) + 1
set @URL = REVERSE(@URL)
set @URL = substring(@URL, 1, @Pos)
return @URL
END

GO

GRANT EXEC ON dbo.ServerURL TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######    SourceDatebaseTrunk  ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[SourceDatebaseTrunk] ()  
RETURNS  nvarchar (255) AS  
BEGIN 
-- Returns the name of the source database as found in the BaseURL, i.e. without the "Diversity" prefix
-- TEST: select dbo.SourceDatebaseTrunk()
declare @DB nvarchar(255)
declare @Pos int
set @DB =  dbo.BaseURLofSource()
if (@DB LIKE '%/') set @DB = substring(@DB, 1, len(@DB) - 1)
set @DB = REVERSE(@DB)
set @Pos = len(@DB) - charindex('/', @DB) + 1
set @DB = REVERSE(@DB)
set @DB = Upper(substring(@DB, @Pos + 1, 1)) + substring(@DB, @Pos + 2, 255)
return @DB
END

GO

GRANT EXEC ON dbo.[SourceDatebaseTrunk] TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   SourceDatebase        ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[SourceDatebase] ()  
RETURNS  nvarchar (255) AS  
BEGIN 
-- Returns the name of the source database
-- TEST: select dbo.SourceDatebase()
declare @DB nvarchar(255)
set @DB =  'Diversity' + dbo.SourceDatebaseTrunk()
return @DB
END

GO

GRANT EXEC ON dbo.[SourceDatebase] TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   HighResolutionImagePath  ######################################################################################
--#####################################################################################################################




CREATE FUNCTION [dbo].[HighResolutionImagePath] (@URI varchar(255))  
RETURNS  nvarchar (255) AS  
-- Function translates the path of an image into the corresponding path of a web resource with of high resolution version of the image
BEGIN 
if @URI LIKE 'http://pictures.snsb.info/BsmLichensColl/%.jpg' OR @URI LIKE  'http://pictures.snsb.info/BsmVPlantsColl/%.jpg' OR @URI LIKE 'http://pictures.snsb.info/MsbVPlantsColl/%.jpg'
begin
set @URI =  REPLACE(REPLACE(REPLACE(@URI, 'http://pictures.snsb.info/', 'http://zoomview.snsb.info/'), '/web/', '/'), '.jpg', '.html')
end
else
begin
set @URI = ''
end
return @URI
END


GO

GRANT EXEC ON dbo.[HighResolutionImagePath] TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   Projects Database  ######################################################################################
--#####################################################################################################################

CREATE  FUNCTION [dbo].[ProjectsDatabase] ()  
RETURNS  nvarchar (255) as 
begin
-- select dbo.ProjectsDatabase()
declare @DB nvarchar(50)
set @DB = 'Diversity' + (select dbo.[SourceDatebaseTrunk]())
set @DB = REPLACE(@DB, 'Collection', 'Projects')
return @DB
end
go

GRANT EXEC ON dbo.ProjectsDatabase  TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.02'
END

GO




declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.54'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [CacheDatabase]   ######################################################################################
--######   Table holding the cache databases connected to the database   ############################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.TABLES T where t.TABLE_NAME = 'CacheDatabase') = 0
begin
CREATE TABLE [dbo].[CacheDatabase](
	[Server] [varchar](50) NOT NULL,
	[DatabaseName] [varchar](50) NOT NULL,
	[Port] [smallint] NOT NULL,
	[Version] [varchar](50) NULL,
 CONSTRAINT [PK_CacheDatabase] PRIMARY KEY CLUSTERED 
(
	[Server] ASC,
	[DatabaseName] ASC,
	[Port] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or IP of the server where the cache database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase', @level2type=N'COLUMN',@level2name=N'Server'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the cache database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase', @level2type=N'COLUMN',@level2name=N'DatabaseName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The port of the server where the cache database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase', @level2type=N'COLUMN',@level2name=N'Port'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the cache database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase', @level2type=N'COLUMN',@level2name=N'Version'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Table holding the cache databases connected to the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase'
end
GO




GRANT SELECT ON [dbo].[CacheDatabase] TO Administrator
GO


--#####################################################################################################################
--######   Eintrag der CacheDB   ######################################################################################
--######   if possible, insert an existing cache DB into the new table    ######################################################################
--#####################################################################################################################


declare @DB nvarchar(50)
set @DB = (select replace(min(d.DOMAIN_CATALOG), 'DiversityCollection', 'DiversityCollectionCache') from INFORMATION_SCHEMA.DOMAINS d)
if (select count(*) from sys.databases s where s.name = @DB) = 1
begin
	declare @server varchar(50)
	set @server = (select substring(dbo.BaseURL(), 8, 50))
	set @server = (select SUBSTRING(@server, 1, charindex('/', @server)-1))
	if (SELECT COUNT(*) FROM CacheDatabase C WHERE C.[Server] = @server and C.DatabaseName = @DB and C.Port = 5432) = 0
	begin
		INSERT INTO [dbo].[CacheDatabase] ([Server], [DatabaseName],[Port],[Version])
		SELECT @server, @DB, 5432, '01:00:00'
	end
end
GO


--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.08' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.55'
END

GO



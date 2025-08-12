
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.28'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Agents Database     ########################################################################################
--######   If name of the agents database does not correspond to the name of the collection database, 
--######   this function must be edited to ensure a retrieval of the metadata   
--######   turn to you administrator to ensure the correct name 
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_SCHEMA = 'dbo' and R.ROUTINE_NAME = 'AgentsDatabase') = 0
begin

	declare @DB nvarchar(50)
	declare @Prefix nvarchar(50)
	set @Prefix = 'DiversityAgents'

	-- try a name according to the collection database
	set @DB = 'Diversity' + (select dbo.[SourceDatabaseTrunk]())
	set @DB = REPLACE(@DB, 'Collection', 'Agents')
	if(select count(*) from sys.databases where name = @DB) = 0 begin set @DB = '' end

	-- if only 1 database exists
	if (len(@DB) = 0) and (select count(*) from sys.databases where name like @Prefix + '%') = 1
	begin
		set @DB = (select top 1 name from sys.databases where name like  @Prefix + '%')
	end

	-- try a name similar to the collection database
	declare @V nvarchar(50);
	declare @H nvarchar(50);
	set @H = (select dbo.[SourceDatabaseTrunk]());
	set @H = REPLACE(@H, 'Collection', '');
	set @V = @H;
	while (len(@V) > 3) and (len(@DB) = 0)
	begin
		set @V = SUBSTRING(@V, 2, 50)
		if (len(@DB) = 0) and (select count(*) from sys.databases where name like  @Prefix + '%' + @V + '%') > 0
		begin
			set @DB = (select top 1 name from sys.databases where name like  @Prefix + '%' + @V + '%')
		end
		set @H = SUBSTRING(@H, 1, len(@H) - 1)
		if (len(@DB) = 0) and (select count(*) from sys.databases where name like  @Prefix + '%' + @H + '%') > 0
		begin
			set @DB = (select top 1 name from sys.databases where name like  @Prefix + '%' + @H + '%')
		end
	end

	-- take first database
	if (len(@DB) = 0) and (select count(*) from sys.databases where name like  @Prefix + '%') > 0
	begin
		set @DB = (select top 1 name from sys.databases where name like  @Prefix + '%')
	end

/*
15.4.2021 - in Client verschoben
*/

declare @AgentsDatabase nvarchar(500)
begin try
set @AgentsDatabase = ( select dbo.AgentsDatabase())
end try
begin catch

	declare @SQL nvarchar(max)
	set @SQL = (select 'CREATE FUNCTION [dbo].[AgentsDatabase] ()  
	RETURNS  nvarchar (255) as 
	begin
	declare @DB nvarchar(50)
	return ''' + @DB + '''
	end')

	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
	exec sp_executesql @SQL
	end catch

	begin try
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the DiversityAgents database where the agents are found' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'AgentsDatabase'
	end try
	begin catch
	end catch
end catch
end
GO

GRANT EXEC ON dbo.AgentsDatabase  TO [CacheUser]
GO


declare @Message nvarchar (500)
declare @AllTestPassed char(3)
set @AllTestPassed = 'YES'
declare @AgentsDatabase nvarchar(500)
begin try
set @AgentsDatabase = ( select dbo.AgentsDatabase())
end try
begin catch
	set @AllTestPassed = 'NO'
	set @Message = 'The function AgentsDatabase did not return a result. Please turn to your administrator to fix this before continuing with the update of the database'
	RAISERROR (@Message, 18, 1) 
end catch
declare @SQL nvarchar(500)
set @SQL = 'DECLARE @i INT; SET @I = (SELECT COUNT(*) FROM ' + @AgentsDatabase + '.dbo.Agent)'
begin try
	exec sp_executesql @SQL
end try
begin catch
	set @AllTestPassed = 'NO'
end catch


--#####################################################################################################################
--######   Check function   ###########################################################################################
--#####################################################################################################################

IF (@AllTestPassed = 'NO')
BEGIN
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   set Grants for CacheAdmin  #################################################################################
--#####################################################################################################################

ALTER ROLE [db_ddladmin] ADD MEMBER [CacheAdmin]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [CacheAdmin]
GO
GRANT UPDATE ON [ProjectPublished] TO [CacheAdmin]
GO

--#####################################################################################################################
--######   Create auxilliary user for command shell   #################################################################
--#####################################################################################################################

-- Create login 'cmdshell'
IF (SELECT COUNT(*) FROM [sys].[sql_logins] WHERE [name] = 'cmdshell') = 0
BEGIN
	DECLARE @SQL nvarchar(1000)
	DECLARE @PW varchar(100);
	DECLARE @DB varchar(200);
	SET @DB = (SELECT DB_NAME());
	SET @PW = (SELECT SUBSTRING(CONVERT(varchar, RAND()), 3, 5) + SUBSTRING(CONVERT(varchar, RAND()), 3, 5) + SUBSTRING(CONVERT(varchar, RAND()), 3, 5) + SUBSTRING(CONVERT(varchar, RAND()), 3, 5));
	SET @SQL = 'USE [master]; CREATE LOGIN [cmdshell] WITH PASSWORD = ''' + @PW + ''', CHECK_POLICY=OFF;';
	EXEC sys.sp_executesql @SQL;
	SET @SQL = 'USE [master]; ALTER LOGIN [cmdshell] DISABLE;';
	EXEC sys.sp_executesql @SQL;

	SET @SQL = 'USE [master]; CREATE USER [cmdshell] FOR LOGIN [cmdshell]; GRANT EXEC ON xp_cmdshell TO [cmdshell];';
	EXEC sys.sp_executesql @SQL;

	SET @SQL = 'USE [' + @DB + '];';
	EXEC sys.sp_executesql @SQL;
END
GO

IF (SELECT COUNT(*) FROM [sys].[database_principals] WHERE [type]='S' AND [name]='cmdshell') = 0
	CREATE USER [cmdshell] FOR LOGIN [cmdshell];


--#####################################################################################################################
--######   Create auxilliary table for file transfer to postgres   ####################################################
--#####################################################################################################################

CREATE TABLE [dbo].[bcpPostgresTableDefinition](
    [SchemaName] [varchar](200) NOT NULL,
    [TableName] [varchar](200) NOT NULL,
    [ColumnName] [varchar](200) NOT NULL,
    [DataType] [varchar](50) NULL,
    [OrdinalPositon] [int] NULL,
 CONSTRAINT [PK_bcpPostgresTableDefinition] PRIMARY KEY CLUSTERED
(
    [SchemaName] ASC,
    [TableName] ASC,
    [ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

GRANT INSERT ON [dbo].[bcpPostgresTableDefinition] TO [CacheAdmin] 
GO
GRANT DELETE ON [dbo].[bcpPostgresTableDefinition] TO [CacheAdmin] 
GO
GRANT SELECT ON [dbo].[bcpPostgresTableDefinition] TO [CacheUser]
GO


--#####################################################################################################################
--######   Create procedures for file transfer to postgres   ##########################################################
--#####################################################################################################################

-- =============================================
-- Author:    Markus Weiss
-- Create date: 2020/04/21
-- Description:    Create view for bcp transfer
-- =============================================
CREATE PROCEDURE [dbo].[procBcpViewCreate]
    @TableName varchar(200),
    @Schema varchar(200)

AS
BEGIN
    SET NOCOUNT ON;
    /*
    Test:
    DECLARE @RC int
    DECLARE @TableName varchar(200)
    DECLARE @Schema varchar(200)
    SET @TableName = 'CacheCollectionSpecimen' --'Agent'
    SET @Schema = 'Project_TestLokal' --'dbo'
    EXECUTE @RC = [dbo].[procBcpViewCreate]
    @TableName, @Schema

    */

    declare @SQL nvarchar(MAX);
    declare @Columns Table(ColumName varchar(200) NOT NULL, DataType varchar(50) NOT NULL, OrdinalPositon int NULL);
    declare @ColumnsPostgres Table(ColumName varchar(200) NOT NULL, DataType varchar(50) NOT NULL, OrdinalPositon int NULL);
    declare @View varchar(250);
    set @View = 'TransferView_' + @TableName;

    INSERT INTO @ColumnsPostgres(ColumName, DataType, OrdinalPositon)
    SELECT C.ColumnName, C.DataType, C.OrdinalPositon
    FROM dbo.bcpPostgresTableDefinition C
    WHERE C.TableName = @TableName
    AND C.SchemaName = @Schema
    ORDER BY C.OrdinalPositon;

    INSERT INTO @Columns(ColumName, DataType, OrdinalPositon)
    SELECT C.COLUMN_NAME, C.DATA_TYPE, C.ORDINAL_POSITION
    FROM INFORMATION_SCHEMA.COLUMNS C
    WHERE C.TABLE_NAME = @TableName
    AND C.TABLE_SCHEMA = @Schema
    ORDER BY C.ORDINAL_POSITION;

    declare @ColumnName varchar(200);
    declare @DataType varchar(50);
    set @SQL = '';
    DECLARE ColumnCursor CURSOR FOR
    SELECT C.ColumName FROM @ColumnsPostgres C ORDER BY C.OrdinalPositon
    OPEN ColumnCursor
    FETCH NEXT FROM ColumnCursor INTO @ColumnName
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @SQL <> ''
        BEGIN
            SET @SQL = @SQL + ', ';
        END
        IF (SELECT COUNT(*) FROM @Columns C WHERE C.ColumName = @ColumnName) = 0
        begin
            SET @SQL = @SQL + 'NULL AS [' + @ColumnName + ']';
        end
        else
        begin
            SET @DataType = (SELECT C.DataType FROM @Columns C WHERE C.ColumName = @ColumnName)
            IF @DataType LIKE '%int'
            OR @DataType = 'numeric'
            OR @DataType = 'bit'
            OR @DataType = 'decimal'
            OR @DataType = 'decimal'
            OR @DataType LIKE '%money'
            OR @DataType = 'real'
            OR @DataType = 'float'
            OR @DataType LIKE 'date%'
            OR @DataType LIKE '%time'
            BEGIN
                SET @SQL = @SQL + @ColumnName;
            END
            ELSE
            BEGIN
                SET @SQL = @SQL + 'CASE WHEN [' + @ColumnName + '] IS NULL THEN NULL ELSE ''"'' + REPLACE([' + @ColumnName + '], ''"'', ''""'') + ''"'' END AS [' + @ColumnName + ']';
            END
        end
           FETCH NEXT FROM ColumnCursor INTO @ColumnName
    END
    CLOSE ColumnCursor
    DEALLOCATE ColumnCursor

    declare @i int
    set @i = 1;
    set @Schema = @Schema;
    set @view = @View;

    DECLARE @SqlCheck nvarchar(4000);
    DECLARE @ParameterDefinition nvarchar(500);
    SET @ParameterDefinition = N'@view varchar(200), @Schema varchar(200), @i INT OUTPUT';
    SET @SqlCheck = 'select @i = count(*) from INFORMATION_SCHEMA.VIEWS v where    v.TABLE_NAME =  @view  and v.TABLE_SCHEMA =  @Schema ';
    EXEC sys.sp_executesql
      @SqlCheck
      , @Params = @ParameterDefinition
      , @view = @view
      , @Schema = @Schema
      , @i = @i OUTPUT;


    if (@i = 1)
    begin
        DECLARE @SqlDrop nvarchar(4000);
        set @SqlDrop = 'DROP VIEW ' + @Schema + '.' + @View;
        exec sp_executesql @SqlDrop
    end

    set @SQL = 'CREATE VIEW ' + @Schema + '.' + @View + ' AS SELECT ' + @SQL + ' FROM ' + @Schema + '.' + @TableName;
    exec sp_executesql @SQL

END
GO

GRANT EXEC ON [dbo].[procBcpViewCreate] TO [CacheAdmin]
GO


-- =============================================
-- Author:		Anton Link
-- Create date: 2020/04/09
-- Description:	Export table data using bcp
-- =============================================
CREATE PROCEDURE [dbo].[procBcpExport] 
	@TableName varchar(200),
	@Schema varchar(200),
	@TargetPath varchar(200),
	@ProtocolFileName varchar(200)
WITH EXECUTE AS 'cmdshell'
AS 
DECLARE @RC int;
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	/*
	Test:
	DECLARE @RC int
	DECLARE @TableName varchar(200)
	DECLARE @Schema varchar(200)
	DECLARE @TargetPath varchar(200)
	DECLARE @ProtocolFileName varchar(200)

	SET @TableName = 'CacheCharacter'
	SET @Schema = 'Project_LIASlight'
	SET @TargetPath = 'U:\postgres\'
	SET @ProtocolFileName = 'Outfile.txt'

	EXECUTE @RC = [dbo].[procBcpExport] 
	   @TableName
	  ,@Schema
	  ,@TargetPath
	  ,@ProtocolFileName
	GO
	*/

	DECLARE @SQL nvarchar(500);
	DECLARE @BcpCmd varchar(500);
	DECLARE @ToFile varchar(200);
	DECLARE @CheckDatabase varchar(200);
	DECLARE @DatabaseName varchar(200);
	DECLARE @Server varchar(100);
	DECLARE @Port int;
	
	-- Retrieve database name
	SET @DatabaseName = (SELECT DB_NAME());
	SET @SQL = N'SELECT @CheckDatabase = [CATALOG_NAME] FROM [INFORMATION_SCHEMA].[SCHEMATA] WHERE [SCHEMA_NAME]=@Schema;';
	EXECUTE sp_executesql @SQL, N'@Schema varchar(200), @CheckDatabase varchar(200) OUTPUT', @Schema=@Schema, @CheckDatabase=@CheckDatabase OUTPUT;
	IF @DatabaseName <> @CheckDatabase
	BEGIN
		SET @RC = 2;
		RETURN @RC;
	END
	SET @RC = 1;

	-- Normalize target path
	SET @TargetPath = RTRIM(@TargetPath);
	IF ('\' <> RIGHT(@TargetPath, 1)) 
		SET @TargetPath = @TargetPath + '\';
	SET @ToFile = ' >> ' + @TargetPath + @DatabaseName + '\' + @ProtocolFileName;
	SET @TargetPath = @TargetPath + @DatabaseName + '\' + @Schema + '\';

	-- Write table name to protocol file
	SET @BcpCmd = 'ECHO ----------------------------------------------------------------------------------------------------' + @ToFile;
	EXEC xp_cmdshell @BcpCmd
	SET @BcpCmd = 'ECHO Table: ' + @Schema + '.' + @TableName + @ToFile;
	EXEC xp_cmdshell @BcpCmd

	-- Retrieve table using bcp
	SET @Server = (SELECT @@SERVERNAME);
	SET @BcpCmd = 'bcp ' + @DatabaseName + '.' + @Schema + '.TransferView_' + @TableName + ' out "' + @TargetPath + @TableName + '.csv" -w -q';
	SET @BcpCmd = @BcpCmd + ' -T -S"' + @Server + '"';
	SET @BcpCmd = @BcpCmd + @ToFile;
	EXEC @RC = xp_cmdshell @BcpCmd
END
RETURN @RC
GO

GRANT EXEC ON [dbo].[procBcpExport] TO [CacheAdmin]
GO


-- =============================================
-- Author:		Anton Link
-- Create date: 2020/04/14
-- Description:	Initialize bcp transfer
-- =============================================
CREATE PROCEDURE [dbo].[procBcpInitExport]
	@TargetPath varchar(200),
	@Schema varchar(200),
	@ProtocolFileName varchar(200)
WITH EXECUTE AS 'cmdshell'
AS 
DECLARE @RC int;
BEGIN
	SET NOCOUNT ON;
	/*
	Test:
	DECLARE @RC int
	DECLARE @TargetPath varchar(200)
	DECLARE @Schema varchar(200)
	DECLARE @ProtocolFileName varchar(200)
	SET @TargetPath = 'U:\postgres'
	SET @Schema = 'Project_LIASlight'
	SET @ProtocolFileName = 'Outfile.txt'
	EXECUTE @RC = [dbo].[procBcpInitExport] 
	@TargetPath, @Schema, @ProtocolFileName

	*/

	DECLARE @SQL nvarchar(500);
	DECLARE @BcpCmd varchar(500);
	DECLARE @ToFile varchar(200);
	DECLARE @CheckDatabase varchar(200);
	DECLARE @DatabaseName varchar(200);
	
	-- Retrieve database name
	SET @DatabaseName = (SELECT DB_NAME());
	SET @SQL = N'SELECT @CheckDatabase = [CATALOG_NAME] FROM [INFORMATION_SCHEMA].[SCHEMATA] WHERE [SCHEMA_NAME]=@Schema;';
	EXECUTE sp_executesql @SQL, N'@Schema varchar(200), @CheckDatabase varchar(200) OUTPUT', @Schema=@Schema, @CheckDatabase=@CheckDatabase OUTPUT;
	IF @DatabaseName <> @CheckDatabase
	BEGIN
		SET @RC = 2;
		RETURN @RC;
	END
	SET @RC = 1;

	-- Normalize target path
	SET @TargetPath = RTRIM(@TargetPath);
	IF ('\' <> RIGHT(@TargetPath, 1)) 
		SET @TargetPath = @TargetPath + '\';
	SET @DatabaseName = (SELECT [CATALOG_NAME] FROM [INFORMATION_SCHEMA].[SCHEMATA] WHERE [SCHEMA_NAME]=@Schema);
	SET @ToFile = ' >> ' + @TargetPath + @DatabaseName + '\' + @ProtocolFileName;
	--SET @TargetPath = @TargetPath + @DatabaseName + '\' + @Schema + '\';

	-- Create subdirectories
	SET @BcpCmd = 'md ' + @TargetPath + @DatabaseName;
	EXEC xp_cmdshell @BcpCmd

	SET @BcpCmd = 'md ' + @TargetPath + @DatabaseName + '\' + @Schema;
	EXEC xp_cmdshell @BcpCmd

	SET @BcpCmd = 'md ' + @TargetPath + @DatabaseName + '\dbo';
	EXEC xp_cmdshell @BcpCmd

	-- Create protocol file
	SET @BcpCmd = 'echo User: ' + (SELECT CONVERT(varchar, ORIGINAL_LOGIN())) + ', Start: ' +(SELECT CONVERT(varchar, SYSDATETIME())) + ' > '+ @TargetPath + @DatabaseName + '\' + @ProtocolFileName;
	EXEC xp_cmdshell @BcpCmd

	SET @BcpCmd = 'echo Proxy account: >> '+ @TargetPath + @DatabaseName + '\' + @ProtocolFileName;
	EXEC xp_cmdshell @BcpCmd

	SET @BcpCmd = 'whoami >> '+ @TargetPath + @DatabaseName + '\' + @ProtocolFileName;
	EXEC xp_cmdshell @BcpCmd

	SET @RC = 0;
END
RETURN @RC
GO

GRANT EXEC ON [dbo].[procBcpInitExport] TO [CacheAdmin]
GO


-- =============================================
-- Author:		Anton Link
-- Create date: 2020/04/17
-- Description:	Delete table file exported
--              by bcp
-- =============================================
CREATE PROCEDURE [dbo].[procBcpRemoveFile] 
	@TableName varchar(200),
	@Schema varchar(200),
	@TargetPath varchar(200)
WITH EXECUTE AS 'cmdshell'
AS 
DECLARE @RC int;
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	/*
	Test:
	DECLARE @RC int
	DECLARE @TableName varchar(200)
	DECLARE @Schema varchar(200)
	DECLARE @TargetPath varchar(200)
	
	SET @TableName = 'CacheCharacter'
	SET @Schema = 'Project_LIASlight'
	SET @TargetPath = 'U:\postgres\'
	
	EXECUTE @RC = [dbo].[procBcpRemoveFile] 
	   @TableName
	  ,@Schema
	  ,@TargetPath
	GO
	*/

    DECLARE @SQL nvarchar(500);
	DECLARE @BcpCmd varchar(500);
	DECLARE @CheckDatabase varchar(200);
	DECLARE @DatabaseName varchar(200);
	
	-- Retrieve database name
	SET @DatabaseName = (SELECT DB_NAME());
	SET @SQL = N'SELECT @CheckDatabase = [CATALOG_NAME] FROM [INFORMATION_SCHEMA].[SCHEMATA] WHERE [SCHEMA_NAME]=@Schema;';
	EXECUTE sp_executesql @SQL, N'@Schema varchar(200), @CheckDatabase varchar(200) OUTPUT', @Schema=@Schema, @CheckDatabase=@CheckDatabase OUTPUT;
	IF @DatabaseName <> @CheckDatabase
	BEGIN
		SET @RC = 2;
		RETURN @RC;
	END
	SET @RC = 1;

	-- Normalize target path
	SET @TargetPath = RTRIM(@TargetPath);
	IF ('\' <> RIGHT(@TargetPath, 1)) 
		SET @TargetPath = @TargetPath + '\';
	SET @TargetPath = @TargetPath + @DatabaseName + '\' + @Schema + '\';

	-- Retrieve table using bcp
	SET @BcpCmd = 'del "' + @TargetPath + @TableName + '.csv"';
	EXEC @RC = xp_cmdshell @BcpCmd
END
RETURN @RC
GO

GRANT EXEC ON [dbo].[procBcpRemoveFile] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   Target: Add TransferDirectory   ############################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[Target] ADD [TransferDirectory] [varchar](500) NULL;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Directory on the Postgres server used for the transfer of data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target', @level2type=N'COLUMN',@level2name=N'TransferDirectory'
GO

ALTER TABLE [dbo].[Target] ADD [BashFile] [varchar](500) NULL;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BashFile on the Postgres server used for conversion of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Target', @level2type=N'COLUMN',@level2name=N'BashFile'
GO

--#####################################################################################################################
--######   ProjectTarget: Add TransferDirectory   #####################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectTarget] ADD [UseBulkTransfer] bit NULL;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the bulk transfer should be used for the transfer of data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTarget', @level2type=N'COLUMN',@level2name=N'UseBulkTransfer'
GO


--#####################################################################################################################
--######   AgentImage     #############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES C where C.TABLE_NAME = 'AgentImage') = 0
begin
	CREATE TABLE [dbo].[AgentImage](
	[AgentID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[Sequence] [int] NULL,
	[SourceView] [varchar](128) NOT NULL,
	 CONSTRAINT [PK_AgentImage] PRIMARY KEY CLUSTERED 
	(
		[AgentID] ASC,
		[URI] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end
GO


--#####################################################################################################################
--######   AgentSource - Add Version  #################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'AgentSource' AND C.COLUMN_NAME = 'Version') = 0
begin
	ALTER TABLE dbo.AgentSource ADD
	Version int NULL

	ALTER TABLE dbo.AgentSource ADD CONSTRAINT
	DF_AgentSource_Version DEFAULT 0 FOR Version

end
GO

UPDATE [dbo].[AgentSource]
SET Version = 0
GO

--#####################################################################################################################
--######   GazetteerSource - Add Version  #############################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'GazetteerSource' AND C.COLUMN_NAME = 'Version') = 0
begin
	ALTER TABLE dbo.GazetteerSource ADD
	Version int NULL

	ALTER TABLE dbo.GazetteerSource ADD CONSTRAINT
	DF_GazetteerSource_Version DEFAULT 0 FOR Version

end
GO

UPDATE [dbo].[GazetteerSource]
SET Version = 0
GO

--#####################################################################################################################
--######   ReferenceTitleSource - Add Version  ########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ReferenceTitleSource' AND C.COLUMN_NAME = 'Version') = 0
begin
	ALTER TABLE dbo.ReferenceTitleSource ADD
	Version int NULL

	ALTER TABLE dbo.ReferenceTitleSource ADD CONSTRAINT
	DF_ReferenceTitleSource_Version DEFAULT 0 FOR Version

end
GO

UPDATE [dbo].[ReferenceTitleSource]
SET Version = 0
GO

--#####################################################################################################################
--######   SamplingPlotSource - Add Version  ##########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'SamplingPlotSource' AND C.COLUMN_NAME = 'Version') = 0
begin
	ALTER TABLE dbo.SamplingPlotSource ADD
	Version int NULL

	ALTER TABLE dbo.SamplingPlotSource ADD CONSTRAINT
	DF_SamplingPlotSource_Version DEFAULT 0 FOR Version

end
GO

UPDATE [dbo].[SamplingPlotSource]
SET Version = 0
GO

--#####################################################################################################################
--######   ScientificTermSource - Add Version  ########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ScientificTermSource' AND C.COLUMN_NAME = 'Version') = 0
begin
	ALTER TABLE dbo.ScientificTermSource ADD
	Version int NULL

	ALTER TABLE dbo.ScientificTermSource ADD CONSTRAINT
	DF_ScientificTermSource_Version DEFAULT 0 FOR Version

end
GO

UPDATE [dbo].[ScientificTermSource]
SET Version = 0
GO

--#####################################################################################################################
--######   TaxonSynonymySource - Add Version  #########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TaxonSynonymySource' AND C.COLUMN_NAME = 'Version') = 0
begin
	ALTER TABLE dbo.TaxonSynonymySource ADD
	Version int NULL

	ALTER TABLE dbo.TaxonSynonymySource ADD CONSTRAINT
	DF_TaxonSynonymySource_Version DEFAULT 0 FOR Version

end
GO

UPDATE [dbo].[TaxonSynonymySource]
SET Version = 0
GO



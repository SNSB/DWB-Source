
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.34'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######  Generic SetUserProjects  ####################################################################################
--#####################################################################################################################

/****** Object:  StoredProcedure [dbo].[SetUserProjects]    Script Date: 04.02.2021 15:23:35 ******/
IF 1 = (SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE [SPECIFIC_NAME]='SetUserProjects')
DROP PROCEDURE [dbo].[SetUserProjects]
GO

-- =============================================
-- Author:		Anton Link
-- Create date: 20210205
-- Description:	Create database user and assign
--              training projects     
-- =============================================
CREATE PROCEDURE [dbo].[SetUserProjects] 
	-- Add the parameters for the stored procedure here
	@User VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @Result NVARCHAR(200);
	DECLARE @DatabaseName NVARCHAR(MAX);
	DECLARE @DatabaseRole VARCHAR(100);
	DECLARE @DatabaseUser VARCHAR(100);
	DECLARE @Workshop INT;
	DECLARE @Windows INT;
	SET @DatabaseName=(SELECT DB_NAME());
	SET @DatabaseRole=(SELECT TOP 1 [name] FROM [sys].[database_principals] WHERE [type]='R' AND [name] LIKE '%Administrator');
	SET @Workshop=(SELECT COUNT(*) FROM [UserProxy] WHERE [LoginName]='Workshop');
	SET @Windows=CHARINDEX('\', REVERSE(@User));
	SET @Result=''; 

	IF @Windows=0
	BEGIN
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''S'' AND [name]=''' + @User + ''' ');
		EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;

		IF @DatabaseUser IS NULL
		BEGIN
  			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''U'' AND [name] LIKE ''%\' + @User + ''' ');
			EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;
		END
	END
	ELSE
	BEGIN
		SET @User=REVERSE(SUBSTRING(REVERSE(@User), 1, @Windows));
  		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''U'' AND [name] LIKE ''%' + @User + ''' ');
		EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;
	END
	
	IF @DatabaseUser IS NULL
	BEGIN
		SET @Result='User ' + @User + ' is not present in database ' + @DatabaseName;
	END
	ELSE
	BEGIN
		IF NOT @DatabaseRole IS NULL
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'ALTER ROLE [' + @DatabaseRole + '] ADD MEMBER [' + @DatabaseUser + ']; ')
			EXEC sp_executesql @SQL
			SET @Result='Database role ' + @DatabaseRole + ' set for user ' + @DatabaseUser;
		END
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'DELETE FROM [dbo].[ProjectUser] WHERE [LoginName]=''' + @DatabaseUser + ''' ')
		EXEC sp_executesql @SQL
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'DELETE FROM [dbo].[UserProxy] WHERE [LoginName]=''' + @DatabaseUser + ''' ')
		EXEC sp_executesql @SQL

 		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'INSERT INTO [dbo].[UserProxy] ([LoginName], [CombinedNameCache]) VALUES (''' + @DatabaseUser + ''', ''' + @DatabaseUser + '''); ')
		EXEC sp_executesql @SQL

		IF @Workshop > 0
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'INSERT INTO [dbo].[ProjectUser] ([LoginName], [ProjectID]) ' +
							 'SELECT ''' + @DatabaseUser + ''', [ProjectID] FROM [dbo].[ProjectUser] WHERE [LoginName]=''Workshop''; ')
			EXEC sp_executesql @SQL
			SET @Result=@Result + '; Workshop projects assigned for user ' + @DatabaseUser;
		END
		ELSE
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'INSERT INTO [dbo].[ProjectUser] ([LoginName], [ProjectID]) ' +
							 'SELECT ''' + @DatabaseUser + ''', [ProjectID] FROM [dbo].[ProjectProxy] WHERE [ProjectID]>0; ')
			EXEC sp_executesql @SQL
			SET @Result=@Result + '; Available projects assigned for user ' + @DatabaseUser;
		END
	END
	SELECT @Result
END
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '04.04.00' 
END

GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.35'
END

GO

SELECT [dbo].[Version] ()  

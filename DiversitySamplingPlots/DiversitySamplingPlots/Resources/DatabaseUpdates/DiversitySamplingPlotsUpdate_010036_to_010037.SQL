
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.36'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   ViewLocality for query   ###################################################################################
--#####################################################################################################################

CREATE VIEW ViewLocality AS
SELECT        S.PlotID, S.Location1 AS Locality
FROM            SamplingPlotLocalisation AS S INNER JOIN
                         LocalisationSystem AS L ON S.LocalisationSystemID = L.LocalisationSystemID
WHERE        (L.ParsingMethodName = N'Gazetteer')

GO

GRANT SELECT ON  ViewLocality TO [DiversityWorkbenchUser]
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Gazetteer data for query',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewLocality'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Gazetteer data for query',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewLocality'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of column Location1',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewLocality',
@level2type=N'COLUMN', @level2name=N'Locality'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of column Location1',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewLocality',
@level2type=N'COLUMN', @level2name=N'Locality'
END CATCH;
GO


--#####################################################################################################################
--######   SamplingPlotListForCurrentProject - add columns PlotType and CountryCache   ################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[SamplingPlotListForCurrentProject] ()  
RETURNS @SamplingPlotList TABLE ([PlotID] [int] Primary key ,
	[PartOfPlotID] [int] NULL,
	[PlotIdentifier] [nvarchar](500) NOT NULL,
	[PlotGeography_Cache] [geography] NULL,
	[PlotGeometry_Cache] [geometry] NULL,
	[PlotDescription] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[DisplayText] [nvarchar](max) NOT NULL,
	[PlotType] [nvarchar](50) NULL,
	[CountryCache] [nvarchar](50) NULL,
	[ProjectID] [int] NOT NULL)
/*
Returns a table that lists all the SamplingPlot items related to the given project.
MW 26.07.2013
TEST:
SELECT * FROM DBO.SamplingPlotListForCurrentProject()
*/
AS
BEGIN
declare @ProjectID int
set @ProjectID = (SELECT [dbo].[DefaultProjectID] ())
if (@ProjectID IS NULL)
begin
	set @ProjectID = (select min(ProjectID) from ProjectList)
end
INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, PlotType, CountryCache, ProjectID)
SELECT     S.PlotID, S.PartOfPlotID, S.PlotIdentifier, S.PlotGeography_Cache, S.PlotGeometry_Cache, S.PlotDescription, S.InternalNotes, S.PlotIdentifier AS DisplayText, PlotType, CountryCache, @ProjectID
FROM         SamplingPlot AS S INNER JOIN
                      SamplingProject AS P ON S.PlotID = P.PlotID
WHERE     (S.PartOfPlotID IS NULL) AND (P.ProjectID = @ProjectID)
declare @i int
set @i = (select count(*) from SamplingPlot where PlotID not IN (select PlotID from  @SamplingPlotList))
while (@i > 0)
	begin
	INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, PlotType, CountryCache, ProjectID)
	SELECT C.PlotID, C.PartOfPlotID, C.PlotIdentifier, C.PlotGeography_Cache, C.PlotGeometry_Cache, C.PlotDescription, C.InternalNotes, L.DisplayText + ' | ' + C.PlotIdentifier AS DisplayText, C.PlotType, C.CountryCache, @ProjectID
	FROM SamplingPlot C, @SamplingPlotList L, SamplingProject P
	WHERE C.PartOfPlotID = L.PlotID
	AND C.PlotID NOT IN (select PlotID from @SamplingPlotList)
	AND (P.ProjectID = @ProjectID)
	AND C.PlotID = P.PlotID
	set @i = (SELECT     count(*)FROM SamplingPlot C, @SamplingPlotList L, SamplingProject P
	WHERE C.PartOfPlotID = L.PlotID
	AND C.PlotID NOT IN (select PlotID from @SamplingPlotList)
	AND (P.ProjectID = @ProjectID)
	AND C.PlotID = P.PlotID)
end
set @i = (select count(*) from SamplingProject where ProjectID = @ProjectID AND PlotID not IN (select PlotID from  @SamplingPlotList))
if (@i > 0)
begin
	declare @MissingSamplingPlotList TABLE ([PlotID] [int] Primary key ,
		[PartOfPlotID] [int] NULL,
		[PlotIdentifier] [nvarchar](500) NOT NULL,
		[PlotGeography_Cache] [geography] NULL,
		[PlotGeometry_Cache] [geometry] NULL,
		[PlotDescription] [nvarchar](max) NULL,
		[InternalNotes] [nvarchar](max) NULL,
		[DisplayText] [nvarchar](max) NOT NULL,
		[PlotType] [nvarchar](50) NULL,
		[CountryCache] [nvarchar](50) NULL,
		[ParentPlotID] [int] NULL)
	INSERT @MissingSamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, PlotType, CountryCache, ParentPlotID)
	SELECT     S.PlotID, S.PartOfPlotID, S.PlotIdentifier, S.PlotGeography_Cache, S.PlotGeometry_Cache, S.PlotDescription, S.InternalNotes, S.PlotIdentifier AS DisplayText, S.PlotType, S.CountryCache, S.PartOfPlotID
	FROM         SamplingPlot AS S INNER JOIN
						  SamplingProject AS P ON S.PlotID = P.PlotID
	WHERE    (P.ProjectID = @ProjectID)
	set @i = (select COUNT(*) from @MissingSamplingPlotList M where not M.ParentPlotID is null)
	while @i > 0
	begin
		update M SET M.DisplayText = S.PlotIdentifier + ' | ' + M.DisplayText, M.ParentPlotID = S.PartOfPlotID 
		from @MissingSamplingPlotList M, SamplingPlot S
		where M.ParentPlotID = S.PlotID
		set @i = (select COUNT(*) from @MissingSamplingPlotList M where not M.ParentPlotID is null)
	end
	INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, PlotType, CountryCache, ProjectID)
	SELECT PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, PlotType, CountryCache, @ProjectID
	FROM @MissingSamplingPlotList M
	WHERE M.PlotID NOT IN (SELECT PlotID FROM @SamplingPlotList)
end
   RETURN
END

GO



--#####################################################################################################################
--######   ViewSamplingPlotListForCurrentProject - adding missing columns PlotType and CountryCache  ##################
--#####################################################################################################################

ALTER VIEW [dbo].[ViewSamplingPlotListForCurrentProject]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, PlotType, CountryCache, ProjectID
FROM            dbo.SamplingPlotListForCurrentProject() AS ViewSamplingPlotListForCurrentProject
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
--######   Missing descriptions   #####################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the center of the plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Latitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the center of the plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Latitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the center of the plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Longitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the center of the plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Longitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'maximum angle between the center and the circumference as string',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Accuracy angle'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'maximum angle between the center and the circumference as string',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Accuracy angle'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'maximum angle between the center and the circumference as numeric value',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'AccuracyAngle'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'maximum angle between the center and the circumference as numeric value',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'AccuracyAngle'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Area in square meters',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'AreaM2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Area in square meters',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'AreaM2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Area including unit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Area'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Area including unit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Area'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Geography of the plot or a parent plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Geography'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Geography of the plot or a parent plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Geography'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of function BaseURL',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL',
@level2type=N'COLUMN', @level2name=N'BaseURL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of function BaseURL',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL',
@level2type=N'COLUMN', @level2name=N'BaseURL'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the center of the plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Latitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Latitude of the center of the plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Latitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the center of the plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Longitude'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Longitude of the center of the plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Longitude'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'maximum angle between the center and the circumference as string',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Accuracy angle'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'maximum angle between the center and the circumference as string',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Accuracy angle'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'maximum angle between the center and the circumference as numeric value',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'AccuracyAngle'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'maximum angle between the center and the circumference as numeric value',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'AccuracyAngle'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Area in square meters',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'AreaM2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Area in square meters',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'AreaM2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Area including unit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Area'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Area including unit',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Area'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Geography of the plot or a parent plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Geography'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Geography of the plot or a parent plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'Geography'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'URL as retrieved from BaseURL + PlotID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'URL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'URL as retrieved from BaseURL + PlotID',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo',
@level2type=N'COLUMN', @level2name=N'URL'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'PlotIdentifiers from current and parent plots separated by | ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'PlotIdentifiers from current and parent plots separated by | ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'PlotID for which the children should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'PlotID for which the children should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'List of comma separated PlotIDs for which the geography should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotGeography',
@level2type=N'PARAMETER', @level2name=N'@PlotIDs'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'List of comma separated PlotIDs for which the geography should be returned',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotGeography',
@level2type=N'PARAMETER', @level2name=N'@PlotIDs'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'reserved for future, not used in current version',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertSamplingPlotCopy',
@level2type=N'PARAMETER', @level2name=N'@PlotIdentifier'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'reserved for future, not used in current version',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertSamplingPlotCopy',
@level2type=N'PARAMETER', @level2name=N'@PlotIdentifier'
END CATCH;
GO

IF 1 = (SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE [SPECIFIC_NAME]='SetUserProjects')
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Create database user and assign training projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Create database user and assign training projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects'
END CATCH;
GO

IF 1 = (SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE [SPECIFIC_NAME]='SetUserProjects')
BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'database user to assign training projects for',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects',
@level2type=N'PARAMETER', @level2name=N'@User'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'database user to assign training projects for',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'SetUserProjects',
@level2type=N'PARAMETER', @level2name=N'@User'
END CATCH;
GO



--#####################################################################################################################
--######  Missing Grants  #############################################################################################
--#####################################################################################################################

--BaseURL

GRANT EXEC ON [dbo].[BaseURL] TO [DiversityWorkbenchUser]
GO

--VersionClient

GRANT EXEC ON [dbo].[VersionClient] TO [DiversityWorkbenchUser]
GO

--SamplingPlotAllProperties

GRANT SELECT ON [dbo].[SamplingPlotAllProperties] TO [DiversityWorkbenchUser]
GO


--SamplingPlotAllLocalisations

GRANT SELECT ON [dbo].[SamplingPlotAllLocalisations] TO [DiversityWorkbenchUser]
GO


--ViewSamplingPlotAllLocalisations

GRANT SELECT ON [dbo].[ViewSamplingPlotAllLocalisations] TO [DiversityWorkbenchUser]
GO


--SamplingProject_log

GRANT SELECT ON [dbo].[SamplingProject_log] TO [DiversityWorkbenchEditor]
GO

GRANT INSERT ON [dbo].[SamplingProject_log] TO [DiversityWorkbenchEditor]
GO


--SamplingPlotProperty

GRANT SELECT ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchUser]
GO

--SamplingPlotProperty

GRANT SELECT ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchEditor]
GO

GRANT INSERT ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchEditor]
GO

GRANT UPDATE ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchEditor]
GO

GRANT DELETE ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchEditor]
GO

GRANT SELECT ON [dbo].[SamplingPlotProperty_log] TO [DiversityWorkbenchEditor]
GO

GRANT INSERT ON [dbo].[SamplingPlotProperty_log] TO [DiversityWorkbenchEditor]
GO


--SamplingPlotLocalisation


GRANT SELECT ON [dbo].[SamplingPlotLocalisation_log] TO [DiversityWorkbenchEditor]
GO

GRANT INSERT ON [dbo].[SamplingPlotLocalisation_log] TO [DiversityWorkbenchEditor]
GO

--Property

GRANT SELECT ON [dbo].[Property] TO [DiversityWorkbenchEditor]
GO

GRANT INSERT ON [dbo].[Property] TO [DiversityWorkbenchAdministrator]
GO

GRANT UPDATE ON [dbo].[Property] TO [DiversityWorkbenchAdministrator]
GO

GRANT DELETE ON [dbo].[Property] TO [DiversityWorkbenchAdministrator]
GO

GRANT SELECT ON [dbo].[Property_log] TO [DiversityWorkbenchAdministrator]
GO

GRANT INSERT ON [dbo].[Property_log] TO [DiversityWorkbenchAdministrator]
GO


--LocalisationSystem_log

GRANT SELECT ON [dbo].[LocalisationSystem_log] TO [DiversityWorkbenchAdministrator]
GO

GRANT INSERT ON [dbo].[LocalisationSystem_log] TO [DiversityWorkbenchAdministrator]
GO


--ReplicationPublisher

GRANT SELECT ON [dbo].[ReplicationPublisher] TO [DiversityWorkbenchEditor]
GO

GRANT INSERT ON [dbo].[ReplicationPublisher] TO [DiversityWorkbenchAdministrator]
GO

GRANT UPDATE ON [dbo].[ReplicationPublisher] TO [DiversityWorkbenchAdministrator]
GO

GRANT DELETE ON [dbo].[ReplicationPublisher] TO [DiversityWorkbenchAdministrator]
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
RETURN '01.00.37'
END

GO

SELECT [dbo].[Version] ()  

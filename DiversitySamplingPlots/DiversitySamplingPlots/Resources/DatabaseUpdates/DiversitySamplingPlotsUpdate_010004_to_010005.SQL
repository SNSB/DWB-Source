
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.04'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [SamplingPlotListWithProject]   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[SamplingPlotListForCurrentProject] ()  
RETURNS @SamplingPlotList TABLE ([PlotID] [int] Primary key ,
	[PartOfPlotID] [int] NULL,
	[PlotIdentifier] [nvarchar](500) NOT NULL,
	[PlotGeography_Cache] [geography] NULL,
	[PlotGeometry_Cache] [geometry] NULL,
	[PlotDescription] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[DisplayText] [nvarchar](max) NOT NULL)
/*
Returns a table that lists all the SamplingPlot items related to the given project.
MW 26.07.2013
TEST:
SELECT * FROM DBO.SamplingPlotListWithProject(0)
*/
AS
BEGIN
declare @ProjectID int
set @ProjectID = (SELECT [dbo].[DefaultProjectID] ())

-- getting the TopIDs
INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText)
SELECT     S.PlotID, S.PartOfPlotID, S.PlotIdentifier, S.PlotGeography_Cache, S.PlotGeometry_Cache, S.PlotDescription, S.InternalNotes, S.PlotIdentifier AS DisplayText
FROM         SamplingPlot AS S INNER JOIN
                      SamplingProject AS P ON S.PlotID = P.PlotID
WHERE     (S.PartOfPlotID IS NULL) AND (P.ProjectID = @ProjectID)

declare @i int
set @i = (select count(*) from SamplingPlot where PlotID not IN (select PlotID from  @SamplingPlotList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText)
	SELECT C.PlotID, C.PartOfPlotID, C.PlotIdentifier, C.PlotGeography_Cache, C.PlotGeometry_Cache, C.PlotDescription, C.InternalNotes, L.DisplayText + ' | ' + C.PlotIdentifier AS DisplayText
	FROM SamplingPlot C, @SamplingPlotList L, SamplingProject P
	WHERE C.PartOfPlotID = L.PlotID
	AND C.PlotID NOT IN (select PlotID from @SamplingPlotList)
	AND (P.ProjectID = @ProjectID)
	AND C.PlotID = P.PlotID

	set @i = (SELECT     count(*)FROM         SamplingProject AS PP INNER JOIN
	SamplingPlot AS SP INNER JOIN
	SamplingProject AS P INNER JOIN
	SamplingPlot AS S ON P.PlotID = S.PlotID ON SP.PlotID = S.PartOfPlotID ON PP.PlotID = SP.PlotID LEFT OUTER JOIN
	SamplingPlot AS L ON P.PlotID = L.PlotID AND P.PlotID = L.PlotID
	WHERE     (L.PlotID IS NULL) AND (P.ProjectID = @ProjectID) AND (PP.ProjectID = @ProjectID))

end


   RETURN
END


GO

GRANT SELECT ON dbo.[SamplingPlotListForCurrentProject] TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   [CurrentProjectID]   ######################################################################################
--#####################################################################################################################


ALTER TABLE dbo.UserProxy ADD CurrentProjectID int NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The current project ID selected by the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'CurrentProjectID'
GO


--#####################################################################################################################
--######   [DefaultProjectID]   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[DefaultProjectID] ()
RETURNS int
AS
BEGIN
declare @i int
set @i = (SELECT CurrentProjectID from UserProxy where LoginName = USER_NAME())
if @i is null
	begin set @i =(	SELECT min([ProjectID])
	  FROM [dbo].[ProjectUser] p where p.LoginName = USER_NAME())
	end
return @i
END


GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.05'
END

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.21'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  SamplingPlotListForCurrentProject ###########################################################################
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
    [ProjectID] [int] NOT NULL)
/*
Returns a table that lists all the SamplingPlot items related to the given project.
MW 06.06.2016
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
INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, ProjectID)
SELECT     S.PlotID, S.PartOfPlotID, S.PlotIdentifier, S.PlotGeography_Cache, S.PlotGeometry_Cache, S.PlotDescription, S.InternalNotes, S.PlotIdentifier AS DisplayText, @ProjectID
FROM         SamplingPlot AS S INNER JOIN
                      SamplingProject AS P ON S.PlotID = P.PlotID
WHERE     (S.PartOfPlotID IS NULL) AND (P.ProjectID = @ProjectID)
declare @i int
set @i = (select count(*) from SamplingPlot where PlotID not IN (select PlotID from  @SamplingPlotList))
while (@i > 0)
    begin
    INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, ProjectID)
    SELECT C.PlotID, C.PartOfPlotID, C.PlotIdentifier, C.PlotGeography_Cache, C.PlotGeometry_Cache, C.PlotDescription, C.InternalNotes, L.DisplayText + ' | ' + C.PlotIdentifier AS DisplayText, @ProjectID
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
        [ParentPlotID] [int] NULL)
    INSERT @MissingSamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, ParentPlotID)
    SELECT     S.PlotID, S.PartOfPlotID, S.PlotIdentifier, S.PlotGeography_Cache, S.PlotGeometry_Cache, S.PlotDescription, S.InternalNotes, S.PlotIdentifier AS DisplayText, S.PartOfPlotID
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
    INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, ProjectID)
    SELECT PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, @ProjectID
    FROM @MissingSamplingPlotList M
    WHERE M.PlotID NOT IN (SELECT PlotID FROM @SamplingPlotList)
end
   RETURN
END

GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.07.00' 
END

GO

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.22'
END

GO

select [dbo].[Version] ()  

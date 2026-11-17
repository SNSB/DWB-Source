
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.03'
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


CREATE FUNCTION [dbo].[SamplingPlotListWithProject] (@ProjectID [int])  
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

/*
INSERT INTO SamplingPlot
                      (PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes,)
SELECT     PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes
FROM         SamplingPlot
*/


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

	set @i = (select count(*) from SamplingPlot S, SamplingProject P where S.PlotID = P.PlotID AND P.ProjectID = @ProjectID AND S.PlotID not IN (select PlotID from  @SamplingPlotList))
end


   RETURN
END


GO

GRANT SELECT ON dbo.SamplingPlotListWithProject TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.04'
END

GO


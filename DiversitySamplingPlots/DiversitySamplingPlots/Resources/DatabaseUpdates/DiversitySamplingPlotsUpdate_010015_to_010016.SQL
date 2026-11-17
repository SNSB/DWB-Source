
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.15'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   [Procedure procInsertSamplingPlotCopy update]   ############################################################
--#####################################################################################################################


ALTER  PROCEDURE [dbo].[procInsertSamplingPlotCopy]
    (@PlotID int output ,
    @OriginalPlotID int ,
    @PlotIdentifier  nvarchar(50))
AS
declare @count int

-- Sampling Plot
INSERT INTO SamplingPlot
                      (PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes)
SELECT     PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes
FROM         SamplingPlot
WHERE     (PlotID = @OriginalPlotID)

SELECT @PlotID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])


-- SamplingPlotImage
INSERT INTO SamplingPlotImage
                      (PlotID, URI, ResourceURI, ImageType, Notes)
SELECT     @PlotID, URI, ResourceURI, ImageType, Notes
FROM         SamplingPlotImage
WHERE     (PlotID = @OriginalPlotID)


-- SamplingProject
INSERT INTO SamplingProject
                      (PlotID, ProjectID)
SELECT     @PlotID, ProjectID
FROM         SamplingProject
WHERE     (PlotID = @OriginalPlotID)


SELECT @PlotID


GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.16'
END


GO

select [dbo].[Version] ()  



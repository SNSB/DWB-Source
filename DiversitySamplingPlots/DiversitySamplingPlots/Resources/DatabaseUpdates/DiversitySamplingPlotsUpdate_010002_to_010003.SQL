
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.02'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [[SamplingPlotChildNodes]]   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[SamplingPlotChildNodes] (@ID int)  
RETURNS @ItemList TABLE (PlotID int primary key,
	PartOfPlotID int NULL ,
	PlotIdentifier nvarchar (500)   NULL  )
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 26.02.2008

new optimized version: 04.06.2013
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (PlotID int primary key,
	PartOfPlotID int NULL ,
	PlotIdentifier nvarchar (500)   NULL )

	INSERT @TempItem (PlotID , PartOfPlotID, PlotIdentifier ) 
		SELECT PlotID , PartOfPlotID, PlotIdentifier 
		FROM SamplingPlot WHERE PartOfPlotID = @ID 
		
	declare @i int
	set @i = (select COUNT(*) from @TempItem)
	while @i > 0
	begin
		insert into @TempItem (PlotID , PartOfPlotID, PlotIdentifier ) 
		SELECT P.PlotID , P.PartOfPlotID, P.PlotIdentifier 
		FROM SamplingPlot P, @TempItem T 
		WHERE P.PartOfPlotID = T.PlotID AND P.PlotID NOT IN (Select PlotID from @TempItem)

		set @i = (select COUNT(*) from SamplingPlot P, @TempItem T WHERE P.PartOfPlotID = T.PlotID AND P.PlotID NOT IN (Select PlotID from @TempItem))
	end

 INSERT @ItemList (PlotID , PartOfPlotID, PlotIdentifier) 
   SELECT distinct PlotID , PartOfPlotID, PlotIdentifier 
   FROM @TempItem ORDER BY PlotIdentifier
   RETURN
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.03'
END

GO


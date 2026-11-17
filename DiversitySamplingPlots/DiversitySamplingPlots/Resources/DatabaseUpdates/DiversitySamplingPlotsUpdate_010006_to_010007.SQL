
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.06'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   [SamplingPlotChildNodes]   ######################################################################################
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
		ORDER BY PlotIdentifier
		
	declare @i int
	set @i = (select COUNT(*) from @TempItem)
	
	while @i > 0
	begin
		insert into @TempItem (PlotID , PartOfPlotID, PlotIdentifier ) 
		SELECT P.PlotID , P.PartOfPlotID, P.PlotIdentifier 
		FROM SamplingPlot P, @TempItem T 
		WHERE P.PartOfPlotID = T.PlotID AND P.PlotID NOT IN (Select PlotID from @TempItem)
		ORDER BY P.PlotIdentifier
		
		set @i = (select COUNT(*) from SamplingPlot P, @TempItem T WHERE P.PartOfPlotID = T.PlotID AND P.PlotID NOT IN (Select PlotID from @TempItem))
	end
	
 INSERT @ItemList (PlotID , PartOfPlotID, PlotIdentifier) 
   SELECT distinct PlotID , PartOfPlotID, PlotIdentifier 
   FROM @TempItem ORDER BY PlotIdentifier
   RETURN
END

GO

--#####################################################################################################################
--######   [SamplingPlotHierarchy]   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[SamplingPlotHierarchy] (@PlotID int)  
RETURNS @List TABLE ([PlotID] [int] Primary key ,
	[PartOfPlotID] [int] NULL ,
	[PlotIdentifier] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the analysis items related to the given analysis.
MW 26.02.2008
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int

set @TopID = (select PartOfPlotID from SamplingPlot where PlotID = @PlotID) 

set @i = (select count(*) from SamplingPlot where PlotID = @PlotID)

if (@TopID is null )
	set @TopID =  @PlotID
else	
	begin
	while (@i > 0)
		begin
		set @PlotID = (select PartOfPlotID from SamplingPlot where PlotID = @PlotID and not PartOfPlotID is null) 
		set @i = (select count(*) from SamplingPlot where PlotID = @PlotID and not PartOfPlotID is null)
		end
	set @TopID = @PlotID
	end


-- get the ID's of the child nodes
-- copy the root node in the result list
   INSERT @List
   SELECT DISTINCT PlotID, PartOfPlotID, PlotIdentifier
   FROM SamplingPlot
   WHERE SamplingPlot.PlotID = @TopID
   ORDER BY PlotIdentifier

-- copy the child nodes into the result list
   INSERT @List
   SELECT * FROM dbo.SamplingPlotChildNodes (@TopID)

   RETURN
END

GO

--#####################################################################################################################
--######   UserProxy  ######################################################################################
--#####################################################################################################################

GRANT UPDATE ON UserProxy TO DiversityWorkbenchUser
GO

GRANT SELECT ON UserProxy TO DiversityWorkbenchUser
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.07'
END

GO


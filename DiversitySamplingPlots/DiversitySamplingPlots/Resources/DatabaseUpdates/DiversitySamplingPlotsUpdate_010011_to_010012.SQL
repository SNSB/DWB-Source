
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.11'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   SamplingPlotGeography   ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[SamplingPlotGeography] (@PlotIDs varchar(8000) )  
RETURNS @SamplingPlotGeography TABLE ([PlotID] [int] Primary key ,
	[PartOfPlotID] [int] NULL ,
	[Geography] [geography])
/*
Returns a table that lists all plots togeher with the geography possibly derived from the parent plot.
MW 14.02.2014
Test: SELECT * FROM [dbo].[SamplingPlotGeography] ('-479,-478,-477,-476,-475,-474,-473,-472,-471,-470,-469,-468,-467,-466,-465,-464,-463,-766,-765,-764,-763,-762,-761,-760,-759,-758,-757,-756,-755,-754,-753,-752,-751,-750,-749,-748,-747,-746,-745,-744,-743,-742,-741,-740,-739,-738,-737,-736,-735,-734,-733,-732,-731,-730,-729,-728,-727,-726,-725,-724,-723,-722,-721,-720,-719,-718,-717,-716,-715,-714,-713,-712,-711,-710,-709,-708,-707,-706,-705,-704,-703,-702')
Test: SELECT *, Geography.EnvelopeCenter().Lat, Geography.EnvelopeCenter().Long FROM [dbo].[SamplingPlotGeography] ('-480,-199,-479,-478,-477,-476,-475,-474,-473,-472,-471,-470,-469,-468,-467,-466,-465,-464,-463,-766,-765,-764,-763,-762,-761,-760,-759,-758,-757,-756,-755,-754,-753,-752,-751,-750,-749,-748,-747,-746,-745,-744,-743,-742,-741,-740,-739,-738,-737,-736,-735,-734,-733,-732,-731,-730,-729,-728,-727,-726,-725,-724,-723,-722,-721,-720,-719,-718,-717,-716,-715,-714,-713,-712,-711,-710,-709,-708,-707,-706,-705,-704,-703,-702')
*/
AS
BEGIN
	-- Getting the list of IDs
	declare @IDs table (ID int  Primary key)
	declare @sID varchar(50)
	while @PlotIDs <> ''
	begin
		if (CHARINDEX(',', @PlotIDs) > 0)
		begin
		set @sID = rtrim(ltrim(SUBSTRING(@PlotIDs, 1, CHARINDEX(',', @PlotIDs) -1)))
		set @PlotIDs = rtrim(ltrim(SUBSTRING(@PlotIDs, CHARINDEX(',', @PlotIDs) + 1, 8000)))
		if (isnumeric(@sID) = 1)
			begin
			insert into @IDs 
			values( @sID )
			end
		end
		else
		begin
		set @sID = rtrim(ltrim(@PlotIDs))
		set @PlotIDs = ''
		if (isnumeric(@sID) = 1)
			begin
			insert into @IDs 
			values( @sID )
			end
		end
	end
	-- Getting all plots
	INSERT INTO @SamplingPlotGeography 
	SELECT [PlotID]
      ,[PartOfPlotID]
      ,[PlotGeography_Cache]
	  FROM [SamplingPlot] S, @IDs I
	  WHERE S.PlotID = I.ID

	WHILE (SELECT COUNT(*) FROM @IDs) > 0
	BEGIN
		-- remove solved datasets
		DELETE I
		FROM @IDs I, @SamplingPlotGeography S
		WHERE S.PlotID = I.ID
		AND NOT S.Geography IS NULL	
		-- remove missing parents
		DELETE I
		FROM @IDs I, @SamplingPlotGeography S
		WHERE S.PlotID = I.ID
		AND S.PartOfPlotID IS NULL	
		-- derive geography from parent
		UPDATE G SET Geography = S.PlotGeography_Cache
		FROM @SamplingPlotGeography G, [SamplingPlot] S
		WHERE S.PlotID = G.PartOfPlotID
		AND G.Geography IS NULL
		AND NOT S.PlotGeography_Cache IS NULL
		-- update to next parent
		UPDATE G SET PartOfPlotID = S.PartOfPlotID
		FROM @SamplingPlotGeography G, [SamplingPlot] S
		WHERE S.PlotID = G.PartOfPlotID
	END
RETURN	
END

GO

GRANT SELECT ON [dbo].[SamplingPlotGeography] TO DiversityWorkbenchUser
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.05.00' END

GO

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.12'
END

GO


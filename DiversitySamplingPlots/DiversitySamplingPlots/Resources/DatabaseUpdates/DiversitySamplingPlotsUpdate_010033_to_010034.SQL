
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.33'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Alter function SamplingPlotAllLocalisations to disable inherited localisations  ############################
--#####################################################################################################################
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotAllLocalisations]    Script Date: 13.08.2019 10:17:06   ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[SamplingPlotAllLocalisations] ()  
RETURNS @PlotLocalityPresent TABLE (
	[PlotID] int,
    [PartOfPlotID] [int],
    [LocalisationSystemID] int,
    [Location1] nvarchar(255),
	[Location2] nvarchar(255),
    [DisplayText] [nvarchar] (255) NULL,
	PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[LocalisationSystemID] ASC
))
/*
Returns a table that lists all the localisations of a plot included those inherited from a parent plot
MW 29.04.2019
select * from [dbo].[SamplingPlotAllLocalisations] ()  P order by PlotID desc
*/

AS
BEGIN
-- insert the entries as entered by the user
insert into @PlotLocalityPresent(
LocalisationSystemID, Location1, Location2, PlotID, PartOfPlotID, DisplayText)
select distinct 
P.LocalisationSystemID, P.Location1, P.Location2, S.PlotID, S.PartOfPlotID, L.DisplayText
from SamplingPlot S, SamplingPlotLocalisation P, LocalisationSystem L
where S.PlotID = P.PlotID and P.LocalisationSystemID = L.LocalisationSystemID

-- count the missing child entries
declare @i int

set @i = (select count(*) 
from SamplingPlot S, @PlotLocalityPresent P 
where S.PartOfPlotID = P.PlotID -- children plots in S
and NOT EXISTS (select * from @PlotLocalityPresent PPP where S.PlotID = PPP.PlotID and P.LocalisationSystemID = PPP.LocalisationSystemID)) -- where the property is missing

while @i > 0
begin

	insert into @PlotLocalityPresent(LocalisationSystemID, PlotID, Location1, Location2, PartOfPlotID, DisplayText)
	select distinct P.LocalisationSystemID, S.PlotID, P.Location1, P.Location2, S.PartOfPlotID, P.DisplayText 
	from SamplingPlot S, @PlotLocalityPresent P
	where P.PlotID = S.PartOfPlotID
	and NOT EXISTS (select * from @PlotLocalityPresent PPP where S.PlotID = PPP.PlotID and P.LocalisationSystemID = PPP.LocalisationSystemID)

	set @i = (select count(*) 
	from SamplingPlot S, @PlotLocalityPresent P 
	where S.PartOfPlotID = P.PlotID -- children plots in S
	and NOT EXISTS (select * from @PlotLocalityPresent PPP where S.PlotID = PPP.PlotID and P.LocalisationSystemID = PPP.LocalisationSystemID)) -- where the property is missing

end

RETURN
END

GO

--#####################################################################################################################
--######   Alter function SamplingPlotAllProperties to disable inherited properties  ##################################
--#####################################################################################################################
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotAllProperties]    Script Date: 13.08.2019 10:20:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[SamplingPlotAllProperties] ()  
RETURNS @PlotPropertyPresent TABLE ([PropertyID] [int],
    [PlotID] [int],
	[PropertyURI] varchar(255),
    [PartOfPlotID] [int],
    [DisplayText] [nvarchar] (255) NULL,
    [PropertyHierarchyCache] [nvarchar] (max) NULL)
/*
Returns a table that lists all the properties of a plot included those inherited from a parent plot
MW 18.12.2017
select * from [dbo].[SamplingPlotAllProperties] ()  P order by PlotID desc
*/

AS
BEGIN
-- insert the entries as entered by the user
insert into @PlotPropertyPresent(PropertyID, PlotID, PropertyURI, PartOfPlotID, DisplayText, PropertyHierarchyCache)
select distinct P.PropertyID, S.PlotID, P.PropertyURI, S.PartOfPlotID, P.DisplayText, P.PropertyHierarchyCache
from SamplingPlot S, SamplingPlotProperty P
where S.PlotID = P.PlotID
/* enabled again */
-- count the missing child entries
declare @i int
set @i = (select count(*) from SamplingPlot S, @PlotPropertyPresent P where S.PartOfPlotID = P.PlotID -- children plots in S
and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)) -- where the property is missing

-- insert children
while @i > 0
begin
	-- insert the direct children of the entries found so far
	insert into @PlotPropertyPresent(PropertyID, PlotID, PropertyURI, PartOfPlotID, DisplayText, PropertyHierarchyCache)
	select distinct P.PropertyID, S.PlotID, P.PropertyURI, S.PartOfPlotID, P.DisplayText, P.PropertyHierarchyCache from SamplingPlot S, @PlotPropertyPresent P
	where P.PlotID = S.PartOfPlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)

	-- insert the 
	insert into @PlotPropertyPresent(PropertyID, PlotID, PropertyURI, PartOfPlotID, DisplayText, PropertyHierarchyCache)
	select distinct P.PropertyID, S.PlotID, S.PropertyURI, C.PartOfPlotID, P.DisplayText, P.PropertyHierarchyCache from @PlotPropertyPresent S, @PlotPropertyPresent P, SamplingPlot C
	where S.PartOfPlotID = P.PlotID
	and S.PlotID = C.PlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)

	insert into @PlotPropertyPresent(PropertyID, PlotID, PropertyURI, PartOfPlotID, DisplayText, PropertyHierarchyCache)
	select distinct S.PropertyID, C.PlotID, S.PropertyURI, C.PartOfPlotID, S.DisplayText, S.PropertyHierarchyCache from @PlotPropertyPresent S, SamplingPlot P, SamplingPlot C
	where C.PartOfPlotID = P.PlotID
	and S.PlotID = P.PartOfPlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where C.PlotID = PPP.PlotID and S.PropertyID = PPP.PropertyID)

	set @i = (select count(*) from SamplingPlot S, @PlotPropertyPresent P where S.PartOfPlotID = P.PlotID 
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID))
end   
/* enabled again */
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
RETURN '03.00.12.00' 
END

GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.34'
END

GO

SELECT [dbo].[Version] ()  

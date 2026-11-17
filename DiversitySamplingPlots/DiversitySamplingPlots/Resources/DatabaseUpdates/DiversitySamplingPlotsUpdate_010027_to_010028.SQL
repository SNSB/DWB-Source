
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.27'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   update for Database   ######################################################################################
--#####################################################################################################################

/****** Object:  StoredProcedure [dbo].[procSamplingPlotInheritAllProperties]  Script Date: 28.12.2017 10:05:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
Creates a table that lists all the properties of a plot included those inherited from a parent plot
Inserts missing properties to plots
MW/WR 28.12.2017
*/

/*
CREATE  PROCEDURE [dbo].[procSamplingPlotInheritAllProperties] 

AS
BEGIN

declare @PlotPropertyPresent TABLE (
    [PropertyID] [int],
    [PlotID] [int],
    [PartOfPlotID] [int],
    [DisplayText] [nvarchar] (255) NULL,
    [PropertyURI] [varchar] (255) NULL,
    [PropertyHierarchyCache] [nvarchar] (max) NULL,
    [PropertyValue] [nvarchar] (255) NULL,
    [ResponsibleName] [nvarchar] (255) NULL,
    [ResponsibleAgentURI] [varchar] (255) NULL,
    [Notes] [nvarchar] (max) NULL,
	[Present] [int]
)

-- insert the entries as entered by the user
insert into @PlotPropertyPresent(PropertyID, PlotID, PartOfPlotID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, Present)
select distinct P.PropertyID, S.PlotID, S.PartOfPlotID, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.ResponsibleName, P.ResponsibleAgentURI, P.Notes, 1
from SamplingPlot S, SamplingPlotProperty P
where S.PlotID = P.PlotID

-- count the missing child entries
declare @i int
set @i = (select count(*) from SamplingPlot S, @PlotPropertyPresent P where S.PartOfPlotID = P.PlotID -- children plots in S
and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)) -- where the property is missing

-- insert children
while @i > 0
begin
	-- insert the direct children of the entries found so far
	insert into @PlotPropertyPresent(PropertyID, PlotID, PartOfPlotID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes)
	select distinct P.PropertyID, S.PlotID, S.PartOfPlotID, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.ResponsibleName, P.ResponsibleAgentURI, P.Notes from SamplingPlot S, @PlotPropertyPresent P
	where P.PlotID = S.PartOfPlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)

	-- insert the 
	insert into @PlotPropertyPresent(PropertyID, PlotID, PartOfPlotID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes)
	select distinct P.PropertyID, S.PlotID, C.PartOfPlotID, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.ResponsibleName, P.ResponsibleAgentURI, P.Notes from @PlotPropertyPresent S, @PlotPropertyPresent P, SamplingPlot C
	where S.PartOfPlotID = P.PlotID
	and S.PlotID = C.PlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)

	insert into @PlotPropertyPresent(PropertyID, PlotID, PartOfPlotID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes)
	select distinct S.PropertyID, C.PlotID, C.PartOfPlotID, S.DisplayText, S.PropertyURI, S.PropertyHierarchyCache, S.PropertyValue, S.ResponsibleName, S.ResponsibleAgentURI, S.Notes from @PlotPropertyPresent S, SamplingPlot P, SamplingPlot C
	where C.PartOfPlotID = P.PlotID
	and S.PlotID = P.PartOfPlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where C.PlotID = PPP.PlotID and S.PropertyID = PPP.PropertyID)

	set @i = (select count(*) from SamplingPlot S, @PlotPropertyPresent P where S.PartOfPlotID = P.PlotID 
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID))
end   
  
INSERT INTO [dbo].[SamplingPlotProperty]
           ([PlotID]
           ,[PropertyID]
           ,[DisplayText]
           ,[PropertyURI]
		   ,[PropertyHierarchyCache]
		   ,[PropertyValue]
		   ,[ResponsibleName]
		   ,[ResponsibleAgentURI]
		   ,[Notes])
select PPP.PlotID, PPP.PropertyID, PPP.DisplayText, PPP.PropertyURI, PPP.PropertyHierarchyCache, PPP.PropertyValue, PPP.ResponsibleName, PPP.ResponsibleAgentURI, PPP.Notes
from @PlotPropertyPresent PPP where Present is null


RETURN
END

GRANT EXEC ON  [dbo].[procSamplingPlotInheritAllProperties] TO [DiversityWorkbenchAdmin]

GO
*/

/* Delete first if already exists */
BEGIN TRY
    DROP FUNCTION [dbo].[SamplingPlotAllProperties];
END TRY
BEGIN CATCH
END CATCH
GO

CREATE FUNCTION [dbo].[SamplingPlotAllProperties] ()  
RETURNS @PlotPropertyPresent TABLE ([PropertyID] [int],
    [PlotID] [int],
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
insert into @PlotPropertyPresent(PropertyID, PlotID, PartOfPlotID, DisplayText, PropertyHierarchyCache)
select distinct P.PropertyID, S.PlotID, S.PartOfPlotID, P.DisplayText, P.PropertyHierarchyCache
from SamplingPlot S, SamplingPlotProperty P
where S.PlotID = P.PlotID

-- count the missing child entries
declare @i int
set @i = (select count(*) from SamplingPlot S, @PlotPropertyPresent P where S.PartOfPlotID = P.PlotID -- children plots in S
and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)) -- where the property is missing

-- insert children
while @i > 0
begin
	-- insert the direct children of the entries found so far
	insert into @PlotPropertyPresent(PropertyID, PlotID, PartOfPlotID, DisplayText, PropertyHierarchyCache)
	select distinct P.PropertyID, S.PlotID, S.PartOfPlotID, P.DisplayText, P.PropertyHierarchyCache from SamplingPlot S, @PlotPropertyPresent P
	where P.PlotID = S.PartOfPlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)

	-- insert the 
	insert into @PlotPropertyPresent(PropertyID, PlotID, PartOfPlotID, DisplayText, PropertyHierarchyCache)
	select distinct P.PropertyID, S.PlotID, C.PartOfPlotID, P.DisplayText, P.PropertyHierarchyCache from @PlotPropertyPresent S, @PlotPropertyPresent P, SamplingPlot C
	where S.PartOfPlotID = P.PlotID
	and S.PlotID = C.PlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID)

	insert into @PlotPropertyPresent(PropertyID, PlotID, PartOfPlotID, DisplayText, PropertyHierarchyCache)
	select distinct S.PropertyID, C.PlotID, C.PartOfPlotID, S.DisplayText, S.PropertyHierarchyCache from @PlotPropertyPresent S, SamplingPlot P, SamplingPlot C
	where C.PartOfPlotID = P.PlotID
	and S.PlotID = P.PartOfPlotID
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where C.PlotID = PPP.PlotID and S.PropertyID = PPP.PropertyID)

	set @i = (select count(*) from SamplingPlot S, @PlotPropertyPresent P where S.PartOfPlotID = P.PlotID 
	and NOT EXISTS (select * from @PlotPropertyPresent PPP where S.PlotID = PPP.PlotID and P.PropertyID = PPP.PropertyID))
end   

RETURN
END

GO

GRANT SELECT ON  [dbo].[SamplingPlotAllProperties] TO [DiversityWorkbenchEditor]

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

/* wait till new official release is available
ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.09.00' 
END

GO
*/

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.28'
END

GO

SELECT [dbo].[Version] ()  

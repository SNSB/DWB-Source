
/****** Object:  DatabaseRole [DataManager]    Script Date: 19.04.2021 09:33:09 ******/
CREATE ROLE [DataManager]
GO
/****** Object:  DatabaseRole [DiversityWorkbenchAdministrator]    Script Date: 19.04.2021 09:33:09 ******/
CREATE ROLE [DiversityWorkbenchAdministrator]
GO
/****** Object:  DatabaseRole [DiversityWorkbenchEditor]    Script Date: 19.04.2021 09:33:09 ******/
CREATE ROLE [DiversityWorkbenchEditor]
GO
/****** Object:  DatabaseRole [DiversityWorkbenchUser]    Script Date: 19.04.2021 09:33:09 ******/
CREATE ROLE [DiversityWorkbenchUser]
GO
ALTER ROLE [DiversityWorkbenchEditor] ADD MEMBER [DataManager]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [DiversityWorkbenchAdministrator]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [DiversityWorkbenchAdministrator]
GO
ALTER ROLE [DiversityWorkbenchUser] ADD MEMBER [DiversityWorkbenchEditor]
GO
/****** Object:  UserDefinedFunction [dbo].[BaseURL]    Script Date: 19.04.2021 09:33:09 ******/
GRANT EXECUTE ON [dbo].[BaseURL] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[DefaultProjectID]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DefaultProjectID] ()
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
GRANT EXECUTE ON [dbo].[DefaultProjectID] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[DiversityWorkbenchModule]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	
CREATE FUNCTION [dbo].[DiversityWorkbenchModule] ()   RETURNS nvarchar(50) AS BEGIN RETURN 'DiversitySamplingPlots' END  ; 
GO
GRANT EXECUTE ON [dbo].[DiversityWorkbenchModule] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[PrivacyConsentInfo]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   Function PrivacyConsentInfo providing common information within the DiversityWorkbench  ####################
--#####################################################################################################################

CREATE FUNCTION [dbo].[PrivacyConsentInfo] () 
RETURNS varchar (900) 
AS  
BEGIN return 'http://diversityworkbench.net/Portal/Default_Agreement_on_Processing_of_Personal_Data_in_DWB_Software'  
END; 
GO
GRANT ALTER ON [dbo].[PrivacyConsentInfo] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[PrivacyConsentInfo] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotAllLocalisations]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[SamplingPlotAllLocalisations] ()  
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
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotAllProperties]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[SamplingPlotAllProperties] ()  
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
GRANT SELECT ON [dbo].[SamplingPlotAllProperties] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotChildNodes]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SamplingPlotChildNodes] (@ID int)  
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
GRANT SELECT ON [dbo].[SamplingPlotChildNodes] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotGeography]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	INSERT INTO @SamplingPlotGeography 
	SELECT [PlotID]
      ,[PartOfPlotID]
      ,[PlotGeography_Cache]
	  FROM [SamplingPlot] S, @IDs I
	  WHERE S.PlotID = I.ID
	WHILE (SELECT COUNT(*) FROM @IDs) > 0
	BEGIN
		DELETE I
		FROM @IDs I, @SamplingPlotGeography S
		WHERE S.PlotID = I.ID
		AND NOT S.Geography IS NULL	
		DELETE I
		FROM @IDs I, @SamplingPlotGeography S
		WHERE S.PlotID = I.ID
		AND S.PartOfPlotID IS NULL	
		UPDATE G SET Geography = S.PlotGeography_Cache
		FROM @SamplingPlotGeography G, [SamplingPlot] S
		WHERE S.PlotID = G.PartOfPlotID
		AND G.Geography IS NULL
		AND NOT S.PlotGeography_Cache IS NULL
		UPDATE G SET PartOfPlotID = S.PartOfPlotID
		FROM @SamplingPlotGeography G, [SamplingPlot] S
		WHERE S.PlotID = G.PartOfPlotID
	END
RETURN	
END
GO
GRANT SELECT ON [dbo].[SamplingPlotGeography] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotHierarchy]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SamplingPlotHierarchy] (@PlotID int)  
RETURNS @List TABLE ([PlotID] [int] Primary key ,
	[PartOfPlotID] [int] NULL ,
	[PlotIdentifier] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the analysis items related to the given analysis.
MW 26.02.2008
*/
AS
BEGIN
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
   INSERT @List
   SELECT DISTINCT PlotID, PartOfPlotID, PlotIdentifier
   FROM SamplingPlot
   WHERE SamplingPlot.PlotID = @TopID
   ORDER BY PlotIdentifier
   INSERT @List
   SELECT * FROM dbo.SamplingPlotChildNodes (@TopID)
   RETURN
END
GO
GRANT SELECT ON [dbo].[SamplingPlotHierarchy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotList]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[SamplingPlotList] ()  
RETURNS @SamplingPlotList TABLE ([PlotID] [int] Primary key ,
	[PartOfPlotID] [int] NULL,
	[PlotIdentifier] [nvarchar](500) NOT NULL,
	[PlotGeography_Cache] [geography] NULL,
	[PlotGeometry_Cache] [geometry] NULL,
	[PlotDescription] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[DisplayText] [nvarchar](max) NOT NULL)
/*
Returns a table that lists all the SamplingPlot items related to the given SamplingPlot.
MW 02.02.2012
TEST:
SELECT * FROM DBO.SamplingPlotList()
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
SELECT PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, PlotIdentifier
FROM SamplingPlot
WHERE SamplingPlot.PartOfPlotID IS NULL

declare @i int
set @i = (select count(*) from SamplingPlot where PlotID not IN (select PlotID from  @SamplingPlotList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @SamplingPlotList (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText)
	SELECT C.PlotID, C.PartOfPlotID, C.PlotIdentifier, C.PlotGeography_Cache, C.PlotGeometry_Cache, C.PlotDescription, C.InternalNotes, L.DisplayText + ' | ' + C.PlotIdentifier
	FROM SamplingPlot C, @SamplingPlotList L
	WHERE C.PartOfPlotID = L.PlotID
	AND C.PlotID NOT IN (select PlotID from  @SamplingPlotList)

	set @i = (select count(*) from SamplingPlot where PlotID not IN (select PlotID from  @SamplingPlotList))
end


   RETURN
END


GO
GRANT SELECT ON [dbo].[SamplingPlotList] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotListForCurrentProject]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   update for Database   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[SamplingPlotListForCurrentProject] ()  
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
GRANT SELECT ON [dbo].[SamplingPlotListForCurrentProject] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotSuperiorNodes]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[SamplingPlotSuperiorNodes] (@PlotID int)  
RETURNS @List TABLE ([PlotID] [int] Primary key ,
	[PartOfPlotID] [int] NULL ,
	[PlotIdentifier] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
	[PlotHierarchyCache] nvarchar(4000) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the analysis items related to the given analysis.
MW 26.02.2008
TEST:
SELECT * FROM DBO.SamplingPlotSuperiorNodes(-766)
*/
AS
BEGIN

   INSERT  @List (PlotID, PartOfPlotID, PlotIdentifier, PlotHierarchyCache)
   SELECT PlotID, PartOfPlotID, PlotIdentifier, PlotIdentifier FROM dbo.SamplingPlot WHERE PlotID = @PlotID

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
		
		INSERT  @List (PlotID, PartOfPlotID, PlotIdentifier, PlotHierarchyCache)
		SELECT PlotID, PartOfPlotID, PlotIdentifier, PlotIdentifier FROM dbo.SamplingPlot WHERE PlotID = @PlotID
		
		UPDATE L SET L.PlotHierarchyCache = P.PlotIdentifier + ' | ' + L.PlotHierarchyCache
		FROM dbo.SamplingPlot P, @List L 
		WHERE P.PlotID = @PlotID
		AND L.PlotID <> @PlotID

		end
	set @TopID = @PlotID
	end

   RETURN
END

GO
GRANT SELECT ON [dbo].[SamplingPlotSuperiorNodes] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[UserID]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE FUNCTION [dbo].[UserID] () RETURNS int AS 
BEGIN  
declare @ID int;  
SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = SUSER_SNAME()) 
RETURN @ID  
END 
GO
GRANT EXECUTE ON [dbo].[UserID] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[UserName]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE FUNCTION [dbo].[UserName] (@ID varchar(10)) RETURNS nvarchar(50) AS 
BEGIN  
declare @User nvarchar(50);  
SET @User = (SELECT MIN(CASE WHEN U.CombinedNameCache <> '' THEN substring(U.CombinedNameCache, 1, 50) ELSE CASE WHEN U.LoginName <> '' THEN U.LoginName ELSE cast(U.ID as varchar) END END) FROM UserProxy U WHERE cast(U.ID as varchar) = @ID)  
RETURN @User  
END 
GO
GRANT EXECUTE ON [dbo].[UserName] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[Version]    Script Date: 19.04.2021 09:33:09 ******/
GRANT EXECUTE ON [dbo].[Version] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[VersionClient]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '04.04.00' 
END

GO
/****** Object:  Table [dbo].[LocalisationSystem]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalisationSystem](
	[LocalisationSystemID] [int] NOT NULL,
	[LocalisationSystemParentID] [int] NULL,
	[LocalisationSystemName] [nvarchar](100) NOT NULL,
	[DefaultAccuracyOfLocalisation] [nvarchar](50) NULL,
	[DefaultMeasurementUnit] [nvarchar](50) NULL,
	[ParsingMethodName] [nvarchar](50) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayEnable] [bit] NULL,
	[DisplayOrder] [smallint] NULL,
	[Description] [nvarchar](255) NULL,
	[DisplayTextLocation1] [nvarchar](50) NULL,
	[DescriptionLocation1] [nvarchar](255) NULL,
	[DisplayTextLocation2] [nvarchar](50) NULL,
	[DescriptionLocation2] [nvarchar](255) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
 CONSTRAINT [PK_LocalisationSystem_1] PRIMARY KEY CLUSTERED 
(
	[LocalisationSystemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LocalisationSystem] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[LocalisationSystem_log]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalisationSystem_log](
	[LocalisationSystemID] [int] NULL,
	[LocalisationSystemParentID] [int] NULL,
	[LocalisationSystemName] [nvarchar](100) NULL,
	[DefaultAccuracyOfLocalisation] [nvarchar](50) NULL,
	[DefaultMeasurementUnit] [nvarchar](50) NULL,
	[ParsingMethodName] [nvarchar](50) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayEnable] [bit] NULL,
	[DisplayOrder] [smallint] NULL,
	[Description] [nvarchar](255) NULL,
	[DisplayTextLocation1] [nvarchar](50) NULL,
	[DescriptionLocation1] [nvarchar](255) NULL,
	[DisplayTextLocation2] [nvarchar](50) NULL,
	[DescriptionLocation2] [nvarchar](255) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_LocalisationSystem_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectProxy]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectProxy](
	[ProjectID] [int] NOT NULL,
	[Project] [nvarchar](50) NULL,
	[ProjectURI] [varchar](255) NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ProjectProxy] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectProxy] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectProxy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectUser]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectUser](
	[LoginName] [nvarchar](50) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ProjectUser] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectUser] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectUser] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectUser] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectUser] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectUser] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[Property]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Property](
	[PropertyID] [int] NOT NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[PropertyParentID] [int] NULL,
	[PropertyName] [nvarchar](100) NOT NULL,
	[DefaultAccuracyOfProperty] [nvarchar](50) NULL,
	[DefaultMeasurementUnit] [nvarchar](50) NULL,
	[ParsingMethodName] [nvarchar](50) NOT NULL,
	[DisplayEnabled] [bit] NULL,
	[DisplayOrder] [smallint] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[PropertyURI] [varchar](1000) NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[PropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[Property] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[Property_log]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Property_log](
	[PropertyID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[PropertyParentID] [int] NULL,
	[PropertyName] [nvarchar](100) NULL,
	[DefaultAccuracyOfProperty] [nvarchar](50) NULL,
	[DefaultMeasurementUnit] [nvarchar](50) NULL,
	[ParsingMethodName] [nvarchar](50) NULL,
	[DisplayEnabled] [bit] NULL,
	[DisplayOrder] [smallint] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Property_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReplicationPublisher]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReplicationPublisher](
	[DatabaseName] [varchar](255) NOT NULL,
	[Server] [varchar](255) NOT NULL,
	[Port] [smallint] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ReplicationPublisher] PRIMARY KEY CLUSTERED 
(
	[DatabaseName] ASC,
	[Server] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ReplicationPublisher] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[ReplicationPublisher] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[ReplicationPublisher] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ReplicationPublisher] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
/****** Object:  Table [dbo].[SamplingPlot]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingPlot](
	[PlotID] [int] IDENTITY(1,1) NOT NULL,
	[PartOfPlotID] [int] NULL,
	[PlotIdentifier] [nvarchar](500) NOT NULL,
	[PlotGeography_Cache] [geography] NULL,
	[PlotGeometry_Cache] [geometry] NULL,
	[PlotDescription] [nvarchar](max) NULL,
	[PlotType] [nvarchar](50) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[CountryCache] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SamplingPlot] PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[SamplingPlot] TO [DataManager] AS [dbo]
GO
GRANT DELETE ON [dbo].[SamplingPlot] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[SamplingPlot] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[SamplingPlot] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[SamplingPlot] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[SamplingPlot_log]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingPlot_log](
	[PlotID] [int] NULL,
	[PartOfPlotID] [int] NULL,
	[PlotIdentifier] [nvarchar](500) NULL,
	[PlotGeography_Cache] [geography] NULL,
	[PlotGeometry_Cache] [geometry] NULL,
	[PlotDescription] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[PlotType] [nvarchar](50) NULL,
	[CountryCache] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SamplingPlot_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SamplingPlotLocalisation]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingPlotLocalisation](
	[PlotID] [int] NOT NULL,
	[LocalisationSystemID] [int] NOT NULL,
	[Location1] [nvarchar](255) NULL,
	[Location2] [nvarchar](255) NULL,
	[LocationAccuracy] [nvarchar](50) NULL,
	[LocationNotes] [nvarchar](max) NULL,
	[DeterminationDate] [smalldatetime] NULL,
	[DistanceToLocation] [varchar](50) NULL,
	[DirectionToLocation] [varchar](50) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Geography] [geography] NULL,
	[AverageAltitudeCache] [float] NULL,
	[AverageLatitudeCache] [float] NULL,
	[AverageLongitudeCache] [float] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_SamplingPlotLocalisation] PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[LocalisationSystemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[SamplingPlotLocalisation] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[SamplingPlotLocalisation] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[SamplingPlotLocalisation] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[SamplingPlotLocalisation] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[SamplingPlotLocalisation_log]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingPlotLocalisation_log](
	[PlotID] [int] NULL,
	[LocalisationSystemID] [int] NULL,
	[Location1] [nvarchar](255) NULL,
	[Location2] [nvarchar](255) NULL,
	[LocationAccuracy] [nvarchar](50) NULL,
	[LocationNotes] [nvarchar](max) NULL,
	[DeterminationDate] [smalldatetime] NULL,
	[DistanceToLocation] [varchar](50) NULL,
	[DirectionToLocation] [varchar](50) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Geography] [geography] NULL,
	[AverageAltitudeCache] [float] NULL,
	[AverageLatitudeCache] [float] NULL,
	[AverageLongitudeCache] [float] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SamplingPlotLocalisation_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SamplingPlotProperty]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingPlotProperty](
	[PlotID] [int] NOT NULL,
	[PropertyID] [int] NOT NULL,
	[PropertyGeometry] [geometry] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[PropertyURI] [varchar](255) NOT NULL,
	[DisplayText] [nvarchar](255) NULL,
	[PropertyHierarchyCache] [nvarchar](max) NULL,
	[PropertyValue] [nvarchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[AverageValueCache] [float] NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SamplingPlotProperty] PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[PropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT DELETE ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[SamplingPlotProperty_log]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingPlotProperty_log](
	[PlotID] [int] NULL,
	[PropertyID] [int] NULL,
	[PropertyURI] [varchar](255) NULL,
	[DisplayText] [nvarchar](255) NULL,
	[PropertyHierarchyCache] [nvarchar](max) NULL,
	[PropertyValue] [nvarchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[AverageValueCache] [float] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[PropertyGeometry] [geometry] NULL,
 CONSTRAINT [PK_SamplingPlotProperty_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SamplingPlotResource]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingPlotResource](
	[PlotID] [int] NOT NULL,
	[ResourceURI] [varchar](255) NOT NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SamplingPlotResource] PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[ResourceURI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[SamplingPlotResource_log]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingPlotResource_log](
	[PlotID] [int] NULL,
	[ResourceURI] [varchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SamplingPlotResource_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SamplingProject]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingProject](
	[PlotID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SamplingProject] PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[SamplingProject] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[SamplingProject] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[SamplingProject] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[SamplingProject] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[SamplingProject_log]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SamplingProject_log](
	[PlotID] [int] NULL,
	[ProjectID] [int] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SamplingProject_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProxy]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProxy](
	[LoginName] [nvarchar](50) NOT NULL,
	[CombinedNameCache] [nvarchar](255) NOT NULL,
	[UserURI] [varchar](255) NULL,
	[Queries] [xml] NULL,
	[Settings] [xml] NULL,
	[AgentURI] [varchar](255) NULL,
	[CurrentProjectID] [int] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PrivacyConsent] [bit] NULL,
	[PrivacyConsentDate] [datetime] NULL,
 CONSTRAINT [PK_UserProxy] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[UserProxy] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[UserProxy] TO [DiversityWorkbenchUser] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserProxy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewBaseURL]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ViewBaseURL]
    AS
    SELECT dbo.BaseURL() AS BaseURL
GO
GRANT SELECT ON [dbo].[ViewBaseURL] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotGeoInfo]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ViewSamplingPlotGeoInfo]
AS
SELECT     CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL
                      THEN P4.PlotGeography_Cache.EnvelopeCenter().Lat ELSE P3.PlotGeography_Cache.EnvelopeCenter().Lat END ELSE P2.PlotGeography_Cache.EnvelopeCenter().Lat END
                       ELSE P1.PlotGeography_Cache.EnvelopeCenter().Lat END ELSE S.PlotGeography_Cache.EnvelopeCenter().Lat END AS Latitude,
                      CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL
                      THEN P4.PlotGeography_Cache.EnvelopeCenter().Long ELSE P3.PlotGeography_Cache.EnvelopeCenter().Long END ELSE P2.PlotGeography_Cache.EnvelopeCenter().Long
                       END ELSE P1.PlotGeography_Cache.EnvelopeCenter().Long END ELSE S.PlotGeography_Cache.EnvelopeCenter().Long END AS Longitude,
                      CAST(CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL THEN P4.PlotGeography_Cache.EnvelopeAngle() ELSE P3.PlotGeography_Cache.EnvelopeAngle()
                      END ELSE P2.PlotGeography_Cache.EnvelopeAngle() END ELSE P1.PlotGeography_Cache.EnvelopeAngle() END ELSE S.PlotGeography_Cache.EnvelopeAngle()
                      END AS varchar) + ' °' AS [Accuracy angle], CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL THEN P4.PlotGeography_Cache.EnvelopeAngle()
                      ELSE P3.PlotGeography_Cache.EnvelopeAngle() END ELSE P2.PlotGeography_Cache.EnvelopeAngle() END ELSE P1.PlotGeography_Cache.EnvelopeAngle()
                      END ELSE S.PlotGeography_Cache.EnvelopeAngle() END AS AccuracyAngle, CASE WHEN S.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL
                      THEN P4.PlotGeography_Cache.STArea() ELSE P3.PlotGeography_Cache.STArea() END ELSE P2.PlotGeography_Cache.STArea()
                      END ELSE P1.PlotGeography_Cache.STArea() END ELSE S.PlotGeography_Cache.STArea() END AS AreaM2, CASE WHEN S.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P4.PlotGeography_Cache.STArea() > 1000000 THEN CAST(P4.PlotGeography_Cache.STArea() / 1000000 AS Varchar)
                      + ' km2' ELSE CAST(P4.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END ELSE CASE WHEN P3.PlotGeography_Cache.STArea()
                      > 1000000 THEN CAST(P3.PlotGeography_Cache.STArea() / 1000000 AS Varchar) + ' km2' ELSE CAST(P3.PlotGeography_Cache.STArea() AS Varchar)
                      + ' m2' END END ELSE CASE WHEN P2.PlotGeography_Cache.STArea() > 1000000 THEN CAST(P2.PlotGeography_Cache.STArea() / 1000000 AS Varchar)
                      + ' km2' ELSE CAST(P2.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END END ELSE CASE WHEN P1.PlotGeography_Cache.STArea()
                      > 1000000 THEN CAST(P1.PlotGeography_Cache.STArea() / 1000000 AS Varchar) + ' km2' ELSE CAST(P1.PlotGeography_Cache.STArea() AS Varchar)
                      + ' m2' END END ELSE CASE WHEN S.PlotGeography_Cache.STArea() > 1000000 THEN CAST(S.PlotGeography_Cache.STArea() / 1000000 AS Varchar)
                      + ' km2' ELSE CAST(S.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END END AS Area, CASE WHEN S.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL
                      THEN P4.PlotGeography_Cache.ToString() ELSE P3.PlotGeography_Cache.ToString() END ELSE P2.PlotGeography_Cache.ToString()
                      END ELSE P1.PlotGeography_Cache.ToString() END ELSE S.PlotGeography_Cache.ToString() END AS Geography, CASE WHEN S.CountryCache IS NULL
                      THEN CASE WHEN P1.CountryCache IS NULL THEN CASE WHEN P2.CountryCache IS NULL THEN CASE WHEN P3.CountryCache IS NULL
                      THEN P4.CountryCache ELSE P3.CountryCache END ELSE P2.CountryCache END ELSE P1.CountryCache END ELSE S.CountryCache END AS CountryCache,
                      S.PlotID, B.BaseURL + cast(S.PlotID as varchar) AS URL
FROM       [dbo].[ViewBaseURL] AS B,  dbo.SamplingPlot AS P4 RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P3 ON P4.PlotID = P3.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P2 ON P3.PlotID = P2.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P1 ON P2.PlotID = P1.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS S ON P1.PlotID = S.PartOfPlotID
GO
GRANT SELECT ON [dbo].[ViewSamplingPlotGeoInfo] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotHierarchy1]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewSamplingPlotHierarchy1]
AS
SELECT        dbo.SamplingPlot.PlotID, 
						 SamplingPlot_1.PlotIdentifier + ' | ' + 
						 dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID
WHERE        (SamplingPlot_1.PartOfPlotID IS NULL)
GO
GRANT SELECT ON [dbo].[ViewSamplingPlotHierarchy1] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotHierarchy2]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewSamplingPlotHierarchy2]
AS
SELECT        dbo.SamplingPlot.PlotID, 
						 SamplingPlot_2.PlotIdentifier + ' | ' + 
						 SamplingPlot_1.PlotIdentifier + ' | ' + 
						 dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_2 ON SamplingPlot_1.PartOfPlotID = SamplingPlot_2.PlotID
WHERE        (SamplingPlot_2.PartOfPlotID IS NULL)
GO
GRANT SELECT ON [dbo].[ViewSamplingPlotHierarchy2] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotHierarchy3]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewSamplingPlotHierarchy3]
AS
SELECT        dbo.SamplingPlot.PlotID,
						 SamplingPlot_3.PlotIdentifier + ' | ' +  
						 SamplingPlot_2.PlotIdentifier + ' | ' + 
						 SamplingPlot_1.PlotIdentifier + ' | ' + 
                         dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_2 ON SamplingPlot_1.PartOfPlotID = SamplingPlot_2.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_3 ON SamplingPlot_2.PartOfPlotID = SamplingPlot_3.PlotID
WHERE        (SamplingPlot_3.PartOfPlotID IS NULL)
GO
GRANT SELECT ON [dbo].[ViewSamplingPlotHierarchy3] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotHierarchy4]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewSamplingPlotHierarchy4]
AS
SELECT        dbo.SamplingPlot.PlotID,
						 SamplingPlot_4.PlotIdentifier + ' | ' +  
						 SamplingPlot_3.PlotIdentifier + ' | ' + 
						 SamplingPlot_2.PlotIdentifier + ' | ' + 
						 SamplingPlot_1.PlotIdentifier + ' | ' + 
                         dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_2 ON SamplingPlot_1.PartOfPlotID = SamplingPlot_2.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_3 ON SamplingPlot_2.PartOfPlotID = SamplingPlot_3.PlotID  INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_4 ON SamplingPlot_3.PartOfPlotID = SamplingPlot_4.PlotID
WHERE        (SamplingPlot_4.PartOfPlotID IS NULL)
GO
GRANT SELECT ON [dbo].[ViewSamplingPlotHierarchy4] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotHierarchy5]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewSamplingPlotHierarchy5]
AS
SELECT        dbo.SamplingPlot.PlotID,
						 SamplingPlot_5.PlotIdentifier + ' | ' + 
						 SamplingPlot_4.PlotIdentifier + ' | ' + 
						 SamplingPlot_3.PlotIdentifier + ' | ' + 
						 SamplingPlot_2.PlotIdentifier + ' | ' + 
						 SamplingPlot_1.PlotIdentifier + ' | ' + 
                         dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_2 ON SamplingPlot_1.PartOfPlotID = SamplingPlot_2.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_3 ON SamplingPlot_2.PartOfPlotID = SamplingPlot_3.PlotID  INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_4 ON SamplingPlot_3.PartOfPlotID = SamplingPlot_4.PlotID   INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_5 ON SamplingPlot_4.PartOfPlotID = SamplingPlot_5.PlotID
WHERE        (SamplingPlot_5.PartOfPlotID IS NULL)
GO
GRANT SELECT ON [dbo].[ViewSamplingPlotHierarchy5] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotHierarchy]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewSamplingPlotHierarchy]
AS

select PlotID, PlotIdentifier as DisplayText from SamplingPlot s where s.PartOfPlotID is null
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy1
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy2
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy3
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy4
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy5
GO
GRANT SELECT ON [dbo].[ViewSamplingPlotHierarchy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[PlotID_UserAvailable]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[PlotID_UserAvailable]
AS
SELECT     dbo.SamplingProject.PlotID
FROM         dbo.SamplingProject INNER JOIN
                      dbo.ProjectUser ON dbo.SamplingProject.ProjectID = dbo.ProjectUser.ProjectID
WHERE     (dbo.ProjectUser.LoginName = USER_NAME())
GROUP BY dbo.SamplingProject.PlotID
UNION
SELECT     TOP (100) PERCENT dbo.SamplingPlot.PlotID
FROM         dbo.SamplingProject AS SamplingProject_1 RIGHT OUTER JOIN
                      dbo.SamplingPlot ON SamplingProject_1.PlotID = dbo.SamplingPlot.PlotID
GROUP BY dbo.SamplingPlot.PlotID, SamplingProject_1.ProjectID
HAVING      (SamplingProject_1.ProjectID IS NULL)
ORDER BY PlotID
GO
GRANT SELECT ON [dbo].[PlotID_UserAvailable] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ProjectList]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProjectList]
AS
SELECT     TOP 100 PERCENT dbo.ProjectProxy.Project, dbo.ProjectUser.ProjectID
FROM         dbo.ProjectUser INNER JOIN
                      dbo.ProjectProxy ON dbo.ProjectUser.ProjectID = dbo.ProjectProxy.ProjectID
WHERE     (dbo.ProjectUser.LoginName = USER_NAME())
GROUP BY dbo.ProjectProxy.Project, dbo.ProjectUser.ProjectID
ORDER BY dbo.ProjectProxy.Project

GO
GRANT SELECT ON [dbo].[ProjectList] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[SamplingPlotGeoInfo]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SamplingPlotGeoInfo]
AS
SELECT     CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN P4.PlotGeography_Cache.EnvelopeCenter().Lat ELSE P3.PlotGeography_Cache.EnvelopeCenter().Lat END ELSE P2.PlotGeography_Cache.EnvelopeCenter().Lat END
                       ELSE P1.PlotGeography_Cache.EnvelopeCenter().Lat END ELSE S.PlotGeography_Cache.EnvelopeCenter().Lat END AS Latitude, 
                      CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN P4.PlotGeography_Cache.EnvelopeCenter().Long ELSE P3.PlotGeography_Cache.EnvelopeCenter().Long END ELSE P2.PlotGeography_Cache.EnvelopeCenter().Long
                       END ELSE P1.PlotGeography_Cache.EnvelopeCenter().Long END ELSE S.PlotGeography_Cache.EnvelopeCenter().Long END AS Longitude, 
                      CAST(CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL THEN P4.PlotGeography_Cache.EnvelopeAngle() ELSE P3.PlotGeography_Cache.EnvelopeAngle() 
                      END ELSE P2.PlotGeography_Cache.EnvelopeAngle() END ELSE P1.PlotGeography_Cache.EnvelopeAngle() END ELSE S.PlotGeography_Cache.EnvelopeAngle() 
                      END AS varchar) + ' °' AS [Accuracy angle], CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL THEN P4.PlotGeography_Cache.EnvelopeAngle() 
                      ELSE P3.PlotGeography_Cache.EnvelopeAngle() END ELSE P2.PlotGeography_Cache.EnvelopeAngle() END ELSE P1.PlotGeography_Cache.EnvelopeAngle() 
                      END ELSE S.PlotGeography_Cache.EnvelopeAngle() END AS AccuracyAngle, CASE WHEN S.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN P4.PlotGeography_Cache.STArea() ELSE P3.PlotGeography_Cache.STArea() END ELSE P2.PlotGeography_Cache.STArea() 
                      END ELSE P1.PlotGeography_Cache.STArea() END ELSE S.PlotGeography_Cache.STArea() END AS AreaM2, CASE WHEN S.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P4.PlotGeography_Cache.STArea() > 1000000 THEN CAST(P4.PlotGeography_Cache.STArea() / 1000000 AS Varchar) 
                      + ' km2' ELSE CAST(P4.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END ELSE CASE WHEN P3.PlotGeography_Cache.STArea() 
                      > 1000000 THEN CAST(P3.PlotGeography_Cache.STArea() / 1000000 AS Varchar) + ' km2' ELSE CAST(P3.PlotGeography_Cache.STArea() AS Varchar) 
                      + ' m2' END END ELSE CASE WHEN P2.PlotGeography_Cache.STArea() > 1000000 THEN CAST(P2.PlotGeography_Cache.STArea() / 1000000 AS Varchar) 
                      + ' km2' ELSE CAST(P2.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END END ELSE CASE WHEN P1.PlotGeography_Cache.STArea() 
                      > 1000000 THEN CAST(P1.PlotGeography_Cache.STArea() / 1000000 AS Varchar) + ' km2' ELSE CAST(P1.PlotGeography_Cache.STArea() AS Varchar) 
                      + ' m2' END END ELSE CASE WHEN S.PlotGeography_Cache.STArea() > 1000000 THEN CAST(S.PlotGeography_Cache.STArea() / 1000000 AS Varchar) 
                      + ' km2' ELSE CAST(S.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END END AS Area, CASE WHEN S.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN P4.PlotGeography_Cache.ToString() ELSE P3.PlotGeography_Cache.ToString() END ELSE P2.PlotGeography_Cache.ToString() 
                      END ELSE P1.PlotGeography_Cache.ToString() END ELSE S.PlotGeography_Cache.ToString() END AS Geography, CASE WHEN S.CountryCache IS NULL 
                      THEN CASE WHEN P1.CountryCache IS NULL THEN CASE WHEN P2.CountryCache IS NULL THEN CASE WHEN P3.CountryCache IS NULL 
                      THEN P4.CountryCache ELSE P3.CountryCache END ELSE P2.CountryCache END ELSE P1.CountryCache END ELSE S.CountryCache END AS CountryCache, 
                      S.PlotID
FROM         dbo.SamplingPlot AS P4 RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P3 ON P4.PlotID = P3.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P2 ON P3.PlotID = P2.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P1 ON P2.PlotID = P1.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS S ON P1.PlotID = S.PartOfPlotID
GO
GRANT SELECT ON [dbo].[SamplingPlotGeoInfo] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlot]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewSamplingPlot]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache,
PlotDescription, InternalNotes, CountryCache, PlotType
FROM            dbo.SamplingPlot
GO
GRANT SELECT ON [dbo].[ViewSamplingPlot] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotAllLocalisations]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewSamplingPlotAllLocalisations]
AS
SELECT        PlotID, PartOfPlotID, LocalisationSystemID, Location1, Location2, DisplayText
FROM            dbo.SamplingPlotAllLocalisations() AS SamplingPlotAllLocalisations

GO
/****** Object:  View [dbo].[ViewSamplingPlotList]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ViewSamplingPlotList]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache,
                         PlotDescription, InternalNotes, DisplayText
FROM            dbo.SamplingPlotList() AS ViewSamplingPlotList

GO
GRANT SELECT ON [dbo].[ViewSamplingPlotList] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewSamplingPlotListForCurrentProject]    Script Date: 19.04.2021 09:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewSamplingPlotListForCurrentProject]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText, ProjectID
FROM            dbo.SamplingPlotListForCurrentProject() AS ViewSamplingPlotListForCurrentProject
GO
GRANT SELECT ON [dbo].[ViewSamplingPlotListForCurrentProject] TO [DiversityWorkbenchUser] AS [dbo]
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (3, NULL, N'MTB (A, CH, D)', NULL, NULL, N'MTB', N'MTB (A, CH, D)', 1, 300, N'Messtischblatt (MTB) / TK25 (used in A, CH, D)', N'MTB', N'Messtischblatt', N'Quadrant', N'Quadrant', N'1cfa403e-2f6d-de11-871c-001ec9d7ae4b', NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (4, NULL, N'Altitude (mNN)', NULL, N'm NN', N'Altitude', N'Altitude (mNN)', 1, 200, N'Altitude above seelevel (mNN)', N'Alt.from', N'Altitude from', N'Alt.to', N'Altitude to', N'1dfa403e-2f6d-de11-871c-001ec9d7ae4b', NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (7, NULL, N'Named area (DiversityGazetteers)', NULL, NULL, N'Gazetteer', N'Named area (DiversityGazetteers)', 1, 10, N'Named area (DiversityGazetteer)', N'Location', N'Location', N'Thes. code', N'Thesaurus code', N'20fa403e-2f6d-de11-871c-001ec9d7ae4b', NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'dbo', CAST(N'2018-12-07T11:07:54.160' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (8, NULL, N'Coordinates WGS84', NULL, NULL, N'Coordinates', N'Coord. WGS84', 1, 130, N'Coordinates of the World Geodetic System 1984', N'Long. (EW)', NULL, N'Lat. (NS)', NULL, N'21fa403e-2f6d-de11-871c-001ec9d7ae4b', NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (10, NULL, N'Exposition', NULL, N'°', N'Exposition', N'Exposition', 1, 600, N'Exposition of e.g. a hillside in relation to the sun', N'Exp. from', N'Exposition', N'to', NULL, N'23fa403e-2f6d-de11-871c-001ec9d7ae4b', NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (11, NULL, N'Slope', NULL, N'°', N'Slope', N'Slope', 1, 700, N'Slope or steepness of the surface of e.g. a hillside', N'Slope from', N'Slope', N'to', NULL, N'24fa403e-2f6d-de11-871c-001ec9d7ae4b', NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (14, NULL, N'Depth', NULL, N'm', N'Height', N'Depth', 1, 201, N'Depth in meters below the surface of e.g. a water body at which the collection was made', N'Depth from', N'All material collected was at least this deep', N'to', N'All material collected was at most this deep', N'27fa403e-2f6d-de11-871c-001ec9d7ae4b', NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (15, NULL, N'Height', NULL, N'm', N'Height', N'Height', 1, 202, N'Height resp. distance from groundlevel when e.g. located in a tree', N'Height from', N'All material collected was at least at this hight', N'to', N'All material collected was at most at this hight', N'28fa403e-2f6d-de11-871c-001ec9d7ae4b', NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (18, 7, N'2. Named area (DiversityGazetteers)', NULL, NULL, N'Gazetteer', N'2. Named area (DiversityGazetteers)', 1, 11, N'2. Named area (DiversityGazetteer)', N'Location', N'Location', N'Thes. code', N'Thesaurus code', N'1664e2f5-07fa-e811-8184-809ad4898767', N'dbo', CAST(N'2018-12-07T11:07:54.173' AS DateTime), N'dbo', CAST(N'2018-12-07T11:07:54.173' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (19, 7, N'3. Named area (DiversityGazetteers)', NULL, NULL, N'Gazetteer', N'3. Named area (DiversityGazetteers)', 1, 12, N'3. Named area (DiversityGazetteer)', N'Location', N'Location', N'Thes. code', N'Thesaurus code', N'1764e2f5-07fa-e811-8184-809ad4898767', N'dbo', CAST(N'2018-12-07T11:07:54.173' AS DateTime), N'dbo', CAST(N'2018-12-07T11:07:54.173' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (20, 7, N'4. Named area (DiversityGazetteers)', NULL, NULL, N'Gazetteer', N'4. Named area (DiversityGazetteers)', 1, 13, N'4. Named area (DiversityGazetteer)', N'Location', N'Location', N'Thes. code', N'Thesaurus code', N'1864e2f5-07fa-e811-8184-809ad4898767', N'dbo', CAST(N'2018-12-07T11:07:54.173' AS DateTime), N'dbo', CAST(N'2018-12-07T11:07:54.173' AS DateTime))
GO
INSERT [dbo].[LocalisationSystem] ([LocalisationSystemID], [LocalisationSystemParentID], [LocalisationSystemName], [DefaultAccuracyOfLocalisation], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayText], [DisplayEnable], [DisplayOrder], [Description], [DisplayTextLocation1], [DescriptionLocation1], [DisplayTextLocation2], [DescriptionLocation2], [RowGUID], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen]) VALUES (21, 7, N'5. Named area (DiversityGazetteers)', NULL, NULL, N'Gazetteer', N'5. Named area (DiversityGazetteers)', 1, 14, N'5. Named area (DiversityGazetteer)', N'Location', N'Location', N'Thes. code', N'Thesaurus code', N'1964e2f5-07fa-e811-8184-809ad4898767', N'dbo', CAST(N'2018-12-07T11:07:54.173' AS DateTime), N'dbo', CAST(N'2018-12-07T11:07:54.173' AS DateTime))
GO

INSERT [dbo].[ProjectProxy] ([ProjectID], [Project], [ProjectURI], [RowGUID]) VALUES (0, N'DiversityWorkbench', NULL, N'57b83eca-eb8e-e611-8136-f9e85bafd01e')
GO
INSERT [dbo].[ProjectUser] ([LoginName], [ProjectID], [RowGUID]) VALUES (N'dbo', 0, N'58b83eca-eb8e-e611-8136-f9e85bafd01e')
GO
INSERT [dbo].[Property] ([PropertyID], [DisplayText], [Description], [PropertyParentID], [PropertyName], [DefaultAccuracyOfProperty], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayEnabled], [DisplayOrder], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID], [PropertyURI]) VALUES (1, N'European Nature Information System (EUNIS)', N'European Nature Information System, http://eunis.eea.eu.int/index.jsp', NULL, N'European Nature Information System (EUNIS)', NULL, NULL, N'Vegetation', 1, NULL, NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'59b83eca-eb8e-e611-8136-f9e85bafd01e', NULL)
GO
INSERT [dbo].[Property] ([PropertyID], [DisplayText], [Description], [PropertyParentID], [PropertyName], [DefaultAccuracyOfProperty], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayEnabled], [DisplayOrder], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID], [PropertyURI]) VALUES (2, N'EUNIS 2012 (European Nature Information System)', NULL, NULL, N'EUNIS 2012 (European Nature Information System)', NULL, NULL, N'Vegetation', 1, NULL, N'dbo', CAST(N'2018-12-07T11:07:54.957' AS DateTime), N'dbo', CAST(N'2018-12-07T11:07:54.957' AS DateTime), N'1a64e2f5-07fa-e811-8184-809ad4898767', N'http://tnt.diversityworkbench.de/ScientificTerms_TNT/Terminology/2')
GO
INSERT [dbo].[Property] ([PropertyID], [DisplayText], [Description], [PropertyParentID], [PropertyName], [DefaultAccuracyOfProperty], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayEnabled], [DisplayOrder], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID], [PropertyURI]) VALUES (10, N'Geographic regions', N'Import aus Tabelle Naturraum der Datenbank Mykologie aus Görlitz', NULL, N'Geographic regions', NULL, NULL, N'Vegetation', 1, NULL, NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'5ab83eca-eb8e-e611-8136-f9e85bafd01e', NULL)
GO
INSERT [dbo].[Property] ([PropertyID], [DisplayText], [Description], [PropertyParentID], [PropertyName], [DefaultAccuracyOfProperty], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayEnabled], [DisplayOrder], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID], [PropertyURI]) VALUES (20, N'Chronostratigraphy', N'Chronostratigraphie nach Angaben der Bayerischen Staatssammlung für Paläontologie', NULL, N'Chronostratigraphy', NULL, NULL, N'Chronostratigraphy', 1, NULL, NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'5bb83eca-eb8e-e611-8136-f9e85bafd01e', NULL)
GO
INSERT [dbo].[Property] ([PropertyID], [DisplayText], [Description], [PropertyParentID], [PropertyName], [DefaultAccuracyOfProperty], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayEnabled], [DisplayOrder], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID], [PropertyURI]) VALUES (30, N'Lithostratigraphy', N'Lithostratigraphie nach Angaben der Bayerischen Staatssammlung für Paläontologie', NULL, N'Lithostratigraphy', NULL, NULL, N'Lithostratigraphy', 1, NULL, NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'5cb83eca-eb8e-e611-8136-f9e85bafd01e', NULL)
GO
INSERT [dbo].[Property] ([PropertyID], [DisplayText], [Description], [PropertyParentID], [PropertyName], [DefaultAccuracyOfProperty], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayEnabled], [DisplayOrder], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID], [PropertyURI]) VALUES (40, N'Lebensraumtyp (LfU)', N'Lebensraumtypen (Bayerisches Landesamt für Umwelt)', NULL, N'Lebensraumtypen (LfU)', NULL, NULL, N'Vegetation', 1, NULL, NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'5db83eca-eb8e-e611-8136-f9e85bafd01e', NULL)
GO
INSERT [dbo].[Property] ([PropertyID], [DisplayText], [Description], [PropertyParentID], [PropertyName], [DefaultAccuracyOfProperty], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayEnabled], [DisplayOrder], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID], [PropertyURI]) VALUES (41, N'Biotoptypen (BFN)', NULL, NULL, N'Biotoptypen (BFN)', NULL, NULL, N'Vegetation', 1, NULL, N'dbo', CAST(N'2018-12-07T11:07:54.970' AS DateTime), N'dbo', CAST(N'2018-12-07T11:07:54.970' AS DateTime), N'1b64e2f5-07fa-e811-8184-809ad4898767', N'http://tnt.diversityworkbench.de/ScientificTerms_TNT/Terminology/41')
GO
INSERT [dbo].[Property] ([PropertyID], [DisplayText], [Description], [PropertyParentID], [PropertyName], [DefaultAccuracyOfProperty], [DefaultMeasurementUnit], [ParsingMethodName], [DisplayEnabled], [DisplayOrder], [LogCreatedBy], [LogCreatedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID], [PropertyURI]) VALUES (50, N'Pflanzengesellschaften', NULL, NULL, N'Pflanzengesellschaften', NULL, NULL, N'Vegetation', 1, NULL, NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'5eb83eca-eb8e-e611-8136-f9e85bafd01e', NULL)
GO
SET IDENTITY_INSERT [dbo].[UserProxy] ON 
;INSERT [dbo].[UserProxy] ([LoginName], [CombinedNameCache], [UserURI], [Queries], [Settings], [AgentURI], [CurrentProjectID], [RowGUID], [ID], [PrivacyConsent], [PrivacyConsentDate]) VALUES (N'dbo', N'Owner of database', NULL, NULL, NULL, NULL, 0, N'5fb83eca-eb8e-e611-8136-f9e85bafd01e', 1, NULL, NULL)
;SET IDENTITY_INSERT [dbo].[UserProxy] OFF
GO
ALTER TABLE [dbo].[LocalisationSystem] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[LocalisationSystem] ADD  CONSTRAINT [DF_LocalisationSystem_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[LocalisationSystem] ADD  CONSTRAINT [DF_LocalisationSystem_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[LocalisationSystem] ADD  CONSTRAINT [DF_LocalisationSystem_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[LocalisationSystem] ADD  CONSTRAINT [DF_LocalisationSystem_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[LocalisationSystem_log] ADD  CONSTRAINT [DF_LocalisationSystem_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[LocalisationSystem_log] ADD  CONSTRAINT [DF_LocalisationSystem_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[LocalisationSystem_log] ADD  CONSTRAINT [DF_LocalisationSystem_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectProxy] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectUser] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[Property] ADD  CONSTRAINT [DF_Property_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[Property] ADD  CONSTRAINT [DF_Property_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[Property] ADD  CONSTRAINT [DF_Property_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[Property] ADD  CONSTRAINT [DF_Property_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[Property] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[Property_log] ADD  CONSTRAINT [DF_Property_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[Property_log] ADD  CONSTRAINT [DF_Property_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[Property_log] ADD  CONSTRAINT [DF_Property_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[SamplingPlot] ADD  CONSTRAINT [DF_SamplingPlot_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[SamplingPlot] ADD  CONSTRAINT [DF_SamplingPlot_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[SamplingPlot] ADD  CONSTRAINT [DF_SamplingPlot_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[SamplingPlot] ADD  CONSTRAINT [DF_SamplingPlot_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[SamplingPlot] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[SamplingPlot_log] ADD  CONSTRAINT [DF_SamplingPlot_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[SamplingPlot_log] ADD  CONSTRAINT [DF_SamplingPlot_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[SamplingPlot_log] ADD  CONSTRAINT [DF_SamplingPlot_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_2_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_2_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_2_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_2_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] ADD  CONSTRAINT [DF__Collectio__RowGU__17D92C16]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation_log] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation_log] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation_log] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[SamplingPlotProperty] ADD  CONSTRAINT [DF_SamplingPlotProperty_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[SamplingPlotProperty] ADD  CONSTRAINT [DF_SamplingPlotProperty_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[SamplingPlotProperty] ADD  CONSTRAINT [DF_SamplingPlotProperty_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[SamplingPlotProperty] ADD  CONSTRAINT [DF_SamplingPlotProperty_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[SamplingPlotProperty] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[SamplingPlotProperty_log] ADD  CONSTRAINT [DF_SamplingPlotProperty_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[SamplingPlotProperty_log] ADD  CONSTRAINT [DF_SamplingPlotProperty_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[SamplingPlotProperty_log] ADD  CONSTRAINT [DF_SamplingPlotProperty_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[SamplingPlotResource] ADD  CONSTRAINT [DF_SamplingPlotResource_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[SamplingPlotResource] ADD  CONSTRAINT [DF_SamplingPlotResource_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[SamplingPlotResource] ADD  CONSTRAINT [DF_SamplingPlotResource_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[SamplingPlotResource] ADD  CONSTRAINT [DF_SamplingPlotResource_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[SamplingPlotResource] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[SamplingPlotResource_log] ADD  CONSTRAINT [DF_SamplingPlotResource_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[SamplingPlotResource_log] ADD  CONSTRAINT [DF_SamplingPlotResource_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[SamplingPlotResource_log] ADD  CONSTRAINT [DF_SamplingPlotResource_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[SamplingProject] ADD  CONSTRAINT [DF_SamplingProject_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[SamplingProject] ADD  CONSTRAINT [DF_SamplingProject_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[SamplingProject] ADD  CONSTRAINT [DF_SamplingProject_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[SamplingProject] ADD  CONSTRAINT [DF_SamplingProject_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[SamplingProject] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[SamplingProject_log] ADD  CONSTRAINT [DF_SamplingProject_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[SamplingProject_log] ADD  CONSTRAINT [DF_SamplingProject_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[SamplingProject_log] ADD  CONSTRAINT [DF_SamplingProject_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[UserProxy] ADD  CONSTRAINT [DF_UserProxy_CombinedNameCache]  DEFAULT (NULL) FOR [CombinedNameCache]
GO
ALTER TABLE [dbo].[UserProxy] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[LocalisationSystem]  WITH CHECK ADD  CONSTRAINT [FK_LocalisationSystem_LocalisationSystem] FOREIGN KEY([LocalisationSystemParentID])
REFERENCES [dbo].[LocalisationSystem] ([LocalisationSystemID])
GO
ALTER TABLE [dbo].[LocalisationSystem] CHECK CONSTRAINT [FK_LocalisationSystem_LocalisationSystem]
GO
ALTER TABLE [dbo].[ProjectUser]  WITH CHECK ADD  CONSTRAINT [FK_ProjectUser_ProjectProxy] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectProxy] ([ProjectID])
GO
ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_ProjectProxy]
GO
ALTER TABLE [dbo].[ProjectUser]  WITH CHECK ADD  CONSTRAINT [FK_ProjectUser_UserProxy] FOREIGN KEY([LoginName])
REFERENCES [dbo].[UserProxy] ([LoginName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_UserProxy]
GO
ALTER TABLE [dbo].[SamplingPlot]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlot_SamplingPlot] FOREIGN KEY([PartOfPlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID])
GO
ALTER TABLE [dbo].[SamplingPlot] CHECK CONSTRAINT [FK_SamplingPlot_SamplingPlot]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotLocalisation_LocalisationSystem] FOREIGN KEY([LocalisationSystemID])
REFERENCES [dbo].[LocalisationSystem] ([LocalisationSystemID])
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] CHECK CONSTRAINT [FK_SamplingPlotLocalisation_LocalisationSystem]
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotLocalisation_SamplingPlot] FOREIGN KEY([PlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID])
GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] CHECK CONSTRAINT [FK_SamplingPlotLocalisation_SamplingPlot]
GO
ALTER TABLE [dbo].[SamplingPlotProperty]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotProperty_Property] FOREIGN KEY([PropertyID])
REFERENCES [dbo].[Property] ([PropertyID])
GO
ALTER TABLE [dbo].[SamplingPlotProperty] CHECK CONSTRAINT [FK_SamplingPlotProperty_Property]
GO
ALTER TABLE [dbo].[SamplingPlotProperty]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotProperty_SamplingPlot] FOREIGN KEY([PlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID])
GO
ALTER TABLE [dbo].[SamplingPlotProperty] CHECK CONSTRAINT [FK_SamplingPlotProperty_SamplingPlot]
GO
ALTER TABLE [dbo].[SamplingPlotResource]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotResource_SamplingPlot] FOREIGN KEY([PlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SamplingPlotResource] CHECK CONSTRAINT [FK_SamplingPlotResource_SamplingPlot]
GO
ALTER TABLE [dbo].[SamplingProject]  WITH CHECK ADD  CONSTRAINT [FK_SamplingProject_ProjectProxy] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectProxy] ([ProjectID])
GO
ALTER TABLE [dbo].[SamplingProject] CHECK CONSTRAINT [FK_SamplingProject_ProjectProxy]
GO
ALTER TABLE [dbo].[SamplingProject]  WITH CHECK ADD  CONSTRAINT [FK_SamplingProject_SamplingPlot] FOREIGN KEY([PlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID])
GO
ALTER TABLE [dbo].[SamplingProject] CHECK CONSTRAINT [FK_SamplingProject_SamplingPlot]
GO
/****** Object:  StoredProcedure [dbo].[procInsertSamplingPlotCopy]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [dbo].[procInsertSamplingPlotCopy]
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

-- SamplingProject
INSERT INTO SamplingProject
                      (PlotID, ProjectID)
SELECT     @PlotID, ProjectID
FROM         SamplingProject
WHERE     (PlotID = @OriginalPlotID)

-- SamplingPlotLocalisation
INSERT INTO SamplingPlotLocalisation
	(PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, 
	ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache)
SELECT  @PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, 
		ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache
FROM         SamplingPlotLocalisation
WHERE     (PlotID = @OriginalPlotID)

-- SamplingPlotProperty
INSERT INTO SamplingPlotProperty
	(PlotID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache)
SELECT  @PlotID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache
FROM         SamplingPlotProperty
WHERE     (PlotID = @OriginalPlotID)

-- SamplingPlotResource
INSERT INTO SamplingPlotResource
	(PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes)
SELECT  @PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes
FROM         SamplingPlotResource
WHERE     (PlotID = @OriginalPlotID)

SELECT @PlotID

GO
GRANT EXECUTE ON [dbo].[procInsertSamplingPlotCopy] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Trigger [dbo].[trgDelLocalisationSystem]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create delete trigger

CREATE TRIGGER [dbo].[trgDelLocalisationSystem] ON [dbo].[LocalisationSystem] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO LocalisationSystem_Log (LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, DefaultAccuracyOfLocalisation, DefaultMeasurementUnit, ParsingMethodName, DisplayText, DisplayEnable, DisplayOrder, Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2, DescriptionLocation2, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.LocalisationSystemID, deleted.LocalisationSystemParentID, deleted.LocalisationSystemName, deleted.DefaultAccuracyOfLocalisation, deleted.DefaultMeasurementUnit, deleted.ParsingMethodName, deleted.DisplayText, deleted.DisplayEnable, deleted.DisplayOrder, deleted.Description, deleted.DisplayTextLocation1, deleted.DescriptionLocation1, deleted.DisplayTextLocation2, deleted.DescriptionLocation2, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED


GO
ALTER TABLE [dbo].[LocalisationSystem] ENABLE TRIGGER [trgDelLocalisationSystem]
GO
/****** Object:  Trigger [dbo].[trgUpdLocalisationSystem]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create update trigger

CREATE TRIGGER [dbo].[trgUpdLocalisationSystem] ON [dbo].[LocalisationSystem] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO LocalisationSystem_Log (LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, DefaultAccuracyOfLocalisation, DefaultMeasurementUnit, ParsingMethodName, DisplayText, DisplayEnable, DisplayOrder, Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2, DescriptionLocation2, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.LocalisationSystemID, deleted.LocalisationSystemParentID, deleted.LocalisationSystemName, deleted.DefaultAccuracyOfLocalisation, deleted.DefaultMeasurementUnit, deleted.ParsingMethodName, deleted.DisplayText, deleted.DisplayEnable, deleted.DisplayOrder, deleted.Description, deleted.DisplayTextLocation1, deleted.DescriptionLocation1, deleted.DisplayTextLocation2, deleted.DescriptionLocation2, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update LocalisationSystem
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM LocalisationSystem, deleted 
where 1 = 1 
AND LocalisationSystem.LocalisationSystemID = deleted.LocalisationSystemID

GO
ALTER TABLE [dbo].[LocalisationSystem] ENABLE TRIGGER [trgUpdLocalisationSystem]
GO
/****** Object:  Trigger [dbo].[trgDelProperty]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create delete trigger

CREATE TRIGGER [dbo].[trgDelProperty] ON [dbo].[Property] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Property_Log (PropertyID, DisplayText, Description, PropertyParentID, PropertyName, DefaultAccuracyOfProperty, DefaultMeasurementUnit, ParsingMethodName, DisplayEnabled, DisplayOrder, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.PropertyID, deleted.DisplayText, deleted.Description, deleted.PropertyParentID, deleted.PropertyName, deleted.DefaultAccuracyOfProperty, deleted.DefaultMeasurementUnit, deleted.ParsingMethodName, deleted.DisplayEnabled, deleted.DisplayOrder, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED


GO
ALTER TABLE [dbo].[Property] ENABLE TRIGGER [trgDelProperty]
GO
/****** Object:  Trigger [dbo].[trgUpdProperty]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create update trigger

CREATE TRIGGER [dbo].[trgUpdProperty] ON [dbo].[Property] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Property_Log (PropertyID, DisplayText, Description, PropertyParentID, PropertyName, DefaultAccuracyOfProperty, DefaultMeasurementUnit, ParsingMethodName, DisplayEnabled, DisplayOrder, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.PropertyID, deleted.DisplayText, deleted.Description, deleted.PropertyParentID, deleted.PropertyName, deleted.DefaultAccuracyOfProperty, deleted.DefaultMeasurementUnit, deleted.ParsingMethodName, deleted.DisplayEnabled, deleted.DisplayOrder, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update Property
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Property, deleted 
where 1 = 1 
AND Property.PropertyID = deleted.PropertyID

GO
ALTER TABLE [dbo].[Property] ENABLE TRIGGER [trgUpdProperty]
GO
/****** Object:  Trigger [dbo].[trgDelSamplingPlot]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingPlot] ON [dbo].[SamplingPlot] 
--FOR DELETE AS 
--INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, PlotType, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, CountryCache, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.PlotType, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.CountryCache, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [dbo].[trgDelSamplingPlot] ON [dbo].[SamplingPlot] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, PlotType, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, CountryCache, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.PlotType, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.CountryCache, deleted.RowGUID,  'D'
FROM DELETED


GO
ALTER TABLE [dbo].[SamplingPlot] ENABLE TRIGGER [trgDelSamplingPlot]
GO
/****** Object:  Trigger [dbo].[trgUpdSamplingPlot]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create update trigger

-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgUpdSamplingPlot] ON [dbo].[SamplingPlot] 
--FOR UPDATE AS
--INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, PlotType, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, CountryCache, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.PlotType, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.CountryCache, deleted.RowGUID,  'U'
--FROM DELETED
--Update SamplingPlot
--set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
--FROM SamplingPlot, deleted 
--where 1 = 1 
--AND SamplingPlot.PlotID = deleted.PlotID
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [dbo].[trgUpdSamplingPlot] ON [dbo].[SamplingPlot] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, PlotType, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, CountryCache, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.PlotType, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.CountryCache, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlot
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlot, deleted 
where 1 = 1 
AND SamplingPlot.PlotID = deleted.PlotID

GO
ALTER TABLE [dbo].[SamplingPlot] ENABLE TRIGGER [trgUpdSamplingPlot]
GO
/****** Object:  Trigger [dbo].[trgDelSamplingPlotLocalisation]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
--FOR DELETE AS 
--INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [dbo].[trgDelSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] ENABLE TRIGGER [trgDelSamplingPlotLocalisation]
GO
/****** Object:  Trigger [dbo].[trgInsSamplingPlotLocalisation]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   [trgInsSamplingPlotLocalisation] ################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgInsSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR INSERT AS
/*
Setting the average coordinates if the entered values are numeric
*/
Update S
set S.AverageLongitudeCache = cast(replace(I.Location1, ',', '.') as varchar)
, S.AverageLatitudeCache = cast(replace(I.Location2, ',', '.') as varchar)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND isnumeric(I.Location1 + 'e0') = 1
AND isnumeric(I.Location2 + 'e0') = 1
AND I.LocalisationSystemID in (3,7,8)

/*
Setting the average coordinates if the entered values are numeric - second trial
*/
Update S
set S.AverageLongitudeCache = cast(replace(I.Location1, ',', '.') as float)
, S.AverageLatitudeCache = cast(replace(I.Location2, ',', '.') as float)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND isnumeric(replace(I.Location1, ',', '.') + 'e0') = 1
AND isnumeric(replace(I.Location2, ',', '.') + 'e0') = 1
AND I.LocalisationSystemID in (3,7,8)

/*
Setting the average altitude if the entered values are numeric
*/
Update S
set S.AverageAltitudeCache = 
CASE 
	WHEN I.Location2 IS NULL OR RTRIM(i.Location2) = ''
	THEN CAST(REPLACE(I.Location1, ',', '.') AS FLOAT)
	ELSE
	(CAST(REPLACE(I.Location1, ',', '.') AS FLOAT) + CAST(REPLACE(I.Location2, ',', '.') AS FLOAT))/2
END
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.LocalisationSystemID in (4)
AND ISNUMERIC(I.Location1) = 1
AND NOT I.Location1 IS NULL

/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set Geography =
case when I.Geography IS null OR I.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+' ' +replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')', 4326)
else I.Geography end
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 180
AND (S.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
OR S.Geography IS NULL)

/*
Setting the cached geography of the plot if it is missing or for WGS84
*/
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P, inserted I
where 1 = 1
AND L.PlotID = I.PlotID
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL)

GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] ENABLE TRIGGER [trgInsSamplingPlotLocalisation]
GO
/****** Object:  Trigger [dbo].[trgUpdSamplingPlotLocalisation]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   [trgUpdSamplingPlotLocalisation] ################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR UPDATE AS
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState)
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED
Update SamplingPlotLocalisation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotLocalisation, deleted
where 1 = 1
AND SamplingPlotLocalisation.LocalisationSystemID = deleted.LocalisationSystemID
AND SamplingPlotLocalisation.PlotID = deleted.PlotID

/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set S.AverageAltitudeCache = 
CASE 
	WHEN I.Location2 IS NULL OR RTRIM(i.Location2) = ''
	THEN CAST(REPLACE(I.Location1, ',', '.') AS FLOAT)
	ELSE
	(CAST(REPLACE(I.Location1, ',', '.') AS FLOAT) + CAST(REPLACE(I.Location2, ',', '.') AS FLOAT))/2
END
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.LocalisationSystemID in (4)
AND ISNUMERIC(I.Location1) = 1
AND NOT I.Location1 IS NULL

/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set Geography =
case when I.Geography IS null OR I.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+' ' +replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')', 4326)
else I.Geography end
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 180
AND (S.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
OR S.Geography IS NULL)

/*
Setting the cached geography of the plot if it is missing or for WGS84
*/
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P, inserted I
where 1 = 1
AND L.PlotID = I.PlotID
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL)

GO
ALTER TABLE [dbo].[SamplingPlotLocalisation] ENABLE TRIGGER [trgUpdSamplingPlotLocalisation]
GO
/****** Object:  Trigger [dbo].[trgDelSamplingPlotProperty]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create delete trigger

CREATE TRIGGER [dbo].[trgDelSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotProperty_Log (PlotID, PropertyID, PropertyGeometry, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, PropertyURI, DisplayText, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.PropertyID, deleted.PropertyGeometry, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.PropertyURI, deleted.DisplayText, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID,  'D'
FROM DELETED


GO
ALTER TABLE [dbo].[SamplingPlotProperty] ENABLE TRIGGER [trgDelSamplingPlotProperty]
GO
/****** Object:  Trigger [dbo].[trgUpdSamplingPlotProperty]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create update trigger

CREATE TRIGGER [dbo].[trgUpdSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotProperty_Log (PlotID, PropertyID, PropertyGeometry, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, PropertyURI, DisplayText, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.PropertyID, deleted.PropertyGeometry, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.PropertyURI, deleted.DisplayText, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotProperty
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotProperty, deleted 
where 1 = 1 
AND SamplingPlotProperty.PlotID = deleted.PlotID
AND SamplingPlotProperty.PropertyID = deleted.PropertyID

GO
ALTER TABLE [dbo].[SamplingPlotProperty] ENABLE TRIGGER [trgUpdSamplingPlotProperty]
GO
/****** Object:  Trigger [dbo].[trgDelSamplingPlotResource]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
--FOR DELETE AS 
--INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [dbo].[trgDelSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


GO
ALTER TABLE [dbo].[SamplingPlotResource] ENABLE TRIGGER [trgDelSamplingPlotResource]
GO
/****** Object:  Trigger [dbo].[trgUpdSamplingPlotResource]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create update trigger

-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgUpdSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
--FOR UPDATE AS
--INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
--FROM DELETED
--Update SamplingPlotResource
--set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
--FROM SamplingPlotResource, deleted 
--where 1 = 1 
--AND SamplingPlotResource.PlotID = deleted.PlotID
--AND SamplingPlotResource.ResourceURI = deleted.ResourceURI
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [dbo].[trgUpdSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotResource
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotResource, deleted 
where 1 = 1 
AND SamplingPlotResource.PlotID = deleted.PlotID
AND SamplingPlotResource.ResourceURI = deleted.ResourceURI

GO
ALTER TABLE [dbo].[SamplingPlotResource] ENABLE TRIGGER [trgUpdSamplingPlotResource]
GO
/****** Object:  Trigger [dbo].[trgDelSamplingProject]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create delete trigger

-- Delete trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgDelSamplingProject] ON [dbo].[SamplingProject] 
--FOR DELETE AS 
--INSERT INTO SamplingProject_Log (PlotID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
--FROM DELETED
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [dbo].[trgDelSamplingProject] ON [dbo].[SamplingProject] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingProject_Log (PlotID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


GO
ALTER TABLE [dbo].[SamplingProject] ENABLE TRIGGER [trgDelSamplingProject]
GO
/****** Object:  Trigger [dbo].[trgUpdSamplingProject]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Create update trigger

-- Update trigger is NO STANDARD trigger ----------------------------------------------------------------

--CREATE TRIGGER [trgUpdSamplingProject] ON [dbo].[SamplingProject] 
--FOR UPDATE AS
--INSERT INTO SamplingProject_Log (PlotID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
--SELECT deleted.PlotID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
--FROM DELETED
--Update SamplingProject
--set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
--FROM SamplingProject, deleted 
--where 1 = 1 
--AND SamplingProject.PlotID = deleted.PlotID
--AND SamplingProject.ProjectID = deleted.ProjectID
--


----------------------------------------------------------------------------------------

CREATE TRIGGER [dbo].[trgUpdSamplingProject] ON [dbo].[SamplingProject] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingProject_Log (PlotID, ProjectID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.ProjectID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingProject
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingProject, deleted 
where 1 = 1 
AND SamplingProject.PlotID = deleted.PlotID
AND SamplingProject.ProjectID = deleted.ProjectID

GO
ALTER TABLE [dbo].[SamplingProject] ENABLE TRIGGER [trgUpdSamplingProject]
GO
/****** Object:  Trigger [dbo].[trgUpdUserProxy]    Script Date: 19.04.2021 09:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   UserProxy - Trigger for setting PrivacyConsentDate   #######################################################
--#####################################################################################################################
CREATE TRIGGER [dbo].[trgUpdUserProxy] ON [dbo].[UserProxy] 
FOR UPDATE AS 
declare @PC bit 
if (select count(*) from deleted) = 1 
begin 
  set @PC = (select case when I.PrivacyConsent <> D.PrivacyConsent 
    or (I.PrivacyConsent is null and not D.PrivacyConsent is null) 
    or (not I.PrivacyConsent is null and D.PrivacyConsent is null) 
    then 1 else 0 end from inserted I, deleted D) 
  if (@PC = 1) 
  begin 
    UPDATE U SET PrivacyConsentDate = GETDATE()  
    FROM UserProxy U, deleted D 
    WHERE U.ID = D.ID 
  end 
end 
GO
ALTER TABLE [dbo].[UserProxy] ENABLE TRIGGER [trgUpdUserProxy]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the localisation system (= Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LocalisationSystemID of the superior LocalisationSystem' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the system used for the determination of the place of the collection, e. g. Gauss-Krüger, MTB, GIS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LocalisationSystemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfLocalisation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfLocalisation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfLocalisation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default for the accuracy of values that can be reached with this method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfLocalisation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default measurement unit for the localisation system, e.g. m, geograpic coordinates' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DefaultMeasurementUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal value, specifying a programming method used for parsing text in fields Location1/Location2 in table SamplingPlotLocalisation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'ParsingMethodName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the localisation system as displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Specifies if this item is enabled to be used within the database. LocalisationSystems can be disabled to avoid seeing them, but to keep the definition for the future.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the localisation method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayTextLocation1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayTextLocation1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=3915 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayTextLocation1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the attribute Location1 in the table CollectionGeography as displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayTextLocation1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DescriptionLocation1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DescriptionLocation1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=1620 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DescriptionLocation1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the attribute Location1 in the table CollectionGeography as displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DescriptionLocation1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayTextLocation2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayTextLocation2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=65535 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayTextLocation2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the attribute Location2 in the table CollectionGeography as displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DisplayTextLocation2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnHidden', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DescriptionLocation2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnOrder', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DescriptionLocation2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_ColumnWidth', @value=1485 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DescriptionLocation2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the attribute Location2 in the table CollectionGeography as displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'DescriptionLocation2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DefaultView', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geographic localisation systems, e.g. coordinates' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Filter', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_OrderBy', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_OrderByOn', @value=NULL , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Orientation', @value=0 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_TableMaxRecords', @value=10000 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocalisationSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project to which the specimen belongs (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or title of the project as shown in a user interface  (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'Project'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the sampling property, Primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the property as shown e.g. in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description of the property' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PropertyID of the superior property' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the property used to describe the place of the collection, may be derived from content of the module DiversityScientificTerms e.g. Pflanzengesellschaften' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default for the accuracy of values that can be reached with this method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfProperty'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal value, specifying a programming method used for parsing text in field PropertyValue in table CollectionEventProperty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'ParsingMethodName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Specifies if this item is enabled to be used within the database. Properties can be disabled to avoid seeing them, but to keep the definition for the future.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DisplayEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the publishing database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or address of the server where the publishing database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Server'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The port used by the server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the sampling plot, Primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'PlotID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the plot of which the current plot is a part of, Foreign key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'PartOfPlotID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The identifier or name of the plot' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'PlotIdentifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The cached geography of the plot as geographical entry, e.g. as a polygon derived from the geography in table SamplingPlotPoints' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'PlotGeography_Cache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The cached geometry of a sampling plot, e.g. as a polygon derived from the geometry in table SamplingPlotPoints' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'PlotGeometry_Cache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A short text, describing the sampling plot' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'PlotDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the plot, e.g. trap' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'PlotType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes regarding the sampling plot, not published on the web' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The country where the collection sampling plot is located. Cached value derived from an entry in SamplingPlotLocalisation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'CountryCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A sampling plot with its geographical position and geometry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of SamplingPlot (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'PlotID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of LocalisationSystem (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'LocalisationSystemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Either a named location selected from a thesaurus (e. g. ''Germany, Bavaria, Kleindingharting'') or altitude range or other values (e. g. 100-200 m)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'Location1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Corresponding value to Location1 e.g. ID or URI of gazetteer or thesaurus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'Location2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The accuracy of the determination of this locality' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'LocationAccuracy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the location' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'LocationNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of the determination of the geographical localisation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'DeterminationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Distance from the specified place to the real location of the sampling plot (m)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'DistanceToLocation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Direction from the specified place to the real location of the sampling plot (Degrees rel. to north)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'DirectionToLocation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent (person or organization) responsible for this entry.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'ResponsibleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or organisation responsible for the data (see e.g. module DiversityAgents)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geography of the localisation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'Geography'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Calculated altitude as parsed from the location fields' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'AverageAltitudeCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Calculated latitude as parsed from the location fields' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'AverageLatitudeCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Calculated longitude as parsed from the location fields' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'AverageLongitudeCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geographic localisation of a sampling plot' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotLocalisation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the sampling plot, foreign key and part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PlotID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the property to which the sampling plot property belongs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geometry of the property in relation to the geometry of the sampling plot, e.g. MultiPoints for lower and upper border of a stratigraphic layer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyGeometry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to an external datasource e.g. DiversityScientificTerms' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The text for the property as shown e.g. in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A cached text of the complete name of the descriptor including superior categories if present' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyHierarchyCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of a captured feature  e.g. temperature, pH, vegetation etc. If there is a range this is the lower or first value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent (person or organization) responsible for this entry.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'ResponsibleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or organisation responsible for the data (see e.g. module DiversityAgents)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the property of the colletion site.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'For numeric values - a cached average value according to the' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'AverageValueCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the sampling plot, foreign key and part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'PlotID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to the file or link of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'ResourceURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent (person or organization) responsible for this entry.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'ResponsibleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or organisation responsible for the data (see e.g. module DiversityAgents)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the resource.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the sampling plot, foreign key and part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingProject', @level2type=N'COLUMN',@level2name=N'PlotID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project to which the sampling plot belongs (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingProject', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingProject', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingProject', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingProject', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingProject', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The projects to which a sampling plot is related' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingProject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A login name which the user uses  for access the DivesityWorkbench, Microsoft domains, etc..' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The short name of the user, e.g. P. Smith' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'CombinedNameCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of a user in a remote module, e.g. refering to UserInfo.UserID in database DiversityUsers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'UserURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Queries created by the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Queries'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The settings for the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Settings'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A link to a DiversityAgents module where further informations about the user is available.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The current project ID selected by the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'CurrentProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user consents the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time and date when the user consented or refused the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsentDate'
GO

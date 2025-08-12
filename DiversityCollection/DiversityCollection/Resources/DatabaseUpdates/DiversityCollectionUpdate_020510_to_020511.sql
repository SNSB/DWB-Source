--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT


declare @Version varchar(10)
set @Version = (SELECT [dbo].[Version]())
IF (SELECT [dbo].[Version]()) <> '02.05.09'
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version 02.05.09. Current version = ' + @Version
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  EventSeriesChildNodes  ####################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[EventSeriesChildNodes] (@ID int)  
RETURNS @ItemList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (500)  NULL ,
   Notes nvarchar (500) ,
   [Geography] geography)

BEGIN
RETURN
END
GO



ALTER FUNCTION [dbo].[EventSeriesChildNodes] (@ID int)  
RETURNS @ItemList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (500)  NULL ,
   Notes nvarchar (500) ,
   [Geography] geography)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 09.06.2009
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (500)  NULL ,
   Notes nvarchar (500)  ,
   [Geography] geography)

-- insert the first childs into the table
 INSERT @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes, [Geography]) 
	SELECT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes, [Geography]
	FROM CollectionEventSeries WHERE SeriesParentID = @ID 

-- for each child get the childs
   DECLARE HierarchyCursor  CURSOR for
   select SeriesID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   AND @ParentID not in (select SeriesID from @TempItem)
   AND @ParentID not in (select SeriesParentID from @TempItem)
   BEGIN
		insert into @TempItem select SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes, [Geography] 
		from dbo.EventSeriesChildNodes (@ParentID) where SeriesID not in (select SeriesID from @TempItem)
   		FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes) 
   SELECT distinct SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description],  Notes
   FROM @TempItem ORDER BY DateStart
 UPDATE L SET [Geography] = E.[Geography]
 FROM @ItemList L, CollectionEventSeries E
 WHERE E.SeriesID = L.SeriesID

   RETURN
END


GO


--#####################################################################################################################
--######   EventSeriesHierarchy   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[EventSeriesHierarchy] (@SeriesID int)  
RETURNS @EventSeriesList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.EventSeriesTopID(@SeriesID) )

-- get the ID s of the child nodes
DECLARE @TempItem TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

	INSERT @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes, [Geography]) 
	SELECT * FROM dbo.EventSeriesChildNodes (@TopID)

-- copy the root node in the result list
   INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes)
   SELECT DISTINCT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes
   FROM CollectionEventSeries
   WHERE CollectionEventSeries.SeriesID = @TopID
   AND SeriesID NOT IN (SELECT SeriesID FROM @EventSeriesList)

   -- copy the child nodes into the result list
   INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes)
   SELECT DISTINCT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes
   FROM CollectionEventSeries
   WHERE CollectionEventSeries.SeriesID in (select SeriesID from @TempItem)
   AND SeriesID NOT IN (SELECT SeriesID FROM @EventSeriesList)
   ORDER BY DateStart
   
   -- set the geography
	UPDATE L SET [Geography] = E.[Geography]
	FROM @EventSeriesList L, CollectionEventSeries E
	WHERE E.SeriesID = L.SeriesID

   RETURN
END

GO


--#####################################################################################################################
--######   CollectionEventSeriesHierarchy   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionEventSeriesHierarchy] (@SeriesID int)  
RETURNS @EventSeriesList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.EventSeriesTopID(@SeriesID) )

-- get the ID s of the child nodes
DECLARE @TempItem TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

	INSERT @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes, [Geography]) 
	SELECT * FROM dbo.EventSeriesChildNodes (@TopID)

-- copy the root node in the result list
   INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes)
   SELECT DISTINCT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes
   FROM CollectionEventSeries
   WHERE CollectionEventSeries.SeriesID = @TopID
   AND SeriesID NOT IN (SELECT SeriesID FROM @EventSeriesList)

   -- copy the child nodes into the result list
   INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes)
   SELECT DISTINCT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes
   FROM CollectionEventSeries
   WHERE CollectionEventSeries.SeriesID in (select SeriesID from @TempItem)
   AND SeriesID NOT IN (SELECT SeriesID FROM @EventSeriesList)
   ORDER BY DateStart
   
   -- set the geography
	UPDATE L SET [Geography] = E.[Geography]
	FROM @EventSeriesList L, CollectionEventSeries E
	WHERE E.SeriesID = L.SeriesID

   RETURN
END





GO

--#####################################################################################################################
--######   EventSeriesSuperiorList   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[EventSeriesSuperiorList] (@SeriesID int)  
RETURNS @EventSeriesList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

/*
Returns a table that lists all the Series above the given Series.
MW 02.01.2006
*/
AS
BEGIN

	while (not @SeriesID is null)
		begin
		INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes)
		SELECT DISTINCT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes
		FROM CollectionEventSeries
		WHERE CollectionEventSeries.SeriesID = @SeriesID
		AND CollectionEventSeries.SeriesID NOT IN (SELECT SeriesID FROM @EventSeriesList)
		set @SeriesID = (select SeriesParentID from CollectionEventSeries where SeriesID = @SeriesID)
		IF @SeriesID = (select SeriesParentID from CollectionEventSeries where SeriesID = @SeriesID)
			begin 
			set @SeriesID = null
			end 
		end
		
	UPDATE L SET [Geography] = E.[Geography]
	FROM @EventSeriesList L, CollectionEventSeries E
	WHERE E.SeriesID = L.SeriesID

   RETURN
END


GO



--#####################################################################################################################
--######   setting DateStart in  CollectionEventSeries  ######################################################################################
--#####################################################################################################################


Update e set [DateStart] = [DateCache]
  FROM [DiversityCollection].[dbo].[CollectionEventSeries]
e where Not e.DateCache is null and e.DateStart is null

--#####################################################################################################################
--######   setting DateStart in  CollectionEventSeries  ######################################################################################
--#####################################################################################################################


UPDATE [DiversityCollection].[dbo].[CollMaterialCategory_Enum]
   SET [Description] = 'dried specimen mounted on sheet as stored in a botanical collection'
 WHERE Code = 'herbarium sheets'
GO

UPDATE [DiversityCollection].[dbo].[CollMaterialCategory_Enum]
   SET DisplayText = 'herbarium sheet'
 WHERE Code = 'herbarium sheets'

UPDATE [DiversityCollection].[dbo].[CollMaterialCategory_Enum]
   SET DisplayText = 'culture'
 WHERE Code = 'cultures'
 
 
 
update    m set m.DisplayOrder =   506
  FROM [DiversityCollection].[dbo].[CollMaterialCategory_Enum] m
  where m.Code = 'skull'

update    m set m.DisplayOrder =   501
  FROM [DiversityCollection].[dbo].[CollMaterialCategory_Enum] m
  where m.Code = 'complete skeleton'
  
update    m set m.DisplayOrder =   502
  FROM [DiversityCollection].[dbo].[CollMaterialCategory_Enum] m
  where m.Code = 'incomplete skeleton'

update    m set m.DisplayOrder =   507
  FROM [DiversityCollection].[dbo].[CollMaterialCategory_Enum] m
  where m.Code = 'tooth'




GO


--#####################################################################################################################
--######   Korrektur FirstLines   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[FirstLines] 
(@CollectionSpecimenIDs varchar(8000))   
RETURNS @List TABLE (
	[CollectionSpecimenID] [int] Primary key, --
	[Accession_number] [nvarchar](50) NULL, --
-- WITHHOLDINGREASONS
	[Data_withholding_reason] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collector] [nvarchar](255) NULL, --
--CollectionEvent
	[Collectors_event_number] [nvarchar](50) NULL, --
	[Collection_day] [tinyint] NULL, --
	[Collection_month] [tinyint] NULL, --
	[Collection_year] [smallint] NULL, --
	[Collection_date_supplement] [nvarchar](100) NULL, --
	[Collection_time] [varchar](50) NULL, --
	[Collection_time_span] [varchar](50) NULL, --
	[Country] [nvarchar](50) NULL, --
	[Locality_description] [nvarchar](max) NULL, --
	[Habitat_description] [nvarchar](max) NULL, -- 
	[Collecting_method] [nvarchar](max) NULL, --
	[Collection_event_notes] [nvarchar](max) NULL, --
--Localisation
	[Named_area] [nvarchar](255) NULL, -- 
	[NamedAreaLocation2] [nvarchar](255) NULL, --
	[Remove_link_to_gazetteer] [int] NULL,
	[Distance_to_location] [varchar](50) NULL, --
	[Direction_to_location] [varchar](50) NULL, --
	[Longitude] [nvarchar](255) NULL, --
	[Latitude] [nvarchar](255) NULL, --
	[Coordinates_accuracy] [nvarchar](50) NULL, --
	[Link_to_GoogleMaps] [int] NULL,
	[Altitude_from] [nvarchar](255) NULL, --
	[Altitude_to] [nvarchar](255) NULL, --
	[Altitude_accuracy] [nvarchar](50) NULL, --
	[MTB] [nvarchar](255) NULL, --
	[Quadrant] [nvarchar](255) NULL, --
	[Notes_for_MTB] [nvarchar](max) NULL, --
	[Sampling_plot] [nvarchar](255) NULL, --
	[Link_to_SamplingPlots] [nvarchar](255) NULL, --
	[Remove_link_to_SamplingPlots] [int] NULL,
	[Accuracy_of_sampling_plot] [nvarchar](50) NULL, --
	[Latitude_of_sampling_plot] [real] NULL, --
	[Longitude_of_sampling_plot] [real] NULL, --
--Properties
	[Geographic_region] [nvarchar](255) NULL, --
	[Lithostratigraphy] [nvarchar](255) NULL, --
	[Chronostratigraphy] [nvarchar](255) NULL, --
--Agent
	[Collectors_name] [nvarchar](255) NULL, --
	[Link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_collector] [int] NULL,
	[Collectors_number] [nvarchar](50) NULL, --
	[Notes_about_collector] [nvarchar](max) NULL, --
--Accession
	[Accession_day] [tinyint] NULL, --
	[Accession_month] [tinyint] NULL, --
	[Accession_year] [smallint] NULL, --
	[Accession_date_supplement] [nvarchar](255) NULL, --
--Depositor
	[Depositors_name] [nvarchar](255) NULL, --
	[Depositors_link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_Depositor] [int] NULL,
	[Depositors_accession_number] [nvarchar](50) NULL, --
--Exsiccate
	[Exsiccata_abbreviation] [nvarchar](255) NULL, --
	[Link_to_DiversityExsiccatae] [varchar](255) NULL, --
	[Remove_link_to_exsiccatae] [int] NULL,
	[Exsiccata_number] [nvarchar](50) NULL, --
--Notes
	[Original_notes] [nvarchar](max) NULL, --
	[Additional_notes] [nvarchar](max) NULL, --
	[Internal_notes] [nvarchar](max) NULL, --
--Label
	[Label_title] [nvarchar](255) NULL, --
	[Label_type] [nvarchar](50) NULL, --
	[Label_transcription_state] [nvarchar](50) NULL, --
	[Label_transcription_notes] [nvarchar](255) NULL, --
	[Problems] [nvarchar](255) NULL, --
--1. Organism
	[Taxonomic_group] [nvarchar](50) NULL, --
	[Relation_type] [nvarchar](50) NULL, --
	[Colonised_substrate_part] [nvarchar](255) NULL, --
	[Life_stage] [nvarchar](255) NULL, --
	[Gender] [nvarchar](50) NULL, --
	[Number_of_units] [smallint] NULL, --
	[Circumstances] [nvarchar](50) NULL, -- 
	[Order_of_taxon] [nvarchar](255) NULL, --
	[Family_of_taxon] [nvarchar](255) NULL, --
	[Identifier_of_organism] [nvarchar](50) NULL, --
	[Description_of_organism] [nvarchar](50) NULL, --
	[Only_observed] [bit] NULL, --
	[Notes_for_organism] [nvarchar](max) NULL, --
--1. Identification
	[Taxonomic_name] [nvarchar](255) NULL, --
	[Link_to_DiversityTaxonNames] [varchar](255) NULL, --
	[Remove_link_for_identification] [int] NULL, 
	[Vernacular_term] [nvarchar](255) NULL, --
	[Identification_day] [tinyint] NULL, -- 
	[Identification_month] [tinyint] NULL, --
	[Identification_year] [smallint] NULL, --
	[Identification_category] [nvarchar](50) NULL, --
	[Identification_qualifier] [nvarchar](50) NULL, --
	[Type_status] [nvarchar](50) NULL, --
	[Type_notes] [nvarchar](max) NULL, --
	[Notes_for_identification] [nvarchar](max) NULL, --
	[Reference_title] [nvarchar](255) NULL, --
	[Link_to_DiversityReferences] [varchar](255) NULL, --
	[Remove_link_for_reference] [int] NULL,
	[Responsible] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_responsible] [varchar](255) NULL, --
	[Remove_link_for_determiner] [int] NULL,
	[Analysis] [nvarchar](50) NULL, --
	[AnalysisID] [int] NULL, --
	[Analysis_number] [nvarchar](50) NULL, --
	[Analysis_result] [nvarchar](max) NULL, --
--2. Organism	
	[Taxonomic_group_of_second_organism] [nvarchar](50) NULL, --
	[Life_stage_of_second_organism] [nvarchar](255) NULL, --
	[Gender_of_second_organism] [nvarchar](50) NULL, --
	[Number_of_units_of_second_organism] [smallint] NULL, --
	[Circumstances_of_second_organism] [nvarchar](50) NULL, -- 
	[Identifier_of_second_organism] [nvarchar](50) NULL, --
	[Description_of_second_organism] [nvarchar](50) NULL, --
	[Only_observed_of_second_organism] [bit] NULL, --
	[Notes_for_second_organism] [nvarchar](max) NULL, --
--2. Identification	
	[Taxonomic_name_of_second_organism] [nvarchar](255) NULL, --
	[Link_to_DiversityTaxonNames_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_second_organism] [int] NULL,
	[Vernacular_term_of_second_organism] [nvarchar](255) NULL, --
	[Identification_day_of_second_organism] [tinyint] NULL, -- 
	[Identification_month_of_second_organism] [tinyint] NULL, --
	[Identification_year_of_second_organism] [smallint] NULL, --
	[Identification_category_of_second_organism] [nvarchar](50) NULL, --
	[Identification_qualifier_of_second_organism] [nvarchar](50) NULL, --
	[Type_status_of_second_organism] [nvarchar](50) NULL, --
	[Type_notes_of_second_organism] [nvarchar](max) NULL, --
	[Notes_for_identification_of_second_organism] [nvarchar](max) NULL, --
	[Reference_title_of_second_organism] [nvarchar](255) NULL, --
	[Link_to_DiversityReferences_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_reference_of_second_organism] [int] NULL,
	[Responsible_of_second_organism] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_responsible_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_responsible_of_second_organism] [int] NULL,
--Storage	
	[Collection] [int] NULL, --
	[Material_category] [nvarchar](50) NULL, --
	[Storage_location] [nvarchar](255) NULL, --
	[Stock] [tinyint] NULL, --
	[Preparation_method] [nvarchar](max) NULL, --
	[Preparation_date] [datetime] NULL, --
	[Notes_for_part] [nvarchar](max) NULL, --
--Transaction
	[_TransactionID] [int] NULL, --
	[_Transaction] [nvarchar](200) NULL, --
	[On_loan] [int] NULL, --
--Hidden fields
	[_CollectionEventID] [int] NULL, --
	[_IdentificationUnitID] [int] NULL, --
	[_IdentificationSequence] [smallint] NULL, --
	[_SecondUnitID] [int] NULL, --
	[_SecondSequence] [smallint] NULL, --
	[_SpecimenPartID] [int] NULL, --
	[_CoordinatesAverageLatitudeCache] [real] NULL, --
	[_CoordinatesAverageLongitudeCache] [real] NULL, --
	[_CoordinatesLocationNotes] [nvarchar](max) NULL, --
	[_GeographicRegionPropertyURI] [varchar](255) NULL, --
	[_LithostratigraphyPropertyURI] [varchar](255) NULL, --
	[_ChronostratigraphyPropertyURI] [varchar](255) NULL, --
	[_NamedAverageLatitudeCache] [real] NULL, --
	[_NamedAverageLongitudeCache] [real] NULL, --
	[_LithostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_ChronostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_SecondUnitFamilyCache] [nvarchar](255) NULL, --
	[_SecondUnitOrderCache] [nvarchar](255) NULL, --
	[_AverageAltitudeCache] [real] NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
MW 18.11.2009 
TEST: 
Select * from dbo.FirstLines('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select * from dbo.FirstLines('3251, 3252')
*/ 
AS 
BEGIN 

declare @IDs table (ID int  Primary key)
declare @sID varchar(50)
while @CollectionSpecimenIDs <> ''
begin
	if (CHARINDEX(',', @CollectionSpecimenIDs) > 0)
	begin
	set @sID = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, 1, CHARINDEX(',', @CollectionSpecimenIDs) -1)))
	set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 2, 8000)))
	if (isnumeric(@sID) = 1)
		begin
		insert into @IDs 
		values( @sID )
		end
	end
	else
	begin
	set @sID = rtrim(ltrim(@CollectionSpecimenIDs))
	set @CollectionSpecimenIDs = ''
	if (isnumeric(@sID) = 1)
		begin
		insert into @IDs 
		values( @sID )
		end
	end
end

--- Specimen
insert into @List (CollectionSpecimenID
, Accession_number
, Data_withholding_reason
, _CollectionEventID
, Accession_day
, Accession_month
, Accession_year
, Accession_date_supplement
, Depositors_name
, Depositors_link_to_DiversityAgents
, Depositors_accession_number
, Exsiccata_abbreviation
, Link_to_DiversityExsiccatae
, Original_notes
, Additional_notes
, Internal_notes
, Label_title
, Label_type
, Label_transcription_state
, Label_transcription_notes
, Problems
)
select S.CollectionSpecimenID
, S.AccessionNumber
, S.DataWithholdingReason
, S.CollectionEventID 
, AccessionDay
, AccessionMonth
, AccessionYear
, AccessionDateSupplement
, DepositorsName
, DepositorsAgentURI
, DepositorsAccessionNumber
, ExsiccataAbbreviation
, ExsiccataURI
, OriginalNotes
, AdditionalNotes
, InternalNotes
, LabelTitle
, LabelType
, LabelTranscriptionState
, LabelTranscriptionNotes
, Problems
from dbo.CollectionSpecimen S
where S.CollectionSpecimenID in (select ID from @IDs)  



--- Event

update L
set L.Collection_day = E.CollectionDay
, L.Collection_month = E.CollectionMonth
, L.Collection_year = E.CollectionYear
, L.Collection_date_supplement = E.CollectionDateSupplement
, L.Collection_time = E.CollectionTime
, L.Collection_time_span = E.CollectionTimeSpan
, L.Country = E.CountryCache
, L.Locality_description = E.LocalityDescription
, L.Habitat_description = E.HabitatDescription
, L.Collecting_method = E.CollectingMethod
, L.Collection_event_notes = E.Notes
, L.Data_withholding_reason_for_collection_event = E.DataWithholdingReason
, L.Collectors_event_number = E.CollectorsEventNumber
from @List L,
CollectionEvent E
where L._CollectionEventID = E.CollectionEventID



--- Named Area

update L
set L.Named_area = E.Location1
, L.NamedAreaLocation2 = E.Location2
, L.Distance_to_location = E.DistanceToLocation
, L.Direction_to_location = E.DirectionToLocation
, L._NamedAverageLatitudeCache = E.AverageLatitudeCache
, L._NamedAverageLongitudeCache = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 7


--- Coordinates

update L
set L.Longitude = E.Location1
, L.Latitude = E.Location2
, L.Coordinates_accuracy = E.LocationAccuracy
, L._CoordinatesAverageLatitudeCache = E.AverageLatitudeCache
, L._CoordinatesAverageLongitudeCache = E.AverageLongitudeCache
, L._CoordinatesLocationNotes = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 8


--- Altitude

update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4



--- MTB

update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 3



--- SamplingPlots

update L
set L.Sampling_plot = E.Location1
, L.Link_to_SamplingPlots = E.Location2
, L.Accuracy_of_sampling_plot = E.LocationAccuracy
, L.Latitude_of_sampling_plot = E.AverageLatitudeCache
, L.Longitude_of_sampling_plot = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 13



--- GeographicRegions

update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 10


--- Lithostratigraphy

update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 30



--- Chronostratigraphy

update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 20



--- Collector

update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
--,dbo.CollectionAgent Amin
where L.CollectionSpecimenID = A.CollectionSpecimenID
--and A.CollectionSpecimenID = Amin.CollectionSpecimenID
and EXISTS (SELECT CollectionSpecimenID
	FROM dbo.CollectionAgent AS Amin
	GROUP BY CollectionSpecimenID
	HAVING (A.CollectionSpecimenID = Amin.CollectionSpecimenID) 
	AND (MIN(Amin.CollectorsSequence) = A.CollectorsSequence))

update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
where L.CollectionSpecimenID = A.CollectionSpecimenID
and L.Collectors_name is null
and A.CollectorsSequence is null
and EXISTS (SELECT CollectionSpecimenID
	FROM dbo.CollectionAgent AS Amin
	GROUP BY CollectionSpecimenID
	HAVING (A.CollectionSpecimenID = Amin.CollectionSpecimenID) 
	AND (MIN(Amin.LogCreatedWhen) = A.LogCreatedWhen))

--- IdentificationUnit
-- getting the unit IDs of the specimen
declare @AllUnitIDs table (UnitID int  Primary key, ID int, DisplayOrder smallint, RelatedUnitID int)
declare @UnitIDs table (UnitID int  Primary key, ID int, DisplayOrder smallint, RelatedUnitID int)

insert into @AllUnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
select U.IdentificationUnitID, U.CollectionSpecimenID, U.DisplayOrder, U.RelatedUnitID
from IdentificationUnit as U, @IDs as IDs
where DisplayOrder > 0
and IDs.ID = U.CollectionSpecimenID 

insert into @UnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
select U.UnitID, U.ID, U.DisplayOrder, U.RelatedUnitID
from @AllUnitIDs as U
where exists (select * from @AllUnitIDs aU group by aU.ID having min(aU.DisplayOrder) = U.DisplayOrder)

update L
set L.Taxonomic_group = I.TaxonomicGroup
, L._IdentificationUnitID = I.IdentificationUnitID
, L.Relation_type = I.RelationType
, L.Colonised_substrate_part = I.ColonisedSubstratePart
, L.Life_stage = I.LifeStage
, L.Gender = I.Gender
, L.Number_of_units = I.NumberOfUnits
, L.Circumstances = I.Circumstances
, L.Order_of_taxon = I.OrderCache
, L.Family_of_taxon = I.FamilyCache
, L.Identifier_of_organism = I.UnitIdentifier
, L.Description_of_organism = I.UnitDescription
, L.Only_observed = I.OnlyObserved
, L.Notes_for_organism = I.Notes
, L.Exsiccata_number = I.ExsiccataNumber
, L._SecondUnitID = U1.RelatedUnitID
from @List L,
@UnitIDs U1,
dbo.IdentificationUnit I
where L.CollectionSpecimenID = U1.ID
and L.CollectionSpecimenID = I.CollectionSpecimenID
and U1.ID = I.CollectionSpecimenID
and U1.UnitID = I.IdentificationUnitID



--- Identification

update L
set L._IdentificationSequence = I.IdentificationSequence
, L.Taxonomic_name = I.TaxonomicName
, L.Link_to_DiversityTaxonNames = I.NameURI
, L.Vernacular_term = I.VernacularTerm
, L.Identification_day = I.IdentificationDay
, L.Identification_month = I.IdentificationMonth
, L.Identification_year = I.IdentificationYear
, L.Identification_category = I.IdentificationCategory
, L.Identification_qualifier = I.IdentificationQualifier
, L.Type_status = I.TypeStatus
, L.Type_notes = I.TypeNotes
, L.Notes_for_identification = I.Notes
, L.Reference_title = I.ReferenceTitle
, L.Link_to_DiversityReferences = I.ReferenceURI
, L.Responsible = I.ResponsibleName
, L.Link_to_DiversityAgents_for_responsible = I.ResponsibleAgentURI
from @List L,
dbo.Identification I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.Identification AS Imax
	GROUP BY CollectionSpecimenID, IdentificationUnitID
	HAVING (Imax.CollectionSpecimenID = I.CollectionSpecimenID) AND (Imax.IdentificationUnitID = I.IdentificationUnitID) AND 
	(MAX(Imax.IdentificationSequence) = I.IdentificationSequence))



--- IdentificationUnitAnalysis

update L
set L.AnalysisID = I.AnalysisID
, L.Analysis_number = I.AnalysisNumber
, L.Analysis_result = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (MIN(Imin.AnalysisID) = I.AnalysisID) 
	AND (MIN(Imin.AnalysisNumber) = I.AnalysisNumber))

update L
set L.Analysis = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID = A.AnalysisID



--- SecondIdentificationUnit

update L

set L.Taxonomic_group_of_second_organism = I.TaxonomicGroup

--, L._IdentificationUnitID = I.IdentificationUnitID
, L.Life_stage_of_second_organism = I.LifeStage
, L.Gender_of_second_organism = I.Gender
, L.Number_of_units_of_second_organism = I.NumberOfUnits
, L.Circumstances_of_second_organism = I.Circumstances
, L.Identifier_of_second_organism = I.UnitIdentifier
, L.Description_of_second_organism = I.UnitDescription
, L.Only_observed_of_second_organism = I.OnlyObserved
, L.Notes_for_second_organism = I.Notes

, L._SecondUnitFamilyCache = I.FamilyCache
, L._SecondUnitOrderCache = I.OrderCache
from @List L,
dbo.IdentificationUnit I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._SecondUnitID = I.IdentificationUnitID


--- SecondIdentification

update L
set L._SecondSequence = I.IdentificationSequence
, L.Taxonomic_name_of_second_organism = I.TaxonomicName
, L.Link_to_DiversityTaxonNames_of_second_organism = I.NameURI
, L.Vernacular_term_of_second_organism = I.VernacularTerm

, L.Identification_day_of_second_organism = I.IdentificationDay
, L.Identification_month_of_second_organism = I.IdentificationMonth
, L.Identification_year_of_second_organism = I.IdentificationYear
, L.Identification_category_of_second_organism = I.IdentificationCategory
, L.Identification_qualifier_of_second_organism = I.IdentificationQualifier
, L.Type_status_of_second_organism = I.TypeStatus
, L.Type_notes_of_second_organism = I.TypeNotes
, L.Notes_for_identification_of_second_organism = I.Notes
, L.Reference_title_of_second_organism = I.ReferenceTitle
, L.Link_to_DiversityReferences_of_second_organism = I.ReferenceURI
, L.Responsible_of_second_organism = I.ResponsibleName
, L.Link_to_DiversityAgents_for_responsible_of_second_organism = I.ResponsibleAgentURI

from @List L,
dbo.Identification I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._SecondUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.Identification AS Imax
	GROUP BY CollectionSpecimenID, IdentificationUnitID
	HAVING (Imax.CollectionSpecimenID = I.CollectionSpecimenID) AND (Imax.IdentificationUnitID = I.IdentificationUnitID) AND 
	(MAX(Imax.IdentificationSequence) = I.IdentificationSequence))
	
	
--- CollectionSpecimenPart	

update L
set L._SpecimenPartID = P.SpecimenPartID
, L.Collection = P.CollectionID
, L.Material_category = P.MaterialCategory
, L.Storage_location = P.StorageLocation
, L.Stock = P.Stock
, L.Preparation_method = P.PreparationMethod
, L.Preparation_date = P.PreparationDate
, L.Notes_for_part = P.Notes
from @List L,
dbo.CollectionSpecimenPart P
where L.CollectionSpecimenID = P.CollectionSpecimenID
and EXISTS
	(SELECT Pmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenPart AS Pmin
	GROUP BY Pmin.CollectionSpecimenID
	HAVING (Pmin.CollectionSpecimenID = P.CollectionSpecimenID) AND (MIN(Pmin.SpecimenPartID) = P.SpecimenPartID))



--- Transaction

update L
set L._TransactionID = P.TransactionID
, L.On_loan = P.IsOnLoan
from @List L,
dbo.CollectionSpecimenTransaction P
where L.CollectionSpecimenID = P.CollectionSpecimenID
and L._SpecimenPartID = P.SpecimenPartID
and EXISTS
	(SELECT Tmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenTransaction AS Tmin
	GROUP BY Tmin.CollectionSpecimenID, Tmin.SpecimenPartID
	HAVING (Tmin.CollectionSpecimenID = P.CollectionSpecimenID) 
	AND Tmin.SpecimenPartID = P.SpecimenPartID
	AND (MIN(Tmin.TransactionID) = P.TransactionID))
	
	
update L
set L._Transaction = T.TransactionTitle
from @List L,
dbo.[Transaction] T
where L._TransactionID = T.TransactionID
	
                  
RETURN 
END   



GO


--#####################################################################################################################
--######   AnalysisProjectList   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AnalysisChildNodes] (@ID int)  
RETURNS @ItemList TABLE (AnalysisID int primary key,
	AnalysisParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	MeasurementUnit nvarchar (50)   NULL ,
	Notes nvarchar  (1000)   NULL ,
	AnalysisURI varchar  (255)   NULL ,
	OnlyHierarchy [bit] NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  
AS
BEGIN
   RETURN
END

GO


ALTER FUNCTION [dbo].[AnalysisChildNodes] (@ID int)  
RETURNS @ItemList TABLE (AnalysisID int primary key,
	AnalysisParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	MeasurementUnit nvarchar (50)   NULL ,
	Notes nvarchar  (1000)   NULL ,
	AnalysisURI varchar  (255)   NULL ,
	OnlyHierarchy [bit] NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (AnalysisID int primary key,
	AnalysisParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	MeasurementUnit nvarchar (50)   NULL ,
	Notes nvarchar  (1000)   NULL ,
	AnalysisURI varchar  (255)   NULL,
	OnlyHierarchy [bit] NULL ,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)

 INSERT @TempItem (AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, Notes , AnalysisURI, OnlyHierarchy, RowGUID) 
	SELECT AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, Notes , AnalysisURI, OnlyHierarchy, RowGUID
	FROM Analysis WHERE AnalysisParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select AnalysisID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, Notes , AnalysisURI, OnlyHierarchy, RowGUID
	from dbo.AnalysisChildNodes (@ParentID) where AnalysisID not in (select AnalysisID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, Notes , AnalysisURI, OnlyHierarchy, RowGUID) 
   SELECT distinct AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, Notes , AnalysisURI, OnlyHierarchy, RowGUID
   FROM @TempItem ORDER BY DisplayText
   RETURN
END

GO

--#####################################################################################################################
--######   AnalysisProjectList   ######################################################################################
--#####################################################################################################################




ALTER FUNCTION [dbo].[AnalysisProjectList] (@ProjectID int)   
RETURNS @AnalysisList TABLE ([AnalysisID] [int] Primary key , 	
[AnalysisParentID] [int] NULL , 	
[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL , 	
[MeasurementUnit] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL , 	
[AnalysisURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
[OnlyHierarchy] [bit] NULL,
[RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)  
AS BEGIN  
RETURN 
END  
GO

ALTER FUNCTION [dbo].[AnalysisProjectList] (@ProjectID int)   
RETURNS @AnalysisList TABLE ([AnalysisID] [int] Primary key , 	
[AnalysisParentID] [int] NULL , 	
[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL , 	
[MeasurementUnit] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL , 	
[AnalysisURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
[OnlyHierarchy] [bit] NULL,
[RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)  
/* 
Returns a table that lists all the analysis items related to the given project. 
MW 08.08.2009 
TEST: 
Select * from AnalysisProjectList(3)  
Select * from AnalysisProjectList(372)  
*/ 
AS BEGIN  
--ALTER TABLE @AnalysisList ADD  CONSTRAINT [DF__Analysis__RowGUI__29A2D696]  DEFAULT (newsequentialid()) FOR [RowGUID]

INSERT INTO @AnalysisList            
([AnalysisID]            
,[AnalysisParentID]            
,[DisplayText]            
,[Description]            
,[MeasurementUnit]            
,[Notes]            
,[AnalysisURI]
,[OnlyHierarchy]
,[RowGUID]) 
SELECT Analysis.AnalysisID, Analysis.AnalysisParentID, Analysis.DisplayText, Analysis.Description, 
Analysis.MeasurementUnit, Analysis.Notes,  Analysis.AnalysisURI, Analysis.OnlyHierarchy, Analysis.RowGUID
FROM  ProjectAnalysis 
INNER JOIN Analysis ON ProjectAnalysis.AnalysisID = Analysis.AnalysisID 
WHERE ProjectAnalysis.ProjectID = @ProjectID  

DECLARE @TempItem TABLE (AnalysisID int primary key) 

INSERT INTO @TempItem ([AnalysisID]) 
SELECT Analysis.AnalysisID 
FROM  ProjectAnalysis 
INNER JOIN Analysis ON ProjectAnalysis.AnalysisID = Analysis.AnalysisID 
WHERE ProjectAnalysis.ProjectID = @ProjectID  
declare @ParentID int  
DECLARE HierarchyCursor  CURSOR for 	select AnalysisID from @TempItem 	
open HierarchyCursor 	
FETCH next from HierarchyCursor into @ParentID 	
WHILE @@FETCH_STATUS = 0 	
BEGIN 	
insert into @AnalysisList ( AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, 
Notes , AnalysisURI, OnlyHierarchy, RowGUID) 
select AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, 
Notes , AnalysisURI, OnlyHierarchy, RowGUID
from dbo.AnalysisChildNodes (@ParentID) 
where AnalysisID not in (select AnalysisID from @AnalysisList) 	
FETCH NEXT FROM HierarchyCursor into @ParentID 	END 
CLOSE HierarchyCursor 
DEALLOCATE HierarchyCursor  
--DELETE FROM  @AnalysisList WHERE OnlyHierarchy = 1  
RETURN 
END  

GO



--#####################################################################################################################
--######   AnalysisResultForProject   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[AnalysisResultForProject] (@ProjectID int)  
RETURNS @AnalysisResult TABLE (
	[AnalysisID] [int] NOT NULL,
	[AnalysisResult] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[Notes] [nvarchar](500) NULL,
	[RowGUID] [int] Identity NOT NULL)

/*
Returns the contents of the table AnalysisResult used in a project.
MW 15.07.2009
Test:
select * from [dbo].[AnalysisResultForProject] (1100)
*/

AS
BEGIN

INSERT INTO @AnalysisResult (AnalysisID, AnalysisResult, [Description], DisplayText, DisplayOrder, Notes)
SELECT     AnalysisResult.AnalysisID, AnalysisResult.AnalysisResult, AnalysisResult.Description, AnalysisResult.DisplayText, AnalysisResult.DisplayOrder, 
AnalysisResult.Notes
FROM         AnalysisResult INNER JOIN
AnalysisProjectList(@ProjectID) P ON AnalysisResult.AnalysisID = P.AnalysisID

RETURN
END

GO

--#####################################################################################################################
--######   AnalysisTaxonomicGroupForProject   ######################################################################################
--#####################################################################################################################


	ALTER FUNCTION [dbo].[AnalysisTaxonomicGroupForProject] (@ProjectID int)  
	RETURNS @AnalysisTaxonomicGroup TABLE ([AnalysisID] [int] NOT NULL,
		[TaxonomicGroup] [nvarchar](50) NOT NULL,
		[RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)
	/*
	Returns the contents of the table AnalysisTaxonomicGroup used in a project including the whole hierarchy.
	MW 15.07.2009
	Test:
	select * from [dbo].[AnalysisTaxonomicGroupForProject] (1100)
	*/
	AS
	BEGIN
		declare @Temp TABLE (AnalysisID int NOT NULL, TaxonomicGroup [nvarchar](50) NOT NULL, ID int Identity NOT NULL)
		insert @Temp (AnalysisID, TaxonomicGroup)
		SELECT A.AnalysisID, A.TaxonomicGroup
		FROM AnalysisTaxonomicGroup A 
		INNER JOIN ProjectAnalysis P ON A.AnalysisID = P.AnalysisID 
		WHERE     (P.ProjectID = @ProjectID)

		declare @TempID TABLE (ID int NOT NULL)
		INSERT @TempID (ID) SELECT ID FROM @Temp
		DECLARE @ID INT
		DECLARE @AnalysisID int
		DECLARE @TaxonomicGroup nvarchar(50)
		WHILE (SELECT COUNT(*) FROM @TempID) > 0
		BEGIN
			SET @ID = (SELECT MIN(ID) FROM @TempID)
			SET @AnalysisID = (SELECT AnalysisID FROM @Temp WHERE ID = @ID)
			SET @TaxonomicGroup = (SELECT TaxonomicGroup FROM @Temp WHERE ID = @ID)
			INSERT INTO @Temp (AnalysisID, TaxonomicGroup)
			SELECT AnalysisID, @TaxonomicGroup FROM [DiversityCollection].[dbo].[AnalysisList] (@ProjectID, @TaxonomicGroup)
			DELETE FROM @TempID WHERE ID = @ID
		END

		insert @AnalysisTaxonomicGroup (AnalysisID, TaxonomicGroup)
		SELECT DISTINCT AnalysisID, TaxonomicGroup
		FROM @Temp
		
		update AT
		set AT.RowGUID = A.RowGUID
		from @AnalysisTaxonomicGroup AT, dbo.AnalysisTaxonomicGroup A
		where AT.AnalysisID = A.AnalysisID
		and AT.TaxonomicGroup = A.TaxonomicGroup

		RETURN
	END
	
	GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[AnalysisHierarchy] (@AnalysisID int)  
RETURNS @AnalysisList TABLE ([AnalysisID] [int] Primary key ,
	[AnalysisParentID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[MeasurementUnit] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[AnalysisURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[OnlyHierarchy] [bit] NULL)  


/*
Returns a table that lists all the analysis items related to the given analysis.
MW 02.01.2006
Test
SELECT  *  FROM dbo.AnalysisHierarchy(82)

*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int

set @TopID = (select AnalysisParentID from Analysis where AnalysisID = @AnalysisID) 

set @i = (select count(*) from Analysis where AnalysisID = @AnalysisID)

if (@TopID is null )
	set @TopID =  @AnalysisID
else	
	begin
	while (@i > 0)
		begin
		set @AnalysisID = (select AnalysisParentID from Analysis where AnalysisID = @AnalysisID and not AnalysisParentID is null) 
		set @i = (select count(*) from Analysis where AnalysisID = @AnalysisID and not AnalysisParentID is null)
		end
	set @TopID = @AnalysisID
	end

-- copy the root node in the result list
   INSERT @AnalysisList
   SELECT DISTINCT AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy
   FROM Analysis
   WHERE Analysis.AnalysisID = @TopID

-- copy the child nodes into the result list
   INSERT @AnalysisList
   SELECT AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy 
   FROM dbo.AnalysisChildNodes (@TopID)
   
-- delete the heaeders
   /*DELETE A FROM @AnalysisList A
   WHERE A.OnlyHierarchy = 1*/

   RETURN
END

GO



--#####################################################################################################################
--######   CollectionEventLocalisation: real -> float   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionEventLocalisation_log ALTER COLUMN AverageAltitudeCache float
GO

ALTER TABLE CollectionEventLocalisation_log ALTER COLUMN AverageLatitudeCache float
GO

ALTER TABLE CollectionEventLocalisation_log ALTER COLUMN AverageLongitudeCache float
GO

ALTER TABLE CollectionEventLocalisation ALTER COLUMN AverageAltitudeCache float
GO

ALTER TABLE CollectionEventLocalisation ALTER COLUMN AverageLatitudeCache float
GO

ALTER TABLE CollectionEventLocalisation ALTER COLUMN AverageLongitudeCache float
GO

--#####################################################################################################################
--######   UserProxy: Queries   ######################################################################################
--#####################################################################################################################


ALTER TABLE UserProxy ADD Queries [xml] NULL
GO

--#####################################################################################################################
--######   Processing   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[ProcessingChildNodes] (@ID int)  
RETURNS @ItemList TABLE (ProcessingID int primary key,
	ProcessingParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	Notes nvarchar  (1000)   NULL ,
	ProcessingURI varchar  (255)   NULL ,
	OnlyHierarchy [bit] NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  
	AS
BEGIN
RETURN
END
GO



ALTER FUNCTION [dbo].[ProcessingChildNodes] (@ID int)  
RETURNS @ItemList TABLE (ProcessingID int primary key,
	ProcessingParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	Notes nvarchar  (1000)   NULL ,
	ProcessingURI varchar  (255)   NULL ,
	OnlyHierarchy [bit] NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  
	
	
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 21.10.2009
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (ProcessingID int primary key,
	ProcessingParentID int NULL ,
	DisplayText nvarchar (50)   NULL ,
	Description nvarchar  (500)   NULL ,
	Notes nvarchar  (1000)   NULL ,
	ProcessingURI varchar  (255)   NULL ,
	OnlyHierarchy [bit] NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  

 INSERT @TempItem (ProcessingID , ProcessingParentID, DisplayText , Description , Notes , ProcessingURI, OnlyHierarchy, RowGUID) 
	SELECT ProcessingID , ProcessingParentID, DisplayText , Description , Notes , ProcessingURI, OnlyHierarchy, RowGUID
	FROM Processing WHERE ProcessingParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select ProcessingID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select ProcessingID , ProcessingParentID, DisplayText , Description , Notes , ProcessingURI, OnlyHierarchy, RowGUID
	from dbo.ProcessingChildNodes (@ParentID) where ProcessingID not in (select ProcessingID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (ProcessingID , ProcessingParentID, DisplayText , Description , Notes , ProcessingURI, OnlyHierarchy, RowGUID) 
   SELECT distinct ProcessingID , ProcessingParentID, DisplayText , Description , Notes , ProcessingURI, OnlyHierarchy, RowGUID
   FROM @TempItem ORDER BY DisplayText
   RETURN
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



/*

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.09'
END

GO
*/

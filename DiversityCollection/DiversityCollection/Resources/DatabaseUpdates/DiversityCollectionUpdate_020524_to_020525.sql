declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.24'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   CollectionHierarchy   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[CollectionHierarchyAll] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the collections including a display text with the whole hierarchy.
MW 02.01.2006
TEST:
SELECT * FROM DBO.CollectionHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, DisplayText)
SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
FROM Collection
WHERE Collection.CollectionParentID IS NULL

declare @i int
set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @CollectionList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, DisplayText)
	SELECT DISTINCT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.CollectionOwner
	, C.DisplayOrder, L.DisplayText + ' | ' + C.CollectionName
	FROM Collection C, @CollectionList L
	WHERE C.CollectionParentID = L.CollectionID
	AND C.CollectionID NOT IN (select CollectionID from  @CollectionList)

	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @CollectionList))
end

   RETURN
END
go




--#####################################################################################################################
--######   [FirstLinesUnit_3]   ######################################################################################
--#####################################################################################################################





ALTER FUNCTION [dbo].[FirstLinesUnit_3] 
(@CollectionSpecimenIDs varchar(4000), @AnalysisIDs varchar(4000), @AnalysisStartDate date, @AnalysisEndDate date)   
RETURNS @List TABLE (
	[IdentificationUnitID] [int] Primary key,
	[CollectionSpecimenID] [int], --
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
	[Locality_description] [nvarchar](255) NULL, --
	[Habitat_description] [nvarchar](255) NULL, -- 
	[Collecting_method] [nvarchar](255) NULL, --
	[Collection_event_notes] [nvarchar](255) NULL, --
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
	[Notes_for_Altitude] [nvarchar](255) NULL, --
	[MTB] [nvarchar](255) NULL, --
	[Quadrant] [nvarchar](255) NULL, --
	[Notes_for_MTB] [nvarchar](255) NULL, --
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
--Organism
	[Taxonomic_group] [nvarchar](50) NULL, --
	[Relation_type] [nvarchar](50) NULL, --
	[Colonised_substrate_part] [nvarchar](255) NULL, --
	[Related_organism] [nvarchar] (200) NULL,
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
--Identification
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
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, --
	[Remove_link_for_determiner] [int] NULL,
--Analysis
	[Analysis_0] [nvarchar](50) NULL, --
	[AnalysisID_0] [int] NULL, --
	[Analysis_number_0] [nvarchar](50) NULL, --
	[Analysis_result_0] [nvarchar](max) NULL, --
	
	[Analysis_1] [nvarchar](50) NULL, --
	[AnalysisID_1] [int] NULL, --
	[Analysis_number_1] [nvarchar](50) NULL, --
	[Analysis_result_1] [nvarchar](max) NULL, --
	
	[Analysis_2] [nvarchar](50) NULL, --
	[AnalysisID_2] [int] NULL, --
	[Analysis_number_2] [nvarchar](50) NULL, --
	[Analysis_result_2] [nvarchar](max) NULL, --
	
	[Analysis_3] [nvarchar](50) NULL, --
	[AnalysisID_3] [int] NULL, --
	[Analysis_number_3] [nvarchar](50) NULL, --
	[Analysis_result_3] [nvarchar](max) NULL, --
	
	[Analysis_4] [nvarchar](50) NULL, --
	[AnalysisID_4] [int] NULL, --
	[Analysis_number_4] [nvarchar](50) NULL, --
	[Analysis_result_4] [nvarchar](max) NULL, --
	
	[Analysis_5] [nvarchar](50) NULL, --
	[AnalysisID_5] [int] NULL, --
	[Analysis_number_5] [nvarchar](50) NULL, --
	[Analysis_result_5] [nvarchar](max) NULL, --
	
	[Analysis_6] [nvarchar](50) NULL, --
	[AnalysisID_6] [int] NULL, --
	[Analysis_number_6] [nvarchar](50) NULL, --
	[Analysis_result_6] [nvarchar](max) NULL, --
	
	[Analysis_7] [nvarchar](50) NULL, --
	[AnalysisID_7] [int] NULL, --
	[Analysis_number_7] [nvarchar](50) NULL, --
	[Analysis_result_7] [nvarchar](max) NULL, --
	
	[Analysis_8] [nvarchar](50) NULL, --
	[AnalysisID_8] [int] NULL, --
	[Analysis_number_8] [nvarchar](50) NULL, --
	[Analysis_result_8] [nvarchar](max) NULL, --
	
	[Analysis_9] [nvarchar](50) NULL, --
	[AnalysisID_9] [int] NULL, --
	[Analysis_number_9] [nvarchar](50) NULL, --
	[Analysis_result_9] [nvarchar](max) NULL, --
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
	[_SpecimenPartID] [int] NULL, --
	[_CoordinatesAverageLatitudeCache] [real] NULL, --
	[_CoordinatesAverageLongitudeCache] [real] NULL, --
	[_CoordinatesLocationNotes] [nvarchar](255) NULL, --
	[_GeographicRegionPropertyURI] [varchar](255) NULL, --
	[_LithostratigraphyPropertyURI] [varchar](255) NULL, --
	[_ChronostratigraphyPropertyURI] [varchar](255) NULL, --
	[_NamedAverageLatitudeCache] [real] NULL, --
	[_NamedAverageLongitudeCache] [real] NULL, --
	[_LithostratigraphyPropertyHierarchyCache] [nvarchar](255) NULL, --
	[_ChronostratigraphyPropertyHierarchyCache] [nvarchar](255) NULL, --
	[_AverageAltitudeCache] [real] NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
MW 18.11.2009 
TEST: 
Select * from dbo.FirstLinesUnit('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select * from dbo.FirstLinesUnit('3251, 3252', '34', null, null) order by CollectionSpecimenID, IdentificationUnitID
Select * from dbo.FirstLinesUnit('193610, 193611') order by CollectionSpecimenID, IdentificationUnitID
Select * from dbo.FirstLinesUnit_2('193610, 193611', '55, 64', null, null) order by CollectionSpecimenID, IdentificationUnitID
Select * from dbo.FirstLinesUnit_2('193610, 193611', '64', null, null) order by CollectionSpecimenID, IdentificationUnitID
Select * from dbo.FirstLinesUnit_2('193610, 193611', null, '2000/2/1', '2010/12/31') order by CollectionSpecimenID, IdentificationUnitID
Select * from dbo.FirstLinesUnit_2('3251, 3252', null, null, null) order by CollectionSpecimenID, IdentificationUnitID
Select * from dbo.FirstLinesUnit_2('3251, 3252', null, '2000/2/1', '2010/12/31') order by CollectionSpecimenID, IdentificationUnitID
Select * from dbo.FirstLinesUnit_3('3251, 3252', null, '2000/2/1', '2010/12/31') order by CollectionSpecimenID, IdentificationUnitID

SELECT *
FROM dbo.FirstLinesUnit_3  ('193972', '18,75', '2011/8/17', null) 
ORDER BY Accession_number, CollectionSpecimenID, IdentificationUnitID 

--Meldung 241, Ebene 16, Status 1, Zeile 1
--Fehler beim Konvertieren einer Zeichenfolge in ein Datum und/oder eine Uhrzeit.

SELECT *
FROM dbo.FirstLinesUnit_3  ('193972', '43,81,49,18,75,51', '2011/8/17', null) 
ORDER BY Accession_number, CollectionSpecimenID, IdentificationUnitID 

--Meldung 2627, Ebene 14, Status 1, Zeile 1
--Verletzung der PRIMARY KEY-Einschränkung 'PK__#60158C8__5B789DE861FDD500'. Ein doppelter Schlüssel kann in das 'dbo.@AnalysisID'-Objekt nicht eingefügt werden.
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
	set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 2, 4000)))
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
insert into @List (
IdentificationUnitID
, CollectionSpecimenID
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
, Taxonomic_group
, Relation_type
, Colonised_substrate_part
, Life_stage
, Gender
, Number_of_units
, Circumstances
, Order_of_taxon
, Family_of_taxon
, Identifier_of_organism
, Description_of_organism
, Only_observed
, Notes_for_organism
, Exsiccata_number
)
select 
U.IdentificationUnitID
, S.CollectionSpecimenID
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
, U.TaxonomicGroup
, U.RelationType
, U.ColonisedSubstratePart
, U.LifeStage
, U.Gender
, U.NumberOfUnits
, U.Circumstances
, U.OrderCache
, U.FamilyCache
, U.UnitIdentifier
, U.UnitDescription
, U.OnlyObserved
, U.Notes
, U.ExsiccataNumber
from dbo.CollectionSpecimen S, dbo.IdentificationUnit U, dbo.CollectionSpecimenID_UserAvailable A
where S.CollectionSpecimenID in (select ID from @IDs) 
and U.CollectionSpecimenID = S.CollectionSpecimenID 
and S.CollectionSpecimenID = A.CollectionSpecimenID



--- Event

update L
set L.Collection_day = E.CollectionDay
, L.Collection_month = E.CollectionMonth
, L.Collection_year = E.CollectionYear
, L.Collection_date_supplement = E.CollectionDateSupplement
, L.Collection_time = E.CollectionTime
, L.Collection_time_span = E.CollectionTimeSpan
, L.Country = E.CountryCache
, L.Locality_description = cast(E.LocalityDescription as nvarchar(255))
, L.Habitat_description = cast(E.HabitatDescription as nvarchar(255))
, L.Collecting_method = cast(E.CollectingMethod as nvarchar(255))
, L.Collection_event_notes = cast(E.Notes as nvarchar(255))
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
, L._CoordinatesLocationNotes = cast(E.LocationNotes as nvarchar (255))
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
, L.Notes_for_MTB = cast(E.LocationNotes as nvarchar(255))
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
, L._LithostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 30



--- Chronostratigraphy

update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
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
, L.Determiner = I.ResponsibleName
, L.Link_to_DiversityAgents_for_determiner = I.ResponsibleAgentURI
from @List L,
dbo.Identification I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.Identification AS Imax
	GROUP BY CollectionSpecimenID, IdentificationUnitID
	HAVING (Imax.CollectionSpecimenID = I.CollectionSpecimenID) AND (Imax.IdentificationUnitID = I.IdentificationUnitID) AND 
	(MAX(Imax.IdentificationSequence) = I.IdentificationSequence))



-- ANALYSIS --- ###############################################################################################################

-- getting the AnalysisID's that should be shown

declare @AnalysisID_0 int
declare @AnalysisID_1 int
declare @AnalysisID_2 int
declare @AnalysisID_3 int
declare @AnalysisID_4 int
declare @AnalysisID_5 int
declare @AnalysisID_6 int
declare @AnalysisID_7 int
declare @AnalysisID_8 int
declare @AnalysisID_9 int



if (not @AnalysisIDs is null and @AnalysisIDs <> '')
begin
	set @AnalysisIDs = REPLACE(@AnalysisIDs, ' ', '')
	declare @AnalysisID table (ID int Identity(0,1), AnalysisID int Primary key)
	declare @sAnalysisID varchar(50)
	declare @iAnalysis int
	set @iAnalysis = 0
	while @AnalysisIDs <> '' and @iAnalysis < 10
	begin
		if (CHARINDEX(',', @AnalysisIDs) > 0)
		begin
		set @sAnalysisID = rtrim(ltrim(SUBSTRING(@AnalysisIDs, 1, CHARINDEX(',', @AnalysisIDs) -1)))
		set @AnalysisIDs = rtrim(ltrim(SUBSTRING(@AnalysisIDs, CHARINDEX(',', @AnalysisIDs) + 1, 4000)))
		if (isnumeric(@sID) = 1 and (select count(*) from @AnalysisID where AnalysisID = @sAnalysisID) = 0)
			begin
			insert into @AnalysisID (AnalysisID)
			values( @sAnalysisID )
			end
		end
		else
		begin
		set @sAnalysisID = rtrim(ltrim(@AnalysisIDs))
		set @AnalysisIDs = ''
		if (isnumeric(@sAnalysisID) = 1 and (select count(*) from @AnalysisID where AnalysisID = @sAnalysisID) = 0)
			begin
			insert into @AnalysisID (AnalysisID)
			values( @sAnalysisID )
			end
		end
		set @iAnalysis = (select count(*) from @AnalysisID)
	end
		
	set @AnalysisID_0 = (select AnalysisID from @AnalysisID where ID = 0)
	set @AnalysisID_1 = (select AnalysisID from @AnalysisID where ID = 1)
	set @AnalysisID_2 = (select AnalysisID from @AnalysisID where ID = 2)
	set @AnalysisID_3 = (select AnalysisID from @AnalysisID where ID = 3)
	set @AnalysisID_4 = (select AnalysisID from @AnalysisID where ID = 4)
	set @AnalysisID_5 = (select AnalysisID from @AnalysisID where ID = 5)
	set @AnalysisID_6 = (select AnalysisID from @AnalysisID where ID = 6)
	set @AnalysisID_7 = (select AnalysisID from @AnalysisID where ID = 7)
	set @AnalysisID_8 = (select AnalysisID from @AnalysisID where ID = 8)
	set @AnalysisID_9 = (select AnalysisID from @AnalysisID where ID = 9)
end



--############### ANALYSIS 0 ###############

update L
set L.AnalysisID_0 = I.AnalysisID
, L.Analysis_number_0 = I.AnalysisNumber
, L.Analysis_result_0 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_0
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_0)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_0 = null
	, L.Analysis_number_0 = null
	, L.Analysis_result_0 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_0
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_0 = null
	, L.Analysis_number_0 = null
	, L.Analysis_result_0 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_0
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end

update L
set L.Analysis_0 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_0 = A.AnalysisID



--############### ANALYSIS 1 ###############

update L
set L.AnalysisID_1 = I.AnalysisID
, L.Analysis_number_1 = I.AnalysisNumber
, L.Analysis_result_1 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_1
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_1)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_1 = null
	, L.Analysis_number_1 = null
	, L.Analysis_result_1 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_1
	and I.AnalysisID = @AnalysisID_1
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_1 = null
	, L.Analysis_number_1 = null
	, L.Analysis_result_1 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_1
	and I.AnalysisID = @AnalysisID_1
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_1 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_1 = A.AnalysisID


--############### ANALYSIS 2 ###############

update L
set L.AnalysisID_2 = I.AnalysisID
, L.Analysis_number_2 = I.AnalysisNumber
, L.Analysis_result_2 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_2
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_2
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_2)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_2 = null
	, L.Analysis_number_2 = null
	, L.Analysis_result_2 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_2
	and I.AnalysisID = @AnalysisID_2
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_2 = null
	, L.Analysis_number_2 = null
	, L.Analysis_result_2 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_2
	and I.AnalysisID = @AnalysisID_2
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_2 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_2 = A.AnalysisID



--############### ANALYSIS 3 ###############

update L
set L.AnalysisID_3 = I.AnalysisID
, L.Analysis_number_3 = I.AnalysisNumber
, L.Analysis_result_3 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_3
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_3
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_3)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_3 = null
	, L.Analysis_number_3 = null
	, L.Analysis_result_3 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_3
	and I.AnalysisID = @AnalysisID_3
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_3 = null
	, L.Analysis_number_3 = null
	, L.Analysis_result_3 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_3
	and I.AnalysisID = @AnalysisID_3
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_3 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_3 = A.AnalysisID


--############### ANALYSIS 4 ###############

update L
set L.AnalysisID_4 = I.AnalysisID
, L.Analysis_number_4 = I.AnalysisNumber
, L.Analysis_result_4 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_4
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_4
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_4)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_4 = null
	, L.Analysis_number_4 = null
	, L.Analysis_result_4 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_4
	and I.AnalysisID = @AnalysisID_4
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_4 = null
	, L.Analysis_number_4 = null
	, L.Analysis_result_4 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_4
	and I.AnalysisID = @AnalysisID_4
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_4 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_4 = A.AnalysisID



--############### ANALYSIS 5 ###############

update L
set L.AnalysisID_5 = I.AnalysisID
, L.Analysis_number_5 = I.AnalysisNumber
, L.Analysis_result_5 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_5
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_5
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_5)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_5 = null
	, L.Analysis_number_5 = null
	, L.Analysis_result_5 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_5
	and I.AnalysisID = @AnalysisID_5
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_5 = null
	, L.Analysis_number_5 = null
	, L.Analysis_result_5 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_5
	and I.AnalysisID = @AnalysisID_5
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_5 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_5 = A.AnalysisID




--############### ANALYSIS 6 ###############

update L
set L.AnalysisID_6 = I.AnalysisID
, L.Analysis_number_6 = I.AnalysisNumber
, L.Analysis_result_6 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_6
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_6
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_6)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_6 = null
	, L.Analysis_number_6 = null
	, L.Analysis_result_6 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_6
	and I.AnalysisID = @AnalysisID_6
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_6 = null
	, L.Analysis_number_6 = null
	, L.Analysis_result_6 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_6
	and I.AnalysisID = @AnalysisID_6
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_6 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_6 = A.AnalysisID



--############### ANALYSIS 7 ###############

update L
set L.AnalysisID_7 = I.AnalysisID
, L.Analysis_number_7 = I.AnalysisNumber
, L.Analysis_result_7 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_7
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_7
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_7)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_7 = null
	, L.Analysis_number_7 = null
	, L.Analysis_result_7 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_7
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_7 = null
	, L.Analysis_number_7 = null
	, L.Analysis_result_7 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_7
	and I.AnalysisID = @AnalysisID_7
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_7 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_7 = A.AnalysisID


--############### ANALYSIS 8 ###############

update L
set L.AnalysisID_8 = I.AnalysisID
, L.Analysis_number_8 = I.AnalysisNumber
, L.Analysis_result_8 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_8
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_8
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_8)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_8 = null
	, L.Analysis_number_8 = null
	, L.Analysis_result_8 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_8
	and I.AnalysisID = @AnalysisID_8
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_8 = null
	, L.Analysis_number_8 = null
	, L.Analysis_result_8 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_8
	and I.AnalysisID = @AnalysisID_8
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_8 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_8 = A.AnalysisID



--############### ANALYSIS 9 ###############

update L
set L.AnalysisID_9 = I.AnalysisID
, L.Analysis_number_9 = I.AnalysisNumber
, L.Analysis_result_9 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L.IdentificationUnitID = I.IdentificationUnitID
and I.AnalysisID = @AnalysisID_9
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_9
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.AnalysisID = @AnalysisID_9)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_9 = null
	, L.Analysis_number_9 = null
	, L.Analysis_result_9 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_9
	and I.AnalysisID = @AnalysisID_9
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_9 = null
	, L.Analysis_number_9 = null
	, L.Analysis_result_9 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L.IdentificationUnitID = I.IdentificationUnitID
	and I.AnalysisID = L.AnalysisID_9
	and I.AnalysisID = @AnalysisID_9
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_9 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_9 = A.AnalysisID




--- Related Organism
Update L
set L.Related_Organism = Urel.LastIdentificationCache
from  @List L
, dbo.IdentificationUnit U
, dbo.IdentificationUnit Urel
where U.IdentificationUnitID = L.IdentificationUnitID
and U.RelatedUnitID = Urel.IdentificationUnitID	
and U.CollectionSpecimenID = Urel.CollectionSpecimenID
	
	
	
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
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.25'
END

GO

select [dbo].[Version] ()  


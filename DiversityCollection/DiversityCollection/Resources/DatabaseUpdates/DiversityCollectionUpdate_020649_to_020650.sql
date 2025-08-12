declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.49'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  ISSUE #14  ##################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  CollectionSpecimen: Change Problems to nvarchar(MAX)  #######################################################
--#####################################################################################################################

ALTER TABLE [dbo].[CollectionSpecimen] ALTER COLUMN [Problems] nvarchar(MAX)
GO 
ALTER TABLE [dbo].[CollectionSpecimen_log] ALTER COLUMN [Problems] nvarchar(MAX)
GO 


--#####################################################################################################################
--######   FirstLines_4: Change Problems to nvarchar(MAX)  ############################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[FirstLines_4] 
(@CollectionSpecimenIDs varchar(8000))   
RETURNS @List TABLE (
	--Specimen
	[CollectionSpecimenID] [int] Primary key, --
	[Accession_number] [nvarchar](50) NULL, --
	-- withholding
	[Data_withholding_reason] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collector] [nvarchar](255) NULL, --
	[Collectors_event_number] [nvarchar](50) NULL, --
	-- event
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
	--localisation
	[Named_area] [nvarchar](255) NULL, -- 
	[NamedAreaLocation2] [nvarchar](255) NULL, --
	[Remove_link_to_gazetteer] [int] NULL,
	[Distance_to_location] [varchar](50) NULL, --
	[Direction_to_location] [varchar](50) NULL, --
	[Longitude] [nvarchar](255) NULL, --
	[Latitude] [nvarchar](255) NULL, --
	[Coordinates_accuracy] [nvarchar](50) NULL, --
	[Link_to_GoogleMaps] [int] NULL,
	[_CoordinatesLocationNotes] [nvarchar](max) NULL, --
	[Altitude_from] [nvarchar](255) NULL, --
	[Altitude_to] [nvarchar](255) NULL, --
	[Altitude_accuracy] [nvarchar](50) NULL, --
	[Notes_for_Altitude] [nvarchar](max) NULL, --
	[MTB] [nvarchar](255) NULL, --
	[Quadrant] [nvarchar](255) NULL, --
	[Notes_for_MTB] [nvarchar](max) NULL, --
	[Sampling_plot] [nvarchar](255) NULL, --
	[Link_to_SamplingPlots] [nvarchar](255) NULL, --
	[Remove_link_to_SamplingPlots] [int] NULL,
	[Accuracy_of_sampling_plot] [nvarchar](50) NULL, --
	[Latitude_of_sampling_plot] [real] NULL, --
	[Longitude_of_sampling_plot] [real] NULL, --
	--site properties
	[Geographic_region] [nvarchar](255) NULL, --
	[Lithostratigraphy] [nvarchar](255) NULL, --
	[Chronostratigraphy] [nvarchar](255) NULL, --
	[Biostratigraphy] [nvarchar](255) NULL, --
	--collector
	[Collectors_name] [nvarchar](255) NULL, --
	[Link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_collector] [int] NULL,
	[Collectors_number] [nvarchar](50) NULL, --
	[Notes_about_collector] [nvarchar](max) NULL, --
	--Accession etc.
	[Accession_day] [tinyint] NULL, --
	[Accession_month] [tinyint] NULL, --
	[Accession_year] [smallint] NULL, --
	[Accession_date_supplement] [nvarchar](255) NULL, --
	[Depositors_name] [nvarchar](255) NULL, --
	[Depositors_link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_Depositor] [int] NULL,
	[Depositors_accession_number] [nvarchar](50) NULL, --
	[Exsiccata_abbreviation] [nvarchar](255) NULL, --
	[Link_to_DiversityExsiccatae] [varchar](255) NULL, --
	[Remove_link_to_exsiccatae] [int] NULL,
	[Exsiccata_number] [nvarchar](50) NULL, --
	[Original_notes] [nvarchar](max) NULL, --
	[Additional_notes] [nvarchar](max) NULL, --
	[Internal_notes] [nvarchar](max) NULL, --
	[Label_title] [nvarchar](max) NULL, --
	[Label_type] [nvarchar](50) NULL, --
	[Label_transcription_state] [nvarchar](50) NULL, --
	[Label_transcription_notes] [nvarchar](255) NULL, --
	[Problems] [nvarchar](MAX) NULL, --
	[External_datasource] [int] NULL, --
	[External_identifier] [nvarchar](100) NULL, --

	-- unit
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
	-- identification
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
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, --
	[Remove_link_for_determiner] [int] NULL,
	-- analysis
	[Analysis] [nvarchar](50) NULL, --
	[AnalysisID] [int] NULL, --
	[Analysis_number] [nvarchar](50) NULL, --
	[Analysis_result] [nvarchar](max) NULL, --
	-- 2. unit
	[Taxonomic_group_of_second_organism] [nvarchar](50) NULL, --
	[Life_stage_of_second_organism] [nvarchar](255) NULL, --
	[Gender_of_second_organism] [nvarchar](50) NULL, --
	[Number_of_units_of_second_organism] [smallint] NULL, --
	[Circumstances_of_second_organism] [nvarchar](50) NULL, -- 
	[Identifier_of_second_organism] [nvarchar](50) NULL, --
	[Description_of_second_organism] [nvarchar](50) NULL, --
	[Only_observed_of_second_organism] [bit] NULL, --
	[Notes_for_second_organism] [nvarchar](max) NULL, --
	-- 2. indent
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
	[Determiner_of_second_organism] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_determiner_of_second_organism] [int] NULL,
	-- part
	[Collection] [int] NULL, --
	[Material_category] [nvarchar](50) NULL, --
	[Storage_location] [nvarchar](255) NULL, --
	[Stock] [float] NULL, --
	[Part_accession_number] [nvarchar](50) NULL, --
	[Storage_container] [nvarchar](500) NULL, --
	[Preparation_method] [nvarchar](max) NULL, --
	[Preparation_date] [datetime] NULL, --
	[Notes_for_part] [nvarchar](max) NULL, --
	--relation
	[Related_specimen_URL] [varchar](255) NULL, --
	[Related_specimen_display_text] [varchar](255) NULL, --
	[Link_to_DiversityCollection_for_relation] [varchar](255) NULL, --
	[Type_of_relation] [nvarchar](50) NULL, --
	[Related_specimen_description] [nvarchar](max) NULL, --
	[Related_specimen_notes] [nvarchar](max) NULL, --
	[Relation_is_internal] [bit] NULL, --
	-- hidden columns
	[_TransactionID] [int] NULL, --
	[_Transaction] [nvarchar](200) NULL, --
	[_CollectionEventID] [int] NULL, --
	[_IdentificationUnitID] [int] NULL, --
	[_IdentificationSequence] [smallint] NULL, --
	[_SecondUnitID] [int] NULL, --
	[_SecondSequence] [smallint] NULL, --
	[_SpecimenPartID] [int] NULL, --
	[_CoordinatesAverageLatitudeCache] [real] NULL, --
	[_CoordinatesAverageLongitudeCache] [real] NULL, --
	[_GeographicRegionPropertyURI] [varchar](255) NULL, --
	[_LithostratigraphyPropertyURI] [varchar](255) NULL, --
	[_ChronostratigraphyPropertyURI] [varchar](255) NULL, --
	[_BiostratigraphyPropertyURI] [varchar](255) NULL, --
	[_NamedAverageLatitudeCache] [real] NULL, --
	[_NamedAverageLongitudeCache] [real] NULL, --
	[_LithostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_ChronostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_BiostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_SecondUnitFamilyCache] [nvarchar](255) NULL, --
	[_SecondUnitOrderCache] [nvarchar](255) NULL, --
	[_AverageAltitudeCache] [real] NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
Adding Relations, 
Specimen: Reference, ExternalSource
Part: Container, Description
Biostratigraphy


MW 18.03.2015 
TEST: 
Select * from dbo.FirstLines_3('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select 	[Altitude_from],
	[Altitude_to],
	[Altitude_accuracy],
	[Notes_for_Altitude],
	[MTB] [nvarchar],
	[Quadrant] [nvarchar],
	[Notes_for_MTB],
	[Sampling_plot],
	[Link_to_SamplingPlots],
	[Remove_link_to_SamplingPlots]
 from dbo.FirstLines_3('3251, 3252')
 Select *
 from dbo.FirstLines_3('3251,3252')
 Select *
 from dbo.FirstLines_3('3251, 3252')
 Select *
 from dbo.FirstLines_3('3251, 3252,')
 Select *
 from dbo.FirstLines_3('3251,')
 Select *
 from dbo.FirstLines_3('3251')
*/ 
AS 
BEGIN 
declare @IDs table (ID int  Primary key)
declare @sID varchar(50)
declare @sLenth int
set @sLenth = len(@CollectionSpecimenIDs)
while @CollectionSpecimenIDs <> ''-- AND charindex(',',@CollectionSpecimenIDs) > 0
begin
	if (CHARINDEX(',', @CollectionSpecimenIDs) > 0)
		begin
		set @sID = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, 1, CHARINDEX(',', @CollectionSpecimenIDs) -1)))
		set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 1, 8000)))
		if (isnumeric(@sID) = 1)
			begin
				insert into @IDs 
				values( @sID )
			end
		end
	else
		begin
		if (isnumeric(@CollectionSpecimenIDs) = 1 AND ((select count(*) from @IDs) = 0 OR len(rtrim(ltrim(@CollectionSpecimenIDs))) >= len(@sID) OR @sLenth < 8000))
			begin
				set @sID = rtrim(ltrim(@CollectionSpecimenIDs))
				insert into @IDs 
				values( @sID )
			end
		set @CollectionSpecimenIDs = ''
		end
end

-- insert basic informations to specimen
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
, External_datasource
, External_identifier
--, Reference_title_for_specimen
--, Link_to_DiversityReferences_for_specimen
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
, ExternalDatasourceID
, ExternalIdentifier
--, ReferenceTitle
--, ReferenceURI
from dbo.CollectionSpecimen S, dbo.CollectionSpecimenID_UserAvailable U
where S.CollectionSpecimenID in (select ID from @IDs)  
and U.CollectionSpecimenID = S.CollectionSpecimenID

-- insert information about collection event
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

-- insert gazetteer infos
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

-- insert Coordinates 
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

-- Altitide
update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
, L.Notes_for_Altitude = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4

--MTB
update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 3

--Sampling Plot
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

--Geographic_region
update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 10

--Lithostratigraphy
update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 30

--Chronostratigraphy
update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 20

--Chronostratigraphy
update L
set L.Biostratigraphy = P.DisplayText
, L._BiostratigraphyPropertyURI = P.PropertyURI
, L._BiostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 60

--Collector
update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
where L.CollectionSpecimenID = A.CollectionSpecimenID
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

-- getting the units
declare @AllUnitIDs table (UnitID int  Primary key, ID int, DisplayOrder smallint, RelatedUnitID int)
insert into @AllUnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
select U.IdentificationUnitID, U.CollectionSpecimenID, U.DisplayOrder, U.RelatedUnitID
from IdentificationUnit as U, @IDs as IDs
where DisplayOrder > 0
and IDs.ID = U.CollectionSpecimenID 

declare @FirstUnitIDs table (UnitID int  Primary key, ID int, DisplayOrder smallint)--, RelatedUnitID int)
insert into @FirstUnitIDs (UnitID, ID, DisplayOrder)--, RelatedUnitID)
select U.IdentificationUnitID, MIN(U.CollectionSpecimenID), Min(U.DisplayOrder)--, U.RelatedUnitID
from IdentificationUnit as U inner join @IDs as IDs 
on IDs.ID = U.CollectionSpecimenID 
and U.DisplayOrder > 0
group by U.IdentificationUnitID--, U.CollectionSpecimenID


declare @UnitIDs table (UnitID int  Primary key, ID int, DisplayOrder smallint, RelatedUnitID int)

--insert into @UnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
--select U.UnitID, U.ID, U.DisplayOrder, U.RelatedUnitID
--from @AllUnitIDs as U,  @AllUnitIDs as M
--where U.ID = M.ID
--group by M.ID, U.DisplayOrder, U.ID, U.RelatedUnitID, U.UnitID
--having U.DisplayOrder = MIN(M.DisplayOrder)

-- neue Version - optimiert

insert into @UnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
select U.UnitID, U.ID, U.DisplayOrder, U.RelatedUnitID
from @AllUnitIDs as U
inner join @FirstUnitIDs as M 
on U.ID = M.ID and U.DisplayOrder = M.DisplayOrder
group by M.ID, U.DisplayOrder, U.ID, U.RelatedUnitID, U.UnitID


--Unit
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

--Identification
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
--, L.Reference_title = I.ReferenceTitle
--, L.Link_to_DiversityReferences = I.ReferenceURI
, L.Determiner = I.ResponsibleName
, L.Link_to_DiversityAgents_for_determiner = I.ResponsibleAgentURI
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

--Analysis
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

-- 2. Unit
update L
set L.Taxonomic_group_of_second_organism = I.TaxonomicGroup
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

-- 2. Ident
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
--, L.Reference_title_of_second_organism = I.ReferenceTitle
--, L.Link_to_DiversityReferences_of_second_organism = I.ReferenceURI
, L.Determiner_of_second_organism = I.ResponsibleName
, L.Link_to_DiversityAgents_for_determiner_of_second_organism = I.ResponsibleAgentURI
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

-- Part
update L
set L._SpecimenPartID = P.SpecimenPartID
, L.Collection = P.CollectionID
, L.Material_category = P.MaterialCategory
, L.Storage_location = P.StorageLocation
, L.Stock = P.Stock
, L.Preparation_method = P.PreparationMethod
, L.Preparation_date = P.PreparationDate
, L.Notes_for_part = P.Notes
, L.Storage_container = P.StorageContainer
, L.Part_accession_number = P.AccessionNumber
from @List L,
dbo.CollectionSpecimenPart P
where L.CollectionSpecimenID = P.CollectionSpecimenID
and EXISTS
	(SELECT Pmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenPart AS Pmin
	GROUP BY Pmin.CollectionSpecimenID
	HAVING (Pmin.CollectionSpecimenID = P.CollectionSpecimenID) AND (MIN(Pmin.SpecimenPartID) = P.SpecimenPartID))

-- Transaction
update L
set L._TransactionID = P.TransactionID
--, L.On_loan = P.IsOnLoan
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

--Relation
update L
set L.Related_specimen_URL = R.RelatedSpecimenURI
, L.Related_specimen_description = R.RelatedSpecimenDescription
, L.Relation_is_internal = R.IsInternalRelationCache
, L.Type_of_relation = R.RelationType
, L.Link_to_DiversityCollection_for_relation = case when R.IsInternalRelationCache = 1 then R.RelatedSpecimenURI else '' end
, L.Related_specimen_display_text = R.RelatedSpecimenDisplayText
, L.Related_specimen_notes = R.Notes
from @List L,
dbo.CollectionSpecimenRelation R
where L.CollectionSpecimenID = R.CollectionSpecimenID
and EXISTS
	(SELECT Rmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenRelation AS Rmin
	GROUP BY Rmin.CollectionSpecimenID
	HAVING (Rmin.CollectionSpecimenID = R.CollectionSpecimenID) AND (MIN(Rmin.RelatedSpecimenURI) = R.RelatedSpecimenURI))


RETURN 
END   

GO



--#####################################################################################################################
--######   [FirstLinesPart_2] - Change Problems to nvarchar(MAX)   ####################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[FirstLinesPart_2] 
(@CollectionSpecimenIDs varchar(8000), 
@AnalysisIDs varchar(8000), @AnalysisStartDate date, @AnalysisEndDate date, 
@ProcessingID int, @ProcessingStartDate datetime, @ProcessingEndDate datetime)   
RETURNS @List TABLE (
	[SpecimenPartID] [int] Primary key,
	[CollectionSpecimenID] [int], --
	[Accession_number] [nvarchar](50) NULL, --
	[Data_withholding_reason] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collector] [nvarchar](255) NULL, --
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
	[Geographic_region] [nvarchar](255) NULL, --
	[Lithostratigraphy] [nvarchar](255) NULL, --
	[Chronostratigraphy] [nvarchar](255) NULL, --
	[Collectors_name] [nvarchar](255) NULL, --
	[Link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_collector] [int] NULL,
	[Collectors_number] [nvarchar](50) NULL, --
	[Notes_about_collector] [nvarchar](max) NULL, --
	[Accession_day] [tinyint] NULL, --
	[Accession_month] [tinyint] NULL, --
	[Accession_year] [smallint] NULL, --
	[Accession_date_supplement] [nvarchar](255) NULL, --
	[Depositors_name] [nvarchar](255) NULL, --
	[Depositors_link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_Depositor] [int] NULL,
	[Depositors_accession_number] [nvarchar](50) NULL, --
	[Exsiccata_abbreviation] [nvarchar](255) NULL, --
	[Link_to_DiversityExsiccatae] [varchar](255) NULL, --
	[Remove_link_to_exsiccatae] [int] NULL,
	[Exsiccata_number] [nvarchar](50) NULL, --
	[Original_notes] [nvarchar](max) NULL, --
	[Additional_notes] [nvarchar](max) NULL, --
	[Internal_notes] [nvarchar](max) NULL, --
	[Label_title] [nvarchar](255) NULL, --
	[Label_type] [nvarchar](50) NULL, --
	[Label_transcription_state] [nvarchar](50) NULL, --
	[Label_transcription_notes] [nvarchar](255) NULL, --
	[Problems] [nvarchar](MAX) NULL, --
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
	--[Reference_title] [nvarchar](255) NULL, --
	--[Link_to_DiversityReferences] [varchar](255) NULL, --
	--[Remove_link_for_reference] [int] NULL,
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, --
	[Remove_link_for_determiner] [int] NULL,
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
	[Preparation_method] [nvarchar](max) NULL, --
	[Preparation_date] [datetime] NULL, --
	[Part_accession_number] [nvarchar](50) NULL, --
	[Part_sublabel] [nvarchar](50) NULL, --
	[Collection] [int] NULL, --
	[Material_category] [nvarchar](50) NULL, --
	[Storage_location] [nvarchar](255) NULL, --
	[Storage_container] [nvarchar](500) NULL, --
	[Stock] [float] NULL, --
	[Stock_unit] [nvarchar](50) NULL, --
	[Notes_for_part] [nvarchar](max) NULL, --
	[Description_of_unit_in_part] [nvarchar](500) NULL, --
	[Processing_date_1] [datetime] NULL,
	[ProcessingID_1] [int] NULL,
	[Processing_Protocoll_1] [nvarchar](100) NULL,
	[Processing_duration_1] [varchar](50) NULL,
	[Processing_notes_1] [nvarchar](max) NULL,
	[Processing_date_2] [datetime] NULL,
	[ProcessingID_2] [int] NULL,
	[Processing_Protocoll_2] [nvarchar](100) NULL,
	[Processing_duration_2] [varchar](50) NULL,
	[Processing_notes_2] [nvarchar](max) NULL,
	[Processing_date_3] [datetime] NULL,
	[ProcessingID_3] [int] NULL,
	[Processing_Protocoll_3] [nvarchar](100) NULL,
	[Processing_duration_3] [varchar](50) NULL,
	[Processing_notes_3] [nvarchar](max) NULL,
	[Processing_date_4] [datetime] NULL,
	[ProcessingID_4] [int] NULL,
	[Processing_Protocoll_4] [nvarchar](100) NULL,
	[Processing_duration_4] [varchar](50) NULL,
	[Processing_notes_4] [nvarchar](max) NULL,
	[Processing_date_5] [datetime] NULL,
	[ProcessingID_5] [int] NULL,
	[Processing_Protocoll_5] [nvarchar](100) NULL,
	[Processing_duration_5] [varchar](50) NULL,
	[Processing_notes_5] [nvarchar](max) NULL,
	[_TransactionID] [int] NULL, --
	[_Transaction] [nvarchar](200) NULL, --
	[On_loan] [int] NULL, --
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
MW 18.08.2011 
TEST: 
Select * from dbo.FirstLinesPart('3251, 3252', '34', null, null, null, null) order by CollectionSpecimenID, SpecimenPartID
Select * from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '1/1/2000', '12/12/2010') order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '1/1/2000', '12/12/2010') P order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '2000/2/1', '2010/12/31') P order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, null, null) P order by CollectionSpecimenID, SpecimenPartID
select * from CollectionSpecimenProcessing
Select * from dbo.FirstLinesPart('177930', '26,39,41,44,45', null, null, null, null, null) P order by CollectionSpecimenID, SpecimenPartID
Select * from dbo.FirstLinesPart('177930', '26,39,41,44,45', null, null, 8, null, null) P order by CollectionSpecimenID, SpecimenPartID
Select * from dbo.FirstLinesPart('3251,3252', '26,39,41,44,45', null, null, 8, null, null) P order by CollectionSpecimenID, SpecimenPartID
*/ 
AS 
BEGIN 
declare @IDs table (ID int  Primary key)
declare @sID varchar(50)
-- getting the IDs out of the string
while @CollectionSpecimenIDs <> ''
begin
	if (CHARINDEX(',', @CollectionSpecimenIDs) > 0)
		begin
		set @sID = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, 1, CHARINDEX(',', @CollectionSpecimenIDs) -1)))
		set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 1, 8000)))
		if (isnumeric(@sID) = 1)
			begin
			insert into @IDs 
			values( @sID )
			end
		end
	else
		begin
		if (isnumeric(@CollectionSpecimenIDs) = 1 AND ((select count(*) from @IDs) = 0 OR len(rtrim(ltrim(@CollectionSpecimenIDs))) >= len(@sID)))
			begin
			set @sID = rtrim(ltrim(@CollectionSpecimenIDs))
			insert into @IDs 
			values( @sID )
			end
		set @CollectionSpecimenIDs = ''
		end
end
insert into @List (
SpecimenPartID
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
, Part_accession_number
, [Collection]
, Material_category
, Notes_for_part
, Part_sublabel
, Preparation_date
, Preparation_method
, Stock
, Stock_unit
, Storage_location
, Storage_container
)
select 
P.SpecimenPartID
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
, P.AccessionNumber
, P.CollectionID
, P.MaterialCategory
, P.Notes
, P.PartSublabel
, P.PreparationDate
, P.PreparationMethod
, P.Stock
, P.StockUnit
, P.StorageLocation
, P.StorageContainer
from dbo.CollectionSpecimen S, dbo.CollectionSpecimenPart P, dbo.CollectionSpecimenID_UserAvailable A
where S.CollectionSpecimenID in (select ID from @IDs) 
and P.CollectionSpecimenID = S.CollectionSpecimenID 
and S.CollectionSpecimenID = A.CollectionSpecimenID

-- setting the data from CollectionEvent
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

-- setting the data from CollectionEventLocalisation
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

update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4

update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = cast(E.LocationNotes as nvarchar(255))
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 3

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

update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 10

update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 30

update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 20

update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
where L.CollectionSpecimenID = A.CollectionSpecimenID
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
declare @AllUnitIDs table (UnitID int, ID int, DisplayOrder smallint, PartID int)
declare @UnitIDs table (UnitID int, ID int, DisplayOrder smallint, PartID int)
insert into @AllUnitIDs (UnitID, ID, DisplayOrder, PartID)
select P.IdentificationUnitID, P.CollectionSpecimenID, P.DisplayOrder, P.SpecimenPartID
from @IDs as IDs, IdentificationUnitInPart as P, @List as L
where P.DisplayOrder > 0
and IDs.ID = P.CollectionSpecimenID 
and P.SpecimenPartID = L.SpecimenPartID
insert into @UnitIDs (UnitID, ID, DisplayOrder, PartID)
select U.UnitID, U.ID, U.DisplayOrder, U.PartID
from @AllUnitIDs as U
where exists (select * from @AllUnitIDs aU group by aU.ID having min(aU.DisplayOrder) = U.DisplayOrder)

update L
set L.Taxonomic_group = IU.TaxonomicGroup
, L._IdentificationUnitID = IU.IdentificationUnitID
, L.Relation_type = IU.RelationType
, L.Colonised_substrate_part = IU.ColonisedSubstratePart
, L.Life_stage = IU.LifeStage
, L.Gender = IU.Gender
, L.Number_of_units = IU.NumberOfUnits
, L.Circumstances = IU.Circumstances
, L.Order_of_taxon = IU.OrderCache
, L.Family_of_taxon = IU.FamilyCache
, L.Identifier_of_organism = IU.UnitIdentifier
, L.Description_of_organism = IU.UnitDescription
, L.Only_observed = IU.OnlyObserved
, L.Notes_for_organism = IU.Notes
, L.Exsiccata_number = IU.ExsiccataNumber
, L.Description_of_unit_in_part = UP.[Description]
from @List L,
IdentificationUnitInPart UP,
dbo.IdentificationUnit IU,
@UnitIDs U
where L.CollectionSpecimenID = UP.CollectionSpecimenID
and L.SpecimenPartID = UP.SpecimenPartID
and L.CollectionSpecimenID = IU.CollectionSpecimenID
and UP.CollectionSpecimenID = IU.CollectionSpecimenID
and UP.IdentificationUnitID = IU.IdentificationUnitID
and U.UnitID = UP.IdentificationUnitID
and U.PartID = UP.SpecimenPartID
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
--, L.Reference_title = I.ReferenceTitle
--, L.Link_to_DiversityReferences = I.ReferenceURI
, L.Determiner = I.ResponsibleName
, L.Link_to_DiversityAgents_for_determiner = I.ResponsibleAgentURI
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
		if (isnumeric(@sID) = 1 and (select count(*) from @AnalysisID where AnalysisID = @sID) = 0)
			begin
			insert into @AnalysisID (AnalysisID)
			values( @sAnalysisID )
			end
		end
		else
		begin
		set @sAnalysisID = rtrim(ltrim(@AnalysisIDs))
		set @AnalysisIDs = ''
		if (isnumeric(@sAnalysisID) = 1 and (select count(*) from @AnalysisID where AnalysisID = @sID) = 0)
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

update L
set L.AnalysisID_0 = I.AnalysisID
, L.Analysis_number_0 = I.AnalysisNumber
, L.Analysis_result_0 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_0
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_1 = I.AnalysisID
, L.Analysis_number_1 = I.AnalysisNumber
, L.Analysis_result_1 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_1
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_2 = I.AnalysisID
, L.Analysis_number_2 = I.AnalysisNumber
, L.Analysis_result_2 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_2
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_2
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_3 = I.AnalysisID
, L.Analysis_number_3 = I.AnalysisNumber
, L.Analysis_result_3 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_3
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_3
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_4 = I.AnalysisID
, L.Analysis_number_4 = I.AnalysisNumber
, L.Analysis_result_4 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_4
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_4
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_5 = I.AnalysisID
, L.Analysis_number_5 = I.AnalysisNumber
, L.Analysis_result_5 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_5
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_5
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_6 = I.AnalysisID
, L.Analysis_number_6 = I.AnalysisNumber
, L.Analysis_result_6 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_6
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_6
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_7 = I.AnalysisID
, L.Analysis_number_7 = I.AnalysisNumber
, L.Analysis_result_7 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_7
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_7
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_8 = I.AnalysisID
, L.Analysis_number_8 = I.AnalysisNumber
, L.Analysis_result_8 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_8
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_8
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
update L
set L.AnalysisID_9 = I.AnalysisID
, L.Analysis_number_9 = I.AnalysisNumber
, L.Analysis_result_9 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_9
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_9
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
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

/*
Update L
set L.Related_Organism = Urel.LastIdentificationCache
from  @List L
, dbo.IdentificationUnit U
, dbo.IdentificationUnit Urel
where U.IdentificationUnitID = L._IdentificationUnitID
and U.RelatedUnitID = Urel.IdentificationUnitID	
and U.CollectionSpecimenID = Urel.CollectionSpecimenID
*/

declare @IdentificationUnitIDs TABLE (UnitID int not null Primary key, LastIdentificationCache nvarchar(500))
insert into @IdentificationUnitIDs (UnitID) select L._IdentificationUnitID from @List L where not L._IdentificationUnitID is null group by L._IdentificationUnitID

Update L
set L.LastIdentificationCache = Urel.LastIdentificationCache
from  @IdentificationUnitIDs L
, dbo.IdentificationUnit U
, dbo.IdentificationUnit Urel
where U.IdentificationUnitID = L.UnitID
and U.RelatedUnitID = Urel.IdentificationUnitID	
and U.CollectionSpecimenID = Urel.CollectionSpecimenID

Update L
set L.Related_Organism = U.LastIdentificationCache
from  @List L, @IdentificationUnitIDs U
where U.UnitID = L._IdentificationUnitID


declare @ProcessingIDs varchar(4000)
if (@ProcessingID is null)
begin
	declare @Processing table (ProcessingID int Primary key)
	insert into @Processing
	select ProcessingID from Processing
end
else
begin
	insert into @Processing
	select @ProcessingID
end
declare @StartDate datetime
declare @EndDate datetime
if (ISDATE(@ProcessingStartDate) = 1) begin set @StartDate = @ProcessingStartDate end
else begin set @StartDate = (select MIN(ProcessingDate) from CollectionSpecimenProcessing) end
if (ISDATE(@ProcessingEndDate) = 1) begin set @EndDate = @ProcessingEndDate end
else begin set @EndDate = (select MAX(ProcessingDate) from CollectionSpecimenProcessing) end

update L
set L.Processing_date_1 = P.ProcessingDate
, L.ProcessingID_1 = P.ProcessingID
, L.Processing_duration_1 = P.ProcessingDuration
, L.Processing_notes_1 = P.Notes
, L.Processing_Protocoll_1 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID

update L
set L.Processing_date_2 = P.ProcessingDate
, L.ProcessingID_2 = P.ProcessingID
, L.Processing_duration_2 = P.ProcessingDuration
, L.Processing_notes_2 = P.Notes
, L.Processing_Protocoll_2 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_1
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID

update L
set L.Processing_date_3 = P.ProcessingDate
, L.ProcessingID_3 = P.ProcessingID
, L.Processing_duration_3 = P.ProcessingDuration
, L.Processing_notes_3 = P.Notes
, L.Processing_Protocoll_3 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_2
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID

update L
set L.Processing_date_4 = P.ProcessingDate
, L.ProcessingID_4 = P.ProcessingID
, L.Processing_duration_4 = P.ProcessingDuration
, L.Processing_notes_4 = P.Notes
, L.Processing_Protocoll_4 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_3
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID

update L
set L.Processing_date_5 = P.ProcessingDate
, L.ProcessingID_5 = P.ProcessingID
, L.Processing_duration_5 = P.ProcessingDuration
, L.Processing_notes_5 = P.Notes
, L.Processing_Protocoll_5 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S 
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.ProcessingDate > L.Processing_date_4
AND S.SpecimenPartID = P.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID

update L
set L._TransactionID = P.TransactionID
, L.On_loan = P.IsOnLoan
from @List L,
dbo.CollectionSpecimenTransaction P
where L.CollectionSpecimenID = P.CollectionSpecimenID
and L.SpecimenPartID = P.SpecimenPartID
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
--######   FirstLinesUnit_4: Change Problems to nvarchar(MAX)   #######################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[FirstLinesUnit_4] 
(@CollectionSpecimenIDs varchar(8000), @AnalysisIDs varchar(8000), @AnalysisStartDate date, @AnalysisEndDate date)   
RETURNS @List TABLE (
	[IdentificationUnitID] [int] Primary key,
	[CollectionSpecimenID] [int], 
	[Accession_number] [nvarchar](50) NULL, 
	[Data_withholding_reason] [nvarchar](255) NULL, 
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, 
	[Data_withholding_reason_for_collector] [nvarchar](255) NULL, 
	[Collectors_event_number] [nvarchar](50) NULL, 
	[Collection_day] [tinyint] NULL, 
	[Collection_month] [tinyint] NULL, 
	[Collection_year] [smallint] NULL, 
	[Collection_date_supplement] [nvarchar](100) NULL, 
	[Collection_time] [varchar](50) NULL, 
	[Collection_time_span] [varchar](50) NULL, 
	[Country] [nvarchar](50) NULL, 
	[Locality_description] [nvarchar](255) NULL, 
	[Habitat_description] [nvarchar](255) NULL, 
	[Collecting_method] [nvarchar](255) NULL, 
	[Collection_event_notes] [nvarchar](255) NULL, 
	[Named_area] [nvarchar](255) NULL, 
	[NamedAreaLocation2] [nvarchar](255) NULL, 
	[Remove_link_to_gazetteer] [int] NULL,
	[Distance_to_location] [varchar](50) NULL, 
	[Direction_to_location] [varchar](50) NULL, 
	[Longitude] [nvarchar](255) NULL, 
	[Latitude] [nvarchar](255) NULL, 
	[Coordinates_accuracy] [nvarchar](50) NULL, 
	[Link_to_GoogleMaps] [int] NULL,
	[Altitude_from] [nvarchar](255) NULL, 
	[Altitude_to] [nvarchar](255) NULL, 
	[Altitude_accuracy] [nvarchar](50) NULL, 
	[Notes_for_Altitude] [nvarchar](255) NULL, 
	[MTB] [nvarchar](255) NULL, 
	[Quadrant] [nvarchar](255) NULL, 
	[Notes_for_MTB] [nvarchar](255) NULL, 
	[Sampling_plot] [nvarchar](255) NULL, 
	[Link_to_SamplingPlots] [nvarchar](255) NULL, 
	[Remove_link_to_SamplingPlots] [int] NULL,
	[Accuracy_of_sampling_plot] [nvarchar](50) NULL, 
	[Latitude_of_sampling_plot] [real] NULL, 
	[Longitude_of_sampling_plot] [real] NULL, 
	[Geographic_region] [nvarchar](255) NULL, 
	[Lithostratigraphy] [nvarchar](255) NULL, 
	[Chronostratigraphy] [nvarchar](255) NULL, 
	[Collectors_name] [nvarchar](255) NULL, 
	[Link_to_DiversityAgents] [varchar](255) NULL, 
	[Remove_link_for_collector] [int] NULL,
	[Collectors_number] [nvarchar](50) NULL, 
	[Notes_about_collector] [nvarchar](max) NULL, 
	[Accession_day] [tinyint] NULL, 
	[Accession_month] [tinyint] NULL, 
	[Accession_year] [smallint] NULL, 
	[Accession_date_supplement] [nvarchar](255) NULL, 
	[Depositors_name] [nvarchar](255) NULL, 
	[Depositors_link_to_DiversityAgents] [varchar](255) NULL, 
	[Remove_link_for_Depositor] [int] NULL,
	[Depositors_accession_number] [nvarchar](50) NULL, 
	[Exsiccata_abbreviation] [nvarchar](255) NULL, 
	[Link_to_DiversityExsiccatae] [varchar](255) NULL, 
	[Remove_link_to_exsiccatae] [int] NULL,
	[Exsiccata_number] [nvarchar](50) NULL, 
	[Original_notes] [nvarchar](max) NULL, 
	[Additional_notes] [nvarchar](max) NULL, 
	[Internal_notes] [nvarchar](max) NULL, 
	[Label_title] [nvarchar](255) NULL, 
	[Label_type] [nvarchar](50) NULL, 
	[Label_transcription_state] [nvarchar](50) NULL, 
	[Label_transcription_notes] [nvarchar](255) NULL, 
	[Problems] [nvarchar](MAX) NULL, 
	[Taxonomic_group] [nvarchar](50) NULL, 
	[Relation_type] [nvarchar](50) NULL, 
	[Colonised_substrate_part] [nvarchar](255) NULL, 
	[Related_organism] [nvarchar] (200) NULL,
	[Life_stage] [nvarchar](255) NULL, 
	[Gender] [nvarchar](50) NULL, 
	[Number_of_units] [smallint] NULL, 
	[Circumstances] [nvarchar](50) NULL, 
	[Order_of_taxon] [nvarchar](255) NULL, 
	[Family_of_taxon] [nvarchar](255) NULL, 
	[Identifier_of_organism] [nvarchar](50) NULL, 
	[Description_of_organism] [nvarchar](50) NULL, 
	[Only_observed] [bit] NULL, 
	[Notes_for_organism] [nvarchar](max) NULL, 
	[Taxonomic_name] [nvarchar](255) NULL, 
	[Link_to_DiversityTaxonNames] [varchar](255) NULL, 
	[Remove_link_for_identification] [int] NULL, 
	[Vernacular_term] [nvarchar](255) NULL, 
	[Identification_day] [tinyint] NULL, 
	[Identification_month] [tinyint] NULL, 
	[Identification_year] [smallint] NULL, 
	[Identification_category] [nvarchar](50) NULL, 
	[Identification_qualifier] [nvarchar](50) NULL, 
	[Type_status] [nvarchar](50) NULL, 
	[Type_notes] [nvarchar](max) NULL, 
	[Notes_for_identification] [nvarchar](max) NULL, 
	--[Reference_title] [nvarchar](255) NULL, 
	--[Link_to_DiversityReferences] [varchar](255) NULL, 
	--[Remove_link_for_reference] [int] NULL,
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, 
	[Remove_link_for_determiner] [int] NULL,
	[Analysis_0] [nvarchar](50) NULL, 
	[AnalysisID_0] [int] NULL, 
	[Analysis_number_0] [nvarchar](50) NULL, 
	[Analysis_result_0] [nvarchar](max) NULL, 
	[Analysis_1] [nvarchar](50) NULL, 
	[AnalysisID_1] [int] NULL, 
	[Analysis_number_1] [nvarchar](50) NULL, 
	[Analysis_result_1] [nvarchar](max) NULL, 
	[Analysis_2] [nvarchar](50) NULL, 
	[AnalysisID_2] [int] NULL, 
	[Analysis_number_2] [nvarchar](50) NULL, 
	[Analysis_result_2] [nvarchar](max) NULL, 
	[Analysis_3] [nvarchar](50) NULL, 
	[AnalysisID_3] [int] NULL, 
	[Analysis_number_3] [nvarchar](50) NULL, 
	[Analysis_result_3] [nvarchar](max) NULL, 
	[Analysis_4] [nvarchar](50) NULL, 
	[AnalysisID_4] [int] NULL, 
	[Analysis_number_4] [nvarchar](50) NULL, 
	[Analysis_result_4] [nvarchar](max) NULL, 
	[Analysis_5] [nvarchar](50) NULL, 
	[AnalysisID_5] [int] NULL, 
	[Analysis_number_5] [nvarchar](50) NULL, 
	[Analysis_result_5] [nvarchar](max) NULL, 
	[Analysis_6] [nvarchar](50) NULL, 
	[AnalysisID_6] [int] NULL, 
	[Analysis_number_6] [nvarchar](50) NULL, 
	[Analysis_result_6] [nvarchar](max) NULL, 
	[Analysis_7] [nvarchar](50) NULL, 
	[AnalysisID_7] [int] NULL, 
	[Analysis_number_7] [nvarchar](50) NULL, 
	[Analysis_result_7] [nvarchar](max) NULL, 
	[Analysis_8] [nvarchar](50) NULL, 
	[AnalysisID_8] [int] NULL, 
	[Analysis_number_8] [nvarchar](50) NULL, 
	[Analysis_result_8] [nvarchar](max) NULL, 
	[Analysis_9] [nvarchar](50) NULL, 
	[AnalysisID_9] [int] NULL, 
	[Analysis_number_9] [nvarchar](50) NULL, 
	[Analysis_result_9] [nvarchar](max) NULL, 
	[Collection] [int] NULL, 
	[Material_category] [nvarchar](50) NULL, 
	[Storage_location] [nvarchar](255) NULL, 
	[Stock] [float] NULL, 
	[Preparation_method] [nvarchar](max) NULL, 
	[Preparation_date] [datetime] NULL, 
	[Notes_for_part] [nvarchar](max) NULL, 
	[_TransactionID] [int] NULL, 
	[_Transaction] [nvarchar](200) NULL, 
	[On_loan] [int] NULL, 
	[_CollectionEventID] [int] NULL, 
	[_IdentificationUnitID] [int] NULL, 
	[_IdentificationSequence] [smallint] NULL, 
	[_SpecimenPartID] [int] NULL, 
	[_CoordinatesAverageLatitudeCache] [real] NULL, 
	[_CoordinatesAverageLongitudeCache] [real] NULL, 
	[_CoordinatesLocationNotes] [nvarchar](255) NULL, 
	[_GeographicRegionPropertyURI] [varchar](255) NULL, 
	[_LithostratigraphyPropertyURI] [varchar](255) NULL, 
	[_ChronostratigraphyPropertyURI] [varchar](255) NULL, 
	[_NamedAverageLatitudeCache] [real] NULL, 
	[_NamedAverageLongitudeCache] [real] NULL, 
	[_LithostratigraphyPropertyHierarchyCache] [nvarchar](255) NULL, 
	[_ChronostratigraphyPropertyHierarchyCache] [nvarchar](255) NULL, 
	[_AverageAltitudeCache] [real] NULL)     
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
Select * from dbo.FirstLinesUnit_3('3251,3252,', null, '2000/2/1', '2010/12/31') order by CollectionSpecimenID, IdentificationUnitID
SELECT *
FROM dbo.FirstLinesUnit_3  ('193972', '18,75', '2011/8/17', null) 
ORDER BY Accession_number, CollectionSpecimenID, IdentificationUnitID 
SELECT *
FROM dbo.FirstLinesUnit_3  ('193972', '43,81,49,18,75,51', '2011/8/17', null) 
ORDER BY Accession_number, CollectionSpecimenID, IdentificationUnitID 
*/ 
AS 
BEGIN 
declare @IDs table (ID int  Primary key)
declare @sID varchar(50)
/*
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
*/
while @CollectionSpecimenIDs <> ''
begin
	if (CHARINDEX(',', @CollectionSpecimenIDs) > 0)
		begin
		set @sID = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, 1, CHARINDEX(',', @CollectionSpecimenIDs) -1)))
		set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 1, 8000)))
		if (isnumeric(@sID) = 1)
			begin
			insert into @IDs 
			values( @sID )
			end
		end
	else
		begin
		if (isnumeric(@CollectionSpecimenIDs) = 1 AND ((select count(*) from @IDs) = 0 OR len(rtrim(ltrim(@CollectionSpecimenIDs))) >= len(@sID)))
			begin
			set @sID = rtrim(ltrim(@CollectionSpecimenIDs))
			insert into @IDs 
			values( @sID )
			end
		set @CollectionSpecimenIDs = ''
		end
end
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
update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4
update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = cast(E.LocationNotes as nvarchar(255))
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 3
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
update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 10
update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 30
update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 20
update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
where L.CollectionSpecimenID = A.CollectionSpecimenID
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
--, L.Reference_title = I.ReferenceTitle
--, L.Link_to_DiversityReferences = I.ReferenceURI
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
Update L
set L.Related_Organism = Urel.LastIdentificationCache
from  @List L
, dbo.IdentificationUnit U
, dbo.IdentificationUnit Urel
where U.IdentificationUnitID = L.IdentificationUnitID
and U.RelatedUnitID = Urel.IdentificationUnitID	
and U.CollectionSpecimenID = Urel.CollectionSpecimenID
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
--######  ISSUE #81  ##################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   New taxonomic group Protista  ##############################################################################
--#####################################################################################################################

IF(SELECT COUNT(*) FROM [CollMaterialCategory_Enum] WHERE [Code] = 'Protista') = 0
BEGIN
INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes])
     VALUES
           ('Protista'
           ,'Paraphyletic grouping of all descendants of the last eukaryotic common ancestor'
           ,'Protista'
           ,105
           ,1
           ,'Protists are a diverse group of eukaryotes (organisms whose cells possess a nucleus) that are primarily single-celled and microscopic but exhibit a wide variety of shapes and life strategies')

END
GO

--#####################################################################################################################
--######   New material category core sample  #########################################################################
--#####################################################################################################################

IF(SELECT COUNT(*) FROM [CollMaterialCategory_Enum] WHERE [Code] = 'core sample') = 0
BEGIN
INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes]
           ,[ParentCode])
     VALUES
           ('core sample'
           ,'core sample like an ice core, a drill core sample from soil, wood or rock etc.'
           ,'core sample'
           ,830
           ,1
           ,'especially for wood core samples'
           ,'other specimen')

END
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.50'
END

GO


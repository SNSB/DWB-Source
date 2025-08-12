declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.46'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#################################################################################################################################
--######   trgInsCollectionEvent - Bugfix   #######################################################################################
--#################################################################################################################################

ALTER TRIGGER [dbo].[trgInsCollectionEvent] ON [dbo].[CollectionEvent] 
FOR INSERT AS

/*  Date: 06.11.2018  */ 

/* setting the date fields */ 
Update CollectionEvent set CollectionDate = 
case when E.CollectionMonth is null or E.CollectionDay is null or E.CollectionYear is null then null 
else case when ISDATE(convert(varchar(40), cast(I.CollectionYear as varchar) + '-' 
	+ case when I.CollectionMonth < 10 then '0' else '' end + cast(I.CollectionMonth as varchar)  + '-' 
	+ case when I.CollectionDay < 10 then '0' else '' end + cast(I.CollectionDay as varchar) + 'T00:00:00.000Z', 127)) = 1
	AND I.CollectionYear > 1760
	AND I.CollectionMonth between 1 and 12
	AND I.CollectionDay between 1 and 31
then cast(convert(varchar(40), cast(I.CollectionYear as varchar) + '-' 
	+ case when I.CollectionMonth < 10 then '0' else '' end + cast(I.CollectionMonth as varchar)  + '-' 
	+ case when I.CollectionDay < 10 then '0' else '' end + cast(I.CollectionDay as varchar) + 'T00:00:00.000Z', 127) as datetime)
else null end end 
FROM CollectionEvent E, inserted I  
where E.CollectionEventID = I.CollectionEventID   

GO

--#####################################################################################################################
--######   FirstLines_4: Optimized Query for IdentificationUnit  ######################################################
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
	[Problems] [nvarchar](255) NULL, --
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
--######   Enabling requests in transaction   #########################################################################
--#####################################################################################################################

update e set [DisplayEnable] = 1
  FROM [dbo].[CollTransactionType_Enum] e where code = 'request'

  GO

--#####################################################################################################################
--######   CollectionEventSeriesHierarchySuperior #####################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[CollectionEventSeriesHierarchySuperior] (@SeriesID int)  
RETURNS @SeriesList TABLE ([SeriesID] [int] Primary key ,
	[SeriesParentID] [int] NULL ,
	[Description] [nvarchar](max) NOT NULL,
	[SeriesCode] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[Geography] [geography] NULL,
	[DateStart] [datetime] NULL,
	[DateEnd] [datetime] NULL)
/*
Returns a table that lists the given and all the items superior to the given series.
MW 02.09.2016
Test:
SELECT * FROM dbo.[CollectionEventSeriesHierarchySuperior](-8151)
*/
AS
BEGIN
declare @ParentID int
declare @i int

-- insert the given collection
   INSERT @SeriesList
SELECT [SeriesID]
      ,[SeriesParentID]
      ,[Description]
      ,[SeriesCode]
      ,[Notes]
      ,[Geography]
      ,[DateStart]
      ,[DateEnd]
   FROM CollectionEventSeries
   WHERE CollectionEventSeries.SeriesID = @SeriesID

-- insert the superior collections
-- check if there is any superior collection
if (select count(*) from @SeriesList where SeriesID = @SeriesID and SeriesParentID is null) = 1
	RETURN

-- getting the superiors
set @ParentID = (select MAX(SeriesParentID) from @SeriesList where not SeriesParentID is null AND SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesList)) 
set @i = 1
while (@i > 0)
begin
	set @ParentID = (select MAX(SeriesParentID) from @SeriesList where not SeriesParentID is null AND SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesList)) 
	--set @ParentID = @SeriesID
	INSERT @SeriesList
SELECT [SeriesID]
      ,[SeriesParentID]
      ,[Description]
      ,[SeriesCode]
      ,[Notes]
      ,[Geography]
      ,[DateStart]
      ,[DateEnd]
	FROM CollectionEventSeries
	WHERE CollectionEventSeries.SeriesID = @ParentID
	AND CollectionEventSeries.SeriesID NOT IN (SELECT SeriesID FROM @SeriesList)
	set @i = (select count(*) from @SeriesList where not SeriesParentID is null AND SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesList))
end

RETURN
END

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists the given and all the items superior to the given event series' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'CollectionEventSeriesHierarchySuperior'
GO

GRANT SELECT ON CollectionEventSeriesHierarchySuperior TO [USER]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.47'
END

GO


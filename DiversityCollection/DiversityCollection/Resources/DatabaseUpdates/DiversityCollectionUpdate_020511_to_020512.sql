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
IF (SELECT [dbo].[Version]()) <> '02.05.11'
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version 02.05.11. Current version = ' + @Version
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  Dutch RD coordinates  ####################################################
--#####################################################################################################################

if (Select COUNT(*) from [LocalisationSystem] where LocalisationSystemID = 16) = 0
begin
  INSERT INTO [LocalisationSystem]
           ([LocalisationSystemID]
           ,[LocalisationSystemParentID]
           ,[LocalisationSystemName]
           ,[DefaultMeasurementUnit]
           ,[ParsingMethodName]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[DisplayOrder]
           ,[Description]
           ,[DisplayTextLocation1]
           ,[DescriptionLocation1]
           ,[DisplayTextLocation2]
           ,[DescriptionLocation2])
     VALUES
           (16
           ,9
           ,'Dutch RD coordinates'
           ,'km'
           ,'RD'
           ,'Dutch RD coordinates'
           ,1
           ,122
           ,'The Dutch RD coordinate system (Rijksdriehoeksmeting). The reference point is the Onze-Lieve-Vrouwentoren (Our Lady´s Tower) in Amersfoort, with RD coordinates (155.000, 463.000) and geographic coordinates approximately 52°9´N 5°23´E'
           ,'west-east'
           ,'west-east coordinate between 0 and 280 km'
           ,'south-north'
           ,'south-north coordinate between 300 and 620 km')
end
GO


--#####################################################################################################################
--######  Dutch RD coordinates  ####################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT

if (Select COUNT(*) from [CollIdentificationQualifier_Enum] where [Code] = 'cf. hybrid') = 0
begin
  INSERT INTO [CollIdentificationQualifier_Enum]
           ([Code]
      ,[Description]
      ,[DisplayText]
      ,[DisplayOrder]
      ,[DisplayEnable])
     VALUES
           ('cf. hybrid'
           ,'doubtful identification of hybrid. Example: cf. Salix alba x fragilis'
           ,'cf. hybrid'
           ,135
           ,1)
end
GO

--#####################################################################################################################
--######  video  ####################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT


if (Select COUNT(*) from [CollEventImageType_Enum] where [Code] = 'video') = 0
begin
INSERT INTO [CollEventImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('video'
           ,'video record'
           ,'video'
           ,12
           ,1)
end

if (Select COUNT(*) from [CollSpecimenImageType_Enum] where [Code] = 'video') = 0
begin
INSERT INTO [CollSpecimenImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('video'
           ,'video record'
           ,'video'
           ,12
           ,1)
end

GO





--#####################################################################################################################
--######  photograph  ####################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT


if (Select COUNT(*) from [CollEventImageType_Enum] where [Code] = 'photograph') = 0
begin
INSERT INTO [CollEventImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('photograph'
           ,'photographic image of a collection site'
           ,'photograph'
           ,3
           ,1)
end

if (Select COUNT(*) from [CollSpecimenImageType_Enum] where [Code] = 'photograph') = 0
begin
INSERT INTO [CollSpecimenImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('photograph'
           ,'photographic image of a specimen'
           ,'photograph'
           ,3
           ,1)
end

GO





--#####################################################################################################################
--######   otolith   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT


if (Select COUNT(*) from [CollMaterialCategory_Enum] where [Code] = 'otolith') = 0
begin
INSERT INTO [CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('otolith'
           ,'otolith of fishes'
           ,'otolith'
           ,508
           ,1
           ,'bones')
end
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT


/****** Object:  UserDefinedFunction [dbo].[FirstLines]    Script Date: 09/27/2010 12:44:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######   Korrektur FirstLines   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[FirstLines_2] 
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
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, --
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
	[Determiner_of_second_organism] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_determiner_of_second_organism] [int] NULL,
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
Select * from dbo.FirstLines_2('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select * from dbo.FirstLines_2('3251, 3252')
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

GRANT SELECT ON [dbo].[FirstLines_2] TO [DiversityCollectionUser] AS [dbo]
GO



--#####################################################################################################################
--######   [FirstLinesUnit_2]   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT


/****** Object:  UserDefinedFunction [dbo].[FirstLinesUnit_2]    Script Date: 09/27/2010 12:44:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[FirstLinesUnit_2] 
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
Select * from dbo.FirstLinesUnit_2('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select * from dbo.FirstLinesUnit_2('3251, 3252', '34', null, null) order by CollectionSpecimenID, IdentificationUnitID
Select * from dbo.FirstLinesUnit_2('193610, 193611') order by CollectionSpecimenID, IdentificationUnitID
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
from dbo.CollectionSpecimen S, dbo.IdentificationUnit U
where S.CollectionSpecimenID in (select ID from @IDs) 
and U.CollectionSpecimenID = S.CollectionSpecimenID 



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



-- ANALYSIS 

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
	declare @AnalysisID table (ID int Identity(0,1), AnalysisID int Primary key)
	declare @sAnalysisID varchar(50)
	declare @iAnalysis int
	set @iAnalysis = 0
	while @AnalysisIDs <> '' and @iAnalysis < 10
	begin
		if (CHARINDEX(',', @AnalysisIDs) > 0)
		begin
		set @sAnalysisID = rtrim(ltrim(SUBSTRING(@AnalysisIDs, 1, CHARINDEX(',', @AnalysisIDs) -1)))
		set @AnalysisIDs = rtrim(ltrim(SUBSTRING(@AnalysisIDs, CHARINDEX(',', @AnalysisIDs) + 2, 4000)))
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

else
begin --- default values for the AnalysisIDs if nothing was given

	set @AnalysisID_0 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where 1=1
		and L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID)
		
	set @AnalysisID_1 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0)
		
	set @AnalysisID_2 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0
		and A.AnalysisID <> @AnalysisID_1)
		
	set @AnalysisID_3 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0
		and A.AnalysisID <> @AnalysisID_1
		and A.AnalysisID <> @AnalysisID_2)

	set @AnalysisID_4 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0
		and A.AnalysisID <> @AnalysisID_1
		and A.AnalysisID <> @AnalysisID_2
		and A.AnalysisID <> @AnalysisID_3)

	set @AnalysisID_5 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0
		and A.AnalysisID <> @AnalysisID_1
		and A.AnalysisID <> @AnalysisID_2
		and A.AnalysisID <> @AnalysisID_3
		and A.AnalysisID <> @AnalysisID_4)

	set @AnalysisID_6 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0
		and A.AnalysisID <> @AnalysisID_1
		and A.AnalysisID <> @AnalysisID_2
		and A.AnalysisID <> @AnalysisID_3
		and A.AnalysisID <> @AnalysisID_4
		and A.AnalysisID <> @AnalysisID_5)

	set @AnalysisID_7 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0
		and A.AnalysisID <> @AnalysisID_1
		and A.AnalysisID <> @AnalysisID_2
		and A.AnalysisID <> @AnalysisID_3
		and A.AnalysisID <> @AnalysisID_4
		and A.AnalysisID <> @AnalysisID_5
		and A.AnalysisID <> @AnalysisID_6)

	set @AnalysisID_8 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0
		and A.AnalysisID <> @AnalysisID_1
		and A.AnalysisID <> @AnalysisID_2
		and A.AnalysisID <> @AnalysisID_3
		and A.AnalysisID <> @AnalysisID_4
		and A.AnalysisID <> @AnalysisID_5
		and A.AnalysisID <> @AnalysisID_6
		and A.AnalysisID <> @AnalysisID_7)

	set @AnalysisID_9 = (select min(A.AnalysisID) 
		from @List L, dbo.IdentificationUnitAnalysis A 
		where L.CollectionSpecimenID = A.CollectionSpecimenID 
		and L.IdentificationUnitID = A.IdentificationUnitID
		and A.AnalysisID <> @AnalysisID_0
		and A.AnalysisID <> @AnalysisID_1
		and A.AnalysisID <> @AnalysisID_2
		and A.AnalysisID <> @AnalysisID_3
		and A.AnalysisID <> @AnalysisID_4
		and A.AnalysisID <> @AnalysisID_5
		and A.AnalysisID <> @AnalysisID_6
		and A.AnalysisID <> @AnalysisID_7
		and A.AnalysisID <> @AnalysisID_8)
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

GRANT SELECT ON [dbo].[FirstLinesUnit_2] TO [DiversityCollectionUser] AS [dbo]
GO



--#####################################################################################################################
--######   [[AnalysisTaxonomicGroupForProject]]   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT

GO

	ALTER FUNCTION [dbo].[AnalysisTaxonomicGroupForProject] (@ProjectID int)  
	RETURNS @AnalysisTaxonomicGroup TABLE ([AnalysisID] [int] NOT NULL,
		[TaxonomicGroup] [nvarchar](50) NOT NULL,
		[RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)
	/*
	Returns the contents of the table AnalysisTaxonomicGroup used in a project including the whole hierarchy.
	MW 15.07.2009
	Test:
	select * from [dbo].[AnalysisTaxonomicGroupForProject] (372)
	*/
	AS
	BEGIN
		declare @Temp TABLE (AnalysisID int NOT NULL
		, TaxonomicGroup [nvarchar](50) NOT NULL
		, ID int Identity NOT NULL
		, [RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)
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
			INSERT INTO @Temp (AnalysisID, TaxonomicGroup, RowGUID)
			SELECT A.AnalysisID, @TaxonomicGroup, A.RowGUID
			FROM [DiversityCollection].[dbo].[AnalysisList] (@ProjectID, @TaxonomicGroup) L, Analysis A
			WHERE A.AnalysisID = L.AnalysisID AND A.OnlyHierarchy = 0
			DELETE FROM @TempID WHERE ID = @ID
		END

		insert @AnalysisTaxonomicGroup (AnalysisID, TaxonomicGroup, RowGUID)
		SELECT DISTINCT AnalysisID, TaxonomicGroup, RowGUID
		FROM @Temp
		WHERE NOT RowGUID IS NULL
		
		RETURN
	END
	
	GO
	
	
--#####################################################################################################################
--######   [DiversityWorkbenchModule]   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
GO
	
CREATE FUNCTION [dbo].[DiversityWorkbenchModule] ()   RETURNS nvarchar(50) AS BEGIN RETURN 'DiversityCollection' END  ; 
GO

GRANT EXECUTE ON [dbo].[DiversityWorkbenchModule] TO [DiversityCollectionUser] AS [dbo]
GO




--#####################################################################################################################
--######   Collection Image   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
GO


CREATE TABLE [dbo].[CollectionImage_log](
	[CollectionID] [int] NULL,
	[URI] [varchar](255) NULL,
	[ImageType] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionImage_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CollectionImage]    Script Date: 12/14/2010 15:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CollectionImage](
	[CollectionID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[ImageType] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CollectionImage] PRIMARY KEY CLUSTERED 
(
	[CollectionID] ASC,
	[URI] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of Collection (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'CollectionID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The complete URI address of the image. ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'URI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the image, e.g. label' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'ImageType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the collection image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
/****** Object:  Trigger [trgUpdCollectionImage]    Script Date: 12/14/2010 15:40:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdCollectionImage] ON [dbo].[CollectionImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.6 */ 
/*  Date: 14.12.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionImage_Log (CollectionID, URI, ImageType, Notes, DataWithholdingReason, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionID, deleted.URI, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionImage, deleted 
where 1 = 1 
AND CollectionImage.CollectionID = deleted.CollectionID
AND CollectionImage.URI = deleted.URI
GO
/****** Object:  Trigger [trgDelCollectionImage]    Script Date: 12/14/2010 15:40:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelCollectionImage] ON [dbo].[CollectionImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.6 */ 
/*  Date: 14.12.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionImage_Log (CollectionID, URI, ImageType, Notes, DataWithholdingReason, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionID, deleted.URI, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED
GO
/****** Object:  Default [DF_CollectionImage_LogCreatedWhen]    Script Date: 12/14/2010 15:40:01 ******/
ALTER TABLE [dbo].[CollectionImage] ADD  CONSTRAINT [DF_CollectionImage_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
/****** Object:  Default [DF_CollectionImage_LogCreatedBy]    Script Date: 12/14/2010 15:40:01 ******/
ALTER TABLE [dbo].[CollectionImage] ADD  CONSTRAINT [DF_CollectionImage_LogCreatedBy]  DEFAULT (user_name()) FOR [LogInsertedBy]
GO
/****** Object:  Default [DF_CollectionImage_LogUpdatedWhen]    Script Date: 12/14/2010 15:40:01 ******/
ALTER TABLE [dbo].[CollectionImage] ADD  CONSTRAINT [DF_CollectionImage_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
/****** Object:  Default [DF_CollectionImage_LogUpdatedBy]    Script Date: 12/14/2010 15:40:01 ******/
ALTER TABLE [dbo].[CollectionImage] ADD  CONSTRAINT [DF_CollectionImage_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
/****** Object:  Default [DF_CollectionImage_RowGUID]    Script Date: 12/14/2010 15:40:01 ******/
ALTER TABLE [dbo].[CollectionImage] ADD  CONSTRAINT [DF_CollectionImage_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
/****** Object:  Default [DF_CollectionImage_Log_LogState]    Script Date: 12/14/2010 15:40:02 ******/
ALTER TABLE [dbo].[CollectionImage_log] ADD  CONSTRAINT [DF_CollectionImage_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
/****** Object:  Default [DF_CollectionImage_Log_LogDate]    Script Date: 12/14/2010 15:40:02 ******/
ALTER TABLE [dbo].[CollectionImage_log] ADD  CONSTRAINT [DF_CollectionImage_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
/****** Object:  Default [DF_CollectionImage_Log_LogUser]    Script Date: 12/14/2010 15:40:02 ******/
ALTER TABLE [dbo].[CollectionImage_log] ADD  CONSTRAINT [DF_CollectionImage_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
/****** Object:  ForeignKey [FK_CollectionImage_Collection]    Script Date: 12/14/2010 15:40:01 ******/
ALTER TABLE [dbo].[CollectionImage]  WITH CHECK ADD  CONSTRAINT [FK_CollectionImage_Collection] FOREIGN KEY([CollectionID])
REFERENCES [dbo].[Collection] ([CollectionID])
GO
ALTER TABLE [dbo].[CollectionImage] CHECK CONSTRAINT [FK_CollectionImage_Collection]
GO
GRANT DELETE ON [dbo].[CollectionImage] TO [DiversityCollectionAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[CollectionImage] TO [DiversityCollectionAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionImage] TO [DiversityCollectionAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CollectionImage] TO [DiversityCollectionAdministrator] AS [dbo]
GO
GRANT DELETE ON [dbo].[CollectionImage] TO [DiversityCollectionManager] AS [dbo]
GO
GRANT INSERT ON [dbo].[CollectionImage] TO [DiversityCollectionManager] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionImage] TO [DiversityCollectionManager] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CollectionImage] TO [DiversityCollectionManager] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionImage] TO [DiversityCollectionUser] AS [dbo]
GO
GRANT INSERT ON [dbo].[CollectionImage_log] TO [DiversityCollectionAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionImage_log] TO [DiversityCollectionAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[CollectionImage_log] TO [DiversityCollectionManager] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionImage_log] TO [DiversityCollectionManager] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollectionImage_log] TO [DiversityCollectionUser] AS [dbo]
GO


--#####################################################################################################################
--######   fossil material categories   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
GO

update m set [DisplayEnable] = 1, Code = 'fossil specimen', Description = 'fossil specimen', DisplayText = 'fossil specimen' 
  FROM [dbo].[CollMaterialCategory_Enum] m
where Code = 'fossile specimen'
GO



INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil scales'
           ,'fossil scales of fish'
           ,'fossil scales'
           ,710
           ,1
           ,'fossil specimen')
GO


INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil shell'
           ,'fossil shell of animal'
           ,'fossil shell'
           ,711
           ,1
           ,'fossil specimen')
GO

INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('trace fossil'
           ,'trace fossil of animal'
           ,'trace fossil'
           ,712
           ,1
           ,'fossil specimen')
GO


INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('mould'
           ,'mould of animal'
           ,'mould'
           ,713
           ,1
           ,'fossil specimen')
GO



INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil bones'
           ,'fossil bones or skeleton from vertebrates'
           ,'fossil bones'
           ,700
           ,1
           ,'fossil specimen')
GO

INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil skull'
           ,'fossil skull of a vertrebrate'
           ,'fossil skull'
           ,701
           ,1
           ,'fossil bones')
GO

INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil complete skeleton'
           ,'fossil complete skeleton of a vertrebrate'
           ,'fossil complete skeleton'
           ,702
           ,1
           ,'fossil bones')
GO


INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil postcranial skeleton'
           ,'fossil postcranial skeleton of a vertrebrate'
           ,'fossil postcranial skeleton'
           ,703
           ,1
           ,'fossil bones')
GO


INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil incomplete skeleton'
           ,'fossil incomplete skeleton of a vertrebrate'
           ,'fossil incomplete skeleton'
           ,705
           ,1
           ,'fossil bones')
GO




INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil single bones'
           ,'fossil single bones of a vertrebrate'
           ,'fossil single bones'
           ,706
           ,1
           ,'fossil bones')
GO


INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil tooth'
           ,'fossil tooth of an animal'
           ,'fossil tooth'
           ,706
           ,1
           ,'fossil bones')
GO



INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('fossil otolith'
           ,'fossil otolith of fishes'
           ,'fossil otolith'
           ,708
           ,1
           ,'fossil bones')
GO








--#####################################################################################################################
--######   [FirstLinesSeries]   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
GO



ALTER FUNCTION [dbo].[FirstLinesSeries] 
(@CollectionSpecimenIDs varchar(8000))   
RETURNS @List TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   Description nvarchar(500) NULL,
   SeriesCode nvarchar(50) NULL,
   [Geography] geography,
   Notes nvarchar(500) NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
MW 18.11.2009 
TEST: 
Select * from dbo.FirstLinesSeries('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select * from dbo.FirstLinesSeries('107487, 107489, 107504, 107506, 98644')
Select * from dbo.FirstLinesSeries('98644')
Select * from dbo.FirstLinesSeries('98637')
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

-- EventID
declare @EventID TABLE (CollectionEventID int primary key)
INSERT INTO @EventID (CollectionEventID)
SELECT DISTINCT S.CollectionEventID
FROM CollectionSpecimen S, @IDs ID
WHERE NOT CollectionEventID IS NULL
AND S.CollectionSpecimenID = ID.ID 

-- SeriesID
declare @SeriesID TABLE (SeriesID int primary key)

-- the direct series of the collection event
INSERT INTO @SeriesID (SeriesID)
SELECT E.SeriesID
FROM CollectionEvent E, @EventID ID
WHERE ID.CollectionEventID = E.CollectionEventID
AND NOT E.SeriesID IS NULL
GROUP BY E.SeriesID


-- the parents of the series
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)



INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesParentID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesID
AND NOT S.SeriesParentID IS NULL
AND S.SeriesParentID NOT IN (SELECT SeriesID FROM @SeriesID)

-- the child series of the result set
INSERT INTO @SeriesID (SeriesID)
SELECT S.SeriesID
FROM CollectionEventSeries S, @SeriesID ID
WHERE ID.SeriesID = S.SeriesParentID
AND NOT S.SeriesID IS NULL
AND S.SeriesID NOT IN (SELECT SeriesID FROM @SeriesID)


--- Series
insert into @List (
SeriesID, 
SeriesParentID, 
Description, 
SeriesCode, 
Notes, 
Geography, 
DateStart, 
DateEnd
)
SELECT     
S.SeriesID, 
SeriesParentID, 
Description, 
SeriesCode, 
Notes, 
Geography, 
DateStart, 
DateEnd
FROM  CollectionEventSeries S, @SeriesID ID
where S.SeriesID = ID.SeriesID 


RETURN 
END   

GO

--#####################################################################################################################
--######   Korrektur [FirstLinesEvent]   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
GO



ALTER FUNCTION [dbo].[FirstLinesEvent] 
(@CollectionSpecimenIDs varchar(8000))   
RETURNS @List TABLE (
	[CollectionEventID] [int] Primary key, --
	--[Accession_number] [nvarchar](50) NULL, --
-- WITHHOLDINGREASONS
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, --
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
--Hidden fields
	[_CollectionEventID] [int] NULL, --
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
	[_AverageAltitudeCache] [real] NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
MW 18.11.2009 
TEST: 
Select * from dbo.FirstLinesEvent('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select * from dbo.FirstLinesEvent('3251, 3252')
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





--- Event
insert into @List (
  CollectionEventID
, Collection_day
, Collection_month
, Collection_year
, Collection_date_supplement
, Collection_time
, Collection_time_span
, Country
, Locality_description
, Habitat_description
, Collecting_method
, Collection_event_notes
, Data_withholding_reason_for_collection_event
, Collectors_event_number
)
select 
  CollectionEventID
, CollectionDay
, CollectionMonth
, CollectionYear
, CollectionDateSupplement
, CollectionTime
, CollectionTimeSpan
, CountryCache
, LocalityDescription
, HabitatDescription
, CollectingMethod
, Notes
, DataWithholdingReason
, CollectorsEventNumber
from dbo.CollectionEvent E
where CollectionEventID in 
(select CollectionEventID from CollectionSpecimen 
where CollectionSpecimenID in( select ID from @IDs )
and not CollectionEventID is null)  




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
where L.CollectionEventID = E.CollectionEventID
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
where L.CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 8


--- Altitude

update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L.CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4



--- MTB

update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L.CollectionEventID = E.CollectionEventID
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
where L.CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 13



--- GeographicRegions

update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L.CollectionEventID = P.CollectionEventID
and P.PropertyID = 10


--- Lithostratigraphy

update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L.CollectionEventID = P.CollectionEventID
and P.PropertyID = 30



--- Chronostratigraphy

update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L.CollectionEventID = P.CollectionEventID
and P.PropertyID = 20
	
                  
RETURN 
END   

GO





--#####################################################################################################################
--######   Korrektur [EventSeriesHierarchy]   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
GO



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
Test
SELECT * FROM  dbo.EventSeriesHierarchy(-1733)
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.EventSeriesTopID(@SeriesID) )

declare @List TABLE (SeriesID int primary key,
   SeriesParentID int NULL)
   
-- inserting the start values  
	INSERT @List (SeriesID) Values(@TopID)
	INSERT @List (SeriesID, SeriesParentID) SELECT SeriesID, SeriesParentID FROM dbo.EventSeriesChildNodes (@TopID)
	
-- getting the whole hierarchy	
	set @i = (select COUNT(*) from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P))
	while @i > 0
	begin
		INSERT @List (SeriesID, SeriesParentID) 
			SELECT E.SeriesID, E.SeriesParentID from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P)
		set @i = (select COUNT(*) from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P))
	end

	INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes)
	SELECT E.SeriesID, E.SeriesParentID, DateStart, DateEnd, SeriesCode, [Description], Notes
	FROM CollectionEventSeries E, @List L where L.SeriesID = E.SeriesID
   
-- set the geography
	UPDATE L SET [Geography] = E.[Geography]
	FROM @EventSeriesList L, CollectionEventSeries E
	WHERE E.SeriesID = L.SeriesID

   RETURN
END
GO


--#####################################################################################################################
--######   Processing   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT

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



CREATE FUNCTION [dbo].[ProcessingProjectList] (@ProjectID int)   
RETURNS @ProcessingList TABLE ([ProcessingID] [int] Primary key , 	
[ProcessingParentID] [int] NULL , 	
[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL , 	
[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL , 	
[ProcessingURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
[OnlyHierarchy] [bit] NULL,
[RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)  
/* 
Returns a table that lists all the Processing items related to the given project. 
MW 08.08.2009 
TEST: 
Select * from ProcessingProjectList(3)  
Select * from ProcessingProjectList(372)  
*/ 
AS BEGIN  
--ALTER TABLE @ProcessingList ADD  CONSTRAINT [DF__Processing__RowGUI__29A2D696]  DEFAULT (newsequentialid()) FOR [RowGUID]

INSERT INTO @ProcessingList            
([ProcessingID]            
,[ProcessingParentID]            
,[DisplayText]            
,[Description]            
,[Notes]            
,[ProcessingURI]
,[OnlyHierarchy]
,[RowGUID]) 
SELECT Processing.ProcessingID, Processing.ProcessingParentID, Processing.DisplayText, Processing.Description, 
Processing.Notes,  Processing.ProcessingURI, Processing.OnlyHierarchy, Processing.RowGUID
FROM  ProjectProcessing 
INNER JOIN Processing ON ProjectProcessing.ProcessingID = Processing.ProcessingID 
WHERE ProjectProcessing.ProjectID = @ProjectID  

DECLARE @TempItem TABLE (ProcessingID int primary key) 

INSERT INTO @TempItem ([ProcessingID]) 
SELECT Processing.ProcessingID 
FROM  ProjectProcessing 
INNER JOIN Processing ON ProjectProcessing.ProcessingID = Processing.ProcessingID 
WHERE ProjectProcessing.ProjectID = @ProjectID  
declare @ParentID int  
DECLARE HierarchyCursor  CURSOR for 	select ProcessingID from @TempItem 	
open HierarchyCursor 	
FETCH next from HierarchyCursor into @ParentID 	
WHILE @@FETCH_STATUS = 0 	
BEGIN 	
insert into @ProcessingList ( ProcessingID , ProcessingParentID, DisplayText , Description , 
Notes , ProcessingURI, OnlyHierarchy, RowGUID) 
select ProcessingID , ProcessingParentID, DisplayText , Description ,  
Notes , ProcessingURI, OnlyHierarchy, RowGUID
from dbo.ProcessingChildNodes (@ParentID) 
where ProcessingID not in (select ProcessingID from @ProcessingList) 	
FETCH NEXT FROM HierarchyCursor into @ParentID 	END 
CLOSE HierarchyCursor 
DEALLOCATE HierarchyCursor  
--DELETE FROM  @ProcessingList WHERE OnlyHierarchy = 1  
RETURN 
END  


GO


GRANT SELECT ON [dbo].[ProcessingProjectList] TO [DiversityCollectionUser] AS [dbo]
GO


--#####################################################################################################################
--######   Label in Feldbeschreibung als ImageType loeschen   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO

EXEC sys.sp_updateextendedproperty  @name=N'MS_Description', @value=N'Type of the image, e.g. photograph' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'ImageType'
GO


--#####################################################################################################################
--######   Korrektur FirstLines   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO


ALTER FUNCTION [dbo].[FirstLines_2] 
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
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, --
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
	[Determiner_of_second_organism] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_determiner_of_second_organism] [int] NULL,
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
Select * from dbo.FirstLines_2('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select * from dbo.FirstLines_2('3251, 3252')
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
, L.Notes_for_Altitude = E.LocationNotes
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
--######   AverageAltitude   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO



CREATE FUNCTION AverageAltitude 
(
	@Geography geography
)
RETURNS float
AS
BEGIN
	/*
	Test:
	DECLARE @g geography;
	SET @g = geography::STGeomFromText('LINESTRING(-122.360 47.656, -122.343 47.656, -122.373 47.686 )', 4326);
	SET @g = geography::STGeomFromText('LINESTRING(-122.360 47.656 50, -122.343 47.656 60, -122.373 47.686 )', 4326);
	SELECT dbo.AverageAltitude(@g);
	*/
	-- Declare the return variable
	DECLARE @Alt float;
	
	SET @Alt = 0;
	DECLARE @AltPoint float;
	DECLARE @AltPoints int;
	SET @AltPoints = 0;
	DECLARE @Points int;
	SET @Points = @Geography.STNumPoints();
	DECLARE @i int;
	SET @i = 1;
	WHILE @i <= @Points 
	BEGIN
		SET @AltPoint = (SELECT @Geography.STPointN(@i).Z);
		IF NOT (SELECT @AltPoint) IS NULL
		BEGIN
			SET @Alt = @Alt + @AltPoint;
			SET @AltPoints = @AltPoints + 1;
		END
		SET @i = @i + 1;
	END
	IF @AltPoints > 0
	BEGIN
		SET @Alt = (SELECT @Alt / @AltPoints)
	END
	ELSE
	BEGIN
		SET @Alt = NULL
	END
	
	-- Return the result of the function
	RETURN @Alt

END
GO

--#####################################################################################################################
--######   tissue sample   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO

if (SELECT COUNT(*)
  FROM [dbo].[CollMaterialCategory_Enum]
M where m.Code = 'tissue sample') = 0
begin

INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('tissue sample'
           ,'tissue sample'
           ,'tissue sample'
           ,95
           ,1
           ,'other specimen')
end
go



--#####################################################################################################################
--######   arthropod statt arthropode   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO

UPDATE t SET T.ParentCode = null
FROM    CollTaxonomicGroup_Enum AS t
WHERE     (t.code = 'insect')
go


--SELECT     Code, DisplayText
UPDATE t SET T.Code = 'arthropod', T.DisplayText = 'arthropod'
FROM    CollTaxonomicGroup_Enum AS t
WHERE     (Code = 'arthropode')
go

UPDATE t SET T.ParentCode = 'arthropod'
FROM    CollTaxonomicGroup_Enum AS t
WHERE     (t.code = 'insect')
go

--#####################################################################################################################
--######   Observations   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO



update m set [DisplayEnable] = 0
--select * 
  FROM [DiversityCollection_TestPart].[dbo].[CollMaterialCategory_Enum]
M where m.Code like '%observation'
GO


--#####################################################################################################################
--######   Korrektur ProcessingHierarchy  ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO


/****** Object:  UserDefinedFunction [dbo].[ProcessingHierarchy]    Script Date: 10/21/2009 15:13:52 ******/
ALTER FUNCTION [dbo].[ProcessingHierarchy] (@ID int)  
RETURNS @List TABLE ([ProcessingID] [int] Primary key ,
	[ProcessingParentID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[ProcessingURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[OnlyHierarchy] [bit] NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  

/*
Returns a table that lists all the analysis items related to the given processing.
MW 21.10.2009
TEST:
SELECT * FROM dbo.ProcessingHierarchy(5)
*/
AS
BEGIN
-- getting the TopID
declare @TopID int
declare @i int

set @TopID = (select ProcessingParentID from Processing where ProcessingID = @ID) 

set @i = (select count(*) from Processing where ProcessingID = @ID)

if (@TopID is null )
	set @TopID =  @ID
else	
	begin
	while (@i > 0)
		begin
		set @ID = (select ProcessingParentID from Processing where ProcessingID = @ID and not ProcessingParentID is null) 
		set @i = (select count(*) from Processing where ProcessingID = @ID and not ProcessingParentID is null)
		end
	set @TopID = @ID
	end


-- get the ID's of the child nodes
-- copy the root node in the result list
   INSERT @List
   SELECT DISTINCT ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI, OnlyHierarchy, RowGUID
   FROM Processing
   WHERE Processing.ProcessingID = @TopID

-- copy the child nodes into the result list
   INSERT @List
   SELECT * FROM dbo.ProcessingChildNodes (@TopID)

   RETURN
END
GO



--#####################################################################################################################
--######   Korrektur ProcessingListForPart   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO



ALTER FUNCTION [dbo].[ProcessingListForPart] (@CollectionSpecimenID int, @SpecimenPartID int)  
RETURNS @ProcessingList TABLE ([ProcessingID] [int]  Primary key,
	[ProcessingParentID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[ProcessingURI] [varchar](255) NULL,
	[DisplayTextHierarchy] nvarchar (255))

/*
Returns a table that lists all the processing items related to the given part.
the list depends upon the processing types available for a material category
and the projects available for a processing
MW 08.08.2009
TEST:
SELECT * FROM dbo.ProcessingListForPart(177930,  NULL)
SELECT * FROM dbo.ProcessingListForPart(177930, 153619)

*/
AS
BEGIN

DECLARE @ProcessingID int

-- PROJECTS

-- GET THE Processing ACCORDING TO PROJECTS
DECLARE @ProcessingProjectCursor TABLE (ProcessingID int primary key)

INSERT INTO @ProcessingProjectCursor (ProcessingID)
SELECT DISTINCT A.ProcessingID
FROM  CollectionProject AS P INNER JOIN
ProjectProcessing AS A ON P.ProjectID = A.ProjectID INNER JOIN
CollectionSpecimen AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID
--WHERE (U.SpecimenPartID = @SpecimenPartID)



-- GET THE Processing CHILDS ACCORDING TO PROJECTS
DECLARE @ProcessingProject TABLE (ProcessingID int primary key)

INSERT INTO @ProcessingProject (ProcessingID)
SELECT DISTINCT A.ProcessingID
FROM  CollectionProject AS P INNER JOIN
ProjectProcessing AS A ON P.ProjectID = A.ProjectID INNER JOIN
CollectionSpecimenPart AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID

DECLARE ProcessingProjectCursor  CURSOR FOR
	SELECT ProcessingID FROM @ProcessingProjectCursor
	OPEN ProcessingProjectCursor
	FETCH next from ProcessingProjectCursor into @ProcessingID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO @ProcessingProject (ProcessingID)
		SELECT DISTINCT ProcessingID
		FROM ProcessingHierarchy(@ProcessingID) C
		WHERE C.ProcessingID NOT IN (SELECT ProcessingID FROM @ProcessingProject)

		INSERT INTO @ProcessingProject (ProcessingID)
		SELECT DISTINCT ProcessingID
		FROM ProcessingChildNodes(@ProcessingID) C
		WHERE C.ProcessingID NOT IN (SELECT ProcessingID FROM @ProcessingProject)
		FETCH NEXT FROM ProcessingProjectCursor INTO @ProcessingID
	END
CLOSE ProcessingProjectCursor
DEALLOCATE ProcessingProjectCursor




-- Material

-- GET THE PROCESSING ACCORDING TO MATERIAL
DECLARE @ProcessingMaterialCursor TABLE (ProcessingID int primary key)

IF NOT @SpecimenPartID IS NULL
BEGIN
INSERT INTO @ProcessingMaterialCursor (ProcessingID)
SELECT DISTINCT  A.ProcessingID
FROM CollectionSpecimenPart AS U INNER JOIN
ProcessingMaterialCategory AS T ON U.MaterialCategory = T.MaterialCategory INNER JOIN
Processing AS A ON T.ProcessingID = A.ProcessingID
WHERE (U.SpecimenPartID = @SpecimenPartID)
AND (U.CollectionSpecimenID = @CollectionSpecimenID)
END

IF @SpecimenPartID IS NULL
BEGIN
INSERT INTO @ProcessingMaterialCursor (ProcessingID)
SELECT DISTINCT  A.ProcessingID
FROM CollectionSpecimenPart AS U INNER JOIN
ProcessingMaterialCategory AS T ON U.MaterialCategory = T.MaterialCategory INNER JOIN
Processing AS A ON T.ProcessingID = A.ProcessingID
WHERE (U.CollectionSpecimenID = @CollectionSpecimenID)
END


-- GET THE Processing CHILDS ACCORDING TO Material
DECLARE @ProcessingMaterial TABLE (ProcessingID int primary key)

IF NOT @SpecimenPartID IS NULL
BEGIN
INSERT INTO @ProcessingMaterial (ProcessingID)
SELECT DISTINCT  A.ProcessingID
FROM CollectionSpecimenPart AS U INNER JOIN
ProcessingMaterialCategory AS T ON U.MaterialCategory = T.MaterialCategory INNER JOIN
Processing AS A ON T.ProcessingID = A.ProcessingID
WHERE (U.SpecimenPartID = @SpecimenPartID)
AND (U.CollectionSpecimenID = @CollectionSpecimenID)
END

IF @SpecimenPartID IS NULL
BEGIN
INSERT INTO @ProcessingMaterial (ProcessingID)
SELECT DISTINCT  A.ProcessingID
FROM CollectionSpecimenPart AS U INNER JOIN
ProcessingMaterialCategory AS T ON U.MaterialCategory = T.MaterialCategory INNER JOIN
Processing AS A ON T.ProcessingID = A.ProcessingID
WHERE (U.CollectionSpecimenID = @CollectionSpecimenID)
END

DECLARE ProcessingMaterialCursor  CURSOR FOR
	SELECT ProcessingID FROM @ProcessingMaterialCursor
	OPEN ProcessingMaterialCursor
	FETCH next from ProcessingMaterialCursor into @ProcessingID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO @ProcessingMaterial (ProcessingID)
		SELECT DISTINCT ProcessingID
		FROM ProcessingHierarchy(@ProcessingID) C
		WHERE C.ProcessingID NOT IN (SELECT ProcessingID FROM @ProcessingMaterial)

		INSERT INTO @ProcessingMaterial (ProcessingID)
		SELECT DISTINCT ProcessingID
		FROM ProcessingChildNodes(@ProcessingID) C
		WHERE C.ProcessingID NOT IN (SELECT ProcessingID FROM @ProcessingMaterial)
		
		FETCH NEXT FROM ProcessingMaterialCursor INTO @ProcessingID
	END
CLOSE ProcessingMaterialCursor
DEALLOCATE ProcessingMaterialCursor

INSERT INTO @ProcessingList
           ([ProcessingID]
           ,[ProcessingParentID]
           ,[DisplayText]
           ,[Description]
           ,[Notes]
           ,[ProcessingURI]
           ,[DisplayTextHierarchy])
SELECT A.ProcessingID, A.ProcessingParentID, A.DisplayText, A.Description, A.Notes, A.ProcessingURI, 
	CASE WHEN Ap3.DisplayText IS NULL 
	THEN '' ELSE Ap3.DisplayText + ' - ' END + CASE WHEN Ap2.DisplayText IS NULL 
	THEN '' ELSE Ap2.DisplayText + ' - ' END + CASE WHEN Ap1.DisplayText IS NULL 
	THEN '' ELSE Ap1.DisplayText + ' - ' END + A.DisplayText 
	AS DisplayTextHierarchy
FROM Processing AS Ap3 RIGHT OUTER JOIN Processing AS Ap2 ON Ap3.ProcessingID = Ap2.ProcessingParentID RIGHT OUTER JOIN
	Processing AS Ap1 ON Ap2.ProcessingID = Ap1.ProcessingParentID RIGHT OUTER JOIN
	Processing AS A  ON Ap1.ProcessingID = A.ProcessingParentID
	INNER JOIN @ProcessingMaterial AS T ON A.ProcessingID = T.ProcessingID 
	INNER JOIN @ProcessingProject AS P ON A.ProcessingID = P.ProcessingID

   RETURN
END
GO



--#####################################################################################################################
--######   DiversityMobile   ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
GO

ALTER TABLE UserProxy ADD Settings [xml] NULL
GO


--#####################################################################################################################
--######   DiversityMobile   ######################################################################################
--#####################################################################################################################


CREATE    FUNCTION [dbo].[DiversityMobile_TaxonomicGroups] ()  
RETURNS @TaxonomicGroups TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the taxonomic groups as defined in the settings related to the given project as stored in DiversityProjects.
MW 20.01.2010

Test:
select * from [dbo].[DiversityMobile_TaxonomicGroups] () 
 */
AS
BEGIN
-- filling the table
if (select U.Settings.query('/Settings/DiversityMobile/TaxonomicGroups') from UserProxy U where U.LoginName = USER_NAME()) is null
or (select cast(U.Settings.query('data(/Settings/DiversityMobile/TaxonomicGroups)') as nvarchar(50)) from UserProxy U where U.LoginName = USER_NAME()) = ''
begin
	INSERT INTO @TaxonomicGroups ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollTaxonomicGroup_Enum]
	t where t.DisplayEnable = 1
end 
else
begin
	declare @TaxGroupVisibile varchar(500);
	set @TaxGroupVisibile = (
	select cast(U.Settings.query('data(/Settings/DiversityMobile/TaxonomicGroups)') as nvarchar(50))
		from UserProxy U
		where not U.Settings.query('/Settings/DiversityMobile/TaxonomicGroups') is null
		and U.LoginName = USER_NAME()
	)
	declare @Code nvarchar(50)
	declare @DisplayText nvarchar(50)
	while (len(@TaxGroupVisibile) > 0)
	begin
		if (select CHARINDEX('|',@TaxGroupVisibile)) > 0
		begin
			set @Code = (select SUBSTRING(@TaxGroupVisibile, 1, CHARINDEX('|',@TaxGroupVisibile) - 1 ))
		end
		else
		begin
			set @Code = @TaxGroupVisibile
		end
		set @DisplayText = (select DisplayText from CollTaxonomicGroup_Enum e where e.Code = @Code)
		INSERT INTO @TaxonomicGroups (Code, DisplayText) values (@Code, @DisplayText)
		if (select CHARINDEX('|',@TaxGroupVisibile)) > 0
		begin
			set @TaxGroupVisibile = rtrim((select SUBSTRING(@TaxGroupVisibile, CHARINDEX('|',@TaxGroupVisibile)+1, 500)))
		end
		else 
		begin
			set @TaxGroupVisibile = ''
		end
	end
end
   RETURN
END

GRANT SELECT ON [dbo].[DiversityMobile_TaxonomicGroups] TO [DiversityCollectionUser] AS [dbo]
GO






CREATE  FUNCTION [dbo].[DiversityMobile_MaterialCategories] ()  
RETURNS @MaterialCategories TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the taxonomic groups as defined in the settings related to the given project as stored in DiversityProjects.
MW 20.01.2010

Test:
select * from [dbo].[DiversityMobile_MaterialCategories] () 
 */
AS
BEGIN
-- filling the table
if (select U.Settings.query('/Settings/DiversityMobile/MaterialCategories') from UserProxy U where U.LoginName = USER_NAME()) is null
or (select cast(U.Settings.query('data(/Settings/DiversityMobile/MaterialCategories)') as nvarchar(50)) from UserProxy U where U.LoginName = USER_NAME()) = ''
begin
	INSERT INTO @MaterialCategories ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollMaterialCategory_Enum]
	t where t.DisplayEnable = 1
end 
else
begin
	declare @GroupVisibile varchar(500);
	set @GroupVisibile = (
	select cast(U.Settings.query('data(/Settings/DiversityMobile/MaterialCategories)') as nvarchar(50))
		from UserProxy U
		where not U.Settings.query('/Settings/DiversityMobile/MaterialCategories') is null
		and U.LoginName = USER_NAME()
	)
	declare @Code nvarchar(50)
	declare @DisplayText nvarchar(50)
	while (len(@GroupVisibile) > 0)
	begin
		if (select CHARINDEX('|',@GroupVisibile)) > 0
		begin
			set @Code = (select SUBSTRING(@GroupVisibile, 1, CHARINDEX('|',@GroupVisibile) - 1 ))
		end
		else
		begin
			set @Code = @GroupVisibile
		end
		set @DisplayText = (select DisplayText from CollMaterialCategory_Enum e where e.Code = @Code)
		INSERT INTO @MaterialCategories (Code, DisplayText) values (@Code, @DisplayText)
		if (select CHARINDEX('|',@GroupVisibile)) > 0
		begin
			set @GroupVisibile = rtrim((select SUBSTRING(@GroupVisibile, CHARINDEX('|',@GroupVisibile)+1, 500)))
		end
		else 
		begin
			set @GroupVisibile = ''
		end
	end
end
   RETURN
END
GO

GRANT SELECT ON [dbo].[DiversityMobile_MaterialCategories] TO [DiversityCollectionUser] AS [dbo]
GO



--#####################################################################################################################
--######   DiversityMobile   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[DiversityMobile_ProjectList] ()   
RETURNS @ProjectList TABLE ([ProjectID] [int] Primary key , 	
[DisplayText] [nvarchar] (50))  
/* 
Returns a table that lists all the project for a user
MW 08.08.2009 
TEST: 
Select * from [DiversityMobile_ProjectList]()
*/ 
AS BEGIN  

INSERT INTO @ProjectList            
([ProjectID]            
,[DisplayText]) 
SELECT  U.ProjectID ,   P.Project
FROM         ProjectUser AS U INNER JOIN
                      ProjectProxy AS P ON U.ProjectID = P.ProjectID
WHERE     (U.LoginName = USER_NAME())
ORDER BY P.Project

RETURN 
END  



GO

GRANT SELECT ON [dbo].[DiversityMobile_ProjectList] TO [DiversityCollectionUser] AS [dbo]
GO




CREATE FUNCTION [dbo].[DiversityMobile_UserInfo] ()   
RETURNS @UserInfo TABLE ([LoginName] [nvarchar] (50) Primary key , 	
[UserName] [nvarchar] (50),
[AgentUri] [varchar] (255))  
/* 
Returns a table that lists all the project for a user
MW 08.08.2009 
TEST: 
Select * from [DiversityMobile_UserInfo]()
*/ 
AS BEGIN  

INSERT INTO @UserInfo            
([LoginName]            
,[UserName]
,[AgentUri]) 
SELECT  U.LoginName,
case when U.CombinedNameCache like '%, % (%' then SUBSTRING(U.CombinedNameCache, 1, charindex(' ', U.CombinedNameCache ))
+  SUBSTRING(U.CombinedNameCache, charindex(' ', U.CombinedNameCache )+1, 1) + '.' else 
case when U.CombinedNameCache like '%, %' then SUBSTRING(U.CombinedNameCache, 1, charindex(' ', U.CombinedNameCache ))
+  SUBSTRING(U.CombinedNameCache, charindex(' ', U.CombinedNameCache )+1, 1) + '.' else U.CombinedNameCache end
end, 
u.AgentURI
FROM UserProxy AS U 
WHERE (U.LoginName = USER_NAME())

RETURN 
END  

GO

GRANT SELECT ON [dbo].[DiversityMobile_UserInfo] TO [DiversityCollectionUser] AS [dbo]
GO






CREATE  FUNCTION [dbo].[DiversityMobile_EventImageTypes] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_EventImageTypes] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollEventImageType_Enum]
	t where t.DisplayEnable = 1
	and t.Code not in ('image')
   RETURN
END
GO

GRANT SELECT ON [dbo].[DiversityMobile_EventImageTypes] TO [DiversityCollectionUser] AS [dbo]
GO





CREATE  FUNCTION [dbo].[DiversityMobile_IdentificationCategories] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_IdentificationCategories] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollIdentificationCategory_Enum]
	t where t.DisplayEnable = 1
	and t.Code in ('determination')
   RETURN
END
GO

GRANT SELECT ON [dbo].[DiversityMobile_IdentificationCategories] TO [DiversityCollectionUser] AS [dbo]
GO






CREATE  FUNCTION [dbo].[DiversityMobile_IdentificationQualifiers] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_IdentificationQualifiers] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollIdentificationQualifier_Enum]
	t where t.DisplayEnable = 1
	and t.Code not in ('sp. nov.')
   RETURN
END
GO

GRANT SELECT ON [dbo].[DiversityMobile_IdentificationQualifiers] TO [DiversityCollectionUser] AS [dbo]
GO









CREATE  FUNCTION [dbo].[DiversityMobile_SpecimenImageTypes] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_SpecimenImageTypes] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollSpecimenImageType_Enum]
	t where t.DisplayEnable = 1
	and t.Code not in ('drawing', 'image', 'label', 'SEM image','photography', 'TEM image')
   RETURN
END
GO

GRANT SELECT ON [dbo].[DiversityMobile_SpecimenImageTypes] TO [DiversityCollectionUser] AS [dbo]
GO








CREATE  FUNCTION [dbo].[DiversityMobile_UnitRelationTypes] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_UnitRelationTypes] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollUnitRelationType_Enum]
	t where t.DisplayEnable = 1
	and t.Code not in ('')
   RETURN
END
GO

GRANT SELECT ON [dbo].[DiversityMobile_UnitRelationTypes] TO [DiversityCollectionUser] AS [dbo]
GO






CREATE FUNCTION [dbo].[DiversityMobile_AnalysisTaxonomicGroupsForProject](@ProjectID int)  
RETURNS @AnalysisTaxonomicGroup TABLE ([AnalysisID] [int] NOT NULL,
	[TaxonomicGroup] [nvarchar](50) NOT NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)
/*
Returns the contents of the table AnalysisTaxonomicGroup used in a project including the whole hierarchy.
MW 15.07.2009
Test:
select * from [dbo].[AnalysisTaxonomicGroupForProject] (372)
*/
AS
BEGIN
	declare @Temp TABLE (AnalysisID int NOT NULL
	, TaxonomicGroup [nvarchar](50) NOT NULL
	, ID int Identity NOT NULL
	, [RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)
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
		INSERT INTO @Temp (AnalysisID, TaxonomicGroup, RowGUID)
		SELECT A.AnalysisID, @TaxonomicGroup, A.RowGUID
		FROM [DiversityCollection].[dbo].[AnalysisList] (@ProjectID, @TaxonomicGroup) L, Analysis A
		WHERE A.AnalysisID = L.AnalysisID AND A.OnlyHierarchy = 0
		DELETE FROM @TempID WHERE ID = @ID
	END

	insert @AnalysisTaxonomicGroup (AnalysisID, TaxonomicGroup, RowGUID)
	SELECT DISTINCT AnalysisID, TaxonomicGroup, RowGUID
	FROM @Temp
	WHERE NOT RowGUID IS NULL
	
	RETURN
END
GO

GRANT SELECT ON [dbo].[DiversityMobile_AnalysisTaxonomicGroupsForProject] TO [DiversityCollectionUser] AS [dbo]
GO








CREATE FUNCTION [dbo].[DiversityMobile_AnalysisProjectList] (@ProjectID int)   
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



GRANT SELECT ON [dbo].[DiversityMobile_AnalysisProjectList] TO [DiversityCollectionUser] AS [dbo]
GO





--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################




/*

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.12'
END

GO
*/

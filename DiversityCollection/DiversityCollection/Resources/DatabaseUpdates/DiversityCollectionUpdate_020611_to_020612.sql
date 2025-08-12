declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.11'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  CollectionSpecimenPartRegulation: Description of log columns  ###############################################
--#####################################################################################################################

if (SELECT count(*)
FROM sys.extended_properties AS ep
INNER JOIN  sys.tables AS t ON ep.major_id = t.object_id 
INNER JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id
WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = 'CollectionSpecimenPartRegulation' AND c.name = 'LogCreatedWhen') = 0
begin
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
end
GO

if (SELECT count(*)
FROM sys.extended_properties AS ep
INNER JOIN  sys.tables AS t ON ep.major_id = t.object_id 
INNER JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id
WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = 'CollectionSpecimenPartRegulation' AND c.name = 'LogCreatedBy') = 0
begin
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
end
GO

if (SELECT count(*)
FROM sys.extended_properties AS ep
INNER JOIN  sys.tables AS t ON ep.major_id = t.object_id 
INNER JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id
WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = 'CollectionSpecimenPartRegulation' AND c.name = 'LogUpdatedWhen') = 0
begin
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
end
GO

if (SELECT count(*)
FROM sys.extended_properties AS ep
INNER JOIN  sys.tables AS t ON ep.major_id = t.object_id 
INNER JOIN  sys.columns AS c ON ep.major_id = c.object_id  AND ep.minor_id = c.column_id
WHERE class = 1 AND ep.name = 'MS_Description' AND t.name = 'CollectionSpecimenPartRegulation' AND c.name = 'LogUpdatedBy') = 0
begin
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
end
GO


--#####################################################################################################################
--######  trgUpdIdentification - bugfix writing IdentificationDate  ###################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdIdentification] ON [dbo].[Identification] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* setting the version in the main table */ 
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)

/* updating the LastIdentificationCache in IdentificationUnit */
update IdentificationUnit
set LastIdentificationCache = 
case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end 
from IdentificationUnit, Identification a, inserted
where IdentificationUnit.CollectionSpecimenID = inserted.CollectionSpecimenID
and IdentificationUnit.IdentificationUnitID = inserted.IdentificationUnitID
and a.CollectionSpecimenID = inserted.CollectionSpecimenID
and a.IdentificationUnitID = inserted.IdentificationUnitID
and a.IdentificationSequence = 
(select max(b.IdentificationSequence) 
from Identification b
where b.CollectionSpecimenID = a.CollectionSpecimenID
and b.IdentificationUnitID = a.IdentificationUnitID
group by b.IdentificationUnitID, b.CollectionSpecimenID)
and (LastIdentificationCache is null or LastIdentificationCache <> case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end )

/* updating the logging columns */
Update Identification
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM Identification, deleted 
where 1 = 1 
AND Identification.CollectionSpecimenID = deleted.CollectionSpecimenID
AND Identification.IdentificationSequence = deleted.IdentificationSequence
AND Identification.IdentificationUnitID = deleted.IdentificationUnitID

if (select count(*) from deleted) = 1
begin
	declare @Date datetime
	set @Date = NULL
	begin try
		set @Date = (select convert(datetime, cast(I.[IdentificationYear] as varchar) + '-' + cast(I.[IdentificationMonth] as varchar) + '-' + cast(I.[IdentificationDay] as varchar), 120) 
		FROM Identification I, deleted D
		where 1 = 1 
		AND I.CollectionSpecimenID = D.CollectionSpecimenID
		AND I.IdentificationSequence = D.IdentificationSequence
		AND I.IdentificationUnitID = D.IdentificationUnitID
		AND NOT I.[IdentificationYear] IS NULL
		AND NOT I.[IdentificationMonth] IS NULL
		AND NOT I.[IdentificationDay] IS NULL
		)
	end try
	begin catch
		set @Date = NULL
	end catch

	Update Identification
	set IdentificationDate = @Date
	FROM Identification, deleted 
	where 1 = 1 
	AND Identification.CollectionSpecimenID = deleted.CollectionSpecimenID
	AND Identification.IdentificationSequence = deleted.IdentificationSequence
	AND Identification.IdentificationUnitID = deleted.IdentificationUnitID
end

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IdentificationSequence, deleted.IdentificationDate, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationYear, deleted.IdentificationDateSupplement, deleted.IdentificationDateCategory, deleted.VernacularTerm, deleted.TermUri, deleted.TaxonomicName, deleted.NameURI, deleted.IdentificationCategory, deleted.IdentificationQualifier, deleted.TypeStatus, deleted.TypeNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IdentificationSequence, deleted.IdentificationDate, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationYear, deleted.IdentificationDateSupplement, deleted.IdentificationDateCategory, deleted.VernacularTerm, deleted.TermUri, deleted.TaxonomicName, deleted.NameURI, deleted.IdentificationCategory, deleted.IdentificationQualifier, deleted.TypeStatus, deleted.TypeNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO

--#####################################################################################################################
--######   trgInsCollectionEventLocalisation: Round WGS84 to 6 digits   ###############################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgInsCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR INSERT AS
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int
set @i = (select count(*) from inserted) 
if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM inserted)
	DECLARE @Altitude float
	DECLARE @Latitude float
	DECLARE @Longitude float
	DECLARE @Geography geography
	DECLARE @ParsingMethod nvarchar(50)
	DECLARE @LocalisationSystemName nvarchar(100)
	SET @ParsingMethod = (SELECT LocalisationSystem.ParsingMethodName
	FROM inserted ,LocalisationSystem 
	WHERE inserted.LocalisationSystemID = LocalisationSystem.LocalisationSystemID
	AND inserted.CollectionEventID = @ID) 
	SET @LocalisationSystemName = (SELECT LocalisationSystem.LocalisationSystemName
	FROM inserted ,LocalisationSystem 
	WHERE inserted.LocalisationSystemID = LocalisationSystem.LocalisationSystemID
	AND inserted.CollectionEventID = @ID) 
	IF @ParsingMethod = 'Altitude'
	BEGIN TRY
		SET @Altitude = (SELECT CAST(REPLACE(Location1, ',', '.') AS FLOAT) FROM inserted WHERE NOT Location1 IS NULL AND ISNUMERIC(Location1) =  1 AND AverageAltitudeCache IS NULL)
		DECLARE @Alt2 float
		SET @Alt2 = (SELECT CAST(REPLACE(Location2, ',', '.') AS FLOAT) FROM inserted WHERE NOT Location2 IS NULL AND ISNUMERIC(Location2) =  1 AND AverageAltitudeCache IS NULL)
		IF (NOT @Alt2 IS NULL)
			BEGIN
			SET @Altitude = ((SELECT @Altitude + @Alt2) / 2)
			END
		IF (NOT @Altitude IS NULL)
			BEGIN 
			UPDATE CollectionEventLocalisation SET CollectionEventLocalisation.AverageAltitudeCache = @Altitude
			FROM CollectionEventLocalisation, inserted
			WHERE inserted.LocalisationSystemID = CollectionEventLocalisation.LocalisationSystemID
			AND CollectionEventLocalisation.CollectionEventID = @ID
			AND inserted.CollectionEventID = @ID
			END 
	END TRY
	BEGIN CATCH
	END CATCH
	IF @ParsingMethod = 'Coordinates'
	BEGIN TRY
		SET @Latitude = (SELECT CAST(CAST(REPLACE(Location2, ',', '.') AS decimal(10,6)) AS FLOAT) FROM inserted WHERE NOT Location2 IS NULL AND ISNUMERIC(Location2) =  1 AND AverageLatitudeCache IS NULL)
		SET @Longitude = (SELECT CAST(CAST(REPLACE(Location1, ',', '.') AS decimal(10,6)) AS FLOAT) FROM inserted WHERE NOT Location1 IS NULL AND ISNUMERIC(Location1) =  1 AND AverageLongitudeCache IS NULL)
		IF (NOT @Longitude IS NULL AND NOT @Latitude IS NULL)
			BEGIN
			UPDATE CollectionEventLocalisation SET 
			CollectionEventLocalisation.AverageLatitudeCache = @Latitude,
			CollectionEventLocalisation.AverageLongitudeCache = @Longitude
			FROM CollectionEventLocalisation, inserted
			WHERE inserted.LocalisationSystemID = CollectionEventLocalisation.LocalisationSystemID
			AND CollectionEventLocalisation.CollectionEventID = @ID
			AND inserted.CollectionEventID = @ID
			END
	END TRY
	BEGIN CATCH
	END CATCH
	IF @LocalisationSystemName = 'Coordinates WGS84'
	BEGIN TRY
		SET @Latitude = (SELECT CAST(CAST(REPLACE(Location2, ',', '.') AS decimal(10,6)) AS FLOAT) FROM inserted WHERE NOT Location2 IS NULL AND ISNUMERIC(Location2) =  1 AND AverageLatitudeCache IS NULL)
		SET @Longitude = (SELECT CAST(CAST(REPLACE(Location1, ',', '.') AS decimal(10,6)) AS FLOAT) FROM inserted WHERE NOT Location1 IS NULL AND ISNUMERIC(Location1) =  1 AND AverageLongitudeCache IS NULL)
		SET @Geography = geography::STPointFromText('POINT(' + CAST(@Longitude AS VARCHAR) + ' ' + CAST(@Latitude AS VARCHAR) + ')', 4326);
		IF (NOT @Geography IS NULL)
			BEGIN
			UPDATE CollectionEventLocalisation SET 
			CollectionEventLocalisation.Geography = @Geography
			FROM CollectionEventLocalisation, inserted
			WHERE inserted.LocalisationSystemID = CollectionEventLocalisation.LocalisationSystemID
			AND CollectionEventLocalisation.CollectionEventID = @ID
			AND inserted.CollectionEventID = @ID
			AND inserted.Geography IS NULL
			END
		SET @Latitude = (SELECT inserted.Geography.EnvelopeCenter().Lat FROM inserted WHERE Location2 IS NULL AND NOT inserted.Geography IS NULL AND AverageLatitudeCache IS NULL)
		SET @Longitude = (SELECT inserted.Geography.EnvelopeCenter().Long FROM inserted WHERE Location1 IS NULL AND NOT inserted.Geography IS NULL AND AverageLongitudeCache IS NULL)
		SET @Geography = (SELECT inserted.Geography FROM inserted);
		IF (NOT @Geography IS NULL 
		AND NOT @Latitude IS NULL
		AND NOT @Longitude IS NULL)
		BEGIN
			UPDATE CollectionEventLocalisation SET 
			CollectionEventLocalisation.Location1 = @Longitude,
			CollectionEventLocalisation.Location2 = @Latitude,
			CollectionEventLocalisation.AverageLongitudeCache = @Longitude,
			CollectionEventLocalisation.AverageLatitudeCache = @Latitude
			FROM CollectionEventLocalisation, inserted
			WHERE inserted.LocalisationSystemID = CollectionEventLocalisation.LocalisationSystemID
			AND CollectionEventLocalisation.CollectionEventID = @ID
			AND inserted.CollectionEventID = @ID
			AND NOT inserted.Geography IS NULL
		END
	END TRY
	BEGIN CATCH
	END CATCH
   EXECUTE procSetVersionCollectionEvent @ID
END
GO

--#####################################################################################################################
--######   FirstLinesUnit_3: Stock -> float   #########################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[FirstLinesUnit_3] 
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
	[Problems] [nvarchar](255) NULL, 
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
	[Reference_title] [nvarchar](255) NULL, 
	[Link_to_DiversityReferences] [varchar](255) NULL, 
	[Remove_link_for_reference] [int] NULL,
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
--######   FirstLines_3: Stock -> float   #############################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[FirstLines_3] 
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
	[Label_title] [nvarchar](255) NULL, --
	[Label_type] [nvarchar](50) NULL, --
	[Label_transcription_state] [nvarchar](50) NULL, --
	[Label_transcription_notes] [nvarchar](255) NULL, --
	[Problems] [nvarchar](255) NULL, --
	[External_datasource] [int] NULL, --
	[External_identifier] [nvarchar](100) NULL, --
	[Reference_title_for_specimen] [nvarchar](255) NULL, --
	[Link_to_DiversityReferences_for_specimen] [varchar](255) NULL, --
	[Remove_link_of_reference_for_specimen] [int] NULL,
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
	[Reference_title] [nvarchar](255) NULL, --
	[Link_to_DiversityReferences] [varchar](255) NULL, --
	[Remove_link_for_reference] [int] NULL,
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
	[Reference_title_of_second_organism] [nvarchar](255) NULL, --
	[Link_to_DiversityReferences_of_second_organism] [varchar](255) NULL, --
	[Remove_link_for_reference_of_second_organism] [int] NULL,
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
	--[On_loan] [int] NULL, --
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
, Reference_title_for_specimen
, Link_to_DiversityReferences_for_specimen
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
, ReferenceTitle
, ReferenceURI
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
declare @UnitIDs table (UnitID int  Primary key, ID int, DisplayOrder smallint, RelatedUnitID int)
insert into @AllUnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
select U.IdentificationUnitID, U.CollectionSpecimenID, U.DisplayOrder, U.RelatedUnitID
from IdentificationUnit as U, @IDs as IDs
where DisplayOrder > 0
and IDs.ID = U.CollectionSpecimenID 
insert into @UnitIDs (UnitID, ID, DisplayOrder, RelatedUnitID)
select U.UnitID, U.ID, U.DisplayOrder, U.RelatedUnitID
from @AllUnitIDs as U,  @AllUnitIDs as M
where U.ID = M.ID
group by M.ID, U.DisplayOrder, U.ID, U.RelatedUnitID, U.UnitID
having U.DisplayOrder = MIN(M.DisplayOrder)

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
--######   CollectionSpecimen_Core2: Use CollectionMonth instead of month(E.CollectionDate) etc.   ####################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionSpecimen_Core2]
AS
SELECT     S.CollectionSpecimenID, S.Version, S.CollectionEventID, S.CollectionID, S.AccessionNumber, S.AccessionDate, S.AccessionDay, S.AccessionMonth, S.AccessionYear, 
                      S.AccessionDateSupplement, S.AccessionDateCategory, S.DepositorsName, S.DepositorsAgentURI, S.DepositorsAccessionNumber, S.LabelTitle, S.LabelType, 
                      S.LabelTranscriptionState, S.LabelTranscriptionNotes, S.ExsiccataURI, S.ExsiccataAbbreviation, S.OriginalNotes, S.AdditionalNotes, S.ReferenceTitle, 
                      S.ReferenceURI, S.Problems, S.DataWithholdingReason, CASE WHEN E.CollectionDate IS NULL AND E.CollectionYear IS NULL AND E.CollectionMonth IS NULL AND 
                      E.CollectionDay IS NULL THEN NULL ELSE CASE WHEN E.CollectionDate IS NULL THEN CASE WHEN E.CollectionYear IS NULL 
                      THEN '----' ELSE CAST(E.CollectionYear AS varchar) END + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CASE WHEN E.CollectionMonth IS NULL 
                      THEN '--' ELSE CAST(E.CollectionMonth AS varchar) END + '/' + CASE WHEN E.CollectionDay IS NULL 
                      THEN '--' ELSE CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(E.CollectionDay AS varchar) END ELSE CAST(year(E.CollectionDate) AS varchar) 
                      + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CAST(CollectionMonth AS varchar) 
                      + '/' + CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(CollectionDay AS varchar) END END + CASE WHEN E.LocalityDescription IS NULL 
                      THEN '' ELSE '   ' + E.LocalityDescription END AS CollectionDate, E.LocalityDescription AS Locality, E.HabitatDescription AS Habitat, S.InternalNotes, 
                      S.ExternalDatasourceID, S.ExternalIdentifier, S.LogCreatedWhen, S.LogCreatedBy, S.LogUpdatedWhen, S.LogUpdatedBy
FROM         dbo.CollectionSpecimen AS S INNER JOIN
                      dbo.CollectionSpecimenID_Available AS UA ON S.CollectionSpecimenID = UA.CollectionSpecimenID LEFT OUTER JOIN
                      dbo.CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID

GO


--#####################################################################################################################
--######  Description for table Regulation  ###########################################################################
--#####################################################################################################################

BEGIN TRY;
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Regulations e.g. concerning the collection of specimens',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Regulation';
END TRY
BEGIN CATCH;
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Regulations e.g. concerning the collection of specimens',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Regulation';
END CATCH;
GO

--#####################################################################################################################
--######  Default for DataWithholdingReason - typing error fixed  #####################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimen DROP CONSTRAINT [DF_CollectionSpecimen_DataWithholdingReason] ;
ALTER TABLE CollectionSpecimen ADD  CONSTRAINT [DF_CollectionSpecimen_DataWithholdingReason] DEFAULT (N'Withhold by default') FOR DataWithholdingReason;
GO

--#####################################################################################################################
--######  MultiColumnQuery - including event and series images  #######################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[MultiColumnQuery]
(	
	@Target nvarchar(50), 
	@QueryString nvarchar(500)
)
RETURNS @ID table(CollectionSpecimenID int not null Primary Key)
AS
/*
Test
select CollectionSpecimenID from dbo.MultiColumnQuery('Identifier', 'SAPM-PI-00%')
select S.* from dbo.MultiColumnQuery('Identifier', 'SAPM-PI-00%') M, CollectionSpecimen S
where M.CollectionSpecimenID = S.CollectionSpecimenID
*/
begin
	--declare @TempID table(CollectionSpecimenID int not null)

if (@Target = 'Identifier')
	begin
	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimen S, CollectionEvent E
	WHERE S.CollectionEventID = E.CollectionEventID AND E.CollectorsEventNumber LIKE @QueryString
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimen 
	WHERE (AccessionNumber LIKE @QueryString OR DepositorsAccessionNumber LIKE @QueryString OR ExternalIdentifier LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionAgent 
	WHERE CollectorsNumber LIKE @QueryString
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimenPart 
	WHERE (StorageLocation LIKE @QueryString OR AccessionNumber LIKE @QueryString OR PartSublabel LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimenTransaction 
	WHERE AccessionNumber LIKE @QueryString
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimenRelation 
	WHERE (RelatedSpecimenURI LIKE @QueryString OR RelatedSpecimenDisplayText LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM Identification 
	WHERE (VernacularTerm LIKE @QueryString OR TaxonomicName LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM IdentificationUnit 
	WHERE (ExsiccataNumber LIKE @QueryString OR UnitIdentifier LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM IdentificationUnitAnalysis 
	WHERE AnalysisNumber LIKE @QueryString 
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	end

	if (@Target = 'Withhold')
	begin

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimen S, CollectionEvent E
	WHERE S.CollectionEventID = E.CollectionEventID AND E.DataWithholdingReason LIKE @QueryString
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionAgent 
	WHERE DataWithholdingReason LIKE @QueryString 
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimen_Core 
	WHERE DataWithholdingReason LIKE @QueryString 
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimenPart 
	WHERE DataWithholdingReason LIKE @QueryString 
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimenImage 
	WHERE DataWithholdingReason LIKE @QueryString 
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionEventImage E, CollectionSpecimen S
	WHERE S.CollectionEventID = E.CollectionEventID AND E.DataWithholdingReason LIKE @QueryString 
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionEventSeriesImage I, CollectionEvent E, CollectionSpecimen S
	WHERE S.CollectionEventID = E.CollectionEventID AND I.SeriesID = E.SeriesID AND I.DataWithholdingReason LIKE @QueryString 
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	end

	if (@Target = 'Notes')
	begin

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimen S, CollectionEvent E
	WHERE S.CollectionEventID = E.CollectionEventID AND E.Notes LIKE @QueryString
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionAgent 
	WHERE Notes LIKE @QueryString 
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimen_Core 
	WHERE (LabelTitle LIKE @QueryString OR LabelTranscriptionNotes LIKE @QueryString OR OriginalNotes LIKE @QueryString 
	OR AdditionalNotes LIKE @QueryString OR Problems LIKE @QueryString 	OR InternalNotes LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimenImage 
	WHERE (Notes LIKE @QueryString OR Notes LIKE @QueryString OR CopyrightStatement LIKE @QueryString 
	OR InternalNotes LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimenPart 
	WHERE (Notes LIKE @QueryString OR Notes LIKE @QueryString OR PreparationMethod LIKE @QueryString )
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM CollectionSpecimenRelation 
	WHERE (Notes LIKE @QueryString OR Notes LIKE @QueryString OR RelatedSpecimenDescription LIKE @QueryString )
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM Identification 
	WHERE (TypeNotes LIKE @QueryString OR Notes LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM IdentificationUnit 
	WHERE (Circumstances LIKE @QueryString OR UnitDescription LIKE @QueryString OR Notes LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM IdentificationUnitAnalysis 
	WHERE (Notes LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM IdentificationUnitGeoAnalysis 
	WHERE (Notes LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	INSERT INTO @ID (CollectionSpecimenID) 
	SELECT CollectionSpecimenID FROM IdentificationUnitInPart 
	WHERE (Description LIKE @QueryString)
	AND CollectionSpecimenID NOT IN (SELECT CollectionSpecimenID FROM @ID)
	GROUP BY CollectionSpecimenID

	end


RETURN 
end

GO


--#####################################################################################################################
--######  trgUpdIdentification - writing IdentificationDate - bugfix  #################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdIdentification] ON [dbo].[Identification] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* setting the version in the main table */ 
DECLARE @Version int
SET @Version = -1
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
	DECLARE @ID int
	SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
	EXECUTE procSetVersionCollectionSpecimen @ID
	SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
end 


/* updating the LastIdentificationCache in IdentificationUnit */
update IdentificationUnit
set LastIdentificationCache = 
case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end 
from IdentificationUnit, Identification a, inserted
where IdentificationUnit.CollectionSpecimenID = inserted.CollectionSpecimenID
and IdentificationUnit.IdentificationUnitID = inserted.IdentificationUnitID
and a.CollectionSpecimenID = inserted.CollectionSpecimenID
and a.IdentificationUnitID = inserted.IdentificationUnitID
and a.IdentificationSequence = 
(select max(b.IdentificationSequence) 
from Identification b
where b.CollectionSpecimenID = a.CollectionSpecimenID
and b.IdentificationUnitID = a.IdentificationUnitID
group by b.IdentificationUnitID, b.CollectionSpecimenID)
and (LastIdentificationCache is null or LastIdentificationCache <> case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end )

/* updating the logging columns and the IdentificationDate */
Update Identification
set LogUpdatedWhen = getdate()
, LogUpdatedBy = SYSTEM_USER
, IdentificationDate = case when I.[IdentificationYear] is null or I.[IdentificationMonth] is null or I.[IdentificationDay] is null
	OR I.IdentificationDay not between 1 and 31 OR I.IdentificationMonth not between 1 and 12 or I.IdentificationYear not between 1754 and year(getdate()) + 1 then NULL 
	else convert(datetime, cast(I.[IdentificationYear] as varchar) + '-' + cast(I.[IdentificationMonth] as varchar) + '-' + cast(I.[IdentificationDay] as varchar), 120) end
FROM Identification I, deleted D
where 1 = 1 
AND I.CollectionSpecimenID = D.CollectionSpecimenID
AND I.IdentificationSequence = D.IdentificationSequence
AND I.IdentificationUnitID = D.IdentificationUnitID

/* saving the original dataset in the logging table */ 
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT D.CollectionSpecimenID, D.IdentificationUnitID, D.IdentificationSequence, D.IdentificationDate, D.IdentificationDay, D.IdentificationMonth, D.IdentificationYear, D.IdentificationDateSupplement, D.IdentificationDateCategory, D.VernacularTerm, D.TermUri, D.TaxonomicName, D.NameURI, D.IdentificationCategory, D.IdentificationQualifier, D.TypeStatus, D.TypeNotes, D.ReferenceTitle, D.ReferenceURI, D.ReferenceDetails, D.Notes, D.ResponsibleName, D.ResponsibleAgentURI, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, @Version, 'U' 
FROM DELETED D

GO


--#####################################################################################################################
--######  NextFreeAccNumber   #########################################################################################
--######  Bugfix:  Including parts in search for next number   ########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[NextFreeAccNumber] (@AccessionNumber nvarchar(50), @IncludeSpecimen bit, @IncludePart bit)  
/*
returns next free accession number for parts or specimen
optionally including either parts or specimen
assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453
with a leading string and a numeric end
MW 05.09.2013
TEST:
select dbo.[NextFreeAccNumber] ('0033933', 1, 1)
select dbo.[NextFreeAccNumber] ('0033933', 0, 1)
select dbo.[NextFreeAccNumber] ('00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 0, 1)
select dbo.[NextFreeAccNumber] ('ZSM_DIP-000', 1, 1)
select dbo.[NextFreeAccNumber] ('1907/9', 1, 1)
select dbo.[NextFreeAccNumber] ('ZSM-MA-9', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0013622', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014900', 1, 1)
select dbo.[NextFreeAccNumber] ('MB-FA-000001', 1, 1) 
select dbo.[NextFreeAccNumber] ('MB-FA-000101', 1, 1) 
select dbo.[NextFreeAccNumber] ('MB-006118', 1, 0)
select dbo.[NextFreeAccNumber] ('SMNS-B-PH-2017/942', 1, 1) 
select dbo.[NextFreeAccNumber] ('SMNS-B-PH-2017/02196', 1, 1) 
select dbo.[NextFreeAccNumber] ('SMNK-ARA 08643', 1, 1)
select dbo.[NextFreeAccNumber] ('Test', 1, 1)
SELECT [dbo].[NextFreeAccNumber] ('P-001', 1, 1)
*/
RETURNS nvarchar (50)
AS
BEGIN 
-- declaration of variables
declare @NextAcc nvarchar(50)			-- the result of the function
	set @NextAcc = null
declare @Start int						-- the numeric starting value
declare @NumericStartString nvarchar(50)-- the string containing the numeric part of the accession number
declare @NumericStartLength int			-- the length of the numeric part of the accession number
declare @EndString nvarchar(50)			-- an end of the accession number that is not numeric
	set @EndString = ''
declare @Position tinyint				-- the starting position of the numeric part 
declare @LastNumber int					-- the last numeric value that has been generated for testing										
declare @T Table (ID int identity(1, 1),-- temporary table keeping the numbers
	NumericPart int NULL,
    NumericString nvarchar(50) NULL,
    AccessionNumberGenerated nvarchar(50) NULL,
    AccessionNumberInCollection nvarchar(50) NULL)
declare @LastAccessionNumber nvarchar(50)-- the last AccessionNumber that has been generated for testing
	set @LastAccessionNumber = @AccessionNumber

-- getting the starting parameters of the accession number
declare @Prefix nvarchar(50)
set @Position = len(@AccessionNumber) 
if (@AccessionNumber NOT LIKE '%[^0-9]%')
begin
	set @Prefix = substring(@AccessionNumber, 1, len(@AccessionNumber) - len(cast(cast(@AccessionNumber as int) as varchar)))
	set @Start = cast(@AccessionNumber as int)
end
else
begin
	if (substring(reverse(@AccessionNumber), 1, 1) NOT LIKE  '[0-9]')
	begin
		set @EndString = substring(reverse(@AccessionNumber), 1, 1)
		declare @EndPos int
		set @EndPos = LEN(@AccessionNumber) - 1
		while (substring(@AccessionNumber, @EndPos, 1) NOT LIKE  '[0-9]' AND @EndPos > 0)
		begin
			set @EndString = substring(@AccessionNumber, @EndPos, 1) + @EndString
			set @EndPos = @EndPos - 1
		end
	end
	if(@EndString <> '')
	begin
		set @Position = @Position - len(@EndString)
	end
	while (substring(@AccessionNumber, @Position, 1) LIKE '[0-9]')
	begin
		set @NumericStartString = substring(@AccessionNumber, @Position, len(@AccessionNumber) - len(@EndString) - len(@Prefix) + 1)
		set @Start = CAST(@NumericStartString as int)
		set @Position = @Position - 1
		set @Prefix = substring(@AccessionNumber, 1, @Position)
	end
end

set @NumericStartLength = len(@NumericStartString)

-- getting leeding 0
declare @Leeding0 varchar(50)
declare @iLeed int
set @iLeed = @Position + 1
set @Leeding0 = ''
while (substring(@AccessionNumber, @iLeed, 1) = '0')
begin
	set @Leeding0 = @Leeding0 + '0'
	set @iLeed = @iLeed + 1
end

declare @LengthNumericPart int
set @LengthNumericPart = len(@NumericStartString)

-- walk through the existing numbers and find a not used number
while @NextAcc is null
begin
	-- filling the temporary table
	if (@IncludeSpecimen = 1)
	begin    
		INSERT INTO @T (AccessionNumberInCollection)
		SELECT DISTINCT TOP 100  AccessionNumber 
		FROM CollectionSpecimen  
		WHERE AccessionNumber LIKE @Prefix + '%'
		AND AccessionNumber > @LastAccessionNumber
		AND isnumeric(SUBSTRING(AccessionNumber, len(@Prefix) + 1, 50)) = 1
	end
	if (@IncludePart = 1)
	begin    
		INSERT INTO @T (AccessionNumberInCollection)
		SELECT DISTINCT TOP 100 AccessionNumber 
		FROM CollectionSpecimenPart  
		WHERE AccessionNumber LIKE @Prefix + '%'
		AND AccessionNumber > @LastAccessionNumber
		AND isnumeric(SUBSTRING(AccessionNumber, len(@Prefix) + 1, 50)) = 1
	end
	if (select COUNT(*) from @T) = 0
	begin
		INSERT INTO @T (AccessionNumberInCollection)
		SELECT @LastAccessionNumber 
	end

	-- setting the numbers
	UPDATE @T SET NumericPart = ID + @Start; 
	UPDATE @T SET NumericString = @Leeding0 + cast(NumericPart as varchar);

	-- if the with is restricted, shorten it
	if (@Leeding0 <> '')
	begin
		UPDATE @T SET NumericString = substring(NumericString, len(NumericString) - @LengthNumericPart + 1, @LengthNumericPart)
		WHERE len(NumericString) > @LengthNumericPart;
	end

	-- set the generated numbers
	UPDATE @T SET AccessionNumberGenerated = @Prefix + NumericString;

	-- start for the next circle
	set @LastAccessionNumber = (select max(AccessionNumberGenerated) from @T)
	set @LastNumber = (select max(NumericPart) from @T)

	-- remove those that are fitting
	if (select count(*) from @T) > 1
	begin
		delete from @T where AccessionNumberGenerated = rtrim(ltrim(AccessionNumberInCollection))
	end
	-- remove matches in the database
	if (select count(*) from @T) > 0
	begin
		if @IncludeSpecimen = 1
		begin
		Delete T from @T T inner join CollectionSpecimen S on S.AccessionNumber = T.AccessionNumberGenerated
		end
		if @IncludePart = 1
		begin
		Delete T from @T T inner join CollectionSpecimenPart S on S.AccessionNumber = T.AccessionNumberGenerated
		end
	end

	if (select count(*) from @T) > 0
	begin
		set @NextAcc = (select min(AccessionNumberGenerated) + @EndString from @T)
	end

	-- if nothing is left, exit the while loop
	if (select count(*) from CollectionSpecimen S where S.AccessionNumber > @LastAccessionNumber) = 0
	begin
		if (@NextAcc is null)
		begin
			set @NextAcc = ''
		end
	end

end

if (@NextAcc = '' AND not @LastNumber is null)
begin
	-- construction of the numeric part
	set @NextAcc = @Leeding0 + cast(@LastNumber + 1 as varchar)
	if (Len(@Leeding0) > 0 and len(@NextAcc) > @NumericStartLength)
		set @NextAcc = substring(@NextAcc, len(@NextAcc) - @NumericStartLength, 50)
	set @NextAcc = @Prefix + @NextAcc + @EndString
end

return (@NextAcc)
END
GO


--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.08' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.12'
END

GO


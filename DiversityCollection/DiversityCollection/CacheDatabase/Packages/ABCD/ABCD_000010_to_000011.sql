--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 11
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   abcd__unit_gathering: 
--######   Provide empty string in "DateTime_ISODateTimeBegin" if year is missing
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__unit_gathering(
	)
RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- cleaning table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering";

-- removing key
ALTER TABLE "#project#"."ABCD_Unit_Gathering" DROP CONSTRAINT IF EXISTS "ABCD_Unit_Gathering_pkey";

-- insert data
INSERT INTO "#project#"."ABCD_Unit_Gathering"(
            "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
            "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
            "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum")
SELECT DISTINCT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    e."CountryCache" AS "Country_Name",
    CASE
    -- Full date: Year, Month, and Day
    WHEN e."CollectionYear" IS NOT NULL AND e."CollectionMonth" IS NOT NULL AND e."CollectionDay" IS NOT NULL THEN
        concat_ws('-',
            e."CollectionYear"::text,
            lpad(e."CollectionMonth"::text, 2, '0'),
            lpad(e."CollectionDay"::text, 2, '0')
        )::text
    -- Year and Month only
    WHEN e."CollectionYear" IS NOT NULL AND e."CollectionMonth" IS NOT NULL THEN
        concat_ws('-',
            e."CollectionYear"::text,
            lpad(e."CollectionMonth"::text, 2, '0')
        )::text
    -- Year only
    WHEN e."CollectionYear" IS NOT NULL THEN
        e."CollectionYear"::text
    -- Month and Day only (custom format: --MM-DD)
    WHEN e."CollectionMonth" IS NOT NULL AND e."CollectionDay" IS NOT NULL THEN
    concat(
        '--',
        lpad(e."CollectionMonth"::text, 2, '0'),
        '-',
        lpad(e."CollectionDay"::text, 2, '0')
    )::text
    -- Only Month (ISO 8601 workaround: --MM)
    WHEN e."CollectionMonth" IS NOT NULL THEN
        concat('--', lpad(e."CollectionMonth"::text, 2, '0'))::text
    -- No valid date components
    ELSE
        NULL::text
    END AS "DateTime_ISODateTimeBegin",
    e."LocalityDescription" AS "LocalityText",
    "#project#".abcd__latitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    "#project#".abcd__longitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
    ''::character varying(50) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName",
    ''::character varying(254) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank",
    u."IdentificationUnitID",
        CASE
            WHEN "#project#".abcd__latitude(e."CollectionEventID") IS NULL THEN NULL::text
            ELSE 'WGS84'::text
        END AS "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEvent" e ON e."CollectionEventID" = s."CollectionEventID";

-- writing the ISO Code for the country
SET ROLE "CacheAdmin"; -- ensure access to table Gazetteer
-- filling the Country table if empty
IF (SELECT COUNT(*) FROM "#project#"."ABCD__CountryName_ISO3166Code") = 0 
THEN
	INSERT INTO "#project#"."ABCD__CountryName_ISO3166Code" ("CountryName", "ISO3166Code")
	SELECT "C"."Name",
	MIN("G"."Name") AS "ISO3166Code"
	FROM "Gazetteer" "C"
	JOIN "Gazetteer" "G" ON "C"."PlaceID" = "G"."PlaceID"
	WHERE "G"."LanguageCode"::text = 'ISO 3166 ALPHA-3'::text AND ("C"."LanguageCode"::text !~~ 'ISO %'::text OR "C"."LanguageCode" IS NULL) AND "C"."NameID" <> "G"."NameID"
	GROUP BY "C"."Name";
END IF;

SET ROLE "CacheAdmin";

-- drop table if existing
DROP TABLE IF EXISTS ABCD__Unit_Gathering_ISO3166Code;

-- creating the temporary table containing the ISO Code
CREATE TEMP TABLE ABCD__Unit_Gathering_ISO3166Code AS SELECT "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
       "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
       "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
       "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
       "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "I"."ISO3166Code"
  FROM "#project#"."ABCD__CountryName_ISO3166Code" "I"
     RIGHT JOIN "#project#"."ABCD_Unit_Gathering" "A" ON "A"."Country_Name" = "I"."CountryName";

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering";

-- inserting the data
INSERT INTO "#project#"."ABCD_Unit_Gathering"(
            "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
            "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
            "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "ISO3166Code")
SELECT "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
       "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
       "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
       "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
       "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "ISO3166Code"
  FROM ABCD__Unit_Gathering_ISO3166Code;

-- Adding key
ALTER TABLE "#project#"."ABCD_Unit_Gathering" 
  ADD CONSTRAINT "ABCD_Unit_Gathering_pkey" PRIMARY KEY("ID");

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__unit_gathering()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_gathering() TO "CacheUser";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_gathering() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_gathering() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__unit_gathering()
    IS 'Filling ABCD_Unit_Gathering via a temp table for inclusion of coutry code ISO3166';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."DateTime_ISODateTimeBegin"
    IS 'ABCD: Unit/Gathering/DateTime/ISODateTimeBegin. Retrieved from columns CollectionYear, CollectionMonth, and CollectionDay in table CollectionEvent. 
Formatting rules:
-- Full date: Year, Month, and Day -> Convert to YYYY-MM-DD
-- Year and Month only -> Convert to YYYY-MM
-- Year only -> Convert to YYYY
-- Month and Day only -> --MM-DD
-- Only Month --MM
-- No valid date components -> NULL::text';

--#####################################################################################################################
--######   encode_uri(text) - for encoding uris from  https://stackoverflow.com/a/60260190  ###########################
--#####################################################################################################################

create or replace function encode_uri(text) returns text as $$
    select string_agg(
        case
            when bytes > 1 or c !~ '[0-9a-zA-Z_.!~*''();,/?:@&=+$#-]+' then 
                regexp_replace(encode(convert_to(c, 'utf-8')::bytea, 'hex'), '(..)', E'%\\1', 'g')
            else 
                c
        end,
        ''
    )
    from (
        select c, octet_length(c) bytes
        from regexp_split_to_table($1, '') c
    ) q;
$$ language sql immutable strict;


ALTER FUNCTION encode_uri(text)
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION encode_uri(text) TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION encode_uri(text) TO PUBLIC;

COMMENT ON FUNCTION encode_uri(text)
    IS 'Encoding for URL according to https://stackoverflow.com/a/60260190';



--#####################################################################################################################
--######  Adaption of ABCD_Unit with encoded RecordURI and strict ISODAteTimeFormat ###################################
--#####################################################################################################################

--we have to drop before changing the type timestamp to char
DROP VIEW IF EXISTS public."ABCD_Unit";

CREATE OR REPLACE VIEW public."ABCD_Unit"
 AS
SELECT "ABCD_Unit"."ID",
    "ABCD_Unit"."UnitGUID",
    "ABCD_Unit"."SourceInstitutionID",
    "ABCD_Unit"."SourceID",
    "ABCD_Unit"."UnitID",
    TO_CHAR("ABCD_Unit"."DateLastEdited", 'YYYY-MM-DD"T"HH24:MI:SS') AS "DateLastEdited",
    "ABCD_Unit"."Identification_Taxon_ScientificName_FullScientificName",
    "ABCD_Unit"."Identification_Taxon_ScientificName_Qualifier",
    "ABCD_Unit"."RecordBasis",
    "ABCD_Unit"."KindOfUnit",
    "ABCD_Unit"."Identification_Taxon_HigherTaxonName",
    "ABCD_Unit"."KindOfUnit_Language",
    "ABCD_Unit"."HerbariumUnit_Exsiccatum",
    encode_uri("ABCD_Unit"."RecordURI")::character varying(500) AS "RecordURI",
    "ABCD_Unit"."CollectionSpecimenID",
    "ABCD_Unit"."IdentificationUnitID",
    "ABCD_Unit"."Identification_Reference_URI",
    "ABCD_Unit"."Identification_Reference_CitationDetail",
    "ABCD_Unit"."Identification_Reference_ReferenceGUID",
    "ABCD_Unit"."Identification_Reference_TitleCitation"
    FROM "#project#"."ABCD_Unit";


ALTER TABLE public."ABCD_Unit"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Unit"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/ restricted to taxa with missing qualifier or a qualifier unlike cf ...';

GRANT ALL ON TABLE public."ABCD_Unit" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit"."UnitGUID"
    IS 'ABCD: Unit/DataSets/DataSet/Units/Unit/UnitGUID. Retrieved from StableIdentifier as defined by the basic address for stable identifiers in DiversityCollection extendend with the CollectionSpecimenID for the specimen and if present the IdentificationUnitID for the Unit and the SpecimenPartID for the part';

COMMENT ON COLUMN public."ABCD_Unit"."SourceInstitutionID"
    IS 'ABCD: Unit/SourceInstitutionID. Retrieved from DiversityProjects - Settings - ABCD - Source - InstitutionID';

COMMENT ON COLUMN public."ABCD_Unit"."SourceID"
    IS 'ABCD: Unit/SourceID. Retrieved from DiversityProjects - Settings - ABCD - Source - ID';

COMMENT ON COLUMN public."ABCD_Unit"."UnitID"
    IS 'ABCD: Unit/UnitID. Retrieved from DiversityCollection: AccessionNumber of the specimen if present, otherwise the CollectionSpecimenID + '' / '' + IdentificationUnitID of the Unit + only if a part is present '' / '' + AccessionNumber of the part if present otherwise SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit"."DateLastEdited"
    IS 'ABCD: Unit/DateLastEdited. Retrieved from DiversityCollection from the column LogUpdatedWhen in table CollectionSpecimen. The value will be formatted as ISO 8601 using TO_CHAR("ABCD_Unit"."DateLastEdited", ''YYYY-MM-DD"T"HH24:MI:SS'').';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Taxon_ScientificName_FullScientificName"
    IS 'ABCD: Unit/Identifications/Identification/Result/TaxonIdentified/ScientificName/FullScientificNameString. Retrieved from DiversityCollection from the column LastIdentificationCache in table IdentificationUnit';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Taxon_ScientificName_Qualifier"
    IS 'ABCD: Unit/Identifications/Identification/Result/TaxonIdentified/ScientificName/IdentificationQualifier. Retrieved from DiversityCollection from the column IdentificationQualifier in table Identification (Last valid identification)';

COMMENT ON COLUMN public."ABCD_Unit"."RecordBasis"
    IS 'ABCD: Unit/RecordBasis. Retrieved from DiversityCollection. For observations without a part taken from column RetrievalType in table IdentificationUnit. If missing HumanObservation. For parts taken from the column MaterialCategory in table CollectionSpecimenPart translated according to GBIF definitions';

COMMENT ON COLUMN public."ABCD_Unit"."KindOfUnit"
    IS 'ABCD: Unit/KindOfUnit. Retrieved from DiversityCollection. For parts taken from the column MaterialCategory in table CollectionSpecimenPart otherwise RetrievalType in table IdentificationUnit, if missing human observation';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Taxon_HigherTaxonName"
    IS 'ABCD: Unit/Identifications/Identification/Result/TaxonIdentified/HigherTaxa/HigherTaxon/HigherTaxonName. Retrieved from DiversityCollection from column TaxonomicGroup in table IdentificationUnit, translated according to GBIF definitions';

COMMENT ON COLUMN public."ABCD_Unit"."KindOfUnit_Language"
    IS 'ABCD: Unit/KindOfUnit. Retried from view = en';

COMMENT ON COLUMN public."ABCD_Unit"."HerbariumUnit_Exsiccatum"
    IS 'ABCD: Unit/HerbariumUnit/Exsiccatum. Retrieved from DiversityCollection, column ExsiccataAbbreviation in table CollectionSpecimen';

COMMENT ON COLUMN public."ABCD_Unit"."RecordURI"
    IS 'ABCD: Unit/RecordURI. Retrieved from DiversityProjects - Settings - ABCD - RecordURI extended with informations from DiversityCollection depending on the RecordURI: For http:biocase... AccessionNumber of the specimen if present otherwise CollectionSpecimenID + '' / '' + IdentificationUnitID of the Unit + if a part is present the AccessionNumber of the part otherwise the SpecimenPartID. For and other RecordURI extended with the CollectionSpecimenID for the specimen';

COMMENT ON COLUMN public."ABCD_Unit"."CollectionSpecimenID"
    IS 'CollectionSpecimenID of the specimen';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Reference_URI"
    IS 'Corresponds to ABCD /Identification/References/Reference/URI, Fixed text';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Reference_CitationDetail"
    IS 'Corresponds to ABCD /Identification/References/Reference/CitationDetail, contains NameID e.g. from DiversityTaxonNames';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Reference_ReferenceGUID"
    IS 'Corresponds to ABCD /Identification/References/Reference/ReferenceGUID, Reference on REST-Service e.g. DTNtaxonlists-DiversityTaxonNames_Plants';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Reference_TitleCitation"
    IS 'Corresponds to ABCD /Identification/References/Reference/TitleCitation, Fixed text';


--#####################################################################################################################
--######   abcd__measurementorfact: MeasurementDateTime format to ISO8601  ###################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__measurementorfact(
	)
RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- Cleaning the table
TRUNCATE TABLE "#project#"."ABCD_MeasurementOrFact";

-- Removing the indices
ALTER TABLE "#project#"."ABCD_MeasurementOrFact" DROP CONSTRAINT IF EXISTS "ABCD_MeasurementOrFact_pkey";
DROP INDEX IF EXISTS "#project#".abcd_measurementorfact_id;

-- insert the data
INSERT INTO "#project#"."ABCD_MeasurementOrFact"(
            "ID", 
			"Parameter", 
			"UnitOfMeasurement", 
			"LowerValue", 
			"MeasurementDateTime", 
            "MeasuredBy", 
			"MeasurementOrFactReference",
			"IdentificationUnitID", 
			"SpecimenPartID", 
			"CollectionSpecimenID", 
            "AnalysisID", 
			"AnalysisNumber")
SELECT concat(ua."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    a."DisplayText" AS "Parameter",
    a."MeasurementUnit" AS "UnitOfMeasurement",
    ua."AnalysisResult" AS "LowerValue",
    CASE
    -- Format: YYYY/MM/DD -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
    WHEN ua."AnalysisDate" ~ '^[0-9]{4}/[0-9]{2}/[0-9]{2}$' THEN 
        CONCAT(
            TO_CHAR(TO_DATE(REPLACE(ua."AnalysisDate", '/', '-'), 'YYYY-MM-DD'), 'YYYY-MM-DD'),
            'T00:00:00'
        )
    -- Format: YYYY-MM-DD -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
    WHEN ua."AnalysisDate" ~ '^[0-9]{4}-[0-9]{2}-[0-9]{2}$' THEN 
        CONCAT(ua."AnalysisDate", 'T00:00:00')
     -- Format: DD.MM.YYYY -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
    WHEN ua."AnalysisDate" ~ '^[0-9]{2}\.[0-9]{2}\.[0-9]{4}$' THEN 
        CONCAT(
            TO_CHAR(TO_DATE(ua."AnalysisDate", 'DD.MM.YYYY'), 'YYYY-MM-DD'),
            'T00:00:00'
        )
    -- Format: YYYY-MM-DD hh:mm:ss -> Convert to ISO 8601 with T separator
    WHEN ua."AnalysisDate" ~ '^[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}$' THEN 
        REPLACE(ua."AnalysisDate", ' ', 'T')
    -- Format: YYYY-MM-DD hh:mm:ss.ttt -> Convert to ISO 8601 with T separator
    WHEN ua."AnalysisDate" ~ '^[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}\.[0-9]{3}$' THEN 
        REPLACE(ua."AnalysisDate", ' ', 'T')
    -- Format: YYYY/MM/DD-YYYY/MM/DD -> Convert to YYYY-MM-DDTHH:MM:SS
    WHEN ua."AnalysisDate" ~ '^[0-9]{4}/[0-9]{2}/[0-9]{2}-[0-9]{4}/[0-9]{2}/[0-9]{2}$' THEN 
        CONCAT(
            TO_CHAR(TO_DATE(REPLACE(SPLIT_PART(ua."AnalysisDate", '-', 1), '/', '-'), 'YYYY-MM-DD'), 'YYYY-MM-DD'),
            'T00:00:00'
        )
    -- Format: YYYY-MM-DD/YYYY-MM-DD -> Convert to YYYY-MM-DDTHH:MM:SS
    WHEN ua."AnalysisDate" ~ '^[0-9]{4}-[0-9]{2}-[0-9]{2}/[0-9]{4}-[0-9]{2}-[0-9]{2}$' THEN 
        CONCAT(
            SPLIT_PART(ua."AnalysisDate", '/', 1), 'T00:00:00'
        )
    -- Default: returns the content of AnalysisDate without formating 
    ELSE ua."AnalysisDate"
    END AS "MeasurementDateTime",
    ua."ResponsibleName" AS "MeasuredBy",
	a."AnalysisURI" AS "MeasurementOrFactReference",
    ua."IdentificationUnitID",
    up."SpecimenPartID",
    ua."CollectionSpecimenID",
    ua."AnalysisID",
    ua."AnalysisNumber"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnitAnalysis" ua ON ua."IdentificationUnitID" = up."IdentificationUnitID" and ua."CollectionSpecimenID" = up."CollectionSpecimenID"
     JOIN "#project#"."CacheAnalysis" a ON a."AnalysisID" = ua."AnalysisID";

-- inserting the indices
ALTER TABLE "#project#"."ABCD_MeasurementOrFact"
  ADD CONSTRAINT "ABCD_MeasurementOrFact_pkey" PRIMARY KEY("ID", "AnalysisID", "AnalysisNumber");
  
CREATE INDEX abcd_measurementorfact_id
  ON "#project#"."ABCD_MeasurementOrFact"
  USING btree
  ("ID" COLLATE pg_catalog."default");

     
end;
$BODY$;

ALTER FUNCTION "#project#".abcd__measurementorfact()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO "CacheUser";
GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO PUBLIC;

COMMENT ON COLUMN "ABCD_MeasurementOrFact"."MeasurementDateTime" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasurementDateTime. Retrieved from table IdentificationUnitAnalysis - AnalysisDate.
Formatting rules:
-- Format: YYYY/MM/DD -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
-- Format: YYYY-MM-DD -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
-- Format: DD.MM.YYYY -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
-- Format: YYYY-MM-DD hh:mm:ss -> Convert to ISO 8601 with T separator
-- Format: YYYY-MM-DD hh:mm:ss.ttt -> Convert to ISO 8601 with T separator
-- Format: YYYY/MM/DD-YYYY/MM/DD -> Convert to YYYY-MM-DDTHH:MM:SS
-- Format: YYYY-MM-DD/YYYY-MM-DD -> Convert to YYYY-MM-DDTHH:MM:SS
-- Default: Returns the content of AnalysisDate without formatting.';

--#####################################################################################################################
--######  ABCD_RecordBasis_MaterialCategories new mapping #############################################################
--#####################################################################################################################


INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories"(
    "MaterialCategory", "RecordBasis", "PreparationType")
    VALUES ('absence observation', 'AbsenceObservation', 'no treatment');


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 11 WHERE "Package" = 'ABCD'
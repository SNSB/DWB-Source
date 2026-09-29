--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 16
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   Adding new taxonomic groups   ##############################################################################
--#####################################################################################################################


INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups"(
"TaxonomicGroup", "Kingdom")
VALUES ('archaebacterium', 'Archaea');

INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups"(
"TaxonomicGroup", "Kingdom")
VALUES ('protozoan', 'Protozoa');

INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups"(
"TaxonomicGroup", "Kingdom")
VALUES ('chromist', 'Chromista');

--#####################################################################################################################
--######  new table "#project#"."ABCD__EndangeredSpecies"  ############################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."ABCD__EndangeredSpecies"
(
    "NameID" integer,
    "BaseURL" character varying(500),
    CONSTRAINT "ABCD__EndangeredSpecies_pkey" PRIMARY KEY ("NameID", "BaseURL")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS "#project#"."ABCD__EndangeredSpecies"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."ABCD__EndangeredSpecies" TO "CacheAdmin";


--#####################################################################################################################
--######  New function "#project#".abcd__endangeredspecies  ###########################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__endangeredspecies(
	)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$

begin

--Setting the role
SET ROLE "CacheAdmin";


-- Find EndangeredSpecies

-- Cleaning the table
TRUNCATE TABLE "#project#"."ABCD__EndangeredSpecies";

-- Insert taxa with analysisID = 80 - corresponds to Gesperrte Arten LfU
INSERT INTO "#project#"."ABCD__EndangeredSpecies"(
	"NameID", "BaseURL")
SELECT I."NameID", I."BaseURL"
FROM "#project#"."CacheIdentification" I
INNER JOIN public."TaxonAnalysis" A ON A."NameID" = I."NameID" AND I."BaseURL" = A."BaseURL" AND A."AnalysisID" = 80 AND I."BaseURL" = 'http://tnt.diversityworkbench.de/TaxonNames_Plants/'
GROUP BY I."NameID", I."BaseURL";
-- Insert synonmys
INSERT INTO "#project#"."ABCD__EndangeredSpecies"(
	"NameID", "BaseURL")
SELECT S."NameID", S."BaseURL"
FROM "#project#"."ABCD__EndangeredSpecies" E
INNER JOIN public."TaxonSynonymy" S ON E."NameID" = S."AcceptedNameID" AND S."BaseURL" = E."BaseURL" AND S."BaseURL" = 'http://tnt.diversityworkbench.de/TaxonNames_Plants/'
AND NOT EXISTS(SELECT * FROM "#project#"."ABCD__EndangeredSpecies" A WHERE S."NameID" = A."NameID" AND S."BaseURL" = A."BaseURL")
GROUP BY S."NameID", S."BaseURL";

INSERT INTO "#project#"."ABCD__EndangeredSpecies"(
	"NameID", "BaseURL")
SELECT S."AcceptedNameID", S."BaseURL"
FROM "#project#"."ABCD__EndangeredSpecies" E
INNER JOIN public."TaxonSynonymy" S ON E."NameID" = S."NameID" AND S."BaseURL" = E."BaseURL" AND S."BaseURL" = 'http://tnt.diversityworkbench.de/TaxonNames_Plants/'
AND NOT EXISTS(SELECT * FROM "#project#"."ABCD__EndangeredSpecies" A WHERE S."AcceptedNameID" = A."NameID" AND S."BaseURL" = A."BaseURL")
GROUP BY S."AcceptedNameID", S."BaseURL";


-- insert inferior taxa with status H (3 times to catch also the taxa with more than 1 level of inferiority)
INSERT INTO "#project#"."ABCD__EndangeredSpecies"(
	"NameID", "BaseURL")
SELECT S."NameID", S."BaseURL"
FROM "#project#"."ABCD__EndangeredSpecies" E
INNER JOIN public."TaxonSynonymy" S ON E."NameID" = S."NameParentID" AND E."BaseURL" = S."BaseURL" AND S."BaseURL" = 'http://tnt.diversityworkbench.de/TaxonNames_Plants/'
INNER JOIN public."TaxonAnalysis" A ON A."NameID" = S."NameID" AND S."BaseURL" = A."BaseURL" AND A."AnalysisID" = 2 AND A."AnalysisValue" IN ( 'H', 'H?')
AND NOT EXISTS(SELECT * FROM "#project#"."ABCD__EndangeredSpecies" A WHERE S."NameID" = A."NameID" AND S."BaseURL" = A."BaseURL")
GROUP BY S."NameID", S."BaseURL";

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__endangeredspecies()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__endangeredspecies() TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__endangeredspecies() TO "CacheUser";

GRANT EXECUTE ON FUNCTION "#project#".abcd__endangeredspecies() TO PUBLIC;


--#####################################################################################################################
--######   New view ABCD__EndangeredSpeciesEventID       ##############################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."ABCD__EndangeredSpeciesEventID"
 AS
 SELECT s."CollectionEventID"
   FROM "#project#"."ABCD__EndangeredSpecies" t,
    "#project#"."CacheCollectionSpecimen" s,
    "#project#"."CacheIdentification" i
  WHERE t."NameID" = i."NameID" AND i."CollectionSpecimenID" = s."CollectionSpecimenID";

ALTER TABLE "#project#"."ABCD__EndangeredSpeciesEventID"
    OWNER TO "CacheAdmin";

GRANT ALL ON TABLE "#project#"."ABCD__EndangeredSpeciesEventID" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__EndangeredSpeciesEventID" TO "CacheUser";




--#####################################################################################################################
--######  New view "#project#"."ABCD__WrongStatusUnitID"  #############################################################
--#####################################################################################################################



CREATE OR REPLACE VIEW "#project#"."ABCD__WrongStatusUnitID"
 AS
 SELECT "IdentificationUnitID"
   FROM "#project#"."CacheIdentificationUnitAnalysis" AS "U"
   INNER JOIN "#project#"."CacheAnalysis" AS "A" ON "A"."AnalysisID" = "U"."AnalysisID" AND "A"."DisplayText" = 'Floristischer Status'
  WHERE "A"."AnalysisID" = 2 AND ("U"."AnalysisResult" = ANY (ARRAY['2'::text, '3'::text, '4'::text, '5'::text, '6'::text, '7'::text, '8'::text, '9'::text, '?'::text, '+'::text, '-'::text, 'F'::text, 'V'::text, 'X'::text, '99'::text]));

ALTER TABLE "#project#"."ABCD__WrongStatusUnitID"
    OWNER TO "CacheAdmin";

GRANT ALL ON TABLE "#project#"."ABCD__WrongStatusUnitID" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__WrongStatusUnitID" TO "CacheUser";




--#####################################################################################################################
--######  adapt function "#project#".abcd__measurementorfact  #########################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__measurementorfact(
	)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$

begin

--Setting the role
SET ROLE "CacheAdmin";


-- Find EndangeredSpecies
PERFORM "#project#".abcd__endangeredspecies();


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
     JOIN "#project#"."CacheAnalysis" a ON a."AnalysisID" = ua."AnalysisID"
	 WHERE ua."IdentificationUnitID" NOT IN(SELECT "IdentificationUnitID" FROM "#project#"."ABCD__WrongStatusUnitID");

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

GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO "CacheUser";

GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO PUBLIC;



--#####################################################################################################################
--######   function abcd__Unit_Gathering                                     ##########################################
--######   Endangered species without locality text, coordinates etc.        ##########################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__unit_gathering(
	)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
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
            "ID", "Country_Name", "DateTime_ISODateTimeBegin", "DateTime_ISODateTimeEnd", "Code", 
            "LocalityText", 
            "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
            "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod",
            "SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters",
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "CollectionSpecimenID", "CollectionEventID")
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

    CASE
    -- Full date: Year, Month, and Day
    WHEN e."CollectionEndYear" IS NOT NULL AND e."CollectionEndMonth" IS NOT NULL AND e."CollectionEndDay" IS NOT NULL THEN
        concat_ws('-',
            e."CollectionEndYear"::text,
            lpad(e."CollectionEndMonth"::text, 2, '0'),
            lpad(e."CollectionEndDay"::text, 2, '0')
        )::text
    -- Year and Month only
    WHEN e."CollectionEndYear" IS NOT NULL AND e."CollectionEndMonth" IS NOT NULL THEN
        concat_ws('-',
            e."CollectionEndYear"::text,
            lpad(e."CollectionEndMonth"::text, 2, '0')
        )::text
    -- Year only
    WHEN e."CollectionEndYear" IS NOT NULL THEN
        e."CollectionEndYear"::text
    -- Month and Day only (custom format: --MM-DD)
    WHEN e."CollectionEndMonth" IS NOT NULL AND e."CollectionEndDay" IS NOT NULL THEN
    concat(
        '--',
        lpad(e."CollectionEndMonth"::text, 2, '0'),
        '-',
        lpad(e."CollectionEndDay"::text, 2, '0')
    )::text
    -- Only Month (ISO 8601 workaround: --MM)
    WHEN e."CollectionEndMonth" IS NOT NULL THEN
        concat('--', lpad(e."CollectionEndMonth"::text, 2, '0'))::text
    -- No valid date components
    ELSE
        NULL::text
    END AS "DateTime_ISODateTimeBegin",
    e."CollectorsEventNumber" AS "Code",
    case when es."CollectionEventID" is null then e."LocalityDescription" else '' end AS "LocalityText", -- if the event is listed as endangered species, the locality description is not provided
    case when es."CollectionEventID" is null then "#project#".abcd__latitude(e."CollectionEventID") else NULL end AS "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    case when es."CollectionEventID" is null then "#project#".abcd__longitude(e."CollectionEventID") else NULL end AS "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
    case when es."CollectionEventID" is null then "#project#".abcd__coordinatemethod(e."CollectionEventID") else NULL end AS "SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod",
    case when es."CollectionEventID" is null then "#project#".abcd__coordinateerrordistanceinmeters(e."CollectionEventID") else NULL end AS "SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters",
    ''::character varying(50) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName",
    ''::character varying(254) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank",
    u."IdentificationUnitID",
        CASE
            WHEN "#project#".abcd__latitude(e."CollectionEventID") IS NULL or es."CollectionEventID" is not null THEN NULL::text
            ELSE 'WGS84'::text
        END AS "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum",
        s."CollectionSpecimenID",
        e."CollectionEventID"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEvent" e ON e."CollectionEventID" = s."CollectionEventID"
     LEFT JOIN "#project#"."ABCD__EndangeredSpeciesEventID" es on e."CollectionEventID" = es."CollectionEventID"; -- Linking to the table of endangered species events to determine if the locality description should be provided

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
CREATE TEMP TABLE ABCD__Unit_Gathering_ISO3166Code AS SELECT "ID", "Country_Name", "DateTime_ISODateTimeBegin", "DateTime_ISODateTimeEnd", "Code",
       "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
       "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod", 
       "SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters",
       "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
       "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
       "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "I"."ISO3166Code", "A"."CollectionSpecimenID", "A"."CollectionEventID"
  FROM "#project#"."ABCD__CountryName_ISO3166Code" "I"
     RIGHT JOIN "#project#"."ABCD_Unit_Gathering" "A" ON "A"."Country_Name" = "I"."CountryName";

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering";

-- inserting the data
INSERT INTO "#project#"."ABCD_Unit_Gathering"(
            "ID", "Country_Name", "DateTime_ISODateTimeBegin", "DateTime_ISODateTimeEnd", "Code",
            "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
            "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod", 
            "SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters",
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "ISO3166Code", "CollectionSpecimenID", "CollectionEventID")
SELECT "ID", "Country_Name", "DateTime_ISODateTimeBegin", "DateTime_ISODateTimeEnd", "Code",
       "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
       "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod", 
       "SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters",
       "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
       "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
       "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "ISO3166Code", "CollectionSpecimenID", "CollectionEventID"
  FROM ABCD__Unit_Gathering_ISO3166Code;

-- Adding key
ALTER TABLE "#project#"."ABCD_Unit_Gathering" 
  ADD CONSTRAINT "ABCD_Unit_Gathering_pkey" PRIMARY KEY("ID");

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__unit_gathering()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_gathering() TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_gathering() TO "CacheUser";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_gathering() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__unit_gathering()
    IS 'Filling ABCD_Unit_Gathering via a temp table for inclusion of coutry code ISO3166';




--#####################################################################################################################
--######   public."ABCD_Unit_Identification" with preferred flag   ####################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit_Identification"
 AS
 SELECT "ID" 
	,"AcceptedName" AS "Taxon_ScientificName_FullScientificName"
	,"AcceptedNameSinAuthor" AS "InformalNameString"
	,1::boolean AS "PreferredFlag"
   FROM "#project#"."ABCD_Unit" AS "U"
   INNER JOIN "#project#"."ABCD__Unit_LastIdentification" AS "L" 
   ON "U"."CollectionSpecimenID" = "L"."CollectionSpecimenID" AND "U"."IdentificationUnitID" = "L"."IdentificationUnitID"
   INNER JOIN "#project#"."CacheIdentification" AS "I" 
   ON "I"."CollectionSpecimenID" = "L"."CollectionSpecimenID" AND "I"."IdentificationUnitID" = "L"."IdentificationUnitID" AND "I"."IdentificationSequence" = "L"."IdentificationSequence"
   INNER JOIN public."TaxonSynonymy" AS "T" 
   ON "T"."NameID" = "I"."NameID" AND "T"."BaseURL" = "I"."BaseURL"
	 UNION
    SELECT "U"."ID",
    "U"."Identification_Taxon_ScientificName_FullScientificName" AS "Taxon_ScientificName_FullScientificName",
    "U"."InformalNameString",
    0::boolean AS "PreferredFlag"
   FROM "#project#"."ABCD_Unit" AS "U"
     JOIN "#project#"."ABCD__Unit_LastIdentification" AS "L" ON "U"."CollectionSpecimenID" = "L"."CollectionSpecimenID" AND "U"."IdentificationUnitID" = "L"."IdentificationUnitID";

ALTER TABLE public."ABCD_Unit_Identification"
    OWNER TO "CacheAdmin";

COMMENT ON VIEW public."ABCD_Unit_Identification"
    IS 'ABCD entity /DataSets/DataSet/Identification/ for accepted names with preferred flag set to true';

GRANT ALL ON TABLE public."ABCD_Unit_Identification" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_Identification" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit_Identification"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit_Identification"."Taxon_ScientificName_FullScientificName"
    IS 'ABCD: Unit/Identifications/Identification/Result/TaxonIdentified/ScientificName/FullScientificNameString. Accepted name retrieved from DiversityTaxonNames for the last identification';

COMMENT ON COLUMN public."ABCD_Unit_Identification"."InformalNameString"
    IS 'Corresponds to ABCD Unit/Identifications/Identification/Result/TaxonIdentified/InformalNameString. Derived from TaxonNameSinAuthor';

COMMENT ON COLUMN public."ABCD_Unit_Identification"."PreferredFlag"
    IS 'Corresponds to ABCD Unit/Identifications/Identification/Result/TaxonIdentified/PreferredFlag. Indicates the accepted name for data with preferred flag set to true';


--#####################################################################################################################
--######   public."ABCD_Unit_Identification_References"   #############################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit_Identification_References"
 AS
SELECT U."ID",
    R."ReferenceDetails" AS "Detail",
    R."ReferenceTitle" AS "Text",
    R."ReferenceID"::character varying(50) AS "ReferenceGUID",
    R."ReferenceURI" AS "ResourceURI",
    R."CollectionSpecimenID",
    R."IdentificationUnitID",
    R."IdentificationSequence"
FROM "#project#"."ABCD_Unit" U
INNER JOIN "#project#"."CacheCollectionSpecimenReference" R
ON R."CollectionSpecimenID" = U."CollectionSpecimenID" AND R."IdentificationUnitID" = U."IdentificationUnitID" 
INNER JOIN "#project#"."CacheIdentification" I ON I."CollectionSpecimenID" = R."CollectionSpecimenID" AND I."IdentificationUnitID" = R."IdentificationUnitID" AND I."IdentificationSequence" = R."IdentificationSequence"
UNION
SELECT U."ID", 
    E."ExternalNameURI" AS "Details",
    D."ExternalDatabaseName" AS "Text",
    U."Identification_Reference_ReferenceGUID" AS "ReferenceGUID",
    U."Identification_Reference_URI" AS "ResourceURI",
    U."CollectionSpecimenID",
    U."IdentificationUnitID",
    NULL AS "IdentificationSequence"
    FROM "#project#"."ABCD_Unit" U
    INNER JOIN "#project#"."CacheIdentification" I ON U."IdentificationUnitID" = I."IdentificationUnitID"
    INNER JOIN public."TaxonNameExternalID" E ON E."NameID" = I."NameID"
    INNER JOIN public."TaxonNameExternalDatabase" D ON D."ExternalDatabaseID" = E."ExternalDatabaseID" AND E."BaseURL" = D."BaseURL"
UNION
SELECT U."ID", 
	'Taxon list of vascular plants from Bavaria, Germany compiled in the context of the BFL Project'::character varying AS "Text", 
    I."NameID"::character varying(50) AS "Details",
	'http://www.diversitymobile.net/wiki/About_%22Taxon_list_of_vascular_plants_from_Bavaria,_Germany_compiled_in_the_context_of_the_BFL_project%22'::character varying,
	concat('http://services.snsb.info/DTNtaxonlists/rest/v0.1/names/DiversityTaxonNames_Plants/', I."NameID"::character varying)::character varying AS "ReferenceGUID",
    U."CollectionSpecimenID",
    U."IdentificationUnitID"
    NULL AS "IdentificationSequence"
    FROM "#project#"."ABCD_Unit" U
    INNER JOIN "#project#"."CacheIdentification" I ON U."IdentificationUnitID" = I."IdentificationUnitID"
    INNER JOIN public."TaxonSynonymy" S ON S."NameID" = I."NameID"
    AND S."BaseURL" = 'http://tnt.diversityworkbench.de/TaxonNames_Plants/';


ALTER TABLE public."ABCD_Unit_Identification_References"
    OWNER TO "CacheAdmin";

COMMENT ON VIEW public."ABCD_Unit_Identification_References"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/Identification/References/Reference';

GRANT ALL ON TABLE public."ABCD_Unit_Identification_References" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_Identification_References" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."Identification_Reference_URI"
    IS 'Corresponds to ABCD /Identification/References/Reference/URI, Fixed text';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."Identification_Reference_CitationDetail"
    IS 'Corresponds to ABCD /Identification/References/Reference/CitationDetail, contains NameID e.g. from DiversityTaxonNames';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."Identification_Reference_ReferenceGUID"
    IS 'Corresponds to ABCD /Identification/References/Reference/ReferenceGUID, Reference on REST-Service e.g. DTNtaxonlists-DiversityTaxonNames_Plants';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."Identification_Reference_TitleCitation"
    IS 'Corresponds to ABCD /Identification/References/Reference/TitleCitation, Fixed text';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."CollectionSpecimenID"
    IS 'Part of primary key of source table';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."IdentificationUnitID"
    IS 'Part of primary key of source table';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."IdentificationSequence"
    IS 'Part of primary key of source table';


--#####################################################################################################################
--######   public."ABCD_Unit" zusätzliche Spalte "NamedCollectionsOrSurveys_NamedCollectionOrSurvey"  #################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit"
 AS
 SELECT "ID",
    "UnitGUID",
    "SourceInstitutionID",
    "SourceID",
    "UnitID",
    to_char("DateLastEdited", 'YYYY-MM-DD"T"HH24:MI:SS'::text) AS "DateLastEdited",
    "Identification_Taxon_ScientificName_FullScientificName",
    "Identification_Taxon_ScientificName_Qualifier",
    "RecordBasis",
    "KindOfUnit",
    "Identification_Taxon_HigherTaxonName",
    "InformalNameString",
    "KindOfUnit_Language",
    "HerbariumUnit_Exsiccatum",
    encode_uri("RecordURI"::text)::character varying(500) AS "RecordURI",
    "CollectionSpecimenID",
    "IdentificationUnitID",
    "Identification_Reference_URI",
    "Identification_Reference_CitationDetail",
    "Identification_Reference_ReferenceGUID",
    "Identification_Reference_TitleCitation", 
    SUBSTRING('#project#' FROM 9) AS "NamedCollectionsOrSurveys_NamedCollectionOrSurvey"
   FROM "#project#"."ABCD_Unit";

ALTER TABLE public."ABCD_Unit"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Unit"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/ ';
    -- restricted to taxa with missing qualifier or a qualifier unlike cf ...'; Only in BayernFlora (AddOn)

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

COMMENT ON COLUMN public."ABCD_Unit"."NamedCollectionsOrSurveys_NamedCollectionOrSurvey"
    IS 'ABCD: Unit/NamedCollectionsOrSurveys/NamedCollectionsOrSurvey, Fixed text, Project in DWB';

--#####################################################################################################################
--######   New view public.ABCD_NameBotanical including data from DTN    ##############################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_NameBotanical"
 AS
SELECT U."ID",
T."GenusOrSupragenericName" AS "GenusOrMonomial",
T."SpeciesEpithet" AS "FirstEpithet",
T."InfraspecificEpithet",
"TaxonomicRank" AS "Rank",
case when T."TaxonName" like '% x %' then 'x' else NULL end AS "HybridFlag",
case when T."CombiningAuthors" <> '' then T."BasionymAuthors" else '' end AS "AuthorTeamParenthesis",
case when T."CombiningAuthors" <> '' then T."CombiningAuthors" else T."BasionymAuthors" end AS "AuthorTeam"
FROM "#project#"."ABCD_Unit" U
JOIN "#project#"."ABCD__Unit_LastIdentification" L ON U."CollectionSpecimenID" = L."CollectionSpecimenID" AND U."IdentificationUnitID" = L."IdentificationUnitID"
JOIN "#project#"."CacheIdentification" I ON I."CollectionSpecimenID" = L."CollectionSpecimenID" 
AND I."IdentificationUnitID" = L."IdentificationUnitID" AND I."IdentificationSequence" = L."IdentificationSequence"
AND I."BaseURL" = 'http://tnt.diversityworkbench.de/TaxonNames_Plants/';
JOIN public."TaxonSynonymy" T ON T."NameID" = I."NameID" AND T."BaseURL" = I."BaseURL";


COMMENT ON VIEW public."ABCD_NameBotanical"
    IS 'ABCD: ScientificName / NameAtomised / Botanical';

COMMENT ON COLUMN public."ABCD_NameBotanical"."GenusOrMonomial"
    IS 'ABCD: ScientificName/NameAtomised/Botanical/GenusOrMonomial. Derived from GenusOrSupragenericName in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameBotanical"."FirstEpithet"
    IS 'ABCD: ScientificName/NameAtomised/Botanical/FirstEpithet. Derived from SpeciesEpithet in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameBotanical"."InfraspecificEpithet"
    IS 'ABCD: ScientificName/NameAtomised/Botanical/InfraspecificEpithet. Derived from InfraspecificEpithet in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameBotanical"."Rank"
    IS 'ABCD: ScientificName/NameAtomised/Botanical/Rank. Derived from TaxonomicRank in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameBotanical"."HybridFlag"
    IS 'ABCD: ScientificName/NameAtomised/Botanical/HybridFlag. Derived from IsHybrid in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameBotanical"."AuthorTeamParenthesis"
    IS 'ABCD: ScientificName/NameAtomised/Botanical/AuthorTeamParenthesis. Derived from BasionymAuthors in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameBotanical"."AuthorTeam"
    IS 'ABCD: ScientificName/NameAtomised/Botanical/AuthorTeam. Derived from CombiningAuthors in DTN.TaxonName';


--#####################################################################################################################
--######   New view public.ABCD_NameZoological including data from DTN    #############################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_NameZoological"
 AS
SELECT U."ID",
T."GenusOrSupragenericName" AS "GenusOrMonomial",
T."SpeciesEpithet",
T."InfraspecificEpithet" AS "SubspeciesEpithet",
--T."InfragenericEpithet" AS "Subgenus",
--case when T."IsRecombination" = 0::boolean then concat(T."BasionymAuthors", ' ', T."BasionymAuthorsYear") else '' end AS "AuthorTeamOriginalAndYear",
--case when T."IsRecombination" = 1::boolean then concat('(', T."BasionymAuthors", ' ', T."BasionymAuthorsYear", ')') else '' end AS "AuthorTeamParenthesisAndYear", 
case when T."CombiningAuthors" <> '' then concat(T."CombiningAuthors", ' ', T."YearOfPubl") else '' end AS "CombinationAuthorTeamAndYear"
FROM "#project#"."ABCD_Unit" U
JOIN "#project#"."ABCD__Unit_LastIdentification" L ON U."CollectionSpecimenID" = L."CollectionSpecimenID" AND U."IdentificationUnitID" = L."IdentificationUnitID"
JOIN "#project#"."CacheIdentification" I ON I."CollectionSpecimenID" = L."CollectionSpecimenID" 
AND I."IdentificationUnitID" = L."IdentificationUnitID" AND I."IdentificationSequence" = L."IdentificationSequence"
AND I."BaseURL" IN ( 'http://tnt.diversityworkbench.de/TaxonNames_Animalia/', 'http://tnt.diversityworkbench.de/TaxonNames_Insecta/', 'http://tnt.diversityworkbench.de/TaxonNames_Fossils/' )
JOIN public."TaxonSynonymy" T ON T."NameID" = I."NameID" AND T."BaseURL" = I."BaseURL";


COMMENT ON VIEW public."ABCD_NameZoological"
    IS 'ABCD: ScientificName / NameAtomised / Zoological';

COMMENT ON COLUMN public."ABCD_NameZoological"."GenusOrMonomial"
    IS 'ABCD: ScientificName/NameAtomised/Zoological/GenusOrMonomial. Derived from GenusOrSupragenericName in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameZoological"."SpeciesEpithet"
    IS 'ABCD: ScientificName/NameAtomised/Zoological/SpeciesEpithet. Derived from SpeciesEpithet in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameZoological"."SubspeciesEpithet"
    IS 'ABCD: ScientificName/NameAtomised/Zoological/SubspeciesEpithet. Derived from InfraspecificEpithet in DTN.TaxonName';

--COMMENT ON COLUMN public."ABCD_NameZoological"."Subgenus"
--    IS 'ABCD: ScientificName/NameAtomised/Zoological/Subgenus. Derived from InfragenericEpithet in DTN.TaxonName';

--COMMENT ON COLUMN public."ABCD_NameZoological"."AuthorTeamOriginalAndYear"
--    IS 'ABCD: ScientificName/NameAtomised/Zoological/AuthorTeamOriginalAndYear. Derived from BasionymAuthors and BasionymAuthorsYear in DTN.TaxonName';

--COMMENT ON COLUMN public."ABCD_NameZoological"."AuthorTeamParenthesisAndYear"
--    IS 'ABCD: ScientificName/NameAtomised/Zoological/AuthorTeamParenthesisAndYear. Derived from BasionymAuthors and BasionymAuthorsYear in DTN.TaxonName';

COMMENT ON COLUMN public."ABCD_NameZoological"."CombinationAuthorTeamAndYear"
    IS 'ABCD: ScientificName/NameAtomised/Zoological/CombinationAuthorTeamAndYear. Derived from CombiningAuthors and YearOfPubl in DTN.TaxonName';




--#####################################################################################################################
--######   ABCD_MetaData_All: Metadata for all related projects of the specimen in the current project ################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_MetaData_All"
 AS
 SELECT m."ProjectID",
    m."DatasetDetails" AS "Description_Representation_Details",
    m."DatasetTitle" AS "Description_Representation_Title",
    m."DatasetURI" AS "Description_Representation_URI",
    m."DateModified" AS "RevisionData_DateModified",
    m."OwnerEmail" AS "Owner_EmailAddress",
    m."OwnerLogoURI" AS "Owner_LogoURI",
    m."OwnerContactPerson" AS "Owner_Person_FullName",
    m."OwnerContactRole" AS "Owner_Role",
    m."OwnerURI" AS "Owner_URL",
    m."CopyrightURI" AS "IPRStatements_Copyright_URI",
    m."DisclaimersText" AS "IPRStatements_Disclaimer_Text",
    m."DisclaimersURI" AS "IPRStatements_Disclaimer_URI",
    m."LicenseText" AS "IPRStatements_License_Text",
    m."LicenseURI" AS "IPRStatements_License_URI",
    'en'::character varying(2) AS "IPRStatements_License_Language",
    m."TermsOfUseText" AS "IPRStatements_TermsOfUse_Text",
    m."OwnerOrganizationName" AS "Owner_Organisation_Name_Text",
    m."OwnerOrganizationAbbrev" AS "Owner_Organisation_Name_Abbreviation",
    m."OwnerAddress" AS "Owner_Address",
    m."OwnerTelephone" AS "Owner_Telephone_Number",
    m."IPRText" AS "IPRStatements_IPRDeclaration_Text",
    m."IPRDetails" AS "IPRStatements_IPRDeclaration_Details",
    m."IPRURI" AS "IPRStatements_IPRDeclaration_URI",
    m."CopyrightText" AS "IPRStatements_Copyright_Text",
    m."CopyrightDetails" AS "IPRStatements_Copyright_Details",
    m."LicensesDetails" AS "IPRStatements_License_Details",
    m."TermsOfUseDetails" AS "IPRStatements_TermsOfUse_Details",
    m."TermsOfUseURI" AS "IPRStatements_TermsOfUse_URI",
    m."DisclaimersDetails" AS "IPRStatements_Disclaimer_Details",
    m."AcknowledgementsText" AS "IPRStatements_Acknowledgement_Text",
    m."AcknowledgementsDetails" AS "IPRStatements_Acknowledgement_Details",
    m."AcknowledgementsURI" AS "IPRStatements_Acknowledgement_URI",
        CASE
            WHEN r."Citation" <> ''::text THEN r."Citation"::character varying
            ELSE m."CitationsText"
        END::text AS "IPRStatements_Citation_Text",
    m."CitationsDetails" AS "IPRStatements_Citation_Details",
    m."CitationsURI" AS "IPRStatements_Citation_URI",
    m."StableIdentifier" AS "DatasetGUID"
   FROM "#project#"."CacheMetadata" m
     LEFT JOIN "#project#"."ABCD__ProjectCitation" r ON m."ProjectID" = r."ProjectID";

ALTER TABLE public."ABCD_MetaData_All"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_MetaData_All"
    IS 'ABCD entity /DataSets/DataSet/Metadata';

GRANT ALL ON TABLE public."ABCD_MetaData_All" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_MetaData_All" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_MetaData_All"."ProjectID"
    IS 'ID of the project retrieved from DiversityProjects';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Description_Representation_Details"
    IS 'ABCD: Dataset/Metadata/Description/Representation/Details. Retrieved from DiversityProjects - Project - PublicDescription';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Description_Representation_Title"
    IS 'ABCD: Dataset/Metadata/Description/Representation/Title. Retrieved from DiversityProjects - Settings - ABCD - Dataset - Title';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Description_Representation_URI"
    IS 'ABCD: Dataset/Metadata/Description/Representation/URI. Retrieved from DiversityProjects - Settings - ABCD - Dataset - URI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."RevisionData_DateModified"
    IS 'ABCD: Dataset/Metadata/RevisionData/DateModified. Retrieved from the first level SQL-Server cache database for DiversityCollection corresponding to the date and time of the last transfer into the cache database (ProjectPublished.LastUpdatedWhen)';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Owner_EmailAddress"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/EmailAddress. Retrieved from DiversityProjects - Settings - ABCD - Owner - Email';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Owner_LogoURI"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/LogoURI. Retrieved from DiversityProjects - Settings - ABCD - Owner - LogoURI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Copyright_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/URI. Retrieved from DiversityProjects - Settings - ABCD - Copyright - URI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Disclaimer_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/Text. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Text';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Disclaimer_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/URI. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - URI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_License_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/Text. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Text';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_License_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/URI. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - URI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_License_Language"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/URI. Defined in view = en';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_TermsOfUse_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/Text. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - Text';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Owner_Organisation_Name_Text"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/Organisation/Name/Representation/Text. Retrieved from DiversityProjects - Settings - ABCD - Owner - OrganisationName';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Owner_Organisation_Name_Abbreviation"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/Organisation/Name/Representation/Abbreviation. Retrieved from DiversityProjects - Settings - ABCD - Owner - OrganisationAbbrev';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Owner_Address"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/Addresses/Address. Retrieved from DiversityProjects - Settings - ABCD - Owner - Address';

COMMENT ON COLUMN public."ABCD_MetaData_All"."Owner_Telephone_Number"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/TelephoneNumbers/TelephoneNumber/Number. Retrieved from DiversityProjects - Settings - ABCD - Owner - Telephone';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_IPRDeclaration_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/Text. Retrieved from DiversityProjects - Settings - ABCD - IPR - Text';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_IPRDeclaration_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/Details. Retrieved from DiversityProjects - Settings - ABCD - IPR - Details';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_IPRDeclaration_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/URI. Retrieved from DiversityProjects - Settings - ABCD - IPR - URI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Copyright_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/Text. Retrieved from DiversityProjects - Settings - ABCD - Copyright - Text';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Copyright_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/Details. Retrieved from DiversityProjects - Settings - ABCD - Copyright - Details';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_License_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/Details. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Details';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_TermsOfUse_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/Details. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - Details';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_TermsOfUse_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/URI. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - URI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Disclaimer_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/Details. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Details';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Acknowledgement_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/Text. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - Text';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Acknowledgement_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/Details. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - Details';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Acknowledgement_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/URI. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - URI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Citation_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/Text. Retrieved from DiversityProjects: 
First dataset in table ProjectReference where type = ''BioCASe (GFBio)''. 
Authors from table ProjectAgent with type = ''Author'' according to their sequence. If none are found ''Anonymous''. 
Current year. Content of column ProjectTitle in table Project. 
Marker ''[Dataset]''. 
''Version: '' + date of transfer into the ABCD tables as year + month + day: yyyymmdd.
If available publishers: ''Data Publisher: '' + Agents with role ''Publisher'' from table ProjectAgent. URI from table ProjectReference if present. 
If entry in table ProjectReference is missing taken from Settings - ABCD - Citations - Text';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Citation_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/Details. Retrieved from DiversityProjects - Settings - ABCD - Citations - Details';

COMMENT ON COLUMN public."ABCD_MetaData_All"."IPRStatements_Citation_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/URI. Retrieved from DiversityProjects - Settings - ABCD - Citations - URI';

COMMENT ON COLUMN public."ABCD_MetaData_All"."DatasetGUID"
    IS 'ABCD: Dataset/DatasetGUID. Retrieved from DiversityProjects - StableIdentifier (= basic address for stable identifiers + ID of the project)';

    
--#####################################################################################################################
--######   new view ABCD_Unit_Gathering_Project in public   ###########################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit_Gathering_Project"
 AS
select "U"."ID", "M"."ProjectTitle"
from "#project#"."ABCD_Unit" AS "U"
join "#project#"."CacheCollectionProject" AS "CP" ON "U"."CollectionSpecimenID" = "CP"."CollectionSpecimenID"
join "#project#"."CacheMetadata" AS "M" ON "M"."ProjectID" = "CP"."ProjectID";

ALTER TABLE public."ABCD_Unit_Gathering_Project"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Unit_Gathering_Project"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering';

GRANT ALL ON TABLE public."ABCD_Unit_Gathering_Project" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_Gathering_Project" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit_Gathering_Project"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_Project"."ProjectTitle"
    IS 'ABCD: Unit/Gathering/Project/ProjectTitle. Retrieved from DiversityProject - Title of the project linked to a specimen';



--#####################################################################################################################
--######   Adaption of ABCD_Unit_Gathering_CoordinatesGrid for endangered species    ##################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit_Gathering_CoordinatesGrid"
 AS
 SELECT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    'TK25'::character varying(50) AS "GridCellSystem",
    e."Location1" AS "GridCellCode",
    CASE WHEN ES."CollectionEventID" IS NULL THEN e."Location2" ELSE  "substring"(e."Location2"::text, 1, 1)::character varying(255) END AS "GridQualifier",
    e."RecordingMethod" AS "Method",
    e."LocationAccuracy" AS "GeoreferenceRemarks"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEventLocalisation" e ON e."CollectionEventID" = s."CollectionEventID" AND e."LocalisationSystemID" = 3
     LEFT JOIN "#project#"."ABCD__EndangeredSpeciesEventID" ES ON ES."CollectionEventID" = e."CollectionEventID";

ALTER TABLE public."ABCD_Unit_Gathering_CoordinatesGrid"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Unit_Gathering_CoordinatesGrid"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering/CoordinateSets/CoordinateSet/CoordinatesGrid. Data retrieved from table CollectionEventLocalisation for TK25';

GRANT ALL ON TABLE public."ABCD_Unit_Gathering_CoordinatesGrid" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_Gathering_CoordinatesGrid" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."GridCellSystem"
    IS 'ABCD: Unit/Gathering/Gathering/CoordinateSets/CoordinateSet/CoordinatesGrid/GridCellSystem. Retrieved from View = TK25';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."GridCellCode"
    IS 'ABCD: Unit/Gathering/Gathering/CoordinateSets/CoordinateSet/CoordinatesGrid/GridCellSystem/GridCellCode. Retrieved from column Location1 in table CollectionEventLocalisation. Corresponds to TK25';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."GridQualifier"
    IS 'ABCD: Unit/Gathering/Gathering/CoordinateSets/CoordinateSet/CoordinatesGrid/GridCellSystem/GridQualifier. Retrieved from column Location2 in table CollectionEventLocalisation. Corresponds to Quadrant in TK25';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."Method"
    IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinateSet/Method. Retrieved from column RecordingMethod in table CollectionEventLocalisation';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."GeoreferenceRemarks"
    IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinateSet/GeoreferenceRemarks. Retrieved from column LocationAccuracy in table CollectionEventLocalisation';




--#####################################################################################################################
--######   new view ABCD_Unit_SourceInstitutionID in public   #########################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW public."ABCD_Unit_SourceInstitutionID"
 AS
select "U"."ID", "E"."ExternalDatasourceInstitution" AS "SourceInstitutionID"
from "#project#"."ABCD_Unit" AS "U"
join "#project#"."CacheCollectionSpecimen" AS "S" ON "U"."CollectionSpecimenID" = "S"."CollectionSpecimenID"
join "#project#"."CacheCollectionExternalDatasource" AS "E" ON "E"."ExternalDatasourceID" = "S"."ExternalDatasourceID";

ALTER TABLE public."ABCD_Unit_SourceInstitutionID"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Unit_SourceInstitutionID"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/SourceInstitutionID. Retrieved from CollectionExternalDatasource.ExternalDatasourceInstitution';

GRANT ALL ON TABLE public."ABCD_Unit_SourceInstitutionID" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_SourceInstitutionID" TO "CacheUser";


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 16 WHERE "Package" = 'ABCD'
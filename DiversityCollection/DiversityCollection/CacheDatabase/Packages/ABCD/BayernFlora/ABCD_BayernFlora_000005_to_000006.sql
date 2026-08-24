--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package BayernFloraABCD to version 6
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   function abcd__Unit_Gathering: 
--######   Provide empty string in "DateTime_ISODateTimeBegin" if year is missing
--#####################################################################################################################

-- in Package übertragen
/*
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

-- Start of difference for AddOn
-- adding columns if missing
ALTER TABLE "#project#"."ABCD_Unit_Gathering"
ADD COLUMN IF NOT EXISTS "CollectionSpecimenID" integer;

ALTER TABLE "#project#"."ABCD_Unit_Gathering"
ADD COLUMN IF NOT EXISTS "CollectionEventID" integer;

-- getting basic records
INSERT INTO "#project#"."ABCD_Unit_Gathering"(
            "ID",
			"Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "CollectionSpecimenID")
SELECT DISTINCT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    ''::character varying(50) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName",
    ''::character varying(254) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank",
    u."IdentificationUnitID", u."CollectionSpecimenID"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID" 
	 AND u."CollectionSpecimenID" = up."CollectionSpecimenID";

-- getting CollectionEventID
CREATE TEMP TABLE ABCD__Unit_Gathering_CollectionEventID AS 
	SELECT "ID",
			"Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", g."CollectionSpecimenID", s."CollectionEventID"  
FROM "#project#"."ABCD_Unit_Gathering" AS g
     JOIN "#project#"."CacheCollectionSpecimen" AS s ON g."CollectionSpecimenID" = s."CollectionSpecimenID";

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering";

-- Refill including CollectionEventID
INSERT INTO "#project#"."ABCD_Unit_Gathering"(
            "ID",
			"Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "CollectionSpecimenID", "CollectionEventID")
SELECT "ID",
			"Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "CollectionSpecimenID", "CollectionEventID"
   FROM ABCD__Unit_Gathering_CollectionEventID;

-- remove temp table
DROP TABLE ABCD__Unit_Gathering_CollectionEventID;


-- getting data from collection event
-- drop table if existing
DROP TABLE IF EXISTS ABCD__Unit_Gathering_CollectionEvent;

CREATE TEMP TABLE ABCD__Unit_Gathering_CollectionEvent AS 
SELECT DISTINCT e."CollectionEventID",
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
        concat_ws('--',
            lpad(e."CollectionMonth"::text, 2, '0'),
            '-',
            lpad(e."CollectionMonth"::text, 2, '0')
        )::text
    -- Only Month (ISO 8601 workaround: --MM)
    WHEN e."CollectionMonth" IS NOT NULL THEN
        concat('--', lpad(e."CollectionMonth"::text, 2, '0'))::text
    -- No valid date components
    ELSE
        NULL::text
    END AS "DateTime_ISODateTimeBegin",
    case when es."CollectionEventID" is null then e."LocalityDescription" else '' end AS "LocalityText",
    "#project#".abcd__latitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    "#project#".abcd__longitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal"   
	FROM "#project#"."CacheCollectionEvent" e 
     LEFT JOIN "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID" es on e."CollectionEventID" = es."CollectionEventID";

-- unite data from unit and event
CREATE TEMP TABLE ABCD__Unit_Gathering_UnitAndEvent AS 
SELECT g."ID",
			g."Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            g."Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            g."IdentificationUnitID", g."CollectionSpecimenID", 
			e."CollectionEventID",
    e."Country_Name",
     e."DateTime_ISODateTimeBegin",
    e."LocalityText",
    e."SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    e."SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal"   
	FROM ABCD__Unit_Gathering_CollectionEvent e
	JOIN "#project#"."ABCD_Unit_Gathering" g ON  e."CollectionEventID" =  g."CollectionEventID";


	-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering";

-- Refill with united data
INSERT INTO "#project#"."ABCD_Unit_Gathering"(
            "ID",
			"Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "CollectionSpecimenID", 
			"CollectionEventID",
    "Country_Name",
     "DateTime_ISODateTimeBegin",
    "LocalityText",
    "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
	"SiteCoordinateSets_CoordinatesLatLong_SpatialDatum")
SELECT "ID",
			"Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "CollectionSpecimenID", 
			"CollectionEventID",
    "Country_Name",
     "DateTime_ISODateTimeBegin",
    "LocalityText",
    "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
	CASE
            WHEN "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal" IS NULL THEN NULL::text
            ELSE 'WGS84'::text
        END AS "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum"
   FROM ABCD__Unit_Gathering_UnitAndEvent;

-- remove temp table
DROP TABLE ABCD__Unit_Gathering_CollectionEvent;
DROP TABLE ABCD__Unit_Gathering_UnitAndEvent;

-- End of difference for AddOn

-- replaced and optimized part

---- insert data
--INSERT INTO "#project#"."ABCD_Unit_Gathering"(
--            "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
--            "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
--            "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
--            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
--            "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum")
--SELECT DISTINCT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
--    e."CountryCache" AS "Country_Name",
--     concat_ws('-'::text, COALESCE(e."CollectionYear"::character varying(4), '-'::character varying), lpad(e."CollectionMonth"::character varying(2)::text, 2, '0'::text), lpad(e."CollectionDay"::character varying(2)::text, 2, '0'::text))::character varying(10) AS "DateTime_ISODateTimeBegin",
--    e."LocalityDescription" AS "LocalityText",
--    "#project#".abcd__latitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
--    "#project#".abcd__longitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
--    ''::character varying(50) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName",
--    ''::character varying(254) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank",
--    u."IdentificationUnitID",
--        CASE
--            WHEN "#project#".abcd__latitude(e."CollectionEventID") IS NULL THEN NULL::text
--            ELSE 'WGS84'::text
--        END AS "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum"
--   FROM "#project#"."CacheIdentificationUnitInPart" up
--     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
--     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
--     JOIN "#project#"."CacheCollectionEvent" e ON e."CollectionEventID" = s."CollectionEventID";

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
*/

--#####################################################################################################################
--######   view public."ABCD_MeasurementOrFact": 
--######   Exclusion of stati listed in ABCD__BayernFlora_WrongStatusUnitID
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_MeasurementOrFact"
 AS
 SELECT "ID",
    "Parameter",
    "UnitOfMeasurement",
    "LowerValue",
    "MeasurementDateTime",
    "MeasuredBy",
    "IdentificationUnitID",
    "SpecimenPartID",
    "CollectionSpecimenID",
    "AnalysisID",
    "AnalysisNumber",
    "MeasurementOrFactReference"
   FROM "#project#"."ABCD_MeasurementOrFact" M
   WHERE NOT EXISTS
   (SELECT * FROM "#project#"."ABCD__BayernFlora_WrongStatusUnitID" W 
   WHERE M."IdentificationUnitID" = W."IdentificationUnitID" AND M."AnalysisID" = 2);

ALTER TABLE public."ABCD_MeasurementOrFact"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_MeasurementOrFact"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/MultiMediaObjects/MultiMediaObject/';

GRANT ALL ON TABLE public."ABCD_MeasurementOrFact" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_MeasurementOrFact" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."Parameter"
    IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/Parameter. Retrieved from table Analysis - DisplayText';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."UnitOfMeasurement"
    IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/UnitOfMeasurement. Retrieved from table Analysis - MeasurementUnit';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."LowerValue"
    IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/LowerValue. Retrieved from table IdentificationUnitAnalysis - AnalysisResult';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."MeasurementDateTime"
    IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasurementDateTime. Retrieved from table IdentificationUnitAnalysis - AnalysisDate.
Formatting rules:
-- Format: YYYY/MM/DD -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
-- Format: YYYY-MM-DD -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
-- Format: DD.MM.YYYY -> Convert to YYYY-MM-DDTHH:MM:SS (add T and 00:00:00)
-- Format: YYYY-MM-DD hh:mm:ss -> Convert to ISO 8601 with T separator
-- Format: YYYY-MM-DD hh:mm:ss.ttt -> Convert to ISO 8601 with T separator
-- Format: YYYY/MM/DD-YYYY/MM/DD -> Convert to YYYY-MM-DDTHH:MM:SS
-- Format: YYYY-MM-DD/YYYY-MM-DD -> Convert to YYYY-MM-DDTHH:MM:SS
-- Default: Returns the content of AnalysisDate without formatting.';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."MeasuredBy"
    IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasuredBy. Retrieved from table IdentificationUnitAnalysis - ResponsibleName';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."IdentificationUnitID"
    IS 'PK of table IdentificationUnitID';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."SpecimenPartID"
    IS 'PK of table CollectionSpecimenPart';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."AnalysisID"
    IS 'PK of table Analysis';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."AnalysisNumber"
    IS 'Retrieved from table IdentificationUnitAnalysis - AnalysisNumber';

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."MeasurementOrFactReference"
    IS 'ABCD entity to provide information about the analysis';



--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."PackageAddOn" SET "Version" = 6 WHERE "Package" = 'ABCD' AND "AddOn" = 'ABCD_BayernFlora'
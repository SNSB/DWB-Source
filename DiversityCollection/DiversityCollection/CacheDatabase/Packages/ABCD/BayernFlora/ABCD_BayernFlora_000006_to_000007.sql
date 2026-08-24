--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package BayernFloraABCD to version 7
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   New view ABCD__BayernFlora_EndangeredSpeciesBase         ###################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase"
 AS
 SELECT T."NameID"
   FROM public."TaxonSynonymy" T
   JOIN public."TaxonAnalysis" A ON T."NameID" = A."NameID" AND T."SourceView" = A."SourceView" AND A."AnalysisID" = 80;

ALTER VIEW "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase"
    OWNER TO "CacheAdmin";

GRANT ALL ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" TO "CacheUser";


--#####################################################################################################################
--######   New view ABCD__BayernFlora_EndangeredSpeciesNameID       ###################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID"
 AS
 SELECT b."NameID"
   FROM "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" b
UNION
 SELECT t."NameID"
   FROM "TaxonSynonymy" t
     JOIN "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" b ON t."AcceptedNameID" = b."NameID"
UNION
 SELECT t."NameID"
   FROM "TaxonSynonymy" t
     JOIN "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" b ON t."NameParentID" = b."NameID";

ALTER TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID"
    OWNER TO "CacheAdmin";

GRANT ALL ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID" TO "CacheUser";



--#####################################################################################################################
--######   New view ABCD__BayernFlora_EndangeredSpeciesEventID       ###################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID"
 AS
 SELECT s."CollectionEventID"
   FROM "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID" t,
    "#project#"."CacheCollectionSpecimen" s,
    "#project#"."CacheIdentification" i
  WHERE t."NameID" = i."NameID" AND i."CollectionSpecimenID" = s."CollectionSpecimenID";

ALTER TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID"
    OWNER TO "CacheAdmin";

GRANT ALL ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID" TO "CacheUser";




--#####################################################################################################################
--######   Adaption of function abcd__unit to fill ABCD__Unit_LastIdentification     ##################################
--######   Differences to main version: Explizit values in "Identification_Reference_ReferenceGUID",   ################
--######	"Identification_Reference_TitleCitation", "Identification_Reference_CitationDetail",       ################
--######	"Identification_Reference_URI"                                                             ################
--######	Restriction to "Identification_Taxon_ScientificName_Qualifier" NOT LIKE 'cf %'             ################
--######	AND NOT I."NameID" IS NULL                                                                 ################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__unit(
	)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$

begin

--Setting the role
SET ROLE "CacheAdmin";

-- cleaning the last identification table
TRUNCATE TABLE "#project#"."ABCD__Unit_LastIdentification";
-- filling the last identification table
INSERT INTO "#project#"."ABCD__Unit_LastIdentification"(
            "CollectionSpecimenID", "IdentificationUnitID", "IdentificationSequence")
SELECT DISTINCT I."CollectionSpecimenID", I."IdentificationUnitID", MAX(I."IdentificationSequence")
FROM "#project#"."CacheIdentification" AS I
GROUP BY I."CollectionSpecimenID", I."IdentificationUnitID";


-- cleaning the table
TRUNCATE TABLE "#project#"."ABCD_Unit";

-- insert Units without parts
-- Difference to ABCD: adding columns for Reference and restriction to qualifier missing or not like cf
INSERT INTO "#project#"."ABCD_Unit"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", 
            "InformalNameString",
            "RecordBasis", "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", "IdentificationUnitID",
			"Identification_Reference_ReferenceGUID",
			"Identification_Reference_TitleCitation", "Identification_Reference_CitationDetail",
			"Identification_Reference_URI")
SELECT DISTINCT A."ID",
    A."UnitGUID",
    case when A."SourceInstitutionID" is null then '' else A."SourceInstitutionID" end,
    A."SourceID",
    A."UnitID",
    A."DateLastEdited",
    A."Identification_Taxon_ScientificName_FullScientificName",
    A."Identification_Taxon_ScientificName_Qualifier",
    T."TaxonNameSinAuthor",
    A."RecordBasis",
    A."KindOfUnit",
    A."Identification_Taxon_HigherTaxonName",
    A."KindOfUnit_Language",
    A."HerbariumUnit_Exsiccatum",
    A."RecordURI",
    A."CollectionSpecimenID", 
    A."IdentificationUnitID",
	concat('http://services.snsb.info/DTNtaxonlists/rest/v0.1/names/DiversityTaxonNames_Plants/', I."NameID"::character varying)::character varying,
	'Taxon list of vascular plants from Bavaria, Germany compiled in the context of the BFL Project'::character varying, 
    I."NameID",
	'http://www.diversitymobile.net/wiki/About_%22Taxon_list_of_vascular_plants_from_Bavaria,_Germany_compiled_in_the_context_of_the_BFL_project%22'::character varying

   FROM "#project#"."ABCD__UnitNoPart" AS A
   JOIN "#project#"."CacheIdentificationUnit" AS U
   ON U."IdentificationUnitID" = A."IdentificationUnitID"
   AND (A."Identification_Taxon_ScientificName_Qualifier" NOT LIKE 'cf %'
		OR A."Identification_Taxon_ScientificName_Qualifier" IS NULL)
   JOIN "#project#"."CacheIdentification" AS I 
   ON U."IdentificationUnitID" = I."IdentificationUnitID"
   AND U."LastIdentificationCache" = I."TaxonomicName"
   AND NOT I."NameID" IS NULL
   JOIN "#project#"."ABCD__Unit_LastIdentification" AS L
   ON L."IdentificationUnitID" = I."IdentificationUnitID"
   AND L."IdentificationSequence" = I."IdentificationSequence"
   AND L."CollectionSpecimenID" = I."CollectionSpecimenID"
   JOIN public."TaxonSynonymy" T ON T."NameURI" = I."NameURI";
   
-- insert Units with parts 
-- Difference to ABCD: adding columns for Reference and restriction to qualifier missing or not like cf
INSERT INTO "#project#"."ABCD_Unit"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", 
            "InformalNameString", 
            "RecordBasis", "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID", 
			"Identification_Reference_ReferenceGUID",
			"Identification_Reference_TitleCitation", "Identification_Reference_CitationDetail",
			"Identification_Reference_URI")
SELECT DISTINCT A."ID",
    A."UnitGUID",
    case when A."SourceInstitutionID" is null then '' else A."SourceInstitutionID" end,
    A."SourceID",
    A."UnitID",
    A."DateLastEdited",
    A."Identification_Taxon_ScientificName_FullScientificName",
    A."Identification_Taxon_ScientificName_Qualifier",
    T."TaxonNameSinAuthor",
    A."RecordBasis",
    A."KindOfUnit",
    A."Identification_Taxon_HigherTaxonName",
    A."KindOfUnit_Language",
    A."HerbariumUnit_Exsiccatum",
    A."RecordURI",
    A."CollectionSpecimenID", 
    A."IdentificationUnitID",
	concat('http://services.snsb.info/DTNtaxonlists/rest/v0.1/names/DiversityTaxonNames_Plants/', I."NameID"::character varying)::character varying,
	'Taxon list of vascular plants from Bavaria, Germany compiled in the context of the BFL Project'::character varying, I."NameID",
	'http://www.diversitymobile.net/wiki/About_%22Taxon_list_of_vascular_plants_from_Bavaria,_Germany_compiled_in_the_context_of_the_BFL_project%22'::character varying
   FROM "#project#"."ABCD__UnitPart" AS A
   JOIN "#project#"."CacheIdentification" AS I
   ON A."IdentificationUnitID" = I."IdentificationUnitID" AND A."CollectionSpecimenID" = I."CollectionSpecimenID"
   JOIN "#project#"."CacheIdentificationUnit" AS U
   ON U."IdentificationUnitID" = I."IdentificationUnitID" AND U."CollectionSpecimenID" = I."CollectionSpecimenID"
   AND U."LastIdentificationCache" = I."TaxonomicName"
   AND (A."Identification_Taxon_ScientificName_Qualifier" NOT LIKE 'cf %'
		OR A."Identification_Taxon_ScientificName_Qualifier" IS NULL)
   AND NOT I."NameID" IS NULL
   JOIN "#project#"."ABCD__Unit_LastIdentification" AS L
   ON L."IdentificationUnitID" = I."IdentificationUnitID"
   AND L."IdentificationSequence" = I."IdentificationSequence"
   AND L."CollectionSpecimenID" = I."CollectionSpecimenID"
   JOIN public."TaxonSynonymy" T ON T."NameURI" = I."NameURI";
   
-- cleaning the source tables
TRUNCATE TABLE "#project#"."ABCD__UnitPart";
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

  
end;
$BODY$;

ALTER FUNCTION "#project#".abcd__unit()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unit() TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unit() TO PUBLIC;


--#####################################################################################################################
--######   function abcd__Unit_Gathering differing from main version: 
--######   Endangered species without locality text
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
    "#project#".abcd__latitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    "#project#".abcd__longitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
    "#project#".abcd__coordinatemethod(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod",
    "#project#".abcd__coordinateerrordistanceinmeters(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters",
    ''::character varying(50) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName",
    ''::character varying(254) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank",
    u."IdentificationUnitID",
        CASE
            WHEN "#project#".abcd__latitude(e."CollectionEventID") IS NULL THEN NULL::text
            ELSE 'WGS84'::text
        END AS "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum",
        s."CollectionSpecimenID",
        e."CollectionEventID"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEvent" e ON e."CollectionEventID" = s."CollectionEventID"
     LEFT JOIN "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID" es on e."CollectionEventID" = es."CollectionEventID"; -- Linking to the table of endangered species events to determine if the locality description should be provided

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
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."PackageAddOn" SET "Version" = 7 WHERE "Package" = 'ABCD' AND "AddOn" = 'ABCD_BayernFlora'
--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package BayernFloraABCD to version 3
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   abcd__unit: adding new columns for reference  ##############################################################
--######   Difference to ABCD: 
--######   adding columns  Identification_Reference_ReferenceGUID,Identification_Reference_TitleCitation,
--######                   Identification_Reference_CitationDetail,Identification_Reference_URI
--######   restriction to qualifier missing or not like cf, restriction to linked identifications
--#####################################################################################################################
	
CREATE OR REPLACE FUNCTION "#project#".abcd__unit()
    RETURNS void
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- cleaning the table
TRUNCATE TABLE "#project#"."ABCD_Unit";

-- insert Units without parts
-- Difference to ABCD: adding columns for Reference and restriction to qualifier missing or not like cf
INSERT INTO "#project#"."ABCD_Unit"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID"
			, "Identification_Reference_ReferenceGUID",
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
   FROM "#project#"."ABCD__UnitNoPart" AS A
   JOIN "#project#"."CacheIdentificationUnit" AS U
   ON U."IdentificationUnitID" = A."IdentificationUnitID"
   AND (A."Identification_Taxon_ScientificName_Qualifier" NOT LIKE 'cf %'
		OR A."Identification_Taxon_ScientificName_Qualifier" IS NULL)
   JOIN "#project#"."CacheIdentification" AS I 
   ON U."IdentificationUnitID" = I."IdentificationUnitID"
   AND U."LastIdentificationCache" = I."TaxonomicName"
   AND NOT I."NameID" IS NULL;
   
-- insert Units with parts 
-- Difference to ABCD: adding columns for Reference and restriction to qualifier missing or not like cf
INSERT INTO "#project#"."ABCD_Unit"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
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
   , "#project#"."CacheIdentification" AS I
   , "#project#"."CacheIdentificationUnit" AS U
   WHERE A."IdentificationUnitID" = I."IdentificationUnitID"
   AND U."IdentificationUnitID" = I."IdentificationUnitID"
   AND U."LastIdentificationCache" = I."TaxonomicName"
   AND (A."Identification_Taxon_ScientificName_Qualifier" NOT LIKE 'cf %'
		OR A."Identification_Taxon_ScientificName_Qualifier" IS NULL)
   AND NOT I."NameID" IS NULL;
   
-- cleaning the source tables
TRUNCATE TABLE "#project#"."ABCD__UnitPart";
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";
  
end;
$BODY$;

ALTER FUNCTION "#project#".abcd__unit() OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unit() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unit() TO "CacheUser";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unit() TO PUBLIC;


--#####################################################################################################################
--######   abcd__measurementorfact: 
--######   Difference to ABCD: 
--######   adding new column MeasurementOrFactReference for link to explanation site   
--######   Type of analysis + (AnalysisNumber) as Parameter e.g. Floristischer Status (2)
--#####################################################################################################################


---- No difference to ABCD: Column MeasurementOrFactReference for explanation of floristic status meanwhile in main version
	

--CREATE OR REPLACE FUNCTION "#project#".abcd__measurementorfact()
--RETURNS void
--    LANGUAGE 'plpgsql'
--    COST 100
--    VOLATILE 
--AS $BODY$
--begin

----Setting the role
--SET ROLE "CacheAdmin";

---- Cleaning the table
--TRUNCATE TABLE "#project#"."ABCD_MeasurementOrFact";

---- Removing the indices
--ALTER TABLE "#project#"."ABCD_MeasurementOrFact" DROP CONSTRAINT IF EXISTS "ABCD_MeasurementOrFact_pkey";
--DROP INDEX IF EXISTS "#project#".abcd_measurementorfact_id;

---- insert the data
--INSERT INTO "#project#"."ABCD_MeasurementOrFact"(
--            "ID", "Parameter", "UnitOfMeasurement", "LowerValue", 
--			"MeasurementDateTime",   "MeasuredBy", 
--			"IdentificationUnitID", "SpecimenPartID", "CollectionSpecimenID",  "AnalysisID", "AnalysisNumber"
--			, "MeasurementOrFactReference")
--SELECT concat(ua."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
--    a."DisplayText" AS "Parameter", a."MeasurementUnit" AS "UnitOfMeasurement", ua."AnalysisResult" AS "LowerValue",
--    ua."AnalysisDate" AS "MeasurementDateTime", ua."ResponsibleName" AS "MeasuredBy", 
--    ua."IdentificationUnitID",  up."SpecimenPartID", ua."CollectionSpecimenID", ua."AnalysisID", ua."AnalysisNumber"
--	, a."AnalysisURI" AS "MeasurementOrFactReference"
--   FROM "#project#"."CacheIdentificationUnitInPart" up
--     RIGHT JOIN "#project#"."CacheIdentificationUnitAnalysis" ua ON ua."IdentificationUnitID" = up."IdentificationUnitID" and ua."CollectionSpecimenID" = up."CollectionSpecimenID"
--     JOIN "#project#"."CacheAnalysis" a ON a."AnalysisID" = ua."AnalysisID";

---- inserting the indices
--ALTER TABLE "#project#"."ABCD_MeasurementOrFact"
--  ADD CONSTRAINT "ABCD_MeasurementOrFact_pkey" PRIMARY KEY("ID", "AnalysisID", "AnalysisNumber");
  
--CREATE INDEX abcd_measurementorfact_id
--  ON "#project#"."ABCD_MeasurementOrFact"
--  USING btree
--  ("ID" COLLATE pg_catalog."default");

     
--end;
--$BODY$;

--ALTER FUNCTION "#project#".abcd__measurementorfact()
--    OWNER TO "CacheAdmin";

--GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO "CacheUser";
--GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO "CacheAdmin";
--GRANT EXECUTE ON FUNCTION "#project#".abcd__measurementorfact() TO PUBLIC;



--#####################################################################################################################
--######   abcd__unitpart: SourceInstitutionID from CollectionAcronym resp. CollectionName   ##########################
--######   Difference to ABCD: 
--######   Exclusion of IdentificationUnits with wrong status
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__unitpart()
 RETURNS void
 LANGUAGE plpgsql
AS $function$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- cleaning the table ABCD__UnitPart
TRUNCATE TABLE "#project#"."ABCD__UnitPart";

-- removing key
ALTER TABLE "#project#"."ABCD__UnitPart" DROP CONSTRAINT IF EXISTS "ABCD__UnitPart_pkey";

-- insert Units with parts
-- Difference to ABCD: Exclusion of units with wrong status
INSERT INTO "#project#"."ABCD__UnitPart"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", "IdentificationUnitID")
SELECT DISTINCT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
        CASE
            WHEN up."StableIdentifier" IS NULL THEN u."StableIdentifier"
            ELSE up."StableIdentifier"
        END::text AS "UnitGUID",
    CASE WHEN c."CollectionAcronym" <> '' THEN c."CollectionAcronym" ELSE c."CollectionName" END,
    m."ProjectTitleCode" AS "SourceID",
    btrim(concat(
        CASE
            WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
            ELSE u."CollectionSpecimenID"::character varying
        END, ' / ', u."IdentificationUnitID"::character varying,
        CASE
            WHEN p."AccessionNumber"::text <> ''::text THEN concat(' / ', p."AccessionNumber")
            ELSE
            CASE
                WHEN up."SpecimenPartID"::text <> ''::text THEN concat(' / ', up."SpecimenPartID"::character varying)
                ELSE ''::text
            END
        END)) AS "UnitID",
    s."LogUpdatedWhen" AS "DateLastEdited",
    btrim(u."LastIdentificationCache")::character varying(255) AS "Identification_Taxon_ScientificName_FullScientificName",
    r."RecordBasis",
    CASE WHEN p."MaterialCategory" IS NULL THEN u."RetrievalType" ELSE p."MaterialCategory"::character varying(254) END AS "KindOfUnit",
    k."Kingdom" AS "Identification_Taxon_HigherTaxonName",
    'en'::character varying(2) AS "KindOfUnit_Language",
    concat(s."ExsiccataAbbreviation", case when u."ExsiccataNumber"::text <> ''::text
		   THEN ' Exs. no. ' else '' end, u."ExsiccataNumber") AS "HerbariumUnit_Exsiccatum",
        CASE
            WHEN m."RecordUri"::text ~~ 'http://biocase%'::text THEN concat(m."RecordUri", btrim(concat(
            CASE
                WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
                ELSE u."CollectionSpecimenID"::character varying
            END, ' / ', u."IdentificationUnitID"::character varying,
            CASE
                WHEN up."SpecimenPartID"::text <> ''::text THEN 
		    CASE 
			WHEN p."AccessionNumber"::text <> ''::text THEN
			    concat(' / ', p."AccessionNumber"::character varying)
			ELSE
			    concat(' / ', up."SpecimenPartID"::character varying)
		    END
                ELSE ''::text
            END)))
            ELSE
            CASE
                WHEN m."RecordUri"::text <> ''::text THEN concat(m."RecordUri", s."CollectionSpecimenID"::character varying)
                ELSE NULL::text
            END
        END AS "RecordURI",
    u."CollectionSpecimenID",
    u."IdentificationUnitID"
   FROM "#project#"."CacheCollectionSpecimenPart" p
     JOIN "#project#"."CacheIdentificationUnitInPart" up ON p."SpecimenPartID" = up."SpecimenPartID"
     JOIN "#project#"."ABCD__RecordBasis" r ON r."IdentificationUnitID" = up."IdentificationUnitID" AND r."SpecimenPartID" = up."SpecimenPartID"
     JOIN "#project#"."CacheIdentificationUnit" u  ON r."IdentificationUnitID" = u."IdentificationUnitID"
     JOIN "#project#"."ABCD__Kingdom" k ON u."IdentificationUnitID" = k."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON s."CollectionSpecimenID" = u."CollectionSpecimenID"
     JOIN "#project#"."CacheCollection" c ON c."CollectionID" = p."CollectionID"
     CROSS JOIN "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid()
 WHERE u."IdentificationUnitID" NOT IN (SELECT "IdentificationUnitID" FROM "#project#"."ABCD__BayernFlora_WrongStatusUnitID");

-- adding the key
ALTER TABLE "#project#"."ABCD__UnitPart"
  ADD CONSTRAINT "ABCD__UnitPart_pkey" PRIMARY KEY("ID");

end;
$function$
;

COMMENT ON FUNCTION "#project#".abcd__unitpart() 
	IS 'Transfer data into table ABCD__UnitPart';

	


--#####################################################################################################################
--######   function abcd__Unit_Gathering: optimized  ##################################################################
--######   Difference to ABCD: 
--######   Exclusion of endangered species
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
CREATE TEMP TABLE ABCD__Unit_Gathering_CollectionEvent AS 
SELECT DISTINCT e."CollectionEventID",
    e."CountryCache" AS "Country_Name",
     concat_ws('-'::text, COALESCE(e."CollectionYear"::character varying(4), '-'::character varying), lpad(e."CollectionMonth"::character varying(2)::text, 2, '0'::text), 
     lpad(e."CollectionDay"::character varying(2)::text, 2, '0'::text))::character varying(10) AS "DateTime_ISODateTimeBegin",
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




--#####################################################################################################################
--######   Comments on ABCD views in public   #########################################################################
--#####################################################################################################################

COMMENT ON VIEW public."ABCD_Unit"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/ restricted to taxa with missing qualifier or a qualifier unlike cf ...';


COMMENT ON COLUMN public."ABCD_Unit_Gathering"."LocalityText"
    IS 'ABCD: Unit/Gathering/LocalityText. Retrieved from column LocalityDescription in table CollectionEvent excluding endagered species';	

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."PackageAddOn" SET "Version" = 4 WHERE "Package" = 'ABCD' AND "AddOn" = 'ABCD_BayernFlora'


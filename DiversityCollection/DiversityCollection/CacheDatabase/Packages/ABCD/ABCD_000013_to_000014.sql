--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 14
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   New table ABCD__Unit_LastIdentification                  ###################################################
--######       contains last identification of an unit              ###################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."ABCD__Unit_LastIdentification"
(
    "CollectionSpecimenID" integer NOT NULL,
    "IdentificationUnitID" integer NOT NULL,
    "IdentificationSequence" smallint NOT NULL DEFAULT 1,
    CONSTRAINT "ABCD__Unit_LastIdentification_pkey" PRIMARY KEY ("CollectionSpecimenID", "IdentificationUnitID")
);

COMMENT ON TABLE "#project#"."ABCD__Unit_LastIdentification"
    IS 'Last identification of a unit';


GRANT ALL ON TABLE "#project#"."ABCD__Unit_LastIdentification" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."ABCD__Unit_LastIdentification" TO "CacheUser";

--#####################################################################################################################
--######   New column InformalNameString in ABCD_Unit               ###################################################
--#####################################################################################################################

ALTER TABLE IF EXISTS "#project#"."ABCD_Unit"
    ADD COLUMN "InformalNameString" character varying(255) COLLATE pg_catalog."default";

COMMENT ON COLUMN "#project#"."ABCD_Unit"."InformalNameString"
    IS 'Informal name of the unit corresponding to name without author';


--#####################################################################################################################
--######   Adaption of function abcd__unit to fill ABCD__Unit_LastIdentification     ##################################
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
	''::character varying,
	''::character varying, 
    I."NameID",
	''::character varying
   FROM "#project#"."ABCD__UnitNoPart" AS A
   JOIN "#project#"."CacheIdentificationUnit" AS U
   ON U."IdentificationUnitID" = A."IdentificationUnitID"
   JOIN "#project#"."CacheIdentification" AS I 
   ON U."IdentificationUnitID" = I."IdentificationUnitID"
   AND U."LastIdentificationCache" = I."TaxonomicName"
   JOIN "#project#"."ABCD__Unit_LastIdentification" AS L
   ON L."IdentificationUnitID" = I."IdentificationUnitID"
   AND L."IdentificationSequence" = I."IdentificationSequence"
   AND L."CollectionSpecimenID" = I."CollectionSpecimenID"
   LEFT OUTER JOIN public."TaxonSynonymy" T ON T."NameURI" = I."NameURI";
   
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
	''::character varying,
	''::character varying, 
    I."NameID",
	''::character varying
   FROM "#project#"."ABCD__UnitPart" AS A
   JOIN "#project#"."CacheIdentification" AS I
   ON A."IdentificationUnitID" = I."IdentificationUnitID" AND A."CollectionSpecimenID" = I."CollectionSpecimenID"
   JOIN "#project#"."CacheIdentificationUnit" AS U
   ON U."IdentificationUnitID" = I."IdentificationUnitID" AND U."CollectionSpecimenID" = I."CollectionSpecimenID"
   AND U."LastIdentificationCache" = I."TaxonomicName"
   JOIN "#project#"."ABCD__Unit_LastIdentification" AS L
   ON L."IdentificationUnitID" = I."IdentificationUnitID"
   AND L."IdentificationSequence" = I."IdentificationSequence"
   AND L."CollectionSpecimenID" = I."CollectionSpecimenID"
   LEFT OUTER JOIN public."TaxonSynonymy" T ON T."NameURI" = I."NameURI";
   
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
--######   Include new column "InformalNameString" in public.ABCD_Unit         ########################################
--#####################################################################################################################

DROP VIEW public."ABCD_Unit";


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
    "Identification_Reference_TitleCitation"
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

COMMENT ON COLUMN public."ABCD_Unit"."InformalNameString"
    IS 'Corresponds to ABCD TaxonIdentified/InformalNameString. Derived from TaxonNameSinAuthor';


--#####################################################################################################################
--######   New View public ABCD_Unit_Identification_References      ###################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit_Identification_References"
 AS
  SELECT U."ID",
    R."ReferenceDetails" AS "CitationDetail",
    R."ReferenceID" AS "ReferenceGUID",
    R."ReferenceTitle" AS "TitleCitation",
    R."ReferenceURI" AS "URI",
    R."CollectionSpecimenID",
    R."IdentificationUnitID"
FROM "#project#"."ABCD_Unit" U
JOIN "#project#"."CacheCollectionSpecimenReference" R
ON R."CollectionSpecimenID" = U."CollectionSpecimenID" AND R."IdentificationUnitID" = U."IdentificationUnitID"; 


ALTER TABLE public."ABCD_Unit_Identification_References"
    OWNER TO "CacheAdmin";
GRANT ALL ON TABLE public."ABCD_Unit_Identification_References" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_Identification_References" TO "CacheUser";

COMMENT ON VIEW public."ABCD_Unit_Identification_References"
    IS 'ABCD entity /Unit/Identification/References. Retrieved from CollectionSpecimenReference linked to last identification';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."CitationDetail"
    IS 'ABCD entity /Unit/Identification/References.Identification/Reference/CitationDetail. Retrieved from CacheCollectionSpecimenReference.ReferenceDetails. Contains NameID or ExternalNameURI from DiversityTaxonNames';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."ReferenceGUID"
    IS 'ABCD entity /Unit/Identification/References.Identification/Reference/ReferenceGUID. Retrieved from CacheCollectionSpecimenReference.ReferenceID';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."TitleCitation"
    IS 'ABCD entity /Unit/Identification/References.Identification/Reference/TitleCitation. Retrieved from CacheCollectionSpecimenReference.ReferenceTitle  ';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."URI"
    IS 'ABCD entity /Unit/Identification/References.Identification/Reference/URI. Retrieved from CacheCollectionSpecimenReference.ReferenceURI';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."CollectionSpecimenID"
    IS 'Retrieved from CacheCollectionSpecimenReference.CollectionSpecimenID';

COMMENT ON COLUMN public."ABCD_Unit_Identification_References"."IdentificationUnitID"
    IS 'Retrieved from CacheCollectionSpecimenReference.IdentificationUnitID';


--#####################################################################################################################
--######   New columns in ABCD_Unit_Gathering:                      ###################################################
--######       Code, DateTime_ISODateTimeEnd, CoordinateMethod      ###################################################
--#####################################################################################################################

ALTER TABLE IF EXISTS "#project#"."ABCD_Unit_Gathering"
    ADD COLUMN "DateTime_ISODateTimeEnd" character varying(10) COLLATE pg_catalog."default";

COMMENT ON COLUMN "#project#"."ABCD_Unit_Gathering"."DateTime_ISODateTimeEnd"
    IS 'End date of a collection event in ISO format';


ALTER TABLE IF EXISTS "#project#"."ABCD_Unit_Gathering"
    ADD COLUMN "Code" character varying(50);

COMMENT ON COLUMN "#project#"."ABCD_Unit_Gathering"."Code"
    IS 'Retrieved from CollectionEvent.CollectorsEventNumber';


ALTER TABLE IF EXISTS "#project#"."ABCD_Unit_Gathering"
    ADD COLUMN "SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod" character varying(50);

COMMENT ON COLUMN "#project#"."ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod"
    IS 'Retrieved from CollectionEventLocalisation.RecordingMethod';


ALTER TABLE IF EXISTS "#project#"."ABCD_Unit_Gathering"
    ADD COLUMN "SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters" character varying(50);

COMMENT ON COLUMN "#project#"."ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters"
    IS 'Retrieved from CollectionEventLocalisation.LocationAccuracy';


ALTER TABLE "#project#"."ABCD_Unit_Gathering"
    ADD COLUMN IF NOT EXISTS "CollectionSpecimenID" integer;

COMMENT ON COLUMN "#project#"."ABCD_Unit_Gathering"."CollectionSpecimenID"
    IS 'Retrieved from CollectionSpecimen.CollectionSpecimenID';


ALTER TABLE "#project#"."ABCD_Unit_Gathering"
    ADD COLUMN IF NOT EXISTS "CollectionEventID" integer;

COMMENT ON COLUMN "#project#"."ABCD_Unit_Gathering"."CollectionEventID"
    IS 'Retrieved from CollectionSpecimen.CollectionEventID';

--#####################################################################################################################
--######   New function abcd__coordinatemethod                      ###################################################
--######       Retrieval of coordinate method according to available data and sequence of localisation systems      ###
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__coordinatemethod(
	int)
    RETURNS character varying(50)
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$

declare CoordinateMethod character varying(50);
declare LocalisationSystemID int;

begin

--Setting the role
SET ROLE "CacheAdmin";

select L."LocalisationSystemID" 
from "#project#"."CacheLocalisationSystem" L, "#project#"."CacheCollectionEventLocalisation" AS E  
where L."LocalisationSystemID" = E."LocalisationSystemID"  AND NOT E."LocalisationSystemID" IS NULL
and E."CollectionEventID" = $1
order by L."Sequence" LIMIT 1 
into LocalisationSystemID;

select E."RecordingMethod" 
from "#project#"."CacheCollectionEventLocalisation" AS E 
where E."LocalisationSystemID" = LocalisationSystemID and E."CollectionEventID" = $1 LIMIT 1
into CoordinateMethod;

return CoordinateMethod;
END;
$BODY$;

ALTER FUNCTION "#project#".abcd__coordinatemethod(int)
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__coordinatemethod(int) TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__coordinatemethod(int) TO "CacheUser";

GRANT EXECUTE ON FUNCTION "#project#".abcd__coordinatemethod(int) TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__coordinatemethod(int)
    IS 'Retrieval of coordinate method according to available data and sequence of localisation systems in table CacheLocalisationSystem';


--#####################################################################################################################
--######   New function abcd__coordinateerrordistanceinmeters                      ####################################
--######       Retrieval of coordinate method according to available data and sequence of localisation systems      ###
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__coordinateerrordistanceinmeters(
	int)
    RETURNS character varying(50)
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$

declare CoordinateErrorDistanceInMeters character varying(50);
declare LocalisationSystemID int;

begin

--Setting the role
SET ROLE "CacheAdmin";

select L."LocalisationSystemID" 
from "#project#"."CacheLocalisationSystem" L, "#project#"."CacheCollectionEventLocalisation" AS E  
where L."LocalisationSystemID" = E."LocalisationSystemID"  AND NOT E."LocalisationSystemID" IS NULL
and E."CollectionEventID" = $1
order by L."Sequence" LIMIT 1 
into LocalisationSystemID;

/*
-- Konversion to numeric is not possible because of the different decimal separators in the data. Therefore, the value is returned as string.
select NULLIF(
        regexp_replace(E."LocationAccuracy", '[^0-9.,]+', '', 'g'),
        ''
    )::numeric 
from "#project#"."CacheCollectionEventLocalisation" AS E 
where E."LocalisationSystemID" = LocalisationSystemID and E."CollectionEventID" = $1 LIMIT 1
into CoordinateErrorDistanceInMeters;
*/

select E."LocationAccuracy"
from "#project#"."CacheCollectionEventLocalisation" AS E 
where E."LocalisationSystemID" = LocalisationSystemID and E."CollectionEventID" = $1 LIMIT 1
into CoordinateErrorDistanceInMeters;

return CoordinateErrorDistanceInMeters;
END;
$BODY$;

ALTER FUNCTION "#project#".abcd__coordinateerrordistanceinmeters(int)
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__coordinateerrordistanceinmeters(int) TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__coordinateerrordistanceinmeters(int) TO "CacheUser";

GRANT EXECUTE ON FUNCTION "#project#".abcd__coordinateerrordistanceinmeters(int) TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__coordinateerrordistanceinmeters(int)
    IS 'Retrieval of coordinate error distance in meters according to available data and sequence of localisation systems in table CacheLocalisationSystem';



--#####################################################################################################################
--######   Adaption of function abcd__unit_gathering     ##############################################################
--######       for new columns Code, DateTime_ISODateTimeEnd, CoordinateMethod      ###################################
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
    e."LocalityDescription" AS "LocalityText",
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
--######   Adaption of view abcd__unit_gathering         ##############################################################
--######       for new columns Code, DateTime_ISODateTimeEnd, CoordinateMethod      ###################################
--#####################################################################################################################

DROP VIEW public."ABCD_Unit_Gathering";

CREATE OR REPLACE VIEW public."ABCD_Unit_Gathering"
 AS
 SELECT "ID",
    "Country_Name",
    "ISO3166Code",
    "DateTime_ISODateTimeBegin",
    "DateTime_ISODateTimeEnd",
    "Code",
    "LocalityText",
    "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
    "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum",
    "SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod",
    "SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters",
    "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName",
    "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank",
    "IdentificationUnitID",
    "CollectionSpecimenID",
    "CollectionEventID"
   FROM "#project#"."ABCD_Unit_Gathering" "G";

ALTER TABLE public."ABCD_Unit_Gathering"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Unit_Gathering"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering';

GRANT ALL ON TABLE public."ABCD_Unit_Gathering" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_Gathering" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."Country_Name"
    IS 'ABCD: Unit/Gathering/Country/Name. Retrieved from column CountryCache in table CollectionEvent';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."ISO3166Code"
    IS 'ABCD: Unit/Gathering/Country/ISO3166Code. Retrieved from DiversityGazetteer according to column CountryCache in table CollectionEvent';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."DateTime_ISODateTimeBegin"
    IS 'ABCD: Unit/Gathering/DateTime/ISODateTimeBegin. Retrieved from columns CollectionYear, CollectionMonth, and CollectionDay in table CollectionEvent. 
Formatting rules:
-- Full date: Year, Month, and Day -> Convert to YYYY-MM-DD
-- Year and Month only -> Convert to YYYY-MM
-- Year only -> Convert to YYYY
-- Month and Day only -> --MM-DD
-- Only Month --MM
-- No valid date components -> NULL::text';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."DateTime_ISODateTimeEnd"
    IS 'ABCD: Unit/Gathering/DateTime/ISODateTimeEnd. Retrieved from columns CollectionEndYear, CollectionEndMonth, and CollectionEndDay in table CollectionEvent. 
Formatting rules:
-- Full date: Year, Month, and Day -> Convert to YYYY-MM-DD
-- Year and Month only -> Convert to YYYY-MM
-- Year only -> Convert to YYYY
-- Month and Day only -> --MM-DD
-- Only Month --MM
-- No valid date components -> NULL::text';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."Code"
    IS 'ABCD: Unit/Gathering/Code. Retrieved from column CollectorsEventNumber in table CollectionEvent';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."LocalityText"
    IS 'ABCD: Unit/Gathering/LocalityText. Retrieved from column LocalityDescription in table CollectionEvent';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal"
    IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinates/CoordinatesLatLong/LatitudeDecimal. Retrieved from column AverageLatitudeCache in table CollectionEventLocalisation depending on the sequence defined for the project';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal"
    IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinates/CoordinatesLatLong/LongitudeDecimal. Retrieved from column AverageLongitudeCache in table CollectionEventLocalisation depending on the sequence defined for the project';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_CoordinateMethod"
    IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinates/CoordinatesLatLong/CoordinateMethod. Retrieved from column RecordingMethod in table CollectionEventLocalisation depending on the sequence defined for the project';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_CoordinateErrorInMeters"
    IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinates/CoordinatesLatLong/CoordinateErrorDistanceInMeters. Retrieved from column LocationAccuracy in table CollectionEventLocalisation depending on the sequence defined for the project';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName"
    IS 'ABCD: Unit/Gathering/Synecology/AssociatedTaxa/TaxonIdentified/HigherTaxa/HigherTaxon/HigherTaxonName. Obsolete';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank"
    IS 'ABCD: Unit/Gathering/Synecology/AssociatedTaxa/TaxonIdentified/HigherTaxa/HigherTaxon/HigherTaxonRank. Obsolete';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."IdentificationUnitID"
    IS 'Retrieved from column IdentificationUnitID in table IdentificationUnit';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_SpatialDatum"
    IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinates/CoordinatesLatLong/SpatialDatum. Retrieved from View = WGS84';
COMMENT ON COLUMN public."ABCD_Unit_Gathering"."CollectionSpecimenID"
    IS 'Retrieved from column CollectionSpecimenID in table CollectionSpecimen';

COMMENT ON COLUMN public."ABCD_Unit_Gathering"."CollectionEventID"
    IS 'Retrieved from column CollectionEventID in table CollectionEvent';


--#####################################################################################################################
--######   New column in ABCD_Unit_Gathering_CoordinatesGrid:                      ####################################
--######        GeoreferenceRemarks                                                ####################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit_Gathering_CoordinatesGrid"
 AS
 SELECT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    'TK25'::character varying(50) AS "GridCellSystem",
    e."Location1" AS "GridCellCode",
    "substring"(e."Location2"::text, 1, 1)::character varying(255) AS "GridQualifier",
    e."RecordingMethod" AS "Method",
    e."LocationAccuracy" AS "GeoreferenceRemarks"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEventLocalisation" e ON e."CollectionEventID" = s."CollectionEventID" AND e."LocalisationSystemID" = 3;

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
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 14 WHERE "Package" = 'ABCD'
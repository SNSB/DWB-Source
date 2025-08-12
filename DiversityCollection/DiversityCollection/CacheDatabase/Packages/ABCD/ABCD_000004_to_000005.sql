--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 5
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   abcd__unit() - SourceInstitutionID: set to '' if NULL   ####################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__unit(
	)
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
INSERT INTO "#project#"."ABCD_Unit"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID")
 SELECT "ABCD__UnitNoPart"."ID",
    "ABCD__UnitNoPart"."UnitGUID",
    case when "ABCD__UnitNoPart"."SourceInstitutionID" IS NULL then '' else "ABCD__UnitNoPart"."SourceInstitutionID" end,
    "ABCD__UnitNoPart"."SourceID",
    "ABCD__UnitNoPart"."UnitID",
    "ABCD__UnitNoPart"."DateLastEdited",
    "ABCD__UnitNoPart"."Identification_Taxon_ScientificName_FullScientificName",
    "ABCD__UnitNoPart"."Identification_Taxon_ScientificName_Qualifier",
    "ABCD__UnitNoPart"."RecordBasis",
    "ABCD__UnitNoPart"."KindOfUnit",
    "ABCD__UnitNoPart"."Identification_Taxon_HigherTaxonName",
    "ABCD__UnitNoPart"."KindOfUnit_Language",
    "ABCD__UnitNoPart"."HerbariumUnit_Exsiccatum",
    "ABCD__UnitNoPart"."RecordURI",
    "ABCD__UnitNoPart"."CollectionSpecimenID", 
    "ABCD__UnitNoPart"."IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart";
   
-- insert Units with parts 
INSERT INTO "#project#"."ABCD_Unit"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID")
 SELECT "ABCD__UnitPart"."ID",
    "ABCD__UnitPart"."UnitGUID",
    "ABCD__UnitPart"."SourceInstitutionID",
    "ABCD__UnitPart"."SourceID",
    "ABCD__UnitPart"."UnitID",
    "ABCD__UnitPart"."DateLastEdited",
    "ABCD__UnitPart"."Identification_Taxon_ScientificName_FullScientificName",
    "ABCD__UnitPart"."Identification_Taxon_ScientificName_Qualifier",
    "ABCD__UnitPart"."RecordBasis",
    "ABCD__UnitPart"."KindOfUnit",
    "ABCD__UnitPart"."Identification_Taxon_HigherTaxonName",
    "ABCD__UnitPart"."KindOfUnit_Language",
    "ABCD__UnitPart"."HerbariumUnit_Exsiccatum",
    "ABCD__UnitPart"."RecordURI",
    "ABCD__UnitPart"."CollectionSpecimenID", 
    "ABCD__UnitPart"."IdentificationUnitID"
   FROM "#project#"."ABCD__UnitPart";
   
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
--######   ABCD_TechnicalContact from DA  and without restriction to one contact  #####################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_TechnicalContact"
 AS
SELECT P."ProjectID",
    c."Email"::character varying(254) AS "Email",
        CASE
            WHEN lower(A."AgentType")::text = 'person'::text 
			THEN concat(A."GivenName", ' ', A."InheritedName")  
			--THEN concat(A."InheritedName", ', ', substring(A."GivenName", 1, 1), '.')  
            ELSE case when A."GivenName" is null then P."AgentName" else A."GivenName" end END::character varying(254) AS "Name",
    ''::character varying(254) AS "Phone",
    ''::character varying(254) AS "Address"
   FROM "#project#"."CacheProjectAgent" AS P
   INNER JOIN "#project#"."CacheProjectAgentRole" AS R 
   ON R."ProjectID" = P."ProjectID" AND R."AgentName" = P."AgentName" --AND R."AgentURI" = P."AgentURI"
   LEFT OUTER JOIN public."Agent" AS A
   ON P."AgentURI" = A."AgentURI" LEFT OUTER JOIN public."AgentContactInformation" C
   ON C."AgentID" = A."AgentID"
   WHERE R."AgentRole" = 'Technical Contact';
   
   ALTER TABLE public."ABCD_TechnicalContact" OWNER TO "CacheAdmin";

COMMENT ON VIEW public."ABCD_TechnicalContact"
    IS 'ABCD entity /DataSets/DataSet/TechnicalContacts/TechnicalContact';

GRANT ALL ON TABLE public."ABCD_TechnicalContact" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_TechnicalContact" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_TechnicalContact"."Address"
    IS 'ABCD 2.06 entity /DataSets/DataSet/TechnicalContacts/TechnicalContact/Address. The address of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table AgentContactInformation with data retrieved from DiversityAgents - Column Address';

COMMENT ON COLUMN public."ABCD_TechnicalContact"."Email"
    IS 'ABCD 2.06 entity /DataSets/DataSet/TechnicalContacts/TechnicalContact/Email. The email address of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table AgentContactInformation with data retrieved from DiversityAgents - Column Telephone';

COMMENT ON COLUMN public."ABCD_TechnicalContact"."Phone"
    IS 'ABCD 2.06 entity /DataSets/DataSet/TechnicalContacts/TechnicalContact/Phone. The phone of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table AgentContactInformation with data retrieved from DiversityAgents - Column Email';

COMMENT ON COLUMN public."ABCD_TechnicalContact"."Name"
    IS 'ABCD 2.06 entity /DataSets/DataSet/TechnicalContacts/TechnicalContact/Name. The name of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table Agent with data retrieved from DiversityAgents - 
	Columns GivenName and InheritedName for persons resp. GivenName for other types';


--#####################################################################################################################
--######   ABCD_ContentContact from DA and without restriction to one contact   #######################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_ContentContact"
 AS
SELECT P."ProjectID",
    C."Email"::character varying(254) AS "Email",
    case when lower(A."AgentType") = 'person' 
	THEN concat(A."GivenName", ' ', A."InheritedName")  
	--THEN concat(A."InheritedName", ', ', substring(A."GivenName", 1, 1), '.')  
	ELSE case when A."GivenName" is null then P."AgentName" else A."GivenName" end END::character varying(254) AS "Name",
    ''::character varying(254) AS "Phone",
    ''::character varying(254) AS "Address"
   FROM "#project#"."CacheProjectAgent" AS P
   INNER JOIN "#project#"."CacheProjectAgentRole" AS R 
   ON R."ProjectID" = P."ProjectID" AND R."AgentName" = P."AgentName" 
   LEFT OUTER JOIN public."Agent" AS A
   ON P."AgentURI" = A."AgentURI" LEFT OUTER JOIN public."AgentContactInformation" C
   ON C."AgentID" = A."AgentID"
   WHERE R."AgentRole" = 'Content Contact';
   
ALTER TABLE public."ABCD_ContentContact"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_ContentContact"
    IS 'ABCD entity /DataSets/DataSet/ContentContacts/ContentContact';

GRANT ALL ON TABLE public."ABCD_ContentContact" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_ContentContact" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_ContentContact"."Address"
    IS 'ABCD 2.06 entity /DataSets/DataSet/ContentContacts/ContentContact/Address. The address address of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table AgentContactInformation with data retrieved from DiversityAgents - Column Address';

COMMENT ON COLUMN public."ABCD_ContentContact"."Email"
    IS 'ABCD 2.06 entity /DataSets/DataSet/ContentContacts/ContentContact/Email. The email address of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table AgentContactInformation with data retrieved from DiversityAgents - Column Telephone';

COMMENT ON COLUMN public."ABCD_ContentContact"."Phone"
    IS 'ABCD 2.06 entity /DataSets/DataSet/ContentContacts/ContentContact/Phone. The phone address of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table AgentContactInformation with data retrieved from DiversityAgents - Column Email';

COMMENT ON COLUMN public."ABCD_ContentContact"."Name"
    IS 'ABCD 2.06 entity /DataSets/DataSet/ContentContacts/ContentContact/Name. The name of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table Agent with data retrieved from DiversityAgents - 
	Columns GivenName and InheritedName for persons resp. GivenName for other types';


--#####################################################################################################################
--######   updating source descriptions   #############################################################################
--#####################################################################################################################

COMMENT ON COLUMN public."ABCD_Metadata"."Description_Representation_Details"
    IS 'ABCD: Dataset/Metadata/Description/Representation/Details. Retrieved from DiversityProjects - Project - PublicDescription';


--#####################################################################################################################
--######   abcd___projectcitation: AgentRole from CacheProjectAgentRole   #############################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd___projectcitation(
	)
RETURNS SETOF integer 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
    ROWS 1000
AS $BODY$

begin

--Setting the role
SET ROLE "CacheAdmin";

-- Clear the table
TRUNCATE TABLE "#project#"."ABCD__ProjectCitation";

-- Insert basic
INSERT INTO "#project#"."ABCD__ProjectCitation"(
            "ProjectID", "ReferenceTitle", "Citation")
SELECT "ProjectID",
    "ReferenceTitle",
    ''
   FROM "#project#"."CacheProjectReference" R WHERE R."ReferenceType" = 'BioCASe (GFBio)' LIMIT 1;
-- insert the Authors
   DECLARE AUTHORS text = '';
	r integer;
	rMax integer = (SELECT max("AgentSequence") 
	FROM "#project#"."CacheProjectAgent" a, "#project#"."CacheProjectAgentRole" ar, public."Agent"	Agent
	WHERE ar."AgentRole" = 'Author' AND a."AgentName" <> '' AND a."AgentName" = ar."AgentName" and a."ProjectID" = ar."ProjectID" and a."AgentURI" = ar."AgentURI"
	and Agent."AgentURI" = a."AgentURI");
   BEGIN
	FOR r IN SELECT "AgentSequence" FROM "#project#"."CacheProjectAgent" a, "#project#"."CacheProjectAgentRole" ar, public."Agent"	Agent
	WHERE ar."AgentRole" = 'Author' AND a."AgentName" <> '' AND a."AgentName" = ar."AgentName" and a."ProjectID" = ar."ProjectID" 
	and a."AgentURI" = ar."AgentURI" and a."AgentURI" = Agent."AgentURI"
	ORDER BY "AgentSequence"
   LOOP
	AUTHORS := (select concat(AUTHORS, case when AUTHORS = '' then '' else case when r = rMax then ' & ' else '; ' end end, 
	case when Agent."InheritedName" <> '' then concat(Agent."InheritedName", ', ') else '' end, Agent."GivenName") 
	from  "#project#"."CacheProjectAgent" a, public."Agent"	Agent 
	where a."AgentSequence" = r and Agent."AgentURI" = a."AgentURI" limit 1);
	RETURN NEXT r;
   END LOOP; 
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = AUTHORS;
   END; 
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = 'Anonymous' WHERE "Citation" = '' OR "Citation" IS NULL;
-- Year
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", ' (', date_part('year', current_date)::character varying(4), '). '));
-- Title and [Dataset]
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", "ProjectTitle", '. [Dataset]. ') from "#project#"."CacheMetadata" WHERE "ProjectID" = "#project#".projectid());
-- Version
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", ' Version: ', date_part('year', current_date)::character varying(4), case when date_part('month', current_date) < 10 then '0' else '' end, date_part('month', current_date)::character varying(2), case when date_part('day', current_date) < 10 then '0' else '' end, date_part('day', current_date)::character varying(2)));
-- Publisher
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", '. Data Publisher: ', MIN(a."AgentName"), '.') 
	FROM "#project#"."CacheProjectAgent"  a, "#project#"."CacheProjectAgentRole" r
	WHERE r."AgentRole" = 'Publisher' AND a."AgentName" <> '' AND a."AgentName" = r."AgentName" and a."ProjectID" = r."ProjectID" and a."AgentURI" = r."AgentURI");
-- URI
   UPDATE "#project#"."ABCD__ProjectCitation" 
   SET "Citation" = concat(C."Citation", ' ', case when R."URI" <> '' then concat(R."URI", '.') else '' end) 
	FROM "#project#"."CacheProjectReference" R JOIN "#project#"."ABCD__ProjectCitation" C ON R."ProjectID" = C."ProjectID" AND R."ReferenceTitle" = C."ReferenceTitle";
end;

$BODY$;

ALTER FUNCTION "#project#".abcd___projectcitation()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd___projectcitation() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd___projectcitation() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd___projectcitation()
    IS 'Setting the content of table ABCD__ProjectCitation according to tables CacheProjectReference, CacheProjectAgent and CacheProjectAgentRole';

--#####################################################################################################################
--######   abcd__idxcacheidentificationunit_collectionspecimenid: Description included and check for existence   ######
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__idxcacheidentificationunit_collectionspecimenid(
	)
RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

if (select count(*) from pg_indexes where tablename = 'CacheIdentificationUnit' and schemaname = '#project#' and indexdef like '% ("CollectionSpecimenID")') = 0
then

CREATE INDEX IF NOT EXISTS idxCacheIdentificationUnit_CollectionSpecimenID
  ON "#project#"."CacheIdentificationUnit"
  USING btree
  ("CollectionSpecimenID");
  end if;

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__idxcacheidentificationunit_collectionspecimenid()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__idxcacheidentificationunit_collectionspecimenid() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__idxcacheidentificationunit_collectionspecimenid() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__idxcacheidentificationunit_collectionspecimenid()
    IS 'Create an index on column CollectionSpecimenID in table CacheIdentificationUnit';


--#####################################################################################################################
--######   abcd__idxcacheidentificationunit_identificationunitid: Include description and check for existence   #######
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__idxcacheidentificationunit_identificationunitid(
	)
RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

if (select count(*) from pg_indexes where tablename = 'CacheIdentificationUnit' and schemaname = '#project#' and indexdef like '% ("IdentificationUnitID")') = 0
then

CREATE INDEX IF NOT EXISTS idxCacheIdentificationUnit_IdentificationUnitID
  ON "#project#"."CacheIdentificationUnit"
  USING btree
  ("IdentificationUnitID");
  
end if;

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__idxcacheidentificationunit_identificationunitid()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__idxcacheidentificationunit_identificationunitid() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__idxcacheidentificationunit_identificationunitid() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__idxcacheidentificationunit_identificationunitid()
    IS 'Create an index on column IdentificationUnitID in table CacheIdentificationUnit';


--#####################################################################################################################
--######   abcd__idxcacheidentificationunitinpart_specimenpartid: Include description and check for existence   #######
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__idxcacheidentificationunitinpart_specimenpartid(
	)
RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

if (select count(*) from pg_indexes where tablename = 'CacheIdentificationUnitInPart' and schemaname = '#project#' and indexdef like '% ("SpecimenPartID")') = 0
then

  CREATE INDEX IF NOT EXISTS idxCacheIdentificationUnitInPart_SpecimenPartID
  ON "#project#"."CacheIdentificationUnitInPart"
  USING btree
  ("SpecimenPartID");

end if;

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__idxcacheidentificationunitinpart_specimenpartid()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__idxcacheidentificationunitinpart_specimenpartid() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__idxcacheidentificationunitinpart_specimenpartid() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__idxcacheidentificationunit_identificationunitid()
    IS 'Create an index on column SpecimenPartID in table CacheIdentificationUnitInPart';


--#####################################################################################################################
--######   ABCD_MeasurementOrFact: Adding MeasurementOrFactReference  #################################################
--#####################################################################################################################

ALTER TABLE "#project#"."ABCD_MeasurementOrFact" ADD COLUMN IF NOT EXISTS "MeasurementOrFactReference" character varying(255) COLLATE pg_catalog."default";

--#####################################################################################################################
--######   abcd__measurementorfact: Adding MeasurementOrFactReference, AnalysisNumber to Parameter  ###################
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
    ua."AnalysisDate" AS "MeasurementDateTime",
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


--#####################################################################################################################
--######   ABCD_Unit_Gathering: Add column SiteCoordinateSets_CoordinatesLatLong_Accuracy   ###########################
--#####################################################################################################################

--ALTER TABLE CREATE TABLE "#project#"."ABCD_Unit_Gathering" ADD "SiteCoordinateSets_CoordinatesLatLong_Accuracy"


--#####################################################################################################################
--######   abcd__unit_gathering: Removing objects if existing   #######################################################
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
     concat_ws('-'::text, COALESCE(e."CollectionYear"::character varying(4), '-'::character varying), lpad(e."CollectionMonth"::character varying(2)::text, 2, '0'::text), lpad(e."CollectionDay"::character varying(2)::text, 2, '0'::text))::character varying(10) AS "DateTime_ISODateTimeBegin",
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


--#####################################################################################################################
--######   ABCD__UnitNoPart: Inclusion of Exssicata number   ##########################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__unitnopart(
	)
RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- cleaning the table
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

-- insert Units without parts
INSERT INTO "#project#"."ABCD__UnitNoPart"(
            "ID", "UnitGUID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "KindOfUnit", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "CollectionSpecimenID", "IdentificationUnitID")
SELECT concat(u."IdentificationUnitID"::character varying, '-') AS "ID",
    u."StableIdentifier" AS "UnitGUID",
    btrim(concat(
        CASE
            WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
            ELSE u."CollectionSpecimenID"::character varying
        END, ' / ', u."IdentificationUnitID"::character varying
        )) AS "UnitID",
    s."LogUpdatedWhen" AS "DateLastEdited",
    btrim(u."LastIdentificationCache")::character varying(255) AS "Identification_Taxon_ScientificName_FullScientificName",
    CASE WHEN u."RetrievalType" IS NULL THEN 'human observation' ELSE u."RetrievalType" END AS "KindOfUnit",
    'en'::character varying(2) AS "KindOfUnit_Language",
    concat(s."ExsiccataAbbreviation", case when u."ExsiccataNumber"::text <> ''::text
		   THEN ' Exs. no. ' else '' end, u."ExsiccataNumber") AS "HerbariumUnit_Exsiccatum",
    u."CollectionSpecimenID",
    u."IdentificationUnitID"
   FROM "#project#"."CacheIdentificationUnit" u
     JOIN "#project#"."CacheCollectionSpecimen" s ON s."CollectionSpecimenID" = u."CollectionSpecimenID"
     LEFT JOIN "#project#"."CacheIdentificationUnitInPart" up ON u."IdentificationUnitID" = up."IdentificationUnitID" 
     WHERE up."IdentificationUnitID" IS NULL;

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__unitnopart()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopart() TO "CacheUser";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopart() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopart() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__unitnopart()
    IS 'Filling ABCD__UnitNoPart from CacheCollectionSpecimen and CacheIdentificationUnit with no data in CacheIdentificationUnitInPart';


--#####################################################################################################################
--######   abcd__unitnopart_removeindices: Check for existence and description included   #############################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__unitnopart_removeindices()
 RETURNS void
 LANGUAGE plpgsql
AS $function$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- removing the key
ALTER TABLE "#project#"."ABCD__UnitNoPart" DROP CONSTRAINT IF EXISTS "ABCD__UnitNoPart_pkey";

-- removing the indices
DROP INDEX IF EXISTS "#project#".ABCD__UnitNoPart_CollectionSpecimenID;
DROP INDEX IF EXISTS "#project#".ABCD__UnitNoPart_ID;
DROP INDEX IF EXISTS "#project#".ABCD__UnitNoPart_IdentificationUnitID;
DROP INDEX IF EXISTS "#project#".abcd__unitnopart_identification_scientificname;

end;
$function$
;

COMMENT ON FUNCTION "#project#".abcd__unitnopart_removeindices() 
	IS 'Removing indices of temporary ABCD tables. Will be recreated in last step of transfer';


--#####################################################################################################################
--######   abcd__unitnopartkingdom: Removing temp table if existent   ################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__unitnopartkingdom(
	)
RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- creating the needed index
CREATE INDEX IF NOT EXISTS ABCD__UnitNoPart_IdentificationUnitID 
  ON "#project#"."ABCD__UnitNoPart" 
  USING btree
  ("IdentificationUnitID");

-- drop table if existing
DROP TABLE IF EXISTS ABCD__UnitNoPartKingdomTemp;

-- creating the temporary table
CREATE TEMP TABLE ABCD__UnitNoPartKingdomTemp AS SELECT a."ID",
    a."UnitGUID",
    a."SourceInstitutionID",
    a."SourceID",
    a."UnitID",
    a."DateLastEdited",
    a."Identification_Taxon_ScientificName_FullScientificName",
    a."Identification_Taxon_ScientificName_Qualifier",
    rnp."RecordBasis" AS "RecordBasis",
    a."KindOfUnit",
    k."Kingdom" AS "Identification_Taxon_HigherTaxonName",
    a."KindOfUnit_Language",
    a."HerbariumUnit_Exsiccatum",
    a."RecordURI",
    a."CollectionSpecimenID",
    a."IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart" a
     JOIN "#project#"."ABCD__Kingdom" k ON a."IdentificationUnitID" = k."IdentificationUnitID"
     JOIN "#project#"."ABCD__RecordBasis_NoPart" rnp ON a."IdentificationUnitID" = rnp."IdentificationUnitID";

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

-- removing the index
DROP INDEX IF EXISTS "#project#".ABCD__UnitNoPart_IdentificationUnitID;

-- inserting the data
INSERT INTO "#project#"."ABCD__UnitNoPart"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID")
SELECT "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
       "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
       "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
       "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
       "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
       "IdentificationUnitID"
  FROM ABCD__UnitNoPartKingdomTemp;

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__unitnopartkingdom()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopartkingdom() TO "CacheUser";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopartkingdom() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopartkingdom() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__unitnopartkingdom() 
	IS 'Filling table ABCD__UnitNoPart via a temp table to include kingdom from table ABCD__Kingdom';


--#####################################################################################################################
--######   abcd__unitnopartmetadata: Removing temp table if existent   ################################################
--#####################################################################################################################

--11.02.2021 - Umbau fuer CacheAdmin
SET ROLE 'CacheAdmin';

CREATE OR REPLACE FUNCTION "#project#".abcd__unitnopartmetadata()
 RETURNS void
 LANGUAGE plpgsql
AS $function$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- drop index if existing
DROP INDEX IF EXISTS ABCD__UnitNoPart_CollectionSpecimenID;

-- creating the needed index
CREATE INDEX IF NOT EXISTS ABCD__UnitNoPart_CollectionSpecimenID
  ON "#project#"."ABCD__UnitNoPart" ("CollectionSpecimenID");
 
-- drop table if existing
DROP TABLE IF EXISTS ABCD__UnitNoPartMetadataTemp;

-- creating the temporary table
CREATE TEMP TABLE ABCD__UnitNoPartMetadataTemp AS SELECT a."ID", 
a."UnitGUID", m."SourceInstitutionID", m."ProjectTitleCode" AS "SourceID", a."UnitID", 
       a."DateLastEdited", a."Identification_Taxon_ScientificName_FullScientificName", 
       a."Identification_Taxon_ScientificName_Qualifier", a."RecordBasis", 
       a."KindOfUnit", a."Identification_Taxon_HigherTaxonName", a."KindOfUnit_Language", 
       a."HerbariumUnit_Exsiccatum", CASE
            WHEN m."RecordUri"::text ~~ 'http://biocase%'::text THEN concat(m."RecordUri", btrim(concat(
            CASE
                WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
                ELSE a."CollectionSpecimenID"::character varying
            END, ' / ', a."IdentificationUnitID"::character varying)))
            ELSE
            CASE
                WHEN m."RecordUri"::text <> ''::text THEN concat(m."RecordUri", s."CollectionSpecimenID"::character varying)
                ELSE NULL::text
            END
        END AS "RecordURI", a."CollectionSpecimenID", 
       a."IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart" a
	JOIN "#project#"."CacheCollectionSpecimen" s ON s."CollectionSpecimenID" = a."CollectionSpecimenID"
	CROSS JOIN "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

--GRANT ALL ON TABLE ABCD__UnitNoPartMetadataTemp TO "CacheAdmin";
/*ALTER TABLE "#project#".ABCD__UnitNoPartMetadataTemp OWNER TO "CacheAdmin";*/

--11.02.2021 - Umbau fuer CacheAdmin
--ALTER TABLE "#project#".ABCD__UnitNoPartMetadataTemp OWNER TO "CacheAdmin";

-- removing the indes
DROP INDEX IF EXISTS "#project#".ABCD__UnitNoPart_CollectionSpecimenID;

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

-- transfer the data into the target table
INSERT INTO "#project#"."ABCD__UnitNoPart"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID")
SELECT "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
       "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
       "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
       "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
       "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
       "IdentificationUnitID"
  FROM ABCD__UnitNoPartMetadataTemp;

end;
$function$
;

COMMENT ON FUNCTION "#project#".abcd__unitnopartmetadata() 
	IS 'Transfer data via a temporay table into ABCD__UnitNoPart';

--11.02.2021 - Umbau fuer CacheAdmin
--ALTER FUNCTION "#project#".abcd__unitnopartmetadata() OWNER TO "CacheAdmin_Project_BSMfungicoll";
ALTER FUNCTION "#project#".abcd__unitnopartmetadata() OWNER TO "CacheAdmin";




--#####################################################################################################################
--######   abcd__unitnopartqualifier: Removing temp table if existent   ###############################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__unitnopartqualifier(
	)
RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- creating the needed index
CREATE INDEX IF NOT EXISTS ABCD__UnitNoPart_IdentificationUnitID 
  ON "#project#"."ABCD__UnitNoPart" 
  USING btree
  ("IdentificationUnitID");
  
-- drop table if existing
DROP TABLE IF EXISTS ABCD__UnitNoPartIdentificationQualifierTemp;

-- creating the temporary table
CREATE TEMP TABLE ABCD__UnitNoPartIdentificationQualifierTemp AS
SELECT MAX(i."IdentificationQualifier")::character varying(50) AS "IdentificationQualifier",
    MAX(a."IdentificationUnitID")::integer AS "IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart" a
   JOIN "#project#"."CacheIdentification" i ON a."IdentificationUnitID" = i."IdentificationUnitID" 
   AND a."Identification_Taxon_ScientificName_FullScientificName"::text = i."TaxonomicName"::text 
   AND i."IdentificationQualifier" <> ''
   GROUP BY i."IdentificationQualifier", a."IdentificationUnitID";

-- drop table if existing
DROP TABLE IF EXISTS ABCD__UnitNoPartQualifierTemp;

-- creating the second temporary table   
CREATE TEMP TABLE ABCD__UnitNoPartQualifierTemp AS  
SELECT a."ID",
       a."UnitGUID",
    a."SourceInstitutionID",
    a."SourceID",
    a."UnitID",
    a."DateLastEdited",
    a."Identification_Taxon_ScientificName_FullScientificName",
    i."IdentificationQualifier" AS "Identification_Taxon_ScientificName_Qualifier",
    a."RecordBasis",
    a."KindOfUnit",
    a."Identification_Taxon_HigherTaxonName",
    a."KindOfUnit_Language",
    a."HerbariumUnit_Exsiccatum",
    a."RecordURI",
    a."CollectionSpecimenID",
    a."IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart" a
   LEFT JOIN ABCD__UnitNoPartIdentificationQualifierTemp i ON i."IdentificationUnitID" = a."IdentificationUnitID";

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

-- removing the index
DROP INDEX IF EXISTS "#project#".ABCD__UnitNoPart_IdentificationUnitID;

-- inserting the data
INSERT INTO "#project#"."ABCD__UnitNoPart"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID")
SELECT "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
       "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
       "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
       "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
       "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
       "IdentificationUnitID"
  FROM ABCD__UnitNoPartQualifierTemp;

end;
$BODY$;

ALTER FUNCTION "#project#".abcd__unitnopartqualifier()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopartqualifier() TO "CacheUser";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopartqualifier() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unitnopartqualifier() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd__unitnopartqualifier() 
	IS 'Transfer data via a temporay table into ABCD__UnitNoPart to include IdentificationQualifier';


--#####################################################################################################################
--######   abcd__unitpart: SourceInstitutionID from CollectionAcronym resp. CollectionName   ##########################
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
     CROSS JOIN "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

-- adding the key
ALTER TABLE "#project#"."ABCD__UnitPart"
  ADD CONSTRAINT "ABCD__UnitPart_pkey" PRIMARY KEY("ID");

end;
$function$
;

COMMENT ON FUNCTION "#project#".abcd__unitpart() 
	IS 'Transfer data into table ABCD__UnitNoPart';

	
--#####################################################################################################################
--######   ABCD_Unit: add column Identification_Reference_URI   #######################################################
--#####################################################################################################################

ALTER TABLE "#project#"."ABCD_Unit"
    ADD COLUMN "Identification_Reference_URI" character varying(150);

COMMENT ON COLUMN "#project#"."ABCD_Unit"."Identification_Reference_URI"
    IS 'Corresponds to ABCD /Identification/References/Reference/URI';
	
--#####################################################################################################################
--######   ABCD_Unit: add column Identification_Reference_CitationDetail   ############################################
--#####################################################################################################################

ALTER TABLE "#project#"."ABCD_Unit"
    ADD COLUMN "Identification_Reference_CitationDetail" integer;

COMMENT ON COLUMN "#project#"."ABCD_Unit"."Identification_Reference_CitationDetail"
    IS 'Corresponds to ABCD /Identification/References/Reference/CitationDetail, contains NameID from DiversityTaxonNames';
	
--#####################################################################################################################
--######   ABCD_Unit: add column Identification_Reference_ReferenceGUID   #############################################
--#####################################################################################################################

ALTER TABLE "#project#"."ABCD_Unit"
    ADD COLUMN "Identification_Reference_ReferenceGUID" character varying(100) COLLATE pg_catalog."default";

COMMENT ON COLUMN "#project#"."ABCD_Unit"."Identification_Reference_ReferenceGUID"
    IS 'Corresponds to ABCD /Identification/References/Reference/ReferenceGUID';
	
--#####################################################################################################################
--######   ABCD_Unit: add column Identification_Reference_TitleCitation   #############################################
--#####################################################################################################################

ALTER TABLE "#project#"."ABCD_Unit"
    ADD COLUMN "Identification_Reference_TitleCitation" character varying(100);

COMMENT ON COLUMN "#project#"."ABCD_Unit"."Identification_Reference_TitleCitation"
    IS 'Corresponds to ABCD /Identification/References/Reference/TitleCitation';

--#####################################################################################################################
--######   ABCD_Unit in public: add new columns   #####################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit"
 AS
 SELECT "ABCD_Unit"."ID",
    "ABCD_Unit"."UnitGUID",
    "ABCD_Unit"."SourceInstitutionID",
    "ABCD_Unit"."SourceID",
    "ABCD_Unit"."UnitID",
    "ABCD_Unit"."DateLastEdited",
    "ABCD_Unit"."Identification_Taxon_ScientificName_FullScientificName",
    "ABCD_Unit"."Identification_Taxon_ScientificName_Qualifier",
    "ABCD_Unit"."RecordBasis",
    "ABCD_Unit"."KindOfUnit",
    "ABCD_Unit"."Identification_Taxon_HigherTaxonName",
    "ABCD_Unit"."KindOfUnit_Language",
    "ABCD_Unit"."HerbariumUnit_Exsiccatum",
    "ABCD_Unit"."RecordURI",
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
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/';

GRANT ALL ON TABLE public."ABCD_Unit" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Reference_ReferenceGUID"
    IS 'Corresponds to ABCD /Identification/References/Reference/ReferenceGUID, Reference on REST-Service e.g. DTNtaxonlists-DiversityTaxonNames_Plants';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Reference_TitleCitation"
    IS 'Corresponds to ABCD /Identification/References/Reference/TitleCitation, Fixed text';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Reference_CitationDetail"
    IS 'Corresponds to ABCD /Identification/References/Reference/CitationDetail, contains NameID e.g. from DiversityTaxonNames';

COMMENT ON COLUMN public."ABCD_Unit"."Identification_Reference_URI"
    IS 'Corresponds to ABCD /Identification/References/Reference/URI, Fixed text';

--#####################################################################################################################
--######   ABCD_MeasurementOrFact: add column MeasurementOrFactReference   ############################################
--#####################################################################################################################

ALTER TABLE "#project#"."ABCD_MeasurementOrFact"
    ADD COLUMN IF NOT EXISTS "MeasurementOrFactReference" character varying(100);

COMMENT ON COLUMN "#project#"."ABCD_MeasurementOrFact"."MeasurementOrFactReference"
    IS 'ABCD entity to provide information about the analysis';

--#####################################################################################################################
--######   ABCD_MeasurementOrFact: change column Parameter to character varying(110)   ################################
--#####################################################################################################################

DROP VIEW IF EXISTS public."ABCD_MeasurementOrFact";

ALTER TABLE "#project#"."ABCD_MeasurementOrFact"
    ALTER COLUMN "Parameter" SET DATA TYPE character varying(110);


--#####################################################################################################################
--######   ABCD_MeasurementOrFact in public: add column MeasurementOrFactReference   ##################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_MeasurementOrFact"
 AS
 SELECT "ABCD_MeasurementOrFact"."ID",
    "ABCD_MeasurementOrFact"."Parameter",
    "ABCD_MeasurementOrFact"."UnitOfMeasurement",
    "ABCD_MeasurementOrFact"."LowerValue",
    "ABCD_MeasurementOrFact"."MeasurementDateTime",
    "ABCD_MeasurementOrFact"."MeasuredBy",
    "ABCD_MeasurementOrFact"."IdentificationUnitID",
    "ABCD_MeasurementOrFact"."SpecimenPartID",
    "ABCD_MeasurementOrFact"."CollectionSpecimenID",
    "ABCD_MeasurementOrFact"."AnalysisID",
    "ABCD_MeasurementOrFact"."AnalysisNumber",
    "ABCD_MeasurementOrFact"."MeasurementOrFactReference"
   FROM "#project#"."ABCD_MeasurementOrFact";

ALTER TABLE public."ABCD_MeasurementOrFact"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_MeasurementOrFact"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/MultiMediaObjects/MultiMediaObject/';

GRANT ALL ON TABLE public."ABCD_MeasurementOrFact" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_MeasurementOrFact" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_MeasurementOrFact"."MeasurementOrFactReference"
    IS 'ABCD entity to provide information about the analysis';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."Parameter" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/Parameter. Retrieved from table Analysis - DisplayText';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."UnitOfMeasurement" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/UnitOfMeasurement. Retrieved from table Analysis - MeasurementUnit';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."LowerValue" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/LowerValue. Retrieved from table IdentificationUnitAnalysis - AnalysisResult';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."MeasurementDateTime" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasurementDateTime. Retrieved from table IdentificationUnitAnalysis - AnalysisDate';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."MeasuredBy" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasuredBy. Retrieved from table IdentificationUnitAnalysis - ResponsibleName';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."MeasuredBy" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasuredBy. Retrieved from table IdentificationUnitAnalysis - ResponsibleName';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."IdentificationUnitID" IS 'PK of table IdentificationUnitID';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."SpecimenPartID" IS 'PK of table CollectionSpecimenPart';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."AnalysisID" IS 'PK of table Analysis';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."AnalysisNumber" IS 'Retrieved from table IdentificationUnitAnalysis - AnalysisNumber';


--#####################################################################################################################
--######   abcd__unit_associations_unitassociation   ##################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__unit_associations_unitassociation(
	)
    RETURNS void
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- clean the table
TRUNCATE TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation";

-- remove the key
ALTER TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation" DROP CONSTRAINT "ABCD_Unit_Associations_UnitAssociation_pkey";

-- remove the indices
DROP INDEX "#project#".abcd_unit_associations_unitassociation_id;

-- insert the data
INSERT INTO "#project#"."ABCD_Unit_Associations_UnitAssociation"(
            "AssociatedUnitID", "SourceInstitutionCode", "SourceName", "AssociationType", 
            "AssociationType_Language", "Comment", "Comment_Language", "IdentificationUnitID", 
            "ID", "ID_Related", "UnitID", "UnitGUID", "KindOfUnit", "SpecimenPartID", 
            "CollectionSpecimenID")
SELECT btrim(concat(
        CASE
            WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
            ELSE u."CollectionSpecimenID"::character varying
        END, ' / ', p."IdentificationUnitID"::character varying,
        CASE
            WHEN pa."AccessionNumber"::text <> ''::text THEN concat(' / ', pa."AccessionNumber")
            ELSE
            CASE
                WHEN pp."SpecimenPartID"::text <> ''::text THEN concat(' / ', pp."SpecimenPartID"::character varying)
                ELSE ''::text
            END
        END)) AS "AssociatedUnitID",
    c."CollectionName" AS "SourceInstitutionCode",
    m."ProjectTitleCode" AS "SourceName",
    u."RelationType" AS "AssociationType",
    'en'::character varying AS "AssociationType_Language",
    ltrim(concat(rtrim(rtrim(u."RelationType"::text)), ' ',
        CASE
            WHEN u."RelationType"::text ~~ '% on'::text THEN ''::text
            ELSE 'on '::text
        END,
        CASE
            WHEN p."LastIdentificationCache"::text = ''::text THEN p."TaxonomicGroup"
            ELSE p."LastIdentificationCache"
        END)) AS "Comment",
    'en'::character varying AS "Comment_Language",
    u."IdentificationUnitID",
    concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    concat(p."IdentificationUnitID"::character varying, '-', pp."SpecimenPartID"::character varying) AS "ID_Related",
    concat(p."CollectionSpecimenID"::character varying, '-', p."IdentificationUnitID"::character varying, '-', pp."SpecimenPartID"::character varying) AS "UnitID",
        CASE
            WHEN pp."StableIdentifier" IS NULL THEN p."StableIdentifier"
            ELSE pp."StableIdentifier"
        END AS "UnitGUID",
    pa."MaterialCategory"::character varying(254) AS "KindOfUnit",
    up."SpecimenPartID",
    u."CollectionSpecimenID"
   FROM "#project#"."CacheCollection" c
	 RIGHT JOIN "#project#"."CacheCollectionSpecimenPart" pa ON pa."CollectionID" = c."CollectionID"
     RIGHT JOIN "#project#"."CacheIdentificationUnitInPart" pp ON pa."SpecimenPartID" = pp."SpecimenPartID"
     RIGHT JOIN "#project#"."CacheIdentificationUnit" p ON p."IdentificationUnitID" = pp."IdentificationUnitID"
     JOIN "#project#"."CacheIdentificationUnit" u ON u."RelatedUnitID" = p."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON s."CollectionSpecimenID" = u."CollectionSpecimenID"
     LEFT JOIN "#project#"."CacheIdentificationUnitInPart" up ON u."IdentificationUnitID" = up."IdentificationUnitID"
     CROSS JOIN "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

-- adding the key
ALTER TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation"
  ADD CONSTRAINT "ABCD_Unit_Associations_UnitAssociation_pkey" PRIMARY KEY("ID", "ID_Related");
  
-- adding the index
CREATE INDEX abcd_unit_associations_unitassociation_id
  ON "#project#"."ABCD_Unit_Associations_UnitAssociation"
  USING btree
  ("ID" COLLATE pg_catalog."default");
       
end;
$BODY$;

ALTER FUNCTION "#project#".abcd__unit_associations_unitassociation()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_associations_unitassociation() TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_associations_unitassociation() TO "CacheUser";
GRANT EXECUTE ON FUNCTION "#project#".abcd__unit_associations_unitassociation() TO PUBLIC;


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 5 WHERE "Package" = 'ABCD'
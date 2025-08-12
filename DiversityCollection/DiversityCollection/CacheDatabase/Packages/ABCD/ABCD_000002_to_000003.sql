--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 3
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   ABCD_Unit   ################################################################################################
--######   The original view including a UNION was to slow for BioCASE  ###############################################
--#####################################################################################################################

-- DROP TABLE "#project#"."ABCD_Unit";

CREATE TABLE "#project#"."ABCD_Unit"
(
  "ID" character varying(25) NOT NULL,
  "UnitGUID" character varying(254),
  "SourceInstitutionID" character varying(254) NOT NULL,
  "SourceID" character varying(254),
  "UnitID" character varying(254),
  "DateLastEdited" timestamp without time zone,
  "Identification_Taxon_ScientificName_FullScientificName" character varying(255),
  "Identification_Taxon_ScientificName_Qualifier" character varying(50),
  "RecordBasis" character varying(50),
  "KindOfUnit" character varying(50),
  "Identification_Taxon_HigherTaxonName" character varying(50),
  "KindOfUnit_Language" character varying(2),
  "HerbariumUnit_Exsiccatum" character varying(255),
  "RecordURI" character varying(500),
  "CollectionSpecimenID" integer,
  "IdentificationUnitID" integer,
  CONSTRAINT "ABCD_Unit_pkey" PRIMARY KEY ("ID")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "#project#"."ABCD_Unit"
  OWNER TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD_Unit" TO "CacheUser";
GRANT ALL ON TABLE "#project#"."ABCD_Unit" TO "CacheAdmin";

COMMENT ON COLUMN "#project#"."ABCD_Unit"."SourceInstitutionID"
    IS 'Must be filled (mandatory for publication in GBIF)';

--#####################################################################################################################
--######   function abcd__Unit_RemoveIndices  #########################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_RemoveIndices()
  RETURNS void AS
$BODY$
begin

-- removing the key
ALTER TABLE "#project#"."ABCD_Unit" DROP CONSTRAINT "ABCD_Unit_pkey";

-- removing the indices
DROP INDEX "#project#".ABCD_Unit_CollectionSpecimenID;
DROP INDEX "#project#".ABCD_Unit_IdentificationUnitID;
DROP INDEX "#project#".ABCD_Unit_Identification_Scientificname;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit_RemoveIndices()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit_RemoveIndices() TO "CacheAdmin";


--#####################################################################################################################
--######   function abcd__Unit_AddingIndices  #########################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_AddingIndices()
  RETURNS void AS
$BODY$
begin

-- inserting the primary key
ALTER TABLE "#project#"."ABCD_Unit"
  ADD CONSTRAINT "ABCD_Unit_pkey" PRIMARY KEY("ID");

-- creating the indices
CREATE INDEX ABCD_Unit_CollectionSpecimenID
  ON "#project#"."ABCD_Unit" 
  USING btree 
  ("CollectionSpecimenID");

CREATE INDEX ABCD_Unit_IdentificationUnitID 
  ON "#project#"."ABCD_Unit" 
  USING btree
  ("IdentificationUnitID");

CREATE INDEX ABCD_Unit_Identification_ScientificName 
  ON "#project#"."ABCD_Unit" 
  USING btree
  ("Identification_Taxon_ScientificName_FullScientificName" COLLATE pg_catalog."default");

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit_AddingIndices()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit_AddingIndices() TO "CacheAdmin";


--#####################################################################################################################
--######   function abcd__Unit  #######################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit()
  RETURNS void AS
$BODY$
begin

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
    "ABCD__UnitNoPart"."SourceInstitutionID",
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
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit() TO "CacheAdmin";


--#####################################################################################################################
--######   ABCD_Unit - adaption to new table   ########################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_Unit" AS 
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
    "ABCD_Unit"."IdentificationUnitID"
   FROM "#project#"."ABCD_Unit";


--#####################################################################################################################
--######   function abcd___ProjectCitation - inclusion of date as version  ############################################
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
   DECLARE AUTHORS character varying(255) = '';
	r integer;
	rMax integer = (SELECT max("AgentSequence") FROM "#project#"."CacheProjectAgent" a
	WHERE a."AgentRole" = 'Author' AND a."AgentName" <> '');
   BEGIN
	FOR r IN SELECT "AgentSequence" FROM "#project#"."CacheProjectAgent" a
	WHERE a."AgentRole" = 'Author' AND a."AgentName" <> '' ORDER BY "AgentSequence"
   LOOP
	AUTHORS := (select concat(AUTHORS, case when AUTHORS = '' then '' else case when r = rMax then ' & ' else '; ' end end, a."AgentName") from  "#project#"."CacheProjectAgent" a where a."AgentSequence" = r);
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
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", '. Data Publisher: ', MIN("AgentName"), '.') 
	from "#project#"."CacheProjectAgent"  a
	WHERE a."AgentRole" = 'Publisher' AND a."AgentName" <> '');
-- URI
   UPDATE "#project#"."ABCD__ProjectCitation" 
   SET "Citation" = concat(C."Citation", ' ', case when R."URI" <> '' then concat(R."URI", '.') else '' end) 
	FROM "#project#"."CacheProjectReference" R JOIN "#project#"."ABCD__ProjectCitation" C ON R."ProjectID" = C."ProjectID" AND R."ReferenceTitle" = C."ReferenceTitle";
end;

$BODY$;


--#####################################################################################################################
--######   ABCD_Metadata - update description of columns  #############################################################
--#####################################################################################################################

COMMENT ON Column "ABCD_Metadata"."RevisionData_DateModified" IS 'ABCD: Dataset/Metadata/RevisionData/DateModified. Retrieved from the first level SQL-Server cache database for DiversityCollection corresponding to the date and time of the last transfer into the cache database (ProjectPublished.LastUpdatedWhen)';

COMMENT ON Column "ABCD_Metadata"."IPRStatements_Citation_Text" IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/Text. Retrieved from DiversityProjects: 
First dataset in table ProjectReference where type = ''BioCASe (GFBio)''. 
Authors from table ProjectAgent with type = ''Author'' according to their sequence. If none are found ''Anonymous''. 
Current year. Content of column ProjectTitle in table Project. 
Marker ''[Dataset]''. 
''Version: '' + date of transfer into the ABCD tables as year + month + day: yyyymmdd.
If available publishers: ''Data Publisher: '' + Agents with role ''Publisher'' from table ProjectAgent. URI from table ProjectReference if present. 
If entry in table ProjectReference is missing taken from Settings - ABCD - Citations - Text';

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 3 WHERE "Package" = 'ABCD'
  





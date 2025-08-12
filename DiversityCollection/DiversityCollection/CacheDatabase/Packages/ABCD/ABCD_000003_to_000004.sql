--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 4
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   ABCD__ProjectCitation: Roles from new table CacheProjectAgentRole   ########################################
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
	rMax integer = (SELECT max("AgentSequence") 
	FROM "#project#"."CacheProjectAgent" a, "#project#"."CacheProjectAgentRole" r
	WHERE r."AgentRole" = 'Author' AND a."AgentName" <> '' AND a."AgentName" = r."AgentName" and a."ProjectID" = r."ProjectID" and a."AgentURI" = r."AgentURI");
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
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", '. Data Publisher: ', MIN(a."AgentName"), '.') 
	FROM "#project#"."CacheProjectAgent"  a, "#project#"."CacheProjectAgentRole" r
	WHERE r."AgentRole" = 'Publisher' AND a."AgentName" <> '' AND a."AgentName" = r."AgentName" and a."ProjectID" = r."ProjectID" and a."AgentURI" = r."AgentURI");
-- URI
   UPDATE "#project#"."ABCD__ProjectCitation" 
   SET "Citation" = concat(C."Citation", ' ', case when R."URI" <> '' then concat(R."URI", '.') else '' end) 
	FROM "#project#"."CacheProjectReference" R JOIN "#project#"."ABCD__ProjectCitation" C ON R."ProjectID" = C."ProjectID" AND R."ReferenceTitle" = C."ReferenceTitle";
end;


$BODY$;


--#####################################################################################################################
--######   ABCD__RecordBasis: Change from view to table   #############################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD__RecordBasis_MaterialCategories"
(
    "MaterialCategory" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "RecordBasis" text COLLATE pg_catalog."default",
    "PreparationType" text COLLATE pg_catalog."default",
    CONSTRAINT "ABCD__RecordBasis_MaterialCategories_pkey" PRIMARY KEY ("MaterialCategory")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE "#project#"."ABCD__RecordBasis_MaterialCategories"
    OWNER to "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."ABCD__RecordBasis_MaterialCategories" TO "CacheUser";
GRANT ALL ON TABLE "#project#"."ABCD__RecordBasis_MaterialCategories" TO "CacheAdmin";


--#####################################################################################################################
--######   ABCD__RecordBasis: Inserting basic data   ##################################################################
--#####################################################################################################################


INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('bones', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('complete skeleton', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('cultures', 'LivingSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('DNA lyophilised', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('DNA sample', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('drawing', 'DrawingOrPhotograph', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('drawing or photograph', 'DrawingOrPhotograph', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('dried specimen', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('earth science specimen', 'EarthScienceSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('egg', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil bones', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil complete skeleton', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil incomplete skeleton', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil otolith', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil postcranial skeleton', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil scales', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil shell', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil single bones', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil skull', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil specimen', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('fossil tooth', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('herbarium sheets', 'PreservedSpecimen', 'dried and pressed'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('icones', 'DrawingOrPhotograph', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('incomplete skeleton', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('living specimen', 'LivingSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('material sample', 'MaterialSample', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('medium', 'MultimediaObject', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('micr. slide', 'PreservedSpecimen', 'microscopic preparation'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('mineral specimen', 'MineralSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('mould', 'HumanObservation', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('nest', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('other specimen', 'OtherSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('otolith', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('pelt', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('photogr. print', 'DrawingOrPhotograph', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('photogr. slide', 'DrawingOrPhotograph', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('postcranial skeleton', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('preserved specimen', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('SEM table', 'PreservedSpecimen', 'microscopic preparation'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('shell', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('single bones', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('skull', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('sound', 'HumanObservation', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('specimen', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('TEM specimen', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('thin section', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('tissue sample', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('tooth', 'PreservedSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('trace', 'HumanObservation', 'no treatment'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('trace fossil', 'FossilSpecimen', 'PreservedSpecimen'); 
INSERT INTO "#project#"."ABCD__RecordBasis_MaterialCategories" ("MaterialCategory", "RecordBasis", "PreparationType")
VALUES('vial', 'PreservedSpecimen', 'no treatment'); 


CREATE OR REPLACE VIEW "#project#"."ABCD__RecordBasis" AS 
 SELECT up."SpecimenPartID",
    up."IdentificationUnitID",
	m."RecordBasis",
	m."PreparationType",
    p."MaterialCategory"
   FROM "#project#"."CacheIdentificationUnitInPart" up,
    "#project#"."CacheCollectionSpecimenPart" p,
	"#project#"."ABCD__RecordBasis_MaterialCategories" m
  WHERE up."SpecimenPartID" = p."SpecimenPartID" and m."MaterialCategory" = p."MaterialCategory";

ALTER TABLE "#project#"."ABCD__RecordBasis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__RecordBasis" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__RecordBasis" TO "CacheUser";

COMMENT ON VIEW "#project#"."ABCD__RecordBasis"   IS 'ABCD auxillary view - translation of material categories within the DBW into RecordBasis and PreparationType in ABCD';
COMMENT ON COLUMN "#project#"."ABCD__RecordBasis"."SpecimenPartID" IS 'Unique key to the table CollectionSpecimenSpecimenPart';
COMMENT ON COLUMN "#project#"."ABCD__RecordBasis"."RecordBasis" IS 'translation of the material category into RecordBasis';
COMMENT ON COLUMN "#project#"."ABCD__RecordBasis"."PreparationType" IS 'translation of the material category into PreparationType';


--#####################################################################################################################
--######   ABCD__Kingdom: Change from view to table   #################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD__Kingdom_TaxonomicGroups"
(
    "TaxonomicGroup" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Kingdom" text COLLATE pg_catalog."default",
    CONSTRAINT "ABCD__Kingdom_TaxonomicGroups_pkey" PRIMARY KEY ("TaxonomicGroup")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE "#project#"."ABCD__Kingdom_TaxonomicGroups"
    OWNER to "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."ABCD__Kingdom_TaxonomicGroups" TO "CacheUser";
GRANT ALL ON TABLE "#project#"."ABCD__Kingdom_TaxonomicGroups" TO "CacheAdmin";


--#####################################################################################################################
--######   ABCD__Kingdom: inserting basic data   ######################################################################
--#####################################################################################################################


INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('alga', 'Plantae'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('amphibian', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('animal', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('arthropod', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('bacterium', 'Bacteria'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('bird', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('bryophyte', 'Plantae'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('cnidaria', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('echinoderm', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('evertebrate', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('fish', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('fungus', 'Fungi'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('gall', 'incertae sedis'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('insect', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('lichen', 'Fungi'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('mammal', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('mineral', 'MineralRock'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('mollusc', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('myxomycete', 'Protozoa'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('other', 'incertae sedis'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('plant', 'Plantae'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('reptile', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('rock', 'MineralRock'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('soil', 'incertae sedis'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('spider', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('vertebrate', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('virus', 'Viruses'); 


CREATE OR REPLACE VIEW "#project#"."ABCD__Kingdom" AS 
 SELECT u."IdentificationUnitID",
        t."Kingdom"
   FROM "#project#"."CacheIdentificationUnit" u
   , "#project#"."ABCD__Kingdom_TaxonomicGroups" t
   WHERE u."TaxonomicGroup" = t."TaxonomicGroup";

ALTER TABLE "#project#"."ABCD__Kingdom"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__Kingdom" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__Kingdom" TO "CacheUser";

COMMENT ON VIEW "#project#"."ABCD__Kingdom" IS 'ABCD auxillary view - translation of taxonomic groups within the DBW into kingdoms in ABCD';
COMMENT ON COLUMN "#project#"."ABCD__Kingdom"."IdentificationUnitID" IS 'Unique key to the table IdentificationUnit';
COMMENT ON COLUMN "#project#"."ABCD__Kingdom"."Kingdom" IS 'translation of taxonomic group into kingdom';


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 4 WHERE "Package" = 'ABCD'
--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 8
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   CacheExternalIdentifier   ##################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheExternalIdentifier"
(
  "ID" integer NOT NULL,
  "ReferencedTable" character varying(128) NOT NULL,
  "ReferencedID" integer NOT NULL,
  "Type" character varying(50) NOT NULL,
  "Identifier" character varying(500),
  "URL" character varying(500),
  "Notes" text,
  CONSTRAINT "CacheExternalIdentifier_pkey" PRIMARY KEY ("ID")
);


ALTER TABLE "#project#"."CacheExternalIdentifier"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheExternalIdentifier" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheExternalIdentifier" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheExternalIdentifier" TO "CacheUser_#project#";
--GRANT ALL ON TABLE "#project#"."CacheExternalIdentifier" TO "CacheAdmin_#project#";

GRANT ALL ON TABLE "#project#"."CacheExternalIdentifier" TO "CacheAdmin";


--#####################################################################################################################
--######   CacheIdentificationUnitGeoAnalysis   #######################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheIdentificationUnitGeoAnalysis"(
	"CollectionSpecimenID" integer NOT NULL,
	"IdentificationUnitID" integer NOT NULL,
	"AnalysisDate"  timestamp without time zone NULL,
	"Geography" text NULL,
	"Geometry" text NULL,
	"ResponsibleName" character varying(255) NULL,
	"ResponsibleAgentURI" character varying(255) NULL,
	"Notes" text NULL,
 CONSTRAINT "CacheIdentificationUnitGeoAnalysis_pkey" PRIMARY KEY ("CollectionSpecimenID","IdentificationUnitID","AnalysisDate") 
 );

 ALTER TABLE "#project#"."CacheIdentificationUnitGeoAnalysis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheIdentificationUnitGeoAnalysis" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheIdentificationUnitGeoAnalysis" TO "CacheUser";



--#####################################################################################################################
--######   CacheCollectionSpecimenReference  ##########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionSpecimenReference"(
	"CollectionSpecimenID" integer NOT NULL,
	"ReferenceID" integer NOT NULL,
	"ReferenceTitle" character varying(400) NOT NULL,
	"ReferenceURI" character varying(500) NULL,
	"IdentificationUnitID" integer NULL,
	"SpecimenPartID" integer NULL,
	"ReferenceDetails" character varying(500) NULL,
	"Notes" text NULL,
	"ResponsibleName" character varying(255) NULL,
	"ResponsibleAgentURI" character varying(255) NULL,
CONSTRAINT "CacheCollectionSpecimenReference_pkey" PRIMARY KEY ("CollectionSpecimenID", "ReferenceID") 
 );

ALTER TABLE "#project#"."CacheCollectionSpecimenReference"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenReference" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenReference" TO GROUP "CacheUser";



--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 10 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



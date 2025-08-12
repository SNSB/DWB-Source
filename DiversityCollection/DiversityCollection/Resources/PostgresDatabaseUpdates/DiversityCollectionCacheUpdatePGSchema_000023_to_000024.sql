--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 23
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   CacheMetadata - DatasetDetails to text, RecordUri to 500 and default for LogLastTransfer  ##################
--#####################################################################################################################

DROP VIEW IF EXISTS public."ABCD_Metadata";

ALTER TABLE "#project#"."CacheMetadata" ALTER COLUMN "LogLastTransfer" SET DEFAULT now();
ALTER TABLE "#project#"."CacheMetadata" ALTER COLUMN "RecordUri" TYPE character varying(500) COLLATE pg_catalog."default";
ALTER TABLE "#project#"."CacheMetadata" ALTER COLUMN "DatasetDetails" SET DATA TYPE text COLLATE pg_catalog."default";


--#####################################################################################################################
--######   CacheProjectAgent - add ProjectAgentID and change PK in case previous update failed ########################
--#####################################################################################################################

TRUNCATE TABLE "#project#"."CacheProjectAgent";

ALTER TABLE "#project#"."CacheProjectAgent"
    ADD COLUMN IF NOT EXISTS "ProjectAgentID" integer NOT NULL;

ALTER TABLE "#project#"."CacheProjectAgent" DROP CONSTRAINT IF EXISTS "CacheProjectAgent_pkey";
ALTER TABLE "#project#"."CacheProjectAgent" DROP CONSTRAINT IF EXISTS "CacheProjectAgent_Temp_pkey";
ALTER TABLE "#project#"."CacheProjectAgent" DROP CONSTRAINT IF EXISTS "CacheProjectAgent_Temp_pkey1";

ALTER TABLE "#project#"."CacheProjectAgent"
    ADD CONSTRAINT "CacheProjectAgent_pkey" PRIMARY KEY ("ProjectID", "ProjectAgentID");


--#####################################################################################################################
--######   Remove NOT NULL constraint from AgentURI in tables CacheProjectAgent and CacheProjectAgentRole  ############
--#####################################################################################################################

ALTER TABLE "#project#"."CacheProjectAgent"
    ALTER COLUMN "AgentURI" DROP NOT NULL;

ALTER TABLE "#project#"."CacheProjectAgentRole"
    ALTER COLUMN "AgentURI" DROP NOT NULL;

ALTER TABLE "#project#"."CacheProjectAgentRole"
    ALTER COLUMN "AgentName" DROP NOT NULL;


--#####################################################################################################################
--######   New table CacheCollectionSpecimenPartDescription  ##########################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."CacheCollectionSpecimenPartDescription"(
	"CollectionSpecimenID" integer NOT NULL,
	"SpecimenPartID" integer NOT NULL,
	"PartDescriptionID" integer NOT NULL,
	"IdentificationUnitID" integer NULL,
	"Description" text NULL,
	"DescriptionTermURI" character varying(500) NULL,
	"Notes" text NULL,
	"DescriptionHierarchyCache" text NULL,
 CONSTRAINT "PK_CacheCollectionSpecimenPartDescription" PRIMARY KEY 
(
	"CollectionSpecimenID",
	"SpecimenPartID",
	"PartDescriptionID"
))
TABLESPACE pg_default;

ALTER TABLE "#project#"."CacheCollectionSpecimenPartDescription"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenPartDescription" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenPartDescription" TO "CacheUser";


--#####################################################################################################################
--######   New table CacheCollectionSpecimenProcessingMethod  #########################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."CacheCollectionSpecimenProcessingMethod"(
	"CollectionSpecimenID" integer NOT NULL,
	"SpecimenProcessingID" integer NOT NULL,
	"MethodID" integer NOT NULL,
	"MethodMarker" character varying(50) NOT NULL,
	"ProcessingID" integer NOT NULL,
 CONSTRAINT "PK_CacheCollectionSpecimenProcessingMethod" PRIMARY KEY 
(
	"CollectionSpecimenID",
	"SpecimenProcessingID",
	"MethodID",
	"ProcessingID",
	"MethodMarker"
))
TABLESPACE pg_default;

ALTER TABLE "#project#"."CacheCollectionSpecimenProcessingMethod"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenProcessingMethod" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenProcessingMethod" TO "CacheUser";


--#####################################################################################################################
--######   New table CacheCollectionSpecimenProcessingMethodParameter  ################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."CacheCollectionSpecimenProcessingMethodParameter"(
	"CollectionSpecimenID" integer NOT NULL,
	"SpecimenProcessingID" integer NOT NULL,
	"ProcessingID" integer NOT NULL,
	"MethodID" integer NOT NULL,
	"MethodMarker" character varying(50) NOT NULL,
	"ParameterID" integer NOT NULL,
	"Value" text NOT NULL,
 CONSTRAINT "PK_CacheCollectionSpecimenProcessingMethodParameterParameter" PRIMARY KEY 
(
	"CollectionSpecimenID",
	"SpecimenProcessingID",
	"ProcessingID",
	"MethodID",
	"ParameterID",
	"MethodMarker"
))
TABLESPACE pg_default;


ALTER TABLE "#project#"."CacheCollectionSpecimenProcessingMethodParameter"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenProcessingMethodParameter" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenProcessingMethodParameter" TO "CacheUser";



--#####################################################################################################################
--######   New table CacheIdentificationUnitAnalysisMethod  ###########################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."CacheIdentificationUnitAnalysisMethod"(
	"CollectionSpecimenID" integer NOT NULL,
	"IdentificationUnitID" integer NOT NULL,
	"MethodID" integer NOT NULL,
	"AnalysisID" integer NOT NULL,
	"AnalysisNumber" character varying(50) NOT NULL,
	"MethodMarker" character varying(50) NOT NULL,
 CONSTRAINT "PK_CacheIdentificationUnitAnalysisMethod" PRIMARY KEY 
(
	"CollectionSpecimenID",
	"IdentificationUnitID",
	"MethodID",
	"AnalysisID",
	"AnalysisNumber",
	"MethodMarker"
))
TABLESPACE pg_default;

ALTER TABLE "#project#"."CacheIdentificationUnitAnalysisMethod"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheIdentificationUnitAnalysisMethod" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."CacheIdentificationUnitAnalysisMethod" TO "CacheUser";


--#####################################################################################################################
--######   New table CacheIdentificationUnitAnalysisMethodParameter  ##################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."CacheIdentificationUnitAnalysisMethodParameter"(
	"CollectionSpecimenID" integer NOT NULL,
	"IdentificationUnitID" integer NOT NULL,
	"AnalysisID" integer NOT NULL,
	"AnalysisNumber" character varying(50) NOT NULL,
	"MethodID" integer NOT NULL,
	"MethodMarker" character varying(50) NOT NULL,
	"ParameterID" integer NOT NULL,
	"Value" text NULL,
 CONSTRAINT "PK_CacheIdentificationUnitAnalysisMethodParameter" PRIMARY KEY 
(
	"CollectionSpecimenID",
	"IdentificationUnitID",
	"AnalysisID",
	"AnalysisNumber",
	"MethodID",
	"MethodMarker",
	"ParameterID"
))
TABLESPACE pg_default;

ALTER TABLE "#project#"."CacheIdentificationUnitAnalysisMethodParameter"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheIdentificationUnitAnalysisMethodParameter" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."CacheIdentificationUnitAnalysisMethodParameter" TO "CacheUser";



--#####################################################################################################################
--######   version  ###################################################################################################
--#####################################################################################################################

DROP FUNCTION "#project#".version();
CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 23 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



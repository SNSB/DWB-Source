--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 23
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   new table CacheCollectionProject  ##########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionProject"
(
    "CollectionSpecimenID" integer NOT NULL,
    "ProjectID" integer NOT NULL,
    CONSTRAINT "CacheCollectionProject_pkey" PRIMARY KEY ("CollectionSpecimenID", "ProjectID")
)

TABLESPACE pg_default;

ALTER TABLE "#project#"."CacheCollectionProject"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimen" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimen" TO "CacheUser";


--#####################################################################################################################
--######   Remove any ownership of project related roles  #############################################################
--#####################################################################################################################

-- aus skript 00.00.01

GRANT USAGE ON SCHEMA "#project#" TO "CacheUser";

ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
    GRANT SELECT ON TABLES
    TO "CacheUser";

ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
    GRANT EXECUTE ON FUNCTIONS
    TO "CacheUser";

ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
    GRANT ALL ON TABLES
    TO "CacheAdmin";

ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
    GRANT ALL ON FUNCTIONS
    TO "CacheAdmin";

-- aus skript 00.00.03

ALTER TABLE "#project#"."CacheLocalisationSystem"
  OWNER TO "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheLocalisationSystem" TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CacheUser";
GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CacheAdmin";

-- aus skript 00.00.10

GRANT ALL ON TABLE "#project#"."CacheExternalIdentifier" TO "CacheAdmin";

-- aus skript 00.00.12

GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenReference" TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionExternalDatasource" TO "CacheAdmin";

-- aus skript 00.00.16

GRANT ALL ON TABLE "#project#"."PackageAddOn" TO "CacheAdmin";

-- aus skript 00.00.18

ALTER TABLE "#project#"."CacheProjectAgent"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheProjectAgent" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheProjectAgent" TO "CacheUser";
ALTER TABLE "#project#"."CacheMetadata"
  OWNER TO "CacheAdmin";
ALTER TABLE "#project#"."CacheProjectReference"
  OWNER TO "CacheAdmin";

-- aus skript 00.00.19

GRANT ALL ON SCHEMA "#project#" TO "CacheAdmin";

ALTER TABLE "#project#"."CacheAnalysis"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheAnnotation"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollection"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionAgent"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionEvent"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionEventLocalisation"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionEventProperty"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionExternalDatasource"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionSpecimen"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionSpecimenImage"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionSpecimenPart"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionSpecimenProcessing"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionSpecimenReference"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheCollectionSpecimenRelation"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheExternalIdentifier"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheIdentification"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheIdentificationUnit"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheIdentificationUnitAnalysis"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheIdentificationUnitGeoAnalysis"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheIdentificationUnitInPart"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheLocalisationSystem"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheMetadata"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheProcessing"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheProjectAgent"
  OWNER TO "CacheAdmin";

ALTER TABLE "#project#"."CacheProjectReference"
  OWNER TO "CacheAdmin";

-- aus skript 00.00.21

GRANT ALL ON TABLE "#project#"."CacheProjectAgentRole" TO "CacheAdmin";

-- aus skript 00.00.22

ALTER TABLE "#project#"."CacheMethod"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheMethod" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheMethod" TO "CacheUser";

ALTER TABLE "#project#"."CacheCollectionEventMethod"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionEventMethod" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionEventMethod" TO "CacheUser";
ALTER TABLE "#project#"."CacheCollectionEventParameterValue"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionEventParameterValue" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionEventParameterValue" TO "CacheUser";

ALTER TABLE "#project#"."CacheCount"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCount" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCount" TO "CacheUser";

-- sonstige
ALTER TABLE "#project#"."CacheCount"
  OWNER TO "CacheAdmin";



--#####################################################################################################################
--######   "CacheMetadata" - ALTER COLUMN DatasetDetails to type text  ################################################
--#####################################################################################################################

DROP VIEW IF EXISTS public."ABCD_Metadata";

ALTER TABLE "#project#"."CacheMetadata"
    ALTER COLUMN "DatasetDetails" type text COLLATE pg_catalog."default";


--#####################################################################################################################
--######   CachePublicUser - Add grant to local group  ################################################################
--#####################################################################################################################

GRANT "CachePublicUser" TO "CacheUser";


--#####################################################################################################################
--######   CacheProjectAgent - add ProjectAgentID and change PK #######################################################
--#####################################################################################################################

TRUNCATE TABLE "#project#"."CacheProjectAgent";

ALTER TABLE "#project#"."CacheProjectAgent"
    ADD COLUMN "ProjectAgentID" integer NOT NULL;

ALTER TABLE "#project#"."CacheProjectAgent" DROP CONSTRAINT IF EXISTS "CacheProjectAgent_pkey";
ALTER TABLE "#project#"."CacheProjectAgent" DROP CONSTRAINT IF EXISTS "CacheProjectAgent_Temp_pkey";
ALTER TABLE "#project#"."CacheProjectAgent" DROP CONSTRAINT IF EXISTS "CacheProjectAgent_Temp_pkey1";

ALTER TABLE "#project#"."CacheProjectAgent"
    ADD CONSTRAINT "CacheProjectAgent_pkey" PRIMARY KEY ("ProjectID", "ProjectAgentID");


--#####################################################################################################################
--######   CacheProjectAgentRole - add ProjectAgentID and change PK ###################################################
--#####################################################################################################################

TRUNCATE TABLE "#project#"."CacheProjectAgentRole";

ALTER TABLE "#project#"."CacheProjectAgentRole"
    ADD COLUMN "ProjectAgentID" integer NOT NULL;

ALTER TABLE "#project#"."CacheProjectAgentRole" DROP CONSTRAINT IF EXISTS "CacheProjectAgentRole_pkey";
ALTER TABLE "#project#"."CacheProjectAgentRole" DROP CONSTRAINT IF EXISTS "CacheProjectAgentRole_Temp_pkey";
ALTER TABLE "#project#"."CacheProjectAgentRole" DROP CONSTRAINT IF EXISTS "CacheProjectAgentRole_Temp_pkey1";

ALTER TABLE "#project#"."CacheProjectAgentRole"
    ADD CONSTRAINT "CacheProjectAgentRole_pkey" PRIMARY KEY ("ProjectID", "ProjectAgentID", "AgentRole");


--#####################################################################################################################
--######  Removing OIDs from tables    ################################################################################
--#####################################################################################################################

ALTER TABLE "#project#"."CacheAnalysis" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheAnnotation" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollection" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionAgent" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionEvent" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionEventLocalisation" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionEventMethod" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionEventParameterValue" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionEventProperty" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionExternalDatasource" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionSpecimen" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionSpecimenImage" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionSpecimenPart" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionSpecimenProcessing" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionSpecimenReference" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCollectionSpecimenRelation" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheCount" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheExternalIdentifier" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheIdentification" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheIdentificationUnit" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheIdentificationUnitAnalysis" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheIdentificationUnitGeoAnalysis" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheIdentificationUnitInPart" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheLocalisationSystem" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheMetadata" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheMethod" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheProcessing" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheProjectAgent" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheProjectAgentRole" SET WITHOUT OIDS;
ALTER TABLE "#project#"."CacheProjectReference" SET WITHOUT OIDS;
ALTER TABLE "#project#"."Package" SET WITHOUT OIDS;
ALTER TABLE "#project#"."PackageAddOn" SET WITHOUT OIDS;




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



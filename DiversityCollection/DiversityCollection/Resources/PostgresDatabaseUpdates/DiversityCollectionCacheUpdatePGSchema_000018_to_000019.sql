--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 19
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   Grant for CacheAdmin in Schema    ##########################################################################
--#####################################################################################################################

--GRANT ALL ON SCHEMA "#project#" TO "CacheAdmin_#project#";
GRANT ALL ON SCHEMA "#project#" TO "CacheAdmin";

--#####################################################################################################################
--######   setting ownership for tables to CacheAdmin_#project#     ###################################################
--#####################################################################################################################

--ALTER TABLE "#project#"."CacheAnalysis"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheAnnotation"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollection"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionAgent"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionEvent"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionEventLocalisation"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionEventProperty"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionExternalDatasource"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionSpecimen"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionSpecimenImage"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionSpecimenPart"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionSpecimenProcessing"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionSpecimenReference"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheCollectionSpecimenRelation"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheExternalIdentifier"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheIdentification"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheIdentificationUnit"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheIdentificationUnitAnalysis"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheIdentificationUnitGeoAnalysis"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheIdentificationUnitInPart"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheLocalisationSystem"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheMetadata"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheProcessing"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheProjectAgent"
--  OWNER TO "CacheAdmin_#project#";

--ALTER TABLE "#project#"."CacheProjectReference"
--  OWNER TO "CacheAdmin_#project#";

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



--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

DROP FUNCTION "#project#".version();
CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 19 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



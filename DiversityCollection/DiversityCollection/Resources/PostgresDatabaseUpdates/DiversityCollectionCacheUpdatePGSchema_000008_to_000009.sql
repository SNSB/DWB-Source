--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 9
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################




--#####################################################################################################################
--######   CacheCollectionSpecimenProcessing - PK         #############################################################
--#####################################################################################################################

ALTER TABLE "#project#"."CacheCollectionSpecimenProcessing" 
DROP CONSTRAINT "CacheCollectionSpecimenProcessing_pkey";

ALTER TABLE "#project#"."CacheCollectionSpecimenProcessing"
  ADD CONSTRAINT "CacheCollectionSpecimenProcessing_pkey" PRIMARY KEY ("CollectionSpecimenID", "SpecimenProcessingID");

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 9 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 20
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   CacheMetadata - RecordURI -> nvarchar(500), DatasetDetails -> text    ######################################
--#####################################################################################################################

ALTER TABLE "#project#"."CacheMetadata" ALTER COLUMN "RecordURI" SET DATA TYPE character varying(500);
ALTER TABLE "#project#"."CacheMetadata" ALTER COLUMN "DatasetDetails" SET DATA TYPE text COLLATE pg_catalog."default";


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
   SELECT 20 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



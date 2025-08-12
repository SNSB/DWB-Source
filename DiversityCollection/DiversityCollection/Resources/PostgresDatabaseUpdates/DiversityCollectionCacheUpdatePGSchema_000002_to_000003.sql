--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 1
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   CacheMetadata: Add BaseURL and LogLastTransfer  ############################################################
--#####################################################################################################################

ALTER TABLE "#project#"."CacheMetadata" ADD "BaseURL" character varying(255) NULL;

ALTER TABLE "#project#"."CacheMetadata" ADD "LogLastTransfer"  timestamp without time zone default now()::timestamp without time zone;


--#####################################################################################################################
--######   CacheLocalisationSystem   ##################################################################################
--#####################################################################################################################


CREATE TABLE "#project#"."CacheLocalisationSystem"
(
  "LocalisationSystemID" integer NOT NULL,
  "DisplayText" character varying(500),
  "Sequence" integer NOT NULL,
  "ParsingMethodName" character varying(50),
  CONSTRAINT "CacheLocalisationSystem_pkey" PRIMARY KEY ("LocalisationSystemID")
);
--ALTER TABLE "#project#"."CacheLocalisationSystem"
--  OWNER TO "CacheAdmin_#project#";
ALTER TABLE "#project#"."CacheLocalisationSystem"
  OWNER TO "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheLocalisationSystem" TO postgres;
GRANT SELECT ON TABLE "#project#"."CacheLocalisationSystem" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheLocalisationSystem" TO "CacheUser_#project#";
--GRANT ALL ON TABLE "#project#"."CacheLocalisationSystem" TO "CacheAdmin_#project#";
GRANT ALL ON TABLE "#project#"."CacheLocalisationSystem" TO "CacheAdmin";


--#####################################################################################################################
--######   highresolutionimagepath   ######################################################################################
--#####################################################################################################################


--GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CacheUser_#project#";
--GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CacheAdmin_#project#";
GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CacheUser";
GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CacheAdmin";


--#####################################################################################################################
--######   version   ######################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 3 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;


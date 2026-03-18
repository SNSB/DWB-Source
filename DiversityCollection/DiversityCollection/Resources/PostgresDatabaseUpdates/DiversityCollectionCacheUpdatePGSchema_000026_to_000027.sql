--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 27
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   New table CacheParameter  ##################################################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."CacheParameter"(
	"MethodID" integer NOT NULL,
	"ParameterID" integer NOT NULL,
	"DisplayText" character varying(50) NULL,
	"Description" text NULL,
	"ParameterURI" character varying(255) NULL,
	"DefaultValue" text NULL,
	"Notes" text NULL,
    CONSTRAINT "CacheParameter_pkey" PRIMARY KEY ("ParameterID", "MethodID")
)
TABLESPACE pg_default;

ALTER TABLE "#project#"."CacheParameter"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheParameter" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."CacheParameter" TO "CacheUser";


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
   SELECT 27 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



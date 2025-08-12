--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 25
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   New table CacheProjectDescriptor  ##########################################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS "#project#"."CacheProjectDescriptor"(
	"ID" integer NOT NULL,
	"ProjectID" integer NOT NULL,
	"Language" character varying(5) NOT NULL,
	"Element" character varying(80) NOT NULL,
	"Content" character varying(255) NULL,
	"ContentURI" character varying(500) NULL,
 CONSTRAINT "PK_CacheCollectionSpecimenPartDescription" PRIMARY KEY 
(
	"ID"
))
TABLESPACE pg_default;

ALTER TABLE "#project#"."CacheProjectDescriptor"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#"."CacheProjectDescriptor" TO "CacheAdmin";

GRANT SELECT ON TABLE "#project#"."CacheProjectDescriptor" TO "CacheUser";



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
   SELECT 25 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



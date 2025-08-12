--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 13
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   CacheProjectReference   ####################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheProjectReference"
(
  "ProjectID" integer NOT NULL,
  "ReferenceTitle" character varying(255) NOT NULL,
  "ReferenceURI" character varying(255) NULL,
  "ReferenceDetails" character varying(50) NULL,
  "ReferenceType" character varying(255) NULL,
  "Notes" text,
  CONSTRAINT "CacheProjectReference_pkey" PRIMARY KEY ("ProjectID", "ReferenceTitle")
);
ALTER TABLE "#project#"."CacheProjectReference"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheProjectReference" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheProjectReference" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheProjectReference" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."CacheCollectionExternalDatasource" TO "CacheAdmin";


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
   SELECT 13 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



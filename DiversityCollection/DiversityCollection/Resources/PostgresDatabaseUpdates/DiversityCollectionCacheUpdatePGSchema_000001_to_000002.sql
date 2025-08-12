--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 1
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   Package   ##################################################################################################
--#####################################################################################################################


CREATE TABLE "#project#"."Package"
(
  "Package" character varying(50) NOT NULL, -- The name of the package
  "Version" integer, -- The version of the package
  "Description" text, -- Description of the package
  "URL" character varying(500)[], -- A link to a website with further informations about the package
  CONSTRAINT "Package_pkey" PRIMARY KEY ("Package")
);
ALTER TABLE "#project#"."Package"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."Package" TO postgres;
GRANT SELECT ON TABLE "#project#"."Package" TO "CacheUser";
--GRANT UPDATE, INSERT, DELETE ON TABLE "#project#"."Package" TO "CacheEditor";
GRANT ALL ON TABLE "#project#"."Package" TO "CacheAdmin";
COMMENT ON COLUMN "#project#"."Package"."Package" IS 'The name of the package';
COMMENT ON COLUMN "#project#"."Package"."Version" IS 'The version of the package';
COMMENT ON COLUMN "#project#"."Package"."Description" IS 'Description of the package';
COMMENT ON COLUMN "#project#"."Package"."URL" IS 'A link to a website with further informations about the package';



--#####################################################################################################################

ALTER TABLE "#project#"."Package"
  OWNER TO "CacheAdmin";
  
--#####################################################################################################################

GRANT ALL ON TABLE "#project#"."Package" TO postgres;

--#####################################################################################################################

GRANT ALL ON TABLE "#project#"."Package" TO "CacheAdmin";

--#####################################################################################################################

GRANT SELECT ON TABLE "#project#"."Package" TO "CacheUser";

--#####################################################################################################################

COMMENT ON COLUMN "#project#"."Package"."Package" IS 'The name of the package';

--#####################################################################################################################

COMMENT ON COLUMN "#project#"."Package"."Version" IS 'The version of the package';

--#####################################################################################################################

COMMENT ON COLUMN "#project#"."Package"."Description" IS 'Description of the package';

--#####################################################################################################################
--######   version   ######################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 1 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;

--#####################################################################################################################

ALTER FUNCTION "#project#".version()
  OWNER TO "CacheAdmin";

--#####################################################################################################################

GRANT EXECUTE ON FUNCTION "#project#".version() TO "CacheAdmin";

--#####################################################################################################################

GRANT EXECUTE ON FUNCTION "#project#".version() TO "CacheUser";



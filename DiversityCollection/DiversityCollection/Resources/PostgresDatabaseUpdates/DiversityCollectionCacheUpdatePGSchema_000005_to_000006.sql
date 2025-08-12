--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 6
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################




--#####################################################################################################################
--######   CacheProcessing   ########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheProcessing"
(
  	"ProcessingID" integer NOT NULL,
	"ProcessingParentID" integer NULL,
	"DisplayText" character varying(50) NULL,
	"Description" text NULL,
	"Notes" text NULL,
	"ProcessingURI" character varying(255) NULL,
	"OnlyHierarchy" character (10) NULL,
  CONSTRAINT "CacheProcessing_pkey" PRIMARY KEY ("ProcessingID")
);
ALTER TABLE "#project#"."CacheProcessing"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheProcessing" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheProcessing" TO GROUP "CacheUser";


--#####################################################################################################################
--######   version   ######################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 6 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;





--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 5
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   CacheCollectionSpecimenProcessing   ########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionSpecimenProcessing"
(
  	"CollectionSpecimenID" integer NOT NULL,
	"SpecimenProcessingID" integer NULL,
	"SpecimenPartID" integer NOT NULL,
	"ProcessingDate" timestamp without time zone NULL,
	"ProcessingID" integer NULL,
	"Protocoll" character varying(100) NULL,
	"ProcessingDuration" character varying(50) NULL,
	"ResponsibleName" character varying(255) NULL,
	"ResponsibleAgentURI" character varying(255) NULL,
	"Notes" text NULL,
  CONSTRAINT "CacheCollectionSpecimenProcessing_pkey" PRIMARY KEY ("CollectionSpecimenID", "SpecimenPartID")
);
ALTER TABLE "#project#"."CacheCollectionSpecimenProcessing"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenProcessing" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenProcessing" TO GROUP "CacheUser";

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 5 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;





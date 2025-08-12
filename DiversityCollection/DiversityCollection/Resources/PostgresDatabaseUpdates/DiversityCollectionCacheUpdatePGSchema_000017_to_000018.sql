--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 18
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   SETTING THE ROLE   ###########################################################################################
--#####################################################################################################################

--GRANT ALL ON SCHEMA "#project#" TO "CacheAdmin_#project#";
--SET ROLE "CacheAdmin_#project#";

GRANT ALL ON SCHEMA "#project#" TO "CacheAdmin";
SET ROLE "CacheAdmin";


--#####################################################################################################################
--######   CacheProjectAgent    #######################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheProjectAgent"
(
  "ProjectID" integer NOT NULL,
  "AgentName" character varying(255) NOT NULL,
  "AgentURI" character varying(255) NOT NULL,
  "AgentRole" character varying(50),
  "AgentType" character varying(50),
  "Notes" text,
  "AgentSequence" integer,
  CONSTRAINT "CacheProjectAgent_Temp_pkey" PRIMARY KEY ("ProjectID", "AgentName", "AgentURI")
);
--ALTER TABLE "#project#"."CacheProjectAgent"
--  OWNER TO "CacheAdmin_#project#";
--GRANT ALL ON TABLE "#project#"."CacheProjectAgent" TO "CacheAdmin_#project#";
--GRANT SELECT ON TABLE "#project#"."CacheProjectAgent" TO "CacheUser_#project#";

ALTER TABLE "#project#"."CacheProjectAgent"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheProjectAgent" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheProjectAgent" TO "CacheUser";


--#####################################################################################################################
--######   Metadata - ProjectTitle if missing     #####################################################################
--#####################################################################################################################

--ALTER TABLE "#project#"."CacheMetadata"
--  OWNER TO "CacheAdmin_#project#";
ALTER TABLE "#project#"."CacheMetadata"
  OWNER TO "CacheAdmin";



CREATE or replace FUNCTION InsertProjectTitle() RETURNS void AS
$BODY$
declare i INTEGER := 0;
begin
select count(*) from information_schema.Columns C where C.table_schema = '#project#' and C.table_name = 'CacheMetadata' and C.column_name = 'ProjectTitle' into i;
if i = 0
then

	ALTER TABLE "#project#"."CacheMetadata" ADD COLUMN "ProjectTitle" character varying(400);

end if;
end;
$BODY$ LANGUAGE plpgsql;

SELECT InsertProjectTitle(); 


DROP FUNCTION InsertProjectTitle();


--#####################################################################################################################
--######   CacheProjectReference - add URI   ##########################################################################
--#####################################################################################################################


--ALTER TABLE "#project#"."CacheProjectReference"
--  OWNER TO "CacheAdmin_#project#";
ALTER TABLE "#project#"."CacheProjectReference"
  OWNER TO "CacheAdmin";


CREATE or replace FUNCTION InsertURI() RETURNS void AS
$BODY$
declare i INTEGER := 0;
begin
select count(*) from information_schema.Columns C where C.table_schema = '#project#' and C.table_name = 'CacheProjectReference' and C.column_name = 'URI' into i;
if i = 0
then

	ALTER TABLE "#project#"."CacheProjectReference" ADD COLUMN "URI" character varying(500);

end if;
end;
$BODY$ LANGUAGE plpgsql;

SELECT InsertURI(); 


DROP FUNCTION InsertURI();

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
   SELECT 18 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



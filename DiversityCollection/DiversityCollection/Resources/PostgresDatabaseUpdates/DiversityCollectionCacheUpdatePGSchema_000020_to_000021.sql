--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 20
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   CacheProjectAgentRole - new table for agent roles  #########################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheProjectAgentRole"
(
    "ProjectID" integer NOT NULL,
    "AgentName" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "AgentURI" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "AgentRole" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "CacheProjectAgentRole_Temp_pkey" PRIMARY KEY ("ProjectID", "AgentName", "AgentURI", "AgentRole")
)
TABLESPACE pg_default;

--ALTER TABLE "#project#"."CacheProjectAgentRole"
--    OWNER to "CacheAdmin_#project#";
ALTER TABLE "#project#"."CacheProjectAgentRole"
    OWNER to "CacheAdmin";


--GRANT ALL ON TABLE "#project#"."CacheProjectAgentRole" TO "CacheAdmin_#project#";
--GRANT SELECT ON TABLE "#project#"."CacheProjectAgentRole" TO "CacheUser_#project#";

GRANT SELECT ON TABLE "#project#"."CacheProjectAgentRole" TO "CacheUser";
GRANT ALL ON TABLE "#project#"."CacheProjectAgentRole" TO "CacheAdmin";


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
   SELECT 21 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



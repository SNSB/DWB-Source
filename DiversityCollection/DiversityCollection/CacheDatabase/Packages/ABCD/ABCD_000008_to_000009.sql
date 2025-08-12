--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 8
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   Bugfix abcd___projectcitation - restriction to main project  ###############################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd___projectcitation(
	)
    RETURNS SETOF integer 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$


begin

--Setting the role
SET ROLE "CacheAdmin";

-- Clear the table
TRUNCATE TABLE "#project#"."ABCD__ProjectCitation";

-- Insert basic
INSERT INTO "#project#"."ABCD__ProjectCitation"(
            "ProjectID", "ReferenceTitle", "Citation")
SELECT "ProjectID",
    "ReferenceTitle",
    ''
   FROM "#project#"."CacheProjectReference" R WHERE R."ReferenceType" = 'BioCASe (GFBio)' LIMIT 1;
-- insert the Authors
   DECLARE AUTHORS text = '';
	r integer;
	rMax integer = (SELECT max("AgentSequence") 
	FROM "#project#"."CacheProjectAgent" a, "#project#"."CacheProjectAgentRole" ar, public."Agent"	Agent
	WHERE ar."AgentRole" = 'Author' AND a."AgentName" <> '' AND a."AgentName" = ar."AgentName" and a."ProjectID" = ar."ProjectID" and a."AgentURI" = ar."AgentURI"
	and Agent."AgentURI" = a."AgentURI" AND a."ProjectID" = (SELECT "#project#".projectid()));
   BEGIN
	FOR r IN SELECT "AgentSequence" FROM "#project#"."CacheProjectAgent" a, "#project#"."CacheProjectAgentRole" ar, public."Agent"	Agent
	WHERE ar."AgentRole" = 'Author' AND a."AgentName" <> '' AND a."AgentName" = ar."AgentName" and a."ProjectID" = ar."ProjectID" 
	and a."AgentURI" = ar."AgentURI" and a."AgentURI" = Agent."AgentURI" AND a."ProjectID" = (SELECT "#project#".projectid())
	ORDER BY "AgentSequence"
   LOOP
	AUTHORS := (select concat(AUTHORS, case when AUTHORS = '' then '' else case when r = rMax then ' & ' else '; ' end end, 
	case when Agent."InheritedName" <> '' then concat(Agent."InheritedName", ', ') else '' end, Agent."GivenName") 
	from  "#project#"."CacheProjectAgent" a, public."Agent"	Agent 
	where a."AgentSequence" = r and Agent."AgentURI" = a."AgentURI" AND a."ProjectID" = (SELECT "#project#".projectid()) limit 1);
	RETURN NEXT r;
   END LOOP; 
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = AUTHORS;
   END; 
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = 'Anonymous' WHERE "Citation" = '' OR "Citation" IS NULL;
-- Year
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", ' (', date_part('year', current_date)::character varying(4), '). '));
-- Title and [Dataset]
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", "ProjectTitle", '. [Dataset]. ') from "#project#"."CacheMetadata" WHERE "ProjectID" = "#project#".projectid());
-- Version
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", ' Version: ', date_part('year', current_date)::character varying(4), case when date_part('month', current_date) < 10 then '0' else '' end, date_part('month', current_date)::character varying(2), case when date_part('day', current_date) < 10 then '0' else '' end, date_part('day', current_date)::character varying(2)));
-- Publisher
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", '. Data Publisher: ', MIN(a."AgentName"), '.') 
	FROM "#project#"."CacheProjectAgent"  a, "#project#"."CacheProjectAgentRole" r
	WHERE r."AgentRole" = 'Publisher' AND a."AgentName" <> '' AND a."AgentName" = r."AgentName" and a."ProjectID" = r."ProjectID" and a."AgentURI" = r."AgentURI");
-- URI
   UPDATE "#project#"."ABCD__ProjectCitation" 
   SET "Citation" = concat(C."Citation", ' ', case when R."URI" <> '' then concat(R."URI", '.') else '' end) 
	FROM "#project#"."CacheProjectReference" R JOIN "#project#"."ABCD__ProjectCitation" C ON R."ProjectID" = C."ProjectID" AND R."ReferenceTitle" = C."ReferenceTitle";
end;

$BODY$;

ALTER FUNCTION "#project#".abcd___projectcitation()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd___projectcitation() TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd___projectcitation() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd___projectcitation()
    IS 'Setting the content of table ABCD__ProjectCitation according to tables CacheProjectReference, CacheProjectAgent and CacheProjectAgentRole';




--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 9 WHERE "Package" = 'ABCD'
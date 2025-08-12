--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 8
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   ABCD_ContentContact: Restriction to main project   #########################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_ContentContact"
 AS
SELECT P."ProjectID",
    C."Email"::character varying(254) AS "Email",
    case when lower(A."AgentType") = 'person' 
	THEN concat(A."GivenName", ' ', A."InheritedName")  
	--THEN concat(A."InheritedName", ', ', substring(A."GivenName", 1, 1), '.')  
	ELSE case when A."GivenName" is null then P."AgentName" else A."GivenName" end END::character varying(254) AS "Name",
    ''::character varying(254) AS "Phone",
    ''::character varying(254) AS "Address"
   FROM "#project#"."CacheProjectAgent" AS P
   INNER JOIN "#project#"."CacheProjectAgentRole" AS R 
   ON R."ProjectID" = P."ProjectID" AND R."AgentName" = P."AgentName" AND P."ProjectID" = "#project#".projectid()
   LEFT OUTER JOIN public."Agent" AS A
   ON P."AgentURI" = A."AgentURI" LEFT OUTER JOIN public."AgentContactInformation" C
   ON C."AgentID" = A."AgentID"
   WHERE R."AgentRole" = 'Content Contact';



--#####################################################################################################################
--######   ABCD_TechnicalContact: Restriction to main project  ########################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_TechnicalContact"
 AS
SELECT P."ProjectID",
    c."Email"::character varying(254) AS "Email",
        CASE
            WHEN lower(A."AgentType")::text = 'person'::text 
			THEN concat(A."GivenName", ' ', A."InheritedName")  
			--THEN concat(A."InheritedName", ', ', substring(A."GivenName", 1, 1), '.')  
            ELSE case when A."GivenName" is null then P."AgentName" else A."GivenName" end END::character varying(254) AS "Name",
    ''::character varying(254) AS "Phone",
    ''::character varying(254) AS "Address"
   FROM "#project#"."CacheProjectAgent" AS P
   INNER JOIN "#project#"."CacheProjectAgentRole" AS R 
   ON R."ProjectID" = P."ProjectID" AND R."AgentName" = P."AgentName" AND P."ProjectID" = "#project#".projectid()
   LEFT OUTER JOIN public."Agent" AS A
   ON P."AgentURI" = A."AgentURI" LEFT OUTER JOIN public."AgentContactInformation" C
   ON C."AgentID" = A."AgentID"
   WHERE R."AgentRole" = 'Technical Contact';

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 8 WHERE "Package" = 'ABCD'
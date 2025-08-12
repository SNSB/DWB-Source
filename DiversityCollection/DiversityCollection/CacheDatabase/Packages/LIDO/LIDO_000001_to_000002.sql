--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package LIDO to version 2
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   SETTING THE ROLE   #########################################################################################
--#####################################################################################################################

GRANT ALL ON SCHEMA "#project#" TO "CacheAdmin";
SET ROLE "CacheAdmin";

--#####################################################################################################################
--######   LIDO_descriptivMetadata_repository   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."LIDO_descriptivMetadata_repository" AS 
 SELECT "S"."CollectionSpecimenID" AS "ID"
   , CASE WHEN "A"."Identifier" <> '' THEN "A"."Identifier" ELSE '###### UNDEFINED ######' END AS "repositoryName_legalBodyID"
   , "I"."LicenseHolder" AS "repositoryName_legalBodyName_appellationValue"
   , "S"."AccessionNumber" AS "WorkID"
   FROM "#project#"."CacheCollectionSpecimenImage" "I" 
   INNER JOIN "#project#"."CacheCollectionSpecimen" "S" ON "I"."CollectionSpecimenID" = "S"."CollectionSpecimenID"
   LEFT OUTER JOIN public."AgentIdentifier" "A" ON concat("A"."BaseURL"::text, "A"."AgentID"::text) = "I"."LicenseHolderAgentURI";

ALTER TABLE public."LIDO_descriptivMetadata_repository"
    OWNER TO "CacheAdmin";
--COMMENT ON VIEW public."LIDO_descriptivMetadata_repository" IS 'LIDO entity eventSet/event';
--COMMENT ON Column "LIDO_descriptivMetadata_repository"."eventPlace_place_gml" IS 'LIDO entity eventSet/event/eventPlace/place/gml';


--#####################################################################################################################
--######   LIDO_administrativeMetadata_resource   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."LIDO_administrativeMetadata_resource" AS 
 SELECT "I"."CollectionSpecimenID" AS "ID"
   , "I"."LicenseHolder" AS "rightsResource_rightsHolder_legalBodyName_appelationValue"
   , "I"."LicenseType" AS "rightsResource_rightsType_term"
   , "I"."LicenseURI" AS "resourceRepresentation_type"
   , "I"."LicenseYear" AS "resourceDateTaken_displayDate"
   , CASE WHEN "I"."Title" <> '' THEN "I"."Title" ELSE '###### UNDEFINED ######' END AS "resourceDescription"
   FROM "#project#"."CacheCollectionSpecimenImage" "I";

ALTER TABLE public."LIDO_descriptivMetadata_repository"
    OWNER TO "CacheAdmin";
--COMMENT ON VIEW public."LIDO_descriptivMetadata_repository" IS 'LIDO entity eventSet/event';
--COMMENT ON Column "LIDO_descriptivMetadata_repository"."eventPlace_place_gml" IS 'LIDO entity eventSet/event/eventPlace/place/gml';


--#####################################################################################################################
--######   LIDO_descriptivMetadata_title   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."LIDO_descriptivMetadata_title" AS 
 SELECT "U"."CollectionSpecimenID" AS "ID"
   , "U"."LastIdentificationCache" AS "appellationValue"
   FROM "#project#"."CacheIdentificationUnit" "U" WHERE "U"."DisplayOrder" = 1;

ALTER TABLE public."LIDO_descriptivMetadata_title"
    OWNER TO "CacheAdmin";
--COMMENT ON VIEW public."LIDO_descriptivMetadata_title" IS 'LIDO entity eventSet/event';
--COMMENT ON Column "LIDO_descriptivMetadata_title"."eventPlace_place_gml" IS 'LIDO entity eventSet/event/eventPlace/place/gml';


--#####################################################################################################################
--######   LIDO_descriptivMetadata_objectWorkType   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."LIDO_descriptivMetadata_objectWorkType" AS 
 SELECT "P"."CollectionSpecimenID" AS "ID"
   , "P"."MaterialCategory" AS "term"
   FROM "#project#"."CacheCollectionSpecimenPart" "P" 
   INNER JOIN "#project#"."CacheIdentificationUnitInPart" "I" ON "I"."SpecimenPartID" = "P"."SpecimenPartID" AND "I"."DisplayOrder" = 1;

ALTER TABLE public."LIDO_descriptivMetadata_objectWorkType"
    OWNER TO "CacheAdmin";
--COMMENT ON VIEW public."LIDO_descriptivMetadata_objectWorkType" IS 'LIDO entity eventSet/event';
--COMMENT ON Column "LIDO_descriptivMetadata_objectWorkType"."eventPlace_place_gml" IS 'LIDO entity eventSet/event/eventPlace/place/gml';


--#####################################################################################################################
--######   LIDO_descriptivMetadata_objectDescription   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."LIDO_descriptivMetadata_objectWorkType" AS 
 SELECT "I"."CollectionSpecimenID" AS "ID"
   , "I"."Notes" AS "descriptionNoteValue"
   FROM "#project#"."CacheCollectionSpecimenImage" "I";

ALTER TABLE public."LIDO_descriptivMetadata_objectWorkType"
    OWNER TO "CacheAdmin";
--COMMENT ON VIEW public."LIDO_descriptivMetadata_objectWorkType" IS 'LIDO entity eventSet/event';
--COMMENT ON Column "LIDO_descriptivMetadata_objectWorkType"."eventPlace_place_gml" IS 'LIDO entity eventSet/event/eventPlace/place/gml';


--#####################################################################################################################
--######   LIDO_descriptiveMetadata_displayEvent_event   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."LIDO_descriptiveMetadata_displayEvent_event" AS 
 SELECT "S"."CollectionSpecimenID" AS "ID"
   , CASE WHEN "E"."LocalityDescription" <> '' THEN "E"."LocalityDescription" ELSE 'unknown' END AS "eventPlace_displayPlace"
   , CASE WHEN "E"."CollectionYear" IS NULL THEN 'unknown' ELSE 
		CASE WHEN "E"."CollectionMonth" IS NULL THEN "E"."CollectionYear"::text ELSE 
			CASE WHEN "E"."CollectionDay" IS NULL THEN concat(case when "E"."CollectionMonth" < 10 then '0' else '' end, "E"."CollectionMonth"::text, '.', "E"."CollectionYear"::text) 
			ELSE concat(case when "E"."CollectionDay" < 10 then '0' else '' end, "E"."CollectionDay"::text, '.', case when "E"."CollectionMonth" < 10 then '0' else '' end, "E"."CollectionMonth"::text, '.', "E"."CollectionYear"::text) END
		END
   END
   , concat("E"."CollectionDay"::text, '-', "E"."CollectionMonth"::text, '-', "E"."CollectionYear"::text) AS "eventDate_date_displayDate"
   , concat("E"."CollectionDay"::text, '-', "E"."CollectionMonth"::text, '-', "E"."CollectionYear"::text) AS "eventDate_date_earliestDate"
   , concat("E"."CollectionEndDay"::text, '-', "E"."CollectionEndMonth"::text, '-', "E"."CollectionEndYear"::text) AS "eventDate_date_latestDate"
   FROM "#project#"."CacheCollectionEvent" "E" 
   INNER JOIN "#project#"."CacheCollectionSpecimen" "S" ON "E"."CollectionEventID" = "S"."CollectionEventID";

ALTER TABLE public."LIDO_descriptiveMetadata_displayEvent_event"
    OWNER TO "CacheAdmin";
--COMMENT ON VIEW public."LIDO_descriptiveMetadata_displayEvent_event" IS 'LIDO entity eventSet/event';
--COMMENT ON Column "LIDO_descriptiveMetadata_displayEvent_event"."eventPlace_place_gml" IS 'LIDO entity eventSet/event/eventPlace/place/gml';


--#####################################################################################################################
--######   LIDO_descriptiveMetadata_displayEvent_event_eventActor   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."LIDO_descriptiveMetadata_displayEvent_event_eventActor" AS 
 SELECT "A"."CollectionSpecimenID" AS "ID"
   , CASE WHEN "A"."CollectorsName" <> '' THEN "A"."CollectorsName" ELSE 'unknown' END::character varying(255)  AS "actorInrole_actor_nameActorSet_appellationValue"
   , CASE WHEN "A"."CollectorsAgentURI" IS NULL THEN 'unknown' ELSE "A"."CollectorsAgentURI" END AS "CollectorsAgentURI"
   FROM "#project#"."CacheCollectionAgent" "A"
   UNION
 SELECT "S"."CollectionSpecimenID" AS "ID"
    ,'unknown'::character varying(255) AS "actorInrole_actor_nameActorSet_appellationValue"
	,'unknown' AS "CollectorsAgentURI"
   FROM "#project#"."CacheCollectionSpecimen" "S" LEFT OUTER JOIN "#project#"."CacheCollectionAgent" "A"
   ON "S"."CollectionSpecimenID" = "A"."CollectionSpecimenID" WHERE "A"."CollectionSpecimenID" IS NULL;


ALTER TABLE public."LIDO_descriptiveMetadata_displayEvent_event_eventActor"
    OWNER TO "CacheAdmin";
--COMMENT ON VIEW public."LIDO_descriptiveMetadata_displayEvent_event_eventActor" IS 'LIDO entity eventSet/event';
--COMMENT ON Column "LIDO_descriptiveMetadata_displayEvent_event_eventActor"."eventPlace_place_gml" IS 'LIDO entity eventSet/event/eventPlace/place/gml';


--#####################################################################################################################
--######   LIDO_administrativeMetadata_record   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."LIDO_administrativeMetadata_record" AS 
 SELECT "I"."CollectionSpecimenID" AS "ID",
    "I"."URI" AS "recordID_recordInfoLink",
    "I"."Title" AS "title"
   FROM "#project#"."CacheCollectionSpecimenImage" "I";

ALTER TABLE public."LIDO_administrativeMetadata_record"
    OWNER TO "CacheAdmin";
--COMMENT ON VIEW public.LIDO_administrativeMetadata_record IS 'LIDO entity eventSet/event';
--COMMENT ON Column LIDO_administrativeMetadata_record."eventPlace_place_gml" IS 'LIDO entity eventSet/event/eventPlace/place/gml';




--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 2 WHERE "Package" = 'LIDO'
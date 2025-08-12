--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 8
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################




--#####################################################################################################################
--######   CacheCollectionEventLocalisation - Geography   #############################################################
--#####################################################################################################################

ALTER TABLE "#project#"."CacheCollectionEventLocalisation" ADD "Geography" text NULL;



--#####################################################################################################################
--######   CacheCollectionAgent - CollectorsAgentURI   ################################################################
--#####################################################################################################################

ALTER TABLE "#project#"."CacheCollectionAgent" ADD "CollectorsAgentURI" character varying(255) NULL;


--#####################################################################################################################
--######   CacheAnnotation   ##########################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheAnnotation"
(
  "AnnotationID" integer NOT NULL,
  "ReferencedAnnotationID" integer NULL,
  "AnnotationType" character varying(50) NOT NULL,
  "Title" character varying(50),
  "Annotation" text,
  "URI" character varying(255),
  "ReferenceDisplayText" character varying(500),
  "ReferenceURI" character varying(255),
  "SourceDisplayText" character varying(500),
  "SourceURI" character varying(255),
  "ReferencedID" integer NOT NULL,
  "ReferencedTable" character varying(500) NOT NULL,
  CONSTRAINT "CacheAnnotation_pkey" PRIMARY KEY ("AnnotationID")
);



--#####################################################################################################################
--######   CacheCollection   ##########################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollection"
(
  "CollectionID" integer NOT NULL,
  "CollectionParentID" integer NULL,
  "CollectionName" character varying(255) NOT NULL,
  "CollectionAcronym" character varying(10),
  "AdministrativeContactName" character varying(500),
  "AdministrativeContactAgentURI" character varying(255),
  "Description" text,
  "Location" character varying(255),
  "CollectionOwner" character varying(255),
  "DisplayOrder" integer NULL,
  CONSTRAINT "CacheCollection_pkey" PRIMARY KEY ("CollectionID")
);

--#####################################################################################################################
--######   version   ######################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 8 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



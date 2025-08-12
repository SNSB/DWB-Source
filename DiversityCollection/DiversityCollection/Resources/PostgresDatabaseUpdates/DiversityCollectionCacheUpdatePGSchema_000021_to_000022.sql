--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 22
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   CacheMethod - new table for Method  ########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheMethod"
(
	"MethodID" integer NOT NULL,
	"MethodParentID" integer NULL,
	"DisplayText" character varying(50) NULL,
	"Description" text NULL,
	"MethodURI" character varying(255) NULL,
	"Notes" text NULL,
    CONSTRAINT "CacheMethod_pkey" PRIMARY KEY ("MethodID")
)
TABLESPACE pg_default;

--ALTER TABLE "#project#"."CacheMethod"
--    OWNER to "CacheAdmin_#project#";
--GRANT ALL ON TABLE "#project#"."CacheMethod" TO "CacheAdmin_#project#";
--GRANT SELECT ON TABLE "#project#"."CacheMethod" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheMethod" TO "CacheUser_#project#";
ALTER TABLE "#project#"."CacheMethod"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheMethod" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheMethod" TO "CacheUser";



--#####################################################################################################################
--######   CacheCollectionEventMethod - new table for CollectionEventMethod  ##########################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionEventMethod"
(
	"CollectionEventID" integer NOT NULL,
	"MethodID" integer NOT NULL,
	"MethodMarker" character varying(50) NOT NULL,
    CONSTRAINT "CacheCollectionEventMethod_pkey" PRIMARY KEY ("CollectionEventID", "MethodID", "MethodMarker")
)
TABLESPACE pg_default;

--ALTER TABLE "#project#"."CacheCollectionEventMethod"
--    OWNER to "CacheAdmin_#project#";
--GRANT ALL ON TABLE "#project#"."CacheCollectionEventMethod" TO "CacheAdmin_#project#";
--GRANT SELECT ON TABLE "#project#"."CacheCollectionEventMethod" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheCollectionEventMethod" TO "CacheUser_#project#";
ALTER TABLE "#project#"."CacheCollectionEventMethod"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionEventMethod" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionEventMethod" TO "CacheUser";


--#####################################################################################################################
--######   CacheCollectionEventParameterValue - new table for CollectionEventParameterValue  ##########################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionEventParameterValue"
(

	"CollectionEventID" integer NOT NULL,
	"MethodID" integer NOT NULL,
	"MethodMarker" character varying(50) NOT NULL,
	"ParameterID" integer NOT NULL,
	"Value" text NULL,
	"Notes" text NULL,
    CONSTRAINT "CacheCollectionEventParameterValue_pkey" PRIMARY KEY ("CollectionEventID", "MethodID", "MethodMarker", "ParameterID")
)
TABLESPACE pg_default;

--ALTER TABLE "#project#"."CacheCollectionEventParameterValue"
--    OWNER to "CacheAdmin_#project#";
--GRANT ALL ON TABLE "#project#"."CacheCollectionEventParameterValue" TO "CacheAdmin_#project#";
--GRANT SELECT ON TABLE "#project#"."CacheCollectionEventParameterValue" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheCollectionEventParameterValue" TO "CacheUser_#project#";
ALTER TABLE "#project#"."CacheCollectionEventParameterValue"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionEventParameterValue" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionEventParameterValue" TO "CacheUser";


--#####################################################################################################################
--######   CacheCount - new table for Count  ##########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCount"
(
	"Tablename" character varying(200) NOT NULL,
	"TotalCount" integer NULL,
    CONSTRAINT "CacheCount_pkey" PRIMARY KEY ("Tablename")
)
TABLESPACE pg_default;

--ALTER TABLE "#project#"."CacheCount"
--    OWNER to "CacheAdmin_#project#";
--GRANT ALL ON TABLE "#project#"."CacheCount" TO "CacheAdmin_#project#";
--GRANT SELECT ON TABLE "#project#"."CacheCount" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheCount" TO "CacheUser_#project#";

ALTER TABLE "#project#"."CacheCount"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCount" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCount" TO "CacheUser";



--#####################################################################################################################
--######   version  ###################################################################################################
--#####################################################################################################################

DROP FUNCTION "#project#".version();
CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 22 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 12
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   CacheCollectionSpecimenReference - ReferenceURI -> NULL  ###################################################
--#####################################################################################################################

DROP TABLE "#project#"."CacheCollectionSpecimenReference";

CREATE TABLE "#project#"."CacheCollectionSpecimenReference"
(
  "CollectionSpecimenID" integer NOT NULL,
  "ReferenceID" integer NOT NULL,
  "ReferenceTitle" character varying(400) NOT NULL,
  "ReferenceURI" character varying(500) NULL,
  "IdentificationUnitID" integer,
  "SpecimenPartID" integer,
  "ReferenceDetails" character varying(500),
  "Notes" text,
  "ResponsibleName" character varying(255),
  "ResponsibleAgentURI" character varying(255),
  CONSTRAINT "CacheCollectionSpecimenReference_pkey" PRIMARY KEY ("CollectionSpecimenID", "ReferenceID")
);
ALTER TABLE "#project#"."CacheCollectionSpecimenReference"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenReference" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenReference" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenReference" TO "CacheUser_#project#";
--GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenReference" TO "CacheAdmin_#project#";
GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenReference" TO "CacheAdmin";

--#####################################################################################################################
--######   CacheCollectionExternalDatasource   ########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionExternalDatasource"
(
  "ExternalDatasourceID" integer NOT NULL,
  "ExternalDatasourceName" character varying(255) NOT NULL,
  "ExternalDatasourceVersion" character varying(255),
  "Rights" character varying(500),
  "ExternalDatasourceAuthors" character varying(200),
  "ExternalDatasourceURI" character varying(300),
  "ExternalDatasourceInstitution" character varying(300),
  CONSTRAINT "CacheCollectionExternalDatasource_pkey" PRIMARY KEY ("ExternalDatasourceID")
);
ALTER TABLE "#project#"."CacheCollectionExternalDatasource"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionExternalDatasource" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionExternalDatasource" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."CacheCollectionExternalDatasource" TO "CacheUser_#project#";
--GRANT ALL ON TABLE "#project#"."CacheCollectionExternalDatasource" TO "CacheAdmin_#project#";
GRANT ALL ON TABLE "#project#"."CacheCollectionExternalDatasource" TO "CacheAdmin";

--#####################################################################################################################
--######   CacheCollectionSpecimen - ExternalDatasourceID   ###########################################################
--#####################################################################################################################

ALTER TABLE "#project#"."CacheCollectionSpecimen" ADD "ExternalDatasourceID" integer;

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
   SELECT 12 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



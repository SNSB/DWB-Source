
--#####################################################################################################################
--######   "ScientificTerm"           #################################################################################
--#####################################################################################################################

ALTER TABLE public."ScientificTerm" ALTER COLUMN "LogInsertedWhen" SET DEFAULT (now())::timestamp without time zone;



--#####################################################################################################################
--######   TaxonNameExternalDatabase   ################################################################################
--#####################################################################################################################


CREATE TABLE "TaxonNameExternalDatabase"
(
  "ExternalDatabaseID" integer NOT NULL,
  "ExternalDatabaseName" character varying(255),
  "ExternalDatabaseVersion" character varying(255),
  "Rights" character varying(500),
  "ExternalDatabaseAuthors" character varying(200),
  "ExternalDatabaseURI" character varying(300),
  "ExternalDatabaseInstitution" character varying(300),
  "ExternalAttribute_NameID" character varying(255),
  "SourceView" character varying(128),
  CONSTRAINT "TaxonNameExternalDatabase_pkey" PRIMARY KEY ("ExternalDatabaseID")
);
ALTER TABLE "TaxonNameExternalDatabase"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "TaxonNameExternalDatabase" TO "CacheAdmin";
GRANT SELECT ON TABLE "TaxonNameExternalDatabase" TO "CacheUser";



--#####################################################################################################################
--######   TaxonNameExternalID         ################################################################################
--#####################################################################################################################


CREATE TABLE "TaxonNameExternalID"
(
  "NameID" integer NOT NULL,
  "ExternalDatabaseID" integer NOT NULL,
  "ExternalNameURI" character varying(255),
  "SourceView" character varying(128),
  CONSTRAINT "TaxonNameExternalID_pkey" PRIMARY KEY ("NameID", "ExternalDatabaseID")
);
ALTER TABLE "TaxonNameExternalID"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "TaxonNameExternalID" TO "CacheAdmin";
GRANT SELECT ON TABLE "TaxonNameExternalID" TO "CacheUser";



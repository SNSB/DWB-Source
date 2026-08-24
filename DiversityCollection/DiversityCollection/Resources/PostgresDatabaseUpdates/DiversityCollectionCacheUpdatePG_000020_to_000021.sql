
--#####################################################################################################################
--######   Setting permissions - needed after change to latest version of Postgres ####################################
--#####################################################################################################################

--ALTER DEFAULT PRIVILEGES IN SCHEMA public
--GRANT INSERT, SELECT, UPDATE, DELETE ON TABLES TO "CacheAdmin";

GRANT USAGE, CREATE ON SCHEMA public TO "CacheAdmin";

--#####################################################################################################################
--######   TaxonAnalysis: Add columns AnalysisNumber (#403) ###########################################################
--#####################################################################################################################

-- Removing data
DELETE FROM public."TaxonAnalysis";

-- Removing PK
ALTER TABLE IF EXISTS public."TaxonAnalysis" DROP CONSTRAINT IF EXISTS "TaxonAnalysis_pkey";

-- adding new column
ALTER TABLE IF EXISTS public."TaxonAnalysis"
    ADD "AnalysisNumber" integer NOT NULL DEFAULT 1;


-- adding PK
ALTER TABLE IF EXISTS public."TaxonAnalysis"
    ADD CONSTRAINT "TaxonAnalysis_pkey" PRIMARY KEY ("NameID", "BaseURL", "ProjectID", "AnalysisID", "AnalysisNumber");


--#####################################################################################################################
--######   New table TaxonRelation. Issue #402  #######################################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS public."TaxonRelation"
(
    "NameID" integer NOT NULL,
    "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL,
    "RelationType" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "RelationNameURI" character varying(400) COLLATE pg_catalog."default" NOT NULL,
    "TaxonNameCache" character varying(255) COLLATE pg_catalog."default",
    "Stage" character varying(500) COLLATE pg_catalog."default",
    "RelatedStage" character varying(500) COLLATE pg_catalog."default",
    "Notes" text COLLATE pg_catalog."default",
    "SourceView" character varying(128) COLLATE pg_catalog."default",
    CONSTRAINT "TaxonRelation_pkey" PRIMARY KEY ("NameID", "BaseURL", "RelationType", "RelationNameURI")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."TaxonRelation"
    OWNER to "CacheAdmin";

REVOKE ALL ON TABLE public."TaxonRelation" FROM "CachePublicUser";
REVOKE ALL ON TABLE public."TaxonRelation" FROM "CacheUser";
REVOKE ALL ON TABLE public."TaxonRelation" FROM PUBLIC;

GRANT ALL ON TABLE public."TaxonRelation" TO "CacheAdmin";

GRANT SELECT ON TABLE public."TaxonRelation" TO "CachePublicUser";

GRANT SELECT ON TABLE public."TaxonRelation" TO "CacheUser";

GRANT SELECT ON TABLE public."TaxonRelation" TO PUBLIC;






















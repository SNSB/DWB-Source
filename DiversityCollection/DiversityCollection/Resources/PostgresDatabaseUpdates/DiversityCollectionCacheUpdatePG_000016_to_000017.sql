
--#####################################################################################################################
--######  version to CacheAdmin   #####################################################################################
--#####################################################################################################################

GRANT ALL PRIVILEGES ON FUNCTION public."version"() TO "CacheAdmin";

ALTER FUNCTION public.version() OWNER TO "CacheAdmin";

--#####################################################################################################################
--######  AgentImage   ################################################################################################
--#####################################################################################################################


CREATE TABLE IF NOT EXISTS public."AgentImage"
(
    "AgentID" integer NOT NULL,
    "URI" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Description" text COLLATE pg_catalog."default",
    "Type" character varying(50) COLLATE pg_catalog."default",
    "Sequence" integer,
    "SourceView" character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT "AgentImage_pkey" PRIMARY KEY ("AgentID", "URI")
)

TABLESPACE pg_default;

ALTER TABLE public."AgentImage"
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE public."AgentImage" TO "CacheAdmin";

GRANT SELECT ON TABLE public."AgentImage" TO "CacheUser";

GRANT SELECT ON TABLE public."AgentImage" TO PUBLIC;


--#####################################################################################################################
--######  CachePublicUser   ###########################################################################################
--#####################################################################################################################

DO $$
BEGIN
  CREATE ROLE "CachePublicUser" WITH 
  NOLOGIN
  NOSUPERUSER
  INHERIT
  NOCREATEDB
  NOCREATEROLE
  NOREPLICATION;
  EXCEPTION WHEN DUPLICATE_OBJECT THEN
  RAISE NOTICE 'not creating role "CachePublicUser" -- it already exists';
END
$$;

GRANT SELECT ON TABLE public."Agent" TO "CachePublicUser";
GRANT SELECT ON TABLE public."AgentContactInformation" TO "CachePublicUser";
GRANT SELECT ON TABLE public."Gazetteer" TO "CachePublicUser";
GRANT SELECT ON TABLE public."GazetteerExternalDatabase" TO "CachePublicUser";
GRANT SELECT ON TABLE public."ReferenceRelator" TO "CachePublicUser";
GRANT SELECT ON TABLE public."ReferenceTitle" TO "CachePublicUser";
GRANT SELECT ON TABLE public."SamplingPlot" TO "CachePublicUser";
GRANT SELECT ON TABLE public."SamplingPlotLocalisation" TO "CachePublicUser";
GRANT SELECT ON TABLE public."SamplingPlotProperty" TO "CachePublicUser";
GRANT SELECT ON TABLE public."ScientificTerm" TO "CachePublicUser";
GRANT SELECT ON TABLE public."TaxonAnalysis" TO "CachePublicUser";
GRANT SELECT ON TABLE public."TaxonAnalysisCategory" TO "CachePublicUser";
GRANT SELECT ON TABLE public."TaxonAnalysisCategoryValue" TO "CachePublicUser";
GRANT SELECT ON TABLE public."TaxonCommonName" TO "CachePublicUser";
GRANT SELECT ON TABLE public."TaxonList" TO "CachePublicUser";
GRANT SELECT ON TABLE public."TaxonNameExternalDatabase" TO "CachePublicUser";
GRANT SELECT ON TABLE public."TaxonNameExternalID" TO "CachePublicUser";
GRANT SELECT ON TABLE public."TaxonSynonymy" TO "CachePublicUser";

GRANT EXECUTE ON FUNCTION public.diversityworkbenchmodule() TO "CachePublicUser";
GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CachePublicUser";
GRANT EXECUTE ON FUNCTION public.version() TO "CachePublicUser";

GRANT SELECT ON TABLE public."Agent" TO "CachePublicUser";

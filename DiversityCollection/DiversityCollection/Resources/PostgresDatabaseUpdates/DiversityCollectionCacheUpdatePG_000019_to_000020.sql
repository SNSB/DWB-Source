--#####################################################################################################################
--######   TaxonCommonName Add columns ReferenceTitle, LogUpdatedWhen Issue #36 #######################################
--#####################################################################################################################

-- Removing data
DELETE FROM public."TaxonCommonName";

-- Removing PK
ALTER TABLE IF EXISTS public."TaxonCommonName" DROP CONSTRAINT IF EXISTS "TaxonCommonName_pkey";

-- Adaption of column size
ALTER TABLE IF EXISTS public."TaxonCommonName"
    ALTER COLUMN "CommonName" TYPE character varying(220);

ALTER TABLE IF EXISTS public."TaxonCommonName"
    ALTER COLUMN "BaseURL" TYPE character varying(255);

-- adding new columns
ALTER TABLE public."TaxonCommonName" ADD COLUMN IF NOT EXISTS "ReferenceTitle" character varying(220) COLLATE pg_catalog."default" NULL;

ALTER TABLE public."TaxonCommonName" ADD COLUMN IF NOT EXISTS "LogUpdatedWhen" timestamp without time zone NULL;


-- adding PK
ALTER TABLE IF EXISTS public."TaxonCommonName"
    ADD CONSTRAINT "TaxonCommonName_pkey" PRIMARY KEY ("NameID", "BaseURL", "CommonName", "LanguageCode", "CountryCode", "LogUpdatedWhen");


--#####################################################################################################################
--######   ScientificTerm: Add column ExternalID. Issue #49  ##########################################################
--#####################################################################################################################

ALTER TABLE IF EXISTS public."ScientificTerm" ADD COLUMN IF NOT EXISTS "ExternalID" character varying(50) COLLATE pg_catalog."default" NULL;

ALTER TABLE IF EXISTS public."ScientificTerm" ALTER COLUMN "SourceView" SET DATA TYPE character varying(400);






















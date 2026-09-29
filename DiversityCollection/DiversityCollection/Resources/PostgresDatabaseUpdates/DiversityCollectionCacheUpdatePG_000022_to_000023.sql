--#####################################################################################################################
--######   Adaptions to get Name Details from DTN   ###################################################################
--#####################################################################################################################
--#####################################################################################################################
--######   TaxonSynonymy - Add new columns for Name Details ###########################################################
--#####################################################################################################################

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "InfragenericEpithet" character varying(200) NULl COLLATE pg_catalog."default";

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "SpeciesEpithet" character varying(100) NULL COLLATE pg_catalog."default";

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "InfraspecificEpithet" character varying(100) NULL COLLATE pg_catalog."default";

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "Authors" character varying(500) NULL COLLATE pg_catalog."default";

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "BasionymAuthors" character varying(100) NULL COLLATE pg_catalog."default";

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "CombiningAuthors" character varying(255) NULL COLLATE pg_catalog."default";

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "SanctioningAuthor" character varying(100) NULL COLLATE pg_catalog."default";

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "NonNomenclaturalNameSuffix" character varying(200) NULL COLLATE pg_catalog."default";

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "IsRecombination" boolean NULL;

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "YearOfPubl" int NULL;

ALTER TABLE IF EXISTS public."TaxonSynonymy" ADD COLUMN "NomenclaturalCode" character varying(50) NULL COLLATE pg_catalog."default";

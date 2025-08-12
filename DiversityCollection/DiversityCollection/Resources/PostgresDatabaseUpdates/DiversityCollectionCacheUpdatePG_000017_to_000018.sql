--#####################################################################################################################
--######  AgentContactInformation - add column BaseURL  ###############################################################
--#####################################################################################################################

DELETE FROM public."AgentContactInformation";

ALTER TABLE public."AgentContactInformation" DROP CONSTRAINT "AgentContactInformation_pkey";

ALTER TABLE public."AgentContactInformation"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."AgentContactInformation"
    ADD CONSTRAINT "AgentContactInformation_pkey" PRIMARY KEY ("AgentID", "BaseURL", "DisplayOrder");


--#####################################################################################################################
--######  AgentImage - add column BaseURL  ############################################################################
--#####################################################################################################################

DELETE FROM public."AgentImage";

ALTER TABLE public."AgentImage" DROP CONSTRAINT "AgentImage_pkey";

ALTER TABLE public."AgentImage"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."AgentImage"
    ADD CONSTRAINT "AgentImage_pkey" PRIMARY KEY ("AgentID", "BaseURL", "URI");


--#####################################################################################################################
--######  GazetteerExternalDatabase - add column BaseURL  #############################################################
--#####################################################################################################################

DELETE FROM public."GazetteerExternalDatabase";

ALTER TABLE public."GazetteerExternalDatabase" DROP CONSTRAINT "GazetteerExternalDatabase_pkey";

ALTER TABLE public."GazetteerExternalDatabase"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."GazetteerExternalDatabase"
    ADD CONSTRAINT "GazetteerExternalDatabase_pkey" PRIMARY KEY ("ExternalDatabaseID", "BaseURL");


--#####################################################################################################################
--######  ReferenceRelator - add column BaseURL  ######################################################################
--#####################################################################################################################

DELETE FROM public."ReferenceRelator";

ALTER TABLE public."ReferenceRelator" DROP CONSTRAINT "ReferenceRelator_pkey";

ALTER TABLE public."ReferenceRelator"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."ReferenceRelator"
    ADD CONSTRAINT "ReferenceRelator_pkey" PRIMARY KEY ("RefID", "BaseURL", "Role", "Sequence");

--#####################################################################################################################
--######  SamplingPlotLocalisation - add column BaseURL  ##############################################################
--#####################################################################################################################

DELETE FROM public."SamplingPlotLocalisation";

ALTER TABLE public."SamplingPlotLocalisation" DROP CONSTRAINT "SamplingPlotLocalisation_pkey";

ALTER TABLE public."SamplingPlotLocalisation"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."SamplingPlotLocalisation"
    ADD CONSTRAINT "SamplingPlotLocalisation_pkey" PRIMARY KEY ("PlotID", "BaseURL", "LocalisationSystemID");


--#####################################################################################################################
--######  SamplingPlotProperty - add column BaseURL  ##################################################################
--#####################################################################################################################

DELETE FROM public."SamplingPlotProperty";

ALTER TABLE public."SamplingPlotProperty" DROP CONSTRAINT "SamplingPlotProperty_pkey";

ALTER TABLE public."SamplingPlotProperty"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."SamplingPlotProperty"
    ADD CONSTRAINT "SamplingPlotProperty_pkey" PRIMARY KEY ("PlotID", "BaseURL", "PropertyID");




--#####################################################################################################################
--######  ScientificTerm - add columns RepresentationID and BaseURL  ##################################################
--#####################################################################################################################

DELETE FROM public."ScientificTerm";

ALTER TABLE public."ScientificTerm"
    ADD COLUMN IF NOT EXISTS "RepresentationID" integer NOT NULL;

ALTER TABLE public."ScientificTerm"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(255) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."ScientificTerm" DROP CONSTRAINT "ScientificTerm_pkey";

ALTER TABLE public."ScientificTerm"
    ADD CONSTRAINT "ScientificTerm_pkey" PRIMARY KEY ("RepresentationID", "BaseURL");


--#####################################################################################################################
--######  TaxonAnalysis - add column BaseURL  #########################################################################
--#####################################################################################################################

DELETE FROM public."TaxonAnalysis";

ALTER TABLE public."TaxonAnalysis" DROP CONSTRAINT "TaxonAnalysis_pkey";

ALTER TABLE public."TaxonAnalysis"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."TaxonAnalysis"
    ADD CONSTRAINT "TaxonAnalysis_pkey" PRIMARY KEY ("NameID", "BaseURL", "ProjectID", "AnalysisID");


--#####################################################################################################################
--######  TaxonAnalysisCategory - add column BaseURL  #################################################################
--#####################################################################################################################

DELETE FROM public."TaxonAnalysisCategory";

ALTER TABLE public."TaxonAnalysisCategory" DROP CONSTRAINT "TaxonAnalysisCategory_pkey";

ALTER TABLE public."TaxonAnalysisCategory"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."TaxonAnalysisCategory"
    ADD CONSTRAINT "TaxonAnalysisCategory_pkey" PRIMARY KEY ("AnalysisID", "BaseURL");


--#####################################################################################################################
--######  TaxonAnalysisCategoryValue - add column BaseURL  ############################################################
--#####################################################################################################################

DELETE FROM public."TaxonAnalysisCategoryValue";

ALTER TABLE public."TaxonAnalysisCategoryValue" DROP CONSTRAINT "TaxonAnalysisCategoryValue_pkey";

ALTER TABLE public."TaxonAnalysisCategoryValue"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."TaxonAnalysisCategoryValue"
    ADD CONSTRAINT "TaxonAnalysisCategoryValue_pkey" PRIMARY KEY ("AnalysisID", "BaseURL", "AnalysisValue");


--#####################################################################################################################
--######  TaxonCommonName - add column BaseURL  #######################################################################
--#####################################################################################################################

DELETE FROM public."TaxonCommonName";

ALTER TABLE public."TaxonCommonName" DROP CONSTRAINT "TaxonCommonName_pkey";

ALTER TABLE public."TaxonCommonName"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."TaxonCommonName"
    ADD CONSTRAINT "TaxonCommonName_pkey" PRIMARY KEY ("NameID", "BaseURL", "CommonName", "LanguageCode", "CountryCode");


--#####################################################################################################################
--######  TaxonList - add column BaseURL  #############################################################################
--#####################################################################################################################

DELETE FROM public."TaxonList";

ALTER TABLE public."TaxonList" DROP CONSTRAINT "TaxonList_pkey";

ALTER TABLE public."TaxonList"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."TaxonList"
    ADD CONSTRAINT "TaxonList_pkey" PRIMARY KEY ("ProjectID", "BaseURL");


--#####################################################################################################################
--######  TaxonNameExternalDatabase - add column BaseURL  #############################################################
--#####################################################################################################################

DELETE FROM public."TaxonNameExternalDatabase";

ALTER TABLE public."TaxonNameExternalDatabase" DROP CONSTRAINT "TaxonNameExternalDatabase_pkey";

ALTER TABLE public."TaxonNameExternalDatabase"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."TaxonNameExternalDatabase"
    ADD CONSTRAINT "TaxonNameExternalDatabase_pkey" PRIMARY KEY ("ExternalDatabaseID", "BaseURL");


--#####################################################################################################################
--######  TaxonNameExternalID - add column BaseURL  ###################################################################
--#####################################################################################################################

DELETE FROM public."TaxonNameExternalID";

ALTER TABLE public."TaxonNameExternalID" DROP CONSTRAINT "TaxonNameExternalID_pkey";

ALTER TABLE public."TaxonNameExternalID"
    ADD COLUMN IF NOT EXISTS "BaseURL" character varying(500) COLLATE pg_catalog."default" NOT NULL;

ALTER TABLE public."TaxonNameExternalID"
    ADD CONSTRAINT "TaxonNameExternalID_pkey" PRIMARY KEY ("NameID", "BaseURL", "ExternalDatabaseID");


















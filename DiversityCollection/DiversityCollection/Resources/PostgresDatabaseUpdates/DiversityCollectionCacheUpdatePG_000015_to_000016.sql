--#####################################################################################################################
--######  Removing OIDs from tables    ################################################################################
--#####################################################################################################################

ALTER TABLE public."Agent" SET WITHOUT OIDS;
ALTER TABLE public."AgentContactInformation" SET WITHOUT OIDS;
ALTER TABLE public."Gazetteer" SET WITHOUT OIDS;
ALTER TABLE public."GazetteerExternalDatabase" SET WITHOUT OIDS;
ALTER TABLE public."ReferenceRelator" SET WITHOUT OIDS;
ALTER TABLE public."ReferenceTitle" SET WITHOUT OIDS;
ALTER TABLE public."SamplingPlot" SET WITHOUT OIDS;
ALTER TABLE public."SamplingPlotLocalisation" SET WITHOUT OIDS;
ALTER TABLE public."SamplingPlotProperty" SET WITHOUT OIDS;
ALTER TABLE public."ScientificTerm" SET WITHOUT OIDS;
ALTER TABLE public."TaxonAnalysis" SET WITHOUT OIDS;
ALTER TABLE public."TaxonAnalysisCategory" SET WITHOUT OIDS;
ALTER TABLE public."TaxonAnalysisCategoryValue" SET WITHOUT OIDS;
ALTER TABLE public."TaxonCommonName" SET WITHOUT OIDS;
ALTER TABLE public."TaxonList" SET WITHOUT OIDS;
ALTER TABLE public."TaxonNameExternalDatabase" SET WITHOUT OIDS;
ALTER TABLE public."TaxonNameExternalID" SET WITHOUT OIDS;
ALTER TABLE public."TaxonSynonymy" SET WITHOUT OIDS;


--#####################################################################################################################
--######   TaxonAnalysisCategory - Add SortingID     ##################################################################
--#####################################################################################################################

ALTER TABLE public."TaxonAnalysisCategory"
    ADD COLUMN IF NOT EXISTS "SortingID" integer NULL;


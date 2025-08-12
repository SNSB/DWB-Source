

--#####################################################################################################################
--######   TaxonAnalysisCategoryValue    ##############################################################################
--#####################################################################################################################

CREATE TABLE "TaxonAnalysisCategoryValue"
(
  "AnalysisID" integer NOT NULL,
  "AnalysisValue" character varying(255) NOT NULL,
  "Description" character varying(500),
  "DisplayText" character varying(50),
  "DisplayOrder" integer,
  "Notes" character varying(500),
  "SourceView" character varying(128) NULL,
  CONSTRAINT "TaxonAnalysisCategoryValue_pkey" PRIMARY KEY ("AnalysisID", "AnalysisValue")
);
ALTER TABLE "TaxonAnalysisCategoryValue"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "TaxonAnalysisCategoryValue" TO "CacheAdmin";
GRANT SELECT ON TABLE "TaxonAnalysisCategoryValue" TO "CacheUser";


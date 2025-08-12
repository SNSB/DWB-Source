
--#####################################################################################################################
--######   TaxonSynonymy         ######################################################################################
--#####################################################################################################################


DROP TABLE "TaxonSynonymy" CASCADE;

CREATE TABLE "TaxonSynonymy"
(
  "NameID" integer NOT NULL,
  "BaseURL" character varying(255) NOT NULL,
  "TaxonName" character varying(255),
  "AcceptedNameID" integer,
  "AcceptedName" character varying(255),
  "TaxonomicRank" character varying(50),
  "SpeciesGenusNameID" integer,
  "GenusOrSupragenericName" character varying(200),
  "NameParentID" integer,
  "TaxonNameSinAuthor" character varying(2000),
  "LogInsertedWhen" timestamp without time zone DEFAULT (now())::timestamp without time zone,
  "ProjectID" integer,
  "SourceView" character varying(100),
  CONSTRAINT "TaxonSynonymy_pkey" PRIMARY KEY ("NameID", "BaseURL")
);
ALTER TABLE "TaxonSynonymy"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "TaxonSynonymy" TO "CacheAdmin";
GRANT SELECT ON TABLE "TaxonSynonymy" TO "CacheUser";



--#####################################################################################################################
--######   TaxonAnalysis        #######################################################################################
--#####################################################################################################################


CREATE TABLE "TaxonAnalysis"
(
  "NameID" integer NOT NULL,
  "ProjectID" integer NOT NULL,
  "AnalysisID" integer NOT NULL,
  "AnalysisValue" text,
  "SourceView" character varying(128),
  CONSTRAINT "TaxonAnalysis_pkey" PRIMARY KEY ("NameID", "ProjectID", "AnalysisID")
);
ALTER TABLE "TaxonAnalysis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "TaxonAnalysis" TO "CacheAdmin";
GRANT SELECT ON TABLE "TaxonAnalysis" TO "CacheUser";


--#####################################################################################################################
--######   TaxonAnalysisCategory    ###################################################################################
--#####################################################################################################################

CREATE TABLE "TaxonAnalysisCategory"
(
  "AnalysisID" integer NOT NULL,
  "AnalysisParentID" integer,
  "DisplayText" character varying(255),
  "Description" text,
  "SourceView" character varying(128),
  CONSTRAINT "TaxonAnalysisCategory_pkey" PRIMARY KEY ("AnalysisID")
);
ALTER TABLE "TaxonAnalysisCategory"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "TaxonAnalysisCategory" TO "CacheAdmin";
GRANT SELECT ON TABLE "TaxonAnalysisCategory" TO "CacheUser";




--#####################################################################################################################
--######   TaxonCommonName      #######################################################################################
--#####################################################################################################################


CREATE TABLE "TaxonCommonName"
(
  "NameID" integer NOT NULL,
  "CommonName" character varying(300) NOT NULL,
  "LanguageCode" character varying(2) NOT NULL,
  "CountryCode" character varying(2) NOT NULL,
  "SourceView" character varying(128),
  CONSTRAINT "TaxonCommonName_pkey" PRIMARY KEY ("NameID", "CommonName", "LanguageCode", "CountryCode")
);
ALTER TABLE "TaxonCommonName"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "TaxonCommonName" TO "CacheAdmin";
GRANT SELECT ON TABLE "TaxonCommonName" TO "CacheUser";



--#####################################################################################################################
--######   TaxonList            #######################################################################################
--#####################################################################################################################

CREATE TABLE "TaxonList"
(
  "ProjectID" integer NOT NULL,
  "Project" character varying(50),
  "DisplayText" character varying(50),
  "SourceView" character varying(128),
  CONSTRAINT "TaxonList_pkey" PRIMARY KEY ("ProjectID")
);
ALTER TABLE "TaxonList"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "TaxonList" TO "CacheAdmin";
GRANT SELECT ON TABLE "TaxonList" TO "CacheUser";


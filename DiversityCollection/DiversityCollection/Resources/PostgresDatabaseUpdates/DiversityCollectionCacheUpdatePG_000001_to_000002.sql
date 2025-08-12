
--#####################################################################################################################
--######   TaxonSynonymy   ######################################################################################
--#####################################################################################################################


CREATE TABLE "public"."TaxonSynonymy"(
	"NameURI" character varying(255) NULL,
	"AcceptedName" character varying(255) NULL,
	"SynNameURI" character varying(255) NULL,
	"SynonymName" character varying(255) NULL,
	"TaxonomicRank" character varying(50) NULL,
	"GenusOrSupragenericName" character varying(200) NULL,
	"SpeciesGenusNameURI" character varying(255) NULL,
	"TaxonNameSinAuthor" character varying(2000) NULL,
	"LogInsertedWhen" timestamp without time zone NULL,
	"ProjectID" integer NULL,
	"Source" character varying(100) NULL,
CONSTRAINT "TaxonSynonymy_pkey" PRIMARY KEY ("SynNameURI") 
 );

ALTER TABLE "public"."TaxonSynonymy"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "public"."TaxonSynonymy" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "public"."TaxonSynonymy" TO "CacheEditor";
GRANT SELECT ON TABLE "public"."TaxonSynonymy" TO GROUP "CacheUser";






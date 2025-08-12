

--#####################################################################################################################
--######   ReferenceTitle   ###########################################################################################
--#####################################################################################################################


CREATE TABLE "ReferenceTitle"
(
	"BaseURL" character varying (500) NOT NULL,
	"RefType" character varying(10) NOT NULL,
	"RefID" integer NOT NULL,
	"RefDescription_Cache" character varying(255) NOT NULL,
	"Title" character varying(4000) NOT NULL,
	"DateYear" integer NULL,
	"DateMonth" integer NULL,
	"DateDay" integer NULL,
	"DateSuppl" character varying(255) NOT NULL,
	"SourceTitle" character varying(4000) NOT NULL,
	"SeriesTitle" character varying(255) NOT NULL,
	"Periodical" character varying(255) NOT NULL,
	"Volume" character varying(255) NOT NULL,
	"Issue" character varying(255) NOT NULL,
	"Pages" character varying(255) NOT NULL,
	"Publisher" character varying(255) NOT NULL,
	"PublPlace" character varying(255) NOT NULL,
	"Edition" integer NULL,
	"DateYear2" integer NULL,
	"DateMonth2" integer NULL,
	"DateDay2" integer NULL,
	"DateSuppl2" character varying(255) NOT NULL,
	"ISSN_ISBN" character varying(18) NOT NULL,
	"Miscellaneous1" character varying(255) NOT NULL,
	"Miscellaneous2" character varying(255) NOT NULL,
	"Miscellaneous3" character varying(255) NOT NULL,
	"UserDef1" character varying(4000) NOT NULL,
	"UserDef2" character varying(4000) NOT NULL,
	"UserDef3" character varying(4000) NOT NULL,
	"UserDef4" character varying(4000) NOT NULL,
	"UserDef5" character varying(4000) NOT NULL,
	"WebLinks" character varying(4000) NOT NULL,
	"LinkToPDF" character varying(4000) NOT NULL,
	"LinkToFullText" character varying(4000) NOT NULL,
	"RelatedLinks" character varying(4000) NOT NULL,
	"LinkToImages" character varying(4000) NOT NULL,
	"SourceRefID" integer NULL,
	"Language" character varying(50) NOT NULL,
	"CitationText" character varying (1000) NOT NULL,
	"CitationFrom" character varying(255) NOT NULL,
	"LogInsertedWhen" timestamp without time zone NOT NULL,
	"ProjectID" integer NOT NULL,
	"SourceView" character varying(128) NOT NULL,
  CONSTRAINT "ReferenceTitle_pkey" PRIMARY KEY ("BaseURL", "RefID")
);
ALTER TABLE "ReferenceTitle"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ReferenceTitle" TO "CacheAdmin";
GRANT SELECT ON TABLE "ReferenceTitle" TO "CacheUser";


--#####################################################################################################################
--######   ReferenceRelator   #########################################################################################
--#####################################################################################################################


CREATE TABLE "ReferenceRelator"
(
	"RefID" integer NOT NULL,
	"Role" character varying(3) NOT NULL,
	"Sequence" integer NOT NULL,
	"Name" character varying(255) NOT NULL,
	"AgentURI" character varying(255) NULL,
	"SortLabel" character varying(255) NULL,
	"Address" character varying(1000) NULL,
	"SourceView" character varying(128) NOT NULL,
  CONSTRAINT "ReferenceRelator_pkey" PRIMARY KEY ("RefID", "Role", "Sequence")
);
ALTER TABLE "ReferenceRelator"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ReferenceRelator" TO "CacheAdmin";
GRANT SELECT ON TABLE "ReferenceRelator" TO "CacheUser";




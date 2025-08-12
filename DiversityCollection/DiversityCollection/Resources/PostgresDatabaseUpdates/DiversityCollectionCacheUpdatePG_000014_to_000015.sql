--#####################################################################################################################
--######   SamplingPlot    ############################################################################################
--#####################################################################################################################

CREATE TABLE "SamplingPlot"(
	"BaseURL" character varying(500) NOT NULL,
	"PlotID" integer NOT NULL,
	"PartOfPlotID" integer NULL,
	"PlotURI" character varying(255) NULL,
	"PlotIdentifier" character varying(500) NULL,
	"PlotGeography_Cache" text NULL,
	"PlotDescription" text NULL,
	"PlotType" character varying(50) NULL,
	"CountryCache" character varying(50) NULL,
	"LogInsertedWhen" timestamp without time zone DEFAULT (now())::timestamp without time zone,
	"ProjectID" integer NOT NULL,
	"SourceView" character varying(400) NULL,
	CONSTRAINT "SamplingPlot_pkey" PRIMARY KEY ("BaseURL", "PlotID"));

ALTER TABLE "SamplingPlot"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "SamplingPlot" TO "CacheAdmin";
GRANT SELECT ON TABLE "SamplingPlot" TO "CacheUser";
GRANT SELECT ON TABLE "SamplingPlot" TO public;


--#####################################################################################################################
--######   SamplingPlotLocalisation    ################################################################################
--#####################################################################################################################

CREATE TABLE "SamplingPlotLocalisation"
(
  "PlotID" integer NOT NULL,
  "LocalisationSystemID" integer NOT NULL,
  "Location1" character varying(255),
  "Location2" character varying(255),
  "LocationAccuracy" character varying(50),
  "LocationNotes" text,
  "Geography" text,
  "DeterminationDate" timestamp with time zone,
  "AverageAltitudeCache" double precision,
  "AverageLatitudeCache" double precision,
  "AverageLongitudeCache" double precision,
  "SourceView" character varying(500),
  CONSTRAINT "SamplingPlotLocalisation_pkey" PRIMARY KEY ("PlotID", "LocalisationSystemID"));

ALTER TABLE "SamplingPlotLocalisation"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "SamplingPlotLocalisation" TO "CacheAdmin";
GRANT SELECT ON TABLE "SamplingPlotLocalisation" TO "CacheUser";
GRANT SELECT ON TABLE "SamplingPlotLocalisation" TO public;


--#####################################################################################################################
--######   SamplingPlotProperty    ####################################################################################
--#####################################################################################################################

CREATE TABLE "SamplingPlotProperty"
(
  "PlotID" integer NOT NULL,
  "PropertyID" integer NOT NULL,
  "DisplayText" character varying(255),
  "PropertyURI" character varying(255),
  "PropertyHierarchyCache" text,
  "PropertyValue" character varying(255),
  "Notes" text,
  "AverageValueCache" double precision,
  "SourceView" character varying(500),
  CONSTRAINT "SamplingPlotProperty_pkey" PRIMARY KEY ("PlotID", "PropertyID"));

ALTER TABLE "SamplingPlotProperty"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "SamplingPlotProperty" TO "CacheAdmin";
GRANT SELECT ON TABLE "SamplingPlotProperty" TO "CacheUser";
GRANT SELECT ON TABLE "SamplingPlotProperty" TO public;







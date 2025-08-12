-- FloraRaster

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package FloraRaster to version 8
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_RoteListeBayern2024   #########################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "RoteListeBayern2024"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 74
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024" TO "CacheAdmin";

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 8 WHERE "Package" = 'FloraRaster'
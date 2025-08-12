-- FloraRaster

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package FloraRaster to version 5
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   FloraRaster__status   ######################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__status" AS 
 SELECT a."CollectionSpecimenID",
    a."IdentificationUnitID",
    a."AnalysisResult" AS flor_status_orig,
    a."AnalysisResult" AS flor_status_korrigiert
   FROM "#project#"."CacheIdentificationUnitAnalysis" a
  WHERE a."AnalysisID" = 2 AND (EXISTS ( SELECT l."CollectionSpecimenID"
           FROM "#project#"."CacheIdentificationUnitAnalysis" l
          GROUP BY l."CollectionSpecimenID", l."IdentificationUnitID", l."AnalysisID"
         HAVING l."IdentificationUnitID" = a."IdentificationUnitID" AND l."AnalysisID" = a."AnalysisID" AND a."AnalysisNumber"::text = max(l."AnalysisNumber"::text)));

/*
-- version with lateral join - ist langsamer als obere Sicht (4 statt 3 sec fuer 100000 Zeilen)

CREATE OR REPLACE VIEW "#project#"."FloraRaster__status" AS 
 SELECT a."CollectionSpecimenID",
    a."IdentificationUnitID",
    a."AnalysisResult" AS flor_status_orig,
    a."AnalysisResult" AS flor_status_korrigiert
   FROM "#project#"."CacheIdentificationUnitAnalysis" as a
     JOIN LATERAL (select l."IdentificationUnitID" from "#project#"."CacheIdentificationUnitAnalysis" as l Where a."IdentificationUnitID" = l."IdentificationUnitID" and a."IdentificationUnitID" = l."IdentificationUnitID" 
     and a."CollectionSpecimenID" = l."CollectionSpecimenID" and a."AnalysisResult" = l."AnalysisResult" and a."AnalysisID" = l."AnalysisID" and l."AnalysisID" = 2 order by l."AnalysisResult" desc limit 1) i on true;

*/

DROP VIEW "#project#"."FloraRaster__status_flor_aktuell";
DROP VIEW "#project#"."FloraRaster__status_flor_korrigiert";
DROP VIEW "#project#"."FloraRaster__status_flor_orig";

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 5 WHERE "Package" = 'FloraRaster'
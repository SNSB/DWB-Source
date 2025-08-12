-- FloraRaster

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package FloraRaster to version 4
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_EndemismusFuerBayern   ########################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" AS 
 SELECT a."NameID",
    min(a."AnalysisValue"::text)::character varying(50) AS "EndemismusFuerBayern"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 3
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_RoteListeBayern2003   #########################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" AS 
 SELECT a."NameID",
    min(a."AnalysisValue"::text)::character varying(50) AS "RoteListeBayern2003"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 1
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" TO "CacheAdmin_#project#";




--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996    ###################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" AS 
 SELECT a."NameID",
    min(a."AnalysisValue"::text)::character varying(50) AS "RoteListeDeutschland1996"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 6
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" TO "CacheAdmin_#project#";



--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern       ###################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" AS 
 SELECT a."NameID",
    min(a."AnalysisValue"::text)::character varying(50) AS "SchutzsstatusInBayern"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 7
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_Schutzverordnung   ############################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" AS 
 SELECT a."NameID",
    min(a."AnalysisValue"::text)::character varying(50) AS "Schutzverordnung"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 9
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" TO "CacheAdmin_#project#";

--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_VerantwortungBayerns   ########################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" AS 
 SELECT a."NameID",
    min(a."AnalysisValue"::text)::character varying(50) AS "VerantwortungBayerns"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 5
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands   ###################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" AS 
 SELECT a."NameID",
    min(a."AnalysisValue"::text)::character varying(50) AS "VerantwortungDeutschlands"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 4
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" TO "CacheAdmin_#project#";



--#####################################################################################################################
--######   FloraRaster_TaxRef   #######################################################################################
--#####################################################################################################################


DROP VIEW "#project#"."FloraRaster_TaxRef";

CREATE OR REPLACE VIEW "#project#"."FloraRaster_TaxRef" AS 
 SELECT t."NameID" AS taxnr,
    t."AcceptedNameID" AS sipnr,
    t."NameParentID" AS aggnr,
    t."TaxonomicRank" AS rang,
    a."AnalyseStatusFuerBayern",
    t."TaxonName" AS "SippennameMitAutoren",
    t."TaxonNameSinAuthor" AS "SippennameOhneAutoren",
    c."DeutscherName",
    ae."EndemismusFuerBayern",
    ar."RoteListeBayern2003",
    a96."RoteListeDeutschland1996",
    asb."SchutzsstatusInBayern",
    asv."Schutzverordnung",
    avb."VerantwortungBayerns",
    avd."VerantwortungDeutschlands"
   FROM "#project#"."FloraRaster__TaxRef_CommonNames" c
     RIGHT JOIN "TaxonSynonymy" t ON c."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis" a ON a."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" ae ON ae."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" ar ON ar."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" a96 ON a96."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" asb ON asb."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" asv ON asv."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" avb ON avb."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" avd ON avd."NameID" = t."NameID";

ALTER TABLE "#project#"."FloraRaster_TaxRef"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_TaxRef" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_TaxRef" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_TaxRef" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster_TaxRef" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 4 WHERE "Package" = 'FloraRaster'
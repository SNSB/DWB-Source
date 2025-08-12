-- FloraRaster

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package FloraRaster to version 2
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis   ######################################################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis" AS 
 SELECT a."NameID",
    min(a."AnalysisValue"::text)::character varying(50) AS "AnalyseStatusFuerBayern"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 2
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis" TO "CacheUser";
  

--#####################################################################################################################
--######   FloraRaster__TaxRef_CommonNames   ##########################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_CommonNames" AS 
 SELECT "TaxonCommonName"."NameID",
    min("TaxonCommonName"."CommonName"::text)::character varying(50) AS "DeutscherName"
   FROM "TaxonCommonName"
  WHERE "TaxonCommonName"."LanguageCode"::text = 'de'::text AND "TaxonCommonName"."CountryCode"::text = 'DE'::text
  GROUP BY "TaxonCommonName"."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_CommonNames"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_CommonNames" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_CommonNames" TO "CacheUser";



--#####################################################################################################################
--######   FloraRaster_TaxRef    ######################################################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."FloraRaster_TaxRef" AS 
 SELECT t."NameID" AS taxnr,
    t."AcceptedNameID" AS sipnr,
    t."NameParentID" AS aggnr,
    t."TaxonomicRank" AS rang,
    a."AnalyseStatusFuerBayern",
    t."AcceptedName" AS "SippennameMitAutoren",
    t."TaxonNameSinAuthor" AS "SippennameOhneAutoren",
    c."DeutscherName"
   FROM "#project#"."FloraRaster__TaxRef_CommonNames" c
     RIGHT JOIN "TaxonSynonymy" t ON c."NameID" = t."NameID"
     LEFT JOIN "#project#"."FloraRaster__TaxRef_Analysis" a ON a."NameID" = t."NameID";

ALTER TABLE "#project#"."FloraRaster_TaxRef"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_TaxRef" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_TaxRef" TO "CacheUser";




--#####################################################################################################################
--######   version   ######################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 3 WHERE "Package" = 'FloraRaster'
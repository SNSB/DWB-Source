-- FloraRaster

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package FloraRaster to version 2
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   FloraRaster_EndangeredSpeciesBase   ########################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_EndangeredSpeciesBase" AS 
SELECT "NameID" FROM "TaxonSynonymy" T WHERE "NameID" IN
(1034, 10377, 10395, 10815, 10828, 10830, 1112, 11167, 11650, 11652, 11654, 11720, 1190, 1198, 12145, 12155, 12156, 12197, 12209, 12262, 12268, 12269, 1232, 1245, 12660, 13307, 13746, 1402, 14428, 
14501, 14712, 1537, 1538, 1546, 17, 1770, 1830, 1851, 1872, 1905, 1934, 1973, 1974, 1975, 20012, 20016, 20017, 20018, 20019, 20020, 20025, 20056, 2025, 2026, 20318, 2034, 20433, 20581, 20589, 20654, 
20664, 20807, 21288, 21323, 2138, 2141, 21460, 21472, 21494, 21496, 21519, 21552, 21553, 21554, 21557, 21898, 2191, 22025, 22491, 22571, 2274, 22775, 22846, 22847, 22848, 22852, 22856, 22863, 22882, 
22891, 22894, 22900, 2292, 22931, 22934, 22935, 2310, 2312, 23441, 23908, 23911, 23919, 23942, 23953, 23954, 23955, 24126, 24203, 24489, 24492, 2455, 24585, 24939, 24990, 25136, 25176, 25178, 25406, 
25435, 25479, 25508, 25825, 26144, 26145, 26241, 26259, 26269, 26275, 26457, 2654, 26549, 26627, 26628, 26632, 26993, 27010, 27011, 27231, 27232, 27311, 2738, 274, 2758, 27773, 2786, 27867, 28144, 
29013, 29589, 29652, 29653, 29674, 29710, 29720, 29722, 29723, 29725, 29730, 29753, 29770, 29772, 29776, 29787, 29872, 29897, 29951, 29973, 29997, 30393, 3071, 30880, 310, 3103, 3160, 32133, 32145, 
32177, 32198, 32259, 32376, 32570, 33643, 33657, 33665, 33676, 33703, 33704, 34040, 34051, 34068, 34093, 3444, 3447, 3454, 3455, 3458, 3459, 3463, 3472, 35128, 3525, 35338, 35389, 35401, 35565, 
35624, 35643, 35644, 35697, 35715, 35716, 35746, 35762, 35785, 35786, 35791, 35867, 3595, 36258, 3697, 3734, 3790, 3808, 3858, 3950, 3955, 3960, 3962, 3963, 3967, 3968, 3983, 3984, 4019, 4020, 4021, 4022,
4024, 4028, 4030, 4038, 4040, 4041, 4042, 4050, 4087, 4170, 418, 425, 4447, 4601, 4627, 4651, 4655, 4664, 4834, 50013, 50014, 5157, 5180, 5181, 5182, 5201, 5289, 5304, 5336, 5392, 5408, 5444, 5445, 
5485, 5486, 5489, 5492, 5714, 5746, 5747, 5789, 579, 5793, 5794, 5812, 585, 5888, 5904, 5906, 5946, 60654, 6073, 60824, 61130, 61209, 6124, 6131, 61475, 61484, 61487, 6149, 61491, 6150, 6151, 6153,
6214, 6242, 6386, 6388, 6394, 6431, 6432, 6433, 6434, 663, 665, 666, 671, 6725, 677, 6800, 7004, 7008, 7096, 71, 7139, 7169, 7170, 7195, 78, 828, 857, 858, 860, 987, 1824);


ALTER TABLE "#project#"."FloraRaster_EndangeredSpeciesBase"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_EndangeredSpeciesBase" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_EndangeredSpeciesBase" TO "CacheUser";





--#####################################################################################################################
--######   FloraRaster_Kopfdaten   ####################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_Kopfdaten" AS 
 SELECT e."CollectionEventID" AS "ID_Kopfdaten",
    (l."Location1"::text || ''::text) || l."Location2"::text AS rasterfeld,
        CASE
            WHEN e."CollectionYear" IS NULL THEN e."CollectionEndYear"
            ELSE e."CollectionYear"
        END AS jahr_von,
        CASE
            WHEN e."CollectionEndYear" IS NULL THEN e."CollectionYear"
            ELSE e."CollectionEndYear"
        END AS jahr_bis,
        CASE
            WHEN l."LocalisationSystemID" = 3 THEN l."LocationAccuracy"
            ELSE NULL::character varying
        END AS unschaerfe,
    e."LocalityDescription",
    e."HabitatDescription"
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 3 AND l."Location2"::text <> ''::text;

ALTER TABLE "#project#"."FloraRaster_Kopfdaten"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_Kopfdaten" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_Kopfdaten" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_Kopfdaten" TO public;
COMMENT ON VIEW "#project#"."FloraRaster_Kopfdaten"
  IS 'Kopfdaten im Floraraster, entsprechen den Fundpunkten';
COMMENT ON COLUMN "#project#"."FloraRaster_Kopfdaten"."ID_Kopfdaten" IS 'Eindeutige ID des Datensatzes, Primärschlüssel';
COMMENT ON COLUMN "#project#"."FloraRaster_Kopfdaten".rasterfeld IS 'Zahl mit 4 Stellen für TK 25 + n Stellen für Quadrant';
COMMENT ON COLUMN "#project#"."FloraRaster_Kopfdaten".jahr_von IS 'Jahr der ersten Erfassung';
COMMENT ON COLUMN "#project#"."FloraRaster_Kopfdaten".jahr_bis IS 'Jahr der letzten Erfassung';


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

ALTER TABLE "#project#"."FloraRaster__status"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__status" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__status" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__status" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__status" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster_Sippendaten   #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_Sippendaten" AS 
 SELECT s."CollectionEventID" AS "ID_Kopfdaten",
    i."NameID" AS taxnr,
    a.flor_status_orig,
    a.flor_status_korrigiert,
    u."IdentificationUnitID"
   FROM "#project#"."FloraRaster__status" a
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON a."IdentificationUnitID" = u."IdentificationUnitID",
    "#project#"."CacheCollectionSpecimen" s,
    "#project#"."CacheIdentification" i
  WHERE u."CollectionSpecimenID" = s."CollectionSpecimenID" AND u."IdentificationUnitID" = i."IdentificationUnitID" AND u."LastIdentificationCache"::text = i."TaxonomicName"::text;

ALTER TABLE "#project#"."FloraRaster_Sippendaten"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_Sippendaten" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_Sippendaten" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_Sippendaten" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster_Sippendaten" TO "CacheAdmin";
COMMENT ON VIEW "#project#"."FloraRaster_Sippendaten"
  IS 'Sippendaten im Floraraster, entsprechen den gefundenen Pflanzen. Datenquelle in DC: IdentificationUnitAnalysis';
COMMENT ON COLUMN "#project#"."FloraRaster_Sippendaten"."ID_Kopfdaten" IS 'Eindeutige ID des Datensatzes, Teil von Primärschlüssel';
COMMENT ON COLUMN "#project#"."FloraRaster_Sippendaten".taxnr IS 'ID in der TaxRef = DiversityTaxonNames_Plants, Bayernflora, Teil von Primärschlüssel';
COMMENT ON COLUMN "#project#"."FloraRaster_Sippendaten".flor_status_orig IS 'Floristischer status, original';
COMMENT ON COLUMN "#project#"."FloraRaster_Sippendaten".flor_status_korrigiert IS 'Floristischer status, korrigiert';


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis   #############################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "AnalyseStatusFuerBayern"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 2
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster_TaxRef_CommonNames   ###########################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_TaxRef_CommonNames" AS 
 SELECT "TaxonCommonName"."NameID",
    min("TaxonCommonName"."CommonName"::text)::character varying(50) AS "DeutscherName"
   FROM "TaxonCommonName"
  WHERE "TaxonCommonName"."LanguageCode"::text = 'de'::text AND "TaxonCommonName"."CountryCode"::text = 'DE'::text
  GROUP BY "TaxonCommonName"."NameID";

ALTER TABLE "#project#"."FloraRaster_TaxRef_CommonNames"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_TaxRef_CommonNames" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_TaxRef_CommonNames" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_TaxRef_CommonNames" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster_TaxRef_CommonNames" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_EndemismusFuerBayern   ########################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "EndemismusFuerBayern"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 3
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_EndemismusFuerBayern" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_RoteListeBayern2003   #########################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "RoteListeBayern2003"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 1
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2003" TO "CacheAdmin";

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
--######   FloraRaster__TaxRef_Analysis_RoteListeBayern2024_Alpen   ###################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_Alpen" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "RoteListeBayern2024_Alpen"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 75
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_Alpen"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_Alpen" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_Alpen" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_Alpen" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_Alpen" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_RoteListeBayern2024_HB   ######################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_HB" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "RoteListeBayern2024_HB"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 78
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_HB"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_HB" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_HB" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_HB" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeBayern2024_HB" TO "CacheAdmin";



--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996    ###################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "RoteListeDeutschland1996"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 6
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_RoteListeDeutschland1996" TO "CacheAdmin";



--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern       ###################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "SchutzsstatusInBayern"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 7
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_SchutzsstatusInBayern" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_Schutzverordnung   ############################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "Schutzverordnung"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 9
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_Schutzverordnung" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_VerantwortungBayerns   ########################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "VerantwortungBayerns"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 5
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungBayerns" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands   ###################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" AS 
 SELECT a."NameID",
    min(a."AnalysisValue")::character varying(50) AS "VerantwortungDeutschlands"
   FROM "TaxonAnalysis" a
  WHERE a."ProjectID" = 1129 AND a."AnalysisID" = 4
  GROUP BY a."NameID";

ALTER TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster__TaxRef_Analysis_VerantwortungDeutschlands" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster_TaxRef    ######################################################################################
--#####################################################################################################################


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
   FROM "#project#"."FloraRaster_TaxRef_CommonNames" c
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
GRANT SELECT ON TABLE "#project#"."FloraRaster_TaxRef" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster_TaxRef" TO "CacheAdmin";

--#####################################################################################################################
--######   FloraRaster_Fotos   ########################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_Fotos" AS 
SELECT si."URI" AS "Link",
    t."AcceptedNameID" AS "SipNr",
    si."CreatorAgent"::character varying(255) AS "Beobachter",
    concat(e."CollectionDay",
        CASE
            WHEN e."CollectionDay" IS NULL THEN ''::text
            ELSE '.'::text
        END, e."CollectionMonth",
        CASE
            WHEN e."CollectionMonth" IS NULL THEN ''::text
            ELSE '.'::text
        END, e."CollectionYear") AS "Datum",
		CASE WHEN  ES."NameID" IS NULL THEN
        CASE
            WHEN "position"(e."LocalityDescription", 'Bayern, '::text) = 0 THEN e."LocalityDescription"
            ELSE "substring"(e."LocalityDescription", 9)
        END 
		ELSE '-'
		END
		AS "Locality",
    e."HabitatDescription" AS "Habitat"
   FROM "#project#"."CacheCollectionSpecimenImage" si 
   INNER JOIN "#project#"."CacheIdentification" i ON si."CollectionSpecimenID" = i."CollectionSpecimenID" AND si."IdentificationUnitID" = i."IdentificationUnitID"
   INNER JOIN "#project#"."CacheIdentificationUnit" u ON i."CollectionSpecimenID" = u."CollectionSpecimenID" AND i."IdentificationUnitID" = u."IdentificationUnitID" AND u."LastIdentificationCache"::text = i."TaxonomicName"::text
   INNER JOIN "#project#"."CacheCollectionSpecimen" s ON si."CollectionSpecimenID" = s."CollectionSpecimenID"
   INNER JOIN "#project#"."CacheCollectionEvent" e ON e."CollectionEventID" = s."CollectionEventID"
   INNER JOIN  "TaxonSynonymy" t ON i."NameURI"::text = concat(t."BaseURL", t."NameID")
   LEFT OUTER JOIN "#project#"."FloraRaster_EndangeredSpeciesBase" ES ON t."NameID" = ES."NameID"
  WHERE NOT (t."NameID" IN ( SELECT "TaxonAnalysis"."NameID"
           FROM "TaxonAnalysis"
          WHERE "TaxonAnalysis"."AnalysisID" = 2 AND "TaxonAnalysis"."AnalysisValue" = '-'::text));

ALTER TABLE "#project#"."FloraRaster_Fotos"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_Fotos" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_Fotos" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_Fotos" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster_Fotos" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster_KartenRasterPunkte   ###########################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_KartenRasterPunkte" AS 
 SELECT i."NameID",
    f.flor_status_orig,
    f.flor_status_korrigiert,
    (l."Location1"::text || ''::text) || l."Location2"::text AS rasterfeld,
        CASE
            WHEN e."CollectionYear" IS NULL THEN e."CollectionEndYear"
            ELSE e."CollectionYear"
        END AS jahr_von,
        CASE
            WHEN e."CollectionEndYear" IS NULL THEN e."CollectionYear"
            ELSE e."CollectionEndYear"
        END AS jahr_bis,
    'Q' AS rtyp,
        CASE
            WHEN l."LocalisationSystemID" = 3 THEN l."LocationAccuracy"
            ELSE NULL::character varying
        END AS unschaerfe,
    i."NameID" AS taxnr
   FROM "#project#"."FloraRaster__status" f
     RIGHT JOIN "#project#"."CacheIdentification" i ON f."CollectionSpecimenID" = i."CollectionSpecimenID" AND f."IdentificationUnitID" = i."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON i."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEvent" e ON s."CollectionEventID" = e."CollectionEventID"
     JOIN "#project#"."CacheCollectionEventLocalisation" l ON e."CollectionEventID" = l."CollectionEventID" AND l."Location2"::text <> ''::text AND l."LocalisationSystemID" = 3
  WHERE (EXISTS ( SELECT i2."IdentificationUnitID"
           FROM "#project#"."CacheIdentification" i2
          WHERE i."IdentificationUnitID" = i2."IdentificationUnitID" AND i."CollectionSpecimenID" = i2."CollectionSpecimenID"
          GROUP BY i2."IdentificationUnitID"
         HAVING i."IdentificationSequence" = max(i2."IdentificationSequence")));

ALTER TABLE "#project#"."FloraRaster_KartenRasterPunkte"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_KartenRasterPunkte" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_KartenRasterPunkte" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_KartenRasterPunkte" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster_KartenRasterPunkte" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster_TaxRefAnalysis as alternative to FloraRaster_TaxRef   ##########################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_TaxRefAnalysis" AS 
 SELECT t."NameID" AS taxnr,
    t."AcceptedNameID" AS sipnr,
    t."NameParentID" AS aggnr,
    t."TaxonomicRank" AS rang,
    t."TaxonName" AS "SippennameMitAutoren",
    t."TaxonNameSinAuthor" AS "SippennameOhneAutoren",
    a."AnalysisValue",
    v."Description" AS "AnalysisDescription",
    a."Notes",
    c."DisplayText" AS "Analysis",
    c."Description",
    p."DisplayText" AS "ParentAnalysis",
    a."AnalysisID",
    c."AnalysisParentID"
   FROM "TaxonSynonymy" t
     JOIN "TaxonAnalysis" a ON a."NameID" = t."NameID"
     JOIN "TaxonAnalysisCategory" c ON a."AnalysisID" = c."AnalysisID"
     LEFT JOIN "TaxonAnalysisCategory" p ON p."AnalysisID" = c."AnalysisParentID"
     LEFT JOIN "TaxonAnalysisCategoryValue" v ON a."AnalysisID" = v."AnalysisID" AND a."AnalysisValue" = v."AnalysisValue"::text;

ALTER TABLE "#project#"."FloraRaster_TaxRefAnalysis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_TaxRefAnalysis" TO postgres;
GRANT SELECT ON TABLE "#project#"."FloraRaster_TaxRefAnalysis" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_TaxRefAnalysis" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster_TaxRefAnalysis" TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster_ReferenceRelator   #############################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_ReferenceRelator" AS 
 SELECT DISTINCT r1."RefID",
    concat(q1.leadinga, q2.amp, q3.trailinga) AS authors
   FROM "ReferenceRelator" r1
     LEFT JOIN LATERAL ( SELECT max(s1."Sequence") AS ms
           FROM "ReferenceRelator" s1
          WHERE s1."RefID" = r1."RefID") q0 ON true
     LEFT JOIN LATERAL ( SELECT array_to_string(ARRAY( SELECT a1."Name"
                   FROM "ReferenceRelator" a1
                  WHERE a1."RefID" = r1."RefID" AND a1."Sequence" < q0.ms
                  ORDER BY a1."Sequence"), ', '::text) AS leadinga) q1 ON true
     LEFT JOIN LATERAL ( SELECT DISTINCT ' & '::text AS amp
           FROM "ReferenceRelator" a2
          WHERE a2."RefID" = r1."RefID" AND a2."Sequence" < q0.ms) q2 ON true
     LEFT JOIN LATERAL ( SELECT a3."Name" AS trailinga
           FROM "ReferenceRelator" a3
          WHERE a3."RefID" = r1."RefID" AND a3."Sequence" = q0.ms) q3 ON true;

ALTER TABLE "#project#"."FloraRaster_ReferenceRelator"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_ReferenceRelator" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_ReferenceRelator" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_ReferenceRelator" TO public;
GRANT ALL ON TABLE "#project#"."FloraRaster_ReferenceRelator" TO "CacheAdmin";

--#####################################################################################################################
--######   idxCacheIdentificationNameID   #############################################################################
--#####################################################################################################################

DO LANGUAGE plpgsql
$$
BEGIN
if (select count(*)
from pg_class t, pg_class i, pg_index ix, pg_attribute a, pg_catalog.pg_namespace n 
where
    t.oid = ix.indrelid
    and i.oid = ix.indexrelid
    and a.attrelid = t.oid
    and a.attnum = ANY(ix.indkey)
    and t.relkind = 'r'
    and t.relname = 'CacheIdentification'
    and a.attname = 'NameID'
    and i.relname = 'idxCacheIdentificationNameID'
    and n.nspname = '#project#'
    and n.oid = t.relnamespace) = 0
then
  CREATE INDEX "idxCacheIdentificationNameID"
  ON "#project#"."CacheIdentification"
  USING btree
  ("NameID");
	ALTER TABLE "#project#"."CacheIdentification" CLUSTER ON
	"idxCacheIdentificationNameID";
  COMMENT ON INDEX "#project#"."idxCacheIdentificationNameID"
  IS 'NameID (Taxnr) is used to pre-filter this list in the webfrontend.';
end if;
exception when others then
    raise notice '% %', SQLERRM, '
    The idxCacheIdentificationNameID for table CacheIdentification could not be created.';--SQLSTATE;

END;
$$;

--#####################################################################################################################
--######   IdxCacheIdentificationTaxonomicName   ######################################################################
--#####################################################################################################################

DO LANGUAGE plpgsql
$$
BEGIN
if (select count(*)
from pg_class t, pg_class i, pg_index ix, pg_attribute a, pg_catalog.pg_namespace n 
where
    t.oid = ix.indrelid
    and i.oid = ix.indexrelid
    and a.attrelid = t.oid
    and a.attnum = ANY(ix.indkey)
    and t.relkind = 'r'
    and t.relname = 'CacheIdentification'
    and a.attname = 'TaxonomicName'
    and i.relname = 'IdxCacheIdentificationTaxonomicName'
    and n.nspname = '#project#'
    and n.oid = t.relnamespace) = 0
then
  CREATE INDEX "IdxCacheIdentificationTaxonomicName"
    ON "#project#"."CacheIdentification" USING btree
    ("IdentificationUnitID", "TaxonomicName" COLLATE pg_catalog."default")
    TABLESPACE pg_default;
  COMMENT ON INDEX "#project#"."IdxCacheIdentificationTaxonomicName"
  IS 'TaxonomicName is used to pre-filter this list in the webfrontend.';
end if;
exception when others then
    raise notice '% %', SQLERRM, '
    The IdxCacheIdentificationTaxonomicName for table CacheIdentification could not be created.';--SQLSTATE;

END;
$$;

--#####################################################################################################################
--######   IdxCacheIdentificationUnitIdentificationUnitID   ###########################################################
--#####################################################################################################################

DO LANGUAGE plpgsql
$$
BEGIN
if (select count(*)
from pg_class t, pg_class i, pg_index ix, pg_attribute a, pg_catalog.pg_namespace n 
where
    t.oid = ix.indrelid
    and i.oid = ix.indexrelid
    and a.attrelid = t.oid
    and a.attnum = ANY(ix.indkey)
    and t.relkind = 'r'
    and t.relname = 'CacheIdentificationUnit'
    and a.attname = 'IdentificationUnitID'
    and i.relname = 'IdxCacheIdentificationUnitIdentificationUnitID'
    and n.nspname = '#project#'
    and n.oid = t.relnamespace) = 0
then
  CREATE INDEX "IdxCacheIdentificationUnitIdentificationUnitID"
    ON "#project#"."CacheIdentificationUnit" USING btree
    ("IdentificationUnitID")
    TABLESPACE pg_default;
end if;
exception when others then
    raise notice '% %', SQLERRM, '
    The IdxCacheIdentificationUnitIdentificationUnitID for table CacheIdentificationUnit could not be created.';--SQLSTATE;

END;
$$;

--#####################################################################################################################
--######   IdxCacheIdentificationUnitLastIdentificationCache   ########################################################
--#####################################################################################################################

DO LANGUAGE plpgsql
$$
BEGIN
if (select count(*)
from pg_class t, pg_class i, pg_index ix, pg_attribute a, pg_catalog.pg_namespace n 
where
    t.oid = ix.indrelid
    and i.oid = ix.indexrelid
    and a.attrelid = t.oid
    and a.attnum = ANY(ix.indkey)
    and t.relkind = 'r'
    and t.relname = 'CacheIdentificationUnit'
    and a.attname = 'LastIdentificationCache'
    and i.relname = 'IdxCacheIdentificationUnitLastIdentificationCache'
    and n.nspname = '#project#'
    and n.oid = t.relnamespace) = 0
then
  CREATE INDEX "IdxCacheIdentificationUnitLastIdentificationCache"
    ON "#project#"."CacheIdentificationUnit" USING btree
    ("IdentificationUnitID", "LastIdentificationCache" COLLATE pg_catalog."default")
    TABLESPACE pg_default;
end if;
exception when others then
    raise notice '% %', SQLERRM, '
    The IdxCacheIdentificationUnitLastIdentificationCache for table CacheIdentificationUnit could not be created.';--SQLSTATE;

END;
$$;


--#####################################################################################################################
--######   CacheIdentificationUnitAnalysis_T_AnalysisID_AnalysisNumber_idx   ##########################################
--#####################################################################################################################

DO LANGUAGE plpgsql
$$
BEGIN
if (select count(*)
from pg_class t, pg_class i, pg_index ix, pg_attribute a, pg_catalog.pg_namespace n 
where
    t.oid = ix.indrelid
    and i.oid = ix.indexrelid
    and a.attrelid = t.oid
    and a.attnum = ANY(ix.indkey)
    and t.relkind = 'r'
    and t.relname = 'CacheIdentificationUnitAnalysis'
    and a.attname = 'AnalysisNumber'
    and i.relname = 'CacheIdentificationUnitAnalysis_T_AnalysisID_AnalysisNumber_idx'
    and n.nspname = '#project#'
    and n.oid = t.relnamespace) = 0
then
  CREATE INDEX "CacheIdentificationUnitAnalysis_T_AnalysisID_AnalysisNumber_idx"
    ON "#project#"."CacheIdentificationUnitAnalysis" USING btree
    ("AnalysisID", "AnalysisNumber" COLLATE pg_catalog."default")
    TABLESPACE pg_default;
end if;
exception when others then
    raise notice '% %', SQLERRM, '
    The CacheIdentificationUnitAnalysis_T_AnalysisID_AnalysisNumber_idx for table CacheIdentificationUnitAnalysis could not be created.';--SQLSTATE;

END;
$$;

--#####################################################################################################################
--######   CacheIdentificationUnitAnalysis_Temp_IdentificationUnitID_idx   ############################################
--#####################################################################################################################

DO LANGUAGE plpgsql
$$
BEGIN
if (select count(*)
from pg_class t, pg_class i, pg_index ix, pg_attribute a, pg_catalog.pg_namespace n 
where
    t.oid = ix.indrelid
    and i.oid = ix.indexrelid
    and a.attrelid = t.oid
    and a.attnum = ANY(ix.indkey)
    and t.relkind = 'r'
    and t.relname = 'CacheIdentificationUnitAnalysis'
    and a.attname = 'IdentificationUnitID'
    and i.relname = 'CacheIdentificationUnitAnalysis_Temp_IdentificationUnitID_idx'
    and n.nspname = '#project#'
    and n.oid = t.relnamespace) = 0
then
  CREATE INDEX "CacheIdentificationUnitAnalysis_Temp_IdentificationUnitID_idx"
    ON "#project#"."CacheIdentificationUnitAnalysis" USING btree
    ("IdentificationUnitID")
    TABLESPACE pg_default;
end if;
exception when others then
    raise notice '% %', SQLERRM, '
    The CacheIdentificationUnitAnalysis_Temp_IdentificationUnitID_idx for table CacheIdentificationUnitAnalysis could not be created.';--SQLSTATE;

END;
$$;

--#####################################################################################################################
--######   New objects for distribution maps   ########################################################################
--#####################################################################################################################
--#####################################################################################################################
--######   FloraRaster_sum_map_definition   ###########################################################################
--#####################################################################################################################
/*
CREATE TABLE IF NOT EXISTS "#project#".FloraRaster_sum_map_definition
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    year_from integer,
    year_to integer,
    definition jsonb,
    started_at timestamp with time zone,
    finished_at timestamp with time zone,
    build_status text,
    CONSTRAINT FloraRaster_sum_map_definition_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE "#project#".FloraRaster_sum_map_definition
    OWNER to "CacheAdmin";

GRANT ALL ON TABLE "#project#".FloraRaster_sum_map_definition TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#".FloraRaster_sum_map_definition TO "CacheUser";
GRANT ALL ON TABLE "#project#".FloraRaster_sum_map_definition TO postgres;
*/
--#####################################################################################################################
--######   FloraRaster_quadrant_sum   #################################################################################
--#####################################################################################################################
/*
CREATE TABLE IF NOT EXISTS "#project#".FloraRaster_quadrant_sum
(
    sum_map_definition_id integer NOT NULL,
    mtb character varying COLLATE pg_catalog."default" NOT NULL,
    quadrant character varying COLLATE pg_catalog."default" NOT NULL,
    result_value jsonb,
    CONSTRAINT FloraRaster_quadrant_sum_pkey PRIMARY KEY (sum_map_definition_id, mtb, quadrant),
    CONSTRAINT sum_map_definition_id_fk FOREIGN KEY (sum_map_definition_id)
        REFERENCES "#project#".FloraRaster_sum_map_definition (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE "#project#".FloraRaster_quadrant_sum
    OWNER to "CacheAdmin";

GRANT DELETE, UPDATE, SELECT, INSERT ON TABLE "#project#".FloraRaster_quadrant_sum TO "CacheAdmin";
*/

--#####################################################################################################################
--######   FloraRaster_CacheCollectionEventLocalisationAVGPos   #######################################################
--#####################################################################################################################
-- Obsolet - wird von Stefan übernommen
/*
CREATE TABLE "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos"
(
    "CollectionEventID" integer,
    "Location1" character varying(255) COLLATE pg_catalog."default",
    "Location2" character varying(255) COLLATE pg_catalog."default",
    quadrant text COLLATE pg_catalog."default",
    mtb_x text COLLATE pg_catalog."default",
    mtb_y text COLLATE pg_catalog."default",
    "LocationAccuracy" character varying(50) COLLATE pg_catalog."default",
    "ResponsibleName" character varying(255) COLLATE pg_catalog."default",
    "CollectionSpecimenID" integer,
    "ExternalDatasourceID" integer,
    "IdentificationUnitID" integer,
    "IdentificationSequence" smallint,
    "TaxonomicName" character varying(255) COLLATE pg_catalog."default",
    "NameID" integer,
    "Version" integer,
    "CollectionDate" timestamp without time zone,
    "CollectionYear" smallint,
    "CollectionDateSupplement" character varying(100) COLLATE pg_catalog."default",
    "LocalityDescription" text COLLATE pg_catalog."default",
    "HabitatDescription" text COLLATE pg_catalog."default",
    "LocalityVerbatim" text COLLATE pg_catalog."default",
    "CountryCache" character varying(50) COLLATE pg_catalog."default",
    "CollectionEndYear" smallint,
    "CollectionMeanYear" integer,
    "AnalysisResult" text COLLATE pg_catalog."default",
    "AcceptedNameID" integer,
    "NameParentID" integer,
    "TaxonomicRank" character varying(50) COLLATE pg_catalog."default",
    "AnalyseStatusFuerBayern" character varying(50) COLLATE pg_catalog."default",
    "geom" character varying(500) COLLATE pg_catalog."default"
);

ALTER TABLE "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos" TO public;
*/

--#####################################################################################################################
--######   floraRaster__CacheCollectionEventLocalisationAVGPos   ######################################################
--#####################################################################################################################
-- Obsolet - wird von Stefan übernommen
/*
CREATE OR REPLACE FUNCTION "#project#".floraraster__cachecollectioneventlocalisationavgpos(
	)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

DROP INDEX IF EXISTS "floraraster_idx_nameid";

TRUNCATE TABLE "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos";

-- getting the IdentificationSequence
CREATE TEMP TABLE TEMP_FloraRaster__IdentificationSequence AS 
SELECT i2."CollectionSpecimenID", i2."IdentificationUnitID", max(i2."IdentificationSequence")  AS "IdentificationSequence"        
FROM "#project#"."CacheIdentification" i2
WHERE i2."NameID" is not null
GROUP BY i2."CollectionSpecimenID", i2."IdentificationUnitID";

-- getting the Identification
CREATE TEMP TABLE TEMP_FloraRaster__Identification AS 
SELECT i2."CollectionSpecimenID", i2."IdentificationUnitID", i2."IdentificationSequence", i2."NameID", i2."TaxonomicName"  
FROM "#project#"."CacheIdentification" i2 inner join TEMP_FloraRaster__IdentificationSequence AS i 
on  i2."CollectionSpecimenID" = i."CollectionSpecimenID"
and i2."IdentificationUnitID" = i."IdentificationUnitID"
and i2."IdentificationSequence" = i."IdentificationSequence";

-- getting the AnalysisNumber
CREATE TEMP TABLE TEMP_FloraRaster__AnalysisNumber AS 
SELECT l."CollectionSpecimenID", l."IdentificationUnitID", l."AnalysisID", max(l."AnalysisNumber"::text) AS "AnalysisNumber"
           FROM "#project#"."CacheIdentificationUnitAnalysis" l
		   WHERE l."AnalysisID" = 2
          GROUP BY l."CollectionSpecimenID", l."IdentificationUnitID", l."AnalysisID";
		  
-- remove temp tables
DROP TABLE IF EXISTS TEMP_FloraRaster__Analysis;
		  
-- getting the Analysis
CREATE TEMP TABLE TEMP_FloraRaster__Analysis AS 
SELECT l."CollectionSpecimenID", l."IdentificationUnitID", l."AnalysisID", l."AnalysisNumber", l."AnalysisResult"
           FROM "#project#"."CacheIdentificationUnitAnalysis" l INNER JOIN TEMP_FloraRaster__AnalysisNumber T 
		   ON l."CollectionSpecimenID" = T."CollectionSpecimenID"
		   AND T."IdentificationUnitID" = l."IdentificationUnitID"
		   AND T."AnalysisID" = l."AnalysisID"
		   AND T."AnalysisNumber" = l."AnalysisNumber";


INSERT INTO "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos"(
	"CollectionEventID", "Location1", "Location2", quadrant, mtb_x, mtb_y, "LocationAccuracy", "ResponsibleName", "CollectionSpecimenID", "ExternalDatasourceID", "IdentificationUnitID", "IdentificationSequence", 
	"TaxonomicName", "NameID", "Version", "CollectionDate", "CollectionYear", "CollectionDateSupplement", "LocalityDescription", "HabitatDescription", "LocalityVerbatim", "CountryCache", "CollectionEndYear", 
	"CollectionMeanYear", "AnalysisResult", "AcceptedNameID", "NameParentID", "TaxonomicRank", "AnalyseStatusFuerBayern", "geom")
SELECT cel."CollectionEventID", cel."Location1", cel."Location2",
cel."Location1" || substr(cel."Location2",1,1) as quadrant,
substr(cel."Location1",3,2) as mtb_x,
substr(cel."Location1",1,2) as mtb_y,
cel."LocationAccuracy", cel."ResponsibleName",
     cs."CollectionSpecimenID", cs."ExternalDatasourceID",
	 iu."IdentificationUnitID",
iu."IdentificationSequence", iu."TaxonomicName", iu."NameID",
	ce."Version", ce."CollectionDate" ,ce."CollectionYear",
ce."CollectionDateSupplement", ce."LocalityDescription",
ce."HabitatDescription", ce."LocalityVerbatim", ce."CountryCache",
ce."CollectionEndYear",
case when ce."CollectionYear" is null then ce."CollectionEndYear" else
                case when ce."CollectionEndYear" is null then ce."CollectionYear" else
                cast((ce."CollectionEndYear" + ce."CollectionYear") / 2 as int) end end as "CollectionMeanYear",
iua."AnalysisResult",
tr.sipnr AS "AcceptedNameID", tr.aggnr AS "NameParentID", tr.rang AS "TaxonomicRank",
ta."AnalyseStatusFuerBayern"
    ,''
	FROM "#project#"."CacheCollectionEventLocalisation" cel
	inner join "#project#"."CacheCollectionSpecimen" cs on
cel."CollectionEventID" = cs."CollectionEventID"
	inner join "#project#"."CacheCollectionEvent" ce on cel."CollectionEventID" = cs."CollectionEventID" and
cel."CollectionEventID" = ce."CollectionEventID"
	inner join TEMP_FloraRaster__Identification iu on
        iu."CollectionSpecimenID" = cs."CollectionSpecimenID"
	left join TEMP_FloraRaster__Analysis iua on
	iua."CollectionSpecimenID" = iu."CollectionSpecimenID" and
	iua."IdentificationUnitID" = iu."IdentificationUnitID" 
	left join "#project#"."FloraRaster__TaxRef_Analysis" ta on ta."NameID" = iu."NameID"
	left join "#project#"."FloraRaster_TaxRef" tr on tr."taxnr" = iu."NameID"
    where "LocalisationSystemID" = 3
;


-- remove temp tables
DROP TABLE IF EXISTS TEMP_FloraRaster__IdentificationSequence;
DROP TABLE IF EXISTS TEMP_FloraRaster__Identification;
DROP TABLE IF EXISTS TEMP_FloraRaster__AnalysisNumber;
DROP TABLE IF EXISTS TEMP_FloraRaster__Analysis;


ALTER TABLE "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos"
	ADD CONSTRAINT cachecollectioneventlocalisationavgpos_optimized_pk
		PRIMARY KEY ("CollectionEventID", "CollectionSpecimenID", "IdentificationUnitID");

CREATE INDEX "floraraster_idx_nameid" ON "#project#"."FloraRaster_CacheCollectionEventLocalisationAVGPos" USING btree ("NameID");

end;
$BODY$;

ALTER FUNCTION "#project#".floraraster__cachecollectioneventlocalisationavgpos() OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".floraraster__cachecollectioneventlocalisationavgpos() TO "CacheAdmin";

--GRANT EXECUTE ON FUNCTION "#project#".floraraster__cachecollectioneventlocalisationavgpos() TO "CacheUser";

--GRANT EXECUTE ON FUNCTION "#project#".floraraster__cachecollectioneventlocalisationavgpos() TO PUBLIC;
*/

--#####################################################################################################################
--######   FloraRaster_KartenRasterPunkte_tbl   #######################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."FloraRaster_KartenRasterPunkte_tbl"
(
    event_id bigint,
    name_id integer,
    status text COLLATE pg_catalog."default",
    mtb text COLLATE pg_catalog."default",
    quadrant text COLLATE pg_catalog."default",
    q4 text COLLATE pg_catalog."default",
    q16 text COLLATE pg_catalog."default",
    rasterfeld text COLLATE pg_catalog."default",
    jahr_von smallint,
    jahr_bis smallint,
    unschaerfe character varying COLLATE pg_catalog."default",
    jahr_mean smallint
);

ALTER TABLE "#project#"."FloraRaster_KartenRasterPunkte_tbl"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_KartenRasterPunkte_tbl" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_KartenRasterPunkte_tbl" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_KartenRasterPunkte_tbl" TO public;




--#####################################################################################################################
--######   floraRaster__KartenRasterPunkte_tbl   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".floraraster__KartenRasterPunkte_tbl(
	)
    RETURNS void
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

DROP INDEX IF EXISTS "#project#"."FloraRasterPunkte_mtb_quadrant_idx";

DROP INDEX IF EXISTS "#project#"."FloraRasterPunkte_name_id_idx";

ALTER TABLE "#project#"."FloraRaster_KartenRasterPunkte_tbl" OWNER TO "CacheAdmin";

TRUNCATE TABLE "#project#"."FloraRaster_KartenRasterPunkte_tbl";


INSERT INTO "#project#"."FloraRaster_KartenRasterPunkte_tbl"(
	event_id, name_id, status, mtb, quadrant, q4, q16, rasterfeld, jahr_von, jahr_bis, unschaerfe, jahr_mean)
SELECT ROW_NUMBER() OVER (ORDER BY 1) as event_id, "NameID" as name_id, flor_status_korrigiert as status,
      substr(rasterfeld, 1, 4) as mtb,
  substr(rasterfeld, 5,1) as quadrant,
  substr(rasterfeld,6,1) as q4,
  substr(rasterfeld,7,1) as q16,
  rasterfeld,
  jahr_von, jahr_bis, unschaerfe,
  case when jahr_von is null then jahr_bis else
                case when jahr_bis is null then jahr_von else
                cast((jahr_bis + jahr_von) / 2 as smallint) end end as jahr_mean
FROM "#project#"."FloraRaster_KartenRasterPunkte" where (jahr_von<2100 or jahr_bis<2100) and (jahr_von>0 or jahr_bis>0)  and substr(rasterfeld, 5,1)>='1' and substr(rasterfeld, 5,1)<='4';


CREATE INDEX "FloraRasterPunkte_mtb_quadrant_idx"
    ON "#project#"."FloraRaster_KartenRasterPunkte_tbl" USING btree
    (mtb COLLATE pg_catalog."default", quadrant COLLATE pg_catalog."default")
    TABLESPACE pg_default;


CREATE INDEX "FloraRasterPunkte_name_id_idx"
    ON "#project#"."FloraRaster_KartenRasterPunkte_tbl" USING btree
    (name_id)
    TABLESPACE pg_default;

ALTER TABLE "#project#"."FloraRaster_KartenRasterPunkte_tbl"
    CLUSTER ON "FloraRasterPunkte_name_id_idx";
end;
$BODY$;

ALTER FUNCTION "#project#".floraraster__KartenRasterPunkte_tbl() OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".floraraster__KartenRasterPunkte_tbl() TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster_ExpandedNameId_name_id_seq   ###################################################################
--#####################################################################################################################

CREATE SEQUENCE IF NOT EXISTS "#project#"."FloraRaster_ExpandedNameId_name_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE "#project#"."FloraRaster_ExpandedNameId_name_id_seq"
    OWNER TO "CacheAdmin";


--#####################################################################################################################
--######   FloraRaster_ExpandedNameId   ###############################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."FloraRaster_ExpandedNameId"
(
    name_id integer NOT NULL DEFAULT nextval('"#project#"."FloraRaster_ExpandedNameId_name_id_seq"'::regclass),
    children integer[],
    synonyms integer[],
    CONSTRAINT "ExpandedNameId_pkey" PRIMARY KEY (name_id)
);

ALTER TABLE "#project#"."FloraRaster_ExpandedNameId"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_ExpandedNameId" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_ExpandedNameId" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_ExpandedNameId" TO public;


--#####################################################################################################################
--######   FloraRaster_sippen_basis   #################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."FloraRaster_sippen_basis"
(
    name_id integer,
    mtb text COLLATE pg_catalog."default",
    quadrant text COLLATE pg_catalog."default",
    jahr_mean smallint,
    children integer[]
);
ALTER TABLE "#project#"."FloraRaster_sippen_basis"
    OWNER to "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_sippen_basis" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_sippen_basis" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_sippen_basis" TO public;


--#####################################################################################################################
--######   floraRaster__sippen_basis   ################################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".floraraster__sippen_basis(
	)
    RETURNS void
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    
AS $BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";


DROP INDEX IF EXISTS "FloraRaster_sippen_basis_jahr_#project#";

TRUNCATE TABLE "#project#"."FloraRaster_sippen_basis";

INSERT INTO "#project#"."FloraRaster_sippen_basis"(
	name_id, mtb, quadrant, jahr_mean, children)
select b.name_id, b.mtb, b.quadrant, b.jahr_mean, a.children
from
"#project#"."FloraRaster_ExpandedNameId" a right join
(SELECT
"#project#"."FloraRaster_KartenRasterPunkte_tbl".name_id
AS name_id,
"#project#"."FloraRaster_KartenRasterPunkte_tbl".mtb AS
mtb,
"#project#"."FloraRaster_KartenRasterPunkte_tbl".quadrant AS
quadrant,
"#project#"."FloraRaster_KartenRasterPunkte_tbl".jahr_mean AS
jahr_mean
FROM "#project#"."FloraRaster_KartenRasterPunkte_tbl"
WHERE
"#project#"."FloraRaster_KartenRasterPunkte_tbl".name_id > 0
and "#project#"."FloraRaster_KartenRasterPunkte_tbl".quadrant >= '1'
and "#project#"."FloraRaster_KartenRasterPunkte_tbl".quadrant <= '4'
group by name_id, mtb, quadrant, jahr_mean) as b on a.name_id = b.name_id;


CREATE INDEX IF NOT EXISTS "FloraRaster_sippen_basis_jahr_#project#"
    ON "#project#"."FloraRaster_sippen_basis" USING btree
    (jahr_mean ASC NULLS LAST)
    TABLESPACE pg_default;
end;
$BODY$;

ALTER FUNCTION "#project#".floraraster__sippen_basis() OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".floraraster__sippen_basis() TO "CacheAdmin";







--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 2 WHERE "Package" = 'FloraRaster'
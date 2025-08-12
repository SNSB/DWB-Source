-- GUC

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package GUC to version 2
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   GUC__WGS84   ###############################################################################################
--#####################################################################################################################

--DROP VIEW "#project#"."GUC__WGS84";

CREATE OR REPLACE VIEW "#project#"."GUC__WGS84" AS 
 SELECT e."CollectionEventID",
    l."LocationAccuracy" AS erfassungsgenauigkeit,
    concat('Point (', l."AverageLatitudeCache", ' ', l."AverageLongitudeCache", ')') AS punkt_berechnet_wkt,
        CASE
            WHEN l."Geography" ~~ 'POINT (%'::text THEN l."Geography"
            ELSE NULL::text
        END AS verortung_punkt_wkt,
        CASE
            WHEN l."Geography" ~~ 'POLYGON (%'::text THEN l."Geography"
            ELSE NULL::text
        END AS verortung_flaeche_wkt
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 8 AND l."Location2"::text <> ''::text;

ALTER TABLE "#project#"."GUC__WGS84"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__WGS84" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__WGS84" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__WGS84" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__WGS84" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__TOPONYM                       #########################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__TOPONYM" AS 
 SELECT e."CollectionEventID",
    l."Location1" AS toponym
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 19 AND l."Location2"::text <> ''::text;

ALTER TABLE "#project#"."GUC__TOPONYM"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__TOPONYM" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__TOPONYM" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__TOPONYM" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__TOPONYM" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__TK                         ############################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__TK" AS 
 SELECT e."CollectionEventID",
    l."Location1" AS tk,
    l."Location2" AS quadrant,
    concat(l."AverageLatitudeCache"::character varying, ' ', l."AverageLongitudeCache"::character varying) AS punkt_berechnet_wkt
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 3 AND l."Location2"::text <> ''::text;

ALTER TABLE "#project#"."GUC__TK"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__TK" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__TK" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__TK" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__TK" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__SpecimenRelation                 ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__SpecimenRelation" AS 
 SELECT c."CollectionName",
    r."IdentificationUnitID",
    r."CollectionSpecimenID"
   FROM "#project#"."CacheCollectionSpecimenRelation" r,
    "#project#"."CacheCollection" c
  WHERE r."RelatedSpecimenCollectionID" = c."CollectionID";

ALTER TABLE "#project#"."GUC__SpecimenRelation"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__SpecimenRelation" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__SpecimenRelation" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__SpecimenRelation" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__SpecimenRelation" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__SpecimenPart     ######################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__SpecimenPart" AS 
 SELECT c."CollectionName" AS collection,
        CASE
            WHEN p."MaterialCategory"::text = 'herbarium sheets'::text THEN 'Sammlung'::text
            ELSE
            CASE
                WHEN p."MaterialCategory"::text = 'human observation'::text THEN 'Freilanderfassung'::text
                ELSE ''::text
            END
        END AS material,
    u."IdentificationUnitID"
   FROM "#project#"."CacheCollectionSpecimenPart" p,
    "#project#"."CacheCollection" c,
    "#project#"."CacheIdentificationUnitInPart" u
  WHERE p."CollectionID" = c."CollectionID" AND u."SpecimenPartID" = p."SpecimenPartID" AND u."CollectionSpecimenID" = p."CollectionSpecimenID";

ALTER TABLE "#project#"."GUC__SpecimenPart"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__SpecimenPart" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__SpecimenPart" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__SpecimenPart" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__SpecimenPart" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__NATURRAUM             #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__NATURRAUM" AS 
 SELECT e."CollectionEventID",
    l."Location1" AS naturraum,
    l."Location2" AS naturraum_code_quellsystem
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 18 AND l."Location2"::text <> ''::text;

ALTER TABLE "#project#"."GUC__NATURRAUM"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__NATURRAUM" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__NATURRAUM" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__NATURRAUM" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__NATURRAUM" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__Landkreis             #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__Landkreis" AS 
 SELECT e."CollectionEventID",
    l."Location1" AS landkreis,
    l."Location2" AS landkreis_code_quellsystem,
    l."Location2" AS landkreis_code_lfu
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 7 AND l."Location2"::text <> ''::text;

ALTER TABLE "#project#"."GUC__Landkreis"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__Landkreis" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__Landkreis" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__Landkreis" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__Landkreis" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__LEBENSRAUM            #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__LEBENSRAUM" AS 
 SELECT e."CollectionEventID",
    p."DisplayText" AS lebensraum,
    p."PropertyURI" AS lebensraum_code_lfu,
    reverse(substr(reverse(p."PropertyURI"::text), 1, strpos(reverse(p."PropertyURI"::text), '/'::text) - 1)) AS lebensraum_code_quellsystem
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventProperty" p
  WHERE e."CollectionEventID" = p."CollectionEventID" AND p."PropertyID" = 40 AND p."DisplayText"::text <> ''::text;

ALTER TABLE "#project#"."GUC__LEBENSRAUM"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__LEBENSRAUM" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__LEBENSRAUM" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__LEBENSRAUM" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__LEBENSRAUM" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__IdentificationUnitAnalysis   ##########################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__IdentificationUnitAnalysis" AS 
 SELECT a."IdentificationUnitID" AS nachweis_id_quellsystem,
        CASE
            WHEN a."AnalysisResult" = '4'::text THEN 'F'::text
            ELSE
            CASE
                WHEN a."AnalysisResult" = 'I'::text THEN 'H'::text
                ELSE
                CASE
                    WHEN a."AnalysisResult" = 'E'::text OR a."AnalysisResult" = 'F'::text OR a."AnalysisResult" = 'U'::text THEN a."AnalysisResult"
                    ELSE NULL::text
                END
            END
        END AS status_code_lfu,
        CASE
            WHEN a."AnalysisResult" = '2'::text THEN 'Angabe falsch am Ort'::text
            ELSE
            CASE
                WHEN a."AnalysisResult" = '3'::text THEN 'Angabe falsch im Rasterfeld'::text
                ELSE
                CASE
                    WHEN a."AnalysisResult" = '5'::text THEN 'Angabe zweifelhaft im Rasterfeld'::text
                    ELSE
                    CASE
                        WHEN a."AnalysisResult" = 'A'::text THEN 'angepflanzt (zur Zierde oder zur Bereicherung der Landschaft), aber ohne wirtschaftliche Nutzungsabsicht (vgl. K)'::text
                        ELSE
                        CASE
                            WHEN a."AnalysisResult" = 'X'::text THEN 'ausgestorben'::text
                            ELSE
                            CASE
                                WHEN a."AnalysisResult" = '+'::text THEN 'ausgestorben oder verschollen'::text
                                ELSE
                                CASE
                                    WHEN a."AnalysisResult" = 'E'::text THEN 'eingebürgert'::text
                                    ELSE
                                    CASE
WHEN a."AnalysisResult" = 'E?'::text THEN 'eingebürgert (Zuordnung unsicher)'::text
ELSE
CASE
 WHEN a."AnalysisResult" = 'H'::text THEN 'einheimisch'::text
 ELSE
 CASE
  WHEN a."AnalysisResult" = 'I'::text THEN 'einheimisch'::text
  ELSE
  CASE
   WHEN a."AnalysisResult" = '4'::text OR a."AnalysisResult" = 'F'::text THEN 'fraglich ob vorkommend'::text
   ELSE
   CASE
    WHEN a."AnalysisResult" = '-'::text THEN 'kommt hier nicht vor entgegen anderslautenden Angaben'::text
    ELSE
    CASE
     WHEN a."AnalysisResult" = 'K'::text THEN 'kultiviert, für land- und forstwirtschaftliche Zwecke (seltener auch gärtnerische, z.B. Baumschulen, Schnittblumen), mit wirtschaftlicher Nutzungsabsicht'::text
     ELSE
     CASE
      WHEN a."AnalysisResult" = 'R'::text THEN 'Kulturrelikt, ehemals am Wuchsort gepflanzt, aber nach Nutzungsaufgabe an der Pflanzstelle sich haltend (aber nicht ausbreitend wie bei E)'::text
      ELSE
      CASE
       WHEN a."AnalysisResult" = 'W'::text THEN 'natürlich unbeständig'::text
       ELSE
       CASE
        WHEN a."AnalysisResult" = '1'::text THEN 'Neufund, Bestätigung'::text
        ELSE
        CASE
         WHEN a."AnalysisResult" = '*'::text THEN 'Normalstatus: einheimisch, spontan wachsend, ohne direkten menschlichen Einfluss (urwüchsig oder vor 1500 durch den Menschen als Wildpflanze im Gebiet eingeschleppt)'::text
         ELSE
         CASE
          WHEN a."AnalysisResult" = '6'::text THEN 'sicher ausgestorben am Ort'::text
          ELSE
          CASE
           WHEN a."AnalysisResult" = '8'::text THEN 'sicher ausgestorben im Rasterfeld'::text
           ELSE
           CASE
            WHEN a."AnalysisResult" = '?'::text THEN 'Status unbekannt'::text
            ELSE
            CASE
             WHEN a."AnalysisResult" = 'S'::text THEN 'synanthrop im Allgemeinen, also A, E, K oder R, ansonsten keine genauere Kategorie feststellbar'::text
             ELSE
             CASE
              WHEN a."AnalysisResult" = 'U'::text THEN 'unbeständig'::text
              ELSE
              CASE
               WHEN a."AnalysisResult" = 'V'::text THEN 'verschollen (unsicher ob hier wirklich schon ausgestorben)'::text
               ELSE
               CASE
                WHEN a."AnalysisResult" = '7'::text THEN 'verschollen am Ort'::text
                ELSE
                CASE
                 WHEN a."AnalysisResult" = '9'::text THEN 'verschollen im Rasterfeld'::text
                 ELSE
                 CASE
                  WHEN a."AnalysisResult" = 'Z'::text THEN 'zweifelhaft ob einheimisch, kann (nahezu) jedes der anderen Statuskategorien sein, ist aber nicht näher feststellbar.'::text
                  ELSE ''::text
                 END
                END
               END
              END
             END
            END
           END
          END
         END
        END
       END
      END
     END
    END
   END
  END
 END
END
                                    END
                                END
                            END
                        END
                    END
                END
            END
        END AS status,
    a."AnalysisResult" AS status_code_quellsystem
   FROM "#project#"."CacheIdentificationUnitAnalysis" a
  WHERE a."AnalysisID" = 2 AND a."AnalysisNumber"::text = '1'::text;

ALTER TABLE "#project#"."GUC__IdentificationUnitAnalysis"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__IdentificationUnitAnalysis" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__IdentificationUnitAnalysis" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__IdentificationUnitAnalysis" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__IdentificationUnitAnalysis" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__Hoehe                 #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__Hoehe" AS 
 SELECT e."CollectionEventID",
    l."Location1" AS hoehe_von,
    l."Location2" AS hoehe_bis
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 4 AND l."Location1"::text <> ''::text;

ALTER TABLE "#project#"."GUC__Hoehe"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__Hoehe" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__Hoehe" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__Hoehe" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__Hoehe" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC_SAMMLER                #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC_SAMMLER" AS 
 SELECT u."IdentificationUnitID",
    ltrim(rtrim(substr(a."CollectorsName"::text, 1, strpos(a."CollectorsName"::text, ','::text) - 1))) AS sammler_nachname,
    ltrim(rtrim(substr(a."CollectorsName"::text, strpos(a."CollectorsName"::text, ','::text) + 1, 500))) AS sammler_vorname,
    a."CollectorsName"
   FROM "#project#"."CacheCollectionAgent" a,
    "#project#"."CacheIdentificationUnit" u
  WHERE u."CollectionSpecimenID" = a."CollectionSpecimenID";

ALTER TABLE "#project#"."GUC_SAMMLER"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC_SAMMLER" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC_SAMMLER" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC_SAMMLER" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC_SAMMLER" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC_NACHWEIS               #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC_NACHWEIS" AS 
 SELECT e."CollectionEventID" AS fundort_id_quellsystem,
    u."IdentificationUnitID" AS nachweis_id_quellsystem,
        CASE
            WHEN u."OnlyObserved" = '1'::bpchar THEN 'Freilanderfassung'::text
            ELSE ''::text
        END AS quellentyp,
    u."NumberOfUnits" AS anzahl,
    e."CollectionDay" AS beobachtungsdatum_tag,
    e."CollectionMonth" AS beobachtungsdatum_monat,
    e."CollectionYear" AS beobachtungsdatum_jahr,
        CASE
            WHEN i."IdentificationQualifier"::text ~~ 's. %'::text OR i."IdentificationQualifier"::text = 'agg.'::text THEN 'Standardsicherheit'::text
            ELSE
            CASE
                WHEN i."IdentificationQualifier" IS NULL OR i."IdentificationQualifier"::text = ''::text OR i."IdentificationQualifier"::text ~~ '% ssp.'::text THEN ''::text
                ELSE 'unsicher'::text
            END
        END::character varying(50) AS datenqualitaet,
    e."HabitatDescription" AS engerer_fundort,
    ltrim(rtrim(substr(i."ResponsibleName"::text, 1, strpos(i."ResponsibleName"::text, ','::text) - 1))) AS determinator_nachname,
    ltrim(rtrim(substr(i."ResponsibleName"::text, strpos(i."ResponsibleName"::text, ','::text) + 1, 500))) AS determinator_vorname,
    s."ReferenceTitle" AS literatur,
    s."LogUpdatedWhen" AS datum_letzte_aenderung,
    concat(
        CASE
            WHEN s."OriginalNotes" IS NULL OR s."OriginalNotes" = ''::text THEN ''::text
            ELSE concat(s."OriginalNotes", '; ')
        END,
        CASE
            WHEN s."AdditionalNotes" IS NULL OR s."AdditionalNotes" = ''::text THEN ''::text
            ELSE concat(s."AdditionalNotes", '; ')
        END,
        CASE
            WHEN u."Notes" IS NULL OR u."Notes" = ''::text THEN ''::text
            ELSE concat(u."Notes", '; ')
        END,
        CASE
            WHEN i."Notes" IS NULL OR i."Notes" = ''::text THEN ''::text
            ELSE concat(i."Notes", '; ')
        END) AS bemerkung,
    concat(
        CASE
            WHEN p.collection IS NULL THEN ''::character varying
            ELSE p.collection
        END, ' ',
        CASE
            WHEN r."CollectionName" IS NULL THEN ''::character varying
            ELSE r."CollectionName"
        END, ' ',
        CASE
            WHEN p.material IS NULL THEN ''::text
            ELSE p.material
        END) AS verbleib_nachname,
    a.status_code_quellsystem,
    a.status,
    i."NameURI" AS artencode
   FROM "#project#"."CacheIdentification" i,
    "#project#"."CacheCollectionSpecimen" s,
    "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheIdentificationUnit" u
     LEFT JOIN "#project#"."GUC__IdentificationUnitAnalysis" a ON a.nachweis_id_quellsystem = u."IdentificationUnitID"
     LEFT JOIN "#project#"."GUC__SpecimenPart" p ON p."IdentificationUnitID" = u."IdentificationUnitID"
     LEFT JOIN "#project#"."GUC__SpecimenRelation" r ON p."IdentificationUnitID" = u."IdentificationUnitID"
  WHERE u."CollectionSpecimenID" = s."CollectionSpecimenID" AND e."CollectionEventID" = s."CollectionEventID" AND i."IdentificationUnitID" = u."IdentificationUnitID" AND i."TaxonomicName"::text = u."LastIdentificationCache"::text;

ALTER TABLE "#project#"."GUC_NACHWEIS"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC_NACHWEIS" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC_NACHWEIS" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC_NACHWEIS" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC_NACHWEIS" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC_FUNDORT                #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC_FUNDORT" AS 
 SELECT e."CollectionEventID" AS fundort_id_quellsystem,
    e."Notes" AS bemerkung,
    s."LogUpdatedWhen" AS "datum_letzte_Änderung_dwb",
    e."LocalityDescription" AS lage,
    h.hoehe_von,
    h.hoehe_bis,
    l.landkreis,
    l.landkreis_code_quellsystem,
    l.landkreis_code_lfu,
    n.naturraum,
    n.naturraum_code_quellsystem,
    t.tk,
    t.quadrant,
    o.toponym,
    w.erfassungsgenauigkeit,
    lr.lebensraum,
    lr.lebensraum_code_lfu,
    lr.lebensraum_code_quellsystem
   FROM "#project#"."CacheCollectionSpecimen" s,
    "#project#"."CacheCollectionEvent" e
     LEFT JOIN "#project#"."GUC__Hoehe" h ON e."CollectionEventID" = h."CollectionEventID"
     LEFT JOIN "#project#"."GUC__Landkreis" l ON e."CollectionEventID" = l."CollectionEventID"
     LEFT JOIN "#project#"."GUC__NATURRAUM" n ON e."CollectionEventID" = n."CollectionEventID"
     LEFT JOIN "#project#"."GUC__TK" t ON e."CollectionEventID" = t."CollectionEventID"
     LEFT JOIN "#project#"."GUC__TOPONYM" o ON e."CollectionEventID" = o."CollectionEventID"
     LEFT JOIN "#project#"."GUC__WGS84" w ON e."CollectionEventID" = w."CollectionEventID"
     LEFT JOIN "#project#"."GUC__LEBENSRAUM" lr ON e."CollectionEventID" = lr."CollectionEventID"
  WHERE e."CollectionEventID" = s."CollectionEventID";

ALTER TABLE "#project#"."GUC_FUNDORT"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC_FUNDORT" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC_FUNDORT" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC_FUNDORT" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC_FUNDORT" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 2 WHERE "Package" = 'GUC'
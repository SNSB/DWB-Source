-- GUC

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package GUC to version 6
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################




--#####################################################################################################################
--######   GUC__TK        #############################################################################################
--#####################################################################################################################

--DROP VIEW "#project#"."GUC__TK";

CREATE OR REPLACE VIEW "#project#"."GUC__TK" AS 
 SELECT e."CollectionEventID",
    l."Location1" AS tk,
    l."Location2" AS quadrant,
    case when not l."AverageLatitudeCache" is null then concat('Point ('::character varying, l."AverageLatitudeCache"::character varying, ' ', l."AverageLongitudeCache"::character varying,')'::character varying ) 
    else NULL end
    AS punkt_berechnet_wkt
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 3 AND l."Location2"::text <> ''::text;



--#####################################################################################################################
--######   GUC_FUNDORT        #########################################################################################
--#####################################################################################################################

DROP VIEW "#project#"."GUC_FUNDORT";

CREATE OR REPLACE VIEW "#project#"."GUC_FUNDORT" AS 
 SELECT e."CollectionEventID" AS fundort_id_quellsystem,
    t.tk,
    w.verortung_punkt_wkt,
    ''::character(1) AS ist_raster,
    ''::character(1) AS rasterart,
    ''::character(1) AS erfassungsmassstab,
    w.verortung_flaeche_wkt,
    w.erfassungsgenauigkeit,
    l.landkreis,
    n.naturraum_code_quellsystem,
    ''::character(1) AS naturraum_code_lfu,
    ''::character(1) AS lebensraum_flaeche,
    ''::character(1) AS lebensraum_breite,
    ''::character(1) AS schutzvorschlag,
    ''::character(1) AS schutzvorschlag_code_lfu,
    ''::character(1) AS schutzvorschlag_code_quellsystem,
    ''::character(1) AS gefaehrdung,
    ''::character(1) AS gefaehrdung_code_lfu,
    ''::character(1) AS gefaehrdung_code_quellsystem,
    ''::character(1) AS nutzung,
    ''::character(1) AS nutzung_code_lfu,
    ''::character(1) AS nutzung_code_quellsystem,
    ''::character(1) AS schutzstatus,
    ''::character(1) AS schutzstatus_code_lfu,
    ''::character(1) AS schutzstatus_code_quellsystem,
    ''::character(1) AS vorhandenen_fundort_loeschen,
    e."Notes" AS bemerkung,
    s."LogUpdatedWhen" AS "datum_letzte_Änderung_dwb",
    t.quadrant,
    h.hoehe_von,
    h.hoehe_bis,
    o.toponym,
    lr.lebensraum,
    lr.lebensraum_code_lfu,
    lr.lebensraum_code_quellsystem,
    e."LocalityDescription" AS lage,
    l.landkreis_code_quellsystem,
    n.naturraum,
    ''::character(1) AS verortung_linie_wkt,
    t.punkt_berechnet_wkt,
    l.landkreis_code_lfu
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
--######   GUC__IdentificationLast   ##################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."GUC__IdentificationLast" AS 
 SELECT i."CollectionSpecimenID",
    i."IdentificationUnitID",
    Max(i."IdentificationSequence") AS "LastIdentificationSequence"
   FROM "#project#"."CacheIdentification" i
GROUP BY i."CollectionSpecimenID",
    i."IdentificationUnitID";
    
ALTER TABLE "#project#"."GUC__IdentificationLast"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."GUC__IdentificationLast" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."GUC__IdentificationLast" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__IdentificationLast" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__IdentificationLast" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__LfuCode              ##################################################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."GUC__LfuCode" AS 
 SELECT concat(s."BaseURL", s."NameID"::text) AS "NameURI",
    e."ExternalNameURI"
   FROM  "TaxonSynonymy" s
     JOIN "TaxonNameExternalID" e ON s."NameID" = e."NameID"
     JOIN "TaxonNameExternalDatabase" d ON e."ExternalDatabaseID" = d."ExternalDatabaseID"
     WHERE d."ExternalDatabaseName" = 'Bayerisches Landesamt für Umwelt';

ALTER TABLE "#project#"."GUC__LfuCode"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."GUC__LfuCode" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."GUC__LfuCode" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__LfuCode" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__LfuCode" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC__Identification       ##################################################################################
--#####################################################################################################################

--DROP VIEW "#project#"."GUC__Identification";

CREATE OR REPLACE VIEW "#project#"."GUC__Identification" AS 
 SELECT i."CollectionSpecimenID",
    i."IdentificationUnitID",
    i."IdentificationSequence",
    i."Notes",
    lc."ExternalNameURI" AS artencode,
        CASE
            WHEN i."IdentificationQualifier"::text ~~ 's. %'::text OR i."IdentificationQualifier"::text = 'agg.'::text THEN 'Standardsicherheit'::text
            ELSE
            CASE
                WHEN i."IdentificationQualifier" IS NULL OR i."IdentificationQualifier"::text = ''::text OR i."IdentificationQualifier"::text ~~ '% ssp.'::text THEN ''::text
                ELSE 'unsicher'::text
            END
        END::character varying(50) AS datenqualitaet,
        CASE
            WHEN a."InheritedName"::text <> ''::text THEN a."InheritedName"
            ELSE
            CASE
                WHEN i."ResponsibleName"::text ~~ '%,%'::text THEN ltrim(rtrim(substr(i."ResponsibleName"::text, 1, strpos(i."ResponsibleName"::text, ','::text) - 1)))::character varying
                ELSE i."ResponsibleName"
            END::character varying(255)
        END AS determinator_nachname,
        CASE
            WHEN a."GivenName"::text <> ''::text THEN a."GivenName"
            ELSE
            CASE
                WHEN i."ResponsibleName"::text ~~ '%,%'::text THEN ltrim(rtrim(substr(i."ResponsibleName"::text, strpos(i."ResponsibleName"::text, ','::text) + 1, 500)))
                ELSE ''::text
            END::character varying(255)
        END AS determinator_vorname,
    ac."City" AS determinator_ort,
    i."TaxonomicName" AS taxonomicname,
    lc."ExternalNameURI" AS LfuCode
   FROM  "#project#"."GUC__IdentificationLast" l
     JOIN "#project#"."CacheIdentification" i ON i."CollectionSpecimenID" = l."CollectionSpecimenID" AND i."IdentificationUnitID" = l."IdentificationUnitID" AND i."IdentificationSequence" = "LastIdentificationSequence"
     LEFT JOIN "Agent" a ON i."ResponsibleAgentURI"::text = concat(a."BaseURL", a."AgentID"::text)
     LEFT JOIN "AgentContactInformation" ac ON a."AgentID" = ac."AgentID" AND a."SourceView"::text = ac."SourceView"::text
     LEFT JOIN "#project#"."GUC__LfuCode" lc ON i."NameURI" = lc."NameURI";


--#####################################################################################################################
--######   GUC_NACHWEIS   #############################################################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."GUC_NACHWEIS" AS 
 SELECT e."CollectionEventID" AS fundort_id_quellsystem,
    u."IdentificationUnitID" AS nachweis_id_quellsystem,
        CASE
            WHEN u."OnlyObserved" = 'True'::bpchar THEN 'Freilanderfassung'::text
            ELSE p.material
        END AS quellentyp,
    u."NumberOfUnits" AS anzahl,
    ''::character(1) AS anzahlgenauigkeit,
    ''::character(1) AS bestandschaetzung_von,
    ''::character(1) AS bestandschaetzung_bis,
    e."CollectionDay" AS beobachtungsdatum_tag,
    e."CollectionMonth" AS beobachtungsdatum_monat,
    e."CollectionYear" AS beobachtungsdatum_jahr,
    i.datenqualitaet,
    e."HabitatDescription" AS engerer_fundort,
    ''::character(1) AS punkt_berechnet_wkt,
    ''::character(1) AS verortung_punkt_wkt,
    ''::character(1) AS verortung_linie_wkt,
    ''::character(1) AS verortung_flaeche_wkt,
    ''::character(1) AS nachweisstadium,
    ''::character(1) AS nachweisstadium_code_lfu,
    ''::character(1) AS nachweisstadium_code_quellsystem,
    ''::character(1) AS methodik,
    ''::character(1) AS methodik_code_lfu,
    ''::character(1) AS methodik_code_quellsystem,
    a.status_code_lfu,
    ''::character(1) AS bearbeiter_vorname,
    ''::character(1) AS bearbeiter_ort,
    i.determinator_nachname,
    i.determinator_vorname,
    i.determinator_ort,
    ''::character(1) AS verbleib_vorname,
    ''::character(1) AS verbleib_bemerkung,
    ''::character(1) AS auftraggeber,
    ''::character(1) AS auftraggeber_code_lfu,
    ''::character(1) AS auftraggeber_code_quellsystem,
    ''::character(1) AS projektgattung,
    ''::character(1) AS projektgattung_code_lfu,
    ''::character(1) AS projektgattung_code_quellsystem,
    ''::character(1) AS ffh_populationsstruktur,
    ''::character(1) AS ffh_populationsstruktur_bemerkung,
    ''::character(1) AS ffh_habitatqualitaet,
    ''::character(1) AS ffh_habitatqualitaet_bemerkung,
    ''::character(1) AS ffh_beeintraechtigungen,
    ''::character(1) AS ffh_beeintraechtigungen_bemerkung,
    s."ReferenceTitle" AS literatur,
    ''::character(1) AS zusatz_vitalitaet,
    ''::character(1) AS zusatz_phaenologie,
    ''::character(1) AS abundanz_skala,
    ''::character(1) AS abundanz_skala_wert,
    ''::character(1) AS abundanz_bezug_typ,
    ''::character(1) AS abundanz_bezug_wert,
    ''::character(1) AS vorhandenen_nachweis_loeschen,
    s."LogUpdatedWhen" AS datum_letzte_aenderung,
    an."Annotation" AS bearbeiter_nachname,
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
    i.artencode,
    es."ExternalDatasourceName" AS projekt
   FROM "#project#"."CacheCollectionSpecimen" s
     JOIN "#project#"."CacheCollectionEvent" e ON e."CollectionEventID" = s."CollectionEventID"
     JOIN "#project#"."CacheIdentificationUnit" u ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."GUC__Identification" i ON i."IdentificationUnitID" = u."IdentificationUnitID" AND i.taxonomicname::text = u."LastIdentificationCache"::text
     LEFT JOIN "#project#"."GUC__IdentificationUnitAnalysis" a ON a.nachweis_id_quellsystem = u."IdentificationUnitID"
     LEFT JOIN "#project#"."GUC__SpecimenPart" p ON p."IdentificationUnitID" = u."IdentificationUnitID"
     LEFT JOIN "#project#"."GUC__SpecimenRelation" r ON p."IdentificationUnitID" = u."IdentificationUnitID"
     LEFT JOIN "#project#"."CacheAnnotation" an ON an."ReferencedID" = u."CollectionSpecimenID" AND an."ReferencedTable"::text = 'CollectionSpecimen'::text
     LEFT JOIN "#project#"."CacheCollectionExternalDatasource" es ON s."ExternalDatasourceID" = es."ExternalDatasourceID";


--#####################################################################################################################
--######   GUC__IdentificationUnitAnalysis   ##########################################################################
--#####################################################################################################################




CREATE OR REPLACE VIEW "#project#"."GUC__IdentificationUnitAnalysis" AS 
 SELECT a."IdentificationUnitID" AS nachweis_id_quellsystem,
        CASE
            WHEN a."AnalysisResult" = 'A'::text THEN 'AN'::text
            ELSE
            CASE
                WHEN a."AnalysisResult" = 'E'::text THEN 'EG'::text
                ELSE
                CASE
                    WHEN a."AnalysisResult" = 'I'::text THEN 'IN'::text
                    ELSE 
		    CASE
			WHEN a."AnalysisResult" = 'K'::text THEN 'KU'::text
			ELSE
			CASE 
			    WHEN a."AnalysisResult" = 'Z'::text THEN 'VZ'::text
			    ELSE
			    CASE 
			        WHEN a."AnalysisResult" = 'S'::text THEN 'SY'::text
			        ELSE
			        CASE
			            WHEN a."AnalysisResult" = 'U'::text THEN 'UN'::text
			            ELSE NULL
			        END
			    END
			END
		    END
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

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 6 WHERE "Package" = 'GUC'

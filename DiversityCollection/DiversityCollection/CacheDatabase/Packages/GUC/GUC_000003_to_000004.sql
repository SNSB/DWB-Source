-- GUC

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package GUC to version 3
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   GUC__Landkreis             #################################################################################
--#####################################################################################################################

DROP VIEW "#project#"."GUC_FUNDORT"; 

DROP VIEW "#project#"."GUC__Landkreis";

CREATE OR REPLACE VIEW "#project#"."GUC__Landkreis" AS 
 SELECT e."CollectionEventID",
    l."Location1" AS landkreis,
    case when l."Location2" like '%/%' then reverse(substring(reverse(l."Location2") from 1 for position('/' in reverse(l."Location2")) - 1)) else '' end AS landkreis_code_quellsystem,
    case when l."Location2" like '%/%' then reverse(substring(reverse(l."Location2") from 1 for position('/' in reverse(l."Location2")) - 1)) else '' end AS landkreis_code_lfu
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
    w.punkt_berechnet_wkt,
    w.verortung_flaeche_wkt,
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
--######   GUC_NACHWEIS               #################################################################################
--#####################################################################################################################

DROP VIEW "#project#"."GUC_NACHWEIS";
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
    cast(case when i."ResponsibleName" like '%,%' then ltrim(rtrim(substr(i."ResponsibleName"::text, 1, strpos(i."ResponsibleName"::text, ','::text) - 1))) else i."ResponsibleName" end as character varying(255)) AS determinator_nachname,
    cast(case when i."ResponsibleName" like '%,%' then ltrim(rtrim(substr(i."ResponsibleName"::text, strpos(i."ResponsibleName"::text, ','::text) + 1, 500))) else '' end as character varying(255)) AS determinator_vorname,
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
    case when i."NameURI" like '%/%' then reverse(substring(reverse(i."NameURI") from 1 for position('/' in reverse(i."NameURI")) - 1)) else '' end AS artencode
   FROM "#project#"."CacheIdentification" i,
    "#project#"."CacheCollectionSpecimen" s,
    "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheIdentificationUnit" u
     LEFT JOIN "#project#"."GUC__IdentificationUnitAnalysis" a ON a.nachweis_id_quellsystem = u."IdentificationUnitID"
     LEFT JOIN "#project#"."GUC__SpecimenPart" p ON p."IdentificationUnitID" = u."IdentificationUnitID"
     LEFT JOIN "#project#"."GUC__SpecimenRelation" r ON p."IdentificationUnitID" = u."IdentificationUnitID"
  WHERE u."CollectionSpecimenID" = s."CollectionSpecimenID" AND e."CollectionEventID" = s."CollectionEventID" AND i."IdentificationUnitID" = u."IdentificationUnitID" AND i."TaxonomicName"::text = u."LastIdentificationCache"::text;



--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 4 WHERE "Package" = 'GUC'
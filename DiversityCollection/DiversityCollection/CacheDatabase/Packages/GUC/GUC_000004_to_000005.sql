-- GUC

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package GUC to version 5
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   GUC_FUNDORT                #################################################################################
--#####################################################################################################################

DROP VIEW "#project#"."GUC_FUNDORT";

CREATE OR REPLACE VIEW "#project#"."GUC_FUNDORT" AS 
 SELECT e."CollectionEventID" AS fundort_id_quellsystem,
    t.tk,
    w.verortung_punkt_wkt,
    ''::character(1) AS ist_raster,
    ''::character(1) AS RASTERART,
    ''::character(1) AS ERFASSUNGSMASSSTAB,
    w.verortung_flaeche_wkt,
    w.erfassungsgenauigkeit,
    l.landkreis,
    n.naturraum_code_quellsystem,
    ''::character(1) AS NATURRAUM_CODE_LFU,
    ''::character(1) AS LEBENSRAUM_FLAECHE,
    ''::character(1) AS LEBENSRAUM_BREITE,
    ''::character(1) AS SCHUTZVORSCHLAG,				
    ''::character(1) AS SCHUTZVORSCHLAG_CODE_LFU,				
    ''::character(1) AS SCHUTZVORSCHLAG_CODE_QUELLSYSTEM,				
    ''::character(1) AS GEFAEHRDUNG,					
    ''::character(1) AS GEFAEHRDUNG_CODE_LFU,			
    ''::character(1) AS GEFAEHRDUNG_CODE_QUELLSYSTEM,				
    ''::character(1) AS NUTZUNG,			
    ''::character(1) AS NUTZUNG_CODE_LFU,	
    ''::character(1) AS NUTZUNG_CODE_QUELLSYSTEM,	
    ''::character(1) AS SCHUTZSTATUS,
    ''::character(1) AS SCHUTZSTATUS_CODE_LFU,	
    ''::character(1) AS SCHUTZSTATUS_CODE_QUELLSYSTEM,	
    ''::character(1) AS ORHANDENEN_FUNDORT_LOESCHEN,
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
    '' AS VERORTUNG_LINIE_WKT,
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
--######   GUC__Identification        #################################################################################
--#####################################################################################################################

DROP VIEW "#project#"."GUC_NACHWEIS";

--DROP VIEW "#project#"."GUC__Identification";

CREATE OR REPLACE VIEW "#project#"."GUC__Identification" AS 
SELECT "CollectionSpecimenID", "IdentificationUnitID", "IdentificationSequence", I."Notes",
"NameURI" AS ARTENCODE, 
       CASE
            WHEN i."IdentificationQualifier"::text ~~ 's. %'::text OR i."IdentificationQualifier"::text = 'agg.'::text THEN 'Standardsicherheit'::text
            ELSE
            CASE
                WHEN i."IdentificationQualifier" IS NULL OR i."IdentificationQualifier"::text = ''::text OR i."IdentificationQualifier"::text ~~ '% ssp.'::text THEN ''::text
                ELSE 'unsicher'::text
            END
        END::character varying(50) AS datenqualitaet,
case when A."InheritedName" <> '' then A."InheritedName" else
	cast(case when i."ResponsibleName" like '%,%' then ltrim(rtrim(substr(i."ResponsibleName"::text, 1, strpos(i."ResponsibleName"::text, ','::text) - 1))) else i."ResponsibleName" end as character varying(255)) 
	end
AS determinator_nachname,
case when A."GivenName" <> '' then A."GivenName" else
cast(case when i."ResponsibleName" like '%,%' then ltrim(rtrim(substr(i."ResponsibleName"::text, strpos(i."ResponsibleName"::text, ','::text) + 1, 500))) else '' end as character varying(255))
end AS determinator_vorname,    
AC."City" AS DETERMINATOR_ORT,
    i."TaxonomicName" AS TaxonomicName
  FROM "#project#"."CacheIdentification" AS I 
  left join "public"."Agent" A ON I."ResponsibleAgentURI" = concat(A."BaseURL", A."AgentID"::text)
  left join "public"."AgentContactInformation" AC ON a."AgentID" = AC."AgentID" AND  a."SourceView" = AC."SourceView";

ALTER TABLE "#project#"."GUC__Identification"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC__Identification" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC__Identification" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC__Identification" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC__Identification" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   GUC_NACHWEIS               #################################################################################
--#####################################################################################################################



CREATE OR REPLACE VIEW "#project#"."GUC_NACHWEIS" AS 
 SELECT e."CollectionEventID" AS fundort_id_quellsystem,
    u."IdentificationUnitID" AS nachweis_id_quellsystem,
        CASE
            WHEN u."OnlyObserved" = '1'::bpchar THEN 'Freilanderfassung'::text
            ELSE p."material"
        END AS quellentyp,
    u."NumberOfUnits" AS anzahl,
    ''::character(1) AS ANZAHLGENAUIGKEIT,
    ''::character(1) AS BESTANDSCHAETZUNG_VON,
    ''::character(1) AS BESTANDSCHAETZUNG_BIS,
    e."CollectionDay" AS beobachtungsdatum_tag,
    e."CollectionMonth" AS beobachtungsdatum_monat,
    e."CollectionYear" AS beobachtungsdatum_jahr,
    i.datenqualitaet,
    e."HabitatDescription" AS engerer_fundort,
    ''::character(1) AS PUNKT_BERECHNET_WKT,			
    ''::character(1) AS VERORTUNG_PUNKT_WKT,				
    ''::character(1) AS VERORTUNG_LINIE_WKT,				
    ''::character(1) AS VERORTUNG_FLAECHE_WKT,				
    ''::character(1) AS NACHWEISSTADIUM,				
    ''::character(1) AS NACHWEISSTADIUM_CODE_LFU,		
    ''::character(1) AS NACHWEISSTADIUM_CODE_QUELLSYSTEM,			
    ''::character(1) AS METHODIK,			
    ''::character(1) AS METHODIK_CODE_LFU,					
    ''::character(1) AS METHODIK_CODE_QUELLSYSTEM,
    a.STATUS_CODE_LFU,
    ''::character(1) AS BEARBEITER_VORNAME,			
    ''::character(1) AS BEARBEITER_ORT,
    i.determinator_nachname,
    i.determinator_vorname,
    i.determinator_ort,
    ''::character(1) AS VERBLEIB_VORNAME,		
    ''::character(1) AS VERBLEIB_BEMERKUNG,				
    ''::character(1) AS AUFTRAGGEBER,			
    ''::character(1) AS AUFTRAGGEBER_CODE_LFU,		
    ''::character(1) AS AUFTRAGGEBER_CODE_QUELLSYSTEM,				
    ''::character(1) AS PROJEKTGATTUNG,		
    ''::character(1) AS PROJEKTGATTUNG_CODE_LFU,			
    ''::character(1) AS PROJEKTGATTUNG_CODE_QUELLSYSTEM,			
    ''::character(1) AS FFH_POPULATIONSSTRUKTUR,			
    ''::character(1) AS FFH_POPULATIONSSTRUKTUR_BEMERKUNG,		
    ''::character(1) AS FFH_HABITATQUALITAET,					
    ''::character(1) AS FFH_HABITATQUALITAET_BEMERKUNG,				
    ''::character(1) AS FFH_BEEINTRAECHTIGUNGEN,			
    ''::character(1) AS FFH_BEEINTRAECHTIGUNGEN_BEMERKUNG,
    s."ReferenceTitle" AS literatur,
    ''::character(1) AS ZUSATZ_VITALITAET,			
    ''::character(1) AS ZUSATZ_PHAENOLOGIE,				
    ''::character(1) AS ABUNDANZ_SKALA,				
    ''::character(1) AS ABUNDANZ_SKALA_WERT,				
    ''::character(1) AS ABUNDANZ_BEZUG_TYP,			
    ''::character(1) AS ABUNDANZ_BEZUG_WERT,			
    ''::character(1) AS VORHANDENEN_NACHWEIS_LOESCHEN,
    s."LogUpdatedWhen" AS datum_letzte_aenderung,
    AN."Annotation" AS BEARBEITER_NACHNAME,
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
    es."ExternalDatasourceName" AS PROJEKT
  FROM "#project#"."CacheCollectionSpecimen" s 
  inner join "#project#"."CacheCollectionEvent" e on e."CollectionEventID" = s."CollectionEventID" 
    inner join "#project#"."CacheIdentificationUnit" u on  u."CollectionSpecimenID" = s."CollectionSpecimenID"
    inner join "#project#"."GUC__Identification" i on i."IdentificationUnitID" = u."IdentificationUnitID"  AND i.taxonomicname::text = u."LastIdentificationCache"::text
    LEFT JOIN "#project#"."GUC__IdentificationUnitAnalysis" a ON a.nachweis_id_quellsystem = u."IdentificationUnitID"
    LEFT JOIN "#project#"."GUC__SpecimenPart" p ON p."IdentificationUnitID" = u."IdentificationUnitID"
    LEFT JOIN "#project#"."GUC__SpecimenRelation" r ON p."IdentificationUnitID" = u."IdentificationUnitID"
    LEFT JOIN "#project#"."CacheAnnotation" AN ON AN."ReferencedID" = u."CollectionSpecimenID" AND AN."ReferencedTable" = 'CollectionSpecimen'
    LEFT JOIN "#project#"."CacheCollectionExternalDatasource" ES ON S."ExternalDatasourceID" = es."ExternalDatasourceID";

--#####################################################################################################################
--######   GUC_SAMMLER   ##############################################################################################
--#####################################################################################################################

DROP VIEW "#project#"."GUC_SAMMLER";

CREATE OR REPLACE VIEW "#project#"."GUC_SAMMLER" AS 
    SELECT case when AG."InheritedName" <> '' THEN AG."InheritedName" ELSE
        CASE
            WHEN a."CollectorsName"::text ~~ '%,%'::text THEN ltrim(rtrim(substr(a."CollectorsName"::text, 1, strpos(a."CollectorsName"::text, ','::text) - 1)))::character varying
            ELSE a."CollectorsName"
        END END::character varying(255) AS sammler_nachname,
        case when AG."GivenName" <> '' THEN AG."GivenName" ELSE
        CASE
            WHEN a."CollectorsName"::text ~~ '%,%'::text THEN ltrim(rtrim(substr(a."CollectorsName"::text, strpos(a."CollectorsName"::text, ','::text) + 1, 500)))
            ELSE ''::text
        END end::character varying(255) AS sammler_vorname,
        AGC."City" AS SAMMLER_ORT,
         u."IdentificationUnitID" AS NACHWEIS_ID_QUELLSYSTEM
   FROM "#project#"."CacheIdentificationUnit" u,
   "#project#"."CacheCollectionAgent" a
    left join "public"."Agent" AG ON a."CollectorsAgentURI" = concat(AG."BaseURL", AG."AgentID"::text)
    left join "public"."AgentContactInformation" AGC ON ag."AgentID" = AGC."AgentID" AND  ag."SourceView" = AGC."SourceView"
  WHERE u."CollectionSpecimenID" = a."CollectionSpecimenID";

  ALTER TABLE "#project#"."GUC_SAMMLER"
  OWNER TO postgres;
GRANT ALL ON TABLE "#project#"."GUC_SAMMLER" TO postgres;
GRANT SELECT ON TABLE "#project#"."GUC_SAMMLER" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."GUC_SAMMLER" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."GUC_SAMMLER" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 5 WHERE "Package" = 'GUC'
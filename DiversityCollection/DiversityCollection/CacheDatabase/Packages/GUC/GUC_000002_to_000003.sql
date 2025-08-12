-- GUC

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package GUC to version 3
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   GUC_SAMMLER                #################################################################################
--#####################################################################################################################

DROP VIEW "#project#"."GUC_SAMMLER";
CREATE OR REPLACE VIEW "#project#"."GUC_SAMMLER" AS 
SELECT u."IdentificationUnitID",
    cast(case when a."CollectorsName" like '%,%' then ltrim(rtrim(substr(a."CollectorsName"::text, 1, strpos(a."CollectorsName"::text, ','::text) - 1))) else a."CollectorsName" end as character varying(255)) AS sammler_nachname,
    cast(case when a."CollectorsName" like '%,%' then ltrim(rtrim(substr(a."CollectorsName"::text, strpos(a."CollectorsName"::text, ','::text) + 1, 500))) else '' end as character varying(255)) AS sammler_vorname,
    a."CollectorsName"
   FROM "#project#"."CacheCollectionAgent" a,
    "#project#"."CacheIdentificationUnit" u
  WHERE u."CollectionSpecimenID" = a."CollectionSpecimenID";


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
    i."NameURI" AS artencode
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

UPDATE "#project#"."Package" SET "Version" = 3 WHERE "Package" = 'GUC'
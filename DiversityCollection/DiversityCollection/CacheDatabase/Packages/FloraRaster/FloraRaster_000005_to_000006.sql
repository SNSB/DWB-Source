-- FloraRaster

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package FloraRaster to version 6
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   FloraRaster_Kopfdaten: Completion of missing years   #######################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_Kopfdaten" AS 
 SELECT e."CollectionEventID" AS "ID_Kopfdaten",
    (l."Location1"::text || ''::text) || l."Location2"::text AS rasterfeld,
    case when e."CollectionYear" is null then e."CollectionEndYear" else e."CollectionYear" end AS jahr_von,
    case when e."CollectionEndYear" is null then e."CollectionYear" else e."CollectionEndYear" end AS jahr_bis
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 3 AND l."Location2"::text <> ''::text;


--#####################################################################################################################
--######   FloraRaster_Sippendaten: Analysis optional and NameID from Identification   ################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_Sippendaten" AS 
SELECT s."CollectionEventID" AS "ID_Kopfdaten",
    I."NameID" AS taxnr,
    a.flor_status_orig,
    a.flor_status_korrigiert
   FROM "#project#"."FloraRaster__status" a right join
    "#project#"."CacheIdentificationUnit" u on a."IdentificationUnitID" = u."IdentificationUnitID",
    "#project#"."CacheCollectionSpecimen" s,
    "#project#"."CacheIdentification" i
  WHERE u."CollectionSpecimenID" = s."CollectionSpecimenID" 
  AND u."IdentificationUnitID" = i."IdentificationUnitID" AND u."LastIdentificationCache"::text = i."TaxonomicName"::text;


--#####################################################################################################################
--######   FloraRaster_Fotos   ########################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."FloraRaster_Fotos" AS 
 SELECT si."URI" AS "Link",
    t."AcceptedNameID" AS "SipNr",
    a."CollectorsName" AS "Beobachter",
    concat(e."CollectionDay",
        CASE
            WHEN e."CollectionDay" IS NULL THEN ''::text
            ELSE '.'::text
        END, e."CollectionMonth",
        CASE
            WHEN e."CollectionMonth" IS NULL THEN ''::text
            ELSE '.'::text
        END, e."CollectionYear") AS "Datum",
        CASE
            WHEN "position"(e."LocalityDescription", 'Bayern, '::text) = 0 THEN e."LocalityDescription"
            ELSE "substring"(e."LocalityDescription", 9)
        END AS "Locality",
    i."Notes"
   FROM "#project#"."CacheCollectionSpecimenImage" si,
    "#project#"."CacheIdentification" i,
    "#project#"."CacheIdentificationUnit" u,
    "#project#"."CacheCollectionAgent" a,
    "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionSpecimen" s,
    "TaxonSynonymy" t
  WHERE si."CollectionSpecimenID" = s."CollectionSpecimenID" AND i."CollectionSpecimenID" = s."CollectionSpecimenID" AND a."CollectionSpecimenID" = s."CollectionSpecimenID" 
  AND e."CollectionEventID" = s."CollectionEventID" AND i."NameURI"::text = concat(t."BaseURL", t."NameID") AND u."IdentificationUnitID" = i."IdentificationUnitID" 
  AND u."LastIdentificationCache"::text = i."TaxonomicName"::text;

ALTER TABLE "#project#"."FloraRaster_Fotos"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."FloraRaster_Fotos" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."FloraRaster_Fotos" TO "CacheUser";
GRANT SELECT ON TABLE "#project#"."FloraRaster_Fotos" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster_Fotos" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 6 WHERE "Package" = 'FloraRaster'
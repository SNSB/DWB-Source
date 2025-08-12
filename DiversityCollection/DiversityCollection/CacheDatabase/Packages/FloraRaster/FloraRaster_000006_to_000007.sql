-- FloraRaster

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package FloraRaster to version 7
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   FloraRaster_Kopfdaten including unschaerfe   ###############################################################
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
        case when l."LocalisationSystemID" = 3 then l."LocationAccuracy" else null end as "unschaerfe"
   FROM "#project#"."CacheCollectionEvent" e,
    "#project#"."CacheCollectionEventLocalisation" l
  WHERE e."CollectionEventID" = l."CollectionEventID" AND l."LocalisationSystemID" = 3 AND l."Location2"::text <> ''::text;


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
        END AS unschaerfe
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
GRANT SELECT ON TABLE "#project#"."FloraRaster_KartenRasterPunkte" TO "CacheUser_#project#";
GRANT ALL ON TABLE "#project#"."FloraRaster_KartenRasterPunkte" TO "CacheAdmin_#project#";


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 7 WHERE "Package" = 'FloraRaster'
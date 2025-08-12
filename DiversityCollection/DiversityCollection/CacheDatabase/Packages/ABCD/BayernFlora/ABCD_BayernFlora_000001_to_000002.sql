--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package BayernFloraABCD to version 2
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   ABCD__BayernFlora_EndangeredSpeciesBase   ##################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" AS 
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


ALTER TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" TO "CacheUser";


--#####################################################################################################################
--######   ABCD__BayernFlora_EndangeredSpeciesNameID   ################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID" AS 
SELECT "NameID" FROM "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" B
UNION  
SELECT T."NameID" FROM "TaxonSynonymy" T JOIN "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" B ON T."AcceptedNameID" = B."NameID"
UNION  
SELECT T."NameID" FROM "TaxonSynonymy" T JOIN "#project#"."ABCD__BayernFlora_EndangeredSpeciesBase" B ON T."NameParentID" = B."NameID";


ALTER TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID" TO "CacheUser";


--#####################################################################################################################
--######   ABCD__BayernFlora_EndangeredSpeciesEventID   ###############################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID" AS 
SELECT S."CollectionEventID" FROM "#project#"."ABCD__BayernFlora_EndangeredSpeciesNameID" T, 
"#project#"."CacheCollectionSpecimen" S,
"#project#"."CacheIdentification" I
WHERE T."NameID" = I."NameID"
AND I."CollectionSpecimenID" = S."CollectionSpecimenID";


ALTER TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID" TO "CacheUser";


--#####################################################################################################################
--######   ABCD__BayernFlora_WrongStatusUnitID   ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__BayernFlora_WrongStatusUnitID" AS 
SELECT "IdentificationUnitID"
  FROM "#project#"."CacheIdentificationUnitAnalysis"
  WHERE "AnalysisID" = 2 AND "AnalysisResult" IN ('2', '3', '4', '5', '6', '7', '8', '9', '?', '+', '-', 'F', 'V', 'X');


ALTER TABLE "#project#"."ABCD__BayernFlora_WrongStatusUnitID"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__BayernFlora_WrongStatusUnitID" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__BayernFlora_WrongStatusUnitID" TO "CacheUser";


--#####################################################################################################################
--######   function abcd__UnitPart: Without those with wrong status  ##################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__unitpart()
 RETURNS void
 LANGUAGE plpgsql
AS $function$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- cleaning the table ABCD__UnitPart
TRUNCATE TABLE "#project#"."ABCD__UnitPart";

-- removing key
ALTER TABLE "#project#"."ABCD__UnitPart" DROP CONSTRAINT IF EXISTS "ABCD__UnitPart_pkey";

-- insert Units with parts
-- Difference to ABCD: Exclusion of units with wrong status
INSERT INTO "#project#"."ABCD__UnitPart"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", "IdentificationUnitID")
SELECT DISTINCT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
        CASE
            WHEN up."StableIdentifier" IS NULL THEN u."StableIdentifier"
            ELSE up."StableIdentifier"
        END::text AS "UnitGUID",
    CASE WHEN c."CollectionAcronym" <> '' THEN c."CollectionAcronym" ELSE c."CollectionName" END,
    m."ProjectTitleCode" AS "SourceID",
    btrim(concat(
        CASE
            WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
            ELSE u."CollectionSpecimenID"::character varying
        END, ' / ', u."IdentificationUnitID"::character varying,
        CASE
            WHEN p."AccessionNumber"::text <> ''::text THEN concat(' / ', p."AccessionNumber")
            ELSE
            CASE
                WHEN up."SpecimenPartID"::text <> ''::text THEN concat(' / ', up."SpecimenPartID"::character varying)
                ELSE ''::text
            END
        END)) AS "UnitID",
    s."LogUpdatedWhen" AS "DateLastEdited",
    btrim(u."LastIdentificationCache")::character varying(255) AS "Identification_Taxon_ScientificName_FullScientificName",
    r."RecordBasis",
    CASE WHEN p."MaterialCategory" IS NULL THEN u."RetrievalType" ELSE p."MaterialCategory"::character varying(254) END AS "KindOfUnit",
    k."Kingdom" AS "Identification_Taxon_HigherTaxonName",
    'en'::character varying(2) AS "KindOfUnit_Language",
    concat(s."ExsiccataAbbreviation", case when u."ExsiccataNumber"::text <> ''::text
		   THEN ' Exs. no. ' else '' end, u."ExsiccataNumber") AS "HerbariumUnit_Exsiccatum",
        CASE
            WHEN m."RecordUri"::text ~~ 'http://biocase%'::text THEN concat(m."RecordUri", btrim(concat(
            CASE
                WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
                ELSE u."CollectionSpecimenID"::character varying
            END, ' / ', u."IdentificationUnitID"::character varying,
            CASE
                WHEN up."SpecimenPartID"::text <> ''::text THEN 
		    CASE 
			WHEN p."AccessionNumber"::text <> ''::text THEN
			    concat(' / ', p."AccessionNumber"::character varying)
			ELSE
			    concat(' / ', up."SpecimenPartID"::character varying)
		    END
                ELSE ''::text
            END)))
            ELSE
            CASE
                WHEN m."RecordUri"::text <> ''::text THEN concat(m."RecordUri", s."CollectionSpecimenID"::character varying)
                ELSE NULL::text
            END
        END AS "RecordURI",
    u."CollectionSpecimenID",
    u."IdentificationUnitID"
   FROM "#project#"."CacheCollectionSpecimenPart" p
     JOIN "#project#"."CacheIdentificationUnitInPart" up ON p."SpecimenPartID" = up."SpecimenPartID"
     JOIN "#project#"."ABCD__RecordBasis" r ON r."IdentificationUnitID" = up."IdentificationUnitID" AND r."SpecimenPartID" = up."SpecimenPartID"
     JOIN "#project#"."CacheIdentificationUnit" u  ON r."IdentificationUnitID" = u."IdentificationUnitID"
     JOIN "#project#"."ABCD__Kingdom" k ON u."IdentificationUnitID" = k."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON s."CollectionSpecimenID" = u."CollectionSpecimenID"
     JOIN "#project#"."CacheCollection" c ON c."CollectionID" = p."CollectionID"
     CROSS JOIN "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid()
 WHERE u."IdentificationUnitID" NOT IN (SELECT "IdentificationUnitID" FROM "#project#"."ABCD__BayernFlora_WrongStatusUnitID");

-- adding the key
ALTER TABLE "#project#"."ABCD__UnitPart"
  ADD CONSTRAINT "ABCD__UnitPart_pkey" PRIMARY KEY("ID");

end;
$function$
;

COMMENT ON FUNCTION "#project#".abcd__unitpart() 
	IS 'Transfer data into table ABCD__UnitPart';

	

--#####################################################################################################################
--######   function abcd__UnitNoPart: Without those with wrong status  ################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__UnitNoPart()
  RETURNS void AS
$BODY$
begin

-- cleaning the table
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

-- insert Units without parts
INSERT INTO "#project#"."ABCD__UnitNoPart"(
            "ID", "UnitGUID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "KindOfUnit", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "CollectionSpecimenID", "IdentificationUnitID")
SELECT concat(u."IdentificationUnitID"::character varying, '-') AS "ID",
    u."StableIdentifier" AS "UnitGUID",
    btrim(concat(
        CASE
            WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
            ELSE u."CollectionSpecimenID"::character varying
        END, ' / ', u."IdentificationUnitID"::character varying
        )) AS "UnitID",
    s."LogUpdatedWhen" AS "DateLastEdited",
    btrim(u."LastIdentificationCache")::character varying(255) AS "Identification_Taxon_ScientificName_FullScientificName",
    CASE WHEN u."RetrievalType" IS NULL THEN 'human observation' ELSE u."RetrievalType" END AS "KindOfUnit",
    'en'::character varying(2) AS "KindOfUnit_Language",
    s."ExsiccataAbbreviation" AS "HerbariumUnit_Exsiccatum",
    u."CollectionSpecimenID",
    u."IdentificationUnitID"
   FROM "#project#"."CacheIdentificationUnit" u
     JOIN "#project#"."CacheCollectionSpecimen" s ON s."CollectionSpecimenID" = u."CollectionSpecimenID"
     LEFT JOIN "#project#"."CacheIdentificationUnitInPart" up ON u."IdentificationUnitID" = up."IdentificationUnitID" 
     WHERE up."IdentificationUnitID" IS NULL
     AND u."IdentificationUnitID" NOT IN(SELECT "IdentificationUnitID" FROM "#project#"."ABCD__BayernFlora_WrongStatusUnitID");

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__UnitNoPart()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__UnitNoPart() TO "CacheAdmin";


--#####################################################################################################################
--######   function abcd__Unit_Associations_UnitAssociation: Without those with wrong status  #########################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_Associations_UnitAssociation()
  RETURNS void AS
$BODY$
begin

-- clean the table
TRUNCATE TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation";

-- remove the key
ALTER TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation" DROP CONSTRAINT "ABCD_Unit_Associations_UnitAssociation_pkey";

-- remove the indices
DROP INDEX "#project#".abcd_unit_associations_unitassociation_id;

-- insert the data
INSERT INTO "#project#"."ABCD_Unit_Associations_UnitAssociation"(
            "AssociatedUnitID", "SourceInstitutionCode", "SourceName", "AssociationType", 
            "AssociationType_Language", "Comment", "Comment_Language", "IdentificationUnitID", 
            "ID", "ID_Related", "UnitID", "UnitGUID", "KindOfUnit", "SpecimenPartID", 
            "CollectionSpecimenID")
SELECT btrim(concat(
        CASE
            WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
            ELSE u."CollectionSpecimenID"::character varying
        END, ' / ', p."IdentificationUnitID"::character varying,
        CASE
            WHEN pa."AccessionNumber"::text <> ''::text THEN concat(' / ', pa."AccessionNumber")
            ELSE
            CASE
                WHEN pp."SpecimenPartID"::text <> ''::text THEN concat(' / ', pp."SpecimenPartID"::character varying)
                ELSE ''::text
            END
        END)) AS "AssociatedUnitID",
    m."SourceInstitutionID" AS "SourceInstitutionCode",
    m."ProjectTitleCode" AS "SourceName",
    u."RelationType" AS "AssociationType",
    'en'::character varying AS "AssociationType_Language",
    ltrim(concat(rtrim(rtrim(u."RelationType"::text)), ' ',
        CASE
            WHEN u."RelationType"::text ~~ '% on'::text THEN ''::text
            ELSE 'on '::text
        END,
        CASE
            WHEN p."LastIdentificationCache"::text = ''::text THEN p."TaxonomicGroup"
            ELSE p."LastIdentificationCache"
        END)) AS "Comment",
    'en'::character varying AS "Comment_Language",
    u."IdentificationUnitID",
    concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    concat(p."IdentificationUnitID"::character varying, '-', pp."SpecimenPartID"::character varying) AS "ID_Related",
    concat(p."CollectionSpecimenID"::character varying, '-', p."IdentificationUnitID"::character varying, '-', pp."SpecimenPartID"::character varying) AS "UnitID",
        CASE
            WHEN pp."StableIdentifier" IS NULL THEN p."StableIdentifier"
            ELSE pp."StableIdentifier"
        END AS "UnitGUID",
    pa."MaterialCategory"::character varying(254) AS "KindOfUnit",
    up."SpecimenPartID",
    u."CollectionSpecimenID"
   FROM "#project#"."CacheCollectionSpecimenPart" pa
     RIGHT JOIN "#project#"."CacheIdentificationUnitInPart" pp ON pa."SpecimenPartID" = pp."SpecimenPartID"
     RIGHT JOIN "#project#"."CacheIdentificationUnit" p ON p."IdentificationUnitID" = pp."IdentificationUnitID"
     JOIN "#project#"."CacheIdentificationUnit" u ON u."RelatedUnitID" = p."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON s."CollectionSpecimenID" = u."CollectionSpecimenID"
     LEFT JOIN "#project#"."CacheIdentificationUnitInPart" up ON u."IdentificationUnitID" = up."IdentificationUnitID"
     CROSS JOIN "#project#"."CacheMetadata" m
     WHERE u."IdentificationUnitID" NOT IN(SELECT "IdentificationUnitID" FROM "#project#"."ABCD__BayernFlora_WrongStatusUnitID")
     AND p."IdentificationUnitID" NOT IN(SELECT "IdentificationUnitID" FROM "#project#"."ABCD__BayernFlora_WrongStatusUnitID");

-- adding the key
ALTER TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation"
  ADD CONSTRAINT "ABCD_Unit_Associations_UnitAssociation_pkey" PRIMARY KEY("ID", "ID_Related");
  
-- adding the index
CREATE INDEX abcd_unit_associations_unitassociation_id
  ON "#project#"."ABCD_Unit_Associations_UnitAssociation"
  USING btree
  ("ID" COLLATE pg_catalog."default");
       
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit_Associations_UnitAssociation()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit_Associations_UnitAssociation() TO "CacheAdmin";


--#####################################################################################################################
--######   function abcd__Unit_Gathering: Endangered species without locality text  ###################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_Gathering()
  RETURNS void AS
$BODY$
begin

-- cleaning table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering";

-- removing key
ALTER TABLE "#project#"."ABCD_Unit_Gathering" DROP CONSTRAINT "ABCD_Unit_Gathering_pkey";

-- insert data
INSERT INTO "#project#"."ABCD_Unit_Gathering"(
            "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
            "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
            "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum")
SELECT DISTINCT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    e."CountryCache" AS "Country_Name",
     concat_ws('-'::text, COALESCE(e."CollectionYear"::character varying(4), '-'::character varying), lpad(e."CollectionMonth"::character varying(2)::text, 2, '0'::text), 
     lpad(e."CollectionDay"::character varying(2)::text, 2, '0'::text))::character varying(10) AS "DateTime_ISODateTimeBegin",
    case when es."CollectionEventID" is null then e."LocalityDescription" else '' end AS "LocalityText",
    "#project#".abcd__latitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    "#project#".abcd__longitude(e."CollectionEventID") AS "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
    ''::character varying(50) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName",
    ''::character varying(254) AS "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank",
    u."IdentificationUnitID",
        CASE
            WHEN "#project#".abcd__latitude(e."CollectionEventID") IS NULL THEN NULL::text
            ELSE 'WGS84'::text
        END AS "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEvent" e ON e."CollectionEventID" = s."CollectionEventID"
     LEFT JOIN "#project#"."ABCD__BayernFlora_EndangeredSpeciesEventID" es on e."CollectionEventID" = es."CollectionEventID";

-- writing the ISO Code for the country
SET ROLE "CacheAdmin"; -- ensure access to table Gazetteer
-- filling the Country table if empty
IF (SELECT COUNT(*) FROM "#project#"."ABCD__CountryName_ISO3166Code") = 0 
THEN
	INSERT INTO "#project#"."ABCD__CountryName_ISO3166Code" ("CountryName", "ISO3166Code")
	SELECT "C"."Name",
	MIN("G"."Name") AS "ISO3166Code"
	FROM "Gazetteer" "C"
	JOIN "Gazetteer" "G" ON "C"."PlaceID" = "G"."PlaceID"
	WHERE "G"."LanguageCode"::text = 'ISO 3166 ALPHA-3'::text AND ("C"."LanguageCode"::text !~~ 'ISO %'::text OR "C"."LanguageCode" IS NULL) AND "C"."NameID" <> "G"."NameID"
	GROUP BY "C"."Name";
END IF;

SET ROLE "CacheAdmin_#project#";

-- drop table if existing
DROP TABLE IF EXISTS ABCD__Unit_Gathering_ISO3166Code;

-- creating the temporary table containing the ISO Code
CREATE TEMP TABLE ABCD__Unit_Gathering_ISO3166Code AS SELECT "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
       "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
       "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
       "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
       "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "I"."ISO3166Code"
  FROM "#project#"."ABCD__CountryName_ISO3166Code" "I"
     RIGHT JOIN "#project#"."ABCD_Unit_Gathering" "A" ON "A"."Country_Name" = "I"."CountryName";

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering";

-- inserting the data
INSERT INTO "#project#"."ABCD_Unit_Gathering"(
            "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
            "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
            "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
            "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
            "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "ISO3166Code")
SELECT "ID", "Country_Name", "DateTime_ISODateTimeBegin", 
       "LocalityText", "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal", 
       "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal", "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName", 
       "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank", 
       "IdentificationUnitID", "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum", "ISO3166Code"
  FROM ABCD__Unit_Gathering_ISO3166Code;

-- Adding key
ALTER TABLE "#project#"."ABCD_Unit_Gathering" 
  ADD CONSTRAINT "ABCD_Unit_Gathering_pkey" PRIMARY KEY("ID");

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit_Gathering()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit_Gathering() TO "CacheAdmin";


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."PackageAddOn" SET "Version" = 2 WHERE "Package" = 'ABCD' AND "AddOn" = 'ABCD_BayernFlora'


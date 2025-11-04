--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 13
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--###### ABCD_Unit_Gathering_CoordinatesGrid for TK25 #############################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit_Gathering_CoordinatesGrid"
AS
SELECT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
'TK25'::character varying(50) AS "GridCellSystem",
e."Location1" AS "GridCellCode",
substring(e."Location2", 1, 1)::character varying(255) AS "GridQualifier",
e."RecordingMethod" AS "Method"
FROM "#project#"."CacheIdentificationUnitInPart" up
RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
JOIN "#project#"."CacheCollectionEventLocalisation" e ON e."CollectionEventID" = s."CollectionEventID" AND e."LocalisationSystemID" = 3;

ALTER TABLE public."ABCD_Unit_Gathering_CoordinatesGrid"
OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Unit_Gathering_CoordinatesGrid"
IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering/CoordinateSets/CoordinateSet/CoordinatesGrid. Data retrieved from table CollectionEventLocalisation for TK25';

GRANT ALL ON TABLE public."ABCD_Unit_Gathering_CoordinatesGrid" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_Gathering_CoordinatesGrid" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."ID"
IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."GridCellSystem"
IS 'ABCD: Unit/Gathering/Gathering/CoordinateSets/CoordinateSet/CoordinatesGrid/GridCellSystem. Retrieved from View = TK25';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."GridCellCode"
IS 'ABCD: Unit/Gathering/Gathering/CoordinateSets/CoordinateSet/CoordinatesGrid/GridCellSystem/GridCellCode. Retrieved from column Location1 in table CollectionEventLocalisation. Corresponds to TK25';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."GridQualifier"
IS 'ABCD: Unit/Gathering/Gathering/CoordinateSets/CoordinateSet/CoordinatesGrid/GridCellSystem/GridQualifier. Retrieved from column Location2 in table CollectionEventLocalisation. Corresponds to Quadrant in TK25';

COMMENT ON COLUMN public."ABCD_Unit_Gathering_CoordinatesGrid"."Method"
IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinateSet/Method. Retrieved from column RecordingMethod in table CollectionEventLocalisation';


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 13 WHERE "Package" = 'ABCD'
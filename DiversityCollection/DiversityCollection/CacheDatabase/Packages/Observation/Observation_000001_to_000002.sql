-- Observation

--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package Observation to version 2
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   Observation - Event data  ##################################################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "#project#"."Observation__MTB" AS 
 SELECT M."CollectionEventID"
 , M."Location1" AS "MTB",  M."Location2" AS "Quadrant", M."LocationAccuracy" AS "Accuracy"
   FROM "#project#"."CacheCollectionEventLocalisation" AS M
  where M."LocalisationSystemID" = 3;


CREATE OR REPLACE VIEW "#project#"."Observation__Altitude" AS 
 SELECT A."CollectionEventID"
 , A."Location1" AS "AltitudeFrom",  A."Location2" AS "AltitudeTo"
   FROM "#project#"."CacheCollectionEventLocalisation" A
  where A."LocalisationSystemID" = 4;

  
CREATE OR REPLACE VIEW "#project#"."Observation__GaussKrueger" AS 
 SELECT G."CollectionEventID"
  , G."Location1" AS "RW_GaussKrueger",  G."Location2" AS "HW_GaussKrueger"
   FROM "#project#"."CacheCollectionEventLocalisation" G 
  where G."LocalisationSystemID" = 2;

  
CREATE OR REPLACE VIEW "#project#"."Observation__WGS84" AS 
 SELECT W."CollectionEventID"
  , W."Location1" AS "Longitude_WGS84",  W."Location2" AS "Latitude_WGS84"
   FROM "#project#"."CacheCollectionEventLocalisation" W 
  where W."LocalisationSystemID" = 8;

  
CREATE OR REPLACE VIEW "#project#"."Observation__NamedArea" AS 
 SELECT V."CollectionEventID"
 , V."Location1" AS "NamedArea"
   FROM "#project#"."CacheCollectionEventLocalisation" V 
  where V."LocalisationSystemID" = 7;



CREATE OR REPLACE VIEW "#project#"."Observation__Event" AS 
 SELECT E."CollectionEventID", E."CollectionDay", E."CollectionMonth", E."CollectionYear"
 , E."CollectionEndDay", E."CollectionEndMonth", E."CollectionEndYear"
 , E."LocalityDescription", E."HabitatDescription"
 , M."MTB",  M."Quadrant", M."Accuracy"
 , A."AltitudeFrom",  A."AltitudeTo"
 , G."RW_GaussKrueger",  G."HW_GaussKrueger"
 , W."Longitude_WGS84",  W."Latitude_WGS84"
 , V."NamedArea"
   FROM "#project#"."CacheCollectionEvent" E
   left outer join "#project#"."Observation__MTB" M ON  E."CollectionEventID" = M."CollectionEventID"
   left outer join "#project#"."Observation__WGS84" W ON  E."CollectionEventID" = W."CollectionEventID"
   left outer join "#project#"."Observation__GaussKrueger" G ON  E."CollectionEventID" = G."CollectionEventID"
   left outer join "#project#"."Observation__Altitude" A ON  E."CollectionEventID" = A."CollectionEventID"
   left outer join "#project#"."Observation__NamedArea" V ON  E."CollectionEventID" = V."CollectionEventID";

--#####################################################################################################################

ALTER TABLE "#project#"."Observation__Event"
  OWNER TO "CacheAdmin";

--#####################################################################################################################

GRANT ALL ON TABLE "#project#"."Observation__Event" TO "CacheAdmin";

--#####################################################################################################################

GRANT SELECT ON TABLE "#project#"."Observation__Event" TO "CacheUser";

--#####################################################################################################################

COMMENT ON VIEW "#project#"."Observation__Event"
  IS 'Data of the collection events where the organisms where observed';
COMMENT ON COLUMN "#project#"."Observation__Event"."MTB" IS 'Messtischblatt';
COMMENT ON COLUMN "#project#"."Observation__Event"."RW_GaussKrueger" IS 'Rechtswert - Gauss Krueger';
COMMENT ON COLUMN "#project#"."Observation__Event"."HW_GaussKrueger" IS 'Hochwert - Gauss Krueger';




--#####################################################################################################################
--######   Observation__Identification   ##############################################################################
--#####################################################################################################################
--DROP VIEW "#project#"."Observation__Identification";

CREATE OR REPLACE VIEW "#project#"."Observation__Identification" AS 
SELECT I."CollectionSpecimenID", I."IdentificationUnitID", I."TaxonomicName", I."BaseURL", I."NameID", T."AcceptedName", T."AcceptedNameID", I."ResponsibleName", 
       U."NumberOfUnits", case when U."Notes" <> '' and I."Notes" <> '' then concat(U."Notes", '; ', I."Notes") 
       else case when U."Notes" <> '' then U."Notes" else I."Notes" end end As "Notes", S."CollectionEventID", E."ExternalDatasourceName"
FROM       "#project#"."CacheIdentificationUnit" AS U 
inner join "#project#"."CacheIdentification" AS I on U."IdentificationUnitID" = I."IdentificationUnitID"
inner join "#project#"."CacheCollectionSpecimen" AS S on U."CollectionSpecimenID" = S."CollectionSpecimenID"
left outer join "#project#"."CacheCollectionExternalDatasource" AS E on E."ExternalDatasourceID" = S."ExternalDatasourceID"
left outer join "public"."TaxonSynonymy" AS T on T."NameID" = I."NameID" and T."BaseURL" = I."BaseURL"
WHERE         exists(select I1."CollectionSpecimenID" from "#project#"."CacheIdentification" AS I1 
where I1."IdentificationUnitID" = I."IdentificationUnitID" 
group by I1."CollectionSpecimenID", I."IdentificationUnitID" having I."IdentificationSequence" = max(I1."IdentificationSequence"));

ALTER TABLE "#project#"."Observation__Identification"
OWNER TO  "CacheAdmin";
GRANT ALL ON TABLE "#project#"."Observation__Identification" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."Observation__Identification" TO "CacheUser";




--#####################################################################################################################
--######   Observation__Agent1     ####################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."Observation__Agent1" AS 
SELECT        A."CollectionSpecimenID", A."CollectorsName", DA."AgentName", A."CollectorsSequence"
FROM          "#project#"."CacheCollectionAgent" AS A
left outer join "public"."Agent" AS DA ON A."CollectorsAgentURI" = concat(DA."BaseURL", DA."AgentID")
WHERE        exists(select A1."CollectionSpecimenID" from "#project#"."CacheCollectionAgent" AS A1 
where A1."CollectionSpecimenID" = A."CollectionSpecimenID" 
group by A1."CollectionSpecimenID" having A."CollectorsSequence" = min(A1."CollectorsSequence"));

ALTER TABLE "#project#"."Observation__Agent1"
OWNER TO  "CacheAdmin";
GRANT ALL ON TABLE "#project#"."Observation__Agent1" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."Observation__Agent1" TO "CacheUser";



--#####################################################################################################################
--######   Observation__Agent...   ####################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."Observation__Agent2following" AS 
SELECT        A."CollectionSpecimenID", A."CollectorsName", A."CollectorsSequence"
FROM          "#project#"."CacheCollectionAgent" AS A, "#project#"."Observation__Agent1" AS Agent1
WHERE  A."CollectionSpecimenID" = Agent1."CollectionSpecimenID"  
and A."CollectorsSequence" > Agent1."CollectorsSequence";


CREATE OR REPLACE VIEW "#project#"."Observation__Agent2" AS 
SELECT        A."CollectionSpecimenID", A."CollectorsName", A."CollectorsSequence"
FROM          "#project#"."Observation__Agent2following" AS A
WHERE        exists(select A1."CollectionSpecimenID" from "#project#"."Observation__Agent2following" AS A1 
where A1."CollectionSpecimenID" = A."CollectionSpecimenID" 
group by A1."CollectionSpecimenID" having A."CollectorsSequence" = min(A1."CollectorsSequence"));


CREATE OR REPLACE VIEW "#project#"."Observation__Agent3following" AS 
SELECT        A."CollectionSpecimenID", A."CollectorsName", A."CollectorsSequence"
FROM          "#project#"."CacheCollectionAgent" AS A, "#project#"."Observation__Agent2" AS Agent2
WHERE  A."CollectionSpecimenID" = Agent2."CollectionSpecimenID"  
and A."CollectorsSequence" > Agent2."CollectorsSequence";


CREATE OR REPLACE VIEW "#project#"."Observation__Agent3" AS 
SELECT        A."CollectionSpecimenID", A."CollectorsName", A."CollectorsSequence"
FROM          "#project#"."Observation__Agent3following" AS A
WHERE        exists(select A1."CollectionSpecimenID" from "#project#"."Observation__Agent3following" AS A1 
where A1."CollectionSpecimenID" = A."CollectionSpecimenID" 
group by A1."CollectionSpecimenID" having A."CollectorsSequence" = min(A1."CollectorsSequence"));



--#####################################################################################################################
--######   Observation__Agent      ####################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."Observation__Agent" AS
SELECT        A1."CollectionSpecimenID", A1."CollectorsName" AS "Collector1", A2."CollectorsName" AS "Collector2", A3."CollectorsName" AS "Collector3"
FROM          "#project#"."Observation__Agent1" AS A1
left outer join "#project#"."Observation__Agent2" AS A2 ON A1."CollectionSpecimenID" = A2."CollectionSpecimenID"
left outer join "#project#"."Observation__Agent3" AS A3 ON A1."CollectionSpecimenID" = A3."CollectionSpecimenID";

ALTER TABLE "#project#"."Observation__Agent1"
OWNER TO  "CacheAdmin";
GRANT ALL ON TABLE "#project#"."Observation__Agent1" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."Observation__Agent1" TO "CacheUser";




--#####################################################################################################################
--######   Observation__Analysis   ####################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."Observation__Analysis" AS 
SELECT        A."IdentificationUnitID", A."AnalysisResult" AS "FloristischerStatus"
FROM          "#project#"."CacheIdentificationUnitAnalysis" AS A
WHERE        A."AnalysisID" = 2 
AND exists(select A1."CollectionSpecimenID" from "#project#"."CacheIdentificationUnitAnalysis" AS A1 
where A1."CollectionSpecimenID" = A."CollectionSpecimenID" 
group by A1."CollectionSpecimenID" having A."AnalysisNumber" = max(A1."AnalysisNumber"));

ALTER TABLE "#project#"."Observation__Analysis"
OWNER TO  "CacheAdmin";
GRANT ALL ON TABLE "#project#"."Observation__Analysis" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."Observation__Analysis" TO "CacheUser";



--#####################################################################################################################
--######   Observation__Part   ########################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."Observation__Part" AS 
SELECT        C."CollectionName", UP."IdentificationUnitID"
FROM          "#project#"."CacheCollectionSpecimenPart" AS P
, "#project#"."CacheCollection" AS C
, "#project#"."CacheIdentificationUnitInPart" AS UP
WHERE     P."CollectionSpecimenID" = UP."CollectionSpecimenID";

ALTER TABLE "#project#"."Observation__Part"
OWNER TO  "CacheAdmin";
GRANT ALL ON TABLE "#project#"."Observation__Part" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."Observation__Part" TO "CacheUser";



--#####################################################################################################################
--######   Observation   ##############################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."Observation" AS 
SELECT   I."TaxonomicName", I."BaseURL", I."NameID"
, I."AcceptedName", I."AcceptedNameID"
, UA."FloristischerStatus"
, E."MTB",  E."Quadrant", E."Accuracy", E."LocalityDescription", E."HabitatDescription", E."NamedArea"
, E."CollectionDay", E."CollectionMonth", E."CollectionYear"
, E."CollectionEndDay", E."CollectionEndMonth", E."CollectionEndYear"
, E."AltitudeFrom",  E."AltitudeTo"
, E."RW_GaussKrueger",  E."HW_GaussKrueger"
, E."Longitude_WGS84",  E."Latitude_WGS84"
, I."ResponsibleName"
, A."Collector1", A."Collector2", A."Collector3"
, I."ExternalDatasourceName", I."Notes"
, P."CollectionName", I."IdentificationUnitID" 
FROM          "#project#"."Observation__Event" AS E
inner join "#project#"."Observation__Identification" AS I ON E."CollectionEventID" = I."CollectionEventID"
left outer join "#project#"."Observation__Analysis" AS UA ON UA."IdentificationUnitID" = I."IdentificationUnitID"
left outer join "#project#"."Observation__Part" AS P ON P."IdentificationUnitID" = I."IdentificationUnitID"
left outer join "#project#"."Observation__Agent" AS A ON A."CollectionSpecimenID" = I."CollectionSpecimenID";


COMMENT ON VIEW "#project#"."Observation"
  IS 'Data of the observations of organisms';
COMMENT ON COLUMN "#project#"."Observation"."TaxonomicName" IS 'Taxonomic name of the organism according to the last identification';
COMMENT ON COLUMN "#project#"."Observation"."BaseURL" IS 'The link to the source of the taxonomic name';
COMMENT ON COLUMN "#project#"."Observation"."NameID" IS 'The ID of the name in the source for the taxonomic name';
COMMENT ON COLUMN "#project#"."Observation"."AcceptedName" IS 'The accepted name for the taxon according to the source of the name';
COMMENT ON COLUMN "#project#"."Observation"."AcceptedNameID" IS 'The ID of the accepted name in the source for the taxonomic name';
COMMENT ON COLUMN "#project#"."Observation"."FloristischerStatus" IS 'Floristischer status of the observed organism (according to the project BayernFlora)';
COMMENT ON COLUMN "#project#"."Observation"."MTB" IS 'Messtischblatt';
COMMENT ON COLUMN "#project#"."Observation"."Quadrant" IS 'Quadrant';
COMMENT ON COLUMN "#project#"."Observation"."Accuracy" IS 'Accuracy of the localisation';
COMMENT ON COLUMN "#project#"."Observation"."LocalityDescription" IS 'Description of the locality where the organism was found';
COMMENT ON COLUMN "#project#"."Observation"."HabitatDescription" IS 'Description of the habitat where the organism was found';
COMMENT ON COLUMN "#project#"."Observation"."NamedArea" IS 'The political area where the organism was found';
COMMENT ON COLUMN "#project#"."Observation"."RW_GaussKrueger" IS 'Rechtswert - Gauss Krueger';
COMMENT ON COLUMN "#project#"."Observation"."HW_GaussKrueger" IS 'Hochwert - Gauss Krueger';




--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 2 WHERE "Package" = 'Observation'
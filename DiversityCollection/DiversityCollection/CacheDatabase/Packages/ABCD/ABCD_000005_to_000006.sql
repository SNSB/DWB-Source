--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 6
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   abcd___projectcitation: Agentname taken from table public.Agent   ##########################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd___projectcitation(
	)
RETURNS SETOF integer 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
    ROWS 1000
AS $BODY$

begin

--Setting the role
SET ROLE "CacheAdmin";

-- Clear the table
TRUNCATE TABLE "#project#"."ABCD__ProjectCitation";

-- Insert basic
INSERT INTO "#project#"."ABCD__ProjectCitation"(
            "ProjectID", "ReferenceTitle", "Citation")
SELECT "ProjectID",
    "ReferenceTitle",
    ''
   FROM "#project#"."CacheProjectReference" R WHERE R."ReferenceType" = 'BioCASe (GFBio)' LIMIT 1;
-- insert the Authors
   DECLARE AUTHORS text = '';
	r integer;
	rMax integer = (SELECT max("AgentSequence") 
	FROM "#project#"."CacheProjectAgent" a, "#project#"."CacheProjectAgentRole" ar, public."Agent"	Agent
	WHERE ar."AgentRole" = 'Author' AND a."AgentName" <> '' AND a."AgentName" = ar."AgentName" and a."ProjectID" = ar."ProjectID" and a."AgentURI" = ar."AgentURI"
	and Agent."AgentURI" = a."AgentURI");
   BEGIN
	FOR r IN SELECT "AgentSequence" FROM "#project#"."CacheProjectAgent" a, "#project#"."CacheProjectAgentRole" ar, public."Agent"	Agent
	WHERE ar."AgentRole" = 'Author' AND a."AgentName" <> '' AND a."AgentName" = ar."AgentName" and a."ProjectID" = ar."ProjectID" 
	and a."AgentURI" = ar."AgentURI" and a."AgentURI" = Agent."AgentURI"
	ORDER BY "AgentSequence"
   LOOP
	AUTHORS := (select concat(AUTHORS, case when AUTHORS = '' then '' else case when r = rMax then ' & ' else '; ' end end, 
	case when Agent."InheritedName" <> '' then concat(Agent."InheritedName", ', ') else '' end, Agent."GivenName") 
	from  "#project#"."CacheProjectAgent" a, public."Agent"	Agent 
	where a."AgentSequence" = r and Agent."AgentURI" = a."AgentURI" limit 1);
	RETURN NEXT r;
   END LOOP; 
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = AUTHORS;
   END; 
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = 'Anonymous' WHERE "Citation" = '' OR "Citation" IS NULL;
-- Year
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", ' (', date_part('year', current_date)::character varying(4), '). '));
-- Title and [Dataset]
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", "ProjectTitle", '. [Dataset]. ') from "#project#"."CacheMetadata" WHERE "ProjectID" = "#project#".projectid());
-- Version
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", ' Version: ', date_part('year', current_date)::character varying(4), case when date_part('month', current_date) < 10 then '0' else '' end, date_part('month', current_date)::character varying(2), case when date_part('day', current_date) < 10 then '0' else '' end, date_part('day', current_date)::character varying(2)));
-- Publisher
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", '. Data Publisher: ', MIN(a."AgentName"), '.') 
	FROM "#project#"."CacheProjectAgent"  a, "#project#"."CacheProjectAgentRole" r
	WHERE r."AgentRole" = 'Publisher' AND a."AgentName" <> '' AND a."AgentName" = r."AgentName" and a."ProjectID" = r."ProjectID" and a."AgentURI" = r."AgentURI");
-- URI
   UPDATE "#project#"."ABCD__ProjectCitation" 
   SET "Citation" = concat(C."Citation", ' ', case when R."URI" <> '' then concat(R."URI", '.') else '' end) 
	FROM "#project#"."CacheProjectReference" R JOIN "#project#"."ABCD__ProjectCitation" C ON R."ProjectID" = C."ProjectID" AND R."ReferenceTitle" = C."ReferenceTitle";
end;

$BODY$;

ALTER FUNCTION "#project#".abcd___projectcitation()
    OWNER TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd___projectcitation() TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION "#project#".abcd___projectcitation() TO PUBLIC;

COMMENT ON FUNCTION "#project#".abcd___projectcitation()
    IS 'Setting the content of table ABCD__ProjectCitation according to tables CacheProjectReference, CacheProjectAgent and CacheProjectAgentRole';


--#####################################################################################################################
--######   ABCD_EFG_UnitStratigraphicDetermination_Chronostratigraphy   ###############################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_EFG_UnitStratigraphicDetermination_Chronostratigraphy"
 AS
 SELECT "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"."ID"
    , "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"."Term" AS "ChronostratigraphicName"
	--,'' AS "ChronoStratigraphicDivision"
   FROM "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm";

ALTER TABLE public."ABCD_EFG_UnitStratigraphicDetermination_Chronostratigraphy"
    OWNER TO "CacheAdmin";

GRANT ALL ON TABLE public."ABCD_EFG_UnitStratigraphicDetermination_Chronostratigraphy" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_EFG_UnitStratigraphicDetermination_Chronostratigraphy" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_EFG_UnitStratigraphicDetermination_Chronostratigraphy"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_EFG_UnitStratigraphicDetermination_Chronostratigraphy"."ChronostratigraphicName"
    IS 'ABCD EFG: /EarthScienceSpecimen/UnitStratigraphicDetermination/ChronostratigraphicAttributions/ChronostratigraphicAttribution/ChronostratigraphicName . Retrieved from column DisplayText in table CacheCollectionEventProperty for Property Chronostratigraphy';
--COMMENT ON COLUMN public."ABCD_EFG_UnitStratigraphicDetermination"."ChronoStratigraphicDivision"
--    IS 'ABCD EFG: /EarthScienceSpecimen/UnitStratigraphicDetermination/ChronostratigraphicAttributions/ChronostratigraphicAttribution/ChronostratigraphicName . Retrieved from column DisplayText in table CacheCollectionEventProperty for Property Chronostratigraphy';


--#####################################################################################################################
--######   ABCD_EFG_UnitStratigraphicDetermination_Lithostratigraphy   ###############################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_EFG_UnitStratigraphicDetermination_Lithostratigraphy"
 AS
 SELECT "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"."ID"
    , "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"."Term" AS "LithostratigraphicName"
	--,'' AS "ChronoStratigraphicDivision"
   FROM "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm";

ALTER TABLE public."ABCD_EFG_UnitStratigraphicDetermination_Lithostratigraphy"
    OWNER TO "CacheAdmin";

GRANT ALL ON TABLE public."ABCD_EFG_UnitStratigraphicDetermination_Lithostratigraphy" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_EFG_UnitStratigraphicDetermination_Lithostratigraphy" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_EFG_UnitStratigraphicDetermination_Lithostratigraphy"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_EFG_UnitStratigraphicDetermination_Lithostratigraphy"."LithostratigraphicName"
    IS 'ABCD EFG: /EarthScienceSpecimen/UnitStratigraphicDetermination/LithostratigraphicAttributions/LithostratigraphicAttribution/LithostratigraphicName . Retrieved from column DisplayText in table CacheCollectionEventProperty for Property Lithostratigraphy';
--COMMENT ON COLUMN public."ABCD_EFG_UnitStratigraphicDetermination"."ChronoStratigraphicDivision"
--    IS 'ABCD EFG: /EarthScienceSpecimen/UnitStratigraphicDetermination/ChronostratigraphicAttributions/ChronostratigraphicAttribution/ChronostratigraphicName . Retrieved from column DisplayText in table CacheCollectionEventProperty for Property Chronostratigraphy';


--#####################################################################################################################
--######   ABCD_Unit_Identification_Extension_EFG   ###################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Unit_Identification_Extension_EFG"
 AS
 SELECT "ABCD_Unit"."ID",
    "ABCD_Unit"."CollectionSpecimenID",
    "ABCD_Unit"."IdentificationUnitID",
    "ABCD_Unit"."Identification_Taxon_ScientificName_FullScientificName" AS "MineralRockIdentified_FullScientificNameString"
   FROM "#project#"."ABCD_Unit"
   WHERE "ABCD_Unit"."Identification_Taxon_HigherTaxonName" = 'MineralRock';

ALTER TABLE public."ABCD_Unit_Identification_Extension_EFG"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Unit_Identification_Extension_EFG"
    IS 'ABCD entity /DataSets/DataSet/Units/Unit/';

GRANT ALL ON TABLE public."ABCD_Unit_Identification_Extension_EFG" TO "CacheAdmin";
GRANT SELECT ON TABLE public."ABCD_Unit_Identification_Extension_EFG" TO "CacheUser";

COMMENT ON COLUMN public."ABCD_Unit_Identification_Extension_EFG"."ID"
    IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';

COMMENT ON COLUMN public."ABCD_Unit_Identification_Extension_EFG"."CollectionSpecimenID"
    IS 'CollectionSpecimenID of the specimen';

COMMENT ON COLUMN public."ABCD_Unit_Identification_Extension_EFG"."IdentificationUnitID"
    IS 'IdentificationUnitID of the unit';

COMMENT ON COLUMN public."ABCD_Unit_Identification_Extension_EFG"."MineralRockIdentified_FullScientificNameString"
    IS 'Corresponds to ABCD_EFG .../MineralRockIdentified/ClassifiedName/FullScientificNameString';


--#####################################################################################################################
--######   ABCD_Metadata - restriction to ProjectID   #################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_Metadata"
 AS
 SELECT m."ProjectID",
    m."DatasetDetails" AS "Description_Representation_Details",
    m."DatasetTitle" AS "Description_Representation_Title",
    m."DatasetURI" AS "Description_Representation_URI",
    m."DateModified" AS "RevisionData_DateModified",
    m."OwnerEmail" AS "Owner_EmailAddress",
    m."OwnerLogoURI" AS "Owner_LogoURI",
    m."OwnerContactPerson" AS "Owner_Person_FullName",
    m."OwnerContactRole" AS "Owner_Role",
    m."OwnerURI" AS "Owner_URL",
    m."CopyrightURI" AS "IPRStatements_Copyright_URI",
    m."DisclaimersText" AS "IPRStatements_Disclaimer_Text",
    m."DisclaimersURI" AS "IPRStatements_Disclaimer_URI",
    m."LicenseText" AS "IPRStatements_License_Text",
    m."LicenseURI" AS "IPRStatements_License_URI",
    'en'::character varying(2) AS "IPRStatements_License_Language",
    m."TermsOfUseText" AS "IPRStatements_TermsOfUse_Text",
    m."OwnerOrganizationName" AS "Owner_Organisation_Name_Text",
    m."OwnerOrganizationAbbrev" AS "Owner_Organisation_Name_Abbreviation",
    m."OwnerAddress" AS "Owner_Address",
    m."OwnerTelephone" AS "Owner_Telephone_Number",
    m."IPRText" AS "IPRStatements_IPRDeclaration_Text",
    m."IPRDetails" AS "IPRStatements_IPRDeclaration_Details",
    m."IPRURI" AS "IPRStatements_IPRDeclaration_URI",
    m."CopyrightText" AS "IPRStatements_Copyright_Text",
    m."CopyrightDetails" AS "IPRStatements_Copyright_Details",
    m."LicensesDetails" AS "IPRStatements_License_Details",
    m."TermsOfUseDetails" AS "IPRStatements_TermsOfUse_Details",
    m."TermsOfUseURI" AS "IPRStatements_TermsOfUse_URI",
    m."DisclaimersDetails" AS "IPRStatements_Disclaimer_Details",
    m."AcknowledgementsText" AS "IPRStatements_Acknowledgement_Text",
    m."AcknowledgementsDetails" AS "IPRStatements_Acknowledgement_Details",
    m."AcknowledgementsURI" AS "IPRStatements_Acknowledgement_URI",
        CASE
            WHEN r."Citation" <> ''::text THEN r."Citation"::character varying
            ELSE m."CitationsText"
        END::text AS "IPRStatements_Citation_Text",
    m."CitationsDetails" AS "IPRStatements_Citation_Details",
    m."CitationsURI" AS "IPRStatements_Citation_URI",
    m."StableIdentifier" AS "DatasetGUID"
   FROM "Project_Tutorial"."CacheMetadata" m
     LEFT JOIN "Project_Tutorial"."ABCD__ProjectCitation" r ON m."ProjectID" = r."ProjectID" 
	 WHERE m."ProjectID" = "Project_Tutorial".projectid();

ALTER TABLE public."ABCD_Metadata"
    OWNER TO "CacheAdmin";
COMMENT ON VIEW public."ABCD_Metadata"
    IS 'ABCD entity /DataSets/DataSet/Metadata';

GRANT SELECT ON TABLE public."ABCD_Metadata" TO "CacheUser";
GRANT ALL ON TABLE public."ABCD_Metadata" TO "CacheAdmin";

COMMENT ON COLUMN public."ABCD_Metadata"."ProjectID"
    IS 'ID of the project retrieved from DiversityProjects';

COMMENT ON COLUMN public."ABCD_Metadata"."Description_Representation_Details"
    IS 'ABCD: Dataset/Metadata/Description/Representation/Details. Retrieved from DiversityProjects - Project - PublicDescription';

COMMENT ON COLUMN public."ABCD_Metadata"."Description_Representation_Title"
    IS 'ABCD: Dataset/Metadata/Description/Representation/Title. Retrieved from DiversityProjects - Settings - ABCD - Dataset - Title';

COMMENT ON COLUMN public."ABCD_Metadata"."Description_Representation_URI"
    IS 'ABCD: Dataset/Metadata/Description/Representation/URI. Retrieved from DiversityProjects - Settings - ABCD - Dataset - URI';

COMMENT ON COLUMN public."ABCD_Metadata"."RevisionData_DateModified"
    IS 'ABCD: Dataset/Metadata/RevisionData/DateModified. Retrieved from the first level SQL-Server cache database for DiversityCollection corresponding to the date and time of the last transfer into the cache database (ProjectPublished.LastUpdatedWhen)';

COMMENT ON COLUMN public."ABCD_Metadata"."Owner_EmailAddress"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/EmailAddress. Retrieved from DiversityProjects - Settings - ABCD - Owner - Email';

COMMENT ON COLUMN public."ABCD_Metadata"."Owner_LogoURI"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/LogoURI. Retrieved from DiversityProjects - Settings - ABCD - Owner - LogoURI';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Copyright_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/URI. Retrieved from DiversityProjects - Settings - ABCD - Copyright - URI';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Disclaimer_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/Text. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Text';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Disclaimer_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/URI. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - URI';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_License_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/Text. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Text';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_License_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/URI. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - URI';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_License_Language"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/URI. Defined in view = en';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_TermsOfUse_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/Text. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - Text';

COMMENT ON COLUMN public."ABCD_Metadata"."Owner_Organisation_Name_Text"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/Organisation/Name/Representation/Text. Retrieved from DiversityProjects - Settings - ABCD - Owner - OrganisationName';

COMMENT ON COLUMN public."ABCD_Metadata"."Owner_Organisation_Name_Abbreviation"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/Organisation/Name/Representation/Abbreviation. Retrieved from DiversityProjects - Settings - ABCD - Owner - OrganisationAbbrev';

COMMENT ON COLUMN public."ABCD_Metadata"."Owner_Address"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/Addresses/Address. Retrieved from DiversityProjects - Settings - ABCD - Owner - Address';

COMMENT ON COLUMN public."ABCD_Metadata"."Owner_Telephone_Number"
    IS 'ABCD: Dataset/Metadata/Owners/Owner/TelephoneNumbers/TelephoneNumber/Number. Retrieved from DiversityProjects - Settings - ABCD - Owner - Telephone';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_IPRDeclaration_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/Text. Retrieved from DiversityProjects - Settings - ABCD - IPR - Text';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_IPRDeclaration_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/Details. Retrieved from DiversityProjects - Settings - ABCD - IPR - Details';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_IPRDeclaration_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/URI. Retrieved from DiversityProjects - Settings - ABCD - IPR - URI';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Copyright_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/Text. Retrieved from DiversityProjects - Settings - ABCD - Copyright - Text';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Copyright_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/Details. Retrieved from DiversityProjects - Settings - ABCD - Copyright - Details';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_License_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/Details. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Details';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_TermsOfUse_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/Details. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - Details';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_TermsOfUse_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/URI. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - URI';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Disclaimer_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/Details. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Details';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Acknowledgement_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/Text. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - Text';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Acknowledgement_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/Details. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - Details';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Acknowledgement_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/URI. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - URI';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Citation_Text"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/Text. Retrieved from DiversityProjects: 
First dataset in table ProjectReference where type = ''BioCASe (GFBio)''. 
Authors from table ProjectAgent with type = ''Author'' according to their sequence. If none are found ''Anonymous''. 
Current year. Content of column ProjectTitle in table Project. 
Marker ''[Dataset]''. 
''Version: '' + date of transfer into the ABCD tables as year + month + day: yyyymmdd.
If available publishers: ''Data Publisher: '' + Agents with role ''Publisher'' from table ProjectAgent. URI from table ProjectReference if present. 
If entry in table ProjectReference is missing taken from Settings - ABCD - Citations - Text';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Citation_Details"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/Details. Retrieved from DiversityProjects - Settings - ABCD - Citations - Details';

COMMENT ON COLUMN public."ABCD_Metadata"."IPRStatements_Citation_URI"
    IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/URI. Retrieved from DiversityProjects - Settings - ABCD - Citations - URI';

COMMENT ON COLUMN public."ABCD_Metadata"."DatasetGUID"
    IS 'ABCD: Dataset/DatasetGUID. Retrieved from DiversityProjects - StableIdentifier (= basic address for stable identifiers + ID of the project)';


--#####################################################################################################################
--######   ABCD_MetaData   ############################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW public."ABCD_MetaData"
 AS
 SELECT *
   FROM "ABCD_Metadata" AS m;

ALTER TABLE public."ABCD_MetaData"
    OWNER TO "CacheAdmin";

COMMENT ON VIEW public."ABCD_MetaData"
    IS 'View on original ABCD_Metadata due to case sensitiv naming';

GRANT SELECT ON TABLE public."ABCD_MetaData" TO "CacheUser";
GRANT ALL ON TABLE public."ABCD_MetaData" TO "CacheAdmin";

--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 6 WHERE "Package" = 'ABCD'
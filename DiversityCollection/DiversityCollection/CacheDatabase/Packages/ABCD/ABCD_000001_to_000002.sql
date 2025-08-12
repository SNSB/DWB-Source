--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 2
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   SETTING THE ROLE   #########################################################################################
--#####################################################################################################################

GRANT ALL ON SCHEMA "#project#" TO "CacheAdmin";
SET ROLE "CacheAdmin";

--#####################################################################################################################
--######   abcd__latitude   ###########################################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__latitude(integer)
  RETURNS numeric AS
$BODY$
declare Latitude decimal;
declare LocalisationSystemID int;

begin

--Setting the role
SET ROLE "CacheAdmin";

select L."LocalisationSystemID" 
from "#project#"."CacheLocalisationSystem" L, "#project#"."CacheCollectionEventLocalisation" AS E  
where L."LocalisationSystemID" = E."LocalisationSystemID"  AND NOT E."LocalisationSystemID" IS NULL
and E."CollectionEventID" = $1
order by L."Sequence" LIMIT 1 
into LocalisationSystemID;

select E."AverageLatitudeCache" 
from "#project#"."CacheCollectionEventLocalisation" AS E 
where E."LocalisationSystemID" = LocalisationSystemID and E."CollectionEventID" = $1 LIMIT 1
into Latitude;

return Latitude;
END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
  
ALTER FUNCTION "#project#".abcd__latitude(integer)
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__latitude(integer) TO public;
GRANT EXECUTE ON FUNCTION "#project#".abcd__latitude(integer) TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__latitude(integer) TO "CacheUser";
COMMENT ON FUNCTION "#project#".abcd__latitude(integer) IS 'Retrieval of latitude according to available data and sequence of localisation systems in table CacheLocalisationSystem';




--#####################################################################################################################
--######   abcd__longitude   ##########################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__longitude(integer)
  RETURNS numeric AS
$BODY$
declare Longitude decimal;
declare LocalisationSystemID int;

begin

--Setting the role
SET ROLE "CacheAdmin";

select L."LocalisationSystemID" 
from "#project#"."CacheLocalisationSystem" L, "#project#"."CacheCollectionEventLocalisation" AS E  
where L."LocalisationSystemID" = E."LocalisationSystemID"  AND NOT E."LocalisationSystemID" IS NULL
and E."CollectionEventID" = $1
order by L."Sequence" LIMIT 1 
into LocalisationSystemID;

select E."AverageLongitudeCache" 
from "#project#"."CacheCollectionEventLocalisation" AS E 
where E."LocalisationSystemID" = LocalisationSystemID and E."CollectionEventID" = $1 LIMIT 1
into Longitude;

return Longitude;
END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__longitude(integer)
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__longitude(integer) TO public;
GRANT EXECUTE ON FUNCTION "#project#".abcd__longitude(integer) TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__longitude(integer) TO "CacheUser";
COMMENT ON FUNCTION "#project#".abcd__longitude(integer) IS 'Retrieval of longitude according to available data and sequence of localisation systems in table CacheLocalisationSystem';




--#####################################################################################################################
--######   ABCD__ProjectCitation   ###################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD__ProjectCitation"
(
  "ProjectID" integer NOT NULL,
  "ReferenceTitle" character varying(255) NOT NULL,
  "Citation" text,
  CONSTRAINT "ABCD__ProjectCitation_pkey" PRIMARY KEY ("ProjectID", "ReferenceTitle")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "#project#"."ABCD__ProjectCitation"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__ProjectCitation" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__ProjectCitation" TO "CacheUser";


--#####################################################################################################################
--######   function abcd___ProjectCitation  ###########################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd___ProjectCitation()
  RETURNS SETOF integer AS
$BODY$
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
   DECLARE AUTHORS character varying(255) = '';
	r integer;
	rMax integer = (SELECT max("AgentSequence") FROM "#project#"."CacheProjectAgent" a
	WHERE a."AgentRole" = 'Author' AND a."AgentName" <> '');
   BEGIN
	FOR r IN SELECT "AgentSequence" FROM "#project#"."CacheProjectAgent" a
	WHERE a."AgentRole" = 'Author' AND a."AgentName" <> '' ORDER BY "AgentSequence"
   LOOP
	AUTHORS := (select concat(AUTHORS, case when AUTHORS = '' then '' else case when r = rMax then ' & ' else '; ' end end, a."AgentName") from  "#project#"."CacheProjectAgent" a where a."AgentSequence" = r);
	RETURN NEXT r;
   END LOOP; 
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = AUTHORS;
   END; 
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = 'Anonymous' WHERE "Citation" = '' OR "Citation" IS NULL;
-- Year
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", ' (', date_part('year', current_date)::character varying(4), '). '));
-- Title and [Dataset]
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", "ProjectTitle", '. [Dataset]') from "#project#"."CacheMetadata" WHERE "ProjectID" = "#project#".projectid());
-- Publisher
   UPDATE "#project#"."ABCD__ProjectCitation" SET "Citation" = (select concat("Citation", '. Data Publisher: ', MIN("AgentName"), '.') 
	from "#project#"."CacheProjectAgent"  a
	WHERE a."AgentRole" = 'Publisher' AND a."AgentName" <> '');
-- URI
   UPDATE "#project#"."ABCD__ProjectCitation" 
   SET "Citation" = concat(C."Citation", ' ', case when R."URI" <> '' then concat(R."URI", '.') else '' end) 
	FROM "#project#"."CacheProjectReference" R JOIN "#project#"."ABCD__ProjectCitation" C ON R."ProjectID" = C."ProjectID" AND R."ReferenceTitle" = C."ReferenceTitle";
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd___ProjectCitation()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd___ProjectCitation() TO "CacheAdmin";


--#####################################################################################################################
--######   insert data into ABCD__ProjectCitation   ##################################################################
--#####################################################################################################################

--SELECT "#project#".abcd___ProjectCitation();


--#####################################################################################################################
--######   ABCD_Metadata   ############################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_Metadata" AS 
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
   FROM "#project#"."CacheMetadata" m
     LEFT JOIN "#project#"."ABCD__ProjectCitation" r ON m."ProjectID" = r."ProjectID" 
	 WHERE m."ProjectID" = "#project#".projectid();

ALTER TABLE "ABCD_Metadata"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_Metadata" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_Metadata" TO "CacheUser";

COMMENT ON VIEW "ABCD_Metadata" IS 'ABCD entity /DataSets/DataSet/Metadata';
COMMENT ON Column "ABCD_Metadata"."ProjectID" IS 'ID of the project retrieved from DiversityProjects';
COMMENT ON Column "ABCD_Metadata"."Description_Representation_Details" IS 'ABCD: Dataset/Metadata/Description/Representation/Details. Retrieved from DiversityProjects - Settings - ABCD - Dataset - Details';
COMMENT ON Column "ABCD_Metadata"."Description_Representation_Title" IS 'ABCD: Dataset/Metadata/Description/Representation/Title. Retrieved from DiversityProjects - Settings - ABCD - Dataset - Title';
COMMENT ON Column "ABCD_Metadata"."Description_Representation_URI" IS 'ABCD: Dataset/Metadata/Description/Representation/URI. Retrieved from DiversityProjects - Settings - ABCD - Dataset - URI';
COMMENT ON Column "ABCD_Metadata"."RevisionData_DateModified" IS 'ABCD: Dataset/Metadata/RevisionData/DateModified. Retrieved from the cache database for DiversityCollection corresponding to the date and time of the last transfer into the cache database (ProjectPublished.LastUpdatedWhen)';
COMMENT ON Column "ABCD_Metadata"."Owner_EmailAddress" IS 'ABCD: Dataset/Metadata/Owners/Owner/EmailAddress. Retrieved from DiversityProjects - Settings - ABCD - Owner - Email';
COMMENT ON Column "ABCD_Metadata"."Owner_LogoURI" IS 'ABCD: Dataset/Metadata/Owners/Owner/LogoURI. Retrieved from DiversityProjects - Settings - ABCD - Owner - LogoURI';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Copyright_URI" IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/URI. Retrieved from DiversityProjects - Settings - ABCD - Copyright - URI';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Disclaimer_Text" IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/Text. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Text';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Disclaimer_URI" IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/URI. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - URI';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_License_Text" IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/Text. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Text';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_License_URI" IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/URI. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - URI';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_License_Language" IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/URI. Defined in view = en';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_TermsOfUse_Text" IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/Text. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - Text';
COMMENT ON Column "ABCD_Metadata"."Owner_Organisation_Name_Text" IS 'ABCD: Dataset/Metadata/Owners/Owner/Organisation/Name/Representation/Text. Retrieved from DiversityProjects - Settings - ABCD - Owner - OrganisationName';
COMMENT ON Column "ABCD_Metadata"."Owner_Organisation_Name_Abbreviation" IS 'ABCD: Dataset/Metadata/Owners/Owner/Organisation/Name/Representation/Abbreviation. Retrieved from DiversityProjects - Settings - ABCD - Owner - OrganisationAbbrev';
COMMENT ON Column "ABCD_Metadata"."Owner_Address" IS 'ABCD: Dataset/Metadata/Owners/Owner/Addresses/Address. Retrieved from DiversityProjects - Settings - ABCD - Owner - Address';
COMMENT ON Column "ABCD_Metadata"."Owner_Telephone_Number" IS 'ABCD: Dataset/Metadata/Owners/Owner/TelephoneNumbers/TelephoneNumber/Number. Retrieved from DiversityProjects - Settings - ABCD - Owner - Telephone';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_IPRDeclaration_Text" IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/Text. Retrieved from DiversityProjects - Settings - ABCD - IPR - Text';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_IPRDeclaration_Details" IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/Details. Retrieved from DiversityProjects - Settings - ABCD - IPR - Details';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_IPRDeclaration_URI" IS 'ABCD: Dataset/Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/URI. Retrieved from DiversityProjects - Settings - ABCD - IPR - URI';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Copyright_Text" IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/Text. Retrieved from DiversityProjects - Settings - ABCD - Copyright - Text';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Copyright_Details" IS 'ABCD: Dataset/Metadata/IPRStatements/Copyrights/Copyright/Details. Retrieved from DiversityProjects - Settings - ABCD - Copyright - Details';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_License_Details" IS 'ABCD: Dataset/Metadata/IPRStatements/Licenses/License/Details. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Details';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_TermsOfUse_Details" IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/Details. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - Details';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_TermsOfUse_URI" IS 'ABCD: Dataset/Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/URI. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - URI';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Disclaimer_Details" IS 'ABCD: Dataset/Metadata/IPRStatements/Disclaimers/Disclaimer/Details. Retrieved from DiversityProjects - Settings - ABCD - Disclaimers - Details';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Acknowledgement_Text" IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/Text. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - Text';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Acknowledgement_Details" IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/Details. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - Details';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Acknowledgement_URI" IS 'ABCD: Dataset/Metadata/IPRStatements/Acknowledgements/Acknowledgement/URI. Retrieved from DiversityProjects - Settings - ABCD - Acknowledgements - URI';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Citation_Text" IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/Text. Retrieved from DiversityProjects: 
First dataset in table ProjectReference where type = ''BioCASe (GFBio)''. Authors from table ProjectAgent with type = ''Author'' according to their sequence. If none are found ''Anonymous''. 
Current year. Content of column ProjectTitle in table Project. Marker [Dataset]. 
If available publishers: ''Data Publisher: '' + Agents with role ''Publisher'' from table ProjectAgent. URI from table ProjectReference if present. 
If entry in table ProjectReference is missing taken from Settings - ABCD - Citations - Text';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Citation_Details" IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/Details. Retrieved from DiversityProjects - Settings - ABCD - Citations - Details';
COMMENT ON Column "ABCD_Metadata"."IPRStatements_Citation_URI" IS 'ABCD: Dataset/Metadata/IPRStatements/Citations/Citation/URI. Retrieved from DiversityProjects - Settings - ABCD - Citations - URI';
COMMENT ON Column "ABCD_Metadata"."DatasetGUID" IS 'ABCD: Dataset/DatasetGUID. Retrieved from DiversityProjects - StableIdentifier (= basic address for stable identifiers + ID of the project)';


--#####################################################################################################################
--######   ABCD_TechnicalContact   ####################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_TechnicalContact" AS 
 SELECT m."ProjectID",
    m."TechnicalContactEmail" AS "Email",
    m."TechnicalContactName" AS "Name",
    m."TechnicalContactPhone" AS "Phone",
    m."TechnicalContactAddress" AS "Address"
   FROM "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

ALTER TABLE "ABCD_TechnicalContact"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_TechnicalContact" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_TechnicalContact" TO "CacheUser";
COMMENT ON VIEW "ABCD_TechnicalContact" IS 'ABCD entity /DataSets/DataSet/TechnicalContacts/TechnicalContact';
COMMENT ON COLUMN "ABCD_TechnicalContact"."ProjectID" IS 'ID of the project. Retrieved from DiversityProjects. PK of table Project';
COMMENT ON COLUMN "ABCD_TechnicalContact"."Email" IS 'The email address of the agent. Retrieved from DiversityProjects - Settings - ABCD - TechnicalContact - Email';
COMMENT ON COLUMN "ABCD_TechnicalContact"."Name" IS 'Person, person team, or role name (e. g., ''head of department''). Retrieved from DiversityProjects - Settings - ABCD - TechnicalContact - Name';
COMMENT ON COLUMN "ABCD_TechnicalContact"."Phone" IS 'Phone number of the agent. Retrieved from DiversityProjects - Settings - ABCD - TechnicalContact - Phone';
COMMENT ON COLUMN "ABCD_TechnicalContact"."Address" IS 'Address of the agent. Retrieved from DiversityProjects - Settings - ABCD - TechnicalContact - Address';

--#####################################################################################################################
--######   ABCD_ContentContact   ######################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_ContentContact" AS 
 SELECT m."ProjectID",
    m."ContentContactEmail" AS "Email",
    m."ContentContactName" AS "Name",
    m."ContentContactPhone" AS "Phone",
    m."ContentContactAddress" AS "Address"
   FROM "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

ALTER TABLE "ABCD_ContentContact"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_ContentContact" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_ContentContact" TO "CacheUser";
COMMENT ON VIEW "ABCD_ContentContact" IS 'ABCD entity /DataSets/DataSet/ContentContacts/ContentContact';
COMMENT ON COLUMN "ABCD_ContentContact"."ProjectID" IS 'ID of the project. Retrieved from DiversityProjects. PK of table Project';
COMMENT ON COLUMN "ABCD_ContentContact"."Email" IS 'The email address of the agent. Retrieved from DiversityProjects - Settings - ABCD - ContentContact - Email';
COMMENT ON COLUMN "ABCD_ContentContact"."Name" IS 'Person, person team, or role name (e. g., ''head of department''). Retrieved from DiversityProjects - Settings - ABCD - ContentContact - Name';
COMMENT ON COLUMN "ABCD_ContentContact"."Phone" IS 'Phone number of the agent. Retrieved from DiversityProjects - Settings - ABCD - ContentContact - Phone';
COMMENT ON COLUMN "ABCD_ContentContact"."Address" IS 'Address of the agent. Retrieved from DiversityProjects - Settings - ABCD - ContentContact - Address';


--#####################################################################################################################
--######   ABCD_DatasetGUID   #########################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_DatasetGUID" AS 
 SELECT m."ProjectID",
    m."StableIdentifier" AS "DatasetGUID"
   FROM "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

ALTER TABLE "ABCD_DatasetGUID"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_DatasetGUID" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_DatasetGUID" TO "CacheUser";
COMMENT ON VIEW "ABCD_DatasetGUID" IS 'ABCD entity /DataSets/DataSet/DatasetGUID';
COMMENT ON COLUMN "ABCD_DatasetGUID"."ProjectID" IS 'ID of the project. Retrieved from DiversityProjects. PK of table Project';
COMMENT ON Column "ABCD_DatasetGUID"."DatasetGUID" IS 'ABCD: Dataset/DatasetGUID. Retrieved from DiversityProjects - StableIdentifier (= basic address for stable identifiers + ID of the project)';


--#####################################################################################################################
--######   ABCD__RecordBasis_NoPart   #################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__RecordBasis_NoPart" AS 
 SELECT up."SpecimenPartID",
    u."IdentificationUnitID",
	CASE WHEN u."RetrievalType" IS NULL THEN 'HumanObservation'
	ELSE
		CASE
		    WHEN u."RetrievalType"::text ~~ '% %'::text THEN concat(upper(btrim("substring"(u."RetrievalType"::text, 1, 1))), btrim("substring"(u."RetrievalType"::text, 2, "position"(u."RetrievalType"::text, ' '::text) - 1)), upper(btrim("substring"(u."RetrievalType"::text, "position"(u."RetrievalType"::text, ' '::text) + 1, 1))), btrim("substring"(u."RetrievalType"::text, "position"(u."RetrievalType"::text, ' '::text) + 2)))
		    ELSE concat(upper(btrim("substring"(u."RetrievalType"::text, 1, 1))), btrim("substring"(u."RetrievalType"::text, 2)))
		END 
        END AS "RecordBasis",
    'no treatment'::character varying(50) AS "PreparationType",
    NULL::character varying(50) AS "MaterialCategory"
   FROM "#project#"."CacheIdentificationUnit" u
     LEFT JOIN "#project#"."CacheIdentificationUnitInPart" up ON up."CollectionSpecimenID" = u."CollectionSpecimenID" and up."IdentificationUnitID" = u."IdentificationUnitID"
  WHERE up."IdentificationUnitID" IS NULL;

ALTER TABLE "#project#"."ABCD__RecordBasis_NoPart"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__RecordBasis_NoPart" TO postgres;
GRANT SELECT ON TABLE "#project#"."ABCD__RecordBasis_NoPart" TO "CacheUser";
GRANT ALL ON TABLE "#project#"."ABCD__RecordBasis_NoPart" TO "CacheAdmin";



--#####################################################################################################################
--######   ABCD__RecordBasis   ########################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__RecordBasis" AS 
 SELECT up."SpecimenPartID",
    up."IdentificationUnitID",
	CASE
	WHEN p."MaterialCategory"::text = 'icones'::text OR p."MaterialCategory"::text ~~ 'drawing%'::text OR p."MaterialCategory"::text ~~ 'photogr. %'::text 
	THEN 'DrawingOrPhotograph'::text
	ELSE
	    CASE
	    WHEN p."MaterialCategory"::text = 'cultures'::text OR p."MaterialCategory"::text = 'living specimen'::text 
	    THEN 'LivingSpecimen'::text
	    ELSE
		CASE
		WHEN p."MaterialCategory"::text = 'medium'::text 
		THEN 'MultimediaObject'::text
		ELSE
		    CASE
		    WHEN p."MaterialCategory"::text = 'other specimen'::text 
		    THEN 'OtherSpecimen'::text
		    ELSE
			CASE
			WHEN p."MaterialCategory"::text = 'machine observation'::text 
			THEN 'MachineObservation'::text
			ELSE
			    CASE
			    WHEN p."MaterialCategory"::text = 'trace'::text OR p."MaterialCategory"::text = 'mould'::text OR p."MaterialCategory"::text = 'sound'::text OR p."MaterialCategory"::text = 'observation'::text 
				OR p."MaterialCategory"::text = 'human observation'::text 
			    THEN 'HumanObservation'::text
			    ELSE
				CASE
				WHEN p."MaterialCategory"::text ~~ 'fossil %'::text OR p."MaterialCategory"::text ~~ '% fossil'::text 
				THEN 'FossilSpecimen'::text
				ELSE 
				    CASE
				    WHEN p."MaterialCategory"::text = 'earth science specimen'::text
				    THEN 'EarthScienceSpecimen'::text
				    ELSE
					CASE
					WHEN p."MaterialCategory"::text = 'mineral specimen'::text
					THEN 'MineralSpecimen'::text
					ELSE
					    CASE
					    WHEN p."MaterialCategory"::text = 'material sample'::text
					    THEN 'MaterialSample'::text
					    ELSE 'PreservedSpecimen'::text
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
        AS "RecordBasis",
	CASE
	    WHEN p."MaterialCategory"::text = 'micr. slide'::text OR p."MaterialCategory"::text = 'SEM table'::text 
	    THEN 'microscopic preparation'::text
	    ELSE
		CASE
		    WHEN p."MaterialCategory"::text = 'herbarium sheets'::text THEN 'dried and pressed'::text
		    ELSE
		    CASE
			WHEN p."MaterialCategory"::text = 'trace'::text OR p."MaterialCategory"::text = 'vial'::text THEN 'no treatment'::text
			ELSE 'PreservedSpecimen'::text
		    END
		END
        END 
        AS "PreparationType",
    p."MaterialCategory"
   FROM "#project#"."CacheIdentificationUnitInPart" up,
    "#project#"."CacheCollectionSpecimenPart" p
  WHERE up."SpecimenPartID" = p."SpecimenPartID";

ALTER TABLE "#project#"."ABCD__RecordBasis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__RecordBasis" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__RecordBasis" TO "CacheUser";
COMMENT ON VIEW "#project#"."ABCD__RecordBasis"   IS 'ABCD auxillary view - translation of material categories within the DBW into RecordBasis and PreparationType in ABCD';
COMMENT ON COLUMN "#project#"."ABCD__RecordBasis"."SpecimenPartID" IS 'Unique key to the table CollectionSpecimenSpecimenPart';
COMMENT ON COLUMN "#project#"."ABCD__RecordBasis"."RecordBasis" IS 'translation of the material category into RecordBasis';
COMMENT ON COLUMN "#project#"."ABCD__RecordBasis"."PreparationType" IS 'translation of the material category into PreparationType';


--#####################################################################################################################
--######   ABCD__Kingdom   ############################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__Kingdom" AS 
 SELECT "CacheIdentificationUnit"."IdentificationUnitID",
        CASE
            WHEN "CacheIdentificationUnit"."TaxonomicGroup"::text = 'mollusc'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'animal'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'arthropod'::text 
		OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'insect'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'echinoderm'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'vertebrate'::text 
		OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'fish'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'amphibian'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'reptile'::text 
		OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'mammal'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'cnidaria'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'evertebrate'::text 
		OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'bird'::text 
	    THEN 'Animalia'::text
            ELSE
            CASE
                WHEN "CacheIdentificationUnit"."TaxonomicGroup"::text = 'alga'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'bryophyte'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'plant'::text 
                OR "CacheIdentificationUnit"."TaxonomicGroup"::text = ''::text 
                THEN 'Plantae'::text
                ELSE
                CASE
                    WHEN "CacheIdentificationUnit"."TaxonomicGroup"::text = 'bacterium'::text 
                    THEN 'Bacteria'::text
                    ELSE
                    CASE
                        WHEN "CacheIdentificationUnit"."TaxonomicGroup"::text = 'virus'::text 
                        THEN 'Viruses'::text
                        ELSE
                        CASE
                            WHEN "CacheIdentificationUnit"."TaxonomicGroup"::text = 'fungus'::text OR "CacheIdentificationUnit"."TaxonomicGroup"::text = 'lichen'::text 
                            THEN 'Fungi'::text
                            ELSE
                            CASE
                                WHEN "CacheIdentificationUnit"."TaxonomicGroup"::text = 'myxomycete'::text 
                                THEN 'Protozoa'::text
                                ELSE
                                CASE
                                    WHEN "CacheIdentificationUnit"."TaxonomicGroup"::text = 'chromista'::text 
                                    THEN 'Chromista'::text
                                    ELSE
                                    CASE
				    WHEN "CacheIdentificationUnit"."TaxonomicGroup"::text = 'archaea'::text 
				    THEN 'Archaea'::text
				    ELSE 'incertae sedis'::text
                                    END
                                END
                            END
                        END
                    END
                END
            END
        END AS "Kingdom"
   FROM "#project#"."CacheIdentificationUnit";

ALTER TABLE "#project#"."ABCD__Kingdom"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__Kingdom" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__Kingdom" TO "CacheUser";
COMMENT ON VIEW "#project#"."ABCD__Kingdom" IS 'ABCD auxillary view - translation of taxonomic groups within the DBW into kingdoms in ABCD';
COMMENT ON COLUMN "#project#"."ABCD__Kingdom"."IdentificationUnitID" IS 'Unique key to the table IdentificationUnit';
COMMENT ON COLUMN "#project#"."ABCD__Kingdom"."Kingdom" IS 'translation of taxonomic group into kingdom';


--#####################################################################################################################
--######   ABCD__1_Unit_Type_1   ######################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__1_Unit_Type_1" AS 
 SELECT i."IdentificationUnitID",
    min(i."IdentificationSequence") AS "IdentificationSequence"
   FROM "#project#"."CacheIdentification" i
  WHERE i."TypeStatus"::text <> ''::text
  GROUP BY i."IdentificationUnitID";

ALTER TABLE "#project#"."ABCD__1_Unit_Type_1"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__1_Unit_Type_1" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__1_Unit_Type_1" TO "CacheUser";
COMMENT ON VIEW "#project#"."ABCD__1_Unit_Type_1" IS 'ABCD auxillary view - search for first Type within an identification unit';
COMMENT ON COLUMN "#project#"."ABCD__1_Unit_Type_1"."IdentificationUnitID" IS 'Unique key to the table IdentificationUnit';
COMMENT ON COLUMN "#project#"."ABCD__1_Unit_Type_1"."IdentificationSequence" IS 'IdentificationSequence of first type';


--#####################################################################################################################
--######   ABCD__2_Unit_Type   ########################################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__2_Unit_Type" AS 
 SELECT i."TaxonomicName",
    i."TypeStatus",
    i."IdentificationUnitID"
   FROM "#project#"."ABCD__1_Unit_Type_1" t1,
    "#project#"."CacheIdentification" i
  WHERE i."IdentificationUnitID" = t1."IdentificationUnitID" AND i."IdentificationSequence" = t1."IdentificationSequence";

ALTER TABLE "#project#"."ABCD__2_Unit_Type"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__2_Unit_Type" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__2_Unit_Type" TO "CacheUser";
COMMENT ON VIEW "#project#"."ABCD__2_Unit_Type" IS 'ABCD auxillary view - search type within an identification unit';
COMMENT ON COLUMN "#project#"."ABCD__2_Unit_Type"."TaxonomicName" IS 'Taxonomic name of type';
COMMENT ON COLUMN "#project#"."ABCD__2_Unit_Type"."TypeStatus" IS 'TypeStatus of the unit';
COMMENT ON COLUMN "#project#"."ABCD__2_Unit_Type"."IdentificationUnitID" IS 'Unique key in IdentificationUnit';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD__MultiMediaObject   ###################################################################################
--#####################################################################################################################
--#####################################################################################################################



--#####################################################################################################################
--######   ABCD__1_MultiMediaObject_DisplayOrder   ####################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "#project#"."ABCD__1_MultiMediaObject_DisplayOrder" AS 
 SELECT t."CollectionSpecimenID",
    t."URI",
    row_number() OVER (ORDER BY ( SELECT 1)) AS "DisplayOrder"
   FROM ( SELECT t_1."CollectionSpecimenID",
            t_1."URI"
           FROM "#project#"."CacheCollectionSpecimenImage" t_1
          ORDER BY t_1."CollectionSpecimenID", t_1."URI") t;

ALTER TABLE "#project#"."ABCD__1_MultiMediaObject_DisplayOrder"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__1_MultiMediaObject_DisplayOrder" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__1_MultiMediaObject_DisplayOrder" TO "CacheUser";
COMMENT ON VIEW "#project#"."ABCD__1_MultiMediaObject_DisplayOrder" IS 'ABCD auxillary view - Display order for the images';
COMMENT ON COLUMN "#project#"."ABCD__1_MultiMediaObject_DisplayOrder"."CollectionSpecimenID" IS 'Unique column in table CollectionSpecimen';
COMMENT ON COLUMN "#project#"."ABCD__1_MultiMediaObject_DisplayOrder"."DisplayOrder" IS 'Generated display order for the images';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD__MultiMediaObject   ###################################################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD__MultiMediaObject"
(
  "ID" character varying(25),
  "URI" character varying(255),
  "CollectionSpecimenID" integer,
  "IdentificationUnitID" integer,
  "SpecimenPartID" integer,
  "MediaFormat"  character varying(4),
  "DisplayOrder" bigint,
  "ProductURI" character varying,
  "ImageSpecimenPartID" integer,
  "ImageIdentificationUnitID" integer,
  "LicenseType" character varying(500),
  "LicenseNotes" character varying(500),
  "LicenseURI" character varying(500),
  "CreatorAgent" character varying(500),
  "CopyrightStatement" character varying(500),
  CONSTRAINT "ABCD__MultiMediaObject_pkey" PRIMARY KEY ("ID", "URI")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD__MultiMediaObject"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__MultiMediaObject" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__MultiMediaObject" TO "CacheUser";


--#####################################################################################################################
--######   Create index for ABCD__MultiMediaObject  ###################################################################
--#####################################################################################################################

CREATE INDEX ABCD__MultiMediaObject_ID
  ON "#project#"."ABCD__MultiMediaObject" ("ID");


--#####################################################################################################################
--######   function ABCD__MultiMediaObject  ###########################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__multimediaobject()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- Cleaning the table
TRUNCATE TABLE "#project#"."ABCD__MultiMediaObject";

-- Removing indices
ALTER TABLE "#project#"."ABCD__MultiMediaObject" DROP CONSTRAINT "ABCD__MultiMediaObject_pkey";
DROP INDEX "#project#".abcd__multimediaobject_id;

-- images for units without parts
--    images assigned to units
INSERT INTO "#project#"."ABCD__MultiMediaObject"(
            "ID", "CollectionSpecimenID", "URI", "MediaFormat", "IdentificationUnitID", 
            "DisplayOrder", "ProductURI", "ImageSpecimenPartID", "ImageIdentificationUnitID", 
            "LicenseType", "LicenseNotes", "LicenseURI", "CreatorAgent", 
            "CopyrightStatement", "SpecimenPartID")
 SELECT DISTINCT concat(
        u."IdentificationUnitID"::character varying, '-') AS "ID",
    i."CollectionSpecimenID",
    i."URI",
    "substring"(i."URI"::text, length(i."URI"::text) - strpos(reverse(i."URI"::text), '.'::text) + 2, 255)::character varying(4) AS "MediaFormat",
    u."IdentificationUnitID",
        CASE
            WHEN i."DisplayOrder" IS NULL THEN d."DisplayOrder"
            ELSE i."DisplayOrder"::bigint
        END AS "DisplayOrder",
        CASE
            WHEN i."URI"::text ~~ '%.html'::text THEN i."URI"
            ELSE highresolutionimagepath(i."URI")
        END AS "ProductURI",
    i."SpecimenPartID" AS "ImageSpecimenPartID",
    i."IdentificationUnitID" AS "ImageIdentificationUnitID",
    i."LicenseType",
    i."LicenseNotes",
    i."LicenseURI",
    i."CreatorAgent",
    i."IPR" AS "CopyrightStatement",
    NULL::integer AS "SpecimenPartID"
   FROM "#project#"."CacheCollectionSpecimenImage" i
     JOIN "#project#"."CacheIdentificationUnit" u ON i."CollectionSpecimenID" = u."CollectionSpecimenID"
     JOIN "#project#"."ABCD__1_MultiMediaObject_DisplayOrder" d ON i."CollectionSpecimenID" = d."CollectionSpecimenID" AND i."URI"::text = d."URI"::text
     LEFT JOIN "#project#"."CacheIdentificationUnitInPart" up ON i."CollectionSpecimenID" = up."CollectionSpecimenID" AND i."SpecimenPartID" = up."SpecimenPartID"
  WHERE up."SpecimenPartID" IS NULL
  AND i."IdentificationUnitID" = u."IdentificationUnitID";

-- images not assigned to units
INSERT INTO "#project#"."ABCD__MultiMediaObject"(
            "ID", "CollectionSpecimenID", "URI", "MediaFormat", "IdentificationUnitID", 
            "DisplayOrder", "ProductURI", "ImageSpecimenPartID", "ImageIdentificationUnitID", 
            "LicenseType", "LicenseNotes", "LicenseURI", "CreatorAgent", 
            "CopyrightStatement", "SpecimenPartID")
 SELECT DISTINCT concat(
        u."IdentificationUnitID"::character varying, '-') AS "ID",
    i."CollectionSpecimenID",
    i."URI",
    "substring"(i."URI"::text, length(i."URI"::text) - strpos(reverse(i."URI"::text), '.'::text) + 2, 255)::character varying(4) AS "MediaFormat",
    u."IdentificationUnitID",
        CASE
            WHEN i."DisplayOrder" IS NULL THEN d."DisplayOrder"
            ELSE i."DisplayOrder"::bigint
        END AS "DisplayOrder",
        CASE
            WHEN i."URI"::text ~~ '%.html'::text THEN i."URI"
            ELSE highresolutionimagepath(i."URI")
        END AS "ProductURI",
    i."SpecimenPartID" AS "ImageSpecimenPartID",
    i."IdentificationUnitID" AS "ImageIdentificationUnitID",
    i."LicenseType",
    i."LicenseNotes",
    i."LicenseURI",
    i."CreatorAgent",
    i."IPR" AS "CopyrightStatement",
    NULL::integer AS "SpecimenPartID"
   FROM "#project#"."CacheCollectionSpecimenImage" i
     JOIN "#project#"."CacheIdentificationUnit" u ON i."CollectionSpecimenID" = u."CollectionSpecimenID"
     JOIN "#project#"."ABCD__1_MultiMediaObject_DisplayOrder" d ON i."CollectionSpecimenID" = d."CollectionSpecimenID" AND i."URI"::text = d."URI"::text
     LEFT JOIN "#project#"."CacheIdentificationUnitInPart" up ON i."CollectionSpecimenID" = up."CollectionSpecimenID" AND i."SpecimenPartID" = up."SpecimenPartID"
  WHERE up."SpecimenPartID" IS NULL
  AND i."IdentificationUnitID" is null;

-- images for units with parts
--    images assigned to parts and units
INSERT INTO "#project#"."ABCD__MultiMediaObject"(
            "ID", "CollectionSpecimenID", "URI", "MediaFormat", "IdentificationUnitID", 
            "DisplayOrder", "ProductURI", "ImageSpecimenPartID", "ImageIdentificationUnitID", 
            "LicenseType", "LicenseNotes", "LicenseURI", "CreatorAgent", 
            "CopyrightStatement", "SpecimenPartID")
SELECT DISTINCT concat(up."IdentificationUnitID"::character varying, '-',
        CASE
            WHEN i."SpecimenPartID" IS NULL THEN up."SpecimenPartID"
            ELSE i."SpecimenPartID"
        END::character varying) AS "ID",
    up."CollectionSpecimenID",
    i."URI",
    "substring"(i."URI"::text, length(i."URI"::text) - strpos(reverse(i."URI"::text), '.'::text) + 2, 255)::character varying(4) AS "MediaFormat",
    up."IdentificationUnitID",
        CASE
            WHEN i."DisplayOrder" IS NULL THEN d."DisplayOrder"
            ELSE i."DisplayOrder"::bigint
        END AS "DisplayOrder",
        CASE
            WHEN i."URI"::text ~~ '%.html'::text THEN i."URI"
            ELSE highresolutionimagepath(i."URI")
        END AS "ProductURI",
    i."SpecimenPartID" AS "ImageSpecimenPartID",
    i."IdentificationUnitID" AS "ImageIdentificationUnitID",
    i."LicenseType",
    i."LicenseNotes",
    i."LicenseURI",
    i."CreatorAgent",
    i."IPR" AS "CopyrightStatement",
        CASE
            WHEN i."SpecimenPartID" IS NULL THEN up."SpecimenPartID"
            ELSE i."SpecimenPartID"
        END::integer AS "SpecimenPartID"
   FROM "#project#"."CacheCollectionSpecimenImage" i
     JOIN "#project#"."CacheIdentificationUnitInPart" up ON i."CollectionSpecimenID" = up."CollectionSpecimenID"
     JOIN "#project#"."ABCD__1_MultiMediaObject_DisplayOrder" d ON i."CollectionSpecimenID" = d."CollectionSpecimenID" AND i."URI"::text = d."URI"::text
     WHERE  i."IdentificationUnitID" = up."IdentificationUnitID";


INSERT INTO "#project#"."ABCD__MultiMediaObject"(
            "ID", "CollectionSpecimenID", "URI", "MediaFormat", "IdentificationUnitID", 
            "DisplayOrder", "ProductURI", "ImageSpecimenPartID", "ImageIdentificationUnitID", 
            "LicenseType", "LicenseNotes", "LicenseURI", "CreatorAgent", 
            "CopyrightStatement", "SpecimenPartID")
SELECT DISTINCT concat(up."IdentificationUnitID"::character varying, '-',
        CASE
            WHEN i."SpecimenPartID" IS NULL THEN up."SpecimenPartID"
            ELSE i."SpecimenPartID"
        END::character varying) AS "ID",
    up."CollectionSpecimenID",
    i."URI",
    "substring"(i."URI"::text, length(i."URI"::text) - strpos(reverse(i."URI"::text), '.'::text) + 2, 255)::character varying(4) AS "MediaFormat",
    up."IdentificationUnitID",
        CASE
            WHEN i."DisplayOrder" IS NULL THEN d."DisplayOrder"
            ELSE i."DisplayOrder"::bigint
        END AS "DisplayOrder",
        CASE
            WHEN i."URI"::text ~~ '%.html'::text THEN i."URI"
            ELSE highresolutionimagepath(i."URI")
        END AS "ProductURI",
    i."SpecimenPartID" AS "ImageSpecimenPartID",
    i."IdentificationUnitID" AS "ImageIdentificationUnitID",
    i."LicenseType",
    i."LicenseNotes",
    i."LicenseURI",
    i."CreatorAgent",
    i."IPR" AS "CopyrightStatement",
        CASE
            WHEN i."SpecimenPartID" IS NULL THEN up."SpecimenPartID"
            ELSE i."SpecimenPartID"
        END::integer AS "SpecimenPartID"
   FROM "#project#"."CacheCollectionSpecimenImage" i
     JOIN "#project#"."CacheIdentificationUnitInPart" up ON i."CollectionSpecimenID" = up."CollectionSpecimenID"
     JOIN "#project#"."ABCD__1_MultiMediaObject_DisplayOrder" d ON i."CollectionSpecimenID" = d."CollectionSpecimenID" AND i."URI"::text = d."URI"::text
     WHERE  i."IdentificationUnitID" is null;

-- inserting the Primary key
ALTER TABLE "#project#"."ABCD__MultiMediaObject"
  ADD CONSTRAINT "ABCD__MultiMediaObject_pkey" PRIMARY KEY("ID", "URI");

-- inserting
CREATE INDEX ABCD__MultiMediaObject_ID
  ON "#project#"."ABCD__MultiMediaObject" ("ID");
       
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

  
ALTER FUNCTION "#project#".ABCD__MultiMediaObject()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".ABCD__MultiMediaObject() TO "CacheAdmin";

--#####################################################################################################################
--######   Insert data into ABCD__MultiMediaObject  ###################################################################
--#####################################################################################################################

--SELECT "#project#".ABCD__MultiMediaObject();


--#####################################################################################################################
--######   ABCD_MultiMediaObject in schema public  ####################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_MultiMediaObject" AS 
 SELECT "N"."ID",
    "N"."CollectionSpecimenID",
    "N"."IdentificationUnitID",
    "N"."URI" AS "fileUri",
    "N"."MediaFormat" AS "fileFormat",
    concat(repeat('0'::text, 8 - length("N"."DisplayOrder"::character varying::text)), "N"."DisplayOrder"::character varying) AS "DisplayOrder",
    "N"."ProductURI",
    "N"."ImageSpecimenPartID",
    "N"."ImageIdentificationUnitID",
    "N"."LicenseType" AS "IPR_License_Text",
    "N"."LicenseNotes" AS "IPR_License_Details",
    "N"."LicenseURI" AS "IPR_License_URI",
    "N"."CreatorAgent",
        CASE
            WHEN "N"."CopyrightStatement"::text <> ''::text THEN
            CASE
                WHEN "N"."CopyrightStatement"::text ~~ '©%'::text THEN "N"."CopyrightStatement"::text
                ELSE concat('© ', "N"."CopyrightStatement")
            END
            ELSE ''::text
        END::character varying(500) AS "IPR_Copyright_Text",
        "M"."TermsOfUseText" AS "IPR_TermsOfUse_Text"
   FROM "#project#"."ABCD__MultiMediaObject" "N"
   cross join "#project#"."CacheMetadata" "M" WHERE "M"."ProjectID" = "#project#".projectid();
 
ALTER TABLE "ABCD_MultiMediaObject"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_MultiMediaObject" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_MultiMediaObject" TO "CacheUser";

COMMENT ON VIEW "ABCD_MultiMediaObject" IS 'ABCD entity /DataSets/DataSet/Units/Unit/MultiMediaObjects/MultiMediaObject/';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."CollectionSpecimenID" IS 'Unique column in table CollectionSpecimen';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."IdentificationUnitID" IS 'Unique column in table IdentificationUnit';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."ImageSpecimenPartID" IS 'Unique column in table CollectionSpecmienPart for images linked to a single part within the specimen';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."ImageIdentificationUnitID" IS 'Unique column in table IdentificationUnit for images linked to a single unit within the specimen';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."fileUri" IS 'ABCD: Unit/MultiMediaObjects/MultiMediaObject/FileURI. Retrieved from table CollectionSpecimenImage - URI';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."fileFormat" IS 'ABCD: Unit/MultiMediaObjects/MultiMediaObject/Format. Retrieved from table CollectionSpecimenImage - URI - extension';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."DisplayOrder" IS 'Retrieved from table CollectionSpecimenImage - DisplayOrder converted to 8 digit text with leading 0';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."ProductURI" IS 'ABCD: Unit/MultiMediaObjects/MultiMediaObject/ProductURI. Retrieved from table CollectionSpecimenImage - URI. For projects where a high resolution image is available, the path of the high resolution image';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."IPR_License_Text" IS 'ABCD: Unit/MultiMediaObjects/MultiMediaObject/IPR/Licenses/License/Text. Retrieved from table CollectionSpecimenImage - LicenseType';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."IPR_License_Details" IS 'ABCD: Unit/MultiMediaObjects/MultiMediaObject/IPR/Licenses/License/Details. Retrieved from table CollectionSpecimenImage - LicenseNotes';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."IPR_License_URI" IS 'ABCD: Unit/MultiMediaObjects/MultiMediaObject/IPR/Licenses/License/Details. Retrieved from table CollectionSpecimenImage - LicenseURI';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."CreatorAgent" IS 'ABCD: Unit/MultiMediaObjects/MultiMediaObject/Creator. Retrieved from table CollectionSpecimenImage - CreatorAgent';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."IPR_Copyright_Text" IS 'ABCD: Unit/MultiMediaObjects/Copyrights/Copyright/Text. Retrieved from table CollectionSpecimenImage - CopyrightStatement with inserted ©';
COMMENT ON COLUMN "ABCD_MultiMediaObject"."IPR_TermsOfUse_Text" IS 'ABCD: Unit/MultiMediaObjects/TermsOfUseStatements/TermsOfUse/Text. Retrieved from DiversityProjects - Settings - ABCD - TermsOfUse - Text';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD_MeasurementOrFact   ###################################################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD_MeasurementOrFact"
(
  "ID" character varying(25),
  "AnalysisID" integer,
  "AnalysisNumber" character varying(50),
  "Parameter" character varying(50),
  "UnitOfMeasurement" character varying(50),
  "LowerValue" text,
  "MeasurementDateTime" character varying(50),
  "MeasuredBy" character varying(255),
  "IdentificationUnitID" integer,
  "SpecimenPartID" integer,
  "CollectionSpecimenID" integer,
  CONSTRAINT "ABCD_MeasurementOrFact_pkey" PRIMARY KEY ("ID", "AnalysisID", "AnalysisNumber")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "#project#"."ABCD_MeasurementOrFact"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD_MeasurementOrFact" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD_MeasurementOrFact" TO "CacheUser";


--#####################################################################################################################
--######  Create index for ABCD_MeasurementOrFact   ##################################################################
--#####################################################################################################################

CREATE INDEX ABCD_MeasurementOrFact_ID
  ON "#project#"."ABCD_MeasurementOrFact" ("ID");
  
--95 sec
  

--#####################################################################################################################
--######   function abcd__MeasurementOrFact  ##########################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__MeasurementOrFact()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- Cleaning the table
TRUNCATE TABLE "#project#"."ABCD_MeasurementOrFact";

-- Removing the indices
ALTER TABLE "#project#"."ABCD_MeasurementOrFact" DROP CONSTRAINT "ABCD_MeasurementOrFact_pkey";
DROP INDEX "#project#".abcd_measurementorfact_id;

-- insert the data
INSERT INTO "#project#"."ABCD_MeasurementOrFact"(
            "ID", "Parameter", "UnitOfMeasurement", "LowerValue", "MeasurementDateTime", 
            "MeasuredBy", "IdentificationUnitID", "SpecimenPartID", "CollectionSpecimenID", 
            "AnalysisID", "AnalysisNumber")
SELECT concat(ua."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    a."DisplayText" AS "Parameter",
    a."MeasurementUnit" AS "UnitOfMeasurement",
    ua."AnalysisResult" AS "LowerValue",
    ua."AnalysisDate" AS "MeasurementDateTime",
    ua."ResponsibleName" AS "MeasuredBy",
    ua."IdentificationUnitID",
    up."SpecimenPartID",
    ua."CollectionSpecimenID",
    ua."AnalysisID",
    ua."AnalysisNumber"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnitAnalysis" ua ON ua."IdentificationUnitID" = up."IdentificationUnitID" and ua."CollectionSpecimenID" = up."CollectionSpecimenID"
     JOIN "#project#"."CacheAnalysis" a ON a."AnalysisID" = ua."AnalysisID";

-- inserting the indices
ALTER TABLE "#project#"."ABCD_MeasurementOrFact"
  ADD CONSTRAINT "ABCD_MeasurementOrFact_pkey" PRIMARY KEY("ID", "AnalysisID", "AnalysisNumber");
  
CREATE INDEX abcd_measurementorfact_id
  ON "#project#"."ABCD_MeasurementOrFact"
  USING btree
  ("ID" COLLATE pg_catalog."default");

     
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__MeasurementOrFact()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__MeasurementOrFact() TO "CacheAdmin";


--#####################################################################################################################
--######   insert data into ABCD_MeasurementOrFact   ##################################################################
--#####################################################################################################################

--SELECT "#project#".abcd__MeasurementOrFact();

-- 153 sec


--#####################################################################################################################
--######   ABCD_MeasurementOrFact in schema public   ##################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_MeasurementOrFact" AS 
 SELECT "ABCD_MeasurementOrFact"."ID",
    "ABCD_MeasurementOrFact"."Parameter",
    "ABCD_MeasurementOrFact"."UnitOfMeasurement",
    "ABCD_MeasurementOrFact"."LowerValue",
    "ABCD_MeasurementOrFact"."MeasurementDateTime",
    "ABCD_MeasurementOrFact"."MeasuredBy",
    "ABCD_MeasurementOrFact"."IdentificationUnitID",
    "ABCD_MeasurementOrFact"."SpecimenPartID",
    "ABCD_MeasurementOrFact"."CollectionSpecimenID",
    "ABCD_MeasurementOrFact"."AnalysisID",
    "ABCD_MeasurementOrFact"."AnalysisNumber"
   FROM "#project#"."ABCD_MeasurementOrFact";

ALTER TABLE "ABCD_MeasurementOrFact"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_MeasurementOrFact" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_MeasurementOrFact" TO "CacheUser";
COMMENT ON VIEW "ABCD_MeasurementOrFact" IS 'ABCD entity /DataSets/DataSet/Units/Unit/MultiMediaObjects/MultiMediaObject/';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."Parameter" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/Parameter. Retrieved from table Analysis - DisplayText';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."UnitOfMeasurement" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/UnitOfMeasurement. Retrieved from table Analysis - MeasurementUnit';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."LowerValue" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/LowerValue. Retrieved from table IdentificationUnitAnalysis - AnalysisResult';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."MeasurementDateTime" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasurementDateTime. Retrieved from table IdentificationUnitAnalysis - AnalysisDate';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."MeasuredBy" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasuredBy. Retrieved from table IdentificationUnitAnalysis - ResponsibleName';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."MeasuredBy" IS 'ABCD: Unit/MeasurementsOrFacts/MeasurementOrFact/MeasurementOrFactAtomised/MeasuredBy. Retrieved from table IdentificationUnitAnalysis - ResponsibleName';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."IdentificationUnitID" IS 'PK of table IdentificationUnitID';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."SpecimenPartID" IS 'PK of table CollectionSpecimenPart';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."AnalysisID" IS 'PK of table Analysis';
COMMENT ON COLUMN "ABCD_MeasurementOrFact"."AnalysisNumber" IS 'Retrieved from table IdentificationUnitAnalysis - AnalysisNumber';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD__UnitPart   ###########################################################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD__UnitPart"
(
  "ID" character varying(25),
  "UnitGUID" character varying(254),
  "SourceInstitutionID" character varying(254),
  "SourceID" character varying(254),
  "UnitID" character varying(254),
  "DateLastEdited" timestamp without time zone,
  "Identification_Taxon_ScientificName_FullScientificName" character varying(255),
  "Identification_Taxon_ScientificName_Qualifier" character varying(50),
  "RecordBasis" character varying(50),
  "KindOfUnit" character varying(50),
  "Identification_Taxon_HigherTaxonName" character varying(50),
  "KindOfUnit_Language" character varying(2),
  "HerbariumUnit_Exsiccatum" character varying(255),
  "RecordURI" character varying(500),
  "CollectionSpecimenID" integer,
  "IdentificationUnitID" integer,
  CONSTRAINT "ABCD__UnitPart_pkey" PRIMARY KEY ("ID")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD__UnitPart"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__UnitPart" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__UnitPart" TO "CacheUser";


--#####################################################################################################################
--######   function abcd__UnitPart  ###################################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__UnitPart()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- cleaning the table
TRUNCATE TABLE "#project#"."ABCD__UnitPart";

-- removing key
ALTER TABLE "#project#"."ABCD__UnitPart" DROP CONSTRAINT "ABCD__UnitPart_pkey";

-- insert Units with parts
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
    m."SourceInstitutionID",
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
    s."ExsiccataAbbreviation" AS "HerbariumUnit_Exsiccatum",
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
     CROSS JOIN "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

-- adding the key
ALTER TABLE "#project#"."ABCD__UnitPart"
  ADD CONSTRAINT "ABCD__UnitPart_pkey" PRIMARY KEY("ID");

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__UnitPart()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__UnitPart() TO "CacheAdmin";

--#####################################################################################################################
--######   fill ABCD__UnitPart   ######################################################################################
--#####################################################################################################################

--SELECT "#project#".abcd__UnitPart();


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD__UnitNoPart   #########################################################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD__UnitNoPart"
(
  "ID" character varying(25),
  "UnitGUID" character varying(254),
  "SourceInstitutionID" character varying(254),
  "SourceID" character varying(254),
  "UnitID" character varying(254),
  "DateLastEdited" timestamp without time zone,
  "Identification_Taxon_ScientificName_FullScientificName" character varying(255),
  "Identification_Taxon_ScientificName_Qualifier" character varying(50),
  "RecordBasis" character varying(50),
  "KindOfUnit" character varying(50),
  "Identification_Taxon_HigherTaxonName" character varying(50),
  "KindOfUnit_Language" character varying(2),
  "HerbariumUnit_Exsiccatum" character varying(255),
  "RecordURI" character varying(500),
  "CollectionSpecimenID" integer,
  "IdentificationUnitID" integer,
  CONSTRAINT "ABCD__UnitNoPart_pkey" PRIMARY KEY ("ID")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD__UnitNoPart"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__UnitNoPart" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__UnitNoPart" TO "CacheUser";


--#####################################################################################################################
--######  Create index for ABCD__UnitNoPart for CollectionSpecimenID  #################################################
--#####################################################################################################################

CREATE INDEX ABCD__UnitNoPart_CollectionSpecimenID
  ON "#project#"."ABCD__UnitNoPart" ("CollectionSpecimenID");
  
--11387 ms

--#####################################################################################################################
--######  Create index for ABCD__UnitNoPart for ID  ###################################################################
--#####################################################################################################################

CREATE INDEX ABCD__UnitNoPart_ID
  ON "#project#"."ABCD__UnitNoPart" ("ID");
  
--57 s

--#####################################################################################################################
--######  Create index for ABCD__UnitNoPart for IdentificationUnitID  #################################################
--#####################################################################################################################

CREATE INDEX ABCD__UnitNoPart_IdentificationUnitID 
  ON "#project#"."ABCD__UnitNoPart" ("IdentificationUnitID");

--#####################################################################################################################
--######  Create index for ABCD__UnitNoPart for Identification_Taxon_ScientificName_FullScientificName  ###############
--#####################################################################################################################

CREATE INDEX ABCD__UnitNoPart_Identification_ScientificName 
  ON "#project#"."ABCD__UnitNoPart" ("Identification_Taxon_ScientificName_FullScientificName");  


--#####################################################################################################################
--######   function abcd__UnitNoPart_RemoveIndices  ###################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__UnitNoPart_RemoveIndices()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- removing the key
ALTER TABLE "#project#"."ABCD__UnitNoPart" DROP CONSTRAINT "ABCD__UnitNoPart_pkey";

-- removing the indices
DROP INDEX "#project#".ABCD__UnitNoPart_CollectionSpecimenID;
DROP INDEX "#project#".ABCD__UnitNoPart_ID;
DROP INDEX "#project#".ABCD__UnitNoPart_IdentificationUnitID;
DROP INDEX "#project#".abcd__unitnopart_identification_scientificname;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__UnitNoPart_RemoveIndices()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__UnitNoPart_RemoveIndices() TO "CacheAdmin";



--#####################################################################################################################
--######   function abcd__UnitNoPart_AddingIndices  ###################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__UnitNoPart_AddingIndices()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- inserting the primary key
ALTER TABLE "#project#"."ABCD__UnitNoPart"
  ADD CONSTRAINT "ABCD__UnitNoPart_pkey" PRIMARY KEY("ID");

-- creating the indices
CREATE INDEX ABCD__UnitNoPart_CollectionSpecimenID
  ON "#project#"."ABCD__UnitNoPart" 
  USING btree 
  ("CollectionSpecimenID");

CREATE INDEX ABCD__UnitNoPart_ID
  ON "#project#"."ABCD__UnitNoPart" 
  USING btree
  ("ID" COLLATE pg_catalog."default");

CREATE INDEX ABCD__UnitNoPart_IdentificationUnitID 
  ON "#project#"."ABCD__UnitNoPart" 
  USING btree
  ("IdentificationUnitID");

CREATE INDEX ABCD__UnitNoPart_Identification_ScientificName 
  ON "#project#"."ABCD__UnitNoPart" 
  USING btree
  ("Identification_Taxon_ScientificName_FullScientificName" COLLATE pg_catalog."default");

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__UnitNoPart_AddingIndices()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__UnitNoPart_AddingIndices() TO "CacheAdmin";


--#####################################################################################################################
--######   function abcd__UnitNoPart  #################################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__UnitNoPart()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

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
     WHERE up."IdentificationUnitID" IS NULL;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__UnitNoPart()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__UnitNoPart() TO "CacheAdmin";


--#####################################################################################################################
--######   fill ABCD__UnitNoPart   ####################################################################################
--#####################################################################################################################

--SELECT "#project#".abcd__UnitNoPart();
--100588 ms.


--#####################################################################################################################
--######   function abcd__UnitNoPartMetadata  #########################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__UnitNoPartMetadata()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- creating the needed index
CREATE INDEX ABCD__UnitNoPart_CollectionSpecimenID
  ON "#project#"."ABCD__UnitNoPart" ("CollectionSpecimenID");

-- creating the temporary table
CREATE TEMP TABLE ABCD__UnitNoPartMetadataTemp AS SELECT a."ID", 
a."UnitGUID", m."SourceInstitutionID", m."ProjectTitleCode" AS "SourceID", a."UnitID", 
       a."DateLastEdited", a."Identification_Taxon_ScientificName_FullScientificName", 
       a."Identification_Taxon_ScientificName_Qualifier", a."RecordBasis", 
       a."KindOfUnit", a."Identification_Taxon_HigherTaxonName", a."KindOfUnit_Language", 
       a."HerbariumUnit_Exsiccatum", CASE
            WHEN m."RecordUri"::text ~~ 'http://biocase%'::text THEN concat(m."RecordUri", btrim(concat(
            CASE
                WHEN s."AccessionNumber"::text <> ''::text THEN s."AccessionNumber"
                ELSE a."CollectionSpecimenID"::character varying
            END, ' / ', a."IdentificationUnitID"::character varying)))
            ELSE
            CASE
                WHEN m."RecordUri"::text <> ''::text THEN concat(m."RecordUri", s."CollectionSpecimenID"::character varying)
                ELSE NULL::text
            END
        END AS "RecordURI", a."CollectionSpecimenID", 
       a."IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart" a
	JOIN "#project#"."CacheCollectionSpecimen" s ON s."CollectionSpecimenID" = a."CollectionSpecimenID"
	CROSS JOIN "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

-- removing the indes
DROP INDEX "#project#".ABCD__UnitNoPart_CollectionSpecimenID;

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

-- transfer the data into the target table
INSERT INTO "#project#"."ABCD__UnitNoPart"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID")
SELECT "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
       "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
       "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
       "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
       "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
       "IdentificationUnitID"
  FROM ABCD__UnitNoPartMetadataTemp;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
  
ALTER FUNCTION "#project#".abcd__UnitNoPartMetadata()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__UnitNoPartMetadata() TO "CacheAdmin";




--#####################################################################################################################
--######   writing the metadata using abcd__UnitNoPartMetadata  #######################################################
--#####################################################################################################################

--select "#project#".abcd__UnitNoPartMetadata();

--222682 ms



--#####################################################################################################################
--######   function abcd__UnitNoPartKingdom  ##########################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__UnitNoPartKingdom()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- creating the needed index
CREATE INDEX ABCD__UnitNoPart_IdentificationUnitID 
  ON "#project#"."ABCD__UnitNoPart" 
  USING btree
  ("IdentificationUnitID");

-- creating the temporary table
CREATE TEMP TABLE ABCD__UnitNoPartKingdomTemp AS SELECT a."ID",
    a."UnitGUID",
    a."SourceInstitutionID",
    a."SourceID",
    a."UnitID",
    a."DateLastEdited",
    a."Identification_Taxon_ScientificName_FullScientificName",
    a."Identification_Taxon_ScientificName_Qualifier",
    rnp."RecordBasis" AS "RecordBasis",
    a."KindOfUnit",
    k."Kingdom" AS "Identification_Taxon_HigherTaxonName",
    a."KindOfUnit_Language",
    a."HerbariumUnit_Exsiccatum",
    a."RecordURI",
    a."CollectionSpecimenID",
    a."IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart" a
     JOIN "#project#"."ABCD__Kingdom" k ON a."IdentificationUnitID" = k."IdentificationUnitID"
     JOIN "#project#"."ABCD__RecordBasis_NoPart" rnp ON a."IdentificationUnitID" = rnp."IdentificationUnitID";

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

-- removing the index
DROP INDEX "#project#".ABCD__UnitNoPart_IdentificationUnitID;

-- inserting the data
INSERT INTO "#project#"."ABCD__UnitNoPart"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID")
SELECT "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
       "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
       "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
       "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
       "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
       "IdentificationUnitID"
  FROM ABCD__UnitNoPartKingdomTemp;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
  
ALTER FUNCTION "#project#".abcd__UnitNoPartKingdom()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__UnitNoPartKingdom() TO "CacheAdmin";


--#####################################################################################################################
--######   writing RecordBasis and HigherTaxon using the function abcd__UnitNoPartKingdom  ############################
--#####################################################################################################################

--select "#project#".abcd__UnitNoPartKingdom();



--#####################################################################################################################
--######   function abcd__UnitNoPartQualifier  ########################################################################
--#####################################################################################################################


CREATE OR REPLACE FUNCTION "#project#".abcd__UnitNoPartQualifier()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- creating the needed index
CREATE INDEX ABCD__UnitNoPart_IdentificationUnitID 
  ON "#project#"."ABCD__UnitNoPart" 
  USING btree
  ("IdentificationUnitID");

-- creating the temporary table
CREATE TEMP TABLE ABCD__UnitNoPartIdentificationQualifierTemp AS
SELECT MAX(i."IdentificationQualifier")::character varying(50) AS "IdentificationQualifier",
    MAX(a."IdentificationUnitID")::integer AS "IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart" a
   JOIN "#project#"."CacheIdentification" i ON a."IdentificationUnitID" = i."IdentificationUnitID" 
   AND a."Identification_Taxon_ScientificName_FullScientificName"::text = i."TaxonomicName"::text 
   AND i."IdentificationQualifier" <> ''
   GROUP BY i."IdentificationQualifier", a."IdentificationUnitID";

-- creating the second temporary table   
CREATE TEMP TABLE ABCD__UnitNoPartQualifierTemp AS  
SELECT a."ID",
       a."UnitGUID",
    a."SourceInstitutionID",
    a."SourceID",
    a."UnitID",
    a."DateLastEdited",
    a."Identification_Taxon_ScientificName_FullScientificName",
    i."IdentificationQualifier" AS "Identification_Taxon_ScientificName_Qualifier",
    a."RecordBasis",
    a."KindOfUnit",
    a."Identification_Taxon_HigherTaxonName",
    a."KindOfUnit_Language",
    a."HerbariumUnit_Exsiccatum",
    a."RecordURI",
    a."CollectionSpecimenID",
    a."IdentificationUnitID"
   FROM "#project#"."ABCD__UnitNoPart" a
   LEFT JOIN ABCD__UnitNoPartIdentificationQualifierTemp i ON i."IdentificationUnitID" = a."IdentificationUnitID";

-- cleaning the target table
TRUNCATE TABLE "#project#"."ABCD__UnitNoPart";

-- removing the index
DROP INDEX "#project#".ABCD__UnitNoPart_IdentificationUnitID;

-- inserting the data
INSERT INTO "#project#"."ABCD__UnitNoPart"(
            "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
            "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
            "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
            "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
            "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
            "IdentificationUnitID")
SELECT "ID", "UnitGUID", "SourceInstitutionID", "SourceID", "UnitID", 
       "DateLastEdited", "Identification_Taxon_ScientificName_FullScientificName", 
       "Identification_Taxon_ScientificName_Qualifier", "RecordBasis", 
       "KindOfUnit", "Identification_Taxon_HigherTaxonName", "KindOfUnit_Language", 
       "HerbariumUnit_Exsiccatum", "RecordURI", "CollectionSpecimenID", 
       "IdentificationUnitID"
  FROM ABCD__UnitNoPartQualifierTemp;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
  
ALTER FUNCTION "#project#".abcd__UnitNoPartQualifier()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__UnitNoPartQualifier() TO "CacheAdmin";

--#####################################################################################################################
--######   writing the qualifier using the function abcd__UnitNoPartQualifier  ########################################
--#####################################################################################################################

--select "#project#".abcd__UnitNoPartQualifier();

--230200 ms. = ca. 4 min

  
--#####################################################################################################################
--######   idxCacheIdentificationUnitInPart_SpecimenPartID  ###########################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__idxCacheIdentificationUnitInPart_SpecimenPartID()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

if (select count(*) from pg_indexes where tablename = 'CacheIdentificationUnitInPart' and schemaname = '#project#' and indexdef like '% ("SpecimenPartID")') = 0
then

  CREATE INDEX idxCacheIdentificationUnitInPart_SpecimenPartID
  ON "#project#"."CacheIdentificationUnitInPart"
  USING btree
  ("SpecimenPartID");

end if;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

ALTER FUNCTION "#project#".abcd__idxCacheIdentificationUnitInPart_SpecimenPartID()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__idxCacheIdentificationUnitInPart_SpecimenPartID() TO "CacheAdmin";


--######   creating the index  ###########################################################

select "#project#".abcd__idxCacheIdentificationUnitInPart_SpecimenPartID();


--#####################################################################################################################
--######   idxCacheIdentificationUnit_IdentificationUnitID  ###########################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__idxCacheIdentificationUnit_IdentificationUnitID()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

if (select count(*) from pg_indexes where tablename = 'CacheIdentificationUnit' and schemaname = '#project#' and indexdef like '% ("IdentificationUnitID")') = 0
then

CREATE INDEX idxCacheIdentificationUnit_IdentificationUnitID
  ON "#project#"."CacheIdentificationUnit"
  USING btree
  ("IdentificationUnitID");
  
end if;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

ALTER FUNCTION "#project#".abcd__idxCacheIdentificationUnit_IdentificationUnitID()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__idxCacheIdentificationUnit_IdentificationUnitID() TO "CacheAdmin";


--######   creating the index  ###########################################################

select "#project#".abcd__idxCacheIdentificationUnit_IdentificationUnitID();

--10 sec

--#####################################################################################################################
--######   idxCacheIdentificationUnit_CollectionSpecimenID  ###########################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__idxCacheIdentificationUnit_CollectionSpecimenID()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

if (select count(*) from pg_indexes where tablename = 'CacheIdentificationUnit' and schemaname = '#project#' and indexdef like '% ("CollectionSpecimenID")') = 0
then

CREATE INDEX idxCacheIdentificationUnit_CollectionSpecimenID
  ON "#project#"."CacheIdentificationUnit"
  USING btree
  ("CollectionSpecimenID");
  end if;

end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

ALTER FUNCTION "#project#".abcd__idxcacheidentificationunit_collectionspecimenid()
  OWNER TO "CacheAdmin"; 
GRANT EXECUTE ON FUNCTION "#project#".abcd__idxcacheidentificationunit_collectionspecimenid() TO "CacheAdmin";


--######   creating the index  ###########################################################

select "#project#".abcd__idxCacheIdentificationUnit_CollectionSpecimenID();

-- 11 sec


--#####################################################################################################################
--######   ABCD_Unit in schema public   ###############################################################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "ABCD_Unit" AS 
 SELECT "ABCD__UnitNoPart"."ID",
    "ABCD__UnitNoPart"."UnitGUID",
    "ABCD__UnitNoPart"."SourceInstitutionID",
    "ABCD__UnitNoPart"."SourceID",
    "ABCD__UnitNoPart"."UnitID",
    "ABCD__UnitNoPart"."DateLastEdited",
    "ABCD__UnitNoPart"."Identification_Taxon_ScientificName_FullScientificName",
    "ABCD__UnitNoPart"."Identification_Taxon_ScientificName_Qualifier",
    "ABCD__UnitNoPart"."RecordBasis",
    "ABCD__UnitNoPart"."KindOfUnit",
    "ABCD__UnitNoPart"."Identification_Taxon_HigherTaxonName",
    "ABCD__UnitNoPart"."KindOfUnit_Language",
    "ABCD__UnitNoPart"."HerbariumUnit_Exsiccatum",
    "ABCD__UnitNoPart"."RecordURI",
    "ABCD__UnitNoPart"."CollectionSpecimenID"
   FROM "#project#"."ABCD__UnitNoPart"
   UNION
   SELECT "ABCD__UnitPart"."ID",
    "ABCD__UnitPart"."UnitGUID",
    "ABCD__UnitPart"."SourceInstitutionID",
    "ABCD__UnitPart"."SourceID",
    "ABCD__UnitPart"."UnitID",
    "ABCD__UnitPart"."DateLastEdited",
    "ABCD__UnitPart"."Identification_Taxon_ScientificName_FullScientificName",
    "ABCD__UnitPart"."Identification_Taxon_ScientificName_Qualifier",
    "ABCD__UnitPart"."RecordBasis",
    "ABCD__UnitPart"."KindOfUnit",
    "ABCD__UnitPart"."Identification_Taxon_HigherTaxonName",
    "ABCD__UnitPart"."KindOfUnit_Language",
    "ABCD__UnitPart"."HerbariumUnit_Exsiccatum",
    "ABCD__UnitPart"."RecordURI",
    "ABCD__UnitPart"."CollectionSpecimenID"
   FROM "#project#"."ABCD__UnitPart";

ALTER TABLE "ABCD_Unit"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_Unit" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_Unit" TO "CacheUser";
COMMENT ON VIEW "ABCD_Unit" IS 'ABCD entity /DataSets/DataSet/Units/Unit/';
COMMENT ON COLUMN "ABCD_Unit"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit"."UnitGUID" IS 'ABCD: Unit/DataSets/DataSet/Units/Unit/UnitGUID. Retrieved from StableIdentifier as defined by the basic address for stable identifiers in DiversityCollection extendend with the CollectionSpecimenID for the specimen and if present the IdentificationUnitID for the Unit and the SpecimenPartID for the part';
COMMENT ON COLUMN "ABCD_Unit"."SourceInstitutionID" IS 'ABCD: Unit/SourceInstitutionID. Retrieved from DiversityProjects - Settings - ABCD - Source - InstitutionID';
COMMENT ON COLUMN "ABCD_Unit"."SourceID" IS 'ABCD: Unit/SourceID. Retrieved from DiversityProjects - Settings - ABCD - Source - ID';
COMMENT ON COLUMN "ABCD_Unit"."UnitID" IS 'ABCD: Unit/UnitID. Retrieved from DiversityCollection: AccessionNumber of the specimen if present, otherwise the CollectionSpecimenID + '' / '' + IdentificationUnitID of the Unit + only if a part is present '' / '' + AccessionNumber of the part if present otherwise SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit"."DateLastEdited" IS 'ABCD: Unit/DateLastEdited. Retrieved from DiversityCollection from the column LogUpdatedWhen in table CollectionSpecimen';
COMMENT ON COLUMN "ABCD_Unit"."Identification_Taxon_ScientificName_FullScientificName" IS 'ABCD: Unit/Identifications/Identification/Result/TaxonIdentified/ScientificName/FullScientificNameString. Retrieved from DiversityCollection from the column LastIdentificationCache in table IdentificationUnit';
COMMENT ON COLUMN "ABCD_Unit"."Identification_Taxon_ScientificName_Qualifier" IS 'ABCD: Unit/Identifications/Identification/Result/TaxonIdentified/ScientificName/IdentificationQualifier. Retrieved from DiversityCollection from the column IdentificationQualifier in table Identification (Last valid identification)';
COMMENT ON COLUMN "ABCD_Unit"."RecordBasis" IS 'ABCD: Unit/RecordBasis. Retrieved from DiversityCollection. For observations without a part taken from column RetrievalType in table IdentificationUnit. If missing HumanObservation. For parts taken from the column MaterialCategory in table CollectionSpecimenPart translated according to GBIF definitions';
COMMENT ON COLUMN "ABCD_Unit"."KindOfUnit" IS 'ABCD: Unit/KindOfUnit. Retrieved from DiversityCollection. For parts taken from the column MaterialCategory in table CollectionSpecimenPart otherwise RetrievalType in table IdentificationUnit, if missing human observation';
COMMENT ON COLUMN "ABCD_Unit"."Identification_Taxon_HigherTaxonName" IS 'ABCD: Unit/Identifications/Identification/Result/TaxonIdentified/HigherTaxa/HigherTaxon/HigherTaxonName. Retrieved from DiversityCollection from column TaxonomicGroup in table IdentificationUnit, translated according to GBIF definitions';
COMMENT ON COLUMN "ABCD_Unit"."KindOfUnit_Language" IS 'ABCD: Unit/KindOfUnit. Retried from view = en';
COMMENT ON COLUMN "ABCD_Unit"."HerbariumUnit_Exsiccatum" IS 'ABCD: Unit/HerbariumUnit/Exsiccatum. Retrieved from DiversityCollection, column ExsiccataAbbreviation in table CollectionSpecimen';
COMMENT ON COLUMN "ABCD_Unit"."RecordURI" IS 'ABCD: Unit/RecordURI. Retrieved from DiversityProjects - Settings - ABCD - RecordURI extended with informations from DiversityCollection depending on the RecordURI: For http:biocase... AccessionNumber of the specimen if present otherwise CollectionSpecimenID + '' / '' + IdentificationUnitID of the Unit + if a part is present the AccessionNumber of the part otherwise the SpecimenPartID. For and other RecordURI extended with the CollectionSpecimenID for the specimen';
COMMENT ON COLUMN "ABCD_Unit"."CollectionSpecimenID" IS 'CollectionSpecimenID of the specimen';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD_Unit_Associations_UnitAssociation   ###################################################################
--#####################################################################################################################
--#####################################################################################################################


CREATE TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation"
(
  "AssociatedUnitID" character varying(254),
  "SourceInstitutionCode" character varying(254),
  "SourceName" character varying(254),
  "AssociationType" character varying(50),
  "AssociationType_Language" character varying(2),
  "Comment" text,
  "Comment_Language" character varying(2),
  "IdentificationUnitID" integer,
  "ID" character varying(25),
  "ID_Related" character varying(25),
  "UnitID" character varying(50),
  "UnitGUID" character varying(500),
  "KindOfUnit" character varying(254),
  "SpecimenPartID" integer,
  "CollectionSpecimenID" integer,
  CONSTRAINT "ABCD_Unit_Associations_UnitAssociation_pkey" PRIMARY KEY ("ID", "ID_Related")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation" TO "CacheUser";

COMMENT ON TABLE "#project#"."ABCD_Unit_Associations_UnitAssociation" IS 'ABCD entity /DataSets/DataSet/Units/Unit/Associations/UnitAssociation';


--#####################################################################################################################
--######   creating the index for ABCD_Unit_Associations_UnitAssociation  #############################################
--#####################################################################################################################

CREATE INDEX ABCD_Unit_Associations_UnitAssociation_ID
  ON "#project#"."ABCD_Unit_Associations_UnitAssociation" ("ID");


--#####################################################################################################################
--######   function abcd__Unit_Associations_UnitAssociation  ##########################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_Associations_UnitAssociation()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

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
     CROSS JOIN "#project#"."CacheMetadata" m WHERE m."ProjectID" = "#project#".projectid();

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
--######   filling ABCD_Unit_Associations_UnitAssociation with data  ##################################################
--#####################################################################################################################

--SELECT "#project#".abcd__Unit_Associations_UnitAssociation();

-- 6 sec


--#####################################################################################################################
--######   ABCD_Unit_Associations_UnitAssociation in schema public   ##################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_Unit_Associations_UnitAssociation" AS 
 SELECT "A"."AssociatedUnitID",
    "A"."SourceInstitutionCode",
    "A"."SourceName",
    "A"."AssociationType",
    "A"."AssociationType_Language",
    "A"."Comment",
    "A"."Comment_Language",
    "A"."IdentificationUnitID",
    "A"."ID",
    "A"."ID_Related",
    "A"."UnitID",
    "A"."UnitGUID",
    "A"."KindOfUnit",
    "A"."SpecimenPartID",
    "A"."CollectionSpecimenID"
   FROM "#project#"."ABCD_Unit_Associations_UnitAssociation" "A";

ALTER TABLE "ABCD_Unit_Associations_UnitAssociation"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_Unit_Associations_UnitAssociation" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_Unit_Associations_UnitAssociation" TO "CacheUser";
COMMENT ON VIEW "ABCD_Unit_Associations_UnitAssociation" IS 'ABCD entity /DataSets/DataSet/Units/Unit/Associations/UnitAssociation';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."AssociatedUnitID" IS 'ABCD: Unit/Associations/UnitAssociation/AssociatedUnitID. Retrieved from DiversityCollection: AccessionNumber of the associated specimen if present, otherwise the CollectionSpecimenID + '' / '' + IdentificationUnitID of the Unit + only if a part is present '' / '' + AccessionNumber of the part if present otherwise SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."SourceInstitutionCode" IS 'ABCD: Unit/Associations/UnitAssociation/AssociatedUnitSourceInstitutionCode. Retrieved from DiversityProjects - Settings - ABCD - Source - InstitutionID';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."SourceName" IS 'ABCD: Unit/Associations/UnitAssociation/AssociatedUnitSourceName. Retrieved from DiversityProjects, column Project in table Project';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."AssociationType" IS 'ABCD: Unit/Associations/UnitAssociation/AssociationType. Retrieved from DiversityCollection, column RelationType in table IdentificationUnit';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."AssociationType_Language" IS 'ABCD: Unit/Associations/UnitAssociation/AssociationType. Retrieved from View = en';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."Comment" IS 'ABCD: Unit/Associations/UnitAssociation/Comment. Retrieved from DiversityCollection, column RelationType + '' on '' + LastIdentificationCache from table IdentificationUnit from the related unit';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."Comment_Language" IS 'ABCD: Unit/Associations/UnitAssociation/Comment. Retrieved from View = en';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."IdentificationUnitID" IS 'Retrieved from DiversityCollection, column IdentificationUnitID from table IdentificationUnit';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."ID_Related" IS 'Unique ID for the related Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."UnitID" IS 'ABCD: Unit/Associations/UnitAssociation/AssociatedUnitID. Retrieved from DiversityCollection, CollectionSpecimenID of specimen + ''-'' +  IdentificationUnitID of unit + ''-'' + SpecimenPartID of part if part is present';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."UnitGUID" IS 'Stable identifer of part if part is present otherwise stable identifier of unit. StableIdentifier = Basic address defined in DiversityCollection + CollectionSpecimenID + ''/'' + IdentificationUnitID + if part is present ''/'' + SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."KindOfUnit" IS 'Retrieved from DiversityCollection, column MaterialCategory in table CollectionSpecimenPart';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."SpecimenPartID" IS 'Retrieved from DiversityCollection, column SpecimenPartID in table CollectionSpecimenPart';
COMMENT ON COLUMN "ABCD_Unit_Associations_UnitAssociation"."CollectionSpecimenID" IS 'Retrieved from DiversityCollection, column CollectionSpecimenID in table CollectionSpecimen';


--#####################################################################################################################
--######   ABCD__Country_ISO3166Code   ################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD__CountryName_ISO3166Code"
(
  "CountryName" character varying(254),
  "ISO3166Code" character varying(3),
  CONSTRAINT "ABCD__CountryName_ISO3166Code_pkey" PRIMARY KEY ("CountryName")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD__CountryName_ISO3166Code"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD__CountryName_ISO3166Code" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD__CountryName_ISO3166Code" TO "CacheUser";



--#####################################################################################################################
--#####################################################################################################################
--######   ABCD_Unit_Gathering   ######################################################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD_Unit_Gathering"
(
  "ID" character varying(25),
  "Country_Name" character varying(50),
  "ISO3166Code" character varying(3),
  "DateTime_ISODateTimeBegin" character varying(10),
  "LocalityText" text,
  "SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal" numeric,
  "SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal" numeric,
  "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName" character varying(50),
  "Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank" character varying(254),
  "IdentificationUnitID" integer,
  "SiteCoordinateSets_CoordinatesLatLong_SpatialDatum" character varying(50),  
  CONSTRAINT "ABCD_Unit_Gathering_pkey" PRIMARY KEY ("ID") 
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD_Unit_Gathering"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD_Unit_Gathering" TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD_Unit_Gathering" TO postgres;
GRANT SELECT ON TABLE "#project#"."ABCD_Unit_Gathering" TO "CacheUser";



--#####################################################################################################################
--######   function abcd__Unit_Gathering  #############################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_Gathering()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

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
     concat_ws('-'::text, COALESCE(e."CollectionYear"::character varying(4), '-'::character varying), lpad(e."CollectionMonth"::character varying(2)::text, 2, '0'::text), lpad(e."CollectionDay"::character varying(2)::text, 2, '0'::text))::character varying(10) AS "DateTime_ISODateTimeBegin",
    e."LocalityDescription" AS "LocalityText",
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
     JOIN "#project#"."CacheCollectionEvent" e ON e."CollectionEventID" = s."CollectionEventID";

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

SET ROLE "CacheAdmin";

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

/*     
UPDATE "#project#"."ABCD_Unit_Gathering" SET "ISO3166Code" = "G"."Name"::text
   FROM "Gazetteer" "C"
     JOIN "Gazetteer" "G" ON "C"."PlaceID" = "G"."PlaceID"
     JOIN "#project#"."ABCD_Unit_Gathering" "A" ON "A"."Country_Name" = "C"."Name"
  WHERE "G"."LanguageCode"::text = 'ISO 3166 ALPHA-3'::text AND ("C"."LanguageCode"::text !~~ 'ISO %'::text OR "C"."LanguageCode" IS NULL) AND "C"."NameID" <> "G"."NameID";
*/
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
--######   filling ABCD_Unit_Gathering with data  #####################################################################
--#####################################################################################################################

--SELECT "#project#".abcd__Unit_Gathering();

--867245 ms = 14 min
-- 879978 ms. = 15 min
-- test: select count(*) from "#project#"."ABCD_Unit_Gathering";
--4181051



--#####################################################################################################################
--######   ABCD_Unit_Gathering in schema public  ######################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_Unit_Gathering" AS 
 SELECT "G"."ID",
    "G"."Country_Name",
    "G"."ISO3166Code",
    "G"."DateTime_ISODateTimeBegin",
    "G"."LocalityText",
    "G"."SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal",
    "G"."SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal",
    "G"."Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName",
    "G"."Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank",
    "G"."IdentificationUnitID",
    "G"."SiteCoordinateSets_CoordinatesLatLong_SpatialDatum"
   FROM "#project#"."ABCD_Unit_Gathering" "G";

ALTER TABLE "ABCD_Unit_Gathering"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_Unit_Gathering" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_Unit_Gathering" TO "CacheUser";
COMMENT ON VIEW "ABCD_Unit_Gathering" IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."Country_Name" IS 'ABCD: Unit/Gathering/Country/Name. Retrieved from column CountryCache in table CollectionEvent';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."ISO3166Code" IS 'ABCD: Unit/Gathering/Country/ISO3166Code. Retrieved from DiversityGazetteer according to column CountryCache in table CollectionEvent';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."DateTime_ISODateTimeBegin" IS 'ABCD: Unit/Gathering/DateTime/ISODateTimeBegin. Retrieved from columns CollectionYear, CollectionMonth and CollectionDay in table CollectionEvent';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."LocalityText" IS 'ABCD: Unit/Gathering/LocalityText. Retrieved from column LocalityDescription in table CollectionEvent';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_LatitudeDecimal" IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinates/CoordinatesLatLong/LatitudeDecimal. Retrieved from column AverageLatitudeCache in table CollectionEventLocalisation depending on the sequence defined for the project';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_LongitudeDecimal" IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinates/CoordinatesLatLong/LongitudeDecimal. Retrieved from column AverageLongitudeCache in table CollectionEventLocalisation depending on the sequence defined for the project';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonName" IS 'ABCD: Unit/Gathering/Synecology/AssociatedTaxa/TaxonIdentified/HigherTaxa/HigherTaxon/HigherTaxonName. Obsolete';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."Synecology_AssociatedTaxa_TaxonIdentified_HigherTaxonRank" IS 'ABCD: Unit/Gathering/Synecology/AssociatedTaxa/TaxonIdentified/HigherTaxa/HigherTaxon/HigherTaxonRank. Obsolete';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."IdentificationUnitID" IS 'Retrieved from column IdentificationUnitID in table IdentificationUnit';
COMMENT ON COLUMN "ABCD_Unit_Gathering"."SiteCoordinateSets_CoordinatesLatLong_SpatialDatum" IS 'ABCD: Unit/Gathering/SiteCoordinateSets/SiteCoordinates/CoordinatesLatLong/SpatialDatum. Retrieved from View = WGS84';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD_Unit_Gathering_Agents   ###############################################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD_Unit_Gathering_Agents"
(
  "ID" character varying(25),
  "GatheringAgent_AgentText" character varying(350),
  "CollectionSpecimenID" integer,
  CONSTRAINT "ABCD_Unit_Gathering_Agents_pkey" PRIMARY KEY ("ID", "GatheringAgent_AgentText")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD_Unit_Gathering_Agents"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD_Unit_Gathering_Agents" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD_Unit_Gathering_Agents" TO "CacheUser";


--#####################################################################################################################
--######   creating an index for ABCD_Unit_Gathering_Agents  ##########################################################
--#####################################################################################################################

CREATE INDEX ABCD_Unit_Gathering_Agents_ID
  ON "#project#"."ABCD_Unit_Gathering_Agents" ("ID");


--#####################################################################################################################
--######   function abcd__Unit_Gathering_Agents  ######################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_Gathering_Agents()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- clean table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering_Agents";

-- remove key
ALTER TABLE "#project#"."ABCD_Unit_Gathering_Agents" DROP CONSTRAINT "ABCD_Unit_Gathering_Agents_pkey";
DROP INDEX "#project#".abcd_unit_gathering_agents_id;

-- insert data
INSERT INTO "#project#"."ABCD_Unit_Gathering_Agents"(
            "ID", "GatheringAgent_AgentText", "CollectionSpecimenID")
SELECT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    concat(a."CollectorsName", CASE WHEN a."CollectorsNumber" IS NULL OR a."CollectorsNumber" = '' THEN '' ELSE concat(' (no. ', a."CollectorsNumber", ')') END) AS "GatheringAgent_AgentText",
    a."CollectionSpecimenID"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionAgent" a ON a."CollectionSpecimenID" = u."CollectionSpecimenID";

-- add key
ALTER TABLE "#project#"."ABCD_Unit_Gathering_Agents"
  ADD CONSTRAINT "ABCD_Unit_Gathering_Agents_pkey" PRIMARY KEY("ID", "GatheringAgent_AgentText");
CREATE INDEX abcd_unit_gathering_agents_id
  ON "#project#"."ABCD_Unit_Gathering_Agents"
  USING btree
  ("ID" COLLATE pg_catalog."default");
       
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit_Gathering_Agents()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit_Gathering_Agents() TO "CacheAdmin";


--#####################################################################################################################
--######   filing ABCD_Unit_Gathering_Agents with data  ###############################################################
--#####################################################################################################################

--SELECT "#project#".abcd__Unit_Gathering_Agents();

--30666 ms.


--#####################################################################################################################
--######   ABCD_Unit_Gathering_Agents in schema public  ###############################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_Unit_Gathering_Agents" AS 
 SELECT "ABCD_Unit_Gathering_Agents"."ID",
    "ABCD_Unit_Gathering_Agents"."GatheringAgent_AgentText",
    "ABCD_Unit_Gathering_Agents"."CollectionSpecimenID"
   FROM "#project#"."ABCD_Unit_Gathering_Agents";

ALTER TABLE "ABCD_Unit_Gathering_Agents"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_Unit_Gathering_Agents" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_Unit_Gathering_Agents" TO "CacheUser";
COMMENT ON VIEW "ABCD_Unit_Gathering_Agents" IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering';
COMMENT ON COLUMN "ABCD_Unit_Gathering_Agents"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_Gathering_Agents"."GatheringAgent_AgentText" IS 'ABCD: Unit/Gathering/Agents/GatheringAgent/AgentText. Retrieved from columns CollectorsName + if present '' (no. '' + CollectorsNumber + '')'' in table CollectionAgent';
COMMENT ON COLUMN "ABCD_Unit_Gathering_Agents"."CollectionSpecimenID" IS 'Retrieved from column CollectionSpecimenID in table CollectionAgent';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm   #################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"
(
  "ID" character varying(25),
  "Term" character varying(255),
  CONSTRAINT "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm_pkey" PRIMARY KEY ("ID", "Term")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm" TO "CacheUser";

COMMENT ON TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"
  IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering/Stratigraphy/ChronostratigraphicTerms/ChronostratigraphicTerm/';
COMMENT ON COLUMN "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"."ID" IS 'Unique identifier for the data';


--#####################################################################################################################
--######   creating index for ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm  ###############################
--#####################################################################################################################

CREATE UNIQUE INDEX ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm_ID
  ON "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm" ("ID");
  

--#####################################################################################################################
--######   function abcd__Unit_Gathering_Stratigraphy_ChronostratigraphicTerm  ########################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_Gathering_Stratigraphy_ChronostratigraphicTerm()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- clean table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm";

-- remove key
ALTER TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm" DROP CONSTRAINT "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm_pkey";
DROP INDEX "#project#".abcd_unit_gathering_stratigraphy_chronostratigraphicterm_id;

-- insert data
INSERT INTO "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"(
            "ID", "Term")
SELECT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    e."DisplayText" AS "Term"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEventProperty" e ON e."CollectionEventID" = s."CollectionEventID"
  WHERE e."PropertyID" = 20;

-- add key
ALTER TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"
  ADD CONSTRAINT "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm_pkey" PRIMARY KEY("ID", "Term");
CREATE UNIQUE INDEX abcd_unit_gathering_stratigraphy_chronostratigraphicterm_id
  ON "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"
  USING btree
  ("ID" COLLATE pg_catalog."default");
                
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit_Gathering_Stratigraphy_ChronostratigraphicTerm()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit_Gathering_Stratigraphy_ChronostratigraphicTerm() TO "CacheAdmin";


--#####################################################################################################################
--######   filling ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm  with data  ###############################
--#####################################################################################################################

--SELECT "#project#".abcd__Unit_Gathering_Stratigraphy_ChronostratigraphicTerm();


--#####################################################################################################################
--######   ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm  in schema public   ###############################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm" AS 
 SELECT "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"."ID",
    "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"."Term"
   FROM "#project#"."ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm";

ALTER TABLE "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm" TO "CacheUser";
COMMENT ON VIEW "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm" IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering';
COMMENT ON COLUMN "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm"."Term" IS 'ABCD: Unit/Gathering/Stratigraphy/ChronostratigraphicTerms/ChronostratigraphicTerm. Retrieved from column DisplayText in table CacheCollectionEventProperty for Property Chronostratigraphy';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm   ##################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"
(
  "ID" character varying(25),
  "Term" character varying(255),
  CONSTRAINT "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm_pkey" PRIMARY KEY ("ID", "Term")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" TO "CacheUser";

COMMENT ON TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering/Stratigraphy/LithostratigraphicTerms/LithostratigraphicTerm/';
COMMENT ON COLUMN "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"."ID" IS 'Unique identifier for the data';


--#####################################################################################################################
--######   create index for ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm   #################################
--#####################################################################################################################

CREATE UNIQUE INDEX ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm_ID
  ON "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" ("ID");


--#####################################################################################################################
--######   function abcd__Unit_Gathering_Stratigraphy_LithostratigraphicTerm  #########################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_Gathering_Stratigraphy_LithostratigraphicTerm()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- clean table
TRUNCATE TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm";

-- remove key
ALTER TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" DROP CONSTRAINT "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm_pkey";
DROP INDEX "#project#".abcd_unit_gathering_stratigraphy_lithostratigraphicterm_id;

-- insert data
INSERT INTO  "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"(
            "ID", "Term")
SELECT concat(u."IdentificationUnitID"::character varying, '-', up."SpecimenPartID"::character varying) AS "ID",
    e."DisplayText" AS "Term"
   FROM "#project#"."CacheIdentificationUnitInPart" up
     RIGHT JOIN "#project#"."CacheIdentificationUnit" u ON u."IdentificationUnitID" = up."IdentificationUnitID"
     JOIN "#project#"."CacheCollectionSpecimen" s ON u."CollectionSpecimenID" = s."CollectionSpecimenID"
     JOIN "#project#"."CacheCollectionEventProperty" e ON e."CollectionEventID" = s."CollectionEventID"
  WHERE e."PropertyID" = 30;

-- Add key
ALTER TABLE "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"
  ADD CONSTRAINT "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm_pkey" PRIMARY KEY("ID", "Term");
CREATE UNIQUE INDEX abcd_unit_gathering_stratigraphy_lithostratigraphicterm_id
  ON "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"
  USING btree
  ("ID" COLLATE pg_catalog."default");
              
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit_Gathering_Stratigraphy_LithostratigraphicTerm()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit_Gathering_Stratigraphy_LithostratigraphicTerm() TO "CacheAdmin";


--#####################################################################################################################
--######   filling ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm with data  #################################
--#####################################################################################################################

--SELECT "#project#".abcd__Unit_Gathering_Stratigraphy_LithostratigraphicTerm();



--#####################################################################################################################
--######   ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm in schema public   #################################
--#####################################################################################################################


CREATE OR REPLACE VIEW "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" AS 
 SELECT "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"."ID",
    "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"."Term"
   FROM "#project#"."ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm";

ALTER TABLE "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" TO "CacheUser";
COMMENT ON VIEW "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm" IS 'ABCD entity /DataSets/DataSet/Units/Unit/Gathering';
COMMENT ON COLUMN "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm"."Term" IS 'ABCD: Unit/Gathering/Stratigraphy/LithostratigraphicTerms/LithostratigraphicTerm. Retrieved from column DisplayText in table CacheCollectionEventProperty for Property Lithostratigraphy';


--#####################################################################################################################
--#####################################################################################################################
--######   ABCD_Unit_SpecimenUnit   ###################################################################################
--#####################################################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."ABCD_Unit_SpecimenUnit"
(
  "ID" character varying(25),
  "NomenclaturalTypeDesignation_TypifiedName_FullScientificName" character varying(255),
  "NomenclaturalTypeDesignation_TypeStatus" character varying(50),
  "Preparation_PreparationType" character varying(50),
  CONSTRAINT "ABCD_Unit_SpecimenUnit_pkey" PRIMARY KEY ("ID")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "#project#"."ABCD_Unit_SpecimenUnit"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."ABCD_Unit_SpecimenUnit" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."ABCD_Unit_SpecimenUnit" TO "CacheUser";
COMMENT ON TABLE "#project#"."ABCD_Unit_SpecimenUnit" IS 'ABCD search type within an identification unit';
COMMENT ON COLUMN "#project#"."ABCD_Unit_SpecimenUnit"."NomenclaturalTypeDesignation_TypifiedName_FullScientificName" IS 'ABCD entity /DataSets/DataSet/Units/Unit/SpecimenUnit/NomenclaturalTypeDesignations/NomenclaturalTypeDesignation/TypifiedName/FullScientificNameString';
COMMENT ON COLUMN "#project#"."ABCD_Unit_SpecimenUnit"."NomenclaturalTypeDesignation_TypeStatus" IS 'ABCD entity /DataSets/DataSet/Units/Unit/SpecimenUnit/NomenclaturalTypeDesignations/NomenclaturalTypeDesignation/TypeStatus';
COMMENT ON COLUMN "#project#"."ABCD_Unit_SpecimenUnit"."Preparation_PreparationType" IS 'ABCD entity /DataSets/DataSet/Units/Unit/SpecimenUnit/Preparations/Preparation/PreparationType';


--#####################################################################################################################
--######  creating index for ABCD_Unit_SpecimenUnit   #################################################################
--#####################################################################################################################

CREATE UNIQUE INDEX ABCD_Unit_SpecimenUnit_ID
  ON "#project#"."ABCD_Unit_SpecimenUnit" ("ID");

--#####################################################################################################################
--######   function abcd__Unit_SpecimenUnit  ##########################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION "#project#".abcd__Unit_SpecimenUnit()
  RETURNS void AS
$BODY$
begin

--Setting the role
SET ROLE "CacheAdmin";

-- clean table
TRUNCATE TABLE "#project#"."ABCD_Unit_SpecimenUnit";

-- remove key
ALTER TABLE "#project#"."ABCD_Unit_SpecimenUnit" DROP CONSTRAINT "ABCD_Unit_SpecimenUnit_pkey";
DROP INDEX "#project#".abcd_unit_specimenunit_id;

-- insert data
INSERT INTO "#project#"."ABCD_Unit_SpecimenUnit"(
            "ID", "NomenclaturalTypeDesignation_TypifiedName_FullScientificName", 
            "NomenclaturalTypeDesignation_TypeStatus", "Preparation_PreparationType")
SELECT concat(t."IdentificationUnitID"::character varying, '-', p."SpecimenPartID"::character varying) AS "ID",
    t."TaxonomicName" AS "NomenclaturalTypeDesignation_TypifiedName_FullScientificName",
    t."TypeStatus" AS "NomenclaturalTypeDesignation_TypeStatus",
    p."PreparationType" AS "Preparation_PreparationType"
   FROM "#project#"."ABCD__2_Unit_Type" t
     LEFT JOIN "#project#"."ABCD__RecordBasis" p ON t."IdentificationUnitID" = p."IdentificationUnitID";
     
-- add key
ALTER TABLE "#project#"."ABCD_Unit_SpecimenUnit"
  ADD CONSTRAINT "ABCD_Unit_SpecimenUnit_pkey" PRIMARY KEY("ID");
CREATE UNIQUE INDEX abcd_unit_specimenunit_id
  ON "#project#"."ABCD_Unit_SpecimenUnit"
  USING btree
  ("ID" COLLATE pg_catalog."default");
                   
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "#project#".abcd__Unit_SpecimenUnit()
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION "#project#".abcd__Unit_SpecimenUnit() TO "CacheAdmin";


--#####################################################################################################################
--######  filling ABCD_Unit_SpecimenUnit with data  ###################################################################
--#####################################################################################################################

--SELECT "#project#".abcd__Unit_SpecimenUnit();



--#####################################################################################################################
--######   ABCD_Unit_SpecimenUnit in schema public   ##################################################################
--#####################################################################################################################

CREATE OR REPLACE VIEW "ABCD_Unit_SpecimenUnit" AS 
 SELECT "ABCD_Unit_SpecimenUnit"."ID",
    "ABCD_Unit_SpecimenUnit"."NomenclaturalTypeDesignation_TypifiedName_FullScientificName",
    "ABCD_Unit_SpecimenUnit"."NomenclaturalTypeDesignation_TypeStatus",
    "ABCD_Unit_SpecimenUnit"."Preparation_PreparationType"
   FROM "#project#"."ABCD_Unit_SpecimenUnit";

ALTER TABLE "ABCD_Unit_SpecimenUnit"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "ABCD_Unit_SpecimenUnit" TO "CacheAdmin";
GRANT SELECT ON TABLE "ABCD_Unit_SpecimenUnit" TO "CacheUser";
COMMENT ON VIEW "ABCD_Unit_SpecimenUnit" IS 'ABCD search type within an identification unit';
COMMENT ON COLUMN "ABCD_Unit_SpecimenUnit"."ID" IS 'Unique ID for the Unit, combined from IdentificationUnitID and SpecimenPartID';
COMMENT ON COLUMN "ABCD_Unit_SpecimenUnit"."NomenclaturalTypeDesignation_TypifiedName_FullScientificName" IS 'ABCD: /DataSets/DataSet/Units/Unit/SpecimenUnit/NomenclaturalTypeDesignations/NomenclaturalTypeDesignation/TypifiedName/FullScientificNameString. Retrieved from TaxonomicName in table Identification';
COMMENT ON COLUMN "ABCD_Unit_SpecimenUnit"."NomenclaturalTypeDesignation_TypeStatus" IS 'ABCD: /DataSets/DataSet/Units/Unit/SpecimenUnit/NomenclaturalTypeDesignations/NomenclaturalTypeDesignation/TypeStatus. Retrieved from column TypeStatus in table Identification';
COMMENT ON COLUMN "ABCD_Unit_SpecimenUnit"."Preparation_PreparationType" IS 'ABCD: /DataSets/DataSet/Units/Unit/SpecimenUnit/Preparations/Preparation/PreparationType. Retrieved from column MaterialCategory in table CollectionSpecimenPart and translated according to GBIF definitions resp. no treatment for observations';


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 2 WHERE "Package" = 'ABCD'
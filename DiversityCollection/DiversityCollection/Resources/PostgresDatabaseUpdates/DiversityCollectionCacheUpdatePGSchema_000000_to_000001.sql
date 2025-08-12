--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 1
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   Roles within Schema   ######################################################################################
--#####################################################################################################################


--######   User   #####################################################################################################
--###### this is obsolet and will no longer be used ###################################################################
--#####################################################################################################################

--CREATE or replace FUNCTION MakeCacheUser() RETURNS void AS
--$BODY$
--declare i INTEGER := 0;
--begin
--SELECT count(*) FROM pg_roles R where R.rolname = 'CacheUser_#project#' into i;
--if i = 0
--then
--	CREATE ROLE "CacheUser_#project#" VALID UNTIL 'infinity' ADMIN "CacheAdmin";
--	GRANT "CacheUser_#project#" TO "CacheAdmin" WITH ADMIN OPTION;
--end if;
--end;
--$BODY$ LANGUAGE plpgsql;

--SELECT MakeCacheUser(); 

--DROP FUNCTION MakeCacheUser();

--GRANT "CacheUser_#project#" TO CURRENT_USER WITH ADMIN OPTION;


--######   Editor   ######################################################################################

--CREATE or replace FUNCTION MakeCacheEditor() RETURNS void AS
--$BODY$
--declare i INTEGER := 0;
--begin
--SELECT count(*) FROM pg_roles R where R.rolname = 'CacheEditor_#project#' into i;
--if i = 0
--then
--	CREATE ROLE "CacheEditor_#project#" IN ROLE "CacheUser_#project#"  VALID UNTIL 'infinity';
--end if;
--end;
--$BODY$ LANGUAGE plpgsql;

--SELECT MakeCacheEditor(); 

--DROP FUNCTION MakeCacheEditor();


--######   Admin   ####################################################################################################
--###### this is obsolet and will no longer be used ###################################################################
--#####################################################################################################################

--CREATE or replace FUNCTION MakeCacheAdmin() RETURNS void AS
--$BODY$
--declare i INTEGER := 0;
--begin
--SELECT count(*) FROM pg_roles R where R.rolname = 'CacheAdmin_#project#' into i;
--if i = 0
--then
--      CREATE ROLE "CacheAdmin_#project#" with CREATEDB CREATEROLE IN ROLE "CacheUser_#project#" VALID UNTIL 'infinity' ADMIN "CacheAdmin"; 
--	  GRANT "CacheAdmin_#project#" TO "CacheAdmin" WITH ADMIN OPTION;
--end if;
--end;
--$BODY$ LANGUAGE plpgsql;

--SELECT MakeCacheAdmin(); 

--DROP FUNCTION MakeCacheAdmin();

--GRANT "CacheAdmin_#project#" TO CURRENT_USER WITH ADMIN OPTION;

--#####################################################################################################################

--GRANT USAGE ON SCHEMA "#project#" TO "CacheUser_#project#";
GRANT USAGE ON SCHEMA "#project#" TO "CacheUser";

--#####################################################################################################################

--ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
--    GRANT SELECT ON TABLES
--    TO "CacheUser_#project#";
ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
    GRANT SELECT ON TABLES
    TO "CacheUser";


--#####################################################################################################################

--ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
--    GRANT EXECUTE ON FUNCTIONS
--    TO "CacheUser_#project#";
ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
    GRANT EXECUTE ON FUNCTIONS
    TO "CacheUser";

    
--#####################################################################################################################

--ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
--    GRANT ALL ON TABLES
--    TO "CacheAdmin_#project#";
ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
    GRANT ALL ON TABLES
    TO "CacheAdmin";


--#####################################################################################################################

--ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
--    GRANT ALL ON FUNCTIONS
--    TO "CacheAdmin_#project#";
ALTER DEFAULT PRIVILEGES IN SCHEMA "#project#"
    GRANT ALL ON FUNCTIONS
    TO "CacheAdmin";



--#####################################################################################################################
--######   main Roles   ###############################################################################################
--#####################################################################################################################


--GRANT "CacheUser_#project#" TO "CacheUser";
--GRANT "CacheAdmin_#project#" TO "CacheAdmin";



--#####################################################################################################################
--######   CacheMetadata   ############################################################################################
--#####################################################################################################################


CREATE TABLE "#project#"."CacheMetadata"
(
  "ProjectID" integer NOT NULL,
  "ProjectTitleCode" character varying(254),
  "TaxonomicGroup" character varying(254),
  "DatasetGUID" character varying(254),
  "TechnicalContactName" character varying(254),
  "TechnicalContactEmail" character varying(254),
  "TechnicalContactPhone" character varying(254),
  "TechnicalContactAddress" character varying(254),
  "ContentContactName" character varying(254),
  "ContentContactEmail" character varying(254),
  "ContentContactPhone" character varying(254),
  "ContentContactAddress" character varying(254),
  "OtherProviderUDDI" character varying(254),
  "DatasetTitle" character varying(254),
  "DatasetDetails" character varying(254),
  "DatasetCoverage" character varying(254),
  "DatasetURI" character varying(254),
  "DatasetIconURI" character varying(254),
  "DatasetVersionMajor" character varying(254),
  "DatasetCreators" character varying(254),
  "DatasetContributors" character varying(254),
  "DateCreated" character varying(254),
  "DateModified" character varying(254),
  "SourceID" character varying(254),
  "SourceInstitutionID" character varying(254),
  "OwnerOrganizationName" character varying(254),
  "OwnerOrganizationAbbrev" character varying(254),
  "OwnerContactPerson" character varying(254),
  "OwnerContactRole" character varying(254),
  "OwnerAddress" character varying(254),
  "OwnerTelephone" character varying(254),
  "OwnerEmail" character varying(254),
  "OwnerURI" character varying(254),
  "OwnerLogoURI" character varying(254),
  "IPRText" character varying(254),
  "IPRDetails" character varying(254),
  "IPRURI" character varying(254),
  "CopyrightText" character varying(254),
  "CopyrightDetails" character varying(254),
  "CopyrightURI" character varying(254),
  "TermsOfUseText" character varying(254),
  "TermsOfUseDetails" character varying(254),
  "TermsOfUseURI" character varying(254),
  "DisclaimersText" character varying(254),
  "DisclaimersDetails" character varying(254),
  "DisclaimersURI" character varying(254),
  "LicenseText" character varying(254),
  "LicensesDetails" character varying(254),
  "LicenseURI" character varying(254),
  "AcknowledgementsText" character varying(254),
  "AcknowledgementsDetails" character varying(254),
  "AcknowledgementsURI" character varying(254),
  "CitationsText" character varying(254),
  "CitationsDetails" character varying(254),
  "CitationsURI" character varying(254),
  "RecordBasis" character varying(254),
  "KindOfUnit" character varying(254),
  "HigherTaxonRank" character varying(254),
  CONSTRAINT "ABCD_Metadata_pkey" PRIMARY KEY ("ProjectID")
);
ALTER TABLE "#project#"."CacheMetadata"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheMetadata" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheMetadata" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheMetadata" TO "CacheUser";


--#####################################################################################################################
--######   CacheCollectionSpecimenPart   ######################################################################################
--#####################################################################################################################



CREATE TABLE "#project#"."CacheCollectionSpecimenPart"
(
  	"CollectionSpecimenID" integer NOT NULL,
	"SpecimenPartID" integer NOT NULL,
	"DerivedFromSpecimenPartID" integer NULL,
	"PreparationMethod" text NULL,
	"PreparationDate" timestamp without time zone NULL,
	"PartSublabel" character varying(50) NULL,
	"CollectionID" integer NOT NULL,
	"MaterialCategory" character varying(50) NOT NULL,
	"StorageLocation" character varying(255) NULL,
	"Stock" double precision NULL,
	"Notes" text NULL,
	"AccessionNumber" character varying(50) NULL,
	"StorageContainer" character varying(500) NULL,
	"StockUnit" character varying(50) NULL,
	"ResponsibleName" character varying(255) NULL,
  CONSTRAINT "CacheCollectionSpecimenPart_pkey" PRIMARY KEY ("CollectionSpecimenID", "SpecimenPartID")
);
ALTER TABLE "#project#"."CacheCollectionSpecimenPart"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenPart" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheCollectionSpecimenPart" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenPart" TO GROUP "CacheUser";



--#####################################################################################################################
--######   CacheCollectionSpecimen   ######################################################################################
--#####################################################################################################################



CREATE TABLE "#project#"."CacheCollectionSpecimen"
(
	"CollectionSpecimenID" integer NOT NULL,
	"LabelTranscriptionNotes" character varying(255) NULL,
	"OriginalNotes" text NULL,
	"LogUpdatedWhen" timestamp without time zone NULL,
	"CollectionEventID" integer NULL,
	"AccessionNumber" character varying(50) NULL,
	"AccessionDate" timestamp without time zone NULL,
	"AccessionDay" smallint NULL,
	"AccessionMonth" smallint NULL,
	"AccessionYear" smallint NULL,
	"DepositorsName" character varying(255) NULL,
	"DepositorsAccessionNumber" character varying(50) NULL,
	"ExsiccataURI" character varying(255) NULL,
	"ExsiccataAbbreviation" character varying(255) NULL,
	"AdditionalNotes" text NULL,
	"ReferenceTitle" character varying(255) NULL,
	"ReferenceURI" character varying(255) NULL,
  CONSTRAINT "CacheCollectionSpecimen_pkey" PRIMARY KEY ("CollectionSpecimenID")
);
ALTER TABLE "#project#"."CacheCollectionSpecimen"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimen" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimen" TO GROUP "CacheUser";


--#####################################################################################################################
--######   CacheCollectionAgent   ######################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionAgent"
(
 	"CollectionSpecimenID" integer NOT NULL,
	"CollectorsName" character varying(255) NOT NULL,
	"CollectorsSequence" timestamp without time zone NULL,
	"CollectorsNumber" character varying(50) NULL,
  CONSTRAINT "CacheCollectionAgent_pkey" PRIMARY KEY ("CollectionSpecimenID", "CollectorsName")
);
ALTER TABLE "#project#"."CacheCollectionAgent"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionAgent" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheCollectionAgent" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheCollectionAgent" TO "CacheUser";


--#####################################################################################################################
--######   CacheCollectionEvent   ######################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionEvent"(
	"CollectionEventID" integer NOT NULL,
	"Version" integer NOT NULL,
	"CollectorsEventNumber" character varying(50) NULL,
	"CollectionDate" timestamp without time zone NULL,
	"CollectionDay" smallint NULL,
	"CollectionMonth" smallint NULL,
	"CollectionYear" smallint NULL,
	"CollectionDateSupplement" character varying(100) NULL,
	"CollectionTime" character varying(50) NULL,
	"CollectionTimeSpan" character varying(50) NULL,
	"LocalityDescription" text NULL,
	"HabitatDescription" text NULL,
	"ReferenceTitle" character varying(255) NULL,
	"CollectingMethod" text NULL,
	"Notes" text NULL,
	"CountryCache" character varying(50) NULL,
	"ReferenceDetails" character varying(50) NULL,
	"LocalityVerbatim" text NULL,
	"CollectionEndDay" smallint NULL,
	"CollectionEndMonth" smallint NULL,
	"CollectionEndYear" smallint NULL,
 CONSTRAINT "CacheCollectionEvent_pkey" PRIMARY KEY ("CollectionEventID" )
 );

ALTER TABLE "#project#"."CacheCollectionEvent"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionEvent" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheCollectionEvent" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheCollectionEvent" TO "CacheUser";


--#####################################################################################################################
--######   CacheCollectionEventLocalisation   #########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionEventLocalisation"(
	"CollectionEventID" integer NOT NULL,
	"LocalisationSystemID" integer NOT NULL,
	"Location1" character varying(255) NULL,
	"Location2" character varying(255) NULL,
	"LocationAccuracy" character varying(50) NULL,
	"LocationNotes" text NULL,
	"DeterminationDate" timestamp with time zone NULL,
	"DistanceToLocation" character varying(50) NULL,
	"DirectionToLocation" character varying(50) NULL,
	"ResponsibleName" character varying(255) NULL,
	"ResponsibleAgentURI" character varying(255) NULL,
	"AverageAltitudeCache" double precision NULL,
	"AverageLatitudeCache" double precision NULL,
	"AverageLongitudeCache" double precision NULL,
	"RecordingMethod" character varying(500) NULL,
 CONSTRAINT "CacheCollectionEventLocalisation_pkey" PRIMARY KEY (	"CollectionEventID" ,	"LocalisationSystemID" ) 
 );

 ALTER TABLE "#project#"."CacheCollectionEventLocalisation"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionEventLocalisation" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheCollectionEventLocalisation" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheCollectionEventLocalisation" TO "CacheUser";


--#####################################################################################################################
--######   CacheIdentificationUnitAnalysis   ##########################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheIdentificationUnitAnalysis"(
	"CollectionSpecimenID" integer NOT NULL,
	"IdentificationUnitID" integer NOT NULL,
	"AnalysisID" integer NOT NULL,
	"AnalysisNumber" character varying(50) NOT NULL,
	"AnalysisResult" text NULL,
	"ExternalAnalysisURI" character varying(255) NULL,
	"ResponsibleName" character varying(255) NULL,
	"ResponsibleAgentURI" character varying(255) NULL,
	"AnalysisDate" character varying(50) NULL,
	"SpecimenPartID" integer NULL,
	"Notes" text NULL,
 CONSTRAINT "CacheIdentificationUnitAnalysis_pkey" PRIMARY KEY ("CollectionSpecimenID","IdentificationUnitID","AnalysisID","AnalysisNumber") 
 );

 ALTER TABLE "#project#"."CacheIdentificationUnitAnalysis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheIdentificationUnitAnalysis" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheIdentificationUnitAnalysis" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheIdentificationUnitAnalysis" TO "CacheUser";


--#####################################################################################################################
--######   CacheCollectionSpecimenImage   ######################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheCollectionSpecimenImage"
(
	"CollectionSpecimenID" integer NOT NULL,
	"URI" character varying(255) NOT NULL,
	"ResourceURI" character varying(255) NULL,
	"SpecimenPartID" integer NULL,
	"IdentificationUnitID" integer NULL,
	"ImageType" character varying(50) NULL,
	"Notes" text NULL,
	"LicenseURI" character varying(500) NULL,
	"LicenseNotes" character varying(500) NULL,
	"DisplayOrder" integer NULL,
	"LicenseYear" character varying(50) NULL,
	"LicenseHolderAgentURI" character varying(500) NULL,
	"LicenseHolder" character varying(500) NULL,
	"LicenseType" character varying(500) NULL,
	"CopyrightStatement" character varying(500) NULL,
	"CreatorAgentURI" character varying(255) NULL,
	"CreatorAgent" character varying(500) NULL,
	"IPR" character varying(500) NULL,
	"Title" character varying(500) NULL,
  CONSTRAINT "CacheCollectionSpecimenImage_pkey" PRIMARY KEY ("CollectionSpecimenID", "URI")
);
ALTER TABLE "#project#"."CacheCollectionSpecimenImage"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenImage" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheCollectionSpecimenImage" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenImage" TO "CacheUser";



--#####################################################################################################################
--######   CacheIdentificationUnit   ######################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheIdentificationUnit"(
	"CollectionSpecimenID" integer NOT NULL,
	"IdentificationUnitID" integer NOT NULL,
	"LastIdentificationCache" character varying(255) NOT NULL,
	"TaxonomicGroup" character varying(50) NOT NULL,
	"RelatedUnitID" integer NULL,
	"RelationType" character varying(50) NULL,
	"ExsiccataNumber" character varying(50) NULL,
	"DisplayOrder" smallint NOT NULL,
	"ColonisedSubstratePart" character varying(255) NULL,
	"FamilyCache" character varying(255) NULL,
	"OrderCache" character varying(255) NULL,
	"LifeStage" character varying(255) NULL,
	"Gender" character varying(50) NULL,
	"HierarchyCache" character varying(500) NULL,
	"UnitIdentifier" character varying(50) NULL,
	"UnitDescription" character varying(50) NULL,
	"Circumstances" character varying(50) NULL,
	"Notes" text NULL,
	"NumberOfUnits" smallint NULL,
	"OnlyObserved" character(10) NULL,
CONSTRAINT "CacheIdentificationUnit_pkey" PRIMARY KEY ("CollectionSpecimenID", "IdentificationUnitID") 
 );

ALTER TABLE "#project#"."CacheIdentificationUnit"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheIdentificationUnit" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheIdentificationUnit" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheIdentificationUnit" TO GROUP "CacheUser";



--#####################################################################################################################
--######   CacheAnalysis   ######################################################################################
--#####################################################################################################################

CREATE TABLE "#project#"."CacheAnalysis"(
	"AnalysisID" integer NOT NULL,
	"AnalysisParentID" integer NULL,
	"DisplayText" character varying(50) NULL,
	"Description" text NULL,
	"MeasurementUnit" character varying(50) NULL,
	"Notes" text NULL,
	"AnalysisURI" character varying(255) NULL,
 CONSTRAINT "CacheAnalysis_pkey" PRIMARY KEY ("AnalysisID") 
 );

ALTER TABLE "#project#"."CacheAnalysis"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheAnalysis" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheAnalysis" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheAnalysis" TO GROUP "CacheUser";


--#####################################################################################################################
--######   CacheIdentificationUnitInPart   ######################################################################################
--#####################################################################################################################



CREATE TABLE "#project#"."CacheIdentificationUnitInPart"(
	"CollectionSpecimenID" integer NOT NULL,
	"IdentificationUnitID" integer NOT NULL,
	"SpecimenPartID" integer NOT NULL,
	"DisplayOrder" smallint NOT NULL,
	"Description" character varying(500) NULL,
 CONSTRAINT "CacheIdentificationUnitInPart_pkey" PRIMARY KEY ("CollectionSpecimenID", "IdentificationUnitID", "SpecimenPartID") 
 );

ALTER TABLE "#project#"."CacheIdentificationUnitInPart"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheIdentificationUnitInPart" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheIdentificationUnitInPart" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheIdentificationUnitInPart" TO GROUP "CacheUser";



--#####################################################################################################################
--######   CacheCollectionEventProperty  ######################################################################################
--#####################################################################################################################


CREATE TABLE "#project#"."CacheCollectionEventProperty"(
	"CollectionEventID" integer NOT NULL,
	"PropertyID" integer NOT NULL,
	"DisplayText" character varying(255) NULL,
	"PropertyURI" character varying(255) NULL,
	"PropertyHierarchyCache" text NULL,
	"PropertyValue" character varying(255) NULL,
	"ResponsibleName" character varying(255) NULL,
	"ResponsibleAgentURI" character varying(255) NULL,
	"Notes" text NULL,
	"AverageValueCache" double precision NULL,
CONSTRAINT "CacheCollectionEventProperty_pkey" PRIMARY KEY ("CollectionEventID", "PropertyID") 
 );

ALTER TABLE "#project#"."CacheCollectionEventProperty"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionEventProperty" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheCollectionEventProperty" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheCollectionEventProperty" TO GROUP "CacheUser";




--#####################################################################################################################
--######   CacheCollectionSpecimenRelation  ######################################################################################
--#####################################################################################################################


CREATE TABLE "#project#"."CacheCollectionSpecimenRelation"(
	"CollectionSpecimenID" integer NOT NULL,
	"RelatedSpecimenURI" character varying(255) NOT NULL,
	"RelatedSpecimenDisplayText" character varying(255) NOT NULL,
	"RelationType" character varying(50) NULL,
	"RelatedSpecimenCollectionID" integer NULL,
	"RelatedSpecimenDescription" text NULL,
	"Notes" text NULL,
	"IdentificationUnitID" integer NULL,
	"SpecimenPartID" integer NULL,
CONSTRAINT "CacheCollectionSpecimenRelation_pkey" PRIMARY KEY ("CollectionSpecimenID", "RelatedSpecimenURI", "RelatedSpecimenDisplayText") 
 );

ALTER TABLE "#project#"."CacheCollectionSpecimenRelation"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheCollectionSpecimenRelation" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheCollectionSpecimenRelation" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheCollectionSpecimenRelation" TO GROUP "CacheUser";



--#####################################################################################################################
--######   CacheIdentification  ######################################################################################
--#####################################################################################################################


CREATE TABLE "#project#"."CacheIdentification"(
	"CollectionSpecimenID" integer NOT NULL,
	"IdentificationUnitID" integer NOT NULL,
	"IdentificationSequence" smallint NOT NULL DEFAULT ((1)),
	"IdentificationDate" timestamp without time zone NULL,
	"IdentificationDay" smallint NULL,
	"IdentificationMonth" smallint NULL,
	"IdentificationYear" smallint NULL,
	"IdentificationDateSupplement" character varying(255) NULL,
	"IdentificationDateCategory" character varying(50) NULL,
	"VernacularTerm" character varying(255) NULL,
	"TaxonomicName" character varying(255) NULL,
	"NameURI" character varying(255) NULL,
	"IdentificationCategory" character varying(50) NULL,
	"IdentificationQualifier" character varying(50) NULL,
	"TypeStatus" character varying(50) NULL,
	"TypeNotes" text NULL,
	"ReferenceTitle" character varying(255) NULL,
	"ReferenceURI" character varying(255) NULL,
	"ReferenceDetails" character varying(50) NULL,
	"Notes" text NULL,
	"ResponsibleName" character varying(255) NULL,
	"ResponsibleAgentURI" character varying(255) NULL,
CONSTRAINT "CacheIdentification_pkey" PRIMARY KEY ("CollectionSpecimenID", "IdentificationUnitID", "IdentificationSequence") 
 );

ALTER TABLE "#project#"."CacheIdentification"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."CacheIdentification" TO "CacheAdmin";
--GRANT INSERT, DELETE ON TABLE "#project#"."CacheIdentification" TO "CacheEditor";
GRANT SELECT ON TABLE "#project#"."CacheIdentification" TO GROUP "CacheUser";





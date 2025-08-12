

--#####################################################################################################################
--######   TaxonAnalysis: adding Notes, AnalysisValue -> Text   #######################################################
--#####################################################################################################################

DO LANGUAGE plpgsql
$$
BEGIN
if (select "character_maximum_length" from information_schema.Columns C where C.table_schema = 'public' and C.table_name = 'TaxonAnalysis' and C.column_name = 'AnalysisValue') = 255
then
	ALTER TABLE "TaxonAnalysis" ALTER COLUMN "AnalysisValue" TYPE text;
end if;
exception when others then
    raise notice '% %', SQLERRM, '
    The type of the column AnalysisValue in table TaxonAnalysis could not be changed to text. 
    If you do have values greater then 255 characters, Please remove all views pointing on TaxonAnalysis and change the type manually';--SQLSTATE;

END;
$$;


--#####################################################################################################################
--######   TaxonAnalysisCategory: Adding AnalysisURI, ReferenceTitle, ReferenceURI   ##################################
--#####################################################################################################################

ALTER TABLE "TaxonAnalysisCategory" ADD "AnalysisURI" character varying(255) NULL;

ALTER TABLE "TaxonAnalysisCategory" ADD "ReferenceTitle" character varying(600) NULL;

ALTER TABLE "TaxonAnalysisCategory" ADD "ReferenceURI" character varying(255) NULL;



--#####################################################################################################################
--######   Gazetteer: ADD ExternalNameID, ExternalDatabaseID    #######################################################
--#####################################################################################################################

ALTER TABLE "Gazetteer" ADD "ExternalNameID" character varying(50) NULL;
GO

ALTER TABLE "Gazetteer" ADD "ExternalDatabaseID" integer NULL;
GO

--#####################################################################################################################
--######   GazetteerExternalDatabase    ###############################################################################
--#####################################################################################################################

CREATE TABLE "GazetteerExternalDatabase"(
	"ExternalDatabaseID" integer NOT NULL,
	"ExternalDatabaseName" character varying(60) NOT NULL,
	"ExternalDatabaseVersion" character varying(255) NOT NULL,
	"ExternalAttribute_NameID" character varying(255) NULL,
	"ExternalAttribute_PlaceID" character varying(255) NULL,
	"ExternalCoordinatePrecision" character varying(255) NULL,
	CONSTRAINT "GazetteerExternalDatabase_pkey" PRIMARY KEY ("ExternalDatabaseID"));

ALTER TABLE "GazetteerExternalDatabase"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "GazetteerExternalDatabase" TO "CacheAdmin";
GRANT SELECT ON TABLE "GazetteerExternalDatabase" TO "CacheUser";









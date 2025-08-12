

--#####################################################################################################################
--######   Agent: AgentRole -> 255, adding AgentURI   #################################################################
--#####################################################################################################################

ALTER TABLE "Agent" ALTER COLUMN "AgentRole" SET DATA TYPE character varying(255);

ALTER TABLE "Agent" ADD COLUMN "AgentURI" character varying(255);


--#####################################################################################################################
--######   Gazetteer adding NameURI   #################################################################################
--#####################################################################################################################

ALTER TABLE "Gazetteer" ADD "NameURI" character varying(255) NULL;


--#####################################################################################################################
--######   ReferenceTitle adding ReferenceURI   #######################################################################
--#####################################################################################################################

ALTER TABLE "ReferenceTitle" ADD "ReferenceURI" character varying(255) NULL;


--#####################################################################################################################
--######   TaxonSynonymy adding NameURI   #############################################################################
--#####################################################################################################################

ALTER TABLE "TaxonSynonymy" ADD "NameURI" character varying(255) NULL;


--#####################################################################################################################
--######   highresolutionimagepath - case insensitive  ################################################################
--#####################################################################################################################

CREATE OR REPLACE FUNCTION public.highresolutionimagepath(character varying)
  RETURNS character varying AS
$BODY$
declare URI character varying(254) := $1;
begin
if LOWER($1) LIKE 'http://pictures.snsb.info/BsmLichensColl/%.jpg' 
OR LOWER($1) LIKE  'http://pictures.snsb.info/BsmVPlantsColl/%.jpg' 
OR LOWER($1) LIKE 'http://pictures.snsb.info/MsbVPlantsColl/%.jpg'
then
select REPLACE(REPLACE(REPLACE(URI, 'http://pictures.snsb.info/', 'http://zoomview.snsb.info/'), '/web/', '/'), '.jpg', '.html') into URI;
else
select '' into URI;
end if;
return URI;
END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION public.highresolutionimagepath(character varying)
  OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO public;
GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO postgres;
GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CacheUser";
GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO "CacheAdmin";
COMMENT ON FUNCTION public.highresolutionimagepath(character varying) IS 'Returns the path for the high resolution images if available';


--#####################################################################################################################
--######   TaxonNameExternalDatabase - ExternalDatabaseName -> 800   ##################################################
--#####################################################################################################################

ALTER TABLE "TaxonNameExternalDatabase" ALTER COLUMN "ExternalDatabaseName" SET DATA TYPE character varying(800);






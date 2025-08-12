
--#####################################################################################################################
--######   ScientificTerm        ######################################################################################
--#####################################################################################################################


CREATE TABLE "public"."ScientificTerm"(
	"RepresentationURI" character varying(255) NULL,
	"DisplayText" character varying(255) NULL,
	"HierarchyCache" character varying(900) NULL,
	"HierarchyCacheDown" character varying(900) NULL,
	"RankingTerm" character varying(200) NULL,
	"LogInsertedWhen" timestamp without time zone DEFAULT (now())::timestamp without time zone,
	"SourceView" character varying(200) NULL,
CONSTRAINT "ScientificTerm_pkey" PRIMARY KEY ("RepresentationURI") 
 );

ALTER TABLE "public"."ScientificTerm"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "public"."ScientificTerm" TO "CacheAdmin";
GRANT SELECT ON TABLE "public"."ScientificTerm" TO GROUP "CacheUser";



--#####################################################################################################################
--######   highresolutionimagepath        #############################################################################
--#####################################################################################################################



CREATE OR REPLACE FUNCTION public.highresolutionimagepath(character varying)
  RETURNS character varying AS
$BODY$
declare URI character varying(254) := $1;
begin
if $1 LIKE 'http://pictures.snsb.info/BsmLichensColl/%.jpg' 
OR $1 LIKE  'http://pictures.snsb.info/BsmVPlantsColl/%.jpg' 
OR $1 LIKE 'http://pictures.snsb.info/MsbVPlantsColl/%.jpg'
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

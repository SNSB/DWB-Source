--#####################################################################################################################
--######   TaxonAnalysis: adding Notes   ##############################################################################
--#####################################################################################################################

DO LANGUAGE plpgsql
$$
BEGIN
if (select count(*) from information_schema.Columns C where C.table_schema = 'public' and C.table_name = 'TaxonAnalysis' and C.column_name = 'Notes') = 0
then
	ALTER TABLE "TaxonAnalysis" ADD COLUMN "Notes" text;
end if;
END;
$$;



--#####################################################################################################################
--######   highresolutionimagepath - case insensitive and inclusion of ZSM pictures  ##################################
--#####################################################################################################################

ALTER FUNCTION highresolutionimagepath(character varying)
  OWNER TO "CacheAdmin";

CREATE OR REPLACE FUNCTION highresolutionimagepath(character varying)
  RETURNS character varying AS
$BODY$
declare URI character varying(254) := $1;
begin
if LOWER($1) LIKE 'http://pictures.snsb.info/bsmlichenscoll/%.jpg' 
OR LOWER($1) LIKE 'http://pictures.snsb.info/bsmvplantscoll/%.jpg' 
OR LOWER($1) LIKE 'http://pictures.snsb.info/msbvplantscoll/%.jpg'
OR LOWER($1) LIKE 'http://pictures.snsb.info/zsmlepidopteracoll/%.jpg'
OR LOWER($1) LIKE 'http://pictures.snsb.info/zsmhymenopteracoll/%.jpg'
OR LOWER($1) LIKE 'http://pictures.snsb.info/zsmcoleopteracoll/%.jpg'
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







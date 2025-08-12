--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres project to version 16
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   Metadata - RecordURI if missing     ##########################################################################################
--#####################################################################################################################

CREATE or replace FUNCTION InsertRecordURI() RETURNS void AS
$BODY$
declare i INTEGER := 0;
begin
select count(*) from information_schema.Columns C where C.table_schema = '#project#' and C.table_name = 'CacheMetadata' and C.column_name = 'RecordUri' into i;
if i = 0
then

	ALTER TABLE "#project#"."CacheMetadata" ADD COLUMN "RecordUri" character varying(254);

end if;
end;
$BODY$ LANGUAGE plpgsql;

SELECT InsertRecordURI(); 


DROP FUNCTION InsertRecordURI();

--#####################################################################################################################
--######   PackageAddOn      ##########################################################################################
--#####################################################################################################################


CREATE TABLE "#project#"."PackageAddOn"
(
  "Package" character varying(50) NOT NULL,
  "AddOn" character varying(50) NOT NULL,
  "Version" integer,
  CONSTRAINT "PackageAddOn_pkey" PRIMARY KEY ("Package", "AddOn")
);
ALTER TABLE "#project#"."PackageAddOn"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "#project#"."PackageAddOn" TO "CacheAdmin";
GRANT SELECT ON TABLE "#project#"."PackageAddOn" TO "CacheUser";
--GRANT SELECT ON TABLE "#project#"."PackageAddOn" TO "CacheUser_#project#";
--GRANT ALL ON TABLE "#project#"."PackageAddOn" TO "CacheAdmin_#project#";
GRANT ALL ON TABLE "#project#"."PackageAddOn" TO "CacheAdmin";


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

DROP FUNCTION "#project#".version();
CREATE OR REPLACE FUNCTION "#project#".version()
  RETURNS integer AS
$BODY$
declare
	v integer;
BEGIN
   SELECT 16 into v;
   RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;



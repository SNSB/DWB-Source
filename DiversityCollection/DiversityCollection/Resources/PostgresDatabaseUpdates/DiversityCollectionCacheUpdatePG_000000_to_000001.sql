
--#####################################################################################################################
--######   Roles   ####################################################################################################
--#####################################################################################################################

-- transferred into client

--CREATE or replace FUNCTION MakeCacheUser() RETURNS void AS
--$BODY$
--declare i INTEGER := 0;
--begin
--SELECT count(*) FROM pg_roles R where R.rolname = 'CacheUser' into i;
--if i = 0
--then
--	CREATE ROLE "CacheUser" VALID UNTIL 'infinity';
--end if;
--end;
--$BODY$ LANGUAGE plpgsql;

--SELECT MakeCacheUser(); 

--DROP FUNCTION MakeCacheUser();

--GRANT "CacheUser" TO CURRENT_USER WITH ADMIN OPTION;


--CREATE or replace FUNCTION MakeCacheAdmin() RETURNS void AS
--$BODY$
--declare i INTEGER := 0;
--begin
--SELECT count(*) FROM pg_roles R where R.rolname = 'CacheAdmin' into i;
--if i = 0
--then
--      CREATE ROLE "CacheAdmin" with CREATEDB CREATEROLE IN ROLE "CacheUser" VALID UNTIL 'infinity'; 
--end if;
--end;
--$BODY$ LANGUAGE plpgsql;

--SELECT MakeCacheAdmin(); 

--DROP FUNCTION MakeCacheAdmin();

--GRANT "CacheAdmin" TO CURRENT_USER WITH ADMIN OPTION;



--CREATE or replace FUNCTION MakeCacheEditor() RETURNS void AS
--$BODY$
--declare i INTEGER := 0;
--begin
--SELECT count(*) FROM pg_roles R where R.rolname = 'CacheEditor' into i;
--if i = 0
--then
--	CREATE ROLE "CacheEditor" IN ROLE "CacheUser"  VALID UNTIL 'infinity';
--end if;
--end;
--$BODY$ LANGUAGE plpgsql;

--SELECT MakeCacheEditor(); 

--DROP FUNCTION MakeCacheEditor();


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

-- transferred into client

--SET ROLE "CacheAdmin";
--ALTER FUNCTION version()
--  OWNER TO "CacheAdmin";

--GRANT EXECUTE ON FUNCTION version() TO GROUP "CacheUser";


--#####################################################################################################################
--######   DiversityWorkbenchModule   #################################################################################
--#####################################################################################################################

-- transferred into client


--ALTER FUNCTION diversityworkbenchmodule()
--  OWNER TO "CacheAdmin";

--GRANT EXECUTE ON FUNCTION diversityworkbenchmodule() TO GROUP "CacheUser";












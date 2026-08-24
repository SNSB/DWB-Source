--#####################################################################################################################
--#####################################################################################################################
-- Script to create dwb_template_cache_initialized
-- After creation you need to login to the dwb_template_cache_initialized and run the rest of the script, see Step3
-- Run this as superuser (postgres) once per PostgreSQL server
--#####################################################################################################################
--#####################################################################################################################


-- Step: Create the template database
CREATE DATABASE dwb_template_cache_initialized 
  OWNER "CacheAdmin" 
  ENCODING 'UTF8' 
  CONNECTION LIMIT=-1;

-- Step: Connect to dwb_template_cache_initialized and run the following:
\c dwb_template_cache_initialized

-- Grant default privileges
GRANT USAGE ON SCHEMA public TO "CacheUser";
GRANT ALL ON SCHEMA public TO "CacheAdmin";

ALTER DEFAULT PRIVILEGES GRANT SELECT ON TABLES TO "CacheUser";
ALTER DEFAULT PRIVILEGES GRANT ALL ON TABLES TO "CacheAdmin";
ALTER DEFAULT PRIVILEGES GRANT ALL ON FUNCTIONS TO "CacheAdmin";
ALTER DEFAULT PRIVILEGES GRANT ALL ON SEQUENCES TO "CacheAdmin";

-- Create the version() function
CREATE OR REPLACE FUNCTION public.version()
  RETURNS text AS
$BODY$
DECLARE
  v text;
BEGIN
  SELECT '00.00.00' INTO v;
  RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;

ALTER FUNCTION public.version() OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION public.version() TO "CacheUser";
GRANT EXECUTE ON FUNCTION public.version() TO "CacheAdmin";

-- Create the diversityworkbenchmodule() function
CREATE OR REPLACE FUNCTION public.diversityworkbenchmodule()
  RETURNS text AS
$BODY$
DECLARE
  v text;
BEGIN
  SELECT 'DiversityCollectionCache' INTO v;
  RETURN v;
END;
$BODY$
  LANGUAGE plpgsql STABLE
  COST 100;

ALTER FUNCTION public.diversityworkbenchmodule() OWNER TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION public.diversityworkbenchmodule() TO "CacheUser";
GRANT EXECUTE ON FUNCTION public.diversityworkbenchmodule() TO "CacheAdmin";

-- Mark as template (must be done as superuser)
UPDATE pg_database 
SET datistemplate = true, datallowconn = false 
WHERE datname = 'dwb_template_cache_initialized';



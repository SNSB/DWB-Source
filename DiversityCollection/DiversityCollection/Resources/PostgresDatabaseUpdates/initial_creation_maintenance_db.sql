--#####################################################################################################################
--#####################################################################################################################
-- Script to create dwb_maintenance_db
-- After creation you need to login to the dwb_maintenance_db and run the rest of the script, see Step2
-- Run this as superuser (postgres) once per PostgreSQL server
--#####################################################################################################################
--#####################################################################################################################



-- Step 1: Create the  database
CREATE DATABASE dwb_maintenance_db
    WITH
    OWNER postgres
    ENCODING = 'UTF8'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

-- Step 2: Connect  and run the following:
\c dwb_maintenance_db

-- Grant default privileges
GRANT CONNECT ON DATABASE dwb_maintenance_db TO "CacheAdmin";
GRANT ALL ON DATABASE dwb_maintenance_db TO postgres;

-- Create function terminate_database_sessions
CREATE OR REPLACE FUNCTION public.terminate_database_sessions(
	p_database_name text)
    RETURNS TABLE(pid integer, terminated boolean) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE SECURITY DEFINER PARALLEL UNSAFE
    ROWS 1000

    SET search_path=pg_catalog, public
AS $BODY$
BEGIN
    -- Verify the caller is a member of CacheAdmin
    IF NOT pg_has_role(current_user, 'CacheAdmin', 'MEMBER') THEN
        RAISE EXCEPTION 'Permission denied: User must be a member of CacheAdmin role';
    END IF;

    -- Verify that the database owner is CacheAdmin
    IF NOT EXISTS (
        SELECT 1 FROM pg_database 
        WHERE datname = p_database_name 
          AND pg_get_userbyid(datdba) = 'CacheAdmin'
    ) THEN
        RAISE EXCEPTION 'Database owner must be CacheAdmin for database: %', p_database_name;
    END IF;
    
    -- Prevent terminating connections to system databases
    IF p_database_name IN ('postgres', 'template0', 'template1', 'dwb_maintenance_db', 'dwb_template_cache_initialized') THEN
        RAISE EXCEPTION 'Cannot terminate sessions for system/template database: %', p_database_name;
    END IF;
    
    RETURN QUERY
    SELECT 
        sa.pid::INTEGER,
        pg_terminate_backend(sa.pid) AS terminated
    FROM pg_stat_activity sa
    WHERE sa.datname = p_database_name 
      AND sa.pid <> pg_backend_pid();
END;
$BODY$;

ALTER FUNCTION public.terminate_database_sessions(text)
    OWNER TO postgres;

GRANT EXECUTE ON FUNCTION public.terminate_database_sessions(text) TO "CacheAdmin";
GRANT EXECUTE ON FUNCTION public.terminate_database_sessions(text) TO postgres;

COMMENT ON FUNCTION public.terminate_database_sessions(text)
    IS 'Terminates all connections to the specified database. Requires CacheAdmin role membership and the database owner must be CacheAdmin.';


CREATE OR REPLACE FUNCTION public.validate_database_operation(
	p_operation text,
	p_database_name text,
	p_target_name text DEFAULT NULL::text,
	p_backup_suffix text DEFAULT '_backup'::text)
    RETURNS boolean
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE SECURITY DEFINER PARALLEL UNSAFE
    SET search_path=pg_catalog, public
AS $BODY$
DECLARE
    v_backup_name TEXT;
BEGIN
    -- Verify the caller is a member of CacheAdmin
    IF NOT pg_has_role(current_user, 'CacheAdmin', 'MEMBER') THEN
        RAISE EXCEPTION 'Permission denied: User must be a member of CacheAdmin role';
    END IF;
    
    -- Validate database name
    IF p_database_name IS NULL OR p_database_name = '' THEN
        RAISE EXCEPTION 'Database name cannot be empty';
    END IF;
    
    -- Prevent operations on system databases
    IF p_database_name IN ('postgres', 'template0', 'template1', 'dwb_maintenance_db', 'dwb_template_cache_initialized') THEN
        RAISE EXCEPTION 'Cannot perform % on system/template database: %', p_operation, p_database_name;
    END IF;
    
    -- Operation-specific validations
    CASE p_operation
    
        WHEN 'CREATE' THEN
            -- Target darf nicht existieren
            IF EXISTS (SELECT 1 FROM pg_database WHERE datname = p_database_name) THEN
                RAISE EXCEPTION 'Database already exists: %', p_database_name;
            END IF;
            
        WHEN 'DROP' THEN
            -- Source muss existieren
            IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = p_database_name) THEN
                RAISE EXCEPTION 'Database does not exist: %', p_database_name;
            END IF;

			-- Verify that the database owner is CacheAdmin
		    IF NOT EXISTS (
		        SELECT 1 FROM pg_database 
		        WHERE datname = p_database_name 
		          AND pg_get_userbyid(datdba) = 'CacheAdmin'
		    ) THEN
		        RAISE EXCEPTION 'Database owner must be CacheAdmin for database: %', p_database_name;
		    END IF;
            
        WHEN 'RENAME' THEN
            -- Source muss existieren
            IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = p_database_name) THEN
                RAISE EXCEPTION 'Database does not exist: %', p_database_name;
            END IF;
            -- Target-Name muss angegeben sein
            IF p_target_name IS NULL OR p_target_name = '' THEN
                RAISE EXCEPTION 'Target name is required for RENAME';
            END IF;
            -- Target darf kein System-DB-Name sein
            IF p_target_name IN ('postgres', 'template0', 'template1', 'dwb_maintenance_db', 'dwb_template_cache_initialized') THEN
                RAISE EXCEPTION 'Cannot use system/template database name as target: %', p_target_name;
            END IF;
            -- Target darf nicht existieren
            IF EXISTS (SELECT 1 FROM pg_database WHERE datname = p_target_name) THEN
                RAISE EXCEPTION 'Target database already exists: %', p_target_name;
            END IF;

			-- Verify that the database owner is CacheAdmin
		    IF NOT EXISTS (
		        SELECT 1 FROM pg_database 
		        WHERE datname = p_database_name 
		          AND pg_get_userbyid(datdba) = 'CacheAdmin'
		    ) THEN
		        RAISE EXCEPTION 'Database owner must be CacheAdmin for database: %', p_database_name;
		    END IF;
            
        WHEN 'COPY' THEN
            -- Source muss existieren
            IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = p_database_name) THEN
                RAISE EXCEPTION 'Source database does not exist: %', p_database_name;
            END IF;
            -- Target-Name muss angegeben sein
            IF p_target_name IS NULL OR p_target_name = '' THEN
                RAISE EXCEPTION 'Target name is required for COPY';
            END IF;
            -- Target darf kein System-DB-Name sein
            IF p_target_name IN ('postgres', 'template0', 'template1', 'dwb_maintenance_db', 'dwb_template_cache_initialized') THEN
                RAISE EXCEPTION 'Cannot use system/template database name as target: %', p_target_name;
            END IF;
            -- Target darf nicht existieren
            IF EXISTS (SELECT 1 FROM pg_database WHERE datname = p_target_name) THEN
                RAISE EXCEPTION 'Target database already exists: %', p_target_name;
            END IF;
            
            
        ELSE
            RAISE EXCEPTION 'Unknown operation: %', p_operation;
            
    END CASE;
    
    RETURN TRUE;
END;
$BODY$;

ALTER FUNCTION public.validate_database_operation(text, text, text, text)
    OWNER TO postgres;

GRANT EXECUTE ON FUNCTION public.validate_database_operation(text, text, text, text) TO "CacheAdmin";

GRANT EXECUTE ON FUNCTION public.validate_database_operation(text, text, text, text) TO postgres;

REVOKE ALL ON FUNCTION public.validate_database_operation(text, text, text, text) FROM PUBLIC;

COMMENT ON FUNCTION public.validate_database_operation(text, text, text, text)
    IS 'Validates database operations before execution. Operations: CREATE, DROP, RENAME, COPY, REPLACE';

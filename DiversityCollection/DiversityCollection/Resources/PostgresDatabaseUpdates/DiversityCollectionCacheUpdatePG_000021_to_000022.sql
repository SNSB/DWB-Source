--#####################################################################################################################
--######   Setting permissions - needed after change to version 17 of Postgres ####################################
--#####################################################################################################################

-- schema access
GRANT USAGE, CREATE ON SCHEMA public TO "CacheAdmin";
-- CacheUser can access objects in public schema ??
GRANT USAGE ON SCHEMA public TO "CacheUser";

-- allow CacheAdmin to manage CacheUser and CachePublicUser
GRANT "CacheUser" TO "CacheAdmin" WITH ADMIN OPTION;
GRANT "CachePublicUser" TO "CacheAdmin" WITH ADMIN OPTION;

-- evtl einmal Re-grant privileges on existing objects
GRANT SELECT ON ALL TABLES IN SCHEMA public TO "CacheUser";

-- Ensure future objects get correct privileges
ALTER DEFAULT PRIVILEGES FOR ROLE "CacheAdmin" IN SCHEMA public
    GRANT SELECT ON TABLES TO "CacheUser";

--#####################################################################################################################
--######   ReferenceTitle Add AuthorsCache for authors from RLL #######################################################
--#####################################################################################################################

ALTER TABLE public."ReferenceTitle" ADD COLUMN IF NOT EXISTS "AuthorsCache" character varying(1000) COLLATE pg_catalog."default" NULL;

--#####################################################################################################################
--######  AgentIdentifier  ############################################################################################
--#####################################################################################################################

CREATE TABLE IF NOT EXISTS public."AgentIdentifier"(
	"AgentID" integer NOT NULL,
	"Identifier" character varying(190) NOT NULL,
	"IdentifierURI" character varying(500) NULL,
	"Type" character varying(50) NULL,
	"Notes" text NULL,
	"SourceView" character varying(128) NOT NULL,
	"BaseURL" character varying(500) NOT NULL,
 CONSTRAINT "PK_AgentIdentifier" PRIMARY KEY
(
	"AgentID",
	"BaseURL",
	"Identifier"
) ) 
TABLESPACE pg_default;

ALTER TABLE public."AgentIdentifier" OWNER to "CacheAdmin";

GRANT ALL ON TABLE public."AgentIdentifier" TO "CacheAdmin";

GRANT SELECT ON TABLE public."AgentIdentifier" TO "CacheUser";



















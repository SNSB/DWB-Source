
--#####################################################################################################################
--######   "Agent"           ##########################################################################################
--#####################################################################################################################


CREATE TABLE "Agent"
(
  "BaseURL" character varying(255) NOT NULL,
  "AgentID" integer NOT NULL,
  "AgentParentID" integer,
  "AgentName" character varying(200),
  "AgentTitle" character varying(50),
  "GivenName" character varying(255),
  "GivenNamePostfix" character varying(50),
  "InheritedNamePrefix" character varying(50),
  "InheritedName" character varying(255),
  "InheritedNamePostfix" character varying(50),
  "Abbreviation" character varying(50),
  "AgentType" character varying(50),
  "AgentRole" character varying(50),
  "AgentGender" character varying(50),
  "Description" character varying(1000),
  "OriginalSpelling" character varying(200),
  "Notes" text,
  "ValidFromDate" timestamp without time zone DEFAULT (now())::timestamp without time zone,
  "ValidUntilDate" timestamp without time zone DEFAULT (now())::timestamp without time zone,
  "SynonymToAgentID" integer,
  "ProjectID" integer NOT NULL,
  "LogInsertedWhen" timestamp without time zone DEFAULT (now())::timestamp without time zone,
  "SourceView" character varying(200) NOT NULL,
  CONSTRAINT "Agent_pkey" PRIMARY KEY ("AgentID", "BaseURL")
);
ALTER TABLE "Agent"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "Agent" TO "CacheAdmin";
GRANT SELECT ON TABLE "Agent" TO "CacheUser";



--#####################################################################################################################
--######   "AgentContactInformation"  #################################################################################
--#####################################################################################################################


CREATE TABLE "AgentContactInformation"
(
  "AgentID" integer NOT NULL,
  "DisplayOrder" integer NOT NULL,
  "AddressType" character varying(50),
  "Country" character varying(255),
  "City" character varying(255),
  "PostalCode" character varying(50),
  "Streetaddress" character varying(255),
  "Address" character varying(255),
  "Telephone" character varying(50),
  "CellularPhone" character varying(50),
  "Telefax" character varying(50),
  "Email" character varying(255),
  "URI" character varying(255),
  "Notes" text,
  "ValidFrom" timestamp without time zone DEFAULT (now())::timestamp without time zone,
  "ValidUntil" timestamp without time zone DEFAULT (now())::timestamp without time zone,
  "SourceView" character varying(200),
  CONSTRAINT "AgentContactInformation_pkey" PRIMARY KEY ("AgentID", "DisplayOrder")
);
ALTER TABLE "AgentContactInformation"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "AgentContactInformation" TO "CacheAdmin";
GRANT SELECT ON TABLE "AgentContactInformation" TO "CacheUser";


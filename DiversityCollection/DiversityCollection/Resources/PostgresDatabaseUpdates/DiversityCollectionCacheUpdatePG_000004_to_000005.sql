
--#####################################################################################################################
--######   "Gazetteer"           ######################################################################################
--#####################################################################################################################


CREATE TABLE "Gazetteer"
(
  "BaseURL" character varying(255) NOT NULL,
  "NameID" integer NOT NULL,
  "Name" character varying(400),
  "LanguageCode" character varying(50),
  "PlaceID" integer NOT NULL,
  "PlaceType" character varying(50),
  "PreferredName" character varying(400),
  "PreferredNameID" integer NOT NULL,
  "PreferredNameLanguageCode" character varying(50),
  "LogInsertedWhen" timestamp without time zone DEFAULT (now())::timestamp without time zone,
  "SourceView" character varying(200),
  CONSTRAINT "Gazetteer_pkey" PRIMARY KEY ("NameID", "BaseURL")
);
ALTER TABLE "Gazetteer"
  OWNER TO "CacheAdmin";
GRANT ALL ON TABLE "Gazetteer" TO "CacheAdmin";
GRANT SELECT ON TABLE "Gazetteer" TO "CacheUser";

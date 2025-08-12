--#####################################################################################################################
--######   Grants for public   ########################################################################################
--#####################################################################################################################

GRANT SELECT ON TABLE "Agent" TO public;
GRANT SELECT ON TABLE "AgentContactInformation" TO public;

GRANT SELECT ON TABLE "Gazetteer" TO public;
GRANT SELECT ON TABLE "GazetteerExternalDatabase" TO public;

GRANT SELECT ON TABLE "ReferenceRelator" TO public;
GRANT SELECT ON TABLE "ReferenceTitle" TO public;

GRANT SELECT ON TABLE "ScientificTerm" TO public;

GRANT SELECT ON TABLE "TaxonAnalysis" TO public;
GRANT SELECT ON TABLE "TaxonAnalysisCategory" TO public;
GRANT SELECT ON TABLE "TaxonAnalysisCategoryValue" TO public;
GRANT SELECT ON TABLE "TaxonCommonName" TO public;
GRANT SELECT ON TABLE "TaxonList" TO public;
GRANT SELECT ON TABLE "TaxonNameExternalDatabase" TO public;
GRANT SELECT ON TABLE "TaxonNameExternalID" TO public;
GRANT SELECT ON TABLE "TaxonSynonymy" TO public;


--#####################################################################################################################
--######   functions in public - Grant for public  ####################################################################
--#####################################################################################################################

GRANT EXECUTE ON FUNCTION public.highresolutionimagepath(character varying) TO public;

GRANT EXECUTE ON FUNCTION public.version() TO PUBLIC;

GRANT EXECUTE ON FUNCTION public.diversityworkbenchmodule() TO PUBLIC;








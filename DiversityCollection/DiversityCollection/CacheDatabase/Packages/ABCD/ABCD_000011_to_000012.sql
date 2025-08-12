--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 12
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   ABCD_ContentContact fixing column descriptions   ###########################################################
--#####################################################################################################################

COMMENT ON COLUMN public."ABCD_ContentContact"."Email"
    IS 'ABCD 2.06 entity /DataSets/DataSet/ContentContacts/ContentContact/Email. The email address of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table AgentContactInformation with data retrieved from DiversityAgents - Column Email';

COMMENT ON COLUMN public."ABCD_ContentContact"."Phone"
    IS 'ABCD 2.06 entity /DataSets/DataSet/ContentContacts/ContentContact/Phone. The phone address of the agent. 
	Retrieved from DiversityProjects - ProjectAgent - linked via URL to the source in table AgentContactInformation with data retrieved from DiversityAgents - Column Telephone';


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 12 WHERE "Package" = 'ABCD'
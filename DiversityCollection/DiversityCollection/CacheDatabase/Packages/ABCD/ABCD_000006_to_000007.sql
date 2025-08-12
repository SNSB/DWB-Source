--#####################################################################################################################
--#####################################################################################################################
--Skript for Update of Postgres package ABCD to version 7
--replace "#project#" with Name of the project
--the string at the begin of the line --## is used to mark end and begin of a command
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   ABCD__Kingdom_TaxonomicGroups: inserting new data   ########################################################
--#####################################################################################################################

INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('Coleoptera', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('Diptera', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('Heteroptera', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('Hymenoptera', 'Animalia'); 
INSERT INTO "#project#"."ABCD__Kingdom_TaxonomicGroups" ("TaxonomicGroup", "Kingdom")
VALUES('Lepidoptera', 'Animalia'); 


--#####################################################################################################################
--######   version   ##################################################################################################
--#####################################################################################################################

UPDATE "#project#"."Package" SET "Version" = 7 WHERE "Package" = 'ABCD'
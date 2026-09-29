declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.57'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   New material categories scan and synthetic  ################################################################
--#####################################################################################################################

if (select count(*) from CollMaterialCategory_Enum where Code = '3D data') = 0
begin
INSERT INTO CollMaterialCategory_Enum
(Code, Description, DisplayText, DisplayOrder, DisplayEnable, ParentCode)
VALUES        ('3D data', '3D data of an object e.g. created with a 3D-scanner', '3D data', 150, 1, 'medium')
end
GO


if (select count(*) from CollMaterialCategory_Enum where Code = 'synthetic specimen') = 0
begin
INSERT INTO CollMaterialCategory_Enum
(Code, Description, DisplayText, DisplayOrder, DisplayEnable, ParentCode)
VALUES        ('synthetic specimen', 'synthetic materials e.g. used in a 3D-printer for artefacts', 'synthetic specimen', 900, 1, 'specimen')
end
GO



--#####################################################################################################################
--######   Adding new taxonomic groups archaebacterium, protozoan, chromist  ##########################################
--#####################################################################################################################

if (select count(*) from CollTaxonomicGroup_Enum where code = 'archaebacterium') = 0
begin
INSERT INTO CollTaxonomicGroup_Enum
(Code, Description, DisplayText, DisplayOrder, DisplayEnable)
VALUES        ('archaebacterium', 'member of the kingdom Archaebacteria', 'archaebacterium', 22, 1)
end
GO

if (select count(*) from CollTaxonomicGroup_Enum where code = 'protozoan') = 0
begin
INSERT INTO CollTaxonomicGroup_Enum
(Code, Description, DisplayText, DisplayOrder, DisplayEnable)
VALUES        ('protozoan', 'member of the paraphyletic group Protista', 'protozoan', 107, 1)
end
GO

if (select count(*) from CollTaxonomicGroup_Enum where code = 'chromist') = 0
begin
INSERT INTO CollTaxonomicGroup_Enum
(Code, Description, DisplayText, DisplayOrder, DisplayEnable)
VALUES        ('chromist', 'member of the kingdom Chromista', 'chromist', 103, 1)
end
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.58'
END

GO
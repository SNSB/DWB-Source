
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.30'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   GALLS   ######################################################################################
--#####################################################################################################################

INSERT INTO [CollTaxonomicGroup_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,DisplayOrder)
     VALUES
           ('gall'
           ,'growth modification or deformation of plant tissue or fungal plectenchyma, induced by a different organism '
           ,'gall'
           ,1
           ,122)
GO


--#####################################################################################################################
--######   hiding herbivore   ######################################################################################
--#####################################################################################################################

update [CollTaxonomicGroup_Enum]
set [DisplayEnable] = 0
where Code = 'herbivore'

GO

--#####################################################################################################################
--######   EntityAccessibility_Enum   ######################################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.EntityAccessibility_Enum TO Administrator
GO

--#####################################################################################################################
--######   Parent, Mutant   ######################################################################################
--#####################################################################################################################


--INSERT INTO [CollUnitRelationType_Enum]
--           ([Code]
--           ,[Description]
--           ,[DisplayText]
--           ,[DisplayOrder]
--           ,[DisplayEnable])
--     VALUES
--           ('Parent of',	
--           'parent association (i.e. in the literal genetical sense, not in an abstract sense)',	
--           'Parent of',	
--           192,	
--           1)
--GO

INSERT INTO [CollUnitRelationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('Mutant of',	
           'A mutant of the parent organism',	
           'Mutant of',	
           200,	
           1)
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.31'
END

GO



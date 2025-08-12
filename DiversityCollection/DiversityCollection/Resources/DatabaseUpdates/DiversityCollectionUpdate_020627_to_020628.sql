declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.27'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  trgInsCollectionTask - use CollectionID if provided  ########################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgInsCollectionTask] ON [dbo].[CollectionTask] 
INSTEAD OF INSERT AS

/*  Date: 20.07.2021  */ 
INSERT INTO [dbo].[CollectionTask]
           ([CollectionTaskParentID]
		  ,[CollectionID]
		  ,[TaskID]
		  ,[DisplayOrder]
		  ,[DisplayText]
		  ,[CollectionSpecimenID]
		  ,[SpecimenPartID]
		  ,[TransactionID]
		  ,[ModuleUri]
		  ,[TaskStart]
		  ,[TaskEnd]
		  ,[Result]
		  ,[URI]
		  ,[NumberValue]
		  ,[BoolValue], MetricDescription, MetricSource, MetricUnit, ResponsibleAgent, ResponsibleAgentURI
		  ,[Description]
          ,[Notes])
SELECT i.[CollectionTaskParentID]
		,case when c.[CollectionID] is null then i.CollectionID else case when i.CollectionID is null then c.CollectionID else i.CollectionID end end
		,i.[TaskID]
		,i.[DisplayOrder]
		,i.[DisplayText]
		,i.[CollectionSpecimenID]
		,i.[SpecimenPartID]
		,i.[TransactionID]
		,i.[ModuleUri]
		,i.[TaskStart]
		,i.[TaskEnd]
		,i.[Result]
		,i.[URI]
		,i.[NumberValue]
		,i.[BoolValue], i.MetricDescription, i.MetricSource, i.MetricUnit, i.ResponsibleAgent, i.ResponsibleAgentURI
		,i.[Description]
		,i.[Notes]
  FROM inserted i left outer join [dbo].[CollectionTask] c ON i.CollectionTaskParentID = c.CollectionTaskID

  GO


--#####################################################################################################################
--######  new taxonomic groups for insects  ###########################################################################
--#####################################################################################################################

INSERT INTO CollTaxonomicGroup_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        ('Coleoptera', 'Coleoptera', 'Coleoptera', 1, 'insect')

INSERT INTO CollTaxonomicGroup_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        ('Diptera', 'Diptera', 'Diptera', 1, 'insect')

INSERT INTO CollTaxonomicGroup_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        ('Heteroptera', 'Heteroptera', 'Heteroptera', 1, 'insect')

INSERT INTO CollTaxonomicGroup_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        ('Hymenoptera', 'Hymenoptera', 'Hymenoptera', 1, 'insect')

INSERT INTO CollTaxonomicGroup_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        ('Lepidoptera', 'Lepidoptera', 'Lepidoptera', 1, 'insect')

GO

--#####################################################################################################################
--######  new task exhibition  ########################################################################################
--#####################################################################################################################

INSERT INTO TaskType_Enum (Code, Description, DisplayText, DisplayEnable) VALUES ('Exhibition', 'Exhibition', 'Exhibition', 1);
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.28'
END

GO


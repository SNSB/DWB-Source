
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.19'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   New types for Descriptor    ################################################################################
--#####################################################################################################################

if (select count(*)
from [ProjectDescriptorElement]
where [ElementID] = 3) = 0
begin
	INSERT INTO [dbo].[ProjectDescriptorElement]
           ([ElementID]
           ,[Abbreviation]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[Description]
           ,[DisplayEnabled])
     VALUES
           (3
           ,'Geographical name'
           ,'Geographical name'
           ,300
           ,'A geographical or topographical name from DiversityGazetteers or a related webservice'
           ,1)
end
else
begin
	UPDATE E SET [Abbreviation] = 'Geographical name', [DisplayText] = 'Geographical name', [DisplayOrder] = 300,
	[Description] = 'A geographical or topographical name from DiversityGazetteers or a related webservice',
	[DisplayEnabled] = 1
	FROM [dbo].[ProjectDescriptorElement] E
	WHERE E.[ElementID] = 3
end


if (select count(*)
from [ProjectDescriptorElement]
where [ElementID] = 4) = 0
begin
	INSERT INTO [dbo].[ProjectDescriptorElement]
           ([ElementID]
           ,[Abbreviation]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[Description]
           ,[DisplayEnabled])
     VALUES
           (4
           ,'Term'
           ,'Scientific term'
           ,400
           ,'A scientific term or parameter from DiversityScientificTerms or a related webservice'
           ,1)
end
else
begin
	UPDATE E SET [Abbreviation] = 'Term', [DisplayText] = 'Scientific term', [DisplayOrder] = 400,
	[Description] = 'A scientific term or parameter from DiversityScientificTerms or a related webservice',
	[DisplayEnabled] = 1
	FROM [dbo].[ProjectDescriptorElement] E
	WHERE E.[ElementID] = 4
end


if (select count(*)
from [ProjectDescriptorElement]
where [ElementID] = 5) = 0
begin
	INSERT INTO [dbo].[ProjectDescriptorElement]
           ([ElementID]
           ,[Abbreviation]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[Description]
           ,[DisplayEnabled])
     VALUES
           (5
           ,'Taxon'
           ,'Taxon name'
           ,500
           ,'A taxonomic name from DiversityTaxonNames or a related webservice'
           ,1)
end
else
begin
	UPDATE E SET [Abbreviation] = 'Taxon', [DisplayText] = 'Taxon name', [DisplayOrder] = 500,
	[Description] = 'A taxonomic name from DiversityTaxonNames or a related webservice',
	[DisplayEnabled] = 1
	FROM [dbo].[ProjectDescriptorElement] E
	WHERE E.[ElementID] = 5
end


GO


--#####################################################################################################################
--######   Missing descriptions  ######################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectChildNodes',
@level2type=N'PARAMETER', @level2name=N'@ID' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectChildNodes' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the items related to the given item',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the items related to the given item',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectHierarchy' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the parents of the given project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectParents'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the parents of the given project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectParents' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the project as stored in table Project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectParents',
@level2type=N'PARAMETER', @level2name=N'@ProjectID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the project as stored in table Project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectParents',
@level2type=N'PARAMETER', @level2name=N'@ProjectID' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The level of the hierarchy: 1 = level of selected project; increasing with parents in hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectParents',
@level2type=N'COLUMN', @level2name=N'HierarchyLevel'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The level of the hierarchy: 1 = level of selected project; increasing with parents in hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectParents',
@level2type=N'COLUMN', @level2name=N'HierarchyLevel' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the settings depending on a setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the settings depending on a setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'PARAMETER', @level2name=N'@SettingID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'PARAMETER', @level2name=N'@SettingID' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the setting as defined in table Setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'SettingID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'ID of the setting as defined in table Setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'SettingID' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the superior setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'ParentSettingID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the superior setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'ParentSettingID' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Short abbreviated description of the setting, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Short abbreviated description of the setting, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'DisplayText' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The sequence in which the items should be shown in an interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'DisplayOrder'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The sequence in which the items should be shown in an interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'DisplayOrder' 
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of the setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'Description'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Description of the setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyChildren',
@level2type=N'COLUMN', @level2name=N'Description' 
END CATCH;
GO




--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.03.02' END
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.20'
END

GO


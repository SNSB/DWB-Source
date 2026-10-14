
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.11'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ProjectReference - new descriptions     ####################################################################
--#####################################################################################################################

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of table Project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The title of the data publication of the project data package or literature reference publication where information on the project and/ or on project dataset(s) was published. Note this is only a cached value where ReferenceURI is present' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'URI of project data citation/ data package citation or URI of literature reference where information on the project and/ or on project dataset(s) was published, e.g. referring to the module DiversityReferences' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Details within the project data citation/ data package citation, e.g. versioning, or details within the literature reference, e.g. pages' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The type of the data citation or literature reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceType'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Notes on the project data citation or literature reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The URI of a reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'IsCitation'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The way the reference should be cited; suggested citation style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceCitation'
GO



--#####################################################################################################################
--######   ViewDataCitation     #######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewDataCitation] AS
SELECT [ProjectID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[ReferenceType]
      ,[Notes]
      ,[URI]
      ,[ReferenceCitation]
  FROM [dbo].[ProjectReference] R where R.IsCitation = 1
')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of table Project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the data publication of the project data package. Note this is only a cached value where ReferenceURI is present.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of project data citation/ data package citation of the project data package, e.g. referring to the module DiversityReferences.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details within the project data citation/ data package citation, e.g. versioning' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the project data citation/ data package citation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the project data citation/ data package citation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of project data citation/ data package citation with information on the project dataset(s)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'URI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the project data/ data package should be cited; suggested citation style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceCitation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The citation of published data from the project;  references of the data publication of the project data package' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation'
GO

GRANT SELECT ON [ViewDataCitation] TO [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   ViewLiteratureReference    #################################################################################
--#####################################################################################################################


CREATE VIEW [dbo].[ViewLiteratureReference] AS
SELECT [ProjectID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[ReferenceType]
      ,[Notes]
      ,[URI]
      ,[ReferenceCitation]
  FROM [dbo].[ProjectReference] R where R.IsCitation = 0

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of table Project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the literature reference publication where information on the project and/ or on project dataset(s) was published. Note this is only a cached value where ReferenceURI is present.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of literature reference with information on the project and/ or on the project dataset(s), e.g. referring to the module DiversityReferences.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details within the literature reference, e.g. pages' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the literature reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the literature reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the literature reference with information on the project dataset(s)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'URI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the literature reference should be cited; suggested citation style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceCitation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The references related to the project;  references of literature publications where information on the project and/ or on project dataset(s) is given' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference'
GO

GRANT SELECT ON [ViewLiteratureReference] TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######  Description of database roles  ##############################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty   
@name = N'MS_Description',   
@value = N'Database Role with reading accesss to the database.',  
@level0type = N'USER',  
@level0name = 'DiversityWorkbenchUser'; 
GO
EXEC sys.sp_addextendedproperty   
@name = N'MS_Description',   
@value = N'Database Role with read/write accesss to the database.',  
@level0type = N'USER',  
@level0name = 'DiversityWorkbenchEditor'; 
GO
EXEC sys.sp_addextendedproperty   
@name = N'MS_Description',   
@value = N'Database Role for administrative actions within the database.',  
@level0type = N'USER',  
@level0name = 'DiversityWorkbenchAdministrator'; 
GO
--#####################################################################################################################
--#####################################################################################################################
--######  Description of storded procedures  ##########################################################################
--#####################################################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Copy project including all depending data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertProjectCopy'
GO

declare @D nvarchar(500);
set @D = N'Output parameter, ID of the new project';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertProjectCopy',
@level2type=N'PARAMETER', @level2name=N'@ProjectID'
GO
declare @D nvarchar(500);
set @D = N'ID of the project that should be copied';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertProjectCopy',
@level2type=N'PARAMETER', @level2name=N'@CopyID'
GO
declare @D nvarchar(500);
set @D = N'Title of the new project';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertProjectCopy',
@level2type=N'PARAMETER', @level2name=N'@ProjectTitle'
GO
--#####################################################################################################################
--#####################################################################################################################
--######  Description of functions  ###################################################################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  BaseURL  ####################################################################################################
--#####################################################################################################################

declare @D nvarchar(500);
set @D = N'Provides the basic address for accessing the database';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'BaseURL'
GO
--#####################################################################################################################
--######  DiversityWorkbenchModule  ###################################################################################
--#####################################################################################################################

declare @D nvarchar(500);
set @D = N'Provides the name of the DiversityWorkbench module';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityWorkbenchModule'
GO
--#####################################################################################################################
--######  ProjectList  ################################################################################################
--#####################################################################################################################

declare @D nvarchar(500);
set @D = N'The list of Projects accessible for the current user';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectList'
GO

declare @D nvarchar(500);
set @D = N'ID of the project';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'ProjectList',
@level2type=N'COLUMN', @level2name=N'ProjectID'
GO
--#####################################################################################################################
--######  SettingHierarchyAll  ########################################################################################
--#####################################################################################################################

declare @D nvarchar(500);
set @D = N'Returns a table that lists all the settings including a display text with the whole hierarchy';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyAll'
GO

declare @D nvarchar(500);
set @D = N'ID of the setting as defined in table Setting';
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=@D,
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyAll',
@level2type=N'COLUMN', @level2name=N'SettingID'
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the superior setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyAll',
@level2type=N'COLUMN', @level2name=N'ParentSettingID'
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyAll',
@level2type=N'COLUMN', @level2name=N'DisplayText';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The sequence in which the items should be shown in an interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyAll',
@level2type=N'COLUMN', @level2name=N'DisplayOrder';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyAll',
@level2type=N'COLUMN', @level2name=N'Description';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The hierarchy composed of the display texts of the superior settings',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyAll',
@level2type=N'COLUMN', @level2name=N'Hierarchy';
GO

--#####################################################################################################################
--######  SettingHierarchyNonProject  #################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the settings including a display text with the whole hierarchy that are not included in a project so far',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyNonProject';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyNonProject',
@level2type=N'PARAMETER', @level2name=N'@ProjectID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the setting as defined in table Setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyNonProject',
@level2type=N'COLUMN', @level2name=N'SettingID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the superior setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyNonProject',
@level2type=N'COLUMN', @level2name=N'ParentSettingID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyNonProject',
@level2type=N'COLUMN', @level2name=N'DisplayText';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The sequence in which the items should be shown in an interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyNonProject',
@level2type=N'COLUMN', @level2name=N'DisplayOrder';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyNonProject',
@level2type=N'COLUMN', @level2name=N'Description';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The hierarchy composed of the display texts of the superior settings',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyNonProject',
@level2type=N'COLUMN', @level2name=N'Hierarchy';
GO

--#####################################################################################################################
--######  SettingHierarchyProject  ####################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Outdated. 
Replaced by SettingHierarchyProjectAll.
Returns the settings of a project including all settings inherited from superior projects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProject';
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Input parameter. Enter a valid ProjectID.', 
@level0type = N'SCHEMA', @level0name = [dbo],
@level1type = N'FUNCTION', @level1name = SettingHierarchyProject,
@level2type = N'PARAMETER', @level2name ='@ProjectID';
GO
EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'The ID of the setting as defined in table Setting', 
@level0type = N'SCHEMA', @level0name = [dbo],
@level1type = N'FUNCTION', @level1name = SettingHierarchyProject,
@level2type = N'COLUMN', @level2name ='SettingID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProject',
@level2type=N'COLUMN', @level2name=N'DisplayText';
GO
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The sequence in which the items should be shown in an interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProject',
@level2type=N'COLUMN', @level2name=N'DisplayOrder';
GO
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProject',
@level2type=N'COLUMN', @level2name=N'Description';
GO
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The hierarchy composed of the display texts of the superior settings',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProject',
@level2type=N'COLUMN', @level2name=N'Hierarchy';
GO
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The value of the setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProject',
@level2type=N'COLUMN', @level2name=N'Value';
GO

--#####################################################################################################################
--######  SettingHierarchyProjectAll  #################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the settings for a project including those inherited from superior projects.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Input parameter. Enter a valid ProjectID.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'PARAMETER', @level2name=N'@ProjectID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the setting as defined in table Setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'SettingID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the superior setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'ParentSettingID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'DisplayText';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The sequence in which the items should be shown in an interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'DisplayOrder';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'Description';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The hierarchy composed of the display texts of the superior settings',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'Hierarchy';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The value of the setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'Value';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the superior project from which this setting has been inherited',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'ProjectID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The name of the project from which this setting has been inherited',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingHierarchyProjectAll',
@level2type=N'COLUMN', @level2name=N'Project';
GO


--#####################################################################################################################
--######  SettingsForProject  #########################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the settings for a project including those inherited from parent projects including formatting options.
Used for Metadata retrieval by DiversityCollection cache databases',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'PARAMETER', @level2name=N'@ProjectID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'A optional filter for the settings',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'PARAMETER', @level2name=N'@SettingFilter';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Optional. A spacer placed between the ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'PARAMETER', @level2name=N'@Spacer';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Formatting mode of the column ProjectSetting
1: Whole hierarchy
2: Remove leading value of @SettingFilter from hierarchy ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'PARAMETER', @level2name=N'@DisplayType';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the Setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'COLUMN', @level2name=N'SettingID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the superior setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'COLUMN', @level2name=N'ParentSettingID';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'COLUMN', @level2name=N'DisplayText';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'COLUMN', @level2name=N'Description';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The hierarchy composed of the display texts of the superior settings
formatting depending from parameter ',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'COLUMN', @level2name=N'ProjectSetting';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The value of the setting',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SettingsForProject',
@level2type=N'COLUMN', @level2name=N'Value';
GO

--#####################################################################################################################
--######  StableIdentifier  ###########################################################################################
--#####################################################################################################################
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The stable identifier published for a project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'ID of the project',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'StableIdentifier',
@level2type=N'PARAMETER', @level2name=N'@ProjectID';
GO

-- existing only if created by user
declare @SQL nvarchar(max)
set @SQL = (select 'EXEC sys.sp_addextendedproperty 
@name=N''MS_Description'', @value=N''Base for stable identifier if non is given for a single project'',
@level0type=N''SCHEMA'', @level0name=N''dbo'',
@level1type=N''FUNCTION'', @level1name=N''StableIdentifierBase'';
')

begin try
exec sp_executesql @SQL
end try
begin catch
end catch
GO

--#####################################################################################################################
--######  Version  ####################################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Version';
GO

EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Version of the client software compatible with the version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'VersionClient';
GO

--#####################################################################################################################
--#####################################################################################################################
--######  Description of views  #######################################################################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  ViewBaseURL  ################################################################################################
--#####################################################################################################################

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the reference should be cited; suggested citation style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewBaseURL', @level2type=N'COLUMN',@level2name=N'BaseURL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The references related to the project;  references of publications where information on the project and/ or on project dataset(s) is given' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewBaseURL'
GO



--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.04' END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.12'
END

GO


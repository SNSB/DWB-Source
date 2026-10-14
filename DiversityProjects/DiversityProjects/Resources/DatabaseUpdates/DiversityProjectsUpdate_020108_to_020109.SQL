
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.08'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  ViewProject: adding StableIdentifier   ######################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[ViewProject]
AS
SELECT [ProjectID]
      ,[ProjectParentID]
      ,[Project]
      ,[ProjectTitle]
      ,[ProjectDescription]
      ,[ProjectNotes]
      ,[ProjectVersion]
      ,[ProjectURL]
      ,[ProjectRights]
      ,[ProjectCopyright]
      ,[ProjectLicenseURI]
      ,[LogInsertedBy]
      ,[LogInsertedWhen]
      ,[LogUpdatedBy]
      ,[LogUpdatedWhen]
      ,[RowGUID]
	  ,dbo.StableIdentifier(ProjectID) as StableIdentifier
  FROM [dbo].[Project]
  GO


--#####################################################################################################################
--######  Project: adding ProjectType   ###############################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = 'Project' and C.COLUMN_NAME = 'ProjectType') = 0
begin
	ALTER TABLE [dbo].[Project] ADD [ProjectType] [nvarchar](50) NULL

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectType'

	ALTER TABLE [dbo].[Project_log] ADD [ProjectType] [nvarchar](50) NULL
end
GO



--#####################################################################################################################
--######  ProjectType_Enum   ##########################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES C where c.TABLE_NAME = 'ProjectType_Enum') = 0
begin

	CREATE TABLE [dbo].[ProjectType_Enum](
		[Code] [nvarchar](50) NOT NULL,
		[Description] [nvarchar](500) NULL,
		[DisplayText] [nvarchar](50) NULL,
		[DisplayOrder] [smallint] NULL,
		[DisplayEnable] [bit] NULL,
		[InternalNotes] [nvarchar](500) NULL,
		[ParentCode] [nvarchar](50) NULL,
		[RowGUID] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_ProjectType_Enum] PRIMARY KEY CLUSTERED 
	(
		[Code] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ProjectType_Enum] ADD  CONSTRAINT [DF_ProjectType_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'Code'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'Description'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'

	ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_ProjectType_Enum] FOREIGN KEY([ProjectType])
	REFERENCES [dbo].[ProjectType_Enum] ([Code])

	ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_ProjectType_Enum]
END
GO

GRANT SELECT ON [dbo].[ProjectType_Enum] TO [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######  Adding types and update of present data   ###################################################################
--#####################################################################################################################


DISABLE TRIGGER dbo.[trgUpdProject] ON dbo.[Project]
GO

--Agents
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'Agents') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('Agents'
           ,'Project for the module DiversityAgents'
           ,'Agents'
		   ,1)
UPDATE [dbo].[Project]
   SET [ProjectType] = 'Agents'
 WHERE ProjectType LIKE '%collectors' OR ProjectType LIKE '%agents' OR ProjectType LIKE '%authors' OR ProjectType LIKE '%addresses'
end
GO

--Collection
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'Collection') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('Collection'
           ,'Project for the module DiversityCollection'
           ,'Collection'
		   ,1)

UPDATE [dbo].[Project]
   SET [ProjectType] = 'Collection'
 WHERE ProjectType LIKE '%coll'
end
GO

--Descriptions
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'Descriptions') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('Descriptions'
           ,'Project for the module DiversityDescriptions'
           ,'Descriptions'
		   ,1)
end
GO

--Gazetteer
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'Gazetteer') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('Gazetteer'
           ,'Project for the module DiversityGazetteer'
           ,'Gazetteer'
		   ,1)

UPDATE [dbo].[Project]
   SET [ProjectType] = 'Gazetteer'
 WHERE ProjectType LIKE '%gaz'
end
GO

--References
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'References') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('References'
           ,'Project for the module DiversityReferences'
           ,'References'
		   ,1)

UPDATE [dbo].[Project]
   SET [ProjectType] = 'References'
 WHERE ProjectType LIKE '%lit'
end
GO

--SamplingPlots
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'SamplingPlots') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('SamplingPlots'
           ,'Project for the module DiversitySamplingPlots'
           ,'SamplingPlots'
		   ,1)

UPDATE [dbo].[Project]
   SET [ProjectType] = 'SamplingPlots'
 WHERE ProjectType LIKE '%plots'
end
GO

--ScientificTerms
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'ScientificTerms') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('ScientificTerms'
           ,'Project for the module DiversityScientificTerms'
           ,'ScientificTerms'
		   ,1)

UPDATE [dbo].[Project]
   SET [ProjectType] = 'ScientificTerms'
 WHERE ProjectType LIKE '%terms'
end
GO

--TaxonNames
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'TaxonNames') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('TaxonNames'
           ,'Project for the module DiversityTaxonNames'
           ,'TaxonNames'
		   ,1)

UPDATE [dbo].[Project]
   SET [ProjectType] = 'TaxonNames'
 WHERE ProjectType LIKE '%names'
end
GO

--Regulation
if (select count(*) from [dbo].[ProjectType_Enum] t where t.Code = 'Regulation') = 0
begin
INSERT INTO [dbo].[ProjectType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
		   ,DisplayEnable)
     VALUES
           ('Regulation'
           ,'Regulation as used e.g. in DiversityCollection'
           ,'Regulation'
		   ,1)
end
GO

ENABLE TRIGGER dbo.[trgUpdProject]  ON dbo.[Project]
GO



--#####################################################################################################################
--######  Adaption of trigger of table Project      ###################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelProject] ON [dbo].[Project] 
FOR DELETE AS 
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, ProjectDescription, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectType, deleted.ProjectDescription, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED

GO


ALTER TRIGGER [dbo].[trgUpdProject] ON [dbo].[Project] 
FOR UPDATE AS
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, ProjectDescription, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectType, deleted.ProjectDescription, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED
Update Project
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM Project, deleted 
where 1 = 1 
AND Project.ProjectID = deleted.ProjectID

GO



--#####################################################################################################################
--######   Project_Core: adding ProjectType for use in linked server  #################################################
--#####################################################################################################################


ALTER VIEW [dbo].[Project_Core]
AS
SELECT     P.ProjectID, P.ProjectParentID, P.Project, P.ProjectTitle, P.ProjectType, P.ProjectDescription, P.ProjectNotes, P.ProjectCopyright, P.ProjectRights, P.ProjectLicenseURI, 
                      P.ProjectVersion, P.ProjectURL, cast(P.ProjectSettings as nvarchar(max)) AS ProjectSettings, P.LogInsertedBy, P.LogInsertedWhen, P.LogUpdatedBy, P.LogUpdatedWhen, P.RowGUID
FROM         dbo.Project AS P INNER JOIN
                      dbo.ProjectList() AS L ON P.ProjectID = L.ProjectID

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
RETURN '02.01.09'
END

GO


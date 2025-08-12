
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '00.00.00'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


/****** Object:  DatabaseRole [CacheAdministrator]    Script Date: 26.08.2015 17:20:59 ******/
CREATE ROLE [CacheAdministrator]
GO

/****** Object:  DatabaseRole [CollectionCacheUser]    Script Date: 26.08.2015 17:21:16 ******/
CREATE ROLE [CollectionCacheUser]
GO


GRANT EXEC ON dbo.Version TO CollectionCacheUser
GO



--#####################################################################################################################
--######   [DiversityWorkbenchModule]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) 
DROP FUNCTION [dbo].[DiversityWorkbenchModule]
GO
CREATE FUNCTION [dbo].[DiversityWorkbenchModule] () RETURNS nvarchar(50) AS BEGIN RETURN 'DiversityCollectionCache' END
GO
                
GRANT EXEC ON [DiversityWorkbenchModule] TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   ProjectPublished   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectPublished') = 0
begin
CREATE TABLE [dbo].[ProjectPublished](
	[ProjectID] [int] NOT NULL,
	[Project] [nvarchar](50) NULL,
	[CoordinatePrecision] tinyint NULL,
	[ProjectURI] varchar(255) NULL
 CONSTRAINT [PK_ProjectPublished] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] 

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project to which the specimen belongs (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'ProjectID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or title of the project as shown in a user interface (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'Project'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional reduction of the precision of the coordinates within the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'CoordinatePrecision'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'ProjectURI'

--INSERT INTO [dbo].[ProjectPublished]
--           ([ProjectID]
--           ,[Project])
--SELECT [ProjectID]
--      ,[Project]
--  FROM [dbo].[ProjectProxy]

end
GO

GRANT INSERT ON ProjectPublished TO [CacheAdministrator] 
GO

GRANT DELETE ON ProjectPublished TO [CacheAdministrator] 
GO

GRANT SELECT ON ProjectPublished TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   create view ProjectProxy to look at ProjectPublished   ######################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectProxy') = 0
begin

declare @SQL nvarchar(500)
set @SQL = 'CREATE VIEW [dbo].[ProjectProxy]
	AS
	SELECT        ProjectID, Project, ProjectURI
	FROM           ProjectPublished'
begin try
	exec sp_executesql @SQL
end try
begin catch
end catch
end
GO

GRANT INSERT ON [ProjectProxy] TO [CacheAdministrator] 
GO

GRANT DELETE ON [ProjectProxy] TO [CacheAdministrator] 
GO

GRANT SELECT ON [ProjectProxy] TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.00'
END

GO



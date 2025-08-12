
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

--#####################################################################################################################
--######   Roles                        ###############################################################################
--######   2 Roles, CacheAdmin (insert, delete etc.) and CacheUser (select)                       #####################
--######   Updates of the database etc. are performed by the dbo or sysadmin                      #####################
--#####################################################################################################################

if (select count(*) from sysusers where issqlrole = 1 and name = 'CacheAdmin') = 0
begin
/****** Object:  DatabaseRole [CacheAdmin]    Script Date: 26.08.2015 17:20:59 ******/
	CREATE ROLE [CacheAdmin]
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Role for the administration of the cache database, e.g.  with the permission to transfer data to the cache database' , @level0type=N'USER',@level0name=N'CacheAdmin'
end
GO


if (select count(*) from sysusers where issqlrole = 1 and name = 'CacheUser') = 0
begin
	/****** Object:  DatabaseRole [CacheUser]    Script Date: 26.08.2015 17:21:16 ******/
	CREATE ROLE [CacheUser]
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Role with read only access to the data' , @level0type=N'USER',@level0name=N'CacheUser'
end
GO

EXEC sp_addrolemember 'CacheUser', 'CacheAdmin'
GO

GRANT EXEC ON dbo.Version TO CacheUser
GO

GRANT EXECUTE ON [dbo].[Version] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   [DiversityWorkbenchModule]   ###############################################################################
--######   the function used to identify the database as cache database for DiversityCollection   #####################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) 
DROP FUNCTION [dbo].[DiversityWorkbenchModule]
GO
CREATE FUNCTION [dbo].[DiversityWorkbenchModule] () RETURNS nvarchar(50) AS BEGIN RETURN 'DiversityCollectionCache' END
GO
                
GRANT EXEC ON [DiversityWorkbenchModule] TO [CacheUser]
GO


--#####################################################################################################################
--######   ProjectPublished      ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectPublished') = 0
begin
CREATE TABLE [dbo].[ProjectPublished](
	[ProjectID] [int] NOT NULL,
	[Project] [nvarchar](50) NULL,
	[CoordinatePrecision] tinyint NULL,
	[ProjectURI] varchar(255) NULL,
	[LastUpdatedWhen] [datetime] NULL CONSTRAINT [DF_ProjectPublished_LastUpdatedWhen]  DEFAULT (getdate()),
	[LastUpdatedBy] [nvarchar](50) NULL CONSTRAINT [DF_ProjectPublished_LastUpdatedBy]  DEFAULT (suser_sname()),
 CONSTRAINT [PK_ProjectPublished] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] 

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project to which the specimen belongs (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'ProjectID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or title of the project as shown in a user interface (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'Project'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional reduction of the precision of the coordinates within the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'CoordinatePrecision'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'ProjectURI'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date of the last update of the project data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'LastUpdatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The user reponsible for the last update.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'LastUpdatedBy'


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The projects published via the cache database (Details about the projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished'

end
GO

GRANT INSERT ON ProjectPublished TO [CacheAdmin] 
GO

GRANT DELETE ON ProjectPublished TO [CacheAdmin] 
GO

GRANT SELECT ON ProjectPublished TO [CacheUser]
GO




--#####################################################################################################################
--######   create view ProjectProxy to look at ProjectPublished   #####################################################
--######              enables treatment analog to main database   #####################################################
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

GRANT INSERT ON [ProjectProxy] TO [CacheAdmin] 
GO

GRANT DELETE ON [ProjectProxy] TO [CacheAdmin] 
GO

GRANT SELECT ON [ProjectProxy] TO [CacheUser]
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



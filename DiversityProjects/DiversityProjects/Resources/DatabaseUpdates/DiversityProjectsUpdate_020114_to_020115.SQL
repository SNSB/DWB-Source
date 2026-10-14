
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.14'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   ProjectDefinition - removing missing columns  ##############################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectDefinition') = 1
begin
declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ProjectDefinition]
AS
SELECT     P.ProjectID, P.Project AS ProjectCode, P.ProjectTitle, PP.Project AS SuperiorProject, P.ProjectDescription, 
                       P.ProjectNotes, P.ProjectVersion
FROM         dbo.Project AS P LEFT OUTER JOIN
                      dbo.Project AS PP ON P.ProjectParentID = PP.ProjectID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

end
GO


--#####################################################################################################################
--######   ViewProject  - removing missing columns   ##################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ViewProject') = 1
begin
declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewProject]
AS
SELECT [ProjectID]
      ,[ProjectParentID]
      ,[Project]
      ,[ProjectTitle]
      ,[ProjectDescription]
      ,[ProjectNotes]
      ,[ProjectVersion]
      ,[ProjectURL]
	  ,dbo.StableIdentifier(ProjectID) as StableIdentifier
	  ,ProjectType
	  ,InternalDescription
	  ,PublicDescription
	  ,ProjectDescriptionType
      ,[LogInsertedBy]
      ,[LogInsertedWhen]
      ,[LogUpdatedBy]
      ,[LogUpdatedWhen]
      ,[RowGUID]
  FROM [dbo].[Project]')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

end
GO


--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.08' END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.15'
END

GO


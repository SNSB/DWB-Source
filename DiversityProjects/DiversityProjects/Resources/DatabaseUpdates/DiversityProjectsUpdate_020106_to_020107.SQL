
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.06'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Adding roles      ##########################################################################################
--#####################################################################################################################

if (select count(*) from [dbo].[ProjectAgentRole_Enum] r where r.Code = 'curator') = 0
begin
INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText])
     VALUES
           ('curator'
           ,'curator'
           ,'curator')
end
GO


if (select count(*) from [dbo].[ProjectAgentRole_Enum] r where r.Code = 'author') = 0
begin
INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText])
     VALUES
           ('author'
           ,'author'
           ,'author')
end
GO


--#####################################################################################################################
--######   ProjectIdentifier           ################################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.[ProjectIdentifier] TO [DiversityWorkbenchUser];
GO

GRANT SELECT ON dbo.[ProjectIdentifierType] TO [DiversityWorkbenchUser];
GO

--#####################################################################################################################
--######   Project -> nvarchar(255)    ################################################################################
--#####################################################################################################################

ALTER TABLE Project ALTER COLUMN Project nvarchar(255) not null;
GO

ALTER TABLE Project_log ALTER COLUMN Project nvarchar(255) not null;
GO

--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.02' END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.07'
END

GO


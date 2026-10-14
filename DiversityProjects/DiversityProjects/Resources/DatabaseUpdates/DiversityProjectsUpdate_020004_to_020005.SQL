
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.00.04'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [DF_ProjectAgent_RowGUID]   ######################################################################################
--#####################################################################################################################



ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


--#####################################################################################################################
--######   [ProjectAgentRole_Enum]   ######################################################################################
--#####################################################################################################################

if (SELECT COUNT(*) FROM ProjectAgentRole_Enum WHERE Code = 'editor') = 0
begin
INSERT INTO [dbo].[ProjectAgentRole_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('editor'
           ,'editor'
           ,'editor'
           ,1
           ,1)
end
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.00.05'
END

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.24'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

declare @Version int
set @Version = (select cast(SUBSTRING(@@Version, len('Microsoft SQL Server') + 2, 4) as int))
IF (@Version < 2016)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for SQL Server version 2016 or higher. Current version = ' + cast(@Version as varchar)
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######  New titles in AgentTitle_Enum  ##############################################################################
--#####################################################################################################################

if (select count(*) from AgentTitle_Enum c where c.Code = 'PhD') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('PhD'
           ,'PhD'
           ,'PhD'
           ,1)
end
GO

if (select count(*) from AgentTitle_Enum c where c.Code = 'M.Sc. PhD') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('M.Sc. PhD'
           ,'M.Sc. PhD'
           ,'M.Sc. PhD'
           ,1)
end
GO


if (select count(*) from AgentTitle_Enum c where c.Code = 'RNDr. PhD') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('RNDr. PhD'
           ,'RNDr. PhD'
           ,'RNDr. PhD'
           ,1)
end
GO


if (select count(*) from AgentTitle_Enum c where c.Code = 'Dr. rer. nat.') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Dr. rer. nat.'
           ,'Dr. rer. nat.'
           ,'Dr. rer. nat.'
           ,1)
end
GO


if (select count(*) from AgentTitle_Enum c where c.Code = 'Prof. Ing. DrSc.') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Prof. Ing. DrSc.'
           ,'Prof. Ing. DrSc.'
           ,'Prof. Ing. DrSc.'
           ,1)
end
GO


if (select count(*) from AgentTitle_Enum c where c.Code = 'M.Sc.') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('M.Sc.'
           ,'M.Sc.'
           ,'M.Sc.'
           ,1)
end
GO


if (select count(*) from AgentTitle_Enum c where c.Code = 'Univ. Prof. Dr.') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Univ. Prof. Dr.'
           ,'Univ. Prof. Dr.'
           ,'Univ. Prof. Dr.'
           ,1)
end
GO


if (select count(*) from AgentTitle_Enum c where c.Code = 'Univ. Dipl. Biologe') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Univ. Dipl. Biologe'
           ,'Univ. Dipl. Biologe'
           ,'Univ. Dipl. Biologe'
           ,1)
end
GO

if (select count(*) from AgentTitle_Enum c where c.Code = 'Dipl.-Ing.') = 0
begin
INSERT INTO [dbo].[AgentTitle_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Dipl.-Ing.'
           ,'Dipl.-Ing.'
           ,'Dipl.-Ing.'
           ,1)
end
GO



--#####################################################################################################################
--######  New gender diverse in AgentGender_Enum  #####################################################################
--#####################################################################################################################

if (select count(*) from AgentGender_Enum c where c.Code = 'diverse') = 0
begin
INSERT INTO [dbo].[AgentGender_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('diverse'
           ,'diverse'
           ,'diverse'
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
RETURN '02.01.25'
END

GO

select [dbo].[Version] ()  


declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.07'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Descriptions for Recordbasis  ##############################################################################
--#####################################################################################################################

update CollMaterialCategory_Enum set Description = 'A record describing a preserved specimen (not living)'
where Code = 'preserved specimen'

update CollMaterialCategory_Enum set Description = 'A record describing a preserved specimen that is a fossil'
where Code = 'fossil specimen'

update CollMaterialCategory_Enum set Description = 'A record describing a specimen which is alive (not preserved)'
where Code = 'living specimen'

update CollRetrievalType_Enum set Description = 'A record describing an output of a human observation process'
where Code = 'human observation'

update CollRetrievalType_Enum set Description = 'A record describing an output of a machine observation process'
where Code = 'machine observation'

update CollRetrievalType_Enum set Description = 'A record describing an output of a literature research'
where Code = 'Literature'

update CollMaterialCategory_Enum set Description = 'A record describing a static visual representation (digital or physical)'
where Code = 'drawing or photograph'


--#####################################################################################################################
--######   Inserting missing material categories  #####################################################################
--#####################################################################################################################

if (select count(*) from [dbo].[CollMaterialCategory_Enum] M where M.Code = 'mineral specimen') = 0
begin
INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('mineral specimen'
           ,'A record describing a preserved specimen which is a mineral'
           ,'mineral specimen'
           ,800
           ,1
           ,'other specimen')
end
GO

if (select count(*) from [dbo].[CollMaterialCategory_Enum] M where M.Code = 'earth science specimen') = 0
begin
INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('earth science specimen'
           ,'A record describing a geo-scientific object which is no pure mineral, rock or fossil specimen (but a mixture), or a geo-scientific specimen with unknown traits'
           ,'earth science specimen'
           ,810
           ,1
           ,'other specimen')
end
GO

if (select count(*) from [dbo].[CollMaterialCategory_Enum] M where M.Code = 'material sample') = 0
begin
INSERT INTO [dbo].[CollMaterialCategory_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('material sample'
           ,'A record describing the physical result of a sampling (or subsampling) event (sample either preserved or destructively processed)'
           ,'material sample'
           ,820
           ,1
           ,'other specimen')
end
GO


--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.08' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.08'
END

GO


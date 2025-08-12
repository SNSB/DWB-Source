declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.23'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   Collection -1   ######################################################################################
--#####################################################################################################################
declare @i int;
set @i = (select COUNT(*) from [Collection] where [CollectionID] = -1);
if @i = 0
begin

set identity_insert [Collection] on;

INSERT INTO [dbo].[Collection]
           ([CollectionID]
           ,[CollectionName])
     VALUES
           (-1
           ,'No collection')
           
set identity_insert [Collection] on;

end
go



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.24'
END

GO

select [dbo].[Version] ()  


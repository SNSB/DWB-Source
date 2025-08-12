declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.78'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   SetXmlValue    #############################################################################################
--#####################################################################################################################

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON -- important - otherwise execution will fail
GO

CREATE Procedure [dbo].[SetXmlValue] 
@Table nvarchar(128), 
@Column nvarchar(128), 
@Path nvarchar(4000),  
@Value nvarchar(4000),
@WhereClause nvarchar(4000)
AS  
/*
Setting a value of an XML node

Test:
EXEC dbo.SetXmlValue 'UserProxy', 'Settings', '/Settings/ModuleSource/Identification/fungus', '[TNT.DIVERSITYWORKBENCH.DE,5432].DiversityTaxonNames_Animalia', 'LoginName = USER_NAME()'

*/
BEGIN 
if len(@Value) = 0 begin set @Value = ' ' end
declare @SQL nvarchar(max)
set @SQL = (select 'DECLARE @Setting xml;
SET @Setting = (SELECT T.'+ @Column + ' FROM ' + @Table + ' AS T WHERE ' + @WhereClause + ');
set @Setting.modify(''replace value of (' + @Path + '/text())[1] with "' + @Value + '"'');
update T set T.' + @Column + ' = @Setting 
FROM ' + @Table + ' AS T  
WHERE ' + @WhereClause)

begin try
exec sp_executesql @SQL
end try
begin catch
end catch

END

GO

GRANT EXEC ON [dbo].[SetXmlValue] TO [User]
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.79'
END

GO


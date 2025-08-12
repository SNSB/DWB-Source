declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.80'
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

ALTER Procedure [dbo].[SetXmlValue] 
@Table nvarchar(128), 
@Column nvarchar(128), 
@Path nvarchar(4000),  
@Value nvarchar(4000),
@WhereClause nvarchar(4000)
AS  
/*
Setting a value of an XML node
if the value is empty, the value will be set to ' ' for otherwise an update is not possible any more

Test:
EXEC dbo.SetXmlValue 'UserProxy', 'Settings', '/Settings/ModuleSource/Identification/fungus', '[TNT.DIVERSITYWORKBENCH.DE,5432].DiversityTaxonNames_Animalia', 'LoginName = USER_NAME()'

*/
BEGIN 
declare @SQL nvarchar(max)
if len(@Value) = 0 begin set @Value = ' ' end
set @SQL = (select 'SET ANSI_NULLS ON;
DECLARE @Setting xml;
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

--#####################################################################################################################
--######   SetXmlAttribute    #########################################################################################
--#####################################################################################################################

CREATE Procedure [dbo].[SetXmlAttribute] 
@Table nvarchar(128), 
@Column nvarchar(128), 
@Path nvarchar(4000),  
@Attribute nvarchar(128),
@Value nvarchar(4000),
@WhereClause nvarchar(4000)
AS  
/*
Setting a value of an XML node

Test:
EXEC dbo.SetXmlAttribute 'UserProxy', 'Settings', '/Settings/ModuleSource/Identification/TaxonomicGroup/insect', 'reponse', 'terse', 'LoginName = USER_NAME()'
SELECT [Settings] FROM [dbo].[UserProxy] WHERE LoginName = USER_NAME()

declare @Table nvarchar(128), 
@Column nvarchar(128), 
@Path nvarchar(4000),  
@Attribute nvarchar(128),
@Value nvarchar(4000),
@WhereClause nvarchar(4000)
set @Table = 'UserProxy'
set @Column = 'Settings'
set @Path = '/Settings/ModuleSource/Identification/TaxonomicGroup/insect'
set @Attribute = 'reponse'
set @Value = 'full'
set @WhereClause = 'LoginName = USER_NAME()'

declare @SQL nvarchar(max)
set @SQL = (select 'DECLARE @Setting xml;
SET @Setting = (SELECT T.'+ @Column + ' FROM ' + @Table + ' AS T WHERE ' + @WhereClause + ');
set @Setting.modify(''insert attribute ' + @Attribute + '{"' + @Value + '"} into (' + @Path + ')[1];
update T set T.' + @Column + ' = @Setting 
FROM ' + @Table + ' AS T  
WHERE ' + @WhereClause)

*/
BEGIN 
declare @SQL nvarchar(max)
if len(@Value) = 0 begin set @Value = ' ' end
-- try to insert the attribute
set @SQL = (select 'SET ANSI_NULLS ON;
DECLARE @Setting xml;
SET @Setting = (SELECT T.'+ @Column + ' FROM ' + @Table + ' AS T WHERE ' + @WhereClause + ');
set @Setting.modify(''insert attribute ' + @Attribute + '{"' + @Value + '"} into (' + @Path + ')[1]'');
update T set T.' + @Column + ' = @Setting 
FROM ' + @Table + ' AS T  
WHERE ' + @WhereClause)

begin try
exec sp_executesql @SQL
end try
begin catch
end catch
	-- if insert failed try to update the attribute
	set @SQL = (select 'SET ANSI_NULLS ON;
	DECLARE @Setting xml;
	SET @Setting = (SELECT T.'+ @Column + ' FROM ' + @Table + ' AS T WHERE ' + @WhereClause + ');
	set @Setting.modify(''replace value of (' + @Path + '/@' + @Attribute + ')[1] with "' + @Value + '"'');
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

GRANT EXEC ON  [dbo].[SetXmlAttribute]  TO [User]
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.81'
END

GO


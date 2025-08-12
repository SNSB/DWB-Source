declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.79'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   spider         #############################################################################################
--#####################################################################################################################

INSERT INTO [dbo].[CollTaxonomicGroup_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('spider'
           ,'Spider: Including Araneae (Mesothelae, Mygalomorphae and Araneomorphae)'
           ,'spider'
           ,1
           ,'arthropod')
GO



--#####################################################################################################################
--######   IdentificationUnit_Core   ##################################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[IdentificationUnit_Core]
AS
SELECT        U.CollectionSpecimenID, U.IdentificationUnitID, U.LastIdentificationCache, U.FamilyCache, U.OrderCache, U.TaxonomicGroup, U.OnlyObserved, U.RelatedUnitID, 
                         U.RelationType, U.ColonisedSubstratePart, U.LifeStage, U.Gender, U.NumberOfUnits, U.ExsiccataNumber, U.ExsiccataIdentification, U.UnitIdentifier, 
                         U.UnitDescription, U.Circumstances, U.DisplayOrder, U.Notes, U.HierarchyCache, U.RetrievalType, U.ParentUnitID, U.DataWithholdingReason, 
                         U.NumberOfUnitsModifier
FROM            dbo.IdentificationUnit AS U INNER JOIN
                         dbo.CollectionSpecimenID_UserAvailable AS A ON U.CollectionSpecimenID = A.CollectionSpecimenID

GO



--#####################################################################################################################
--######   SetXmlValue    #############################################################################################
--#####################################################################################################################

DROP Procedure  [dbo].[SetXmlValue] 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON  -- important - otherwise execution will fail
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
if the value is empty, the value will be set to ' ' for otherwise an update is not possible any more

Test:
EXEC dbo.SetXmlValue 'UserProxy', 'Settings', '/Settings/ModuleSource/Identification/fungus', '[TNT.DIVERSITYWORKBENCH.DE,5432].DiversityTaxonNames_Animalia', 'LoginName = USER_NAME()'

*/
BEGIN 
declare @SQL nvarchar(max)
if len(@Value) = 0 begin set @Value = ' ' end
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
RETURN '02.05.80'
END

GO


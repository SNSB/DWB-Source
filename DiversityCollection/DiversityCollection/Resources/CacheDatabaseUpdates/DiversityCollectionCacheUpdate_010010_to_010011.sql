
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.10'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   ViewIdentificationUnit   ###################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ViewIdentificationUnit]
AS
SELECT  DISTINCT       U.CollectionSpecimenID, U.IdentificationUnitID, U.LastIdentificationCache, U.TaxonomicGroup, U.RelatedUnitID, U.RelationType, U.ExsiccataNumber, 
U.DisplayOrder, U.ColonisedSubstratePart, U.FamilyCache, U.OrderCache, U.LifeStage, U.Gender, U.HierarchyCache, U.UnitIdentifier, U.UnitDescription, 
U.Circumstances, U.Notes, U.NumberOfUnits, U.OnlyObserved, U.RetrievalType
FROM            ' +  dbo.SourceDatabase() + '.dbo.IdentificationUnit AS U INNER JOIN
dbo.ViewCollectionSpecimen AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID
WHERE        (U.DataWithholdingReason IS NULL) OR
(U.DataWithholdingReason = N'''')')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.08'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO





--#####################################################################################################################
--######   ViewCollectionSpecimenProcessing   #########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenProcessing]
AS
SELECT   DISTINCT      P.CollectionSpecimenID, P.SpecimenProcessingID, P.ProcessingDate, P.ProcessingID, P.Protocoll, P.ProcessingDuration, P.ResponsibleName, P.Notes, P.SpecimenPartID
FROM            dbo.ViewCollectionSpecimenPart AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenProcessing AS P ON S.SpecimenPartID = P.SpecimenPartID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('ViewCollectionSpecimenProcessing') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View for all not withheld collection specimen processings' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewCollectionSpecimenProcessing'
GO

GRANT SELECT ON ViewCollectionSpecimenProcessing TO [CacheUser]
GO






--#####################################################################################################################
--######   ViewCollectionSpecimenImage   ##############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimenImage]
AS
SELECT        I.CollectionSpecimenID, I.URI, I.ResourceURI, I.SpecimenPartID, I.IdentificationUnitID, I.ImageType, I.Notes, I.LicenseURI, I.LicenseNotes, I.DisplayOrder, 
I.LicenseYear, I.LicenseHolderAgentURI, I.LicenseHolder, I.LicenseType, I.CopyrightStatement, I.CreatorAgentURI, I.CreatorAgent, I.IPR, I.Title
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenImage AS I 
WHERE        (I.DataWithholdingReason IS NULL OR
I.DataWithholdingReason = N'''')')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

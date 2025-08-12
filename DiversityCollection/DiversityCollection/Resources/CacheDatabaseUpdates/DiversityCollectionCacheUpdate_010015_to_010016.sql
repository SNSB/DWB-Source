
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.15'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Geography in Localisation    ###############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ViewCollectionEventLocalisation]
AS
SELECT   DISTINCT     L.CollectionEventID, L.LocalisationSystemID, L.Location1, L.Location2, L.LocationAccuracy, L.LocationNotes, L.DeterminationDate, L.DistanceToLocation, 
L.DirectionToLocation, L.ResponsibleName, L.ResponsibleAgentURI, L.AverageAltitudeCache, L.AverageLatitudeCache, L.AverageLongitudeCache, 
L.RecordingMethod, L.[Geography].ToString() AS [Geography]
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionEventLocalisation AS L INNER JOIN
dbo.ViewCollectionEvent AS E ON L.CollectionEventID = E.CollectionEventID
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 7, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   CollectorsAgentURI           ###############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER VIEW [dbo].[ViewCollectionAgent]
AS
SELECT    DISTINCT     TOP (100) PERCENT A.CollectionSpecimenID, A.CollectorsName, A.CollectorsSequence, A.CollectorsNumber, A.CollectorsAgentURI
FROM            dbo.ViewCollectionSpecimen AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionAgent AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID
WHERE        (A.DataWithholdingReason = N'''') OR
                         (A.DataWithholdingReason IS NULL)
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 7, 80000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######  ProjectPublished - TransferProtocol   #######################################################################
--#####################################################################################################################

ALTER TABLE ProjectPublished ADD [TransferProtocol] [nvarchar](max) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the transfer of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'TransferProtocol'
GO

--#####################################################################################################################
--######  ProjectPublished - IncludeInTransfer   ######################################################################
--#####################################################################################################################

ALTER TABLE ProjectPublished ADD IncludeInTransfer bit NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the project should be included in a schedule based data transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'IncludeInTransfer'
GO


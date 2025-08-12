
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.04'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--#####################################################################################################################
--######   Views for published data   ###################################
--#####################################################################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   ViewCollectionEvent   #########################################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionEvent]
AS
SELECT   DISTINCT     E.CollectionEventID, E.Version, E.CollectorsEventNumber, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionDate END AS CollectionDate, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionDay END AS CollectionDay, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionMonth END AS CollectionMonth, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionYear END AS CollectionYear, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionDateSupplement END AS CollectionDateSupplement, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionTime END AS CollectionTime, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionTimeSpan END AS CollectionTimeSpan, 
E.LocalityDescription, E.HabitatDescription, E.ReferenceTitle, E.CollectingMethod, E.Notes, E.CountryCache, 
E.ReferenceDetails, E.LocalityVerbatim, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionEndDay END AS CollectionEndDay, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionEndMonth END AS CollectionEndMonth, 
CASE WHEN E.DataWithholdingReasonDate <> '''' THEN NULL ELSE E.CollectionEndYear END AS CollectionEndYear
FROM dbo.ViewCollectionSpecimen AS S INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID
WHERE        (E.DataWithholdingReason = '''') OR
(E.DataWithholdingReason IS NULL)')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.05'
END

GO



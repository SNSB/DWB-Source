
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.05'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   adding missing kindom  ######################################################################################
--#####################################################################################################################

if (select count(*) from [EnumKingdom] where Kingdom = 'Viruses') = 0
begin
INSERT [dbo].[EnumKingdom] ([Kingdom]) VALUES (N'Viruses')
end
GO


if (select count(*) from [EnumTaxonomicGroup] where [Code] = 'virus') = 0
begin
INSERT [dbo].[EnumTaxonomicGroup] ([Code], [DisplayText], [Kingdom]) VALUES (N'virus', N'virus', N'Viruses')
end
GO

--#####################################################################################################################
--######   CollectionEvent  ######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[CollectionEvent]
AS
SELECT        CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, 
                         CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, Notes, 
                         CountryCache, DataWithholdingReason
FROM            ' + dbo.SourceDatebase() + '.dbo.CollectionEvent
WHERE        (DataWithholdingReason = '''') OR
                         (DataWithholdingReason IS NULL)')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON CollectionEvent TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   CollectionEventLithostratigraphy  ######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[CollectionEventLithostratigraphy]
AS
SELECT        P.CollectionEventID, P.PropertyID, 
replace (
CASE WHEN P.PropertyHierarchyCache is null 
THEN P.DisplayText
ELSE P.DisplayText + 
	case when P.DisplayText = P.PropertyHierarchyCache 
	then '''' 
	else 
		case when ltrim(replace (P.PropertyHierarchyCache, ''/'', ''|'')) like ''| %'' 
		then '''' 
		else '' | '' 
		end 
	+  P.PropertyHierarchyCache 
	end 
END, ''/'', ''|'') AS Display, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.ResponsibleName, P.ResponsibleAgentURI, 
                         P.Notes, P.AverageValueCache
FROM            ' + dbo.SourceDatebase() + '.dbo.CollectionEventProperty AS P INNER JOIN
                         dbo.CollectionEvent ON P.CollectionEventID = dbo.CollectionEvent.CollectionEventID
WHERE        (P.PropertyID = 30)')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON CollectionEventLithostratigraphy TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   CollectionEventChronostratigraphy  ######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[CollectionEventChronostratigraphy]
AS
SELECT        P.CollectionEventID, P.PropertyID, 
replace (
CASE WHEN P.PropertyHierarchyCache is null 
THEN P.DisplayText
ELSE P.DisplayText + 
	case when P.DisplayText = P.PropertyHierarchyCache 
	then '''' 
	else 
		case when ltrim(replace (P.PropertyHierarchyCache, ''/'', ''|'')) like ''| %'' 
		then '''' 
		else '' | '' 
		end 
	+  P.PropertyHierarchyCache 
	end 
END, ''/'', ''|'') AS Display, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, 
                         P.ResponsibleName, P.ResponsibleAgentURI, P.Notes, P.AverageValueCache
FROM            ' + dbo.SourceDatebase() + '.dbo.CollectionEventProperty AS P INNER JOIN
                         dbo.CollectionEvent ON P.CollectionEventID = dbo.CollectionEvent.CollectionEventID
WHERE        (P.PropertyID = 20)')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON CollectionEventChronostratigraphy TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   CollectionProject  ######################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[CollectionProject]
AS
SELECT        C.CollectionSpecimenID, C.ProjectID, PP.Project
FROM            ' + dbo.SourceDatebase() + '.dbo.CollectionProject AS C INNER JOIN
                         dbo.ProjectProxy AS PP ON C.ProjectID = PP.ProjectID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON CollectionProject TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   Changing view CollectionSpecimen  ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[CollectionSpecimen]
AS
SELECT        S.CollectionSpecimenID, NULL AS Version, LTRIM(RTRIM(S.AccessionNumber)) AS AccessionNumber, CASE WHEN 1 = 1 THEN CASE WHEN E.CollectionYear IS NULL
                          THEN NULL ELSE CAST(E.CollectionYear AS VARCHAR) + CASE WHEN E.CollectionMonth IS NULL THEN '''' ELSE + ''-'' + REPLICATE(''0'', 
                         2 - LEN(CAST(E.CollectionMonth AS VARCHAR))) + CAST(E.CollectionMonth AS VARCHAR) + CASE WHEN E.CollectionDay IS NULL THEN '''' ELSE ''-'' + REPLICATE(''0'', 
                         2 - LEN(CAST(E.CollectionDay AS VARCHAR))) + CAST(E.CollectionDay AS VARCHAR) END END END ELSE NULL END AS CollectionDate, E.CollectionDay, 
                         E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, E.LocalityDescription, E.CountryCache, S.LabelTranscriptionNotes, 
                         CASE WHEN S.ExsiccataAbbreviation LIKE ''<%>'' THEN NULL ELSE S.ExsiccataAbbreviation END AS ExsiccataAbbreviation, S.OriginalNotes, 
                         E.CollectorsEventNumber, E.DataWithholdingReason AS EventDataWithholdingReason, dbo.CollectionEventChronostratigraphy.Display AS Chronostratigraphy, 
                         dbo.CollectionEventLithostratigraphy.DisplayText AS Lithostratigraphy, S.LogUpdatedWhen
FROM            dbo.CollectionEventLithostratigraphy RIGHT OUTER JOIN
                         dbo.CollectionEvent AS E ON dbo.CollectionEventLithostratigraphy.CollectionEventID = E.CollectionEventID LEFT OUTER JOIN
                         dbo.CollectionEventChronostratigraphy ON E.CollectionEventID = dbo.CollectionEventChronostratigraphy.CollectionEventID 
						 RIGHT OUTER JOIN ' + dbo.SourceDatebase() + '.dbo.CollectionSpecimen AS S INNER JOIN
                         dbo.CollectionProject ON S.CollectionSpecimenID = dbo.CollectionProject.CollectionSpecimenID ON E.CollectionEventID = S.CollectionEventID
WHERE        (S.DataWithholdingReason = '''') OR
                         (S.DataWithholdingReason IS NULL)
GROUP BY S.CollectionSpecimenID, S.AccessionNumber, E.CollectionDay, E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, E.LocalityDescription, 
                         E.CountryCache, S.LabelTranscriptionNotes, S.OriginalNotes, E.CollectorsEventNumber, E.DataWithholdingReason, dbo.CollectionEventChronostratigraphy.Display, 
                         dbo.CollectionEventLithostratigraphy.DisplayText, S.ExsiccataAbbreviation, S.LogUpdatedWhen')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch
GO

GRANT SELECT ON CollectionSpecimen TO [CollectionCacheUser]
GO



--#####################################################################################################################
--######   procTransferCollectionSpecimen  ######################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procTransferCollectionSpecimen] 
AS
TRUNCATE TABLE CollectionSpecimenCache

INSERT INTO CollectionSpecimenCache
                      (CollectionSpecimenID, Version, AccessionNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, LocalityDescription, 
                      CountryCache, LabelTranscriptionNotes, ExsiccataAbbreviation, OriginalNotes, CollectorsEventNumber, Chronostratigraphy, Lithostratigraphy, DateLastEdited)
SELECT     CollectionSpecimenID, Version, AccessionNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, LocalityDescription, 
                      CountryCache, LabelTranscriptionNotes, ExsiccataAbbreviation, OriginalNotes, CollectorsEventNumber, Chronostratigraphy, Lithostratigraphy, LogUpdatedWhen
FROM         CollectionSpecimen

DECLARE @i int
SET @i = (SELECT COUNT(*) FROM CollectionSpecimenCache)
SELECT CAST(@i AS VARCHAR) + '' collection specimen imported''')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch
GO


GRANT EXEC ON procTransferCollectionSpecimen TO [CollectionCacheUser]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.06'
END

GO



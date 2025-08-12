
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.33'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   Tools   ######################################################################################
--#####################################################################################################################


GRANT EXECUTE ON XML SCHEMA COLLECTION::Tools TO [User]
GO


--#####################################################################################################################
--######   [DiversityMobile_EventsForProject]   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[DiversityMobile_EventsForProject] (@ProjectID int, @Locality nvarchar(500))   
RETURNS @Events TABLE (
	[CollectionEventID] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[SeriesID] [int] NULL,
	[CollectorsEventNumber] [nvarchar](50) NULL,
	[CollectionDate] [datetime] NULL,
	[CollectionDay] [int] NULL,
	[CollectionMonth] [int] NULL,
	[CollectionYear] [int] NULL,
	[CollectionDateSupplement] [nvarchar](100) NULL,
	[CollectionDateCategory] [nvarchar](50) NULL,
	[CollectionTime] [varchar](50) NULL,
	[CollectionTimeSpan] [varchar](50) NULL,
	[LocalityDescription] [nvarchar](max) NULL,
	[HabitatDescription] [nvarchar](max) NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[CollectingMethod] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CountryCache] [nvarchar](50) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL)
/* 
Returns a table that lists all the analysis items related to the given project. 
MW 08.08.2009 
TEST: 
Select * from DiversityMobile_AnalysisProjectList(3)  
Select * from DiversityMobile_AnalysisProjectList(372)  
*/ 
AS BEGIN  

INSERT INTO @Events
           ([CollectionEventID]
           ,[Version]
           ,[SeriesID]
           ,[CollectorsEventNumber]
           ,[CollectionDate]
           ,[CollectionDay]
           ,[CollectionMonth]
           ,[CollectionYear]
           ,[CollectionDateSupplement]
           ,[CollectionDateCategory]
           ,[CollectionTime]
           ,[CollectionTimeSpan]
           ,[LocalityDescription]
           ,[HabitatDescription]
           ,[ReferenceTitle]
           ,[ReferenceURI]
           ,[ReferenceDetails]
           ,[CollectingMethod]
           ,[Notes]
           ,[CountryCache]
           ,[DataWithholdingReason]
           ,[LogCreatedWhen]
           ,[LogCreatedBy]
           ,[LogUpdatedWhen]
           ,[LogUpdatedBy]
           ,[RowGUID])
SELECT DISTINCT    E.CollectionEventID, E.Version, E.SeriesID, E.CollectorsEventNumber, E.CollectionDate, E.CollectionDay, E.CollectionMonth, E.CollectionYear, 
      E.CollectionDateSupplement, E.CollectionDateCategory, E.CollectionTime, E.CollectionTimeSpan, E.LocalityDescription, E.HabitatDescription, E.ReferenceTitle, 
      E.ReferenceURI, E.ReferenceDetails, E.CollectingMethod, E.Notes, E.CountryCache, E.DataWithholdingReason, E.LogCreatedWhen, E.LogCreatedBy, 
      E.LogUpdatedWhen, E.LogUpdatedBy, E.RowGUID
FROM         CollectionEvent AS E INNER JOIN
      CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID INNER JOIN
      CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID
WHERE     (P.ProjectID = @ProjectID)
AND E.LocalityDescription LIKE '%' + @Locality + '%'


RETURN 
END  

GO


--#####################################################################################################################
--######   CollectionSpecimen_log   ######################################################################################
--#####################################################################################################################

GRANT DELETE ON CollectionSpecimen_log TO [Administrator]
GO

--#####################################################################################################################
--######   CacheDB   ######################################################################################
--#####################################################################################################################

CREATE VIEW [CacheDB_CollectionAgent] 
AS SELECT TOP (100) PERCENT CollectionSpecimenID, CollectorsName, CollectorsSequence, CollectorsNumber 
FROM CollectionAgent AS A 
WHERE (DataWithholdingReason = N'') OR (DataWithholdingReason IS NULL) 
ORDER BY CollectorsName
GO

GRANT SELECT ON [CacheDB_CollectionAgent] TO [CacheUser]
GO

CREATE VIEW [dbo].[CacheDB_CollectionEvent]  
AS SELECT CollectionEventID, Version, SeriesID, CollectorsEventNumber, 
CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionDateCategory, 
CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, Notes, CountryCache, DataWithholdingReason 
FROM DiversityCollection_Test.dbo.CollectionEvent AS E 
WHERE (DataWithholdingReason = '') OR (DataWithholdingReason IS NULL)
GO

GRANT SELECT ON [CacheDB_CollectionEvent] TO [CacheUser]
GO

CREATE VIEW [CacheDB_CollectionEventChronostratigraphy] 
AS SELECT P.CollectionEventID, P.PropertyID, CASE WHEN P.PropertyURI IS NULL THEN CASE WHEN P.DisplayText = substring(P.PropertyHierarchyCache, 1, LEN(P.DisplayText)) 
THEN P.PropertyHierarchyCache ELSE P.DisplayText + P.PropertyHierarchyCache END ELSE CASE WHEN P.DisplayText = P.PropertyHierarchyCache THEN P.DisplayText 
ELSE P.DisplayText + P.PropertyHierarchyCache END END AS Display, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.ResponsibleName, 
P.ResponsibleAgentURI, P.Notes, P.AverageValueCache 
FROM CollectionEventProperty AS P INNER JOIN [CacheDB_CollectionEvent] AS E ON P.CollectionEventID = E.CollectionEventID 
WHERE (P.PropertyID = 20)
GO

GRANT SELECT ON [CacheDB_CollectionEventChronostratigraphy] TO [CacheUser]
GO

CREATE VIEW [CacheDB_CollectionEventLithostratigraphy] 
AS SELECT     P.CollectionEventID, P.PropertyID, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.ResponsibleName,  P.ResponsibleAgentURI, 
P.Notes, P.AverageValueCache 
FROM         CollectionEventProperty AS P 
INNER JOIN [CacheDB_CollectionEvent] AS E ON P.CollectionEventID = E.CollectionEventID 
WHERE     (P.PropertyID = 30) 
GO

GRANT SELECT ON [CacheDB_CollectionEventLithostratigraphy] TO [CacheUser]
GO

CREATE VIEW [CacheDB_CollectionProject] 
AS SELECT     C.CollectionSpecimenID, C.ProjectID, PP.Project 
FROM         CollectionProject AS C INNER JOIN ProjectProxy AS PP ON C.ProjectID = PP.ProjectID 
GO

GRANT SELECT ON [CacheDB_CollectionProject] TO [CacheUser]
GO

CREATE VIEW [dbo].[CacheDB_Identification_Last]
AS
SELECT     CollectionSpecimenID, IdentificationUnitID, MAX(IdentificationSequence) AS IdentificationSequence
FROM         DiversityCollection.dbo.Identification
GROUP BY CollectionSpecimenID, IdentificationUnitID
GO
GRANT SELECT ON [CacheDB_Identification_Last] TO [CacheUser]
GO

CREATE VIEW [dbo].[CacheDB_Identification_First]
AS
SELECT     CollectionSpecimenID, IdentificationUnitID, MIN(IdentificationSequence) AS IdentificationSequence
FROM         DiversityCollection.dbo.Identification
GROUP BY CollectionSpecimenID, IdentificationUnitID
GO
GRANT SELECT ON [CacheDB_Identification_First] TO [CacheUser]
GO

CREATE VIEW [dbo].[CacheDB_Identification_FirstEntry]
AS
SELECT     I.CollectionSpecimenID, I.IdentificationUnitID, CASE WHEN I.TaxonomicName IS NULL OR
                      I.TaxonomicName = '' THEN I.VernacularTerm ELSE I.TaxonomicName END AS TaxonomicNameFirstEntry
FROM         Identification AS I INNER JOIN
                      CacheDB_Identification_First F ON I.CollectionSpecimenID = F.CollectionSpecimenID AND 
                      I.IdentificationUnitID = F.IdentificationUnitID AND I.IdentificationSequence = F.IdentificationSequence
GO
GRANT SELECT ON [CacheDB_Identification_FirstEntry] TO [CacheUser]
GO

--#####################################################################################################################
--######   CacheDB   ######################################################################################
--#####################################################################################################################

--if (select COUNT(*) from INFORMATION_SCHEMA.ROUTINES R 
--where R.ROUTINE_TYPE = 'FUNCTION'
--and R.ROUTINE_NAME = 'ProcessingHierarchyAll') = 0 
--begin
CREATE FUNCTION [dbo].[ProcessingHierarchyAll] ()  
RETURNS @ProcessingList TABLE ([ProcessingID] [int] Primary key ,
	[ProcessingParentID] [int] NULL ,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[ProcessingURI] [varchar](255) NULL,
	[OnlyHierarchy] [bit] NULL,
	[HierarchyDisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the Processing items related to the given Processing.
MW 02.01.2006
TEST:
SELECT [HierarchyDisplayText], * FROM DBO.ProcessingHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @ProcessingList (ProcessingID, ProcessingParentID, DisplayText, Description,  Notes, ProcessingURI, OnlyHierarchy, HierarchyDisplayText)
SELECT DISTINCT ProcessingID, ProcessingParentID, DisplayText, Description,  Notes, ProcessingURI, OnlyHierarchy
, DisplayText
FROM Processing
WHERE Processing.ProcessingParentID IS NULL

declare @i int
set @i = (select count(*) from Processing where ProcessingID not IN (select ProcessingID from  @ProcessingList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @ProcessingList (ProcessingID, ProcessingParentID, DisplayText, Description,  Notes, ProcessingURI, OnlyHierarchy, HierarchyDisplayText)
	SELECT DISTINCT C.ProcessingID, C.ProcessingParentID, C.DisplayText, C.Description,  C.Notes, C.ProcessingURI, C.OnlyHierarchy, L.HierarchyDisplayText + ' | ' + C.DisplayText
	FROM Processing C, @ProcessingList L
	WHERE C.ProcessingParentID = L.ProcessingID
	AND C.ProcessingID NOT IN (select ProcessingID from  @ProcessingList)

	set @i = (select count(*) from Processing where ProcessingID not IN (select ProcessingID from  @ProcessingList))
end

   RETURN
END


GO

GRANT SELECT ON [dbo].[ProcessingHierarchyAll] TO [USER]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.34'
END

GO



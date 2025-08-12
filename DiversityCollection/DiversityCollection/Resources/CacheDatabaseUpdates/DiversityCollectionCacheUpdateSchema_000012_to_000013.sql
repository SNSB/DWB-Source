
--#####################################################################################################################
--######   Geography in CacheCollectionEventLocalisation  #############################################################
--#####################################################################################################################

if (
select count(*) from INFORMATION_SCHEMA.COLUMNS C 
where C.Column_Name = 'Geography'
and C.TABLE_NAME = 'CacheCollectionEventLocalisation'
and C.TABLE_SCHEMA = '#project#')  = 0
begin
ALTER TABLE [#project#].CacheCollectionEventLocalisation ADD [Geography] nvarchar(MAX)
end
GO

--#####################################################################################################################
--######   procPublishCollectionEventLocalisation  ####################################################################
--#####################################################################################################################


ALTER  PROCEDURE [#project#].[procPublishCollectionEventLocalisation] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEventLocalisation]
*/
truncate table [#project#].CacheCollectionEventLocalisation

INSERT INTO [#project#].[CacheCollectionEventLocalisation]
           ([CollectionEventID]
      ,[LocalisationSystemID]
      ,[Location1]
      ,[Location2]
      ,[LocationAccuracy]
      ,[LocationNotes]
      ,[DeterminationDate]
      ,[DistanceToLocation]
      ,[DirectionToLocation]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AverageAltitudeCache]
      ,[AverageLatitudeCache]
      ,[AverageLongitudeCache]
      ,[RecordingMethod]
	  ,[Geography])
SELECT DISTINCT E.[CollectionEventID]
      ,E.[LocalisationSystemID]
      ,[Location1]
      ,[Location2]
      ,[LocationAccuracy]
      ,[LocationNotes]
      ,[DeterminationDate]
      ,[DistanceToLocation]
      ,[DirectionToLocation]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[AverageAltitudeCache]
      ,[AverageLatitudeCache]
      ,[AverageLongitudeCache]
      ,[RecordingMethod]
	  ,[Geography]
  FROM [dbo].[ViewCollectionEventLocalisation] E,
dbo.ViewCollectionSpecimen S, 
dbo.ViewCollectionProject P,
[#project#].CacheLocalisationSystem L
  where E.CollectionEventID = S.CollectionEventID 
  AND S.CollectionSpecimenID = p.CollectionSpecimenID
  AND L.[LocalisationSystemID] = E.[LocalisationSystemID]
  and p.ProjectID = [#project#].ProjectID()

if not (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())  is null
begin
	declare @Precision int
	set @Precision = (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())

	-- rounding average values for all entries
	update L 
	set [AverageLatitudeCache] = round(L.[AverageLatitudeCache], @Precision ),
	[AverageLongitudeCache] = round([AverageLongitudeCache], @Precision),
	[Geography] = NULL
	from [#project#].[CacheCollectionEventLocalisation] L

	-- rounding location values for coordinate entries
	update E 
	set [Location2] = round(E.[AverageLatitudeCache], @Precision ),
	[Location1] = round(E.[AverageLongitudeCache], @Precision)
	from [#project#].[CacheCollectionEventLocalisation] E,
	[#project#].[CacheLocalisationSystem] L
	WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
	AND L.ParsingMethodName = 'Coordinates'

	-- rounding location values for Gauss Krueger entries
	declare @GK int
	if @Precision < 5
	begin
		set @GK = 5 - @Precision
		update E 
		set [Location2] = substring([Location2], 1, 7 - @GK) + replicate('0', @GK)  ,
		[Location1] = substring([Location1], 1, 7 - @GK) + replicate('0', @GK)  
		from [#project#].[CacheCollectionEventLocalisation] E,
		[#project#].[CacheLocalisationSystem] L
		WHERE L.[LocalisationSystemID] = E.[LocalisationSystemID]
		AND L.ParsingMethodName = 'GK'
	end
end

GO

--#####################################################################################################################
--######   CollectorsAgentURI in CacheCollectionAgent  #############################################################
--#####################################################################################################################
if (
select count(*) from INFORMATION_SCHEMA.COLUMNS C 
where C.Column_Name = 'CollectorsAgentURI'
and C.TABLE_NAME = 'CacheCollectionAgent'
and C.TABLE_SCHEMA = '#project#')  = 0
begin
ALTER TABLE [#project#].CacheCollectionAgent ADD [CollectorsAgentURI] nvarchar(500)
end
GO


--#####################################################################################################################
--######   procPublishCollectionAgent  ################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [#project#].[procPublishCollectionAgent] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionAgent]
*/
truncate table [#project#].CacheCollectionAgent

declare @AnonymCollector TABLE (
	[ID] int Identity NOT NULL,
	[CollectorsName] [nvarchar](400) NOT NULL  Primary key,
	[Anonymisation] [nvarchar](50) NOT NULL) 

INSERT INTO @AnonymCollector(CollectorsName, Anonymisation)
SELECT CollectorsName, Anonymisation
FROM ' +  dbo.SourceDatabase() + '.dbo.AnonymCollector

INSERT INTO [#project#].[CacheCollectionAgent]
           ([CollectionSpecimenID]
           ,[CollectorsName]
           ,[CollectorsSequence]
           ,[CollectorsNumber]
		   ,[CollectorsAgentURI])
SELECT DISTINCT C.CollectionSpecimenID, CASE WHEN A.Anonymisation IS NULL THEN C.CollectorsName COLLATE DATABASE_DEFAULT ELSE A.Anonymisation COLLATE DATABASE_DEFAULT + '' '' + CAST (A.ID AS varchar) END AS CollectorsName, C.CollectorsSequence, 
CASE WHEN A.Anonymisation IS NULL THEN C.CollectorsNumber ELSE NULL END AS CollectorsNumber, CollectorsAgentURI
FROM [#project#].CacheCollectionSpecimen AS S INNER JOIN [' +  dbo.SourceDatabase() + '].dbo.CollectionAgent AS C ON S.CollectionSpecimenID = C.CollectionSpecimenID 
LEFT OUTER JOIN @AnonymCollector AS A ON C.CollectorsName COLLATE DATABASE_DEFAULT = A.CollectorsName COLLATE DATABASE_DEFAULT
WHERE (C.DataWithholdingReason = N'''') OR (C.DataWithholdingReason IS NULL)
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   CacheAnnotation  ###########################################################################################
--#####################################################################################################################

if (
select count(*) from INFORMATION_SCHEMA.Tables t 
where t.TABLE_NAME = 'CacheAnnotation'
and t.TABLE_SCHEMA = '#project#')  = 0
begin
CREATE TABLE [#project#].[CacheAnnotation](
	[AnnotationID] [int] NOT NULL,
	[ReferencedAnnotationID] [int] NULL,
	[AnnotationType] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[Annotation] [nvarchar](max) NOT NULL,
	[URI] [varchar](255) NULL,
	[ReferenceDisplayText] [nvarchar](500) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[SourceDisplayText] [nvarchar](500) NULL,
	[SourceURI] [varchar](255) NULL,
	[ReferencedID] [int] NOT NULL,
	[ReferencedTable] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Annotation] PRIMARY KEY CLUSTERED 
(
	[AnnotationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end
GO


--#####################################################################################################################
--######   procPublishAnnotation  #####################################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishAnnotation] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishAnnotation]
*/
truncate table [#project#].CacheAnnotation

-- Event
INSERT INTO [#project#].[CacheAnnotation]
(AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, 
ReferenceDisplayText, ReferenceURI, 
SourceDisplayText, SourceURI,  
ReferencedID, ReferencedTable)
SELECT        
AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, 
ReferenceDisplayText, A.ReferenceURI, 
SourceDisplayText, SourceURI,  
ReferencedID, ReferencedTable
FROM  ' +  dbo.SourceDatabase() + '.dbo.Annotation A, 
[#project#].CacheCollectionEvent AS E
WHERE (A.IsInternal IS NULL OR A.IsInternal = 0)
and A.ReferencedTable = ''CollectionEvent'' and A.ReferencedID = E.CollectionEventID
')

set @SQL = (@SQL + '
-- Specimen
INSERT INTO [#project#].[CacheAnnotation]
(AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, 
ReferenceDisplayText, ReferenceURI, 
SourceDisplayText, SourceURI,  
ReferencedID, ReferencedTable)
SELECT        
AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, 
ReferenceDisplayText, A.ReferenceURI, 
SourceDisplayText, SourceURI,  
ReferencedID, ReferencedTable
FROM  ' +  dbo.SourceDatabase() + '.dbo.Annotation A, 
[#project#].CacheCollectionSpecimen AS S
WHERE (A.IsInternal IS NULL OR A.IsInternal = 0)
and A.ReferencedTable = ''CollectionSpecimen'' and A.ReferencedID = S.CollectionSpecimenID
')

set @SQL = (@SQL + '
-- Part
INSERT INTO [#project#].[CacheAnnotation]
(AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, 
ReferenceDisplayText, ReferenceURI, 
SourceDisplayText, SourceURI,  
ReferencedID, ReferencedTable)
SELECT        
AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, 
ReferenceDisplayText, A.ReferenceURI, 
SourceDisplayText, SourceURI,  
ReferencedID, ReferencedTable
FROM  ' +  dbo.SourceDatabase() + '.dbo.Annotation A, 
[#project#].CacheCollectionSpecimenPart AS P
WHERE (A.IsInternal IS NULL OR A.IsInternal = 0)
and A.ReferencedTable = ''CollectionSpecimenPart'' and A.ReferencedID = P.SpecimenPartID
')

set @SQL = (@SQL + '
-- Unit
INSERT INTO [#project#].[CacheAnnotation]
(AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, 
ReferenceDisplayText, ReferenceURI, 
SourceDisplayText, SourceURI,  
ReferencedID, ReferencedTable)
SELECT        
AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, 
ReferenceDisplayText, A.ReferenceURI, 
SourceDisplayText, SourceURI,  
ReferencedID, ReferencedTable
FROM  ' +  dbo.SourceDatabase() + '.dbo.Annotation A, 
[#project#].CacheIdentificationUnit AS U
WHERE (A.IsInternal IS NULL OR A.IsInternal = 0)
and A.ReferencedTable = ''IdentificationUnit'' and A.ReferencedID = U.IdentificationUnitID
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   CacheCollection  ###########################################################################################
--#####################################################################################################################
if (
select count(*) from INFORMATION_SCHEMA.Tables t 
where t.TABLE_NAME = 'CacheCollection'
and t.TABLE_SCHEMA = '#project#')  = 0
begin
CREATE TABLE [#project#].[CacheCollection](
	[CollectionID] [int] NOT NULL,
	[CollectionParentID] [int] NULL,
	[CollectionName] [nvarchar](255) NOT NULL,
	[CollectionAcronym] [nvarchar](10) NULL,
	[AdministrativeContactName] [nvarchar](500) NULL,
	[AdministrativeContactAgentURI] [varchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[Location] [nvarchar](255) NULL,
	[CollectionOwner] [nvarchar](255) NULL,
	[DisplayOrder] [smallint] NULL,
 CONSTRAINT [PK_Collection] PRIMARY KEY CLUSTERED 
(
	[CollectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end
GO


--#####################################################################################################################
--######   procPublishCollection  #####################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollection] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollection]
*/
truncate table [#project#].CacheCollection

-- Event
INSERT INTO [#project#].[CacheCollection]
(CollectionID, CollectionParentID, CollectionName, CollectionAcronym, 
AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, 
CollectionOwner, DisplayOrder)
SELECT    DISTINCT    
C.CollectionID, CollectionParentID, CollectionName, CollectionAcronym, 
AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, 
CollectionOwner, DisplayOrder
FROM  ' +  dbo.SourceDatabase() + '.dbo.Collection C, 
[#project#].CacheCollectionSpecimenPart AS P
WHERE P.CollectionID = C.CollectionID
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO



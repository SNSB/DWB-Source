declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.99'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######  StableIdentifier    #########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[StableIdentifier] (@ProjectID int, @CollectionSpecimenID int, @IdentificationUnitID int, @SpecimenPartID int)
RETURNS varchar (500)
/*Returns a stable identfier for a dataset.*/
AS
BEGIN
declare @StableIdentifierBase varchar(500)
set @StableIdentifierBase = (SELECT case when [StableIdentifierBase] is null or [StableIdentifierBase] = ''
	then dbo.BaseURL()
	else [StableIdentifierBase]
	end
  FROM [dbo].[ProjectProxy]
p where p.ProjectID = @ProjectID)
if (@StableIdentifierBase not like '%/')
begin
	set @StableIdentifierBase = @StableIdentifierBase + '/'
end
declare @StableIdentifier varchar(500)
set @StableIdentifier = @StableIdentifierBase + cast(@CollectionSpecimenID as varchar) 
if (@IdentificationUnitID IS NOT NULL)
begin
	set @StableIdentifier = @StableIdentifier + '/' + cast(@IdentificationUnitID as varchar) 
	if (@SpecimenPartID IS NOT NULL)
	begin
		set @StableIdentifier = @StableIdentifier + '/' + cast(@SpecimenPartID as varchar) 
	end
end
return @StableIdentifier
END

GO


--#####################################################################################################################
--######  CacheDatabase2  #############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CacheDatabase2') = 0
begin

CREATE TABLE [dbo].[CacheDatabase2](
	[Server] [varchar](50) NOT NULL,
	[DatabaseName] [varchar](50) NOT NULL,
	[Port] [smallint] NOT NULL,
	[Version] [varchar](50) NULL,
 CONSTRAINT [PK_CacheDatabase2] PRIMARY KEY CLUSTERED 
(
	[Server] ASC,
	[DatabaseName] ASC,
	[Port] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or IP of the server where the cache database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase2', @level2type=N'COLUMN',@level2name=N'Server'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the cache database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase2', @level2type=N'COLUMN',@level2name=N'DatabaseName'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The port of the server where the cache database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase2', @level2type=N'COLUMN',@level2name=N'Port'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the cache database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase2', @level2type=N'COLUMN',@level2name=N'Version'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Table holding the cache databases connected to the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CacheDatabase2'

GRANT SELECT ON CacheDatabase2 TO [CacheUser]

GRANT INSERT ON CacheDatabase2 TO [CacheAdmin]


--INSERT INTO [dbo].[CacheDatabase2]
--           ([Server]
--           ,[DatabaseName]
--           ,[Port]
--           ,[Version])
--SELECT TOP 1
--       [Server]
--      ,[DatabaseName]
--      ,[Port]
--      ,[Version]
--  FROM [dbo].[CacheDatabase]

end
GO

--#####################################################################################################################
--######  procCopyCollectionSpecimen2  ################################################################################
--#####################################################################################################################


ALTER  PROCEDURE [dbo].[procCopyCollectionSpecimen2] 
	(@CollectionSpecimenID int output ,
	@OriginalCollectionSpecimenID int ,
	@AccessionNumber  nvarchar(50),
	@EventCopyMode int,
	@IncludedTables nvarchar(4000))
AS
declare @count int
declare @EventID int

/*
Copy a collection specimen
@EventCopyMode
-1: dont copy the event, leave the entry in table CollectionSpecimen empty
0:  take same event as original specimen
1:  create new event with the same data as the old specimen

@IncludedTables contains list of tables that are copied according to the users choice
*/

if (@EventCopyMode = 0) set @EventID = (SELECT CollectionEventID FROM CollectionSpecimen WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID)
if (@EventCopyMode < 0) set @EventID = null

DECLARE @CollectionEventID int
DECLARE @OriginalCollectionEventID int

if (@EventCopyMode > 0) 
begin
	DECLARE @RC int
	set @OriginalCollectionEventID = (SELECT CollectionEventID FROM CollectionSpecimen WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID)

	-- CollectionEvent
	INSERT INTO CollectionEvent
		  (SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, 
		  CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, 
		  Notes, CountryCache, DataWithholdingReason)
	SELECT SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, 
		  CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, 
		  Notes, CountryCache, DataWithholdingReason
	FROM  CollectionEvent
	WHERE CollectionEventID = @OriginalCollectionEventID

	SET @CollectionEventID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

	-- CollectionEventLocalisation
	INSERT INTO CollectionEventLocalisation
		(CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, 
		DirectionToLocation, ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache)
	SELECT @CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, 
		DirectionToLocation, ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache
	FROM CollectionEventLocalisation
	WHERE (CollectionEventID = @OriginalCollectionEventID)

	-- CollectionEventProperty
	INSERT INTO CollectionEventProperty
		(CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue,  
		ResponsibleName, ResponsibleAgentURI, Notes,AverageValueCache)
	SELECT @CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue,  
		ResponsibleName, ResponsibleAgentURI, Notes,AverageValueCache
	FROM CollectionEventProperty
	WHERE (CollectionEventID = @OriginalCollectionEventID)

	-- CollectionEventImage
	IF (@IncludedTables LIKE '%|CollectionEventImage|%')
	BEGIN
		INSERT INTO CollectionEventImage
			(CollectionEventID, URI, ResourceURI, ImageType, Notes)
		SELECT @CollectionEventID, URI, ResourceURI, ImageType, Notes
		FROM CollectionEventImage
		WHERE (CollectionEventID = @OriginalCollectionEventID)
	END
	set @EventID = @CollectionEventID
end

-- CollectionSpecimen
INSERT INTO CollectionSpecimen (CollectionEventID, CollectionID, AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, 
	AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, 
	LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, InternalNotes, ReferenceTitle, ReferenceURI, 
	Problems, DataWithholdingReason)
SELECT @EventID, CollectionID, @AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, 
	AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, 
	LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, InternalNotes, ReferenceTitle, ReferenceURI, 
	Problems, DataWithholdingReason
FROM CollectionSpecimen
WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID

SET @CollectionSpecimenID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

-- CollectionProject
INSERT INTO CollectionProject (CollectionSpecimenID, ProjectID)
SELECT @CollectionSpecimenID, ProjectID
FROM CollectionProject
WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID

-- CollectionAgent
IF (@IncludedTables LIKE '%|CollectionAgent|%')
BEGIN
	INSERT INTO CollectionAgent ( CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason)
	SELECT   @CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason
	FROM CollectionAgent
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
END

-- IdentificationUnit
DECLARE @TempUnitTable TABLE (IdentificationUnitID int primary key,
	OriginalIdentificationUnitID int NULL)
DECLARE @IdentificationUnitID int
DECLARE @OriginalIdentificationUnitID int

DECLARE UnitCursor CURSOR FOR
SELECT IdentificationUnitID FROM IdentificationUnit WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
OPEN UnitCursor
FETCH NEXT FROM UnitCursor INTO @OriginalIdentificationUnitID
WHILE @@FETCH_STATUS = 0
BEGIN
	-- IdentificationUnit
	INSERT INTO IdentificationUnit (CollectionSpecimenID, LastIdentificationCache, FamilyCache, OrderCache, TaxonomicGroup, 
		OnlyObserved, RelationType, ColonisedSubstratePart, LifeStage, Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, 
		Circumstances, DisplayOrder, Notes)
	SELECT @CollectionSpecimenID, LastIdentificationCache, FamilyCache, OrderCache, TaxonomicGroup, 
		OnlyObserved, RelationType, ColonisedSubstratePart, LifeStage, Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, 
		Circumstances, DisplayOrder, Notes
	FROM IdentificationUnit
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND IdentificationUnitID = @OriginalIdentificationUnitID

	SET @IdentificationUnitID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])
	INSERT INTO @TempUnitTable (IdentificationUnitID, OriginalIdentificationUnitID) VALUES (@IdentificationUnitID, @OriginalIdentificationUnitID)

	-- Identification
	IF (@IncludedTables LIKE '%|Identification|%')
	BEGIN
		INSERT INTO Identification (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermURI, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, Notes, ResponsibleName, ResponsibleAgentURI)
		SELECT @CollectionSpecimenID, @IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, Notes, ResponsibleName, ResponsibleAgentURI
		FROM Identification
		WHERE Identification.CollectionSpecimenID = @OriginalCollectionSpecimenID
		and Identification.IdentificationUnitID = @OriginalIdentificationUnitID
	END

   	FETCH NEXT FROM UnitCursor INTO @OriginalIdentificationUnitID
END
CLOSE UnitCursor
DEALLOCATE UnitCursor

-- Fixing the relations between the units
DECLARE @I INT
SET @I = (SELECT COUNT(*) FROM @TempUnitTable)
IF @I > 0
BEGIN
	UPDATE N SET N.RelatedUnitID = NH.IdentificationUnitID
	FROM IdentificationUnit N, IdentificationUnit NH, @TempUnitTable T, @TempUnitTable TH, IdentificationUnit O--, IdentificationUnit OH
	WHERE N.IdentificationUnitID = T.IdentificationUnitID
	AND O.IdentificationUnitID = T.OriginalIdentificationUnitID
	AND O.RelatedUnitID = TH.OriginalIdentificationUnitID
	AND TH.IdentificationUnitID = NH.IdentificationUnitID

	UPDATE N SET N.ParentUnitID = NH.IdentificationUnitID
	FROM IdentificationUnit N, IdentificationUnit NH, @TempUnitTable T, @TempUnitTable TH, IdentificationUnit O--, IdentificationUnit OH
	WHERE N.IdentificationUnitID = T.IdentificationUnitID
	AND O.IdentificationUnitID = T.OriginalIdentificationUnitID
	AND O.ParentUnitID = TH.OriginalIdentificationUnitID
	AND TH.IdentificationUnitID = NH.IdentificationUnitID
END

-- CollectionSpecimenPart
DECLARE @TempPartTable TABLE (SpecimenPartID int primary key,
	OriginalSpecimenPartID int NULL)
DECLARE @SpecimenPartID int
DECLARE @OriginalSpecimenPartID int

DECLARE PartCursor CURSOR FOR
SELECT SpecimenPartID FROM CollectionSpecimenPart WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
OPEN PartCursor
FETCH NEXT FROM PartCursor INTO @OriginalSpecimenPartID
WHILE @@FETCH_STATUS = 0
BEGIN
	-- CollectionSpecimenPart
	INSERT INTO CollectionSpecimenPart ( CollectionSpecimenID, PreparationMethod, PreparationDate, 
		AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, Stock, Notes)
	SELECT @CollectionSpecimenID, PreparationMethod, PreparationDate, 
		AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, Stock, Notes
	FROM CollectionSpecimenPart
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND SpecimenPartID = @OriginalSpecimenPartID

	SET @SpecimenPartID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])
	INSERT INTO @TempPartTable (SpecimenPartID, OriginalSpecimenPartID) VALUES (@SpecimenPartID, @OriginalSpecimenPartID)

	FETCH NEXT FROM PartCursor INTO @OriginalSpecimenPartID
END
CLOSE PartCursor
DEALLOCATE PartCursor

-- Fixing the relations between the parts
DECLARE @P INT
SET @P = (SELECT COUNT(*) FROM @TempUnitTable)
IF @P > 0
BEGIN
	UPDATE N SET N.DerivedFromSpecimenPartID = NH.SpecimenPartID
	FROM CollectionSpecimenPart N, CollectionSpecimenPart NH, @TempPartTable T, @TempPartTable TH, CollectionSpecimenPart O--, IdentificationUnit OH
	WHERE N.SpecimenPartID = T.SpecimenPartID
	AND O.SpecimenPartID = T.OriginalSpecimenPartID
	AND O.DerivedFromSpecimenPartID = TH.OriginalSpecimenPartID
	AND TH.SpecimenPartID = NH.SpecimenPartID
END

--IdentificationUnitInPart
--IF (@IncludedTables LIKE '%|IdentificationUnit|%' AND @IncludedTables LIKE '%|CollectionSpecimenPart|%')
BEGIN
	INSERT INTO IdentificationUnitInPart (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, DisplayOrder)
	SELECT @CollectionSpecimenID, U.IdentificationUnitID, P.SpecimenPartID, I.DisplayOrder
	FROM IdentificationUnitInPart I, @TempPartTable P, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID
END


-- CollectionRelation
IF (@IncludedTables LIKE '%|CollectionSpecimenRelation|%')
BEGIN
	INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache
	FROM CollectionSpecimenRelation
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND [IdentificationUnitID] IS NULL
	AND [SpecimenPartID] IS NULL

	INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, U.IdentificationUnitID
	FROM CollectionSpecimenRelation R, @TempUnitTable U
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND [SpecimenPartID] IS NULL

	INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, SpecimenPartID)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, P.SpecimenPartID
	FROM CollectionSpecimenRelation R, @TempPartTable P
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND [IdentificationUnitID] IS NULL
	AND R.[SpecimenPartID] = P.OriginalSpecimenPartID

	INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID, SpecimenPartID)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, U.IdentificationUnitID, P.SpecimenPartID
	FROM CollectionSpecimenRelation R, @TempPartTable P, @TempUnitTable U
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND R.[SpecimenPartID] = P.OriginalSpecimenPartID

END


--IdentificationUnitAnalysis
--IdentificationUnitAnalysisMethod
--IdentificationUnitAnalysisMethodParameter
IF (@IncludedTables LIKE '%|IdentificationUnitAnalysis|%')
BEGIN
	-- for parts
	INSERT INTO [dbo].[IdentificationUnitAnalysis]
           ([CollectionSpecimenID]
           ,[IdentificationUnitID]
           ,[AnalysisID]
           ,[AnalysisNumber]
           ,[AnalysisResult]
           ,[ExternalAnalysisURI]
           ,[ResponsibleName]
           ,[ResponsibleAgentURI]
           ,[AnalysisDate]
           ,[SpecimenPartID]
           ,[Notes])
	SELECT DISTINCT @CollectionSpecimenID, U.IdentificationUnitID
	, I.AnalysisID, I.AnalysisNumber, I.AnalysisResult, I.ExternalAnalysisURI, I.ResponsibleName, I.ResponsibleAgentURI, I.AnalysisDate
	, P.SpecimenPartID, I.Notes
	FROM IdentificationUnitAnalysis I, @TempPartTable P, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	-- for non parts
	INSERT INTO [dbo].[IdentificationUnitAnalysis]
           ([CollectionSpecimenID]
           ,[IdentificationUnitID]
           ,[AnalysisID]
           ,[AnalysisNumber]
           ,[AnalysisResult]
           ,[ExternalAnalysisURI]
           ,[ResponsibleName]
           ,[ResponsibleAgentURI]
           ,[AnalysisDate]
           ,[SpecimenPartID]
           ,[Notes])
	SELECT DISTINCT @CollectionSpecimenID, U.IdentificationUnitID
	, I.AnalysisID, I.AnalysisNumber, I.AnalysisResult, I.ExternalAnalysisURI, I.ResponsibleName, I.ResponsibleAgentURI, I.AnalysisDate
	, I.SpecimenPartID, I.Notes
	FROM IdentificationUnitAnalysis I, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID IS NULL
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	IF (@IncludedTables LIKE '%|IdentificationUnitAnalysisMethod|%')
	BEGIN
		INSERT INTO [dbo].[IdentificationUnitAnalysisMethod]
			   ([CollectionSpecimenID]
			   ,[IdentificationUnitID]
			   ,[MethodID]
			   ,[AnalysisID]
			   ,[AnalysisNumber]
			   ,[MethodMarker])
		SELECT @CollectionSpecimenID, U.IdentificationUnitID, I.MethodID
		, I.AnalysisID, I.AnalysisNumber, I.MethodMarker
		FROM IdentificationUnitAnalysisMethod I, @TempUnitTable U
		WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
		AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

		INSERT INTO [dbo].[IdentificationUnitAnalysisMethodParameter]
			   ([CollectionSpecimenID]
			   ,[IdentificationUnitID]
			   ,[AnalysisID]
			   ,[AnalysisNumber]
			   ,[MethodID]
			   ,[ParameterID]
			   ,[Value]
			   ,[MethodMarker])
		SELECT @CollectionSpecimenID, U.IdentificationUnitID, I.AnalysisID, I.AnalysisNumber, I.MethodID
		, I.ParameterID, I.Value, I.MethodMarker
		FROM IdentificationUnitAnalysisMethodParameter I, @TempUnitTable U
		WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
		AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID
	END
END


--CollectionSpecimenProcessing
--CollectionSpecimenProcessingMethod
--CollectionSpecimenProcessingMethodParameter
IF (@IncludedTables LIKE '%|CollectionSpecimenProcessing|%')
BEGIN
	-- CollectionSpecimenProcessing
	INSERT INTO CollectionSpecimenProcessing
		(CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration,  
		ResponsibleName, ResponsibleAgentURI, Notes)
	SELECT @CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, T.SpecimenPartID, ProcessingDuration,  
		ResponsibleName, ResponsibleAgentURI, Notes
	FROM CollectionSpecimenProcessing P, @TempPartTable T
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND T.OriginalSpecimenPartID = @OriginalSpecimenPartID

	IF (@IncludedTables LIKE '%|CollectionSpecimenProcessingMethod|%')
	BEGIN
		-- CollectionSpecimenProcessingMethod
		INSERT INTO CollectionSpecimenProcessingMethod
			(CollectionSpecimenID, SpecimenProcessingID, MethodID, MethodMarker, ProcessingID)
		SELECT @CollectionSpecimenID, P.SpecimenProcessingID, M.MethodID, M.MethodMarker, M.ProcessingID
		FROM CollectionSpecimenProcessingMethod M, CollectionSpecimenProcessing P, @TempPartTable T
		WHERE M.CollectionSpecimenID = @OriginalCollectionSpecimenID
		AND P.CollectionSpecimenID = @CollectionSpecimenID
		AND P.SpecimenPartID = T.SpecimenPartID

		-- CollectionSpecimenProcessingMethodParameter
		INSERT INTO CollectionSpecimenProcessingMethodParameter
			(CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, MethodMarker, ParameterID, Value)
		SELECT @CollectionSpecimenID, P.SpecimenProcessingID, P.ProcessingID, M.MethodID, M.MethodMarker, M.ParameterID, M.Value
		FROM CollectionSpecimenProcessingMethodParameter M, CollectionSpecimenProcessing P, @TempPartTable T
		WHERE M.CollectionSpecimenID = @OriginalCollectionSpecimenID
		AND P.CollectionSpecimenID = @CollectionSpecimenID
		AND P.SpecimenPartID = T.SpecimenPartID
	END
END


--CollectionSpecimenImage
IF (@IncludedTables LIKE '%|CollectionSpecimenImage|%')
BEGIN
	INSERT INTO CollectionSpecimenImage
		(CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI)
	SELECT @CollectionSpecimenID, URI, ResourceURI, P.SpecimenPartID, U.IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI
	FROM CollectionSpecimenImage I, @TempPartTable P, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenImage
		(CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI)
	SELECT @CollectionSpecimenID, URI, ResourceURI, P.SpecimenPartID, I.IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI
	FROM CollectionSpecimenImage I, @TempPartTable P
	WHERE I.IdentificationUnitID IS NULL
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenImage
		(CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI)
	SELECT @CollectionSpecimenID, URI, ResourceURI, I.SpecimenPartID, U.IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI
	FROM CollectionSpecimenImage I, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID IS NULL
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenImage
		(CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI)
	SELECT @CollectionSpecimenID, URI, ResourceURI, I.SpecimenPartID, I.IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, 
		LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, LicenseNotes, LicenseURI
	FROM CollectionSpecimenImage I
	WHERE I.IdentificationUnitID IS NULL
	AND I.SpecimenPartID IS NULL
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

END


--CollectionSpecimenImageProperty
IF (@IncludedTables LIKE '%|CollectionSpecimenImageProperty|%')
BEGIN
	INSERT INTO CollectionSpecimenImageProperty
		   (CollectionSpecimenID, URI, Property, Description, ImageArea)
	SELECT @CollectionSpecimenID, URI, Property, Description, ImageArea
	FROM CollectionSpecimenImageProperty I
	WHERE I.CollectionSpecimenID = @OriginalCollectionSpecimenID
END


--CollectionSpecimenReference
IF (@IncludedTables LIKE '%|CollectionSpecimenReference|%')
BEGIN
	INSERT INTO CollectionSpecimenReference
		(CollectionSpecimenID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, U.IdentificationUnitID, P.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R, @TempPartTable P, @TempUnitTable U
	WHERE R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND R.SpecimenPartID = P.OriginalSpecimenPartID
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		(CollectionSpecimenID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, R.IdentificationUnitID, P.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R, @TempPartTable P
	WHERE R.IdentificationUnitID IS NULL
	AND R.SpecimenPartID = P.OriginalSpecimenPartID
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		(CollectionSpecimenID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, U.IdentificationUnitID, R.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R, @TempUnitTable U
	WHERE R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND R.SpecimenPartID IS NULL
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		(CollectionSpecimenID, ReferenceTitle, ReferenceURI, IdentificationUnitID, SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, R.IdentificationUnitID, R.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R
	WHERE R.IdentificationUnitID IS NULL
	AND R.SpecimenPartID IS NULL
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

END

--CollectionSpecimenTransaction
IF (@IncludedTables LIKE '%|CollectionSpecimenTransaction|%')
BEGIN
	INSERT INTO CollectionSpecimenTransaction
		(CollectionSpecimenID, TransactionID, SpecimenPartID, AccessionNumber, TransactionReturnID)
	SELECT @CollectionSpecimenID, TransactionID, P.SpecimenPartID, AccessionNumber, TransactionReturnID
	FROM CollectionSpecimenTransaction T, @TempPartTable P
	WHERE T.SpecimenPartID = P.OriginalSpecimenPartID
	AND T.CollectionSpecimenID = @OriginalCollectionSpecimenID
END

--Annotation
IF (@IncludedTables LIKE '%|Annotation|%')
BEGIN
	INSERT INTO [dbo].[Annotation]
           ([ReferencedAnnotationID]
           ,[AnnotationType]
           ,[Title]
           ,[Annotation]
           ,[URI]
           ,[ReferenceDisplayText]
           ,[ReferenceURI]
           ,[SourceDisplayText]
           ,[SourceURI]
           ,[IsInternal]
           ,[ReferencedID]
           ,[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
	, A.SourceDisplayText, A.SourceURI, A.IsInternal, @CollectionSpecimenID, A.ReferencedTable
	FROM Annotation A
	WHERE A.ReferencedTable = 'CollectionSpecimen' AND A.ReferencedID = @OriginalCollectionSpecimenID

	INSERT INTO [dbo].[Annotation]
           ([ReferencedAnnotationID]
           ,[AnnotationType]
           ,[Title]
           ,[Annotation]
           ,[URI]
           ,[ReferenceDisplayText]
           ,[ReferenceURI]
           ,[SourceDisplayText]
           ,[SourceURI]
           ,[IsInternal]
           ,[ReferencedID]
           ,[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
	, A.SourceDisplayText, A.SourceURI, A.IsInternal, U.IdentificationUnitID, A.ReferencedTable
	FROM Annotation A, @TempUnitTable U
	WHERE A.ReferencedTable = 'IdentificationUnit' AND A.ReferencedID = U.OriginalIdentificationUnitID

	INSERT INTO [dbo].[Annotation]
           ([ReferencedAnnotationID]
           ,[AnnotationType]
           ,[Title]
           ,[Annotation]
           ,[URI]
           ,[ReferenceDisplayText]
           ,[ReferenceURI]
           ,[SourceDisplayText]
           ,[SourceURI]
           ,[IsInternal]
           ,[ReferencedID]
           ,[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
	, A.SourceDisplayText, A.SourceURI, A.IsInternal, P.SpecimenPartID, A.ReferencedTable
	FROM Annotation A, @TempPartTable P
	WHERE A.ReferencedTable = 'CollectionSpecimenPart' AND A.ReferencedID = P.OriginalSpecimenPartID

	if @EventCopyMode = 1 AND NOT @CollectionEventID IS NULL
	BEGIN
		INSERT INTO [dbo].[Annotation]
			   ([ReferencedAnnotationID]
			   ,[AnnotationType]
			   ,[Title]
			   ,[Annotation]
			   ,[URI]
			   ,[ReferenceDisplayText]
			   ,[ReferenceURI]
			   ,[SourceDisplayText]
			   ,[SourceURI]
			   ,[IsInternal]
			   ,[ReferencedID]
			   ,[ReferencedTable])
		SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
		, A.SourceDisplayText, A.SourceURI, A.IsInternal, @CollectionEventID, A.ReferencedTable
		FROM Annotation A
		WHERE A.ReferencedTable = 'CollectionEvent' AND A.ReferencedID = @OriginalCollectionEventID
	END

END

--ExternalIdentifier
IF (@IncludedTables LIKE '%|ExternalIdentifier|%')
BEGIN
	INSERT INTO [dbo].[ExternalIdentifier]
           ([ReferencedTable]
           ,[ReferencedID]
           ,[Type]
           ,[Identifier]
           ,[URL]
           ,[Notes])
	SELECT T.ReferencedTable, @CollectionSpecimenID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T
	WHERE T.ReferencedTable = 'CollectionSpecimen' AND T.ReferencedID = @OriginalCollectionSpecimenID

	INSERT INTO [dbo].[ExternalIdentifier]
           ([ReferencedTable]
           ,[ReferencedID]
           ,[Type]
           ,[Identifier]
           ,[URL]
           ,[Notes])
	SELECT T.ReferencedTable, U.IdentificationUnitID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T, @TempUnitTable U
	WHERE T.ReferencedTable = 'IdentificationUnit' AND T.ReferencedID = U.OriginalIdentificationUnitID

	INSERT INTO [dbo].[ExternalIdentifier]
           ([ReferencedTable]
           ,[ReferencedID]
           ,[Type]
           ,[Identifier]
           ,[URL]
           ,[Notes])
	SELECT T.ReferencedTable, P.SpecimenPartID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T, @TempPartTable P
	WHERE T.ReferencedTable = 'CollectionSpecimenPart' AND T.ReferencedID = P.OriginalSpecimenPartID

	if @EventCopyMode = 1 AND NOT @CollectionEventID IS NULL
	BEGIN
		INSERT INTO [dbo].[ExternalIdentifier]
			   ([ReferencedTable]
			   ,[ReferencedID]
			   ,[Type]
			   ,[Identifier]
			   ,[URL]
			   ,[Notes])
		SELECT T.ReferencedTable, @CollectionEventID, T.Type, T.Identifier, T.URL, T.Notes
		FROM ExternalIdentifier T
		WHERE T.ReferencedTable = 'CollectionEvent' AND T.ReferencedID = @OriginalCollectionEventID
	END

END

SELECT @CollectionSpecimenID

GO


--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.08' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.00'
END

GO


declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.22'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Table CollectionSpecimenPartDescription: new column DescriptionHierarchyCache incl. log and trigger #########
--#####################################################################################################################

ALTER TABLE CollectionSpecimenPartDescription ADD DescriptionHierarchyCache nvarchar(MAX) NULL;
ALTER TABLE CollectionSpecimenPartDescription_log ADD DescriptionHierarchyCache nvarchar(MAX) NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hierarchy of the description. For values linked to a module, a cached value provided by the module' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartDescription', @level2type=N'COLUMN',@level2name=N'DescriptionHierarchyCache'
GO

--#####################################################################################################################
--######  trgDelCollectionSpecimenPartDescription   ###################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgDelCollectionSpecimenPartDescription] ON [dbo].[CollectionSpecimenPartDescription] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 6/7/2017  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, DescriptionHierarchyCache, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID, IdentificationUnitID,  LogVersion,  LogState) 
SELECT d.CollectionSpecimenID, d.Description, d.DescriptionTermURI, d.DescriptionHierarchyCache, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.PartDescriptionID, d.RowGUID, d.SpecimenPartID, d.IdentificationUnitID,  @Version,  'D'
FROM DELETED D
end
else
begin
if (select count(*) FROM DELETED D, CollectionSpecimen S WHERE d.CollectionSpecimenID = S.CollectionSpecimenID) > 0 
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, DescriptionHierarchyCache, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID, IdentificationUnitID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.Description, d.DescriptionTermURI, d.DescriptionHierarchyCache, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.PartDescriptionID, d.RowGUID, d.SpecimenPartID, d.IdentificationUnitID, CollectionSpecimen.Version, 'D' 
FROM DELETED D, CollectionSpecimen
WHERE d.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
else
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, Description, DescriptionTermURI, DescriptionHierarchyCache, LogCreatedBy, LogCreatedWhen, LogUpdatedBy, LogUpdatedWhen, Notes, PartDescriptionID, RowGUID, SpecimenPartID, IdentificationUnitID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.Description, d.DescriptionTermURI, d.DescriptionHierarchyCache, d.LogCreatedBy, d.LogCreatedWhen, d.LogUpdatedBy, d.LogUpdatedWhen, d.Notes, d.PartDescriptionID, d.RowGUID, d.SpecimenPartID, d.IdentificationUnitID, -1, 'D' 
FROM DELETED D
end
end
GO


--#####################################################################################################################
--######   trgUpdCollectionSpecimenPartDescription    #################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenPartDescription] ON [dbo].[CollectionSpecimenPartDescription] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.7 */ 
/*  Date: 7/25/2016  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, DescriptionHierarchyCache, Notes, IdentificationUnitID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT d.CollectionSpecimenID, d.SpecimenPartID, d.PartDescriptionID, d.Description, d.DescriptionTermURI, d.DescriptionHierarchyCache, d.Notes, d.IdentificationUnitID, d.LogCreatedWhen, d.LogCreatedBy, d.LogUpdatedWhen, d.LogUpdatedBy, d.RowGUID,  @Version,  'U'
FROM DELETED D
end
else
begin
INSERT INTO CollectionSpecimenPartDescription_Log (CollectionSpecimenID, SpecimenPartID, PartDescriptionID, Description, DescriptionTermURI, DescriptionHierarchyCache, Notes, IdentificationUnitID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT d.CollectionSpecimenID, d.SpecimenPartID, d.PartDescriptionID, d.Description, d.DescriptionTermURI, d.DescriptionHierarchyCache, d.Notes, d.IdentificationUnitID, d.LogCreatedWhen, d.LogCreatedBy, d.LogUpdatedWhen, d.LogUpdatedBy, d.RowGUID, CollectionSpecimen.Version, 'U' 
FROM DELETED D, CollectionSpecimen
WHERE d.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenPartDescription
set LogUpdatedWhen = getdate(), LogUpdatedBy = cast([dbo].[UserID]() as varchar)
FROM CollectionSpecimenPartDescription, deleted D
where 1 = 1 
AND CollectionSpecimenPartDescription.CollectionSpecimenID = d.CollectionSpecimenID
AND CollectionSpecimenPartDescription.PartDescriptionID = d.PartDescriptionID
AND CollectionSpecimenPartDescription.SpecimenPartID = d.SpecimenPartID
GO


--#####################################################################################################################
--######  CollectionSpecimenPartDescription: Transfer Notes to DescriptionHierarchyCache  #############################
--#####################################################################################################################

UPDATE D
   SET [DescriptionHierarchyCache] = [Notes]
      ,[Notes] = ''
	FROM  [dbo].[CollectionSpecimenPartDescription] D
 WHERE D.DescriptionTermURI <> '' AND D.[Notes] <> ''
GO


--#####################################################################################################################
--######  procCopyCollectionSpecimen2: Inclusion of dependent identification ##########################################
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
		  (SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionEndDay, CollectionEndMonth, CollectionEndYear, 
		  CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, 
		  Notes, CountryCache, DataWithholdingReason, DataWithholdingReasonDate)
	SELECT SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionEndDay, CollectionEndMonth, CollectionEndYear, 
		  CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, 
		  Notes, CountryCache, DataWithholdingReason, DataWithholdingReasonDate
	FROM  CollectionEvent
	WHERE CollectionEventID = @OriginalCollectionEventID

	SET @CollectionEventID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

	-- CollectionEventLocalisation
	INSERT INTO CollectionEventLocalisation
		(CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, 
		DirectionToLocation, ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, [Geography])
	SELECT @CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, 
		DirectionToLocation, ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, [Geography]
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

		-- CollectionEventMethod
	IF (@IncludedTables LIKE '%|CollectionEventMethod|%')
	BEGIN
		INSERT INTO CollectionEventMethod 
					 (CollectionEventID, MethodID, MethodMarker)
		SELECT       @CollectionEventID, MethodID, MethodMarker
		FROM            CollectionEventMethod
		WHERE (CollectionEventID = @OriginalCollectionEventID)

		INSERT INTO CollectionEventParameterValue
                    (CollectionEventID, MethodID, MethodMarker, ParameterID, Value, Notes)
		SELECT      @CollectionEventID, MethodID, MethodMarker, ParameterID, Value, Notes
		FROM            CollectionEventParameterValue
		WHERE (CollectionEventID = @OriginalCollectionEventID)
	END

	set @EventID = @CollectionEventID

end

-- CollectionSpecimen
INSERT INTO CollectionSpecimen 
(CollectionEventID, CollectionID, AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, 
	AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, 
	LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, InternalNotes, 
	Problems, DataWithholdingReason)
SELECT @EventID, CollectionID, @AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, 
	AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, 
	LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, InternalNotes, 
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
	INSERT INTO IdentificationUnit 
	      (CollectionSpecimenID, LastIdentificationCache, FamilyCache, OrderCache, HierarchyCache, TaxonomicGroup, 
		OnlyObserved, RelationType, ColonisedSubstratePart, LifeStage, Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier,
		Circumstances, DisplayOrder, Notes)
	SELECT @CollectionSpecimenID, LastIdentificationCache, FamilyCache, OrderCache, HierarchyCache, TaxonomicGroup, 
		OnlyObserved, RelationType, ColonisedSubstratePart, LifeStage, Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier,
		Circumstances, DisplayOrder, Notes
	FROM IdentificationUnit
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND IdentificationUnitID = @OriginalIdentificationUnitID

	SET @IdentificationUnitID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])
	INSERT INTO @TempUnitTable (IdentificationUnitID, OriginalIdentificationUnitID) VALUES (@IdentificationUnitID, @OriginalIdentificationUnitID)

	-- Identification
	IF (@IncludedTables LIKE '%|Identification|%')
	BEGIN
		-- not dependent
		INSERT INTO Identification 
		     (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermURI, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, Notes, ResponsibleName, ResponsibleAgentURI)
		SELECT @CollectionSpecimenID, @IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, Notes, ResponsibleName, ResponsibleAgentURI
		FROM Identification
		WHERE Identification.CollectionSpecimenID = @OriginalCollectionSpecimenID
		and Identification.IdentificationUnitID = @OriginalIdentificationUnitID
		and Identification.DependsOnIdentificationSequence IS NULL
		-- dependent
		INSERT INTO Identification 
		     (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermURI, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, Notes, ResponsibleName, ResponsibleAgentURI, DependsOnIdentificationSequence)
		SELECT @CollectionSpecimenID, @IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, Notes, ResponsibleName, ResponsibleAgentURI, DependsOnIdentificationSequence
		FROM Identification
		WHERE Identification.CollectionSpecimenID = @OriginalCollectionSpecimenID
		and Identification.IdentificationUnitID = @OriginalIdentificationUnitID
		and NOT Identification.DependsOnIdentificationSequence IS NULL
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

DECLARE @FetchStatusPartCursor int
DECLARE @FetchStatusProcessingCursor int
DECLARE PartCursor CURSOR FOR
SELECT SpecimenPartID FROM CollectionSpecimenPart WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
OPEN PartCursor
FETCH NEXT FROM PartCursor INTO @OriginalSpecimenPartID
SET @FetchStatusPartCursor = @@FETCH_STATUS
WHILE @FetchStatusPartCursor = 0
BEGIN
	-- CollectionSpecimenPart
	INSERT INTO CollectionSpecimenPart 
	       (CollectionSpecimenID, PreparationMethod, PreparationDate, 
		AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, Stock, Notes)
	SELECT @CollectionSpecimenID, PreparationMethod, PreparationDate, 
		AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, Stock, Notes
	FROM CollectionSpecimenPart
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND SpecimenPartID = @OriginalSpecimenPartID

	SET @SpecimenPartID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])
	INSERT INTO @TempPartTable (SpecimenPartID, OriginalSpecimenPartID) VALUES (@SpecimenPartID, @OriginalSpecimenPartID)



	--CollectionSpecimenProcessing
	--CollectionSpecimenProcessingMethod
	--CollectionSpecimenProcessingMethodParameter
	IF (@IncludedTables LIKE '%|CollectionSpecimenProcessing|%')
	BEGIN
		DECLARE @OriginalSpecimenProcessingID int
		DECLARE @SpecimenProcessingID int
		DECLARE ProcessingCursor CURSOR FOR
		SELECT SpecimenProcessingID FROM CollectionSpecimenProcessing WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID AND SpecimenPartID = @OriginalSpecimenPartID
		OPEN ProcessingCursor
		FETCH NEXT FROM ProcessingCursor INTO @OriginalSpecimenProcessingID
		SET @FetchStatusProcessingCursor = @@FETCH_STATUS
		WHILE @FetchStatusProcessingCursor = 0
		BEGIN
			-- CollectionSpecimenProcessing
			INSERT INTO CollectionSpecimenProcessing
				   (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration,  
				ResponsibleName, ResponsibleAgentURI, Notes)
			SELECT @CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, @SpecimenPartID, ProcessingDuration,  
				ResponsibleName, ResponsibleAgentURI, Notes
			FROM CollectionSpecimenProcessing P
			WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
			AND P.SpecimenPartID = @OriginalSpecimenPartID
			AND P.SpecimenProcessingID = @OriginalSpecimenProcessingID

			SET @SpecimenProcessingID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

			IF (@IncludedTables LIKE '%|CollectionSpecimenProcessingMethod|%')
			BEGIN
				-- CollectionSpecimenProcessingMethod
				INSERT INTO CollectionSpecimenProcessingMethod
					   (CollectionSpecimenID, SpecimenProcessingID, MethodID, MethodMarker, ProcessingID)
				SELECT @CollectionSpecimenID, @SpecimenProcessingID, M.MethodID, M.MethodMarker, M.ProcessingID
				FROM CollectionSpecimenProcessingMethod M
				WHERE M.CollectionSpecimenID = @OriginalCollectionSpecimenID
				AND M.SpecimenProcessingID = @OriginalSpecimenProcessingID

				-- CollectionSpecimenProcessingMethodParameter
				INSERT INTO CollectionSpecimenProcessingMethodParameter
					  (CollectionSpecimenID, SpecimenProcessingID, ProcessingID, MethodID, MethodMarker, ParameterID, Value)
				SELECT @CollectionSpecimenID, @SpecimenProcessingID, M.ProcessingID, M.MethodID, M.MethodMarker, M.ParameterID, M.Value
				FROM CollectionSpecimenProcessingMethodParameter M
				WHERE M.CollectionSpecimenID = @OriginalCollectionSpecimenID
				AND M.SpecimenProcessingID = @OriginalSpecimenProcessingID
			END
			FETCH NEXT FROM ProcessingCursor INTO @OriginalSpecimenProcessingID
			SET @FetchStatusProcessingCursor = @@FETCH_STATUS
		END
		CLOSE ProcessingCursor
		DEALLOCATE ProcessingCursor
	END

	-- CollectionSpecimenPartDescription
	IF (@IncludedTables LIKE '%|CollectionSpecimenPartDescription|%')
	BEGIN
		INSERT INTO [dbo].[CollectionSpecimenPartDescription]
			  ([CollectionSpecimenID],[SpecimenPartID],[PartDescriptionID],[Description],[DescriptionTermURI],[Notes])
		SELECT @CollectionSpecimenID, @SpecimenPartID, [PartDescriptionID],[Description],[DescriptionTermURI],[Notes]
		  FROM [dbo].[CollectionSpecimenPartDescription]
	END

	FETCH NEXT FROM PartCursor INTO @OriginalSpecimenPartID
	SET @FetchStatusPartCursor = @@FETCH_STATUS
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
	INSERT INTO IdentificationUnitInPart 
	       (CollectionSpecimenID,   IdentificationUnitID,  SpecimenPartID,    DisplayOrder)
	SELECT @CollectionSpecimenID, U.IdentificationUnitID, P.SpecimenPartID, I.DisplayOrder
	FROM IdentificationUnitInPart I, @TempPartTable P, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID
END


-- CollectionRelation
IF (@IncludedTables LIKE '%|CollectionSpecimenRelation|%')
BEGIN
	INSERT INTO CollectionSpecimenRelation 
	       (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache
	FROM CollectionSpecimenRelation
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND [IdentificationUnitID] IS NULL
	AND [SpecimenPartID] IS NULL

	INSERT INTO CollectionSpecimenRelation 
	       (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, U.IdentificationUnitID
	FROM CollectionSpecimenRelation R, @TempUnitTable U
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND [SpecimenPartID] IS NULL

	INSERT INTO CollectionSpecimenRelation 
	       (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, SpecimenPartID)
	SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
		RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, P.SpecimenPartID
	FROM CollectionSpecimenRelation R, @TempPartTable P
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND [IdentificationUnitID] IS NULL
	AND R.[SpecimenPartID] = P.OriginalSpecimenPartID

	INSERT INTO CollectionSpecimenRelation 
	       (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
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
           ([CollectionSpecimenID],[IdentificationUnitID]
		   ,[AnalysisID],[AnalysisNumber],[AnalysisResult],[ExternalAnalysisURI],[ResponsibleName],[ResponsibleAgentURI],[AnalysisDate]
           ,[SpecimenPartID],[Notes])
	SELECT DISTINCT @CollectionSpecimenID, U.IdentificationUnitID
		, I.AnalysisID, I.AnalysisNumber, I.AnalysisResult, I.ExternalAnalysisURI, I.ResponsibleName, I.ResponsibleAgentURI, I.AnalysisDate
		, P.SpecimenPartID, I.Notes
	FROM IdentificationUnitAnalysis I, @TempPartTable P, @TempUnitTable U
	WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND I.SpecimenPartID = P.OriginalSpecimenPartID
	AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

	-- for non parts
	INSERT INTO [dbo].[IdentificationUnitAnalysis]
           ([CollectionSpecimenID],[IdentificationUnitID]
           ,[AnalysisID],[AnalysisNumber],[AnalysisResult],[ExternalAnalysisURI],[ResponsibleName],[ResponsibleAgentURI],[AnalysisDate]
           ,[SpecimenPartID],[Notes])
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
			   ([CollectionSpecimenID],[IdentificationUnitID],[MethodID]
			   ,[AnalysisID],[AnalysisNumber],[MethodMarker])
		SELECT @CollectionSpecimenID, U.IdentificationUnitID, I.MethodID
		, I.AnalysisID, I.AnalysisNumber, I.MethodMarker
		FROM IdentificationUnitAnalysisMethod I, @TempUnitTable U
		WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
		AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

		INSERT INTO [dbo].[IdentificationUnitAnalysisMethodParameter]
			   ([CollectionSpecimenID],[IdentificationUnitID],[AnalysisID],[AnalysisNumber],[MethodID]
			   ,[ParameterID],[Value],[MethodMarker])
		SELECT @CollectionSpecimenID, U.IdentificationUnitID, I.AnalysisID, I.AnalysisNumber, I.MethodID
		, I.ParameterID, I.Value, I.MethodMarker
		FROM IdentificationUnitAnalysisMethodParameter I, @TempUnitTable U
		WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
		AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID
	END
END

--IdentificationGoeUnitAnalysis
IF (@IncludedTables LIKE '%|IdentificationUnitGeoAnalysis|%')
BEGIN
	INSERT INTO [dbo].[IdentificationUnitGeoAnalysis]
		 ([CollectionSpecimenID],[IdentificationUnitID],[AnalysisDate],[Geography],[Geometry],[ResponsibleName],[ResponsibleAgentURI],[Notes])
	SELECT @CollectionSpecimenID,[IdentificationUnitID],[AnalysisDate],[Geography],[Geometry],[ResponsibleName],[ResponsibleAgentURI],[Notes]
	  FROM [dbo].[IdentificationUnitGeoAnalysis] I
	WHERE I.CollectionSpecimenID = @OriginalCollectionSpecimenID
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
		   (CollectionSpecimenID, ReferenceTitle,    ReferenceURI,   IdentificationUnitID,   SpecimenPartID,   ReferenceDetails,   Notes,   ResponsibleName, ResponsibleAgentURI, IdentificationSequence)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, U.IdentificationUnitID, P.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI, R.IdentificationSequence
	FROM CollectionSpecimenReference AS R, @TempPartTable P, @TempUnitTable U
	WHERE R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND R.SpecimenPartID = P.OriginalSpecimenPartID
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		   (CollectionSpecimenID,   ReferenceTitle,   ReferenceURI,   IdentificationUnitID,   SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, R.IdentificationUnitID, P.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R, @TempPartTable P
	WHERE R.IdentificationUnitID IS NULL
	AND R.SpecimenPartID = P.OriginalSpecimenPartID
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		   (CollectionSpecimenID,   ReferenceTitle,   ReferenceURI,   IdentificationUnitID,   SpecimenPartID,   ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
	SELECT @CollectionSpecimenID, R.ReferenceTitle, R.ReferenceURI, U.IdentificationUnitID, R.SpecimenPartID, R.ReferenceDetails, R.Notes, R.ResponsibleName, R.ResponsibleAgentURI
	FROM CollectionSpecimenReference AS R, @TempUnitTable U
	WHERE R.IdentificationUnitID = U.OriginalIdentificationUnitID
	AND R.SpecimenPartID IS NULL
	AND R.CollectionSpecimenID = @OriginalCollectionSpecimenID

	INSERT INTO CollectionSpecimenReference
		   (CollectionSpecimenID,   ReferenceTitle,   ReferenceURI,   IdentificationUnitID,   SpecimenPartID, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI)
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
           ([ReferencedAnnotationID],[AnnotationType],[Title],[Annotation],[URI],[ReferenceDisplayText],[ReferenceURI]
           ,[SourceDisplayText],[SourceURI],[IsInternal],[ReferencedID],[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
	, A.SourceDisplayText, A.SourceURI, A.IsInternal, @CollectionSpecimenID, A.ReferencedTable
	FROM Annotation A
	WHERE A.ReferencedTable = 'CollectionSpecimen' AND A.ReferencedID = @OriginalCollectionSpecimenID

	INSERT INTO [dbo].[Annotation]
           ([ReferencedAnnotationID],[AnnotationType],[Title],[Annotation] ,[URI] ,[ReferenceDisplayText]  ,[ReferenceURI]
           ,[SourceDisplayText]  ,[SourceURI] ,[IsInternal]   ,[ReferencedID]  ,[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
		 , A.SourceDisplayText, A.SourceURI, A.IsInternal, U.IdentificationUnitID, A.ReferencedTable
	FROM Annotation A, @TempUnitTable U
	WHERE A.ReferencedTable = 'IdentificationUnit' AND A.ReferencedID = U.OriginalIdentificationUnitID

	INSERT INTO [dbo].[Annotation]
           ([ReferencedAnnotationID] ,[AnnotationType] ,[Title]  ,[Annotation] ,[URI]  ,[ReferenceDisplayText] ,[ReferenceURI]
           ,[SourceDisplayText],[SourceURI] ,[IsInternal],[ReferencedID] ,[ReferencedTable])
	SELECT A.ReferencedAnnotationID, A.AnnotationType, A.Title, A.Annotation, A.URI, A.ReferenceDisplayText, A.ReferenceURI
		, A.SourceDisplayText, A.SourceURI, A.IsInternal, P.SpecimenPartID, A.ReferencedTable
	FROM Annotation A, @TempPartTable P
	WHERE A.ReferencedTable = 'CollectionSpecimenPart' AND A.ReferencedID = P.OriginalSpecimenPartID

	if @EventCopyMode = 1 AND NOT @CollectionEventID IS NULL
	BEGIN
		INSERT INTO [dbo].[Annotation]
			   ([ReferencedAnnotationID],[AnnotationType],[Title],[Annotation]  ,[URI]  ,[ReferenceDisplayText]  ,[ReferenceURI]
			   ,[SourceDisplayText]  ,[SourceURI] ,[IsInternal] ,[ReferencedID] ,[ReferencedTable])
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
           ([ReferencedTable] ,[ReferencedID]  ,[Type]   ,[Identifier]  ,[URL]  ,[Notes])
	SELECT T.ReferencedTable, @CollectionSpecimenID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T
	WHERE T.ReferencedTable = 'CollectionSpecimen' AND T.ReferencedID = @OriginalCollectionSpecimenID

	INSERT INTO [dbo].[ExternalIdentifier]
           ([ReferencedTable] ,[ReferencedID]    ,[Type]  ,[Identifier]  ,[URL] ,[Notes])
	SELECT T.ReferencedTable, U.IdentificationUnitID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T, @TempUnitTable U
	WHERE T.ReferencedTable = 'IdentificationUnit' AND T.ReferencedID = U.OriginalIdentificationUnitID

	INSERT INTO [dbo].[ExternalIdentifier]
           ([ReferencedTable]  ,[ReferencedID] ,[Type]  ,[Identifier]  ,[URL] ,[Notes])
	SELECT T.ReferencedTable, P.SpecimenPartID, T.Type, T.Identifier, T.URL, T.Notes
	FROM ExternalIdentifier T, @TempPartTable P
	WHERE T.ReferencedTable = 'CollectionSpecimenPart' AND T.ReferencedID = P.OriginalSpecimenPartID

	if @EventCopyMode = 1 AND NOT @CollectionEventID IS NULL
	BEGIN
		INSERT INTO [dbo].[ExternalIdentifier]
			   ([ReferencedTable] ,[ReferencedID] ,[Type]   ,[Identifier]  ,[URL]  ,[Notes])
		SELECT T.ReferencedTable, @CollectionEventID, T.Type, T.Identifier, T.URL, T.Notes
		FROM ExternalIdentifier T
		WHERE T.ReferencedTable = 'CollectionEvent' AND T.ReferencedID = @OriginalCollectionEventID
	END

END

SELECT @CollectionSpecimenID

GO

--#####################################################################################################################
--######  New ROLE AdminNonProject ####################################################################################
--#####################################################################################################################

CREATE ROLE [AdminNonProject]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permissions as Administrator without permission to change projects' , @level0type=N'USER',@level0name=N'AdminNonProject'
GO


EXEC sp_addrolemember N'Administrator', N'AdminNonProject';

DENY DELETE ON dbo.[ProjectUser] TO [AdminNonProject];
DENY INSERT ON dbo.[ProjectUser] TO [AdminNonProject];
DENY UPDATE ON dbo.[ProjectUser] TO [AdminNonProject];

DENY DELETE ON dbo.[ProjectProxy] TO [AdminNonProject];
DENY INSERT ON dbo.[ProjectProxy] TO [AdminNonProject];
DENY UPDATE ON dbo.[ProjectProxy] TO [AdminNonProject];

DENY DELETE ON dbo.[UserProxy] TO [AdminNonProject];
DENY INSERT ON dbo.[UserProxy] TO [AdminNonProject];
DENY UPDATE ON dbo.[UserProxy] TO [AdminNonProject];

GO

--#####################################################################################################################
--######  Generic SetUserProjects  ####################################################################################
--#####################################################################################################################

/****** Object:  StoredProcedure [dbo].[SetUserProjects]    Script Date: 04.02.2021 15:23:35 ******/
IF 1 = (SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE [SPECIFIC_NAME]='SetUserProjects')
DROP PROCEDURE [dbo].[SetUserProjects]
GO

-- =============================================
-- Author:		Anton Link
-- Create date: 20210205
-- Description:	Create database user and assign
--              training projects     
-- =============================================
CREATE PROCEDURE [dbo].[SetUserProjects] 
	-- Add the parameters for the stored procedure here
	@User VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @Result NVARCHAR(200);
	DECLARE @DatabaseName NVARCHAR(MAX);
	DECLARE @DatabaseRole VARCHAR(100);
	DECLARE @DatabaseUser VARCHAR(100);
	DECLARE @Workshop INT;
	DECLARE @Windows INT;
	SET @DatabaseName=(SELECT DB_NAME());
	SET @DatabaseRole=(SELECT TOP 1 [name] FROM [sys].[database_principals] WHERE [type]='R' AND [name] LIKE '%Administrator');
	SET @Workshop=(SELECT COUNT(*) FROM [UserProxy] WHERE [LoginName]='Workshop');
	SET @Windows=CHARINDEX('\', REVERSE(@User));
	SET @Result=''; 

	IF @Windows=0
	BEGIN
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''S'' AND [name]=''' + @User + ''' ');
		EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;

		IF @DatabaseUser IS NULL
		BEGIN
  			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''U'' AND [name] LIKE ''%\' + @User + ''' ');
			EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;
		END
	END
	ELSE
	BEGIN
		SET @User=REVERSE(SUBSTRING(REVERSE(@User), 1, @Windows));
  		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'SELECT @DatabaseUser = [name] FROM [sys].[database_principals] WHERE [type]=''U'' AND [name] LIKE ''%' + @User + ''' ');
		EXECUTE sp_executesql @SQL, N'@DatabaseUser varchar(100) OUTPUT', @DatabaseUser=@DatabaseUser OUTPUT;
	END
	
	IF @DatabaseUser IS NULL
	BEGIN
		SET @Result='User ' + @User + ' is not present in database ' + @DatabaseName;
	END
	ELSE
	BEGIN
		IF NOT @DatabaseRole IS NULL
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'ALTER ROLE [' + @DatabaseRole + '] ADD MEMBER [' + @DatabaseUser + ']; ')
			EXEC sp_executesql @SQL
			SET @Result='Database role ' + @DatabaseRole + ' set for user ' + @DatabaseUser;
		END
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'DELETE FROM [dbo].[ProjectUser] WHERE [LoginName]=''' + @DatabaseUser + ''' ')
		EXEC sp_executesql @SQL
		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'DELETE FROM [dbo].[UserProxy] WHERE [LoginName]=''' + @DatabaseUser + ''' ')
		EXEC sp_executesql @SQL

 		SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
						 'INSERT INTO [dbo].[UserProxy] ([LoginName], [CombinedNameCache]) VALUES (''' + @DatabaseUser + ''', ''' + @DatabaseUser + '''); ')
		EXEC sp_executesql @SQL

		IF @Workshop > 0
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'INSERT INTO [dbo].[ProjectUser] ([LoginName], [ProjectID]) ' +
							 'SELECT ''' + @DatabaseUser + ''', [ProjectID] FROM [dbo].[ProjectUser] WHERE [LoginName]=''Workshop''; ')
			EXEC sp_executesql @SQL
			SET @Result=@Result + '; Workshop projects assigned for user ' + @DatabaseUser;
		END
		ELSE
		BEGIN
			SET @SQL=(SELECT 'USE [' + @DatabaseName + ']; ' +
							 'INSERT INTO [dbo].[ProjectUser] ([LoginName], [ProjectID]) ' +
							 'SELECT ''' + @DatabaseUser + ''', [ProjectID] FROM [dbo].[ProjectProxy] WHERE [ProjectID]>0; ')
			EXEC sp_executesql @SQL
			SET @Result=@Result + '; Available projects assigned for user ' + @DatabaseUser;
		END
	END
	SELECT @Result
END
GO


--#####################################################################################################################
--######   New table PropertyType_Enum  ###############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[PropertyType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_PropertyType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PropertyType_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[PropertyType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_PropertyType_Enum_PropertyType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[PropertyType_Enum] ([Code])
GO

ALTER TABLE [dbo].[PropertyType_Enum] CHECK CONSTRAINT [FK_PropertyType_Enum_PropertyType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PropertyType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PropertyType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PropertyType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PropertyType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PropertyType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PropertyType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PropertyType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The types of a Property, e.g. Chronostratigraphy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PropertyType_Enum'
GO


--#####################################################################################################################
--######   Content of table PropertyType_Enum  ########################################################################
--#####################################################################################################################


INSERT INTO PropertyType_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        
('Stratigraphy', 'Stratigraphy', 'Stratigraphy', 1, NULL)

INSERT INTO PropertyType_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        
('Chronostratigraphy', 'Chronostratigraphy', 'Chronostratigraphy',  1, 'Stratigraphy')

INSERT INTO PropertyType_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        
('Lithostratigraphy', 'Lithostratigraphy', 'Lithostratigraphy', 1, 'Stratigraphy')

INSERT INTO PropertyType_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        
('Biostratigraphy', 'Biostratigraphy', 'Biostratigraphy', 1, 'Stratigraphy')

INSERT INTO PropertyType_Enum
(Code, Description, DisplayText, DisplayEnable, ParentCode)
VALUES        
('Vegetation', 'Vegetation', 'Vegetation', 1, NULL)


--#####################################################################################################################
--######   New column PropertyType in Property  #######################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[Property] ADD [PropertyType] [nvarchar](50) NULL;

ALTER TABLE [dbo].[Property]  WITH CHECK ADD  CONSTRAINT [FK_Property_PropertyType_Enum] FOREIGN KEY([PropertyType])
REFERENCES [dbo].[PropertyType_Enum] ([Code])
GO

ALTER TABLE [dbo].[Property] CHECK CONSTRAINT [FK_Property_PropertyType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the collection site property, e.g. Chronostratigraphy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyType'
GO


--#####################################################################################################################
--######   Update of the column descriptions in table Property  #######################################################
--#####################################################################################################################


EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Unique ID for the property (= primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyID'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'PropertyID of the superior property' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyParentID'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Name of the system used for the description of the collection site, e.g. Chronostratigraphy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyName'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The default for the accuracy of values which can be reached with this method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfProperty'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The default measurement unit for the characterisation system, e.g. pH' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DefaultMeasurementUnit'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Internal value, specifying a programming method used for parsing text in table CollectionEventProperty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'ParsingMethodName'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the site property as displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Specifies, if this item is enabled to be used within the database. Properties can be disabled to avoid seeing them, but keep the definition for the future.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DisplayEnabled'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Description of the property' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'Description'
GO

--#####################################################################################################################
--######   Set PropertyType in table Property  ########################################################################
--#####################################################################################################################

UPDATE [dbo].[Property]
   SET [PropertyType] = 'Chronostratigraphy'
 WHERE [PropertyName] = 'Chronostratigraphy'
GO

UPDATE [dbo].[Property]
   SET [PropertyType] = 'Lithostratigraphy'
 WHERE [PropertyName] = 'Lithostratigraphy'
GO

UPDATE [dbo].[Property]
   SET [PropertyType] = 'Biostratigraphy'
 WHERE [PropertyName] = 'Biostratigraphy'
GO

UPDATE [dbo].[Property]
   SET [PropertyType] = 'Vegetation'
 WHERE [ParsingMethodName] = 'Vegetation'
GO


--#####################################################################################################################
--######   Grant DELETE to CollectionManager for table CollectionSpecimen  ############################################
--#####################################################################################################################

GRANT DELETE ON dbo.[CollectionSpecimen] TO [CollectionManager];
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.23'
END

GO


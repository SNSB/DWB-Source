
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.58'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   CollectionSpecimenImage   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenImage ADD LicenseNotes nvarchar(500) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notice on license for the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'LicenseNotes'
GO

ALTER TABLE CollectionSpecimenImage ADD LicenseURI varchar(500) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the license for the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage', @level2type=N'COLUMN',@level2name=N'LicenseURI'
GO


ALTER TABLE CollectionSpecimenImage_log ADD LicenseNotes nvarchar(500) NULL
GO

ALTER TABLE CollectionSpecimenImage_log ADD LicenseURI varchar(500) NULL
GO



/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenImage]    Script Date: 22.05.2015 14:43:20 ******/
ALTER TRIGGER [dbo].[trgDelCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
FOR DELETE AS 
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
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LicenseNotes, LicenseURI,  LogVersion,  LogState, DisplayOrder) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.LicenseNotes, deleted.LicenseURI,  @Version,  'D', deleted.DisplayOrder
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LicenseNotes, LicenseURI,  LogVersion, LogState, DisplayOrder) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.LicenseNotes, deleted.LicenseURI, CollectionSpecimen.Version, 'D' , deleted.DisplayOrder
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenImage]    Script Date: 22.05.2015 14:44:50 ******/
ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenImage] ON [dbo].[CollectionSpecimenImage] 
FOR UPDATE AS
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
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LicenseNotes, LicenseURI,  LogVersion,  LogState, DisplayOrder) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.LicenseNotes, deleted.LicenseURI,  @Version,  'U', deleted.DisplayOrder
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenImage_Log (CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Description, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, LicenseNotes, LicenseURI,  LogVersion, LogState, DisplayOrder) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.ResourceURI, deleted.SpecimenPartID, deleted.IdentificationUnitID, deleted.ImageType, deleted.Description, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.LicenseNotes, deleted.LicenseURI, CollectionSpecimen.Version, 'U', deleted.DisplayOrder 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
Update CollectionSpecimenImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenImage, deleted 
where 1 = 1 
AND CollectionSpecimenImage.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenImage.URI = deleted.URI

GO



--#####################################################################################################################
--######   procCopyCollectionSpecimen   ######################################################################################
--######   Site properties were not copied ################################################################
--#####################################################################################################################


ALTER  PROCEDURE [dbo].[procCopyCollectionSpecimen] 
	(@CollectionSpecimenID int output ,
	@OriginalCollectionSpecimenID int ,
	@AccessionNumber  nvarchar(50),
	@EventCopyMode int,
	@CopyUnits int)
AS
declare @count int
declare @EventID int

/*
Copy a collection specimen
@EventCopyMode
-1: dont copy the event, leave the entry in table CollectionSpecimen empty
0:  take same event as original specimen
1:  create new event with the same data as the old specimen
*/

if (@EventCopyMode = 0) set @EventID = (SELECT CollectionEventID FROM CollectionSpecimen WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID)
if (@EventCopyMode < 0) set @EventID = null

if (@EventCopyMode > 0) 
begin
	DECLARE @RC int
	DECLARE @CollectionEventID int
	DECLARE @OriginalCollectionEventID int
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

	-- CollectionEventImage
	INSERT INTO CollectionEventImage
		(CollectionEventID, URI, ResourceURI, ImageType, Notes)
	SELECT @CollectionEventID, URI, ResourceURI, ImageType, Notes
	FROM CollectionEventImage
	WHERE (CollectionEventID = @OriginalCollectionEventID)

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
INSERT INTO CollectionAgent ( CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason)
SELECT   @CollectionSpecimenID, CollectorsName, CollectorsAgentURI, CollectorsSequence, CollectorsNumber, Notes, DataWithholdingReason
FROM CollectionAgent
WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID

-- CollectionRelation
INSERT INTO CollectionSpecimenRelation ( CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
	RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache)
SELECT @CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, 
	RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache
FROM CollectionSpecimenRelation
WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID


if @CopyUnits > 0
begin
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
		INSERT INTO Identification (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, Notes, ResponsibleName, ResponsibleAgentURI)
		SELECT @CollectionSpecimenID, @IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, 
			  IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, 
			  TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, Notes, ResponsibleName, ResponsibleAgentURI
		FROM Identification
		WHERE Identification.CollectionSpecimenID = @OriginalCollectionSpecimenID
		and Identification.IdentificationUnitID = @OriginalIdentificationUnitID

   		FETCH NEXT FROM UnitCursor INTO @OriginalIdentificationUnitID
	END
	CLOSE UnitCursor
	DEALLOCATE UnitCursor

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
	END
end

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

	-- CollectionSpecimenProcessing
	INSERT INTO CollectionSpecimenProcessing
		(CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration,  
		ResponsibleName, ResponsibleAgentURI,Notes)
	SELECT @CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration,  
		ResponsibleName, ResponsibleAgentURI,Notes
	FROM CollectionSpecimenProcessing
	WHERE CollectionSpecimenID = @OriginalCollectionSpecimenID
	AND SpecimenPartID = @OriginalSpecimenPartID

	FETCH NEXT FROM PartCursor INTO @OriginalSpecimenPartID
END
CLOSE PartCursor
DEALLOCATE PartCursor

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

INSERT INTO IdentificationUnitInPart (CollectionSpecimenID, IdentificationUnitID, SpecimenPartID, DisplayOrder)
SELECT @CollectionSpecimenID, U.IdentificationUnitID, P.SpecimenPartID, I.DisplayOrder
FROM IdentificationUnitInPart I, @TempPartTable P, @TempUnitTable U
WHERE I.IdentificationUnitID = U.OriginalIdentificationUnitID
AND I.SpecimenPartID = P.OriginalSpecimenPartID
AND I.CollectionSpecimenID = @OriginalCollectionSpecimenID

SELECT @CollectionSpecimenID

GO

--#####################################################################################################################
--######   fixing an typing error   ######################################################################################
--#####################################################################################################################


UPDATE [dbo].[LocalisationSystem] SET [Description] = 'Altitude above sea level (mNN)'
WHERE [LocalisationSystemID] = 4
GO


--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.09' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.59'
END

GO



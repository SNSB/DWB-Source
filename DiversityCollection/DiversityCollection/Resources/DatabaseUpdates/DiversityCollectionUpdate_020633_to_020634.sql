declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.33'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   CollectionSpecimen_Core2: Removal of LocalityDescription from Date   #######################################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionSpecimen_Core2]
AS
SELECT     S.CollectionSpecimenID, S.Version, S.CollectionEventID, S.CollectionID, S.AccessionNumber, S.AccessionDate, S.AccessionDay, S.AccessionMonth, S.AccessionYear, 
                      S.AccessionDateSupplement, S.AccessionDateCategory, S.DepositorsName, S.DepositorsAgentURI, S.DepositorsAccessionNumber, S.LabelTitle, S.LabelType, 
                      S.LabelTranscriptionState, S.LabelTranscriptionNotes, S.ExsiccataURI, S.ExsiccataAbbreviation, S.OriginalNotes, S.AdditionalNotes, S.ReferenceTitle, 
                      S.ReferenceURI, S.Problems, S.DataWithholdingReason, CASE WHEN E.CollectionDate IS NULL AND E.CollectionYear IS NULL AND E.CollectionMonth IS NULL AND 
                      E.CollectionDay IS NULL THEN NULL ELSE CASE WHEN E.CollectionDate IS NULL THEN CASE WHEN E.CollectionYear IS NULL 
                      THEN '----' ELSE CAST(E.CollectionYear AS varchar) END + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CASE WHEN E.CollectionMonth IS NULL 
                      THEN '--' ELSE CAST(E.CollectionMonth AS varchar) END + '/' + CASE WHEN E.CollectionDay IS NULL 
                      THEN '--' ELSE CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(E.CollectionDay AS varchar) END ELSE CAST(year(E.CollectionDate) AS varchar) 
                      + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CAST(CollectionMonth AS varchar) 
                      + '/' + CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(CollectionDay AS varchar) END END AS CollectionDate, E.LocalityDescription AS Locality, E.HabitatDescription AS Habitat, S.InternalNotes, 
                      S.ExternalDatasourceID, S.ExternalIdentifier, S.LogCreatedWhen, S.LogCreatedBy, S.LogUpdatedWhen, S.LogUpdatedBy
FROM         dbo.CollectionSpecimen AS S INNER JOIN
                      dbo.CollectionSpecimenID_Available AS UA ON S.CollectionSpecimenID = UA.CollectionSpecimenID LEFT OUTER JOIN
                      dbo.CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID

GO


--#####################################################################################################################
--######   CollectionEvent_Core2 - new column for date ################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionEvent_Core2]
AS
SELECT        E.CollectionEventID, E.Version, E.SeriesID, E.CollectorsEventNumber, E.CollectionDate, E.CollectionDay, E.CollectionMonth, E.CollectionYear, E.CollectionEndDay, E.CollectionEndMonth, E.CollectionEndYear, 
                         E.CollectionDateSupplement, E.CollectionDateCategory, E.CollectionTime, E.CollectionTimeSpan, E.LocalityDescription, E.LocalityVerbatim, E.HabitatDescription, E.ReferenceTitle, E.ReferenceDetails, E.ReferenceURI, 
                         E.CollectingMethod, E.Notes, E.CountryCache, E.DataWithholdingReason, E.DataWithholdingReasonDate, 
CASE WHEN E.CollectionDate IS NULL AND E.CollectionYear IS NULL AND E.CollectionMonth IS NULL AND E.CollectionDay IS NULL 
                         THEN NULL ELSE CASE WHEN E.CollectionDate IS NULL THEN CASE WHEN E.CollectionYear IS NULL THEN '----' ELSE CAST(E.CollectionYear AS varchar) 
                         END + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CASE WHEN E.CollectionMonth IS NULL THEN '--' ELSE CAST(E.CollectionMonth AS varchar) END + '/' + CASE WHEN E.CollectionDay IS NULL 
                         THEN '--' ELSE CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(E.CollectionDay AS varchar) END ELSE CAST(year(E.CollectionDate) AS varchar) 
                         + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CAST(CollectionMonth AS varchar) + '/' + CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(CollectionDay AS varchar) 
                         END END AS CollectionDate_YMD
FROM            dbo.CollectionEvent AS E INNER JOIN
                         dbo.CollectionEventID_AvailableNotReadOnly AS A ON E.CollectionEventID = A.CollectionEventID
GO


--#####################################################################################################################
--######   trgUpdCollectionSpecimen - Removal of obsolet part and adaption to DSGVO   #################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimen] ON [dbo].[CollectionSpecimen] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 03.04.2012  */ 

if not update(Version) 
BEGIN

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
END 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimen_Log (CollectionSpecimenID, Version, CollectionEventID, CollectionID, AccessionNumber, AccessionDate, AccessionDay, AccessionMonth, AccessionYear, AccessionDateSupplement, AccessionDateCategory, DepositorsName, DepositorsAgentURI, DepositorsAccessionNumber, LabelTitle, LabelType, LabelTranscriptionState, LabelTranscriptionNotes, ExsiccataURI, ExsiccataAbbreviation, OriginalNotes, AdditionalNotes, ReferenceTitle, ReferenceURI, Problems, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, InternalNotes, ExternalDatasourceID, ExternalIdentifier, RowGUID, ReferenceDetails,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.Version, deleted.CollectionEventID, deleted.CollectionID, deleted.AccessionNumber, deleted.AccessionDate, deleted.AccessionDay, deleted.AccessionMonth, deleted.AccessionYear, deleted.AccessionDateSupplement, deleted.AccessionDateCategory, deleted.DepositorsName, deleted.DepositorsAgentURI, deleted.DepositorsAccessionNumber, deleted.LabelTitle, deleted.LabelType, deleted.LabelTranscriptionState, deleted.LabelTranscriptionNotes, deleted.ExsiccataURI, deleted.ExsiccataAbbreviation, deleted.OriginalNotes, deleted.AdditionalNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.Problems, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.InternalNotes, deleted.ExternalDatasourceID, deleted.ExternalIdentifier, deleted.RowGUID, deleted.ReferenceDetails,  'U'
FROM DELETED

END

/* updating the logging columns */
Update CollectionSpecimen
set LogUpdatedWhen = getdate(), LogUpdatedBy = cast([dbo].[UserID]() as varchar)
FROM CollectionSpecimen, deleted 
where 1 = 1 
AND CollectionSpecimen.CollectionSpecimenID = deleted.CollectionSpecimenID

/* setting the date in ProjectProxy if the DataWithholdingReason is not filled or kept filled */ 
Update P 
set P.LastChanges = getdate()
FROM CollectionProject C, ProjectProxy P, deleted D, inserted I
where C.CollectionSpecimenID = @ID
AND C.ProjectID = P.ProjectID
AND C.CollectionSpecimenID = D.CollectionSpecimenID
AND (
((I.DataWithholdingReason IS NULL OR I.DataWithholdingReason = '') AND D.DataWithholdingReason <> '') 
OR
((D.DataWithholdingReason IS NULL OR D.DataWithholdingReason = '') AND I.DataWithholdingReason <> '') 
OR
((D.DataWithholdingReason IS NULL OR D.DataWithholdingReason = '') AND (I.DataWithholdingReason IS NULL OR I.DataWithholdingReason = '')) 
)

GO

--#####################################################################################################################
--######  Optional repair of previous skript  #########################################################################
--#####################################################################################################################
--######  New Table ProjectMaterialCategory  ##########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectMaterialCategory') = 0
begin
	CREATE TABLE [dbo].[ProjectMaterialCategory](
		[ProjectID] [int] NOT NULL,
		[MaterialCategory] [nvarchar](50) NOT NULL,
		[LogUpdatedWhen] [datetime] NULL,
		[LogUpdatedBy] [nvarchar](50) NULL,
		[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	 CONSTRAINT [PK_ProjectMaterialCategory] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[MaterialCategory] ASC
	)WITH (PAD_INDEX = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ProjectMaterialCategory] ADD  CONSTRAINT [DF_ProjectMaterialCategory_ProjectID]  DEFAULT ((1)) FOR [ProjectID]

	ALTER TABLE [dbo].[ProjectMaterialCategory] ADD  CONSTRAINT [DF_ProjectMaterialCategory_MaterialCategory]  DEFAULT (N'specimen') FOR [MaterialCategory]

	ALTER TABLE [dbo].[ProjectMaterialCategory] ADD  CONSTRAINT [DF_ProjectMaterialCategory_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]

	ALTER TABLE [dbo].[ProjectMaterialCategory] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]

	ALTER TABLE [dbo].[ProjectMaterialCategory] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]

	ALTER TABLE [dbo].[ProjectMaterialCategory]  WITH CHECK ADD  CONSTRAINT [FK_ProjectMaterialCategory_CollMaterialCategory_Enum] FOREIGN KEY([MaterialCategory])
	REFERENCES [dbo].[CollMaterialCategory_Enum] ([Code])

	ALTER TABLE [dbo].[ProjectMaterialCategory] CHECK CONSTRAINT [FK_ProjectMaterialCategory_CollMaterialCategory_Enum]

	ALTER TABLE [dbo].[ProjectMaterialCategory]  WITH CHECK ADD  CONSTRAINT [FK_ProjectMaterialCategory_Project] FOREIGN KEY([ProjectID])
	REFERENCES [dbo].[ProjectProxy] ([ProjectID])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ProjectMaterialCategory] CHECK CONSTRAINT [FK_ProjectMaterialCategory_Project]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Project. Refers to ProjectID in table ProjectProxy (foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectMaterialCategory', @level2type=N'COLUMN',@level2name=N'ProjectID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Material category of specimen. Examples: ''herbarium sheets'', ''drawings'', ''microscopic slides'' etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectMaterialCategory', @level2type=N'COLUMN',@level2name=N'MaterialCategory'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectMaterialCategory', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectMaterialCategory', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The material categorys which are possible for a certain Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectMaterialCategory'

end

GRANT SELECT ON [dbo].[ProjectMaterialCategory] TO [User]
GO
GRANT INSERT ON [dbo].[ProjectMaterialCategory] TO [Administrator]
GO
GRANT UPDATE ON [dbo].[ProjectMaterialCategory] TO [Administrator]
GO
GRANT DELETE ON [dbo].[ProjectMaterialCategory] TO [Administrator]
GO


--#####################################################################################################################
--######  trgUpdProjectMaterialCategory  ##############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE TRIGGER [dbo].[trgUpdProjectMaterialCategory] ON [dbo].[ProjectMaterialCategory] 
FOR UPDATE AS

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM ProjectMaterialCategory T, deleted D 
WHERE T.MaterialCategory = D.MaterialCategory
AND T.ProjectID = D.ProjectID;

ALTER TABLE [dbo].[ProjectMaterialCategory] ENABLE TRIGGER [trgUpdProjectMaterialCategory];')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   New table ProjectTaxonomicGroup    #########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectTaxonomicGroup') = 0
begin
	CREATE TABLE [dbo].[ProjectTaxonomicGroup](
		[ProjectID] [int] NOT NULL,
		[TaxonomicGroup] [nvarchar](50) NOT NULL,
		[LogUpdatedWhen] [datetime] NULL,
		[LogUpdatedBy] [nvarchar](50) NULL,
		[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	 CONSTRAINT [PK_ProjectTaxonomicGroup] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[TaxonomicGroup] ASC
	)WITH (PAD_INDEX = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ProjectTaxonomicGroup] ADD  CONSTRAINT [DF_ProjectTaxonomicGroup_ProjectID]  DEFAULT ((1)) FOR [ProjectID]

	ALTER TABLE [dbo].[ProjectTaxonomicGroup] ADD  CONSTRAINT [DF_ProjectTaxonomicGroup_TaxonomicGroup]  DEFAULT (N'plant') FOR [TaxonomicGroup]

	ALTER TABLE [dbo].[ProjectTaxonomicGroup] ADD  CONSTRAINT [DF_ProjectTaxonomicGroup_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]

	ALTER TABLE [dbo].[ProjectTaxonomicGroup] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]

	ALTER TABLE [dbo].[ProjectTaxonomicGroup] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]

	ALTER TABLE [dbo].[ProjectTaxonomicGroup]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTaxonomicGroup_CollTaxonomicGroup_Enum] FOREIGN KEY([TaxonomicGroup])
	REFERENCES [dbo].[CollTaxonomicGroup_Enum] ([Code])

	ALTER TABLE [dbo].[ProjectTaxonomicGroup] CHECK CONSTRAINT [FK_ProjectTaxonomicGroup_CollTaxonomicGroup_Enum]

	ALTER TABLE [dbo].[ProjectTaxonomicGroup]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTaxonomicGroup_Project] FOREIGN KEY([ProjectID])
	REFERENCES [dbo].[ProjectProxy] ([ProjectID])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ProjectTaxonomicGroup] CHECK CONSTRAINT [FK_ProjectTaxonomicGroup_Project]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Project. Refers to ProjectID in table ProjectProxy (foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTaxonomicGroup', @level2type=N'COLUMN',@level2name=N'ProjectID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Taxonomic group of specimen. Examples: ''plant'', ''animal'', etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTaxonomicGroup', @level2type=N'COLUMN',@level2name=N'TaxonomicGroup'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTaxonomicGroup', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTaxonomicGroup', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The taxonomic group which are possible for a certain Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTaxonomicGroup'
end

GRANT SELECT ON [dbo].[ProjectTaxonomicGroup] TO [User]
GO
GRANT INSERT ON [dbo].[ProjectTaxonomicGroup] TO [Administrator]
GO
GRANT UPDATE ON [dbo].[ProjectTaxonomicGroup] TO [Administrator]
GO
GRANT DELETE ON [dbo].[ProjectTaxonomicGroup] TO [Administrator]
GO


--#####################################################################################################################
--######  trgUpdProjectTaxonomicGroup  ################################################################################
--#####################################################################################################################
declare @SQL nvarchar(max)
set @SQL = (select 'CREATE TRIGGER [dbo].[trgUpdProjectTaxonomicGroup] ON [dbo].[ProjectTaxonomicGroup] 
FOR UPDATE AS

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM ProjectTaxonomicGroup T, deleted D 
WHERE T.TaxonomicGroup = D.TaxonomicGroup
AND T.ProjectID = D.ProjectID;

ALTER TABLE [dbo].[ProjectTaxonomicGroup] ENABLE TRIGGER [trgUpdProjectTaxonomicGroup];')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######  procCopyCollectionSpecimen2: Bugfix for getting correct Specimen ID #########################################
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
/*
-- not needed here - Result of procedure creates several lines with the last line containing the correct ID. Problem solved in application
IF @CollectionSpecimenID < (SELECT MAX(CollectionSpecimenID) FROM CollectionSpecimen)
BEGIN
	SET @CollectionSpecimenID = (SELECT MAX(CollectionSpecimenID) FROM CollectionSpecimen)
END
*/

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
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.34'
END

GO


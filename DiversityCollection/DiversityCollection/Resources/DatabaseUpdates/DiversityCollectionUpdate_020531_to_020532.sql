
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.31'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   ParentUnitID   ######################################################################################
--#####################################################################################################################

ALTER TABLE IdentificationUnit ADD ParentUnitID int NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The IdentificationUnitID of a parent organism of which this organism is a child of (= foreign key).' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnit', @level2type=N'COLUMN',@level2name=N'ParentUnitID'
GO

ALTER TABLE [dbo].[IdentificationUnit]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnit_IdentificationUnit1] FOREIGN KEY([CollectionSpecimenID], [ParentUnitID])
REFERENCES [dbo].[IdentificationUnit] ([CollectionSpecimenID], [IdentificationUnitID])
GO


ALTER TABLE IdentificationUnit_log ADD ParentUnitID nvarchar(255) NULL
GO



--#####################################################################################################################
--######   trgDelIdentificationUnit   ######################################################################################
--#####################################################################################################################



ALTER TRIGGER [dbo].[trgDelIdentificationUnit] ON [dbo].[IdentificationUnit] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 


/* setting the version in the main table */ 
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID,
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState)  
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end


GO



--#####################################################################################################################
--######   trgUpdIdentificationUnit   ######################################################################################
--#####################################################################################################################



ALTER TRIGGER [dbo].[trgUpdIdentificationUnit] ON [dbo].[IdentificationUnit] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* setting the version in the main table */ 
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)


/* updating the logging columns */
Update IdentificationUnit
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnit, deleted 
where 1 = 1 
AND IdentificationUnit.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnit.IdentificationUnitID = deleted.IdentificationUnitID


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnit_Log (CollectionSpecimenID, IdentificationUnitID, LastIdentificationCache, FamilyCache, OrderCache, 
TaxonomicGroup, OnlyObserved, RelatedUnitID, RelationType, ColonisedSubstratePart, LifeStage, 
Gender, NumberOfUnits, ExsiccataNumber, ExsiccataIdentification, UnitIdentifier, UnitDescription, Circumstances, DisplayOrder, 
Notes, RowGUID, HierarchyCache, ParentUnitID, 
LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState)  
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.LastIdentificationCache, deleted.FamilyCache, deleted.OrderCache,
deleted.TaxonomicGroup, deleted.OnlyObserved, deleted.RelatedUnitID, deleted.RelationType, deleted.ColonisedSubstratePart, deleted.LifeStage, 
deleted.Gender, deleted.NumberOfUnits, deleted.ExsiccataNumber, deleted.ExsiccataIdentification, deleted.UnitIdentifier, deleted.UnitDescription, deleted.Circumstances, deleted.DisplayOrder, 
deleted.Notes, deleted.RowGUID, deleted.HierarchyCache, deleted.ParentUnitID, 
deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end


GO



--#####################################################################################################################
--######   Child   ######################################################################################
--#####################################################################################################################

INSERT INTO [CollUnitRelationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('Child of',	
           'child association (i.e. in the literal genetical sense, not in an abstract sense)',	
           'Child of',	
           191,	
           1)
GO


--#####################################################################################################################
--######   Geography in  CollectionEventLocalisation  ######################################################################################
--#####################################################################################################################



ALTER TRIGGER [dbo].[trgInsCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR INSERT AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.03.2013  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from inserted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM inserted)

	-- Update of the Cache fields if possible
	DECLARE @Altitude float
	DECLARE @Latitude float
	DECLARE @Longitude float
	DECLARE @Geography geography
	DECLARE @ParsingMethod nvarchar(50)
	DECLARE @LocalisationSystemName nvarchar(100)

	SET @ParsingMethod = (SELECT LocalisationSystem.ParsingMethodName
	FROM inserted ,LocalisationSystem 
	WHERE inserted.LocalisationSystemID = LocalisationSystem.LocalisationSystemID
	AND inserted.CollectionEventID = @ID) 

	SET @LocalisationSystemName = (SELECT LocalisationSystem.LocalisationSystemName
	FROM inserted ,LocalisationSystem 
	WHERE inserted.LocalisationSystemID = LocalisationSystem.LocalisationSystemID
	AND inserted.CollectionEventID = @ID) 

	IF @ParsingMethod = 'Altitude'
	BEGIN TRY
		SET @Altitude = (SELECT CAST(REPLACE(Location1, ',', '.') AS FLOAT) FROM inserted WHERE NOT Location1 IS NULL AND ISNUMERIC(Location1) =  1 AND AverageAltitudeCache IS NULL)
		DECLARE @Alt2 float
		SET @Alt2 = (SELECT CAST(REPLACE(Location2, ',', '.') AS FLOAT) FROM inserted WHERE NOT Location2 IS NULL AND ISNUMERIC(Location2) =  1 AND AverageAltitudeCache IS NULL)
		IF (NOT @Alt2 IS NULL)
			BEGIN
			SET @Altitude = ((SELECT @Altitude + @Alt2) / 2)
			END
		IF (NOT @Altitude IS NULL)
			BEGIN 
			UPDATE CollectionEventLocalisation SET CollectionEventLocalisation.AverageAltitudeCache = @Altitude
			FROM CollectionEventLocalisation, inserted
			WHERE inserted.LocalisationSystemID = CollectionEventLocalisation.LocalisationSystemID
			AND CollectionEventLocalisation.CollectionEventID = @ID
			AND inserted.CollectionEventID = @ID
			END 
	END TRY
	BEGIN CATCH
	END CATCH

	IF @ParsingMethod = 'Coordinates'
	BEGIN TRY
		SET @Latitude = (SELECT CAST(REPLACE(Location2, ',', '.') AS FLOAT) FROM inserted WHERE NOT Location2 IS NULL AND ISNUMERIC(Location2) =  1 AND AverageLatitudeCache IS NULL)
		SET @Longitude = (SELECT CAST(REPLACE(Location1, ',', '.') AS FLOAT) FROM inserted WHERE NOT Location1 IS NULL AND ISNUMERIC(Location1) =  1 AND AverageLongitudeCache IS NULL)
		IF (NOT @Longitude IS NULL AND NOT @Latitude IS NULL)
			BEGIN
			UPDATE CollectionEventLocalisation SET 
			CollectionEventLocalisation.AverageLatitudeCache = @Latitude,
			CollectionEventLocalisation.AverageLongitudeCache = @Longitude
			FROM CollectionEventLocalisation, inserted
			WHERE inserted.LocalisationSystemID = CollectionEventLocalisation.LocalisationSystemID
			AND CollectionEventLocalisation.CollectionEventID = @ID
			AND inserted.CollectionEventID = @ID
			END
	END TRY
	BEGIN CATCH
	END CATCH

	IF @LocalisationSystemName = 'Coordinates WGS84'
	BEGIN TRY
		SET @Latitude = (SELECT CAST(REPLACE(Location2, ',', '.') AS FLOAT) FROM inserted WHERE NOT Location2 IS NULL AND ISNUMERIC(Location2) =  1 AND AverageLatitudeCache IS NULL)
		SET @Longitude = (SELECT CAST(REPLACE(Location1, ',', '.') AS FLOAT) FROM inserted WHERE NOT Location1 IS NULL AND ISNUMERIC(Location1) =  1 AND AverageLongitudeCache IS NULL)
		SET @Geography = geography::STPointFromText('POINT(' + CAST(@Longitude AS VARCHAR) + ' ' + CAST(@Latitude AS VARCHAR) + ')', 4326);
		IF (NOT @Geography IS NULL)
			BEGIN
			UPDATE CollectionEventLocalisation SET 
			CollectionEventLocalisation.Geography = @Geography
			FROM CollectionEventLocalisation, inserted
			WHERE inserted.LocalisationSystemID = CollectionEventLocalisation.LocalisationSystemID
			AND CollectionEventLocalisation.CollectionEventID = @ID
			AND inserted.CollectionEventID = @ID
			AND inserted.Geography IS NULL
			END
		SET @Latitude = (SELECT inserted.Geography.EnvelopeCenter().Lat FROM inserted WHERE Location2 IS NULL AND NOT inserted.Geography IS NULL AND AverageLatitudeCache IS NULL)
		SET @Longitude = (SELECT inserted.Geography.EnvelopeCenter().Long FROM inserted WHERE Location1 IS NULL AND NOT inserted.Geography IS NULL AND AverageLongitudeCache IS NULL)
		SET @Geography = (SELECT inserted.Geography FROM inserted);
		IF (NOT @Geography IS NULL 
		AND NOT @Latitude IS NULL
		AND NOT @Longitude IS NULL)
		BEGIN
			UPDATE CollectionEventLocalisation SET 
			CollectionEventLocalisation.Location1 = @Longitude,
			CollectionEventLocalisation.Location2 = @Latitude,
			CollectionEventLocalisation.AverageLongitudeCache = @Longitude,
			CollectionEventLocalisation.AverageLatitudeCache = @Latitude
			FROM CollectionEventLocalisation, inserted
			WHERE inserted.LocalisationSystemID = CollectionEventLocalisation.LocalisationSystemID
			AND CollectionEventLocalisation.CollectionEventID = @ID
			AND inserted.CollectionEventID = @ID
			AND NOT inserted.Geography IS NULL
		END
	END TRY
	BEGIN CATCH
	END CATCH

   EXECUTE procSetVersionCollectionEvent @ID
END
GO
--#####################################################################################################################
--######   ToolHierarchyAll   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[ToolHierarchyAll] ()  
RETURNS @ToolList TABLE ([ToolID] [int] Primary key ,
	[ToolParentID] [int] NULL ,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[ToolUsageTemplate] xml,
	[Notes] [nvarchar](max) NULL,
	[ToolURI] [varchar](255) NULL,
	[OnlyHierarchy] [bit] NULL,
	[HierarchyDisplayText] [nvarchar] (900) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the Tool items related to the given Tool.
MW 02.01.2006
TEST:
SELECT * FROM DBO.ToolHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @ToolList (ToolID, ToolParentID, Name, Description, ToolUsageTemplate, Notes, ToolURI, OnlyHierarchy, HierarchyDisplayText)
SELECT DISTINCT ToolID, ToolParentID, Name, Description, CAST(ToolUsageTemplate AS nvarchar(4000)), Notes, ToolURI, OnlyHierarchy, Name
FROM Tool
WHERE Tool.ToolParentID IS NULL

declare @i int
set @i = (select count(*) from Tool where ToolID not IN (select ToolID from  @ToolList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @ToolList (ToolID, ToolParentID, Name, Description, ToolUsageTemplate, Notes, ToolURI, OnlyHierarchy, HierarchyDisplayText)
	SELECT DISTINCT C.ToolID, C.ToolParentID, C.Name, C.Description, CAST(C.ToolUsageTemplate AS nvarchar(4000)), C.Notes, C.ToolURI, C.OnlyHierarchy, L.HierarchyDisplayText + ' | ' + C.Name
	FROM Tool C, @ToolList L
	WHERE C.ToolParentID = L.ToolID
	AND C.ToolID NOT IN (select ToolID from  @ToolList)

	set @i = (select count(*) from Tool where ToolID not IN (select ToolID from  @ToolList))
end


   RETURN
END
GO

GRANT SELECT ON DBO.ToolHierarchyAll TO [USER]
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.32'
END

GO



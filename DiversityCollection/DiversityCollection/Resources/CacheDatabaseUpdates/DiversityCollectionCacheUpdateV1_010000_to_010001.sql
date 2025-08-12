
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.00'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   CoordinatePrecision in table ProjectPublished   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectPublished' and C.COLUMN_NAME = 'CoordinatePrecision') = 0
begin
ALTER TABLE [dbo].[ProjectPublished] ADD CoordinatePrecision tinyint NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional reduction of the precision of the coordinates within the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'CoordinatePrecision'
end
GO

--#####################################################################################################################
--######   CollectionSpecimenCache   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionSpecimenCache') = 0
begin
	CREATE TABLE [dbo].[CollectionSpecimenCache](
		[CollectionSpecimenID] [int] NOT NULL,
		[Version] [int] NULL,
		[AccessionNumber] [nvarchar](50) NULL,
		[CollectionDate] [varchar](50) NULL,
		[CollectionDay] [tinyint] NULL,
		[CollectionMonth] [tinyint] NULL,
		[CollectionYear] [smallint] NULL,
		[CollectionDateSupplement] [nvarchar](100) NULL,
		[LocalityDescription] [nvarchar](max) NULL,
		[CountryCache] [nvarchar](50) NULL,
		[LabelTranscriptionNotes] [nvarchar](255) NULL,
		[ExsiccataAbbreviation] [nvarchar](255) NULL,
		[OriginalNotes] [nvarchar](max) NULL,
		[Latitude] [float] NULL,
		[Longitude] [float] NULL,
		[LocationAccuracy] [nvarchar](50) NULL,
		[DistanceToLocation] [nvarchar](50) NULL,
		[DirectionToLocation] [nvarchar](50) NULL,
		[CollectorsEventNumber] [nvarchar](50) NULL,
		[Chronostratigraphy] [nvarchar](500) NULL,
		[Lithostratigraphy] [nvarchar](500) NULL,
		[RecordURI] [varchar](255) NULL,
		[LogInsertedWhen] [smalldatetime] NULL CONSTRAINT [DF_CollectionSpecimenCache_LogInsertedWhen]  DEFAULT (getdate()),
		[CollectionEventID] [int] NULL,
		[CountryIsoCode] [varchar](50) NULL,
		[DateLastEdited] [datetime] NULL,
	 CONSTRAINT [PK_CollectionSpecimenCache] PRIMARY KEY CLUSTERED 
	(
		[CollectionSpecimenID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The accuracy of the determination of this locality' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache', @level2type=N'COLUMN',@level2name=N'LocationAccuracy'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Distance from the specified place to the real location of the colletion event (m)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache', @level2type=N'COLUMN',@level2name=N'DistanceToLocation'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Direction from the specified place to the real location of the colletion event (Degrees rel. to north)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache', @level2type=N'COLUMN',@level2name=N'DirectionToLocation'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number assigned to a collection event  by the collector (= ''field number'')' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache', @level2type=N'COLUMN',@level2name=N'CollectorsEventNumber'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the event' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache', @level2type=N'COLUMN',@level2name=N'CollectionEventID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ISO 3166 Code of the country' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache', @level2type=N'COLUMN',@level2name=N'CountryIsoCode'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the cached data from CollectionSpecimen and CollectionEvent for transfer.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache'

end


--#####################################################################################################################
--######   CollectionEventID in table CollectionSpecimenCache   ######################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionSpecimenCache' and C.COLUMN_NAME = 'CollectionEventID') = 0
begin
ALTER TABLE [dbo].[CollectionSpecimenCache] ADD CollectionEventID int NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the event' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenCache', @level2type=N'COLUMN',@level2name=N'CollectionEventID'
end
GO


--#####################################################################################################################
--######   ProjectURI in table ProjectPublished   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectPublished' and C.COLUMN_NAME = 'ProjectURI') = 0
begin
ALTER TABLE [dbo].[ProjectPublished] ADD ProjectURI varchar(255) NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectPublished', @level2type=N'COLUMN',@level2name=N'ProjectURI'
end
GO




--#########################################################################################################################################
--######   [procRefreshCollectionSpecimenCacheCoordinates] Inclusion of SamplingPlots and Accuray of coordinates   ###################################
--#########################################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [dbo].[procRefreshCollectionSpecimenCacheCoordinates] 
AS
-- setting the CollectionEventID to ease subsequent update
UPDATE C SET C.CollectionEventID = E.[CollectionEventID] 
FROM [dbo].[CollectionSpecimenCache] C, CollectionSpecimenEventID E
WHERE C.CollectionSpecimenID = E.CollectionSpecimenID

DECLARE @LocalisationSystemID int

-- the sequence of the LocalisationSystems that should be scanned to insert the coordiates
DECLARE @TempLocalisationSystem TABLE (OrderSequence int primary key, LocalisationSystemID int)
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (1, 8)
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (2, 13)
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (3, 6)
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (4, 2)
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (5, 9)
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (6, 1)
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (7, 3)
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (8, 7)

-- inserting the coordinates
while ((select count (*) from @TempLocalisationSystem) > 0) 
begin
	set @LocalisationSystemID = (select min(LocalisationSystemID) from @TempLocalisationSystem where OrderSequence = (select min(OrderSequence) from @TempLocalisationSystem))
	
	UPDATE CC
	SET CC.Latitude = CAST(L.AverageLatitudeCache AS float),
	CC.Longitude = L.AverageLongitudeCache
	FROM CollectionSpecimenCache CC, 
	dbo.CollectionEvent E,
	dbo.CollectionEventLocalisation L
	WHERE CC.CollectionEventID = L.CollectionEventID
	AND L.LocalisationSystemID = @LocalisationSystemID
	AND CC.Latitude IS NULL
	AND L.AverageLatitudeCache <> 0
	
	delete @TempLocalisationSystem where LocalisationSystemID = @LocalisationSystemID

end

-- reducing the precision of the coordinates for those project where a reduction is demanded
DECLARE @CoordinatePrecision TABLE(ProjectID int primary key, CoordinatePrecision tinyint)

-- getting all projects where the accuracy of the coordinates should be reduced
insert into @CoordinatePrecision (ProjectID, CoordinatePrecision)
select ProjectID, CoordinatePrecision from [dbo].[ProjectPublished] PP
where not PP.CoordinatePrecision is null

declare @ProjectID int
set @ProjectID = (select min(ProjectID) from @CoordinatePrecision)
-- reducing the precision
while (select count(*) from @CoordinatePrecision) > 0
begin
	UPDATE CC
	SET CC.Latitude = round(CC.Latitude, CA.CoordinatePrecision),
	CC.Longitude = round(CC.Longitude, CA.CoordinatePrecision)
	FROM CollectionSpecimenCache CC, CollectionProject P, @CoordinatePrecision CA
	where CC.CollectionSpecimenID = P.CollectionSpecimenID and P.ProjectID = CA.ProjectID
	and CC.Latitude <> 0 or CC.Longitude <> 0
	and CA.ProjectID = @ProjectID

	set @ProjectID = (select min(ProjectID) from @CoordinatePrecision)

	delete CA from @CoordinatePrecision CA
	where CA.ProjectID = @ProjectID

end')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

--CREATE  PROCEDURE [dbo].[procRefreshCollectionSpecimenCacheCoordinates] 
--AS
---- setting the CollectionEventID to ease subsequent update
--UPDATE C SET C.CollectionEventID = E.[CollectionEventID] 
--FROM [dbo].[CollectionSpecimenCache] C, CollectionSpecimenEventID E
--WHERE C.CollectionSpecimenID = E.CollectionSpecimenID

--DECLARE @LocalisationSystemID int

---- the sequence of the LocalisationSystems that should be scanned to insert the coordiates
--DECLARE @TempLocalisationSystem TABLE (OrderSequence int primary key, LocalisationSystemID int)
--	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (1, 8)
--	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (2, 13)
--	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (3, 6)
--	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (4, 2)
--	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (5, 9)
--	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (6, 1)
--	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (7, 3)
--	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (8, 7)

---- inserting the coordinates
--while ((select count (*) from @TempLocalisationSystem) > 0) 
--begin
--	set @LocalisationSystemID = (select min(LocalisationSystemID) from @TempLocalisationSystem where OrderSequence = (select min(OrderSequence) from @TempLocalisationSystem))
	
--	UPDATE CC
--	SET CC.Latitude = CAST(L.AverageLatitudeCache AS float),
--	CC.Longitude = L.AverageLongitudeCache
--	FROM CollectionSpecimenCache CC, 
--	dbo.CollectionEvent E,
--	dbo.CollectionEventLocalisation L
--	WHERE CC.CollectionEventID = L.CollectionEventID
--	AND L.LocalisationSystemID = @LocalisationSystemID
--	AND CC.Latitude IS NULL
--	AND L.AverageLatitudeCache <> 0
	
--	delete @TempLocalisationSystem where LocalisationSystemID = @LocalisationSystemID

--end

---- reducing the precision of the coordinates for those project where a reduction is demanded
--DECLARE @CoordinatePrecision TABLE(ProjectID int primary key, CoordinatePrecision tinyint)

---- getting all projects where the accuracy of the coordinates should be reduced
--insert into @CoordinatePrecision (ProjectID, CoordinatePrecision)
--select ProjectID, CoordinatePrecision from [dbo].[ProjectPublished] PP
--where not PP.CoordinatePrecision is null

--declare @ProjectID int
--set @ProjectID = (select min(ProjectID) from @CoordinatePrecision)
---- reducing the precision
--while (select count(*) from @CoordinatePrecision) > 0
--begin
--	UPDATE CC
--	SET CC.Latitude = round(CC.Latitude, CA.CoordinatePrecision),
--	CC.Longitude = round(CC.Longitude, CA.CoordinatePrecision)
--	FROM CollectionSpecimenCache CC, CollectionProject P, @CoordinatePrecision CA
--	where CC.CollectionSpecimenID = P.CollectionSpecimenID and P.ProjectID = CA.ProjectID
--	and CC.Latitude <> 0 or CC.Longitude <> 0
--	and CA.ProjectID = @ProjectID

--	set @ProjectID = (select min(ProjectID) from @CoordinatePrecision)

--	delete CA from @CoordinatePrecision CA
--	where CA.ProjectID = @ProjectID

--end

--GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.01'
END

GO



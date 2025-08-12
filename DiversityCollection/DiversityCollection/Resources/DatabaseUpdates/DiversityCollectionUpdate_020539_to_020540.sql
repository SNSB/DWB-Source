
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.39'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [[FirstLinesEvent_2]]   ######################################################################################
-- Erweiterung um MTB_accuracy
--#####################################################################################################################


CREATE FUNCTION [dbo].[FirstLinesEvent_2] 
(@CollectionSpecimenIDs varchar(8000))   
RETURNS @List TABLE (
	[CollectionEventID] [int] Primary key, --
	--[Accession_number] [nvarchar](50) NULL, --
-- WITHHOLDINGREASONS
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, --
--CollectionEvent
	[Collectors_event_number] [nvarchar](50) NULL, --
	[Collection_day] [tinyint] NULL, --
	[Collection_month] [tinyint] NULL, --
	[Collection_year] [smallint] NULL, --
	[Collection_date_supplement] [nvarchar](100) NULL, --
	[Collection_time] [varchar](50) NULL, --
	[Collection_time_span] [varchar](50) NULL, --
	[Country] [nvarchar](50) NULL, --
	[Locality_description] [nvarchar](max) NULL, --
	[Habitat_description] [nvarchar](max) NULL, -- 
	[Collecting_method] [nvarchar](max) NULL, --
	[Collection_event_notes] [nvarchar](max) NULL, --
--Localisation
	[Named_area] [nvarchar](255) NULL, -- 
	[NamedAreaLocation2] [nvarchar](255) NULL, --
	[Remove_link_to_gazetteer] [int] NULL,
	[Distance_to_location] [varchar](50) NULL, --
	[Direction_to_location] [varchar](50) NULL, --
	[Longitude] [nvarchar](255) NULL, --
	[Latitude] [nvarchar](255) NULL, --
	[Coordinates_accuracy] [nvarchar](50) NULL, --
	[Link_to_GoogleMaps] [int] NULL,
	[Altitude_from] [nvarchar](255) NULL, --
	[Altitude_to] [nvarchar](255) NULL, --
	[Altitude_accuracy] [nvarchar](50) NULL, --
	[MTB] [nvarchar](255) NULL, --
	[Quadrant] [nvarchar](255) NULL, --
	[Notes_for_MTB] [nvarchar](max) NULL, --
	[MTB_accuracy] [nvarchar](50) NULL, --
	[Sampling_plot] [nvarchar](255) NULL, --
	[Link_to_SamplingPlots] [nvarchar](255) NULL, --
	[Remove_link_to_SamplingPlots] [int] NULL,
	[Accuracy_of_sampling_plot] [nvarchar](50) NULL, --
	[Latitude_of_sampling_plot] [real] NULL, --
	[Longitude_of_sampling_plot] [real] NULL, --
--Properties
	[Geographic_region] [nvarchar](255) NULL, --
	[Lithostratigraphy] [nvarchar](255) NULL, --
	[Chronostratigraphy] [nvarchar](255) NULL, --
--Hidden fields
	[_CollectionEventID] [int] NULL, --
	[_CoordinatesAverageLatitudeCache] [real] NULL, --
	[_CoordinatesAverageLongitudeCache] [real] NULL, --
	[_CoordinatesLocationNotes] [nvarchar](max) NULL, --
	[_GeographicRegionPropertyURI] [varchar](255) NULL, --
	[_LithostratigraphyPropertyURI] [varchar](255) NULL, --
	[_ChronostratigraphyPropertyURI] [varchar](255) NULL, --
	[_NamedAverageLatitudeCache] [real] NULL, --
	[_NamedAverageLongitudeCache] [real] NULL, --
	[_LithostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_ChronostratigraphyPropertyHierarchyCache] [nvarchar](max) NULL, --
	[_AverageAltitudeCache] [real] NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
MW 18.11.2009 
TEST: 
Select * from dbo.FirstLinesEvent('189876, 189882, 189885, 189891, 189900, 189905, 189919, 189923, 189936, 189939, 189941, 189956, 189974, 189975, 189984, 189988, 189990, 189995, 190014, 190016, 190020, 190028, 190040, 190049, 190051, 190055, 190058, 190062, 190073, 190080, 190081, 190085, 190091, 190108, 190117, 190120, 190122, 190128, 190130, 190142')
Select * from dbo.FirstLinesEvent('3251, 3252')
*/ 
AS 
BEGIN 

declare @IDs table (ID int  Primary key)
declare @sID varchar(50)
while @CollectionSpecimenIDs <> ''
begin
	if (CHARINDEX(',', @CollectionSpecimenIDs) > 0)
	begin
	set @sID = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, 1, CHARINDEX(',', @CollectionSpecimenIDs) -1)))
	set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 2, 8000)))
	if (isnumeric(@sID) = 1)
		begin
		insert into @IDs 
		values( @sID )
		end
	end
	else
	begin
	set @sID = rtrim(ltrim(@CollectionSpecimenIDs))
	set @CollectionSpecimenIDs = ''
	if (isnumeric(@sID) = 1)
		begin
		insert into @IDs 
		values( @sID )
		end
	end
end





--- Event
insert into @List (
  CollectionEventID
, Collection_day
, Collection_month
, Collection_year
, Collection_date_supplement
, Collection_time
, Collection_time_span
, Country
, Locality_description
, Habitat_description
, Collecting_method
, Collection_event_notes
, Data_withholding_reason_for_collection_event
, Collectors_event_number
)
select 
  CollectionEventID
, CollectionDay
, CollectionMonth
, CollectionYear
, CollectionDateSupplement
, CollectionTime
, CollectionTimeSpan
, CountryCache
, LocalityDescription
, HabitatDescription
, CollectingMethod
, Notes
, DataWithholdingReason
, CollectorsEventNumber
from dbo.CollectionEvent E
where CollectionEventID in 
(select CollectionEventID from CollectionSpecimen S, dbo.CollectionSpecimenID_UserAvailable A
where S.CollectionSpecimenID in( select ID from @IDs )
and not CollectionEventID is null
and S.CollectionSpecimenID = A.CollectionSpecimenID)  




--- Named Area

update L
set L.Named_area = E.Location1
, L.NamedAreaLocation2 = E.Location2
, L.Distance_to_location = E.DistanceToLocation
, L.Direction_to_location = E.DirectionToLocation
, L._NamedAverageLatitudeCache = E.AverageLatitudeCache
, L._NamedAverageLongitudeCache = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L.CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 7


--- Coordinates

update L
set L.Longitude = E.Location1
, L.Latitude = E.Location2
, L.Coordinates_accuracy = E.LocationAccuracy
, L._CoordinatesAverageLatitudeCache = E.AverageLatitudeCache
, L._CoordinatesAverageLongitudeCache = E.AverageLongitudeCache
, L._CoordinatesLocationNotes = E.LocationNotes
from @List L,
dbo.CollectionEventLocalisation E
where L.CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 8


--- Altitude

update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L.CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4



--- MTB

update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = E.LocationNotes
, L.MTB_accuracy = E.LocationAccuracy
from @List L,
dbo.CollectionEventLocalisation E
where L.CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 3



--- SamplingPlots

update L
set L.Sampling_plot = E.Location1
, L.Link_to_SamplingPlots = E.Location2
, L.Accuracy_of_sampling_plot = E.LocationAccuracy
, L.Latitude_of_sampling_plot = E.AverageLatitudeCache
, L.Longitude_of_sampling_plot = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L.CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 13



--- GeographicRegions

update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L.CollectionEventID = P.CollectionEventID
and P.PropertyID = 10


--- Lithostratigraphy

update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L.CollectionEventID = P.CollectionEventID
and P.PropertyID = 30



--- Chronostratigraphy

update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = P.PropertyHierarchyCache
from @List L,
dbo.CollectionEventProperty P
where L.CollectionEventID = P.CollectionEventID
and P.PropertyID = 20
	
                  
RETURN 
END   

GO

GRANT SELECT ON [dbo].[FirstLinesEvent_2] TO [User]
GO

--#####################################################################################################################
--######   catalogueoflife - Korrektur   ######################################################################################
--#####################################################################################################################


UPDATE I SET I.[TaxonomicName] = REPLACE([TaxonomicName], '  ', ' ')
  FROM [Identification] I
where I.NameURI like 'http://webservice.catalogueoflife.org/annual-checklist/%'
and I.TaxonomicName like '%  %'

GO


--#####################################################################################################################
--######   CollectionEventParameterValue   ######################################################################################
--#####################################################################################################################

ALTER TABLE dbo.CollectionEventParameterValue ADD 	[LogInsertedWhen] [datetime] NULL
GO
ALTER TABLE dbo.CollectionEventParameterValue ADD   [LogInsertedBy] [nvarchar](50) NULL
GO
ALTER TABLE dbo.CollectionEventParameterValue ADD	[LogUpdatedWhen] [datetime] NULL
GO
ALTER TABLE dbo.CollectionEventParameterValue ADD	[LogUpdatedBy] [nvarchar](50) NULL
GO
ALTER TABLE dbo.CollectionEventParameterValue ADD	[RowGUID] [uniqueidentifier] NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

--#####################################################################################################################
--######   CollectionEventParameterValue_log   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionEventParameterValue_log] 
([CollectionEventID] [int] NULL,
 [MethodID] [int] NULL,
 [ParameterID] [int] NULL,
 [Value] [nvarchar] ( MAX ) COLLATE Latin1_General_CI_AS NULL,
 [LogInsertedWhen] [datetime] NULL,
 [LogInsertedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [LogUpdatedWhen] [datetime] NULL,
 [LogUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
 [RowGUID] [uniqueidentifier] NULL,
 [LogState] [char](1) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_CollectionEventParameterValue_Log_LogState]  DEFAULT ('U'), 
[LogDate] [datetime] NOT NULL CONSTRAINT [DF_CollectionEventParameterValue_Log_LogDate]  DEFAULT (getdate()), 
[LogUser] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_CollectionEventParameterValue_Log_LogUser]  DEFAULT (user_name()), 
[LogID] [int] IDENTITY(1,1) NOT NULL, 
 CONSTRAINT [PK_CollectionEventParameterValue_Log] PRIMARY KEY CLUSTERED 
([LogID] ASC )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) 
ON [PRIMARY] 

GO

GRANT INSERT ON CollectionEventParameterValue_log TO [Editor]
GO
GRANT SELECT ON CollectionEventParameterValue_log TO [Editor]
GO



CREATE TRIGGER [dbo].[trgDelCollectionEventParameterValue] ON [dbo].[CollectionEventParameterValue] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 15.11.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventParameterValue_Log (CollectionEventID, MethodID, ParameterID, Value, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO


CREATE TRIGGER [dbo].[trgUpdCollectionEventParameterValue] ON [dbo].[CollectionEventParameterValue] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 15.11.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventParameterValue_Log (CollectionEventID, MethodID, ParameterID, Value, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionEventParameterValue
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventParameterValue, deleted 
where 1 = 1 
AND CollectionEventParameterValue.CollectionEventID = deleted.CollectionEventID
AND CollectionEventParameterValue.MethodID = deleted.MethodID
AND CollectionEventParameterValue.ParameterID = deleted.ParameterID
GO


--#####################################################################################################################
--######   [MethodForAnalysis]   ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[MethodForAnalysis](
	[AnalysisID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_MethodForAnalysis] PRIMARY KEY CLUSTERED 
(
	[AnalysisID] ASC,
	[MethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Analysis (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'AnalysisID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Method (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Methods available for a Analysis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForAnalysis'
GO

ALTER TABLE [dbo].[MethodForAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_MethodForAnalysis_Analysis] FOREIGN KEY([AnalysisID])
REFERENCES [dbo].[Analysis] ([AnalysisID])
GO

ALTER TABLE [dbo].[MethodForAnalysis] CHECK CONSTRAINT [FK_MethodForAnalysis_Analysis]
GO

ALTER TABLE [dbo].[MethodForAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_MethodForAnalysis_Method] FOREIGN KEY([MethodID])
REFERENCES [dbo].[Method] ([MethodID])
GO

ALTER TABLE [dbo].[MethodForAnalysis] CHECK CONSTRAINT [FK_MethodForAnalysis_Method]
GO

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[MethodForAnalysis] ADD  CONSTRAINT [DF_MethodForAnalysis_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT SELECT ON MethodForAnalysis TO [User]
GO

GRANT UPDATE ON MethodForAnalysis TO [Administrator]
GO

GRANT INSERT ON MethodForAnalysis TO [Administrator]
GO

GRANT DELETE ON MethodForAnalysis TO [Administrator]
GO


--#####################################################################################################################
--######   [MethodForProcessing]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[MethodForProcessing](
	[ProcessingID] [int] NOT NULL,
	[MethodID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_MethodForProcessing] PRIMARY KEY CLUSTERED 
(
	[ProcessingID] ASC,
	[MethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the processing (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'ProcessingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the Method (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'MethodID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Methods available for a processing' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MethodForProcessing'
GO

ALTER TABLE [dbo].[MethodForProcessing]  WITH CHECK ADD  CONSTRAINT [FK_MethodForProcessing_Method] FOREIGN KEY([MethodID])
REFERENCES [dbo].[Method] ([MethodID])
GO

ALTER TABLE [dbo].[MethodForProcessing] CHECK CONSTRAINT [FK_MethodForProcessing_Method]
GO

ALTER TABLE [dbo].[MethodForProcessing]  WITH CHECK ADD  CONSTRAINT [FK_MethodForProcessing_Processing] FOREIGN KEY([ProcessingID])
REFERENCES [dbo].[Processing] ([ProcessingID])
GO

ALTER TABLE [dbo].[MethodForProcessing] CHECK CONSTRAINT [FK_MethodForProcessing_Processing]
GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[MethodForProcessing] ADD  CONSTRAINT [DF_MethodForProcessing_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

GRANT SELECT ON [MethodForProcessing] TO [User]
GO

GRANT UPDATE ON [MethodForProcessing] TO [Administrator]
GO

GRANT INSERT ON [MethodForProcessing] TO [Administrator]
GO

GRANT DELETE ON [MethodForProcessing] TO [Administrator]
GO

--#####################################################################################################################
--######   CacheDB   ######################################################################################
--#####################################################################################################################

CREATE ROLE [CacheUser] AUTHORIZATION [dbo]
GO

GRANT SELECT ON [CacheDB_CollectionAgent] TO [CacheUser]
GO

GRANT SELECT ON [CacheDB_CollectionEvent] TO [CacheUser]
GO

GRANT SELECT ON [CacheDB_CollectionEventChronostratigraphy] TO [CacheUser]
GO

GRANT SELECT ON [CacheDB_CollectionEventLithostratigraphy] TO [CacheUser]
GO

GRANT SELECT ON [CacheDB_CollectionProject] TO [CacheUser]
GO

GRANT SELECT ON [CacheDB_Identification_Last] TO [CacheUser]
GO

GRANT SELECT ON [CacheDB_Identification_First] TO [CacheUser]
GO

GRANT SELECT ON [CacheDB_Identification_FirstEntry] TO [CacheUser]
GO

--#####################################################################################################################
--######   [CollectionHierarchy]   ######################################################################################
--#####################################################################################################################


/****** Object:  UserDefinedFunction [dbo].[CollectionHierarchy]    Script Date: 07/08/2013 17:07:22 ******/

ALTER FUNCTION [dbo].[CollectionHierarchy] (@CollectionID int)  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint]  NULL)

/*
Returns a table that lists all the analysis items related to the given analysis.
MW 02.01.2006
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int

set @TopID = (select CollectionParentID from Collection where CollectionID = @CollectionID) 

set @i = (select count(*) from Collection where CollectionID = @CollectionID)

if (@TopID is null )
	set @TopID =  @CollectionID
else	
	begin
	while (@i > 0)
		begin
		set @CollectionID = (select CollectionParentID from Collection where CollectionID = @CollectionID and not CollectionParentID is null) 
		set @i = (select count(*) from Collection where CollectionID = @CollectionID and not CollectionParentID is null)
		end
	set @TopID = @CollectionID
	end

-- copy the root node in the result list

   INSERT @CollectionList
   SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
   FROM Collection
   WHERE Collection.CollectionID = @TopID

-- copy the child nodes into the result list
  INSERT @CollectionList
   SELECT * FROM dbo.CollectionChildNodes (@TopID)

   RETURN
END

GO


/****** Object:  UserDefinedFunction [dbo].[CollectionChildNodes]    Script Date: 07/08/2013 17:07:39 ******/

ALTER FUNCTION [dbo].[CollectionChildNodes] (@ID int)  
RETURNS @ItemList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint] NULL)

INSERT @TempItem (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder) 
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
	FROM Collection WHERE CollectionParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select CollectionID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
	from dbo.CollectionChildNodes (@ParentID) where CollectionID not in (select CollectionID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder) 
   SELECT distinct CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
   FROM @TempItem ORDER BY CollectionName
   RETURN
END

GO

--#####################################################################################################################
--######   [ToolForAnalysis]   ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ToolForAnalysis](
	[AnalysisID] [int] NOT NULL,
	[ToolID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_ToolForAnalysis] PRIMARY KEY CLUSTERED 
(
	[AnalysisID] ASC,
	[ToolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the analysis (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForAnalysis', @level2type=N'COLUMN',@level2name=N'AnalysisID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the analysis tool (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForAnalysis', @level2type=N'COLUMN',@level2name=N'ToolID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForAnalysis', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForAnalysis', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForAnalysis', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForAnalysis', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tools available for an analysis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForAnalysis'
GO

ALTER TABLE [dbo].[ToolForAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_ToolForAnalysis_Analysis] FOREIGN KEY([AnalysisID])
REFERENCES [dbo].[Analysis] ([AnalysisID])
GO

ALTER TABLE [dbo].[ToolForAnalysis] CHECK CONSTRAINT [FK_ToolForAnalysis_Analysis]
GO

ALTER TABLE [dbo].[ToolForAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_ToolForAnalysis_Tool] FOREIGN KEY([ToolID])
REFERENCES [dbo].[Tool] ([ToolID])
GO

ALTER TABLE [dbo].[ToolForAnalysis] CHECK CONSTRAINT [FK_ToolForAnalysis_Tool]
GO

ALTER TABLE [dbo].[ToolForAnalysis] ADD  CONSTRAINT [DF_ToolForAnalysis_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[ToolForAnalysis] ADD  CONSTRAINT [DF_ToolForAnalysis_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[ToolForAnalysis] ADD  CONSTRAINT [DF_ToolForAnalysis_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ToolForAnalysis] ADD  CONSTRAINT [DF_ToolForAnalysis_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ToolForAnalysis] ADD  CONSTRAINT [DF_ToolForAnalysis_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT SELECT ON ToolForAnalysis TO [USER]
GO

GRANT UPDATE ON ToolForAnalysis TO [Administrator]
GO

GRANT INSERT ON ToolForAnalysis TO [Administrator]
GO

GRANT DELETE ON ToolForAnalysis TO [Administrator]
GO

--#####################################################################################################################
--######   [ToolForProcessing]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ToolForProcessing](
	[ProcessingID] [int] NOT NULL,
	[ToolID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_ToolForProcessing] PRIMARY KEY CLUSTERED 
(
	[ProcessingID] ASC,
	[ToolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the processing (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForProcessing', @level2type=N'COLUMN',@level2name=N'ProcessingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the processing tool (Part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForProcessing', @level2type=N'COLUMN',@level2name=N'ToolID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForProcessing', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForProcessing', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForProcessing', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForProcessing', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tools available for a processing' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ToolForProcessing'
GO

ALTER TABLE [dbo].[ToolForProcessing]  WITH CHECK ADD  CONSTRAINT [FK_ToolForProcessing_Processing] FOREIGN KEY([ProcessingID])
REFERENCES [dbo].[Processing] ([ProcessingID])
GO

ALTER TABLE [dbo].[ToolForProcessing] CHECK CONSTRAINT [FK_ToolForProcessing_Processing]
GO

ALTER TABLE [dbo].[ToolForProcessing]  WITH CHECK ADD  CONSTRAINT [FK_ToolForProcessing_Tool] FOREIGN KEY([ToolID])
REFERENCES [dbo].[Tool] ([ToolID])
GO

ALTER TABLE [dbo].[ToolForProcessing] CHECK CONSTRAINT [FK_ToolForProcessing_Tool]
GO

ALTER TABLE [dbo].[ToolForProcessing] ADD  CONSTRAINT [DF_ToolForProcessing_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[ToolForProcessing] ADD  CONSTRAINT [DF_ToolForProcessing_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[ToolForProcessing] ADD  CONSTRAINT [DF_ToolForProcessing_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ToolForProcessing] ADD  CONSTRAINT [DF_ToolForProcessing_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ToolForProcessing] ADD  CONSTRAINT [DF_ToolForProcessing_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT SELECT ON ToolForProcessing TO [USER]
GO

GRANT UPDATE ON ToolForProcessing TO [Administrator]
GO

GRANT INSERT ON ToolForProcessing TO [Administrator]
GO

GRANT DELETE ON ToolForProcessing TO [Administrator]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.40'
END

GO



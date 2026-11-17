
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.01'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   [trgSamplingPlotLocalisation]   ######################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgSamplingPlotLocalisation] 
   ON  [dbo].[SamplingPlotLocalisation] 
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
/* updating the AverageAltitudeCache column */
Update SamplingPlotLocalisation
set [AverageAltitudeCache] = 
(case when isnumeric(Location1) = 1 then cast(replace(Location1, ',', '.') as real) else 0 end +
 case when isnumeric(Location2) = 1 then cast(replace(Location2, ',', '.') as real) else 0 end)
 / (case when isnumeric(Location1) = 1 then 1 else 0 end +
 case when isnumeric(Location2) = 1 then 1 else 0 end)
FROM SamplingPlotLocalisation 
where 1 = 1 
AND (isnumeric(Location1) = 1 OR Location1 is null)
AND (isnumeric(Location2) = 1 OR Location2 is null)
AND SamplingPlotLocalisation.AverageAltitudeCache is null
AND LocalisationSystemID in (4)   
   


/* updating the AverageCache columns */
Update SamplingPlotLocalisation
set [AverageLongitudeCache] = cast(replace(inserted.Location1, ',', '.') as varchar)
, AverageLatitudeCache = cast(replace(inserted.Location2, ',', '.') as varchar)
FROM SamplingPlotLocalisation, inserted 
where 1 = 1 
AND isnumeric(inserted.Location1) = 1
AND isnumeric(inserted.Location2) = 1
AND SamplingPlotLocalisation.AverageLatitudeCache is null
AND SamplingPlotLocalisation.AverageLongitudeCache is null
AND inserted.LocalisationSystemID in (8)   
   
   
/* updating the geography column */
Update SamplingPlotLocalisation
set Geography = 
case when inserted.Geography IS null then 
	geography::STPointFromText('POINT(' + replace(cast(inserted.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(inserted.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
else inserted.Geography end
FROM SamplingPlotLocalisation, inserted 
where 1 = 1 
AND SamplingPlotLocalisation.PlotID = inserted.PlotID
AND SamplingPlotLocalisation.LocalisationSystemID = inserted.LocalisationSystemID
AND inserted.AverageLatitudeCache between -90 and 90
AND inserted.AverageLongitudeCache between -180 and 180
AND (SamplingPlotLocalisation.Geography.ToString() <> inserted.Geography.ToString()
OR SamplingPlotLocalisation.Geography IS NULL)


Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P, inserted
where 1 = 1 
AND L.PlotID = inserted.PlotID
AND L.LocalisationSystemID = inserted.LocalisationSystemID
AND P.PlotID = L.PlotID
AND P.PlotGeography_Cache IS NULL
AND NOT L.Geography IS NULL

END

GO


--#####################################################################################################################
--######   CountryCache   ######################################################################################
--#####################################################################################################################
declare @i int
set @i = (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C
where C.COLUMN_NAME = 'CountryCache' and C.TABLE_NAME = 'SamplingPlot')

if (@i = 0)
begin

ALTER TABLE dbo.SamplingPlot ADD [CountryCache] [nvarchar](50) NULL
end

if (@i = 0)
begin
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The country where the collection sampling plot is located. Cached value derived from an entry in SamplingPlotLocalisation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlot', @level2type=N'COLUMN',@level2name=N'CountryCache'
end
go
--#####################################################################################################################
--######   SamplingPlot_log   ######################################################################################
--#####################################################################################################################

declare @i int
set @i = (select COUNT(*) from INFORMATION_SCHEMA.TABLES T
where T.TABLE_NAME = 'SamplingPlot_log')


if (@i = 0)
begin
CREATE TABLE [dbo].[SamplingPlot_log](
	[PlotID] [int] NULL,
	[PartOfPlotID] [int] NULL,
	[PlotIdentifier] [nvarchar](500) NULL,
	[PlotGeography_Cache] [geography] NULL,
	[PlotGeometry_Cache] [geometry] NULL,
	[PlotDescription] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[PlotType] [nvarchar](50) NULL,
	[CountryCache] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SamplingPlot_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[SamplingPlot_log] ADD  CONSTRAINT [DF_SamplingPlot_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[SamplingPlot_log] ADD  CONSTRAINT [DF_SamplingPlot_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[SamplingPlot_log] ADD  CONSTRAINT [DF_SamplingPlot_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO

--#####################################################################################################################
--######   trgDelSamplingPlot   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelSamplingPlot] ON [dbo].[SamplingPlot] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 20.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, PlotType, CountryCache,  LogState) 
SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.PlotType, deleted.CountryCache,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   trgUpdSamplingPlot   ######################################################################################
--#####################################################################################################################



CREATE TRIGGER [dbo].[trgUpdSamplingPlot] ON [dbo].[SamplingPlot] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 20.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlot_Log (PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, PlotType, CountryCache,  LogState) 
SELECT deleted.PlotID, deleted.PartOfPlotID, deleted.PlotIdentifier, deleted.PlotGeography_Cache, deleted.PlotGeometry_Cache, deleted.PlotDescription, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.PlotType, deleted.CountryCache,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlot
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlot, deleted 
where 1 = 1 
AND SamplingPlot.PlotID = deleted.PlotID
GO

--#####################################################################################################################
--######   SamplingPlotLocalisation_log   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[SamplingPlotLocalisation_log](
	[PlotID] [int] NULL,
	[LocalisationSystemID] [int] NULL,
	[Location1] [nvarchar](255) NULL,
	[Location2] [nvarchar](255) NULL,
	[LocationAccuracy] [nvarchar](50) NULL,
	[LocationNotes] [nvarchar](max) NULL,
	[DeterminationDate] [smalldatetime] NULL,
	[DistanceToLocation] [varchar](50) NULL,
	[DirectionToLocation] [varchar](50) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Geography] [geography] NULL,
	[AverageAltitudeCache] [float] NULL,
	[AverageLatitudeCache] [float] NULL,
	[AverageLongitudeCache] [float] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SamplingPlotLocalisation_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[SamplingPlotLocalisation_log] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[SamplingPlotLocalisation_log] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[SamplingPlotLocalisation_log] ADD  CONSTRAINT [DF_SamplingPlotLocalisation_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


--#####################################################################################################################
--######   trgDelSamplingPlotLocalisation   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 20.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   trgUpdSamplingPlotLocalisation   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgUpdSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 20.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotLocalisation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotLocalisation, deleted 
where 1 = 1 
AND SamplingPlotLocalisation.LocalisationSystemID = deleted.LocalisationSystemID
AND SamplingPlotLocalisation.PlotID = deleted.PlotID
GO


--#####################################################################################################################
--######   SamplingPlotProperty   ######################################################################################
--#####################################################################################################################

ALTER TABLE SamplingPlotProperty ADD [PropertyURI] [varchar](255) NOT NULL
GO
ALTER TABLE SamplingPlotProperty ADD [DisplayText] [nvarchar](255) NULL
GO
ALTER TABLE SamplingPlotProperty ADD [PropertyHierarchyCache] [nvarchar](max) NULL
GO
ALTER TABLE SamplingPlotProperty ADD [PropertyValue] [nvarchar](255) NULL
GO
ALTER TABLE SamplingPlotProperty ADD [ResponsibleName] [nvarchar](255) NULL
GO
ALTER TABLE SamplingPlotProperty ADD [ResponsibleAgentURI] [varchar](255) NULL
GO
ALTER TABLE SamplingPlotProperty ADD [Notes] [nvarchar](max) NULL
GO
ALTER TABLE SamplingPlotProperty ADD [AverageValueCache] [float] NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to an external datasource e.g. DiversityScientificTerms' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The text for the property as shown e.g. in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A cached text of the complete name of the descriptor including superior categories if present' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyHierarchyCache'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of a captured feature  e.g. temperature, pH, vegetation etc. If there is a range this is the lower or first value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent (person or organization) responsible for this entry.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'ResponsibleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or organisation responsible for the data (see e.g. module DiversityAgents)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the property of the colletion site.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'For numeric values - a cached average value according to the' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'AverageValueCache'
GO



--#####################################################################################################################
--######   SamplingPlotProperty_log   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[SamplingPlotProperty_log](
	[PlotID] [int] NULL,
	[PropertyID] [int] NULL,
	[PropertyURI] [varchar](255) NULL,
	[DisplayText] [nvarchar](255) NULL,
	[PropertyHierarchyCache] [nvarchar](max) NULL,
	[PropertyValue] [nvarchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[AverageValueCache] [float] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SamplingPlotProperty_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[SamplingPlotProperty_log] ADD  CONSTRAINT [DF_SamplingPlotProperty_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[SamplingPlotProperty_log] ADD  CONSTRAINT [DF_SamplingPlotProperty_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[SamplingPlotProperty_log] ADD  CONSTRAINT [DF_SamplingPlotProperty_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


--#####################################################################################################################
--######   trgDelSamplingPlotProperty   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 20.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotProperty_Log (PlotID, PropertyID, PropertyURI, DisplayText, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.PlotID, deleted.PropertyID, deleted.PropertyURI, deleted.DisplayText, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   trgUpdSamplingPlotProperty   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 20.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotProperty_Log (PlotID, PropertyID, PropertyURI, DisplayText, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.PlotID, deleted.PropertyID, deleted.PropertyURI, deleted.DisplayText, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotProperty
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotProperty, deleted 
where 1 = 1 
AND SamplingPlotProperty.PlotID = deleted.PlotID
AND SamplingPlotProperty.PropertyID = deleted.PropertyID
AND SamplingPlotProperty.PropertyURI = deleted.PropertyURI
GO

--#####################################################################################################################
--######   SamplingPlotResource_log   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[SamplingPlotResource_log](
	[PlotID] [int] NULL,
	[ResourceURI] [varchar](255) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SamplingPlotResource_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[SamplingPlotResource_log] ADD  CONSTRAINT [DF_SamplingPlotResource_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[SamplingPlotResource_log] ADD  CONSTRAINT [DF_SamplingPlotResource_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[SamplingPlotResource_log] ADD  CONSTRAINT [DF_SamplingPlotResource_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


--#####################################################################################################################
--######   trgDelSamplingPlotResource   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 20.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   trgUpdSamplingPlotResource   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgUpdSamplingPlotResource] ON [dbo].[SamplingPlotResource] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 20.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotResource_Log (PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.PlotID, deleted.ResourceURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update SamplingPlotResource
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotResource, deleted 
where 1 = 1 
AND SamplingPlotResource.PlotID = deleted.PlotID
AND SamplingPlotResource.ResourceURI = deleted.ResourceURI
GO


--#####################################################################################################################
--######   SamplingPlotGeoInfo   ######################################################################################
--#####################################################################################################################


CREATE VIEW [dbo].[SamplingPlotGeoInfo]
AS
SELECT     CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN P4.PlotGeography_Cache.EnvelopeCenter().Lat ELSE P3.PlotGeography_Cache.EnvelopeCenter().Lat END ELSE P2.PlotGeography_Cache.EnvelopeCenter().Lat END
                       ELSE P1.PlotGeography_Cache.EnvelopeCenter().Lat END ELSE S.PlotGeography_Cache.EnvelopeCenter().Lat END AS Latitude, 
                      CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN P4.PlotGeography_Cache.EnvelopeCenter().Long ELSE P3.PlotGeography_Cache.EnvelopeCenter().Long END ELSE P2.PlotGeography_Cache.EnvelopeCenter().Long
                       END ELSE P1.PlotGeography_Cache.EnvelopeCenter().Long END ELSE S.PlotGeography_Cache.EnvelopeCenter().Long END AS Longitude, 
                      CAST(CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P3.PlotGeography_Cache IS NULL THEN P4.PlotGeography_Cache.EnvelopeAngle() ELSE P3.PlotGeography_Cache.EnvelopeAngle() 
                      END ELSE P2.PlotGeography_Cache.EnvelopeAngle() END ELSE P1.PlotGeography_Cache.EnvelopeAngle() END ELSE S.PlotGeography_Cache.EnvelopeAngle() 
                      END AS varchar) + ' °' AS [Accuracy angle], CASE WHEN S.PlotGeography_Cache IS NULL THEN CASE WHEN P1.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL THEN P4.PlotGeography_Cache.EnvelopeAngle() 
                      ELSE P3.PlotGeography_Cache.EnvelopeAngle() END ELSE P2.PlotGeography_Cache.EnvelopeAngle() END ELSE P1.PlotGeography_Cache.EnvelopeAngle() 
                      END ELSE S.PlotGeography_Cache.EnvelopeAngle() END AS AccuracyAngle, CASE WHEN S.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN P4.PlotGeography_Cache.STArea() ELSE P3.PlotGeography_Cache.STArea() END ELSE P2.PlotGeography_Cache.STArea() 
                      END ELSE P1.PlotGeography_Cache.STArea() END ELSE S.PlotGeography_Cache.STArea() END AS AreaM2, CASE WHEN S.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P4.PlotGeography_Cache.STArea() > 1000000 THEN CAST(P4.PlotGeography_Cache.STArea() / 1000000 AS Varchar) 
                      + ' km2' ELSE CAST(P4.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END ELSE CASE WHEN P3.PlotGeography_Cache.STArea() 
                      > 1000000 THEN CAST(P3.PlotGeography_Cache.STArea() / 1000000 AS Varchar) + ' km2' ELSE CAST(P3.PlotGeography_Cache.STArea() AS Varchar) 
                      + ' m2' END END ELSE CASE WHEN P2.PlotGeography_Cache.STArea() > 1000000 THEN CAST(P2.PlotGeography_Cache.STArea() / 1000000 AS Varchar) 
                      + ' km2' ELSE CAST(P2.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END END ELSE CASE WHEN P1.PlotGeography_Cache.STArea() 
                      > 1000000 THEN CAST(P1.PlotGeography_Cache.STArea() / 1000000 AS Varchar) + ' km2' ELSE CAST(P1.PlotGeography_Cache.STArea() AS Varchar) 
                      + ' m2' END END ELSE CASE WHEN S.PlotGeography_Cache.STArea() > 1000000 THEN CAST(S.PlotGeography_Cache.STArea() / 1000000 AS Varchar) 
                      + ' km2' ELSE CAST(S.PlotGeography_Cache.STArea() AS Varchar) + ' m2' END END AS Area, CASE WHEN S.PlotGeography_Cache IS NULL 
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL 
                      THEN P4.PlotGeography_Cache.ToString() ELSE P3.PlotGeography_Cache.ToString() END ELSE P2.PlotGeography_Cache.ToString() 
                      END ELSE P1.PlotGeography_Cache.ToString() END ELSE S.PlotGeography_Cache.ToString() END AS Geography, CASE WHEN S.CountryCache IS NULL 
                      THEN CASE WHEN P1.CountryCache IS NULL THEN CASE WHEN P2.CountryCache IS NULL THEN CASE WHEN P3.CountryCache IS NULL 
                      THEN P4.CountryCache ELSE P3.CountryCache END ELSE P2.CountryCache END ELSE P1.CountryCache END ELSE S.CountryCache END AS CountryCache, 
                      S.PlotID
FROM         dbo.SamplingPlot AS P4 RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P3 ON P4.PlotID = P3.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P2 ON P3.PlotID = P2.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P1 ON P2.PlotID = P1.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS S ON P1.PlotID = S.PartOfPlotID

GO

GRANT SELECT ON SamplingPlotGeoInfo TO DiversityWorkbenchUser
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.02'
END

GO


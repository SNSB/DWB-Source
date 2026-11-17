
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.25'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   update for Database   ######################################################################################
--#####################################################################################################################

--### Add 4 Gazetteers ### 

UPDATE P 
SET P.LocalisationSystemName = 'Named area (DiversityGazetteers)', P.DisplayText = 'Named area (DiversityGazetteers)'
FROM LocalisationSystem P 
WHERE P.LocalisationSystemID = 7
UPDATE P 
SET P.LocalisationSystemName = 'Named area 2 (DiversityGazetteers)', P.DisplayText = 'Named area 2 (DiversityGazetteers)' 
FROM LocalisationSystem P 
WHERE P.LocalisationSystemID = 18
UPDATE P 
SET P.LocalisationSystemName = 'Named area 3 (DiversityGazetteers)', P.DisplayText = 'Named area 3 (DiversityGazetteers)' 
FROM LocalisationSystem P 
WHERE P.LocalisationSystemID = 19
UPDATE P 
SET P.LocalisationSystemName = 'Named area 4 (DiversityGazetteers)', P.DisplayText = 'Named area 4 (DiversityGazetteers)' 
FROM LocalisationSystem P 
WHERE P.LocalisationSystemID = 20
UPDATE P 
SET P.LocalisationSystemName = 'Named area 5 (DiversityGazetteers)', P.DisplayText = 'Named area 5 (DiversityGazetteers)' 
FROM LocalisationSystem P 
WHERE P.LocalisationSystemID = 21

declare @Test int;
set @Test = (SELECT COUNT (*) from [LocalisationSystem] L where L.LocalisationSystemName like 'Named area % (DiversityGazetteers)')
if (@Test = 0)
BEGIN
	declare @Pos int;
	set @Pos = 2;
	while (select @Pos) < 6
	begin
	INSERT INTO [LocalisationSystem]
			   ([LocalisationSystemID]
			   ,[LocalisationSystemParentID]
			   ,[LocalisationSystemName]
			   ,[DefaultAccuracyOfLocalisation]
			   ,[DefaultMeasurementUnit]
			   ,[ParsingMethodName]
			   ,[DisplayText]
			   ,[DisplayEnable]
			   ,[DisplayOrder]
			   ,[Description]
			   ,[DisplayTextLocation1]
			   ,[DescriptionLocation1]
			   ,[DisplayTextLocation2]
			   ,[DescriptionLocation2])
	SELECT 16 + @Pos
		  ,7
		  ,cast(@Pos as CHAR(1)) + '. ' + [LocalisationSystemName]
		  ,[DefaultAccuracyOfLocalisation]
		  ,[DefaultMeasurementUnit]
		  ,[ParsingMethodName]
		  ,cast(@Pos as CHAR(1)) + '. ' + [DisplayText]
		  ,[DisplayEnable]
		  ,[DisplayOrder] - 1 + @Pos
		  ,cast(@Pos as CHAR(1)) + '. ' + [Description]
		  ,[DisplayTextLocation1]
		  ,[DescriptionLocation1]
		  ,[DisplayTextLocation2]
		  ,[DescriptionLocation2]
	  FROM [LocalisationSystem] L
	  WHERE L.LocalisationSystemID = 7

	  set @Pos = @Pos + 1;
	end  
END
GO

--### Insert missing Table [dbo].[SamplingPlotResource] ###

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'SamplingPlotResource') = 0
begin

CREATE TABLE [dbo].[SamplingPlotResource](
	[PlotID] [int] NOT NULL,
	[ResourceURI] [varchar](255) NOT NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SamplingPlotResource] PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[ResourceURI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[SamplingPlotResource] ADD  CONSTRAINT [DF_SamplingPlotResource_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]

ALTER TABLE [dbo].[SamplingPlotResource] ADD  CONSTRAINT [DF_SamplingPlotResource_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]

ALTER TABLE [dbo].[SamplingPlotResource] ADD  CONSTRAINT [DF_SamplingPlotResource_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]

ALTER TABLE [dbo].[SamplingPlotResource] ADD  CONSTRAINT [DF_SamplingPlotResource_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]

ALTER TABLE [dbo].[SamplingPlotResource] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]

ALTER TABLE [dbo].[SamplingPlotResource]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotResource_SamplingPlot] FOREIGN KEY([PlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[SamplingPlotResource] CHECK CONSTRAINT [FK_SamplingPlotResource_SamplingPlot]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the sampling plot, foreign key and part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'PlotID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to the file or link of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'ResourceURI'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent (person or organization) responsible for this entry.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'ResponsibleName'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or organisation responsible for the data (see e.g. module DiversityAgents)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the resource.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'Notes'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotResource', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'

end

GO


-- delete constraint in SamplingPlot, if any --
if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R where r.CONSTRAINT_NAME = 'FK_SamplingPlot_SamplingPlotPointType_Enum') > 0
begin
ALTER TABLE [dbo].[SamplingPlot] DROP CONSTRAINT [FK_SamplingPlot_SamplingPlotPointType_Enum]
end
go 

-- delete constraint in SamplingPlotPoint, if any --
if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R where r.CONSTRAINT_NAME = 'FK_SamplingPlotPoint_SamplingPlotPointType_Enum') > 0
begin
ALTER TABLE [dbo].[SamplingPlotPoint] DROP CONSTRAINT [FK_SamplingPlotPoint_SamplingPlotPointType_Enum]
end
go 

--### Delete obsolete Tables ###

begin try
drop table [dbo].[SamplingPlotImage]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotImage_log]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[Measurement]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingObject]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingObjectInPlot]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotMeasurement]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotMeasurement_log]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotPoint]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotPointInImage]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotPointType_Enum]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotResource_RowGUID]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_TK25_in_WGS84]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_TK25_Quadrant]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_TK25Namen]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[zz_TK25_fin]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[zz_TK25_Koordinaten]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[zz_TK25mitQuadranten]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_GEOA]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_LOCALITYData]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_NAT]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_Plot_Unique]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_Plot_Unique_neu_20130409]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_Plot_Unique_neu_20130412]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_Plot_Unique_UC]
end try
begin catch
end catch 
go

-- ### drop again in case that there has been dependencies

begin try
drop table [dbo].[SamplingPlotImage]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotImage_log]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[Measurement]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingObject]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingObjectInPlot]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotMeasurement]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotMeasurement_log]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotPoint]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotPointInImage]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotPointType_Enum]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[SamplingPlotResource_RowGUID]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_TK25_in_WGS84]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_TK25_Quadrant]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_TK25Namen]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[zz_TK25_fin]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[zz_TK25_Koordinaten]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[zz_TK25mitQuadranten]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_GEOA]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_LOCALITYData]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_NAT]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_Plot_Unique]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_Plot_Unique_neu_20130409]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_Plot_Unique_neu_20130412]
end try
begin catch
end catch 
go

begin try
drop table [dbo].[xx_Plot_Unique_UC]
end try
begin catch
end catch 
go

-- ### delete obsolete functions ###

begin try
drop function [dbo].[SamplingPlotListWithProject]
end try
begin catch
end catch 
go

begin try
drop function [dbo].[SamplingPlotsHierarchyAll]
end try
begin catch
end catch 
go

begin try
drop function [dbo].[SamplingPlotsPropertyAll]
end try
begin catch
end catch 
go

begin try
drop function [dbo].[SamplingPlotListForCurrentProject_old]
end try
begin catch
end catch 
go

begin try
drop function [dbo].[getAccuracyOfGeographyOfPlot]
end try
begin catch
end catch 
go

begin try
drop function [dbo].[getGeographyForPlotFromPoints]
end try
begin catch
end catch 
go

begin try
drop function [dbo].[TopPlotID]

end try
begin catch
end catch 
go

-- ### delete obsolete views ###

begin try
drop view [dbo].[SamplingPlot_Core]
end try
begin catch
end catch 
go

begin try
drop view [dbo].[TK25_Quadrant]
end try
begin catch
end catch 
go

begin try
drop view [dbo].[TK25_Quadrant_1]
end try
begin catch
end catch 
go

begin try
drop view [dbo].[TK25_Quadrant_2]
end try
begin catch
end catch 
go

begin try
drop view [dbo].[TK25_Quadrant_3]
end try
begin catch
end catch 
go

begin try
drop view [dbo].[TK25_Quadrant_4]
end try
begin catch
end catch 
go

begin try
drop view [dbo].[TK25_QuadrantPoints]
end try
begin catch
end catch 
go

begin try
drop view [dbo].[zz_TK25mitQuadrantenGeography]
end try
begin catch
end catch 
go

--#################################################
--###  ViewSamplingPlot ###########################
--#################################################

-- ### add if not yet exist ###


declare @SQL nvarchar(max)

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlot]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache,
PlotDescription, InternalNotes, CountryCache, PlotType
FROM            dbo.SamplingPlot')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO 

declare @SQL nvarchar(max)

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotHierarchy1]
AS
SELECT        dbo.SamplingPlot.PlotID, 
						 SamplingPlot_1.PlotIdentifier + '' | '' + 
						 dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID
WHERE        (SamplingPlot_1.PartOfPlotID IS NULL)')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

declare @SQL nvarchar(max)

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotHierarchy2]
AS
SELECT        dbo.SamplingPlot.PlotID, 
						 SamplingPlot_2.PlotIdentifier + '' | '' + 
						 SamplingPlot_1.PlotIdentifier + '' | '' + 
						 dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_2 ON SamplingPlot_1.PartOfPlotID = SamplingPlot_2.PlotID
WHERE        (SamplingPlot_2.PartOfPlotID IS NULL)')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

declare @SQL nvarchar(max)

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotHierarchy3]
AS
SELECT        dbo.SamplingPlot.PlotID,
						 SamplingPlot_3.PlotIdentifier + '' | '' +  
						 SamplingPlot_2.PlotIdentifier + '' | '' + 
						 SamplingPlot_1.PlotIdentifier + '' | '' + 
                         dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_2 ON SamplingPlot_1.PartOfPlotID = SamplingPlot_2.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_3 ON SamplingPlot_2.PartOfPlotID = SamplingPlot_3.PlotID
WHERE        (SamplingPlot_3.PartOfPlotID IS NULL)')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

declare @SQL nvarchar(max)

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotHierarchy4]
AS
SELECT        dbo.SamplingPlot.PlotID,
						 SamplingPlot_4.PlotIdentifier + '' | '' +  
						 SamplingPlot_3.PlotIdentifier + '' | '' + 
						 SamplingPlot_2.PlotIdentifier + '' | '' + 
						 SamplingPlot_1.PlotIdentifier + '' | '' + 
                         dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_2 ON SamplingPlot_1.PartOfPlotID = SamplingPlot_2.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_3 ON SamplingPlot_2.PartOfPlotID = SamplingPlot_3.PlotID  INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_4 ON SamplingPlot_3.PartOfPlotID = SamplingPlot_4.PlotID
WHERE        (SamplingPlot_4.PartOfPlotID IS NULL)')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

declare @SQL nvarchar(max)

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotHierarchy5]
AS
SELECT        dbo.SamplingPlot.PlotID,
						 SamplingPlot_5.PlotIdentifier + '' | '' + 
						 SamplingPlot_4.PlotIdentifier + '' | '' + 
						 SamplingPlot_3.PlotIdentifier + '' | '' + 
						 SamplingPlot_2.PlotIdentifier + '' | '' + 
						 SamplingPlot_1.PlotIdentifier + '' | '' + 
                         dbo.SamplingPlot.PlotIdentifier AS DisplayText
FROM            dbo.SamplingPlot INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_1 ON dbo.SamplingPlot.PartOfPlotID = SamplingPlot_1.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_2 ON SamplingPlot_1.PartOfPlotID = SamplingPlot_2.PlotID INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_3 ON SamplingPlot_2.PartOfPlotID = SamplingPlot_3.PlotID  INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_4 ON SamplingPlot_3.PartOfPlotID = SamplingPlot_4.PlotID   INNER JOIN
                         dbo.SamplingPlot AS SamplingPlot_5 ON SamplingPlot_4.PartOfPlotID = SamplingPlot_5.PlotID
WHERE        (SamplingPlot_5.PartOfPlotID IS NULL)')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

declare @SQL nvarchar(max)

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotHierarchy]
AS

select PlotID, PlotIdentifier as DisplayText from SamplingPlot s where s.PartOfPlotID is null
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy1
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy2
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy3
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy4
union
SELECT        *
FROM            dbo.ViewSamplingPlotHierarchy5')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

 
GRANT SELECT ON ViewSamplingPlotHierarchy TO [DiversityWorkbenchUser]
GO
GRANT SELECT ON ViewSamplingPlotHierarchy1 TO [DiversityWorkbenchUser]
GO
GRANT SELECT ON ViewSamplingPlotHierarchy2 TO [DiversityWorkbenchUser]
GO
GRANT SELECT ON ViewSamplingPlotHierarchy3 TO [DiversityWorkbenchUser]
GO
GRANT SELECT ON ViewSamplingPlotHierarchy4 TO [DiversityWorkbenchUser]
GO
GRANT SELECT ON ViewSamplingPlotHierarchy5 TO [DiversityWorkbenchUser]
GO

GRANT SELECT ON [ViewSamplingPlot] TO [DiversityWorkbenchUser]
GO 



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.08.02' 
END

GO

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.26'
END

GO

select [dbo].[Version] ()  

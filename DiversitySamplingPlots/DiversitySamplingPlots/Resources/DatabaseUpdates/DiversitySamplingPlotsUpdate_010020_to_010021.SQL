
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.20'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   [trgInsSamplingPlotLocalisation] ################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgInsSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR INSERT AS
/*
Setting the average coordinates if the entered values are numeric
*/
Update S
set S.[AverageLongitudeCache] = cast(replace(I.Location1, ',', '.') as varchar)
, S.AverageLatitudeCache = cast(replace(I.Location2, ',', '.') as varchar)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND isnumeric(I.Location1 + 'e0') = 1
AND isnumeric(I.Location2 + 'e0') = 1
AND I.LocalisationSystemID in (3,7,8)

/*
Setting the average coordinates if the entered values are numeric - second trial
*/
Update S
set S.[AverageLongitudeCache] = cast(replace(I.Location1, ',', '.') as float)
, S.AverageLatitudeCache = cast(replace(I.Location2, ',', '.') as float)
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND isnumeric(replace(I.Location1, ',', '.') + 'e0') = 1
AND isnumeric(replace(I.Location1, ',', '.') + 'e0') = 1
AND I.LocalisationSystemID in (3,7,8)


/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set Geography =
case when I.Geography IS null OR I.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+' ' +replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')', 4326)
else I.Geography end
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 180
AND (S.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
OR S.Geography IS NULL)

/*
Setting the cached geography of the plot if it is missing or for WGS84
*/
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P, inserted I
where 1 = 1
AND L.PlotID = I.PlotID
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL)

GO

--#####################################################################################################################
--######   [trgUpdSamplingPlotLocalisation] ################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdSamplingPlotLocalisation] ON [dbo].[SamplingPlotLocalisation]
FOR UPDATE AS
INSERT INTO SamplingPlotLocalisation_Log (PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState)
SELECT deleted.PlotID, deleted.LocalisationSystemID, deleted.Location1, deleted.Location2, deleted.LocationAccuracy, deleted.LocationNotes, deleted.DeterminationDate, deleted.DistanceToLocation, deleted.DirectionToLocation, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Geography, deleted.AverageAltitudeCache, deleted.AverageLatitudeCache, deleted.AverageLongitudeCache, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED
Update SamplingPlotLocalisation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM SamplingPlotLocalisation, deleted
where 1 = 1
AND SamplingPlotLocalisation.LocalisationSystemID = deleted.LocalisationSystemID
AND SamplingPlotLocalisation.PlotID = deleted.PlotID

/*
Setting the geography where average coordinates are available in within the correct range
*/
Update S
set Geography =
case when I.Geography IS null OR I.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
then geography::STPointFromText('POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+' ' +replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')', 4326)
else I.Geography end
FROM SamplingPlotLocalisation S, inserted I
where 1 = 1
AND S.PlotID = I.PlotID
AND S.LocalisationSystemID = I.LocalisationSystemID
AND I.AverageLatitudeCache between -90 and 90
AND I.AverageLongitudeCache between -180 and 180
AND (S.Geography.ToString() <> 'POINT(' + replace(cast(cast(I.[AverageLongitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+ ' ' + replace(cast(cast(I.[AverageLatitudeCache] as decimal(20,10)) as varchar(20)), ',', '.')+')'
OR S.Geography IS NULL)

/*
Setting the cached geography of the plot if it is missing or for WGS84
*/
Update P
set P.PlotGeography_Cache = L.Geography
FROM SamplingPlotLocalisation L, SamplingPlot P, inserted I
where 1 = 1
AND L.PlotID = I.PlotID
AND P.PlotID = L.PlotID
AND L.LocalisationSystemID = I.LocalisationSystemID
AND NOT L.Geography IS NULL
AND (P.PlotGeography_Cache IS NULL)

GO

--#################################################
--###  ViewSamplingPlot ###########################
--#################################################

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotListForCurrentProject]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache,
                         PlotDescription, InternalNotes, DisplayText
FROM            dbo.SamplingPlotListForCurrentProject() AS SamplingPlotListForCurrentProject')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


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

GRANT SELECT ON ViewSamplingPlotListForCurrentProject TO [DiversityWorkbenchUser]
GO


set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotList]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache, PlotGeometry_Cache, PlotDescription, InternalNotes, DisplayText
FROM            dbo.SamplingPlotList() AS ViewSamplingPlotList')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch
GO

GRANT SELECT ON [ViewSamplingPlotList] TO [DiversityWorkbenchUser]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.07.00' 
END

GO

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.21'
END

GO

select [dbo].[Version] ()  

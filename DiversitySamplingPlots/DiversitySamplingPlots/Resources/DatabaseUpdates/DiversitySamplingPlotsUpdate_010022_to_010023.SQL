
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.22'
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

declare @SQL nvarchar(max)

set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotList]
AS
SELECT        PlotID, PartOfPlotID, PlotIdentifier, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache,
                         PlotDescription, InternalNotes, DisplayText
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



set @SQL = (select 'CREATE VIEW [dbo].[ViewBaseURL]
    AS
    SELECT dbo.BaseURL() AS BaseURL')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch
GO

GRANT SELECT ON [ViewBaseURL] TO [DiversityWorkbenchUser]
GO 


set @SQL = (select 'CREATE VIEW [dbo].[ViewSamplingPlotGeoInfo]
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
                      + '' km2'' ELSE CAST(P4.PlotGeography_Cache.STArea() AS Varchar) + '' m2'' END ELSE CASE WHEN P3.PlotGeography_Cache.STArea()
                      > 1000000 THEN CAST(P3.PlotGeography_Cache.STArea() / 1000000 AS Varchar) + '' km2'' ELSE CAST(P3.PlotGeography_Cache.STArea() AS Varchar)
                      + '' m2'' END END ELSE CASE WHEN P2.PlotGeography_Cache.STArea() > 1000000 THEN CAST(P2.PlotGeography_Cache.STArea() / 1000000 AS Varchar)
                      + '' km2'' ELSE CAST(P2.PlotGeography_Cache.STArea() AS Varchar) + '' m2'' END END ELSE CASE WHEN P1.PlotGeography_Cache.STArea()
                      > 1000000 THEN CAST(P1.PlotGeography_Cache.STArea() / 1000000 AS Varchar) + '' km2'' ELSE CAST(P1.PlotGeography_Cache.STArea() AS Varchar)
                      + '' m2'' END END ELSE CASE WHEN S.PlotGeography_Cache.STArea() > 1000000 THEN CAST(S.PlotGeography_Cache.STArea() / 1000000 AS Varchar)
                      + '' km2'' ELSE CAST(S.PlotGeography_Cache.STArea() AS Varchar) + '' m2'' END END AS Area, CASE WHEN S.PlotGeography_Cache IS NULL
                      THEN CASE WHEN P1.PlotGeography_Cache IS NULL THEN CASE WHEN P2.PlotGeography_Cache IS NULL THEN CASE WHEN P3.PlotGeography_Cache IS NULL
                      THEN P4.PlotGeography_Cache.ToString() ELSE P3.PlotGeography_Cache.ToString() END ELSE P2.PlotGeography_Cache.ToString()
                      END ELSE P1.PlotGeography_Cache.ToString() END ELSE S.PlotGeography_Cache.ToString() END AS Geography, CASE WHEN S.CountryCache IS NULL
                      THEN CASE WHEN P1.CountryCache IS NULL THEN CASE WHEN P2.CountryCache IS NULL THEN CASE WHEN P3.CountryCache IS NULL
                      THEN P4.CountryCache ELSE P3.CountryCache END ELSE P2.CountryCache END ELSE P1.CountryCache END ELSE S.CountryCache END AS CountryCache,
                      S.PlotID, B.BaseURL + cast(S.PlotID as varchar) AS URL
FROM       [dbo].[ViewBaseURL] AS B,  dbo.SamplingPlot AS P4 RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P3 ON P4.PlotID = P3.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P2 ON P3.PlotID = P2.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS P1 ON P2.PlotID = P1.PartOfPlotID RIGHT OUTER JOIN
                      dbo.SamplingPlot AS S ON P1.PlotID = S.PartOfPlotID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch
GO


GRANT SELECT ON [ViewSamplingPlotGeoInfo] TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.08.00' 
END

GO

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.23'
END

GO

select [dbo].[Version] ()  

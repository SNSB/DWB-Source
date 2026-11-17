
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.38'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   DSGVO - Anpassung vorhandener Daten ########################################################################
--#####################################################################################################################

DISABLE TRIGGER trgUpdSamplingPlot ON [dbo].[SamplingPlot]
GO

UPDATE S SET [LogCreatedBy] = U.ID
  FROM [DiversitySamplingPlots_Test].[dbo].[SamplingPlot] S 
  INNER JOIN UserProxy U ON S.LogCreatedBy = U.LoginName;

UPDATE S SET [LogUpdatedBy] = U.ID
  FROM [DiversitySamplingPlots_Test].[dbo].[SamplingPlot] S 
  INNER JOIN UserProxy U ON S.LogUpdatedBy = U.LoginName;


ENABLE TRIGGER trgUpdSamplingPlot ON [dbo].[SamplingPlot]
GO

--#####################################################################################################################
--######   Anpassung der Trigger for Table SamplingPlotProperty     ##################################################
--#####################################################################################################################

IF(SELECT count(*)
FROM sys.triggers t
INNER JOIN sys.tables tb
    ON t.parent_id = tb.object_id
WHERE tb.name = 'SamplingPlotProperty'
  AND t.name = 'trgDelSamplingPlotProperty') = 0
 begin
	declare @SQL nvarchar(max)
	set @SQL = (select '
		CREATE TRIGGER [dbo].[trgDelSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
		FOR DELETE AS 
		/* saving the original dataset in the logging table */ 
		INSERT INTO SamplingPlotProperty_Log (PlotID, PropertyID, PropertyGeometry, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, PropertyURI, DisplayText, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID, LogUser,  LogState) 
		SELECT deleted.PlotID, deleted.PropertyID, deleted.PropertyGeometry, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.PropertyURI, deleted.DisplayText, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID, cast(dbo.UserID() as varchar),  ''D''
		FROM DELETED')

	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
	exec sp_executesql @SQL
	end catch
end


GO

IF(SELECT count(*)
FROM sys.triggers t
INNER JOIN sys.tables tb
    ON t.parent_id = tb.object_id
WHERE tb.name = 'SamplingPlotProperty'
  AND t.name = 'trgUpdSamplingPlotProperty') = 0
 begin
	declare @SQL nvarchar(max)
	set @SQL = (select '
		CREATE  TRIGGER [dbo].[trgUpdSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversitySamplingPlots  3.0.6.2 */ 
/*  Date: 11/27/2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO SamplingPlotProperty_Log (PlotID, PropertyID, PropertyGeometry, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, PropertyURI, DisplayText, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache, RowGUID, LogUser,  LogState) 
SELECT deleted.PlotID, deleted.PropertyID, deleted.PropertyGeometry, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.PropertyURI, deleted.DisplayText, deleted.PropertyHierarchyCache, deleted.PropertyValue, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.AverageValueCache, deleted.RowGUID, cast(dbo.UserID() as varchar),  ''U''
FROM DELETED


/* updating the logging columns */
Update SamplingPlotProperty
set LogUpdatedWhen = getdate(), LogUpdatedBy = cast(dbo.UserID() as varchar)
FROM SamplingPlotProperty, deleted 
where 1 = 1 
AND SamplingPlotProperty.PlotID = deleted.PlotID
AND SamplingPlotProperty.PropertyID = deleted.PropertyID')

	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 20000)
	exec sp_executesql @SQL
	end catch
end

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '04.06.00' 
END

GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.39'
END

GO

SELECT [dbo].[Version] ()  

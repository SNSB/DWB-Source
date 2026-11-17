
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.37'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Adding missing permissions #################################################################################
--#####################################################################################################################

GRANT SELECT ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchUser]
GO

GRANT SELECT ON [dbo].[Property] TO [DiversityWorkbenchUser]
GO

GRANT INSERT ON [dbo].[ProjectProxy] TO [DiversityWorkbenchAdministrator]
GO
GRANT DELETE ON [dbo].[ProjectProxy] TO [DiversityWorkbenchAdministrator]
GO
GRANT DELETE ON [dbo].[SamplingPlot_log] TO [DiversityWorkbenchAdministrator]
GO



--#####################################################################################################################
--######   Adding cascade delete for SamplingProject to SamplingPlot ##################################################
--#####################################################################################################################

ALTER TABLE [dbo].[SamplingProject] DROP CONSTRAINT [FK_SamplingProject_SamplingPlot]
GO

ALTER TABLE [dbo].[SamplingProject]  WITH CHECK ADD  CONSTRAINT [FK_SamplingProject_SamplingPlot] FOREIGN KEY([PlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[SamplingProject] CHECK CONSTRAINT [FK_SamplingProject_SamplingPlot]
GO

--#####################################################################################################################
--######   Adding cascade delete for SamplingPlotLocalisation to SamplingPlot #########################################
--#####################################################################################################################

ALTER TABLE [dbo].[SamplingPlotLocalisation] DROP CONSTRAINT [FK_SamplingPlotLocalisation_SamplingPlot]
GO

ALTER TABLE [dbo].[SamplingPlotLocalisation]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotLocalisation_SamplingPlot] FOREIGN KEY([PlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[SamplingPlotLocalisation] CHECK CONSTRAINT [FK_SamplingPlotLocalisation_SamplingPlot]
GO

--#####################################################################################################################
--######   Adding cascade delete for SamplingPlotProperty to SamplingPlot     #########################################
--#####################################################################################################################

ALTER TABLE [dbo].[SamplingPlotProperty] DROP CONSTRAINT [FK_SamplingPlotProperty_SamplingPlot]
GO

ALTER TABLE [dbo].[SamplingPlotProperty]  WITH CHECK ADD  CONSTRAINT [FK_SamplingPlotProperty_SamplingPlot] FOREIGN KEY([PlotID])
REFERENCES [dbo].[SamplingPlot] ([PlotID]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[SamplingPlotProperty] CHECK CONSTRAINT [FK_SamplingPlotProperty_SamplingPlot]
GO

--#####################################################################################################################
--######   Adding missing column PropertyGeometry for Table SamplingPlotProperty     ##################################
--#####################################################################################################################

IF (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS T where T.TABLE_NAME = 'SamplingPlotProperty' AND T.COLUMN_NAME = 'PropertyGeometry') = 0
BEGIN
   ALTER TABLE SamplingPlotProperty ADD [PropertyGeometry] [geometry] NULL;

   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The geometry of the property in relation to the geometry of the sampling plot, e.g. MultiPoints for lower and upper border of a stratigraphic layer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyGeometry';
   
END
GO




--#####################################################################################################################
--######   Adding missing Trigger for Table SamplingPlotProperty     ##################################################
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
RETURN '01.00.38'
END

GO

SELECT [dbo].[Version] ()  

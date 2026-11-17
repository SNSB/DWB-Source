
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.09'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Property   ######################################################################################
--#####################################################################################################################

ALTER TABLE [Property] ADD 	[PropertyParentID] [int] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PropertyID of the superior property' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyParentID'
GO

ALTER TABLE [Property] ADD [PropertyName] [nvarchar](100) NOT NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the property used to describe the place of the collection, may be derived from content of the module DiversityScientificTerms e.g. Pflanzengesellschaften' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyName'
GO

ALTER TABLE [Property] ADD [DefaultAccuracyOfProperty] [nvarchar](50) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default for the accuracy of values that can be reached with this method' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DefaultAccuracyOfProperty'
GO

ALTER TABLE [Property] ADD [DefaultMeasurementUnit] [nvarchar](50) NULL
GO

ALTER TABLE [Property] ADD [ParsingMethodName] [nvarchar](50) NOT NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal value, specifying a programming method used for parsing text in field PropertyValue in table CollectionEventProperty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'ParsingMethodName'
GO

ALTER TABLE [Property] ADD [DisplayEnabled] [bit] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Specifies if this item is enabled to be used within the database. Properties can be disabled to avoid seeing them, but to keep the definition for the future.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DisplayEnabled'
GO

ALTER TABLE [Property] ADD [DisplayOrder] [smallint] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO


--#####################################################################################################################
--######   SamplingPlotProperty   ######################################################################################
--#####################################################################################################################

declare @i int
set @i = (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C
where C.COLUMN_NAME = 'PropertyURI' and C.TABLE_NAME = 'SamplingPlotProperty')

if (@i = 0)
begin
ALTER TABLE [SamplingPlotProperty] ADD [PropertyURI] [varchar](255) NOT NULL
end
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to an external datasource e.g. DiversityScientificTerms' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyURI'
GO

ALTER TABLE [SamplingPlotProperty] ADD [DisplayText] [nvarchar](255) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The text for the property as shown e.g. in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

ALTER TABLE [SamplingPlotProperty] ADD [PropertyHierarchyCache] [nvarchar](max) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A cached text of the complete name of the descriptor including superior categories if present' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyHierarchyCache'
GO

ALTER TABLE [SamplingPlotProperty] ADD [PropertyValue] [nvarchar](255) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of a captured feature  e.g. temperature, pH, vegetation etc. If there is a range this is the lower or first value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'PropertyValue'
GO

ALTER TABLE [SamplingPlotProperty] ADD [ResponsibleName] [nvarchar](255) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent (person or organization) responsible for this entry.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'ResponsibleName'
GO

ALTER TABLE [SamplingPlotProperty] ADD [ResponsibleAgentURI] [varchar](255) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the person or organisation responsible for the data (see e.g. module DiversityAgents)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'ResponsibleAgentURI'
GO

ALTER TABLE [SamplingPlotProperty] ADD [Notes] [nvarchar](max) NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the property of the colletion site.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'Notes'
GO

ALTER TABLE [SamplingPlotProperty] ADD [AverageValueCache] [float] NULL
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'For numeric values - a cached average value according to the' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SamplingPlotProperty', @level2type=N'COLUMN',@level2name=N'AverageValueCache'
GO


--#####################################################################################################################
--######   [trgDelSamplingPlotProperty]   ######################################################################################
--#####################################################################################################################



ALTER TRIGGER [dbo].[trgDelSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
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
--######   [[trgUpdSamplingPlotProperty]]   ######################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdSamplingPlotProperty] ON [dbo].[SamplingPlotProperty] 
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
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.10'
END

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.35'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   [CollectionSpecimen_Core]   ######################################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionSpecimen_Core]
AS
SELECT     dbo.CollectionSpecimen.CollectionSpecimenID, dbo.CollectionSpecimen.Version, dbo.CollectionSpecimen.CollectionEventID, dbo.CollectionSpecimen.CollectionID, 
                      dbo.CollectionSpecimen.AccessionNumber, dbo.CollectionSpecimen.AccessionDate, dbo.CollectionSpecimen.AccessionDay, dbo.CollectionSpecimen.AccessionMonth, 
                      dbo.CollectionSpecimen.AccessionYear, dbo.CollectionSpecimen.AccessionDateSupplement, dbo.CollectionSpecimen.AccessionDateCategory, 
                      dbo.CollectionSpecimen.DepositorsName, dbo.CollectionSpecimen.DepositorsAgentURI, dbo.CollectionSpecimen.DepositorsAccessionNumber, 
                      dbo.CollectionSpecimen.LabelTitle, dbo.CollectionSpecimen.LabelType, dbo.CollectionSpecimen.LabelTranscriptionState, 
                      dbo.CollectionSpecimen.LabelTranscriptionNotes, dbo.CollectionSpecimen.ExsiccataURI, dbo.CollectionSpecimen.ExsiccataAbbreviation, 
                      dbo.CollectionSpecimen.OriginalNotes, dbo.CollectionSpecimen.AdditionalNotes, dbo.CollectionSpecimen.ReferenceTitle, dbo.CollectionSpecimen.ReferenceURI, 
                      dbo.CollectionSpecimen.Problems, dbo.CollectionSpecimen.DataWithholdingReason, CASE WHEN dbo.CollectionEvent.CollectionDate IS NULL AND 
                      dbo.CollectionEvent.CollectionYear IS NULL AND dbo.CollectionEvent.CollectionMonth IS NULL AND dbo.CollectionEvent.CollectionDay IS NULL THEN NULL 
                      ELSE CASE WHEN dbo.CollectionEvent.CollectionDate IS NULL THEN CASE WHEN dbo.CollectionEvent.CollectionYear IS NULL 
                      THEN '-' ELSE CAST(dbo.CollectionEvent.CollectionYear AS varchar) END + '/' + CASE WHEN dbo.CollectionEvent.CollectionMonth IS NULL 
                      THEN '-' ELSE CAST(dbo.CollectionEvent.CollectionMonth AS varchar) END + '/' + CASE WHEN dbo.CollectionEvent.CollectionDay IS NULL 
                      THEN '-' ELSE CAST(dbo.CollectionEvent.CollectionDay AS varchar) END ELSE CAST(year(dbo.CollectionEvent.CollectionDate) AS varchar) 
                      + '/' + CAST(month(dbo.CollectionEvent.CollectionDate) AS varchar) + '/' + CAST(day(dbo.CollectionEvent.CollectionDate) AS varchar) 
                      END END + CASE WHEN dbo.CollectionEvent.LocalityDescription IS NULL THEN '' ELSE '   ' + dbo.CollectionEvent.LocalityDescription END AS CollectionDate, 
                      dbo.CollectionEvent.LocalityDescription AS Locality, dbo.CollectionEvent.HabitatDescription AS Habitat, dbo.CollectionSpecimen.InternalNotes, 
                      dbo.CollectionSpecimen.ExternalDatasourceID, dbo.CollectionSpecimen.ExternalIdentifier, dbo.CollectionSpecimen.LogCreatedWhen, 
                      dbo.CollectionSpecimen.LogCreatedBy, dbo.CollectionSpecimen.LogUpdatedWhen, dbo.CollectionSpecimen.LogUpdatedBy
FROM         dbo.CollectionSpecimen INNER JOIN
                      dbo.CollectionSpecimenID_UserAvailable ON 
                      dbo.CollectionSpecimen.CollectionSpecimenID = dbo.CollectionSpecimenID_UserAvailable.CollectionSpecimenID LEFT OUTER JOIN
                      dbo.CollectionEvent ON dbo.CollectionSpecimen.CollectionEventID = dbo.CollectionEvent.CollectionEventID

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
RETURN '02.05.36'
END

GO



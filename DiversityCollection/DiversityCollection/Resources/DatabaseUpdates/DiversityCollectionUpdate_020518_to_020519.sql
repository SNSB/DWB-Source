declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.18'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   CollectionExternalDatasource   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionExternalDatasource ADD [LogCreatedWhen] [datetime] NULL
GO
ALTER TABLE CollectionExternalDatasource ADD  CONSTRAINT [DF_CollectionExternalDatasource_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionExternalDatasource', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO



ALTER TABLE CollectionExternalDatasource ADD [LogCreatedBy] [nvarchar](50) NULL
GO
ALTER TABLE CollectionExternalDatasource ADD CONSTRAINT [DF_CollectionExternalDatasource_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionExternalDatasource', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO



ALTER TABLE CollectionExternalDatasource ADD [LogUpdatedWhen] [datetime] NULL
GO
ALTER TABLE CollectionExternalDatasource ADD  CONSTRAINT [DF_CollectionExternalDatasource_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionExternalDatasource', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO



ALTER TABLE CollectionExternalDatasource ADD [LogUpdatedBy] [nvarchar](50) NULL
GO
ALTER TABLE CollectionExternalDatasource ADD  CONSTRAINT [DF_CollectionExternalDatasource_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionExternalDatasource', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO



CREATE TABLE [dbo].[CollectionExternalDatasource_log](
	[ExternalDatasourceID] [int] NULL,
	[ExternalDatasourceName] [nvarchar](255) NULL,
	[ExternalDatasourceVersion] [nvarchar](255) NULL,
	[Rights] [nvarchar](500) NULL,
	[ExternalDatasourceAuthors] [nvarchar](200) NULL,
	[ExternalDatasourceURI] [nvarchar](300) NULL,
	[ExternalDatasourceInstitution] [nvarchar](300) NULL,
	[InternalNotes] [nvarchar](1500) NULL,
	[ExternalAttribute_NameID] [nvarchar](255) NULL,
	[PreferredSequence] [tinyint] NULL,
	[Disabled] [bit] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionExternalDatasource_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CollectionExternalDatasource_log] ADD  CONSTRAINT [DF_CollectionExternalDatasource_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionExternalDatasource_log] ADD  CONSTRAINT [DF_CollectionExternalDatasource_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionExternalDatasource_log] ADD  CONSTRAINT [DF_CollectionExternalDatasource_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO



GRANT INSERT ON dbo.CollectionExternalDatasource_log TO Administrator
GO




CREATE TRIGGER [dbo].[trgDelCollectionExternalDatasource] ON [dbo].[CollectionExternalDatasource] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 21.03.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionExternalDatasource_Log (ExternalDatasourceID, ExternalDatasourceName, ExternalDatasourceVersion, Rights, ExternalDatasourceAuthors, ExternalDatasourceURI, ExternalDatasourceInstitution, InternalNotes, ExternalAttribute_NameID, PreferredSequence, Disabled, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.ExternalDatasourceID, deleted.ExternalDatasourceName, deleted.ExternalDatasourceVersion, deleted.Rights, deleted.ExternalDatasourceAuthors, deleted.ExternalDatasourceURI, deleted.ExternalDatasourceInstitution, deleted.InternalNotes, deleted.ExternalAttribute_NameID, deleted.PreferredSequence, deleted.Disabled, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'D'
FROM DELETED

GO




CREATE TRIGGER [dbo].[trgUpdCollectionExternalDatasource] ON [dbo].[CollectionExternalDatasource] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 21.03.2012  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionExternalDatasource_Log (ExternalDatasourceID, ExternalDatasourceName, ExternalDatasourceVersion, Rights, ExternalDatasourceAuthors, ExternalDatasourceURI, ExternalDatasourceInstitution, InternalNotes, ExternalAttribute_NameID, PreferredSequence, Disabled, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogState) 
SELECT deleted.ExternalDatasourceID, deleted.ExternalDatasourceName, deleted.ExternalDatasourceVersion, deleted.Rights, deleted.ExternalDatasourceAuthors, deleted.ExternalDatasourceURI, deleted.ExternalDatasourceInstitution, deleted.InternalNotes, deleted.ExternalAttribute_NameID, deleted.PreferredSequence, deleted.Disabled, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionExternalDatasource
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionExternalDatasource, deleted 
where 1 = 1 
AND CollectionExternalDatasource.ExternalDatasourceID = deleted.ExternalDatasourceID
GO


--#####################################################################################################################
--######   [IdentificationUnit]   ######################################################################################
--#####################################################################################################################


update u set u.RelatedUnitID = null
  FROM [IdentificationUnit]
u where Not u.RelatedUnitID is null
and Not exists (Select * from [IdentificationUnit] U2 where U2.CollectionSpecimenID = U.CollectionSpecimenID and U2.IdentificationUnitID = U.RelatedUnitID)
GO


ALTER TABLE [dbo].[IdentificationUnit]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnit_IdentificationUnit] FOREIGN KEY([CollectionSpecimenID], [RelatedUnitID])
REFERENCES [dbo].[IdentificationUnit] ([CollectionSpecimenID], [IdentificationUnitID])
GO

--#####################################################################################################################
--######   [AnalysisResult]   ######################################################################################
--#####################################################################################################################


ALTER TABLE AnalysisResult ADD [RowGUID] [uniqueidentifier] ROWGUIDCOL  DEFAULT (newsequentialid()) NOT NULL
GO

--#####################################################################################################################
--######   [[Annotation]]   ######################################################################################
--#####################################################################################################################


ALTER TABLE [dbo].[Annotation] ADD  CONSTRAINT [DF_Annotation_AnnotationType]  DEFAULT (N'Annotation') FOR [AnnotationType]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.19'
END

GO


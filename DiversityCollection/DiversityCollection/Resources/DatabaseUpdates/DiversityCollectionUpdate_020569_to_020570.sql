
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.69'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Annotation        ##########################################################################################
--######   Views for linked tables     ################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[AnnotationEvent]
AS
SELECT        AnnotationID, ReferencedID AS CollectionEventID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, ReferenceDisplayText, ReferenceURI, 
                         SourceDisplayText, SourceURI, IsInternal
FROM            dbo.Annotation
WHERE        (ReferencedTable = N'CollectionEvent')

GO

GRANT SELECT ON [AnnotationEvent] TO [User]
GO


CREATE VIEW [dbo].[AnnotationPart]
AS
SELECT        AnnotationID, ReferencedID AS SpecimenPartID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, ReferenceDisplayText, ReferenceURI, 
                         SourceDisplayText, SourceURI, IsInternal
FROM            dbo.Annotation
WHERE        (ReferencedTable = N'CollectionSpecimenPart')

GO

GRANT SELECT ON [AnnotationPart] TO [User]
GO


CREATE VIEW [dbo].[AnnotationSpecimen]
AS
SELECT        AnnotationID, ReferencedID AS CollectionSpecimenID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, ReferenceDisplayText, ReferenceURI, 
                         SourceDisplayText, SourceURI, IsInternal
FROM            dbo.Annotation
WHERE        (ReferencedTable = N'CollectionSpecimen')

GO

GRANT SELECT ON [AnnotationSpecimen] TO [User]
GO


CREATE VIEW [dbo].[AnnotationUnit]
AS
SELECT        AnnotationID, ReferencedID AS IdentificationUnitID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, ReferenceDisplayText, ReferenceURI, 
                         SourceDisplayText, SourceURI, IsInternal
FROM            dbo.Annotation
WHERE        (ReferencedTable = N'IdentificationUnit')

GO

GRANT SELECT ON [AnnotationUnit] TO [User]
GO


--#####################################################################################################################
--######   Annotation        ##########################################################################################
--######   ID mit neuen Typen     #####################################################################################
--#####################################################################################################################

/*
Beim Typ " External Identifier" soll es fest angelegte Subtypen geben wie DOI, LSID, UUID, URI, aber auch frei erzeugbare, inhaltlich definierte wie Field ID (z.B GBOL ID,, GFBio ID???), 
Published Sequence ID (INSDC, GenBank), Marker Sequence ID, Barcode ID = Process ID (BOLD), IPEN-ID. 
Zukünftig werden hier v.a. auch aus dem GFBio-Projekt noch verschiedene Nummern hinzu kommen, siehe auch http://gfbio.biowikifarm.net/internal/Subtask_7.2.2_Assessment_of_identifier_systems 
*/

INSERT INTO [dbo].[AnnotationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('ID'
           ,'External Identifier'
           ,'External Identifier'
           ,1
           ,NULL)
GO

INSERT INTO [dbo].[AnnotationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('DOI'
           ,'Digital Object Identifier'
           ,'DOI'
           ,1
           ,'ID')
GO

INSERT INTO [dbo].[AnnotationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('LSID'
           ,'Life Science Identifiers'
           ,'LSID'
           ,1
           ,'ID')
GO

INSERT INTO [dbo].[AnnotationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('UUID'
           ,'Universally Unique Identifier'
           ,'UUID'
           ,1
           ,'ID')
GO

INSERT INTO [dbo].[AnnotationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('IPEN number'
           ,'Number of the International Plant Exchange Network'
           ,'IPEN number'
           ,1
           ,'ID')
GO

--#####################################################################################################################
--######   Log table and trigger for Annotation       #################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Annotation_log](
	[AnnotationID] [int] NULL,
	[ReferencedAnnotationID] [int] NULL,
	[AnnotationType] [nvarchar](50) NULL,
	[Title] [nvarchar](50) NULL,
	[Annotation] [nvarchar](max) NULL,
	[URI] [varchar](255) NULL,
	[ReferenceDisplayText] [nvarchar](500) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[SourceDisplayText] [nvarchar](500) NULL,
	[SourceURI] [varchar](255) NULL,
	[IsInternal] [bit] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[ReferencedTable] [nvarchar](500) NULL,
	[ReferencedID] [int] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Annotation_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Annotation_log] ADD  CONSTRAINT [DF_Annotation_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[Annotation_log] ADD  CONSTRAINT [DF_Annotation_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[Annotation_log] ADD  CONSTRAINT [DF_Annotation_Log_LogUser]  DEFAULT (SYSTEM_USER) FOR [LogUser]
GO


GRANT SELECT ON [Annotation_log] TO Editor
GO

GRANT INSERT ON [Annotation_log] TO Editor
GO


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE TRIGGER [dbo].[trgDelAnnotation] ON [dbo].[Annotation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.3 */ 
/*  Date: 12/23/2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Annotation_Log (AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, ReferenceDisplayText, ReferenceURI, SourceDisplayText, SourceURI, IsInternal, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, ReferencedTable, ReferencedID,  LogState) 
SELECT deleted.AnnotationID, deleted.ReferencedAnnotationID, deleted.AnnotationType, deleted.Title, deleted.Annotation, deleted.URI, deleted.ReferenceDisplayText, deleted.ReferenceURI, deleted.SourceDisplayText, deleted.SourceURI, deleted.IsInternal, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.ReferencedTable, deleted.ReferencedID,  ''D''
FROM DELETED')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE TRIGGER [dbo].[trgUpdAnnotation] ON [dbo].[Annotation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.3 */ 
/*  Date: 12/23/2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Annotation_Log (AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, ReferenceDisplayText, ReferenceURI, SourceDisplayText, SourceURI, IsInternal, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, ReferencedTable, ReferencedID,  LogState) 
SELECT deleted.AnnotationID, deleted.ReferencedAnnotationID, deleted.AnnotationType, deleted.Title, deleted.Annotation, deleted.URI, deleted.ReferenceDisplayText, deleted.ReferenceURI, deleted.SourceDisplayText, deleted.SourceURI, deleted.IsInternal, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.ReferencedTable, deleted.ReferencedID,  ''U''
FROM DELETED


/* updating the logging columns */
Update Annotation
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM Annotation, deleted 
where 1 = 1 
AND Annotation.AnnotationID = deleted.AnnotationID
')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO



--#####################################################################################################################
--######   Foreign key for Relation to CollectionID   #################################################################
--#####################################################################################################################

if (SELECT COUNT(*) FROM [CollectionSpecimenRelation] R WHERE R.RelatedSpecimenCollectionID NOT IN (SELECT CollectionID FROM [Collection])) > 0
begin

	UPDATE R SET R.RelatedSpecimenCollectionID = NULL
	FROM [CollectionSpecimenRelation] R
	WHERE R.RelatedSpecimenCollectionID NOT IN (SELECT CollectionID FROM [Collection])
end


ALTER TABLE [dbo].[CollectionSpecimenRelation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenRelation_Collection] FOREIGN KEY([RelatedSpecimenCollectionID])
REFERENCES [dbo].[Collection] ([CollectionID])
GO

ALTER TABLE [dbo].[CollectionSpecimenRelation] CHECK CONSTRAINT [FK_CollectionSpecimenRelation_Collection]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.70'
END

GO


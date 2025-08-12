declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.29'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  New collection type area ####################################################################################
--#####################################################################################################################
if (select count(*) from [CollCollectionType_Enum] where code = 'area') = 0
begin
INSERT INTO [dbo].[CollCollectionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('area'
           ,'An area within a collection, e.g. within an insect display case'
           ,'area'
           ,1)
end
GO

--#####################################################################################################################
--######  New columns RecordingDate and LocationGeometry for CollectionImage ##########################################
--#####################################################################################################################

ALTER TABLE [dbo].[CollectionImage] ADD [RecordingDate] datetime NULL
GO

ALTER TABLE [dbo].[CollectionImage_log] ADD [RecordingDate] datetime NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The recording date of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'RecordingDate'
GO


ALTER TABLE [dbo].[CollectionImage] ADD [LocationGeometry] geometry NULL
GO

ALTER TABLE [dbo].[CollectionImage_log] ADD [LocationGeometry] geometry NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Geometry of the collection e.g. within a floor plan' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionImage', @level2type=N'COLUMN',@level2name=N'LocationGeometry'
GO



/****** Object:  Trigger [dbo].[trgDelCollectionImage]    Script Date: 02.05.2022 10:09:45 ******/
ALTER TRIGGER [dbo].[trgDelCollectionImage] ON [dbo].[CollectionImage] 
FOR DELETE AS 
INSERT INTO CollectionImage_Log (CollectionID, URI, ImageType, Notes, DataWithholdingReason, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, Description, RecordingDate, LocationGeometry, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogState) 
SELECT deleted.CollectionID, deleted.URI, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.Description, deleted.RecordingDate, deleted.LocationGeometry, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  'D'
FROM DELETED
GO

/****** Object:  Trigger [dbo].[trgUpdCollectionImage]    Script Date: 02.05.2022 10:10:02 ******/
ALTER TRIGGER [dbo].[trgUpdCollectionImage] ON [dbo].[CollectionImage] 
FOR UPDATE AS
INSERT INTO CollectionImage_Log (CollectionID, URI, ImageType, Notes, DataWithholdingReason, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, Description, RecordingDate, LocationGeometry, 
Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  LogState) 
SELECT deleted.CollectionID, deleted.URI, deleted.ImageType, deleted.Notes, deleted.DataWithholdingReason, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID, deleted.Description, deleted.RecordingDate, deleted.LocationGeometry, 
deleted.Title, deleted.IPR, deleted.CreatorAgent, deleted.CreatorAgentURI, deleted.CopyrightStatement, deleted.LicenseType, deleted.InternalNotes, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear,  'U'
FROM DELETED
Update CollectionImage
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionImage, deleted 
where 1 = 1 
AND CollectionImage.CollectionID = deleted.CollectionID
AND CollectionImage.URI = deleted.URI
GO



--#####################################################################################################################
--######  New table CollCollectionImageType_Enum  #####################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollCollectionImageType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[Icon] [image] NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_CollCollectionImageType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollCollectionImageType_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[CollCollectionImageType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_CollCollectionImageType_Enum_CollCollectionImageType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[CollCollectionImageType_Enum] ([Code])
GO

ALTER TABLE [dbo].[CollCollectionImageType_Enum] CHECK CONSTRAINT [FK_CollCollectionImageType_Enum_CollCollectionImageType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A symbol representing this entry in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum', @level2type=N'COLUMN',@level2name=N'Icon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The types of a collection, e.g. cupboard, drawer, box, rack etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollCollectionImageType_Enum'
GO

GRANT SELECT ON [dbo].[CollCollectionImageType_Enum] TO [User]
GO
GRANT UPDATE ON [dbo].[CollCollectionImageType_Enum] TO [Administrator] 
GO
GRANT DELETE ON [dbo].[CollCollectionImageType_Enum] TO [Administrator]
GO
GRANT INSERT ON [dbo].[CollCollectionImageType_Enum] TO [Administrator]
GO



INSERT [dbo].[CollCollectionImageType_Enum] ([Code], [Description], [DisplayText], [DisplayEnable], InternalNotes) 
VALUES (N'plan', N'Old plan optional including geometry of a collection', N'plan', 0, 'For internal use only')
GO

INSERT [dbo].[CollCollectionImageType_Enum] ([Code], [Description], [DisplayText], [DisplayEnable]) 
VALUES (N'image', N'image of a collection, e.g. a insect case', N'image', 1)
GO

INSERT [dbo].[CollCollectionImageType_Enum] ([Code], [Description], [DisplayText], [DisplayEnable]) 
VALUES (N'area', N'area resp. section within a collection, e.g. within a insect case', N'area', 1)
GO

ALTER TABLE [dbo].[CollectionImage]  WITH CHECK ADD  CONSTRAINT [FK_CollectionImage_CollCollectionImageType_Enum] FOREIGN KEY([ImageType])
REFERENCES [dbo].[CollCollectionImageType_Enum] ([Code])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[CollectionImage] CHECK CONSTRAINT [FK_CollectionImage_CollCollectionImageType_Enum]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.30'
END

GO


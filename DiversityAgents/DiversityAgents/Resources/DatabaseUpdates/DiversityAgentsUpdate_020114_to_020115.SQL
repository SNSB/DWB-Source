
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.14'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   AgentImageType_Enum  #######################################################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'AgentImageType_Enum') = 0
begin
	CREATE TABLE [dbo].[AgentImageType_Enum](
		[Code] [nvarchar](50) NOT NULL,
		[Description] [nvarchar](500) NULL,
		[DisplayText] [nvarchar](50) NULL,
		[DisplayOrder] [smallint] NULL,
		[DisplayEnable] [bit] NULL,
		[InternalNotes] [nvarchar](500) NULL,
		[ParentCode] [nvarchar](50) NULL,
		[URL] [nvarchar](500) NULL,
	 CONSTRAINT [PK_AgentImageType_Enum] PRIMARY KEY CLUSTERED 
	(
		[Code] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'Code'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'Description'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further information about this entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImageType_Enum', @level2type=N'COLUMN',@level2name=N'URL'
end


--#####################################################################################################################
--######   AgentImageType_Enum - Logo, Portrait  ######################################################################
--#####################################################################################################################
if (select count(*) from AgentImageType_Enum E where E.Code = 'Logo') = 0
begin
	INSERT INTO [dbo].[AgentImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Logo'
           ,'Logo'
           ,'Logo'
           ,1)
end
GO

if (select count(*) from AgentImageType_Enum E where E.Code = 'Portrait') = 0
begin
	INSERT INTO [dbo].[AgentImageType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('Portrait'
           ,'Portrait'
           ,'Portrait'
           ,1)
end
GO


--#####################################################################################################################
--######   AgentImage: Add Type, Sequence  ############################################################################
--#####################################################################################################################
if (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'AgentImage' and C.COLUMN_NAME = 'Type') = 0
begin
	ALTER TABLE [dbo].[AgentImage] ADD [Type] [nvarchar](50) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'Type'
	ALTER TABLE [dbo].[AgentImage_log] ADD [Type] [nvarchar](50) NULL;

	ALTER TABLE [dbo].[AgentImage]  WITH NOCHECK ADD  CONSTRAINT [FK_AgentImage_AgentImageType_Enum] FOREIGN KEY([Type])
	REFERENCES [dbo].[AgentImageType_Enum] ([Code])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[AgentImage] CHECK CONSTRAINT [FK_AgentImage_AgentImageType_Enum]
end
GO

if (select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'AgentImage' and C.COLUMN_NAME = 'Sequence') = 0
begin
	ALTER TABLE [dbo].[AgentImage] ADD [Sequence] int NULL;
	ALTER TABLE [dbo].[AgentImage] ADD  CONSTRAINT [DF_Agent_Sequence]  DEFAULT ((1)) FOR [Sequence]
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The sequence of the image' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'Sequence'
	ALTER TABLE [dbo].[AgentImage_log] ADD [Sequence] int NULL;
end
GO
UPDATE [dbo].[AgentImage] SET [Sequence] = 1
GO

--#####################################################################################################################
--######   trgDelAgentImage    ########################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelAgentImage] ON [dbo].[AgentImage] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.5 */ 
/*  Date: 18.08.2010  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Type], deleted.[Sequence], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Type], deleted.[Sequence], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'D' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end

GO


--#####################################################################################################################
--######   trgUpdAgentImage    ########################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdAgentImage] ON [dbo].[AgentImage] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  3.0.1.0 */ 
/*  Date: 07.05.2018  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT AgentID FROM deleted)
   EXECUTE procSetVersionAgent @ID
   SET @Version = (SELECT Version FROM Agent WHERE AgentID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Description], deleted.[Type], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'U'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, Agent WHERE deleted.AgentID = Agent.AgentID) > 0 
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Description], deleted.[Type], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'U' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end
else
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], [Type], [Sequence], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.[Description], deleted.[Type], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, -1, 'U' 
FROM DELETED
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  LogUpdatedBy = SUSER_NAME()
FROM AgentImage T, deleted D 
where 1 = 1 
AND T.AgentID = D.AgentID
AND T.URI = D.URI
GO


--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.01.02' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.15'
END

GO

select [dbo].[Version] ()  


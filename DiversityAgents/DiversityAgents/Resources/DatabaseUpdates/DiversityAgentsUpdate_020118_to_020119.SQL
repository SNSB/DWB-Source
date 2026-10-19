
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.18'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  AgentDescriptor  ############################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentDescriptor](
	[AgentID] [int] NOT NULL,
	[DescriptorID] [int] IDENTITY(1,1) NOT NULL,
	[Descriptor] [nvarchar](200) NOT NULL,
	[URL] [varchar](500) NULL,
	[DescriptorType] [nvarchar](50) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AgentDescriptor] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[DescriptorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentDescriptor] ADD  CONSTRAINT [DF_AgentDescriptor_Descriptor]  DEFAULT ('') FOR [Descriptor]
GO

ALTER TABLE [dbo].[AgentDescriptor] ADD  CONSTRAINT [DF_AgentDescriptor_URL]  DEFAULT ('') FOR [URL]
GO

ALTER TABLE [dbo].[AgentDescriptor] ADD  CONSTRAINT [DF_AgentDescriptor_DescriptorType]  DEFAULT (N'Descriptor') FOR [DescriptorType]
GO

ALTER TABLE [dbo].[AgentDescriptor] ADD  CONSTRAINT [DF__AgentDescriptor_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[AgentDescriptor] ADD  CONSTRAINT [DF_AgentDescriptor_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[AgentDescriptor] ADD  CONSTRAINT [DF__AgentDescriptor_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[AgentDescriptor] ADD  CONSTRAINT [DF_AgentDescriptor_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[AgentDescriptor] ADD  CONSTRAINT [DF_AgentDescriptor_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[AgentDescriptor]  WITH NOCHECK ADD  CONSTRAINT [FK_AgentDescriptor_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AgentDescriptor] CHECK CONSTRAINT [FK_AgentDescriptor_Agent]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent  (foreign key + part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor', @level2type=N'COLUMN',@level2name=N'AgentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Descriptor' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor', @level2type=N'COLUMN',@level2name=N'Descriptor'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL of the Descriptor. In case of a module related Descriptor, the link to the module entry resp. the related webservice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor', @level2type=N'COLUMN',@level2name=N'URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the Descriptor as described in table AgentDescriptorType_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor', @level2type=N'COLUMN',@level2name=N'DescriptorType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Descriptors for the agents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptor'
GO

GRANT SELECT ON [AgentDescriptor] TO [DiversityWorkbenchUser]
GO
GRANT INSERT ON [AgentDescriptor] TO [DiversityWorkbenchEditor]
GO
GRANT UPDATE ON [AgentDescriptor] TO [DiversityWorkbenchEditor]
GO
GRANT DELETE ON [AgentDescriptor] TO [DiversityWorkbenchEditor]
GO




--#####################################################################################################################
--######  AgentDescriptor_log  ########################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentDescriptor_log](
	[AgentID] [int] NULL,
	[DescriptorID] [int] NULL,
	[Descriptor] [nvarchar](200) NULL,
	[URL] [varchar](500) NULL,
	[DescriptorType] [nvarchar](50) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogVersion] [int] NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AgentDescriptor_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentDescriptor_log] ADD  CONSTRAINT [DF_AgentDescriptor_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[AgentDescriptor_log] ADD  CONSTRAINT [DF_AgentDescriptor_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[AgentDescriptor_log] ADD  CONSTRAINT [DF_AgentDescriptor_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID](),0)) FOR [LogUser]
GO

GRANT SELECT ON [AgentDescriptor_log] TO [DiversityWorkbenchUser]
GO
GRANT INSERT ON [AgentDescriptor_log] TO [DiversityWorkbenchEditor]
GO



--#####################################################################################################################
--######   trgDelAgentDescriptor    ###################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelAgentDescriptor] ON [dbo].[AgentDescriptor] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.27 */ 
/*  Date: 03.05.2021  */ 


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
INSERT INTO AgentDescriptor_Log (AgentID, DescriptorID, Descriptor, URL, DescriptorType, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.DescriptorID, D.Descriptor, D.URL, D.DescriptorType, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) ,  @Version,  'D'
FROM DELETED D 
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentDescriptor_Log (AgentID, DescriptorID, Descriptor, URL, DescriptorType, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.DescriptorID, D.Descriptor, D.URL, D.DescriptorType, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) , V.Version, 'D' 
FROM DELETED D, Agent V
WHERE D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentDescriptor_Log (AgentID, DescriptorID, Descriptor, URL, DescriptorType, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.DescriptorID, D.Descriptor, D.URL, D.DescriptorType, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) , -1, 'D' 
FROM DELETED D 
end
end
GO

ALTER TABLE [dbo].[AgentDescriptor] ENABLE TRIGGER [trgDelAgentDescriptor]
GO




--#####################################################################################################################
--######   trgUpdAgentDescriptor   ####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdAgentDescriptor] ON [dbo].[AgentDescriptor] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityAgents  4.1.27 */ 
/*  Date: 03.05.2021  */ 

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
INSERT INTO AgentDescriptor_Log (AgentID, DescriptorID, Descriptor, URL, DescriptorType, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion,  LogState) 
SELECT D.AgentID, D.DescriptorID, D.Descriptor, D.URL, D.DescriptorType, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) ,  @Version,  'U'
FROM DELETED D 
end
else
begin
if (select count(*) FROM DELETED D, Agent V WHERE D.AgentID = V.AgentID) > 0 
begin
INSERT INTO AgentDescriptor_Log (AgentID, DescriptorID, Descriptor, URL, DescriptorType, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.DescriptorID, D.Descriptor, D.URL, D.DescriptorType, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) , V.Version, 'U' 
FROM DELETED D, Agent V
WHERE D.AgentID = V.AgentID
end
else
begin
INSERT INTO AgentDescriptor_Log (AgentID, DescriptorID, Descriptor, URL, DescriptorType, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogVersion, LogState) 
SELECT D.AgentID, D.DescriptorID, D.Descriptor, D.URL, D.DescriptorType, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) , -1, 'U' 
FROM DELETED D 
end
end

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM AgentDescriptor T, deleted D where 1 = 1 
AND T.AgentID = D.AgentID
AND T.DescriptorID = D.DescriptorID
GO

ALTER TABLE [dbo].[AgentDescriptor] ENABLE TRIGGER [trgUpdAgentDescriptor]
GO

--#####################################################################################################################
--######   AgentDescriptorType_Enum   #################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentDescriptorType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[ParentCode] [nvarchar](50) NULL,
	[ParentRelation] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[URL] [varchar](500) NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[Icon] [image] NULL,
	[ModuleName] [varchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_AgentDescriptorType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentDescriptorType_Enum] ADD  CONSTRAINT [DF__AgentDescriptor_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[AgentDescriptorType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_AgentDescriptorType_Enum_AgentDescriptorType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[AgentDescriptorType_Enum] ([Code])
GO

ALTER TABLE [dbo].[AgentDescriptorType_Enum] CHECK CONSTRAINT [FK_AgentDescriptorType_Enum_AgentDescriptorType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relation to parent entry, e.g. part of' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'ParentRelation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A link to further information about the enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A symbol representing this entry in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'Icon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the enumerated entry is related to a DiversityWorkbench module or a related webservice, the name of the DiversityWorkbench module, e.g. DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'ModuleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the Descriptors' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentDescriptorType_Enum'
GO



GRANT SELECT ON [AgentDescriptorType_Enum] TO [DiversityWorkbenchUser]
GO
GRANT INSERT ON [AgentDescriptorType_Enum] TO [DiversityWorkbenchAdministrator]
GO
GRANT UPDATE ON [AgentDescriptorType_Enum] TO [DiversityWorkbenchAdministrator]
GO
GRANT DELETE ON [AgentDescriptorType_Enum] TO [DiversityWorkbenchAdministrator]
GO



--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '04.01.00' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.19'
END

GO

select [dbo].[Version] ()  


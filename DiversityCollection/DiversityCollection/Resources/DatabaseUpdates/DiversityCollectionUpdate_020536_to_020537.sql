
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.36'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   [IX_CollectionSpecimenID]   ######################################################################################
--#####################################################################################################################


CREATE NONCLUSTERED INDEX [IX_CollectionSpecimenID_log] ON [dbo].[CollectionSpecimen_log] 
(
	[CollectionSpecimenID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


--#####################################################################################################################
--######   [IX_CollectionEventID_log]   ######################################################################################
--#####################################################################################################################


CREATE NONCLUSTERED INDEX [IX_CollectionEventID_log] ON [dbo].[CollectionEvent_log] 
(
	[CollectionEventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


--#####################################################################################################################
--######   ÖK  ######################################################################################
--#####################################################################################################################


INSERT INTO [LocalisationSystem]
           ([LocalisationSystemID]
           ,[LocalisationSystemParentID]
           ,[LocalisationSystemName]
           ,[ParsingMethodName]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[DisplayOrder]
           ,[Description]
           ,[DisplayTextLocation1]
           ,[DescriptionLocation1]
           ,[DisplayTextLocation2]
           ,[DescriptionLocation2])
     VALUES
           (17
           ,3
           ,'ÖK'
           ,'MTB'
           ,'ÖK'
           ,1
           ,305
           ,'Österreichische Karte, amtliches (topografisches) Kartenwerk Österreichs'
           ,'ÖK'
           ,'Österreichische Karte'
           ,'Quadrant'
           ,'Quadrant')
GO

--#####################################################################################################################
--######   [DefaultProjectID]   ######################################################################################
--#####################################################################################################################

if (select COUNT(*) From INFORMATION_SCHEMA.COLUMNS C 
where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'CurrentProjectID') = 0
begin
alter table dbo.UserProxy ADD [CurrentProjectID] [int] NULL;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The current project selected by the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'CurrentProjectID';
end
GO

ALTER FUNCTION [dbo].[DefaultProjectID] ()
RETURNS int
AS
BEGIN
declare @i int
set @i = (SELECT CurrentProjectID from UserProxy where LoginName = USER_NAME())
if @i is null
	begin set @i =(	SELECT min([ProjectID])
	  FROM [dbo].[ProjectUser] p where p.LoginName = USER_NAME())
	end
return @i
END


GO


--#####################################################################################################################
--######   [ReplicationPublisher]   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ReplicationPublisher](
	[DatabaseName] [varchar](255) NOT NULL,
	[Server] [varchar](255) NOT NULL,
	[Port] [smallint] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ReplicationPublisher] PRIMARY KEY CLUSTERED 
(
	[DatabaseName] ASC,
	[Server] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the publishing database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name or address of the server where the publishing database is located' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Server'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The port used by the server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'Port'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplicationPublisher', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ReplicationPublisher] ADD  CONSTRAINT [DF_ReplicationPublisher_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO


GRANT INSERT ON ReplicationPublisher TO Administrator
GO

GRANT DELETE ON ReplicationPublisher TO Administrator
GO

GRANT SELECT ON ReplicationPublisher TO Editor
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.37'
END

GO



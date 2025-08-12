
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.25'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  ProjectTransfer  ############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectTransfer') = 0
begin
	CREATE TABLE [dbo].[ProjectTransfer](
		[ProjectID] [int] NOT NULL,
		[TransferDate] [datetime] NOT NULL CONSTRAINT [DF_ProjectTransfer_TransferDate]  DEFAULT (getdate()),
		[ResponsibleUserID] [int] NULL CONSTRAINT [DF_ProjectTransfer_ResponsibleUserID]  DEFAULT ((-1)),
		[TargetID] [int] NULL,
		[Package] [nvarchar](50) NULL,
		[Settings] [nvarchar](max) NULL,
	 CONSTRAINT [PK_ProjectTransfer] PRIMARY KEY CLUSTERED 
	(
		[ProjectID] ASC,
		[TransferDate] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project, part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransfer', @level2type=N'COLUMN',@level2name=N'ProjectID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of the transfer. Part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransfer', @level2type=N'COLUMN',@level2name=N'TransferDate'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the user as stored in table UserProxy of the source database, responsible for the transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransfer', @level2type=N'COLUMN',@level2name=N'ResponsibleUserID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the transfer regards a postgres database, the ID of the target (= Postgres database) as stored in table Target' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransfer', @level2type=N'COLUMN',@level2name=N'TargetID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the transfer regards a package, the name of the package, otherwise empty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransfer', @level2type=N'COLUMN',@level2name=N'Package'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The versions, number of transfered data etc. of the objects concerned by the transfer [format: JSON]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransfer', @level2type=N'COLUMN',@level2name=N'Settings'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The transfers of data of a project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectTransfer'

end
GO

GRANT SELECT ON [dbo].[ProjectTransfer] TO [CacheUser]
GO
GRANT INSERT ON [dbo].[ProjectTransfer] TO [CacheAdmin]
GO

--#####################################################################################################################
--######  SourceTransfer  #############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'SourceTransfer') = 0
begin
	CREATE TABLE [dbo].[SourceTransfer](
		[Source] [nvarchar](50) NOT NULL,
		[SourceView] [nvarchar](50) NOT NULL,
		[TransferDate] [datetime] NOT NULL CONSTRAINT [DF_SourceTransfer_TransferDate]  DEFAULT (getdate()),
		[ResponsibleUserID] [int] NULL CONSTRAINT [DF_SourceTransfer_ResponsibleUserID]  DEFAULT ((-1)),
		[TargetID] [int] NULL,
		[Settings] [nvarchar](max) NULL,
	 CONSTRAINT [PK_SourceTransfer] PRIMARY KEY CLUSTERED 
	(
		[Source] ASC,
		[SourceView] ASC,
		[TransferDate] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the source, e.g. Taxa, part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SourceTransfer', @level2type=N'COLUMN',@level2name=N'Source'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the main view for the data defined for retrieving the data from the source. Part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SourceTransfer', @level2type=N'COLUMN',@level2name=N'SourceView'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of the transfer, part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SourceTransfer', @level2type=N'COLUMN',@level2name=N'TransferDate'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the user as stored in table UserProxy of the source database, responsible for the transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SourceTransfer', @level2type=N'COLUMN',@level2name=N'ResponsibleUserID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the transfer regards a postgres database, the ID of the target (= Postgres database) as stored in table Target' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SourceTransfer', @level2type=N'COLUMN',@level2name=N'TargetID'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The versions, number of transfered data etc. of the objects concerned by the transfer [format: JSON]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SourceTransfer', @level2type=N'COLUMN',@level2name=N'Settings'

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The transfers of data of a source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SourceTransfer'

end
GO

GRANT SELECT ON [dbo].[SourceTransfer] TO [CacheUser]
GO
GRANT INSERT ON [dbo].[SourceTransfer] TO [CacheAdmin]
GO

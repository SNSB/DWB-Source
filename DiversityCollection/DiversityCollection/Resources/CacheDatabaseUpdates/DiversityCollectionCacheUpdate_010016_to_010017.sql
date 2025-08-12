
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.16'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   AgentSource    #############################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[AgentSource](
	[SourceView] [nvarchar](200) NOT NULL,
	[Source] [nvarchar](500) NULL,
	[SourceID] [int] NULL,
	[LinkedServerName] [nvarchar](500) NULL,
	[DatabaseName] [nvarchar](50) NULL,
	[Subsets] [nvarchar](500) NULL,
 CONSTRAINT [PK_AgentSource] PRIMARY KEY CLUSTERED 
(
	[SourceView] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'the name of the view retrieving the data from the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the database where the data are taken from' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentSource', @level2type=N'COLUMN',@level2name=N'DatabaseName'
GO

GRANT SELECT ON AgentSource TO [CacheUser]
GO
GRANT DELETE ON AgentSource TO [CacheAdmin] 
GO
GRANT UPDATE ON AgentSource TO [CacheAdmin]
GO
GRANT INSERT ON AgentSource TO [CacheAdmin]
GO



--#####################################################################################################################
--######   Agent       ################################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[Agent](
	[BaseURL] [varchar](500) NOT NULL,
	[AgentID] [int] NOT NULL,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[GivenNamePostfix] [nvarchar](50) NULL,
	[InheritedNamePrefix] [nvarchar](50) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[InheritedNamePostfix] [nvarchar](50) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL ,
	[AgentRole] [nvarchar](255) NULL,
	[AgentGender] [nvarchar](50) NULL,
	[Description] [nvarchar](1000) NULL,
	[OriginalSpelling] [nvarchar](200) NULL,
	[Notes] [nvarchar](max) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidUntilDate] [datetime] NULL,
	[SynonymToAgentID] [int] NULL,
	[ProjectID] [int] NOT NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[SourceView] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED 
(
	[BaseURL] ASC,
	[AgentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'SourceView'
GO


GRANT SELECT ON Agent TO [CacheUser]
GO
GRANT DELETE ON Agent TO [CacheAdmin] 
GO
GRANT UPDATE ON Agent TO [CacheAdmin]
GO
GRANT INSERT ON Agent TO [CacheAdmin]
GO



--#####################################################################################################################
--######   procTransferAgent              #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [dbo].[procTransferAgent] 
@View nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @SQL nvarchar(4000)
	set @SQL = ''INSERT INTO Agent
			  (BaseURL, AgentID, AgentParentID, AgentName, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, AgentRole, AgentGender, 
                         Description, OriginalSpelling, Notes, ValidFromDate, ValidUntilDate, SynonymToAgentID, ProjectID, SourceView)
	SELECT     BaseURL, AgentID, AgentParentID, AgentName, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, AgentRole, AgentGender, 
                         Description, OriginalSpelling, Notes, ValidFromDate, ValidUntilDate, SynonymToAgentID, ProjectID, '''''' + @View + ''''''
	FROM         '' + @View

	exec sp_executesql @SQL
	
	DECLARE @i int
	SET @i = (SELECT COUNT(*) FROM Agent)
	SELECT CAST(@i AS VARCHAR) + '' agents imported''

END')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


GRANT EXEC ON [dbo].[procTransferAgent] TO [CacheAdmin]
GO


--#####################################################################################################################
--######   AgentContactInformation       ##############################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[AgentContactInformation](
	[AgentID] [int] NOT NULL,
	[DisplayOrder] [tinyint] NOT NULL,
	[AddressType] [nvarchar](50) NULL,
	[Country] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[PostalCode] [nvarchar](50) NULL,
	[Streetaddress] [nvarchar](255) NULL,
	[Address] [nvarchar](255) NULL,
	[Telephone] [nvarchar](50) NULL,
	[CellularPhone] [nvarchar](50) NULL,
	[Telefax] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[URI] [nvarchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[ValidFrom] [datetime] NULL,
	[ValidUntil] [datetime] NULL,
	[SourceView] [varchar](128) NOT NULL,
 CONSTRAINT [PK_AgentAddress] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC,
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the source view of the data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'SourceView'
GO

GRANT SELECT ON AgentContactInformation TO [CacheUser]
GO
GRANT DELETE ON AgentContactInformation TO [CacheAdmin] 
GO
GRANT UPDATE ON AgentContactInformation TO [CacheAdmin]
GO
GRANT INSERT ON AgentContactInformation TO [CacheAdmin]
GO


--#####################################################################################################################
--######   procTransferAgentContactInformation     ####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [dbo].[procTransferAgentContactInformation] 
@View nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @SQL nvarchar(4000)
	set @SQL = ''INSERT INTO AgentContactInformation
			  (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, SourceView)
	SELECT     AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, '''''' + @View + ''''''
	FROM         '' + @View

	exec sp_executesql @SQL
	
	DECLARE @i int
	SET @i = (SELECT COUNT(*) FROM AgentContactInformation)
	SELECT CAST(@i AS VARCHAR) + '' agent contacts imported''

END')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


GRANT EXEC ON [dbo].[procTransferAgentContactInformation] TO [CacheAdmin]
GO

--#####################################################################################################################
--######   ViewCollectionSpecimen adding ExternalDatasourceID  ########################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[ViewCollectionSpecimen]
AS
SELECT    DISTINCT    S.CollectionSpecimenID, LabelTranscriptionNotes, OriginalNotes, S.LogUpdatedWhen, CollectionEventID, AccessionNumber, AccessionDate, AccessionDay, 
AccessionMonth, AccessionYear, DepositorsName, DepositorsAccessionNumber, ExsiccataURI, ExsiccataAbbreviation, AdditionalNotes, ReferenceTitle, 
ReferenceURI, ExternalDatasourceID
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimen AS S INNER JOIN
dbo.ProjectPublished AS PP INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.CollectionProject AS P ON PP.ProjectID = P.ProjectID ON S.CollectionSpecimenID = P.CollectionSpecimenID
WHERE        (DataWithholdingReason = '''' OR
DataWithholdingReason IS NULL) AND (S.CollectionSpecimenID NOT IN
(SELECT        CST.CollectionSpecimenID
FROM            ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenTransaction AS CST INNER JOIN
' +  dbo.SourceDatabase() + '.dbo.[Transaction] AS TR ON CST.TransactionID = TR.TransactionID
WHERE        (TR.TransactionType = N''embargo'') AND (TR.BeginDate IS NULL OR
TR.BeginDate <= GETDATE()) AND (TR.AgreedEndDate IS NULL OR
TR.AgreedEndDate >= GETDATE())))')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO


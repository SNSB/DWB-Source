declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.81'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   TransactionCurrency    #####################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[TransactionCurrency] () RETURNS nvarchar(50) AS BEGIN RETURN '€' END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default curreny for payments' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'TransactionCurrency'
GO

GRANT EXEC ON [dbo].[TransactionCurrency] TO [USER]
GO

--#####################################################################################################################
--######   Transaction  DateSupplement   ##############################################################################
--#####################################################################################################################

ALTER Table [Transaction] ADD DateSupplement nvarchar(100) NULL;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Verbal or additional date information, e.g. ''end of summer 1985'', ''first quarter'', ''1888-1892''.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Transaction', @level2type=N'COLUMN',@level2name=N'DateSupplement'
GO


--#####################################################################################################################
--######   TransactionPayment     #####################################################################################
--#####################################################################################################################

if(Select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'TransactionPayment') = 0
begin
	CREATE TABLE [dbo].[TransactionPayment](
		[TransactionID] [int] NOT NULL,
		[PaymentID] [int] IDENTITY(1,1) NOT NULL,
		[Amount] [float] NULL,
		[ForeignAmount] [float] NULL,
		[ForeignCurrency] [nvarchar](50) NULL,
		[PayerName] [nvarchar](500) NULL,
		[PayerAgentURI] [varchar](500) NULL,
		[RecipientName] [nvarchar](500) NULL,
		[RecipientAgentURI] [varchar](500) NULL,
		[PaymentDate] [datetime] NULL,
		[PaymentDateSupplement] [nvarchar](50) NULL,
		[Notes] [nvarchar](max) NULL,
		[LogCreatedWhen] [datetime] NULL,
		[LogCreatedBy] [nvarchar](50) NULL,
		[LogUpdatedWhen] [datetime] NULL,
		[LogUpdatedBy] [nvarchar](50) NULL,
		[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	 CONSTRAINT [PK_TransactionPayment] PRIMARY KEY CLUSTERED 
	(
		[TransactionID] ASC,
		[PaymentID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[TransactionPayment] ADD  CONSTRAINT [DF_TransactionPayment_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
	ALTER TABLE [dbo].[TransactionPayment] ADD  CONSTRAINT [DF_TransactionPayment_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
	ALTER TABLE [dbo].[TransactionPayment] ADD  CONSTRAINT [DF_TransactionPayment_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
	ALTER TABLE [dbo].[TransactionPayment] ADD  CONSTRAINT [DF_TransactionPayment_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
	ALTER TABLE [dbo].[TransactionPayment] ADD  CONSTRAINT [DF_TransactionPayment_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
	
	ALTER TABLE [dbo].[TransactionPayment]  WITH CHECK ADD  CONSTRAINT [FK_TransactionPayment_Transaction] FOREIGN KEY([TransactionID])
	REFERENCES [dbo].[Transaction] ([TransactionID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	ALTER TABLE [dbo].[TransactionPayment] CHECK CONSTRAINT [FK_TransactionPayment_Transaction]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the transaction, refers to table Transaction (= part of primary key and foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'TransactionID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the payment  (= part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'PaymentID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Amount of the payment in the default currency as defined in TransactionCurrency' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'Amount'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the payment was not in the default curreny as defined in TransactionCurrency, the amount of the payment in foreign curreny' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'ForeignAmount'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the payment was not in the default curreny as defined in TransactionCurrency, the foreign currency of the payment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'ForeignCurrency'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person or institution paying the amount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'PayerName'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the source for further infomations about the payer, e.g in the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'PayerAgentURI'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Agent receiving the payment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'RecipientName'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the source for further infomations about the recipient of the payment, e.g in the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'RecipientAgentURI'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of the payment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'PaymentDate'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Supplement to the date of the payment, e.g. if the original date is not a real date like ''summer 1920'' or ''1910 - 1912''' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'PaymentDateSupplement'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the payment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'Notes'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The payments within a transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment'

END
GO

GRANT SELECT ON TransactionPayment TO TransactionUser;
GO

GRANT DELETE ON TransactionPayment TO CollectionManager;
GO

GRANT UPDATE ON TransactionPayment TO CollectionManager;
GO

GRANT INSERT ON TransactionPayment TO CollectionManager;
GO


--#####################################################################################################################
--######   TransactionAgent       #####################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[TransactionAgent](
	[TransactionID] [int] NOT NULL,
	[TransactionAgentID] [int] IDENTITY(1,1) NOT NULL,
	[AgentName] [nvarchar](500) NULL,
	[AgentURI] [varchar](500) NULL,
	[AgentRole] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TransactionAgent] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC,
	[TransactionAgentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


ALTER TABLE [dbo].[TransactionAgent] ADD  CONSTRAINT [DF_TransactionAgent_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[TransactionAgent] ADD  CONSTRAINT [DF_TransactionAgent_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[TransactionAgent] ADD  CONSTRAINT [DF_TransactionAgent_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[TransactionAgent] ADD  CONSTRAINT [DF_TransactionAgent_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[TransactionAgent] ADD  CONSTRAINT [DF_TransactionAgent_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


ALTER TABLE [dbo].[TransactionAgent]  WITH CHECK ADD  CONSTRAINT [FK_TransactionAgent_Transaction] FOREIGN KEY([TransactionID])
REFERENCES [dbo].[Transaction] ([TransactionID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TransactionAgent] CHECK CONSTRAINT [FK_TransactionAgent_Transaction]
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the transaction, refers to table Transaction (= part of primary key and foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'TransactionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent within the transaction  (= part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'TransactionAgentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person or institution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'AgentName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the source for further infomations about the agent, e.g in the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Role of the agent within the transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'AgentRole'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Agents involved in the transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent'
GO


GRANT SELECT ON TransactionAgent TO TransactionUser;
GO

GRANT DELETE ON TransactionAgent TO CollectionManager;
GO

GRANT UPDATE ON TransactionAgent TO CollectionManager;
GO

GRANT INSERT ON TransactionAgent TO CollectionManager;
GO

--#####################################################################################################################
--######   CollectionManager     ######################################################################################
--#####################################################################################################################

sp_addrolemember @rolename = 'TransactionUser', @membername = 'CollectionManager'
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.82'
END

GO


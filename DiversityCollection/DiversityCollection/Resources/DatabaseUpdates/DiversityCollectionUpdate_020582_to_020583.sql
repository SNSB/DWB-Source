declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.82'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   TransactionAgent_log   #####################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[TransactionAgent_log](
	[TransactionID] [int] NULL,
	[TransactionAgentID] [int] NULL,
	[AgentName] [nvarchar](500) NULL,
	[AgentURI] [varchar](500) NULL,
	[AgentRole] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TransactionAgent_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[TransactionAgent_log] ADD  CONSTRAINT [DF_TransactionAgent_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[TransactionAgent_log] ADD  CONSTRAINT [DF_TransactionAgent_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[TransactionAgent_log] ADD  CONSTRAINT [DF_TransactionAgent_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


--#####################################################################################################################
--######   trgDelTransactionAgent        ##############################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelTransactionAgent] ON [dbo].[TransactionAgent] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 4/28/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionAgent_Log (TransactionID, TransactionAgentID, AgentName, AgentURI, AgentRole, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.TransactionID, deleted.TransactionAgentID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO



--#####################################################################################################################
--######   trgUpdTransactionAgent #####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdTransactionAgent] ON [dbo].[TransactionAgent] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 4/28/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionAgent_Log (TransactionID, TransactionAgentID, AgentName, AgentURI, AgentRole, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.TransactionID, deleted.TransactionAgentID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update TransactionAgent
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM TransactionAgent, deleted 
where 1 = 1 
AND TransactionAgent.TransactionAgentID = deleted.TransactionAgentID
AND TransactionAgent.TransactionID = deleted.TransactionID
GO



--#####################################################################################################################
--######   TransactionPayment_log #####################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[TransactionPayment_log](
	[TransactionID] [int] NULL,
	[PaymentID] [int] NULL,
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
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TransactionPayment_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[TransactionPayment_log] ADD  CONSTRAINT [DF_TransactionPayment_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[TransactionPayment_log] ADD  CONSTRAINT [DF_TransactionPayment_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[TransactionPayment_log] ADD  CONSTRAINT [DF_TransactionPayment_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


--#####################################################################################################################
--######   trgDelTransactionPayment  ##################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelTransactionPayment] ON [dbo].[TransactionPayment] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 4/28/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionPayment_Log (TransactionID, PaymentID, Amount, ForeignAmount, ForeignCurrency, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, PaymentDate, PaymentDateSupplement, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.TransactionID, deleted.PaymentID, deleted.Amount, deleted.ForeignAmount, deleted.ForeignCurrency, deleted.PayerName, deleted.PayerAgentURI, deleted.RecipientName, deleted.RecipientAgentURI, deleted.PaymentDate, deleted.PaymentDateSupplement, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######   trgUpdTransactionPayment  ##################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgUpdTransactionPayment] ON [dbo].[TransactionPayment] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 4/28/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionPayment_Log (TransactionID, PaymentID, Amount, ForeignAmount, ForeignCurrency, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, PaymentDate, PaymentDateSupplement, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.TransactionID, deleted.PaymentID, deleted.Amount, deleted.ForeignAmount, deleted.ForeignCurrency, deleted.PayerName, deleted.PayerAgentURI, deleted.RecipientName, deleted.RecipientAgentURI, deleted.PaymentDate, deleted.PaymentDateSupplement, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update TransactionPayment
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM TransactionPayment, deleted 
where 1 = 1 
AND TransactionPayment.PaymentID = deleted.PaymentID
AND TransactionPayment.TransactionID = deleted.TransactionID
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.83'
END

GO


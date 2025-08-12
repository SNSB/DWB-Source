declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.84'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   TransactionPayment - missing columns        ################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TransactionPayment' and C.COLUMN_NAME = 'Identifier') = 0
begin
	ALTER TABLE [dbo].[TransactionPayment] ADD [Identifier] [nvarchar](500) NULL;
	ALTER TABLE [dbo].[TransactionPayment_log] ADD [Identifier] [nvarchar](500) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An identifer for the payment like a booking number or invoice number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'Identifier'
end

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TransactionPayment' and C.COLUMN_NAME = 'PaymentURI') = 0
begin
	ALTER TABLE [dbo].[TransactionPayment] ADD [PaymentURI] [varchar](500) NULL;
	ALTER TABLE [dbo].[TransactionPayment_log] ADD [PaymentURI] [varchar](500) NULL;
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A link to an external administration system for the payment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionPayment', @level2type=N'COLUMN',@level2name=N'PaymentURI'
end

GO

/****** Object:  Trigger [dbo].[trgDelTransactionPayment]    Script Date: 24.05.2016 14:59:35 ******/
ALTER TRIGGER [dbo].[trgDelTransactionPayment] ON [dbo].[TransactionPayment] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 4/14/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionPayment_Log (TransactionID, PaymentID, Amount, ForeignAmount, ForeignCurrency, Identifier, PaymentURI, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, PaymentDate, PaymentDateSupplement, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.TransactionID, deleted.PaymentID, deleted.Amount, deleted.ForeignAmount, deleted.ForeignCurrency, deleted.Identifier, deleted.PaymentURI, deleted.PayerName, deleted.PayerAgentURI, deleted.RecipientName, deleted.RecipientAgentURI, deleted.PaymentDate, deleted.PaymentDateSupplement, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

/****** Object:  Trigger [dbo].[trgUpdTransactionPayment]    Script Date: 24.05.2016 14:59:59 ******/
ALTER TRIGGER [dbo].[trgUpdTransactionPayment] ON [dbo].[TransactionPayment] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.5 */ 
/*  Date: 4/14/2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO TransactionPayment_Log (TransactionID, PaymentID, Amount, ForeignAmount, ForeignCurrency, Identifier, PaymentURI, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, PaymentDate, PaymentDateSupplement, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.TransactionID, deleted.PaymentID, deleted.Amount, deleted.ForeignAmount, deleted.ForeignCurrency, deleted.Identifier, deleted.PaymentURI, deleted.PayerName, deleted.PayerAgentURI, deleted.RecipientName, deleted.RecipientAgentURI, deleted.PaymentDate, deleted.PaymentDateSupplement, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
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
--######  hide observation from material category       ###############################################################
--#####################################################################################################################

UPDATE [dbo].[CollMaterialCategory_Enum]
   SET [DisplayEnable] = 0
 WHERE CODE like '%observation'
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.85'
END

GO



declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.08'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   DataWithholdingReason for AgentContactInformation    #######################################################
--#####################################################################################################################

ALTER TABLE AgentContactInformation ADD [DataWithholdingReason] nvarchar(255) CONSTRAINT [DF_AgentContactInformation_DataWithholdingReason]  DEFAULT (N'Withhold by default')
GO
ALTER TABLE AgentContactInformation_log ADD [DataWithholdingReason] nvarchar(255)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is withhold, the reason for withholding the data, otherwise null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentContactInformation', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO


/****** Object:  Trigger [dbo].[trgDelAgentContactInformation]    Script Date: 16.06.2016 16:35:38 ******/
ALTER TRIGGER [dbo].[trgDelAgentContactInformation] ON [dbo].[AgentContactInformation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.1 */ 
/*  Date: 20.09.2006  */ 


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
INSERT INTO AgentContactInformation_Log (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.DisplayOrder, deleted.AddressType, deleted.Country, deleted.City, deleted.PostalCode, deleted.Streetaddress, deleted.Address, deleted.Telephone, deleted.CellularPhone, deleted.Telefax, deleted.Email, deleted.URI, deleted.Notes, deleted.ValidFrom, deleted.ValidUntil, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO AgentContactInformation_Log (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.DisplayOrder, deleted.AddressType, deleted.Country, deleted.City, deleted.PostalCode, deleted.Streetaddress, deleted.Address, deleted.Telephone, deleted.CellularPhone, deleted.Telefax, deleted.Email, deleted.URI, deleted.Notes, deleted.ValidFrom, deleted.ValidUntil, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'D' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end
GO

/****** Object:  Trigger [dbo].[trgUpdAgentContactInformation]    Script Date: 16.06.2016 16:35:57 ******/
ALTER TRIGGER [dbo].[trgUpdAgentContactInformation] ON [dbo].[AgentContactInformation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.1 */ 
/*  Date: 20.09.2006  */ 

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
INSERT INTO AgentContactInformation_Log (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.DisplayOrder, deleted.AddressType, deleted.Country, deleted.City, deleted.PostalCode, deleted.Streetaddress, deleted.Address, deleted.Telephone, deleted.CellularPhone, deleted.Telefax, deleted.Email, deleted.URI, deleted.Notes, deleted.ValidFrom, deleted.ValidUntil, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO AgentContactInformation_Log (AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, DataWithholdingReason, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.DisplayOrder, deleted.AddressType, deleted.Country, deleted.City, deleted.PostalCode, deleted.Streetaddress, deleted.Address, deleted.Telephone, deleted.CellularPhone, deleted.Telefax, deleted.Email, deleted.URI, deleted.Notes, deleted.ValidFrom, deleted.ValidUntil, deleted.DataWithholdingReason, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'U' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end

/* updating the logging columns */
Update AgentContactInformation
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM AgentContactInformation, deleted 
where 1 = 1 
AND AgentContactInformation.AgentID = deleted.AgentID
AND AgentContactInformation.DisplayOrder = deleted.DisplayOrder
GO

--#####################################################################################################################
--######   setting the default value    ###############################################################################
--#####################################################################################################################



UPDATE A SET A.DataWithholdingReason = 'Withhold by default'
FROM AgentContactInformation A
GO

--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.00.22' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.09'
END

GO

select [dbo].[Version] ()  


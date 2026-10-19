
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.11'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   New column Description in AgentImage    ####################################################################
--#####################################################################################################################
if (SELECT count(*) 
FROM INFORMATION_SCHEMA.COLUMNS C
WHERE C.TABLE_NAME = 'AgentImage' AND C.COLUMN_NAME = 'Description' ) = 0 
begin 
   ALTER TABLE AgentImage ADD [Description] nvarchar(MAX) NULL
   ALTER TABLE AgentImage_log ADD [Description] nvarchar(MAX) NULL
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AgentImage', @level2type=N'COLUMN',@level2name=N'Description'
end
GO

--#####################################################################################################################
--######   Log columns in AgentImage_log    ###########################################################################
--#####################################################################################################################
if (SELECT count(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = 'AgentImage_log' AND C.COLUMN_NAME = 'LogInsertedBy' ) = 0 
begin 
   ALTER TABLE AgentImage_log ADD LogInsertedBy nvarchar(50) NULL
end
GO

if (SELECT count(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = 'AgentImage_log' AND C.COLUMN_NAME = 'LogInsertedWhen' ) = 0 
begin 
   ALTER TABLE AgentImage_log ADD LogInsertedWhen smalldatetime NULL
end
GO

if (SELECT count(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = 'AgentImage_log' AND C.COLUMN_NAME = 'LogUpdatedBy' ) = 0 
begin 
   ALTER TABLE AgentImage_log ADD LogUpdatedBy nvarchar(50) NULL
end
GO

if (SELECT count(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = 'AgentImage_log' AND C.COLUMN_NAME = 'LogUpdatedWhen' ) = 0 
begin 
   ALTER TABLE AgentImage_log ADD LogUpdatedWhen smalldatetime NULL
end
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
INSERT INTO AgentImage_Log (AgentID, URI, [Description], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'D' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end

GO

--#####################################################################################################################
--######   trgUpdAgentImage    ########################################################################################
--#####################################################################################################################
BEGIN TRY/****** Object:  Trigger [trgUpdAgentImage]    Script Date: 06.07.2018 15:36:13 ******/
	DROP TRIGGER [dbo].[trgUpdAgentImage]
END TRY
BEGIN CATCH
END CATCH
GO


CREATE TRIGGER [trgUpdAgentImage] ON [dbo].[AgentImage] 
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
INSERT INTO AgentImage_Log (AgentID, URI, [Description], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion,  LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  @Version,  'U'
FROM DELETED
end
else
begin
if (select count(*) FROM DELETED, Agent WHERE deleted.AgentID = Agent.AgentID) > 0 
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, Agent.Version, 'U' 
FROM DELETED, Agent
WHERE deleted.AgentID = Agent.AgentID
end
else
begin
INSERT INTO AgentImage_Log (AgentID, URI, [Description], LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogVersion, LogState) 
SELECT deleted.AgentID, deleted.URI, deleted.[Description], deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, -1, 'U' 
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
RETURN '03.00.01.00' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.12'
END

GO

select [dbo].[Version] ()  


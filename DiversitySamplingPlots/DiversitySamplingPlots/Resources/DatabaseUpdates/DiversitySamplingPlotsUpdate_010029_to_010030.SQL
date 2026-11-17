
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.29'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Add privacy message for EU-DSGVO  ##########################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   UserProxy - Add ID - according to EU-DSGVO   ###############################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'ID') = 0 
begin 
   ALTER TABLE UserProxy ADD [ID] [int] IDENTITY(1,1) NOT NULL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'ID' end 
GO

--#####################################################################################################################
--######   UserProxy - Add - PrivacyConsent and PrivacyConsentDate   ##################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'PrivacyConsent') = 0 
begin 
   ALTER TABLE UserProxy ADD [PrivacyConsent] [bit] NULL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user consents the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsent' end 
GO

GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsent]) TO [DiversityWorkbenchUser]
GO

if (select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'UserProxy' and c.COLUMN_NAME = 'PrivacyConsentDate') = 0 
begin 
   ALTER TABLE UserProxy ADD [PrivacyConsentDate] [datetime] NULL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time and date when the user consented or refused the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsentDate' end 
GO

GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsentDate]) TO [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######   UserProxy - Trigger for setting PrivacyConsentDate   #######################################################
--#####################################################################################################################
CREATE TRIGGER [dbo].[trgUpdUserProxy] ON [dbo].[UserProxy] 
FOR UPDATE AS 
declare @PC bit 
if (select count(*) from deleted) = 1 
begin 
  set @PC = (select case when I.PrivacyConsent <> D.PrivacyConsent 
    or (I.PrivacyConsent is null and not D.PrivacyConsent is null) 
    or (not I.PrivacyConsent is null and D.PrivacyConsent is null) 
    then 1 else 0 end from inserted I, deleted D) 
  if (@PC = 1) 
  begin 
    UPDATE U SET PrivacyConsentDate = GETDATE()  
    FROM UserProxy U, deleted D 
    WHERE U.ID = D.ID 
  end 
end 
GO

--#####################################################################################################################
--######   Function providing the ID of the user from UserProxy  ######################################################
--#####################################################################################################################

declare @SQL nvarchar(max) set @SQL = (select ' CREATE FUNCTION [dbo].[UserID] () RETURNS int AS 
BEGIN  
declare @ID int;  
SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = SUSER_SNAME()) 
RETURN @ID  
END ') 
begin try 
   exec sp_executesql @SQL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserID' 
end try 
begin catch 
end catch 
GO 
GRANT EXEC ON [dbo].[UserID] TO [DiversityWorkbenchUser] 
GO 

--#####################################################################################################################
--######   Function providing the name of the user from UserProxy  ####################################################
--#####################################################################################################################

declare @SQL nvarchar(max) set @SQL = (select ' CREATE FUNCTION [dbo].[UserName] (@ID varchar(10)) RETURNS nvarchar(50) AS 
BEGIN  
declare @User nvarchar(50);  
SET @User = (SELECT MIN(CASE WHEN U.CombinedNameCache <> '''' THEN substring(U.CombinedNameCache, 1, 50) ELSE CASE WHEN U.LoginName <> '''' THEN U.LoginName ELSE cast(U.ID as varchar) END END) FROM UserProxy U WHERE cast(U.ID as varchar) = @ID)  
RETURN @User  
END ') 
begin try 
   exec sp_executesql @SQL 
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserName' 
end try 
begin catch 
end catch 
GO 
GRANT EXEC ON [dbo].[UserName] TO [DiversityWorkbenchUser] 
GO 

--#####################################################################################################################
--######   Function PrivacyConsentInfo providing common information within the DiversityWorkbench  ####################
--#####################################################################################################################

CREATE FUNCTION [dbo].[PrivacyConsentInfo] () 
RETURNS varchar (900) 
AS  
BEGIN return 'http://diversityworkbench.net/Portal/Default_Agreement_on_Processing_of_Personal_Data_in_DWB_Software'  
END; 
GO 
GRANT EXEC ON [dbo].[PrivacyConsentInfo] TO [DiversityWorkbenchUser] 
GO 
GRANT ALTER ON [dbo].[PrivacyConsentInfo] TO [DiversityWorkbenchAdministrator] 
GO 


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.09.01' 
END

GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.30'
END

GO

SELECT [dbo].[Version] ()  

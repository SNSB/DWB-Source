
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.07'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Check contstraints for date columns in table Agent   #######################################################
--#####################################################################################################################

update a set [ValidFromDay] = NULL, 
  NOTES = CASE WHEN [Notes] IS NULL OR Notes = '' THEN '' ELSE Notes + '; ' END + '[Wrong ValidFromDay has been removed: ' + cast(ValidFromDay as nvarchar) + ']'
  FROM [dbo].[Agent]
a where a.ValidFromDay not between 1 and 31


ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidFromDay] CHECK  (([ValidFromDay]>(0) AND [ValidFromDay]<(32)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidFromDay]
GO

update a set [ValidFromMonth] = NULL, 
  NOTES = CASE WHEN [Notes] IS NULL OR Notes = '' THEN '' ELSE Notes + '; ' END + '[Wrong ValidFromMonth has been removed: ' + cast(ValidFromMonth as nvarchar) + ']'
  FROM [dbo].[Agent]
a where a.[ValidFromMonth] not between 1 and 13


ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidFromMonth] CHECK  (([ValidFromMonth]>(0) AND [ValidFromMonth]<(13)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidFromMonth]
GO

update a set [ValidFromYear] = NULL, 
  NOTES = CASE WHEN [Notes] IS NULL OR Notes = '' THEN '' ELSE Notes + '; ' END + '[Wrong ValidFromYear has been removed: ' + cast(ValidFromYear as nvarchar) + ']'
  FROM [dbo].[Agent]
a where a.[ValidFromYear] not between 0 and year(getdate()) + 2


ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidFromYear] CHECK  (([ValidFromYear]>(0) AND [ValidFromYear]<(year(getdate()) + 2)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidFromYear]
GO


update a set [ValidUntilDay] = NULL, 
  NOTES = CASE WHEN [Notes] IS NULL OR Notes = '' THEN '' ELSE Notes + '; ' END + '[Wrong ValidUntilDay has been removed: ' + cast(ValidUntilDay as nvarchar) + ']'
  FROM [dbo].[Agent]
a where a.ValidUntilDay not between 1 and 31


ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidUntilDay] CHECK  (([ValidUntilDay]>(0) AND [ValidUntilDay]<(32)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidUntilDay]
GO


update a set ValidUntilMonth = NULL, 
  NOTES = CASE WHEN [Notes] IS NULL OR Notes = '' THEN '' ELSE Notes + '; ' END + '[Wrong ValidUntilMonth has been removed: ' + cast(ValidUntilMonth as nvarchar) + ']'
  FROM [dbo].[Agent]
a where a.ValidUntilMonth not between 1 and 13


ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidUntilMonth] CHECK  (([ValidUntilMonth]>(0) AND [ValidUntilMonth]<(13)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidUntilMonth]
GO


update a set ValidUntilYear = NULL, 
  NOTES = CASE WHEN [Notes] IS NULL OR Notes = '' THEN '' ELSE Notes + '; ' END + '[Wrong ValidUntilYear has been removed: ' + cast(ValidUntilYear as nvarchar) + ']'
  FROM [dbo].[Agent]
a where a.ValidUntilYear not between 0 and year(getdate()) + 2

ALTER TABLE [dbo].[Agent]  WITH CHECK ADD  CONSTRAINT [Agent_CK_ValidUntilYear] CHECK  (([ValidUntilYear]>(0) AND [ValidUntilYear]<(year(getdate()) + 100)))
GO
ALTER TABLE [dbo].[Agent] CHECK CONSTRAINT [Agent_CK_ValidUntilYear]
GO



--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.00.16' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.08'
END

GO

select [dbo].[Version] ()  


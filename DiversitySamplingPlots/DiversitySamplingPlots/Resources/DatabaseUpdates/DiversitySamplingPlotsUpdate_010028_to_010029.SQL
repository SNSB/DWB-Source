
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.28'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   update for Database   ######################################################################################
--#####################################################################################################################

-- Grant permissions 
GRANT SELECT ON [dbo].[Property]  TO [DiversityWorkbenchEditor]

GRANT SELECT ON [dbo].[SamplingPlotProperty]  TO [DiversityWorkbenchEditor]
GRANT INSERT ON [dbo].[SamplingPlotProperty]  TO [DiversityWorkbenchAdministrator]
GRANT UPDATE ON [dbo].[SamplingPlotProperty]  TO [DiversityWorkbenchAdministrator]
GRANT DELETE ON [dbo].[SamplingPlotProperty]  TO [DiversityWorkbenchAdministrator]

GRANT SELECT ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchEditor]
GRANT INSERT ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchAdministrator]
GRANT UPDATE ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchAdministrator]
GRANT DELETE ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchAdministrator]

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

/* wait till new official release is available
ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.09.00' 
END

GO
*/

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.29'
END

GO

SELECT [dbo].[Version] ()  

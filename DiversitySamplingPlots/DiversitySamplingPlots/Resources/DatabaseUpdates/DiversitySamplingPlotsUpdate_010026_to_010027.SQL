
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.26'
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

--### Add PropertyURI to table Property ### 

IF (SELECT count(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = 'Property' AND C.COLUMN_NAME = 'PropertyURI') = 0
BEGIN
ALTER TABLE [dbo].[Property] ADD PropertyURI varchar(1000)
END
GO 

--### Add Property EUNIS 2012 ### 

DECLARE @Test int;
SET @Test = (SELECT count (*) FROM [Property] P WHERE P.PropertyID = 2)
IF (@Test = 0)
BEGIN

INSERT INTO [dbo].[Property]
      ([PropertyID]
      ,[PropertyName]
      ,[ParsingMethodName]
      ,[DisplayText]
      ,[DisplayEnabled]
      ,[PropertyURI])
SELECT 
	 2
	,'EUNIS 2012 (European Nature Information System)'
	,'Vegetation'
	,'EUNIS 2012 (European Nature Information System)'
	,1
	,'http://tnt.diversityworkbench.de/ScientificTerms_TNT/Terminology/2';	

END
GO

--### Add Property Biotoptypen (BFN) ### 

DECLARE @Test int;
SET @Test = (SELECT count (*) FROM [Property] P WHERE P.PropertyID = 41)
IF (@Test = 0)
BEGIN

INSERT INTO [dbo].[Property]
      ([PropertyID]
      ,[PropertyName]
      ,[ParsingMethodName]
      ,[DisplayText]
      ,[DisplayEnabled]
      ,[PropertyURI])
SELECT 
	 41
	,'Biotoptypen (BFN)'
	,'Vegetation'
	,'Biotoptypen (BFN)'
	,1
	,'http://tnt.diversityworkbench.de/ScientificTerms_TNT/Terminology/41';	

END
GO

GRANT VIEW DEFINITION TO [DiversityWorkbenchEditor]
GO 


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.08.04' 
END

GO

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.27'
END

GO

SELECT [dbo].[Version] ()  


declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.53'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Property - adding the column PropertyURI  ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.COLUMN_NAME = 'PropertyURI' and C.TABLE_NAME = 'Property') = 0
begin
ALTER TABLE dbo.Property ADD PropertyURI [varchar](1000) NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the property, e.g. as provided by the module DiversityScientificTerms.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Property', @level2type=N'COLUMN',@level2name=N'PropertyURI'
end
GO


--#####################################################################################################################
--######   DEFAULT for DisplayEnabled ######################################################################################
--#####################################################################################################################

if (select count(*) from sys.objects where name = 'DF_Property_DisplayEnabled') = 0
begin
ALTER TABLE dbo.Property ADD CONSTRAINT [DF_Property_DisplayEnabled] DEFAULT (1) FOR DisplayEnabled 
end
GO

--#####################################################################################################################
--######  Property -  Description change size to MAX ######################################################################################
--#####################################################################################################################


ALTER TABLE dbo.Property ALTER COLUMN [Description] nvarchar(MAX) NULL 
GO


--#####################################################################################################################
--######   GRANTS FOR Property  ######################################################################################
--#####################################################################################################################

GRANT INSERT ON [dbo].[Property] TO [Administrator]
GO

GRANT UPDATE ON [dbo].[Property] TO [Administrator]
GO

--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.08' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.54'
END

GO



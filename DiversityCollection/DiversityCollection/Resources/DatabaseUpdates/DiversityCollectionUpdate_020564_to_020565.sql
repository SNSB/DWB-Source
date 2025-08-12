
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.64'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   CacheAdmin            ######################################################################################
--#####################################################################################################################

CREATE ROLE [CacheAdmin]
GO


--#####################################################################################################################
--######   AnonymCollector       ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[AnonymCollector](
	[CollectorsName] [nvarchar](400) NOT NULL,
	[Anonymisation] [nvarchar](50) NULL,
 CONSTRAINT [PK_AnonymCollector] PRIMARY KEY CLUSTERED 
(
	[CollectorsName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT SELECT ON [AnonymCollector] TO [CacheUser]
GO

GRANT INSERT ON [AnonymCollector] TO [CacheAdmin]
GO

GRANT UPDATE ON [AnonymCollector] TO [CacheAdmin]
GO

GRANT DELETE ON [AnonymCollector] TO [CacheAdmin]
GO


--#####################################################################################################################
--######             s           ######################################################################################
--#####################################################################################################################

UPDATE L
SET [DisplayText] = Replace([DisplayText], 'DiversityGazetteer', 'DiversityGazetteers')
      ,[Description] = Replace([Description], 'DiversityGazetteer', 'DiversityGazetteers')
  FROM [dbo].[LocalisationSystem]
L where L.DisplayText like '%DiversityGazetteer%'


GO




--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.00' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.65'
END

GO



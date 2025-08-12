
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.13'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   TaxonAnalysisCategoryValue   ###############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[TaxonAnalysisCategoryValue](
	[AnalysisID] [int] NOT NULL,
	[AnalysisValue] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[Notes] [nvarchar](500) NULL,
	[SourceView] [nvarchar](128) NULL,
 CONSTRAINT [PK_TaxonAnalysisCategoryValue] PRIMARY KEY CLUSTERED 
(
	[AnalysisID] ASC,
	[AnalysisValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 


GRANT SELECT ON TaxonAnalysisCategoryValue TO [CacheUser]
GO
GRANT DELETE ON TaxonAnalysisCategoryValue TO [CacheAdmin] 
GO
GRANT UPDATE ON TaxonAnalysisCategoryValue TO [CacheAdmin]
GO
GRANT INSERT ON TaxonAnalysisCategoryValue TO [CacheAdmin]
GO



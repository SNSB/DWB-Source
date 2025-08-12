

--#####################################################################################################################
--######   CacheIdentificationUnitGeoAnalysis  ########################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheIdentificationUnitGeoAnalysis') = 0
begin

CREATE TABLE [#project#].[CacheIdentificationUnitGeoAnalysis](
	[CollectionSpecimenID] [int] NOT NULL,
	[IdentificationUnitID] [int] NOT NULL,
	[AnalysisDate] [datetime] NOT NULL,
	[Geography] [nvarchar](MAX) NULL,
	[Geometry] [nvarchar](MAX) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheIdentificationUnitGeoAnalysis] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[IdentificationUnitID] ASC,
	[AnalysisDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GRANT SELECT ON [#project#].CacheIdentificationUnitGeoAnalysis TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].CacheIdentificationUnitGeoAnalysis TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].CacheIdentificationUnitGeoAnalysis TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].CacheIdentificationUnitGeoAnalysis TO [CacheAdmin_#project#]
GO



--#####################################################################################################################
--######   procPublishIdentificationUnitGeoAnalysis  ##################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishIdentificationUnitGeoAnalysis] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishIdentificationUnitGeoAnalysis]
*/
truncate table [#project#].CacheIdentificationUnitGeoAnalysis

INSERT INTO [#project#].[CacheIdentificationUnitGeoAnalysis]
           ([CollectionSpecimenID]
      ,[IdentificationUnitID]
      ,[AnalysisDate]
      ,[Geography]
      ,[Geometry]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[Notes])
SELECT DISTINCT G.[CollectionSpecimenID]
      ,G.[IdentificationUnitID]
      ,G.[AnalysisDate]
      ,G.[Geography].ToString()
      ,G.[Geometry].ToString()
      ,G.[ResponsibleName]
      ,G.[ResponsibleAgentURI]
      ,G.[Notes]
  FROM [#project#].[CacheIdentificationUnit] U, ' +  dbo.SourceDatabase() + '.dbo.IdentificationUnitGeoAnalysis G
  where U.CollectionSpecimenID = G.CollectionSpecimenID
  and G.IdentificationUnitID = U.IdentificationUnitID
    
if not (select P.CoordinatePrecision from dbo.ProjectPublished P where P.ProjectID = [#project#].ProjectID())  is null
begin
	-- removing geography for all entries
	update L 
	set [Geography] = NULL
	from [#project#].[CacheIdentificationUnitGeoAnalysis] L
end')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishIdentificationUnitGeoAnalysis] TO [CacheAdmin_#project#] 
GO


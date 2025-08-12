

--#####################################################################################################################
--######   CacheProjectReference  #####################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheProjectReference') = 0
begin
CREATE TABLE [#project#].[CacheProjectReference](
	[ProjectID] [int] NOT NULL,
	[ReferenceTitle] [nvarchar](255) NOT NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[ReferenceType] [nvarchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheProjectReference] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReferenceTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GRANT SELECT ON [#project#].[CacheProjectReference] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheProjectReference] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheProjectReference] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheProjectReference] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   procPublishProjectReference  ###############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishProjectReference] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProjectReference]
*/
truncate table [#project#].CacheProjectReference

INSERT INTO [#project#].[CacheProjectReference]
           ([ProjectID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[ReferenceType]
      ,[Notes])
SELECT DISTINCT [ProjectID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[ReferenceType]
      ,[Notes]
  FROM ' + dbo.ProjectsDatabase() + '.DBO.ProjectReference R
  where R.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

GRANT EXEC ON [#project#].[procPublishProjectReference] TO [CacheAdmin_#project#] 
GO

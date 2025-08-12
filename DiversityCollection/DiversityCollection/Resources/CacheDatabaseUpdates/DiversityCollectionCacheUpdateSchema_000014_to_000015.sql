


--#####################################################################################################################
--######   CacheCollectionSpecimenReference  ##########################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionSpecimenReference') = 0
begin
CREATE TABLE [#project#].[CacheCollectionSpecimenReference](
	[CollectionSpecimenID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[ReferenceTitle] [nvarchar](400) NOT NULL,
	[ReferenceURI] [varchar](500) NULL,
	[IdentificationUnitID] [int] NULL,
	[SpecimenPartID] [int] NULL,
	[ReferenceDetails] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
 CONSTRAINT [PK_CacheCollectionSpecimenReference] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[ReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GRANT SELECT ON [#project#].[CacheCollectionSpecimenReference] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheCollectionSpecimenReference] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheCollectionSpecimenReference] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheCollectionSpecimenReference] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   procPublishCollectionSpecimenReference  ####################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishCollectionSpecimenReference] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenReference]
*/
truncate table [#project#].CacheCollectionSpecimenReference

INSERT INTO [#project#].[CacheCollectionSpecimenReference]
           ([CollectionSpecimenID]
      ,[ReferenceID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[IdentificationUnitID]
      ,[SpecimenPartID]
      ,[ReferenceDetails]
      ,[Notes]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI])
SELECT DISTINCT R.[CollectionSpecimenID]
      ,R.[ReferenceID]
      ,R.[ReferenceTitle]
      ,R.[ReferenceURI]
      ,R.[IdentificationUnitID]
      ,R.[SpecimenPartID]
      ,R.[ReferenceDetails]
      ,R.[Notes]
      ,R.[ResponsibleName]
      ,R.[ResponsibleAgentURI]
 FROM ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimenReference R, 
  [#project#].CacheCollectionSpecimen S
  where R.CollectionSpecimenID = S.CollectionSpecimenID')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


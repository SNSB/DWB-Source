

--#####################################################################################################################
--######   CacheCollectionSpecimenProcessing  #########################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheCollectionSpecimenProcessing') = 0
begin
CREATE TABLE [#project#].[CacheCollectionSpecimenProcessing](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenProcessingID] [int] NOT NULL,
	[ProcessingDate] [datetime] NULL,
	[ProcessingID] [int] NULL,
	[Protocoll] [nvarchar](100) NULL,
	[SpecimenPartID] [int] NULL,
	[ProcessingDuration] [varchar](50) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheCollectionSpecimenProcessing] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenProcessingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GRANT SELECT ON [#project#].[CacheCollectionSpecimenProcessing] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheCollectionSpecimenProcessing] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheCollectionSpecimenProcessing] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheCollectionSpecimenProcessing] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   procCollectionSpecimenProcessing  ##########################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishCollectionSpecimenProcessing] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionSpecimenProcessing]
*/
truncate table [#project#].CacheCollectionSpecimenProcessing

INSERT INTO [#project#].[CacheCollectionSpecimenProcessing]
           (CollectionSpecimenID, 
		   SpecimenProcessingID, 
		   SpecimenPartID, 
		   ProcessingDate, 
		   ProcessingID, 
		   Protocoll, 
		   ProcessingDuration, 
		   ResponsibleName, 
		   ResponsibleAgentURI, 
		   Notes)
SELECT DISTINCT S.[CollectionSpecimenID]
      ,[SpecimenProcessingID]
      ,[SpecimenPartID]
      ,[ProcessingDate]
      ,[ProcessingID]
      ,[Protocoll]
      ,[ProcessingDuration]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[Notes]
  FROM [dbo].[ViewCollectionSpecimenProcessing] S, dbo.ViewCollectionProject P
  where S.CollectionSpecimenID = p.CollectionSpecimenID
  and p.ProjectID = [#project#].ProjectID()')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


GRANT EXEC ON [#project#].[procPublishCollectionSpecimenProcessing] TO [CacheAdmin_#project#] 
GO








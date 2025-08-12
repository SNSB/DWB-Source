

--#####################################################################################################################
--######   CacheProcessing  ###########################################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheProcessing') = 0
begin
CREATE TABLE [#project#].[CacheProcessing](
	[ProcessingID] [int] NOT NULL,
	[ProcessingParentID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[ProcessingURI] [varchar](255) NULL,
	[OnlyHierarchy] [bit] NULL,
 CONSTRAINT [PK_CacheProcessing] PRIMARY KEY CLUSTERED 
(
	[ProcessingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GRANT SELECT ON [#project#].[CacheProcessing] TO [CacheUser_#project#]
GO
GRANT DELETE ON [#project#].[CacheProcessing] TO [CacheAdmin_#project#] 
GO
GRANT UPDATE ON [#project#].[CacheProcessing] TO [CacheAdmin_#project#]
GO
GRANT INSERT ON [#project#].[CacheProcessing] TO [CacheAdmin_#project#]
GO


--#####################################################################################################################
--######   procPublishProcessing  #####################################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishProcessing] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishProcessing]
*/
truncate table [#project#].CacheProcessing

INSERT INTO [#project#].[CacheProcessing]
           (ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI, OnlyHierarchy)
SELECT DISTINCT ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI, OnlyHierarchy
  FROM [dbo].[ViewProcessing]')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO


GRANT EXEC ON [#project#].[procPublishProcessing] TO [CacheAdmin_#project#] 
GO








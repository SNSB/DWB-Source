


--#####################################################################################################################
--######   CacheExternalIdentifier  ###################################################################################
--#####################################################################################################################

if (
select count(*) from INFORMATION_SCHEMA.Tables t 
where t.TABLE_NAME = 'CacheExternalIdentifier'
and t.TABLE_SCHEMA = '#project#')  = 0
begin
CREATE TABLE [#project#].[CacheExternalIdentifier](
	[ID] [int] NOT NULL,
	[ReferencedTable] [nvarchar](128) NOT NULL,
	[ReferencedID] [int] NOT NULL,
	[Type] [nvarchar](50) NULL,
	[Identifier] [nvarchar](500) NULL,
	[URL] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_ExternalIdentifier] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end
GO


--#####################################################################################################################
--######   procPublishExternalIdentifier  #############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE  PROCEDURE [#project#].[procPublishExternalIdentifier] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishExternalIdentifier]
*/
truncate table [#project#].CacheExternalIdentifier

-- Event
INSERT INTO [#project#].[CacheExternalIdentifier]
(ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes)
SELECT        
A.ID, A.ReferencedTable, A.ReferencedID, A.Type, A.Identifier, A.URL, A.Notes
FROM ' +  dbo.SourceDatabase() + '.dbo.ExternalIdentifier A, 
[#project#].CacheCollectionEvent AS E
WHERE  A.ReferencedTable = ''CollectionEvent'' and A.ReferencedID = E.CollectionEventID
')

set @SQL = (@SQL + '
-- Specimen
INSERT INTO [#project#].[CacheExternalIdentifier]
(ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes)
SELECT        
A.ID, A.ReferencedTable, A.ReferencedID, A.Type, A.Identifier, A.URL, A.Notes
FROM ' +  dbo.SourceDatabase() + '.dbo.ExternalIdentifier A, 
[#project#].CacheCollectionSpecimen AS S
WHERE A.ReferencedTable = ''CollectionSpecimen'' and A.ReferencedID = S.CollectionSpecimenID
')

set @SQL = (@SQL + '
-- Part
INSERT INTO [#project#].[CacheExternalIdentifier]
(ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes)
SELECT        
A.ID, A.ReferencedTable, A.ReferencedID, A.Type, A.Identifier, A.URL, A.Notes
FROM ' +  dbo.SourceDatabase() + '.dbo.ExternalIdentifier A, 
[#project#].CacheCollectionSpecimenPart AS P
WHERE A.ReferencedTable = ''CollectionSpecimenPart'' and A.ReferencedID = P.SpecimenPartID
')

set @SQL = (@SQL + '
-- Unit
INSERT INTO [#project#].[CacheExternalIdentifier]
(ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes)
SELECT        
A.ID, A.ReferencedTable, A.ReferencedID, A.Type, A.Identifier, A.URL, A.Notes
FROM ' +  dbo.SourceDatabase() + '.dbo.ExternalIdentifier A, 
[#project#].CacheIdentificationUnit AS U
WHERE A.ReferencedTable = ''IdentificationUnit'' and A.ReferencedID = U.IdentificationUnitID
')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

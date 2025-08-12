

--#####################################################################################################################
--######   Adding TermURI to table CacheIdentification  ###############################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_SCHEMA = '#project#' AND C.TABLE_NAME = 'CacheIdentification' and C.COLUMN_NAME = 'TermURI') = 0
begin
ALTER TABLE [#project#].CacheIdentification ADD [TermURI] [nvarchar](500) NULL
end
GO


--#####################################################################################################################
--######   Grants for CacheCollectionExternalDatasource  ##############################################################
--#####################################################################################################################

GRANT SELECT ON [#project#].CacheCollectionExternalDatasource TO [CacheUser]
GO

GRANT SELECT ON [#project#].CacheCollectionExternalDatasource TO [CacheUser_#project#]
GO

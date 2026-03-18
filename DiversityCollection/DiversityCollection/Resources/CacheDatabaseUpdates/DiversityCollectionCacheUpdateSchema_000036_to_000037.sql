
--#####################################################################################################################
--######   Issue #321  ################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######   New table Parameter   ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CacheParameter' and T.TABLE_SCHEMA = '#project#') = 0
begin

CREATE TABLE [#project#].[CacheParameter](
	[MethodID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[ParameterURI] [varchar](255) NULL,
	[DefaultValue] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_CacheParameter] PRIMARY KEY CLUSTERED 
(
	[ParameterID] ASC,
	[MethodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
/*
-- none of the tables in the CacheDatabase have Description properties, so commenting these out for now
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Method' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheParameter', @level2type=N'COLUMN',@level2name=N'MethodID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Parameter' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheParameter', @level2type=N'COLUMN',@level2name=N'ParameterID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The display text of the parameter' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheParameter', @level2type=N'COLUMN',@level2name=N'DisplayText'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description of the parameter' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheParameter', @level2type=N'COLUMN',@level2name=N'Description'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the parameter containing additional information' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'CacheParameter', @level2type=N'COLUMN',@level2name=N'ParameterURI'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The default value of the parameter' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'DefaultValue'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes for the parameter' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'Notes'
	*/
end
GO 

GRANT SELECT ON [#project#].[CacheParameter] TO [CacheAdmin]
GO
GRANT DELETE ON [#project#].[CacheParameter] TO [CacheAdmin] 
GO
GRANT UPDATE ON [#project#].[CacheParameter] TO [CacheAdmin]
GO
GRANT INSERT ON [#project#].[CacheParameter] TO [CacheAdmin]
GO





--#####################################################################################################################
--######   New procedure procPublishParameter  ########################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE PROCEDURE [#project#].[procPublishParameter] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishParameter]
*/
truncate table [#project#].CacheParameter

INSERT INTO [#project#].[CacheParameter]
   ([MethodID]
      ,[ParameterID]
      ,[DisplayText]
      ,[Description]
      ,[ParameterURI]
      ,[DefaultValue]
      ,[Notes]
)
SELECT [MethodID]
      ,[ParameterID]
      ,[DisplayText]
      ,[Description]
      ,[ParameterURI]
      ,[DefaultValue]
      ,[Notes]
  FROM '  +  dbo.SourceDatabase()  + '.DBO.Parameter')
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
exec sp_executesql @SQL
end catch

GO

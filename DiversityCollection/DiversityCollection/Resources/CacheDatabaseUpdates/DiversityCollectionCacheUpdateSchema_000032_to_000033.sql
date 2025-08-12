
--#####################################################################################################################
--######   ProjectEventProperty for blocking Property  ################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectEventProperty' and T.TABLE_SCHEMA = '#project#') = 0
begin
	CREATE TABLE [#project#].[ProjectEventProperty](
		[PropertyID] int NOT NULL,
	 CONSTRAINT [PK_ProjectEventProperty] PRIMARY KEY CLUSTERED 
	(
		[PropertyID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the EventProperty that should be published' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'ProjectEventProperty', @level2type=N'COLUMN',@level2name=N'PropertyID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EventProperty that should be published' , @level0type=N'SCHEMA',@level0name=N'#project#', @level1type=N'TABLE',@level1name=N'ProjectEventProperty'
end
GO 

GRANT SELECT ON [#project#].[ProjectEventProperty] TO [CacheUser]
GO
GRANT DELETE ON [#project#].[ProjectEventProperty] TO [CacheAdmin] 
GO
GRANT UPDATE ON [#project#].[ProjectEventProperty] TO [CacheAdmin]
GO
GRANT INSERT ON [#project#].[ProjectEventProperty] TO [CacheAdmin]
GO

--#####################################################################################################################
--######   Transfer existing data into ProjectEventProperty   #########################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'INSERT INTO [#project#].ProjectEventProperty (PropertyID) 
SELECT E.PropertyID 
FROM ' +  dbo.SourceDatabase() + '.dbo.CollectionEventProperty E 
INNER JOIN ' +  dbo.SourceDatabase() + '.dbo.CollectionSpecimen S ON S.CollectionEventID = E.CollectionEventID 
INNER JOIN ' +  dbo.SourceDatabase() + '.dbo.CollectionProject P ON S.CollectionSpecimenID = P.CollectionSpecimenID 
AND P.ProjectID = [#project#].ProjectID() 
AND E.PropertyID NOT IN (SELECT PropertyID FROM [#project#].ProjectEventProperty) 
GROUP BY E.PropertyID ')
begin try
exec sp_executesql @SQL
end try
begin catch
exec sp_executesql @SQL
end catch
GO


--#####################################################################################################################
--######   procPublishCollectionEventProperty - blocking EventProperty  ###############################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'ALTER PROCEDURE [#project#].[procPublishCollectionEventProperty] 
AS
/*
-- TEST
EXECUTE  [#project#].[procPublishCollectionEventProperty]
*/
truncate table [#project#].CacheCollectionEventProperty

INSERT INTO [#project#].[CacheCollectionEventProperty]
           ([CollectionEventID]
      ,[PropertyID]
      ,[DisplayText]
      ,[PropertyURI]
      ,[PropertyHierarchyCache]
      ,[PropertyValue]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,[Notes]
      ,[AverageValueCache])
SELECT DISTINCT P.[CollectionEventID]
      ,P.[PropertyID]
      ,[DisplayText]
      ,[PropertyURI]
      ,[PropertyHierarchyCache]
      ,[PropertyValue]
      ,[ResponsibleName]
      ,[ResponsibleAgentURI]
      ,P.[Notes]
      ,[AverageValueCache]
  FROM ' +  dbo.SourceDatabase() + '.[dbo].[CollectionEventProperty] P 
  INNER JOIN [#project#].[CacheCollectionEvent] E ON E.CollectionEventID = P.CollectionEventID 
  INNER JOIN [#project#].[ProjectEventProperty] PE ON PE.PropertyID = P.PropertyID')
  
begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 7, 80000)
exec sp_executesql @SQL
end catch

GO


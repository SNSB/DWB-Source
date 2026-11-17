
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.35'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Missing View ViewDiversityWorkbenchModule for linked server  ################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.VIEWS v where v.TABLE_NAME = 'ViewDiversityWorkbenchModule') = 0
begin
	declare @SQL nvarchar(max)
	set @SQL = (select 'CREATE VIEW [dbo].[ViewDiversityWorkbenchModule]
	AS
	SELECT dbo.DiversityWorkbenchModule() AS DiversityWorkbenchModule
	')
	begin try
	exec sp_executesql @SQL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the DiversityWorkbench module' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDiversityWorkbenchModule', @level2type=N'COLUMN',@level2name=N'DiversityWorkbenchModule'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides the name of the DiversityWorkbench module as defined in function DiversityWorkbenchModule' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDiversityWorkbenchModule'
	end try
	begin catch
	set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 80000)
	exec sp_executesql @SQL
	end catch
end
GO

GRANT SELECT ON [dbo].[ViewDiversityWorkbenchModule] TO [DiversityWorkbenchUser]
GO



--#####################################################################################################################
--######  Missing Grants  #############################################################################################
--#####################################################################################################################

GRANT INSERT ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchEditor]
GO

GRANT UPDATE ON [dbo].[SamplingPlotProperty] TO [DiversityWorkbenchEditor]
GO

GRANT INSERT ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchEditor]
GO

GRANT UPDATE ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchEditor]
GO

GRANT DELETE ON [dbo].[SamplingPlotResource] TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######  Missing Descriptions  #######################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  Missing Descriptions: Tables  ###############################################################################
--#####################################################################################################################


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The proxy table for entries in the module DiversityProjects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectProxy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The proxy table for entries in the module DiversityProjects',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectProxy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The projects a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectUser'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The projects a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectUser'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The login of the user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectUser',
@level2type=N'COLUMN', @level2name=N'LoginName'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The login of the user',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectUser',
@level2type=N'COLUMN', @level2name=N'LoginName'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the project the user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectUser',
@level2type=N'COLUMN', @level2name=N'ProjectID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the project the user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ProjectUser',
@level2type=N'COLUMN', @level2name=N'ProjectID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Properties of the site of the sampling plot as stored in DiversityScientificTerms',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Property'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Properties of the site of the sampling plot as stored in DiversityScientificTerms',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Property'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The default measurement unit for measured properties',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Property',
@level2type=N'COLUMN', @level2name=N'DefaultMeasurementUnit'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The default measurement unit for measured properties',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Property',
@level2type=N'COLUMN', @level2name=N'DefaultMeasurementUnit'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The link to the entry in a DiversityScientificTerms database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Property',
@level2type=N'COLUMN', @level2name=N'PropertyURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The link to the entry in a DiversityScientificTerms database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'Property',
@level2type=N'COLUMN', @level2name=N'PropertyURI'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'For data replication - the links to publishers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ReplicationPublisher'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'For data replication - the links to publishers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'ReplicationPublisher'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Properties of the site of a sampling plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'SamplingPlotProperty'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Properties of the site of a sampling plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'SamplingPlotProperty'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Resouces of the sampling plot, e.g. images',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'SamplingPlotResource'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Resouces of the sampling plot, e.g. images',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'SamplingPlotResource'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Proxy table for users with links to the module DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'UserProxy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Proxy table for users with links to the module DiversityAgents',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'UserProxy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'obsolete - URI of a user in a remote module, e.g. refering to UserInfo.UserID in database DiversityUsers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'UserProxy',
@level2type=N'COLUMN', @level2name=N'UserURI'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'obsolete - URI of a user in a remote module, e.g. refering to UserInfo.UserID in database DiversityUsers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'TABLE', @level1name=N'UserProxy',
@level2type=N'COLUMN', @level2name=N'UserURI'
END CATCH;
GO


--#####################################################################################################################
--######  Missing Descriptions: Views   ###############################################################################
--#####################################################################################################################


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'IDs of the plots a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PlotID_UserAvailable'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'IDs of the plots a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'PlotID_UserAvailable'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The list of the projects a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The list of the projects a user has access to',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ProjectList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Geographical properties of a plot extracted from column PlotGeography_Cache including up to 4 hierarchy levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Geographical properties of a plot extracted from column PlotGeography_Cache including up to 4 hierarchy levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'SamplingPlotGeoInfo'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Providing the BaseURL for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Providing the BaseURL for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewBaseURL'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Providing the content of table SamplingPlot for linked servers with conversion of geography tables to text',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlot'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Providing the content of table SamplingPlot for linked servers with conversion of geography tables to text',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlot'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Providing the result of SamplingPlotAllLocalisations for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotAllLocalisations'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Providing the result of SamplingPlotAllLocalisations for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotAllLocalisations'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Providing the result of SamplingPlotGeoInfo for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Providing the result of SamplingPlotGeoInfo for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotGeoInfo'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The plot hierarchy with up to 5 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The plot hierarchy with up to 5 levels',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy1'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy1'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy2'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy2'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy3'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy3'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy4'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy4'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy5'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Basic for for ViewSamplingPlotHierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotHierarchy5'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Providing the result of SamplingPlotList for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Providing the result of SamplingPlotList for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Providing the result of SamplingPlotListForCurrentProject for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotListForCurrentProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Providing the result of SamplingPlotListForCurrentProject for linked servers',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'VIEW', @level1name=N'ViewSamplingPlotListForCurrentProject'
END CATCH;
GO

--#####################################################################################################################
--######  Missing Descriptions: Functions  ############################################################################
--#####################################################################################################################



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Provides the basic address for accessing the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'BaseURL'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Provides the basic address for accessing the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'BaseURL'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the current project selected by a user or first project available',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DefaultProjectID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the current project selected by a user or first project available',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DefaultProjectID'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The name of the module within the DWB',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityWorkbenchModule'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The name of the module within the DWB',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'DiversityWorkbenchModule'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The website providing information concerning the privacy consent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'PrivacyConsentInfo'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The website providing information concerning the privacy consent',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'PrivacyConsentInfo'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the localisations of a plot included those inherited from a parent plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotAllLocalisations'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the localisations of a plot included those inherited from a parent plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotAllLocalisations'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the properties of a plot included those inherited from a parent plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotAllProperties'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the properties of a plot included those inherited from a parent plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotAllProperties'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotChildNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotChildNodes'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all plots togeher with the geography possibly derived from the parent plot.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotGeography'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all plots togeher with the geography possibly derived from the parent plot.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotGeography'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Content of column PlotGeography_Cache if available within the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotGeography',
@level2type=N'COLUMN', @level2name=N'Geography'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Content of column PlotGeography_Cache if available within the hierarchy',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotGeography',
@level2type=N'COLUMN', @level2name=N'Geography'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the plots related to the given plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotHierarchy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the plots related to the given plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotHierarchy'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the SamplingPlot items related to the given SamplingPlot.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotList'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the SamplingPlot items related to the given SamplingPlot.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotList'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The combined Identifiers of the whole hierarchy related to a plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotList',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The combined Identifiers of the whole hierarchy related to a plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotList',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the SamplingPlot items related to the given project.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotListForCurrentProject'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the SamplingPlot items related to the given project.',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotListForCurrentProject'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The combined Identifiers of the whole hierarchy related to a plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotListForCurrentProject',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The combined Identifiers of the whole hierarchy related to a plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotListForCurrentProject',
@level2type=N'COLUMN', @level2name=N'DisplayText'
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the plots related to the given plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotSuperiorNodes'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Returns a table that lists all the plots related to the given plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotSuperiorNodes'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The combined Identifiers of the whole hierarchy related to a plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotSuperiorNodes',
@level2type=N'COLUMN', @level2name=N'PlotHierarchyCache'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The combined Identifiers of the whole hierarchy related to a plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'SamplingPlotSuperiorNodes',
@level2type=N'COLUMN', @level2name=N'PlotHierarchyCache'
END CATCH;
GO



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Version'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The version of the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'Version'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The version of the client compatible with the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'VersionClient'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The version of the client compatible with the database',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'FUNCTION', @level1name=N'VersionClient'
END CATCH;
GO

--#####################################################################################################################
--######  Missing Descriptions: Procedures  ###########################################################################
--#####################################################################################################################



BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'Creation of a copy of a dataset in table SamplingProject  including the content in depending tables',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertSamplingPlotCopy'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'Creation of a copy of a dataset in table SamplingProject  including the content in depending tables',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertSamplingPlotCopy'
END CATCH;
GO


BEGIN TRY
EXEC sys.sp_addextendedproperty 
@name=N'MS_Description', @value=N'The ID of the original plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertSamplingPlotCopy',
@level2type=N'PARAMETER', @level2name=N'@OriginalPlotID'
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty 
@name=N'MS_Description', @value=N'The ID of the original plot',
@level0type=N'SCHEMA', @level0name=N'dbo',
@level1type=N'PROCEDURE', @level1name=N'procInsertSamplingPlotCopy',
@level2type=N'PARAMETER', @level2name=N'@OriginalPlotID'
END CATCH;
GO

--#####################################################################################################################
--######  Missing Descriptions: Roles  ################################################################################
--#####################################################################################################################

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Permissions as Editor but with permission to delete data in table SamplingPlot',
@level0type = N'USER', @level0name = 'DataManager';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Permissions as Editor but with permission to delete data in table SamplingPlot',
@level0type = N'USER', @level0name = 'DataManager';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role with full permission to the content',
@level0type = N'USER', @level0name = 'DiversityWorkbenchAdministrator';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role with full permission to the content',
@level0type = N'USER', @level0name = 'DiversityWorkbenchAdministrator';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role for editing the content',
@level0type = N'USER', @level0name = 'DiversityWorkbenchEditor';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role for editing the content',
@level0type = N'USER', @level0name = 'DiversityWorkbenchEditor';
END CATCH;
GO

BEGIN TRY
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Role with read only access',
@level0type = N'USER', @level0name = 'DiversityWorkbenchUser';
END TRY
BEGIN CATCH
EXEC sys.sp_updateextendedproperty @name = N'MS_Description', @value = N'Role with read only access',
@level0type = N'USER', @level0name = 'DiversityWorkbenchUser';
END CATCH;
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '04.04.00' 
END

GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.36'
END

GO

SELECT [dbo].[Version] ()  


declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.59'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   CollectionEvent   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionEvent ADD [LocalityVerbatim] [nvarchar](max) NULL
GO
ALTER TABLE CollectionEvent ADD [CollectionEndDay] [dbo].[dtDay] NULL
GO
ALTER TABLE CollectionEvent ADD [CollectionEndMonth] [dbo].[dtMonth] NULL
GO
ALTER TABLE CollectionEvent ADD [CollectionEndYear] [smallint] NULL
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Locality as given in historical context, documents and labels' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEvent', @level2type=N'COLUMN',@level2name=N'LocalityVerbatim'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The day of the date of the event or when the collection event ended' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEvent', @level2type=N'COLUMN',@level2name=N'CollectionEndDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The month of the date of the event or when the collection event ended' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEvent', @level2type=N'COLUMN',@level2name=N'CollectionEndMonth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of the date of the event or when the collection event ended' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEvent', @level2type=N'COLUMN',@level2name=N'CollectionEndYear'
GO

ALTER TABLE CollectionEvent_log ADD [LocalityVerbatim] [nvarchar](max) NULL
GO
ALTER TABLE CollectionEvent_log ADD [CollectionEndDay] [dbo].[dtDay] NULL
GO
ALTER TABLE CollectionEvent_log ADD [CollectionEndMonth] [dbo].[dtMonth] NULL
GO
ALTER TABLE CollectionEvent_log ADD [CollectionEndYear] [smallint] NULL
GO


/****** Object:  Trigger [dbo].[trgDelCollectionEvent]    Script Date: 27.05.2015 11:56:21 ******/
ALTER TRIGGER [dbo].[trgDelCollectionEvent] ON [dbo].[CollectionEvent] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.1.1 */ 
/*  Date: 11.01.2010  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEvent_Log (CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, 
CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionDateCategory, CollectionTime, CollectionTimeSpan, 
LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, CollectingMethod, Notes, CountryCache, 
DataWithholdingReason, RowGUID, ReferenceDetails, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, 
 LogState, LocalityVerbatim, CollectionEndDay, CollectionEndMonth, CollectionEndYear) 
SELECT D.CollectionEventID, D.Version, D.SeriesID, D.CollectorsEventNumber, D.CollectionDate, D.CollectionDay, 
D.CollectionMonth, D.CollectionYear, D.CollectionDateSupplement, D.CollectionDateCategory, D.CollectionTime, D.CollectionTimeSpan, 
D.LocalityDescription, D.HabitatDescription, D.ReferenceTitle, D.ReferenceURI, D.CollectingMethod, D.Notes, D.CountryCache, 
D.DataWithholdingReason, D.RowGUID, D.ReferenceDetails, D.LogCreatedWhen, D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, 
 'D', D.LocalityVerbatim, D.CollectionEndDay, D.CollectionEndMonth, D.CollectionEndYear
FROM DELETED D

GO


/****** Object:  Trigger [dbo].[trgUpdCollectionEvent]    Script Date: 22.04.2015 13:39:45 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionEvent] ON [dbo].[CollectionEvent]  
 FOR UPDATE AS 
 /*  Created by DiversityWorkbench Administration.  */  
 /*  DiversityWorkbenchMaintenance  2.0.0.3 */  
 /*  Date: 30.08.2007  */  
 if not update(Version)  
 BEGIN  /* setting the version in the main table */  
	 DECLARE @i int  
	 DECLARE @ID int 
	 DECLARE @Version int  
	 set @i = (select count(*) from deleted)   
	 if @i = 1  
	 BEGIN     
		 SET  @ID = (SELECT CollectionEventID FROM deleted)    
		 EXECUTE procSetVersionCollectionEvent @ID 
	 END   
	 Update CollectionEvent set CollectionDate = case when isdate (cast (	cast( CollectionEvent.CollectionDay as varchar ) + '.' +  	
	 cast( CollectionEvent.CollectionMonth as varchar )  + '.' +  	
	 cast( CollectionEvent.CollectionYear as varchar ) as varchar) ) = 1 then cast ( 	
	 cast( CollectionEvent.CollectionDay as varchar ) + '.' +  	
	 cast( CollectionEvent.CollectionMonth as varchar )  + '.' +  	
	 cast( CollectionEvent.CollectionYear as varchar ) as datetime)  
	 else null end 
	 FROM CollectionEvent, deleted  
	 where 1 = 1  AND CollectionEvent.CollectionEventID = deleted.CollectionEventID   
	 
	 /* saving the original dataset in the logging table */  
	 INSERT INTO CollectionEvent_Log (
	 CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, 
	 CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionDateCategory, CollectionTime, 
	 CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, ReferenceDetails, 
	 CollectingMethod, Notes, CountryCache, DataWithholdingReason, RowGUID,  LogCreatedWhen, 
	 LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, LogState, LocalityVerbatim, CollectionEndDay, CollectionEndMonth, CollectionEndYear)  
	 SELECT D.CollectionEventID, D.Version, D.SeriesID, D.CollectorsEventNumber, D.CollectionDate, 
	 D.CollectionDay, D.CollectionMonth, D.CollectionYear, D.CollectionDateSupplement, D.CollectionDateCategory, D.CollectionTime, 
	 D.CollectionTimeSpan, D.LocalityDescription, D.HabitatDescription, D.ReferenceTitle, D.ReferenceURI, D.ReferenceDetails, 
	 D.CollectingMethod, D.Notes, D.CountryCache, D.DataWithholdingReason, D.RowGUID,  D.LogCreatedWhen, 
	 D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, 'U', D.LocalityVerbatim, D.CollectionEndDay, D.CollectionEndMonth, D.CollectionEndYear
	 FROM DELETED  D
 END  
 
 Update CollectionEvent set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user 
 FROM CollectionEvent, deleted  
 where 1 = 1  AND CollectionEvent.CollectionEventID = deleted.CollectionEventID



GO


--#####################################################################################################################
--######   CollectionSpecimenRelationInvers   ######################################################################################
--#####################################################################################################################

CREATE VIEW CollectionSpecimenRelationInvers
AS 
SELECT TOP 100 PERCENT substring(R.RelatedSpecimenURI, len(dbo.baseURL()) + 1, 255) AS CollectionSpecimenID,
R.CollectionSpecimenID AS RelatedSpecimenCollectionSpecimenID, S.AccessionNumber AS RelatedSpecimenAccessionNumber, 
R.RelationType, R.RelatedSpecimenCollectionID AS RelatedSpecimenCollectionID, 
R.Notes
FROM CollectionSpecimenRelation R, CollectionSpecimen S  
WHERE (R.IsInternalRelationCache = 1)  
AND S.CollectionSpecimenID = R.CollectionSpecimenID 
and not R.RelatedSpecimenURI IS NULL
and R.RelatedSpecimenURI like dbo.BaseURL() + '%'
ORDER BY RelatedSpecimenDisplayText

GO


GRANT SELECT ON CollectionSpecimenRelationInvers TO [User]
GO

--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.09' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.60'
END

GO



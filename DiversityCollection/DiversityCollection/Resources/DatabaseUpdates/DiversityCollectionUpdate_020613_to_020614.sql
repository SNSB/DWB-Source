declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.13'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis - add CASCADE   #############################
--#####################################################################################################################

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] DROP CONSTRAINT [FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID], [AnalysisID], [AnalysisNumber])
REFERENCES [dbo].[IdentificationUnitAnalysis] ([CollectionSpecimenID], [IdentificationUnitID], [AnalysisID], [AnalysisNumber])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethod] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethod_IdentificationUnitAnalysis]
GO


--#####################################################################################################################
--######   FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod - add CASCADE   ##############
--#####################################################################################################################

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] DROP CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod]
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID], [MethodID], [AnalysisID], [AnalysisNumber], [MethodMarker])
REFERENCES [dbo].[IdentificationUnitAnalysisMethod] ([CollectionSpecimenID], [IdentificationUnitID], [MethodID], [AnalysisID], [AnalysisNumber], [MethodMarker])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IdentificationUnitAnalysisMethodParameter] CHECK CONSTRAINT [FK_IdentificationUnitAnalysisMethodParameter_IdentificationUnitAnalysisMethod]
GO

--#####################################################################################################################
--######   Add trgInsCollectionEvent to ensure correct entries in colum CollectionDate   ##############################
--#####################################################################################################################

BEGIN TRY
DROP TRIGGER [dbo].[trgInsCollectionEvent]
END TRY
BEGIN CATCH
END CATCH
GO

CREATE TRIGGER [dbo].[trgInsCollectionEvent] ON [dbo].[CollectionEvent] 
FOR INSERT AS

/*  Date: 18.05.2018  */ 

/* setting the date fields */ 

Update CollectionEvent set CollectionDate = case when E.CollectionMonth is null or E.CollectionDay is null or E.CollectionYear is null then null 
else case when ISDATE(convert(varchar(40), cast(E.CollectionYear as varchar) + '-' 
+ case when I.CollectionMonth < 10 then '0' else '' end + cast(I.CollectionMonth as varchar)  + '-' 
+ case when I.CollectionDay < 10 then '0' else '' end + cast(I.CollectionDay as varchar) + 'T00:00:00.000Z', 127)) = 1
then cast(convert(varchar(40), cast(I.CollectionYear as varchar) + '-' 
+ case when I.CollectionMonth < 10 then '0' else '' end + cast(I.CollectionMonth as varchar)  + '-' 
+ case when I.CollectionDay < 10 then '0' else '' end + cast(I.CollectionDay as varchar) + 'T00:00:00.000Z', 127) as datetime)
else null end end 
FROM CollectionEvent E, inserted I  
where 1 = 1  AND E.CollectionEventID = I.CollectionEventID   
and isdate (convert(varchar(40), cast(E.CollectionYear as varchar) + '-' 
+ case when I.CollectionMonth < 10 then '0' else '' end + cast(I.CollectionMonth as varchar)  + '-' 
+ case when I.CollectionDay < 10 then '0' else '' end + cast(I.CollectionDay as varchar) + 'T00:00:00.000Z', 127)) = 1
AND I.CollectionYear > 1760
AND I.CollectionMonth between 1 and 12
AND I.CollectionDay between 1 and 31
AND I.CollectionDate IS NULL
GO


--#####################################################################################################################
--######   trgUpdCollectionEvent - cast to ISO-format to ensure correct entries in colum CollectionDate   #############
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdCollectionEvent] ON [dbo].[CollectionEvent]  
 FOR UPDATE AS 
 /*  Created by DiversityWorkbench Administration.  */  
 /*  DiversityWorkbenchMaintenance  2.0.0.3 */  
 /*  Date: 30.08.2007  */  
 /*  Changed: 18.05.2018 - cast date to ISO date for update of CollectionDate */
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
	 Update CollectionEvent set CollectionDate = case when E.CollectionMonth is null or E.CollectionDay is null or E.CollectionYear is null then null 
	 else case when ISDATE(cast(E.CollectionYear as varchar) + '-' + case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar) + '-'  + case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000') = 1
	 then cast(cast(E.CollectionYear as varchar) + '-' + case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar) + '-'  + case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000' as datetime)
	 else null end end 
	 FROM CollectionEvent E, deleted D 
	 where 1 = 1  AND E.CollectionEventID = D.CollectionEventID   
	 and isdate (cast(E.CollectionYear as varchar) + '-' + case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar) + '-'  + case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000') = 1
	 AND E.CollectionYear > 1760
	 AND E.CollectionMonth between 1 and 12
	 AND E.CollectionDay between 1 and 31

	 /* saving the original dataset in the logging table */  
	 INSERT INTO CollectionEvent_Log (
	 CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, 
	 CollectionDay, CollectionMonth, CollectionYear, CollectionDateSupplement, CollectionDateCategory, CollectionTime, 
	 CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, ReferenceURI, ReferenceDetails, 
	 CollectingMethod, Notes, CountryCache, DataWithholdingReason, RowGUID,  LogCreatedWhen, 
	 LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, LogState, LocalityVerbatim, CollectionEndDay, CollectionEndMonth, CollectionEndYear, DataWithholdingReasonDate)  
	 SELECT D.CollectionEventID, D.Version, D.SeriesID, D.CollectorsEventNumber, D.CollectionDate, 
	 D.CollectionDay, D.CollectionMonth, D.CollectionYear, D.CollectionDateSupplement, D.CollectionDateCategory, D.CollectionTime, 
	 D.CollectionTimeSpan, D.LocalityDescription, D.HabitatDescription, D.ReferenceTitle, D.ReferenceURI, D.ReferenceDetails, 
	 D.CollectingMethod, D.Notes, D.CountryCache, D.DataWithholdingReason, D.RowGUID,  D.LogCreatedWhen, 
	 D.LogCreatedBy, D.LogUpdatedWhen, D.LogUpdatedBy, 'U', D.LocalityVerbatim, D.CollectionEndDay, D.CollectionEndMonth, D.CollectionEndYear, D.DataWithholdingReasonDate
	 FROM DELETED  D
 END  
 
 Update CollectionEvent set LogUpdatedWhen = getdate(), LogUpdatedBy =  suser_sname()
 FROM CollectionEvent, deleted  
 where 1 = 1  AND CollectionEvent.CollectionEventID = deleted.CollectionEventID
 GO

--#####################################################################################################################
--######   Correction of dates with wrong format (Month <> Day)    ####################################################
--#####################################################################################################################

Update CollectionEvent set CollectionDate = cast(cast(E.CollectionYear as varchar) + '-' + case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar) + '-'  + case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000' as datetime)
FROM CollectionEvent E
where not CollectionDate is null
and E.CollectionYear > 1760
AND E.CollectionMonth between 1 and 12
AND E.CollectionDay between 1 and 31
and ISDATE(cast(E.CollectionYear as varchar) + '-' + case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar) + '-'  + case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000') = 1
and convert(varchar(40), CollectionDate, 126) <> cast(E.CollectionYear as varchar) + '-' + case when E.CollectionMonth < 10 then '0' else '' end + cast(E.CollectionMonth as varchar) + '-'  + case when E.CollectionDay < 10 then '0' else '' end + cast(E.CollectionDay as varchar) + 'T00:00:00.000'
and Month(CollectionDate) = E.CollectionDay
and Day(CollectionDate) = E.CollectionMonth

GO

--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.09.00' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.14'
END

GO


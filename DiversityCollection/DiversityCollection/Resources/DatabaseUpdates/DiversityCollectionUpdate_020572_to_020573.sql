declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.72'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   public database   ##########################################################################################
--#####################################################################################################################
if (select count(*) from [CollSpecimenRelationType_Enum] where Code = 'public database') = 0
begin
INSERT INTO [dbo].[CollSpecimenRelationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('public database'
           ,'public database'
           ,'public database'
           ,1)
end
GO


--#####################################################################################################################
--######   CollRetrievalType_Enum       ###############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollRetrievalType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL DEFAULT (newsequentialid()),
	[Icon] [image] NULL,
 CONSTRAINT [PK_CollectionRetrievalTypes] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[CollRetrievalType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_CollRetrievalType_Enum_CollRetrievalType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[CollRetrievalType_Enum] ([Code])
GO

ALTER TABLE [dbo].[CollRetrievalType_Enum] CHECK CONSTRAINT [FK_CollRetrievalType_Enum_CollRetrievalType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A symbol representing this entry in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum', @level2type=N'COLUMN',@level2name=N'Icon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The types data about organisms were retrieved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollRetrievalType_Enum'
GO


ALTER TABLE [dbo].[IdentificationUnit] ADD [RetrievalType] [nvarchar](50) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the data about the unit were retrieved, e.g. observation, literature' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnit', @level2type=N'COLUMN',@level2name=N'RetrievalType'
GO

ALTER TABLE [dbo].[IdentificationUnit]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnit_CollRetrievalType_Enum] FOREIGN KEY([RetrievalType])
REFERENCES [dbo].[CollRetrievalType_Enum] ([Code])
GO

ALTER TABLE [dbo].[IdentificationUnit] CHECK CONSTRAINT [FK_IdentificationUnit_CollRetrievalType_Enum]
GO


INSERT INTO [dbo].[CollRetrievalType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('human observation'
           ,'Object observed by a group or person'
           ,'human observation'
           ,1)
GO

INSERT INTO [dbo].[CollRetrievalType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('machine observation'
           ,'multimedia data recorded an autonomous system, e.g. a camera with presence detector or the like'
           ,'machine observation'
           ,1)
GO

INSERT INTO [dbo].[CollRetrievalType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('literature'
           ,'Information taken from the literature'
           ,'literature'
           ,1)
GO




--#####################################################################################################################
--######   IdentificationUnit_Core   ##################################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[IdentificationUnit_Core]
AS
SELECT     U.CollectionSpecimenID, U.IdentificationUnitID, U.LastIdentificationCache, 
                      U.FamilyCache, U.OrderCache, U.TaxonomicGroup, U.OnlyObserved, 
                      U.RelatedUnitID, U.RelationType, U.ColonisedSubstratePart, 
                      U.LifeStage, U.Gender, U.NumberOfUnits, U.ExsiccataNumber, 
                      U.ExsiccataIdentification, U.UnitIdentifier, U.UnitDescription, 
                      U.Circumstances, U.DisplayOrder, U.Notes, U.HierarchyCache, U.RetrievalType
FROM         dbo.IdentificationUnit U INNER JOIN
                      dbo.CollectionSpecimenID_UserAvailable A ON 
                      U.CollectionSpecimenID = A.CollectionSpecimenID

GO



--#####################################################################################################################
--######   trgUpdCollectionEvent   ####################################################################################
--#####################################################################################################################

DROP TRIGGER [dbo].[trgUpdCollectionEvent]
GO

BEGIN TRY
Update CollectionEvent set CollectionDate = case when E.CollectionMonth is null or E.CollectionDay is null or E.CollectionYear is null then null 
else case when ISDATE(cast(E.CollectionMonth as varchar) + '.' + cast(E.CollectionDay as varchar)  + '.' + cast(E.CollectionYear as varchar)) = 1
then cast(cast(E.CollectionMonth as varchar) + '.' + cast(E.CollectionDay as varchar)  + '.' + cast(E.CollectionYear as varchar) as datetime)
else null end end 
FROM CollectionEvent E
END TRY
BEGIN CATCH
END CATCH
GO


CREATE TRIGGER [dbo].[trgUpdCollectionEvent] ON [dbo].[CollectionEvent]  
 FOR UPDATE AS 
 /*  Created by DiversityWorkbench Administration.  */  
 /*  DiversityWorkbenchMaintenance  2.0.0.3 */  
 /*  Date: 30.08.2007  */  
 /*  Changed: 01.12.2015 - sysuser instead of user */
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
	 else case when ISDATE(cast(E.CollectionMonth as varchar) + '.' + cast(E.CollectionDay as varchar)  + '.' + cast(E.CollectionYear as varchar)) = 1
	 then cast(cast(E.CollectionMonth as varchar) + '.' + cast(E.CollectionDay as varchar)  + '.' + cast(E.CollectionYear as varchar) as datetime)
	 else null end end 
	 FROM CollectionEvent E, deleted  
	 where 1 = 1  AND E.CollectionEventID = deleted.CollectionEventID   
	 
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
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.73'
END

GO


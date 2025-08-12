
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.29'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   CollectionSpecimenTransaction   ######################################################################################
--#####################################################################################################################


ALTER TABLE CollectionSpecimenTransaction ADD AccessionNumber nvarchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Accession number that has been assigen to the part of the specimen e.g. in connection with a former inventory.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenTransaction', @level2type=N'COLUMN',@level2name=N'AccessionNumber'
GO

ALTER TABLE CollectionSpecimenTransaction_log ADD AccessionNumber nvarchar(255) NULL
GO

/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenTransaction]    Script Date: 03/07/2013 10:11:51 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenTransaction_Log (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, AccessionNumber, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.TransactionID, deleted.SpecimenPartID, deleted.IsOnLoan, deleted.AccessionNumber, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionSpecimenTransaction
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenTransaction, deleted 
where 1 = 1 
AND CollectionSpecimenTransaction.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenTransaction.TransactionID = deleted.TransactionID
AND CollectionSpecimenTransaction.SpecimenPartID = deleted.SpecimenPartID

GO


/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenTransaction]    Script Date: 03/07/2013 10:16:05 ******/
ALTER TRIGGER [dbo].[trgDelCollectionSpecimenTransaction] ON [dbo].[CollectionSpecimenTransaction] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenTransaction_Log (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, AccessionNumber, RowGUID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.TransactionID, deleted.SpecimenPartID, deleted.IsOnLoan, deleted.AccessionNumber, deleted.RowGUID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   embargo   ######################################################################################
--#####################################################################################################################

INSERT INTO [CollTransactionType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable])
     VALUES
           ('embargo'
           ,'Temporary data embargo for specimen that should not be published within the specified period'
           ,'embargo'
           ,1)
GO



--#####################################################################################################################
--######   [trgUpdCollectionEventLocalisation]   ######################################################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdCollectionEventLocalisation] ON [dbo].[CollectionEventLocalisation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 28.08.2007  */ 
/*  Corrected: 07.03.2013  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionEventID FROM deleted)
   EXECUTE procSetVersionCollectionEvent @ID
   SET @Version = (SELECT Version FROM CollectionEvent WHERE CollectionEventID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, 
LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, 
ResponsibleAgentURI, RecordingMethod, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion,  LogState) 
SELECT D.CollectionEventID, D.LocalisationSystemID, D.Location1, D.Location2, D.LocationAccuracy, D.LocationNotes, 
D.DeterminationDate, D.DistanceToLocation, D.DirectionToLocation, D.ResponsibleName, D.ResponsibleAgentURI, D.RecordingMethod, 
D.Geography, D.AverageAltitudeCache, D.AverageLatitudeCache, D.AverageLongitudeCache, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, 
D.LogUpdatedWhen, D.LogUpdatedBy,  @Version,  'D'
FROM DELETED D, INSERTED I
WHERE D.CollectionEventID = I.CollectionEventID AND I.LocalisationSystemID = D.LocalisationSystemID
AND (
(I.Location1 <> D.Location1 OR I.Location1 IS NULL AND NOT D.Location1 IS NULL OR NOT I.Location1 IS NULL AND D.Location1 IS NULL)
OR (I.Location2 <> D.Location2 OR I.Location2 IS NULL AND NOT D.Location2 IS NULL OR NOT I.Location2 IS NULL AND D.Location2 IS NULL)
OR (I.LocationAccuracy <> D.LocationAccuracy OR I.LocationAccuracy IS NULL AND NOT D.LocationAccuracy IS NULL OR NOT I.LocationAccuracy IS NULL AND D.LocationAccuracy IS NULL)
OR (I.LocationNotes <> D.LocationNotes OR I.LocationNotes IS NULL AND NOT D.LocationNotes IS NULL OR NOT I.LocationNotes IS NULL AND D.LocationNotes IS NULL)
OR (I.DeterminationDate <> D.DeterminationDate OR I.DeterminationDate IS NULL AND NOT D.DeterminationDate IS NULL OR NOT I.DeterminationDate IS NULL AND D.DeterminationDate IS NULL)
OR (I.DistanceToLocation <> D.DistanceToLocation OR I.DistanceToLocation IS NULL AND NOT D.DistanceToLocation IS NULL OR NOT I.DistanceToLocation IS NULL AND D.DistanceToLocation IS NULL)
OR (I.DirectionToLocation <> D.DirectionToLocation OR I.DirectionToLocation IS NULL AND NOT D.DirectionToLocation IS NULL OR NOT I.DirectionToLocation IS NULL AND D.DirectionToLocation IS NULL)
OR (I.ResponsibleName <> D.ResponsibleName OR I.ResponsibleName IS NULL AND NOT D.ResponsibleName IS NULL OR NOT I.ResponsibleName IS NULL AND D.ResponsibleName IS NULL)
OR (I.ResponsibleAgentURI <> D.ResponsibleAgentURI OR I.ResponsibleAgentURI IS NULL AND NOT D.ResponsibleAgentURI IS NULL OR NOT I.ResponsibleAgentURI IS NULL AND D.ResponsibleAgentURI IS NULL)
OR (I.RecordingMethod <> D.RecordingMethod OR I.RecordingMethod IS NULL AND NOT D.RecordingMethod IS NULL OR NOT I.RecordingMethod IS NULL AND D.RecordingMethod IS NULL)
OR (I.Geography.ToString() <> D.Geography.ToString() OR I.Geography IS NULL AND NOT D.Geography IS NULL OR NOT I.Geography IS NULL AND D.Geography IS NULL)
OR (I.AverageAltitudeCache <> D.AverageAltitudeCache OR I.AverageAltitudeCache IS NULL AND NOT D.AverageAltitudeCache IS NULL OR NOT I.AverageAltitudeCache IS NULL AND D.AverageAltitudeCache IS NULL)
OR (I.AverageLatitudeCache <> D.AverageLatitudeCache OR I.AverageLatitudeCache IS NULL AND NOT D.AverageLatitudeCache IS NULL OR NOT I.AverageLatitudeCache IS NULL AND D.AverageLatitudeCache IS NULL)
OR (I.AverageLongitudeCache <> D.AverageLongitudeCache OR I.AverageLongitudeCache IS NULL AND NOT D.AverageLongitudeCache IS NULL OR NOT I.AverageLongitudeCache IS NULL AND D.AverageLongitudeCache IS NULL)
)
end
else
begin
INSERT INTO CollectionEventLocalisation_Log (CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, 
DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName, ResponsibleAgentURI, RecordingMethod, 
Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  
LogVersion, LogState) 
SELECT D.CollectionEventID, D.LocalisationSystemID, D.Location1, D.Location2, D.LocationAccuracy, D.LocationNotes, 
D.DeterminationDate, D.DistanceToLocation, D.DirectionToLocation, D.ResponsibleName, D.ResponsibleAgentURI, D.RecordingMethod, 
D.Geography, D.AverageAltitudeCache, D.AverageLatitudeCache, D.AverageLongitudeCache, D.RowGUID, D.LogCreatedWhen, D.LogCreatedBy, 
D.LogUpdatedWhen, D.LogUpdatedBy, E.Version, 'D' 
FROM DELETED D, CollectionEvent E
WHERE D.CollectionEventID = E.CollectionEventID
end


/* updating the logging columns */
Update CollectionEventLocalisation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionEventLocalisation, deleted 
where 1 = 1 
AND CollectionEventLocalisation.CollectionEventID = deleted.CollectionEventID
AND CollectionEventLocalisation.LocalisationSystemID = deleted.LocalisationSystemID

/* updating the geography column */
Update CollectionEventLocalisation
set Geography = 
case when inserted.Geography IS null then 
	geography::STPointFromText('POINT(' + replace(cast(inserted.[AverageLongitudeCache] as varchar), ',', '.')+' ' +replace(cast(inserted.[AverageLatitudeCache] as varchar), ',', '.')+')', 4326)
else inserted.Geography end
FROM CollectionEventLocalisation, inserted 
where 1 = 1 
AND CollectionEventLocalisation.CollectionEventID = inserted.CollectionEventID
AND CollectionEventLocalisation.LocalisationSystemID = inserted.LocalisationSystemID
AND inserted.AverageLatitudeCache between -90 and 90
AND inserted.AverageLongitudeCache between -180 and 180
AND (CollectionEventLocalisation.Geography.ToString() <> inserted.Geography.ToString()
OR CollectionEventLocalisation.Geography IS NULL)
--AND inserted.Geography IS NULL

GO

--#####################################################################################################################
--######  remove exessive data from CollectionEventLocalisation_log   ######################################################################################
--#####################################################################################################################

delete L
from CollectionEventLocalisation_log L
where exists(
SELECT  *  FROM    CollectionEventLocalisation_log DD
GROUP BY CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, 
                      ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, RecordingMethod
HAVING      (COUNT(*) > 1)
and L.CollectionEventID = DD.CollectionEventID and L.LocalisationSystemID = DD.LocalisationSystemID and L.LogID <> MIN(DD.LogID))
GO



--#####################################################################################################################
--######   Schema: Tools   ######################################################################################
--#####################################################################################################################

CREATE XML SCHEMA COLLECTION Tools AS
N'<xs:schema xmlns:dwb="http://diversityworkbench.net/Schema/" 
           xmlns:xs="http://www.w3.org/2001/XMLSchema" 
           xmlns="http://diversityworkbench.net/Schema/" 
           targetNamespace="http://diversityworkbench.net/Schema/tools" 
           elementFormDefault="qualified" 
           attributeFormDefault="unqualified">
  <xs:element name="Tools">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Tool" minOccurs="1" maxOccurs="unbounded">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Usage" minOccurs="1" maxOccurs="unbounded">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name="ValueEnum"  minOccurs="0" maxOccurs="unbounded" type="xs:string"/>
                  </xs:sequence>
                  <xs:attribute name="Name" use="required" type="xs:string"/>
                  <xs:attribute name="Value" use="required" type="xs:string"/>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
            <xs:attribute name="Name" type="xs:string" use="required"/>
            <xs:attribute name="ToolID" type="xs:int" use="required"/>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>' ;
GO




--#####################################################################################################################
--######   CollectionSpecimenProcessing: ToolUsage   ######################################################################################
--#####################################################################################################################



ALTER TABLE CollectionSpecimenProcessing DROP COLUMN ToolUsage
GO
ALTER TABLE CollectionSpecimenProcessing_log DROP COLUMN ToolUsage
GO


ALTER TABLE CollectionSpecimenProcessing ADD ToolUsage xml(Tools) NULL
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The tools used for the processing and their usage or settings.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenProcessing', @level2type=N'COLUMN',@level2name=N'ToolUsage'
GO

ALTER TABLE CollectionSpecimenProcessing_log ADD ToolUsage xml(Tools) NULL
GO


/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenProcessing]    Script Date: 02/27/2013 09:58:15 ******/


ALTER TRIGGER [dbo].[trgDelCollectionSpecimenProcessing] ON [dbo].[CollectionSpecimenProcessing] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenProcessing]    Script Date: 02/27/2013 10:01:04 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenProcessing] ON [dbo].[CollectionSpecimenProcessing] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 30.08.2007  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenProcessing_Log (CollectionSpecimenID, ProcessingDate, ProcessingID, Protocoll, SpecimenPartID, ProcessingDuration, ResponsibleName, ResponsibleAgentURI, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.ProcessingDate, deleted.ProcessingID, deleted.Protocoll, deleted.SpecimenPartID, deleted.ProcessingDuration, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenProcessing
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenProcessing, deleted 
where 1 = 1 
AND CollectionSpecimenProcessing.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenProcessing.ProcessingDate = deleted.ProcessingDate

GO


--#####################################################################################################################
--######   IdentificationUnitAnalysis: ToolUsage   ######################################################################################
--#####################################################################################################################

ALTER TABLE IdentificationUnitAnalysis DROP COLUMN ToolUsage
GO
ALTER TABLE IdentificationUnitAnalysis_log DROP COLUMN ToolUsage
GO



ALTER TABLE IdentificationUnitAnalysis ADD ToolUsage xml(Tools) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The tools used for the analysis and their usage or settings.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnitAnalysis', @level2type=N'COLUMN',@level2name=N'ToolUsage'
GO

ALTER TABLE IdentificationUnitAnalysis_log ADD ToolUsage xml(Tools) NULL
GO



/****** Object:  Trigger [dbo].[trgDelIdentificationUnitAnalysis]    Script Date: 02/27/2013 10:03:35 ******/

ALTER TRIGGER [dbo].[trgDelIdentificationUnitAnalysis] ON [dbo].[IdentificationUnitAnalysis] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 31.08.2007  */ 


/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 

/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnitAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.ExternalAnalysisURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.AnalysisDate, deleted.SpecimenPartID, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.ExternalAnalysisURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.AnalysisDate, deleted.SpecimenPartID, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
GO


/****** Object:  Trigger [dbo].[trgUpdIdentificationUnitAnalysis]    Script Date: 02/27/2013 10:06:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trgUpdIdentificationUnitAnalysis] ON [dbo].[IdentificationUnitAnalysis] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 31.08.2007  */ 

/* setting the version in the main table */ 
DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
   SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
   EXECUTE procSetVersionCollectionSpecimen @ID
   SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)
END 


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO IdentificationUnitAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.ExternalAnalysisURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.AnalysisDate, deleted.SpecimenPartID, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO IdentificationUnitAnalysis_Log (CollectionSpecimenID, IdentificationUnitID, AnalysisID, AnalysisNumber, AnalysisResult, ExternalAnalysisURI, ResponsibleName, ResponsibleAgentURI, AnalysisDate, SpecimenPartID, Notes, ToolUsage, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.AnalysisID, deleted.AnalysisNumber, deleted.AnalysisResult, deleted.ExternalAnalysisURI, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.AnalysisDate, deleted.SpecimenPartID, deleted.Notes, deleted.ToolUsage, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update IdentificationUnitAnalysis
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM IdentificationUnitAnalysis, deleted 
where 1 = 1 
AND IdentificationUnitAnalysis.CollectionSpecimenID = deleted.CollectionSpecimenID
AND IdentificationUnitAnalysis.IdentificationUnitID = deleted.IdentificationUnitID
AND IdentificationUnitAnalysis.AnalysisID = deleted.AnalysisID
AND IdentificationUnitAnalysis.AnalysisNumber = deleted.AnalysisNumber

GO



--#####################################################################################################################
--######   Tool   ######################################################################################
--#####################################################################################################################



CREATE TABLE [dbo].[Tool](
	[ToolID] [int] IDENTITY(1,1) NOT NULL,
	[ToolParentID] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[ToolURI] [varchar](255) NULL,
	[ToolUsageTemplate] [xml](CONTENT [dbo].[Tools]) NULL,
	[Notes] [nvarchar](max) NULL,
	[OnlyHierarchy] [bit] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_Tool] PRIMARY KEY CLUSTERED 
(
	[ToolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the tool (Primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'ToolID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ToolID of the parent tool if it belongs to a certain type documented in this table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'ToolParentID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the tool as e.g. shown in user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the tool' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI referring to an external documentation of the tool' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'ToolURI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A template for the settings for usage of a tool' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'ToolUsageTemplate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes concerning this analysis tool' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'Notes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the entry is only used for the hierarchical arrangement of the entries' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'OnlyHierarchy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tools used within the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool'
GO

ALTER TABLE [dbo].[Tool]  WITH NOCHECK ADD  CONSTRAINT [FK_Tool_Tool] FOREIGN KEY([ToolParentID])
REFERENCES [dbo].[Tool] ([ToolID])
GO

ALTER TABLE [dbo].[Tool] CHECK CONSTRAINT [FK_Tool_Tool]
GO

ALTER TABLE [dbo].[Tool] ADD  CONSTRAINT [DF_Tool_OnlyHierarchy]  DEFAULT ((0)) FOR [OnlyHierarchy]
GO

ALTER TABLE [dbo].[Tool] ADD  CONSTRAINT [DF_Tool_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO

ALTER TABLE [dbo].[Tool] ADD  CONSTRAINT [DF_Tool_LogCreatedBy]  DEFAULT (user_name()) FOR [LogCreatedBy]
GO

ALTER TABLE [dbo].[Tool] ADD  CONSTRAINT [DF_Tool_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[Tool] ADD  CONSTRAINT [DF_Tool_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[Tool] ADD  CONSTRAINT [DF_Tool_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


GRANT SELECT ON Tool TO [User]
GO

GRANT UPDATE ON Tool TO [Administrator]
GO

GRANT DELETE ON Tool TO [Administrator]
GO

GRANT INSERT ON Tool TO [Administrator]
GO

--#####################################################################################################################
--######   Tool_log   ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Tool_log](
	[ToolID] [int] NULL,
	[ToolParentID] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[ToolURI] [varchar](255) NULL,
	[ToolUsageTemplate] [xml] NULL,
	[Notes] [nvarchar](max) NULL,
	[OnlyHierarchy] [bit] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Tool_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Tool_log] ADD  CONSTRAINT [DF_Tool_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[Tool_log] ADD  CONSTRAINT [DF_Tool_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[Tool_log] ADD  CONSTRAINT [DF_Tool_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON Tool_log TO [User]
GO

GRANT INSERT ON Tool_log TO [Administrator]
GO



--#####################################################################################################################
--######   trgDelTool   ######################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelTool] ON [dbo].[Tool] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 13.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Tool_Log (ToolID, ToolParentID, Name, Description, ToolURI, ToolUsageTemplate, Notes, OnlyHierarchy, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.ToolID, deleted.ToolParentID, deleted.Name, deleted.Description, deleted.ToolURI, deleted.ToolUsageTemplate, deleted.Notes, deleted.OnlyHierarchy, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######   trgUpdTool   ######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdTool] ON [dbo].[Tool] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 13.03.2013  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Tool_Log (ToolID, ToolParentID, Name, Description, ToolURI, ToolUsageTemplate, Notes, OnlyHierarchy, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.ToolID, deleted.ToolParentID, deleted.Name, deleted.Description, deleted.ToolURI, deleted.ToolUsageTemplate, deleted.Notes, deleted.OnlyHierarchy, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update Tool
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM Tool, deleted 
where 1 = 1 
AND Tool.ToolID = deleted.ToolID
GO


--#####################################################################################################################
--######   ToolChildNodes   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[ToolChildNodes] (@ID int)  
RETURNS @ItemList TABLE (ToolID int primary key,
	ToolParentID int NULL ,
	Name nvarchar (255)   NULL ,
	Description nvarchar  (500)   NULL ,
	ToolUsageTemplate xml   NULL ,
	Notes nvarchar  (1000)   NULL ,
	ToolURI varchar  (255)   NULL ,
	OnlyHierarchy [bit] NULL,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)  

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 11.03.2013
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (ToolID int primary key,
	ToolParentID int NULL ,
	Name nvarchar (255)   NULL ,
	Description nvarchar  (500)   NULL ,
	ToolUsageTemplate xml   NULL ,
	Notes nvarchar  (1000)   NULL ,
	ToolURI varchar  (255)   NULL,
	OnlyHierarchy [bit] NULL ,
	RowGUID [uniqueidentifier] ROWGUIDCOL NULL)

 INSERT @TempItem (ToolID , ToolParentID, Name , Description , ToolUsageTemplate, Notes , ToolURI, OnlyHierarchy, RowGUID) 
	SELECT ToolID , ToolParentID, Name , Description , ToolUsageTemplate, Notes , ToolURI, OnlyHierarchy, RowGUID
	FROM Tool WHERE ToolParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select ToolID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select ToolID , ToolParentID, Name , Description , ToolUsageTemplate, Notes , ToolURI, OnlyHierarchy, RowGUID
	from dbo.ToolChildNodes (@ParentID) where ToolID not in (select ToolID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (ToolID , ToolParentID, Name , Description , ToolUsageTemplate, Notes , ToolURI, OnlyHierarchy, RowGUID) 
   SELECT distinct ToolID , ToolParentID, Name , Description , CAST(ToolUsageTemplate AS nvarchar(4000)), Notes , ToolURI, OnlyHierarchy, RowGUID
   FROM @TempItem ORDER BY Name
   
   RETURN
END



GO


GRANT SELECT ON dbo.ToolChildNodes TO [User]
GO


--#####################################################################################################################
--######   ToolHierarchy   ######################################################################################
--#####################################################################################################################




CREATE FUNCTION [dbo].[ToolHierarchy] (@ToolID int)  
RETURNS @ToolList TABLE ([ToolID] [int] Primary key ,
	[ToolParentID] [int] NULL ,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[ToolUsageTemplate] [xml] NULL ,
	[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[ToolURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[OnlyHierarchy] [bit] NULL)  


/*
Returns a table that lists all the Tool items related to the given Tool.
MW 11.03.2013
Test
SELECT  *  FROM dbo.ToolHierarchy(1)

*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int

set @TopID = (select ToolParentID from Tool where ToolID = @ToolID) 

set @i = (select count(*) from Tool where ToolID = @ToolID)

if (@TopID is null )
	set @TopID =  @ToolID
else	
	begin
	while (@i > 0)
		begin
		set @ToolID = (select ToolParentID from Tool where ToolID = @ToolID and not ToolParentID is null) 
		set @i = (select count(*) from Tool where ToolID = @ToolID and not ToolParentID is null)
		end
	set @TopID = @ToolID
	end

-- copy the root node in the result list
   INSERT @ToolList
   SELECT DISTINCT ToolID, ToolParentID, Name, Description, CAST(ToolUsageTemplate AS nvarchar(4000)), Notes, ToolURI, OnlyHierarchy
   FROM Tool
   WHERE Tool.ToolID = @TopID

-- copy the child nodes into the result list
   INSERT @ToolList
   SELECT ToolID, ToolParentID, Name, Description, ToolUsageTemplate, Notes, ToolURI, OnlyHierarchy
   FROM dbo.ToolChildNodes (@TopID)

   RETURN
END



GO

GRANT SELECT ON dbo.ToolHierarchy TO [User]
GO






--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.30'
END

GO



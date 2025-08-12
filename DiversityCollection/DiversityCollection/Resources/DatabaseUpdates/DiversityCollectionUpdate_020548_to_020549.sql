
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.48'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   [LabelTitle]   ######################################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimen ALTER COLUMN LabelTitle nvarchar(max) NULL
GO

ALTER TABLE CollectionSpecimen_log ALTER COLUMN LabelTitle nvarchar(max) NULL
GO

--#####################################################################################################################
--######   [DF_IdentificationUnitAnalysis_ResponsibleName]   ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[IdentificationUnitAnalysis] DROP  CONSTRAINT [DF_IdentificationUnitAnalysis_ResponsibleName]
GO

--#####################################################################################################################
--######   [Queries]  ######################################################################################
--#####################################################################################################################

GRANT UPDATE ON [dbo].[UserProxy] ([Queries]) TO [User] AS [dbo]
GO


--#####################################################################################################################
--######   [ImageArea]  ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[CollectionSpecimenImageProperty] ADD ImageArea geometry NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The area in the image the property refers to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty', @level2type=N'COLUMN',@level2name=N'ImageArea'
GO

ALTER TABLE [dbo].[CollectionSpecimenImageProperty_log] ADD ImageArea geometry NULL
GO


/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenImageProperty]    Script Date: 06/13/2014 14:31:55 ******/

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenImageProperty] ON [dbo].[CollectionSpecimenImageProperty] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 09.04.2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenImageProperty_Log (CollectionSpecimenID, URI, Property, Description, ImageArea, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.Property, deleted.Description, deleted.ImageArea, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenImageProperty]    Script Date: 06/13/2014 14:32:35 ******/
ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenImageProperty] ON [dbo].[CollectionSpecimenImageProperty] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityMaintenance  3.0.0.1 */ 
/*  Date: 09.04.2014  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenImageProperty_Log (CollectionSpecimenID, URI, Property, Description, ImageArea, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.URI, deleted.Property, deleted.Description, deleted.ImageArea, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionSpecimenImageProperty
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenImageProperty, deleted 
where 1 = 1 
AND CollectionSpecimenImageProperty.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenImageProperty.Property = deleted.Property
AND CollectionSpecimenImageProperty.URI = deleted.URI

GO


--#####################################################################################################################
--######   [CollectionSpecimenRelation]  ######################################################################################
--#####################################################################################################################

ALTER TABLE [dbo].CollectionSpecimenRelation ADD IdentificationUnitID int NULL
GO

ALTER TABLE [dbo].CollectionSpecimenRelation ADD SpecimenPartID int NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the relation refers to a part of a specimen, the ID of a related part (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenRelation', @level2type=N'COLUMN',@level2name=N'SpecimenPartID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If relation refers to a certain organism within a specimen, the ID of an IdentificationUnit (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenRelation', @level2type=N'COLUMN',@level2name=N'IdentificationUnitID'
GO

ALTER TABLE [dbo].CollectionSpecimenRelation_log ADD IdentificationUnitID int NULL
GO

ALTER TABLE [dbo].CollectionSpecimenRelation_log ADD SpecimenPartID int NULL
GO

/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenRelation]    Script Date: 06/13/2014 16:03:54 ******/
ALTER TRIGGER [dbo].[trgDelCollectionSpecimenRelation] ON [dbo].[CollectionSpecimenRelation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 18.09.2007  */ 


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
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID, SpecimenPartID, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.RelatedSpecimenURI, deleted.RelatedSpecimenDisplayText, deleted.RelationType, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.Notes, deleted.IsInternalRelationCache, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID, SpecimenPartID, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.RelatedSpecimenURI, deleted.RelatedSpecimenDisplayText, deleted.RelationType, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.Notes, deleted.IsInternalRelationCache, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'D' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO



/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenRelation]    Script Date: 06/13/2014 16:05:40 ******/
ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenRelation] ON [dbo].[CollectionSpecimenRelation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityWorkbenchMaintenance  2.0.0.3 */ 
/*  Date: 18.09.2007  */ 

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
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID, SpecimenPartID, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.RelatedSpecimenURI, deleted.RelatedSpecimenDisplayText, deleted.RelationType, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.Notes, deleted.IsInternalRelationCache, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenRelation_Log (CollectionSpecimenID, RelatedSpecimenURI, RelatedSpecimenDisplayText, RelationType, RelatedSpecimenCollectionID, RelatedSpecimenDescription, Notes, IsInternalRelationCache, IdentificationUnitID, SpecimenPartID, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.RelatedSpecimenURI, deleted.RelatedSpecimenDisplayText, deleted.RelationType, deleted.RelatedSpecimenCollectionID, deleted.RelatedSpecimenDescription, deleted.Notes, deleted.IsInternalRelationCache, deleted.IdentificationUnitID, deleted.SpecimenPartID, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

/* updating the logging columns */
Update CollectionSpecimenRelation
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenRelation, deleted 
where 1 = 1 
AND CollectionSpecimenRelation.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenRelation.RelatedSpecimenURI = deleted.RelatedSpecimenURI

GO


ALTER TABLE [dbo].[CollectionSpecimenRelation]  WITH NOCHECK ADD  CONSTRAINT [FK_CollectionSpecimenRelation_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[CollectionSpecimenRelation] NOCHECK CONSTRAINT [FK_CollectionSpecimenRelation_CollectionSpecimenPart]
GO


ALTER TABLE [dbo].[CollectionSpecimenRelation]  WITH NOCHECK ADD  CONSTRAINT [FK_CollectionSpecimenRelation_IdentificationUnit] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID])
REFERENCES [dbo].[IdentificationUnit] ([CollectionSpecimenID], [IdentificationUnitID])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[CollectionSpecimenRelation] NOCHECK CONSTRAINT [FK_CollectionSpecimenRelation_IdentificationUnit]
GO



--#####################################################################################################################
--######   Replacement of IsOnLoan by explicit return  ######################################################################################
--#####################################################################################################################

BEGIN TRY
BEGIN TRANSACTION TR

-- Insert returns that are missing and that should replace the IsOnLoan field

INSERT INTO [Transaction]
           ([ParentTransactionID]
           ,[TransactionType]
           ,[TransactionTitle]
           ,[AdministratingCollectionID]
           ,[BeginDate]
           ,[ActualEndDate]
           ,[InternalNotes])
SELECT   T.TransactionID AS ParentTransactionID, 'return' AS TransactionType, 
'Return of ' + T.TransactionTitle AS TransactionTitle, T.AdministratingCollectionID,
CASE WHEN T.[ActualEndDate] IS NULL THEN MIN(CT.LogUpdatedWhen) ELSE T.[ActualEndDate] END
  AS BeginDate, CASE WHEN T.[ActualEndDate] IS NULL THEN MAX(CT.LogUpdatedWhen) ELSE T.[ActualEndDate] END AS EndDate,
 'Return inserted by program to replace the obsolete IsOnLoan by a explicit return transaction' AS InternalNotes
FROM         CollectionSpecimenTransaction CT INNER JOIN
                      [Transaction] AS T ON CT.TransactionID = T.TransactionID LEFT OUTER JOIN
                      [Transaction] AS TRet ON T.TransactionID = TRet.ParentTransactionID
WHERE     (CT.IsOnLoan = 0)
GROUP BY T.TransactionType, T.TransactionID, TRet.TransactionType, T.TransactionTitle, T.AdministratingCollectionID,  T.[ActualEndDate]
HAVING      (T.TransactionType = N'loan') AND (TRet.TransactionType IS NULL)


-- Update the datasets that where set on IsOnLoan = 0 by setting the TransactionReturnID to the ID of the new return transactions

update CST SET  CST.TransactionReturnID = TRet.TransactionID                     
FROM         CollectionSpecimenTransaction AS CST INNER JOIN
                      [Transaction] AS T ON CST.TransactionID = T.TransactionID INNER JOIN
                      [Transaction] AS TRet ON T.TransactionID = TRet.ParentTransactionID
WHERE     (CST.IsOnLoan = 0) AND (T.TransactionType = N'loan') 
AND TRet.TransactionType = 'return'
and TRet.InternalNotes = 'Return inserted by program to replace the obsolete IsOnLoan by a explicit return transaction'
and rtrim(ltrim('Return of ' + T.TransactionTitle)) = rtrim(ltrim(TRet.TransactionTitle))


-- Insert the return transactions for the parts for the above loans

INSERT INTO [CollectionSpecimenTransaction]
           ([CollectionSpecimenID]
           ,[TransactionID]
           ,[SpecimenPartID])
SELECT     CST.CollectionSpecimenID, TRet.TransactionID, CST.SpecimenPartID                 
FROM         CollectionSpecimenTransaction AS CST INNER JOIN
          [Transaction] AS T ON CST.TransactionID = T.TransactionID INNER JOIN
          [Transaction] AS TRet ON T.TransactionID = TRet.ParentTransactionID
WHERE     (CST.IsOnLoan = 0) AND (T.TransactionType = N'loan') 
AND TRet.TransactionType = 'return'
and TRet.InternalNotes = 'Return inserted by program to replace the obsolete IsOnLoan by a explicit return transaction'
and rtrim(ltrim('Return of ' + T.TransactionTitle)) = rtrim(ltrim(TRet.TransactionTitle))

COMMIT TRANSACTION TR
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION TR
END CATCH

--#####################################################################################################################
--######   [TransactionDocument]  ######################################################################################
--#####################################################################################################################

GRANT DELETE ON TransactionDocument TO CollectionManager
GO

GRANT DELETE ON TransactionDocument TO Administrator
GO



--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.05' 
END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.49'
END

GO




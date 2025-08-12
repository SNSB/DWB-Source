
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.51'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   CollectionSpecimenImageProperty  ######################################################################################
--#####################################################################################################################

IF (SELECT COUNT(*) FROM fn_listextendedproperty(NULL, 'schema', 'DBO', 'table', 'CollectionSpecimenImageProperty', DEFAULT, default)) = 0
BEGIN
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The properties of images of a collection specimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImageProperty'
END
GO

--#####################################################################################################################
--######   TransactionComment  ######################################################################################
--#####################################################################################################################


IF (SELECT COUNT(*) FROM fn_listextendedproperty(NULL, 'schema', 'DBO', 'table', 'TransactionComment', DEFAULT, default)) = 0
BEGIN
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The standard text phrases for transactions' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionComment'
END
GO

--#####################################################################################################################
--######   [Gazetteer]   ######################################################################################
--#####################################################################################################################
declare @Test int;
set @Test = (SELECT COUNT (*) from [LocalisationSystem] L where L.LocalisationSystemName like '%. Named area (DiversityGazetteer)')
if (@Test = 0)
BEGIN
	declare @Pos int;
	set @Pos = 2;
	while (select @Pos) < 6
	begin
	INSERT INTO [LocalisationSystem]
			   ([LocalisationSystemID]
			   ,[LocalisationSystemParentID]
			   ,[LocalisationSystemName]
			   ,[DefaultAccuracyOfLocalisation]
			   ,[DefaultMeasurementUnit]
			   ,[ParsingMethodName]
			   ,[DisplayText]
			   ,[DisplayEnable]
			   ,[DisplayOrder]
			   ,[Description]
			   ,[DisplayTextLocation1]
			   ,[DescriptionLocation1]
			   ,[DisplayTextLocation2]
			   ,[DescriptionLocation2])
	SELECT 16 + @Pos
		  ,7
		  ,cast(@Pos as CHAR(1)) + '. ' + [LocalisationSystemName]
		  ,[DefaultAccuracyOfLocalisation]
		  ,[DefaultMeasurementUnit]
		  ,[ParsingMethodName]
		  ,cast(@Pos as CHAR(1)) + '. ' + [DisplayText]
		  ,[DisplayEnable]
		  ,[DisplayOrder] - 1 + @Pos
		  ,cast(@Pos as CHAR(1)) + '. ' + [Description]
		  ,[DisplayTextLocation1]
		  ,[DescriptionLocation1]
		  ,[DisplayTextLocation2]
		  ,[DescriptionLocation2]
	  FROM [LocalisationSystem] L
	  WHERE L.LocalisationSystemID = 7
	  
	  
	  INSERT INTO [Entity]
			   ([Entity])/**/
	  SELECT substring([Entity], 1, LEN([Entity]) - 1) + cast(16 + @Pos as varchar)
	  FROM [Entity]
	E where E.Entity = 'LocalisationSystem.LocalisationSystemID.7'


	INSERT INTO [EntityRepresentation]
			   ([Entity]
			   ,[LanguageCode]
			   ,[EntityContext]
			   ,[DisplayText]
			   ,[Abbreviation]
			   ,[Description])/**/
	SELECT substring([Entity], 1, LEN([Entity]) - 1) + cast(16 + @Pos as varchar)
		  ,[LanguageCode]
		  ,[EntityContext]
		  ,[DisplayText]
		  ,[Abbreviation]
		  ,[Description]
	  FROM [EntityRepresentation]
	E where E.Entity = 'LocalisationSystem.LocalisationSystemID.7'


	  set @Pos = @Pos + 1;
	end  
END
GO

--#####################################################################################################################
--######   Biostratigraphy   ######################################################################################
--#####################################################################################################################

IF (SELECT COUNT(*) FROM [Property] P WHERE P.PropertyID = 60) = 0
BEGIN
INSERT INTO [Property]
           ([PropertyID]
           ,[PropertyName]
           ,[ParsingMethodName]
           ,[DisplayText]
           ,[DisplayEnabled]
           ,[Description])
     VALUES
           (60
           ,'Biostratigraphy'
           ,'Stratigraphy'
           ,'Biostratigraphy'
           ,1
           ,'')
END
GO

UPDATE [Property]
   SET [ParsingMethodName] = 'Stratigraphy'
 WHERE [PropertyName] like '%Stratigraphy'
GO




--#####################################################################################################################
--######   TransactionDocument: setting missing display text to type and date+time   ######################################################################################
--#####################################################################################################################



UPDATE TD SET TD.[DisplayText] =
--SELECT *,
      T.TransactionType + ' - ' + CAST(YEAR([Date]) AS varchar) + '-' 
      + CASE WHEN MONTH([Date]) < 10 THEN '0' ELSE '' END
      + CAST(MONTH([Date]) AS varchar) + '-' 
      + CASE WHEN DAY([Date]) < 10 THEN '0' ELSE '' END
      + CAST(DAY([Date]) AS varchar) + ' ' 
      + CASE WHEN DATEPART(HOUR, [Date]) < 10 THEN '0' ELSE '' END
      + CAST(DATEPART(HOUR, [Date]) AS varchar) + ':' 
      + CASE WHEN DATEPART(MINUTE, [Date]) < 10 THEN '0' ELSE '' END
      + CAST(DATEPART(MINUTE, [Date]) AS varchar) + ':' 
      + CASE WHEN DATEPART(SECOND, [Date]) < 10 THEN '0' ELSE '' END
      + CAST(DATEPART(SECOND, [Date]) AS varchar)/**/
  FROM [TransactionDocument] AS TD, [Transaction] T
  WHERE TD.DisplayText IS NULL
  AND TD.TransactionID = T.TransactionID
  
GO

--#####################################################################################################################
--######   [trgDelCollectionSpecimenPart]: Trigger miss column RowGUID  ######################################################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelCollectionSpecimenPart]    Script Date: 22.09.2014 13:49:47 ******/

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenPart] ON [dbo].[CollectionSpecimenPart] 
FOR DELETE AS 
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
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, 
PreparationDate, AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, 
Stock, StockUnit, ResponsibleName, ResponsibleAgentURI, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, 
LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, 
deleted.PreparationDate, deleted.AccessionNumber, deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, 
deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.ResponsibleName, 
deleted.ResponsibleAgentURI, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'D'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, 
PreparationDate, AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, 
Stock, StockUnit, ResponsibleName, ResponsibleAgentURI, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, 
LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, 
deleted.PreparationDate, deleted.AccessionNumber, deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, 
deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.ResponsibleName, deleted.ResponsibleAgentURI, 
deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, 
deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'D'
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO


/****** Object:  Trigger [dbo].[trgUpdCollectionSpecimenPart]    Script Date: 22.09.2014 13:52:19 ******/

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenPart] ON [dbo].[CollectionSpecimenPart] 
FOR UPDATE AS
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
if (not @Version is null) 
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, 
PreparationDate, AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, 
Stock, StockUnit, ResponsibleName, ResponsibleAgentURI, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, 
LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, 
deleted.PreparationDate, deleted.AccessionNumber, deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, 
deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.ResponsibleName, 
deleted.ResponsibleAgentURI, deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, 
deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO CollectionSpecimenPart_Log (CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, 
PreparationDate, AccessionNumber, PartSublabel, CollectionID, MaterialCategory, StorageLocation, StorageContainer, 
Stock, StockUnit, ResponsibleName, ResponsibleAgentURI, Notes, DataWithholdingReason, LogCreatedWhen, LogCreatedBy, 
LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.DerivedFromSpecimenPartID, deleted.PreparationMethod, 
deleted.PreparationDate, deleted.AccessionNumber, deleted.PartSublabel, deleted.CollectionID, deleted.MaterialCategory, 
deleted.StorageLocation, deleted.StorageContainer, deleted.Stock, deleted.StockUnit, deleted.ResponsibleName, deleted.ResponsibleAgentURI, 
deleted.Notes, deleted.DataWithholdingReason, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, 
deleted.LogUpdatedBy, deleted.RowGUID, CollectionSpecimen.Version, 'U'
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end
Update CollectionSpecimenPart
set LogUpdatedWhen = getdate(), LogUpdatedBy = current_user
FROM CollectionSpecimenPart, deleted 
where 1 = 1 
AND CollectionSpecimenPart.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenPart.SpecimenPartID = deleted.SpecimenPartID

GO




--#####################################################################################################################
--######   Description for AccessionDateCategory   ######################################################################################
--#####################################################################################################################


EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Category of the date of the accession e.g. "system", "estimated"  (= foreign key, see  in table CollDateCategory_Enum)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimen', @level2type=N'COLUMN',@level2name=N'AccessionDateCategory'
GO




--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.06' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.52'
END

GO



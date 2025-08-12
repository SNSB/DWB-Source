declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.87'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Transfer of Annotation into Reference     ##################################################################
--#####################################################################################################################


--#####################################################################################################################
--######   CollectionSpecimen                        ##################################################################
--#####################################################################################################################

begin try
	BEGIN TRAN
		INSERT INTO [dbo].[CollectionSpecimenReference]
				   ([CollectionSpecimenID]
				   ,[ReferenceTitle]
				   ,[ReferenceURI]
				   ,[Notes]
				   ,[ResponsibleName]
				   ,[ResponsibleAgentURI]
				   ,[LogCreatedWhen]
				   ,[LogCreatedBy]
				   ,[LogUpdatedWhen]
				   ,[LogUpdatedBy]
				   ,[RowGUID])
		SELECT [ReferencedID] AS CollectionSpecimenID
			  , CASE WHEN [ReferenceDisplayText] IS NULL OR RTRIM([ReferenceDisplayText]) = '' THEN [Annotation] ELSE [ReferenceDisplayText] END AS ReferenceTitle
			  ,A.[ReferenceURI]
			  ,Annotation AS Notes
			  ,[SourceDisplayText] AS ResponsibleName
			  ,[SourceURI] AS ResponsibleAgentURI
			  ,A.[LogCreatedWhen]
			  ,A.[LogCreatedBy]
			  ,A.[LogUpdatedWhen]
			  ,A.[LogUpdatedBy]
			  ,A.[RowGUID]
		  FROM [dbo].[Annotation] A
				WHERE A.[ReferencedTable] = 'CollectionSpecimen'
				and A.AnnotationType = 'Reference'

		DELETE A from [dbo].[Annotation] A
		WHERE A.[ReferencedTable] = 'CollectionSpecimen'
		and A.AnnotationType = 'Reference'

	COMMIT TRAN 
end try
begin catch
	ROLLBACK TRAN
end catch



--#####################################################################################################################
--######   IdentificationUnit                        ##################################################################
--#####################################################################################################################


begin try
	BEGIN TRAN
	INSERT INTO [dbo].[CollectionSpecimenReference]
			   ([CollectionSpecimenID]
			   ,IdentificationUnitID
			   ,[ReferenceTitle]
			   ,[ReferenceURI]
			   ,[Notes]
			   ,[ResponsibleName]
			   ,[ResponsibleAgentURI]
			   ,[LogCreatedWhen]
			   ,[LogCreatedBy]
			   ,[LogUpdatedWhen]
			   ,[LogUpdatedBy]
			   ,[RowGUID])
	SELECT U.CollectionSpecimenID
		  , [ReferencedID] AS IdentificationUnitID
		  , CASE WHEN [ReferenceDisplayText] IS NULL OR RTRIM([ReferenceDisplayText]) = '' THEN [Annotation] ELSE [ReferenceDisplayText] END AS ReferenceTitle
		  ,[ReferenceURI]
		  ,Annotation AS Notes
		  ,[SourceDisplayText] AS ResponsibleName
		  ,[SourceURI] AS ResponsibleAgentURI
		  ,A.[LogCreatedWhen]
		  ,A.[LogCreatedBy]
		  ,A.[LogUpdatedWhen]
		  ,A.[LogUpdatedBy]
		  ,A.[RowGUID]
	  FROM [dbo].[Annotation] A, IdentificationUnit U
			WHERE A.[ReferencedTable] = 'IdentificationUnit'
			and A.AnnotationType = 'Reference'
			and A.ReferencedID = u.IdentificationUnitID

		DELETE A from [dbo].[Annotation] A
		WHERE A.[ReferencedTable] = 'IdentificationUnit'
		and A.AnnotationType = 'Reference'
	COMMIT TRAN 
end try
begin catch
	ROLLBACK TRAN
end catch


--#####################################################################################################################
--######   CollectionSpecimenPart                    ##################################################################
--#####################################################################################################################


begin try
	BEGIN TRAN
	INSERT INTO [dbo].[CollectionSpecimenReference]
			   ([CollectionSpecimenID]
			   ,SpecimenPartID
			   ,[ReferenceTitle]
			   ,[ReferenceURI]
			   ,[Notes]
			   ,[ResponsibleName]
			   ,[ResponsibleAgentURI]
			   ,[LogCreatedWhen]
			   ,[LogCreatedBy]
			   ,[LogUpdatedWhen]
			   ,[LogUpdatedBy]
			   ,[RowGUID])
	SELECT P.CollectionSpecimenID
		  , [ReferencedID] AS SpecimenPartID
		  , CASE WHEN [ReferenceDisplayText] IS NULL OR RTRIM([ReferenceDisplayText]) = '' THEN [Annotation] ELSE [ReferenceDisplayText] END AS ReferenceTitle
		  ,[ReferenceURI]
		  ,Annotation AS Notes
		  ,[SourceDisplayText] AS ResponsibleName
		  ,[SourceURI] AS ResponsibleAgentURI
		  ,A.[LogCreatedWhen]
		  ,A.[LogCreatedBy]
		  ,A.[LogUpdatedWhen]
		  ,A.[LogUpdatedBy]
		  ,A.[RowGUID]
	  FROM [dbo].[Annotation] A, CollectionSpecimenPart P
			WHERE A.[ReferencedTable] = 'CollectionSpecimenPart'
			and A.AnnotationType = 'Reference'
			and A.ReferencedID = P.SpecimenPartID

		DELETE A from [dbo].[Annotation] A
		WHERE A.[ReferencedTable] = 'CollectionSpecimenPart'
		and A.AnnotationType = 'Reference'
	COMMIT TRAN 
end try
begin catch
	ROLLBACK TRAN
end catch

--#####################################################################################################################
--######   CollectionEvent                           ##################################################################
--#####################################################################################################################

begin try
	BEGIN TRAN

	UPDATE E SET E.[ReferenceTitle] = A.ReferenceDisplayText,
	e.[ReferenceURI] = A.[ReferenceURI]
	  FROM [dbo].[Annotation] A, CollectionEvent E
			WHERE A.[ReferencedTable] = 'CollectionEvent'
			and A.AnnotationType = 'Reference'
			and A.ReferencedID = E.CollectionEventID
			and E.[ReferenceTitle] is null
			and E.ReferenceURI is null
			and e.ReferenceDetails is null
			and not A.ReferenceDisplayText is null

		DELETE A from [dbo].[Annotation] A, CollectionEvent E
		WHERE A.[ReferencedTable] = 'CollectionEvent'
		and A.AnnotationType = 'Reference'
		and A.ReferencedID = E.CollectionEventID
		and E.ReferenceTitle = A.ReferenceDisplayText

	COMMIT TRAN 
end try
begin catch
	ROLLBACK TRAN
end catch

--#####################################################################################################################
--######   Clear AnnotationType_Enum if possible     ##################################################################
--#####################################################################################################################


IF (SELECT COUNT(*) FROM Annotation A WHERE A.AnnotationType NOT IN ('Annotation', 'Problem')) = 0
begin
	DELETE A FROM [AnnotationType_Enum] A WHERE CODE NOT IN ('Annotation', 'Problem')
end

GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.88'
END

GO


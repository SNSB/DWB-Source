--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK

declare @Version varchar(10)
set @Version = (SELECT [dbo].[Version]())
IF (SELECT [dbo].[Version]()) <> '02.05.12'
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version 02.05.12. Current version = ' + @Version
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Umbau Parts - partID as Identity   ######################################################################################
--#####################################################################################################################

/*
UEBERSICHT

1.)		Anlegen von Identity ID und xx_OldSpecimenPartID in CollectionSpecimenPart
2.)		Übernehmen der alten SpecimenPartID -> xx_OldSpecimenPartID
3.)		Umschreiben der Fremdschluessel fuer Weitergabe
			dies ist intern nicht moeglich, daher dort manuell (DerivedFromPartID)
4.)		Löschen der internen Verknuepfung
5.)		Aktualisierung SpecimenPartID = ID
6.)		Loeschen der Fremdschluessel
7.)		Loeschen des PK
8.)		Umbenennen SpecimenPartID -> xx_SpecimenPartID	
9.)		Umbenennen ID -> SpecimenPartID
10.)	Nachtragen der internen Verknuepfung
11.)	Anlegen des PK
12.)	Anlegen der Fremdschluessel
13.)	Loeschen der Spalte xx_SpecimenPartID
14.)	Nachtrag der PartID in CollectionSpecimenImage

*/

--USE [DiversityCollection_TestPart]
GO

--1.)		Anlegen von Identity ID und xx_OldSpecimenPartID in CollectionSpecimenPart

	ALTER TABLE [dbo].[CollectionSpecimenPart]
	ADD [ID] [int]  IDENTITY(1,1) NOT NULL
	GO

	ALTER TABLE [dbo].[CollectionSpecimenPart]
	ADD [xx_OldSpecimenPartID] [int] NULL
	GO


--2.)		Übernehmen der alten SpecimenPartID -> xx_OldSpecimenPartID

	UPDATE [dbo].[CollectionSpecimenPart] SET [xx_OldSpecimenPartID] = [SpecimenPartID]
	GO
--(234490 Zeile(n) betroffen)



--3.)		Umschreiben der Fremdschluessel fuer Weitergabe
--			dies ist intern nicht moeglich, daher dort extern per SQL (DerivedFromPartID)

	-- loeschen der Fremdschluessel
	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitInPart_IdentificationUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitInPart]'))
	ALTER TABLE [dbo].[IdentificationUnitInPart] DROP CONSTRAINT [FK_IdentificationUnitInPart_IdentificationUnit1]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitOnPart_CollectionSpecimenPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitInPart]'))
	ALTER TABLE [dbo].[IdentificationUnitInPart] DROP CONSTRAINT [FK_IdentificationUnitOnPart_CollectionSpecimenPart]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitAnalysis_IdentificationUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitAnalysis]'))
	ALTER TABLE [dbo].[IdentificationUnitAnalysis] DROP CONSTRAINT [FK_IdentificationUnitAnalysis_IdentificationUnit]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitAnalysis_IdentificationUnitInPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitAnalysis]'))
	ALTER TABLE [dbo].[IdentificationUnitAnalysis] DROP CONSTRAINT [FK_IdentificationUnitAnalysis_IdentificationUnitInPart]
	GO

	-- neu anlegen
	ALTER TABLE [dbo].[IdentificationUnitInPart]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitInPart_IdentificationUnit1] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID])
	REFERENCES [dbo].[IdentificationUnit] ([CollectionSpecimenID], [IdentificationUnitID])
	GO

	ALTER TABLE [dbo].[IdentificationUnitInPart] CHECK CONSTRAINT [FK_IdentificationUnitInPart_IdentificationUnit1]
	GO

	ALTER TABLE [dbo].[IdentificationUnitInPart]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitOnPart_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[IdentificationUnitInPart] CHECK CONSTRAINT [FK_IdentificationUnitOnPart_CollectionSpecimenPart]
	GO
	

	ALTER TABLE [dbo].[IdentificationUnitAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysis_IdentificationUnit] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID])
	REFERENCES [dbo].[IdentificationUnit] ([CollectionSpecimenID], [IdentificationUnitID])
	GO

	ALTER TABLE [dbo].[IdentificationUnitAnalysis] CHECK CONSTRAINT [FK_IdentificationUnitAnalysis_IdentificationUnit]
	GO

	ALTER TABLE [dbo].[IdentificationUnitAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitAnalysis_IdentificationUnitInPart] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID], [SpecimenPartID])
	REFERENCES [dbo].[IdentificationUnitInPart] ([CollectionSpecimenID], [IdentificationUnitID], [SpecimenPartID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[IdentificationUnitAnalysis] CHECK CONSTRAINT [FK_IdentificationUnitAnalysis_IdentificationUnitInPart]
	GO
	


	-- Processing
	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessing_CollectionSpecimen]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessing]'))
	ALTER TABLE [dbo].[CollectionSpecimenProcessing] DROP CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimen]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessing_CollectionSpecimenPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessing]'))
	ALTER TABLE [dbo].[CollectionSpecimenProcessing] DROP CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart]
	GO
	
	ALTER TABLE [dbo].[CollectionSpecimenProcessing]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimen] FOREIGN KEY([CollectionSpecimenID])
	REFERENCES [dbo].[CollectionSpecimen] ([CollectionSpecimenID])
	GO

	ALTER TABLE [dbo].[CollectionSpecimenProcessing] CHECK CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimen]
	GO

	ALTER TABLE [dbo].[CollectionSpecimenProcessing]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[CollectionSpecimenProcessing] CHECK CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart]
	GO

	-- images

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenImage_CollectionSpecimen]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenImage]'))
	ALTER TABLE [dbo].[CollectionSpecimenImage] DROP CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimen]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenImage_CollectionSpecimenPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenImage]'))
	ALTER TABLE [dbo].[CollectionSpecimenImage] DROP CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimenPart]
	GO
	

	ALTER TABLE [dbo].[CollectionSpecimenImage]  WITH NOCHECK ADD  CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimen] FOREIGN KEY([CollectionSpecimenID])
	REFERENCES [dbo].[CollectionSpecimen] ([CollectionSpecimenID])
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[CollectionSpecimenImage] CHECK CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimen]
	GO

	ALTER TABLE [dbo].[CollectionSpecimenImage]  WITH NOCHECK ADD  CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	ON UPDATE CASCADE
	GO

	ALTER TABLE [dbo].[CollectionSpecimenImage] NOCHECK CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimenPart]
	GO

	
	-- UMBAU TRANSACTION
	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenTransaction_CollectionSpecimenPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenTransaction]'))
	ALTER TABLE [dbo].[CollectionSpecimenTransaction] DROP CONSTRAINT [FK_CollectionSpecimenTransaction_CollectionSpecimenPart]
	GO

	ALTER TABLE [dbo].[CollectionSpecimenTransaction]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenTransaction_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[CollectionSpecimenTransaction] CHECK CONSTRAINT [FK_CollectionSpecimenTransaction_CollectionSpecimenPart]
	GO


--4.)		Löschen der internen Verknuepfung

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenPart_CollectionSpecimenPart1]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenPart]'))
	ALTER TABLE [dbo].[CollectionSpecimenPart] DROP CONSTRAINT [FK_CollectionSpecimenPart_CollectionSpecimenPart1]
	GO

--5.)		Aktualisierung SpecimenPartID = ID

	UPDATE P SET P.SpecimenPartID = P.ID
	FROM [CollectionSpecimenPart] P

/*
Meldung 547, Ebene 16, Status 0, Zeile 2
Die UPDATE-Anweisung steht in Konflikt mit der REFERENCE-Einschränkung 'FK_IdentificationUnitAnalysis_IdentificationUnitInPart'. Der Konflikt trat in der 'DiversityCollection_TestPart'-Datenbank, Tabelle 'dbo.IdentificationUnitAnalysis' auf.
Die Anweisung wurde beendet.
*/


--6.)		Loeschen der Fremdschluessel

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitInPart_IdentificationUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitInPart]'))
	ALTER TABLE [dbo].[IdentificationUnitInPart] DROP CONSTRAINT [FK_IdentificationUnitInPart_IdentificationUnit1]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IdentificationUnitOnPart_CollectionSpecimenPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[IdentificationUnitInPart]'))
	ALTER TABLE [dbo].[IdentificationUnitInPart] DROP CONSTRAINT [FK_IdentificationUnitOnPart_CollectionSpecimenPart]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenTransaction_CollectionSpecimenPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenTransaction]'))
	ALTER TABLE [dbo].[CollectionSpecimenTransaction] DROP CONSTRAINT [FK_CollectionSpecimenTransaction_CollectionSpecimenPart]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenProcessing_CollectionSpecimenPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenProcessing]'))
	ALTER TABLE [dbo].[CollectionSpecimenProcessing] DROP CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart]
	GO
	
	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenImage_CollectionSpecimen]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenImage]'))
	ALTER TABLE [dbo].[CollectionSpecimenImage] DROP CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimen]
	GO

	IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionSpecimenImage_CollectionSpecimenPart]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenImage]'))
	ALTER TABLE [dbo].[CollectionSpecimenImage] DROP CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimenPart]
	GO
	


--7.)		Loeschen des PK

	IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CollectionSpecimenPart]') AND name = N'PK_CollectionSpecimenPart')
	ALTER TABLE [dbo].[CollectionSpecimenPart] DROP CONSTRAINT [PK_CollectionSpecimenPart]
	GO


--8.)		Umbenennen SpecimenPartID -> xx_SpecimenPartID	

	EXEC sp_rename 'CollectionSpecimenPart.SpecimenPartID' ,'xx_SpecimenPartID','COLUMN'


--9.)		Umbenennen ID -> SpecimenPartID

	EXEC sp_rename 'CollectionSpecimenPart.ID' ,'SpecimenPartID','COLUMN'


--10.)	Nachtragen der internen Verknuepfung

	UPDATE Pold SET Pold.DerivedFromSpecimenPartID = Pnew.SpecimenPartID
	FROM         CollectionSpecimenPart AS Pold INNER JOIN
	CollectionSpecimenPart AS Pnew ON Pold.DerivedFromSpecimenPartID = Pnew.xx_OldSpecimenPartID AND Pold.CollectionSpecimenID = Pnew.CollectionSpecimenID


--11.)	Anlegen des PK

	ALTER TABLE [dbo].[CollectionSpecimenPart] ADD  CONSTRAINT [PK_CollectionSpecimenPart] PRIMARY KEY CLUSTERED 
	(
		[CollectionSpecimenID] ASC,
		[SpecimenPartID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	GO


--12.)	Anlegen der Fremdschluessel

	ALTER TABLE [dbo].[CollectionSpecimenPart]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenPart_CollectionSpecimenPart1] FOREIGN KEY([CollectionSpecimenID], [DerivedFromSpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	GO

	ALTER TABLE [dbo].[CollectionSpecimenPart] CHECK CONSTRAINT [FK_CollectionSpecimenPart_CollectionSpecimenPart1]
	GO

	
	ALTER TABLE [dbo].[IdentificationUnitInPart]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitInPart_IdentificationUnit1] FOREIGN KEY([CollectionSpecimenID], [IdentificationUnitID])
	REFERENCES [dbo].[IdentificationUnit] ([CollectionSpecimenID], [IdentificationUnitID])
	GO

	ALTER TABLE [dbo].[IdentificationUnitInPart] CHECK CONSTRAINT [FK_IdentificationUnitInPart_IdentificationUnit1]
	GO

	ALTER TABLE [dbo].[IdentificationUnitInPart]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationUnitOnPart_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[IdentificationUnitInPart] CHECK CONSTRAINT [FK_IdentificationUnitOnPart_CollectionSpecimenPart]
	GO
	
	ALTER TABLE [dbo].[CollectionSpecimenTransaction]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenTransaction_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[CollectionSpecimenTransaction] CHECK CONSTRAINT [FK_CollectionSpecimenTransaction_CollectionSpecimenPart]
	GO

	ALTER TABLE [dbo].[CollectionSpecimenProcessing]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[CollectionSpecimenProcessing] CHECK CONSTRAINT [FK_CollectionSpecimenProcessing_CollectionSpecimenPart]
	GO

	ALTER TABLE [dbo].[CollectionSpecimenImage]  WITH NOCHECK ADD  CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimen] FOREIGN KEY([CollectionSpecimenID])
	REFERENCES [dbo].[CollectionSpecimen] ([CollectionSpecimenID])
	ON DELETE CASCADE
	GO

	ALTER TABLE [dbo].[CollectionSpecimenImage] CHECK CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimen]
	GO

	ALTER TABLE [dbo].[CollectionSpecimenImage]  WITH NOCHECK ADD  CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
	REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])
	ON UPDATE CASCADE
	GO

	ALTER TABLE [dbo].[CollectionSpecimenImage] NOCHECK CONSTRAINT [FK_CollectionSpecimenImage_CollectionSpecimenPart]
	GO


--13.)	Loeschen der Spalte xx_SpecimenPartID

	ALTER TABLE [dbo].[CollectionSpecimenPart] DROP  CONSTRAINT [DF_CollectionSpecimenPart_SpecimenPartID]
	GO

	ALTER TABLE [dbo].[CollectionSpecimenPart]
	DROP COLUMN [xx_SpecimenPartID]
	GO

--14.)	Nachtrag der PartID in CollectionSpecimenImage

	UPDATE I SET I.SpecimenPartID = P.SpecimenPartID
	FROM         CollectionSpecimenImage AS I INNER JOIN
	CollectionSpecimenPart AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID AND I.SpecimenPartID = P.xx_OldSpecimenPartID
	GO


--#####################################################################################################################
--######   [FirstLinesPart]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FirstLinesPart]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[FirstLinesPart] 
(@CollectionSpecimenIDs varchar(4000), 
@AnalysisIDs varchar(4000), @AnalysisStartDate date, @AnalysisEndDate date, 
@ProcessingID int, @ProcessingStartDate datetime, @ProcessingEndDate datetime)   
RETURNS @List TABLE (
	[SpecimenPartID] [int] Primary key
	)
AS 
	BEGIN 
	declare @IDs table (ID int  Primary key)
	RETURN 
END   
'
END
GO



grant select on [dbo].[FirstLinesPart] to DiversityCollectionUser
go




ALTER FUNCTION [dbo].[FirstLinesPart] 
(@CollectionSpecimenIDs varchar(4000), 
@AnalysisIDs varchar(4000), @AnalysisStartDate date, @AnalysisEndDate date, 
@ProcessingID int, @ProcessingStartDate datetime, @ProcessingEndDate datetime)   
RETURNS @List TABLE (
	[SpecimenPartID] [int] Primary key,
	[CollectionSpecimenID] [int], --
	[Accession_number] [nvarchar](50) NULL, --
-- WITHHOLDINGREASONS
	[Data_withholding_reason] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collection_event] [nvarchar](255) NULL, --
	[Data_withholding_reason_for_collector] [nvarchar](255) NULL, --
--CollectionEvent
	[Collectors_event_number] [nvarchar](50) NULL, --
	[Collection_day] [tinyint] NULL, --
	[Collection_month] [tinyint] NULL, --
	[Collection_year] [smallint] NULL, --
	[Collection_date_supplement] [nvarchar](100) NULL, --
	[Collection_time] [varchar](50) NULL, --
	[Collection_time_span] [varchar](50) NULL, --
	[Country] [nvarchar](50) NULL, --
	[Locality_description] [nvarchar](255) NULL, --
	[Habitat_description] [nvarchar](255) NULL, -- 
	[Collecting_method] [nvarchar](255) NULL, --
	[Collection_event_notes] [nvarchar](255) NULL, --
--Localisation
	[Named_area] [nvarchar](255) NULL, -- 
	[NamedAreaLocation2] [nvarchar](255) NULL, --
	[Remove_link_to_gazetteer] [int] NULL,
	[Distance_to_location] [varchar](50) NULL, --
	[Direction_to_location] [varchar](50) NULL, --
	[Longitude] [nvarchar](255) NULL, --
	[Latitude] [nvarchar](255) NULL, --
	[Coordinates_accuracy] [nvarchar](50) NULL, --
	[Link_to_GoogleMaps] [int] NULL,
	[Altitude_from] [nvarchar](255) NULL, --
	[Altitude_to] [nvarchar](255) NULL, --
	[Altitude_accuracy] [nvarchar](50) NULL, --
	[Notes_for_Altitude] [nvarchar](255) NULL, --
	[MTB] [nvarchar](255) NULL, --
	[Quadrant] [nvarchar](255) NULL, --
	[Notes_for_MTB] [nvarchar](255) NULL, --
	[Sampling_plot] [nvarchar](255) NULL, --
	[Link_to_SamplingPlots] [nvarchar](255) NULL, --
	[Remove_link_to_SamplingPlots] [int] NULL,
	[Accuracy_of_sampling_plot] [nvarchar](50) NULL, --
	[Latitude_of_sampling_plot] [real] NULL, --
	[Longitude_of_sampling_plot] [real] NULL, --
--Properties
	[Geographic_region] [nvarchar](255) NULL, --
	[Lithostratigraphy] [nvarchar](255) NULL, --
	[Chronostratigraphy] [nvarchar](255) NULL, --
--Agent
	[Collectors_name] [nvarchar](255) NULL, --
	[Link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_collector] [int] NULL,
	[Collectors_number] [nvarchar](50) NULL, --
	[Notes_about_collector] [nvarchar](max) NULL, --
--Accession
	[Accession_day] [tinyint] NULL, --
	[Accession_month] [tinyint] NULL, --
	[Accession_year] [smallint] NULL, --
	[Accession_date_supplement] [nvarchar](255) NULL, --
--Depositor
	[Depositors_name] [nvarchar](255) NULL, --
	[Depositors_link_to_DiversityAgents] [varchar](255) NULL, --
	[Remove_link_for_Depositor] [int] NULL,
	[Depositors_accession_number] [nvarchar](50) NULL, --
--Exsiccate
	[Exsiccata_abbreviation] [nvarchar](255) NULL, --
	[Link_to_DiversityExsiccatae] [varchar](255) NULL, --
	[Remove_link_to_exsiccatae] [int] NULL,
	[Exsiccata_number] [nvarchar](50) NULL, --
--Notes
	[Original_notes] [nvarchar](max) NULL, --
	[Additional_notes] [nvarchar](max) NULL, --
	[Internal_notes] [nvarchar](max) NULL, --
--Label
	[Label_title] [nvarchar](255) NULL, --
	[Label_type] [nvarchar](50) NULL, --
	[Label_transcription_state] [nvarchar](50) NULL, --
	[Label_transcription_notes] [nvarchar](255) NULL, --
	[Problems] [nvarchar](255) NULL, --
--Organism
	[Taxonomic_group] [nvarchar](50) NULL, --
	[Relation_type] [nvarchar](50) NULL, --
	[Colonised_substrate_part] [nvarchar](255) NULL, --
	[Related_organism] [nvarchar] (200) NULL,
	[Life_stage] [nvarchar](255) NULL, --
	[Gender] [nvarchar](50) NULL, --
	[Number_of_units] [smallint] NULL, --
	[Circumstances] [nvarchar](50) NULL, -- 
	[Order_of_taxon] [nvarchar](255) NULL, --
	[Family_of_taxon] [nvarchar](255) NULL, --
	[Identifier_of_organism] [nvarchar](50) NULL, --
	[Description_of_organism] [nvarchar](50) NULL, --
	[Only_observed] [bit] NULL, --
	[Notes_for_organism] [nvarchar](max) NULL, --
--Identification
	[Taxonomic_name] [nvarchar](255) NULL, --
	[Link_to_DiversityTaxonNames] [varchar](255) NULL, --
	[Remove_link_for_identification] [int] NULL, 
	[Vernacular_term] [nvarchar](255) NULL, --
	[Identification_day] [tinyint] NULL, -- 
	[Identification_month] [tinyint] NULL, --
	[Identification_year] [smallint] NULL, --
	[Identification_category] [nvarchar](50) NULL, --
	[Identification_qualifier] [nvarchar](50) NULL, --
	[Type_status] [nvarchar](50) NULL, --
	[Type_notes] [nvarchar](max) NULL, --
	[Notes_for_identification] [nvarchar](max) NULL, --
	[Reference_title] [nvarchar](255) NULL, --
	[Link_to_DiversityReferences] [varchar](255) NULL, --
	[Remove_link_for_reference] [int] NULL,
	[Determiner] [nvarchar](255) NULL,
	[Link_to_DiversityAgents_for_determiner] [varchar](255) NULL, --
	[Remove_link_for_determiner] [int] NULL,
--Analysis
	[Analysis_0] [nvarchar](50) NULL, --
	[AnalysisID_0] [int] NULL, --
	[Analysis_number_0] [nvarchar](50) NULL, --
	[Analysis_result_0] [nvarchar](max) NULL, --
	
	[Analysis_1] [nvarchar](50) NULL, --
	[AnalysisID_1] [int] NULL, --
	[Analysis_number_1] [nvarchar](50) NULL, --
	[Analysis_result_1] [nvarchar](max) NULL, --
	
	[Analysis_2] [nvarchar](50) NULL, --
	[AnalysisID_2] [int] NULL, --
	[Analysis_number_2] [nvarchar](50) NULL, --
	[Analysis_result_2] [nvarchar](max) NULL, --
	
	[Analysis_3] [nvarchar](50) NULL, --
	[AnalysisID_3] [int] NULL, --
	[Analysis_number_3] [nvarchar](50) NULL, --
	[Analysis_result_3] [nvarchar](max) NULL, --
	
	[Analysis_4] [nvarchar](50) NULL, --
	[AnalysisID_4] [int] NULL, --
	[Analysis_number_4] [nvarchar](50) NULL, --
	[Analysis_result_4] [nvarchar](max) NULL, --
	
	[Analysis_5] [nvarchar](50) NULL, --
	[AnalysisID_5] [int] NULL, --
	[Analysis_number_5] [nvarchar](50) NULL, --
	[Analysis_result_5] [nvarchar](max) NULL, --
	
	[Analysis_6] [nvarchar](50) NULL, --
	[AnalysisID_6] [int] NULL, --
	[Analysis_number_6] [nvarchar](50) NULL, --
	[Analysis_result_6] [nvarchar](max) NULL, --
	
	[Analysis_7] [nvarchar](50) NULL, --
	[AnalysisID_7] [int] NULL, --
	[Analysis_number_7] [nvarchar](50) NULL, --
	[Analysis_result_7] [nvarchar](max) NULL, --
	
	[Analysis_8] [nvarchar](50) NULL, --
	[AnalysisID_8] [int] NULL, --
	[Analysis_number_8] [nvarchar](50) NULL, --
	[Analysis_result_8] [nvarchar](max) NULL, --
	
	[Analysis_9] [nvarchar](50) NULL, --
	[AnalysisID_9] [int] NULL, --
	[Analysis_number_9] [nvarchar](50) NULL, --
	[Analysis_result_9] [nvarchar](max) NULL, --
	
--Storage	
	[Preparation_method] [nvarchar](max) NULL, --
	[Preparation_date] [datetime] NULL, --
	[Part_accession_number] [nvarchar](50) NULL, --
	[Part_sublabel] [nvarchar](50) NULL, --
	[Collection] [int] NULL, --
	[Material_category] [nvarchar](50) NULL, --
	[Storage_location] [nvarchar](255) NULL, --
	[Stock] [tinyint] NULL, --
	[Notes_for_part] [nvarchar](max) NULL, --
	
--Processing
	[Processing_date_1] [datetime] NULL,
	[ProcessingID_1] [int] NULL,
	[Processing_Protocoll_1] [nvarchar](100) NULL,
	[Processing_duration_1] [varchar](50) NULL,
	[Processing_notes_1] [nvarchar](max) NULL,

	[Processing_date_2] [datetime] NULL,
	[ProcessingID_2] [int] NULL,
	[Processing_Protocoll_2] [nvarchar](100) NULL,
	[Processing_duration_2] [varchar](50) NULL,
	[Processing_notes_2] [nvarchar](max) NULL,

	[Processing_date_3] [datetime] NULL,
	[ProcessingID_3] [int] NULL,
	[Processing_Protocoll_3] [nvarchar](100) NULL,
	[Processing_duration_3] [varchar](50) NULL,
	[Processing_notes_3] [nvarchar](max) NULL,

	[Processing_date_4] [datetime] NULL,
	[ProcessingID_4] [int] NULL,
	[Processing_Protocoll_4] [nvarchar](100) NULL,
	[Processing_duration_4] [varchar](50) NULL,
	[Processing_notes_4] [nvarchar](max) NULL,

	[Processing_date_5] [datetime] NULL,
	[ProcessingID_5] [int] NULL,
	[Processing_Protocoll_5] [nvarchar](100) NULL,
	[Processing_duration_5] [varchar](50) NULL,
	[Processing_notes_5] [nvarchar](max) NULL,

--Transaction
	[_TransactionID] [int] NULL, --
	[_Transaction] [nvarchar](200) NULL, --
	[On_loan] [int] NULL, --
--Hidden fields
	[_CollectionEventID] [int] NULL, --
	[_IdentificationUnitID] [int] NULL, --
	[_IdentificationSequence] [smallint] NULL, --
	[_SpecimenPartID] [int] NULL, --
	[_CoordinatesAverageLatitudeCache] [real] NULL, --
	[_CoordinatesAverageLongitudeCache] [real] NULL, --
	[_CoordinatesLocationNotes] [nvarchar](255) NULL, --
	[_GeographicRegionPropertyURI] [varchar](255) NULL, --
	[_LithostratigraphyPropertyURI] [varchar](255) NULL, --
	[_ChronostratigraphyPropertyURI] [varchar](255) NULL, --
	[_NamedAverageLatitudeCache] [real] NULL, --
	[_NamedAverageLongitudeCache] [real] NULL, --
	[_LithostratigraphyPropertyHierarchyCache] [nvarchar](255) NULL, --
	[_ChronostratigraphyPropertyHierarchyCache] [nvarchar](255) NULL, --
	[_AverageAltitudeCache] [real] NULL)     --
/* 
Returns a table that lists all the specimen with the first entries of related tables. 
MW 18.08.2011 
TEST: 
Select * from dbo.FirstLinesPart('3251, 3252', '34', null, null, null, null) order by CollectionSpecimenID, SpecimenPartID
Select * from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '1/1/2000', '12/12/2010') order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '1/1/2000', '12/12/2010') P order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, '2000/2/1', '2010/12/31') P order by CollectionSpecimenID, SpecimenPartID
Select P.Processing_date_0 from dbo.FirstLinesPart('3251, 3252, 177930', '34', null, null, null, null) P order by CollectionSpecimenID, SpecimenPartID
select * from CollectionSpecimenProcessing
Select * from dbo.FirstLinesPart('177930', '26,39,41,44,45', null, null, null, null, null) P order by CollectionSpecimenID, SpecimenPartID
Select * from dbo.FirstLinesPart('177930', '26,39,41,44,45', null, null, 8, null, null) P order by CollectionSpecimenID, SpecimenPartID
*/ 
AS 
BEGIN 

declare @IDs table (ID int  Primary key)
declare @sID varchar(50)
while @CollectionSpecimenIDs <> ''
begin
	if (CHARINDEX(',', @CollectionSpecimenIDs) > 0)
	begin
	set @sID = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, 1, CHARINDEX(',', @CollectionSpecimenIDs) -1)))
	set @CollectionSpecimenIDs = rtrim(ltrim(SUBSTRING(@CollectionSpecimenIDs, CHARINDEX(',', @CollectionSpecimenIDs) + 2, 4000)))
	if (isnumeric(@sID) = 1)
		begin
		insert into @IDs 
		values( @sID )
		end
	end
	else
	begin
	set @sID = rtrim(ltrim(@CollectionSpecimenIDs))
	set @CollectionSpecimenIDs = ''
	if (isnumeric(@sID) = 1)
		begin
		insert into @IDs 
		values( @sID )
		end
	end
end





--- Specimen
insert into @List (
SpecimenPartID
, CollectionSpecimenID
, Accession_number
, Data_withholding_reason
, _CollectionEventID
, Accession_day
, Accession_month
, Accession_year
, Accession_date_supplement
, Depositors_name
, Depositors_link_to_DiversityAgents
, Depositors_accession_number
, Exsiccata_abbreviation
, Link_to_DiversityExsiccatae
, Original_notes
, Additional_notes
, Internal_notes
, Label_title
, Label_type
, Label_transcription_state
, Label_transcription_notes
, Problems
, Part_accession_number
, [Collection]
, Material_category
, Notes_for_part
, Part_sublabel
, Preparation_date
, Preparation_method
, Stock
, Storage_location
)
select 
P.SpecimenPartID
, S.CollectionSpecimenID
, S.AccessionNumber
, S.DataWithholdingReason
, S.CollectionEventID 
, AccessionDay
, AccessionMonth
, AccessionYear
, AccessionDateSupplement
, DepositorsName
, DepositorsAgentURI
, DepositorsAccessionNumber
, ExsiccataAbbreviation
, ExsiccataURI
, OriginalNotes
, AdditionalNotes
, InternalNotes
, LabelTitle
, LabelType
, LabelTranscriptionState
, LabelTranscriptionNotes
, Problems
, P.AccessionNumber
, P.CollectionID
, P.MaterialCategory
, P.Notes
, P.PartSublabel
, P.PreparationDate
, P.PreparationMethod
, P.Stock
, P.StorageLocation
from dbo.CollectionSpecimen S, dbo.CollectionSpecimenPart P
where S.CollectionSpecimenID in (select ID from @IDs) 
and P.CollectionSpecimenID = S.CollectionSpecimenID 



--- Event

update L
set L.Collection_day = E.CollectionDay
, L.Collection_month = E.CollectionMonth
, L.Collection_year = E.CollectionYear
, L.Collection_date_supplement = E.CollectionDateSupplement
, L.Collection_time = E.CollectionTime
, L.Collection_time_span = E.CollectionTimeSpan
, L.Country = E.CountryCache
, L.Locality_description = cast(E.LocalityDescription as nvarchar(255))
, L.Habitat_description = cast(E.HabitatDescription as nvarchar(255))
, L.Collecting_method = cast(E.CollectingMethod as nvarchar(255))
, L.Collection_event_notes = cast(E.Notes as nvarchar(255))
, L.Data_withholding_reason_for_collection_event = E.DataWithholdingReason
, L.Collectors_event_number = E.CollectorsEventNumber
from @List L,
CollectionEvent E
where L._CollectionEventID = E.CollectionEventID



--- Named Area

update L
set L.Named_area = E.Location1
, L.NamedAreaLocation2 = E.Location2
, L.Distance_to_location = E.DistanceToLocation
, L.Direction_to_location = E.DirectionToLocation
, L._NamedAverageLatitudeCache = E.AverageLatitudeCache
, L._NamedAverageLongitudeCache = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 7


--- Coordinates

update L
set L.Longitude = E.Location1
, L.Latitude = E.Location2
, L.Coordinates_accuracy = E.LocationAccuracy
, L._CoordinatesAverageLatitudeCache = E.AverageLatitudeCache
, L._CoordinatesAverageLongitudeCache = E.AverageLongitudeCache
, L._CoordinatesLocationNotes = cast(E.LocationNotes as nvarchar (255))
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 8


--- Altitude

update L
set L.Altitude_from = E.Location1
, L.Altitude_to = E.Location2
, L.Altitude_accuracy = E.LocationAccuracy
, L._AverageAltitudeCache = E.AverageAltitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 4



--- MTB

update L
set L.MTB = E.Location1
, L.Quadrant = E.Location2
, L.Notes_for_MTB = cast(E.LocationNotes as nvarchar(255))
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 3



--- SamplingPlots

update L
set L.Sampling_plot = E.Location1
, L.Link_to_SamplingPlots = E.Location2
, L.Accuracy_of_sampling_plot = E.LocationAccuracy
, L.Latitude_of_sampling_plot = E.AverageLatitudeCache
, L.Longitude_of_sampling_plot = E.AverageLongitudeCache
from @List L,
dbo.CollectionEventLocalisation E
where L._CollectionEventID = E.CollectionEventID
and E.LocalisationSystemID = 13



--- GeographicRegions

update L
set L.Geographic_region = P.DisplayText
, L._GeographicRegionPropertyURI = P.PropertyURI
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 10


--- Lithostratigraphy

update L
set L.Lithostratigraphy = P.DisplayText
, L._LithostratigraphyPropertyURI = P.PropertyURI
, L._LithostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 30



--- Chronostratigraphy

update L
set L.Chronostratigraphy = P.DisplayText
, L._ChronostratigraphyPropertyURI = P.PropertyURI
, L._ChronostratigraphyPropertyHierarchyCache = cast(P.PropertyHierarchyCache as nvarchar (255))
from @List L,
dbo.CollectionEventProperty P
where L._CollectionEventID = P.CollectionEventID
and P.PropertyID = 20



--- Collector

update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
--,dbo.CollectionAgent Amin
where L.CollectionSpecimenID = A.CollectionSpecimenID
--and A.CollectionSpecimenID = Amin.CollectionSpecimenID
and EXISTS (SELECT CollectionSpecimenID
	FROM dbo.CollectionAgent AS Amin
	GROUP BY CollectionSpecimenID
	HAVING (A.CollectionSpecimenID = Amin.CollectionSpecimenID) 
	AND (MIN(Amin.CollectorsSequence) = A.CollectorsSequence))

update L
set L.Data_withholding_reason_for_collector = A.DataWithholdingReason
, L.Collectors_name = A.CollectorsName
, L.Link_to_DiversityAgents = A.CollectorsAgentURI
, L.Collectors_number = A.CollectorsNumber
, L.Notes_about_collector = A.Notes
from @List L,
dbo.CollectionAgent A
where L.CollectionSpecimenID = A.CollectionSpecimenID
and L.Collectors_name is null
and A.CollectorsSequence is null
and EXISTS (SELECT CollectionSpecimenID
	FROM dbo.CollectionAgent AS Amin
	GROUP BY CollectionSpecimenID
	HAVING (A.CollectionSpecimenID = Amin.CollectionSpecimenID) 
	AND (MIN(Amin.LogCreatedWhen) = A.LogCreatedWhen))


--- IdentificationUnit
-- getting the unit IDs of the part
declare @AllUnitIDs table (UnitID int, ID int, DisplayOrder smallint, PartID int)
declare @UnitIDs table (UnitID int, ID int, DisplayOrder smallint, PartID int)

insert into @AllUnitIDs (UnitID, ID, DisplayOrder, PartID)
select P.IdentificationUnitID, P.CollectionSpecimenID, P.DisplayOrder, P.SpecimenPartID
from @IDs as IDs, IdentificationUnitInPart as P, @List as L
where P.DisplayOrder > 0
and IDs.ID = P.CollectionSpecimenID 
and P.SpecimenPartID = L.SpecimenPartID

insert into @UnitIDs (UnitID, ID, DisplayOrder, PartID)
select U.UnitID, U.ID, U.DisplayOrder, U.PartID
from @AllUnitIDs as U
where exists (select * from @AllUnitIDs aU group by aU.ID having min(aU.DisplayOrder) = U.DisplayOrder)


update L
set L.Taxonomic_group = IU.TaxonomicGroup
, L._IdentificationUnitID = IU.IdentificationUnitID
, L.Relation_type = IU.RelationType
, L.Colonised_substrate_part = IU.ColonisedSubstratePart
, L.Life_stage = IU.LifeStage
, L.Gender = IU.Gender
, L.Number_of_units = IU.NumberOfUnits
, L.Circumstances = IU.Circumstances
, L.Order_of_taxon = IU.OrderCache
, L.Family_of_taxon = IU.FamilyCache
, L.Identifier_of_organism = IU.UnitIdentifier
, L.Description_of_organism = IU.UnitDescription
, L.Only_observed = IU.OnlyObserved
, L.Notes_for_organism = IU.Notes
, L.Exsiccata_number = IU.ExsiccataNumber
from @List L,
IdentificationUnitInPart UP,
dbo.IdentificationUnit IU,
@UnitIDs U
where L.CollectionSpecimenID = UP.CollectionSpecimenID
and L.SpecimenPartID = UP.SpecimenPartID
and L.CollectionSpecimenID = IU.CollectionSpecimenID
and UP.CollectionSpecimenID = IU.CollectionSpecimenID
and UP.IdentificationUnitID = IU.IdentificationUnitID
and U.UnitID = UP.IdentificationUnitID
and U.PartID = UP.SpecimenPartID


--- Identification

update L
set L._IdentificationSequence = I.IdentificationSequence
, L.Taxonomic_name = I.TaxonomicName
, L.Link_to_DiversityTaxonNames = I.NameURI
, L.Vernacular_term = I.VernacularTerm
, L.Identification_day = I.IdentificationDay
, L.Identification_month = I.IdentificationMonth
, L.Identification_year = I.IdentificationYear
, L.Identification_category = I.IdentificationCategory
, L.Identification_qualifier = I.IdentificationQualifier
, L.Type_status = I.TypeStatus
, L.Type_notes = I.TypeNotes
, L.Notes_for_identification = I.Notes
, L.Reference_title = I.ReferenceTitle
, L.Link_to_DiversityReferences = I.ReferenceURI
, L.Determiner = I.ResponsibleName
, L.Link_to_DiversityAgents_for_determiner = I.ResponsibleAgentURI
from @List L,
dbo.Identification I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.Identification AS Imax
	GROUP BY CollectionSpecimenID, IdentificationUnitID
	HAVING (Imax.CollectionSpecimenID = I.CollectionSpecimenID) AND (Imax.IdentificationUnitID = I.IdentificationUnitID) AND 
	(MAX(Imax.IdentificationSequence) = I.IdentificationSequence))



-- ANALYSIS --- ###############################################################################################################

-- getting the AnalysisID's that should be shown

declare @AnalysisID_0 int
declare @AnalysisID_1 int
declare @AnalysisID_2 int
declare @AnalysisID_3 int
declare @AnalysisID_4 int
declare @AnalysisID_5 int
declare @AnalysisID_6 int
declare @AnalysisID_7 int
declare @AnalysisID_8 int
declare @AnalysisID_9 int


if (not @AnalysisIDs is null and @AnalysisIDs <> '')
begin
	declare @AnalysisID table (ID int Identity(0,1), AnalysisID int Primary key)
	declare @sAnalysisID varchar(50)
	declare @iAnalysis int
	set @iAnalysis = 0
	while @AnalysisIDs <> '' and @iAnalysis < 10
	begin
		if (CHARINDEX(',', @AnalysisIDs) > 0)
		begin
		set @sAnalysisID = rtrim(ltrim(SUBSTRING(@AnalysisIDs, 1, CHARINDEX(',', @AnalysisIDs) -1)))
		set @AnalysisIDs = rtrim(ltrim(SUBSTRING(@AnalysisIDs, CHARINDEX(',', @AnalysisIDs) + 1, 4000)))
		if (isnumeric(@sID) = 1 and (select count(*) from @AnalysisID where AnalysisID = @sID) = 0)
			begin
			insert into @AnalysisID (AnalysisID)
			values( @sAnalysisID )
			end
		end
		else
		begin
		set @sAnalysisID = rtrim(ltrim(@AnalysisIDs))
		set @AnalysisIDs = ''
		if (isnumeric(@sAnalysisID) = 1 and (select count(*) from @AnalysisID where AnalysisID = @sID) = 0)
			begin
			insert into @AnalysisID (AnalysisID)
			values( @sAnalysisID )
			end
		end
		set @iAnalysis = (select count(*) from @AnalysisID)
	end
		
	set @AnalysisID_0 = (select AnalysisID from @AnalysisID where ID = 0)
	set @AnalysisID_1 = (select AnalysisID from @AnalysisID where ID = 1)
	set @AnalysisID_2 = (select AnalysisID from @AnalysisID where ID = 2)
	set @AnalysisID_3 = (select AnalysisID from @AnalysisID where ID = 3)
	set @AnalysisID_4 = (select AnalysisID from @AnalysisID where ID = 4)
	set @AnalysisID_5 = (select AnalysisID from @AnalysisID where ID = 5)
	set @AnalysisID_6 = (select AnalysisID from @AnalysisID where ID = 6)
	set @AnalysisID_7 = (select AnalysisID from @AnalysisID where ID = 7)
	set @AnalysisID_8 = (select AnalysisID from @AnalysisID where ID = 8)
	set @AnalysisID_9 = (select AnalysisID from @AnalysisID where ID = 9)
end


--############### ANALYSIS 0 ###############


update L
set L.AnalysisID_0 = I.AnalysisID
, L.Analysis_number_0 = I.AnalysisNumber
, L.Analysis_result_0 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_0
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_0)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_0 = null
	, L.Analysis_number_0 = null
	, L.Analysis_result_0 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_0
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_0 = null
	, L.Analysis_number_0 = null
	, L.Analysis_result_0 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_0
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end

update L
set L.Analysis_0 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_0 = A.AnalysisID



--############### ANALYSIS 1 ###############

update L
set L.AnalysisID_1 = I.AnalysisID
, L.Analysis_number_1 = I.AnalysisNumber
, L.Analysis_result_1 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_1
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_1)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_1 = null
	, L.Analysis_number_1 = null
	, L.Analysis_result_1 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_1
	and I.AnalysisID = @AnalysisID_1
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_1 = null
	, L.Analysis_number_1 = null
	, L.Analysis_result_1 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_1
	and I.AnalysisID = @AnalysisID_1
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_1 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_1 = A.AnalysisID


--############### ANALYSIS 2 ###############

update L
set L.AnalysisID_2 = I.AnalysisID
, L.Analysis_number_2 = I.AnalysisNumber
, L.Analysis_result_2 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_2
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_2
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_2)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_2 = null
	, L.Analysis_number_2 = null
	, L.Analysis_result_2 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_2
	and I.AnalysisID = @AnalysisID_2
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_2 = null
	, L.Analysis_number_2 = null
	, L.Analysis_result_2 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_2
	and I.AnalysisID = @AnalysisID_2
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_2 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_2 = A.AnalysisID



--############### ANALYSIS 3 ###############

update L
set L.AnalysisID_3 = I.AnalysisID
, L.Analysis_number_3 = I.AnalysisNumber
, L.Analysis_result_3 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_3
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_3
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_3)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_3 = null
	, L.Analysis_number_3 = null
	, L.Analysis_result_3 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_3
	and I.AnalysisID = @AnalysisID_3
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_3 = null
	, L.Analysis_number_3 = null
	, L.Analysis_result_3 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_3
	and I.AnalysisID = @AnalysisID_3
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_3 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_3 = A.AnalysisID


--############### ANALYSIS 4 ###############

update L
set L.AnalysisID_4 = I.AnalysisID
, L.Analysis_number_4 = I.AnalysisNumber
, L.Analysis_result_4 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_4
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_4
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_4)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_4 = null
	, L.Analysis_number_4 = null
	, L.Analysis_result_4 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_4
	and I.AnalysisID = @AnalysisID_4
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_4 = null
	, L.Analysis_number_4 = null
	, L.Analysis_result_4 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_4
	and I.AnalysisID = @AnalysisID_4
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_4 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_4 = A.AnalysisID



--############### ANALYSIS 5 ###############

update L
set L.AnalysisID_5 = I.AnalysisID
, L.Analysis_number_5 = I.AnalysisNumber
, L.Analysis_result_5 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_5
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_5
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_5)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_5 = null
	, L.Analysis_number_5 = null
	, L.Analysis_result_5 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_5
	and I.AnalysisID = @AnalysisID_5
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_5 = null
	, L.Analysis_number_5 = null
	, L.Analysis_result_5 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_5
	and I.AnalysisID = @AnalysisID_5
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_5 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_5 = A.AnalysisID




--############### ANALYSIS 6 ###############

update L
set L.AnalysisID_6 = I.AnalysisID
, L.Analysis_number_6 = I.AnalysisNumber
, L.Analysis_result_6 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_6
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_6
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_6)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_6 = null
	, L.Analysis_number_6 = null
	, L.Analysis_result_6 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_6
	and I.AnalysisID = @AnalysisID_6
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_6 = null
	, L.Analysis_number_6 = null
	, L.Analysis_result_6 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_6
	and I.AnalysisID = @AnalysisID_6
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_6 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_6 = A.AnalysisID



--############### ANALYSIS 7 ###############

update L
set L.AnalysisID_7 = I.AnalysisID
, L.Analysis_number_7 = I.AnalysisNumber
, L.Analysis_result_7 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_7
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_7
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_7)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))

if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_7 = null
	, L.Analysis_number_7 = null
	, L.Analysis_result_7 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_7
	and I.AnalysisID = @AnalysisID_0
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_7 = null
	, L.Analysis_number_7 = null
	, L.Analysis_result_7 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_7
	and I.AnalysisID = @AnalysisID_7
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_7 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_7 = A.AnalysisID


--############### ANALYSIS 8 ###############

update L
set L.AnalysisID_8 = I.AnalysisID
, L.Analysis_number_8 = I.AnalysisNumber
, L.Analysis_result_8 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_8
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_8
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_8)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_8 = null
	, L.Analysis_number_8 = null
	, L.Analysis_result_8 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_8
	and I.AnalysisID = @AnalysisID_8
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_8 = null
	, L.Analysis_number_8 = null
	, L.Analysis_result_8 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_8
	and I.AnalysisID = @AnalysisID_8
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_8 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_8 = A.AnalysisID



--############### ANALYSIS 9 ###############

update L
set L.AnalysisID_9 = I.AnalysisID
, L.Analysis_number_9 = I.AnalysisNumber
, L.Analysis_result_9 = I.AnalysisResult
from @List L,
dbo.IdentificationUnitAnalysis I
where L.CollectionSpecimenID = I.CollectionSpecimenID
and L._IdentificationUnitID = I.IdentificationUnitID
and L.SpecimenPartID = I.SpecimenPartID
and I.AnalysisID = @AnalysisID_9
and EXISTS
	(SELECT CollectionSpecimenID
	FROM dbo.IdentificationUnitAnalysis AS Imin
	WHERE Imin.AnalysisID = @AnalysisID_9
	GROUP BY Imin.CollectionSpecimenID, Imin.IdentificationUnitID, Imin.AnalysisID, Imin.SpecimenPartID
	HAVING (Imin.CollectionSpecimenID = I.CollectionSpecimenID) 
	AND (Imin.IdentificationUnitID = I.IdentificationUnitID) 
	AND (Imin.SpecimenPartID = I.SpecimenPartID)
	AND (Imin.AnalysisID = @AnalysisID_9)
	AND (Max(Imin.AnalysisNumber) = I.AnalysisNumber))


if (not @AnalysisStartDate is null)
begin
	update L 
	set L.AnalysisID_9 = null
	, L.Analysis_number_9 = null
	, L.Analysis_result_9 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_9
	and I.AnalysisID = @AnalysisID_9
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) < @AnalysisStartDate
end

if (not @AnalysisEndDate is null)
begin
	update L 
	set L.AnalysisID_9 = null
	, L.Analysis_number_9 = null
	, L.Analysis_result_9 = null
	from @List L,
	dbo.IdentificationUnitAnalysis I
	where L.CollectionSpecimenID = I.CollectionSpecimenID
	and L._IdentificationUnitID = I.IdentificationUnitID
	and L.SpecimenPartID = I.SpecimenPartID
	and I.AnalysisID = L.AnalysisID_9
	and I.AnalysisID = @AnalysisID_9
	and isdate(I.AnalysisDate) = 1
	and cast(I.AnalysisDate as date) > @AnalysisEndDate
end


update L
set L.Analysis_9 = A.DisplayText
from @List L,
dbo.Analysis A
where L.AnalysisID_9 = A.AnalysisID




--- Related Organism
Update L
set L.Related_Organism = Urel.LastIdentificationCache
from  @List L
, dbo.IdentificationUnit U
, dbo.IdentificationUnit Urel
where U.IdentificationUnitID = L._IdentificationUnitID
and U.RelatedUnitID = Urel.IdentificationUnitID	
and U.CollectionSpecimenID = Urel.CollectionSpecimenID
	
	
	
-- PROCESSING --- ###############################################################################################################


-- getting all ProcessingID's

declare @ProcessingIDs varchar(4000)
if (@ProcessingID is null)
begin
	declare @Processing table (ProcessingID int Primary key)
	insert into @Processing
	select ProcessingID from Processing
end
else
begin
	insert into @Processing
	select @ProcessingID
end



-- setting the Processing date range that should be shown

declare @StartDate datetime
declare @EndDate datetime

if (ISDATE(@ProcessingStartDate) = 1) begin set @StartDate = @ProcessingStartDate end
else begin set @StartDate = (select MIN(ProcessingDate) from CollectionSpecimenProcessing) end

if (ISDATE(@ProcessingEndDate) = 1) begin set @EndDate = @ProcessingEndDate end
else begin set @EndDate = (select MAX(ProcessingDate) from CollectionSpecimenProcessing) end


-- PROCESSING 1 --- ###############################################################################################################

update L
set L.Processing_date_1 = P.ProcessingDate
, L.ProcessingID_1 = P.ProcessingID
, L.Processing_duration_1 = P.ProcessingDuration
, L.Processing_notes_1 = P.Notes
, L.Processing_Protocoll_1 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID



-- PROCESSING 2 --- ###############################################################################################################

update L
set L.Processing_date_2 = P.ProcessingDate
, L.ProcessingID_2 = P.ProcessingID
, L.Processing_duration_2 = P.ProcessingDuration
, L.Processing_notes_2 = P.Notes
, L.Processing_Protocoll_2 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_1
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID


-- PROCESSING 3 --- ###############################################################################################################

update L
set L.Processing_date_3 = P.ProcessingDate
, L.ProcessingID_3 = P.ProcessingID
, L.Processing_duration_3 = P.ProcessingDuration
, L.Processing_notes_3 = P.Notes
, L.Processing_Protocoll_3 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_2
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID




-- PROCESSING 4 --- ###############################################################################################################

update L
set L.Processing_date_4 = P.ProcessingDate
, L.ProcessingID_4 = P.ProcessingID
, L.Processing_duration_4 = P.ProcessingDuration
, L.Processing_notes_4 = P.Notes
, L.Processing_Protocoll_4 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.SpecimenPartID = P.SpecimenPartID
AND S.ProcessingDate > L.Processing_date_3
AND S.SpecimenPartID = L.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID

-- PROCESSING 5 --- ###############################################################################################################


update L
set L.Processing_date_5 = P.ProcessingDate
, L.ProcessingID_5 = P.ProcessingID
, L.Processing_duration_5 = P.ProcessingDuration
, L.Processing_notes_5 = P.Notes
, L.Processing_Protocoll_5 = P.Protocoll
from @List L, CollectionSpecimenProcessing P, @Processing PP
where P.ProcessingDate between @StartDate and @EndDate
and L.SpecimenPartID = P.SpecimenPartID
and L.CollectionSpecimenID = P.CollectionSpecimenID
and P.ProcessingDate = 
(
SELECT MIN(ProcessingDate) FROM CollectionSpecimenProcessing S 
, @Processing PP2
WHERE S.ProcessingDate BETWEEN @StartDate AND @EndDate 
AND S.ProcessingDate > L.Processing_date_4
AND S.SpecimenPartID = P.SpecimenPartID
and S.ProcessingID = PP2.ProcessingID
GROUP BY S.SpecimenPartID
)
and P.ProcessingID = PP.ProcessingID




--- Transaction

update L
set L._TransactionID = P.TransactionID
, L.On_loan = P.IsOnLoan
from @List L,
dbo.CollectionSpecimenTransaction P
where L.CollectionSpecimenID = P.CollectionSpecimenID
and L.SpecimenPartID = P.SpecimenPartID
and EXISTS
	(SELECT Tmin.CollectionSpecimenID
	FROM dbo.CollectionSpecimenTransaction AS Tmin
	GROUP BY Tmin.CollectionSpecimenID, Tmin.SpecimenPartID
	HAVING (Tmin.CollectionSpecimenID = P.CollectionSpecimenID) 
	AND Tmin.SpecimenPartID = P.SpecimenPartID
	AND (MIN(Tmin.TransactionID) = P.TransactionID))


update L
set L._Transaction = T.TransactionTitle
from @List L,
dbo.[Transaction] T
where L._TransactionID = T.TransactionID
	
                  
RETURN 
END   


GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################





/*

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.13'
END

GO
*/

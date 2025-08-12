
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.32'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   AnalysisHierarchyAll   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[AnalysisHierarchyAll] ()  
RETURNS @AnalysisList TABLE ([AnalysisID] [int] Primary key ,
	[AnalysisParentID] [int] NULL ,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[MeasurementUnit] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[AnalysisURI] [varchar](255) NULL,
	[OnlyHierarchy] [bit] NULL,
	[HierarchyDisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the analysis items related to the given analysis.
MW 02.01.2006
TEST:
SELECT * FROM DBO.AnalysisHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @AnalysisList (AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy, HierarchyDisplayText)
SELECT DISTINCT AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy
, DisplayText
FROM Analysis
WHERE Analysis.AnalysisParentID IS NULL

declare @i int
set @i = (select count(*) from Analysis where AnalysisID not IN (select AnalysisID from  @AnalysisList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @AnalysisList (AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy, HierarchyDisplayText)
	SELECT DISTINCT C.AnalysisID, C.AnalysisParentID, C.DisplayText, C.Description, C.MeasurementUnit, C.Notes, C.AnalysisURI, C.OnlyHierarchy, L.HierarchyDisplayText + ' | ' + C.DisplayText
	FROM Analysis C, @AnalysisList L
	WHERE C.AnalysisParentID = L.AnalysisID
	AND C.AnalysisID NOT IN (select AnalysisID from  @AnalysisList)

	set @i = (select count(*) from Analysis where AnalysisID not IN (select AnalysisID from  @AnalysisList))
end


   RETURN
END

GO

GRANT SELECT ON [dbo].[AnalysisHierarchyAll] TO [User]
GO



--#####################################################################################################################
--######   [ProcessingHierarchyAll]   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[ProcessingHierarchyAll] ()  
RETURNS @ProcessingList TABLE ([ProcessingID] [int] Primary key ,
	[ProcessingParentID] [int] NULL ,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[ProcessingGroup] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[ProcessingURI] [varchar](255) NULL,
	[OnlyHierarchy] [bit] NULL,
	[HierarchyDisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the Processing items related to the given Processing.
MW 02.01.2006
TEST:
SELECT [HierarchyDisplayText], * FROM DBO.ProcessingHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @ProcessingList (ProcessingID, ProcessingParentID, DisplayText, Description, ProcessingGroup, Notes, ProcessingURI, OnlyHierarchy, HierarchyDisplayText)
SELECT DISTINCT ProcessingID, ProcessingParentID, DisplayText, Description, ProcessingGroup, Notes, ProcessingURI, OnlyHierarchy
, DisplayText
FROM Processing
WHERE Processing.ProcessingParentID IS NULL

declare @i int
set @i = (select count(*) from Processing where ProcessingID not IN (select ProcessingID from  @ProcessingList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @ProcessingList (ProcessingID, ProcessingParentID, DisplayText, Description, ProcessingGroup, Notes, ProcessingURI, OnlyHierarchy, HierarchyDisplayText)
	SELECT DISTINCT C.ProcessingID, C.ProcessingParentID, C.DisplayText, C.Description, C.ProcessingGroup, C.Notes, C.ProcessingURI, C.OnlyHierarchy, L.HierarchyDisplayText + ' | ' + C.DisplayText
	FROM Processing C, @ProcessingList L
	WHERE C.ProcessingParentID = L.ProcessingID
	AND C.ProcessingID NOT IN (select ProcessingID from  @ProcessingList)

	set @i = (select count(*) from Processing where ProcessingID not IN (select ProcessingID from  @ProcessingList))
end


   RETURN
END

GO

GRANT SELECT ON [dbo].[ProcessingHierarchyAll] TO [User]
GO




--#####################################################################################################################
--######   [ToolHierarchyAll]   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[ToolHierarchyAll] ()  
RETURNS @ToolList TABLE ([ToolID] [int] Primary key ,
	[ToolParentID] [int] NULL ,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[ToolUsageTemplate] xml,
	[Notes] [nvarchar](max) NULL,
	[ToolURI] [varchar](255) NULL,
	[OnlyHierarchy] [bit] NULL,
	[HierarchyDisplayText] [nvarchar] (900) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the Tool items related to the given Tool.
MW 02.01.2006
TEST:
SELECT * FROM DBO.ToolHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @ToolList (ToolID, ToolParentID, Name, Description, ToolUsageTemplate, Notes, ToolURI, OnlyHierarchy, HierarchyDisplayText)
SELECT DISTINCT ToolID, ToolParentID, Name, Description, CAST(ToolUsageTemplate AS nvarchar(4000)), Notes, ToolURI, OnlyHierarchy, Name
FROM Tool
WHERE Tool.ToolParentID IS NULL

declare @i int
set @i = (select count(*) from Tool where ToolID not IN (select ToolID from  @ToolList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @ToolList (ToolID, ToolParentID, Name, Description, ToolUsageTemplate, Notes, ToolURI, OnlyHierarchy, HierarchyDisplayText)
	SELECT DISTINCT C.ToolID, C.ToolParentID, C.Name, C.Description, CAST(C.ToolUsageTemplate AS nvarchar(4000)), C.Notes, C.ToolURI, C.OnlyHierarchy, L.HierarchyDisplayText + ' | ' + C.Name
	FROM Tool C, @ToolList L
	WHERE C.ToolParentID = L.ToolID
	AND C.ToolID NOT IN (select ToolID from  @ToolList)

	set @i = (select count(*) from Tool where ToolID not IN (select ToolID from  @ToolList))
end


   RETURN
END

GO

GRANT SELECT ON [dbo].[ToolHierarchyAll] TO [User]
GO



--#####################################################################################################################
--######   [TransactionHierarchyAll]   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[TransactionHierarchyAll] ()  
RETURNS @TransactionList TABLE ([TransactionID] [int] Primary key ,
	[ParentTransactionID] [int] NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionTitle] [nvarchar](200) NOT NULL,
	[ReportingCategory] [nvarchar](50) NULL,
	[AdministratingCollectionID] [int] NOT NULL,
	[MaterialDescription] [nvarchar](max) NULL,
	[MaterialCategory] [nvarchar](50) NULL,
	[MaterialCollectors] [nvarchar](max) NULL,
	[FromCollectionID] [int] NULL,
	[FromTransactionPartnerName] [nvarchar](255) NULL,
	[FromTransactionPartnerAgentURI] [varchar](255) NULL,
	[FromTransactionNumber] [nvarchar](50) NULL,
	[ToCollectionID] [int] NULL,
	[ToTransactionPartnerName] [nvarchar](255) NULL,
	[ToTransactionPartnerAgentURI] [varchar](255) NULL,
	[ToTransactionNumber] [nvarchar](50) NULL,
	[NumberOfUnits] [smallint] NULL,
	[Investigator] [nvarchar](50) NULL,
	[TransactionComment] [nvarchar](max) NULL,
	[BeginDate] [datetime] NULL,
	[AgreedEndDate] [datetime] NULL,
	[ActualEndDate] [datetime] NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[ResponsibleName] [nvarchar](255) NULL,
	[ResponsibleAgentURI] [varchar](255) NULL,
	[DisplayText] [nvarchar](500),
	[HierarchyDisplayText] [nvarchar](500))

/*
Returns a table that lists all the transactions items related to the given analysis.
MW 02.01.2006
TEST:
SELECT * FROM DBO.TransactionHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @TransactionList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, 
MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText)
SELECT DISTINCT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, 
MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI
, TransactionTitle
FROM [Transaction]
WHERE [Transaction].ParentTransactionID IS NULL

declare @i int
set @i = (select count(*) from [Transaction] where TransactionID not IN (select TransactionID from  @TransactionList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @TransactionList (TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, 
		MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, 
		ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, 
		ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText)
	SELECT DISTINCT C.TransactionID, C.ParentTransactionID, C.TransactionType, C.TransactionTitle, C.ReportingCategory, C.AdministratingCollectionID, C.MaterialDescription, C.MaterialCategory, 
		C.MaterialCollectors, C.FromCollectionID, C.FromTransactionPartnerName, C.FromTransactionPartnerAgentURI, C.FromTransactionNumber, C.ToCollectionID, 
		C.ToTransactionPartnerName, C.ToTransactionPartnerAgentURI, C.ToTransactionNumber, C.NumberOfUnits, C.Investigator, C.TransactionComment, C.BeginDate, C.AgreedEndDate, 
		C.ActualEndDate, C.InternalNotes, C.ResponsibleName, C.ResponsibleAgentURI, L.DisplayText + ' | ' + C.TransactionTitle
	FROM [Transaction] C, @TransactionList L
	WHERE C.ParentTransactionID = L.TransactionID
	AND C.TransactionID NOT IN (select TransactionID from  @TransactionList)

	set @i = (select count(*) from [Transaction] where TransactionID not IN (select TransactionID from  @TransactionList))
end

update T set HierarchyDisplayText = DisplayText
from @TransactionList T


   RETURN
END

GO

GRANT SELECT ON [dbo].[TransactionHierarchyAll] TO [User]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.33'
END

GO



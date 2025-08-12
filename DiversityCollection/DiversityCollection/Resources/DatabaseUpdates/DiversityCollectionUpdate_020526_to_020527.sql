
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.26'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   [IdentificationUnit_Core]   ######################################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[IdentificationUnit_Core]
AS
SELECT     U.CollectionSpecimenID, U.IdentificationUnitID, U.LastIdentificationCache, 
                      U.FamilyCache, U.OrderCache, U.TaxonomicGroup, U.OnlyObserved, 
                      U.RelatedUnitID, U.RelationType, U.ColonisedSubstratePart, 
                      U.LifeStage, U.Gender, U.NumberOfUnits, U.ExsiccataNumber, 
                      U.ExsiccataIdentification, U.UnitIdentifier, U.UnitDescription, 
                      U.Circumstances, U.DisplayOrder, U.Notes, U.HierarchyCache
FROM         dbo.IdentificationUnit U INNER JOIN
                      dbo.CollectionSpecimenID_UserAvailable A ON 
                      U.CollectionSpecimenID = A.CollectionSpecimenID

GO


--#####################################################################################################################
--######   [ProjectURI]   ######################################################################################
--#####################################################################################################################


ALTER TABLE ProjectProxy ADD ProjectURI varchar(255) NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the project, e.g. as provided by the module DiversityProjects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectURI'
GO


declare @ProjectDB nvarchar(50);
set @ProjectDB = (select MIN(S.CATALOG_NAME) from INFORMATION_SCHEMA.SCHEMATA S)
declare @Module nvarchar(50);
set @Module = (select dbo.DiversityWorkbenchModule())
set @ProjectDB = 'DiversityProjects' + SUBSTRING(@ProjectDB, len(@Module) + 1, 255)
declare @Command nvarchar(500)
DECLARE @ParmDefinition nvarchar(500);
SET @ParmDefinition = N'@UrlOut varchar(60) OUTPUT';
set @Command = (' Select @UrlOut = '  + @ProjectDB + '.dbo.BaseURL()');
declare @BaseURL varchar(255)
Execute sp_executesql @Command, @ParmDefinition,  @UrlOut=@BaseURL OUTPUT;
select @BaseURL


UPDATE P SET P.ProjectURI = @BaseURL + cast(P.ProjectID as varchar)
From ProjectProxy P
where P.ProjectID > 0
GO

--#####################################################################################################################
--######   [DiversityMobile_IdentificationUnitAltitude]   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[DiversityMobile_IdentificationUnitAltitude] (@IdentificationUnitID int)   
RETURNS float
/* 
Returns the first latitude of a unit as stored in table IdentificationUnitGeoAnalysis. 
MW 08.08.2009 
TEST: 
Select dbo.[DiversityMobile_IdentificationUnitAltitude](236379)  
*/ 
AS BEGIN  
declare @F float
set @F = (select [Geography].EnvelopeCenter().Z from [IdentificationUnitGeoAnalysis]
where [IdentificationUnitID] = @IdentificationUnitID
and [AnalysisDate] = 
(
select max([AnalysisDate]) from [IdentificationUnitGeoAnalysis] where [IdentificationUnitID] = @IdentificationUnitID
))

RETURN @F
END  

GO


grant execute on [DiversityMobile_IdentificationUnitAltitude] TO [User]
GO

--#####################################################################################################################
--######   [DiversityMobile_IdentificationUnitLatitude]   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[DiversityMobile_IdentificationUnitLatitude] (@IdentificationUnitID int)   
RETURNS float
/* 
Returns the first latitude of a unit as stored in table IdentificationUnitGeoAnalysis. 
MW 08.08.2009 
TEST: 
Select dbo.[DiversityMobile_IdentificationUnitLatitude](97586)  
*/ 
AS BEGIN  
declare @F float
set @F = (select [Geography].EnvelopeCenter().Lat from [IdentificationUnitGeoAnalysis]
where [IdentificationUnitID] = @IdentificationUnitID
and [AnalysisDate] = 
(
select max([AnalysisDate]) from [IdentificationUnitGeoAnalysis] where [IdentificationUnitID] = @IdentificationUnitID
))

RETURN @F
END  



GO


grant execute on [DiversityMobile_IdentificationUnitLatitude] TO [User]
GO


--#####################################################################################################################
--######   [DiversityMobile_IdentificationUnitLongitude]   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[DiversityMobile_IdentificationUnitLongitude] (@IdentificationUnitID int)   
RETURNS float
/* 
Returns the first latitude of a unit as stored in table IdentificationUnitGeoAnalysis. 
MW 08.08.2009 
TEST: 
Select dbo.[DiversityMobile_IdentificationUnitLongitude](236379)  
*/ 
AS BEGIN  
declare @F float
set @F = (select [Geography].EnvelopeCenter().Long from [IdentificationUnitGeoAnalysis]
where [IdentificationUnitID] = @IdentificationUnitID
and [AnalysisDate] = 
(
select max([AnalysisDate]) from [IdentificationUnitGeoAnalysis] where [IdentificationUnitID] = @IdentificationUnitID
))

RETURN @F
END  


GO



grant execute on [DiversityMobile_IdentificationUnitLongitude] TO [User]
GO



--#####################################################################################################################
--######   [DiversityMobile_EventsForProject]   ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[DiversityMobile_EventsForProject] (@ProjectID int, @Locality nvarchar(500))   
RETURNS @Events TABLE (
	[CollectionEventID] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[SeriesID] [int] NULL,
	[CollectorsEventNumber] [nvarchar](50) NULL,
	[CollectionDate] [datetime] NULL,
	[CollectionDay] [int] NULL,
	[CollectionMonth] [int] NULL,
	[CollectionYear] [int] NULL,
	[CollectionDateSupplement] [nvarchar](100) NULL,
	[CollectionDateCategory] [nvarchar](50) NULL,
	[CollectionTime] [varchar](50) NULL,
	[CollectionTimeSpan] [varchar](50) NULL,
	[LocalityDescription] [nvarchar](max) NULL,
	[HabitatDescription] [nvarchar](max) NULL,
	[ReferenceTitle] [nvarchar](255) NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[CollectingMethod] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CountryCache] [nvarchar](50) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL)
/* 
Returns a table that lists all the analysis items related to the given project. 
MW 08.08.2009 
TEST: 
Select * from DiversityMobile_AnalysisProjectList(3)  
Select * from DiversityMobile_AnalysisProjectList(372)  
*/ 
AS BEGIN  

INSERT INTO @Events
           ([CollectionEventID]
           ,[Version]
           ,[SeriesID]
           ,[CollectorsEventNumber]
           ,[CollectionDate]
           ,[CollectionDay]
           ,[CollectionMonth]
           ,[CollectionYear]
           ,[CollectionDateSupplement]
           ,[CollectionDateCategory]
           ,[CollectionTime]
           ,[CollectionTimeSpan]
           ,[LocalityDescription]
           ,[HabitatDescription]
           ,[ReferenceTitle]
           ,[ReferenceURI]
           ,[ReferenceDetails]
           ,[CollectingMethod]
           ,[Notes]
           ,[CountryCache]
           ,[DataWithholdingReason]
           ,[LogCreatedWhen]
           ,[LogCreatedBy]
           ,[LogUpdatedWhen]
           ,[LogUpdatedBy]
           ,[RowGUID])
SELECT     E.CollectionEventID, E.Version, E.SeriesID, E.CollectorsEventNumber, E.CollectionDate, E.CollectionDay, E.CollectionMonth, E.CollectionYear, 
      E.CollectionDateSupplement, E.CollectionDateCategory, E.CollectionTime, E.CollectionTimeSpan, E.LocalityDescription, E.HabitatDescription, E.ReferenceTitle, 
      E.ReferenceURI, E.ReferenceDetails, E.CollectingMethod, E.Notes, E.CountryCache, E.DataWithholdingReason, E.LogCreatedWhen, E.LogCreatedBy, 
      E.LogUpdatedWhen, E.LogUpdatedBy, E.RowGUID
FROM         CollectionEvent AS E INNER JOIN
      CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID INNER JOIN
      CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID
WHERE     (P.ProjectID = @ProjectID)
AND E.LocalityDescription LIKE '%' + @Locality + '%'


RETURN 
END  
GO

grant select on [DiversityMobile_EventsForProject] TO [User]
GO


--#####################################################################################################################
--######   [NextFreeAccNr]   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[NextFreeAccNr] (@AccessionNumber nvarchar(50))  
/*
returns next free accession number
assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453
with a leading string and a numeric end
MW 31.10.2012
TEST:
select max(AccessionNumber) from CollectionSpecimen
select dbo.[NextFreeAccNr] ('0033933')
select dbo.[NextFreeAccNr] ('00041009')
select dbo.[NextFreeAccNr] ('M-00041009')
select dbo.[NextFreeAccNr] ('M-0014474')
select dbo.[NextFreeAccNr] ('ZSM_DIP-000')
select dbo.[NextFreeAccNr] ('1907/9')
select dbo.[NextFreeAccNr] ('ZSM-MA-9')
select dbo.[NextFreeAccNr] ('M-0013622')
select dbo.[NextFreeAccNr] ('M-0014900')
select dbo.[NextFreeAccNr] ('MB-FA-000001') 
select dbo.[NextFreeAccNr] ('MB-FA-000101') 
*/
RETURNS nvarchar (50)
AS
BEGIN 
declare @NextAcc nvarchar(50)
set @NextAcc = ''
declare @Start int
declare @Position tinyint
declare @Prefix nvarchar(50)
set @Position = len(@AccessionNumber) 
if (isnumeric(@AccessionNumber) = 1)
begin
	set @Prefix = substring(@AccessionNumber, 1, len(@AccessionNumber) - len(cast(cast(@AccessionNumber as int) as varchar)))
	set @Start = cast(@AccessionNumber as int)
end
else
begin
while (isnumeric(rtrim(substring(@AccessionNumber, @Position, len(@AccessionNumber)))) = 1)
begin
	set @Start = CAST(substring(@AccessionNumber, @Position, len(@AccessionNumber)) as int)
	set @Prefix = substring(@AccessionNumber, 1, @Position)
	set @Position = @Position - 1
end
end
if (@Start < 0) 
begin 
	set @Start = @Start * -1;
end
declare @Space nvarchar(1)
set @Space = ''
if (SUBSTRING(@AccessionNumber, @Position + 1, 1) = ' ')
begin
	set @Space = '_'
end
if (LEN(@Prefix) = LEN(@AccessionNumber))
begin
	set @Prefix = SUBSTRING(@Prefix, 1, len(@Prefix) - 1)
end
declare @T Table (ID int identity(1, 1),
	NumericPart int NULL,
    AccessionNumber nvarchar(50) NULL)
INSERT INTO @T (AccessionNumber)
SELECT AccessionNumber 
FROM CollectionSpecimen  
WHERE AccessionNumber LIKE @Prefix + '%'
AND AccessionNumber >= @AccessionNumber
if (select COUNT(*) from @T) = 0
begin
INSERT INTO @T (AccessionNumber)
SELECT @AccessionNumber 
end
UPDATE @T SET NumericPart = ID + @Start;
UPDATE @T SET AccessionNumber = @Prefix 
+ case when (len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) > 0 
then replicate('0', len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) else '' end
+ CAST(NumericPart as varchar);
set @NextAcc = (SELECT MIN(T.AccessionNumber) from @T T left outer join  CollectionSpecimen S on S.AccessionNumber = T.AccessionNumber
	where S.AccessionNumber is null)
return (@NextAcc)
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.27'
END

GO



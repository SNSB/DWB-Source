--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.17'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO




--#####################################################################################################################
--######   [Collectionspecimenpart]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO



/****** Object:  UserDefinedFunction [dbo].[NextFreeAccNr]    Script Date: 02/22/2012 16:39:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER FUNCTION [dbo].[NextFreeAccNr] (@AccessionNumber nvarchar(50))  
/*
returns next free accession number
assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453
with a leading string and a numeric end
MW 23.02.2012
TEST:
select max(AccessionNumber) from CollectionSpecimen
select dbo.[NextFreeAccNr] ('M-00041009')
select dbo.[NextFreeAccNr] ('M-0014474')
select dbo.[NextFreeAccNr] ('ZSM_DIP-000')
select dbo.[NextFreeAccNr] ('1907/9')
select dbo.[NextFreeAccNr] ('ZSM-MA-9')
select dbo.[NextFreeAccNr] ('M-0013622')
select dbo.[NextFreeAccNr] ('M-0014900')
select dbo.[NextFreeAccNr] ('MB-FA-000001') --MB-FA-000200
select dbo.[NextFreeAccNr] ('MB-FA-000101') --MB-FA-000130
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

while (isnumeric(rtrim(substring(@AccessionNumber, @Position, len(@AccessionNumber)))) = 1)
begin
	set @Start = CAST(substring(@AccessionNumber, @Position, len(@AccessionNumber)) as int)
	set @Prefix = substring(@AccessionNumber, 1, @Position)
	set @Position = @Position - 1
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



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AnalysisResultForProject]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[AnalysisResultForProject]
GO

CREATE FUNCTION [dbo].[AnalysisResultForProject] (@ProjectID int)  
RETURNS @AnalysisResult TABLE (
	[AnalysisID] [int] NOT NULL,
	[AnalysisResult] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[Notes] [nvarchar](500) NULL,
	[RowGUID] [int] Identity NOT NULL)

/*
Returns the contents of the table AnalysisResult used in a project.
MW 15.07.2009
Test:
select * from [dbo].[AnalysisResultForProject] (1100)
*/

AS
BEGIN

INSERT INTO @AnalysisResult (AnalysisID, AnalysisResult, [Description], DisplayText, DisplayOrder, Notes)
SELECT     R.AnalysisID, R.AnalysisResult, R.Description, R.DisplayText, R.DisplayOrder, 
R.Notes
FROM         AnalysisResult R INNER JOIN
AnalysisProjectList(@ProjectID) P ON R.AnalysisID = P.AnalysisID

RETURN
END


GO

GRANT SELECT ON [dbo].[AnalysisResultForProject] TO [User]
GO



--#####################################################################################################################
--######   [Collectionspecimenpart]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO
IF (SELECT COUNT(*) FROM [CollUnitRelationType_Enum] WHERE Code = 'Found on') = 0
BEGIN
INSERT INTO [CollUnitRelationType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable])
     VALUES
           ('Found on'
           ,'Organism found on 2nd organism '
           ,'Found on'
           ,35
           ,1)
END
GO







--#####################################################################################################################
--######   [Collectionspecimenpart]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO

GRANT DELETE ON [dbo].[AnalysisTaxonomicGroup] TO [Administrator]
GO

GRANT INSERT ON [dbo].[AnalysisTaxonomicGroup] TO [Administrator]
GO





--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO





--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO











































--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO






--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_CONC
--use DiversityCollection_Monitoring
--use DiversityCollection_SMNK
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_Tutorial
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_ZFMK
GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.18'
END

GO


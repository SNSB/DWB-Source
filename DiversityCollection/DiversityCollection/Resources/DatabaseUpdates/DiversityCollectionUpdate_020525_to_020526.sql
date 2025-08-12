
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.25'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   AnalysisResultForProject   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_AnalysisProjectList]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_AnalysisProjectList]
GO

CREATE FUNCTION [dbo].[DiversityMobile_AnalysisProjectList] (@ProjectID int)   
RETURNS @AnalysisList TABLE ([AnalysisID] [int] Primary key , 	
[AnalysisParentID] [int] NULL , 	
[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL , 	
[MeasurementUnit] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL , 	
[Notes] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL , 	
[AnalysisURI] [varchar] (255) COLLATE Latin1_General_CI_AS NULL,
[OnlyHierarchy] [bit] NULL,
[RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)  
/* 
Returns a table that lists all the analysis items related to the given project. 
MW 08.08.2009 
TEST: 
Select * from DiversityMobile_AnalysisProjectList(3)  
Select * from DiversityMobile_AnalysisProjectList(372)  
*/ 
AS BEGIN  
--ALTER TABLE @AnalysisList ADD  CONSTRAINT [DF__Analysis__RowGUI__29A2D696]  DEFAULT (newsequentialid()) FOR [RowGUID]

INSERT INTO @AnalysisList            
([AnalysisID]            
,[AnalysisParentID]            
,[DisplayText]            
,[Description]            
,[MeasurementUnit]            
,[Notes]            
,[AnalysisURI]
,[OnlyHierarchy]
,[RowGUID]) 
SELECT Analysis.AnalysisID, Analysis.AnalysisParentID, Analysis.DisplayText, Analysis.Description, 
Analysis.MeasurementUnit, Analysis.Notes,  Analysis.AnalysisURI, Analysis.OnlyHierarchy, Analysis.RowGUID
FROM  ProjectAnalysis 
INNER JOIN Analysis ON ProjectAnalysis.AnalysisID = Analysis.AnalysisID 
WHERE ProjectAnalysis.ProjectID = @ProjectID  

DECLARE @TempItem TABLE (AnalysisID int primary key) 

INSERT INTO @TempItem ([AnalysisID]) 
SELECT Analysis.AnalysisID 
FROM  ProjectAnalysis 
INNER JOIN Analysis ON ProjectAnalysis.AnalysisID = Analysis.AnalysisID 
WHERE ProjectAnalysis.ProjectID = @ProjectID  
declare @ParentID int  
DECLARE HierarchyCursor  CURSOR for 	select AnalysisID from @TempItem 	
open HierarchyCursor 	
FETCH next from HierarchyCursor into @ParentID 	
WHILE @@FETCH_STATUS = 0 	
BEGIN 	
insert into @AnalysisList ( AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, 
Notes , AnalysisURI, OnlyHierarchy, RowGUID) 
select AnalysisID , AnalysisParentID, DisplayText , Description , MeasurementUnit, 
Notes , AnalysisURI, OnlyHierarchy, RowGUID
from dbo.AnalysisChildNodes (@ParentID) 
where AnalysisID not in (select AnalysisID from @AnalysisList) 	
FETCH NEXT FROM HierarchyCursor into @ParentID 	END 
CLOSE HierarchyCursor 
DEALLOCATE HierarchyCursor  
--DELETE FROM  @AnalysisList WHERE OnlyHierarchy = 1  
RETURN 
END  



GO

GRANT SELECT ON DiversityMobile_AnalysisProjectList TO [User]
GO

--#####################################################################################################################
--######   AnalysisResultForProject   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_AnalysisResultForProject]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_AnalysisResultForProject]
GO

CREATE FUNCTION [dbo].[DiversityMobile_AnalysisResultForProject] (@ProjectID int)  
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
select * from [dbo].[DiversityMobile_AnalysisResultForProject] (1100)
*/

AS
BEGIN

INSERT INTO @AnalysisResult (AnalysisID, AnalysisResult, [Description], DisplayText, DisplayOrder, Notes)
SELECT     AnalysisResult.AnalysisID, AnalysisResult.AnalysisResult, AnalysisResult.Description, AnalysisResult.DisplayText, AnalysisResult.DisplayOrder, 
AnalysisResult.Notes
FROM         AnalysisResult INNER JOIN
AnalysisProjectList(@ProjectID) P ON AnalysisResult.AnalysisID = P.AnalysisID

RETURN
END



GO

GRANT SELECT ON DiversityMobile_AnalysisResultForProject TO [User]
GO



--#####################################################################################################################
--######   AnalysisResultForProject   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_AnalysisTaxonomicGroupsForProject]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_AnalysisTaxonomicGroupsForProject]
GO

CREATE FUNCTION [dbo].[DiversityMobile_AnalysisTaxonomicGroupsForProject](@ProjectID int)  
RETURNS @AnalysisTaxonomicGroup TABLE ([AnalysisID] [int] NOT NULL,
	[TaxonomicGroup] [nvarchar](50) NOT NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)
/*
Returns the contents of the table AnalysisTaxonomicGroup used in a project including the whole hierarchy.
MW 15.07.2009
Test:
select * from [dbo].[AnalysisTaxonomicGroupForProject] (372)
*/
AS
BEGIN
	declare @Temp TABLE (AnalysisID int NOT NULL
	, TaxonomicGroup [nvarchar](50) NOT NULL
	, ID int Identity NOT NULL
	, [RowGUID] [uniqueidentifier] ROWGUIDCOL NULL)
	insert @Temp (AnalysisID, TaxonomicGroup)
	SELECT A.AnalysisID, A.TaxonomicGroup
	FROM AnalysisTaxonomicGroup A 
	INNER JOIN ProjectAnalysis P ON A.AnalysisID = P.AnalysisID 
	WHERE     (P.ProjectID = @ProjectID)

	declare @TempID TABLE (ID int NOT NULL)
	INSERT @TempID (ID) SELECT ID FROM @Temp
	DECLARE @ID INT
	DECLARE @AnalysisID int
	DECLARE @TaxonomicGroup nvarchar(50)
	WHILE (SELECT COUNT(*) FROM @TempID) > 0
	BEGIN
		SET @ID = (SELECT MIN(ID) FROM @TempID)
		SET @AnalysisID = (SELECT AnalysisID FROM @Temp WHERE ID = @ID)
		SET @TaxonomicGroup = (SELECT TaxonomicGroup FROM @Temp WHERE ID = @ID)
		INSERT INTO @Temp (AnalysisID, TaxonomicGroup, RowGUID)
		SELECT A.AnalysisID, @TaxonomicGroup, A.RowGUID
		FROM [AnalysisList] (@ProjectID, @TaxonomicGroup) L, Analysis A
		WHERE A.AnalysisID = L.AnalysisID AND A.OnlyHierarchy = 0
		DELETE FROM @TempID WHERE ID = @ID
	END

	insert @AnalysisTaxonomicGroup (AnalysisID, TaxonomicGroup, RowGUID)
	SELECT DISTINCT AnalysisID, TaxonomicGroup, RowGUID
	FROM @Temp
	WHERE NOT RowGUID IS NULL
	
	RETURN
END



GO

GRANT SELECT ON DiversityMobile_AnalysisTaxonomicGroupsForProject TO [User]
GO



--#####################################################################################################################
--######   [DiversityMobile_EventImageTypes]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_EventImageTypes]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_EventImageTypes]
GO

CREATE  FUNCTION [dbo].[DiversityMobile_EventImageTypes] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_EventImageTypes] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollEventImageType_Enum]
	t where t.DisplayEnable = 1
	and t.Code not in ('image')
   RETURN
END

GO



GRANT SELECT ON DiversityMobile_EventImageTypes TO [User]
GO


--#####################################################################################################################
--######   [DiversityMobile_IdentificationCategories]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_IdentificationCategories]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_IdentificationCategories]
GO

CREATE  FUNCTION [dbo].[DiversityMobile_IdentificationCategories] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_IdentificationCategories] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollIdentificationCategory_Enum]
	t where t.DisplayEnable = 1
	and t.Code in ('determination')
   RETURN
END

GO


GRANT SELECT ON DiversityMobile_IdentificationCategories TO [User]
GO


--#####################################################################################################################
--######   [DiversityMobile_IdentificationQualifiers]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_IdentificationQualifiers]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_IdentificationQualifiers]
GO

CREATE  FUNCTION [dbo].[DiversityMobile_IdentificationQualifiers] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_IdentificationQualifiers] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollIdentificationQualifier_Enum]
	t where t.DisplayEnable = 1
	and t.Code not in ('sp. nov.')
   RETURN
END

GO



GRANT SELECT ON DiversityMobile_IdentificationQualifiers TO [User]
GO






--#####################################################################################################################
--######     Column  Settings in  UserProxy      ######################################################################################
--#####################################################################################################################

if ( select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'UserProxy'
and C.COLUMN_NAME = 'Settings') = 0
begin
ALTER TABLE UserProxy ADD Settings XML NULL
end

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The settings for the user' , @level0type=N'SCHEMA',@level0name=N'dbo', 
@level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Settings'
GO







--#####################################################################################################################
--######   [DiversityMobile_MaterialCategories]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_MaterialCategories]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_MaterialCategories]
GO


CREATE  FUNCTION [dbo].[DiversityMobile_MaterialCategories] ()  
RETURNS @MaterialCategories TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the taxonomic groups as defined in the settings related to the given project as stored in DiversityProjects.
MW 20.01.2010

Test:
select * from [dbo].[DiversityMobile_MaterialCategories] () 
 */
AS
BEGIN
-- filling the table
if (select U.Settings.query('/Settings/DiversityMobile/MaterialCategories') from UserProxy U where U.LoginName = USER_NAME()) is null
or (select cast(U.Settings.query('data(/Settings/DiversityMobile/MaterialCategories)') as nvarchar(50)) from UserProxy U where U.LoginName = USER_NAME()) = ''
begin
	INSERT INTO @MaterialCategories ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollMaterialCategory_Enum]
	t where t.DisplayEnable = 1
end 
else
begin
	declare @GroupVisibile varchar(500);
	set @GroupVisibile = (
	select cast(U.Settings.query('data(/Settings/DiversityMobile/MaterialCategories)') as nvarchar(50))
		from UserProxy U
		where not U.Settings.query('/Settings/DiversityMobile/MaterialCategories') is null
		and U.LoginName = USER_NAME()
	)
	declare @Code nvarchar(50)
	declare @DisplayText nvarchar(50)
	while (len(@GroupVisibile) > 0)
	begin
		if (select CHARINDEX('|',@GroupVisibile)) > 0
		begin
			set @Code = (select SUBSTRING(@GroupVisibile, 1, CHARINDEX('|',@GroupVisibile) - 1 ))
		end
		else
		begin
			set @Code = @GroupVisibile
		end
		set @DisplayText = (select DisplayText from CollMaterialCategory_Enum e where e.Code = @Code)
		INSERT INTO @MaterialCategories (Code, DisplayText) values (@Code, @DisplayText)
		if (select CHARINDEX('|',@GroupVisibile)) > 0
		begin
			set @GroupVisibile = rtrim((select SUBSTRING(@GroupVisibile, CHARINDEX('|',@GroupVisibile)+1, 500)))
		end
		else 
		begin
			set @GroupVisibile = ''
		end
	end
end
   RETURN
END

GO




GRANT SELECT ON DiversityMobile_MaterialCategories TO [User]
GO




--#####################################################################################################################
--######   [DiversityMobile_ProjectList]   ######################################################################################
--#####################################################################################################################


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_ProjectList]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_ProjectList]
GO

CREATE FUNCTION [dbo].[DiversityMobile_ProjectList] ()   
RETURNS @ProjectList TABLE ([ProjectID] [int] Primary key , 	
[DisplayText] [nvarchar] (50))  
/* 
Returns a table that lists all the project for a user
MW 08.08.2009 
TEST: 
Select * from [DiversityMobile_ProjectList]()
*/ 
AS BEGIN  

INSERT INTO @ProjectList            
([ProjectID]            
,[DisplayText]) 
SELECT  U.ProjectID ,   P.Project
FROM         ProjectUser AS U INNER JOIN
                      ProjectProxy AS P ON U.ProjectID = P.ProjectID
WHERE     (U.LoginName = USER_NAME())
ORDER BY P.Project

RETURN 
END  

GO



GRANT SELECT ON DiversityMobile_ProjectList TO [User]
GO


--#####################################################################################################################
--######   [DiversityMobile_SpecimenImageTypes]   #####################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_SpecimenImageTypes]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_SpecimenImageTypes]
GO


CREATE  FUNCTION [dbo].[DiversityMobile_SpecimenImageTypes] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_SpecimenImageTypes] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollSpecimenImageType_Enum]
	t where t.DisplayEnable = 1
	and t.Code not in ('drawing', 'image', 'label', 'SEM image','photography', 'TEM image')
   RETURN
END

GO




GRANT SELECT ON DiversityMobile_SpecimenImageTypes TO [User]
GO

--#####################################################################################################################
--######   [DiversityMobile_TaxonomicGroups]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_TaxonomicGroups]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_TaxonomicGroups]
GO



CREATE  FUNCTION [dbo].[DiversityMobile_TaxonomicGroups] ()  
RETURNS @TaxonomicGroups TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the taxonomic groups as defined in the settings related to the given project as stored in DiversityProjects.
MW 20.01.2010

Test:
select * from [dbo].[DiversityMobile_TaxonomicGroups] () 
 */
AS
BEGIN
-- filling the table
if (select U.Settings.query('/Settings/DiversityMobile/TaxonomicGroups') from UserProxy U where U.LoginName = USER_NAME()) is null
or (select cast(U.Settings.query('data(/Settings/DiversityMobile/TaxonomicGroups)') as nvarchar(50)) from UserProxy U where U.LoginName = USER_NAME()) = ''
begin
	INSERT INTO @TaxonomicGroups ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollTaxonomicGroup_Enum]
	t where t.DisplayEnable = 1
end 
else
begin
	declare @TaxGroupVisibile varchar(500);
	set @TaxGroupVisibile = (
	select cast(U.Settings.query('data(/Settings/DiversityMobile/TaxonomicGroups)') as nvarchar(50))
		from UserProxy U
		where not U.Settings.query('/Settings/DiversityMobile/TaxonomicGroups') is null
		and U.LoginName = USER_NAME()
	)
	declare @Code nvarchar(50)
	declare @DisplayText nvarchar(50)
	while (len(@TaxGroupVisibile) > 0)
	begin
		if (select CHARINDEX('|',@TaxGroupVisibile)) > 0
		begin
			set @Code = (select SUBSTRING(@TaxGroupVisibile, 1, CHARINDEX('|',@TaxGroupVisibile) - 1 ))
		end
		else
		begin
			set @Code = @TaxGroupVisibile
		end
		set @DisplayText = (select DisplayText from CollTaxonomicGroup_Enum e where e.Code = @Code)
		INSERT INTO @TaxonomicGroups (Code, DisplayText) values (@Code, @DisplayText)
		if (select CHARINDEX('|',@TaxGroupVisibile)) > 0
		begin
			set @TaxGroupVisibile = rtrim((select SUBSTRING(@TaxGroupVisibile, CHARINDEX('|',@TaxGroupVisibile)+1, 500)))
		end
		else 
		begin
			set @TaxGroupVisibile = ''
		end
	end
end
   RETURN
END

GO



GRANT SELECT ON DiversityMobile_TaxonomicGroups TO [User]
GO


--#####################################################################################################################
--######   [DiversityMobile_UnitRelationTypes]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_UnitRelationTypes]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_UnitRelationTypes]
GO

CREATE  FUNCTION [dbo].[DiversityMobile_UnitRelationTypes] ()  
RETURNS @Enum TABLE (
  "Code" character varying(50) NOT NULL,
  "DisplayText" character varying(50)
)

/*
Returns a table that lists all the entries from the table relevant for a mobile application.
MW 22.08.2011

Test:
select * from [dbo].[DiversityMobile_UnitRelationTypes] () 
 */
AS
BEGIN
-- filling the table
	INSERT INTO @Enum ([Code], [DisplayText])
	SELECT [Code]
		  ,[DisplayText]
	FROM [CollUnitRelationType_Enum]
	t where t.DisplayEnable = 1
	and t.Code not in ('')
   RETURN
END

GO



GRANT SELECT ON DiversityMobile_UnitRelationTypes TO [User]
GO


--#####################################################################################################################
--######   [DiversityMobile_UserInfo]   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityMobile_UserInfo]') 
AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[DiversityMobile_UserInfo]
GO


CREATE FUNCTION [dbo].[DiversityMobile_UserInfo] ()   
RETURNS @UserInfo TABLE ([LoginName] [nvarchar] (50) Primary key , 	
[UserName] [nvarchar] (50),
[AgentUri] [varchar] (255))  
/* 
Returns a table that lists all the project for a user
MW 08.08.2009 
TEST: 
Select * from [DiversityMobile_UserInfo]()
*/ 
AS BEGIN  

INSERT INTO @UserInfo            
([LoginName]            
,[UserName]
,[AgentUri]) 
SELECT  U.LoginName,
case when U.CombinedNameCache like '%, % (%' then SUBSTRING(U.CombinedNameCache, 1, charindex(' ', U.CombinedNameCache ))
+  SUBSTRING(U.CombinedNameCache, charindex(' ', U.CombinedNameCache )+1, 1) + '.' else 
case when U.CombinedNameCache like '%, %' then SUBSTRING(U.CombinedNameCache, 1, charindex(' ', U.CombinedNameCache ))
+  SUBSTRING(U.CombinedNameCache, charindex(' ', U.CombinedNameCache )+1, 1) + '.' else U.CombinedNameCache end
end, 
u.AgentURI
FROM UserProxy AS U 
WHERE (U.LoginName = USER_NAME())

RETURN 
END  





GO



GRANT SELECT ON DiversityMobile_UserInfo TO [User]
GO



--#####################################################################################################################
--######   Collection -1   ######################################################################################
--#####################################################################################################################

declare @i int;
set @i = (select COUNT(*) from [Collection] where [CollectionID] = -1);
if @i = 0
begin

set identity_insert [Collection] on;

INSERT INTO [dbo].[Collection]
           ([CollectionID]
           ,[CollectionName]
           ,[Description])
     VALUES
           (-1
           ,'-'
           ,'No collection')
           
set identity_insert [Collection] on;

end
go



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
select dbo.[NextFreeAccNr] ('00041009')
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

if (isnumeric(@AccessionNumber) = 1)
begin
SET @NextAcc = @AccessionNumber + 1
if (LEN(@NextAcc) < LEN(@AccessionNumber))
begin
	while (LEN(@NextAcc) < LEN(@AccessionNumber))
	begin
	set @NextAcc = '0' + @NextAcc
	end
end
return @NextAcc
end

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

--#####################################################################################################################
--######   DF_Identification_ResponsibleName   ######################################################################################
--#####################################################################################################################


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Identification_ResponsibleName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Identification] DROP CONSTRAINT [DF_Identification_ResponsibleName]
END

GO

--#####################################################################################################################
--######   DF_CollectionEventLocalisation_ResponsibleName   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CollectionEventLocalisation_ResponsibleName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CollectionEventLocalisation] DROP CONSTRAINT [DF_CollectionEventLocalisation_ResponsibleName]
END

GO


--#####################################################################################################################
--######   DF_CollectionSpecimenProcessing_ResponsibleName   ######################################################################################
--#####################################################################################################################

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CollectionSpecimenProcessing_ResponsibleName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CollectionSpecimenProcessing] DROP CONSTRAINT [DF_CollectionSpecimenProcessing_ResponsibleName]
END

GO


--#####################################################################################################################
--######   Hexapoda   ######################################################################################
--#####################################################################################################################

UPDATE R SET [Description] = 'Hexapoda: Insekten, Springschwänze etc.'
  FROM [DiversityCollection].[dbo].[EntityRepresentation]
  r where r.Entity = 'CollTaxonomicGroup_Enum.Code.insect'
  and r.LanguageCode = 'de-DE'

  GO

--#####################################################################################################################
--######   UnitDescription   ######################################################################################
--#####################################################################################################################

  
EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Description of the unit, esp. if not an organism but parts or remnants of it were present or observed, e.g. a nest of an insect or a song of a bird' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentificationUnit', @level2type=N'COLUMN',@level2name=N'UnitDescription'
GO

--#####################################################################################################################
--######   ParsingMethodName for Depth, Top50 and Stratigraphy  ######################################################################################
--#####################################################################################################################


update l set [ParsingMethodName] = 'Depth'
FROM [DiversityCollection_Test].[dbo].[LocalisationSystem]
l where l.LocalisationSystemID = 14

update l set [ParsingMethodName] = 'Coordinates'
FROM [DiversityCollection_Test].[dbo].[LocalisationSystem]
l where l.LocalisationSystemID = 1

update p set ParsingMethodName = 'Stratigraphy'
FROM [DiversityCollection_Test].[dbo].[Property]
p where p.PropertyID in (20,30)

  GO

--#####################################################################################################################
--######   setting the VersionClient   ######################################################################################
--#####################################################################################################################

  ALTER FUNCTION [dbo].[VersionClient] () RETURNS nvarchar(11) AS BEGIN RETURN '03.00.05.00' END
  GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.26'
END

GO



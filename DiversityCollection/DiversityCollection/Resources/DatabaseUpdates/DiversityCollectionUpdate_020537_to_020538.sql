
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.37'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   Replication   ######################################################################################
--#####################################################################################################################

EXEC sp_addrolemember 'Replicator', 'Administrator'
GO

GRANT ALTER ON dbo.CollectionExternalDatasource TO Replicator
GO

--#####################################################################################################################
--######   [IX_AccessionNumber]   ######################################################################################
--#####################################################################################################################

CREATE NONCLUSTERED INDEX [IX_AccessionNumber] ON [dbo].[CollectionSpecimenPart] 
(
	[AccessionNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



--#####################################################################################################################
--######   [NextFreeAccNumber]  ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[NextFreeAccNumber] (@AccessionNumber nvarchar(50), @IncludeSpecimen bit, @IncludePart bit)  
/*
returns next free accession number for parts or specimen
optionally including either parts or specimen
assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453
with a leading string and a numeric end
MW 05.09.2013
TEST:
select dbo.[NextFreeAccNumber] ('0033933', 1, 1)
select dbo.[NextFreeAccNumber] ('0033933', 0, 1)
select dbo.[NextFreeAccNumber] ('00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 0, 1)
select dbo.[NextFreeAccNumber] ('ZSM_DIP-000', 1, 1)
select dbo.[NextFreeAccNumber] ('1907/9', 1, 1)
select dbo.[NextFreeAccNumber] ('ZSM-MA-9', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0013622', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014900', 1, 1)
select dbo.[NextFreeAccNumber] ('MB-FA-000001', 1, 1) 
select dbo.[NextFreeAccNumber] ('MB-FA-000101', 1, 1) 
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

-- getting the numerc part and the prefix of the accession number
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

-- getting the current accession numbers from the database
declare @T Table (ID int identity(1, 1),
	NumericPart int NULL,
    AccessionNumber nvarchar(50) NULL)
if (@IncludeSpecimen = 1)
begin    
INSERT INTO @T (AccessionNumber)
SELECT AccessionNumber 
FROM CollectionSpecimen  
WHERE AccessionNumber LIKE @Prefix + '%'
AND AccessionNumber >= @AccessionNumber
end
if (@IncludePart = 1)
begin    
INSERT INTO @T (AccessionNumber)
SELECT AccessionNumber 
FROM CollectionSpecimenPart  
WHERE AccessionNumber LIKE @Prefix + '%'
AND AccessionNumber >= @AccessionNumber
end

-- adding the accession number if nothing was found
if (select COUNT(*) from @T) = 0
begin
INSERT INTO @T (AccessionNumber)
SELECT @AccessionNumber 
end

-- formatting the found accession numbers
UPDATE @T SET NumericPart = ID + @Start;
UPDATE @T SET AccessionNumber = @Prefix 
+ case when (len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) > 0 
then replicate('0', len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) else '' end
+ CAST(NumericPart as varchar);

-- removing the accession numbers that match those in the database
if (@IncludeSpecimen = 1)
begin
	Delete T from @T T inner join  CollectionSpecimen S on S.AccessionNumber = T.AccessionNumber
	where not S.AccessionNumber is null
end	
if (@IncludePart = 1)
begin
	Delete T from @T T inner join  CollectionSpecimenPart S on S.AccessionNumber = T.AccessionNumber
	where not S.AccessionNumber is null
end	

-- getting the first of the remaining numbers
set @NextAcc = (SELECT MIN(T.AccessionNumber) from @T T)
return (@NextAcc)

END

GO

GRANT EXEC ON [dbo].[NextFreeAccNumber] TO Editor
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.38'
END

GO



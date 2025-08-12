
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.18'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  Missing GRANTS ##############################################################################################
--#####################################################################################################################


EXEC sp_addrolemember N'db_owner', N'CacheAdmin'
GO

--#####################################################################################################################
--######   procProjectName  ###########################################################################################
--#####################################################################################################################

create PROCEDURE [dbo].[procProjectName] ( @Schema varchar(50) OUTPUT, @CollectionSpecimenID int)
--RETURNS varchar (500)
AS BEGIN
/*
declare @Project nvarchar(50)
exec dbo.procProjectName @Project output, 8603
select @Project
*/
-- getting the schema 
declare @TempSchema varchar(50)
declare @TempTable table(SchemaName varchar(50))
declare @SchemaOut varchar(50)
--declare @Schema varchar(50)
DECLARE @ParmDefinition nvarchar(500);  
SET @ParmDefinition = N'@ID int, @SchemaOut varchar(50) OUTPUT';  
set @SchemaOut = ''
insert into @TempTable
select S.SCHEMA_NAME from INFORMATION_SCHEMA.SCHEMATA S
where S.SCHEMA_NAME like 'Project_%'
declare @SQL nvarchar(4000)
while (Select count(*) from @TempTable) > 0 AND @SchemaOut = ''
begin
	declare @i int
	set @TempSchema = (select min(SchemaName) from @TempTable)
	set @SQL = 'SELECT @SchemaOut = '''  + @TempSchema + ''' FROM ' + @TempSchema + '.CacheCollectionSpecimen WHERE CollectionSpecimenID = @ID'
	exec sp_executesql @SQL, @ParmDefinition, @ID = @CollectionSpecimenID, @SchemaOut=@Schema OUTPUT;  
	delete T from @TempTable T where T.SchemaName = @TempSchema
end
set @Schema = substring(@Schema, len('Project_') + 1, 500)
return --@Schema
end
GO

--#####################################################################################################################
--######   procUnitIDforABCD  #########################################################################################
--#####################################################################################################################

create PROCEDURE [dbo].[procUnitIDforABCD] ( @UnitID varchar(50) OUTPUT, @CollectionSpecimenID int, @IdentificationUnitID int, @SpecimenPartID int)
as begin
/*
declare @UnitID nvarchar(500)
exec dbo.UnitIDforABCD @UnitID output, 8603, 13236, 8429--null
select @UnitID
*/
-- getting the schema
declare @Project nvarchar(50)
exec dbo.procProjectName @Project output, 8603
declare @Schema varchar(50)
set @Schema = (select 'Project_' + @Project)
DECLARE @ParmDefinition nvarchar(500);  
declare @SQL nvarchar(4000)

-- Getting the UnitID
declare @UnitIDOUT nvarchar(500)
IF (@SpecimenPartID IS NULL)
begin
	SET @ParmDefinition = N'@UID int, @UnitIDOUT varchar(500) OUTPUT';  
	set @SQL = 'SELECT @UnitIDOUT = CASE WHEN S.AccessionNumber <> '''' THEN S.AccessionNumber ELSE cast(S.CollectionSpecimenID as varchar) END  + ''/'' + cast(IdentificationUnitID as varchar)
		FROM  ' + @Schema + '.CacheCollectionSpecimen S, ' + @Schema + '.CacheIdentificationUnit U 
		WHERE U.IdentificationUnitID = @UID AND S.CollectionSpecimenID = U.CollectionSpecimenID'
	exec sp_executesql @SQL, @ParmDefinition, @UID = @IdentificationUnitID, @UnitIDOut=@UnitID OUTPUT;  
END
ELSE
begin
	SET @ParmDefinition = N'@UID int, @PID int, @UnitIDOUT varchar(500) OUTPUT';  
	set @SQL = 'SELECT @UnitIDOUT = CASE WHEN S.AccessionNumber <> '''' THEN S.AccessionNumber ELSE cast(S.CollectionSpecimenID as varchar) END  
		+ ''/'' + cast(IdentificationUnitID as varchar)
		+ ''/'' + CASE WHEN S.AccessionNumber <> '''' THEN cast(P.SpecimenPartID as varchar) ELSE
		CASE WHEN P.AccessionNumber <> '''' THEN P.AccessionNumber ELSE cast(P.SpecimenPartID as varchar) END END
		FROM  ' + @Schema + '.CacheCollectionSpecimen S, ' + @Schema + '.CacheIdentificationUnit U, ' + @Schema + '.CacheCollectionSpecimenPart P
		WHERE U.IdentificationUnitID = @UID AND P.SpecimenPartID = @PID 
		AND S.CollectionSpecimenID = U.CollectionSpecimenID AND S.CollectionSpecimenID = P.CollectionSpecimenID'
	exec sp_executesql @SQL, @ParmDefinition, @UID = @IdentificationUnitID, @PID = @SpecimenPartID, @UnitIDOut=@UnitID OUTPUT;  
end

return
end
go

GRANT EXEC ON [dbo].[procUnitIDforABCD] TO [CacheUser]
GO


--#####################################################################################################################
--######   Agent       ################################################################################################
--#####################################################################################################################



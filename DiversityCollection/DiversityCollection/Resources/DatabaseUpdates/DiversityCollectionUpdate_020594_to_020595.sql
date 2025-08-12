declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.94'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   CollectionSpecimenID_ReadOnly  #############################################################################
--#####################################################################################################################

declare @SQL nvarchar(max)
set @SQL = (select 'CREATE VIEW [dbo].[CollectionSpecimenID_ReadOnly]
AS
SELECT        P.CollectionSpecimenID
FROM            dbo.CollectionProject AS P INNER JOIN
dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
WHERE        (U.LoginName = USER_NAME()) AND (U.ReadOnly = 1) 
GROUP BY P.CollectionSpecimenID')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch

GO

GRANT SELECT ON CollectionSpecimenID_ReadOnly TO [User]
GO




--#####################################################################################################################
--######   Descriptions for Recordbasis  ##############################################################################
--#####################################################################################################################

select * from CollMaterialCategory_Enum

--update CollMaterialCategory_Enum set Description = 'An indication of what a data record describes using a controlled vocabulary' where Code = 'RecordBasis (general)'
--GO
update CollMaterialCategory_Enum set Description = 'A record describing a preserved specimen (not living)' where Code = 'preserved specimen'
GO
update CollMaterialCategory_Enum set Description = 'A record describing a preserved specimen that is a fossil' where Code = 'fossil specimen'
GO
update CollMaterialCategory_Enum set Description = 'A record describing a specimen which is alive (not preserved)' where Code = 'living specimen'
GO
--update CollMaterialCategory_Enum set Description = 'A record describing a preserved specimen which is a mineral' where Code = 'MineralSpecimen'
--GO
--update CollMaterialCategory_Enum set Description = 'A record describing a geo-scientific object which is no pure mineral, rock or fossil specimen (but a mixture), or a geo-scientific specimen with unknown traits	' where Code = 'EarthScienceSpecimen'
--GO
update CollMaterialCategory_Enum set Description = 'A record describing an output of a human observation process' where Code = 'human observation'
GO
update CollMaterialCategory_Enum set Description = 'A record describing an output of a machine observation process' where Code = 'machine observation'
GO
--update CollMaterialCategory_Enum set Description = 'A record describing an output of an observation process with indication of the absence of an observation' where Code = 'AbsenceObservation'
--GO
--update CollMaterialCategory_Enum set Description = 'A record describing an output of a literature research' where Code = 'Literature'
--GO
update CollMaterialCategory_Enum set Description = 'A record describing a static visual representation (digital or physical)' where Code = 'drawing or photograph'
GO
--update CollMaterialCategory_Enum set Description = 'A record describing the physical result of a sampling (or subsampling) event (sample either preserved or destructively processed)' where Code = 'MaterialSample'
--GO
--update CollMaterialCategory_Enum set Description = 'A record describing a multimedia representation (digital or physical)' where Code = 'MultimediaObject'
--GO
--update CollMaterialCategory_Enum set Description = 'A record where the indication (basis) of the record is unknown' where Code = 'Unknown'
--GO
update CollMaterialCategory_Enum set Description = 'A record describing a specimen which is not indicated as preserved, living, fossil or mineral' where Code = 'other specimen'
GO

--#####################################################################################################################
--######  StableIdentifier    #########################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[StableIdentifier] (@ProjectID int, @CollectionSpecimenID int, @IdentificationUnitID int, @SpecimenPartID int)
RETURNS varchar (500)
/*Returns a stable identfier for a dataset.*/
AS
BEGIN
declare @StableIdentifier varchar(500)
declare @StableIdentifierTypeID int
declare @DatabaseName varchar(128)
set @DatabaseName = (SELECT MIN(DOMAIN_CATALOG) FROM INFORMATION_SCHEMA.DOMAINS)
set @StableIdentifierTypeID = (SELECT StableIdentifierTypeID FROM ProjectProxy WHERE ProjectID = @ProjectID)
IF (@SpecimenPartID IS NULL)
	if (@IdentificationUnitID IS NULL)
	BEGIN
	set @StableIdentifier = (SELECT PP.StableIdentifierBase + '/' + @DatabaseName + '/' + cast(P.CollectionSpecimenID as varchar)
	FROM ProjectProxy AS PP INNER JOIN
		CollectionProject AS P ON PP.ProjectID = P.ProjectID 
		AND P.CollectionSpecimenID = @CollectionSpecimenID 
		and P.ProjectID = @ProjectID
	) 
	END
	else
	BEGIN
		set @StableIdentifier = (SELECT PP.StableIdentifierBase + '/' + @DatabaseName + '/' + cast(U.CollectionSpecimenID as varchar) + '/' + cast(U.IdentificationUnitID as varchar)
		FROM ProjectProxy AS PP INNER JOIN
			CollectionProject AS P ON PP.ProjectID = P.ProjectID INNER JOIN
			IdentificationUnit AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID
			AND U.CollectionSpecimenID = @CollectionSpecimenID 
			AND U.IdentificationUnitID = @IdentificationUnitID 
			and P.ProjectID = @ProjectID
		) 
	END
ELSE
	if (@IdentificationUnitID IS NULL)
	BEGIN
	set @StableIdentifier = (SELECT PP.StableIdentifierBase + '/' + @DatabaseName + '/' + cast(P.CollectionSpecimenID as varchar)
	FROM ProjectProxy AS PP INNER JOIN
		CollectionProject AS P ON PP.ProjectID = P.ProjectID 
		AND P.CollectionSpecimenID = @CollectionSpecimenID 
		and P.ProjectID = @ProjectID
	) 
	END
	else
	BEGIN
		set @StableIdentifier = (SELECT PP.StableIdentifierBase + '/' + @DatabaseName + '/' + cast(UP.CollectionSpecimenID as varchar) + '/' + cast(UP.IdentificationUnitID as varchar) + '/' +  cast(UP.SpecimenPartID as varchar)
		FROM ProjectProxy AS PP INNER JOIN
			CollectionProject AS P ON PP.ProjectID = P.ProjectID INNER JOIN
			IdentificationUnit AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID INNER JOIN
			IdentificationUnitInPart AS UP ON U.CollectionSpecimenID = UP.CollectionSpecimenID AND U.IdentificationUnitID = UP.IdentificationUnitID
			AND U.CollectionSpecimenID = @CollectionSpecimenID 
			AND U.IdentificationUnitID = @IdentificationUnitID 
			and UP.SpecimenPartID = @SpecimenPartID
			and P.ProjectID = @ProjectID
		) 
	END
return @StableIdentifier
END

GO


--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.06' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.95'
END

GO


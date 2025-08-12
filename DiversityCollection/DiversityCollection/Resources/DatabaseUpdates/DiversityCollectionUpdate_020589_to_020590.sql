declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.89'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Stable Identifier in ProjectProxy  #########################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectProxy] ADD [StableIdentifierBase] varchar(500) NULL
GO

ALTER TABLE [dbo].[ProjectProxy] ADD [StableIdentifierTypeID] int NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The initial string of the stable identifier for data of this project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'StableIdentifierBase'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the stable identifier for data of this project - evaluated in function StableIdentifier for the creation of stable identifiers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'StableIdentifierTypeID'
GO


--#####################################################################################################################
--######   Function StableIdentifier  #################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[StableIdentifier] (@ProjectID int, @CollectionSpecimenID int, @IdentificationUnitID int, @SpecimenPartID int)
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

GRANT EXEC ON [dbo].[StableIdentifier] TO [User]
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.90'
END

GO


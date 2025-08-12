declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.28'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######  ProjectProxy: new column IsLocked  ##########################################################################
--#####################################################################################################################

ALTER TABLE [dbo].[ProjectProxy] ADD [IsLocked] [bit] NULL;
GO

ALTER TABLE [dbo].[ProjectProxy] ADD  CONSTRAINT [DF_ProjectProxy_IsLocked]  DEFAULT ((0)) FOR [IsLocked]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the data within the project should not be changeed and the access for all users is restricted to read only' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'IsLocked'
GO

--#####################################################################################################################
--######  trgUpdProjectProxy - include new column IsLocked and set dependent values in table ProjectUser ##############
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectProxy] ON [dbo].[ProjectProxy] 
FOR UPDATE AS
Update U
set U.[ReadOnly] = 1
FROM ProjectUser U, inserted D
where 1 = 1 
AND D.ProjectID = U.ProjectID
AND D.IsLocked = 1
GO

ALTER TABLE [dbo].[ProjectProxy] ENABLE TRIGGER [trgUpdProjectProxy]
GO


--#####################################################################################################################
--######  trgInsProjectUser - include new column IsLocked and set ReadOnly values in table ProjectUser ################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgInsProjectUser] ON [dbo].[ProjectUser] 
FOR INSERT AS
Update U
set U.[ReadOnly] = 1
FROM ProjectUser U, inserted I, ProjectProxy P
where 1 = 1 
AND I.ProjectID = U.ProjectID AND P.ProjectID = U.ProjectID
AND P.IsLocked = 1
GO

ALTER TABLE [dbo].[ProjectUser] ENABLE TRIGGER [trgInsProjectUser]
GO


--#####################################################################################################################
--######  trgUpdProjectUser - include new column IsLocked and set ReadOnly values in table ProjectUser ################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectUser] ON [dbo].[ProjectUser] 
FOR UPDATE AS
Update U
set U.[ReadOnly] = 1
FROM ProjectUser U, inserted D, ProjectProxy P
where 1 = 1 
AND D.ProjectID = U.ProjectID AND P.ProjectID = U.ProjectID
AND P.IsLocked = 1
GO

ALTER TABLE [dbo].[ProjectUser] ENABLE TRIGGER [trgUpdProjectUser]
GO

--#####################################################################################################################
--######   CollectionSpecimenID_Locked ################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionSpecimenID_Locked]
AS
SELECT        C.CollectionSpecimenID
FROM            dbo.CollectionProject AS C INNER JOIN
dbo.ProjectProxy AS P ON P.ProjectID = C.ProjectID AND (P.IsLocked = 1) 
GROUP BY C.CollectionSpecimenID
GO

GRANT SELECT ON [CollectionSpecimenID_Locked] TO [User]
GO


--#####################################################################################################################
--######   CollectionSpecimenID_AvailableReadOnly - include CollectionSpecimenID_Locked  ##############################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionSpecimenID_AvailableReadOnly]
AS
SELECT        P.CollectionSpecimenID
FROM            dbo.CollectionProject AS P INNER JOIN
                         dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
WHERE        (U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) AND 
(
	(U.ReadOnly = 1
	AND (P.CollectionSpecimenID NOT IN
    (SELECT        P.CollectionSpecimenID
    FROM            dbo.CollectionProject AS P INNER JOIN
                                dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
    WHERE        (U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) AND (U.ReadOnly = 0))))
	OR
	P.CollectionSpecimenID IN (SELECT L.CollectionSpecimenID FROM [CollectionSpecimenID_Locked] L)
)
GROUP BY P.CollectionSpecimenID

GO


--#####################################################################################################################
--######   CollectionSpecimenID_ReadOnly - include CollectionSpecimenID_Locked  #######################################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionSpecimenID_ReadOnly]
AS
SELECT        P.CollectionSpecimenID
FROM            dbo.CollectionProject AS P INNER JOIN
dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
WHERE        ((U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) AND (U.ReadOnly = 1) 
	OR 
	P.CollectionSpecimenID IN (SELECT L.CollectionSpecimenID FROM [CollectionSpecimenID_Locked] L)
	)
GROUP BY P.CollectionSpecimenID
GO





--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.29'
END

GO


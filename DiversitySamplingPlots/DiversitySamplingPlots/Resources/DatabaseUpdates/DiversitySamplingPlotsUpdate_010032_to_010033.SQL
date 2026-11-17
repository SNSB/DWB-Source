
DECLARE @CurrentVersion varchar(10)
DECLARE @ScriptVersion varchar(10)
SET @CurrentVersion = (SELECT [dbo].[Version]())
SET @CurrentVersion = (SELECT replace(@CurrentVersion, '/', '.'))
SET @ScriptVersion = '01.00.32'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
DECLARE @Message nvarchar (199)
SET @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Create function SamplingPlotAllLocalisations to add PropertyURI  ########################################
--#####################################################################################################################
/****** Object:  UserDefinedFunction [dbo].[SamplingPlotAllLocalisations]    Script Date: 01.07.2019 14:43:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* Delete first if already exists */
BEGIN TRY
    DROP FUNCTION [dbo].[SamplingPlotAllLocalisations];
END TRY
BEGIN CATCH
END CATCH
GO


CREATE FUNCTION [dbo].[SamplingPlotAllLocalisations] ()  
RETURNS @PlotLocalityPresent TABLE (
	[PlotID] int,
    [PartOfPlotID] [int],
    [LocalisationSystemID] int,
    [Location1] nvarchar(255),
	[Location2] nvarchar(255),
    [DisplayText] [nvarchar] (255) NULL,
	PRIMARY KEY CLUSTERED 
(
	[PlotID] ASC,
	[LocalisationSystemID] ASC
))
/*
Returns a table that lists all the properties of a plot included those inherited from a parent plot
MW 29.04.2019
select * from [dbo].[SamplingPlotAllLocalisations_2] ()  P order by PlotID desc
*/

AS
BEGIN
-- insert the entries as entered by the user
insert into @PlotLocalityPresent(
LocalisationSystemID, Location1, Location2, PlotID, PartOfPlotID, DisplayText)
select distinct 
P.LocalisationSystemID, P.Location1, P.Location2, S.PlotID, S.PartOfPlotID, L.DisplayText
from SamplingPlot S, SamplingPlotLocalisation P, LocalisationSystem L
where S.PlotID = P.PlotID and P.LocalisationSystemID = L.LocalisationSystemID

-- count the missing child entries
declare @i int

set @i = (select count(*) 
from SamplingPlot S, @PlotLocalityPresent P 
where S.PartOfPlotID = P.PlotID -- children plots in S
and NOT EXISTS (select * from @PlotLocalityPresent PPP where S.PlotID = PPP.PlotID and P.LocalisationSystemID = PPP.LocalisationSystemID)) -- where the property is missing

while @i > 0
begin

	insert into @PlotLocalityPresent(LocalisationSystemID, PlotID, Location1, Location2, PartOfPlotID, DisplayText)
	select distinct P.LocalisationSystemID, S.PlotID, P.Location1, P.Location2, S.PartOfPlotID, P.DisplayText 
	from SamplingPlot S, @PlotLocalityPresent P
	where P.PlotID = S.PartOfPlotID
	and NOT EXISTS (select * from @PlotLocalityPresent PPP where S.PlotID = PPP.PlotID and P.LocalisationSystemID = PPP.LocalisationSystemID)

	set @i = (select count(*) 
	from SamplingPlot S, @PlotLocalityPresent P 
	where S.PartOfPlotID = P.PlotID -- children plots in S
	and NOT EXISTS (select * from @PlotLocalityPresent PPP where S.PlotID = PPP.PlotID and P.LocalisationSystemID = PPP.LocalisationSystemID)) -- where the property is missing

end

RETURN
END

GO

/****** Object:  View [dbo].[ViewSamplingPlotAllLocalisations]    Script Date: 25.06.2019 10:39:50 ******/

/* Delete first if already exists */
BEGIN TRY
    DROP VIEW [dbo].[ViewSamplingPlotAllLocalisations];
END TRY
BEGIN CATCH
END CATCH
GO

CREATE VIEW [dbo].[ViewSamplingPlotAllLocalisations]
AS
SELECT        PlotID, PartOfPlotID, LocalisationSystemID, Location1, Location2, DisplayText
FROM            dbo.SamplingPlotAllLocalisations() AS SamplingPlotAllLocalisations

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "SamplingPlotAllLocalisations"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 239
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewSamplingPlotAllLocalisations'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewSamplingPlotAllLocalisations'
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN 
RETURN '03.00.11.00' 
END

GO


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.33'
END

GO

SELECT [dbo].[Version] ()  

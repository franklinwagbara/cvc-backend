CREATE VIEW dbo.vAppVessel
AS
SELECT DISTINCT 
                  dbo.Applications.Status, dbo.Facilities.Name AS VesselName, dbo.Applications.Reference, dbo.Applications.CreatedDate, dbo.Applications.SubmittedDate, dbo.Applications.ModifiedDate, dbo.Applications.IsDeleted, 
                  dbo.ApplicationTypes.Name AS AppTypeName, dbo.AspNetUsers.Email, dbo.Companies.Name AS CompanyName, dbo.Facilities.IsLicensed, dbo.VesselTypes.Name AS VesselTypes,
                      (SELECT COUNT(Id) AS Expr1
                       FROM      dbo.Tanks AS Tanks_2
                       WHERE   (FacilityId = dbo.Facilities.Id)) AS NoOfTanks,
                      (SELECT SUM(Capacity) AS Expr1
                       FROM      dbo.Tanks
                       WHERE   (FacilityId = dbo.Facilities.Id)) AS Capacity, dbo.Applications.ApplicationTypeId, dbo.Applications.Id
FROM     dbo.Applications INNER JOIN
                  dbo.ApplicationTypes ON dbo.Applications.ApplicationTypeId = dbo.ApplicationTypes.Id INNER JOIN
                  dbo.Facilities ON dbo.Applications.FacilityId = dbo.Facilities.Id INNER JOIN
                  dbo.AspNetUsers ON dbo.Applications.UserId = dbo.AspNetUsers.Id INNER JOIN
                  dbo.Companies ON dbo.AspNetUsers.CompanyId = dbo.Companies.Id INNER JOIN
                  dbo.VesselTypes ON dbo.Facilities.VesselTypeId = dbo.VesselTypes.Id INNER JOIN
                  dbo.Tanks AS Tanks_1 ON dbo.Facilities.Id = Tanks_1.FacilityId
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vAppVessel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'          DisplayFlags = 280
            TopColumn = 1
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
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vAppVessel';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[57] 4[23] 2[16] 3) )"
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
         Begin Table = "Applications"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 330
               Right = 265
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ApplicationTypes"
            Begin Extent = 
               Top = 6
               Left = 331
               Bottom = 125
               Right = 525
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Facilities"
            Begin Extent = 
               Top = 260
               Left = 308
               Bottom = 423
               Right = 502
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "AspNetUsers"
            Begin Extent = 
               Top = 23
               Left = 577
               Bottom = 186
               Right = 837
            End
            DisplayFlags = 280
            TopColumn = 12
         End
         Begin Table = "Companies"
            Begin Extent = 
               Top = 14
               Left = 914
               Bottom = 177
               Right = 1124
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "VesselTypes"
            Begin Extent = 
               Top = 127
               Left = 336
               Bottom = 246
               Right = 530
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tanks_1"
            Begin Extent = 
               Top = 228
               Left = 788
               Bottom = 391
               Right = 982
            End
  ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vAppVessel';




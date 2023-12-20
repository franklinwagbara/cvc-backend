CREATE VIEW dbo.vPayment
AS
SELECT dbo.Payments.Id, dbo.AspNetUsers.Email AS CompanyEmail, dbo.Companies.Name AS CompanyName, dbo.Applications.VesselName, dbo.Payments.RRR, dbo.Payments.Amount, dbo.Payments.Status AS PaymentStatus, dbo.Applications.Reference AS AppReference, 
             dbo.ExtraPayments.Reference AS ExtraPaymentReference, dbo.Payments.PaymentDate
FROM   dbo.Applications CROSS JOIN
             dbo.AspNetUsers CROSS JOIN
             dbo.Companies CROSS JOIN
             dbo.ExtraPayments CROSS JOIN
             dbo.Payments
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vPayment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'50
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vPayment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[51] 4[19] 2[28] 3) )"
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
               Top = 9
               Left = 57
               Bottom = 536
               Right = 305
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AspNetUsers"
            Begin Extent = 
               Top = 12
               Left = 370
               Bottom = 539
               Right = 673
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Companies"
            Begin Extent = 
               Top = 216
               Left = 1066
               Bottom = 413
               Right = 1307
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ExtraPayments"
            Begin Extent = 
               Top = 9
               Left = 1066
               Bottom = 213
               Right = 1288
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Payments"
            Begin Extent = 
               Top = 25
               Left = 752
               Bottom = 520
               Right = 1012
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
         SortType = 940
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 13', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vPayment';


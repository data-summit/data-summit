USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[DocumentTypes] ON 
GO
INSERT [dbo].[DocumentTypes] ([DocumentTypeId], [Name]) VALUES (1, N'Unknown')
GO
INSERT [dbo].[DocumentTypes] ([DocumentTypeId], [Name]) VALUES (2, N'DrawingPlanView')
GO
INSERT [dbo].[DocumentTypes] ([DocumentTypeId], [Name]) VALUES (3, N'Gantt')
GO
INSERT [dbo].[DocumentTypes] ([DocumentTypeId], [Name]) VALUES (4, N'Report')
GO
INSERT [dbo].[DocumentTypes] ([DocumentTypeId], [Name]) VALUES (5, N'Schematic')
GO
SET IDENTITY_INSERT [dbo].[DocumentTypes] OFF
GO
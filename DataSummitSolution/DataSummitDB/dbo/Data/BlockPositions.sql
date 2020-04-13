USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[BlockPositions] ON 
GO
INSERT [dbo].[BlockPositions] ([BlockPositionId], [Name], [CreatedDate], [UserId]) VALUES (1, N'BottomRight', NULL, NULL)
GO
INSERT [dbo].[BlockPositions] ([BlockPositionId], [Name], [CreatedDate], [UserId]) VALUES (2, N'BottomLeft', NULL, NULL)
GO
INSERT [dbo].[BlockPositions] ([BlockPositionId], [Name], [CreatedDate], [UserId]) VALUES (3, N'TopRight', NULL, NULL)
GO
INSERT [dbo].[BlockPositions] ([BlockPositionId], [Name], [CreatedDate], [UserId]) VALUES (4, N'TopLeft', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[BlockPositions] OFF
GO
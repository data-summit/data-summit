USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[UserInfoTypes] ON 
GO
INSERT [dbo].[UserInfoTypes] ([UserInfoTypeId], [Type], [CreatedDate], [UserId]) VALUES (1, N'Mobile (Personal)', NULL, NULL)
GO
INSERT [dbo].[UserInfoTypes] ([UserInfoTypeId], [Type], [CreatedDate], [UserId]) VALUES (2, N'Phone (Personal)', NULL, NULL)
GO
INSERT [dbo].[UserInfoTypes] ([UserInfoTypeId], [Type], [CreatedDate], [UserId]) VALUES (3, N'Mobile (Business)', NULL, NULL)
GO
INSERT [dbo].[UserInfoTypes] ([UserInfoTypeId], [Type], [CreatedDate], [UserId]) VALUES (4, N'Phone (Business)', NULL, NULL)
GO
INSERT [dbo].[UserInfoTypes] ([UserInfoTypeId], [Type], [CreatedDate], [UserId]) VALUES (6, N'Email', NULL, NULL)
GO
INSERT [dbo].[UserInfoTypes] ([UserInfoTypeId], [Type], [CreatedDate], [UserId]) VALUES (7, N'Fax', NULL, NULL)
GO
INSERT [dbo].[UserInfoTypes] ([UserInfoTypeId], [Type], [CreatedDate], [UserId]) VALUES (8, N'Social Media', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[UserInfoTypes] OFF
GO
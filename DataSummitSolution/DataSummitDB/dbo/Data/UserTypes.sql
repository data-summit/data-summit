USE [DataSummitDB]
GO

INSERT [dbo].[UserTypes] ([UserTypeId], [Value], [CreatedDate]) VALUES (1, N'Employee', CURRENT_TIMESTAMP)
GO
INSERT [dbo].[UserTypes] ([UserTypeId], [Value], [CreatedDate]) VALUES (2, N'Customer', CURRENT_TIMESTAMP)
GO
INSERT [dbo].[UserTypes] ([UserTypeId], [Value], [CreatedDate]) VALUES (3, N'Subcontractor', CURRENT_TIMESTAMP)
GO
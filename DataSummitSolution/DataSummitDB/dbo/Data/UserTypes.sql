USE [DataSummitDB]
GO

INSERT [dbo].[UserTypes] ([UserTypeId], [Value], [CreatedDate]) VALUES (1, N'Employee', CAST(N'2017-11-06T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[UserTypes] ([UserTypeId], [Value], [CreatedDate]) VALUES (2, N'Customer', CAST(N'2017-11-06T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[UserTypes] ([UserTypeId], [Value], [CreatedDate]) VALUES (3, N'Subcontractor', CAST(N'2017-11-06T00:00:00.000' AS DateTime))
GO
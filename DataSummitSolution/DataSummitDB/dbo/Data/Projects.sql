USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[Projects] ON 
GO
INSERT [dbo].[Projects] ([ProjectId], [Name], [StorageAccountName], [StorageAccountKey], [CompanyId], [CreatedDate], [UserId], [Region]) VALUES (1, N'Trial Docs', N'trialdocs', N'ldMUrDMe6CQmPlGbuA4OqJ9I3Nxzg8P3kMF7D1Zrd/LJZebZJvx7Ao3tR6Jz+RzUdJlIJwL3zbfU8rVmdYQvBA==', 1, CAST(N'2017-10-23T10:42:00.000' AS DateTime), 1, N'uksouth')
GO
SET IDENTITY_INSERT [dbo].[Projects] OFF
GO
USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[Companies] ON 
GO
INSERT [dbo].[Companies] ([CompanyId], [Name], [CompanyNumber], [Vatnumber], [CreatedDate], [UserId], [Website], [ResourceGroup], [Region]) VALUES (1, N'LIGHTOS ENERGY LIMITED', N'07389639', N'312662330', CAST(N'2017-11-06T00:00:00.000' AS DateTime), 1, NULL, N'LIGHTOS-ENERGY-LIMITED', N'uksouth')
GO
INSERT [dbo].[Companies] ([CompanyId], [Name], [CompanyNumber], [Vatnumber], [CreatedDate], [UserId], [Website], [ResourceGroup], [Region]) VALUES (2, N'THE GADJET LTD', N'10770635', NULL, CAST(N'2017-11-06T00:00:00.000' AS DateTime), 1, NULL, N'THE-GADJET-LTD', N'uksouth')
GO
INSERT [dbo].[Companies] ([CompanyId], [Name], [CompanyNumber], [Vatnumber], [CreatedDate], [UserId], [Website], [ResourceGroup], [Region]) VALUES (2002, N'SSD Adaptor', N'09237432', N'092374325', CAST(N'2019-11-22T18:11:43.023' AS DateTime), NULL, N'www.c.com', N'SSD-Adaptor', N'uksouth')
GO
SET IDENTITY_INSERT [dbo].[Companies] OFF
GO
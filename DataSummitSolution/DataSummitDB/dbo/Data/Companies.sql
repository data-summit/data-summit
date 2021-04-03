USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[Companies] ON 
GO
INSERT [dbo].[Companies] ([CompanyId], [Name], [CompanyNumber], [Vatnumber], [CreatedDate], [UserId], [Website], [ResourceGroup], [Region]) VALUES (1, N'LIGHTOS ENERGY LIMITED', N'07389639', N'312662330', CURRENT_TIMESTAMP, 1, NULL, N'LIGHTOS-ENERGY-LIMITED', N'uksouth')
GO
INSERT [dbo].[Companies] ([CompanyId], [Name], [CompanyNumber], [Vatnumber], [CreatedDate], [UserId], [Website], [ResourceGroup], [Region]) VALUES (2, N'THE GADJET LTD', N'10770635', NULL, CURRENT_TIMESTAMP, 1, NULL, N'THE-GADJET-LTD', N'uksouth')
GO
SET IDENTITY_INSERT [dbo].[Companies] OFF
GO
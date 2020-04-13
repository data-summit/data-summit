USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[Addresses] ON 
GO
INSERT [dbo].[Addresses] ([AddressId], [NumberName], [Street], [Street2], [Street3], [TownCity], [County], [CountryId], [PostCode], [CreatedDate], [CompanyId], [ProjectId]) VALUES (1, N'Barleys', N'Park View Road', NULL, N'Woldingham', N'Caterham                       ', N'Surrey                         ', 225, N'CR3 7DN', CAST(N'2019-06-01T00:00:00.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Addresses] ([AddressId], [NumberName], [Street], [Street2], [Street3], [TownCity], [County], [CountryId], [PostCode], [CreatedDate], [CompanyId], [ProjectId]) VALUES (2, N'1', N'Fairviews', NULL, NULL, N'Oxted                          ', N'Surrey                         ', 225, N'RH8 9BD', CAST(N'2019-06-01T00:00:00.000' AS DateTime), 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[Addresses] OFF
GO
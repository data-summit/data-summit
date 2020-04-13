USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[AzureCompanyResourceUrls] ON 
GO
INSERT [dbo].[AzureCompanyResourceUrls] ([AzureCompanyResourceUrlId], [Name], [URL], [Key], [ResourceType], [CompanyId]) VALUES (1, N'SplitDocument', N'https://datasummit-functionapps.azurewebsites.net/api/SplitDocument', N'HRJU6m3mijx4q8bbnfQQAaqrYV14dSC5ZNq4FBzkdtMzGNphXaVq6g==', N'Function', 1)
GO
INSERT [dbo].[AzureCompanyResourceUrls] ([AzureCompanyResourceUrlId], [Name], [URL], [Key], [ResourceType], [CompanyId]) VALUES (2, N'ImageToContainer', N'https://datasummit-functionapps.azurewebsites.net/api/ImageToContainer', N'LJy1QbyPBwGi9yChAaAmBqbcaGd8wVPqiTFalgMh/CWRyzt5GaPhcQ==', N'Function', 1)
GO
INSERT [dbo].[AzureCompanyResourceUrls] ([AzureCompanyResourceUrlId], [Name], [URL], [Key], [ResourceType], [CompanyId]) VALUES (3, N'DivideImage', N'https://datasummit-functionapps.azurewebsites.net/api/DivideImage', N'nrbw1TNDDGLH1rVdZ82LksedgDWq79EaurgsrfCyyrjNUijtzDQTxw==', N'Function', 1)
GO
INSERT [dbo].[AzureCompanyResourceUrls] ([AzureCompanyResourceUrlId], [Name], [URL], [Key], [ResourceType], [CompanyId]) VALUES (4, N'RecogniseTextAzure', N'https://datasummit-functionapps.azurewebsites.net/api/RecogniseTextAzure', N'qDF00aaJe1Bxf0BWvY65p6VTuSJR/U7DJMkMdD0F5ZJBAZVFNEevqA==', N'Function', 1)
GO
INSERT [dbo].[AzureCompanyResourceUrls] ([AzureCompanyResourceUrlId], [Name], [URL], [Key], [ResourceType], [CompanyId]) VALUES (5, N'PostProcessing', N'https://datasummit-functionapps.azurewebsites.net/api/PostProcessing', N'ytQe2gUa9UsH070XPOp5GIj3MQ12O8kFXT8TEv3plrW4lsgmEIP7HA==', N'Function', 1)
GO
INSERT [dbo].[AzureCompanyResourceUrls] ([AzureCompanyResourceUrlId], [Name], [URL], [Key], [ResourceType], [CompanyId]) VALUES (10003, N'RecogniseTextGoogle', N'https://datasummit-functionapps.azurewebsites.net/api/RecogniseTextGoogle', N'SaSc8KmZWiYQvGs1ui6x48yDnNtOJbFZcjg16w8QxcGQzbsYuZJNuw==', N'Function', 1)
GO
INSERT [dbo].[AzureCompanyResourceUrls] ([AzureCompanyResourceUrlId], [Name], [URL], [Key], [ResourceType], [CompanyId]) VALUES (10004, N'RecogniseTextAmazon', N'https://datasummit-functionapps.azurewebsites.net/api/RecogniseTextAmazon', N'6PPe6zPtCV/48oqVOFQiEADoHvSlfEC4t7B4MDaluJEzRq8a1T/PBQ==', N'Function', 1)
GO
SET IDENTITY_INSERT [dbo].[AzureCompanyResourceUrls] OFF
GO
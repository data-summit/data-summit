USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[AzureMLResources] ON 
GO
INSERT [dbo].[AzureMLResources] ([AzureMLResourcesId], [Name], [URL], [TrainingKey], [PredicitionKey]) VALUES (1, N'DocumentType', N'https://documentlayout.cognitiveservices.azure.com/customvision/v3.1/Prediction/721dd541-eced-4c52-addc-27b760a00b0d/classify/iterations/Iteration4/url', N'112f446293fc457ea2307881d4469795', N'768d8882d2bd4c139911389ba38bb44a')
GO
INSERT [dbo].[AzureMLResources] ([AzureMLResourcesId], [Name], [URL], [TrainingKey], [PredicitionKey]) VALUES (2, N'DrawingLayout', N'https://documentlayout.cognitiveservices.azure.com/customvision/v3.1/Prediction/144f656a-2714-422d-bbed-376a5bb6ac68/detect/iterations/Iteration1/url', N'112f446293fc457ea2307881d4469795', N'768d8882d2bd4c139911389ba38bb44a')
GO
INSERT [dbo].[AzureMLResources] ([AzureMLResourcesId], [Name], [URL], [TrainingKey], [PredicitionKey]) VALUES (3, N'ReportLayout', N'https://documentlayout.cognitiveservices.azure.com/customvision/v3.1/Prediction/a5faa28a-8b9d-4e62-bfec-b0cb5cb311df/detect/iterations/Iteration4/url', N'112f446293fc457ea2307881d4469795', N'768d8882d2bd4c139911389ba38bb44a')
GO
SET IDENTITY_INSERT [dbo].[AzureMLResources] OFF
GO
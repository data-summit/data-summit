﻿USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[Documents] ON 
GO

INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (1, N'Drawing.jpg', N'https://datasummitstorage.blob.core.windows.net/endpoint-test-documents/Drawing.jpg', N'endpoint-test-documents', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (2, N'Gantt.jpg', N'https://datasummitstorage.blob.core.windows.net/endpoint-test-documents/Gantt.jpg', N'endpoint-test-documents', 0, 1, 1, NULL, NULL, NULL, 9, 1, 0, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 2)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (3, N'Report.jpg', N'https://datasummitstorage.blob.core.windows.net/endpoint-test-documents/Report.jpg', N'endpoint-test-documents', 0, 1, 1, NULL, NULL, NULL, 9, 1, 0, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 2)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (10006, N'M 101_Plantroom Schematic, Heating and HWS.jpg', N'https://trialdocs.blob.core.windows.net/353048fc-e26a-4354-a88f-77ff3965bf29/Original.jpg', N'353048fc-e26a-4354-a88f-77ff3965bf29', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (10007, N'M 102_BCWS Tankroom Schematic.jpg', N'https://trialdocs.blob.core.windows.net/520b9194-27bd-47f2-b7c0-6fd3827d86bd/Original.jpg', N'520b9194-27bd-47f2-b7c0-6fd3827d86bd', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (10008, N'M 103_Heating Schematic, Radiators, LST''s & Air Curtains.jpg', N'https://trialdocs.blob.core.windows.net/64f5d390-1a86-4069-b8c1-5f6bd8544992/Original.jpg', N'64f5d390-1a86-4069-b8c1-5f6bd8544992', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (10011, N'M 104_Heating Schematic Radiators & AHU Heater Batteries.jpg', N'https://trialdocs.blob.core.windows.net/ebcde2ed-f0cd-4fbf-8e2a-b2d841fdadc5/Original.jpg', N'ebcde2ed-f0cd-4fbf-8e2a-b2d841fdadc5', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (10012, N'M 105_Domestic HWS Schematic Sheet 1 of 2.jpg', N'https://trialdocs.blob.core.windows.net/ff3df28d-5920-4d52-9158-3af13da7c30d/Original.jpg', N'ff3df28d-5920-4d52-9158-3af13da7c30d', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (10013, N'M 106_Domestic HWS Schematic Sheet 2 of 2.jpg', N'https://trialdocs.blob.core.windows.net/63aebd28-cb38-4a34-8944-e5fb03ea85ee/Original.jpg', N'63aebd28-cb38-4a34-8944-e5fb03ea85ee', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (10016, N'M 108_Boosted CWS Schematic Sheet 2 of 2.jpg', N'https://trialdocs.blob.core.windows.net/3e402000-3962-49a5-b350-818d3825cde4/Original.jpg', N'3e402000-3962-49a5-b350-818d3825cde4', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO
INSERT [dbo].[Documents] ([DocumentId], [FileName], [BlobUrl], [ContainerName], [Success], [ProjectId], [TemplateVersionId], [AzureConfidence], [GoogleConfidence], [AmazonConfidence], [PaperSizeId], [PaperOrientationId], [Processed], [CreatedDate], [UserId], [Type], [File], [DocumentTypeId]) VALUES (10017, N'M 109_AHU 01 Schematic, Supply.jpg', N'https://trialdocs.blob.core.windows.net/c64fdca6-67b8-465c-be18-9cc1605f372a/Original.jpg', N'c64fdca6-67b8-465c-be18-9cc1605f372a', 0, 1, 3007, NULL, NULL, NULL, NULL, 2, 1, CURRENT_TIMESTAMP, NULL, N'JPG', NULL, 5)
GO

SET IDENTITY_INSERT [dbo].[Documents] OFF
GO
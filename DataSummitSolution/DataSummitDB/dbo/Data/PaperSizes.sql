USE [DataSummitDB]
GO

SET IDENTITY_INSERT [dbo].[PaperSizes] ON 
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (1, N'Letter', 2197, 1701, CAST(216.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (2, N'LetterSmall', 2197, 1701, CAST(216.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (3, N'Tabloid', 3402, 2197, CAST(279.0 AS Decimal(6, 1)), CAST(432.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (4, N'Ledger', 2197, 3402, CAST(432.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (5, N'Legal', 2804, 1701, CAST(216.0 AS Decimal(6, 1)), CAST(356.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (6, N'Statement', 1701, 1103, CAST(140.0 AS Decimal(6, 1)), CAST(216.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (7, N'Executive', 2103, 1449, CAST(184.0 AS Decimal(6, 1)), CAST(267.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (8, N'A3', 3308, 2339, CAST(297.0 AS Decimal(6, 1)), CAST(420.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (9, N'A4', 2339, 1654, CAST(210.0 AS Decimal(6, 1)), CAST(297.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (10, N'A4Small', 2339, 1654, CAST(210.0 AS Decimal(6, 1)), CAST(297.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (11, N'A5', 1654, 1166, CAST(148.0 AS Decimal(6, 1)), CAST(210.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (12, N'B4', 2780, 1969, CAST(250.0 AS Decimal(6, 1)), CAST(353.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (13, N'B5', 1969, 1386, CAST(176.0 AS Decimal(6, 1)), CAST(250.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (14, N'Folio', 2599, 1701, CAST(216.0 AS Decimal(6, 1)), CAST(330.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (15, N'Quarto', 2166, 1693, CAST(215.0 AS Decimal(6, 1)), CAST(275.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (16, N'Standard10x14', 2804, 2000, CAST(254.0 AS Decimal(6, 1)), CAST(356.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (17, N'Standard11x17', 3402, 2197, CAST(279.0 AS Decimal(6, 1)), CAST(432.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (18, N'Note', 2197, 1701, CAST(216.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (19, N'Number9Envelope', 1772, 772, CAST(98.0 AS Decimal(6, 1)), CAST(225.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (20, N'Number10Envelope', 1898, 827, CAST(105.0 AS Decimal(6, 1)), CAST(241.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (21, N'Number11Envelope', 2079, 898, CAST(114.0 AS Decimal(6, 1)), CAST(264.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (22, N'Number12Envelope', 2197, 953, CAST(121.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (23, N'Number14Envelope', 2300, 1000, CAST(127.0 AS Decimal(6, 1)), CAST(292.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (24, N'CSheet', 4403, 3402, CAST(432.0 AS Decimal(6, 1)), CAST(559.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (25, N'DSheet', 6805, 4403, CAST(559.0 AS Decimal(6, 1)), CAST(864.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (26, N'ESheet', 8805, 6805, CAST(864.0 AS Decimal(6, 1)), CAST(1118.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (27, N'DLEnvelope', 1733, 866, CAST(110.0 AS Decimal(6, 1)), CAST(220.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (28, N'C5Envelope', 1804, 1276, CAST(162.0 AS Decimal(6, 1)), CAST(229.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (29, N'C3Envelope', 3607, 2552, CAST(324.0 AS Decimal(6, 1)), CAST(458.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (30, N'C4Envelope', 2552, 1804, CAST(229.0 AS Decimal(6, 1)), CAST(324.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (31, N'C6Envelope', 1276, 898, CAST(114.0 AS Decimal(6, 1)), CAST(162.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (32, N'C65Envelope', 1804, 898, CAST(114.0 AS Decimal(6, 1)), CAST(229.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (33, N'B4Envelope', 2780, 1969, CAST(250.0 AS Decimal(6, 1)), CAST(353.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (34, N'B5Envelope', 1969, 1386, CAST(176.0 AS Decimal(6, 1)), CAST(250.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (35, N'B6Envelope', 984, 1386, CAST(176.0 AS Decimal(6, 1)), CAST(125.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (36, N'ItalyEnvelope', 1811, 866, CAST(110.0 AS Decimal(6, 1)), CAST(230.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (37, N'MonarchEnvelope', 1504, 772, CAST(98.0 AS Decimal(6, 1)), CAST(191.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (38, N'PersonalEnvelope', 1300, 725, CAST(92.0 AS Decimal(6, 1)), CAST(165.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (39, N'USStandardFanfold', 2197, 2977, CAST(378.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (40, N'GermanStandardFanfold', 2402, 1701, CAST(216.0 AS Decimal(6, 1)), CAST(305.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (41, N'GermanLegalFanfold', 2599, 1701, CAST(216.0 AS Decimal(6, 1)), CAST(330.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (42, N'IsoB4', 2780, 1969, CAST(250.0 AS Decimal(6, 1)), CAST(353.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (44, N'Standard9x11', 2197, 1804, CAST(229.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (45, N'Standard10x11', 2197, 2000, CAST(254.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (46, N'Standard15x11', 2197, 3001, CAST(381.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (47, N'InviteEnvelope', 1733, 1733, CAST(220.0 AS Decimal(6, 1)), CAST(220.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (52, N'TabloidExtra', 3599, 2339, CAST(297.0 AS Decimal(6, 1)), CAST(457.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (53, N'A4Extra', 2536, 1859, CAST(236.0 AS Decimal(6, 1)), CAST(322.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (54, N'LetterTransverse', 2197, 1654, CAST(210.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (55, N'A4Transverse', 2339, 1654, CAST(210.0 AS Decimal(6, 1)), CAST(297.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (56, N'LetterExtraTransverse', 2402, 1859, CAST(236.0 AS Decimal(6, 1)), CAST(305.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (57, N'APlus', 2804, 1788, CAST(227.0 AS Decimal(6, 1)), CAST(356.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (58, N'BPlus', 3836, 2402, CAST(305.0 AS Decimal(6, 1)), CAST(487.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (59, N'LetterPlus', 2536, 1701, CAST(216.0 AS Decimal(6, 1)), CAST(322.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (60, N'A4Plus', 2599, 1654, CAST(210.0 AS Decimal(6, 1)), CAST(330.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (61, N'A5Transverse', 1654, 1166, CAST(148.0 AS Decimal(6, 1)), CAST(210.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (62, N'B5Transverse', 2024, 1433, CAST(182.0 AS Decimal(6, 1)), CAST(257.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (63, N'A3Extra', 3505, 2536, CAST(322.0 AS Decimal(6, 1)), CAST(445.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (64, N'A5Extra', 1851, 1370, CAST(174.0 AS Decimal(6, 1)), CAST(235.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (65, N'B5Extra', 2174, 1583, CAST(201.0 AS Decimal(6, 1)), CAST(276.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (66, N'A2', 4678, 3308, CAST(420.0 AS Decimal(6, 1)), CAST(594.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (67, N'A3Transverse', 3308, 2339, CAST(297.0 AS Decimal(6, 1)), CAST(420.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (68, N'A3ExtraTransverse', 3505, 2536, CAST(322.0 AS Decimal(6, 1)), CAST(445.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (70, N'A6', 1166, 827, CAST(105.0 AS Decimal(6, 1)), CAST(148.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (75, N'LetterRotated', 1701, 2197, CAST(279.0 AS Decimal(6, 1)), CAST(216.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (76, N'A3Rotated', 2339, 3308, CAST(420.0 AS Decimal(6, 1)), CAST(297.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (77, N'A4Rotated', 1654, 2339, CAST(297.0 AS Decimal(6, 1)), CAST(210.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (78, N'A5Rotated', 1166, 1654, CAST(210.0 AS Decimal(6, 1)), CAST(148.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (79, N'B4JisRotated', 2024, 2867, CAST(364.0 AS Decimal(6, 1)), CAST(257.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (80, N'B5JisRotated', 1433, 2024, CAST(257.0 AS Decimal(6, 1)), CAST(182.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (83, N'A6Rotated', 827, 1166, CAST(148.0 AS Decimal(6, 1)), CAST(105.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (88, N'B6Jis', 1433, 1008, CAST(128.0 AS Decimal(6, 1)), CAST(182.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (89, N'B6JisRotated', 1008, 1433, CAST(182.0 AS Decimal(6, 1)), CAST(128.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (90, N'Standard12x11', 2197, 2402, CAST(305.0 AS Decimal(6, 1)), CAST(279.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (91, N'A0', 9364, 6624, CAST(841.0 AS Decimal(6, 1)), CAST(1189.0 AS Decimal(6, 1)))
GO
INSERT [dbo].[PaperSizes] ([PaperSizeId], [Name], [PixelWidth], [PixelHeight], [PhysicalWidth], [PhysicalHeight]) VALUES (92, N'A1', 6624, 4678, CAST(594.0 AS Decimal(6, 1)), CAST(841.0 AS Decimal(6, 1)))
GO
SET IDENTITY_INSERT [dbo].[PaperSizes] OFF
GO
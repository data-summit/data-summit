﻿CREATE TABLE [dbo].[DocumentFeatures]
(
	[DocumentFeatureId]	BIGINT			IDENTITY(1,1) NOT NULL,
	[Vendor]			NVARCHAR(255)	NOT NULL,
	[Value]				NVARCHAR(1023)	NOT NULL,
	[DocumentId]			BIGINT			NOT NULL,
	[Left]				BIGINT			NULL, 
    [Top]				BIGINT			NULL, 
    [Width]				BIGINT			NULL, 
    [Height]			BIGINT			NULL, 
    [Feature]			TINYINT			NULL,
    [Center] BIGINT NULL, 
    CONSTRAINT [PK_DocumentFeatureId] PRIMARY KEY CLUSTERED ([DocumentFeatureId] ASC),
	CONSTRAINT [FK_DocumentFeatures_Documents] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[Documents] ([DocumentId]) ON DELETE CASCADE ON UPDATE CASCADE
)
CREATE TABLE [dbo].[DocumentFeatures]
(
	[DocumentFeatureId]	BIGINT			IDENTITY(1,1) NOT NULL,
	[Vendor]			NVARCHAR(255)	NOT NULL,
	[Value]				NVARCHAR(1023)	NOT NULL,
	[Left]				DECIMAL(6, 5)	NULL, 
    [Top]				DECIMAL(6, 5)	NULL, 
    [Width]				DECIMAL(6, 5)	NULL, 
    [Height]			DECIMAL(6, 5)	NULL, 
    [Feature]			NVARCHAR(255)	NOT NULL,
	[Confidence]		DECIMAL(6,5)	NOT NULL,
	[DocumentId]		BIGINT			NOT NULL
    CONSTRAINT [PK_DocumentFeatureId] PRIMARY KEY CLUSTERED ([DocumentFeatureId] ASC),
	CONSTRAINT [FK_DocumentFeatures_Documents] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[Documents] ([DocumentId]) ON DELETE CASCADE ON UPDATE CASCADE
)

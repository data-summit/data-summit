CREATE TABLE [dbo].[DrawingFeatures]
(
	[DrawingFeatureId]	BIGINT			IDENTITY(1,1) NOT NULL,
	[Vendor]			NVARCHAR(255)	NOT NULL,
	[Value]				NVARCHAR(1023)	NOT NULL,
	[DrawingId]			BIGINT			NOT NULL,
	[Left]				BIGINT			NULL, 
    [Top]				BIGINT			NULL, 
    [Width]				BIGINT			NULL, 
    [Height]			BIGINT			NULL, 
    [Feature]			TINYINT			NULL,
    [Center] BIGINT NULL, 
    CONSTRAINT [PK_DrawingFeatureId] PRIMARY KEY CLUSTERED ([DrawingFeatureId] ASC),
	CONSTRAINT [FK_DrawingFeatures_Drawings] FOREIGN KEY ([DrawingId]) REFERENCES [dbo].[Drawings] ([DrawingId]) ON DELETE CASCADE ON UPDATE CASCADE
)

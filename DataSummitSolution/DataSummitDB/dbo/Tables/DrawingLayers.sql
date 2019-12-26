CREATE TABLE [dbo].[DrawingLayers]
(
	[DrawingLayerId]	BIGINT			IDENTITY(1,1) NOT NULL, 
	[Name]				NVARCHAR(1023)	NOT NULL,
	[DrawingId]			BIGINT			NOT NULL,
	CONSTRAINT [PK_DrawingLayerId] PRIMARY KEY CLUSTERED ([DrawingLayerId] ASC),
	CONSTRAINT [FK_DrawingLayers_Drawings] FOREIGN KEY ([DrawingId]) REFERENCES [dbo].[Drawings] ([DrawingId]) ON DELETE CASCADE ON UPDATE CASCADE
)

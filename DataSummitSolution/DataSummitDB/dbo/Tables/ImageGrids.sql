CREATE TABLE [dbo].[ImageGrids]
(
	[ImageGridId]		BIGINT          IDENTITY (1, 1) NOT NULL,
	[Name]				NVARCHAR(MAX)   NOT NULL,
	[WidthStart]		INT				NOT NULL,
	[HeightStart]		INT				NOT NULL,
	[Width]				INT				NOT NULL,
	[Height]			INT				NOT NULL,
	[BlobURL]			NVARCHAR(2083)  NOT NULL,
	[IterationAmazon]	TINYINT			NOT NULL DEFAULT 0,
	[IterationAzure]	TINYINT			NOT NULL DEFAULT 0,
	[IterationGoogle]	TINYINT			NOT NULL DEFAULT 0,
	[ProcessedAmazon]	BIT				NOT NULL DEFAULT 0,
	[ProcessedAzure]	BIT				NOT NULL DEFAULT 0,
	[ProcessedGoogle]	BIT				NOT NULL DEFAULT 0,
	[Type]				TINYINT			NOT NULL,
	[DocumentId]			BIGINT			NOT NULL,
	CONSTRAINT [PK_ImageGrids] PRIMARY KEY CLUSTERED ([ImageGridId] ASC),
	CONSTRAINT [FK_ImageGrids_Documents] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[Documents] ([DocumentId])
)

CREATE TABLE [dbo].[Drawings] (
    [DrawingId]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [FileName]            NVARCHAR (1023) NOT NULL,
    [BlobUrl]             NVARCHAR (1023) NOT NULL,
	[ContainerName]       NVARCHAR (36)   NOT NULL,
    [Success]             BIT             NOT NULL,
    [ProjectId]           INT             NOT NULL,
    [ProfileVersionId]    INT             NOT NULL,
    [AzureConfidence]	  DECIMAL (3, 2)  NULL,
    [GoogleConfidence]    DECIMAL (3, 2)  NULL,
    [AmazonConfidence]    DECIMAL (3, 2)  NULL,
	[PaperSizeId]		  TINYINT		  NULL,
	[PaperOrientationId]  TINYINT		  NOT NULL,
    [Processed]           BIT             NOT NULL,
    [CreatedDate]         DATETIME        NULL,
    [UserId]              NVARCHAR (50)   NULL,
    [Type]                NVARCHAR (7)    NULL,
    [File]				  VARBINARY(MAX)  NULL, 
    CONSTRAINT [PK_Drawing] PRIMARY KEY CLUSTERED ([DrawingId] ASC),
    CONSTRAINT [FK_Drawings_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId]),
	CONSTRAINT [FK_Drawings_PaperSizes] FOREIGN KEY ([PaperSizeId]) REFERENCES [dbo].[PaperSizes] ([PaperSizeId]),
	CONSTRAINT [FK_Drawings_PaperOrientations] FOREIGN KEY ([PaperOrientationId]) REFERENCES [dbo].[PaperOrientations] ([PaperOrientationId])
);


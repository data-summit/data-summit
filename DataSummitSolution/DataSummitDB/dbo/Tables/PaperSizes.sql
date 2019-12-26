CREATE TABLE [dbo].[PaperSizes] (
    [PaperSizeId]    TINYINT        IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (31)  NOT NULL,
    [PixelWidth]     INT            NOT NULL,
    [PixelHeight]    INT            NOT NULL,
    [PhysicalWidth]  DECIMAL (6, 1) NULL,
    [PhysicalHeight] DECIMAL (6, 1) NULL,
    CONSTRAINT [PK_PaperSizes] PRIMARY KEY CLUSTERED ([PaperSizeId] ASC),
);


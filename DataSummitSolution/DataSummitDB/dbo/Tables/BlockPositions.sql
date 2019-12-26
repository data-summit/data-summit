CREATE TABLE [dbo].[BlockPositions] (
    [BlockPositionId] TINYINT       IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (15) NOT NULL,
    [CreatedDate]     DATETIME      NULL,
    [UserId]          BIGINT        NULL,
    CONSTRAINT [PK_BlockPositions] PRIMARY KEY CLUSTERED ([BlockPositionId] ASC)
);


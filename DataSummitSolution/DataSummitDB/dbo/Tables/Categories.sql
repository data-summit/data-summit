CREATE TABLE [dbo].[Categories] (
    [CategoryId]  INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (15) NOT NULL,
    [Description] NTEXT         NULL,
    [Picture]     IMAGE         NULL,
    [CreatedDate] DATETIME      NULL,
    [UserId]      BIGINT        NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);


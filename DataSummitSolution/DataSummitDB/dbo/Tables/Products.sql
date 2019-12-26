CREATE TABLE [dbo].[Products] (
    [ProductId]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (63) NOT NULL,
    [SupplierId]      INT           NULL,
    [CategoryId]      INT           NULL,
    [QuantityPerUnit] NVARCHAR (31) NULL,
    [UnitPrice]       MONEY         NULL,
    [UnitsInStock]    SMALLINT      NULL,
    [UnitsOnOrder]    SMALLINT      NULL,
    [ReorderLevel]    SMALLINT      NULL,
    [Discontinued]    BIT           NOT NULL,
    [CreatedDate]     DATETIME      NULL,
    [UserId]          BIGINT        NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);


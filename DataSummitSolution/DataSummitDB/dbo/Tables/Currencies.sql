CREATE TABLE [dbo].[Currencies] (
    [CurrencyId]     SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Entity]         NVARCHAR (100) NOT NULL,
    [Name]			 NVARCHAR (100) NOT NULL,
    [AlphabeticCode] CHAR (3)       NOT NULL,
    [NumericCode]    CHAR (3)       NULL,
    [MinorUnit]      CHAR (1)       NULL,
    [IsFundYesNo]    BIT            NOT NULL,
    [CreatedDate]    DATETIME       NULL,
    [UserId]         BIGINT         NULL,
    CONSTRAINT [PK_Currencies] PRIMARY KEY CLUSTERED ([CurrencyId] ASC)
);


CREATE TABLE [dbo].[GoogleLanguages] (
    [GoogleLanguageId] TINYINT        NOT NULL,
    [Name]             NVARCHAR (15)  NOT NULL,
    [Code]             NCHAR (3)      NOT NULL,
    [Notes]            NVARCHAR (127) NULL,
    CONSTRAINT [PK_GoogleLanguages] PRIMARY KEY CLUSTERED ([GoogleLanguageId] ASC)
);


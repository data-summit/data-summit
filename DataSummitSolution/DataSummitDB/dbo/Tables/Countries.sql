CREATE TABLE [dbo].[Countries] (
    [CountryId]        SMALLINT    NOT NULL,
    [ISO]              CHAR (3)  NOT NULL,
    [Name]             NVARCHAR(127) NOT NULL,
    [SentenceCaseName] NVARCHAR(127) NOT NULL,
    [ISO3]             CHAR (3)    NULL,
    [numcode]          SMALLINT    NULL,
    [phonecode]        INT         NOT NULL,
    [CreatedDate]      DATETIME    NULL,
    [UserId]           BIGINT      NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);


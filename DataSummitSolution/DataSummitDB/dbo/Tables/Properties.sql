CREATE TABLE [dbo].[Properties] (
    [PropertyId]			BIGINT		IDENTITY (1, 1) NOT NULL,
    [SentenceId]			UNIQUEIDENTIFIER			NOT NULL,
    [ProfileAttributeId]	BIGINT						NOT NULL,
    CONSTRAINT [PK_Properties] PRIMARY KEY CLUSTERED ([PropertyId] ASC),
    CONSTRAINT [FK_Properties_ProfileAttributes] FOREIGN KEY ([ProfileAttributeId]) REFERENCES [dbo].[ProfileAttributes] ([ProfileAttributeId]) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_Properties_Sentences] FOREIGN KEY ([SentenceId]) REFERENCES [dbo].[Sentences] ([SentenceId]) ON DELETE CASCADE ON UPDATE CASCADE
);
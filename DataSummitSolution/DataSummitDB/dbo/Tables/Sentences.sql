﻿CREATE TABLE [dbo].[Sentences]
(
	[SentenceId]	UNIQUEIDENTIFIER	NOT NULL, 
    [Words]				NVARCHAR(MAX)		NOT NULL, 
    [Width]				INT					NOT NULL DEFAULT 0, 
	[Height]			INT					NOT NULL DEFAULT 0, 
	[Left]				INT					NOT NULL DEFAULT 0, 
	[Top]				INT					NOT NULL DEFAULT 0, 
    [Vendor]			NVARCHAR(63)		NOT NULL, 
    [IsUsed]			BIT					NOT NULL DEFAULT 0, 
    [Confidence]		DECIMAL(3, 2)		NULL,
	[SlendernessRatio]	DECIMAL				NULL DEFAULT 0, 
	[DocumentId]			BIGINT				NOT NULL, 

    [ModifiedWords] NVARCHAR(MAX) NULL DEFAULT NULL, 
    CONSTRAINT [PK_SentenceId] PRIMARY KEY CLUSTERED ([SentenceId] ASC),
    CONSTRAINT [FK_Sentences_Documents] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[Documents] ([DocumentId]) ON DELETE CASCADE ON UPDATE CASCADE
)
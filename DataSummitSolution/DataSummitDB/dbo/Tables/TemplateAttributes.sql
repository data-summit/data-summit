﻿CREATE TABLE [dbo].[TemplateAttributes] (
    [TemplateAttributeId]	BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]					NVARCHAR (255) NOT NULL,
    [NameX]					INT            NOT NULL,
    [NameY]					INT            NOT NULL,
    [NameWidth]				SMALLINT       NOT NULL,
    [NameHeight]			SMALLINT       NOT NULL,
    [PaperSizeId]			TINYINT        NOT NULL,
    [BlockPositionId]		TINYINT        NOT NULL,
    [TemplateVersionId]		INT            NOT NULL,
    [CreatedDate]			DATETIME       NULL,
    [UserId]				BIGINT         NULL,
    [Value]					NVARCHAR (MAX) NULL,
    [ValueX]				INT            NULL,
    [ValueY]				INT            NULL,
    [ValueWidth]			SMALLINT       NULL,
    [ValueHeight]			SMALLINT       NULL,
	[StandardAttributeId]	SMALLINT	   NULL,
    CONSTRAINT [PK_TemplateAttributes] PRIMARY KEY CLUSTERED ([TemplateAttributeId] ASC),
    CONSTRAINT [FK_TemplateAttributes_BlockPositions] FOREIGN KEY ([BlockPositionId]) REFERENCES [dbo].[BlockPositions] ([BlockPositionId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TemplateAttributes_PaperSizes] FOREIGN KEY ([PaperSizeId]) REFERENCES [dbo].[PaperSizes] ([PaperSizeId]),
    CONSTRAINT [FK_TemplateAttributes_TemplateVersions] FOREIGN KEY ([TemplateVersionId]) REFERENCES [dbo].[TemplateVersions] ([TemplateVersionId]) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_TemplateAttributes_StandardAttributes] FOREIGN KEY ([StandardAttributeId]) REFERENCES [dbo].[StandardAttributes] ([StandardAttributeId]) 
);


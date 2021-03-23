﻿CREATE TABLE [dbo].[Documents] (
    [DocumentId]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [FileName]            NVARCHAR (1023) NOT NULL,
    [BlobUrl]             NVARCHAR (1023) NOT NULL,
	[ContainerName]       NVARCHAR (36)   NOT NULL,
    [Success]             BIT             NOT NULL,
    [ProjectId]           INT             NOT NULL,
    [TemplateVersionId]   INT             NOT NULL,
    [AzureConfidence]	  DECIMAL (3, 2)  NULL,
    [GoogleConfidence]    DECIMAL (3, 2)  NULL,
    [AmazonConfidence]    DECIMAL (3, 2)  NULL,
	[PaperSizeId]		  TINYINT		  NULL,
	[PaperOrientationId]  TINYINT		  NOT NULL,
    [Processed]           BIT             NOT NULL,
    [CreatedDate]         DATETIME        NULL,
    [UserId]              NVARCHAR (50)   NULL,
    [Type]                NVARCHAR (7)    NULL,
    [File]				  VARBINARY(MAX)  NULL, 
    [DocumentTypeId]      TINYINT         NOT NULL, 
    CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED ([DocumentId] ASC),
    CONSTRAINT [FK_Documents_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId]),
	CONSTRAINT [FK_Documents_PaperSizes] FOREIGN KEY ([PaperSizeId]) REFERENCES [dbo].[PaperSizes] ([PaperSizeId]) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_Documents_PaperOrientations] FOREIGN KEY ([PaperOrientationId]) REFERENCES [dbo].[PaperOrientations] ([PaperOrientationId]), 
    CONSTRAINT [FK_Documents_TemplateVersions] FOREIGN KEY ([TemplateVersionId]) REFERENCES [TemplateVersions]([TemplateVersionId]),
    CONSTRAINT [FK_Documents_DocumentTypes] FOREIGN KEY ([DocumentTypeId]) REFERENCES [DocumentTypes]([DocumentTypeId]) ON DELETE CASCADE ON UPDATE CASCADE
);

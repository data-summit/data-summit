CREATE TABLE [dbo].[TemplateVersions] (
    [TemplateVersionId] INT             IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (1023) NOT NULL,
    [CompanyId]        INT             NOT NULL,
    [Image]            IMAGE           NULL,
    [Width]            INT             NULL,
    [Height]           INT             NULL,
    [CreatedDate]      DATETIME        NULL,
    [UserId]           BIGINT          NULL,
    [WidthOriginal]    INT             NULL,
    [HeightOriginal]   INT             NULL,
    [X]                INT             NULL,
    [Y]                INT             NULL,
    CONSTRAINT [PK_TemplateVersionId] PRIMARY KEY CLUSTERED ([TemplateVersionId] ASC)
);


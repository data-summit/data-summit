CREATE TABLE [dbo].[DocumentTemplates]
(
    [DocumentTemplateId] INT NOT NULL PRIMARY KEY, 
    [DocumentId] BIGINT NULL, 
    [TemplateVersionId] INT NULL, 
    CONSTRAINT [FK_DocumentTemplates_Documents] FOREIGN KEY (DocumentId) REFERENCES [Documents]([DocumentId]), 
    CONSTRAINT [FK_DocumentTemplates_TemplateVersions] FOREIGN KEY ([TemplateVersionId]) REFERENCES [TemplateVersions]([TemplateVersionId])
)

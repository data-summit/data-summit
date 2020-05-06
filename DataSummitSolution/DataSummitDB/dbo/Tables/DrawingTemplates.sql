CREATE TABLE [dbo].[DrawingTemplates]
(
    [DrawingTemplateId] INT NOT NULL PRIMARY KEY, 
    [DrawingId] BIGINT NULL, 
    [ProfileVersionId] INT NULL, 
    CONSTRAINT [FK_DrawingTemplates_Drawings] FOREIGN KEY (DrawingId) REFERENCES [Drawings]([DrawingId]), 
    CONSTRAINT [FK_DrawingTemplates_ProfileVersions] FOREIGN KEY ([ProfileVersionId]) REFERENCES [ProfileVersions]([ProfileVersionId])
)

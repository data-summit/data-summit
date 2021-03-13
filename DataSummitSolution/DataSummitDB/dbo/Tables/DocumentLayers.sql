CREATE TABLE [dbo].[DocumentLayers]
(
	[DocumentLayerId]	BIGINT			IDENTITY(1,1) NOT NULL, 
	[Name]				NVARCHAR(1023)	NOT NULL,
	[DocumentId]		BIGINT			NOT NULL,
	CONSTRAINT [PK_DocumentLayerId] PRIMARY KEY CLUSTERED ([DocumentLayerId] ASC),
	CONSTRAINT [FK_DocumentLayers_Documents] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[Documents] ([DocumentId]) ON DELETE CASCADE ON UPDATE CASCADE
)

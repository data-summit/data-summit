CREATE TABLE [dbo].[StandardAttributes]
(
	[StandardAttributeId]	SMALLINT		IDENTITY (1, 1) NOT NULL,
	[Name]					NVARCHAR(255)	NOT NULL,
	CONSTRAINT [PK_StandardAttributes] PRIMARY KEY ([StandardAttributeId] ASC)
)

CREATE TABLE [dbo].[Tasks]
(
	[TaskId]			BIGINT              IDENTITY (1, 1) NOT NULL,
	[Name]				NVARCHAR (255)		NOT NULL,
	[TimeStamp]			DATETIME			NOT NULL,
	[DrawingId]			BIGINT				NOT NULL,
	[Duration]			TIME				NOT NULL, 
    CONSTRAINT [PK_TaskId] PRIMARY KEY CLUSTERED ([TaskId] ASC),
    CONSTRAINT [FK_Tasks_Drawing] FOREIGN KEY ([DrawingId]) REFERENCES [dbo].[Drawings] ([DrawingId]) ON DELETE CASCADE ON UPDATE CASCADE
)
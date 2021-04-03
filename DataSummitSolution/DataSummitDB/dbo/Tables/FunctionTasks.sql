CREATE TABLE [dbo].[FunctionTasks]
(
	[TaskId]			BIGINT              IDENTITY (1, 1) NOT NULL,
	[Name]				NVARCHAR (255)		NOT NULL,
	[TimeStamp]			DATETIME			NOT NULL,
	[DocumentId]			BIGINT				NOT NULL,
	[Duration]			TIME				NOT NULL, 
    CONSTRAINT [PK_TaskId] PRIMARY KEY CLUSTERED ([TaskId] ASC),
    CONSTRAINT [FK_FunctionTasks_Document] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[Documents] ([DocumentId]) ON DELETE CASCADE ON UPDATE CASCADE
)
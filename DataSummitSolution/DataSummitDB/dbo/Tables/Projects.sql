CREATE TABLE [dbo].[Projects] (
    [ProjectId]			    INT             IDENTITY (1, 1) NOT NULL,
    [Name]				    NVARCHAR (1024) NOT NULL,
	[StorageAccountName]	NVARCHAR(255)	NOT NULL,
	[StorageAccountKey]	    NVARCHAR(255)	NOT NULL,
    [CompanyId]			    INT             NOT NULL,
    [CreatedDate]		    DATETIME        NOT NULL,
    [UserId]			    BIGINT          NOT NULL,
    [Region]                NVARCHAR(31)    NULL, 
    CONSTRAINT [PK_ProjectId] PRIMARY KEY CLUSTERED ([ProjectId] ASC)
);
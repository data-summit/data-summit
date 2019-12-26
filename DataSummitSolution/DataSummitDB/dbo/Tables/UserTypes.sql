CREATE TABLE [dbo].[UserTypes] (
    [UserTypeId] TINYINT    NOT NULL,
    [Value]         NVARCHAR(15) NOT NULL,
    [CreatedDate]   DATETIME   NOT NULL,
    CONSTRAINT [PK_UserTypes] PRIMARY KEY CLUSTERED ([UserTypeId] ASC)
);


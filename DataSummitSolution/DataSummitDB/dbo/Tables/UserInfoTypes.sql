CREATE TABLE [dbo].[UserInfoTypes] (
    [UserInfoTypeId] TINYINT       IDENTITY (1, 1) NOT NULL,
    [Type]              NVARCHAR (63) NOT NULL,
    [CreatedDate]       DATETIME      NULL,
    [UserId]            BIGINT        NULL,
    CONSTRAINT [PK_UserInfoTypes] PRIMARY KEY CLUSTERED ([UserInfoTypeId] ASC)
);


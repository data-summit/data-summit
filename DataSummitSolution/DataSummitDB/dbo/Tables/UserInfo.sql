CREATE TABLE [dbo].[UserInfo] (
    [UserInfoId]		BIGINT        IDENTITY (1, 1) NOT NULL,
    [Value]             NVARCHAR (63) NOT NULL,
    [CreatedDate]       DATETIME      NULL,
    [Id]                NVARCHAR (50) NULL,
    [UserInfoTypeId]	TINYINT       NULL,
    CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED ([UserInfoId] ASC),
    CONSTRAINT [FK_UserInfo_UserInfoTypes] FOREIGN KEY ([UserInfoTypeId]) REFERENCES [dbo].[UserInfoTypes] ([UserInfoTypeId]),
    CONSTRAINT [FK_UserInfo_AspNetUsers] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);


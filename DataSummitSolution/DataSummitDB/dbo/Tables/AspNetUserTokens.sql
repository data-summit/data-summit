CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId]        NVARCHAR (150) NOT NULL,
    [LoginProvider] NVARCHAR (150) NOT NULL,
    [Name]          NVARCHAR (150) NOT NULL,
    [Value]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetUserToken] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC)
);


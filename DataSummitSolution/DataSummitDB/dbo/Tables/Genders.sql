CREATE TABLE [dbo].[Genders] (
    [GenderId]    TINYINT       NOT NULL,
    [Type]        NVARCHAR (15) NOT NULL,
    [CreatedDate] DATETIME      NULL,
    [UserId]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_Genders] PRIMARY KEY CLUSTERED ([GenderId] ASC)
);


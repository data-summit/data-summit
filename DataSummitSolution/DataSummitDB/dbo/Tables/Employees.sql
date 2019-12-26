CREATE TABLE [dbo].[Employees] (
    [EmployeeId]      BIGINT         NOT NULL,
    [FirstName]       NVARCHAR (31)  NOT NULL,
    [MiddleNames]     NVARCHAR (63)  NULL,
    [Surname]         NVARCHAR (31)  NOT NULL,
    [Title]           NVARCHAR (51)  NULL,
    [TitleOfCourtesy] NVARCHAR (31)  NULL,
    [JobTitle]        NVARCHAR (63)  NULL,
    [BirthDate]       DATETIME       NULL,
    [HireDate]        DATETIME       NULL,
    [Notes]           NTEXT          NULL,
    [ReportsTo]       INT            NULL,
    [Photo]           IMAGE          NULL,
    [PhotoPath]       NVARCHAR (255) NULL,
    [GenderId]        TINYINT        NULL,
    [CreatedDate]     DATETIME       NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([EmployeeId] ASC),
    CONSTRAINT [CK_BirthDate] CHECK ([BirthDate]<getdate()),
    CONSTRAINT [FK_Employees_Genders] FOREIGN KEY ([GenderId]) REFERENCES [dbo].[Genders] ([GenderId])
);


CREATE TABLE [dbo].[Companies] (
    [CompanyId]     INT             IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR(63)   NOT NULL,
    [CompanyNumber] CHAR(8)        NULL,
    [Vatnumber]     CHAR(9)        NULL,
    [CreatedDate]   DATETIME        NULL,
    [UserId]        BIGINT          NULL,
    [Website]       NVARCHAR(2083) NULL,
    [ResourceGroup] NVARCHAR(63)    NULL, 
    [Region]        NVARCHAR(31)    NULL, 
    CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED ([CompanyId] ASC)
);


CREATE TABLE [dbo].[EmployeeTerritories] (
    [EmployeeId]  INT           NOT NULL,
    [TerritoryId] NVARCHAR (20) NOT NULL,
    [CreatedDate] DATETIME      NULL,
    [UserId]      BIGINT        NULL,
    CONSTRAINT [PK_EmployeeTerritories] PRIMARY KEY CLUSTERED ([EmployeeId] ASC, [TerritoryId] ASC)
);


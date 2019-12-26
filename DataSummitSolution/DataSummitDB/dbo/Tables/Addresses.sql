CREATE TABLE [dbo].[Addresses] (
    [AddressId]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [NumberName]  NVARCHAR (15) NULL,
    [Street]      NVARCHAR (63) NULL,
    [Street2]     NVARCHAR (63) NULL,
    [Street3]     NVARCHAR (63) NULL,
    [TownCity]    NCHAR (31)    NULL,
    [County]      NCHAR (31)    NULL,
    [CountryId]   SMALLINT      NOT NULL,
    [PostCode]    NVARCHAR (10) NULL,
    [CreatedDate] DATETIME      NULL,
    [CompanyId]   INT           NULL,
    [ProjectId]	  INT			NULL, 
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([AddressId] ASC),
    CONSTRAINT [FK_Addresses_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([CompanyId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Addresses_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([CountryId]) ON UPDATE CASCADE,
	CONSTRAINT [FK_Addresses_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId])
);


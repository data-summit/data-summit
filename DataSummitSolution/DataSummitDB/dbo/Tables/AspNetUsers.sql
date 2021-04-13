CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (50)	  NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [Email]                NVARCHAR (256)     NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [LockoutEnabled]       BIT                NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [NormalizedEmail]      NVARCHAR (256)     NULL,
    [NormalizedUserName]   NVARCHAR (256)     NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [UserName]             NVARCHAR (256)     NULL,
    [IsTrial]			   BIT				  NULL	DEFAULT 1, 
	[Title]				   NVARCHAR (30)	  NULL,
    [FirstName]			   NVARCHAR (50)	  NULL,
	[MiddleNames]		   NVARCHAR (63)      NULL,
    [Surname]			   NVARCHAR (50)	  NULL, 
    [DateOfBirth]		   DATETIME			  NULL,
	[PhotoPath]			   NVARCHAR (255)	  NULL,
    [Photo]				   IMAGE			  NULL,
    [GenderId]			   TINYINT			  NULL	DEFAULT 1, 
    [CreatedDate]		   DATETIME		      NULL, 
    [PositionTitle]		   NVARCHAR (50)	  NULL,
	[UserTypeId]		   TINYINT			  NULL,
    [CompanyId]			   INT				  NULL,
    CONSTRAINT [PK_AspNetUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [EmailIndex]
    ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([NormalizedUserName] ASC);


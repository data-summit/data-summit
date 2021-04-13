CREATE TABLE [dbo].[AzureCompanyResourceUrls]
(
	[AzureCompanyResourceUrlId]	BIGINT					IDENTITY(1,1) NOT NULL, 
    [Name]						NVARCHAR(255)			NOT NULL, 
    [URL]						NVARCHAR(511)			NOT NULL,
	[Key]						NVARCHAR(511)			NOT NULL,
	[ResourceType]				NVARCHAR(63)			NOT NULL,
    [CompanyId]					INT						NOT NULL,
	CONSTRAINT [PK_AzureCompanyResourceUrlId] PRIMARY KEY CLUSTERED ([AzureCompanyResourceUrlId] ASC)
)
 

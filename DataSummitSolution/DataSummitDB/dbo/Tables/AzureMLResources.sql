CREATE TABLE [dbo].[AzureMLResources]
(
	[AzureMLResourcesId]		BIGINT					IDENTITY(1,1) NOT NULL, 
    [Name]						NVARCHAR(255)			NOT NULL, 
    [URL]						NVARCHAR(511)			NOT NULL,
	[TrainingKey]				NVARCHAR(511)			NOT NULL,
	[PredicitionKey]			NVARCHAR(511)			NOT NULL,
	CONSTRAINT [PK_AzureMLResourcesIdId] PRIMARY KEY CLUSTERED ([AzureMLResourcesId] ASC),
)
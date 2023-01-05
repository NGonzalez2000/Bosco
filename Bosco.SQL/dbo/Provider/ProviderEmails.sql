CREATE TABLE [dbo].[ProviderEmails]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[ProviderId] INT FOREIGN KEY REFERENCES [dbo].[Providers](Id) NOT NULL,
	[Email] NVARCHAR(150) NOT NULL
)

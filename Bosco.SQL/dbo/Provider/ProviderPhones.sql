CREATE TABLE [dbo].[ProviderPhones]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[ProviderId] INT FOREIGN KEY REFERENCES [dbo].[Providers](Id) NOT NULL,
	[PhoneNumber] NVARCHAR(150) NOT NULL
)

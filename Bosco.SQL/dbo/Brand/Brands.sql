CREATE TABLE [dbo].[Brands]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ProviderId] INT NOT NULL FOREIGN KEY REFERENCES Providers(Id),
	[Name] NVARCHAR(75) NOT NULL,
	[IsDolarValue] BIT NOT NULL
)

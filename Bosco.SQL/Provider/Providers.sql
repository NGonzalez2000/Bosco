﻿CREATE TABLE [dbo].[Providers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(150) NOT NULL UNIQUE,
	[Web] NVARCHAR(200) NOT NULL,
	[CUIT] NVARCHAR(11) NOT NULL,
	[Observation] NVARCHAR(250) NOT NULL
)
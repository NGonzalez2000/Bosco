﻿CREATE PROCEDURE [dbo].[sp_Reset_ProviderPhoneId]
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS( SELECT 1 FROM ProviderPhones)
BEGIN
	DECLARE @newId AS INT = (SELECT TOP 1 Id FROM ProviderPhones ORDER BY Id DESC)
	DBCC CHECKIDENT('ProviderPhones', RESEED, @newId);
END
ELSE
BEGIN
	DBCC CHECKIDENT('ProviderPhones', RESEED, 0);
END
   
END
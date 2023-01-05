DELETE FROM Brands
DELETE FROM Categories;
DELETE FROM ProviderEmails;
DELETE FROM ProviderPhones;
DELETE FROM ProviderAddress;
DELETE FROM Providers;

DBCC CHECKIDENT('Brands', RESEED, 0);
DBCC CHECKIDENT('Categories', RESEED, 0);
DBCC CHECKIDENT('ProviderEmails', RESEED, 0);
DBCC CHECKIDENT('ProviderPhones', RESEED, 0);
DBCC CHECKIDENT('ProviderAddress', RESEED, 0);
DBCC CHECKIDENT('Providers', RESEED, 0);


SELECT IDENT_CURRENT('Brands') AS CategoriesId;
SELECT IDENT_CURRENT('Categories') AS CategoriesId;
SELECT IDENT_CURRENT('Providers') AS ProviderId;
SELECT IDENT_CURRENT('ProviderEmails') AS ProviderEmailsId;
SELECT IDENT_CURRENT('ProviderPhones') AS ProviderPhonesId;
SELECT IDENT_CURRENT('ProviderAddress') AS ProviderAddressId;
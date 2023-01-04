using Bosco.Core.Data.Interface;
using Bosco.Core.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bosco.Core.Data.Class;

public class ProviderDb : DbContext, IProvidersDb
{
    /* || BASE PROVIDER */
    public async Task<IEnumerable<int>> Insert(ProviderModel provider)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        // declare variables and set the insert of provider
        string query = 
            $"DECLARE @output AS Table(Id int);\n" +
            $"DECLARE @newId AS INT;\n" +
            $"DECLARE @ProviderId AS INT;\n" +

            $"exec sp_Reset_ProviderEmailId;\n" +
            $"exec sp_Reset_ProviderPhoneId;\n" +
            $"exec sp_Reset_ProviderAddressId;\n" +
            $"exec sp_Reset_ProviderId;\n" +

            

            $"set xact_abort on;\n" +
            $"BEGIN TRAN T1;\n" +
            $"INSERT INTO [dbo].[Providers] VALUES('{provider.Name}','{provider.Web}','{provider.CUIT.CUIT}','{provider.Observation}');\n" +
            $"SET @newId = (SELECT SCOPE_IDENTITY());\n" +
            $"SET @ProviderId = @newId;\n" +
            $"INSERT INTO @output VALUES(@newId);\n";

        // insert Address
        query +=
            $"INSERT INTO [dbo].ProviderAddress VALUES(@ProviderId, '{provider.Address.Country}', '{provider.Address.State}', '{provider.Address.City}', " +
            $"'{provider.Address.PostalCode}', '{provider.Address.Street}', '{provider.Address.StreetNumber}');\n" +
            $"SET @newId = (SELECT SCOPE_IDENTITY());\n" +
            $"INSERT INTO @output VALUES(@newId);\n";

        // insert Emails
        foreach(EmailModel email in provider.Emails)
        {
            query +=
                $"INSERT INTO ProviderEmails VALUES(@ProviderId, '{email.Email}');\n" +
                $"SET @newId = (SELECT SCOPE_IDENTITY());\n" +
                $"INSERT INTO @output VALUES(@newId);\n";
        }

        // insert Phones
        foreach(PhoneModel phone in provider.Phones)
        {
            query +=
                $"INSERT INTO ProviderPhones VALUES(@ProviderId, '{phone.PhoneNumber}');\n" +
                $"SET @newId = (SELECT SCOPE_IDENTITY());\n" +
                $"INSERT INTO @output VALUES(@newId);\n";
        }
        // commint and ask for all inserted output
        query +=
            $"COMMIT TRAN T1;\n" +
            $"SELECT * FROM @output;";
        return await connection.QueryAsync<int>(query);
    }
    public async Task<IEnumerable<int>> Update(ProviderModel provider)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        string query =
            $"DECLARE @output as Table(Id int);\n" +
            $"DECLARE @newId as INT;\n" +
            $"DECLARE @emailIds as Table(Id int);\n" +
            $"DECLARE @phoneIds as Table(Id int);\n" +

            $"exec sp_Reset_ProviderEmailId;\n" +
            $"exec sp_Reset_ProviderPhoneId;\n" +
            
            $"SET xact_abort on;\n" +
            $"BEGIN TRAN T1;\n";
        
        query += $"UPDATE [dbo].[Providers] SET [Name] = '{provider.Name}', [Web] = '{provider.Web}', [CUIT] = '{provider.CUIT.CUIT}', [Observation] = '{provider.Observation}' WHERE Id = {provider.Id};\n";
        query +=
            $"UPDATE [dbo].[ProviderAddress] " +
            $"SET Country = '{provider.Address.Country}', " +
            $"[State] = '{provider.Address.State}', " +
            $"City = '{provider.Address.City}', " +
            $"PostalCode = '{provider.Address.PostalCode}', " +
            $"Street = '{provider.Address.Street}', " +
            $"StreetNumber = '{provider.Address.StreetNumber}' " +
            $"WHERE ProviderId = {provider.Id};\n";

        foreach(EmailModel email in provider.Emails)
        {
            // UPDATE AND INSERT news and edited mails
            
            if(email.Id != 0)
            {
                query += $"INSERT INTO @emailIds VALUES({email.Id});\n";
                query += $"UPDATE [dbo].[ProviderEmails] SET Email = '{email.Email}' WHERE ProviderId = {provider.Id} AND Id = {email.Id};\n";
            }

            else
                query += 
                    $"INSERT INTO [dbo].[ProviderEmails] VALUES({provider.Id}, '{email.Email}');\n" +
                    $"SET @newId = (SELECT SCOPE_IDENTITY());\n" +
                    $"INSERT INTO @output VALUES(@newId);\n" +
                    $"INSERT INTO @emailIds VALUES(@newId)\n";
        }
        // Remove the missing email values on the collection
        query += $"DELETE FROM ProviderEmails WHERE ProviderId = {provider.Id} AND Id NOT IN(SELECT * FROM @emailIds);\n";
        
        foreach(PhoneModel phone in provider.Phones)
        {
            //Insert the left ids on the collection
            
            if (phone.Id != 0)
            {
                query += $"INSERT INTO @phoneIds VALUES({phone.Id});\n";
                query += $"UPDATE [dbo].[ProviderPhones] SET PhoneNumber = '{phone.PhoneNumber}' WHERE ProviderId = {provider.Id} AND Id = {phone.Id};";

            }
            else
                query += 
                    $"INSERT INTO ProviderPhones VALUES({provider.Id}, '{phone.PhoneNumber}');" +
                    $"SET @newId = (SELECT SCOPE_IDENTITY());\n" +
                    $"INSERT INTO @output VALUES(@newId);\n" +
                    $"INSERT INTO @phoneIds VALUES(@newId);\n";

        }
        // Remove the missing phone values on the collection
        query += $"DELETE FROM ProviderPhones WHERE ProviderId = {provider.Id} AND Id NOT IN(SELECT * FROM @phoneIds);\n";
            
        
        query += 
            $"COMMIT TRAN T1;\n" +
            $"SELECT * FROM @output;";
        return await connection.QueryAsync<int>(query);
    }
    public async Task<IEnumerable<ProviderModel>> SelectProviders()
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        string query = "" +
            "SELECT Providers.Id, Providers.[Name], Web, Observation, CUIT, " +
            "ProviderPhones.Id, ProviderPhones.PhoneNumber, " +
            "ProviderEmails.Id, ProviderEmails. Email, " +
            "ProviderAddress.Id, ProviderAddress.Country, ProviderAddress.[State], ProviderAddress.City, ProviderAddress.PostalCode, ProviderAddress.Street, ProviderAddress.StreetNumber " +
            "FROM dbo.Providers INNER JOIN\n" +
            "dbo.ProviderPhones ON dbo.Providers.Id = dbo.ProviderPhones.ProviderId INNER JOIN\n" +
            "dbo.ProviderEmails ON dbo.Providers.Id = dbo.ProviderEmails.ProviderId INNER JOIN\n" +
            "dbo.ProviderAddress ON dbo.Providers.Id = dbo.ProviderAddress.ProviderId";
        List<ProviderModel> providers = new(await connection.QueryAsync<ProviderModel,string, PhoneModel, EmailModel, AddressModel, ProviderModel>(query, (provider,CUIT, phone, email, address) => 
        {
            provider.CUIT.CUIT = CUIT;
            provider.Address = address;
            provider.AddEmail(email);
            provider.AddPhone(phone);
            return provider;
        }, splitOn:"Id,CUIT,Id,Id,Id"));

        var temp = providers.GroupBy(p => p.Id).Select(provGroup =>
        {
            ProviderModel prov = provGroup.First();
            
            //map the rows
            prov.Emails = new(provGroup.Select(p => p.Emails.Single()));
            prov.Phones = new(provGroup.Select(p => p.Phones.Single()));

            //delete repited data
            prov.Emails = new(prov.Emails.GroupBy(e => e.Id).Select(e => e.First()));
            prov.Phones = new(prov.Phones.GroupBy(p => p.Id).Select(p => p.First()));
            return prov;
        });
        return temp;
    }

    public async Task Delete(ProviderModel provider)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        string query = $"set xact_abort on;" +
            $"BEGIN TRAN T1;" +
            $"DELETE FROM ProviderEmails WHERE ProviderId = {provider.Id};" +
            $"DELETE FROM ProviderPhones WHERE ProviderId = {provider.Id};" +
            $"DELETE FROM ProviderAddress WHERE ProviderId = {provider.Id};" +
            $"DELETE FROM Providers WHERE Id = {provider.Id};" +
            $"COMMIT TRAN T1;";
        await connection.ExecuteAsync(query);
    }
}

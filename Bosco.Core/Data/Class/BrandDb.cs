using Bosco.Core.Data.Interface;
using Bosco.Core.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Bosco.Core.Data.Class;

public class BrandDb : DbContext, IBrandDb
{
    public async Task Delete(BrandModel brand)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        await connection.ExecuteAsync($"DELETE FROM dbo.Brands WHERE Id = {brand.Id};");
    }

    public async Task<int> Insert(BrandModel brand)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        int dValue = brand.IsDolarValue ? 1 : 0; 
        string query =
            $"exec sp_Reset_BrandId;" +
            $"INSERT INTO [dbo].[Brands] OUTPUT INSERTED.Id VALUES({brand.Provider.Id}, '{brand.Name}', {dValue});";
        return await connection.QueryFirstAsync<int>(query);
    }

    public async Task<IEnumerable<BrandModel>> SelectBrands()
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        string query =
            "SELECT Brands.Id, Brands.Name, Brands.IsDolarValue, Providers.Id, Providers.Name " +
            "FROM dbo.Brands INNER JOIN dbo.Providers ON dbo.Brands.ProviderId = dbo.Providers.Id;";
        return await connection.QueryAsync<BrandModel, ProviderModel, BrandModel>(query, (brand, provider) =>
        {
            brand.Provider = provider;
            return brand;
        },
        splitOn: "Id, Id");
    }

    public async Task Update(BrandModel brand)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        int dValue = brand.IsDolarValue ? 1 : 0;
        string query =
            $"UPDATE Brands " +
            $"SET [Name] = '{brand.Name}', " +
            $"[ProviderId] = {brand.Provider.Id}, " +
            $"IsDolarValue = {dValue} " +
            $"WHERE Id = {brand.Id};";
        await connection.ExecuteAsync(query);
    }
}

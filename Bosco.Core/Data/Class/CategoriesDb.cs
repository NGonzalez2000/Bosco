using Bosco.Core.Data.Interface;
using Bosco.Core.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bosco.Core.Data.Class;

public class CategoriesDb : DbContext, ICategoriesDb
{
    public async Task<int> Insert(CategoryModel category)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        return (await connection.QueryAsync<int>("INSERT INTO [dbo].[Categories] OUTPUT INSERTED.ID values(@Name)", category)).First();
    }

    public async Task Update(CategoryModel category)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        await connection.ExecuteAsync(
            "UPDATE [dbo].[Categories] SET " +
            "[Name] = @Name " +
            "WHERE [Id] = @Id", category);
    }

    public async Task Delete(CategoryModel category)
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        await connection.ExecuteAsync(
            "DELETE FROM [dbo].[Categories] " +
            "WHERE [Id] = @Id", category);
    }
    public async Task<IEnumerable<CategoryModel>> SelectCategories()
    {
        using SqlConnection connection = new(CnnStr(Bosco.Default.ConnectionType));
        return await connection.QueryAsync<CategoryModel>("SELECT * FROM Categories");
    }
}

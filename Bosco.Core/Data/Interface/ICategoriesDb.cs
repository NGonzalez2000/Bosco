using Bosco.Core.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bosco.Core.Data.Interface;
public interface ICategoriesDb
{
    public Task<int> Insert(CategoryModel category);
    public Task Update(CategoryModel category);
    public Task Delete(CategoryModel category);
    public Task<IEnumerable<CategoryModel>> SelectCategories();
}

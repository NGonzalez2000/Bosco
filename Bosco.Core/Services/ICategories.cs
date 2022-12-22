using Bosco.Core.Models;
using System.Data;
using System.Threading.Tasks;

namespace Bosco.Core.Services;
public interface ICategories
{
    public Task<int> Insert(CategoryModel category);
}

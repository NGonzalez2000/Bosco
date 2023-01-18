using Bosco.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bosco.Core.Data.Interface;

public interface IBrandDb
{
    public Task<int> Insert(BrandModel brand);
    public Task Update(BrandModel brand);
    public Task Delete(BrandModel brand);
    public Task<IEnumerable<BrandModel>> SelectBrands();
}

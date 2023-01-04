using Bosco.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bosco.Core.Data.Interface;

public interface IProvidersDb
{
    public Task<IEnumerable<int>> Insert(ProviderModel provider);
    public Task<IEnumerable<int>> Update(ProviderModel provider);
    public Task Delete(ProviderModel provider);
    public Task<IEnumerable<ProviderModel>> SelectProviders();

}

using Bosco.Core.Services;

namespace Bosco.Core.Models.FilterModels;

public class ProviderFilterModel : IFilter
{
    public ProviderModel Provider { get; set; }
    public ProviderFilterModel()
    {
        Provider = new();
    }
    public bool Filter(object o)
    {
        return o is ProviderModel p && CuitCompare(p.CUIT.CUIT) && NameCompare(p.Name);
    }

    private bool CuitCompare(string cuit)
    {
        if (Provider.CUIT.CUIT == "00000000000") return true;
        return cuit == Provider.CUIT.CUIT;
    }
    private bool NameCompare(string name)
    {
        if(string.IsNullOrEmpty(Provider.Name)) return true;
        return name.Contains(Provider.Name);
    }
}

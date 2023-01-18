using Bosco.Core.Collections;
using Bosco.Core.Data.Interface;
using Bosco.Core.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Bosco.Core.Models.FilterModels;

public class BrandFilterModel : INotify,IFilter
{
    private readonly IProvidersDb providerdb;
    private ICollectionView collectionView;
    public BrandModel Brand { get; set; }
    public ListViewCollection<ProviderModel> Providers { get; set; }
    private string provider_SearchText;

    public string Provider_SearchText
    {
        get { return provider_SearchText; }
        set
        {
            provider_SearchText = value;
            collectionView.Refresh();
        }
    }

    public BrandFilterModel(IProvidersDb providerdb)
    {
        Brand = new();
        Providers = new();
        provider_SearchText = string.Empty;
        collectionView = CollectionViewSource.GetDefaultView(Providers);
        collectionView.Filter = ProviderName_Filter;
        this.providerdb = providerdb;
    }
    public async void Load_Providers()
    {
        try
        {
            Providers = new(await providerdb.SelectProviders());
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.GetBaseException().Message,"Brand Filter Model");
            Providers = new();
        }
        collectionView = CollectionViewSource.GetDefaultView(Providers);
        OnPropertyChanged(nameof(Providers));
    }
    public bool Filter(object o)
    {
        return o is BrandModel b && Name_Compare(b.Name) && Provider_Compare(b.Provider);
    }
    private bool Name_Compare(string name)
    {
        if(string.IsNullOrEmpty(Brand.Name)) return true;
        return name.Contains(Brand.Name);
    }
    private bool Provider_Compare(ProviderModel provider)
    {
        if (Providers.SelectedItem is null) return true;
        return Providers.SelectedItem.Id == provider.Id;
    }
    private bool ProviderName_Filter(object o)
    {
        return o is ProviderModel p && p.Name.Contains(provider_SearchText);
    }
}

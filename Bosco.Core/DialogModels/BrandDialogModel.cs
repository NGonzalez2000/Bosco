using Bosco.Core.Collections;
using Bosco.Core.Data;
using Bosco.Core.Data.Interface;
using Bosco.Core.Models;
using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Bosco.Core.DialogModels;

public class BrandDialogModel : INotify
{
    private readonly IDialog dialog;
    private readonly IBrandDb brandContext;
    private readonly IProvidersDb providerContext;
    private ICollectionView? collectionView;
    private CancellationToken token;
    private const string dialogId = "Brand_Dialog";
    private string title;
    private string content;
    private string? searchText;
    private bool closedFlag = false;
    private bool wasDeleted;
    public bool IsEnable { get; set; }
    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }
    public string Content
    {
        get => content;
        set => SetProperty(ref content, value);
    }
    public string? SearchText
    {
        get => searchText;
        set
        {
            if (SetProperty(ref searchText, value) && collectionView != null)
            {
                collectionView.Filter = Filter;
                collectionView.Refresh();
            }
        }
    } 
    public BrandModel Brand { get; set; }
    public ListViewCollection<ProviderModel> Providers { get; set; }
    public BrandDialogModel(IDialog dialog,IBrandDb dbBrands , IProvidersDb dbProviders)
    {
        title = string.Empty;
        content = string.Empty;
        searchText = string.Empty;
        brandContext = dbBrands;
        providerContext = dbProviders;
        Brand = new();
        Providers = new();
        IsEnable = true;
        this.dialog = dialog;
        dialog.SetDataContext(this);
    }

    private void SelectionChanged(ProviderModel? provider)
    {
        if (provider == null) Brand.Provider = new();
        else Brand.Provider = provider.DeepCopy();
    }
    public async Task<BrandModel> NewBrand(CancellationToken token)
    {
        this.token = token;

        Title = "Nueva Marca";
        Content = "CREAR";

        Brand = new();
        OnPropertyChanged(nameof(Brand));

        try
        {
            Providers = new(await providerContext.SelectProviders())
            {
                OnSelectionChanged = SelectionChanged
            };
        }
        catch (Exception ex)
        {
            Providers = new();
            MessageBox.Show("No se han podido cargar los proveedores.\n\n" + ex.GetBaseException().Message);
        }


        collectionView = CollectionViewSource.GetDefaultView(Providers);

        OnPropertyChanged(nameof(Providers));

        await DialogHost.Show(dialog, dialogId, closingEventHandler: ClosingEventHandler_New);

        return Brand;
    }
    public async Task<BrandModel> EditBrand(CancellationToken token, BrandModel brand)
    {
        this.token = token;

        Title = "Editar Marca";
        Content = "EDITAR";

        Brand = brand.DeepCopy();
        OnPropertyChanged(nameof(Brand));
        try
        {
            Providers = new(await providerContext.SelectProviders());
            Providers.SelectedItem = Providers.Where(p => p.Id == Brand.Provider.Id).Single();
            Providers.OnSelectionChanged = SelectionChanged;
        }
        catch (Exception ex)
        {
            Providers = new();
            MessageBox.Show("No se han podido cargar los proveedores.\n\n" + ex.GetBaseException().Message);
        }

        collectionView = CollectionViewSource.GetDefaultView(Providers);
        OnPropertyChanged(nameof(Providers));

        await DialogHost.Show(dialog!, dialogId, closingEventHandler: ClosingEventHandler_Edit);

        return Brand;

    }
    public async Task<bool> DeleteBrand(CancellationToken token, BrandModel brand)
    {
        this.token = token;

        Title = "Eliminar Marca";
        Content = "ELIMINAR";

        Brand = brand.DeepCopy();
        OnPropertyChanged(nameof(Brand));
        Providers = new()
        {
            Brand.Provider
        };
        Providers.SelectedItem = Providers[0];
        OnPropertyChanged(nameof(Providers));

        IsEnable = false;
        OnPropertyChanged(nameof(IsEnable));



        await DialogHost.Show(dialog!, dialogId, closingEventHandler: ClosingEventHandler_Delete);

        return wasDeleted;
    }
    public async void ClosingEventHandler_New(object sender, DialogClosingEventArgs eventArgs)
    {
        if (eventArgs.Parameter is bool parameter &&
                parameter == false || closedFlag || token.IsCancellationRequested) return;

        eventArgs.Cancel();

        if (Brand["Name"] != string.Empty)
        {
            Brand.Error = Brand["Name"];
            return;
        }
        if (Brand["Provider"] != string.Empty)
        {
            Brand.Error = Brand["Provider"];
            return;
        }

        try
        {
            Brand.Id = await brandContext.Insert(Brand);

        }
        catch (Exception ex)
        {
            Brand.Error = ex.GetBaseException().Message;
            return;
        }

        closedFlag = true;
        eventArgs.Session.Close();
    }
    public async void ClosingEventHandler_Edit(object sender, DialogClosingEventArgs eventArgs)
    {

        if (closedFlag) return;
        if (eventArgs.Parameter is bool parameter &&
                parameter == false || token.IsCancellationRequested)
        {
            Brand.Id = 0;
            return;
        }

        eventArgs.Cancel();

        if (Brand["Name"] != string.Empty)
        {
            Brand.Error = Brand["Name"];
            return;
        }

        if (Brand["Provider"] != string.Empty)
        {
            Brand.Error = Brand["Provider"];
            return;
        }

        try
        {
            await brandContext.Update(Brand);
        }
        catch (Exception ex)
        {
            Brand.Error = ex.GetBaseException().Message;
            return;
        }
        closedFlag = true;
        eventArgs.Session.Close();
    }
    public async void ClosingEventHandler_Delete(object sender, DialogClosingEventArgs eventArgs)
    {
        if (closedFlag) return;
        if (eventArgs.Parameter is bool parameter &&
                parameter == false || token.IsCancellationRequested)
        {
            wasDeleted = false;
            return;
        }

        eventArgs.Cancel();


        try
        {
            await brandContext.Delete(Brand);
        }
        catch (Exception ex)
        {
            Brand.Error = ex.GetBaseException().Message;
            wasDeleted = false;
            return;
        }

        wasDeleted = true;

        closedFlag = true;
        eventArgs.Session.Close();
    }
    private bool Filter(object o)
    {
        if (string.IsNullOrEmpty(SearchText)) return true;
        return o is ProviderModel p && p.Name.Contains(SearchText);
    }
}

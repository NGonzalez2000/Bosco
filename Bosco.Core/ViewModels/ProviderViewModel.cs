using Bosco.Core.Collections;
using Bosco.Core.Data.Class;
using Bosco.Core.Data.Interface;
using Bosco.Core.DialogModels;
using Bosco.Core.Models;
using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Bosco.Core.ViewModels;

public class ProviderViewModel : INotify, IViewModel
{
    private IDialog? dialog;
    private readonly IProvidersDb dbContext;
    private CancellationTokenSource? tokenSource;
    public ICommand NewProvider_Command => new RelayCommand(_ => NewProvider_Execute());
    public ICommand EditProvider_Command => new RelayCommand(_ => EditProvider_Execute());
    public ICommand DeleteProvider_Command => new RelayCommand(_ => DeleteProvider_Execute());
    public ListViewCollection<ProviderModel> Providers { get; set; }
    public ProviderViewModel(IProvidersDb dbContext)
    {
        this.dbContext = dbContext;
        Providers = new();
    }
    public async void Opening()
    {
        try
        {
            Providers = new(await dbContext.SelectProviders());
        }
        catch (System.Exception ex)
        {
            Providers = new();
            MessageBox.Show(ex.GetBaseException().Message);
        }
        OnPropertyChanged(nameof(Providers));
    }
    public void Closing()
    {
        if (tokenSource is not null)
        {
            try
            {
                tokenSource.Cancel();
            }
            catch (Exception)
            {

            }
            if (DialogHost.IsDialogOpen("Provider_Dialog"))
                DialogHost.Close("Provider_Dialog");

        }
        Providers.SelectedItem = null;
        Providers = new();
    }
    public void SetDialog(IDialog dialog) => this.dialog = dialog;
    private async void NewProvider_Execute()
    {
        ProviderDialogModel dialogModel = new(dialog!,dbContext);

        tokenSource = new();

        ProviderModel provider = await dialogModel.NewProvider(tokenSource.Token);

        if(provider.Id != 0)
            Providers.Add(provider);

        tokenSource.Dispose();
    }
    private async void EditProvider_Execute()
    {
        if(Providers.SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un proveedor");
            return;
        }
        ProviderDialogModel dialogModel = new(dialog!, dbContext);

        tokenSource = new();

        ProviderModel provider = await dialogModel.EditProvider(tokenSource.Token, Providers.SelectedItem);

        if (provider.Id == -1) return;

        Providers.Edit(provider);
    }
    private async void DeleteProvider_Execute()
    { 
        if(Providers.SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar un proveedor");
            return;
        }
        ProviderDialogModel dialogModel = new(dialog!, dbContext);

        tokenSource = new();

        bool wasDeleted = await dialogModel.DeleteProvider(tokenSource.Token, Providers.SelectedItem);

        if (wasDeleted)
        {
            Providers.Remove(Providers.SelectedItem);
        }
    }
}

using Bosco.Core.Collections;
using Bosco.Core.Data.Interface;
using Bosco.Core.DialogModels;
using Bosco.Core.Models;
using Bosco.Core.Models.FilterModels;
using Bosco.Core.Models.SortModels;
using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Bosco.Core.ViewModels;

public class BrandViewModel : INotify, IViewModel
{
    private readonly IBrandDb brandDb;
    private readonly IProvidersDb providersDb;
    private CancellationTokenSource? tokenSource;
    private IDialog? dialog;
    public BrandFilterModel BrandFilter { get; set; }
    public BrandSortModel BrandSort { get; set; }
    public ListViewCollection<BrandModel> Brands { get; set; }
    public ICommand NewBrand_Command => new RelayCommand(_ => NewBrand_Execute());
    public ICommand EditBrand_Command => new RelayCommand(_ => EditBrand_Execute());
    public ICommand DeleteBrand_Command => new RelayCommand(_ => DeleteBrand_Execute());
    public ICommand FilterBrands_Command => new RelayCommand(_ => FilterBrands_Execute());
    public ICommand SortBrands_Command => new RelayCommand(_ => SortBrands_Execute());
    public BrandViewModel(IBrandDb brandDb, IProvidersDb providersDb)
    {
        this.brandDb = brandDb;
        this.providersDb = providersDb;
        BrandFilter = new(providersDb);
        BrandSort = new();
        Brands = new();
    }

    private async void NewBrand_Execute()
    {
        BrandDialogModel dialogModel = new(dialog!, brandDb, providersDb);

        tokenSource = new();

        BrandModel brand = await dialogModel.NewBrand(tokenSource.Token);

        tokenSource.Dispose();

        if (brand.Id != 0)
            Brands.Add(brand);

    }
    private async void EditBrand_Execute()
    {
        if (Brands.SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar una marca.");
            return;
        }

        BrandDialogModel dialogModel = new(dialog!, brandDb, providersDb);

        tokenSource = new();

        BrandModel brand = await dialogModel.EditBrand(tokenSource.Token, Brands.SelectedItem);

        tokenSource.Dispose();

        if(brand.Id != 0)
            Brands.Edit(brand);
    }
    private async void DeleteBrand_Execute()
    {
        if(Brands.SelectedItem is null)
        {
            MessageBox.Show("Debe seleccionar una marca.");
            return;
        }
        BrandDialogModel dialogModel = new(dialog!, brandDb, providersDb);

        tokenSource = new();

        if (await dialogModel.DeleteBrand(tokenSource.Token, Brands.SelectedItem))
        {
            Brands.Remove(Brands.SelectedItem);
        }
    }
    private void FilterBrands_Execute()
    {
        RefreshView();
    }
    private void SortBrands_Execute()
    {
        Brands.SetSortDescription(BrandSort.Sort());
        RefreshView();
    }
    private void RefreshView()
    {
        Brands.Refresh();
    }
    public async void Opening()
    {
        BrandFilter.Load_Providers();

        try
        {
            Brands = new(await brandDb.SelectBrands());
            Brands.SetFilter(BrandFilter.Filter);
            Brands.SetSortDescription(BrandSort.Sort());
            RefreshView();
        }
        catch (System.Exception ex)
        {
            Brands = new();
            MessageBox.Show(ex.GetBaseException().Message);
        }
        

        OnPropertyChanged(nameof(Brands));
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
            if (DialogHost.IsDialogOpen("Brand_Dialog"))
                DialogHost.Close("Brand_Dialog");

        }

        Brands.SelectedItem = null;
        Brands = new();
    }

    public void SetDialog(IDialog dialogType)
    {
        dialog = dialogType;
    }
}

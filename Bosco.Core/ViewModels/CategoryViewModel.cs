using Bosco.Core.Collections;
using Bosco.Core.Data.Interface;
using Bosco.Core.DialogModels;
using Bosco.Core.Models;
using Bosco.Core.Models.FilterModels;
using Bosco.Core.Models.SortModels;
using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Bosco.Core.ViewModels;

public class CategoryViewModel : INotify, IViewModel
{
	private IDialog? dialogType;
    private readonly ICategoriesDb dbcontext;
	private CancellationTokenSource? tokenSource;
    public ListViewCollection<CategoryModel> Categories { get; set; }
	public CategoryFilterModel CategoriesFilter { get; set; }
	public CategorySortModel CategoriesSort { get; set; }
	public ICommand NewCategory_Command => new RelayCommand(_ => NewCategory_Execute());
	public ICommand EditCategory_Command => new RelayCommand(_ => EditCategory_Execute());
	public ICommand DeleteCategory_Command => new RelayCommand(_ => DeleteCategory_Execute());
	public ICommand FilterCategories_Command => new RelayCommand(_ => FilterCategories_Execute());
	public ICommand SortCategories_Command => new RelayCommand(_ => SortCategories_Execute());
	public CategoryViewModel(ICategoriesDb dbcontext)
	{
        this.dbcontext = dbcontext;
		Categories = new();
		CategoriesFilter = new();
		CategoriesSort = new();
    }
	public async void Opening()
	{
		try
		{
			Categories = new(await dbcontext.SelectCategories());
            Categories.SetFilter(CategoriesFilter.Filter);
			Categories.SetSortDescription(CategoriesSort.Sort());
			RefreshView();
        }
        catch (Exception ex)
		{
			Categories = new();

			MessageBox.Show(ex.GetBaseException().Message);

		}
		OnPropertyChanged(nameof(Categories));


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
			if(DialogHost.IsDialogOpen("Category_Dialog"))
				DialogHost.Close("Category_Dialog");

        }
		Categories.SelectedItem = null;
		Categories = new();
	}
	public void SetDialog(IDialog dialogType)
	{
		this.dialogType = dialogType;
	}
	private async void NewCategory_Execute()
	{
		CategoryDialogModel dialogModel = new(dialogType!,dbcontext);

		tokenSource = new();

		CategoryModel category = await dialogModel.NewCategory(tokenSource.Token);


		if (category.Id == 0) return;

		Categories.Add(category);
		OnPropertyChanged(nameof(Categories));

        tokenSource.Dispose();
    }
	private async void EditCategory_Execute()
	{
		if (Categories.SelectedItem is null)
		{
			MessageBox.Show("Debe seleccionar una categoría");
			return;
		}

		CategoryDialogModel dialogModel = new(dialogType!, dbcontext);

		tokenSource = new();

		CategoryModel category = await dialogModel.EditCategory(tokenSource.Token, Categories.SelectedItem!);

		tokenSource.Dispose();

		if (category.Id == 0) return;

		Categories.Edit(category);
	}
	private async void DeleteCategory_Execute()
	{
		if(Categories.SelectedItem is null)
		{
			MessageBox.Show("Debe seleccionar una categoría.");
			return;
		}
		CategoryDialogModel dialogModel = new(dialogType!, dbcontext);

		tokenSource = new();

		bool response = await dialogModel.DeleteCategory(tokenSource.Token, Categories.SelectedItem);

		if (response) Categories.Remove(Categories.SelectedItem);

	}
	private void FilterCategories_Execute()
	{
		RefreshView();
	}
	private void SortCategories_Execute()
	{
		Categories.SetSortDescription(CategoriesSort.Sort());
		RefreshView();
		OnPropertyChanged(nameof(Categories));
	}
	private void RefreshView() => Categories.Refresh();
}

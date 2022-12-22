using Bosco.Core.Collections;
using Bosco.Core.Data;
using Bosco.Core.DialogModels;
using Bosco.Core.Models;
using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Bosco.Core.ViewModels;

public class CategoryViewModel : INotify
{
	private IDialog? dialogType;
    private readonly CategoriesDb dbcontext;
	private CancellationTokenSource? tokenSource;
    public ListViewCollection<CategoryModel> Categories { get; set; }
	public ICommand NewCategory_Command => new RelayCommand(_ => NewCategory_Execute());
	public ICommand EditCategory_Command => new RelayCommand(_ => EditCategory_Execute());
	public ICommand DeleteCategory_Command => new RelayCommand(_ => DeleteCategory_Execute());
	public CategoryViewModel(CategoriesDb dbcontext)
	{
        this.dbcontext = dbcontext;
		Categories = new();
    }
	public async void Load()
	{
		try
		{
			Categories = new(await dbcontext.SelectCategories());
		}
		catch (Exception ex)
		{
			Categories = new();

			MessageBox.Show(ex.GetBaseException().Message);

		}
		OnPropertyChanged(nameof(Categories));


	}
	public void Clear()
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
}

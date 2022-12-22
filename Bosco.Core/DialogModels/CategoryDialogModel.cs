using Bosco.Core.Data;
using Bosco.Core.Models;
using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bosco.Core.DialogModels;

public class CategoryDialogModel : INotify
{
	private bool closedFlag = false;
    private readonly IDialog dialog;
    private readonly CategoriesDb dbcontext;
	private CancellationToken token;
	private const string dialogId = "Category_Dialog";
	private string title;
	private string content;
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
	public CategoryModel Category { get; set; }
	public CategoryDialogModel(IDialog dialog, CategoriesDb dbcontext)
	{
		Category = new();
		OnPropertyChanged();
		title = string.Empty;
		content = string.Empty;
        this.dialog = dialog;
        this.dbcontext = dbcontext;
        dialog.SetDataContext(this);
		IsEnable = true;
    }
	public async Task<CategoryModel> NewCategory(CancellationToken token)
	{
		this.token = token;

        IsEnable = true;
        OnPropertyChanged(nameof(IsEnable));

        Title = "Nueva Categoría";
		Content = "CREAR";

		await DialogHost.Show(dialog, dialogId, closingEventHandler: ClosingEventHandler_New);

		return Category;
	}
	public async Task<CategoryModel> EditCategory(CancellationToken token, CategoryModel category)
	{
		Category = category.ShallowCopy();
		OnPropertyChanged(nameof(Category));
		IsEnable = true;
        OnPropertyChanged(nameof(IsEnable));

        Category.ChechPropertyErrors();

        Title = "Editar Categoría";
		Content = "EDITAR";

		this.token = token;

		await DialogHost.Show(dialog, dialogId, closingEventHandler: ClosingEventHandler_Edit);

		return Category;
	}
	public async Task<bool> DeleteCategory(CancellationToken token, CategoryModel category)
	{
		Category = category.ShallowCopy();
		OnPropertyChanged(nameof(Category));

		IsEnable = false;
		OnPropertyChanged(nameof(IsEnable));

        Category.ChechPropertyErrors();


        Title = "Eliminar Categoría";
		Content = "ELIMINAR";

		await DialogHost.Show(dialog, dialogId, closingEventHandler: ClosingEventHandler_Delete);

		return wasDeleted;
	}
    public async void ClosingEventHandler_New(object sender, DialogClosingEventArgs eventArgs)
    {
        if (eventArgs.Parameter is bool parameter &&
                parameter == false || closedFlag || token.IsCancellationRequested) return;
        
		eventArgs.Cancel();

		if (Category["Name"] != string.Empty)
		{
			Category.Error = Category["Name"];
			return;
		}

		try
		{
			Category.Id = await dbcontext.Insert(Category);

		}
		catch (Exception ex)
		{
			Category.Error = ex.GetBaseException().Message;
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
			Category.Id = 0;
			return;
		}

        eventArgs.Cancel();

        if (Category["Name"] != string.Empty)
        {
            Category.Error = Category["Name"];
            return;
        }

		try
		{
			await dbcontext.Update(Category);
		}
		catch (Exception ex)
		{
			Category.Error = ex.GetBaseException().Message;
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
            await dbcontext.Delete(Category);
        }
        catch (Exception ex)
        {
            Category.Error = ex.GetBaseException().Message;
			wasDeleted = false;
            return;
        }

		wasDeleted = true;

        closedFlag = true;
        eventArgs.Session.Close();
    }
}

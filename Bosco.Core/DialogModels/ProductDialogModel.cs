using Bosco.Core.Collections;
using Bosco.Core.Data.Interface;
using Bosco.Core.Models;
using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Bosco.Core.DialogModels;

public class ProductDialogModel : INotify
{
	private readonly IProvidersDb _providersDb;
	private readonly IBrandDb _brandDb;
    private readonly IDialog dialog;
    private const string dialogId = "Product_Dialog";
    private CancellationToken token;
    private string title;
    private string content;

    public string Title
    {
        get { return title; }
        set { SetProperty(ref title, value); }
    }
    public string Content
    {
        get => content;
        set => SetProperty(ref content, value);
    }
    public ListViewCollection<ProviderModel> Providers { get; set; }
    public ListViewCollection<BrandModel> Brands { get; set; }
    public ProductModel Product { get; set; }
	public bool IsEnable { get; set; }
    public ProductDialogModel(IProvidersDb providersDb, IBrandDb brandDb, IDialog dialog)
    {
        _providersDb = providersDb;
        _brandDb = brandDb;
        Product = new();
        Providers = new();
        Brands = new();
        title = string.Empty;
        content = string.Empty;
        this.dialog = dialog;
        this.dialog.SetDataContext(this);
    }
    public async Task<ProductModel> NewProduct(CancellationToken token)
    {
        Title = "Nuevo Producto";
        Content = "CREAR";

        this.token = token;

        try
        {
            Providers = new(await _providersDb.SelectProviders());
            Brands = new(await _brandDb.SelectBrands());
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.GetBaseException().Message);
            return Product;
        }

        await DialogHost.Show(dialog, dialogId);
        return Product;
    }
}

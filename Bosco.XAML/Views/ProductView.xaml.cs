using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace Bosco.XAML.Views;
public partial class ProductView : UserControl, IView
{
    public ProductView()
    {
        InitializeComponent();
    }


    public string ButtonDisplay => "Productos";
    public PackIconKind Icon => PackIconKind.Package;
    public void Load()
    {
        
    }
    public void Clear()
    {

    }
}

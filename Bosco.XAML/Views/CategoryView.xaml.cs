using Bosco.Core.Services;
using Bosco.Core.ViewModels;
using Bosco.XAML.Dialogs;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace Bosco.XAML.Views;

public partial class CategoryView : UserControl, IView
{
    public CategoryView(CategoryViewModel dataContext)
    {
        InitializeComponent();
        dataContext.SetDialog(new CategoryDialog());
        DataContext = dataContext;
    }

    public string ButtonDisplay => "Categorías";
    public PackIconKind Icon => PackIconKind.Category; 
    public void Load()
    {
        if(DataContext is IViewModel viewModel)
        {
            viewModel.Opening();
        }
    }
    public void Clear()
    {
        if(DataContext is IViewModel viewModel)
        {
            viewModel.Closing();
        }
    }

    
}

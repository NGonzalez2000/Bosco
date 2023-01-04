using Bosco.Core.Services;
using Bosco.Core.ViewModels;
using Bosco.XAML.Dialogs;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace Bosco.XAML.Views;

public partial class ProviderView : UserControl, IView
{
    public ProviderView(ProviderViewModel dataContext)
    {
        InitializeComponent();
        dataContext.SetDialog(new ProviderDialog());
        DataContext = dataContext;
    }
    public string ButtonDisplay => "Proveedores";

    public PackIconKind Icon => PackIconKind.BriefcaseAccount;

    public void Load()
    {
        if (DataContext is IViewModel viewModel)
        {
            viewModel.Opening();
        }
    }
    public void Clear()
    {
        if (DataContext is IViewModel viewModel)
        {
            viewModel.Closing();
        }
    }

}

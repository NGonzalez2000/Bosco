using Bosco.Core.Services;
using Bosco.Core.ViewModels;
using Bosco.XAML.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Controls;

namespace Bosco.XAML.Views;

public partial class BrandView : UserControl, IView
{
    public BrandView(BrandViewModel dataContext)
    {
        InitializeComponent();
        dataContext.SetDialog(new BrandDialog());
        DataContext = dataContext;
    }

    public string ButtonDisplay => "Marcas";

    public PackIconKind Icon => PackIconKind.CrownOutline;

    public void Clear()
    {
        if (DataContext is IViewModel viewModel)
        {
            viewModel.Closing();
        }
    }

    public void Load()
    {
        if (DataContext is IViewModel viewModel)
        {
            viewModel.Opening();
        }
    }
}

using Bosco.Core.Services;
using Bosco.Core.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Bosco.XAML.Views;
public partial class ProductView : UserControl, IView
{
    public ProductView()
    {
        InitializeComponent();
    }

    string IView.ButtonDisplay { get => "Productos"; }

    void IView.Load()
    {
        MessageBox.Show("I just load.");
    }
}

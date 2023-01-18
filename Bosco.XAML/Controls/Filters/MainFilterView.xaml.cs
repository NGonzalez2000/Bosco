using Bosco.Core.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bosco.XAML.Controls.Filters;
public partial class MainFilterView : UserControl
{


    public UserControl FilterContent
    {
        get { return (UserControl)GetValue(FilterContentProperty); }
        set { SetValue(FilterContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FilterContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterContentProperty =
        DependencyProperty.Register("FilterContent", typeof(UserControl), typeof(MainFilterView), new PropertyMetadata(null));





    public ICommand FilterCommand
    {
        get { return (ICommand)GetValue(FilterCommandProperty); }
        set { SetValue(FilterCommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCommandProperty =
        DependencyProperty.Register("FilterCommand", typeof(ICommand), typeof(MainFilterView), new PropertyMetadata(new RelayCommand(_ => { })));





    public MainFilterView()
    {
        InitializeComponent();
    }
}

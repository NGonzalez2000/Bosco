using Bosco.Core.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bosco.XAML.Controls.Sorts;

public partial class MainSortView : UserControl
{


    public ICommand SortCommand
    {
        get { return (ICommand)GetValue(SortCommandProperty); }
        set { SetValue(SortCommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(MainSortView), new PropertyMetadata(new RelayCommand(_ => { })));



    public UserControl SortContent
    {
        get { return (UserControl)GetValue(SortContentProperty); }
        set { SetValue(SortContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SortContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortContentProperty =
        DependencyProperty.Register("SortContent", typeof(UserControl), typeof(MainSortView), new PropertyMetadata(null));




    public MainSortView()
    {
        InitializeComponent();
    }
}

using Bosco.Core.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bosco.XAML.Controls;

public partial class PhonesControl : UserControl
{
    public ICommand AddPhone_Command
    {
        get { return (ICommand)GetValue(AddPhone_CommandProperty); }
        set { SetValue(AddPhone_CommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AddEmail_Command.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddPhone_CommandProperty =
        DependencyProperty.Register("AddPhone_Command", typeof(ICommand), typeof(PhonesControl), new PropertyMetadata(new RelayCommand(_ => { })));


    public ICommand DeletePhone_Command
    {
        get { return (ICommand)GetValue(DeletePhone_CommandProperty); }
        set { SetValue(DeletePhone_CommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DeleteEmail_Command.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeletePhone_CommandProperty =
        DependencyProperty.Register("DeletePhone_Command", typeof(ICommand), typeof(PhonesControl), new PropertyMetadata(new RelayCommand(_ => { })));
    public PhonesControl()
    {
        InitializeComponent();
    }
}

using Bosco.Core.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bosco.XAML.Controls;
public partial class EmailsControl : UserControl
{
    public ICommand AddEmail_Command
    {
        get { return (ICommand)GetValue(AddEmail_CommandProperty); }
        set { SetValue(AddEmail_CommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AddEmail_Command.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddEmail_CommandProperty =
        DependencyProperty.Register("AddEmail_Command", typeof(ICommand), typeof(EmailsControl), new PropertyMetadata(new RelayCommand(_ => { })));


    public ICommand DeleteEmail_Command
    {
        get { return (ICommand)GetValue(DeleteEmail_CommandProperty); }
        set { SetValue(DeleteEmail_CommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DeleteEmail_Command.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeleteEmail_CommandProperty =
        DependencyProperty.Register("DeleteEmail_Command", typeof(ICommand), typeof(EmailsControl), new PropertyMetadata(new RelayCommand(_ => { })));


    public EmailsControl()
    {
        InitializeComponent();
    }
}

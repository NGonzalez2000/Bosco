using Bosco.Core.Services;
using System.Windows.Controls;

namespace Bosco.XAML.Dialogs;

/// <summary>
/// Interaction logic for CategoryDialog.xaml
/// </summary>
public partial class CategoryDialog : UserControl, IDialog
{
    public CategoryDialog()
    {
        InitializeComponent();
    }

    public void SetDataContext(object dataContext)
    {
        DataContext = dataContext;
    }
}

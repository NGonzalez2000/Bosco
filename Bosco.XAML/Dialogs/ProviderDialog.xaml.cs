using Bosco.Core.Services;
using System.Windows.Controls;

namespace Bosco.XAML.Dialogs;

public partial class ProviderDialog : UserControl, IDialog
{
    public ProviderDialog()
    {
        InitializeComponent();
    }

    public void SetDataContext(object dataContext)
    {
        DataContext = dataContext;
    }
}

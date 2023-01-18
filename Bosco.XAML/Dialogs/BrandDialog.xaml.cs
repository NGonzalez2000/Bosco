using Bosco.Core.Services;
using System.Windows.Controls;

namespace Bosco.XAML.Dialogs;

public partial class BrandDialog : UserControl, IDialog
{
    public BrandDialog()
    {
        InitializeComponent();
    }

    public void SetDataContext(object dataContext)
    {
        DataContext = dataContext;
    }

    private void CbProvider_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
    {
        if (sender is ComboBox cbo)
        {
            if (cbo.Template.FindName("PART_EditableTextBox", cbo) is TextBox txt)
            {
                txt.CharacterCasing = CharacterCasing.Upper;
            }
        }
    }
}

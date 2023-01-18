using System.Windows.Controls;

namespace Bosco.XAML.Controls.Filters;

public partial class BrandFilter : UserControl
{
    public BrandFilter()
    {
        InitializeComponent();
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

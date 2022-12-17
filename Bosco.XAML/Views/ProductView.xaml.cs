using System.Windows;

namespace Bosco.XAML.Views;
public partial class ProductView : BaseUserControl
{
    public ProductView()
    {
        InitializeComponent();
        ButtonDisplay = "Productos";
    }
    public override void Load()
    {
        MessageBox.Show("I'm loading");
        MessageBox.Show(ButtonDisplay);
    }
}

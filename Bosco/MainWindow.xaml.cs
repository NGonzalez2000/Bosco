using System.Windows;

namespace Bosco;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AddDynamicResources();
    }

    private static void AddDynamicResources()
    {
        Application.Current.Resources["HeaderFontSize"] = 21d;
        Application.Current.Resources["SecondaryHeaderFontSize"] = 19d;
        Application.Current.Resources["ButtonFontSize"] = 17d;
        Application.Current.Resources["GeneralFontSize"] = 15d;
    }
}

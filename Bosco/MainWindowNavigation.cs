using Bosco.XAML.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bosco
{
    public class MainWindowNavigation
    {
        List<BaseUserControl> Views;
        private BaseUserControl? selectedView;
        public BaseUserControl? SelectedView
        {
            get => selectedView;
            set
            {
                if (value is not null && value != selectedView)
                {
                    selectedView = value;
                    selectedView.Load();
                }
            }
        }
        public MainWindowNavigation()
        {
            Views = new()
            {
                new ProductView()
            };
            selectedView = null;
            Test();
        }
        private async void Test()
        {
            await Task.Delay(1000);
            SelectedView = Views.First();
        }
    }
}

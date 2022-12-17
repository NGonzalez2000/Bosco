using Bosco.Core.Services;
using Bosco.XAML.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bosco
{
    public class MainWindowNavigation
    {
        List<IView> Views;
        private IView? selectedView;
        private readonly IFrontendNotifier frontendNotifier;

        public IView? SelectedView
        {
            get => selectedView;
            set
            {
                if (value is not null && value != selectedView)
                {
                    frontendNotifier.SetProperty(ref selectedView, value);
                    selectedView!.Load();
                }
            }
        }
        public MainWindowNavigation(IEnumerable<IView> views,IFrontendNotifier frontendNotifier)
        {
            Views = new(views);
            selectedView = null;
            Test();
            this.frontendNotifier = frontendNotifier;
        }
        private async void Test()
        {
            await Task.Delay(1000);
            SelectedView = Views.First();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bosco.XAML.Views
{
    public interface IView
    {
        public void Load();
        public string ButtonDisplay { get; }
    }
}

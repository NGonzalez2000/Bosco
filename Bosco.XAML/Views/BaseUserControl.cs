using System.Security.Policy;
using System.Windows.Controls;

namespace Bosco.XAML.Views;

public partial class BaseUserControl : UserControl
{
	public virtual void Load() { }
	public string ButtonDisplay { get; protected set; } = string.Empty;
}

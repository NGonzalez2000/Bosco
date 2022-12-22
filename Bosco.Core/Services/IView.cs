using MaterialDesignThemes.Wpf;

namespace Bosco.Core.Services;

public interface IView
{
    public void Load();
    public void Clear();
    public string ButtonDisplay { get; }
    public PackIconKind Icon { get; }
}

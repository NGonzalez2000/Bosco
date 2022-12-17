using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bosco.Core.Services;

public interface IFrontendNotifier : INotifyPropertyChanged
{
    public bool SetProperty<T>(ref T member, T value, [CallerMemberName] string? propertyName = null);
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null);
}

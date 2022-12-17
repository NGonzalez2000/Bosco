using System.Collections.Generic;
using System.ComponentModel;

namespace Bosco.Core.Services;

public class FrontendNotifier : IFrontendNotifier
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool SetProperty<T>(ref T member, T value, string? propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(member, value)) return false;

        member = value;
        OnPropertyChanged(propertyName);
        return true;
    }

}

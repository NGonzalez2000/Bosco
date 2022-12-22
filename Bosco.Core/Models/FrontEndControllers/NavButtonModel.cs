using MaterialDesignThemes.Wpf;

namespace Bosco.Core.Models.FrontEndControllers;

public class NavButtonModel
{
    public string Name { get; set; }
    public PackIconKind Icon { get; set; }
    public bool IsSelected { get; set; }
    public NavButtonModel(string name, PackIconKind icon, bool isSelected)
    {
        Name = name;
        Icon = icon;
        IsSelected = isSelected;
    }
}

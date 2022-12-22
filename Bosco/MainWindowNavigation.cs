using Bosco.Core.Collections;
using Bosco.Core.Models.FrontEndControllers;
using Bosco.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bosco;

public class MainWindowNavigation
{
    public ViewCollection Views { get; set; }
    public List<NavButtonModel> ViewNames { get; set; }
    public ICommand Nav_Command => new RelayCommand(Nav_Execute);
    public MainWindowNavigation(IEnumerable<IView> views)
    {
        Views = new(views);
        ViewNames = new();
        foreach(IView view in Views)
        {
            ViewNames.Add(new(view.ButtonDisplay,view.Icon,false));
        }
    }

    private void Nav_Execute(object? param)
    {
        if (param is null)
        {
            MessageBox.Show("No se ha podido cargar la vista seleccionada.");
            return;
        }

        if(param is NavButtonModel button)
        {
            Views.SelectViewByIndex(ViewNames.IndexOf(button));
        }
        
    } 
    
    
    
}

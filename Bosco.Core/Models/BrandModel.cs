using Bosco.Core.Services;
using System;
using System.ComponentModel;

namespace Bosco.Core.Models;

public class BrandModel : INotify, IDataErrorInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDolarValue { get; set; }
    public ProviderModel Provider { get; set; }

    private string error;
    public string Error
    {
        get => error;
        set => SetProperty(ref error, value);
    } 

    public string this[string columnName]
    {
        get
        {
            if(columnName == nameof(Name))
            {
                if (string.IsNullOrEmpty(Name))
                {
                    Error = "Debe asignar un nombre";
                    return "Campo obligatorio";
                }
            }
            if(columnName == nameof(Provider))
            {
                if(Provider.Id == 0)
                {
                    Error = "La marca necesita un proveedor";
                    return "Seleccione una marca";
                }
            }
            return string.Empty;
        }
    }

    public BrandModel()
    {
        Name = string.Empty;
        error = string.Empty;
        Provider = new();
    }

    public BrandModel DeepCopy()
    {
        BrandModel model = new()
        {
            Id = Id,
            Name = Name,
            Provider = Provider.DeepCopy(),
            IsDolarValue = IsDolarValue
        };
        return model; 
    }
}

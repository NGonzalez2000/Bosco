using Bosco.Core.Services;
using System.ComponentModel;

namespace Bosco.Core.Models;

public class CategoryModel : INotify, IDataErrorInfo
{
    public int Id { get; set; }
    public string Name { get; set; }

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
            if (columnName == nameof(Name) && string.IsNullOrEmpty(Name)) return "Campo obligatorio.";
            if (columnName == nameof(Error))return Error;
            return string.Empty;
        }
    }

    public CategoryModel()
    {
        Name = string.Empty;
        error = string.Empty;
    }
    public CategoryModel ShallowCopy()
    {
        CategoryModel? ret = (CategoryModel?)MemberwiseClone();
        if (ret is null) return new();
        return ret;
    }
    public void ChechPropertyErrors()
    {
        Error = this[nameof(Name)];
    }
}

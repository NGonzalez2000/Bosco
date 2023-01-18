using Bosco.Core.Services;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bosco.Core.Models.SortModels;

public class CategorySortModel : ISort
{
    public List<string> SortOptions { get; set; }
    public List<string> SortOrders { get; set; }
    public string SelectedOption { get; set; }
    public string SelectedOrder { get; set; }

    public CategorySortModel()
    {
        SortOptions = new()
        {
            "Nombre"
        };
        SortOrders = new()
        {
            "Ascendente",
            "Descendente"
        };
        SelectedOption = SortOptions[0];
        SelectedOrder = SortOrders[0];
    }
    public SortDescription Sort()
    {
        return new SortDescription(ParseOptions(SelectedOption), ParseOrders(SelectedOrder));
    }

    public string ParseOptions(string option)
    {
        return "Name";
    }

    public ListSortDirection ParseOrders(string order)
    {
        return order == "Ascendente"? ListSortDirection.Ascending : ListSortDirection.Descending;
    }

    
}

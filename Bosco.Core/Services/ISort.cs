using System.Collections.Generic;
using System.ComponentModel;

namespace Bosco.Core.Services;

public interface ISort
{
    public List<string> SortOptions { get; set; }
    public List<string> SortOrders { get; set; }
    public string SelectedOption { get; set; }
    public string SelectedOrder { get; set; }
    public SortDescription Sort();
    public string ParseOptions(string option);
    public ListSortDirection ParseOrders(string order);
}
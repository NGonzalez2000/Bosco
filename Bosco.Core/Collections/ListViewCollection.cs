using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bosco.Core.Collections;

public class ListViewCollection<T> : ObservableCollection<T>
{
	private T? selectedItem;

	public T? SelectedItem
	{
		get { return selectedItem; }
		set { selectedItem = value; }
	}

	public ListViewCollection() : base()
	{
	}
	public ListViewCollection(IEnumerable<T> objects) : base(objects)
	{
	}

    public void Edit(T item)
    {
		if (selectedItem is null) throw new ArgumentNullException(nameof(selectedItem));

		this[IndexOf(selectedItem)] = item;

    }
}

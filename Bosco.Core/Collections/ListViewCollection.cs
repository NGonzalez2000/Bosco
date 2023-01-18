using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Bosco.Core.Collections;

public class ListViewCollection<T> : ObservableCollection<T>
{
	public Action<T?>? OnSelectionChanged { get; set; }
	private readonly ICollectionView? collectionView;
	private T? selectedItem;

	public T? SelectedItem
	{
		get { return selectedItem; }
		set
		{
			selectedItem = value;
			OnSelectionChanged?.Invoke(selectedItem);
		}
	}
	public void SetFilter(Predicate<object>? filter)
	{
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        collectionView!.Filter = filter;

    }
	public void SetSortDescription(SortDescription sortDescription)
	{
		collectionView!.SortDescriptions.Clear();
		collectionView!.SortDescriptions.Add(sortDescription);
	}
	public void Refresh()
	{
		collectionView!.Refresh();
	}

	public ListViewCollection() : base()
	{
	}
	public ListViewCollection(IEnumerable<T> objects) : base(objects)
	{
		collectionView = CollectionViewSource.GetDefaultView(this);
	}


    public void Edit(T item)
    {
		if (selectedItem is null) throw new NullReferenceException(nameof(selectedItem));

		this[IndexOf(selectedItem)] = item;

    }
}

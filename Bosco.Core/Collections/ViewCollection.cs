using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bosco.Core.Collections;

public class ViewCollection : INotify, ICollection<IView>
{
    List<IView> _list;
    private IView? selectedView;
    public IView? SelectedView
    {
        get => selectedView;
        set
        {
            if(selectedView != value && selectedView != null)
            {
                selectedView.Clear();
            }
            if(value is not null && SetProperty(ref selectedView, value))
            {
                selectedView!.Load();
            }
        }
    }
    public void SelectViewByIndex(int index)
    {
        if(index < 0 || index >= _list.Count) throw new ArgumentOutOfRangeException(nameof(index));
        SelectedView = _list[index];
    }
    public ViewCollection(IEnumerable<IView>? list = null)
    {
        _list = list is null ? new() : new(list.OrderBy(x => x.ButtonDisplay));
    }

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public void Add(IView item)
    {
        _list.Add(item);
    }

    public void Clear()
    {
        _list.Clear();
    }

    public bool Contains(IView item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(IView[] array, int arrayIndex)
    {
        if (arrayIndex < 0) throw new ArgumentOutOfRangeException(string.Format("arrayIndex = {0}",arrayIndex));
        for(int i = 0; i < Count && i+arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = _list[i];
        }
    }

    public IEnumerator<IView> GetEnumerator()
    {
        return new ViewEnumerator(this);
    }

    public bool Remove(IView item)
    {
        return _list.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new ViewEnumerator(this);
    }
}
internal class ViewEnumerable : IEnumerable<IView>
{
    public IEnumerator<IView> GetEnumerator()
    {
        return new ViewEnumerator(this);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return new ViewEnumerator(this);
    }
}

internal class ViewEnumerator : IEnumerator<IView>
{
    List<IView> views;
    int curIndx;
    public ViewEnumerator(IEnumerable<IView> views)
    {
        this.views = new(views);
        curIndx = -1;
    }
    public IView Current => views[curIndx];

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        return ++curIndx < views.Count;
    }

    public void Reset()
    {
        curIndx = -1;
    }
}

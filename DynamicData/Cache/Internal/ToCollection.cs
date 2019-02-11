using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace DynamicData.Cache.Internal
{
    //internal sealed class ToList<TObject, TKey, TValue>
    //{
    //    private readonly IObservable<IChangeSet<TObject, TKey>> _source;
    //    private readonly Func<TObject, TKey, TValue> _resultSelector;

    //    public ToList(IObservable<IChangeSet<TObject, TKey>> source, Func<TObject, TKey, TValue> resultSelector)
    //    {
    //        _source = source;
    //        _resultSelector = resultSelector;
    //    }

    //    public IObservable<IEnumerable<TValue>> Run()
    //    {
    //        return _source.Scan((Dictionary<TKey, TObject>) null, (dictionary, changes) =>
    //            {
    //                if (dictionary == null) dictionary = new Dictionary<TKey, TObject>(changes.Count);
    //                dictionary.Clone(changes);
    //                return dictionary;
    //            })
    //            .Select(dictionary => dictionary.Select(kvp => _resultSelector(kvp.Value, kvp.Key)));
    //    }
    //}

    internal sealed class ToList<TObject, TKey, TTransform>
    {
        private readonly IObservable<TTransform> _result;

        public ToList(IObservable<IChangeSet<TObject, TKey>> source, Func<List<TObject>, TTransform> selector)
        {
            _result = source.Scan((List<TObject>)null, (list, changes) =>
            {
                if (list == null) list = new List<TObject>(changes.Count);
                list.Clone(changes);
                return list;
            }).Select(selector);
        }

        public IObservable<TTransform> Run() => _result;
    }

    internal sealed class ToList<TObject, TKey>
    {
        private readonly IObservable<List<TObject>> _result;
        
        public ToList(IObservable<IChangeSet<TObject, TKey>> source, Func<TObject, bool> filter = null, IComparer<TObject> comparer = null)
        {
            _result = source.Scan((List<TObject>) null, (list, changes) =>
            {
                if (list == null) list = new List<TObject>(changes.Count);
                list.Filter(changes, filter);
                if (comparer != null)
                    list.Sort(comparer);
                return list;
            });
        }

        public IObservable<List<TObject>> Run() => _result;
    }
}
using System;
using System.Collections.Generic;
using DynamicData.Kernel;

namespace DynamicData
{
    public interface IConnectableCache<TObject, TKey>
    {
    
    }


    /// <summary>
    /// A cache for observing and querying stateful data.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IObservableCache<TObject, TKey> : IDisposable
    {      
        /// <summary>
        /// Returns an observable of any changes which match the specified key.  The sequence starts with the inital item in the cache (if there is one).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IObservable<Change<TObject, TKey>> Watch(TKey key);

        /// <summary>
        /// Returns a observable of cache changes preceeded with the initital cache state
        /// </summary>
        /// <returns></returns>
        IObservable<IChangeSet<TObject, TKey>> Connect();

        /// <summary>
        /// Returns a filtered stream of cache changes preceeded with the initial filtered state
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="parallelisationOptions">Option to parallise the filter operation  Only applies if the filter parameter is not null</param>
        /// <returns></returns>
        IObservable<IChangeSet<TObject, TKey>> Connect(Func<TObject, bool> filter, ParallelisationOptions parallelisationOptions = null);

        /// <summary>
        /// Gets the keys
        /// </summary>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Gets the Items
        /// </summary>
        IEnumerable<TObject> Items { get; }

        /// <summary>
        /// Gets the key value pairs
        /// </summary>
        IEnumerable<KeyValuePair<TKey,TObject>> KeyValues { get; }

        /// <summary>
        /// Lookup a single item using the specified key.
        /// </summary>
        /// <remarks>
        /// Fast indexed lookup
        /// </remarks>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Optional<TObject> Lookup(TKey key);

        /// <summary>
        /// The total count of cached items
        /// </summary>
        int Count { get; }
    
    }
}
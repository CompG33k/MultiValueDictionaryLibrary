using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiValueDictionaryLibrary
{
//   System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<K,V>>'



/*

The task is to implement IMultiValueDictionary<K, V>.
This interface allows to associate multiple unique values with a single key, for instance:

var dictionary = new MultiValueDictionary<int, string>();
dictionary.Add(1, "value1");
dictionary.Add(1, "value2");

// At this point the dictionary contains one key <1> associated with two values <value1> & <value2>

Notice that the interface is generic, so the implementation must be generic as well, for instance:

var dictionary1 = new MultiValueDictionary<int, string>();
var dictionary2 = new MultiValueDictionary<string, decimal>();


P.S.

You may use any standard type/collection from .NET Framework

P.P.S.
Don't hesitate to ask any questions

*/

using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.Concurrent;

    namespace Challenges
    {
        /// <summary>
        /// Defines interface to storing multiple unique values under a single key
        /// </summary>
        /// <typeparam name="K">type of key</typeparam>
        /// <typeparam name="V">type of value</typeparam>
        public interface IMultiValueDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
        {
            /// <summary>
            /// Adds a value to either existing key or creates a new key and adds the value to it
            /// </summary>
            /// <param name="key">Key to add value to</param>
            /// <param name="value">Value to add</param>
            /// <returns>true if the underlying collection has changed; false otherwise</returns>
            bool Add(K key, V value);

            /// <summary>
            /// returns a sequence of values for the given key. throws KeyNotFoundException if the key is not present
            /// </summary>
            /// <param name="key">key to retrive the sequence of values for</param>
            /// <returns>sequence of values for the given key.</returns>
            IEnumerable<V> Get(K key);

            /// <summary>
            /// returns a sequence of values for the given key. returns empty sequence if the key is not present
            /// </summary>
            /// <param name="key">key to retrieve the sequence of values for</param>
            /// <returns>sequence of values for the given key</returns>
            IEnumerable<V> GetOrDefault(K key);

            /// <summary>
            /// Removes the value from the values associated with the given key. throws KeyNotFoundException if the key is not present
            /// </summary>
            /// <param name="key">key which values need to be adjusted</param>
            /// <param name="value">value to remove from the values for the given key</param>
            void Remove(K key, V value);

            /// <summary>
            /// Removes the given key from the dictionary with all the values associated with it
            /// </summary>
            /// <param name="key"></param>
            void Clear(K key);
        }


        public class MultiValueDictionary<K, V> : IMultiValueDictionary<K, V>
        {
            private Dictionary<K, LinkedList<V>> _dict = new Dictionary<K, LinkedList<V>>();

            /// <summary>
            /// Adds a value to either existing key or creates a new key and adds the value to it
            /// </summary>
            /// <param name="key">Key to add value to</param>
            /// <param name="value">Value to add</param>
            /// <returns>true if the underlying collection has changed; false otherwise</returns>
            public bool Add(K key, V value)
            {
                try
                {
                    if (!_dict.ContainsKey(key))
                    {
                        _dict[key] = new LinkedList<V>();
                    }

                    _dict[key].AddLast(value);
                    return true;
                }
                catch(KeyNotFoundException ex)
                {
                    // Something went wrong
                }
                return false;
            }


            /// <summary>
            /// returns a sequence of values for the given key. throws KeyNotFoundException if the key is not present
            /// </summary>
            /// <param name="key">key to retrive the sequence of values for</param>
            /// <returns>sequence of values for the given key.</returns>
            public IEnumerable<V> Get(K key)
            {
                return _dict[key];
            }

            /// <summary>
            /// returns a sequence of values for the given key. returns empty sequence if the key is not present
            /// </summary>
            /// <param name="key">key to retrieve the sequence of values for</param>
            /// <returns>sequence of values for the given key</returns>
            public IEnumerable<V> GetOrDefault(K key)
            {
                if (_dict.ContainsKey(key))
                    return _dict[key];

                return new LinkedList<V>();
            }

            /// <summary>
            /// Removes the value from the values associated with the given key. throws KeyNotFoundException if the key is not present
            /// </summary>
            /// <param name="key">key which values need to be adjusted</param>
            /// <param name="value">value to remove from the values for the given key</param>
            public void Remove(K key, V value)
            {
                _dict[key].Remove(value);   
            }

            /// <summary>
            /// Removes the given key from the dictionary with all the values associated with it
            /// </summary>
            /// <param name="key"></param>
            public void Clear(K key)
            {
                _dict[key].Clear();
            }

            public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
            {
                foreach (var list in _dict)
                    foreach (var value in list.Value)
                        yield return new KeyValuePair<K, V>(list.Key, value);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

    }
}

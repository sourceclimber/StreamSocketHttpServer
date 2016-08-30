using System.Collections.Generic;

namespace StreamSocketHttpServer
{
    /// <summary>
    /// A dictionary with the default implementation of the .Net framework,
    /// except it does not throw an exception if a non-existing item is accesed with the [] operator
    /// </summary>
    public class MyDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <summary>
        /// returns the value stored for the given key, or null if no value for that key is found
        /// </summary>
        /// <param name="key"></param>
        public new TValue this[TKey key]
        {
            get
            {
                if (ContainsKey(key))
                    return base[key];
                return default(TValue);
            }
            set { base[key] = value; }
        }
    }
}
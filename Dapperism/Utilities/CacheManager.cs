using System;
using System.Collections.Generic;
using System.Linq;

namespace Dapperism.Utilities
{
    internal sealed class CacheManager
    {
        private static readonly Dictionary<string, object> Cache = new Dictionary<string, object>();
        private static CacheManager _instance;
        private static readonly object Lock = new object();

        private CacheManager()
        {

        }

        internal static CacheManager Instance
        {
            get
            {
                lock (Lock)
                {
                    return _instance ?? (_instance = new CacheManager());
                }
            }
        }

        public void Add(string key, object value)
        {

            if (key == null || key.Equals(""))
                throw new ArgumentNullException("key", "Key can not be null or empty.");
            if (value == null || value.Equals(""))
                throw new ArgumentNullException("value", "Value can not be null or empty.");

            Cache.Add(key, value);
        }

        public void Remove(string key)
        {

            if (key == null || key.Equals(""))
                throw new ArgumentNullException("key", "Key can not be null or empty.");

            Cache.Remove(key);
        }

        public void Modify(string key, object value)
        {

            if (key == null || key.Equals(""))
                throw new ArgumentNullException("key", "Key can not be null or empty.");
            if (value == null || value.Equals(""))
                throw new ArgumentNullException("value", "Value can not be null or empty.");
            if (!Contains(key))
                throw new ArgumentNullException("key", "Key does not exist.");

            Cache.Remove(key);
            Cache.Add(key, value);
        }

        public object this[string key]
        {
            get
            {
                return Cache[key];
            }
            set
            {
                Cache[key] = value;
            }
        }

        public object GetValue(string key)
        {

            if (key == null || key.Equals(""))
                throw new ArgumentNullException("key", "Key can not be null or empty.");

            try
            {
                var value = Cache[key];
                return value;
            }
            catch
            {
                return default(object);
            }
        }

        public List<string> Keys
        {
            get
            {
                var keys = Cache.Keys;
                return keys.ToList();
            }
        }

        public List<object> Values
        {
            get
            {
                var values = Cache.Values;
                return values.ToList();
            }
        }

        public bool Contains(string key)
        {
            return Keys.Contains(key);
        }

        public TReturn SafeReader<TReturn>(string key, Func<TReturn> cache)
        {
            if (Instance.Contains(key))
                return (TReturn)Instance[key];

            return (TReturn)(Instance[key] = cache());
        }
    }
}

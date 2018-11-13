using System.Collections.Generic;

namespace ApiWebApp.Services
{
    public class DictionaryCache<T>: 
        ISingletonDictionaryCache<T>, 
        IScopedDictionaryCache<T>, 
        ITransientDictionaryCache<T> 
        where T : class
    {
        private Dictionary<string, object> _cache;

        private Dictionary<string, object> Cache => _cache ?? (_cache = new Dictionary<string, object>());
        public object Get(string key)
        {
            return Cache.TryGetValue(key, out var value) ? value : null;
        }

        public void Set(string key, object value)
        {
            Cache.Add(key,value);
        }

        public void Clear()
        {
            Cache.Clear();
        }
    }
}
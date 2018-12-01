namespace ApiWebApp.Services
{
    public interface IDictionaryCache
    {
        bool TryGet(string key,out object value);
        void Set(string key, object value);
        void Clear();
    }
    public interface IDictionaryCache<T>: IDictionaryCache where T: class
    {
    }
}
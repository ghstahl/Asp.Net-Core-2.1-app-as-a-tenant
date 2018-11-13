namespace ApiWebApp.Services
{
    public interface IDictionaryCache
    {
        object Get(string key);
        void Set(string key, object value);
        void Clear();
    }
    public interface IDictionaryCache<T>: IDictionaryCache where T: class
    {
    }
}
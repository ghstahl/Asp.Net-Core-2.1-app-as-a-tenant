namespace ApiWebApp.Services
{
    public interface IDictionaryCache
    {
        object Get(string key);
        void Set(string key, object value);
    }
    public interface IDictionaryCache<T>: IDictionaryCache where T: class
    {
    }
}
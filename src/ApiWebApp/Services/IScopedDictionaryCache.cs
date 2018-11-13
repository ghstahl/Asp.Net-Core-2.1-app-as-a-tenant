namespace ApiWebApp.Services
{
    public interface IScopedDictionaryCache<T> : IDictionaryCache<T> where T : class
    {
    }
}
namespace ApiWebApp.Services
{
    public interface ISingletonDictionaryCache<T> : IDictionaryCache<T> where T: class
    {
    }
}
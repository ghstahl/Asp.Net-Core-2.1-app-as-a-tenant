namespace ApiWebApp.Services
{
    public interface ITransientDictionaryCache<T> : IDictionaryCache<T> where T : class
    {
    }
}
namespace ApiWebApp.Services
{
    public interface IScopedObjectCache<TContaining, T> : IObjectCache<TContaining,T> 
        where TContaining : class 
        where T : class
    {
    }
}
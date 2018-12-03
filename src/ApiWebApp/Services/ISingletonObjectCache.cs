namespace ApiWebApp.Services
{
    public interface ISingletonObjectCache<TContaining, T>: IObjectCache<TContaining, T>
        where TContaining : class 
        where T : class
    {
    }
}
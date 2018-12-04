namespace ApiWebApp.Services
{
    public interface ISingletonAutoObjectCache<TContaining, T> : IObjectCache<TContaining, T>
        where TContaining : class
        where T : class
    {
    }
}
namespace ApiWebApp.Services
{
    public interface IScopedAutoObjectCache<TContaining, T> : IObjectCache<TContaining, T>
        where TContaining : class
        where T : class
    {
    }
}
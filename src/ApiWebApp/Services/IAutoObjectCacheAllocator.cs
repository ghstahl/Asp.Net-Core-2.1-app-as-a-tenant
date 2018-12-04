namespace ApiWebApp.Services
{
    public interface IAutoObjectCacheAllocator<TContaining, out T> where TContaining : class where T : class
    {
        T Allocate();
    }
}
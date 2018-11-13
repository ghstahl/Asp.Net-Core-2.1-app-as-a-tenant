using Microsoft.Extensions.DependencyInjection;

namespace ApiWebApp.Services
{
    public static class DictionaryCacheExtensions
    {
        public static void AddDictionaryCache(this IServiceCollection services)
        {
            services.AddSingletonDictionaryCache();
            services.AddTransientDictionaryCache();
            services.AddScopedDictionaryCache();
        }
        public static void AddSingletonDictionaryCache(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ISingletonDictionaryCache<>), typeof(DictionaryCache<>));
        }
        public static void AddTransientDictionaryCache(this IServiceCollection services)
        {
            services.AddTransient(typeof(ITransientDictionaryCache<>), typeof(DictionaryCache<>));
        }
        public static void AddScopedDictionaryCache(this IServiceCollection services)
        {
            services.AddScoped(typeof(IScopedDictionaryCache<>), typeof(DictionaryCache<>));
        }
    }
}
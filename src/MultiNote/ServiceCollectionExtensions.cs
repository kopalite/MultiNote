using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MultiNote
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNotifier(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<INotifier, Notifier>();
            services.Configure<NotifierConfig>(config);
        }
    }
}

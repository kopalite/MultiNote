using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiNote.Channels;
using MultiNote.Channels.Slack;

namespace MultiNote
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNotifier(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MultiNoteConfig>(config.GetSection(nameof(MultiNoteConfig)));

            services.AddTransient<INotifier, Notifier>();

            services.AddTransient<INotifierChannel, SlackChannel>();
            services.AddHttpClient<INotifierChannel, SlackChannel>();
            
        }
    }
}

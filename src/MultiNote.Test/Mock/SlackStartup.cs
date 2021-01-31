using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MultiNote.Test.Mock
{
    public class SlackStartup
    {
        public IConfiguration Configuration { get; private set; }

        public SlackStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting();

            services.AddNotifier(Configuration);
            services.AddSingleton<SlackMessageLog>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(ep => ep.MapControllers());
        }
    }
}
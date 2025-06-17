using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using TheBookOfMemory.Models.Client;

namespace TheBookOfMemory.HostBuilders;

public static class BuildApiExtensions
{
    public static IHostBuilder BuildApi(this IHostBuilder builder)
    {

        builder.ConfigureServices((context, services) =>
        {
            var host = new Uri(context.Configuration.GetValue<string>("host") ?? string.Empty);

            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer()
            };
            services.AddRefitClient<IMainApiClient>(settings: refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = host);
        });
        return builder;
    }
}
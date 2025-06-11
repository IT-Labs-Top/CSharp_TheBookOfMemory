using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;
using TheBookOfMemory.ViewModels;
using TheBookOfMemory.ViewModels.Pages;

namespace TheBookOfMemory.HostBuilders
{
    public static class BuildMainNavigationExtension
    {
        public static IHostBuilder BuildMainNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<NavigationStore>();
                services.AddUtilityNavigationServices<NavigationStore>();
                services.AddNavigationService<MainPageViewModel, NavigationStore>();
                services.AddNavigationService<EventPageViewModel, NavigationStore>();

            });

            return builder;
        }
    }
}
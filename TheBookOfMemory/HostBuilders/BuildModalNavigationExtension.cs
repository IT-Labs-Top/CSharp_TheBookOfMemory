using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;
using TheBookOfMemory.ViewModels.Pages;
using TheBookOfMemory.ViewModels.Popups;

namespace TheBookOfMemory.HostBuilders
{
    public static class BuildModalNavigationExtension
    {
        public static IHostBuilder BuildModalNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var time = context.Configuration.GetValue<int>("popupInactivityTime");

                services.AddSingleton<ModalNavigationStore>();
                services.AddUtilityNavigationServices<ModalNavigationStore>();

                services.AddNavigationService<PasswordPopupViewModel, ModalNavigationStore>(s =>
                    new PasswordPopupViewModel(
                        s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                        context.Configuration.GetValue<string>("exitPassword") ?? "1234"));

                services.AddNavigationService<InactivityPopupViewModel, ModalNavigationStore>(s =>
                    new InactivityPopupViewModel(time,
                        s.GetRequiredService<NavigationService<MainPageViewModel>>(),
                        s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>()));
            });

            return builder;
        }
    }
}
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using TheBookOfMemory.Models;
using TheBookOfMemory.Models.Entities;
using TheBookOfMemory.Models.Records;
using TheBookOfMemory.ViewModels;
using TheBookOfMemory.ViewModels.Pages;
using TheBookOfMemory.ViewModels.Popups;
using TheBookOfMemory.ViewModels.Windows;

namespace TheBookOfMemory.HostBuilders
{
    public static class BuildViewsExtension
    {
        public static IHostBuilder BuildViews(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var inactivityConfig = context.Configuration.GetSection("Inactivity").Get<InactivityConfig>();
                services.AddSingleton<IMessenger>(_ => new WeakReferenceMessenger());

                services.AddSingleton<InactivityManager<InactivityPopupViewModel>>(s => new InactivityManager<InactivityPopupViewModel>(
                    inactivityConfig ?? new InactivityConfig(60, 10),
                    s.GetRequiredService<NavigationStore>(),
                    s.GetRequiredService<ModalNavigationStore>(),
                    s.GetRequiredService<NavigationService<InactivityPopupViewModel>>(),
                    s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>()));
                services.AddSingleton<Settings>();
                services.AddSingleton<Filter>(s => new Filter(context.Configuration.GetSection("sliderValue").Get<SliderValue>()));
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new Views.Windows.MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            });
            return builder;
        }
    }
}

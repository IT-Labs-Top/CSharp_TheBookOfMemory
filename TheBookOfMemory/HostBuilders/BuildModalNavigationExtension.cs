using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;
using Serilog;
using TheBookOfMemory.Models.Client;
using TheBookOfMemory.Models.Entities;
using TheBookOfMemory.Models.Records;
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
                var sliderValue = context.Configuration.GetSection("sliderValue").Get<SliderValue>();
                var filter = new Filter { AgeAfter = sliderValue.Maximum, AgeBefore = sliderValue.Minimum };
                services.AddSingleton<ModalNavigationStore>();
                services.AddUtilityNavigationServices<ModalNavigationStore>();

                services
                    .AddParameterNavigationService<FilterPopupViewModel, ModalNavigationStore, (
                        ObservableCollection<Rank>, ObservableCollection<Medal>)>(s => param =>
                        new FilterPopupViewModel(s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                            sliderValue, filter,
                            param.Item1, param.Item2, s.GetRequiredService<IMessenger>()));

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
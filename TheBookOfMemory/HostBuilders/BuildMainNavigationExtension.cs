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

namespace TheBookOfMemory.HostBuilders;

public static class BuildMainNavigationExtension
{
    public static IHostBuilder BuildMainNavigation(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            var sliderValue = context.Configuration.GetSection("sliderValue").Get<SliderValue>();
            services.AddSingleton<NavigationStore>();
            services.AddUtilityNavigationServices<NavigationStore>();
            services.AddNavigationService<MainPageViewModel, NavigationStore>();
            services.AddNavigationService<EventPageViewModel, NavigationStore>();
            services.AddParameterNavigationService<SelectHeroPageViewModel, NavigationStore, string>(s =>
                param => new SelectHeroPageViewModel(param,
                    s.GetRequiredService<IMainApiClient>(),
                    s.GetRequiredService<Filter>(),
                    s.GetRequiredService<Settings>(), 
                    sliderValue,
                    s.GetRequiredService<ILogger>(),
                    s.GetRequiredService<IMessenger>(),
                    s.GetRequiredService<ParameterNavigationService<PersonalInformationViewModel, (People,
                        ObservableCollection<People>)>>(),
                    s.GetRequiredService<ParameterNavigationService<FilterPopupViewModel, (ObservableCollection<Rank>,
                        ObservableCollection<Medal>)>>(),
                    s.GetRequiredService<NavigationService<EventPageViewModel>>()
                ));
            services
                .AddParameterNavigationService<PersonalInformationViewModel, NavigationStore, (People,
                    ObservableCollection<People>)>(s =>
                    param => new PersonalInformationViewModel(param,
                        s.GetRequiredService<IMainApiClient>(),
                        s.GetRequiredService<ILogger>(),
                        s.GetRequiredService<IMessenger>(),
                        s.GetRequiredService<ParameterNavigationService<SelectHeroPageViewModel, string>>(),
                        s.GetRequiredService<NavigationService<MainPageViewModel>>()));
        });

        return builder;
    }
}
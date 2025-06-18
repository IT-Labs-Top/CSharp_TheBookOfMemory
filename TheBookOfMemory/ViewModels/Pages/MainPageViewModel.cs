using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationLib.Services;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.ViewModels.Pages;

public partial class MainPageViewModel(
    Settings settings,
    NavigationService<EventPageViewModel> eventPageNavigationService) : ObservableObject
{
    [ObservableProperty] private Settings _settings = settings;

    [RelayCommand]
    private void EventPageNavigation()
    {
        Settings = new Settings(IsVisuallyImpairedMode: false);
        eventPageNavigationService.Navigate();
    }

    [RelayCommand]
    private void SwitchVisuallyImpairedMode()
    {
        Settings = new Settings(IsVisuallyImpairedMode: true);
        eventPageNavigationService.Navigate();
    }
}
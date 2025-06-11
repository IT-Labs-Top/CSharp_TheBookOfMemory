using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationLib.Services;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.ViewModels.Pages;

public partial class EventPageViewModel(Settings settings, NavigationService<MainPageViewModel> mainPageNavigationService) : ObservableObject
{
    [ObservableProperty] private Settings _settings = settings;
    [RelayCommand] private void MainPageNavigation() => mainPageNavigationService.Navigate();
}
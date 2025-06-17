using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using TheBookOfMemory.ViewModels.Pages;

namespace TheBookOfMemory.ViewModels.Popups;

public partial class InactivityPopupViewModel(int time, NavigationService<MainPageViewModel> mainPageNavigationService,INavigationService closeModalNavigationService)
    : BasePopupViewModel(closeModalNavigationService)
{
    private readonly DispatcherTimer _timer = new(DispatcherPriority.Normal);
    [ObservableProperty] private int _time = time ;

    [RelayCommand]
    private void Loaded()
    {
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += _timer_Tick;
        _timer.Start();
    }

    [RelayCommand]
    private void Unloaded()
    {
        _timer.Tick -= _timer_Tick;
        _timer.Stop();
    }

    private async void _timer_Tick(object? sender, EventArgs e)
    {
        Time--;
        if (Time > 0) return;
        CloseContainerCommand.Execute(false);
        await Task.Delay(100);
        mainPageNavigationService.Navigate();
    }
}
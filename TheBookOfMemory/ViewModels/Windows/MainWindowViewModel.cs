using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using System.Windows.Threading;
using TheBookOfMemory.Helpers;
using TheBookOfMemory.ViewModels.Pages;
using TheBookOfMemory.ViewModels.Popups;

namespace TheBookOfMemory.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject, IRecipient<ViewModelChangedMessage>,
    IRecipient<ModalViewModelChangedMessage>
{
    private readonly DispatcherTimer _timer = new();
    private int _sec;
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly InactivityManager<MainPageViewModel> _inactivityManager;
    private readonly NavigationService<PasswordPopupViewModel> _passwordNavigationService;

    public ObservableObject? CurrentViewModel => _navigationStore.CurrentViewModel;
    public ObservableObject? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
    public bool IsModalOpen => _modalNavigationStore.CurrentViewModel is not null;

    public MainWindowViewModel(
        IMessenger messenger,
        NavigationStore navigationStore,
        ModalNavigationStore modalNavigationStore,
        InactivityManager<MainPageViewModel> inactivityManager,
        NavigationService<PasswordPopupViewModel> passwordNavigationService)
    {
        _navigationStore = navigationStore;
        _modalNavigationStore = modalNavigationStore;
        _inactivityManager = inactivityManager;
        _passwordNavigationService = passwordNavigationService;
        messenger.RegisterAll(this);
    }


    private void Timer(object? sender, EventArgs eventArgs)
    {
        _sec++;
        if (_sec < 7) return;
        _passwordNavigationService.Navigate();
    }

    [RelayCommand]
    private void Loaded()
    {
        ExplorerHelper.KillExplorer();
        _inactivityManager.Activate();
    }

    [RelayCommand]
    private void Closing()
    {
        ExplorerHelper.RunExplorer();
        _inactivityManager.Dispose();
    }

    [RelayCommand]
    private void StopTimer()
    {
        _timer.Tick -= Timer;
        _timer.Stop();
        _sec = 0;
    }

    [RelayCommand]
    private void StartTimer()
    {
        _timer.Stop();
        _sec = 0;
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer;
        _timer.Start();
    }

    public void Receive(ViewModelChangedMessage message) => OnPropertyChanged(nameof(CurrentViewModel));

    public void Receive(ModalViewModelChangedMessage message)
    {
        OnPropertyChanged(nameof(CurrentModalViewModel));
        OnPropertyChanged(nameof(IsModalOpen));
    }
}
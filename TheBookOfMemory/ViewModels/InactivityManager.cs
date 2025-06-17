using CommunityToolkit.Mvvm.ComponentModel;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using TheBookOfMemory.Helpers;
using TheBookOfMemory.Models;
using TheBookOfMemory.ViewModels.Pages;
using TheBookOfMemory.ViewModels.Popups;

namespace TheBookOfMemory.ViewModels;

public class InactivityManager<TInitialViewModel> : IDisposable
    where TInitialViewModel : ObservableObject
{
    private readonly NavigationStore _mainStore;
    private readonly INavigationService _initialNavigationService;
    private readonly INavigationService _closePopupNavigationService;
    private readonly BaseInactivityHelper _inactivity;
    private readonly BaseInactivityHelper _passwordInactivity;
    private readonly ModalNavigationStore _modalNavigationStore;

    private InactivityConfig Config { get; }

    public InactivityManager(
        InactivityConfig config,
        NavigationStore mainStore,
        ModalNavigationStore modalNavigationStore,
        INavigationService initialNavigationService,
        INavigationService closePopupNavigationService)
    {
        _mainStore = mainStore;
        _modalNavigationStore = modalNavigationStore;
        _initialNavigationService = initialNavigationService;
        _closePopupNavigationService = closePopupNavigationService;
        Config = config;
        _inactivity = new BaseInactivityHelper(Config.InactivityTime);
        _passwordInactivity = new BaseInactivityHelper(Config.PasswordInactivityTime);
    }

    public void Activate()
    {
        _inactivity.OnInactivity += _inactivity_OnInactivity;
        _passwordInactivity.OnInactivity += _passwordInactivity_OnInactivity;
    }

    public void Dispose()
    {
        _inactivity.OnInactivity -= _inactivity_OnInactivity;
        _passwordInactivity.OnInactivity -= _passwordInactivity_OnInactivity;
    }

    private void _passwordInactivity_OnInactivity(int inactivityTime)
    {
        if (_modalNavigationStore.CurrentViewModel is not PasswordPopupViewModel) return;
        _closePopupNavigationService.Navigate();
    }

    private void _inactivity_OnInactivity(int inactivityTime)
    {
        if (_mainStore.CurrentViewModel is TInitialViewModel or MainPageViewModel || _modalNavigationStore.CurrentViewModel is InactivityPopupViewModel) return;
        _initialNavigationService.Navigate();
    }
}
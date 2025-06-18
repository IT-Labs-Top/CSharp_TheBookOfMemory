using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.ViewModels.Popups;

public partial class HistoryPopupViewModel(
    PeopleMedia media,
    CloseNavigationService<ModalNavigationStore> closePopupNavigationService) : BasePopupViewModel(closePopupNavigationService)
{
    [ObservableProperty]
    private PeopleMedia _media = media;
}
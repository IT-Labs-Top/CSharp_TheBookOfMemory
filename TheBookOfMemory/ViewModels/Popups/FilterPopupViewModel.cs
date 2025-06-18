using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using TheBookOfMemory.Models.Entities;
using TheBookOfMemory.Models.Messages;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.ViewModels.Popups;

public partial class FilterPopupViewModel(
    CloseNavigationService<ModalNavigationStore> closePopupNavigationService,
    SliderValue sliderValue,
    Filter filter,
    ObservableCollection<Rank> ranks,
    ObservableCollection<Medal> medals,
    IMessenger messenger) : BasePopupViewModel(closePopupNavigationService)
{
    [ObservableProperty] private Filter _filters = filter;
    [ObservableProperty] private SliderValue _sliderValue = sliderValue;

    [ObservableProperty] private ObservableCollection<Rank> _ranks = ranks;
    [ObservableProperty] private ObservableCollection<Medal> _medals = medals;

    [RelayCommand]
    private void AcceptFilter()
    {
        messenger.Send(new FilterMessage(Filters));
        CloseContainerCommand.Execute(false);
    }

    [RelayCommand]
    private void ClearFilters()
    {  Filters.Clear(Ranks.FirstOrDefault(f => f.Id == -1),
        Medals.FirstOrDefault(f => f.Id == -1), (int)SliderValue.Minimum, (int)SliderValue.Maximum);

    }

    [RelayCommand]
    private void Loaded()
    {
        Filters.SelectedMedal ??= Medals.FirstOrDefault(f => f.Id == -1)!;
        Filters.SelectedRank ??= Ranks.FirstOrDefault(f => f.Id == -1)!;
    }

    partial void OnFiltersChanged(Filter value)
    {
        OnPropertyChanged(nameof(Filters));
    }

    partial void OnSliderValueChanged(SliderValue value)
    {
        OnPropertyChanged(nameof(SliderValue));
    }
}
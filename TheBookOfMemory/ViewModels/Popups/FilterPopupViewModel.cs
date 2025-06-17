using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using Serilog;
using TheBookOfMemory.Models.Client;
using TheBookOfMemory.Models.Entities;
using TheBookOfMemory.Models.Messages;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.ViewModels.Popups;

public partial class FilterPopupViewModel(
    CloseNavigationService<ModalNavigationStore> closePopupNavigationService,
    SliderValue sliderValue,
    Filter filter,
    IMainApiClient client,
    ILogger logger,
    IMessenger messenger) : BasePopupViewModel(closePopupNavigationService)
{
    [ObservableProperty] private Filter _filters = filter;
    [ObservableProperty] private SliderValue _sliderValue = sliderValue;

    [ObservableProperty] private List<Rank> _ranks = [];
    [ObservableProperty] private List<Medal> _medals = [];

    [RelayCommand]
    private void AcceptFilter()
    {
        messenger.Send(new FilterMessage(Filters));
        CloseContainerCommand.Execute(false);
    }

    [RelayCommand]
    private void ClearFilters()
    {
        var type = Filters.Type;
        Filters = new Filter{Type = type, SelectedMedal = null, SelectedRank = null,AgeAfter = DateTime.Now.Year, AgeBefore = 1900};
    }

    [RelayCommand]
    private async Task Loaded()
    {
        try
        {
            Ranks = await client.GetRank();
            Medals = await client.GetMedal();
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }
}
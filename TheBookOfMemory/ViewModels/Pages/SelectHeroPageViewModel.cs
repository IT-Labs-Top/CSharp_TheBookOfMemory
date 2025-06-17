using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmNavigationLib.Services;
using Newtonsoft.Json.Bson;
using Serilog;
using TheBookOfMemory.Models.Client;
using TheBookOfMemory.Models.Entities;
using TheBookOfMemory.Models.Enums;
using TheBookOfMemory.Models.Messages;
using TheBookOfMemory.Models.Records;
using TheBookOfMemory.Utilities;
using TheBookOfMemory.ViewModels.Popups;

namespace TheBookOfMemory.ViewModels.Pages;

public partial class SelectHeroPageViewModel(
    string type,
    IMainApiClient client,
    Filter filter,
    ILogger logger,
    IMessenger messenger,
    NavigationService<FilterPopupViewModel> filterPopupNavigationService,
    NavigationService<EventPageViewModel> eventPageNavigationService,
    ParameterNavigationService<PersonalInformationViewModel, (People, ObservableCollection<People>)> personalInformationNavigationService) : ObservableObject, IRecipient<FilterMessage>
{
    [ObservableProperty] private ObservableCollection<People> _peoples = [];
    [ObservableProperty] private ModeType _selectedModeType = ModeType.MainMode;
    [ObservableProperty] private Filter _filter = filter;

    [RelayCommand] private void SwitchMode(ModeType mode) => SelectedModeType = mode;
    [RelayCommand] private void FilterNavigation() => filterPopupNavigationService.Navigate();
    [RelayCommand] private void EventPageNavigation() => eventPageNavigationService.Navigate();

    [RelayCommand]
    private async Task Loaded()
    {
        messenger.RegisterAll(this);
        Filter.Type = type;
        await UpdatePeoplesAsync(type, null, null, null, null);
    }

    private async Task UpdatePeoplesAsync(string typePeople, int? rank, int? medal, int? ageBefore, int? ageAfter)
    {
        var peoples = await GetPeoples(typePeople, rank, medal, ageBefore, ageAfter);
        Peoples.Clear();
        var updatedPeoples = await Task.WhenAll(peoples.Select(async person =>
            person with { Image = await client.LoadImageAndGetPath(logger, person.Image) }));
        foreach (var person in updatedPeoples)
        {
            Peoples.Add(person);
        }
    }

    private async Task<ObservableCollection<People>> GetPeoples(string typePeople, int? rank, int? medal, int? ageBefore, int? ageAfter)
    {
        return [.. await client.GetPeople(typePeople, rank, medal, ageBefore, ageAfter)];
    }

    public async void Receive(FilterMessage message)
    {
        try
        {
            var messageValue = message.Value;
            await UpdatePeoplesAsync(messageValue.Type,messageValue.SelectedRank.Id,messageValue.SelectedMedal.Id, (int)messageValue.AgeBefore, (int)messageValue.AgeAfter);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }

    [RelayCommand]
    private async Task GoToPersonalInformation(People people) => 
        personalInformationNavigationService.Navigate((people, Peoples));
}
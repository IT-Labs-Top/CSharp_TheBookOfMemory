using System;
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
    ParameterNavigationService<PersonalInformationViewModel, (People, ObservableCollection<People>)> personalInformationNavigationService,
    ParameterNavigationService<FilterPopupViewModel,(ObservableCollection<Rank>, ObservableCollection<Medal>)> filterPopupNavigationService,
    NavigationService<EventPageViewModel> eventPageNavigationService) : ObservableObject, IRecipient<FilterMessage>
{
    [ObservableProperty] private ObservableCollection<People> _peoples = [];
    [ObservableProperty] private ModeType _selectedModeType = ModeType.MainMode;
    [ObservableProperty] private Filter _filter = filter;
    [ObservableProperty] private ObservableCollection<Rank> _ranks = [];
    [ObservableProperty] private ObservableCollection<Medal> _medals = [];
    [ObservableProperty] private string _searchQuery = string.Empty;

    [RelayCommand] private void SwitchMode(ModeType mode)
    {
        SelectedModeType = mode;
        if (mode != ModeType.MainMode) return;
        SearchQuery = string.Empty; 
    }
    [RelayCommand] private void FilterNavigation() => filterPopupNavigationService.Navigate((Ranks,Medals));
    [RelayCommand] private void EventPageNavigation() => eventPageNavigationService.Navigate();

    [RelayCommand]
    private async Task Search()
    {
        if (SelectedModeType == ModeType.SearchMode && !string.IsNullOrWhiteSpace(SearchQuery))
        {
            await UpdatePeoplesAsync(Filter.Type, null, null, null, null, SearchQuery);
        }
        else
        {
            await UpdatePeoplesAsync(Filter.Type, null, null, null, null);
        }
    }

    [RelayCommand]
    private async Task Loaded()
    {
        messenger.RegisterAll(this);
        Filter.Type = type;
        await LoadRankAndMedals();
        await UpdatePeoplesAsync(type, null, null, null, null);
    }

    private async Task LoadRankAndMedals()
    {
        Ranks = [new Rank { Id = -1, Title = "Все" }];
        Medals = [new Medal { Id = -1, Title = "Все" }];

        var ranksFromApi = await client.GetRank();
        foreach (var rank in ranksFromApi)
        {
            Ranks.Add(rank);
        }

        var medalsFromApi = await client.GetMedal();
        foreach (var medal in medalsFromApi)
        {
            Medals.Add(medal);
        }

        
    }

    private async Task UpdatePeoplesAsync(string typePeople, int? rank, int? medal, int? ageBefore, int? ageAfter, string searchQuery = null)
    {
        var peoples = await GetPeoples(typePeople, rank, medal, ageBefore, ageAfter);
        Peoples.Clear();

        var filteredPeoples = string.IsNullOrWhiteSpace(searchQuery)
            ? peoples
            : new ObservableCollection<People>(peoples.Where(p =>
                p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                p.Surname.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                p.Patronymic.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)));

        var updatedPeoples = await Task.WhenAll(filteredPeoples.Select(async person =>
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

            if (messageValue.SelectedMedal.Id == -1)
            {
                messageValue.SelectedMedal = null;
            }

            if (messageValue.SelectedRank.Id == -1)
            {
                messageValue.SelectedRank = null;
            }

            await UpdatePeoplesAsync(messageValue.Type, messageValue.SelectedRank?.Id, messageValue.SelectedMedal?.Id, (int?)messageValue.AgeBefore, (int?)messageValue.AgeAfter);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }

    partial void OnSearchQueryChanged(string value)
    {
        SearchCommand.Execute(null);
    }

    [RelayCommand]
    private async Task GoToPersonalInformation(People people) => 
        personalInformationNavigationService.Navigate((people, Peoples));
}
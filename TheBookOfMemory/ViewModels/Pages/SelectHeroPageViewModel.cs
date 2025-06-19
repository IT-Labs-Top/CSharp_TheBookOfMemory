using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmNavigationLib.Services;
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
    Settings settings,
    SliderValue sliderValue,
    ILogger logger,
    IMessenger messenger,
    ParameterNavigationService<PersonalInformationViewModel, (People, ObservableCollection<People>)>
        personalInformationNavigationService,
    ParameterNavigationService<FilterPopupViewModel, (ObservableCollection<Rank>, ObservableCollection<Medal>)>
        filterPopupNavigationService,
    NavigationService<EventPageViewModel> eventPageNavigationService) : ObservableObject, IRecipient<FilterMessage>
{
    [ObservableProperty] private Settings _settings = settings; 
    [ObservableProperty] private ObservableCollection<People> _peoples = [];
    [ObservableProperty] private ModeType _selectedModeType = ModeType.MainMode;
    [ObservableProperty] private Filter _filter = filter;
    [ObservableProperty] private ObservableCollection<Rank> _ranks = [];
    [ObservableProperty] private ObservableCollection<Medal> _medals = [];
    [ObservableProperty] private string _searchQuery = string.Empty;
    [ObservableProperty] private bool _noResultsFound;
    [ObservableProperty] private bool _isVisibleKeyboard = true;

    [RelayCommand] private void SwitchKeyboardVisible() => IsVisibleKeyboard = !IsVisibleKeyboard;

    [RelayCommand]
    private async Task SwitchMode(ModeType mode)
    {
        try
        {
            SelectedModeType = mode;
            if (mode != ModeType.MainMode) return;
            IsVisibleKeyboard = true;
            SearchQuery = string.Empty;
            await UpdatePeoplesAsync(Filter.Type, GetFilter(Filter.SelectedRank, null, null, null),
                GetFilter(null, Filter.SelectedMedal, null, null),
                GetFilter(null, null, null, (int?)Filter.AgeBefore),
                GetFilter(null, null, (int?)Filter.AgeAfter, null));
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }

    [RelayCommand]
    private void FilterNavigation() => filterPopupNavigationService.Navigate((Ranks, Medals));

    [RelayCommand]
    private void EventPageNavigation() => eventPageNavigationService.Navigate();

    [RelayCommand]
    private async Task Search()
    {
        try
        {
            if (SelectedModeType != ModeType.SearchMode) return;
            await UpdatePeoplesAsync(Filter.Type, GetFilter(Filter.SelectedRank, null, null, null),
                GetFilter(null, Filter.SelectedMedal, null, null),
                GetFilter(null, null,null, (int?)Filter.AgeBefore), 
                GetFilter(null,null, (int?)Filter.AgeAfter, null),
                SearchQuery);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
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

    [RelayCommand]
    private async Task Unloaded() => Filter.Clear();

    private int? GetFilter(Rank? rank, Medal? medal , int? ageAfter, int? ageBefore)
    {
        int? number = null;

        if (rank is not null)
        {
            if (rank.Id != -1)
            {
                number = rank.Id;
            }
            else
            {
                return null;
            }
        }

        if (medal is not null)
        {
            if (medal.Id != -1)
            {
                number = medal.Id;
            }
            else
            {
                return null;
            }
        }

        if (ageAfter is not null)
        {
            if (ageAfter == sliderValue.Maximum)
            {
                return null;
            }
        }

        if (ageBefore is not null)
        {
            if (ageBefore == sliderValue.Minimum)
            {
                return null;
            }
        }

        return number;
    }

    private async Task LoadRankAndMedals()
    {
        Ranks = [new Rank { Id = -1, Title = "Все" }];
        Medals = [new Medal { Id = -1, Title = "Все" }];

        var ranksFromApi = await client.GetRank();
        foreach (var rank in ranksFromApi)
            Ranks.Add(rank);

        var medalsFromApi = await client.GetMedal();
        foreach (var medal in medalsFromApi)
            Medals.Add(medal);
    }

    private async Task UpdatePeoplesAsync(string typePeople, int? rank, int? medal, int? ageBefore, int? ageAfter,
        string searchQuery = null)
    {
        try
        {
            var peoples = await GetPeoples(typePeople, rank, medal, ageBefore, ageAfter);
            Peoples.Clear();

            var trimmedQuery = searchQuery?.Trim();
            var filteredPeoples = peoples;

            if (!string.IsNullOrWhiteSpace(trimmedQuery))
            {
                var searchTerms = trimmedQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                filteredPeoples = new ObservableCollection<People>(peoples.Where(p =>
                    searchTerms.All(term =>
                        (p.Name != null && p.Name.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                        (p.Surname != null && p.Surname.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                        (p.Patronymic != null && p.Patronymic.Contains(term, StringComparison.OrdinalIgnoreCase)))));
            }

            NoResultsFound = filteredPeoples.Count == 0 && !string.IsNullOrWhiteSpace(trimmedQuery);

            var updatedPeoples = await Task.WhenAll(filteredPeoples.Select(async person =>
                person with { Image = await client.LoadImageAndGetPath(logger, person.Image) }));

            foreach (var person in updatedPeoples)
                Peoples.Add(person);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }

    private async Task<ObservableCollection<People>> GetPeoples(
        string typePeople, int? rank, int? medal, int? ageBefore, int? ageAfter) => 
        [.. await client.GetPeople(typePeople, rank, medal, ageBefore, ageAfter)];

    public async void Receive(FilterMessage message)
    {
        try
        {
            var messageValue = message.Value;

            if (messageValue.SelectedMedal?.Id == -1)
            {
                messageValue.SelectedMedal = null;
            }

            if (messageValue.SelectedRank?.Id == -1)
            {
                messageValue.SelectedRank = null;
            }

            await UpdatePeoplesAsync(messageValue.Type, messageValue.SelectedRank?.Id, messageValue.SelectedMedal?.Id,
                (int?)messageValue.AgeBefore, (int?)messageValue.AgeAfter);
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
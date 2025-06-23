using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmNavigationLib.Services;
using Serilog;
using System.Collections.ObjectModel;
using TheBookOfMemory.Models.Client;
using TheBookOfMemory.Models.Records;
using TheBookOfMemory.Utilities;
using TheBookOfMemory.ViewModels.Popups;

namespace TheBookOfMemory.ViewModels.Pages;

public partial class PersonalInformationViewModel(
    (People people, ObservableCollection<People> peoples) tuple,
    IMainApiClient client,
    ILogger logger,
    IMessenger messenger,
    ParameterNavigationService<SelectHeroPageViewModel, string> goBackNavigationService,
    NavigationService<MainPageViewModel> mainPageNavigationService,
    ParameterNavigationService<HistoryPopupViewModel, PeopleMedia> historyPopupNavigationService,
    Settings settings) : ObservableObject
{
    [ObservableProperty] 
    private PeopleById _selectedPeople;
    [ObservableProperty] 
    private Settings _settings = settings;

    private People _currentPeople = tuple.people;
    private ObservableCollection<People> _peoples = tuple.peoples;

    [RelayCommand]
    private async Task Loaded()
    {
        messenger.RegisterAll(this);

        var peopleById = await GetPeopleByIdFromPeople(tuple.people);
        SelectedPeople = await ProcessingPeople(peopleById);
    }

    private async Task<PeopleById> ProcessingPeople(PeopleById peopleById)
    {
        var updatedPeople = peopleById with
        {
            Image = await client.LoadImageAndGetPath(logger, peopleById.Image),
            PeopleMedia = (await Task.WhenAll(peopleById.PeopleMedia.Select(async media =>
                media with { Media = await client.LoadImageAndGetPath(logger, media.Media) }))).ToList(),
            BirthDate = new DateTime(peopleById.BirthDate.Year)
        };

        return updatedPeople;
    }

    [RelayCommand]
    private async Task GoBack() =>
        goBackNavigationService.Navigate(tuple.people.Type);

    [RelayCommand]
    private async Task GoToNextHero(PeopleById currentPeople)
    {
        var index = _peoples.IndexOf(_currentPeople);
        if (index++ >= _peoples.Count - 1)
            index = 0;
        _currentPeople = _peoples[index];
        var p = await GetPeopleByIdFromPeople(_currentPeople);
        SelectedPeople = await ProcessingPeople(p);
    }

    private async Task<PeopleById> GetPeopleByIdFromPeople(People people) =>
        await client.GetPeopleById(people.Id);

    [RelayCommand]
    private async Task GoToMain() =>
        mainPageNavigationService.Navigate();

    [RelayCommand]
    private async Task ShowPopup(PeopleMedia media) => historyPopupNavigationService.Navigate(media);
}
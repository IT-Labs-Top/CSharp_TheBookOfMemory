using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MvvmNavigationLib.Services;
using Serilog;
using System.Collections.ObjectModel;
using MvvmNavigationLib.Stores;
using TheBookOfMemory.Models.Client;
using TheBookOfMemory.Models.Records;
using TheBookOfMemory.Utilities;

namespace TheBookOfMemory.ViewModels.Pages;

public partial class PersonalInformationViewModel(
    (People people, ObservableCollection<People> peoples) tuple,
    IMainApiClient client,
    ILogger logger,
    IMessenger messenger,
    ParameterNavigationService<SelectHeroPageViewModel, string> goBackNavigationService,
    NavigationService<MainPageViewModel> mainPageNavigationService) : ObservableObject
{

    [ObservableProperty]
    private PeopleById _selectedPeople;

    private ObservableCollection<People> _peoples = tuple.peoples;

    [RelayCommand]
    private async Task Loaded()
    {
        messenger.RegisterAll(this);

        var peopleById = await GetPeopleByIdFromPeople(tuple.people);
        SelectedPeople = await ProcessingPeople(peopleById);
    }

    private async Task<PeopleById> GetPeopleByIdFromPeople(People people) =>
        await client.GetPeopleById(people.Id);

    private async Task<PeopleById> ProcessingPeople(PeopleById peopleById)
    {
        var updatedPeople = peopleById with
        {
            Image = await client.LoadImageAndGetPath(logger, peopleById.Image),
            PeopleMedia = (await Task.WhenAll(peopleById.PeopleMedia.Select(async media =>
                media with { Media = await client.LoadImageAndGetPath(logger, media.Media) }))).ToList()
        };

        return updatedPeople;
    }

    [RelayCommand]
    private async Task GoBack() =>
        goBackNavigationService.Navigate(tuple.people.Type);

    [RelayCommand]
    private async Task GoToNextHero(PeopleById currentPeople) { }

    [RelayCommand]
    private async Task GoToMain() => 
        mainPageNavigationService.Navigate();
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationLib.Services;
using TheBookOfMemory.Models.Client;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.ViewModels.Pages;

public partial class EventPageViewModel(
    Settings settings,
    IMainApiClient client,
    NavigationService<MainPageViewModel> mainPageNavigationService,
    ParameterNavigationService<SelectHeroPageViewModel, string> navigationSelectNavigationService) : ObservableObject
{
    [ObservableProperty] private Settings _settings = settings;

    [ObservableProperty] private List<Event> _upEvents =
    [
        new("Великая Отечественная война", "vov",
            "Узнайте о жителях Нижней Синячихи, проявивших мужество и героизм в годы Великой Отечественной войны. Их подвиги стали символом стойкости и патриотизма."),
        new("Герои СВО", "svo",
            "Познакомьтесь с современниками — участниками специальной военной операции, представляющими Нижнюю Синячиху. Их истории — свидетельство служения Родине в наше время"),
        new("Афганская война", "afgan",
            "Узнайте о воинах-интернационалистах из Нижней Синячихи, прошедших службу в Афганистане. Их смелость и самоотверженность навсегда останутся в памяти поколений")
    ];

    [ObservableProperty] private List<Event> _downEvents =
    [
        new("Российско-чеченский конфликт", "chechnya",
            "Откройте страницы истории, рассказывающие о жителях Нижней Синячихи, принимавших участие в чеченских кампаниях. Эти люди прошли через сложные испытания во имя мира и стабильности"),
        new("Локальные конфликты", "local",
            "Познакомьтесь с жителями Нижней Синячихи, участвовавшими в различных миротворческих и боевых операциях за пределами страны. Их служба — пример гражданского долга и отваги"),
    ];

    [RelayCommand] private void NavigateToSelectPage(string type) => navigationSelectNavigationService.Navigate(type);
    [RelayCommand] private void MainPageNavigation() => mainPageNavigationService.Navigate();
}
using CommunityToolkit.Mvvm.ComponentModel;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.Models.Entities;

public partial class Filter(SliderValue sliderValue) : ObservableObject
{
    [ObservableProperty] private string _type;
    [ObservableProperty] private Medal? _selectedMedal = null;
    [ObservableProperty] private Rank? _selectedRank = null;
    [ObservableProperty] private double _ageAfter = sliderValue.Maximum;
    [ObservableProperty] private double _ageBefore = sliderValue.Minimum;
    [ObservableProperty] private int _totalSelectedFilters = 0;
    [ObservableProperty] private Dictionary<string, int> _numberOfSelectedFilters = new()
    {
        { "rank", 0 },
        { "medal", 0 },
        { "ageBefore", 0 },
        { "ageAfter", 0 }
    };

    public void Clear(Rank rank, Medal medal, int ageBefore, int ageAfter)
    {
        SelectedRank = rank;
        SelectedMedal = medal;
        AgeAfter = ageAfter;
        AgeBefore = ageBefore;
        NumberOfSelectedFilters = new Dictionary<string, int>
        {
            { "rank", 0 },
            { "medal", 0 },
            { "ageBefore", 0 },
            { "ageAfter", 0 }
        };
    }

    partial void OnAgeBeforeChanged(double value)
    {
        CountingValuesChanged("ageBefore", value != sliderValue.Minimum);
    }

    partial void OnAgeAfterChanged(double value)
    {
        CountingValuesChanged("ageAfter", value != sliderValue.Maximum);
    }

    partial void OnSelectedMedalChanged(Medal? value)
    {
        CountingValuesChanged("medal", value is not null && value?.Id != -1);
    }

    partial void OnSelectedRankChanged(Rank? value)
    {
        CountingValuesChanged("rank", value is not null && value?.Id != -1);
    }

    private void CountingValuesChanged(string key, bool isSelected)
    {
        NumberOfSelectedFilters[key] = isSelected ? 1 : 0;
        TotalSelectedFilters = NumberOfSelectedFilters.Values.Sum();
    }
}
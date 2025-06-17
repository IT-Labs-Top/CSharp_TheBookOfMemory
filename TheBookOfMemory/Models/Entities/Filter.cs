using CommunityToolkit.Mvvm.ComponentModel;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.Models.Entities;

public partial class Filter : ObservableObject
{
    [ObservableProperty] private string _type;
    [ObservableProperty] private Medal? _selectedMedal = null;
    [ObservableProperty] private Rank? _selectedRank = null;
    [ObservableProperty] private double _ageAfter = DateTime.Now.Year;
    [ObservableProperty] private double _ageBefore = 1900;

    public void Clear(Rank rank, Medal medal, int ageBefore, int ageAfter)
    {
        SelectedRank = rank;
        SelectedMedal = medal;
        AgeAfter = ageAfter;
        AgeBefore = ageBefore;
    }
}
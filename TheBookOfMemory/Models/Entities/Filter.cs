using CommunityToolkit.Mvvm.ComponentModel;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.Models.Entities;

public partial class Filter : ObservableObject
{
    [ObservableProperty] private string _type;
    [ObservableProperty] private Medal _selectedMedal;
    [ObservableProperty] private Rank _selectedRank;
    [ObservableProperty] private double _ageAfter;
    [ObservableProperty] private double _ageBefore;
}
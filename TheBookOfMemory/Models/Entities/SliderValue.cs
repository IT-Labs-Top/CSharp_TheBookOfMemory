using CommunityToolkit.Mvvm.ComponentModel;

namespace TheBookOfMemory.Models.Entities;

public partial class SliderValue : ObservableObject
{
    [ObservableProperty] private double _minimum = 1900;
    [ObservableProperty] private double _maximum = DateTime.Now.Year;
};
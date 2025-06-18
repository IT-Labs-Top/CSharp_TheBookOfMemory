using CommunityToolkit.Mvvm.ComponentModel;

namespace TheBookOfMemory.Models.Records;

public partial class Settings : ObservableObject
{
    [ObservableProperty] private bool _isVisuallyImpairedMode = false;
}
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace TheBookOfMemory.Models.Records;

public partial class Rank : ObservableObject
{
    [JsonProperty("id"), ObservableProperty] private int _id;
    [JsonProperty("title"), ObservableProperty] private string _title;
}
    
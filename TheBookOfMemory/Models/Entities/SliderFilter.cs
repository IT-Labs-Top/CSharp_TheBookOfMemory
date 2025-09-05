using Newtonsoft.Json;

namespace TheBookOfMemory.Models.Entities;

public class SliderFilter
{
    [JsonProperty("minYear")] public int MinYear { get; set; }
    [JsonProperty("maxYear")] public int MaxYear { get; set; }
}
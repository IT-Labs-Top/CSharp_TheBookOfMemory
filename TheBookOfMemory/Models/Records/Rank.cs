using Newtonsoft.Json;

namespace TheBookOfMemory.Models.Records;

public record Rank(
    [JsonProperty("id")] int Id,
    [JsonProperty("title")] string Title);
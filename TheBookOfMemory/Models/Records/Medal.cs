using Newtonsoft.Json;

namespace TheBookOfMemory.Models.Records;

public record Medal(
    [JsonProperty("id")] int Id,
    [JsonProperty("title")] string Title);
using Newtonsoft.Json;

namespace TheBookOfMemory.Models.Records;

public record People(
    [JsonProperty("id")]int Id,
    [JsonProperty("type")]string Type,
    [JsonProperty("name")]string Name,
    [JsonProperty("surname")]string Surname,
    [JsonProperty("patronymic")]string Patronymic,
    [JsonProperty("image")]string Image);
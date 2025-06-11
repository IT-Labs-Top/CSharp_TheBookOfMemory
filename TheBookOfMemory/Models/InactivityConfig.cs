using Newtonsoft.Json;

namespace TheBookOfMemory.Models
{
    public record InactivityConfig(
        [property: JsonProperty("inactivityTime")] int InactivityTime,
        [property: JsonProperty("passwordInactivityTime")] int PasswordInactivityTime);
}
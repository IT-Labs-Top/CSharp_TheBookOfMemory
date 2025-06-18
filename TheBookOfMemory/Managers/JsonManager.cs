using System.IO;
using Newtonsoft.Json;

namespace TheBookOfMemory.Managers;

public class JsonManager
{
    public static T ReadJson<T>(string path)
    {
        var jsonContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path + ".json"));
        var deserializeObject = JsonConvert.DeserializeObject<T>(jsonContent);
        return deserializeObject;
    }
}
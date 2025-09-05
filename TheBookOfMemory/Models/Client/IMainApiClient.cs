using Refit;
using System.Net.Http;
using TheBookOfMemory.Models.Entities;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.Models.Client;

public interface IMainApiClient
{
    [Get("/api/people?type={type}&rank={rank}&medal={medal}&ageBefore={ageBefore}&ageAfter={ageAfter}")]
    Task<List<People>> GetPeople(string? type, int? rank, int? medal, int? ageBefore, int? ageAfter);

    [Get("/api/medal")]
    Task<List<Medal>> GetMedal();

    [Get("/api/people/{id}")]
    Task<PeopleById> GetPeopleById(int id);

    [Get("/api/rank")]
    Task<List<Rank>> GetRank();

    [Get("/api/filters")]
    Task<SliderFilter> GetFilters();

    [Get("/{**imageId}")]
    [QueryUriFormat(UriFormat.Unescaped)]
    Task<HttpResponseMessage> LoadImage(string imageId);
}
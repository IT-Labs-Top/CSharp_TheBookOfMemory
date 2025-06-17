namespace TheBookOfMemory.Models.Records;

public record PeopleById(
    int Id,
    string Type,
    string Name,
    string Surname,
    string Patronymic,
    string Image,
    DateTime BirthDate,
    string BirthPlace,
    DateTime? DeathDate,
    string DeathPlace,
    string DeathReason,
    DateTime? InvocationDate,
    Rank Rank,
    string MilitaryUnit,
    string MilitaryBranch,
    DateTime? EndDateOfService,
    string DeathInfo,
    string BurialPlace,
    List<PeopleMedia> PeopleMedia,
    List<Medal> Medals
    );
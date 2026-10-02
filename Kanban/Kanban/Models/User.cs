using System.Globalization;
using System.Text.Json.Serialization;

namespace Kanban.Models;

public sealed record User(int Id, string GivenName, string Surname)
{
    [JsonIgnore]
    public string FullName => $"{GivenName} {Surname}".Trim();

    /// <summary>Upper-case first letters of the given name and surname, e.g. "AL".</summary>
    [JsonIgnore]
    public string Initials => string.Concat(FirstLetter(GivenName), FirstLetter(Surname));

    private static string FirstLetter(string name)
    {
        var trimmed = name.Trim();
        return trimmed.Length == 0
            ? string.Empty
            : new StringInfo(trimmed).SubstringByTextElements(0, 1).ToUpperInvariant();
    }
}

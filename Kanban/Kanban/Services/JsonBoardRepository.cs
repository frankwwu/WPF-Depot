using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Kanban.Models;

namespace Kanban.Services;

/// <summary>Stores the board as a JSON file.</summary>
public sealed class JsonBoardRepository(string path) : IBoardRepository
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    /// <summary>The default location: <c>%LOCALAPPDATA%\Kanban\board.json</c> on Windows.</summary>
    public static string DefaultPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Kanban",
        "board.json");

    public BoardSnapshot? Load()
    {
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            using var stream = File.OpenRead(path);
            return JsonSerializer.Deserialize<BoardSnapshot>(stream, Options);
        }
        catch (JsonException)
        {
            // Don't silently overwrite a file we couldn't read: keep it next to the original.
            File.Move(path, path + ".corrupt", overwrite: true);
            return null;
        }
    }

    public void Save(BoardSnapshot snapshot)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(path));
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Write to a temp file first so a crash mid-write can't leave a half-written board behind.
        var temp = path + ".tmp";
        using (var stream = File.Create(temp))
        {
            JsonSerializer.Serialize(stream, snapshot, Options);
        }

        File.Move(temp, path, overwrite: true);
    }
}

using Kanban.Models;
using Kanban.Services;

namespace Kanban.Tests;

public sealed class JsonBoardRepositoryTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "kanban-tests-" + Guid.NewGuid().ToString("N"));

    private string BoardPath => Path.Combine(_directory, "nested", "board.json");

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }

    [Fact]
    public void Load_returns_null_when_there_is_no_file()
    {
        Assert.Null(new JsonBoardRepository(BoardPath).Load());
    }

    [Fact]
    public void Save_creates_missing_directories_and_round_trips_the_board()
    {
        var repository = new JsonBoardRepository(BoardPath);
        var board = new BoardSnapshot
        {
            Users = [new User(1, "Ada", "Lovelace"), new User(2, "Alan", "Turing")],
            Items =
            [
                new WorkItemSnapshot(1, "Design the analytical engine", WorkItemStatus.InProgress, 1),
                new WorkItemSnapshot(2, "Crack a cipher", WorkItemStatus.Done, null),
            ],
        };

        repository.Save(board);
        var loaded = repository.Load();

        Assert.NotNull(loaded);
        Assert.Equal(board.Users, loaded.Users);
        Assert.Equal(board.Items, loaded.Items);
    }

    [Fact]
    public void The_file_is_human_readable_and_does_not_leave_a_temp_file_behind()
    {
        var repository = new JsonBoardRepository(BoardPath);
        repository.Save(new BoardSnapshot
        {
            Users = [new User(1, "Ada", "Lovelace")],
            Items = [new WorkItemSnapshot(1, "x", WorkItemStatus.InProgress, 1)],
        });

        var json = File.ReadAllText(BoardPath);

        Assert.Contains("\"InProgress\"", json);   // enums as names, not numbers
        Assert.DoesNotContain("fullName", json);   // derived properties aren't stored
        Assert.False(File.Exists(BoardPath + ".tmp"));
    }

    [Fact]
    public void Saving_again_replaces_the_previous_contents()
    {
        var repository = new JsonBoardRepository(BoardPath);

        repository.Save(new BoardSnapshot { Items = [new WorkItemSnapshot(1, "first", WorkItemStatus.New, null)] });
        repository.Save(new BoardSnapshot { Items = [new WorkItemSnapshot(2, "second", WorkItemStatus.New, null)] });

        var item = Assert.Single(repository.Load()!.Items);
        Assert.Equal("second", item.Title);
    }

    [Fact]
    public void A_corrupt_file_is_set_aside_rather_than_overwritten()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(BoardPath)!);
        File.WriteAllText(BoardPath, "{ this is not json");
        var repository = new JsonBoardRepository(BoardPath);

        Assert.Null(repository.Load());

        Assert.False(File.Exists(BoardPath));
        Assert.Equal("{ this is not json", File.ReadAllText(BoardPath + ".corrupt"));
    }

    [Fact]
    public void Missing_properties_fall_back_to_an_empty_board()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(BoardPath)!);
        File.WriteAllText(BoardPath, "{}");

        var loaded = new JsonBoardRepository(BoardPath).Load();

        Assert.NotNull(loaded);
        Assert.Empty(loaded.Users);
        Assert.Empty(loaded.Items);
    }
}

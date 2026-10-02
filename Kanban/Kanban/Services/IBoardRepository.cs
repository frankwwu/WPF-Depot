using Kanban.Models;

namespace Kanban.Services;

/// <summary>Loads and saves the board. Implementations are synchronous; the data is tiny.</summary>
public interface IBoardRepository
{
    /// <summary>Returns the saved board, or <see langword="null"/> if there isn't a usable one.</summary>
    BoardSnapshot? Load();

    void Save(BoardSnapshot snapshot);
}

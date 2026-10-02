# Kanban

A WPF Kanban demo: three columns (New / In Progress / Done), cards you can drag between them, and a clean MVVM split that keeps everything except the XAML unit-testable.

Requires Windows and the [.NET 10 SDK](https://dotnet.microsoft.com/download) (Visual Studio 2026,
or just the `dotnet` CLI).

```powershell
dotnet run --project Kanban     # run the app
dotnet test                     # run the unit tests
```

## Using the board

| To...                | Do this                                                                                            |
| -------------------- | -------------------------------------------------------------------------------------------------- |
| Add a work item      | Type a title in the box at the top and press **Enter** (or **Add**). It lands at the top of *New*. |
| Move a card          | Drag it to another column, or drop it between cards to reorder.                                    |
| Edit the title       | Double-click the card, or use its **⋯** menu.                                                      |
| Assign / unassign    | **⋯** menu (or right-click) → *Assign to* / *Unassign*.                                            |
| Move without a mouse | **⋯** menu → *Move to*.                                                                            |
| Delete               | **⋯** menu → *Delete…* (asks first).                                                               |

The board is saved after every change to `%LOCALAPPDATA%\Kanban\board.json`. Delete that file to get a fresh set of demo data ([Bogus](https://github.com/bchavez/Bogus) generates the names and titles). If the file ever can't be parsed it is renamed to `board.json.corrupt` instead of being overwritten.

The app follows the Windows light/dark setting as of launch, using WPF's built-in Fluent theme for the standard controls and `Themes\Light.xaml` / `Themes\Dark.xaml` for the board's own colors.

## Structure

```
Kanban.sln
├─ Kanban/          net10.0-windows   The WPF app
│   ├─ Models/        WorkItem, User, WorkItemStatus, BoardSnapshot (what gets saved)
│   ├─ Mvvm/          ObservableObject, DelegateCommand / DelegateCommand<T>
│   ├─ ViewModels/    BoardViewModel, ColumnViewModel, CardViewModel
│   ├─ Services/      IBoardRepository + JsonBoardRepository, SampleData,
│   │                 IDialogService + WpfDialogService
│   ├─ MainWindow.xaml(.cs)   Board layout; code-behind is drag-and-drop plumbing only
│   ├─ App.xaml.cs            Composition root: builds the object graph, applies the theme
│   └─ Converters/  Themes/  Views/
└─ Kanban.Tests/    net10.0-windows   xUnit tests for the view models, repository and commands
```

Everything under `Models/`, `Mvvm/`, `ViewModels/` and the non-`Wpf*` files in `Services/` is free of WPF types, which is what makes it testable. That's a convention rather than something the compiler enforces, so keep it that way (or split it back out into a class library if the project grows). Because the tests reference the Windows-targeted app project, they also target `net10.0-windows` and need Windows to build and run.

A few design notes:

* **No DI container.** The whole object graph is three objects wired up in `App.OnStartup`. (This replaced MEF, which is legacy.) If the app grows, `Microsoft.Extensions.DependencyInjection` is the natural next step.
* **View models never touch UI types.** Anything that needs a dialog goes through `IDialogService`; the WPF implementation lives in the app project and the tests use a fake.
* **Each card has its own commands** (`CardViewModel`), so its context menu can bind straight to `EditCommand`, `MoveToCommand`, etc. without passing the card around as a parameter.
* **Drop slots** are counted against the destination column *before* the move; `BoardViewModel.MoveCard` documents and tests the off-by-one that reordering within a column requires.

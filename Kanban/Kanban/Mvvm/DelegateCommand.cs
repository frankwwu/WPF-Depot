using System.Windows.Input;

namespace Kanban.Mvvm;

/// <summary>
/// A parameterless <see cref="ICommand"/>. Call <see cref="RaiseCanExecuteChanged"/> whenever
/// the state that <c>canExecute</c> depends on changes.
/// </summary>
public sealed class DelegateCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public DelegateCommand(Action execute, Func<bool>? canExecute = null)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    /// <summary>Runs the command. This is a no-op while <see cref="CanExecute"/> is <see langword="false"/>.</summary>
    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
        {
            _execute();
        }
    }

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

/// <summary>
/// An <see cref="ICommand"/> that takes a typed parameter. A parameter of the wrong type
/// (or <see langword="null"/> for a non-nullable value type) makes the command non-executable.
/// </summary>
public sealed class DelegateCommand<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool>? _canExecute;

    public DelegateCommand(Action<T> execute, Func<T, bool>? canExecute = null)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) =>
        TryConvert(parameter, out var value) && (_canExecute?.Invoke(value) ?? true);

    /// <summary>Runs the command. This is a no-op while <see cref="CanExecute"/> is <see langword="false"/>.</summary>
    public void Execute(object? parameter)
    {
        if (TryConvert(parameter, out var value) && (_canExecute?.Invoke(value) ?? true))
        {
            _execute(value);
        }
    }

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    private static bool TryConvert(object? parameter, out T value)
    {
        if (parameter is T typed)
        {
            value = typed;
            return true;
        }

        // null is a legal argument for reference types and Nullable<T>.
        if (parameter is null && default(T) is null)
        {
            value = default!;
            return true;
        }

        value = default!;
        return false;
    }
}

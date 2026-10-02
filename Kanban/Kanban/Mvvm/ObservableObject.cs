using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kanban.Mvvm;

/// <summary>Minimal <see cref="INotifyPropertyChanged"/> base class.</summary>
public abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Assigns <paramref name="value"/> to <paramref name="storage"/> and raises
    /// <see cref="PropertyChanged"/> if the value actually changed.
    /// </summary>
    /// <returns><see langword="true"/> if the value changed.</returns>
    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

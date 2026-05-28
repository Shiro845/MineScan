using CommunityToolkit.Mvvm.ComponentModel;

namespace MineScan.Services;

public class NavigationService : ObservableObject
{
    public static NavigationService Instance { get; } = new();

    public object? CurrentPage
    {
        get;
        set => SetProperty(ref field, value);
    }

    public void NavigateTo<T>() where T : new()
    {
        CurrentPage = new T();
    }
}